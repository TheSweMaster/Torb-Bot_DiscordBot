using DiscordBotTest.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotTest.Helpers
{
    public class SkinInfoGetter
    {
        public async Task<string> GetAsync()
        {
            var mainUrl = "https://csgostash.com/containers/skin-cases";

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(mainUrl);

            var xpathCases = "//*[@class='col-lg-4 col-md-6 col-widen text-center']";
            var valueNodeCases = doc.DocumentNode.SelectNodes(xpathCases).ToList();

            var skinInfoList = new List<SkinInfo>();
            foreach (var caseNode in valueNodeCases)
            {
                var caseUrl = caseNode.SelectSingleNode($"{caseNode.XPath}/div/a")?.GetAttributeValue("href", string.Empty) ?? string.Empty;
                var caseDoc = await web.LoadFromWebAsync(caseUrl);

                var xpathSkins = "//*[@class='col-lg-4 col-md-6 col-widen text-center']";
                var valueNodes = caseDoc.DocumentNode.SelectNodes(xpathSkins).ToList();

                var xpathCaseName = "/html/body/div[2]/div[3]/div/div[2]/h1";
                var caseName = caseDoc.DocumentNode.SelectNodes(xpathCaseName).SingleOrDefault()?.InnerText ?? string.Empty;

                foreach (var node in valueNodes)
                {
                    var name = node.SelectSingleNode($"{node.XPath}/div/h3")?.InnerText ?? string.Empty;

                    var rarityText = node.SelectSingleNode($"{node.XPath}/div/a[1]/div/p")?.InnerText?.Replace("-", "") ?? string.Empty;
                    var rarity = GetRarity(rarityText);

                    var imageUrl = node.SelectSingleNode($"{node.XPath}/div/a[2]/img")?.GetAttributeValue("src", string.Empty) ?? string.Empty;

                    var skinInfo = new SkinInfo(name, rarity, imageUrl, caseName);
                    skinInfoList.Add(skinInfo);
                }
            }

            return JsonConvert.SerializeObject(skinInfoList);
        }

        private Rarity GetRarity(string rarityText)
        {
            if (rarityText.Contains(RarityName.MilSpec.ToString()))
            {
                return Rarity.Blue;
            }
            else if (rarityText.Contains(RarityName.Restricted.ToString()))
            {
                return Rarity.Purple;
            }
            else if (rarityText.Contains(RarityName.Classified.ToString()))
            {
                return Rarity.Pink;
            }
            else if (rarityText.Contains(RarityName.Covert.ToString()))
            {
                return Rarity.Red;
            }
            else if (rarityText.Contains(RarityName.Knifes.ToString()))
            {
                return Rarity.Knife;
            }
            else
            {
                throw new Exception("Invaid rarity for rarity name.");
            }
        }
    }
}
