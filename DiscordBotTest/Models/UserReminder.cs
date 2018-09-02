using System;
using System.Timers;

namespace DiscordBotTest.Models
{
    public class UserReminder
    {
        public ulong UserId { get; set; }
        public string Message { get; set; }
        public TimeSpan TimeSpan { get; set; }
        public Timer Timer { get; set; }
        public bool HasReminded { get; set; }
    }
}
