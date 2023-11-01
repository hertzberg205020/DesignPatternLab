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
        var (hero, fire) = src is Hero ? (src as Hero, desc) : (desc as Hero, src);
        
        // Hero 生命值減少 10 滴血
        hero.TakeDamage(10); 
        
        // Fire 從世界中被移除
        fire.World?.RemoveSprite(fire);
        
        // 如果src 為 Hero，src 移動成功
        if (src is Hero)
        {
            src.World?.MoveSprite(src, desc.PositionX);
        }
    }
}