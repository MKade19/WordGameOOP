using WordGameOOP.Models;

namespace WordGameOOP.Contracts;

interface IOutput 
{
    void RoundInfo(int roundNumber);

    void ShowWords(Game? game);

    void ShowCurrentScore(Game? game);

    void ShowTotalScore(IEnumerable<Player> players);
    
    void ShowMessage(string message);
}