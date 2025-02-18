using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.States;

public class StateTests
{
    private readonly IGameIO _gameIO;

    public StateTests()
    {
        _gameIO = new TestGameIO(["0"]);
    }

    #region Constructor and Properties

    [Fact]
    public void Constructor_ShouldSetNameCorrectly()
    {
        // Arrange & Act
        var state = new TestState("TestState", _gameIO);

        // Assert
        Assert.Equal("TestState", state.Name);
        Assert.True(state.CanTakeAction);
        Assert.Equal(0, state.LeftRounds);
    }

    #endregion

    #region State Management

    [Fact]
    public void DecreaseLeftRounds_ShouldDecrementAndResetStateWhenZero()
    {
        // Arrange
        var state = new TestState("TestState", _gameIO);
        state.LeftRounds = 2;
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        state.Role = role;

        // Act
        state.TestDecreaseLeftRounds();

        // Assert
        Assert.Equal(1, state.LeftRounds);

        // Act again to reach 0
        state.TestDecreaseLeftRounds();

        // Assert the state reset
        Assert.Equal(0, state.LeftRounds);
        Assert.IsType<NormalState>(role.State);
    }

    [Fact]
    public void DecreaseLeftRounds_WithZeroRounds_ShouldNotDecrement()
    {
        // Arrange
        var state = new TestState("TestState", _gameIO);
        state.LeftRounds = 0;

        // Act
        state.TestDecreaseLeftRounds();

        // Assert
        Assert.Equal(0, state.LeftRounds);
    }

    #endregion

    #region Attack Behavior

    [Fact]
    public void Attack_WithNullTarget_ShouldThrowException()
    {
        // Arrange
        var state = new TestState("TestState", _gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        state.Role = role;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => state.Attack(null, 10));
    }

    [Fact]
    public void Attack_WithNegativeDamage_ShouldThrowException()
    {
        // Arrange
        var state = new TestState("TestState", _gameIO);
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
        state.Role = role;

        // Act & Assert
        // damage < 0
        Assert.Throws<ArgumentOutOfRangeException>(() => state.Attack(target, -10));
    }

    [Fact]
    public void Attack_WithNormalState_ShouldDealFullDamage()
    {
        // Arrange
        var state = new TestState("TestState", _gameIO);
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
        state.Role = role;

        // Act
        state.Attack(target, 20);

        // Assert
        Assert.Equal(80, target.HealthPoint); // 應該造成完整的傷害
    }

    #endregion


    #region ToString

    [Fact]
    public void ToString_ShouldReturnStateName()
    {
        // Arrange
        var state = new TestState("TestState", _gameIO);

        // Act & Assert
        Assert.Equal("TestState", state.ToString());
    }

    #endregion

    #region State Lifecycle

    [Fact]
    public void EnterState_ShouldBeCalledWhenStateIsSet()
    {
        // Arrange
        var mockState = new MockStateWithLifecycle("MockState", _gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        // Act
        role.EnterState(mockState);

        // Assert
        Assert.True(mockState.EnterStateCalled);
        Assert.Same(role, mockState.Role);
    }

    [Fact]
    public void ExitState_ShouldBeCalledWhenStateIsChanged()
    {
        // Arrange
        var oldState = new MockStateWithLifecycle("OldState", _gameIO);
        var newState = new MockStateWithLifecycle("NewState", _gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        // Act
        role.EnterState(oldState);
        role.EnterState(newState);

        // Assert
        Assert.True(oldState.ExitStateCalled);
        Assert.Null(oldState.Role);
        Assert.True(newState.EnterStateCalled);
        Assert.Same(role, newState.Role);
    }

    [Fact]
    public void EnterState_WithSameState_ShouldNotTriggerLifecycleMethods()
    {
        // Arrange
        var state = new MockStateWithLifecycle("TestState", _gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        // Act
        role.EnterState(state);
        state.ResetFlags();
        role.EnterState(state); // 再次設置相同的狀態

        // Assert
        Assert.False(state.EnterStateCalled);
        Assert.False(state.ExitStateCalled);
        Assert.Same(role, state.Role);
    }

    #endregion


    // Helper test class that exposes protected members for testing
    private class TestState : State
    {
        public TestState(string name, IGameIO gameIO)
            : base(name, gameIO) { }

        public void TestDecreaseLeftRounds()
        {
            DecreaseLeftRounds();
        }
    }

    /// <summary>
    /// Additional mock class for testing lifecycle methods
    /// </summary>
    private class MockStateWithLifecycle : State
    {
        public bool EnterStateCalled { get; private set; }
        public bool ExitStateCalled { get; private set; }

        public MockStateWithLifecycle(string name, IGameIO gameIO)
            : base(name, gameIO)
        {
            ResetFlags();
        }

        public override void EnterState()
        {
            EnterStateCalled = true;
        }

        public override void ExitState()
        {
            ExitStateCalled = true;
        }

        public void ResetFlags()
        {
            EnterStateCalled = false;
            ExitStateCalled = false;
        }
    }
}
