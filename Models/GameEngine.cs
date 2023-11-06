using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordGameOOP;
    
class GameEngine 
{
    private static GameEngine? _gameInstance;
    private const int MIN_LENGTH_OF_START_WORD = 8;
    private const int MIN_LENGTH_OF_PLAYER_WORD = 0;
    private const int MAX_LENGTH_OF_WORD = 30;
    private string? _startWord;
    private Player? FirstPlayer { set; get; }
    private Player? SecondPlayer { set; get; }

    //Dependencies
    private IInput _input;
    private IOutput _output;
    private GameHelper _gameHelper;

    private GameEngine() 
    {
        _input = new Input();
        _output = new Output();
        _gameHelper = new GameHelper();
    }

    public static GameEngine GetInstance() 
    {
        if (_gameInstance == null) {
            _gameInstance = new GameEngine();
        }

        return _gameInstance;
    }

    /// <summary>
    /// Launches the game
    /// </summary>
    public async Task LaunchGame() 
    {
        FirstPlayer = new Player(await _input.WordInput("First player: "));
        SecondPlayer = new Player(await _input.WordInput("Second player: "));

        await FirstPlayer.VerifyPlayer();
        await SecondPlayer.VerifyPlayer();

        _startWord = await _input.WordInput("Start word: ", MIN_LENGTH_OF_START_WORD, MAX_LENGTH_OF_WORD);
        int timeForRound = await _gameHelper.GetRoundTime();

        GameRound(_startWord, timeForRound);
        Console.WriteLine("Game has ended!\nDo you want to restart the game?");
        await GameEnded();
        Console.ReadLine();
    }

    /// <summary>
    /// Manages a round of the game and should it be restarted or not.
    /// </summary>
    /// <param name="startWord">Main word of the game.</param>
    /// <param name="timeForRound">Time for player to input his word.</param>
    /// <param name="roundNumber">Number of round.</param>
    private async void GameRound(string? startWord, int timeForRound, int roundNumber = 1) {
        _output.RoundInfo(roundNumber);

        FirstPlayer.Word = await _input.TimeoutInput(timeForRound, "First player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);
        SecondPlayer.Word = await _input.TimeoutInput(timeForRound, "Second player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);

        if (FirstPlayer.Word.Equals(SecondPlayer.Word, StringComparison.OrdinalIgnoreCase)) {
            Console.WriteLine("Words must be different!");
            GameRound(startWord, timeForRound, roundNumber);
            return;
        }
        
        if (!_gameHelper.CompairingWords(startWord, FirstPlayer.Word, SecondPlayer.Word))
            return;

        GameRound(startWord, timeForRound, roundNumber + 1);
    }

    /// <summary>
    /// Manages should the game be restarted or finished.
    /// </summary>
    private async Task GameEnded() 
    {
        string? endPar = await _input.WordInput("Choose the option (restart, exit): ");

        switch (endPar) {
            case "restart": await LaunchGame();
            break;
            case "exit": return;
            default: 
                Console.WriteLine("Invalid input!");
                await GameEnded();
            break;
        }
    }
}

