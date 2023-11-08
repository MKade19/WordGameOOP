namespace WordGameOOP.Contracts;

interface IInput {
    Task<string> WordInput(string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue);
    Task<string> TimeoutInput(int interval, string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue);
    Task<int> NumberInput(string caption = "Enter number: ");
    
}