using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class ReverseStrategy: IMatchingStrategy
{
    private readonly IMatchingStrategy _matchingStrategy;

    public ReverseStrategy(IMatchingStrategy matchingStrategy)
    {
        _matchingStrategy = matchingStrategy;
    }

    public Individual FindBestMatchForIndividual(Individual individual, List<Individual> individuals)
    {
        // use the original strategy to sort the individuals
        // then return the last one
        _matchingStrategy.FindBestMatchForIndividual(individual, individuals);
        return individuals.Last();
    }
}