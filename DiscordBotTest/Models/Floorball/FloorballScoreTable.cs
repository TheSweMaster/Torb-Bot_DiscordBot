namespace DiscordBotTest.Models.Floorball
{
    public class FloorballScoreTable
    {
        public FloorballScoreTable(int position, string team, int gamesPlayed, int goalDifferens, int points)
        {
            Position = position;
            Team = team;
            GamesPlayed = gamesPlayed;
            GoalDifferens = goalDifferens;
            Points = points;
        }

        public int Position { get; set; }
        public string Team { get; set; }
        public int GamesPlayed { get; set; }
        public int GoalDifferens { get; set; }
        public int Points { get; set; }
    }
}
