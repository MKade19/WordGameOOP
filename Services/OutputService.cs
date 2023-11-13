using WordGameOOP.Exceptions;
using WordGameOOP.Contracts;
using WordGameOOP.Models;
using WordGameOOP.Constants;

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

    /// <summary>
    /// Shows words of the given <paramref name="game"/>
    /// </summary>
    /// <param name="game"></param>
    /// <exception cref="GamesessionNotCreatedException"></exception>
    public void ShowWords(Game? game)
    {
        if (game is null) 
        {
            throw new GamesessionNotCreatedException(MessageConstants.GAME_SESSION_NOT_CREATED_MESSAGE);
        }

        List<string>? words = game.Words;
        ShowList<string>(words);
    }

    /// <summary>
    /// Shows total score of the players in the given <paramref name="game"/>
    /// </summary>
    /// <param name="game"></param>
    /// <exception cref="GamesessionNotCreatedException"></exception>
    public void ShowCurrentScore(Game? game) 
    {
        if (game is null) 
        {
            throw new GamesessionNotCreatedException(MessageConstants.GAME_SESSION_NOT_CREATED_MESSAGE);
        }

        int score = game.FirstPlayer.Score + game.SecondPlayer.Score;
        Console.WriteLine("Current total score is " + score);
    }


    /// <summary>
    /// Shows total score of all given <paramref name="players"/>
    /// </summary>
    /// <param name="players">Collection of players</param>
    /// <exception cref="GamesessionNotCreatedException"></exception>
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

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }
}