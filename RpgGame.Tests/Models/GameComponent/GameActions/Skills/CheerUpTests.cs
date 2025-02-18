using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class CheerUpTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly List<Role> _allies;
    private readonly CheerUp _skill;
    private readonly IGameIO _gameIO;

    public CheerUpTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _allies = new List<Role>();
        _skill = new CheerUp(_gameIO);

        var troop1 = "A";
        var troop2 = "B";

        _executant = new Role(_gameIO)
        {
            Name = "Executant",
            HealthPoint = 100,
            MagicPoint = 200,
            Strength = 10,
            IsHero = true
        };
        _executant.EnterState(new NormalState(_gameIO));
        _executant.Skills.Add(_skill);
        _game.AddRole(troop1, _executant);

        var ally1 = new Role(_gameIO)
        {
            Name = "Ally1",
            HealthPoint = 100,
            MagicPoint = 200,
            Strength = 10,
            IsHero = false
        };
        ally1.EnterState(new NormalState(_gameIO));
        _game.AddRole(troop1, ally1);
        _allies.Add(ally1);

        var ally2 = new Role(_gameIO)
        {
            Name = "Ally2",
            HealthPoint = 100,
            MagicPoint = 200,
            Strength = 10,
            IsHero = false
        };
        ally2.EnterState(new NormalState(_gameIO));
        _game.AddRole(troop1, ally2);
        _allies.Add(ally2);

        var ally3 = new Role(_gameIO)
        {
            Name = "Ally3",
            HealthPoint = 100,
            MagicPoint = 200,
            Strength = 10,
            IsHero = false
        };
        ally3.EnterState(new NormalState(_gameIO));
        _game.AddRole(troop1, ally3);
        _allies.Add(ally3);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var cheerUp = _skill;

        // Assert
        Assert.Equal("鼓舞", cheerUp.Name);
        Assert.Equal(100, cheerUp.MpCost);
        Assert.Equal(TargetType.Ally, cheerUp.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnThree()
    {
        // Arrange

        // Act
        var count = _skill.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(3, count);
    }

    [Fact]
    public void Apply_ShouldApplyCheerUpStateToAllTargets()
    {
        // Act
        _skill.Apply(_game, _executant, _allies);

        // Assert
        foreach (var ally in _allies)
        {
            var state = ally.State;
            Assert.IsType<CheerUpState>(state);
            Assert.Equal(3, state.LeftRounds);
            Assert.Equal("受到鼓舞", state.Name);
        }
    }

    [Fact]
    public void Execute_WithInsufficientMP_ShouldThrowException()
    {
        // Arrange
        _executant.MagicPoint = 50; // Less than required 100 MP

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _skill.Execute(_game, _executant, [])
        );
        Assert.Equal("魔力不足", exception.Message);
    }

    [Fact]
    public void Execute_WithSufficientMP_ShouldDeductMP()
    {
        // Act
        _skill.Execute(_game, _executant, _allies);

        // Assert
        Assert.Equal(100, _executant.MagicPoint); // 200 - 100 = 100
    }

    [Fact]
    public void CheerUpState_ShouldIncreaseDamageAndDecreaseRounds()
    {
        // Arrange

        var enemy = new Role(_gameIO)
        {
            Name = "Enemy",
            HealthPoint = 200,
            MagicPoint = 100,
            Strength = 10,
            IsHero = false
        };
        enemy.EnterState(new NormalState(_gameIO));
        _game.AddRole("B", enemy);

        _skill.Apply(_game, _executant, _allies);

        // Act & Assert - Verify damage increase
        var role = _allies.First();
        role.Attack(enemy, 100);
        Assert.Equal(50, enemy.HealthPoint); // 200 - (100 + 50) = 50

        // Act & Assert - Verify rounds decrease
        Assert.IsType<CheerUpState>(role.State);
        role.State.AfterTakeAction();
        Assert.IsType<CheerUpState>(role.State);
        Assert.Equal(2, role.State.LeftRounds);
    }

    [Fact]
    public void FormatExecuteMessage_ShouldReturnCorrectMessage()
    {
        // Act
        var message = _skill.FormatExecuteMessage(_executant, _allies);

        // Assert
        Assert.Equal("[A]Executant 對 [A]Ally1, [A]Ally2, [A]Ally3 使用了 鼓舞。", message);
    }
}
