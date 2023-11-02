namespace WordGameOOP;

class Game {
    private static Game _gameInstance;
    private const int MIN_LENGTH_OF_START_WORD = 2;
    private const int MIN_LENGTH_OF_PLAYER_WORD = 0;
    private const int MAX_LENGTH_OF_WORD = 30;
    private string _startWord;
    private IOutputInput _outputInput;

    private Game (IOutputInput oi) {
        _outputInput = oi;
    }

    public static Game GetInstance() {
        if (_gameInstance == null) {
            _gameInstance = new Game(new InputOutput());
        }

        return _gameInstance;
    }

    /// <summary>
    /// Launches the game
    /// </summary>
    public void LaunchGame() {
        _startWord = _outputInput.WordInput("Start word: ", MIN_LENGTH_OF_START_WORD, MAX_LENGTH_OF_WORD);
        int timeForRound = _outputInput.NumberInput("Enter time for round(sec): ") * 1000;

        //GameRound(startWord, timeForRound);
        Console.WriteLine("Game has ended!\nDo you want to restart the game?");
        Console.ReadLine();
        //GameEnded();
    }

    /// <summary>
    /// Manages a round of the game and should it be restarted or not.
    /// </summary>
    /// <param name="startWord">Main word of the game.</param>
    /// <param name="timeForRound">Time for player to input his word.</param>
    /// <param name="roundNumber">Number of round.</param>
    public void GameRound(string? startWord, int timeForRound, int roundNumber = 1) {
        string? firstPlayerWord, secondPlayerWord;

        //_outputInput.RoundInfo(roundNumber);

        firstPlayerWord = _outputInput.TimeoutInput(timeForRound, "First player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);
        secondPlayerWord = _outputInput.TimeoutInput(timeForRound, "Second player's word: ", MIN_LENGTH_OF_PLAYER_WORD, MAX_LENGTH_OF_WORD);

        if (firstPlayerWord.Equals(secondPlayerWord, StringComparison.OrdinalIgnoreCase)) {
            Console.WriteLine("Words must be different!");
            GameRound(startWord, timeForRound, roundNumber);
            return;
        }
        
        if (!GameHelper.CompairingWords(startWord, firstPlayerWord, secondPlayerWord))
            return;

        GameRound(startWord, timeForRound, roundNumber + 1);
    }

    /// <summary>
    /// Manages should the game be restarted or finished.
    /// </summary>
    public void GameEnded() {
        string? endPar = _outputInput.WordInput("Choose the option (restart, exit): ");

        switch (endPar) {
            case "restart": LaunchGame();
            break;
            case "exit": return;
            default: 
                Console.WriteLine("Invalid input!");
                GameEnded();
            break;
        }
    }
}