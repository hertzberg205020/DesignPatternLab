namespace RpgGame.Models.GameComponent;

public class CurseRelationship : IRoleDeathObserver
{
    public readonly Role Caster;

    public readonly Role Target;

    public CurseRelationship(Role caster, Role target)
    {
        Caster = caster;
        Target = target;
    }

    /// <summary>
    /// 當受詛咒者死亡時且施咒者仍活著，施咒者的 HP 將會被加上受詛咒者的剩餘 MP。
    /// 每一位角色都可能受到多次詛咒。如果詛咒是來自於同一位施咒者，效果並不會疊加。
    /// </summary>
    public void OnRoleDeath()
    {
        if (Caster.IsDead())
        {
            return;
        }

        var mp = Target.MagicPoint;
        Caster.TakeHeal(mp);
    }

    public override bool Equals(object? obj)
    {
        if (obj is CurseRelationship other)
        {
            return Caster == other.Caster && Target == other.Target;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Caster, Target);
    }
}
