using System.Timers;
using WordGameOOP.Contracts;
using WordGameOOP.Exceptions;
using WordGameOOP.Helpers;
using WordGameOOP.Constants;

namespace WordGameOOP.Services;

class InputService : IInput 
{
    private static System.Timers.Timer? _timer = null;

    private IGameCommandService _commandService;
    private IOutput _output;

    public InputService(IOutput output, IGameCommandService commandService) 
    {
        _output = output;
        _commandService = commandService;
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
    public async Task<string> TimeoutInputAsync(int interval, string caption, int minLength, int maxLength)
    {
        _output.ShowMessage($"You have {interval / 1000} sec to input!");

        if(_timer is null)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += OnTimerElapsed;
        }

        _timer.Stop();
        _timer.Start();

        string? word = await WordInputAsync(caption, minLength, maxLength);
        
        if (_timer.Enabled)
        {
            _timer.Stop();
        }
        else
        {
            word = String.Empty;
        }
            
        return word;
    }

    /// <summary>
    /// Provides input of string.
    /// </summary>
    /// <param name="caption">Caption of input</param>
    /// <param name="minLength">Min length of input</param>
    /// <param name="maxLength">Max length of input</param>
    /// <returns>Inputed string.</returns>
    public async Task<string> WordInputAsync
    (
        string caption = CaptionConstants.WORD_INPUT_DEFAULT, 
        int minLength = LimitsConstants.MIN_DEFAULT_WORD_LENGTH, 
        int maxLength = LimitsConstants.MAX_DEFAULT_WORD_LENGTH
    ) 
    {
        string word;

        while (true)
        {
            try 
            {
                _output.ShowMessage(caption);
                word = Console.ReadLine() ?? String.Empty;

                if (IsCommand(word))
                {
                    await _commandService.ResolveCommandAsync(word);
                    continue;
                }

                ValidationHelper.ValidateWord(word, minLength, maxLength);
                break;
            } 
            catch (InvalidInputException e) 
            {
                _output.ShowMessage(e.Message);
            }
            catch (GamesessionNotCreatedException e)
            {
                _output.ShowMessage(e.Message);
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
    public async Task<int> NumberInputAsync(string caption = CaptionConstants.NUMBER_INPUT_DEFAULT) 
    {
        int number;

        while (true)
        {
            try 
            {
                _output.ShowMessage(caption);
                string? buffer = Console.ReadLine();

                if (IsCommand(buffer))
                {
                    await _commandService.ResolveCommandAsync(buffer);
                    continue;
                }

                number = Convert.ToInt32(buffer);
                break;
            } 
            catch (FormatException e) 
            {
                _output.ShowMessage(e.Message);
            }
            catch (GamesessionNotCreatedException e)
            {
                _output.ShowMessage(e.Message);
            }
        }

        return number;
    }


    /// <summary>
    /// Verify is the given string command or not
    /// </summary>
    /// <param name="buffer">String to verify</param>
    /// <returns>True if it is a command, false if it isn't</returns>
    private bool IsCommand(string? buffer) {
        return buffer?[0] == GameConstants.COMMAND_SYMBOL;
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        try {
            throw new Exceptions.TimeoutException(MessageConstants.TIME_RUN_OUT_MESSAGE);
        } catch (Exceptions.TimeoutException te) {
            _output.ShowMessage(te.Message);
            _timer?.Stop();
            _output.ShowMessage(MessageConstants.PAUSE_MESSAGE);
        }
    }
}