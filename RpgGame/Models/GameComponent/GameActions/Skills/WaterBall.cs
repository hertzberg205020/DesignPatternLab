using RpgGame.Models.GameLogic;

namespace RpgGame.Models.GameComponent.GameActions.Skills;

public class WaterBall : Skill
{
    public WaterBall()
        : base("水球", 50, TargetType.Enemy) { }

    public override int GetRequiredTargetCount(Game game, Role self) => 1;

    /// <summary>
    /// 對目標角色造成 120 點傷害
    /// </summary>
    /// <param name="game"></param>
    /// <param name="executant"></param>
    /// <param name="targets"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void DoExecute(Game game, Role executant, List<Role> targets)
    {
        var target = targets.Single();
        executant.Attack(target, 120);
    }

    public override string FormatExecuteMessage(Role executant, List<Role> targets)
    {
        var target = targets.Single();

        // [1]英雄 對 [2]Slime2 使用了 水球。
        return $"{executant.Name} 對 {target.Name} 使用了 {Name}。";
    }
}