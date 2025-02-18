using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.DecisionStrategies;

public class HumanDecisionMakerTests
{
    private readonly TestGameIO _gameIO;

    public HumanDecisionMakerTests()
    {
        _gameIO = new TestGameIO([]);
    }

    [Fact]
    public void SelectAction_ValidInput_ReturnsCorrectAction()
    {
        // Arrange
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var action1 = new BasicAttack(_gameIO);
        var action2 = new WaterBall(_gameIO);
        var actions = new List<GameAction> { action1, action2 };
        _gameIO.Input("0");

        // Act

        var selectedAction = decisionMaker.SelectAction(actions);

        // Assert
        Assert.Same(action1, selectedAction);
    }

    [Fact]
    public void SelectAction_InvalidThenValidInput_ReturnsCorrectAction()
    {
        // Arrange
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var action1 = new BasicAttack(_gameIO);
        var action2 = new WaterBall(_gameIO);
        var actions = new List<GameAction> { action1, action2 };

        _gameIO.Input("-1");
        _gameIO.Input("2");
        _gameIO.Input("abc");
        _gameIO.Input("1");

        // Act
        var selectedAction = decisionMaker.SelectAction(actions);

        // Assert
        Assert.Same(action2, selectedAction);
    }

    [Fact]
    public void SelectAction_NullActions_ThrowsArgumentNullException()
    {
        // Arrange
        var decisionMaker = new HumanDecisionMaker(_gameIO);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => decisionMaker.SelectAction(null!));
    }

    [Fact]
    public void SelectAction_EmptyActions_ThrowsArgumentException()
    {
        // Arrange
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var actions = new List<GameAction>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => decisionMaker.SelectAction(actions));
    }

    [Fact]
    public void SelectTargets_SingleTarget_ValidInput_ReturnsCorrectTarget()
    {
        // Arrange
        var game = new Game(_gameIO);
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var target1 = new Role(_gameIO)
        {
            Name = "Target1",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target1.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target1);

        var target2 = new Role(_gameIO)
        {
            Name = "Target2",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target2.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target2);

        var candidates = new List<Role> { target1, target2 };

        _gameIO.Input("0");

        // Act
        var selectedTargets = decisionMaker.SelectTargets(candidates, 1);

        // Assert
        Assert.Single(selectedTargets);
        Assert.Same(target1, selectedTargets[0]);
    }

    [Fact]
    public void SelectTargets_MultipleTargets_ValidInput_ReturnsCorrectTargets()
    {
        // Arrange
        var game = new Game(_gameIO);
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var target1 = new Role(_gameIO)
        {
            Name = "Target1",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target1.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target1);
        var target2 = new Role(_gameIO)
        {
            Name = "Target2",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target2.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target2);
        var target3 = new Role(_gameIO)
        {
            Name = "Target3",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target3.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target3);

        var candidates = new List<Role> { target1, target2, target3 };

        _gameIO.Input("0, 2"); // 選擇第一個和第三個目標
        // Act
        var selectedTargets = decisionMaker.SelectTargets(candidates, 2);

        // Assert
        Assert.Equal(2, selectedTargets.Count);
        Assert.Same(target1, selectedTargets[0]);
        Assert.Same(target3, selectedTargets[1]);
    }

    [Fact]
    public void SelectTargets_InvalidThenValidInput_ReturnsCorrectTargets()
    {
        // Arrange
        var game = new Game(_gameIO);
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var target1 = new Role(_gameIO)
        {
            Name = "Target1",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target1.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target1);

        var target2 = new Role(_gameIO)
        {
            Name = "Target2",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var candidates = new List<Role> { target1, target2 };
        target2.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target2);

        // 多次無效輸入後選擇第二個目標
        _gameIO.Input("-1, 3, abc");
        _gameIO.Input("1");

        // Act
        var selectedTargets = decisionMaker.SelectTargets(candidates, 1);

        // Assert
        Assert.Single(selectedTargets);
        Assert.Same(target2, selectedTargets[0]);
    }

    [Fact]
    public void SelectTargets_NullCandidates_ThrowsArgumentNullException()
    {
        // Arrange
        var decisionMaker = new HumanDecisionMaker(_gameIO);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => decisionMaker.SelectTargets(null!, 1));
    }

    [Fact]
    public void SelectTargets_EmptyCandidates_ThrowsArgumentException()
    {
        // Arrange
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var candidates = new List<Role>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => decisionMaker.SelectTargets(candidates, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void SelectTargets_InvalidRequiredCount_ThrowsArgumentException(int requiredCount)
    {
        // Arrange
        var game = new Game(_gameIO);
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var target = new Role(_gameIO)
        {
            Name = "Target",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target);

        var candidates = new List<Role> { target };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => decisionMaker.SelectTargets(candidates, requiredCount)
        );
    }

    [Fact]
    public void SelectTargets_RequiredCountGreaterThanCandidates_ReturnsAllCandidates()
    {
        // Arrange
        var game = new Game(_gameIO);
        var decisionMaker = new HumanDecisionMaker(_gameIO);
        var target1 = new Role(_gameIO)
        {
            Name = "Target1",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target1.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target1);

        var target2 = new Role(_gameIO)
        {
            Name = "Target2",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        target2.EnterState(new NormalState(_gameIO));
        game.AddRole("A", target2);

        var candidates = new List<Role> { target1, target2 };

        // 選擇所有目標
        _gameIO.Input("0, 1");

        // Act
        var selectedTargets = decisionMaker.SelectTargets(candidates, 3);

        // Assert
        Assert.Equal(2, selectedTargets.Count);
        Assert.Same(target1, selectedTargets[0]);
        Assert.Same(target2, selectedTargets[1]);
    }
}
