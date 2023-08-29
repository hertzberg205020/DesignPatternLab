namespace ShowDown.Models;

public class ExchangeHandsRelationship
{
    private readonly Player _proposer;

    private readonly Player _otherPlayer;

    private int Round { get; set; } = 0;

    public ExchangeHandsRelationship(Player proposer, Player otherPlayer)
    {
        _proposer = proposer;
        _otherPlayer = otherPlayer;
    }
    
    public void UpdateState()
    {
        AddOneRound();
        RevertHandCardsAfter3Rounds();
    }

    private void AddOneRound()
    {
        Round += 1;
    }

    private bool IsTimeToRevert()
    {
        return Round >= 3;
    }

    private void RevertHandCardsAfter3Rounds()
    {
        if(IsTimeToRevert())
        {
            (_proposer.HandCards, _otherPlayer.HandCards) = (_otherPlayer.HandCards, _proposer.HandCards);
        }
    }
    
}