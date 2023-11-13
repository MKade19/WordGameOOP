using WordGameOOP.Contracts;
using WordGameOOP.Models;
using WordGameOOP.Constants;

namespace WordGameOOP.Services;
    
class GameEngine 
{
    private static GameEngine? _gameEngineInstance;
    private Game _currentGame;

    //Dependencies
    private IInput _input;
    private IOutput _output;
    private ISingleEntityService<Game> _gameService;
    private IEntityCollectionService<Player> _playerService;
    private IGameCommandService _commandService;

    private GameEngine
    (
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
        _currentGame = Game.Empty();
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
        _gameService.Refresh();
        string? firstPlayerName = await _input.WordInputAsync(CaptionConstants.FIRST_PLAYER_NAME_INPUT);
        string? secondPlayerName = await _input.WordInputAsync(CaptionConstants.SECOND_PLAYER_NAME_INPUT);
        string? startWord = await _input.WordInputAsync(CaptionConstants.START_WORD_INPUT, LimitsConstants.MIN_LENGTH_OF_START_WORD, LimitsConstants.MAX_LENGTH_OF_WORD);
        int timeForRound = await GetRoundTimeAsync();

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

        _currentGame.FirstPlayer.Word = await _input.TimeoutInputAsync
        (
            _currentGame.TimeForRound, 
            CaptionConstants.FIRST_PLAYER_WORD_INPUT, 
            LimitsConstants.MIN_LENGTH_OF_PLAYER_WORD, 
            LimitsConstants.MAX_LENGTH_OF_WORD
        );

        await AddWordIfNotSuggestedAsync(_currentGame.FirstPlayer);

        _currentGame.SecondPlayer.Word = await _input.TimeoutInputAsync
        (
            _currentGame.TimeForRound, 
            CaptionConstants.SECOND_PLAYER_WORD_INPUT, 
            LimitsConstants.MIN_LENGTH_OF_PLAYER_WORD, 
            LimitsConstants.MAX_LENGTH_OF_WORD
        );

        await AddWordIfNotSuggestedAsync(_currentGame.SecondPlayer);
        
        if (!await CompairingWordsAsync(_currentGame.StartWord, _currentGame.FirstPlayer.Word, _currentGame.SecondPlayer.Word))
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
        string? endPar = await _input.WordInputAsync(CaptionConstants.ENDGAME_CAPTION);

        switch (endPar) 
        {
            case GameConstants.RESTART_OPTION: 
                await LaunchGame();
                break;
            case GameConstants.EXIT_OPTION:
                return;
            default: 
                _output.ShowMessage(MessageConstants.INVALID_INPUT_MESSAGE);
                await GameEnded();
                break;
        }
    }

    /// <summary>
    /// Adds the word if it hasn't suggested
    /// </summary>
    /// <param name="player">Player whose word needs to be verified</param>
    /// <returns></returns>
    public async Task AddWordIfNotSuggestedAsync(Player player) 
    {
        if (WasWordSuggested(player.Word))
        {
            player.Word = String.Empty;
            _output.ShowMessage(MessageConstants.WORD_SUGGESTED_MESSAGE);
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
    private async Task<bool> CompairingWordsAsync(string startWord, string firstPlayerWord, string secondPlayerWord) 
    {
        bool firstPlayerResult = DoesWordMatch(firstPlayerWord, startWord);
        bool secondPlayerResult = DoesWordMatch(secondPlayerWord, startWord);

        if (firstPlayerResult && !secondPlayerResult) 
        {
            _output.ShowMessage(MessageConstants.FIRST_PLAYER_WON_MESSAGE);
            await SetResultAsync(GameConstants.FIRST_PLAYER_WON_RESULT);
            return false;
        }

        if (!firstPlayerResult && secondPlayerResult) 
        {
            _output.ShowMessage(MessageConstants.SECOND_PLAYER_WON_MESSAGE);
            await SetResultAsync(GameConstants.SECOND_PLAYER_WON_RESULT);
            return false;
        }

        _output.ShowMessage(MessageConstants.DRAW_MESSAGE);
        return true;
    }

    /// <summary>
    /// Returns true if <paramref name="playerWord"/> matches <paramref name="startWord"/>
    /// </summary>
    /// <param name="playerWord">Word that the player has entered.</param>
    /// <param name="startWord">Main word of the game.</param>
    private bool DoesWordMatch(string playerWord, string startWord) 
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
    public async Task<int> GetRoundTimeAsync() 
    {
        return await _input.NumberInputAsync(CaptionConstants.ROUND_TIME_INPUT) * 1000;
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
        _currentGame.Words.Add(word);
        Game gameFromFile = await _gameService.RestoreAsync();
        gameFromFile.Words = _currentGame.Words;
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

        if (gameFromFile.Result == GameConstants.FIRST_PLAYER_WON_RESULT)
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
}

