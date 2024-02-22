using CardGameFramework.Infra.Models;

namespace CardGameFramework.Big2.Models;

public class BigTwoHumanPlayer : HumanCardPlayer<PokerCard>, IBigTwoPlayer
{
    public HandCard HandCard { get; }
    public BigTwoGame? Game { get; set; }

    public BigTwoHumanPlayer(HandCard handCard) : base(handCard)
    {
        HandCard = handCard;
    }
    
    public IList<int> ParseInput()
    {
        while (true)
        {
            var input = Console.ReadLine();

            try
            {
                return ParseInput(input).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private IEnumerable<int> ParseInput(string? input)
    {
        if (input == null)
        {
            throw new ArgumentNullException(nameof(input));
        }
        // check exception

        var indicesBySplit = input.Split(' ');
        var indices = new List<int>();

        if (indicesBySplit is ["-1"])
        {
            indices.Add(-1);
            return indices;
        }

        foreach (var indexBySplit in indicesBySplit)
        {
            if (!int.TryParse(indexBySplit, out var index))
            {
                throw new ArgumentException("請輸入數字");
            }

            if (index < 0 || index >= HandCard.Cards.Count)
            {
                throw new ArgumentException("請輸入有效的索引值");
            }

            indices.Add(index);
        }

        return indices;
    }
}