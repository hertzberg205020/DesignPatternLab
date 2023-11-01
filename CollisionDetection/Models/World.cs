using CollisionDetection.Handlers;

namespace CollisionDetection.Models;

public class World
{
    private readonly Dictionary<int, Sprite?> _map = new();
    private readonly CollisionHandler _collisionHandler;
    public const int X_AXIS_MIN_BOUNDARY = 0;
    public const int X_AXIS_MAX_BOUNDARY = 29;

    public World(CollisionHandler collisionHandler)
    {
        _collisionHandler = collisionHandler;
        for (int i = X_AXIS_MIN_BOUNDARY; i < X_AXIS_MAX_BOUNDARY; i++)
        {
            _map.Add(i, null!);
        }
    }

    public void AddSprite(Sprite sprite)
    {
        if (_map[sprite.PositionX] != null)
        {
            throw new ArgumentException($"Sprite already exists in position: {sprite.PositionX}");
        }

        _map[sprite.PositionX] = sprite;
        sprite.World = this;
    }

    public void RemoveSprite(Sprite sprite)
    {
        _map[sprite.PositionX] = null!;
        sprite.World = null;
        Console.WriteLine($"Remove sprite: {sprite}");
    }

    public void MoveSprite(Sprite src, int targetPositionX)
    {
        ValidateSprite(src);
        ValidateTargetPosition(targetPositionX);
        EnsureSpriteExistsInMap(src);

        if (src.PositionX == targetPositionX)
        {
            return;
        }

        HandleSpriteMovement(src, targetPositionX);
    }
    
    private void HandleSpriteMovement(Sprite src, int targetPositionX)
    {
        if (_map.TryGetValue(targetPositionX, out var dest))
        {
            if (dest != null)
            {
                _collisionHandler.Handle(src, dest);
            }
            else
            {
                _map[src.PositionX] = null;
                src.PositionX = targetPositionX;
                _map[src.PositionX] = src;
            }
        }
    }
    
    

    private void ValidateSprite(Sprite src)
    {
        if (src.World != this)
        {
            throw new ArgumentException($"Invalid sprite: {src}");
        }
    }

    private static void ValidateTargetPosition(int? position)
    {
        if (position is null)
        {
            throw new ArgumentException("position cannot be null");
        }

        if (position is < X_AXIS_MIN_BOUNDARY or >= X_AXIS_MAX_BOUNDARY)
        {
            throw new ArgumentException($"Invalid position: {position}");
        }
    }

    private void EnsureSpriteExistsInMap(Sprite src)
    {
        if (!_map.ContainsKey(src.PositionX) || _map[src.PositionX] != src)
        {
            throw new ArgumentException($"Sprite not found in current position: {src.PositionX}");
        }
    }
    
    public static List<Sprite> RandomlyDistributeSprites(IEnumerable<Sprite> sprites)
    {
        var random = new Random();
        var positions = Enumerable.Range(X_AXIS_MIN_BOUNDARY, X_AXIS_MAX_BOUNDARY).OrderBy(x => random.Next()).ToList();
        return sprites.Select((sprite, index) =>
        {
            sprite.PositionX = positions[index];
            return sprite;
        }).ToList();
    }

    public Sprite AskUserWhichSpriteToMove()
    {
        try
        {
            Console.WriteLine("Which sprite do you want to move?");
            DisplaySprites();
            var position = AskUserForPosition();
            return _map[position] ?? throw new ArgumentException($"Sprite not found in position: {position}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return AskUserWhichSpriteToMove();
        }
    }

    private int AskUserForPosition()
    {
        Console.WriteLine($"Please enter a position between {X_AXIS_MIN_BOUNDARY} and {X_AXIS_MAX_BOUNDARY - 1}");
        var position = Console.ReadLine();
        if (int.TryParse(position, out var result))
        {
            return result;
        }
        else
        {
            return AskUserForPosition();
        }
    }

    private void DisplaySprites()
    {
        Console.WriteLine("Current sprites:");
        foreach (var sprite in _map.Values)
        {
            if (sprite != null)
            {
                Console.WriteLine(sprite);
            }
        }
    }

    public int AskUserWhereToMoveSprite()
    {
        Console.WriteLine("Where do you want to move it to?");
        return AskUserForPosition();
    }
}