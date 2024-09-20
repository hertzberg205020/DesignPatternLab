namespace FriendshipAnalyzer.Advanced.Models;

public interface IRelationshipAnalyzer
{
    IRelationshipGraph Parse(string script);
}
