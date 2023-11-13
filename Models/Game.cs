using System.Text.Json.Serialization;

namespace WordGameOOP.Models;

class Game {
    [JsonPropertyName("words")]
    public List<string> Words { set; get; }

    [JsonPropertyName("result")]
    public string? Result { set; get; }

    [JsonPropertyName("firstPlayer")]
    public Player FirstPlayer { get; }

    [JsonPropertyName("secondPlayer")]
    public Player SecondPlayer { get; }

    [JsonIgnore]
    public string StartWord { get; }

    [JsonIgnore]
    public int TimeForRound { get; }

    [JsonConstructor]
    public Game(List<string> words, string? result, Player firstPlayer, Player secondPlayer) 
    {
        Words = words;
        Result = result;
        FirstPlayer = firstPlayer;
        SecondPlayer = secondPlayer;
        StartWord = String.Empty;
    }

    public Game(string firstPlayerName, string secondPlayerName, string startWord, int timeForRound) 
    {
        Words = new List<string>();
        FirstPlayer = new Player(firstPlayerName);
        SecondPlayer = new Player(secondPlayerName);
        TimeForRound = timeForRound;
        StartWord = startWord;
    }

    private Game()
    {
        Words = new List<string>();
        FirstPlayer = Player.Empty();
        SecondPlayer = Player.Empty();
        TimeForRound = -1;
        StartWord = String.Empty;
    }

    /// <summary>
    /// Returns empty game
    /// </summary>
    /// <returns></returns>
    public static Game Empty()
    {
        return new Game();
    }
}