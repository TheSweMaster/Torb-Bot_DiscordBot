using DiscordBotTest.Models;
using System.Collections.Generic;

namespace DiscordBotTest.Helpers
{
    public class RunescapeAccountWatchList
    {
        private static readonly Dictionary<string, RSAccountData> RunescapeAccountWatchingList;
        static RunescapeAccountWatchList()
        {
            const ulong MyServerId = 199189022894063627ul;
            const ulong ScrubsServerId = 202956682932912129ul;
            const ulong TestServerId = 430643719880835072ul;

            RunescapeAccountWatchingList = new Dictionary<string, RSAccountData>()
            {
                { "theswemaster", new RSAccountData(new []{ MyServerId, ScrubsServerId }, InitialSkillDataList()) },
                { "swemasterx", new RSAccountData(new []{ MyServerId }, InitialSkillDataList()) },
                { "lynx titan", new RSAccountData(new []{ TestServerId }, TestSkillDataList()) },
                { "skorpish", new RSAccountData(new []{ MyServerId, ScrubsServerId }, InitialSkillDataList()) },
                { "mudkip btw", new RSAccountData(new []{ MyServerId }, InitialSkillDataList()) },
                { "zami_ftw", new RSAccountData(new []{ MyServerId }, InitialSkillDataList()) },
                { "potatodolan", new RSAccountData(new []{ MyServerId, ScrubsServerId }, InitialSkillDataList()) },
                { "paroxysm", new RSAccountData(new []{ MyServerId }, InitialSkillDataList()) },
                { "sicsy", new RSAccountData(new []{ MyServerId }, InitialSkillDataList()) }
            };
        }

        public static Dictionary<string, RSAccountData> GetRunescapeAccountWatchList()
        {
            return RunescapeAccountWatchingList;
        }

        public static bool TryUpdateList(KeyValuePair<string, RSAccountData> keyValuePair)
        {
            if (RunescapeAccountWatchingList.ContainsKey(keyValuePair.Key))
            {
                RunescapeAccountWatchingList[keyValuePair.Key] = keyValuePair.Value;
                return true;
            }

            return RunescapeAccountWatchingList.TryAdd(keyValuePair.Key, keyValuePair.Value);
        }

        public static List<SkillData> InitialSkillDataList()
        {
            return new List<SkillData>()
            {
                new SkillData()
                {
                    Skill = "Overall",
                    Level = -1,
                    Xp = -1,
                    Rank = -1
                }
            };
        }

        private static List<SkillData> TestSkillDataList()
        {
            return new List<SkillData>()
            {
                new SkillData()
                {
                    Skill = "Overall",
                    Level = 100,
                    Xp = 10000,
                    Rank = 800000
                },
                new SkillData()
                {
                    Skill = "Woodcutting",
                    Level = 30,
                    Xp = 100000,
                    Rank = 50000
                },
                new SkillData()
                {
                    Skill = "Crafting",
                    Level = 1,
                    Xp = 300,
                    Rank = 5000000
                },
                new SkillData()
                {
                    Skill = "Cooking",
                    Level = -4,
                    Xp = 600,
                    Rank = 5000001
                },
                new SkillData()
                {
                    Skill = "Hitpoints",
                    Level = 10,
                    Xp = 696969,
                    Rank = 696969
                }
            };
        }
    }
}