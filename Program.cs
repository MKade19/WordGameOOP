namespace WordGameOOP;
internal class Program
{
    private static void Main(string[] args)
    {
        Game game = Game.GetInstance();
        game.LaunchGame();
    }
}