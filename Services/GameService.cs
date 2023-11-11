using WordGameOOP.Models;
using WordGameOOP.Contracts;
using System.Text.Json;

namespace WordGameOOP.Services;

class GameService : ISingleEntityService<Game>
{
    private IStorage _storage;

    private const string GAME_PATH = "resourses/currentGame.json";

    public GameService(IStorage storage) 
    {
        _storage = storage;
    }

    public async Task<Game?> RestoreAsync()
    {
        try 
        {
            string content = await _storage.RestoreAsync(GAME_PATH);  
            return JsonSerializer.Deserialize<Game>(content);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public async Task SaveAsync(Game game)
    {
        try
        {
            string content = JsonSerializer.Serialize(game);
            await _storage.SaveAsync(GAME_PATH, content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}