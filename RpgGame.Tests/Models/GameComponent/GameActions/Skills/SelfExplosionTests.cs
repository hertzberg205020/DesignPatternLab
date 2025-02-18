using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class SelfExplosionTests
{
    private readonly Game _game;
    private readonly Role _executant;
    private readonly List<Role> _targets;
    private readonly SelfExplosion _skill;
    private readonly TestGameIO _gameIO;

    public SelfExplosionTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _skill = new SelfExplosion(_gameIO);
        var troop1 = "Allies";
        var troop2 = "Enemies";

        _executant = new Role(_gameIO)
        {
            Name = "Bomber",
            HealthPoint = 100,
            MagicPoint = 200,
            Strength = 10,
            IsHero = false
        };

        _executant.EnterState(new NormalState(_gameIO));
        _executant.Skills.Add(_skill);
        _game.AddRole(troop1, _executant);

        var ally = new Role(_gameIO)
        {
            Name = "Ally",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10,
            IsHero = false
        };

        ally.EnterState(new NormalState(_gameIO));
        _game.AddRole(troop1, ally);

        var enemy1 = new Role(_gameIO)
        {
            Name = "Enemy1",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10,
            IsHero = false
        };

        enemy1.EnterState(new NormalState(_gameIO));
        _game.AddRole(troop2, enemy1);

        var enemy2 = new Role(_gameIO)
        {
            Name = "Enemy2",
            HealthPoint = 0, // Dead enemy
            MagicPoint = 100,
            Strength = 10,
            IsHero = false
        };
        enemy2.EnterState(new NormalState(_gameIO));
        _game.AddRole(troop2, enemy2);

        _targets = new List<Role> { ally, enemy1, enemy2 };
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var selfExplosion = _skill;

        // Assert
        Assert.Equal("自爆", selfExplosion.Name);
        Assert.Equal(200, selfExplosion.MpCost);
        Assert.Equal(TargetType.All, selfExplosion.TargetType);
    }

    [Fact]
    public void GetRequiredTargetCount_ShouldReturnAllAliveRolesExceptSelf()
    {
        // Act
        var count = _skill.GetRequiredTargetCount(_game, _executant);

        // Assert
        Assert.Equal(2, count); // Only ally (including the executant) and enemy1 are alive, minus self = 1
    }

    [Fact]
    public void Apply_ShouldDamageAllTargetsAndKillExecutant()
    {
        // Act
        _skill.Apply(_game, _executant, _targets);

        // Assert
        Assert.True(_executant.IsDead()); // Executant should be dead
        Assert.IsType<DeadState>(_executant.State); // Executant should be dead

        foreach (var target in _targets)
        {
            Assert.Equal(0, target.HealthPoint); // 100 - 50 = 50
            Assert.True(target.IsDead()); // All targets should
            Assert.IsType<DeadState>(target.State); // be dead
        }
    }

    [Fact]
    public void Execute_WithInsufficientMP_ShouldThrowException()
    {
        // Arrange
        _executant.MagicPoint = 150; // MP < 200

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _skill.Execute(_game, _executant, new List<Role>())
        );
        Assert.Equal("魔力不足", exception.Message);
    }

    [Fact]
    public void Execute_WithSufficientMP_ShouldDeductMP()
    {
        // Arrange
        _executant.MagicPoint = 250; // MP >= 200

        // Act
        _skill.Execute(_game, _executant, new List<Role>());

        // Assert
        Assert.Equal(50, _executant.MagicPoint); // 250 - 200 = 50
    }

    [Fact]
    public void FormatExecuteMessage_ShouldReturnCorrectMessage()
    {
        // Act
        var message = _skill.FormatExecuteMessage(_executant, _targets);

        // Assert
        Assert.Equal(
            "[Allies]Bomber 對 [Allies]Ally, [Enemies]Enemy1, [Enemies]Enemy2 使用了 自爆!",
            message
        );
    }
}
