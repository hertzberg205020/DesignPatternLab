using CardGameFramework.Infra.Models;
using CardGameFramework.Uno.Models;

Console.OutputEncoding = System.Text.Encoding.Unicode;

var players = new List<CardPlayer<UnoCard>>()
{
    new UnoGameAiPlayer(),
    new UnoGameAiPlayer(),
    new UnoGameAiPlayer(),
    new UnoGameAiPlayer(),
};

var deck = new Deck<UnoCard>();
var cardTable = new CardTable();

var game = new UnoCardGame(deck, players, cardTable);
game.Run();
