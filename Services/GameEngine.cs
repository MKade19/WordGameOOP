using WordGameOOP.Contracts;
using WordGameOOP.Models;

namespace WordGameOOP.Services;
    
class GameEngine 
{
    private static GameEngine? _gameEngineInstance;
    private Game? _currentGame;
    private const int MIN_LENGTH_OF_START_WORD = 8;
    private const int MIN_LENGTH_OF_PLAYER_WORD = 0;
    private const int MAX_LENGTH_OF_WORD = 30;
    private const string ENDGAME_CAPTION = "Game has ended!\nDo you want to restart the game?\n Choose the option (restart, exit): ";

    //Dependencies
    private IInput _input;
    private IOutput _output;
    private ISingleEntityService<Game> _gameService;
    private IEntityCollectionService<Player> _playerService;
    private IGameCommandService _commandService;

    private GameEngine(
        IInput input, 
        IOutput output, 
        ISingleEntityService<Game> gameService, 
        IEntityCollectionService<Player> playerService, 
        IGameCommandService commandService
    ) 
    {
        _input = input;
        _output = output;
        _gameService = gameService;
        _playerService = playerService;
        _commandService = commandService;
    }

    public static GameEngine GetInstance
    (
        IInput input, 
        IOutput output, 
        ISingleEntityService<Game> gameService, 
        IEntityCollectionService<Player> playerService, 
        IGameCommandService commandService
    ) 
    {
        if (_gameEngineInstance == null) 
        {
            _gameEngineInstance = new GameEngine(input, output, gameService, playerService, commandService);
        }

        return _gameEngineInstance;
    }

    /// <summary>
    /// Launches the game
    /// </summary>
    public async Task LaunchGame() 
    {
        Refresh();
        string? firstPlayerName = await _input.WordInput("First player: ");
        string? secondPlayerName = await _input.WordInput("Second player: ");
        string? startWord = await _input.WordInput("Start word: ", MIN_LENGTH_OF_START_WORD, MAX_LENGTH_OF_WORD);
        int timeForRound = await GetRoundTime();

        _currentGame = new Game(firstPlayerName, secondPlayerName, startWord, timeForRound);

        await _playerService.AddOneIfNotExistentAsync(_currentGame.FirstPlayer);
        await _playerService.AddOneIfNotExistentAsync(_currentGame.SecondPlayer);
        await _gameService.SaveAsync(_currentGame);

        await GameRound();
        await GameEnded();
    }

    /// <summary>
    /// Manages a round of the game and should it be restarted or not.
    /// </summary>
    /// <param name="startWord">Main word of the game.</param>
    /// <param name="timeForRound">Time for player to input his word.</param>
    /// <param name="roundNumber">Number of round.</param>
    private async Task GameRound(int roundNumber = 1) 
    {
        _output.RoundInfo(roundNumber);

        _currentGame.FirstPlayer.Word = await _input.TimeoutInput(_currentGame.TimeForRound, "First player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);
        await VerifyPlayerWord(_currentGame.FirstPlayer);

        _currentGame.SecondPlayer.Word = await _input.TimeoutInput(_currentGame.TimeForRound, "Second player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);
        await VerifyPlayerWord(_currentGame.SecondPlayer);
        
        if (!await CompairingWords(_currentGame.StartWord, _currentGame.FirstPlayer.Word, _currentGame.SecondPlayer.Word))
        {
            await ChangeScoreAsync();
            return;
        }

        await GameRound(roundNumber + 1);
    }

    /// <summary>
    /// Manages should the game be restarted or finished.
    /// </summary>
    private async Task GameEnded() 
    {
        string? endPar = await _input.WordInput(ENDGAME_CAPTION);

        switch (endPar) 
        {
            case "restart": 
                await LaunchGame();
                break;
            case "exit":
                return;
            default: 
                Console.WriteLine("Invalid input!");
                await GameEnded();
                break;
        }
    }

    public async Task VerifyPlayerWord(Player player) 
    {
        if (WasWordSuggested(player.Word))
        {
            player.Word = "";
            Console.WriteLine("This word has suggested!");
            return;
        }

        await AddWordAsync(player.Word);
    }

    /// <summary>
    /// Returns true if <paramref name="firstPlayerWord"/> and <paramref name="secondPlayerWord"/> both match <paramref name="startWord"/>
    /// <param name="firstPlayerWord">Word that the first player has entered.</param>
    /// <param name="secondPlayerWord">Word that the second player has entered.</param>
    /// <param name="startWord">Main word of the game.</param>
    /// </summary>
    public async Task<bool> CompairingWords(string? startWord, string? firstPlayerWord, string? secondPlayerWord) 
    {
        bool firstPlayerResult = DoesWordMatch(firstPlayerWord, startWord);
        bool secondPlayerResult = DoesWordMatch(secondPlayerWord, startWord);

        if (firstPlayerResult && !secondPlayerResult) 
        {
            Console.WriteLine("First player has won!");
            await SetResultAsync("First");
            return false;
        }

        if (!firstPlayerResult && secondPlayerResult) 
        {
            Console.WriteLine("Second player has won!");
            await SetResultAsync("Second");
            return false;
        }

        Console.WriteLine("Draw! Enter next words.");
        return true;
    }

    /// <summary>
    /// Returns true if <paramref name="playerWord"/> matches <paramref name="startWord"/>
    /// </summary>
    /// <param name="playerWord">Word that the player has entered.</param>
    /// <param name="startWord">Main word of the game.</param>
    private bool DoesWordMatch(string? playerWord, string? startWord) 
    {
        int oldWordLength = playerWord.Length;

        for (int i = 0; i < playerWord.Length; i++) 
        {
            int index = startWord.IndexOf(playerWord[i]);

            if (index == -1) 
            {
                return false;
            }

            startWord = startWord.Remove(index, 1);
            playerWord = playerWord.Remove(i, 1);
            i--;
        }

        return oldWordLength != 0;
    }

    /// <summary>
    /// Provies input for round time and converts it to milleseconds
    /// </summary>
    /// <returns>Round time in milleseconds</returns>
    public async Task<int> GetRoundTime() 
    {
        return await _input.NumberInput("Enter time for round(sec): ") * 1000;
    }

    /// <summary>
    /// Verifies was the <paramref name="word"/> already suggested
    /// </summary>
    /// <param name="word">Word to verify</param>
    /// <returns>True if the <paramref name="word"/> was suggested and false if it wasn't</returns>
    private bool WasWordSuggested(string word) 
    {   
        return _currentGame.Words.Contains(word);
    }

    /// <summary>
    /// Adds <paramref name="word"/> to the current game and to the file of the game
    /// </summary>
    /// <param name="word">Word that should be added</param>
    /// <returns></returns>
    private async Task AddWordAsync(string word) 
    {
        _currentGame?.Words?.Add(word);
        Game gameFromFile = await _gameService.RestoreAsync();
        gameFromFile.Words = _currentGame?.Words;
        await _gameService.SaveAsync(gameFromFile);
    }

    /// <summary>
    /// Sets the <paramref name="result"/> of the game
    /// </summary>
    /// <param name="result">Value of result to set</param>
    /// <returns></returns>
    private async Task SetResultAsync(string result)
    {
        Game gameFromFile = await _gameService.RestoreAsync();
        gameFromFile.Result = result;
        await _gameService.SaveAsync(gameFromFile);
    }

    /// <summary>
    /// Changes the score after setting the result of the game
    /// </summary>
    /// <returns></returns>
    private async Task ChangeScoreAsync()
    {
        Game gameFromFile = await _gameService.RestoreAsync();

        if (gameFromFile.Result == "First")
        {
            gameFromFile.FirstPlayer.Score++;
            await _playerService.SaveOneAsync(gameFromFile.FirstPlayer);
        }
        else
        {
            gameFromFile.SecondPlayer.Score++;
        }

        await _gameService.SaveAsync(gameFromFile);
    }


    /// <summary>
    /// Deletes file with data about current game
    /// </summary>
    private void Refresh() 
    {
        if (File.Exists("resourses/currentGame.json")) 
        {
            File.Delete("resourses/currentGame.json");
        } 
    }
}

