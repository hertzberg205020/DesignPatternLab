using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions.Skills;

public class SkillTests
{
    private class TestSkill : Skill
    {
        public TestSkill(string name, int mpCost, TargetType targetType, IGameIO gameIO)
            : base(name, mpCost, targetType, gameIO) { }

        public override int GetRequiredTargetCount(Game game, Role self)
        {
            return 1;
        }

        public override void Apply(Game game, Role executant, List<Role> targets)
        {
            // 測試用簡單實作
        }

        public override string FormatExecuteMessage(Role executant, List<Role> targets)
        {
            return $"{executant} 使用了 {Name}";
        }
    }

    private readonly Game _game;
    private readonly Role _executant;
    private readonly Role _target;
    private readonly List<Role> _targets;
    private readonly TestGameIO _gameIO;

    public SkillTests()
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
        _target = new Role(_gameIO)
        {
            Name = "Target",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 30,
            IsHero = false
        };
        _targets = new List<Role> { _target };

        _game.AddRole("A", _executant);

        foreach (var target in _targets)
        {
            _game.AddRole("B", target);
        }
    }

    [Theory]
    [InlineData("火球", 50, TargetType.Enemy)]
    [InlineData("治癒", 30, TargetType.Ally)]
    [InlineData("召喚", 150, TargetType.None)]
    public void Constructor_ShouldInitializePropertiesCorrectly(
        string name,
        int mpCost,
        TargetType targetType
    )
    {
        // Arrange & Act
        var skill = new TestSkill(name, mpCost, targetType, _gameIO);

        // Assert
        Assert.Equal(name, skill.Name);
        Assert.Equal(mpCost, skill.MpCost);
        Assert.Equal(targetType, skill.TargetType);
    }

    [Fact]
    public void Execute_WithInsufficientMp_ShouldNotExecuteAndThrowException()
    {
        // Arrange
        var skill = new TestSkill("測試技能", 150, TargetType.Enemy, _gameIO); // MP cost > executant's MP
        var executantWithLowMp = new Role(_gameIO)
        {
            Name = "ExecutantWithLowMp",
            HealthPoint = 100,
            MagicPoint = 50, // MP = 50
            Strength = 50,
            IsHero = true
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => skill.Execute(_game, executantWithLowMp, _targets)
        );
        Assert.Contains("魔力不足", exception.Message);
    }

    [Fact]
    public void Execute_WithSufficientMp_ShouldDeductMp()
    {
        // Arrange
        const int mpCost = 30;
        var skill = new TestSkill("測試技能", mpCost, TargetType.Enemy, _gameIO);
        var expectedMp = _executant.MagicPoint - mpCost;

        // Act
        skill.Execute(_game, _executant, _targets);

        // Assert
        Assert.Equal(expectedMp, _executant.MagicPoint);
    }

    [Fact]
    public void Execute_ShouldDisplayCorrectMessage()
    {
        // Arrange
        var skill = new TestSkill("測試技能", 30, TargetType.Enemy, _gameIO);

        // Act
        skill.Execute(_game, _executant, _targets);

        // Assert
        var output = _gameIO.Outputs[0];
        Assert.Equal($"{_executant} 使用了 {skill.Name}", output);
    }

    [Theory]
    [InlineData(TargetType.Enemy)]
    [InlineData(TargetType.Ally)]
    [InlineData(TargetType.None)]
    public void Execute_WithWrongTargetType_ShouldThrowException(TargetType skillTargetType)
    {
        // Arrange
        var skill = new TestSkill("測試技能", 30, skillTargetType, _gameIO);
        var wrongTarget = new Role(_gameIO)
        {
            Name = "WrongTarget",
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 30,
            IsHero = false
        };

        _game.AddRole("A", _executant);

        if (skillTargetType == TargetType.Enemy)
        {
            _game.AddRole("A", wrongTarget);
        }
        else
        {
            _game.AddRole("B", wrongTarget);
        }

        var wrongTargets = new List<Role> { wrongTarget };

        // Act & Assert
        Assert.Throws<InvalidOperationException>(
            () => skill.Execute(_game, _executant, wrongTargets)
        );
    }

    [Fact]
    public void ToString_ShouldReturnSkillName()
    {
        // Arrange
        const string skillName = "測試技能";
        var skill = new TestSkill(skillName, 30, TargetType.Enemy, _gameIO);

        // Act
        var result = skill.ToString();

        // Assert
        Assert.Equal(skillName, result);
    }
}
