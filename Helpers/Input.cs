using System.Timers;
using WordGameOOP.Contracts;
using WordGameOOP.Exceptions;

namespace WordGameOOP.Helpers;

class Input : IInput {
    private static System.Timers.Timer _timer;

    private IOutput _output;

    public Input() {
        _output = new Output();
    }

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
    public async Task<string> TimeoutInput(int interval, string caption, int minLength = -1, int maxLength = Int32.MaxValue){
        Console.WriteLine($"You have {interval / 1000} sec to input!");

        if(_timer is null){
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += OnTimerElapsed;
        }

        _timer.Stop();
        _timer.Start();

        string? word = await WordInput(caption, minLength, maxLength);
        
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
    public async Task<string> WordInput(string caption = "Enter word: ", int minLength = -1, int maxLength = Int32.MaxValue) {
        string? word;

        while (true){
            try {
                Console.Write(caption);
                word = Console.ReadLine();

                if (IsCommand(word))
                {
                    await _output.ResolveCommand(word);
                    continue;
                }

                Validation.ValidateWord(word, minLength, maxLength);
                break;
            } 
            catch (InvalidInputException e) 
            {
                Console.WriteLine(e.Message);
            }
            catch (GamesessionNotCreatedException e)
            {
                Console.WriteLine(e.Message);
            }
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
    public async Task<int> NumberInput(string caption = "Enter number: ") {
        int number;

        while (true)
        {
            try {
                Console.Write(caption);
                string? buffer = Console.ReadLine();

                if (IsCommand(buffer))
                {
                    await _output.ResolveCommand(buffer);
                    continue;
                }

                number = Convert.ToInt32(buffer);
                break;
            } 
            catch (FormatException e) 
            {
                Console.WriteLine(e.Message);
            }
        }

        return number;
    }

    private bool IsCommand(string? buffer) {
        return buffer?[0] == '/';
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try {
            throw new Exceptions.TimeoutException("\nTime has run out!");
        } catch (Exceptions.TimeoutException te) {
            Console.WriteLine(te.Message);
            _timer.Stop();
            Console.WriteLine("Press enter to continue...");
        }
    }
}