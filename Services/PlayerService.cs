using WordGameOOP.Models;
using WordGameOOP.Contracts;
using System.Text.Json;

namespace WordGameOOP.Services;

class PlayerCollectionService : IEntityCollectionService<Player>
{
    private IStorage _storage;
    private const string PLAYERS_PATH = "resourses/players.json";

    public PlayerCollectionService(IStorage storage) 
    {
        _storage = storage;
    }

    public async Task AddOneIfNotExistentAsync(Player player) 
    {
        try
        {
            IEnumerable<Player> players = await RestoreCollectionAsync();
            
            Player? playerFromfile = players
                .FirstOrDefault(p => p.Name == player.Name);

            if (playerFromfile is null) 
            {
                await AddOneAsync(player);
                return;
            }
            
            player.Score = playerFromfile.Score;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task AddOneAsync(Player player) 
    {
        List<Player> newPlayers = new List<Player>(await RestoreCollectionAsync());
        newPlayers.Add(player);
        await SaveCollectionAsync(newPlayers);
    }

    public async Task<IEnumerable<Player>> RestoreCollectionAsync() 
    {
        try 
        {
            string content = await _storage.RestoreAsync(PLAYERS_PATH);  
            return JsonSerializer.Deserialize<IEnumerable<Player>>(content) ?? Enumerable.Empty<Player>();
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Enumerable.Empty<Player>();
        }
    }

    public async Task SaveOneAsync(Player player) 
    {
        IEnumerable<Player> players = await RestoreCollectionAsync();

        players.Where(p => p.Name.Equals(player.Name, StringComparison.OrdinalIgnoreCase))
            .Select(p => { p = player; return p; });

        await SaveCollectionAsync(players);
    }

    public async Task SaveCollectionAsync(IEnumerable<Player> players)
    {
        try
        {
            string content = JsonSerializer.Serialize(players);
            await _storage.SaveAsync(PLAYERS_PATH, content);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}