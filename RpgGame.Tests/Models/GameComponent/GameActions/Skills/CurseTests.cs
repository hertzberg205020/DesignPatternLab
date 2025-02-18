using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class CurseTests
{
    private readonly Game _game;
    private readonly Role _caster;
    private readonly List<Role> _targets = new();
    private readonly Curse _skill;
    private readonly TestGameIO _gameIO;

    public CurseTests()
    {
        _gameIO = new TestGameIO([]);
        _game = new Game(_gameIO);
        _skill = new Curse(_gameIO);
        _caster = new Role(_gameIO)
        {
            Name = "Caster",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10,
            IsHero = false
        };
        _caster.EnterState(new NormalState(_gameIO));
        _game.AddRole("A", _caster);
        _caster.Skills.Add(_skill);

        var target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10,
            IsHero = false
        };
        target.EnterState(new NormalState(_gameIO));
        _game.AddRole("B", target);
        _targets.Add(target);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        // Arrange & Act
        var curse = new Curse(_gameIO);

        // Assert
        Assert.Equal("詛咒", curse.Name);
        Assert.Equal(100, curse.MpCost);
        Assert.Equal(TargetType.Enemy, curse.TargetType);
    }

    [Fact]
    public void Apply_ShouldCreateCurseRelationship()
    {
        // Act
        _skill.Apply(_game, _caster, _targets);
        var target = _targets.First();

        // Assert
        var observers = target.GetDeathObservers().ToList();
        Assert.Contains(observers, observer => observer is CurseRelationship);

        var curseRelation = observers.OfType<CurseRelationship>().First();
        Assert.Equal(_caster, curseRelation.Caster);
        Assert.Equal(target, curseRelation.Target);
    }

    [Fact]
    public void Apply_WithNullParameters_ShouldThrowArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => _skill.Apply(null, _caster, _targets));
        Assert.Throws<ArgumentNullException>(() => _skill.Apply(_game, null, _targets));
        Assert.Throws<ArgumentNullException>(() => _skill.Apply(_game, _caster, null));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetRequiredTargetCount_ShouldReturnOne(int targetCount)
    {
        // Arrange
        _targets.Clear();
        for (var i = 0; i < targetCount; i++)
        {
            var target = new Role(_gameIO)
            {
                Name = $"Target{i}",
                HealthPoint = 100,
                MagicPoint = 100,
                Strength = 10,
                IsHero = false
            };
            target.EnterState(new NormalState(_gameIO));
            _game.AddRole($"B{i}", target);
            _targets.Add(target);
        }

        // Act
        var count = _skill.GetRequiredTargetCount(_game, _caster);

        // Assert
        Assert.Equal(1, count);
    }

    [Fact]
    public void GetRequiredTargetCount_WithNullParameter_ShouldThrowArgumentNullException()
    {
        // Assert
        Assert.Throws<ArgumentNullException>(() => _skill.GetRequiredTargetCount(null, _caster));
        Assert.Throws<ArgumentNullException>(() => _skill.GetRequiredTargetCount(_game, null));
    }

    [Fact]
    public void Execute_WithMoreThanOneTarget_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _targets.Add(
            new Role(_gameIO)
            {
                Name = "Target2",
                HealthPoint = 100,
                MagicPoint = 100,
                Strength = 10,
                IsHero = false
            }
        );

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _skill.Apply(_game, _caster, _targets));
    }

    [Fact]
    public void Execute_ShouldNotCreateCurseRelationshipIfTargetIsAlreadyCursed()
    {
        // Arrange
        _skill.Apply(_game, _caster, _targets);

        // Act
        _skill.Apply(_game, _caster, _targets); // Apply curse again

        // Assert
        var target = _targets.First();
        var observers = target.GetDeathObservers().ToList();
        Assert.Single(observers);
        var curseRelations = observers.OfType<CurseRelationship>().Count();
        Assert.Equal(1, curseRelations);
    }

    [Fact]
    public void Execute_WithInsufficientMP_ShouldThrowException()
    {
        // Arrange
        _caster.MagicPoint = 50; // Less than required 100 MP

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _skill.Execute(_game, _caster, _targets)
        );
        Assert.Equal("魔力不足", exception.Message);
    }

    [Fact]
    public void Execute_WithSufficientMP_ShouldDeductMP()
    {
        // Arrange
        _caster.MagicPoint = 150; // More than required 100 MP

        // Act
        _skill.Execute(_game, _caster, _targets);

        // Assert
        Assert.Equal(50, _caster.MagicPoint); // 150 - 100 = 50
    }

    [Fact]
    public void CurseRelationship_WhenTargetDies_ShouldHealCaster()
    {
        // Arrange
        var target = _targets.First();

        _skill.Apply(_game, _caster, _targets);

        // Act
        target.TakeDamage(100); // Kill the target

        // Assert
        Assert.Equal(200, _caster.HealthPoint); // 100 + target's MP (100) = 200
    }

    [Fact]
    public void CurseRelationship_WhenCasterIsDead_ShouldNotHealOnTargetDeath()
    {
        _skill.Apply(_game, _caster, _targets);
        _caster.TakeDamage(100); // Kill the caster

        // Act
        var target = _targets.First();
        target.TakeDamage(100); // Kill the target

        // Assert
        Assert.Equal(0, _caster.HealthPoint); // Should remain dead
    }

    [Fact]
    public void FormatExecuteMessage_ShouldReturnCorrectMessage()
    {
        // Act
        var message = _skill.FormatExecuteMessage(_caster, _targets);

        // Assert
        Assert.Equal("[A]Caster 對 [B]Target 使用了 詛咒。", message);
    }

    [Fact]
    public void CurseRelationship_Equals_ShouldWorkCorrectly()
    {
        // Arrange
        var target = _targets.First();
        var relation1 = new CurseRelationship(_caster, target);
        var relation2 = new CurseRelationship(_caster, target);
        var relation3 = new CurseRelationship(target, _caster); // Swapped roles

        // Act & Assert
        Assert.Equal(relation1, relation2);
        Assert.NotEqual(relation1, relation3);
        Assert.NotEqual(relation2, relation3);
    }
}
