using TreasureMap.Exceptions;
using TreasureMap.Models.GameLogic;
using TreasureMap.Models.Roles;
using TreasureMap.Models.Treasures;

namespace TreasureMap.Models.MapComponents;

/**
 * controls all the game objects
 */
public class GameMap
{
    public int Width { get; }
    public int Height { get; }
    public TreasureGame Game { get; set; }

    private readonly Dictionary<Coordinate, MapObject> _positionManager;

    private readonly List<Character> _characters;

    private readonly List<Monster> _monsters;

    private readonly List<Treasure> _treasures;

    private readonly List<Obstacle> _obstacles;

    public List<MapObject> MapObjects => _positionManager.Values.ToList();

    public List<Monster> Monsters => MapObjects.Where(e => e is Monster).Cast<Monster>().ToList();

    public List<Treasure> GetTreasures =>
        MapObjects.Where(e => e is Treasure).Cast<Treasure>().ToList();

    public List<Character> Characters =>
        MapObjects.Where(e => e is Character).Cast<Character>().ToList();

    public Character? Character => Characters.FirstOrDefault();

    public List<Obstacle> Obstacles =>
        MapObjects.Where(e => e is Obstacle).Cast<Obstacle>().ToList();

    public List<Role> Roles =>
        MapObjects
            .OrderByDescending(e => e is Character)
            .Where(e => e is Role)
            .Cast<Role>()
            .ToList();

    public GameMap(
        int width,
        int height,
        List<Character> characters,
        List<Monster> monsters,
        List<Treasure> treasures,
        List<Obstacle> obstacles
    )
    {
        Width = width;
        Height = height;
        _characters = characters;
        _monsters = monsters;
        _treasures = treasures;
        _obstacles = obstacles;
        _positionManager = new();
    }

    public void ShowObjectPositions()
    {
        foreach (var position in _positionManager.Keys)
        {
            Console.WriteLine(position);
        }
    }

    public void RandomlyDistributeObjects()
    {
        var availablePositions = new List<Coordinate>();
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                availablePositions.Add(new Coordinate(x, y));
            }
        }

        // merge all objects
        var objects = new List<MapObject>();
        objects.AddRange(_characters);
        objects.AddRange(_monsters);
        objects.AddRange(_treasures);
        objects.AddRange(_obstacles);

        foreach (var obj in objects)
        {
            if (availablePositions.Count == 0)
            {
                throw new InvalidOperationException("No more available positions on the map.");
            }

            var index = Random.Shared.Next(availablePositions.Count);
            var position = availablePositions[index];

            AddObject(obj, position);
            // 將最後一個元素移動到被移除元素的位置，然後刪除最後一個元素
            availablePositions[index] = availablePositions[availablePositions.Count - 1];
            availablePositions.RemoveAt(index);
        }
    }

    public void RandomlyDecideCharacterDirection()
    {
        var character = Character;

        if (character == null)
        {
            throw new InvalidOperationException("Character not found.");
        }

        // use reflection to get all the values of the enum
        var directions = Enum.GetValues<Direction>();
        var index = Random.Shared.Next(directions.Length);
        character.Direction = directions[index];
    }

    private void NotifyObservers()
    {
        var characterCount = Characters.Count;
        var monsterCount = Monsters.Count;

        Game.UpdateMapState(characterCount, monsterCount);
    }

    public void AddObject(MapObject obj, Coordinate position)
    {
        if (IsValidPosition(position) && !IsPositionOccupied(position))
        {
            obj.GameMap = this;
            obj.Position = position;
            _positionManager.Add(position, obj);
            NotifyObservers();
        }
        else
        {
            throw new InvalidOperationException("Position is invalid or already occupied.");
        }
    }

    public void RemoveObject(MapObject obj)
    {
        var position = obj.Position;

        if (position == null)
        {
            throw new InvalidOperationException("Object not found.");
        }
        Console.WriteLine($"Removing object at {position}");
        _positionManager.Remove(position);
        obj.Position = null;
        obj.GameMap = null;
        NotifyObservers();
    }

    public bool MoveRole(Role role, Coordinate newPosition)
    {
        Console.WriteLine($"Moving {role} to {newPosition}");
        var currentPosition = role.Position;

        if (!ValidatePositionChange(newPosition, currentPosition))
        {
            return false;
        }

        // if the new position is not occupied, move the object
        if (!IsPositionOccupied(newPosition))
        {
            UpdateRolePosition(role, newPosition);
            return true;
        }

        if (!_positionManager.TryGetValue(newPosition, out var touchedObject))
        {
            throw new GameMapObjectNotFoundException();
        }

        Console.WriteLine($"{role} touch {touchedObject}");

        HandleTouch(role, touchedObject);

        if (touchedObject is not Treasure)
            return false;

        UpdateRolePosition(role, newPosition);
        return true;
    }

    private bool ValidatePositionChange(Coordinate newPosition, Coordinate? currentPosition)
    {
        if (currentPosition == null)
        {
            throw new InvalidOperationException("Object not found.");
        }

        if (newPosition.Equals(currentPosition))
        {
            return false;
        }

        if (!IsValidPosition(newPosition))
        {
            Console.WriteLine("Out of bounds");
            return false;
        }

        return true;
    }

    public char[,] GetMapRepresentation()
    {
        char[,] map = new char[Height, Width];

        // Fill with empty space
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                map[y, x] = ' ';
            }
        }

        foreach (var (position, obj) in _positionManager)
        {
            map[position.YAxis, position.XAxis] = obj.Symbol;
        }

        return map;
    }

    /// <summary>
    /// Target hostile roles within range and attack based on character direction
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotImplementedException"></exception>
    public List<Monster> FindDestructibleMonsters(Character character)
    {
        var characterPosition = character.Position;

        if (characterPosition == null)
        {
            throw new InvalidOperationException("Character not found.");
        }

        var monsters = Monsters;

        var obstacles = Obstacles;

        // 獲取角色面向方向上的怪物
        var candidates = FindObjectsInCharacterDirection(
            character,
            monsters.Cast<MapObject>().ToArray()
        );

        // 獲取角色面向方向上的障礙物
        var obstaclesInSight = FindObjectsInCharacterDirection(
            character,
            obstacles.Cast<MapObject>().ToArray()
        );

        var firstObstacle = FindFirstObstacle(obstaclesInSight, characterPosition);

        var monstersInRange = MonstersInRange(candidates, firstObstacle, characterPosition);

        return monstersInRange.Cast<Monster>().ToList();
    }

    private void HandleTouch(Role activeRole, MapObject touchedObject)
    {
        activeRole.Touch(touchedObject);

        Console.WriteLine($"{activeRole} touch {touchedObject}");

        if (touchedObject is not Treasure)
            return;

        RemoveObject(touchedObject);
    }

    private void UpdateRolePosition(Role role, Coordinate newPosition)
    {
        var currentPosition = role.Position;

        if (currentPosition == null)
        {
            throw new InvalidOperationException("The role is not in the map.");
        }

        role.Position = newPosition;

        _positionManager.Remove(currentPosition);

        _positionManager.Add(newPosition, role);
    }

    private static MapObject? FindFirstObstacle(
        List<MapObject> obstaclesInSight,
        Coordinate characterPosition
    )
    {
        return obstaclesInSight.MinBy(obstacle =>
        {
            var obstaclePosition = obstacle.Position;

            if (obstaclePosition == null)
            {
                throw new InvalidOperationException("Obstacle not found.");
            }

            return characterPosition.DistanceTo(obstaclePosition);
        });
    }

    private static List<MapObject> MonstersInRange(
        List<MapObject> candidates,
        MapObject? firstObstacle,
        Coordinate characterPosition
    )
    {
        return candidates
            .Where(monster =>
            {
                var monsterPosition = monster.Position;

                if (monsterPosition == null)
                {
                    throw new InvalidOperationException("Monster not found.");
                }

                if (firstObstacle == null)
                {
                    return true;
                }

                var obstaclePosition = firstObstacle.Position;

                if (obstaclePosition == null)
                {
                    throw new InvalidOperationException("Obstacle not found.");
                }

                return characterPosition.DistanceTo(monsterPosition)
                    < characterPosition.DistanceTo(obstaclePosition);
            })
            .ToList();
    }

    private static List<MapObject> FindObjectsInCharacterDirection(
        Character character,
        params MapObject[] mapObjs
    )
    {
        var characterPosition = character.Position;
        var theDirectionOfCharacter = character.Direction;

        if (characterPosition == null)
        {
            throw new InvalidOperationException("Character not found.");
        }

        return mapObjs
            .Where(obj =>
            {
                var objPosition = obj.Position;

                if (objPosition == null)
                {
                    throw new InvalidOperationException("Monster not found.");
                }

                return theDirectionOfCharacter switch
                {
                    Direction.Up
                        => objPosition.XAxis == characterPosition.XAxis
                            && objPosition.YAxis < characterPosition.YAxis,
                    Direction.Down
                        => objPosition.XAxis == characterPosition.XAxis
                            && objPosition.YAxis > characterPosition.YAxis,
                    Direction.Left
                        => objPosition.YAxis == characterPosition.YAxis
                            && objPosition.XAxis < characterPosition.XAxis,
                    Direction.Right
                        => objPosition.YAxis == characterPosition.YAxis
                            && objPosition.XAxis > characterPosition.XAxis,
                };
            })
            .ToList();
    }

    public bool IsPositionOccupied(Coordinate position)
    {
        return _positionManager.ContainsKey(position);
    }

    private bool IsValidPosition(Coordinate position)
    {
        return position.XAxis >= 0
            && position.XAxis < Width
            && position.YAxis >= 0
            && position.YAxis < Height;
    }
}
