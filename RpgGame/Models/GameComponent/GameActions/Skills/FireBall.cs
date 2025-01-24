using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class FireBall : Skill
{
    public FireBall()
        : base("火球", 50, TargetType.Enemy) { }

    /// <summary>
    /// 所有存活敵軍數量
    /// </summary>
    /// <param name="game"></param>
    /// <param name="self"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override int GetRequiredTargetCount(Game game, Role self)
    {
        ArgumentNullException.ThrowIfNull(self);

        ArgumentNullException.ThrowIfNull(game);

        return game.GetEnemies(self).Count(e => e.IsAlive());
    }

    /// <summary>
    /// 對所有目標角色造成 50 點傷害
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(game);

        ArgumentNullException.ThrowIfNull(executant);

        ArgumentNullException.ThrowIfNull(targets);

        foreach (var target in targets)
        {
            executant.Attack(target, 50);
        }
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        ArgumentNullException.ThrowIfNull(executant);

        ArgumentNullException.ThrowIfNull(targets);

        var targetNames = targets.Select(r => r.Name).ToArray();

        // [1]英雄 對 [2]Slime1, [2]Slime2 使用了 火球。
        return $"{executant.Name} 對 {string.Join(", ", targetNames)} 使用了 {Name}。";
    }
}
