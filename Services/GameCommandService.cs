using WordGameOOP.Contracts;
using WordGameOOP.Models;

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

    public async Task? ResolveCommandAsync(string? command) 
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

    private async Task? ShowWords()
    {
        _output.ShowWords(await _gameService.RestoreAsync());
    }


    private async Task? ShowCurrentScore()
    {
        _output.ShowCurrentScore(await _gameService.RestoreAsync());
    }

    private async Task? ShowTotalScore()
    {
        _output.ShowTotalScore(await _playerService.RestoreCollectionAsync());
    }
}