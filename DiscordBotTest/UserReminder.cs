using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace DiscordBotTest
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
