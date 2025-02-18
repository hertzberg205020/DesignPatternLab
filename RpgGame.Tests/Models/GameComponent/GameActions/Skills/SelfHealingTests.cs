using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class SelfHealingTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly SelfHealing _selfHealing;
    private readonly TestGameIO _gameIO;

    public SelfHealingTests()
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

        _game.AddRole("A", _executant);
        _selfHealing = new SelfHealing(_gameIO);
        _executant.Skills.Add(_selfHealing);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.Equal("自我治療", _selfHealing.Name);
        Assert.Equal(50, _selfHealing.MpCost);
        Assert.Equal(TargetType.Self, _selfHealing.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnZero()
    {
        // Act
        var count = _selfHealing.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(0, count);
    }

    [Theory]
    [InlineData(100, 250)] // 從滿血加到超過最大值
    [InlineData(50, 200)] // 從一半血量加血
    [InlineData(10, 160)] // 從低血量加血
    public void DoExecute_ShouldHeal150HP(int initialHp, int expectedHp)
    {
        // Arrange
        _executant.HealthPoint = initialHp;

        // Act
        _selfHealing.Apply(_game, _executant, []);

        // Assert
        Assert.Equal(expectedHp, _executant.HealthPoint);
    }

    [Fact]
    public void Execute_WithInsufficientMp_ShouldThrowException()
    {
        // Arrange
        var executantWithLowMp = new Role(_gameIO)
        {
            Name = "LowMpExecutant",
            HealthPoint = 100,
            MagicPoint = 30, // MP < 50
            Strength = 50,
            IsHero = false
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _selfHealing.Execute(_game, executantWithLowMp, [])
        );
    }

    [Fact]
    public void Execute_WithSufficientMp_ShouldDeductMp()
    {
        // Arrange
        var expectedMp = _executant.MagicPoint - _selfHealing.MpCost;

        // Act
        _selfHealing.Execute(_game, _executant, []);

        // Assert
        Assert.Equal(expectedMp, _executant.MagicPoint);
    }

    [Fact]
    public void Execute_WithNonEmptyTargets_ShouldNotWork()
    {
        // Arrange
        var targets = new List<Role>
        {
            new Role(_gameIO)
            {
                Name = "Target",
                HealthPoint = 100,
                MagicPoint = 100,
                Strength = 30,
                IsHero = false
            }
        };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => _selfHealing.Execute(_game, _executant, targets)
        );
    }

    [Fact]
    public void FormatExecuteMessage_ShouldFormatCorrectly()
    {
        // Arrange
        var expectedMessage = $"{_executant} 使用了 自我治療。";

        // Act
        var message = _selfHealing.FormatExecuteMessage(_executant, []);

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void Execute_ShouldDisplayCorrectMessages()
    {
        // Arrange

        // Act
        _selfHealing.Execute(_game, _executant, new List<Role>());

        // Assert
        var output = _gameIO.Outputs;
        Assert.Single(output);
        Assert.Equal($"{_executant} 使用了 自我治療。", output[0]);

        // 清理
        Console.SetOut(Console.Out);
    }

    [Fact]
    public void GetRequiredTargetCount_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _selfHealing.GetRequiredTargetCount(null!, _executant)
        );
        Assert.Throws<ArgumentNullException>(
            () => _selfHealing.GetRequiredTargetCount(_game, null!)
        );
    }

    [Fact]
    public void DoExecute_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _selfHealing.Apply(null!, _executant, []));
        Assert.Throws<ArgumentNullException>(() => _selfHealing.Apply(_game, null!, []));
    }
}
