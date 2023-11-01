using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class SameSpriteCollisionHandler: CollisionHandler
{
    public override SpriteType? SpriteType1 => null;
    
    public override SpriteType? SpriteType2 => null;
    
    public SameSpriteCollisionHandler(CollisionHandler? next) : base(next)
    {
    }

    protected override bool IsMatch(Sprite src, Sprite desc)
    {
        return src.Notation == desc.Notation;
    }

    protected override void DoHandle(Sprite src, Sprite desc)
    {
        Console.WriteLine($"Move fail");
    }
}