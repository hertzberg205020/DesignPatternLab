using MatchmakingSystem.Strategies;

namespace MatchmakingSystem.Models;

public class MatchMakingSystem
{
    private readonly IMatchingStrategy _matchingStrategy;
    
    private readonly IEnumerable<Individual> _members;

    public MatchMakingSystem(IMatchingStrategy matchingStrategy, ICollection<Individual> individuals)
    {
        _matchingStrategy = matchingStrategy;
        _members = individuals;
    }
}