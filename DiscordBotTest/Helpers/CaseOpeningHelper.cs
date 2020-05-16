using Discord;
using System;
using System.Collections.Generic;
using System.Linq;

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
                return new CaseOpeningResult(new ShatteredWebCase().GetWeaponSkin(Rarity.Knife));
            }
            else if (drop < 7)
            {
                // Odds: 5/800
                return new CaseOpeningResult(new ShatteredWebCase().GetWeaponSkin(Rarity.Red));
            }
            else if (drop < 33)
            {
                // Odds: 26/800
                return new CaseOpeningResult(new ShatteredWebCase().GetWeaponSkin(Rarity.Pink));
            }
            else if (drop < 163)
            {
                // Odds: 130/800
                return new CaseOpeningResult(new ShatteredWebCase().GetWeaponSkin(Rarity.Purple));
            }
            else
            {
                // Odds: 637/800
                return new CaseOpeningResult(new ShatteredWebCase().GetWeaponSkin(Rarity.Blue));
            }
        }
    }

    public class ShatteredWebCase
    {
        public static string CaseName = "Shattered Web";

        // new SkinInfo("____NAME____", Rarity.Red, "_______Link__________"),
        private static readonly List<SkinInfo> SkinInfoList = new List<SkinInfo>
            {
                new SkinInfo("★ Nomad Knife", Rarity.Knife, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/weapons/base_weapons/weapon_knife_outdoor.fdb3ce5ceef1584781759ef5a7bd6f819bf12e9b.png"),
                new SkinInfo("★ Skeleton Knife", Rarity.Knife, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/weapons/base_weapons/weapon_knife_skeleton.1fc401a844008bcaa89f8381cbe7b550a051609d.png"),
                new SkinInfo("★ Survival Knife", Rarity.Knife, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/weapons/base_weapons/weapon_knife_canis.ae03aed81864dc2ee1e1118bb973418f910098ac.png"),
                new SkinInfo("★ Paracord Knife", Rarity.Knife, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/weapons/base_weapons/weapon_knife_cord.073b5fa991a256ec2264b1c1c581401631eb51cb.png"),

                new SkinInfo("AWP | Containment Breach", Rarity.Red, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_awp_cu_awp_virus_light_large.00307f818d425d94cb8e4eeda1e27699f713fb45.png"),
                new SkinInfo("MAC-10 | Stalker", Rarity.Red, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_mac10_gs_mac10_stalker_light_large.cb4d7a60a69978f1575526f979be8e1e1538a673.png"),

                new SkinInfo("SSG 08 | Bloodshot", Rarity.Pink, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_ssg08_cu_ssg08_tickler_light_large.b6a73afa62f8983211740a1570410df4864f45f6.png"),
                new SkinInfo("SG 553 | Colony IV", Rarity.Pink, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_sg556_cu_sg553_reactor_light_large.af5c07d2528198641adf32cf3498aa7d2823b048.png"),
                new SkinInfo("Tec-9 | Decimator", Rarity.Pink, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_tec9_gs_tec9_decimator_light_large.11236163667e1dd46c3d869d844e384a23544513.png"),

                new SkinInfo("AK-47 | Rat Rod", Rarity.Purple, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_ak47_gs_ak47_nibbler_light_large.1c402d395b628aa5667239eec44640d7f603d754.png"),
                new SkinInfo("AUG | Arctic Wolf", Rarity.Purple, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_aug_cu_aug_whitefang_light_large.051b21da4e56c64ad78ee8a67a0e9e237a4e01b1.png"),
                new SkinInfo("P2000 | Obsidian", Rarity.Purple, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_hkp2000_cu_p2000_obsidian_light_large.0a6ad31fe8f70401ffdd377c289309d0256282c6.png"),
                new SkinInfo("MP7 | Neon Ply", Rarity.Purple, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_mp7_cu_mp7_replica_light_large.f56c050cb5147918efb6872ce61fda358a684cf5.png"),
                new SkinInfo("PP-Bizon | Embargo", Rarity.Purple, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_bizon_cu_bizon_road_warrior_light_large.92dd3aa0346010a6e3a625d01ae4f965195a05d2.png"),

                new SkinInfo("Dual Berettas | Balance", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_elite_gs_dual_elites_rose_light_large.8df8980203b198879875be44656361ccbb41791e.png"),
                new SkinInfo("MP5-SD | Acid Wash", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_mp5sd_gs_mp5_etch_light_large.a86867a43e3607c1826b6d106870bf69642687b0.png"),
                new SkinInfo("M249 | Warbird", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_m249_gs_m249_warbird_veteran_light_large.29a2ee22222b037e6825fb0c230aa799718e4115.png"),
                new SkinInfo("SCAR-20 | Torn", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_scar20_sp_scar20_striker_dust_light_large.0842dbdf0fe714c92f634b376e15c2f8c21b6d56.png"),
                new SkinInfo("R8 Revolver | Memento", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_revolver_gs_r8_memento_light_large.8ca600a94c72b5a3b33bb826a1a588f7e48d5e78.png"),
                new SkinInfo("G3SG1 | Black Sand", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_g3sg1_cu_g3sg1_blacksand_light_large.8a9b364779493ae19a87eb4e73aa47f4432d3f41.png"),
                new SkinInfo("Nova | Plume", Rarity.Blue, "https://steamcdn-a.akamaihd.net/apps/730/icons/econ/default_generated/weapon_nova_cu_nova_featherswing_light_large.49e81792746dc7844ff18c45fb23030cd8b66d59.png"),
            };

        public WeaponSkin GetWeaponSkin(Rarity rarity)
        {
            var skinVariants = SkinInfoList.Count(s => s.Rarity == rarity);
            var skinIndex = new Random().Next(0, skinVariants);

            var skinInfo = SkinInfoList.Where(s => s.Rarity == rarity).ElementAt(skinIndex);

            return new WeaponSkin(skinInfo.Name, skinInfo.Rarity, skinInfo.ImageLink, CaseName);
        }
    }

    public class SkinInfo
    {
        public SkinInfo(string name, Rarity rarity, string imageLink)
        {
            Name = name;
            Rarity = rarity;
            ImageLink = imageLink;
        }

        public string Name { get; }
        public Rarity Rarity { get; }
        public string ImageLink { get; }
        public string Case { get; set; }
    }

    public class WeaponSkin
    {
        public WeaponSkin(string name, Rarity rarity, string imageLink, string weaponCase)
        {
            var floatValue = new Random().NextDouble();

            Name = name;
            Rarity = rarity;
            ImageUrl = imageLink;
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
}
