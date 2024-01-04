using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class HabitsBasedMatchStrategy: IMatchingStrategy
{
    public Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> individuals)
    {
        // first sort the individuals by habits similarity
        // then return the first one
        individuals = individuals
            .OrderByDescending(other => individual.habits.SimilarityTo(other.habits))
            .ToList();
        
        return individuals.First();
    }

    public List<IndividualPair> Match(Individual target, List<Individual> individuals)
    {
        var pairs = individuals.Select(individual => new IndividualPair(target, individual)).ToList();
        
        pairs = ApplyStrategy(pairs);
        
        return pairs;
    }

    public List<IndividualPair> ApplyStrategy(List<IndividualPair> unSortedPairs)
    {
        foreach (var pair in unSortedPairs)
        {
            pair.Score = GetMatchedScore(pair.Individual1, pair.Individual2);
        }

        return unSortedPairs.OrderByDescending(pair => pair.Score).ToList();
    }

    public double GetMatchedScore(Individual individual1, Individual individual2)
    {
        return individual1.habits.SimilarityTo(individual2.habits);
    }
}