using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.States;

public class NormalStateTests
{
    private readonly IGameIO _gameIO;

    public NormalStateTests()
    {
        _gameIO = new TestGameIO(new List<string>());
    }



    [Fact]
    public void Constructor_ShouldSetCorrectName()
    {
        // Arrange & Act
        var state = new NormalState(_gameIO);

        // Assert
        Assert.Equal("正常", state.Name);
    }

    [Fact]
    public void CanTakeAction_ShouldBeTrue()
    {
        // Arrange
        var state = new NormalState(_gameIO);

        // Assert
        Assert.True(state.CanTakeAction);
    }

    [Fact]
    public void Attack_ShouldDealNormalDamage()
    {
        // Arrange
        var state = new NormalState(_gameIO);
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
        Assert.Equal(80, target.HealthPoint); // 正常狀態應該造成完整的傷害
    }

    [Fact]
    public void EnterState_ShouldNotChangeRoleProperties()
    {
        // Arrange
        var state = new NormalState(_gameIO);
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
        state.EnterState();

        // Assert
        Assert.Equal(100, role.HealthPoint);
        Assert.Equal(50, role.MagicPoint);
        Assert.Equal(30, role.Strength);
    }
}
