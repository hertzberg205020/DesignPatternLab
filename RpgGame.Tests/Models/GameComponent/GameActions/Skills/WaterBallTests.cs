using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class WaterBallTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly Role _target;
    private readonly List<Role> _targets;
    private readonly WaterBall _waterBall;
    private readonly TestGameIO _gameIO;

    public WaterBallTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _executant = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 50,
            IsHero = false,
        };

        _executant.EnterState(new NormalState(_gameIO));

        _target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 200,
            MagicPoint = 100,
            Strength = 30,
            IsHero = false,
        };

        _target.EnterState(new NormalState(_gameIO));

        _game.AddRole("A", _executant);

        _game.AddRole("B", _target);

        _targets = new List<Role> { _target };

        _executant.Skills.Add(new WaterBall(_gameIO));

        _waterBall = new WaterBall(_gameIO);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.Equal("水球", _waterBall.Name);
        Assert.Equal(50, _waterBall.MpCost);
        Assert.Equal(TargetType.Enemy, _waterBall.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnOne()
    {
        // Act
        var count = _waterBall.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DoExecute_ShouldDeal120Damage()
    {
        // Arrange
        var expectedHp = _target.HealthPoint - 120;

        // Act
        _waterBall.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(expectedHp, _target.HealthPoint);
    }

    [Fact]
    public void Execute_WithInsufficientMp_ShouldThrowException()
    {
        // Arrange
        var executantWithLowMp = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 0,
            Strength = 50,
            IsHero = false,
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _waterBall.Execute(_game, executantWithLowMp, _targets)
        );
    }

    [Fact]
    public void Execute_WithSufficientMp_ShouldDeductMp()
    {
        // Arrange
        var expectedMp = _executant.MagicPoint - _waterBall.MpCost;

        // Act
        _waterBall.Execute(_game, _executant, _targets);

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
                HealthPoint = 200,
                MagicPoint = 100,
                Strength = 30,
                IsHero = false,
            }
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _waterBall.Execute(_game, _executant, multipleTargets)
        );
    }

    [Fact]
    public void FormatExecuteMessage_ShouldFormatCorrectly()
    {
        // Arrange
        var expectedMessage = $"{_executant} 對 {_target} 使用了 水球。";

        // Act
        var message = _waterBall.FormatExecuteMessage(_executant, _targets);

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void Execute_ShouldDisplayCorrectMessages()
    {
        // Arrange

        // Act
        _waterBall.Execute(_game, _executant, _targets);

        // Assert
        var output = _gameIO.Outputs;

        Assert.Equal(2, output.Count);

        // [1]英雄 對 [2]Slime2 使用了 水球。
        Assert.Equal($"{_executant} 對 {_target} 使用了 水球。", output[0]);
        // [1]英雄 對 [2]Slime2 造成 120 點傷害。
        Assert.Equal($"{_executant} 對 {_target} 造成 120 點傷害。", output[1]);
    }
}
