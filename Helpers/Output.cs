namespace WordGameOOP;

class Output : IOutput 
{
    private Game _currentGame;

    public Output() 
    {
        _currentGame = new Game();
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
        string[]? words = (await _currentGame.GetGame()).Words;

        ShowArray<string>(words);
    }

    public async Task ShowCurrentScore() 
    {
        int score = (await _currentGame.GetGame()).Score;

        Console.WriteLine("Current score is " + score);
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

    public void ShowArray<T>(T[]? array) 
    {
        for (int i = 1; i <= array?.Length; i++) 
        {
            Console.WriteLine($"{i}. {array[i - 1]}");
        }
    }
}