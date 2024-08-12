using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class FireHeroCollisionHandler: CollisionHandler
{
    public override SpriteType? ExpectedSpriteType1 => SpriteType.Hero;
    
    public override SpriteType? ExpectedSpriteType2 => SpriteType.Fire;
    
    public FireHeroCollisionHandler(CollisionHandler? next) : base(next)
    {
    }
    
    protected override void DoHandle(Sprite src, Sprite dest)
    {
        var (hero, fire) = (src as Hero, desc: dest);
        // Hero 生命值減少 10 滴血
        hero.TakeDamage(10); 
        
        // Fire 從世界中被移除
        fire.World?.RemoveSprite(fire);
        src.World?.MoveSprite(src, dest.PositionX);
    }

    protected override void DoHandleInReverseOrder(Sprite src, Sprite dest)
    {
        var (hero, fire) = (dest as Hero, src);
        // Hero 生命值減少 10 滴血
        hero.TakeDamage(10); 
        // Fire 從世界中被移除
        fire.World?.RemoveSprite(fire);
    }
}