using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.GameActions;

public class GameActionTests
{
    private readonly TestGameIO _gameIO;

    private class TestGameAction : GameAction
    {
        public TestGameAction(string name, int mpCost, TargetType targetType, IGameIO gameIO)
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
    private readonly List<Role> _targets;

    public GameActionTests()
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
        _targets =
        [
            new Role(_gameIO)
            {
                Name = "Target1",
                HealthPoint = 100,
                MagicPoint = 100,
                Strength = 50,
                IsHero = false
            }
        ];

        _game.AddRole("A", _executant);
        foreach (var target in _targets)
        {
            _game.AddRole("B", target);
        }
    }

    [Fact]
    public void Constructor_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        const string expectedName = "Test Action";
        const int expectedMpCost = 50;
        const TargetType expectedTargetType = TargetType.Enemy;

        // Act
        var action = new TestGameAction(expectedName, expectedMpCost, expectedTargetType, _gameIO);

        // Assert
        Assert.Equal(expectedName, action.Name);
        Assert.Equal(expectedMpCost, action.MpCost);
        Assert.Equal(expectedTargetType, action.TargetType);
    }

    [Fact]
    public void Execute_ShouldCallFormatExecuteMessage()
    {
        // Arrange
        var action = new TestGameAction("Test Action", 50, TargetType.Enemy, _gameIO);

        // Act
        action.Execute(_game, _executant, _targets);

        // Assert
        // var output = stringWriter.ToString().Trim();
        var output = _gameIO.Outputs;
        Assert.Equal($"{_executant} 使用了 {action.Name}", output[0]);
    }

    [Fact]
    public void ToString_ShouldReturnName()
    {
        // Arrange
        const string expectedName = "Test Action";
        var action = new TestGameAction(expectedName, 50, TargetType.Enemy, _gameIO);

        // Act
        var result = action.ToString();

        // Assert
        Assert.Equal(expectedName, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(100)]
    public void MpCost_ShouldBeSettable(int mpCost)
    {
        // Arrange
        var action = new TestGameAction("Test Action", 0, TargetType.Enemy, _gameIO);

        // Act
        action.MpCost = mpCost;

        // Assert
        Assert.Equal(mpCost, action.MpCost);
    }
}
