using CollisionDetection.Handlers;

namespace CollisionDetection.Models;

public class App
{
    private readonly World _world;
    
    public App()
    {
        // 建立一個世界
        var collisionHandler = new WaterFireCollisionHandler(new WaterHeroCollisionHandler(new FireHeroCollisionHandler(new SameSpriteCollisionHandler(null))));
        _world = new World(collisionHandler);
        
        // 初始有 10 個生命，每個生命都會被隨機賦予一個初始的座標
        var sprites = SpriteFactory.CreateRandomSprites(10);
        World.RandomlyDistributeSprites(sprites);
        
        // 將這些生命加入世界
        sprites.ForEach(s => _world.AddSprite(s));
    }
    
    public void Run()
    {
        while (true)
        {
            // 詢問使用者要移動哪個生命
            var sprite = _world.AskUserWhichSpriteToMove();
            
            // 詢問使用者要移動到哪個位置
            var targetPosition = _world.AskUserWhereToMoveSprite();
            
            // 移動生命
            _world.MoveSprite(sprite, targetPosition);
            Console.WriteLine("===");
        }
    }
    
}