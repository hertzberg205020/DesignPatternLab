using MatchmakingSystem.Strategies;

namespace MatchmakingSystem.Models;

public class MatchMakingSystem
{
    private readonly IMatchingStrategy _matchingStrategy;
    
    public MatchMakingSystem(IMatchingStrategy matchingStrategy)
    {
        _matchingStrategy = matchingStrategy;
    }
    
    // public Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> members)
    // {
    //     return _matchingStrategy.FindBestMatchForIndividual(individual, ref members);
    // }
    
    public Individual FindBestMatchForIndividual(Individual individual, List<Individual> members) => _matchingStrategy.Match(individual, members).First().Individual2;
}