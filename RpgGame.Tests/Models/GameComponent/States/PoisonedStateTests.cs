using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.States;

public class PoisonedStateTests
{
    private readonly IGameIO _gameIO;

    public PoisonedStateTests()
    {
        _gameIO = new TestGameIO([]);
    }

    [Fact]
    public void Constructor_ShouldSetCorrectInitialValues()
    {
        // Arrange & Act
        var state = new PoisonedState(_gameIO);

        // Assert
        Assert.Equal("中毒", state.Name);
        Assert.True(state.CanTakeAction);
        Assert.Equal(3, state.LeftRounds);
    }

    [Fact]
    public void Attack_ShouldDealNormalDamage()
    {
        // Arrange
        var state = new PoisonedState(_gameIO);
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

        // Act
        state.Attack(target, 20);

        // Assert
        Assert.Equal(80, target.HealthPoint); // 中毒狀態不影響攻擊力
        Assert.IsType<PoisonedState>(role.State); // 狀態不會改變
    }

    [Fact]
    public void BeforeTakeAction_ShouldDealPoisonDamage()
    {
        // Arrange
        var state = new PoisonedState(_gameIO);
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

        // Act
        state.BeforeTakeAction();

        // Assert
        Assert.Equal(70, role.HealthPoint); // 每回合受到 30 點毒傷
    }

    [Fact]
    public void BeforeTakeAction_WithLowHealth_ShouldNotKillRole()
    {
        // Arrange
        var state = new PoisonedState(_gameIO);
        var role = new Role(_gameIO)
        {
            Name = "TestRole",
            HealthPoint = 5, // 生命值低於毒傷
            MagicPoint = 50,
            Strength = 30,
            Troop = "A",
            IsHero = true
        };

        role.EnterState(state);

        // Act
        state.BeforeTakeAction();

        // Assert
        Assert.Equal(0, role.HealthPoint); // 角色中毒死亡
        Assert.IsType<DeadState>(role.State); // 角色進入死亡狀態
    }

    [Fact]
    public void AfterTakeAction_ShouldDecreaseLeftRounds()
    {
        // Arrange
        var state = new PoisonedState(_gameIO);

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
        // 第一回合
        state.AfterTakeAction();
        Assert.Equal(2, state.LeftRounds);
        Assert.IsType<PoisonedState>(role.State);

        // 第二回合
        state.AfterTakeAction();
        Assert.Equal(1, state.LeftRounds);
        Assert.IsType<PoisonedState>(role.State);

        // 第三回合
        state.AfterTakeAction();
        Assert.Equal(0, state.LeftRounds);
        Assert.IsType<NormalState>(role.State); // 回合結束後應該恢復正常
    }
}
