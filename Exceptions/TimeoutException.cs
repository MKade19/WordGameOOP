namespace WordGameOOP;

/// <summary>
/// Throws when the timer has elapsed.
/// </summary>
class TimeoutException : Exception {
    public TimeoutException(string? message) : base(message) {
    }
}