using RpgGame.Models;
using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent;

public class RoleTests
{
    private readonly TestGameIO _gameIO;

    public RoleTests()
    {
        _gameIO = new TestGameIO(new[] { "0" }); // 預設輸入
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange
        string name = "TestHero";
        int hp = 100;
        int mp = 50;
        int str = 30;
        bool isHero = true;
        var decisionMaker = new HumanDecisionMaker(_gameIO);

        // Act
        var role = new Role(_gameIO)
        {
            Name = name,
            HealthPoint = hp,
            MagicPoint = mp,
            Strength = str,
            IsHero = isHero,
            DecisionMaker = decisionMaker
        };

        // Assert
        Assert.Equal(name, role.Name);
        Assert.Equal(hp, role.HealthPoint);
        Assert.Equal(mp, role.MagicPoint);
        Assert.Equal(str, role.Strength);
        Assert.Equal(isHero, role.IsHero);
        Assert.Equal(decisionMaker, role.DecisionMaker);
        Assert.Null(role.State); // 初始狀態應為 null
        Assert.Null(role.Game); // 初始遊戲參考應為 null
        Assert.Null(role.Troop); // 初始軍隊應為 null
    }

    [Theory]
    [InlineData(0, true)] // 邊界值：剛好死亡
    [InlineData(-1, true)] // 負血量：死亡
    [InlineData(1, false)] // 最小存活血量
    [InlineData(100, false)] // 正常血量：存活
    public void IsDead_ShouldReturnCorrectState(int hp, bool expectedIsDead)
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = hp,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act
        var isDead = role.IsDead();
        var isAlive = role.IsAlive();

        // Assert
        Assert.Equal(expectedIsDead, isDead);
        Assert.Equal(!expectedIsDead, isAlive); // IsAlive 應該永遠與 IsDead 相反
    }

    [Fact]
    public void IsAlive_ShouldBeOppositeOfIsDead()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act & Assert
        Assert.Equal(!role.IsDead(), role.IsAlive());

        // 改變血量後再次驗證
        role.HealthPoint = 0;
        Assert.Equal(!role.IsDead(), role.IsAlive());

        role.HealthPoint = -1;
        Assert.Equal(!role.IsDead(), role.IsAlive());
    }

    [Theory]
    [InlineData(100, 30, 70)] // 正常傷害
    [InlineData(100, 100, 0)] // 剛好致死傷害
    [InlineData(100, 150, 0)] // 超額傷害
    public void TakeDamage_ShouldReduceHealthPointCorrectly(
        int initialHp,
        int damage,
        int expectedHp
    )
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = initialHp,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        // Act
        role.TakeDamage(damage);

        // Assert
        Assert.Equal(expectedHp, role.HealthPoint);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void TakeDamage_WithNegativeDamage_ShouldThrowException(int damage)
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => role.TakeDamage(damage));
        Assert.Contains("should not be negative", exception.Message);
    }

    [Fact]
    public void TakeDamage_WhenDamageKillsRole_ShouldEnterDeadState()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        // Act
        role.TakeDamage(100); // 造成致死傷害

        // Assert
        Assert.True(role.IsDead());
        Assert.NotNull(role.State);
        Assert.IsType<DeadState>(role.State);
    }

    [Fact]
    public void TakeDamage_WhenNotKilled_ShouldNotChangeState()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act
        role.TakeDamage(50); // 造成非致死傷害

        // Assert
        Assert.False(role.IsDead());
        Assert.Null(role.State); // 初始狀態應該維持為 null
    }

    [Theory]
    [InlineData(50, 30, 80)] // 正常治療
    [InlineData(1, 50, 51)] // 從瀕死狀態治療
    [InlineData(100, 50, 150)] // 超過初始血量的治療
    public void TakeHeal_ShouldIncreaseHealthPointCorrectly(int initialHp, int heal, int expectedHp)
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = initialHp,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act
        role.TakeHeal(heal);

        // Assert
        Assert.Equal(expectedHp, role.HealthPoint);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void TakeHeal_WithNegativeHeal_ShouldThrowException(int heal)
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => role.TakeHeal(heal));
        Assert.Contains("should not be negative", exception.Message);
    }

    [Fact]
    public void TakeHeal_OnDeadRole_ShouldStillIncreaseHP()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 0, // 死亡狀態
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        role.EnterState(new DeadState(_gameIO)); // 確保進入死亡狀態

        // Act
        role.TakeHeal(50);

        // Assert
        Assert.Equal(50, role.HealthPoint);
        Assert.IsType<DeadState>(role.State); // 確認狀態不會改變
    }

    [Fact]
    public void TakeHeal_WithZeroHeal_ShouldNotChangeHP()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var initialHp = role.HealthPoint;

        // Act
        role.TakeHeal(0);

        // Assert
        Assert.Equal(initialHp, role.HealthPoint);
    }

    #region EnterState

    [Fact]
    public void EnterState_FromNullState_ShouldSetNewState()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var newState = new PetrifiedState(_gameIO);

        // Act
        role.EnterState(newState);

        // Assert
        Assert.Same(newState, role.State);
        Assert.Same(role, newState.Role);
    }

    // 用於測試的 Mock State 類別
    private class MockState : State
    {
        public bool EnterStateCalled { get; private set; }
        public bool ExitStateCalled { get; private set; }

        public MockState(string name, IGameIO gameIO)
            : base(name, gameIO)
        {
            EnterStateCalled = false;
            ExitStateCalled = false;
        }

        public override void EnterState()
        {
            EnterStateCalled = true;
            base.EnterState();
        }

        public override void ExitState()
        {
            ExitStateCalled = true;
            base.ExitState();
        }
    }

    [Fact]
    public void EnterState_FromExistingState_ShouldCallExitAndEnterMethods()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var mockOldState = new MockState("OldState", _gameIO);
        var mockNewState = new MockState("NewState", _gameIO);

        role.EnterState(mockOldState);

        // Act
        role.EnterState(mockNewState);

        // Assert
        Assert.True(mockOldState.ExitStateCalled, "ExitState should be called on old state");
        Assert.True(mockNewState.EnterStateCalled, "EnterState should be called on new state");
        Assert.Same(mockNewState, role.State);
        Assert.Same(role, mockNewState.Role);
    }

    [Fact]
    public void EnterState_WithNullState_ShouldThrowException()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => role.EnterState(null));
    }

    [Fact]
    public void EnterState_ShouldMaintainStateConsistency()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var state1 = new PetrifiedState(_gameIO);
        var state2 = new PetrifiedState(_gameIO);

        // Act
        role.EnterState(state1);
        role.EnterState(state2);

        // Assert
        Assert.Same(state2, role.State);
        Assert.Same(role, state2.Role);
        Assert.Null(state1.Role); // 舊狀態應該不再引用 role
    }

    #endregion

    #region Attack

    [Fact]
    public void Attack_InNormalState_ShouldDealBaseDamage()
    {
        var game = new Game(_gameIO);

        // Arrange
        var attacker = new Role(_gameIO)
        {
            Name = "Attacker",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        attacker.EnterState(new NormalState(_gameIO));
        game.AddRole("A", attacker);

        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 20,
            Troop = "B",
            IsHero = false
        };
        target.EnterState(new NormalState(_gameIO));
        game.AddRole("B", target);

        var initialTargetHp = target.HealthPoint;
        var outputs = _gameIO.Outputs;

        // Act
        attacker.Attack(target, attacker.Strength);

        // Assert
        Assert.Equal(initialTargetHp - attacker.Strength, target.HealthPoint);
        Assert.Contains($"{attacker} 對 {target} 造成 {attacker.Strength} 點傷害", outputs[0]);
    }

    [Fact]
    public void Attack_WithNullTarget_ShouldThrowException()
    {
        // Arrange
        var attacker = new Role(_gameIO)
        {
            Name = "Attacker",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        attacker.EnterState(new NormalState(_gameIO));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => attacker.Attack(null, attacker.Strength));
    }

    [Fact]
    public void Attack_WithoutState_ShouldThrowException()
    {
        // Arrange
        var attacker = new Role(_gameIO)
        {
            Name = "Attacker",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 20,
            IsHero = false
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => attacker.Attack(target, attacker.Strength));
    }

    [Theory]
    [InlineData(-1)]
    public void Attack_WithInvalidDamage_ShouldThrowException(int damage)
    {
        // Arrange
        var attacker = new Role(_gameIO)
        {
            Name = "Attacker",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 20,
            IsHero = false
        };
        attacker.EnterState(new NormalState(_gameIO));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => attacker.Attack(target, damage));
        Assert.Contains("should not be negative", exception.Message);
    }

    [Fact]
    public void Attack_InPetrifiedState_ShouldDealNormalDamage()
    {
        // Arrange
        var attacker = new Role(_gameIO)
        {
            Name = "Attacker",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 20,
            Troop = "B",
            IsHero = false
        };

        // attacker 進入一般狀態
        attacker.EnterState(new NormalState(_gameIO));

        // 目標進入石化狀態
        target.EnterState(new PetrifiedState(_gameIO));
        var initialTargetHp = target.HealthPoint;

        // Act
        attacker.Attack(target, attacker.Strength);

        // Assert
        Assert.Equal(initialTargetHp - attacker.Strength, target.HealthPoint); // 正常傷害
        Assert.IsType<PetrifiedState>(target.State); // 確認目標仍在石化狀態
        Assert.False(target.State.CanTakeAction); // 確認目標無法行動
        Assert.Equal(3, target.State.LeftRounds); // 確認剩餘回合數為3
    }

    [Fact]
    public void Attack_WhenPetrified_ShouldNotBeAbleToAttack()
    {
        // Arrange
        var attacker = new Role(_gameIO)
        {
            Name = "Attacker",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 20,
            IsHero = false
        };

        // 攻擊者進入石化狀態
        attacker.EnterState(new PetrifiedState(_gameIO));
        var initialTargetHp = target.HealthPoint;

        // Act & Assert
        // 由於石化狀態下無法行動，應該會拋出異常
        var exception = Assert.Throws<InvalidOperationException>(
            () => attacker.Attack(target, attacker.Strength)
        );
        Assert.Contains("cannot take action", exception.Message);

        // 確認目標沒有受到傷害
        Assert.Equal(initialTargetHp, target.HealthPoint);

        // 確認攻擊者狀態
        Assert.IsType<PetrifiedState>(attacker.State);
        Assert.False(attacker.State.CanTakeAction);
        Assert.Equal(3, attacker.State.LeftRounds);
    }

    #endregion

    #region TakeTurn



    [Fact]
    public void TakeTurn_WithoutState_ShouldThrowException()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => role.TakeTurn());
    }

    [Fact]
    public void TakeTurn_WhenCannotTakeAction_ShouldSkipActionExecution()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var mockState = new MockStateWithActionControl(false, _gameIO);
        role.EnterState(mockState);

        // Act
        role.TakeTurn();

        // Assert
        Assert.True(mockState.BeforeTakeActionCalled, "BeforeTakeAction should be called");
        Assert.True(mockState.AfterTakeActionCalled, "AfterTakeAction should be called");
        Assert.False(
            mockState.ActionExecuted,
            "Action should not be executed when CanTakeAction is false"
        );
    }

    [Theory]
    [InlineData(0)] // 不需要目標
    [InlineData(1)] // 需要1個目標，小於候選目標數，需要選擇
    [InlineData(3)] // 需要3個目標，等於候選目標數
    [InlineData(4)] // 需要4個目標，大於候選目標數
    public void TakeTurn_WhenCanTakeAction_ShouldExecuteAction(int requiredTargetCount)
    {
        // Arrange
        var mockAction = new MockActionWithTargetCount(requiredTargetCount, _gameIO);
        var mockTargets = new List<Role>();
        var mockDecisionMaker = new MockDecisionMaker(mockAction, mockTargets);
        var game = new Game(_gameIO);

        // 建立主要角色
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true,
            DecisionMaker = mockDecisionMaker,
            Game = game
        };

        // 建立三個敵方角色
        var enemies = Enumerable
            .Range(1, 3)
            .Select(i => new Role(_gameIO)
            {
                Name = $"Enemy{i}",
                HealthPoint = 100,
                MagicPoint = 50,
                Strength = 30,
                IsHero = false,
            })
            .ToList();

        // 將角色加入遊戲
        game.AddRole("A", role);

        foreach (var enemy in enemies)
        {
            game.AddRole("B", enemy);
        }

        // 設置狀態
        var mockState = new MockStateWithActionControl(true, _gameIO);
        role.EnterState(mockState);

        // Act
        role.TakeTurn();

        // Assert
        Assert.True(mockState.BeforeTakeActionCalled, "BeforeTakeAction should be called");
        Assert.True(mockState.AfterTakeActionCalled, "AfterTakeAction should be called");
        Assert.True(mockDecisionMaker.SelectActionCalled, "SelectAction should be called");

        // 驗證目標選擇邏輯
        var candidates = enemies; // 在這個測試中，候選目標就是敵人清單
        if (requiredTargetCount == 0 || requiredTargetCount >= candidates.Count)
        {
            Assert.False(
                mockDecisionMaker.SelectTargetsCalled,
                $"SelectTargets should not be called when requiredTargetCount={requiredTargetCount} and candidates.Count={candidates.Count}"
            );
        }
        else
        {
            Assert.True(
                mockDecisionMaker.SelectTargetsCalled,
                $"SelectTargets should be called when requiredTargetCount={requiredTargetCount} and candidates.Count={candidates.Count}"
            );
        }
    }

    #endregion

    #region ChooseAction

    [Fact]
    public void ChooseAction_WithNullDecisionMaker_ShouldThrowException()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true,
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => role.ChooseAction());
    }

    // Mock classes for testing
    private class MockActionWithMpCost : GameAction
    {
        public bool WasExecuted { get; private set; }

        public MockActionWithMpCost(int mpCost, IGameIO gameIO)
            : base("mockAction", mpCost, TargetType.Enemy, gameIO)
        {
            WasExecuted = false;
        }

        public override int GetRequiredTargetCount(Game game, Role self) => 1;

        public override void Apply(Game game, Role executant, List<Role> targets)
        {
            WasExecuted = true;
        }

        public override string FormatExecuteMessage(Role executant, List<Role> targets)
        {
            return string.Empty;
        }
    }

    [Fact]
    public void ChooseAction_WithInsufficientMP_ShouldRetryUntilValidAction()
    {
        // Arrange
        var highMpAction = new MockActionWithMpCost(100, _gameIO); // 需要100 MP
        var lowMpAction = new MockActionWithMpCost(20, _gameIO); // 需要20 MP
        var mockDecisionMaker = new MockDecisionMakerWithActionSequence(
            new[] { highMpAction, lowMpAction }
        );

        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50, // 只有50 MP
            Strength = 30,
            IsHero = true,
            DecisionMaker = mockDecisionMaker,
        };

        // Act
        var selectedAction = role.ChooseAction();

        // Assert
        Assert.Same(lowMpAction, selectedAction);
        Assert.Equal(2, mockDecisionMaker.SelectActionCallCount);
    }

    #endregion

    #region PerformAction

    [Fact]
    public void PerformAction_ShouldDeductMPAndExecuteAction()
    {
        // Arrange
        var game = new Game(_gameIO);
        var mockAction = new MockActionWithMpCost(30, _gameIO);
        var targets = new List<Role>();

        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true,
            Game = game
        };

        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = false
        };

        game.AddRole("A", role);
        game.AddRole("B", target);
        targets.Add(target);

        // Act
        role.PerformAction(mockAction, targets);

        // Assert
        Assert.Equal(20, role.MagicPoint); // 50 - 30 = 20
        Assert.True(mockAction.WasExecuted);
    }

    [Fact]
    public void PerformAction_WithNullGame_ShouldThrowException()
    {
        // Arrange
        var mockAction = new MockActionWithMpCost(30, _gameIO);
        var targets = new List<Role>();

        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => role.PerformAction(mockAction, targets));
    }

    #endregion

    #region Observer Pattern

    [Fact]
    public void RegisterAndNotifyDeathObservers_ShouldWorkCorrectly()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        var mockObserver1 = new MockDeathObserver();
        var mockObserver2 = new MockDeathObserver();

        // Act
        role.RegisterDeathObserver(mockObserver1);
        role.RegisterDeathObserver(mockObserver2);
        role.NotifyDeathObservers();

        // Assert
        Assert.True(mockObserver1.WasNotified);
        Assert.True(mockObserver2.WasNotified);
        Assert.Equal(2, role.GetDeathObservers().Count());
    }

    #endregion

    #region ObtainCandidates

    [Theory]
    [InlineData(TargetType.Self)]
    [InlineData(TargetType.None)]
    public void ObtainCandidates_WithSelfOrNoneTarget_ShouldReturnEmptyList(TargetType targetType)
    {
        // Arrange
        var game = new Game(_gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true,
            Game = game
        };
        var action = new MockActionWithTargetType(targetType, _gameIO);

        // Act
        var candidates = role.ObtainCandidates(action);

        // Assert
        Assert.Empty(candidates);
    }

    [Fact]
    public void ObtainCandidates_WithNullGame_ShouldThrowException()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };
        var action = new MockActionWithTargetType(TargetType.Enemy, _gameIO);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => role.ObtainCandidates(action));
    }

    [Theory]
    [InlineData(TargetType.Enemy)]
    [InlineData(TargetType.Ally)]
    [InlineData(TargetType.All)]
    public void ObtainCandidates_ShouldReturnCorrectTargets(TargetType targetType)
    {
        // Arrange
        var game = new Game(_gameIO);

        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true,
            Game = game
        };

        var ally1 = new Role(_gameIO)
        {
            Name = "Ally1",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        var ally2 = new Role(_gameIO)
        {
            Name = "Ally2",
            HealthPoint = 0, // 死亡的盟友
            MagicPoint = 50,
            Strength = 30,
            IsHero = true
        };

        var enemy1 = new Role(_gameIO)
        {
            Name = "Enemy1",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = false
        };

        var enemy2 = new Role(_gameIO)
        {
            Name = "Enemy2",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = false
        };

        game.AddRole("A", role);
        game.AddRole("A", ally1);
        game.AddRole("A", ally2);
        game.AddRole("B", enemy1);
        game.AddRole("B", enemy2);

        var action = new MockActionWithTargetType(targetType, _gameIO);

        // Act
        var candidates = role.ObtainCandidates(action);

        // Assert
        switch (targetType)
        {
            case TargetType.Enemy:
                Assert.Equal(2, candidates.Count);
                Assert.All(candidates, c => Assert.False(c.IsHero));
                break;
            case TargetType.Ally:
                Assert.Single(candidates);
                Assert.All(candidates, c => Assert.True(c.IsHero));
                Assert.All(candidates, c => Assert.True(c.IsAlive()));
                break;
            case TargetType.All:
                Assert.Equal(3, candidates.Count);
                Assert.Contains(candidates, c => c.IsHero);
                Assert.Contains(candidates, c => !c.IsHero);
                break;
        }
    }

    #endregion

    #region GenerateTargetList and ToString

    [Fact]
    public void GenerateTargetList_ShouldFormatCorrectly()
    {
        // Arrange
        var game = new Game(_gameIO);

        var targets = new List<Role>
        {
            new Role(_gameIO)
            {
                Name = "Hero1",
                HealthPoint = 100,
                MagicPoint = 50,
                Strength = 30,
                Troop = "A",
                IsHero = true
            },
            new Role(_gameIO)
            {
                Name = "Hero2",
                HealthPoint = 100,
                MagicPoint = 50,
                Strength = 30,
                Troop = "B",
                IsHero = true
            }
        };

        // Act
        var result = Role.GenerateTargetList(targets);

        // Assert
        Assert.Equal("(0) [A]Hero1 (1) [B]Hero2", result);
    }

    [Fact]
    public void ToString_ShouldFormatCorrectly()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestHero",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            IsHero = true,
            Troop = "A"
        };

        // Act
        var result = role.ToString();

        // Assert
        Assert.Equal("[A]TestHero", result);
    }

    #endregion

    // Mock decision maker for testing
    private class MockDecisionMaker : IDecisionMaker
    {
        private readonly GameAction _actionToReturn;
        public bool SelectActionCalled { get; private set; }
        public bool SelectTargetsCalled { get; private set; }

        public MockDecisionMaker(GameAction actionToReturn, List<Role> targetsToReturn)
        {
            _actionToReturn = actionToReturn;
            SelectActionCalled = false;
            SelectTargetsCalled = false;
        }

        public GameAction SelectAction(List<GameAction> actions)
        {
            SelectActionCalled = true;
            return _actionToReturn;
        }

        public List<Role> SelectTargets(List<Role> candidates, int requiredCount)
        {
            SelectTargetsCalled = true;
            return candidates.Take(requiredCount).ToList();
        }
    }

    private class DoNothingAction : GameAction
    {
        public DoNothingAction(IGameIO gameIO)
            : base("doNothing", 1, TargetType.Enemy, gameIO) { }

        public override int GetRequiredTargetCount(Game game, Role self) => 1;

        public override void Apply(Game game, Role executant, List<Role> targets) { }

        public override string FormatExecuteMessage(Role executant, List<Role> targets)
        {
            return string.Empty;
        }
    }

    private class MockActionWithTargetCount : GameAction
    {
        private readonly int _requiredTargetCount;

        public MockActionWithTargetCount(int requiredTargetCount, IGameIO gameIO)
            : base("mockAction", 0, TargetType.Enemy, gameIO)
        {
            _requiredTargetCount = requiredTargetCount;
        }

        public override int GetRequiredTargetCount(Game game, Role self) => _requiredTargetCount;

        public override void Apply(Game game, Role executant, List<Role> targets) { }

        public override string FormatExecuteMessage(Role executant, List<Role> targets)
        {
            return string.Empty;
        }
    }

    // Mock state class for testing TakeTurn behavior
    private class MockStateWithActionControl : State
    {
        public bool BeforeTakeActionCalled { get; private set; }
        public bool AfterTakeActionCalled { get; private set; }
        public bool ActionExecuted { get; private set; }
        private readonly bool _canTakeAction;

        public MockStateWithActionControl(bool canTakeAction, IGameIO gameIO)
            : base("MockState", gameIO)
        {
            _canTakeAction = canTakeAction;
            BeforeTakeActionCalled = false;
            AfterTakeActionCalled = false;
        }

        public override bool CanTakeAction => _canTakeAction;

        public override void BeforeTakeAction()
        {
            BeforeTakeActionCalled = true;
            base.BeforeTakeAction();
        }

        public override void AfterTakeAction()
        {
            AfterTakeActionCalled = true;
            base.AfterTakeAction();
        }
    }

    private class MockDecisionMakerWithActionSequence : IDecisionMaker
    {
        private readonly Queue<GameAction> _actionsToReturn;
        public int SelectActionCallCount { get; private set; }

        public MockDecisionMakerWithActionSequence(IEnumerable<GameAction> actions)
        {
            _actionsToReturn = new Queue<GameAction>(actions);
            SelectActionCallCount = 0;
        }

        public GameAction SelectAction(List<GameAction> actions)
        {
            SelectActionCallCount++;
            return _actionsToReturn.Dequeue();
        }

        public List<Role> SelectTargets(List<Role> candidates, int requiredCount)
        {
            return candidates.Take(requiredCount).ToList();
        }
    }

    // Additional mock class for testing
    private class MockActionWithTargetType : GameAction
    {
        public MockActionWithTargetType(TargetType targetType, IGameIO gameIO)
            : base("mockAction", 0, targetType, gameIO) { }

        public override int GetRequiredTargetCount(Game game, Role self) => 1;

        public override void Apply(Game game, Role executant, List<Role> targets) { }

        public override string FormatExecuteMessage(Role executant, List<Role> targets)
        {
            return string.Empty;
        }
    }

    private class MockDeathObserver : IRoleDeathObserver
    {
        public bool WasNotified { get; private set; }

        public MockDeathObserver()
        {
            WasNotified = false;
        }

        public void OnRoleDeath()
        {
            WasNotified = true;
        }
    }
}
