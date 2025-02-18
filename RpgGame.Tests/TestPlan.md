# 測試計畫

## 單元測試

+ Role
    - IsDead
    - TakeDamage
    - Attack
    - TakeHeal
    - EnterState
    - 離開遊戲
    - 基礎攻擊機制
+ GameAction
    - Skill
+ State
+ DecisionMaker
+ Game

針對 PoisonedState 以及其他具又維持時間的狀態，在遭遇技能進入特殊狀態時，都會重新產生出新的狀態物件。
所以並不需要再 State 的 EnterState 方法中調整狀態的持續時間。
