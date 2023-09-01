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
}