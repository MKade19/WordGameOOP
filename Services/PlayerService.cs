using WordGameOOP.Models;
using WordGameOOP.Contracts;
using WordGameOOP.Constants;
using System.Text.Json;

namespace WordGameOOP.Services;

class PlayerCollectionService : IEntityCollectionService<Player>
{
    private IStorage _storage;
    private IOutput _output;

    public PlayerCollectionService(IOutput output, IStorage storage) 
    {
        _output= output;
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
            _output.ShowMessage(e.Message);
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
            string content = await _storage.RestoreAsync(FileConstants.PATH_TO_PLAYERS);  
            return JsonSerializer.Deserialize<IEnumerable<Player>>(content) ?? Enumerable.Empty<Player>();
        }
        catch(Exception e)
        {
            _output.ShowMessage(e.Message);
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
            await _storage.SaveAsync(FileConstants.PATH_TO_PLAYERS, content);
        }
        catch (Exception e)
        {
            _output.ShowMessage(e.Message);
        }
    }
}