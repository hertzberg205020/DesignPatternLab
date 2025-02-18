using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class OnePunchTests
{
    private readonly OnePunchHandler _handler;
    private readonly OnePunch _onePunch;
    private readonly Game _game;
    private readonly Role _executant;
    private readonly List<Role> _targets = new();
    private readonly TestGameIO _gameIO;

    public OnePunchTests()
    {
        _gameIO = new TestGameIO([]);
        // 建立責任鏈
        _handler = new HpGreaterThanFiveHundredHandler(
            new AbnormalStateHandler(
                new CheerupStateHandler(new NormalStateHandler(null, _gameIO), _gameIO),
                _gameIO
            ),
            _gameIO
        );
        _onePunch = new OnePunch(_handler, _gameIO);

        _game = new Game(_gameIO);

        _executant = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 200,
            Strength = 50,
            IsHero = false,
        };
        _executant.EnterState(new NormalState(_gameIO));
        _executant.Skills.Add(_onePunch);
        _game.AddRole("A", _executant);

        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 200,
            MagicPoint = 100,
            Strength = 30,
            IsHero = false,
        };
        target.EnterState(new NormalState(_gameIO));
        _game.AddRole("B", target);
        _targets.Add(target);

        // 改變標準輸出
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Assert
        Assert.Equal("一拳攻擊", _onePunch.Name);
        Assert.Equal(180, _onePunch.MpCost);
        Assert.Equal(TargetType.Enemy, _onePunch.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnOne()
    {
        // Act
        var count = _onePunch.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void Execute_WithInsufficientMP_ShouldThrowException()
    {
        // Arrange
        _executant.MagicPoint = 150; // Less than required 180 MP

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _onePunch.Execute(_game, _executant, _targets)
        );
        Assert.Equal("魔力不足", exception.Message);
    }

    [Fact]
    public void Execute_WithSufficientMP_ShouldDeductMP()
    {
        // Act
        _onePunch.Execute(_game, _executant, _targets);

        // Assert
        Assert.Equal(20, _executant.MagicPoint); // 200 - 180 = 20
    }

    [Fact]
    public void Apply_ToNormalStateTarget_ShouldDeal100Damage()
    {
        // Arrange
        var target = _targets.Single();
        target.EnterState(new NormalState(_gameIO));

        // Act
        _onePunch.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(100, target.HealthPoint); // 200 - 100 = 100
    }

    [Fact]
    public void Apply_ToHighHPTarget_ShouldDeal300Damage()
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 600; // More than 500 HP

        // Act
        _onePunch.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(300, target.HealthPoint); // 600 - 300 = 300
    }

    [Theory]
    [InlineData(typeof(PoisonedState))]
    [InlineData(typeof(PetrifiedState))]
    public void Apply_ToAbnormalStateTarget_ShouldDeal300Damage(Type stateType)
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 400; // More than 300 HP

        var constructors = stateType
            .GetConstructors()
            .Where(c => c.GetParameters().Length == 1)
            .ToList();

        var state = (State)constructors[0].Invoke(new object[] { _gameIO });

        target.EnterState(state);

        // Act
        _onePunch.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(400 - 80 * 3, target.HealthPoint);
    }

    [Fact]
    public void Apply_ToCheerUpStateTarget_ShouldDeal100DamageAndRemoveState()
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 200; // More than 100 HP
        target.EnterState(new CheerUpState(_gameIO));

        // Act
        _onePunch.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(100, target.HealthPoint); // 200 - 100 = 100
        Assert.IsType<NormalState>(target.State);
    }

    [Fact]
    public void Apply_ToCheerUpStateTarget_WhenKilled_ShouldNotChangeState()
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 50; // Less than 100 HP
        target.EnterState(new CheerUpState(_gameIO));

        // Act
        _onePunch.Apply(_game, _executant, _targets);

        // Assert
        Assert.Equal(0, target.HealthPoint);
        Assert.IsType<DeadState>(target.State); // Dead target's state should be DeadState
    }

    [Fact]
    public void FormatExecuteMessage_ShouldReturnCorrectMessage()
    {
        // Act
        var message = _onePunch.FormatExecuteMessage(_executant, _targets);

        var target = _targets.Single();
        Assert.NotNull(target);

        Assert.Equal($"{_executant} 對 {target} 使用了 一拳攻擊。", message);
    }

    [Fact]
    public void Apply_ToNormalStateTarget_ShouldOutputCorrectMessage()
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 200;
        target.EnterState(new NormalState(_gameIO));

        // Act
        _onePunch.Apply(_game, _executant, _targets);
        var output = _gameIO.Outputs;
        Assert.Equal(2, output.Count);

        // Assert
        Assert.Equal($"{_executant} 對 {target} 使用了 一拳攻擊。", output[0]);
        Assert.Equal($"{_executant} 對 {target} 造成 100 點傷害。", output[1]);
        Assert.Equal(100, target.HealthPoint);
    }

    [Fact]
    public void Apply_ToHighHPTarget_ShouldOutputCorrectMessage()
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 600; // More than 500 HP

        // Act
        _onePunch.Apply(_game, _executant, _targets);
        var output = _gameIO.Outputs;

        // Assert
        Assert.Equal($"{_executant} 對 {target} 使用了 一拳攻擊。", output[0]);
        Assert.Equal($"{_executant} 對 {target} 造成 300 點傷害。", output[1]);
        Assert.Equal(300, target.HealthPoint);
    }

    [Theory]
    [InlineData(typeof(PoisonedState))]
    [InlineData(typeof(PetrifiedState))]
    public void Apply_ToAbnormalStateTarget_ShouldOutputCorrectMessage(Type stateType)
    {
        // Arrange
        var target = _targets.Single();
        target.HealthPoint = 400;

        var constructors = stateType
            .GetConstructors()
            .Where(c => c.GetParameters().Length == 1)
            .ToList();

        var state = (State)constructors[0].Invoke(new object[] { _gameIO });

        target.EnterState(state);

        // Act
        _onePunch.Apply(_game, _executant, _targets);
        var output = _gameIO.Outputs;

        // Assert
        Assert.Equal(4, output.Count);
        Assert.Equal($"{_executant} 對 {target} 使用了 一拳攻擊。", output[0]);
        for (var i = 0; i < 3; i++)
        {
            Assert.Equal($"{_executant} 對 {target} 造成 80 點傷害。", output[i + 1]);
        }
        Assert.Equal(400 - (80 * 3), target.HealthPoint);
        Assert.IsType(stateType, target.State);
    }
}
