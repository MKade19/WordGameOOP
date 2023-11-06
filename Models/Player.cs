using System.Reflection.Metadata;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordGameOOP;

class Player 
{
    [JsonPropertyName("playerName")]
    public string Name { set; get; }

    [JsonPropertyName("score")]
    public int Score { set; get; }

    [JsonIgnore]
    public string Word { set; get; }
    public Player (string name) 
    {
        string? word = "";
        int score = 0;

        Name = name;
        Score = score;
        Word = word;
    }

    public async Task VerifyPlayer() 
    {
        try
        {
            List<Player> players = await GetPlayers();

            if (players?.Where(p => p.Name == Name).ToList().Count == 0) 
            {
                throw new NotFoundException("Player has not registered!");       
            }
            
            Player? playerFromfile = players?
                .Where(p => p.Name == Name)
                .ToList()[0];

            Score = playerFromfile.Score;
        }
        catch (NotFoundException ne)
        {
            await AddPlayer();
        }
    }

    private async Task AddPlayer() 
    {
        List<Player> newPlayers = new List<Player>(await GetPlayers());
        newPlayers.Add(this);
        string? content = JsonSerializer.Serialize<List<Player>>(newPlayers);

        StreamWriter sw = new StreamWriter("resourses/players.json");
        await sw.WriteAsync(content);
        sw.Close();
    }

    public async Task<List<Player>> GetPlayers() 
    {
        StreamReader? sr = new StreamReader("resourses/players.json");
        string content = await sr.ReadToEndAsync();
        sr.Close();

        return JsonSerializer.Deserialize<List<Player>>(content);
    }
}