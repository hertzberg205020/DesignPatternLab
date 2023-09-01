using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class DistanceBasedMatchStrategy: IMatchingStrategy
{
    public Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> individuals)
    {
        // first sort the individuals by distance
        // then return the first one
        individuals = individuals.
            OrderBy(other => individual.Location.DistanceTo(other.Location))
            .ToList();
        
        return individuals.First();
    }
}