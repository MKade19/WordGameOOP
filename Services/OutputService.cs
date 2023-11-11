using WordGameOOP.Exceptions;
using WordGameOOP.Contracts;
using WordGameOOP.Models;

namespace WordGameOOP.Services;

class OutputService : IOutput 
{
    /// <summary>
    /// Shows all standard info about the round.
    /// </summary>
    /// <param name="roundNumber">Number of round</param>
    public void RoundInfo(int roundNumber) 
    {
        Console.WriteLine($"To start round {roundNumber} press enter...");
        Console.ReadLine();
        Console.WriteLine("Round " + roundNumber);
    }

    public void ShowWords(Game? game)
    {
        if (game is null) 
        {
            throw new GamesessionNotCreatedException("Game session has hot been created yet!");
        }

        List<string>? words = game.Words;
        ShowList<string>(words);
    }

    public void ShowCurrentScore(Game? game) 
    {
        if (game is null) 
        {
            throw new GamesessionNotCreatedException("Game session has hot been created yet!");
        }

        int score = game.FirstPlayer.Score + game.SecondPlayer.Score;
        Console.WriteLine("Current total score is " + score);
    }

    public void ShowTotalScore(IEnumerable<Player> players)
    {
        int totalScore = 0;

        foreach(Player player in players) 
        {
            totalScore += player.Score;
        }

        Console.WriteLine("Total score is " + totalScore);
    }

    public void ShowList<T>(List<T>? array) 
    {
        for (int i = 1; i <= array?.Count; i++) 
        {
            Console.WriteLine($"{i}. {array[i - 1]}");
        }
    }
}