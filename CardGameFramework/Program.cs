// See https://aka.ms/new-console-template for more information

using CardGameFramework.Models.Commons;
using CardGameFramework.Models.ShowDownGame;
using CardGameFramework.Models.UnoGame;

// var players = new List<CardPlayer<UnoCard>>()
// {
//     new UnoGameAiPlayer(),
//     new UnoGameAiPlayer(),
//     new UnoGameAiPlayer(),
//     new UnoGameAiPlayer(),
// };
//
// var deck = new Deck<UnoCard>();
// var cardTable = new CardTable();
//
// var game = new UnoCardGame(deck, players, cardTable);
// game.Run();

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
