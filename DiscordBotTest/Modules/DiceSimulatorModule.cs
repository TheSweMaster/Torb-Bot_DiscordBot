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
    public class DiceSimulatorModule : ModuleBase<SocketCommandContext>
    {
        [Command("rolldices")]
        public async Task RollDicesCommand(string amount = "1")
        {
            int amountValue = ValidateInput(amount);
            var diceList = RollDices(amountValue);

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"@{Context.User.Username} Rolled {amount} dices!")
                .WithColor(Color.LightOrange);

            foreach (var dice in diceList)
            {
                builder.AddField("Dice:", dice);
            }

            await ReplyAsync("", false, builder.Build());
        }

        [Command("rolldice")]
        public async Task RollDiceCommand()
        {
            var r = new Random();
            var dice = r.Next(1, 7);

            EmbedBuilder builder = new EmbedBuilder();
            builder.WithTitle($"@{Context.User.Username} Rolled a dice with value {dice}!")
                .WithColor(Color.LightOrange);

            await ReplyAsync("", false, builder.Build());
        }

        private List<int> RollDices(int amountValue)
        {
            var r = new Random();
            var list = new List<int>();
            for (int i = 0; i < amountValue; i++)
            {
                list.Add(r.Next(1, 7));
            }
            return list;
        }

        private int ValidateInput(string input)
        {
            try
            {
                int times = Convert.ToInt32(input);
                return times = (times > 6 || times < 1) ? 1 : times;
            }
            catch (Exception)
            {
                Console.WriteLine($"Error, Invalid number input. '{input}'");
                return 1;
            }
        }

    }
}
