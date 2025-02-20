using RpgGame.Models.GameComponent.IO;
using RpgGame.Models.GameComponent.States;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class SelfExplosion : Skill
{
    public SelfExplosion(IGameIO gameIO)
        : base("自爆", 200, TargetType.All, gameIO) { }

    /// <summary>
    /// 戰場上所有角色
    /// </summary>
    /// <param name="game"></param>
    /// <param name="self"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(self, nameof(self));

        // 不包含自己
        return game.GetRoles().Count(r => r.IsAlive() && r != self);
    }

    /// <summary>
    /// 行動角色自殺(行動角色生命值歸零)，
    /// 並且對戰場上所有角色造成 150 點傷害。
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    public override void Apply(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game, nameof(game));
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        foreach (var role in targets)
        {
            executant.Attack(role, 150);
        }

        executant.EnterState(new DeadState(GameIO));
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant, nameof(executant));
        ArgumentNullException.ThrowIfNull(targets, nameof(targets));

        // [1]英雄 對 [1]A, [1]B, [1]C, [1]D, [1]E, [1]F, [1]G, [1]H, [1]I, [2]A, [2]B, [2]C, [2]D, [2]E, [2]F, [2]G, [2]H, [2]I 使用了 自爆。

        return $"{executant} 對 {string.Join(", ", targets)} 使用了 {Name}。";
    }
}
