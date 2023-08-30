﻿using MatchmakingSystem.Models;

namespace MatchmakingSystem.Strategies;

public class HabitsBasedMatchMakingStrategy: IMatchingStrategy
{
    public Individual FindBestMatchForIndividual(Individual individual, IEnumerable<Individual> individuals)
    {
        var members = individuals.ToList();
        var bestMatch = members.First();
        var highestSimilarity = individual.HabitsSimilarityTo(bestMatch);
        
        foreach (var otherIndividual in members.Skip(1))
        {
            var similarity = individual.HabitsSimilarityTo(otherIndividual);
            if (similarity > highestSimilarity)
            {
                bestMatch = otherIndividual;
                highestSimilarity = similarity;
            }
        }
        
        return bestMatch;
    }
}