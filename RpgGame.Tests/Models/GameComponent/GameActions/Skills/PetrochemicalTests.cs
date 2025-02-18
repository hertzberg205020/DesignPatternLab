using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class PetrochemicalTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly Role _target;
    private readonly List<Role> _targets;
    private readonly Petrochemical _petrochemical;
    private readonly TestGameIO _gameIO;

    public PetrochemicalTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _petrochemical = new Petrochemical(_gameIO);
        _executant = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 150,
            Strength = 50,
            IsHero = false
        };
        _executant.EnterState(new NormalState(_gameIO));
        _executant.Skills.Add(_petrochemical);
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

        _targets = [_target];
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.Equal("石化", _petrochemical.Name);
        Assert.Equal(100, _petrochemical.MpCost);
        Assert.Equal(TargetType.Enemy, _petrochemical.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnOne()
    {
        // Act
        var count = _petrochemical.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DoExecute_ShouldApplyPetrifiedState()
    {
        // Act
        _petrochemical.Apply(_game, _executant, _targets);

        // Assert
        Assert.IsType<PetrifiedState>(_target.State);
        var state = (PetrifiedState)_target.State;
        Assert.Equal(3, state.LeftRounds);
        Assert.False(state.CanTakeAction);
    }

    [Fact]
    public void Execute_WithInsufficientMp_ShouldThrowException()
    {
        // Arrange
        var executantWithLowMp = new Role(_gameIO)
        {
            Name = "LowMpExecutant",
            HealthPoint = 100,
            MagicPoint = 50, // MP < 100
            Strength = 50,
            IsHero = false
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _petrochemical.Execute(_game, executantWithLowMp, _targets)
        );
    }

    [Fact]
    public void Execute_WithSufficientMp_ShouldDeductMp()
    {
        // Arrange
        var expectedMp = _executant.MagicPoint - _petrochemical.MpCost;

        // Act
        _petrochemical.Execute(_game, _executant, _targets);

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
            () => _petrochemical.Execute(_game, _executant, multipleTargets)
        );
    }

    [Fact]
    public void FormatExecuteMessage_ShouldFormatCorrectly()
    {
        // Arrange
        // [1]英雄 對 [2]攻擊力超強的BOSS 使用了 石化。
        var expectedMessage = $"{_executant} 對 {_target} 使用了 石化!";

        // Act
        var message = _petrochemical.FormatExecuteMessage(_executant, _targets);

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void PetrifiedTarget_ShouldNotBeAbleToTakeAction()
    {
        // Arrange
        _petrochemical.Execute(_game, _executant, _targets);
        var someRole = new Role(_gameIO)
        {
            Name = "SomeRole",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 30,
            IsHero = false
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _target.Attack(someRole, 50));
    }

    [Fact]
    public void PetrifiedState_ShouldLastThreeRounds()
    {
        // Arrange
        _petrochemical.Execute(_game, _executant, _targets);
        var state = (PetrifiedState)_target.State;

        // Act & Assert
        Assert.Equal(3, state.LeftRounds);

        state.AfterTakeAction();
        Assert.Equal(2, state.LeftRounds);

        state.AfterTakeAction();
        Assert.Equal(1, state.LeftRounds);

        state.AfterTakeAction();
        Assert.Equal(0, state.LeftRounds);
        // assert state is changed to NormalState
        Assert.IsType<NormalState>(_target.State);
    }

    [Fact]
    public void GetRequiredTargetCount_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _petrochemical.GetRequiredTargetCount(null!, _executant)
        );
        Assert.Throws<ArgumentNullException>(
            () => _petrochemical.GetRequiredTargetCount(_game, null!)
        );
    }

    [Fact]
    public void DoExecute_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _petrochemical.Apply(null!, _executant, _targets)
        );
        Assert.Throws<ArgumentNullException>(() => _petrochemical.Apply(_game, null!, _targets));
        Assert.Throws<ArgumentNullException>(() => _petrochemical.Apply(_game, _executant, null!));
    }
}
