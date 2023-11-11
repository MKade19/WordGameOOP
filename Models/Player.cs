using WordGameOOP.Exceptions;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using WordGameOOP.Contracts;
using WordGameOOP.Services;

namespace WordGameOOP.Models;

// interface IStorage 
// {
//     Task SaveAsync();
// }

// class FileStorage : IStorage {

// }

// class GameService : IGameService {
//     Task CreateNewPlayerAsync();
//     Task<Player> FindPlayerAsync();
// }

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
}