using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBotTest.Helpers
{
    public  class RunescapeAccountWatchList
    {
        private static readonly Dictionary<string, int> RunescapeAccountWatchingList;
        static RunescapeAccountWatchList()
        {
            RunescapeAccountWatchingList = new Dictionary<string, int>() { { "theswemaster", -1 }, { "swemasterx", 100 }, { "skorpish", -1 }, { "mudkip btw", -1 } };
        }

        public static Dictionary<string, int> GetRunescapeAccountWatchList()
        {
            return RunescapeAccountWatchingList;
        }

        public static bool TryUpdateList(string rsUsername, int newTotalLevel)
        {
            if (RunescapeAccountWatchingList.ContainsKey(rsUsername))
            {
                RunescapeAccountWatchingList[rsUsername] = newTotalLevel;
                return true;
            }

            return RunescapeAccountWatchingList.TryAdd(rsUsername, newTotalLevel);
        }

        public static bool TryUpdateList(KeyValuePair<string, int> keyValuePair)
        {
            if (RunescapeAccountWatchingList.ContainsKey(keyValuePair.Key))
            {
                RunescapeAccountWatchingList[keyValuePair.Key] = keyValuePair.Value;
                return true;
            }

            return RunescapeAccountWatchingList.TryAdd(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
