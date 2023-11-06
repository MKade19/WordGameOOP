namespace WordGameOOP;
internal class Program
{
    private static async Task Main(string[] args)
    {
        GameEngine gameEnigine = GameEngine.GetInstance();
        await gameEnigine.LaunchGame();
    }
}