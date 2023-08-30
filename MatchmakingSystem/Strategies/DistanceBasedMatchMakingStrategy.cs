using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class DistanceBasedMatchMakingStrategy: IMatchingStrategy
{
    public Individual FindBestMatchForIndividual(Individual individual, IEnumerable<Individual> individuals)
    {
        var members = individuals.ToList();
        var bestMatch = members.First();
        var shortestDistance = individual.Location.DistanceTo(bestMatch.Location);
        foreach (var otherIndividual in members.Skip(1))
        {
            var distance = individual.Location.DistanceTo(otherIndividual.Location);
            if (distance < shortestDistance)
            {
                bestMatch = otherIndividual;
                shortestDistance = distance;
            }
        }

        return bestMatch;
    }
}