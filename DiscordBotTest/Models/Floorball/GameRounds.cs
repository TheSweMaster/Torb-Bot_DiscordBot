using System.Collections.Generic;

namespace DiscordBotTest.Models.Floorball
{
    public class GameRounds
    {
        public GameRounds(string round, string teamHome, string teamAway, string result, string referees, string linkToLineup)
        {
            Round = int.Parse(round);
            TeamHome = teamHome;
            TeamAway = teamAway;
            Result = result;
            Referees = referees;
            LinkToLineup = linkToLineup;
        }

        public int Round { get; set; }
        public string TeamHome { get; set; }
        public string TeamAway { get; set; }
        public string Result { get; set; }
        public string Referees { get; }
        public string LinkToLineup { get; set; }
    }
}
