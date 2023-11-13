namespace WordGameOOP.Contracts;

interface IInput 
{
    Task<string> WordInputAsync(string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue);
    Task<string> TimeoutInputAsync(int interval, string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue);
    Task<int> NumberInputAsync(string caption = "Enter number: ");
}