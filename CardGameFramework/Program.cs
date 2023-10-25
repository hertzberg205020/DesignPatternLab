// See https://aka.ms/new-console-template for more information

using CardGameFramework.Models.Commons;
using CardGameFramework.Models.UnoGame;

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
