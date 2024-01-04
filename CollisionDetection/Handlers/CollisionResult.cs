using CollisionDetection.Models;

namespace CollisionDetection.Handlers;

/// <summary>
/// 不建立 Sprite 與 World 的雙向關聯就需要碰撞結果來告知 World 要不要移除 Sprite
/// </summary>
public class CollisionResult
{
    public readonly bool IsSuccess;

    public Sprite? RemovedSprite { get; set; }
    
    public CollisionResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }
}