using TreasureMap.Models.MapComponents;

namespace TreasureMap.Models.Roles;

public class Monster : Role
{
    public override char Symbol => 'M';

    public override int MaxHp { get; set; } = 300;
    public override int Hp { get; set; } = 1;

    /// <summary>
    /// 主角沒有位於怪物的攻擊範圍之內的話，
    /// 怪物將會自主決定要往哪一個方向移動一格，
    /// 否則怪物會站在原地攻擊主角。
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public override string TakeDecision(params string[] options)
    {
        // 判斷角色是否在怪物的攻擊範圍之內
        var character = GameMap?.Character;

        ArgumentNullException.ThrowIfNull(Position);

        var isInAttackRange =
            character is { Position: not null } && Position.IsNextTo(character.Position);
        return isInAttackRange ? "Attack" : "Move";
    }

    public override Direction MoveDirection()
    {
        // use Random.Shared.Next() to generate a random number
        var directions = new List<Direction>
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right
        };
        return directions[Random.Shared.Next(0, directions.Count)];
    }
}
