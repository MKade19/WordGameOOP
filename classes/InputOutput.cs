using System.Timers;
using System.Runtime.InteropServices;

namespace WordGameOOP;

class InputOutput : IOutputInput {
    private static System.Timers.Timer _timer;

    /// <summary>
    /// Provides time input of string that meets all filters
    /// </summary>
    /// <param name="interval">Time for input</param>
    /// <param name="caption">Caption of input</param>
    /// <param name="minLength">Min length of input</param>
    /// <param name="maxLength">Max length of input</param>
    /// <returns>
    /// Inputed string if timer has not elapsed or 
    /// empty string if it has.
    /// </returns>
    public string TimeoutInput(int interval, string caption, int minLength = -1, int maxLength = Int32.MaxValue){
        Console.WriteLine($"You have {interval / 1000} sec to input!");

        if(_timer is null){
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += OnTimerElapsed;
        }

        _timer.Stop();
        _timer.Start();

        string? word = WordInput(caption, minLength, maxLength);
        
        if (_timer.Enabled)
            _timer.Stop();
        else
            word = "";

        return word;
    }

    /// <summary>
    /// Provides input of string.
    /// </summary>
    /// <param name="caption">Caption of input</param>
    /// <param name="minLength">Min length of input</param>
    /// <param name="maxLength">Max length of input</param>
    /// <returns>Inputed string.</returns>
    public string WordInput(string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue) {
        string? word;

        while (true)
            try {
                Console.Write(caption);
                word = Console.ReadLine();
                Validation.ValidateWord(word, minLength, maxLength);
                break;
            } catch (InvalidInputException e) {
                Console.WriteLine(e.Message);
            }

        return word;
    }

    /// <summary>
    /// Provides input of number
    /// </summary>
    /// <exception cref="FormatException">
    /// Throws if you didn't enter a number.
    /// </exception>
    /// <param name="caption">Caption of input</param>
    /// <returns>Inputed number.</returns>
    public int NumberInput(string caption = "Enter number: ") {
        int number;

        while (true)
            try {
                Console.Write(caption);
                number = Convert.ToInt32(Console.ReadLine());
                break;
            } catch (FormatException e) {
                Console.WriteLine(e.Message);
            }

        return number;
    }

    /// <summary>
    /// Shows all standard info about the round.
    /// </summary>
    /// <param name="roundNumber">Number of round</param>
    public void RoundInfo(int roundNumber) {
        Console.WriteLine($"To start round {roundNumber} press enter...");
        Console.ReadLine();
        Console.WriteLine("Round " + roundNumber);
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try {
            throw new TimeoutException("\nTime has run out!");
        } catch (TimeoutException te) {
            Console.WriteLine(te.Message);
            _timer.Stop();
            Console.WriteLine("Press enter to continue...");
        }
    }
}