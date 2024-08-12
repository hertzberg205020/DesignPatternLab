using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

public abstract class CollisionHandler
{
    protected CollisionHandler(CollisionHandler? next)
    {
        Next = next;
    }

    public CollisionHandler? Next { get; set; }

    public abstract SpriteType? ExpectedSpriteType1 { get; }

    public abstract SpriteType? ExpectedSpriteType2 { get; }

    public void Handle(Sprite src, Sprite dest)
    {
        if (IsMatch(src, dest))
        {
            if (src.Notation == ExpectedSpriteType1)
            {
                DoHandle(src, dest);
            }
            else
            {
                DoHandleInReverseOrder(src, dest);
            }
        }
        else
        {
            Next?.Handle(src, dest);
        }
    }

    protected abstract void DoHandle(Sprite src, Sprite dest);

    // the default implementation is to handle the collision in reverse order
    protected virtual void DoHandleInReverseOrder(Sprite src, Sprite dest) => DoHandle(dest, src);

    protected virtual bool IsMatch(Sprite src, Sprite dest)
    {
        var expectedTypes = new HashSet<SpriteType?> { ExpectedSpriteType1, ExpectedSpriteType2 };
        return expectedTypes.Count == 2
            && expectedTypes.Contains(src.Notation)
            && expectedTypes.Contains(dest.Notation);
    }
}
