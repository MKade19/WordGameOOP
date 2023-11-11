using WordGameOOP.Models;
using WordGameOOP.Services;

namespace WordGameOOP;
internal class Program
{
    private static async Task Main(string[] args)
    {
        FileStorage fileStorage = new FileStorage();
        OutputService outputService = new OutputService();

        PlayerCollectionService playerService = new PlayerCollectionService(fileStorage);
        GameService gameService = new GameService(fileStorage);

        GameCommandService commandService = new GameCommandService(outputService, gameService, playerService);
        InputService inputService = new InputService(commandService);

        GameEngine gameEngine = GameEngine.GetInstance
        (
            inputService,
            outputService,
            gameService,
            playerService,
            commandService
        );
        await gameEngine.LaunchGame();
    }
}