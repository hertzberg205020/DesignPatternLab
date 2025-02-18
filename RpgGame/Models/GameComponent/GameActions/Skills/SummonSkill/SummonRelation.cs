namespace RpgGame.Models.GameComponent.GameActions.Skills.SummonSkill;

public class SummonRelation : IRoleDeathObserver
{
    private readonly Role _summoner;

    private readonly Slime _summonedSlime;

    public SummonRelation(Role summoner, Slime summonedSlime)
    {
        _summoner = summoner;
        _summonedSlime = summonedSlime;
    }

    /// <summary>
    /// 當史萊姆死亡時且召喚者仍活著，
    /// 召喚者可以增加 30 點 HP
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public void OnRoleDeath()
    {
        if (_summoner.IsDead())
        {
            return;
        }

        _summoner.TakeHeal(30);
    }
}
