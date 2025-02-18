using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class SummonTests
{
    private readonly TestGameIO _gameIO;
    private readonly Game _game;
    private readonly Role _summoner;
    private readonly Role _enemy;
    private readonly Summon _skill;

    public SummonTests()
    {
        _gameIO = new TestGameIO(Array.Empty<string>());
        _game = new Game(_gameIO);
        _skill = new Summon(_gameIO);
        // 設置召喚者
        _summoner = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 150,
            Strength = 50,
            IsHero = false
        };
        _summoner.EnterState(new NormalState(_gameIO));
        _summoner.Skills.Add(_skill);
        _game.AddRole("1", _summoner);

        // 設置敵人
        _enemy = new Role(_gameIO)
        {
            Name = "Enemy",
            HealthPoint = 100,
            MagicPoint = 50,
            Strength = 20,
            IsHero = false
        };
        _enemy.EnterState(new NormalState(_gameIO));
        _game.AddRole("2", _enemy);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var summon = new Summon(_gameIO);

        // Assert
        Assert.Equal("召喚", summon.Name);
        Assert.Equal(150, summon.MpCost);
        Assert.Equal(TargetType.None, summon.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnZero()
    {
        // Act
        var count = _skill.GetRequiredTargetCount(_game, _summoner);

        // Assert
        Assert.Equal(0, count);
    }

    [Fact]
    public void Apply_ShouldSummonSlimeAndAddToTroop()
    {
        // Act
        _skill.Apply(_game, _summoner, []);

        // Assert
        var allies = _game.GetAllies(_summoner);

        Assert.Contains(allies, role => role is Slime);

        var slime = allies.First(role => role is Slime);
        Assert.Equal("Slime", slime.Name);
        Assert.Equal(100, slime.HealthPoint);
        Assert.Equal(0, slime.MagicPoint);
        Assert.Equal(10, slime.Strength);
        Assert.False(slime.IsHero);
        Assert.Equal(_game, slime.Game);
        Assert.Equal(_summoner.Troop, slime.Troop);
    }

    [Fact]
    public void Execute_WithInsufficientMP_ShouldThrowException()
    {
        // Arrange

        // Less than required 150 MP
        _summoner.MagicPoint = 100;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _skill.Execute(_game, _summoner, new List<Role>())
        );
        Assert.Equal("魔力不足", exception.Message);
    }

    [Fact]
    public void Execute_WithSufficientMP_ShouldDeductMP()
    {
        // Arrange
        _summoner.MagicPoint = 200;

        // Act
        _skill.Execute(_game, _summoner, new List<Role>());

        // Assert
        Assert.Equal(50, _summoner.MagicPoint); // 200 - 150 = 50
    }

    [Fact]
    public void SummonedSlimeDeath_ShouldHealSummoner()
    {
        // Arrange
        _summoner.HealthPoint = 70;

        _skill.Execute(_game, _summoner, new List<Role>());
        var slime = _game.GetRoles().First(role => role is Slime);

        // Act
        slime.TakeDamage(100); // Kill the slime

        // Assert
        Assert.Equal(100, _summoner.HealthPoint); // 70 + 30 = 100
    }

    [Fact]
    public void FormatExecuteMessage_ShouldReturnCorrectMessage()
    {
        // Act
        var message = _skill.FormatExecuteMessage(_summoner, new List<Role>());

        // Assert
        Assert.Equal($"{_summoner} 使用了 召喚。", message);
    }

    [Fact]
    public void SummonedSlime_ShouldAttackEnemy()
    {
        // Arrange
        var targets = new List<Role> { _enemy };

        // Act
        _skill.Execute(_game, _summoner, []); // 召喚 Slime
        var slime = _game.GetAllies(_summoner).First(r => r.Name == "Slime");

        // 確保 Slime 被正確召喚
        Assert.NotNull(slime);
        Assert.Equal(100, slime.HealthPoint);
        Assert.Equal(0, slime.MagicPoint);
        Assert.Equal(50, slime.Strength);

        // 模擬 Slime 的回合
        var executeRoleTurnMethod = typeof(Game).GetMethod(
            "ExecuteRoleTurn",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );
        executeRoleTurnMethod.Invoke(_game, new object[] { slime });

        // Assert
        Assert.Equal(50, _enemy.HealthPoint); // 100 - 50 (Slime的力量)
        var outputs = _gameIO.Outputs;
        Assert.Contains(outputs, output => output.Contains("[1]Slime 攻擊 [2]Enemy")); // 驗證攻擊訊息
    }
}
