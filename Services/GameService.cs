using WordGameOOP.Models;
using WordGameOOP.Contracts;
using WordGameOOP.Constants;
using System.Text.Json;

namespace WordGameOOP.Services;

class GameService : ISingleEntityService<Game>
{
    private IOutput _output;
    private IStorage _storage;

    public GameService(IOutput output, IStorage storage) 
    {
        _output = output;
        _storage = storage;
    }

    /// <summary>
    /// Restores the game from file asynchronously
    /// </summary>
    /// <returns></returns>
    public async Task<Game> RestoreAsync()
    {
        try 
        {
            string content = await _storage.RestoreAsync(FileConstants.PATH_TO_GAME);  
            return JsonSerializer.Deserialize<Game>(content) ?? Game.Empty();
        }
        catch(Exception e)
        {
            _output.ShowMessage(e.Message);
            return Game.Empty();
        }
    }

    /// <summary>
    /// Saves the given <paramref name="game"/> asynchronously
    /// </summary>
    /// <param name="game">Game to save</param>
    /// <returns></returns>
    public async Task SaveAsync(Game game)
    {
        try
        {
            string content = JsonSerializer.Serialize(game);
            await _storage.SaveAsync(FileConstants.PATH_TO_GAME, content);
        }
        catch (Exception e)
        {
            _output.ShowMessage(e.Message);
        }
    }

    /// <summary>
    /// Deletes file with data about current game
    /// </summary>
    public void Refresh() 
    {
        _storage.Delete(FileConstants.PATH_TO_GAME);
    }
}