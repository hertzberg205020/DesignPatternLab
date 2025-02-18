using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions;

public class BasicAttackTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly Role _target;
    private readonly List<Role> _targets;
    private readonly TestGameIO _gameIO;

    public BasicAttackTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _executant = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 50,
            IsHero = true
        };
        _executant.EnterState(new NormalState(_gameIO));

        _target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 50,
            IsHero = false
        };
        _target.EnterState(new NormalState(_gameIO));

        _targets = new List<Role> { _target };

        _game.AddRole("A", _executant);

        foreach (var target in _targets)
        {
            _game.AddRole("B", target);
        }
    }

    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Arrange & Act
        var basicAttack = new BasicAttack(_gameIO);

        // Assert
        Assert.Equal("普通攻擊", basicAttack.Name);
        Assert.Equal(0, basicAttack.MpCost);
        Assert.Equal(TargetType.Enemy, basicAttack.TargetType);
    }

    [Fact]
    public void Constructor_WithMpCost_ShouldSetMpCostCorrectly()
    {
        // Arrange & Act
        var basicAttack = new BasicAttack(_gameIO);

        // Assert
        Assert.Equal(0, basicAttack.MpCost);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnOne()
    {
        // Arrange
        var basicAttack = new BasicAttack(_gameIO);

        // Act
        var count = basicAttack.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void DoExecute_ShouldDealDamageEqualToExecutantStrength()
    {
        // Arrange
        var basicAttack = new BasicAttack(_gameIO);
        var expectedHp = _target.HealthPoint - _executant.Strength;

        // Act
        basicAttack.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(expectedHp, _target.HealthPoint);
    }

    [Fact]
    public void Execute_WithMultipleTargets_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var basicAttack = new BasicAttack(_gameIO);

        var anotherTarget = new Role(_gameIO)
        {
            Name = "Target2",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 50,
            IsHero = false
        };

        anotherTarget.EnterState(new NormalState(_gameIO));

        var multipleTargets = new List<Role> { _target, anotherTarget };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => basicAttack.Apply(_game, _executant, multipleTargets)
        );
    }

    [Fact]
    public void FormatExecuteMessage_ShouldFormatCorrectly()
    {
        // Arrange
        var basicAttack = new BasicAttack(_gameIO);
        var expectedMessage = $"{_executant} 攻擊 {_target}。";

        // Act
        var message = basicAttack.FormatExecuteMessage(_executant, _targets);

        // Assert
        Assert.Equal(expectedMessage, message);
    }

    [Fact]
    public void Execute_ShouldDisplayCorrectMessage()
    {
        // Arrange
        var basicAttack = new BasicAttack(_gameIO);

        var output = _gameIO.Outputs;


        // Act
        basicAttack.Execute(_game, _executant, _targets);

        // Assert
        // var output = stringWriter.ToString().Trim().Split(Environment.NewLine);
        Assert.Equal(2, output.Count); // 應該有兩行輸出
        Assert.Equal($"{_executant} 攻擊 {_target}。", output[0]); // 第一行是攻擊訊息
        Assert.Equal($"{_executant} 對 {_target} 造成 50 點傷害。", output[1]); // 第二行是受傷訊息
    }
}
