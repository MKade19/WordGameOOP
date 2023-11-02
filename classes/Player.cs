namespace WordGameOOP;

class Player {
    public string Name { set; get; }
    public int Score { set; get; }
    public Player (string name, int score = 0) {
        Name = name;
        Score = score;
    }
}