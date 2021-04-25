using System.Collections.Generic;

namespace DiscordBotTest.Models.Floorball
{
    public class Player
    {
        public Player(string playerPosition, string playerNumber, string playerName)
        {
            PlayerPosition = playerPosition;
            PlayerNumber = int.TryParse(playerNumber, out int number) ? number : 0;
            PlayerName = playerName;
        }

        public string PlayerPosition { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }
    }

    public class PlayerLineup
    {
        public PlayerLineup()
        {
            Players = new List<Player>();
        }

        public int Round { get; set; }
        public HomeOrAway HomeOrAway { get; set; }
        public string Team { get; set; }
        public List<Player> Players { get; set; }
    }

    public enum HomeOrAway
    {
        Hemma,
        Borta
    }

    public enum Position
    {
        Målvakt,
        Back,
        Center,
        Forward
    }
}

