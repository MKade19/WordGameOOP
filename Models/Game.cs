using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace WordGameOOP.Models;

class Game {
    [JsonPropertyName("words")]
    public List<string> Words { set; get; }

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
    public Game() {}
    public Game(string firstPlayerName, string secondPlayerName, string startWord, int timeForRound) 
    {
        Words = new List<string>();
        FirstPlayer = new Player(firstPlayerName);
        SecondPlayer = new Player(secondPlayerName);
        TimeForRound = timeForRound;
        StartWord = startWord;
    }

    public bool IsWordSuggested(string word) 
    {   
        return Words.Contains(word);
    }

    public async Task AddWord(string? word) 
    {
        Words?.Add(word);
        Game gameFromFile = await Restore();
        gameFromFile.Words = Words;
        await gameFromFile.Save();
    }

    public async Task SetResult(string result)
    {
        Game gameFromFile = await Restore();
        gameFromFile.Result = result;
        await gameFromFile.Save();
    }

    public async Task<Game> Restore() 
    {
        StreamReader? sr = new StreamReader("resourses/currentGame.json");
        string content = await sr.ReadToEndAsync();
        sr.Close();

        return JsonSerializer.Deserialize<Game>(content);
    }

    public async Task Save() 
    {
        StreamWriter? sw = new StreamWriter("resourses/currentGame.json");
        await sw.WriteAsync(JsonSerializer.Serialize<Game>(this));
        sw.Close();
    }

    public void Refresh() 
    {
        if (File.Exists("resourses/currentGame.json")) 
        {
            File.Delete("resourses/currentGame.json");
        } 
    }
}