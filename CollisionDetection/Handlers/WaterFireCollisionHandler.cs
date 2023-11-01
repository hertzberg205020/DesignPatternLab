using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class WaterFireCollisionHandler: CollisionHandler
{
    public override SpriteType? SpriteType1 => SpriteType.Fire;
    
    public override SpriteType? SpriteType2 => SpriteType.Water;
    
    public WaterFireCollisionHandler(CollisionHandler? next) : base(next)
    {
    }
    
    protected override void DoHandle(Sprite src, Sprite desc)
    {
        // Water 從世界中被移除。
        src.World?.RemoveSprite(src);
        // Fire 從世界中被移除。
        desc.World?.RemoveSprite(desc);
    }
}