using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotTest
{
    public static class RunescapeAccountList
    {
        private static readonly Dictionary<string, string> RunescapeAccounts;
        private const string TestUser = "TheSweMasterX#5203";
        static RunescapeAccountList()
        {
            RunescapeAccounts = new Dictionary<string, string>(){ { TestUser, "theswemaster" }, };
        }

        public static bool TryUpdateList(string userName, string rsUsername)
        {
            if (RunescapeAccounts.ContainsKey(userName))
            {
                RunescapeAccounts[userName] = rsUsername;
                return true;
            }

            return RunescapeAccounts.TryAdd(userName, rsUsername);
        }

        public static Dictionary<string, string> GetRunescapeAccountList()
        {
            return RunescapeAccounts;
        }

    }
}
