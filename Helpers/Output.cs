using WordGameOOP.Exceptions;
using WordGameOOP.Contracts;
using WordGameOOP.Models;

namespace WordGameOOP.Helpers;

class Output : IOutput 
{
    private Game _game;

    public Output() 
    {
        _game = new Game();
    }

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

    public async Task ResolveCommand(string? command) 
    {
        switch (command)
        {
            case "/show-words":
                await ShowWords();
                break;
            case "/score":
                await ShowCurrentScore();
                break;
            case "/total-score":
                await ShowTotalScore();
                break;
            default:
                Console.WriteLine("Invalid command!");
                break;
        }
    }

    public async Task ShowWords() 
    {
        Game game = await _game.Restore();

        if (game is null) 
        {
            throw new GamesessionNotCreatedException("Game session has hot been created yet!");
        }

        List<string>? words = game.Words;
        ShowList<string>(words);
    }

    public async Task ShowCurrentScore() 
    {
        Game game = await _game.Restore();

        if (game is null) 
        {
            throw new GamesessionNotCreatedException("Game session has hot been created yet!");
        }

        int score = game.FirstPlayer.Score + game.SecondPlayer.Score;
        Console.WriteLine("Current total score is " + score);
    }

    public async Task ShowTotalScore() 
    {
        List<Player> players = await new Player("").GetPlayers();
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