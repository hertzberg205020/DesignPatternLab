using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class PoisonTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly Role _target;
    private readonly List<Role> _targets;
    private readonly Poison _poison;
    private readonly TestGameIO _gameIO;

    public PoisonTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _executant = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 50,
            IsHero = false
        };
        _executant.EnterState(new NormalState(_gameIO));
        _poison = new Poison(_gameIO);
        _executant.Skills.Add(_poison);
        _game.AddRole("A", _executant);

        _target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 200,
            MagicPoint = 100,
            Strength = 30,
            IsHero = false
        };
        _target.EnterState(new NormalState(_gameIO));
        _game.AddRole("B", _target);

        _targets = new List<Role> { _target };
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.Equal("下毒", _poison.Name);
        Assert.Equal(80, _poison.MpCost);
        Assert.Equal(TargetType.Enemy, _poison.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnOne()
    {
        // Act
        var count = _poison.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void Apply_ShouldTransferPoisonedState()
    {
        // Act
        _poison.Apply(_game, _executant, _targets);

        // Assert
        Assert.IsType<PoisonedState>(_target.State);
        var state = (PoisonedState)_target.State;
        Assert.Equal(3, state.LeftRounds);
    }

    [Fact]
    public void Execute_WithInsufficientMp_ShouldThrowException()
    {
        // Arrange
        var executantWithLowMp = new Role(_gameIO)
        {
            Name = "LowMpExecutant",
            HealthPoint = 100,
            MagicPoint = 50, // MP < 80
            Strength = 50,
            IsHero = false
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _poison.Execute(_game, executantWithLowMp, _targets)
        );
    }

    [Fact]
    public void Execute_WithSufficientMp_ShouldDeductMp()
    {
        // Arrange
        var expectedMp = _executant.MagicPoint - _poison.MpCost;

        // Act
        _poison.Execute(_game, _executant, _targets);

        // Assert
        Assert.Equal(expectedMp, _executant.MagicPoint);
    }

    [Fact]
    public void Execute_WithMultipleTargets_ShouldThrowException()
    {
        // Arrange
        var multipleTargets = new List<Role>
        {
            _target,
            new Role(_gameIO)
            {
                Name = "Target2",
                HealthPoint = 100,
                MagicPoint = 100,
                Strength = 30,
                IsHero = false
            }
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _poison.Execute(_game, _executant, multipleTargets)
        );
    }

    [Fact]
    public void FormatExecuteMessage_ShouldFormatCorrectly()
    {
        // Arrange
        var expectedMessage = $"{_executant} 對 {_target} 使用了 下毒。";

        // Act
        var message = _poison.FormatExecuteMessage(_executant, _targets);

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void Execute_ShouldDisplayCorrectMessages()
    {
        // Arrange

        // Act
        _poison.Execute(_game, _executant, _targets);

        // Assert
        var output = _gameIO.Outputs;
        Assert.Single(output);
        Assert.Equal($"{_executant} 對 {_target} 使用了 下毒。", output[0]);
    }

    [Fact]
    public void PoisonedTarget_ShouldTakeDamageBeforeAction()
    {
        // Arrange
        _poison.Execute(_game, _executant, _targets);
        var initialHp = _target.HealthPoint;

        // Act
        _target.State?.BeforeTakeAction();

        // Assert
        Assert.Equal(initialHp - 30, _target.HealthPoint);
    }

    [Fact]
    public void PoisonedState_ShouldLastThreeRounds()
    {
        // Arrange
        _poison.Execute(_game, _executant, _targets);
        var state = (PoisonedState)_target.State;
        var initialHp = _target.HealthPoint;

        Assert.NotNull(state);

        // 第一回合
        state.BeforeTakeAction();
        Assert.Equal(initialHp - 30, _target.HealthPoint);
        state.AfterTakeAction();
        Assert.Equal(2, state.LeftRounds);

        // 第二回合
        state.BeforeTakeAction();
        Assert.Equal(initialHp - 60, _target.HealthPoint);
        state.AfterTakeAction();
        Assert.Equal(1, state.LeftRounds);

        // 第三回合
        state.BeforeTakeAction();
        Assert.Equal(initialHp - 90, _target.HealthPoint);
        state.AfterTakeAction();
        Assert.Equal(0, state.LeftRounds);

        // 狀態恢復為 NormalState
        Assert.IsType<NormalState>(_target.State);
    }

    [Fact]
    public void PoisonedState_DamageShouldBeAppliedBeforeAction()
    {
        // Arrange
        _poison.Execute(_game, _executant, _targets);
        var state = (PoisonedState)_target.State;

        Assert.NotNull(state);

        var initialHp = _target.HealthPoint;

        // Act & Assert
        // 確保傷害在行動前就被套用
        state.BeforeTakeAction();
        Assert.Equal(initialHp - 30, _target.HealthPoint);
    }

    [Fact]
    public void GetRequiredTargetCount_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _poison.GetRequiredTargetCount(null!, _executant)
        );
        Assert.Throws<ArgumentNullException>(() => _poison.GetRequiredTargetCount(_game, null!));
    }

    [Fact]
    public void SkillApply_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _poison.Apply(null!, _executant, _targets));
        Assert.Throws<ArgumentNullException>(() => _poison.Apply(_game, null!, _targets));
        Assert.Throws<ArgumentNullException>(() => _poison.Apply(_game, _executant, null!));
    }
}
