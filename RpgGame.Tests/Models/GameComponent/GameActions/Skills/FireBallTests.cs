using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class FireBallTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly List<Role> _targets;
    private readonly FireBall _fireBall;
    private readonly TestGameIO _gameIO;

    public FireBallTests()
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
        _game.AddRole("A", _executant);

        _targets = new List<Role>
        {
            new Role(_gameIO)
            {
                Name = "Target1",
                HealthPoint = 200,
                MagicPoint = 100,
                Strength = 30,
                IsHero = false
            },
            new Role(_gameIO)
            {
                Name = "Target2",
                HealthPoint = 200,
                MagicPoint = 100,
                Strength = 30,
                IsHero = false
            },
            new Role(_gameIO)
            {
                Name = "Target3",
                HealthPoint = 200,
                MagicPoint = 100,
                Strength = 30,
                IsHero = false
            }
        };

        foreach (var target in _targets)
        {
            target.EnterState(new NormalState(_gameIO));
            _game.AddRole("B", target);
        }

        _fireBall = new FireBall(_gameIO);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.Equal("火球", _fireBall.Name);
        Assert.Equal(50, _fireBall.MpCost);
        Assert.Equal(TargetType.Enemy, _fireBall.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnNumberOfAliveEnemies()
    {
        // Arrange
        var enemies = _targets.ToList(); // 複製一份避免影響其他測試

        // Act
        var count = _fireBall.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(enemies.Count, count);
    }

    [Fact]
    public void GetRequiredTargetCount_WithDeadEnemies_ShouldOnlyCountAliveEnemies()
    {
        // Arrange
        var enemies = _targets.ToList();
        enemies[0].TakeDamage(200); // 殺死第一個敵人

        // Act
        var count = _fireBall.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(enemies.Count(e => e.IsAlive()), count);
    }

    [Fact]
    public void DoExecute_ShouldDeal50DamageToAllTargets()
    {
        // Arrange
        var initialHps = _targets.Select(t => t.HealthPoint).ToList();

        // Act
        _fireBall.Apply(_game, _executant, _targets);

        // Assert
        for (var i = 0; i < _targets.Count; i++)
        {
            Assert.Equal(initialHps[i] - 50, _targets[i].HealthPoint);
        }
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
            () => _fireBall.Execute(_game, executantWithLowMp, _targets)
        );
    }

    [Fact]
    public void Execute_WithSufficientMp_ShouldDeductMp()
    {
        // Arrange
        _executant.MagicPoint = 100;
        var expectedMp = _executant.MagicPoint - _fireBall.MpCost;

        // Act
        _fireBall.Execute(_game, _executant, _targets);

        // Assert
        Assert.Equal(expectedMp, _executant.MagicPoint);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    public void Execute_WithDifferentNumberOfTargets_ShouldWork(int targetCount)
    {
        // Arrange
        var selectedTargets = _targets.Take(targetCount).ToList();

        // Act
        var exception = Record.Exception(
            () => _fireBall.Execute(_game, _executant, selectedTargets)
        );

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void FormatExecuteMessage_WithMultipleTargets_ShouldFormatCorrectly()
    {
        // Arrange
        // [1]英雄 對 [2]Slime1, [2]Slime2 使用了 火球。
        var expectedMessage = $"{_executant} 對 {string.Join(", ", _targets)} 使用了 火球。";

        // Act
        var message = _fireBall.FormatExecuteMessage(_executant, _targets);

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void Execute_ShouldDisplayCorrectMessages()
    {
        // Arrange

        // Act
        _fireBall.Execute(_game, _executant, _targets);

        // Assert
        var output = _gameIO.Outputs;
        Assert.Equal(_targets.Count + 1, output.Count); // 技能訊息 + 每個目標的傷害訊息
        Assert.Equal($"{_executant} 對 {string.Join(", ", _targets)} 使用了 火球。", output[0]);
        for (var i = 0; i < _targets.Count; i++)
        {
            // // [A]Executant 對 [B]Target1 造成 50 點傷害。
            Assert.Equal($"{_executant} 對 {_targets[i]} 造成 50 點傷害。", output[i + 1]);
        }
    }

    [Fact]
    public void GetRequiredTargetCount_WithNullGame_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => _fireBall.GetRequiredTargetCount(null!, _executant)
        );
    }

    [Fact]
    public void GetRequiredTargetCount_WithNullSelf_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _fireBall.GetRequiredTargetCount(_game, null!));
    }

    [Fact]
    public void DoExecute_WithNullParameters_ShouldThrowArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _fireBall.Apply(null!, _executant, _targets));
        Assert.Throws<ArgumentNullException>(() => _fireBall.Apply(_game, null!, _targets));
        Assert.Throws<ArgumentNullException>(() => _fireBall.Apply(_game, _executant, null!));
    }
}
