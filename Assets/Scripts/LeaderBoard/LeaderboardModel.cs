[System.Serializable]
public class LeaderboardModel
{
    public string[] names;
    public int[] scores;

    public LeaderboardModel(LeaderBoardManager leaderBoardManager)
    {
        this.names = leaderBoardManager.names;
        this.scores = leaderBoardManager.scores;
    }
}