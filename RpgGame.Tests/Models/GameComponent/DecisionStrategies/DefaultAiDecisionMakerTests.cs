using RpgGame.Tests.Helpers;

namespace RpgGame.Tests.Models.GameComponent.DecisionStrategies;

public class DefaultAiDecisionMakerTests
{
    private readonly TestGameIO _gameIO;

    public DefaultAiDecisionMakerTests()
    {
        _gameIO = new TestGameIO([]);
    }

    [Fact]
    public void SelectAction_SingleAction_ReturnsTheOnlyAction()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
        var action = new BasicAttack(_gameIO);
        var actions = new List<GameAction> { action };

        // Act
        var selectedAction = decisionMaker.SelectAction(actions);

        // Assert
        Assert.Same(action, selectedAction);
    }

    [Fact]
    public void SelectAction_MultipleActions_ReturnsActionsInSequence()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
        var action1 = new BasicAttack(_gameIO);
        var action2 = new WaterBall(_gameIO);
        var action3 = new FireBall(_gameIO);
        var actions = new List<GameAction> { action1, action2, action3 };

        // Act & Assert
        // First call should return first action
        var selected1 = decisionMaker.SelectAction(actions);
        Assert.Same(action1, selected1);

        // Second call should return second action
        var selected2 = decisionMaker.SelectAction(actions);
        Assert.Same(action2, selected2);

        // Third call should return third action
        var selected3 = decisionMaker.SelectAction(actions);
        Assert.Same(action3, selected3);

        // Fourth call should cycle back to first action
        var selected4 = decisionMaker.SelectAction(actions);
        Assert.Same(action1, selected4);
    }

    [Fact]
    public void SelectAction_NullActions_ThrowsArgumentNullException()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => decisionMaker.SelectAction(null!));
    }

    [Fact]
    public void SelectAction_EmptyActions_ThrowsArgumentException()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
        var actions = new List<GameAction>();

        // Act & Assert
        Assert.Throws<ArgumentException>(() => decisionMaker.SelectAction(actions));
    }

    [Fact]
    public void SelectTargets_SingleTarget_ReturnsTheOnlyTarget()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
        var target = new Role(_gameIO)
        {
            Name = "Target",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var candidates = new List<Role> { target };

        // Act
        var selectedTargets = decisionMaker.SelectTargets(candidates, 1);

        // Assert
        Assert.Single(selectedTargets);
        Assert.Same(target, selectedTargets[0]);
    }

    [Fact]
    public void SelectTargets_MultipleTargets_ReturnsTargetsInSequence()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
        var target1 = new Role(_gameIO)
        {
            Name = "Target1",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var target2 = new Role(_gameIO)
        {
            Name = "Target2",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var target3 = new Role(_gameIO)
        {
            Name = "Target3",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var candidates = new List<Role> { target1, target2, target3 };

        // Act
        var selectedTargets1 = decisionMaker.SelectTargets(candidates, 2);
        var selectedTargets2 = decisionMaker.SelectTargets(candidates, 2);

        // Assert
        Assert.Equal(2, selectedTargets1.Count);
        Assert.Equal(2, selectedTargets2.Count);
        Assert.Same(target1, selectedTargets1[0]);
        Assert.Same(target2, selectedTargets1[1]);
        Assert.Same(target2, selectedTargets2[0]);
        Assert.Same(target3, selectedTargets2[1]);
    }

    [Fact]
    public void SelectTargets_RequiredCountGreaterThanCandidates_ReturnsAllCandidates()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
        var target1 = new Role(_gameIO)
        {
            Name = "Target1",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var target2 = new Role(_gameIO)
        {
            Name = "Target2",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var candidates = new List<Role> { target1, target2 };

        // Act
        var selectedTargets = decisionMaker.SelectTargets(candidates, 3);

        // Assert
        Assert.Equal(2, selectedTargets.Count);
        Assert.Same(target1, selectedTargets[0]);
        Assert.Same(target2, selectedTargets[1]);
    }

    [Fact]
    public void SelectTargets_NullCandidates_ThrowsArgumentNullException()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => decisionMaker.SelectTargets(null!, 1));
    }

    [Fact]
    public void SelectTargets_EmptyCandidates_ThrowsArgumentException()
    {
        // Arrange
        var decisionMaker = new DefaultAiDecisionMaker();
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
        var decisionMaker = new DefaultAiDecisionMaker();
        var target = new Role(_gameIO)
        {
            Name = "Target",
            IsHero = false,
            HealthPoint = 100,
            MagicPoint = 100,
            Strength = 10
        };
        var candidates = new List<Role> { target };

        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => decisionMaker.SelectTargets(candidates, requiredCount)
        );
    }
}
