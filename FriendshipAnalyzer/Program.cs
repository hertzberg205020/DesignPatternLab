using FriendshipAnalyzer.Advanced.Models;
using FriendshipAnalyzer.Basic.Models;
using FriendshipAnalyzer.Collaborator;

namespace FriendshipAnalyzer;

class Program
{
    static async Task Main(string[] args)
    {
        var script = await SetUp();
        AdvancedDesignTest(script);
    }

    private static void BasicDesignTest(string script)
    {
        var adapter = new Adapter(new SuperRelationshipAnalyzer());
        adapter.Parse(script);
        var mutualFriends = adapter.GetMutualFriends("B", "C");
        Console.WriteLine("Mutual friends of B and C:");
        Console.WriteLine("----------------------------");
        var friends = mutualFriends as string[] ?? mutualFriends.ToArray();
        Console.WriteLine($"Number of mutual friends: {friends.Length}");
        foreach (var friend in friends)
        {
            Console.WriteLine(friend);
        }
    }

    private static void AdvancedDesignTest(string script)
    {
        var analyzer = new RelationAnalyzer();
        var graph = analyzer.Parse(script);
        var res = graph.HasConnection("A", "E");
        if (res)
        {
            Console.WriteLine("A and E are connected");
            var path = graph.FindConnectionPath("A", "E");
            Console.WriteLine("Path from A to E:");
            Console.WriteLine("------------------");
            // use " -> "
            Console.WriteLine(string.Join(" -> ", path));
        }
        else
        {
            Console.WriteLine("A and E are not connected");
        }
        Console.WriteLine(res);
    }

    private static async Task<string> SetUp()
    {
        var root = AppDomain.CurrentDomain.BaseDirectory;
        var filePath = Path.Combine(root, "Resources", "data.txt");
        var script = await ParseAsync(filePath);
        return script;
    }

    private static async Task<string> ParseAsync(string filePath)
    {
        return await File.ReadAllTextAsync(filePath);
    }
}
