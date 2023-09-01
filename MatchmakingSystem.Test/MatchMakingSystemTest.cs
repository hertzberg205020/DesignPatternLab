using FluentAssertions;
using MatchmakingSystem.Models;
using MatchmakingSystem.Strategies;
using Xunit.Abstractions;

namespace MatchmakingSystem.Test;

public class MatchMakingSystemTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public MatchMakingSystemTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private Individual Target { get; } = new Individual(1, "Male", 19, "", new Coord(1.0, 1.0), "跑步", "游泳", "打羽球");


    private static List<Individual> CreatIndividuals()
    {
        return new List<Individual>()
        {
            new Individual(2, "Female", 20, "", new Coord(2.0, 2.0), "跑步", "游泳", "旅遊"),
            new Individual(3, "Female", 21, "", new Coord(3.0, 3.0), "打撞球", "旅遊", "瑜珈"),
            new Individual(4, "Male", 22, "", new Coord(4.0, 4.0), "閱讀", "游泳", "打羽球", "跑步"),
        };
    }

    [Fact]
    public void HabitsBasedStrategyTest()
    {
        var individuals = CreatIndividuals();

        var strategy = new HabitsBasedMatchStrategy();
        var system = new MatchMakingSystem(strategy);
        var result = system.FindBestMatchForIndividual(Target, ref individuals);
        result.Id.Should().Be(4);
    }
    
    [Fact]
    public void HabitsBasedReverseStrategyTest()
    {
        var individuals = CreatIndividuals();

        var strategy = new ReverseStrategy(new HabitsBasedMatchStrategy());
        var system = new MatchMakingSystem(strategy);
        var result = system.FindBestMatchForIndividual(Target, ref individuals);
        
        result.Id.Should().Be(3);
    }
    
    [Fact]
    public void DistanceBasedStrategyTest()
    {
        var individuals = CreatIndividuals();

        var strategy = new DistanceBasedMatchStrategy();
        var system = new MatchMakingSystem(strategy);
        var result = system.FindBestMatchForIndividual(Target, ref individuals);
        
        result.Id.Should().Be(2);
    }
    
    [Fact]
    public void DistanceBasedReverseStrategyTest()
    {
        var individuals = CreatIndividuals();

        var strategy = new ReverseStrategy(new DistanceBasedMatchStrategy());
        var system = new MatchMakingSystem(strategy);
        var result = system.FindBestMatchForIndividual(Target, ref individuals);
        
        result.Id.Should().Be(3);
    }
}