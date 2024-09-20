namespace FriendshipAnalyzer.Basic.Models;

public interface IRelationshipAnalyzer
{
    void Parse(string script);

    IEnumerable<string> GetMutualFriends(string name1, string name2);
}
