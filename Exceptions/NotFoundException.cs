namespace WordGameOOP.Exceptions;

/// <summary>
/// Throws when element is not found.
/// </summary>
class NotFoundException : Exception 
{
    public NotFoundException(string? message) : base(message) {}
}