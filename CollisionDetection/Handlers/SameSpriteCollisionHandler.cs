using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public class SameSpriteCollisionHandler: CollisionHandler
{
    public override SpriteType? ExpectedSpriteType1 => null;
    
    public override SpriteType? ExpectedSpriteType2 => null;
    
    public SameSpriteCollisionHandler(CollisionHandler? next) : base(next)
    {
    }

    protected override bool IsMatch(Sprite src, Sprite dest)
    {
        return src.Notation == dest.Notation;
    }

    protected override void DoHandle(Sprite src, Sprite dest)
    {
        Console.WriteLine($"Move fail");
    }
}