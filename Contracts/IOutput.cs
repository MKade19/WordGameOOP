using WordGameOOP.Models;

namespace WordGameOOP.Contracts;

interface IOutput {
    public void RoundInfo(int roundNumber);

    public void ShowWords(Game? game);

    public void ShowCurrentScore(Game? game);

    public void ShowTotalScore(IEnumerable<Player> players);
}