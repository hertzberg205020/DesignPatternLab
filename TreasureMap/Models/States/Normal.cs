using TreasureMap.Models.Roles;

namespace TreasureMap.Models.States;

public class Normal : State
{
    public Normal(Role role)
        : base(role, "正常") { }
}
