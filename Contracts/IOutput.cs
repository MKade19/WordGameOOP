namespace WordGameOOP.Contracts;

interface IOutput {
    public void RoundInfo(int roundNumber);

    public Task ShowWords();

    public Task ShowCurrentScore();

    public Task ShowTotalScore();

    public Task ResolveCommand(string? command);
}