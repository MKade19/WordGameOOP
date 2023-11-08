using WordGameOOP.Contracts;
using WordGameOOP.Helpers;

namespace WordGameOOP.Models;
    
class GameEngine 
{
    private static GameEngine? _gameInstance;
    private const int MIN_LENGTH_OF_START_WORD = 8;
    private const int MIN_LENGTH_OF_PLAYER_WORD = 0;
    private const int MAX_LENGTH_OF_WORD = 30;

    //Dependencies
    private IInput _input;
    private IOutput _output;
    private GameHelper _gameHelper;
    private Game _game;

    private GameEngine() 
    {
        _input = new Input();
        _output = new Output();
        _gameHelper = new GameHelper();
    }

    public static GameEngine GetInstance() 
    {
        if (_gameInstance == null) 
        {
            _gameInstance = new GameEngine();
        }

        return _gameInstance;
    }

    /// <summary>
    /// Launches the game
    /// </summary>
    public async Task LaunchGame() 
    {
        _game.Refresh();
        string? firstPlayerName = await _input.WordInput("First player: ");
        string? secondPlayerName = await _input.WordInput("Second player: ");

        string? startWord = await _input.WordInput("Start word: ", MIN_LENGTH_OF_START_WORD, MAX_LENGTH_OF_WORD);
        int timeForRound = await _gameHelper.GetRoundTime();

        _game = new Game(firstPlayerName, secondPlayerName, startWord, timeForRound);
        await _game.FirstPlayer.VerifyPlayer();
        await _game.SecondPlayer.VerifyPlayer();
        _game.Save();

        await GameRound();
        Console.WriteLine("Game has ended!\nDo you want to restart the game?");
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

        _game.FirstPlayer.Word = await _input.TimeoutInput(_game.TimeForRound, "First player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);
        await VerifyPlayerWord(_game.FirstPlayer.Word);

        _game.SecondPlayer.Word = await _input.TimeoutInput(_game.TimeForRound, "Second player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);
        await VerifyPlayerWord(_game.SecondPlayer.Word);
        
        if (!await _gameHelper.CompairingWords(_game.StartWord, _game.FirstPlayer.Word, _game.SecondPlayer.Word))
        {
            return;
        }

        await GameRound(roundNumber + 1);
    }

    /// <summary>
    /// Manages should the game be restarted or finished.
    /// </summary>
    private async Task GameEnded() 
    {
        string? endPar = await _input.WordInput("Choose the option (restart, exit): ");

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

    public async Task VerifyPlayerWord(string word) 
    {
        if (_game.IsWordSuggested(word))
        {
            Console.WriteLine("This word has suggested!");
            return;
        }

        await _game.AddWord(word);
    }
}

