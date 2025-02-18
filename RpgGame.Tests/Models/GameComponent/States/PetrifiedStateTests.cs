using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.States;

public class PetrifiedStateTests
{
    private readonly IGameIO _gameIO;

    public PetrifiedStateTests()
    {
        _gameIO = new TestGameIO([]);
    }

    [Fact]
    public void Constructor_ShouldSetCorrectInitialValues()
    {
        // Arrange & Act
        var state = new PetrifiedState(_gameIO);

        // Assert
        Assert.Equal("石化", state.Name);
        Assert.False(state.CanTakeAction); // 石化狀態下無法行動
        Assert.Equal(3, state.LeftRounds);
    }

    [Fact]
    public void Attack_ShouldNotDealDamage()
    {
        // Arrange
        var state = new PetrifiedState(_gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
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
            Strength = 30,
            Troop = "B",
            IsHero = false
        };

        role.EnterState(state);

        // Assert
        Assert.Throws<InvalidOperationException>(() => state.Attack(target, 20));
    }

    [Fact]
    public void AfterTakeAction_ShouldDecreaseLeftRoundsAndResetState()
    {
        // Arrange
        var state = new PetrifiedState(_gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        role.EnterState(state);

        // 模擬三回合
        for (int i = 3; i > 0; i--)
        {
            // Act
            state.AfterTakeAction();

            // Assert
            if (i > 1)
            {
                Assert.Equal(i - 1, state.LeftRounds);
                Assert.IsType<PetrifiedState>(role.State);
                Assert.False(role.State.CanTakeAction);
            }
            else
            {
                Assert.Equal(0, state.LeftRounds);
                Assert.IsType<NormalState>(role.State);
                Assert.True(role.State.CanTakeAction);
            }
        }
    }

    [Fact]
    public void TakeTurn_ShouldNotAllowAction()
    {
        // Arrange
        var state = new PetrifiedState(_gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        role.EnterState(state);

        // 模擬一個動作
        var mockAction = new MockAction(_gameIO);
        var mockDecisionMaker = new MockDecisionMaker(mockAction);
        role.DecisionMaker = mockDecisionMaker;

        // Act
        role.TakeTurn();

        // Assert
        Assert.False(mockAction.WasExecuted); // 確保動作沒有被執行
    }

    // Helper classes for testing
    private class MockAction : GameAction
    {
        public bool WasExecuted { get; private set; }

        public MockAction(IGameIO gameIO)
            : base("MockAction", 0, TargetType.Enemy, gameIO)
        {
            WasExecuted = false;
        }

        public override void Apply(Game game, Role executant, List<Role> targets)
        {
            WasExecuted = true;
        }

        public override int GetRequiredTargetCount(Game game, Role self) => 1;

        public override string FormatExecuteMessage(Role executant, List<Role> targets) =>
            string.Empty;
    }

    private class MockDecisionMaker : IDecisionMaker
    {
        private readonly GameAction _actionToReturn;

        public MockDecisionMaker(GameAction actionToReturn)
        {
            _actionToReturn = actionToReturn;
        }

        public GameAction SelectAction(List<GameAction> actions) => _actionToReturn;

        public List<Role> SelectTargets(List<Role> candidates, int requiredCount) =>
            candidates.Take(requiredCount).ToList();
    }
}
