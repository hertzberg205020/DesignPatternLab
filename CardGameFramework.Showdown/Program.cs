using CardGameFramework.Infra.Models;
using CardGameFramework.Showdown.Models;

Console.OutputEncoding = System.Text.Encoding.Unicode;

var players = new List<CardPlayer<PokerCard>>()
{
    new ShowDownGameAiPlayer(),
    new ShowDownGameAiPlayer(),
    new ShowDownGameAiPlayer(),
    new ShowDownGameAiPlayer(),
};

var deck = new Deck<PokerCard>();

var game = new ShowDownGame(deck, players);
game.Run();