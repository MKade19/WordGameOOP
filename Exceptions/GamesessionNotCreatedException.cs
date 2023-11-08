namespace WordGameOOP.Exceptions;

/// <summary>
/// Throws when the gamesession has not been created.
/// </summary>
class GamesessionNotCreatedException : Exception
{
    public GamesessionNotCreatedException(string? message) : base(message) {}
}