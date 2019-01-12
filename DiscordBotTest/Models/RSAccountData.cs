using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBotTest.Models
{
    public class RSAccountData
    {
        public ulong[] ServerIds { get; set; }
        public List<SkillData> SkillDataList { get; set; }

        public RSAccountData(ulong[] serverId, List<SkillData> skillDataList)
        {
            ServerIds = serverId;
            SkillDataList = skillDataList;
        }
    }
}
