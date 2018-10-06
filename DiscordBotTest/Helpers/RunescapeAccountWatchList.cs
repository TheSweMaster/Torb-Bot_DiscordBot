using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBotTest.Helpers
{
    public class RunescapeAccountWatchList
    {
        private static readonly Dictionary<string, List<SkillData>> RunescapeAccountWatchingList;
        static RunescapeAccountWatchList()
        {
            RunescapeAccountWatchingList = new Dictionary<string, List<SkillData>>()
            {
                { "theswemaster", InitialSkillDataList() },
                { "swemasterx", TestSkillDataList() },
                { "skorpish", InitialSkillDataList() },
                { "mudkip btw", InitialSkillDataList() }
            };
        }

        public static Dictionary<string, List<SkillData>> GetRunescapeAccountWatchList()
        {
            return RunescapeAccountWatchingList;
        }

        public static bool TryUpdateList(KeyValuePair<string, List<SkillData>> keyValuePair)
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
                    Level = 4,
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