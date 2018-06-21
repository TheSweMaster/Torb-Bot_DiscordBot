using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class CaseSimulator : ModuleBase<SocketCommandContext>
    {
        private static Color _color;

        [Command("opencase")]
        public async Task OpenCase()
        {
            var caseResult = OpenOneCase();

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"@{Context.User.Username} opened a {caseResult.Rarity} rarity skin!")
                .WithColor(caseResult.Color);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("opencases")]
        public async Task OpenCases(string input = "1")
        {
            var result = RunCaseSimulator(input);

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"@{Context.User.Username} opened {result.Openings} cases!")
                .AddField("Blue skins:", result.BlueCount)
                .AddField("Purple skins:", result.PurpleCount)
                .AddField("Pink skins:", result.PinkCount)
                .AddField("Red skins:", result.RedCount)
                .AddField("Knives:", result.KnifeCount)
                .AddField("Blue Streak:", result.BlueStreak)
                .AddField("Non-Blue Streak:", result.NonBlueStreak)
                .WithColor(Color.Green);

            await ReplyAsync("", false, builder.Build());
        }

        public class CaseOpeningResult
        {
            public string Rarity { get; set; }
            public Color Color { get; set; }

        }


        private int ValidateInput(string input)
        {
            try
            {
                int times = Convert.ToInt32(input);
                return times = (times > 10000 || times < 1) ? 1 : times;
            }
            catch (Exception)
            {
                Console.WriteLine($"Error, Invalid number input. '{input}'");
                return 1;
            }
        }

        private ManyCaseOpeningResult RunCaseSimulator(string input)
        {
            var r = new Random();
            var sb = new StringBuilder();
            var result = new ManyCaseOpeningResult();

            //int times = 0;
            int openings = 0;
            int knifeCount = 0;
            int redCount = 0;
            int pinkCount = 0;
            int purpleCount = 0;
            int blueCount = 0;
            int longestNonBlueStreak = 0;
            int longestBlueStreak = 0;
            int blueStreak = 0;
            int nonBlueStreak = 0;

            int times = ValidateInput(input);

            var resultList = new List<string>();

            while (times > 0)
            {
                openings++;

                var drop = r.Next(0, 800);
                if (drop < 2)
                {
                    //Odds: 2/800
                    sb.Append("Knife!");
                    _color = Color.Gold;
                    knifeCount++;
                    nonBlueStreak++;
                    blueStreak = 0;
                }
                else if (drop < 7)
                {
                    //Odds: 5/800
                    sb.Append("Red");
                    _color = Color.Red;
                    redCount++;
                    nonBlueStreak++;
                    blueStreak = 0;
                }
                else if (drop < 33)
                {
                    //Odds: 26/800
                    sb.Append("Pink");
                    _color = Color.Magenta;
                    pinkCount++;
                    nonBlueStreak++;
                    blueStreak = 0;
                }
                else if (drop < 163)
                {
                    //Odds: 130/800
                    sb.Append("Purple");
                    _color = Color.Purple;
                    purpleCount++;
                    nonBlueStreak++;
                    blueStreak = 0;
                }
                else
                {
                    //Odds: 637/800
                    sb.Append("Blue");
                    _color = Color.Blue;
                    blueCount++;
                    blueStreak++;
                    nonBlueStreak = 0;
                }

                if (blueStreak > longestBlueStreak)
                {
                    longestBlueStreak = blueStreak;
                }

                if (nonBlueStreak > longestNonBlueStreak)
                {
                    longestNonBlueStreak = nonBlueStreak;
                }

                var resultRarity = sb.ToString();
                //Console.WriteLine(resultRarity);
                resultList.Add(resultRarity);

                sb.Clear();
                times--;
            }

            var knifeCount2 = resultList.Where(x => x == "Knife!").ToList().Count;
            var blueCount2 = resultList.Where(x => x == "Blue").ToList().Count;
            var purpleCount2 = resultList.Where(x => x == "Purple").ToList().Count;
            var pinkCount2 = resultList.Where(x => x == "Pink").ToList().Count;
            var redCount2 = resultList.Where(x => x == "Red").ToList().Count;

            var blueStreakOdds = Math.Pow((double)637 / 800, longestBlueStreak);
            var nonBlueStreakOdds = Math.Pow((double)163 / 800, longestNonBlueStreak);

            //Console.WriteLine($"Longest Blue Streak: {longestBlueStreak}," +
            //    $" Odds: {Math.Round(blueStreakOdds * 100, 4)}%");

            //Console.WriteLine($"Longest Non-Blue Streak: {longestNonBlueStreak}," +
            //    $" Odds: {Math.Round(nonBlueStreakOdds * 100, 4)}%");

            result.Openings = openings;
            result.BlueCount = blueCount;
            result.PurpleCount = purpleCount;
            result.PinkCount = pinkCount;
            result.RedCount = redCount;
            result.KnifeCount = knifeCount;
            result.BlueStreak = longestBlueStreak;
            result.NonBlueStreak = longestNonBlueStreak;

            return result;
        }

        public class ManyCaseOpeningResult
        {
            public int Openings { get; set; }
            public int BlueCount { get; set; }
            public int PurpleCount { get; set; }
            public int PinkCount { get; set; }
            public int RedCount { get; set; }
            public int KnifeCount { get; set; }
            public int BlueStreak { get; set; }
            public int NonBlueStreak { get; set; }

        }

        public CaseOpeningResult OpenOneCase()
        {
            var r = new Random();
            var caseResult = new CaseOpeningResult();

            var drop = r.Next(0, 800);
            if (drop < 2)
            {
                //Odds: 2/800
                caseResult.Rarity = "Knife!";
                caseResult.Color = Color.Gold;
            }
            else if (drop < 7)
            {
                //Odds: 5/800
                caseResult.Rarity = "Red";
                caseResult.Color = Color.Red;
            }
            else if (drop < 33)
            {
                //Odds: 26/800
                caseResult.Rarity = "Pink";
                caseResult.Color = Color.Magenta;
            }
            else if (drop < 163)
            {
                //Odds: 130/800
                caseResult.Rarity = "Purple";
                caseResult.Color = Color.Purple;
            }
            else
            {
                //Odds: 637/800
                caseResult.Rarity = "Blue";
                caseResult.Color = Color.Blue;
            }

            return caseResult;
        }

    }
}
