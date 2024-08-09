using System.Reflection;

namespace TreasureMap.Models.Treasures;

public class TreasureFactory
{
    private static readonly List<(
        Type Type,
        double Probability,
        double CumulativeProbability
    )> TreasureTypes;

    static TreasureFactory()
    {
        var types = Assembly
            .GetAssembly(typeof(Treasure))
            ?.GetTypes()
            .Where(t =>
                t is { IsClass: true, IsAbstract: false }
                && t.IsSubclassOf(typeof(Treasure))
                && typeof(ITreasure).IsAssignableFrom(t)
            )
            .Select(t =>
            {
                var probabilityProperty = t.GetProperty(
                    "Probability",
                    BindingFlags.Public | BindingFlags.Static
                );
                var probability = (double)(
                    probabilityProperty?.GetValue(null)
                    ?? throw new InvalidOperationException($"Probability not defined for {t.Name}")
                );
                return (t, probability, 0.0); // 初始化累積概率為0
            })
            .OrderBy(_ => Random.Shared.Next())
            .ToList();

        if (types == null || types.Count == 0)
            throw new InvalidOperationException("No treasure types found.");

        var totalProbability = types.Sum(t => t.probability);
        double cumulativeProbability = 0;

        TreasureTypes = new List<(Type Type, double Probability, double CumulativeProbability)>();

        foreach (var (type, probability, _) in types)
        {
            var normalizedProbability = probability / totalProbability;
            cumulativeProbability += normalizedProbability;
            TreasureTypes.Add((type, normalizedProbability, cumulativeProbability));
        }
    }

    public static List<Treasure> CreateTreasures(int count)
    {
        var treasures = new List<Treasure>(count);

        for (var i = 0; i < count; i++)
        {
            var randomValue = Random.Shared.NextDouble();
            var selectedType = TreasureTypes
                .First(t => randomValue <= t.CumulativeProbability)
                .Type;
            var treasure = (Treasure)Activator.CreateInstance(selectedType)!;
            treasures.Add(treasure);
        }

        return treasures;
    }
}
