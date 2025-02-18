using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.States;

public class DeadStateTests
{
    private readonly IGameIO _gameIO;

    public DeadStateTests()
    {
        _gameIO = new TestGameIO(new List<string>());
    }

    [Fact]
    public void Constructor_ShouldSetCorrectName()
    {
        // Arrange & Act
        var state = new DeadState(_gameIO);

        // Assert
        Assert.Equal("死亡", state.Name);
    }

    [Fact]
    public void Constructor_ShouldSetCanTakeActionToFalse()
    {
        // Arrange & Act
        var state = new DeadState(_gameIO);

        // Assert
        Assert.False(state.CanTakeAction);
    }

    [Fact]
    public void Constructor_ShouldSetLeftRoundsToZero()
    {
        // Arrange & Act
        var state = new DeadState(_gameIO);

        // Assert
        Assert.Equal(0, state.LeftRounds);
    }

    [Fact]
    public void Attack_ShouldNotDealDamage()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 0, // 死亡狀態
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        var state = new DeadState(_gameIO);

        role.EnterState(state);

        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 30,
            Troop = "B",
            IsHero = false
        };
        target.EnterState(new NormalState(_gameIO));

        // Act
        state.Attack(target, 20);

        // Assert
        Assert.Equal(100, target.HealthPoint); // 死亡狀態不應該造成傷害
    }

    [Fact]
    public void EnterState_ShouldNotifyDeathObservers()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 0,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        var mockObserver = new MockDeathObserver();
        role.RegisterDeathObserver(mockObserver);
        var state = new DeadState(_gameIO);
        role.EnterState(state);

        // Act
        state.EnterState();

        // Assert
        Assert.True(mockObserver.WasNotified);
    }

    [Fact]
    public void DecreaseLeftRounds_ShouldNotChangeState()
    {
        // Arrange
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 0,
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };
        var state = new DeadState(_gameIO);
        role.EnterState(state);

        // Act
        state.TestDecreaseLeftRounds();

        // Assert
        Assert.IsType<DeadState>(role.State);
        Assert.Equal(0, state.LeftRounds);
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

public static class TestExtensions
{
    public static void TestDecreaseLeftRounds(this State state)
    {
        var method = typeof(State).GetMethod(
            "DecreaseLeftRounds",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        method?.Invoke(state, null);
    }
}
