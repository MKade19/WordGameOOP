using WordGameOOP.Services;

namespace WordGameOOP;
internal class Program
{
    private static async Task Main(string[] args)
    {
        OutputService outputService = new OutputService();
        FileStorage fileStorage = new FileStorage(outputService);

        PlayerCollectionService playerService = new PlayerCollectionService(outputService, fileStorage);
        GameService gameService = new GameService(outputService, fileStorage);

        GameCommandService commandService = new GameCommandService(outputService, gameService, playerService);
        InputService inputService = new InputService(outputService, commandService);

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