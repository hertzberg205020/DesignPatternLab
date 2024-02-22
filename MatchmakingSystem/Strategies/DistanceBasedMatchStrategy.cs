using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class DistanceBasedMatchStrategy: IMatchingStrategy
{
    // public Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> individuals)
    // {
    //     // first sort the individuals by distance
    //     // then return the first one
    //     individuals = individuals.
    //         OrderBy(other => individual.Location.DistanceTo(other.Location))
    //         .ToList();
    //     
    //     return individuals.First();
    // }

    public List<IndividualPair> Match(Individual target, List<Individual> individuals)
    {
        var pairs = individuals.Select(individual => new IndividualPair(target, individual)).ToList();
        
        pairs = ApplyStrategy(pairs);
        
        return pairs;
    }

    public List<IndividualPair> ApplyStrategy(List<IndividualPair> unSortedPairs)
    {
        foreach (var unSortedPair in unSortedPairs)
        {
            unSortedPair.Score = GetMatchedScore(unSortedPair.Individual1, unSortedPair.Individual2);
        }
        
        return unSortedPairs.OrderByDescending(pair => pair.Score).ToList();
    }

    public double GetMatchedScore(Individual individual1, Individual individual2)
    {
        return -individual1.Location.DistanceTo(individual2.Location);
    }
}