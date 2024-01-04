namespace MatchmakingSystem.Models;

public class IndividualPair
{
    public Individual Individual1 { get; }
    public Individual Individual2 { get; }

    public double Score { get; set; }

    public IndividualPair(Individual individual1, Individual individual2)
    {
        Individual1 = individual1;
        Individual2 = individual2;
    }

}