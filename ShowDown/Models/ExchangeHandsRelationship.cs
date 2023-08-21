namespace ShowDown.Models;

public class ExchangeHandsRelationship
{
    public Player Proposer { get; set; }
    
    public Player OtherPlayer { get; set; }

    public int Round { get; private set; } = 0;

    public ExchangeHandsRelationship(Player proposer, Player otherPlayer)
    {
        Proposer = proposer;
        OtherPlayer = otherPlayer;
    }
    
    public void AddOneRound()
    {
        Round += 1;
    }
    
    public bool IsTimeToRevert()
    {
        return Round >= 3;
    }
    
}