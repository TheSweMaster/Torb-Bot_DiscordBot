using Discord;
using DiscordBotTest.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DiscordBotTest.Helpers
{
    public class CaseOpeningHelper
    {
        public CaseOpeningResult OpenOneCase()
        {
            var drop = new Random().Next(0, 800);
            if (drop < 2)
            {
                // Odds: 2/800
                return new CaseOpeningResult(GetRandomWeaponSkin(Rarity.Knife));
            }
            else if (drop < 7)
            {
                // Odds: 5/800
                return new CaseOpeningResult(GetRandomWeaponSkin(Rarity.Red));
            }
            else if (drop < 33)
            {
                // Odds: 26/800
                return new CaseOpeningResult(GetRandomWeaponSkin(Rarity.Pink));
            }
            else if (drop < 163)
            {
                // Odds: 130/800
                return new CaseOpeningResult(GetRandomWeaponSkin(Rarity.Purple));
            }
            else
            {
                // Odds: 637/800
                return new CaseOpeningResult(GetRandomWeaponSkin(Rarity.Blue));
            }
        }

        public WeaponSkin GetRandomWeaponSkin(Rarity rarity)
        {
            const string WeaponSkinsFilePath = @"Data\CSGO_WeaponSkins_v1.json";

            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), WeaponSkinsFilePath);
            var jsonFile = File.ReadAllText(path, Encoding.UTF8);

            var allSkins = JsonConvert.DeserializeObject<List<SkinInfo>>(jsonFile);

            var skinVariants = allSkins.Count(s => s.Rarity == rarity);
            var skinIndex = new Random().Next(0, skinVariants);

            var skinInfo = allSkins.Where(s => s.Rarity == rarity).ElementAt(skinIndex);

            return new WeaponSkin(skinInfo.Name, skinInfo.Rarity, skinInfo.ImageUrl, skinInfo.CaseName);
        }
    }

    public class WeaponSkin
    {
        public WeaponSkin(string name, Rarity rarity, string imageUrl, string weaponCase)
        {
            var floatValue = new Random().NextDouble();

            Name = name;
            Rarity = rarity;
            ImageUrl = imageUrl;
            WeaponCase = weaponCase;
            Condition = GetCondition(floatValue);
            FloatValue = floatValue;
        }

        public string Name { get; }
        public Rarity Rarity { get; }
        public string ImageUrl { get; }
        public string WeaponCase { get; }
        public Condition Condition { get; }
        public double FloatValue { get; }

        public bool IsRareItem()
        {
            return Rarity == Rarity.Knife || Rarity == Rarity.Red || Rarity == Rarity.Pink;
        }

        private Condition GetCondition(double floatValue)
        {
            if (floatValue < 0d)
            {
                throw new Exception("Invalid float value");
            }
            else if (floatValue < 0.07d)
            {
                // 0 - 0.07       Factory New
                return Condition.FactoryNew;
            }
            else if (floatValue < 0.15d)
            {
                // 0.07 - 0.15    Minimal Wear
                return Condition.MinimalWear;
            }
            else if (floatValue < 0.38d)
            {
                // 0.15 - 0.38    Field-Tested
                return Condition.FieldTested;
            }
            else if (floatValue < 0.45d)
            {
                // 0.38 - 0.45    Well-Worn
                return Condition.WellWorn;
            }
            else if (floatValue <= 1d)
            {
                // 0.45 - 1       Battle-Scarred
                return Condition.BattleScarred;
            }
            else
            {
                throw new Exception("Invalid float value");
            }
        }
    }

    public class CaseOpeningResult
    {
        public CaseOpeningResult(WeaponSkin weaponSkin)
        {
            WeaponSkin = weaponSkin;
            Color = GetColor(weaponSkin.Rarity);
        }

        private static Color GetColor(Rarity rarity)
        {
            switch (rarity)
            {
                case Rarity.Blue:
                    return new Color(56, 89, 255);
                case Rarity.Purple:
                    return new Color(136, 71, 255);
                case Rarity.Pink:
                    return new Color(211, 46, 230);
                case Rarity.Red:
                    return new Color(235, 75, 75);
                case Rarity.Knife:
                    return new Color(240, 190, 29);
                default:
                    throw new Exception("Invalid Rarity for Color");
            }
        }

        public WeaponSkin WeaponSkin { get; }
        public Color Color { get; }
    }

    public enum Condition
    {
        FactoryNew,
        MinimalWear,
        FieldTested,
        WellWorn,
        BattleScarred
    }

    public enum Rarity
    {
        Blue,
        Purple,
        Pink,
        Red,
        Knife
    }

    public enum RarityName
    {
        MilSpec,
        Restricted,
        Classified,
        Covert,
        Knifes
    }

}
