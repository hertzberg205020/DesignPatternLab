using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class FireHeroCollisionHandler: CollisionHandler
{
    public override SpriteType? SpriteType1 => SpriteType.Hero;
    
    public override SpriteType? SpriteType2 => SpriteType.Fire;
    
    public FireHeroCollisionHandler(CollisionHandler? next) : base(next)
    {
    }
    
    protected override void DoHandle(Sprite src, Sprite desc)
    {
        var (hero, fire) = (src as Hero, desc);
        // Hero 生命值減少 10 滴血
        hero.TakeDamage(10); 
        
        // Fire 從世界中被移除
        fire.World?.RemoveSprite(fire);
        src.World?.MoveSprite(src, desc.PositionX);
    }

    protected override void DoHandleInReverseOrder(Sprite src, Sprite desc)
    {
        var (hero, fire) = (desc as Hero, src);
        // Hero 生命值減少 10 滴血
        hero.TakeDamage(10); 
        // Fire 從世界中被移除
        fire.World?.RemoveSprite(fire);
    }
}