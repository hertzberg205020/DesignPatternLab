using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class WaterFireCollisionHandler: CollisionHandler
{
    public override SpriteType? ExpectedSpriteType1 => SpriteType.Fire;
    
    public override SpriteType? ExpectedSpriteType2 => SpriteType.Water;
    
    public WaterFireCollisionHandler(CollisionHandler? next) : base(next)
    {
    }
    
    protected override void DoHandle(Sprite src, Sprite dest)
    {
        // Water 從世界中被移除。
        src.World?.RemoveSprite(src);
        // Fire 從世界中被移除。
        dest.World?.RemoveSprite(dest);
    }
}