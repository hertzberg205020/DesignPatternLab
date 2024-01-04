using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class ReverseStrategy: IMatchingStrategy
{
    private readonly IMatchingStrategy _matchingStrategy;

    public ReverseStrategy(IMatchingStrategy matchingStrategy)
    {
        _matchingStrategy = matchingStrategy;
    }

    public Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> individuals)
    {
        // use the original strategy to sort the individuals
        // then return the last one
        _matchingStrategy.FindBestMatchForIndividual(individual, ref individuals);
        return individuals.Last();
    }

    public List<IndividualPair> Match(Individual target, List<Individual> individuals)
    {
        var pairs = individuals.Select(individual => new IndividualPair(target, individual)).ToList();
        
        return ApplyStrategy(pairs);
    }

    public List<IndividualPair> ApplyStrategy(List<IndividualPair> unSortedPairs)
    {
        foreach (var unSortedPair in unSortedPairs)
        {
            unSortedPair.Score = GetMatchedScore(unSortedPair.Individual1, unSortedPair.Individual2);
        }
        
        return unSortedPairs.OrderBy(pair => pair.Score).ToList();
    }

    public double GetMatchedScore(Individual individual1, Individual individual2)
    {
        return _matchingStrategy.GetMatchedScore(individual1, individual2);
    }
}