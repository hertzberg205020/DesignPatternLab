using CollisionDetection.Models;

namespace CollisionDetection;

public static class SpriteFactory
{
    private static readonly Random Random = new Random();
    
    private static readonly Array SpriteTypes = Enum.GetValues(typeof(SpriteType));

    private static Sprite CreateRandomSprite()
    {
        // 隨機選擇一個SpriteType
        SpriteType randomType = (SpriteType)(SpriteTypes.GetValue(Random.Next(SpriteTypes.Length)) ?? throw new InvalidOperationException());

        return randomType switch
        {
            SpriteType.Hero => new Hero(randomType),
            SpriteType.Fire => new Sprite(randomType),
            SpriteType.Water => new Sprite(randomType),
            _ => throw new ArgumentException("Unknown sprite type")
        };
    }

    public static List<Sprite> CreateRandomSprites(int count)
    {
        var sprites = new List<Sprite>();
        for(var i = 0; i < count; i++)
        {
            sprites.Add(CreateRandomSprite());
        }
        return sprites;
    }
}