using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public abstract class CollisionHandler
{
    public CollisionHandler(CollisionHandler? next)
    {
        Next = next;
    }

    public CollisionHandler? Next { get; set; }

    public abstract SpriteType? SpriteType1 { get; }

    public abstract SpriteType? SpriteType2 { get; }

    public void Handle(Sprite src, Sprite desc)
    {
        if (IsMatch(src, desc))
        {
            if (src.Notation == SpriteType1)
            {
                DoHandle(src, desc);
            }
            else
            {
                DoHandleInReverseOrder(src, desc);
            }
        }
        else
        {
            Next?.Handle(src, desc);
        }
    }

    protected abstract void DoHandle(Sprite src, Sprite desc);
    // the default implementation is to handle the collision in reverse order
    protected virtual void DoHandleInReverseOrder(Sprite src, Sprite desc) => DoHandle(desc, src);

    protected virtual bool IsMatch(Sprite src, Sprite desc)
    {
        var spriteType1 = SpriteType1;
        var spriteType2 = SpriteType2;
        var coupledSprites = new HashSet<SpriteType?>() { spriteType1, spriteType2 };

        return coupledSprites.Count == 2 &&
               coupledSprites.Contains(src.Notation) &&
               coupledSprites.Contains(desc.Notation);
    }
}