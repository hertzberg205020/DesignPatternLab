using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class WaterHeroCollisionHandler: CollisionHandler
{
    public override SpriteType? SpriteType1 => SpriteType.Water;
    
    public override SpriteType? SpriteType2 => SpriteType.Hero;
    
    public WaterHeroCollisionHandler(CollisionHandler? next) : base(next)
    {
    }
    
    protected override void DoHandle(Sprite src, Sprite desc)
    {
        var (hero, water) = src is Hero ? (src as Hero, desc) : (desc as Hero, src);
        
        // Hero 生命值增加 10 滴血
        hero!.TakeDamage(-10);
        
        // Water 從世界中被移除
        water.World?.RemoveSprite(water);
        
        // 如果src 為 Hero，src 移動成功
        if (src is Hero)
        {
            src.World?.MoveSprite(src, desc.PositionX);
        }
    }
}