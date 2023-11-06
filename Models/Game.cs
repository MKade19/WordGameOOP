using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordGameOOP;

class Game {
    [JsonPropertyName("words")]
    public string[]? Words { get; }

    [JsonPropertyName("result")]
    public string? Result { get; }

    [JsonPropertyName("score")]
    public int Score { get; }

    public async Task<Game> GetGame() {
        StreamReader? sr = new StreamReader("resourses/currentGame.json");
        string content = await sr.ReadToEndAsync();
        sr.Close();

        return JsonSerializer.Deserialize<Game>(content);
    }
}