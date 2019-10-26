namespace DiscordBotTest.Models
{
    public class FloorBallTable
    {
        public FloorBallTable(string rank, string team, string played, string wins, string draws, string losses,
            string goalStats, string goalDifferens, string points)
        {
            Rank = rank;
            Team = team;
            Played = int.Parse(played);
            Wins = int.Parse(wins);
            Draws = draws;
            Losses = int.Parse(losses);
            GoalStats = goalStats;
            GoalDifferens = goalDifferens;
            Points = int.Parse(points);
        }

        public string Rank { get; set; }
        public string Team { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public string Draws { get; set; }
        public int Losses { get; set; }
        public string GoalStats { get; set; }
        public string GoalDifferens { get; set; }
        public int Points { get; set; }
    }
}
