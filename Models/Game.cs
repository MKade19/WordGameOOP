using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace WordGameOOP.Models;

class Game {
    [JsonPropertyName("words")]
    public List<string>? Words { set; get; }

    [JsonPropertyName("result")]
    public string? Result { set; get; }

    [JsonPropertyName("firstPlayer")]
    public Player? FirstPlayer { get; }

    [JsonPropertyName("secondPlayer")]
    public Player? SecondPlayer { get; }

    [JsonIgnore]
    public string? StartWord { get; }

    [JsonIgnore]
    public int TimeForRound { get; }

    [JsonConstructor]
    public Game(List<string>? words, string? result, Player? firstPlayer, Player? secondPlayer) 
    {
        Words = words;
        Result = result;
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
    }

    public Game(string firstPlayerName, string secondPlayerName, string startWord, int timeForRound) 
    {
        Words = new List<string>();
        FirstPlayer = new Player(firstPlayerName);
        SecondPlayer = new Player(secondPlayerName);
        TimeForRound = timeForRound;
        StartWord = startWord;
    }

    public Game() {}
}