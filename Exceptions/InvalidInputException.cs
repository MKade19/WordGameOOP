namespace WordGameOOP.Exceptions;

/// <summary>
/// Throws when the input wasn't in correct format.
/// </summary>
class InvalidInputException : Exception 
{
    public InvalidInputException(string? message) : base(message) {}
}