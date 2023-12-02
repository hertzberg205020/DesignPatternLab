using CardGameFramework.Infra.Models;
using CardGameFramework.Uno.Enums;

namespace CardGameFramework.Uno.Models;

public class UnoCard: Card<Color, Number>
{
    public UnoCard(Color category, Number value) : base(category, value)
    {
    }
    
    /// <summary>
    /// 是否符合Uno遊戲的出牌規則
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool MatchCard(UnoCard? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (ReferenceEquals(null, other)) return false;
        
        return Equals(Category, other.Category) || Equals(Value, other.Value);
    }
}