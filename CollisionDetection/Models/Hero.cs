namespace CollisionDetection.Models;

public class Hero: Sprite
{
    public int Hp { get; private set; } = 30;
    
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            World?.RemoveSprite(this);
        }
    }

    public Hero(SpriteType notation) : base(notation)
    {
    }
}