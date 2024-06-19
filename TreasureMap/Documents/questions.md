# prompt

你是一位經驗豐富的軟體開發人員，擅長物件導向分析與設計。且樂於指導別人。
等下請你根據提問者的問題，一步步引導他完成一個二維地圖冒險遊戲的設計。
這個題目主要是用來學習設計模式中的狀態模式(State Pattern)。
請你引導他以 C# 的 Console 應用程式為例，來完成這個遊戲的設計。
請先不要撰寫程式，先做物件導向分析與設計，請以 `mermaid` 最為你討論類別圖的輸出。當徹底設計完成後，學員回覆開始設計後，再開始撰寫程式。

```csharp
class Treasure
{

}

```

```csharp
class Role
{
    public int Hp { get; set; }

    public State State {get; set; }

    public void ChangeState(State state);

    public void Move();

    public void Attack();

    public void OnDamage(int damage);

    public void OnHealing(int hp);

    public void BeforeAction();

    public void OnTouch();

    // 每回合所要採取的行動
    public void TakeAction();

    public void FindAttackTarget();

    public Direction DecideMoveDirection();

    private void ShowMoveOptions();

    public void TakeActionPartial();

    public void AfterAction();

    public void ExecuteTurn()
    {
        OnTurnBegin();
        UpdateStateRounds();
        for (int i = 0; i < State.ActionCount; i++)
        {
            BeforeAction();
            PerformAction();
            AfterAction();
        }
        OnTurnEnd();
    }

    protected void UpdateStateRounds();

     protected virtual void OnTurnBegin()
    {
        while (StateEffectQueue.Count > 0)
        {
            StateEffectQueue.Dequeue().Execute(this);
        }
    }

    protected virtual void OnTurnEnd()
    {
        if (IsAccelerated)
        {
            AcceleratedTurnsLeft--;
            if (AcceleratedTurnsLeft <= 0)
            {
                IsAccelerated = false;
                State = new NormalState();
            }
        }
    }

    protected virtual void BeforeAction() { }
    protected virtual void AfterAction() { }
    protected abstract void PerformAction();

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (IsAccelerated)
        {
            IsAccelerated = false;
            AcceleratedTurnsLeft = 0;
            State = new NormalState();
        }
    }
}
```

```csharp
public class Game
{
    public void Start();

    public void RemoveMapObject(MapObject object);

    public void IsMapObjectExists(MapObject object);

    public void IsTouched(MapObject object1, MapObject object2);

    public void Render();

    public bool IsEnd();

    public void BeginRound();

    public void OnRound();

    public void OnCharacterRound();

    public void OnMonsterRound();

    public void ExecuteTurn();

    public void EndRound();

    public void OnGameEnd();
}
```

```csharp
public class State
{
    public int LeftRounds { get; set; }

    public int ActionCount { get; set; } = 1;

    public void Execute(Role role);
}
```

```csharp
```

讓我們討論在 OnTurnStart 還是 OnTurnEnd 中維護 State 的 LeftRounds。
如果我們在 OnTurnStart 中維護 LeftRounds,那麼當角色進入一個新的狀態時,它將在下一個回合開始時才開始倒數。這意味著,如果一個狀態的持續時間為 1 回合,那麼角色實際上會在該狀態下停留 2 個回合。
相反,如果我們在 OnTurnEnd 中維護 LeftRounds,那麼當角色進入一個新的狀態時,它將立即開始倒數,並在當前回合結束時減少 LeftRounds。這樣,如果一個狀態的持續時間為 1 回合,那麼角色將在該狀態下只停留 1 個回合。

我認為在 Role 內部新增一個 hook 函數以維護 LeftRounds 是更好的選擇，
其作用的時機為 OnTurnBegin 之前，
這樣可以更好地控制狀態的持續時間。
目前遇到幾個問題: 持續 2 回合的無敵狀態，持續 5 回合的治療狀態。
2回合的無敵狀態表示在進入無敵狀態後，接下來的兩個回合內，角色將不會受到任何傷害。可以理解為 2 輪內室無敵的。而 5 回合的治療狀態表示在進入治療狀態後，接下來的五個回合內，角色每回合將回復 10 點 HP。請問該如何維護 LeftRounds ?

```csharp
public void ExecuteTurn()
{
    OnTurnBegin();
    for (int i = 0; i < State.ActionCount; i++)
    {
        BeforeAction();
        PerformAction();
        AfterAction();
    }
    OnTurnEnd();
}
```
