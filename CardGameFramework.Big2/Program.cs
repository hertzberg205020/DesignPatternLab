using CardGameFramework.Big2.Mock;
using CardGameFramework.Big2.Models;
using CardGameFramework.Infra.Models;

var players = new List<CardPlayer<PokerCard>>
{
    new BigTwoHumanPlayer(new HandCard()),
    new BigTwoHumanPlayer(new HandCard()),
    new BigTwoHumanPlayer(new HandCard()),
    new BigTwoHumanPlayer(new HandCard()),
};

var deck = MockDeck.CreateDeck();
var game = new BigTwoGame(deck, players);
game.Run();
