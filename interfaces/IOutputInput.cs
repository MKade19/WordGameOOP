namespace WordGameOOP;

interface IOutputInput {
    string WordInput(string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue);
    string TimeoutInput(int interval, string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue);
    int NumberInput(string caption = "Enter number: ");
}