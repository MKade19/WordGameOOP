using System.Text.Json.Serialization;

namespace WordGameOOP.Models;

class Player 
{
    [JsonPropertyName("playerName")]
    public string Name { set; get; }

    [JsonPropertyName("score")]
    public int Score { set; get; }

    [JsonIgnore]
    public string Word { set; get; }

    [JsonConstructor]
    public Player(string name) 
    {
        Name = name;
        Score = 0;
        Word = String.Empty;
    }

    private Player()
    {
        Name = String.Empty;
        Score = -1;
        Word = String.Empty;
    }

    /// <summary>
    /// Returns empty player
    /// </summary>
    /// <returns></returns>
    public static Player Empty()
    {
        return new Player();
    }
}