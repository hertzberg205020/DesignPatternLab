using System.Text.RegularExpressions;
using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public interface IMatchingStrategy
{
    Individual FindBestMatchForIndividual(Individual individual, ref List<Individual> individuals);

    List<IndividualPair> Match(Individual target, List<Individual> individuals);

    List<IndividualPair> ApplyStrategy(List<IndividualPair> unSortedPairs);

    Double GetMatchedScore(Individual individual1, Individual individual2);
}
