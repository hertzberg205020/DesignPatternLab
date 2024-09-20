using System.Text;
using System.Text.RegularExpressions;

namespace FriendshipAnalyzer.Advanced.Models;

public class RelationAnalyzer : IRelationshipAnalyzer
{
    public IRelationshipGraph Parse(string script)
    {
        var convertedContent = new StringBuilder();
        var lines = Regex.Split(script, @"\r\n|\r|\n");
        var graph = new RelationshipGraph();

        foreach (var line in lines)
        {
            var parts = line.Split(':');

            if (parts.Length == 2)
            {
                var node = parts[0].Trim();

                var connections = parts[1].Trim().Split(' ');

                foreach (var connection in connections)
                {
                    graph.AddFriendship(node, connection);
                }
            }
        }

        return graph;
    }
}
