using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.States;

public class CheerUpStateTests
{
    private readonly IGameIO _gameIO;

    public CheerUpStateTests()
    {
        _gameIO = new TestGameIO(new List<string>());
    }

    [Fact]
    public void Constructor_ShouldSetCorrectInitialValues()
    {
        // Arrange & Act
        var state = new CheerUpState(_gameIO);

        // Assert
        Assert.Equal("受到鼓舞", state.Name);
        Assert.True(state.CanTakeAction);
        Assert.Equal(3, state.LeftRounds);
    }

    [Theory]
    [InlineData(100, 100 + 50)] // 100 的傷害應該提升到 120
    [InlineData(50, 50 + 50)] // 50 的傷害應該提升到 60
    [InlineData(25, 25 + 50)] // 25 的傷害應該提升到 30
    public void Attack_ShouldIncreaseDamageByTwentyPercent(int baseDamage, int expectedDamage)
    {
        // Arrange
        var state = new CheerUpState(_gameIO);
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
            HealthPoint = 200, // 確保有足夠的生命值測試傷害
            MagicPoint = 50,
            Strength = 30,
            Troop = "B",
            IsHero = false
        };
        role.EnterState(state);

        // Act
        role.Attack(target, baseDamage);

        // Assert
        Assert.Equal(200 - expectedDamage, target.HealthPoint);
    }

    [Fact]
    public void AfterTakeAction_ShouldDecreaseLeftRoundsAndResetState()
    {
        // Arrange
        var state = new CheerUpState(_gameIO);
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
                Assert.IsType<CheerUpState>(role.State);

                // 確認在鼓舞狀態下攻擊力仍然提升
                var target = new Role(_gameIO)
                {
                    Name = "Target",
                    HealthPoint = 1000,
                    MagicPoint = 50,
                    Strength = 30,
                    Troop = "B",
                    IsHero = false
                };

                role.Attack(target, 100);
                // 傷害應該多 50 點傷害
                Assert.Equal(1000 - 150, target.HealthPoint);
            }
            else
            {
                Assert.Equal(0, state.LeftRounds);
                Assert.IsType<NormalState>(role.State);

                // 確認恢復正常後攻擊力不再提升
                var target = new Role(_gameIO)
                {
                    Name = "Target",
                    HealthPoint = 100,
                    MagicPoint = 50,
                    Strength = 30,
                    Troop = "B",
                    IsHero = false
                };
                role.State.Attack(target, 100);
                Assert.Equal(0, target.HealthPoint - 0); // 傷害應該正常
            }
        }
    }

    [Fact]
    public void Attack_WithNullTarget_ShouldThrowException()
    {
        // Arrange
        var state = new CheerUpState(_gameIO);
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

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => role.Attack(null, 10));
    }
}
