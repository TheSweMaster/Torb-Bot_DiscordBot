using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Helpers
{
    public static class RSHighScoreHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public static Encoding Encoding => Encoding.UTF8;

        public static async Task<Stream> TryGetStreamData(string rsUsername)
        {
            Stream stream;
            try
            {
                stream = await _httpClient.GetStreamAsync($"http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player={rsUsername}");
            }
            catch (Exception)
            {
                throw;
            }

            return stream;
        }

        public static IEnumerable<string> ReadLines(Func<Stream> streamProvider)
        {
            using (var stream = streamProvider())
            using (var reader = new StreamReader(stream, Encoding))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static int GetSkillTotalLevel(List<string> lines)
        {
            var skillDataList = new List<SkillData>();
            for (int i = 0; i < skillNamesList.Length; i++)
            {
                var dataList = lines[i].Split(',');

                skillDataList.Add(
                new SkillData()
                {
                    Skill = skillNamesList[i],
                    Level = int.TryParse(dataList[1], out int level) ? level : -1,
                    Xp = int.TryParse(dataList[2], out int xp) ? xp : -1,
                });
            }
            SetHitpointsSkillData(skillDataList);
            return CalculateTotalLevel(skillDataList);
        }

        public static int CalculateTotalLevel(List<SkillData> skillDataList)
        {
            var overallSkillLevel = skillDataList.Find(x => x.Skill == "Overall").Level;
            if (overallSkillLevel == 0)
            {
                return skillDataList.Sum(x => x.Level);
            }
            return overallSkillLevel;
        }

        public static async Task<string> GetLazySkillTotalLevel(string rsUsername)
        {
            var stringData = await _httpClient.GetStringAsync($"http://services.runescape.com/m=hiscore_oldschool/index_lite.ws?player={rsUsername}");
            stringData = stringData.Substring(0, 20);
            var dataArray = stringData.Split(',');
            var currentTotalLevel = dataArray[1];

            return currentTotalLevel;
        }

        public static List<SkillData> GetSkillDataList(List<string> lines)
        {
            var skillDataList = new List<SkillData>();
            for (int i = 0; i < skillNamesList.Length; i++)
            {
                var dataList = lines[i].Split(',');

                skillDataList.Add(
                new SkillData()
                {
                    Skill = skillNamesList[i],
                    Rank = int.TryParse(dataList[0], out int rank) ? rank : -1,
                    Level = int.TryParse(dataList[1], out int level) ? level : -1,
                    Xp = int.TryParse(dataList[2], out int xp) ? xp : -1,
                    ImageLink = $"http://www.runescape.com/img/rsp777/hiscores/skill_icon_{skillNamesList[i]}1.gif",
                    EmojiCode = $"<:skill_icon_{skillNamesList[i].ToLower()}1:{GetEmojiCode(i)}>",
                });
            }

            SetHitpointsSkillData(skillDataList);
            //SetCombatLevelSkillData(skillDataList);
            SetOverallSkillData(skillDataList);

            return skillDataList;
        }

        private static void SetCombatLevelSkillData(List<SkillData> skillDataList)
        {
            var combatSkill = new SkillData()
            {
                Skill = skillNamesList[24],
                Rank = -1,
                Level = CalculateCombatLevel(skillDataList),
                Xp = -1,
                ImageLink = "",
                EmojiCode = $"<:skill_icon_combat1:{GetEmojiCode(24)}>",
            };
            skillDataList.Add(combatSkill);
        }

        private static int CalculateCombatLevel(List<SkillData> skillDataList)
        {
            var attack = GetSkillDataByName(skillDataList, "Attack").Level;
            var defence = GetSkillDataByName(skillDataList, "Defence").Level;
            var strength = GetSkillDataByName(skillDataList, "Strength").Level;
            var hitpoints = GetSkillDataByName(skillDataList, "Hitpoints").Level;
            var prayer = GetSkillDataByName(skillDataList, "Prayer").Level;
            var ranged = GetSkillDataByName(skillDataList, "Ranged").Level;
            var magic = GetSkillDataByName(skillDataList, "Magic").Level;

            var basee = (defence + hitpoints + Math.Floor(prayer / 2.0)) * 0.25;
            var melee = (attack + strength) * 0.325;
            var range = Math.Floor(ranged * 1.5) * 0.325;
            var mage = Math.Floor(magic * 1.5) * 0.325;
            var max = new[] { melee, range, mage }.Max();

            var cobmatLevel = (int)Math.Round(basee + max, 2);
            return cobmatLevel;
        }

        private static void SetHitpointsSkillData(List<SkillData> skillDataList)
        {
            var hitpointSkill = GetSkillDataByName(skillDataList, "Hitpoints");

            if (hitpointSkill.Level < 10 || hitpointSkill.Xp < 1154)
            {
                hitpointSkill.Level = 10;
                hitpointSkill.Xp = 1154;
            }
        }

        private static void SetOverallSkillData(List<SkillData> skillDataList)
        {
            var overallSkill = GetSkillDataByName(skillDataList, "Overall");

            if (overallSkill.Level == 0 || overallSkill.Xp == 0)
            {
                overallSkill.Level = skillDataList.Sum(x => x.Level);
                overallSkill.Xp = skillDataList.Sum(x => x.Xp);
            }
        }

        private static SkillData GetSkillDataByName(List<SkillData> skillDataList, string skillName)
        {
            return skillDataList.Find(s => s.Skill == skillName);
        }

        private static string GetEmojiCode(int i)
        {
            return skillIconsList[i].Substring(34, 18);
        }

        private static readonly string[] skillIconsList =
        {
            "https://cdn.discordapp.com/emojis/457953594616840215.png?v=1", // Overall
            "https://cdn.discordapp.com/emojis/457953594793000992.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594931413023.png?v=1",
            "https://cdn.discordapp.com/emojis/457953595028144131.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594943995905.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594998652939.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594977550348.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594834944032.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594935869442.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594856177694.png?v=1",
            "https://cdn.discordapp.com/emojis/457953595028144129.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594981744642.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594700726275.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594654851074.png?v=1", // Crafting
            "https://cdn.discordapp.com/emojis/457953595028013078.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594960773120.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594948452362.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594935607298.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594990264371.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594927218691.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594956709910.png?v=1",
            "https://cdn.discordapp.com/emojis/457953595086864415.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594780680225.png?v=1",
            "https://cdn.discordapp.com/emojis/457953594940063744.png?v=1", // Construction
            //"https://cdn.discordapp.com/emojis/541624296389214220.png?v=1", // Combat Level
        };

        private static readonly string[] skillNamesList =
        {
            "Overall",
            "Attack",   "Defence",  "Strength",
            "Hitpoints",    "Ranged",   "Prayer",
            "Magic",    "Cooking",  "Woodcutting",
            "Fletching",    "Fishing",  "Firemaking",
            "Crafting", "Smithing", "Mining",
            "Herblore", "Agility",  "Thieving",
            "Slayer",   "Farming",  "Runecraft",
            "Hunter",   "Construction", //"Combat Level",
        };
    }
}
