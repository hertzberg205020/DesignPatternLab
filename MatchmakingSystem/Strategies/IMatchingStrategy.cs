using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public interface IMatchingStrategy
{
    Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> individuals);
}