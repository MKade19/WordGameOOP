using WordGameOOP.Contracts;
using WordGameOOP.Models;
using WordGameOOP.Constants;

namespace WordGameOOP.Services;
class GameCommandService : IGameCommandService
{
    private IOutput _output;
    private ISingleEntityService<Game> _gameService;
    private IEntityCollectionService<Player> _playerService;

    public GameCommandService(IOutput output, ISingleEntityService<Game> gameService, IEntityCollectionService<Player> playerService) 
    {
        _output = output;
        _gameService = gameService;
        _playerService = playerService;
    }

    public async Task ResolveCommandAsync(string? command) 
    {
        switch (command)
        {
            case CommandConstants.WORDS_COMMAND:
                await ShowWords();
                break;
            case CommandConstants.CURRENT_SCORE_COMMAND:
                await ShowCurrentScore();
                break;
            case CommandConstants.TOTAL_SCORE_COMMAND:
                await ShowTotalScore();
                break;
            default:
                _output.ShowMessage(MessageConstants.INVALID_COMMAND_MESSAGE);
                break;
        }
    }

    private async Task ShowWords()
    {
        _output.ShowWords(await _gameService.RestoreAsync());
    }


    private async Task ShowCurrentScore()
    {
        _output.ShowCurrentScore(await _gameService.RestoreAsync());
    }

    private async Task ShowTotalScore()
    {
        _output.ShowTotalScore(await _playerService.RestoreCollectionAsync());
    }
}