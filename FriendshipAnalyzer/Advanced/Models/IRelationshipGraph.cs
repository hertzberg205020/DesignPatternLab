namespace FriendshipAnalyzer.Advanced.Models;

public interface IRelationshipGraph
{
    bool HasConnection(string name1, string name2);
    List<string> FindConnectionPath(string name1, string name2);
}
