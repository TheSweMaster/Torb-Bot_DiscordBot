using DiscordBotTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace DiscordBotTest.Converters
{
    public class RSAccountDataRepository
    {
        public static readonly string FilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\RsAccountData.json";
        public static readonly Encoding Encoding = Encoding.UTF8;


        public void Save(RSAccountData rSAccountData)
        {
            var json = RSAccountDataConverter.ToJson(rSAccountData);
            using (StreamWriter writer = new StreamWriter(FilePath, false, Encoding))
            {
                writer.Write($"{json}");
            }
        }

        public RSAccountData Get()
        {
            if (!File.Exists(FilePath))
            {
                return new RSAccountData(new ulong[] { }, new List<SkillData>());
            }

            var json = File.ReadAllText(FilePath, Encoding);
            return RSAccountDataConverter.ToRSAccountData(json);
        }
    }
}
