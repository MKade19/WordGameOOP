namespace WordGameOOP.Contracts;

interface IGameCommandService
{
    Task? ResolveCommandAsync(string? command);
}