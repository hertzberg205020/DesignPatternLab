namespace FriendshipAnalyzer.Advanced.Models;

public class RelationshipGraph : IRelationshipGraph
{
    private readonly Dictionary<string, HashSet<string>> _people;

    public RelationshipGraph()
    {
        _people = new Dictionary<string, HashSet<string>>();
    }

    private void AddPerson(string name)
    {
        if (!_people.ContainsKey(name))
        {
            _people[name] = new HashSet<string>();
        }
    }

    public void AddFriendship(string name1, string name2)
    {
        if (!_people.ContainsKey(name1))
        {
            AddPerson(name1);
        }
        if (!_people.ContainsKey(name2))
        {
            AddPerson(name2);
        }

        _people[name1].Add(name2);
        _people[name2].Add(name1);
    }

    private List<string> GetFriends(string personName)
    {
        if (_people.TryGetValue(personName, out var friends))
        {
            return new List<string>(friends);
        }
        return [];
    }

    public List<string> GetMutualFriends(string person1, string person2)
    {
        if (_people.ContainsKey(person1) && _people.ContainsKey(person2))
        {
            var friends1 = new HashSet<string>(GetFriends(person1));
            var friends2 = new HashSet<string>(GetFriends(person2));
            friends1.IntersectWith(friends2);
            return new List<string>(friends1);
        }
        return new List<string>();
    }

    public bool HasConnection(string person1, string person2)
    {
        if (!_people.ContainsKey(person1) || !_people.ContainsKey(person2))
        {
            return false;
        }

        if (person1 == person2)
        {
            return true;
        }

        var visited = new HashSet<string>();
        var queue = new Queue<string>();

        queue.Enqueue(person1);
        visited.Add(person1);

        while (queue.Count > 0)
        {
            string current = queue.Dequeue();

            foreach (var friend in _people[current])
            {
                if (friend == person2)
                {
                    return true;
                }

                if (!visited.Contains(friend))
                {
                    visited.Add(friend);
                    queue.Enqueue(friend);
                }
            }
        }

        return false;
    }

    public List<string> FindConnectionPath(string person1, string person2)
    {
        if (!_people.ContainsKey(person1) || !_people.ContainsKey(person2))
        {
            return new List<string>();
        }

        if (person1 == person2)
        {
            return new List<string> { person1 };
        }

        // key: current vertex, value: predecessor vertex
        var predecessorMap = new Dictionary<string, string>();
        var queue = new Queue<string>();

        queue.Enqueue(person1);
        predecessorMap[person1] = string.Empty; // No predecessor for the first vertex

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var friend in _people[current])
            {
                if (!predecessorMap.ContainsKey(friend))
                {
                    predecessorMap[friend] = current;
                    queue.Enqueue(friend);

                    if (friend == person2)
                    {
                        return ReconstructPath(predecessorMap, person2);
                    }
                }
            }
        }

        return new List<string>(); // No path found
    }

    private static List<string> ReconstructPath(
        Dictionary<string, string> parentMap,
        string endPerson
    )
    {
        var path = new List<string>();
        var current = endPerson;

        while (!string.IsNullOrEmpty(current))
        {
            path.Add(current);
            current = parentMap[current];
        }

        path.Reverse();
        return path;
    }
}
