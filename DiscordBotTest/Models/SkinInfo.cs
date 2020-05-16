using DiscordBotTest.Helpers;

namespace DiscordBotTest.Models
{
    public class SkinInfo
    {
        public SkinInfo(string name, Rarity rarity, string imageUrl, string caseName)
        {
            Name = name;
            Rarity = rarity;
            ImageUrl = imageUrl;
            CaseName = caseName;
        }

        public string Name { get; }
        public Rarity Rarity { get; }
        public string ImageUrl { get; }
        public string CaseName { get; }
    }
}
