using System.Text;
using System.Text.RegularExpressions;
using FriendshipAnalyzer.Collaborator;

namespace FriendshipAnalyzer.Basic.Models;

public class Adapter : IRelationshipAnalyzer
{
    private readonly SuperRelationshipAnalyzer _superRelationshipAnalyzer;
    private readonly HashSet<string> _processedNodes = new();

    public Adapter(SuperRelationshipAnalyzer superRelationshipAnalyzer)
    {
        _superRelationshipAnalyzer = superRelationshipAnalyzer;
    }

    public void Parse(string script)
    {
        var convertedContent = new StringBuilder();
        string[] lines = Regex.Split(script, @"\r\n|\r|\n");

        foreach (var line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2)
            {
                var node = parts[0].Trim();
                _processedNodes.Add(node);
                var connections = parts[1].Trim().Split(' ');

                foreach (var connection in connections)
                {
                    convertedContent.AppendLine($"{node} -- {connection}");
                }
            }
        }

        _superRelationshipAnalyzer.Init(convertedContent.ToString());
    }

    public IEnumerable<string> GetMutualFriends(string name1, string name2)
    {
        foreach (var node in _processedNodes)
        {
            if (_superRelationshipAnalyzer.IsMutualFriend(node, name1, name2))
            {
                yield return node;
            }
        }
    }
}
