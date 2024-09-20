using System.Text.RegularExpressions;

namespace FriendshipAnalyzer.Collaborator;

public class SuperRelationshipAnalyzer
{
    private Dictionary<string, List<string>> _adjacencyList;

    public SuperRelationshipAnalyzer()
    {
        _adjacencyList = new Dictionary<string, List<string>>();
    }

    public void InitializeGraph()
    {
        _adjacencyList = new Dictionary<string, List<string>>();
    }

    public void Init(string script)
    {
        var lines = Regex.Split(script, @"\r\n|\r|\n");

        foreach (var line in lines)
        {
            var nodes = line.Split(new string[] { " -- " }, StringSplitOptions.None);
            if (nodes.Length == 2)
            {
                AddEdge(nodes[0], nodes[1]);
                AddEdge(nodes[1], nodes[0]); // 因為是無向圖，所以要加兩次
            }
        }
    }

    private void AddEdge(string node1, string node2)
    {
        if (!_adjacencyList.ContainsKey(node1))
        {
            _adjacencyList[node1] = new List<string>();
        }
        _adjacencyList[node1].Add(node2);
    }

    public bool IsMutualFriend(string targetName, string name1, string name2)
    {
        if (
            !_adjacencyList.ContainsKey(targetName)
            || !_adjacencyList.ContainsKey(name1)
            || !_adjacencyList.ContainsKey(name2)
        )
        {
            return false;
        }

        return _adjacencyList[targetName].Contains(name1)
            && _adjacencyList[targetName].Contains(name2);
    }
}
