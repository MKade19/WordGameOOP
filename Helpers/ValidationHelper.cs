using WordGameOOP.Exceptions;

namespace WordGameOOP.Helpers;

static class ValidationHelper 
{
    /// <summary>
    /// Throws exception if the word wasn't in correct format.
    /// </summary>
    /// <param name="word"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <exception cref="InvalidInputException"></exception>
    public static void ValidateWord(string word, int min, int max) 
    {    
        if (word.Length > max || word.Length < min) 
        {
            throw new InvalidInputException($"Invalid input! (word should be from {min} to {max} characters long)");
        }
    }
}