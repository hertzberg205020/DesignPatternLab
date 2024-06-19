using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class WaterHeroCollisionHandler: CollisionHandler
{
    public override SpriteType? SpriteType1 => SpriteType.Hero;
    
    public override SpriteType? SpriteType2 => SpriteType.Water;
    
    public WaterHeroCollisionHandler(CollisionHandler? next) : base(next)
    {
    }
    
    protected override void DoHandle(Sprite src, Sprite desc)
    {
        var (hero, water) = (src as Hero, desc);
        
        // Hero 生命值增加 10 滴血
        hero!.TakeDamage(-10);
        
        // Water 從世界中被移除
        water.World?.RemoveSprite(water);
        
        // 如果src 為 Hero，src 移動成功
        src.World?.MoveSprite(src, desc.PositionX);
    }
    
    protected override void DoHandleInReverseOrder(Sprite src, Sprite desc)
    {
        var (hero, water) = (desc as Hero, src);
        
        // Hero 生命值增加 10 滴血
        hero!.TakeDamage(-10);
        
        // Water 從世界中被移除
        water.World?.RemoveSprite(water);
    }
}