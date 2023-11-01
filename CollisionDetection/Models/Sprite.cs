namespace CollisionDetection.Models;

public class Sprite
{
    public readonly SpriteType Notation;
    
    public int PositionX { get; set; }

    public World? World { get; set; }

    public Sprite(SpriteType notation)
    {
        Notation = notation;
    }
    
    public Sprite(SpriteType notation, int positionX) : this(notation)
    {
        PositionX = positionX;
    }

    public override string ToString()
    {
        return $"{Notation} on ({PositionX})";
    }
}