using Discord;
using Discord.Commands;
using DiscordBotTest.Helpers;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class CaseSimulatorModule : ModuleBase<SocketCommandContext>
    {
        [Command("opencase")]
        public async Task OpenCaseCommand()
        {
            var caseResult = new CaseOpeningHelper().OpenOneCase();

            var builder = new EmbedBuilder();
            builder.WithTitle($"@{Context.User.Username} opened a {caseResult.WeaponSkin.Name}")
            .WithDescription($"Exterior: {caseResult.WeaponSkin.Condition} " +
            $"\nFloat: {caseResult.WeaponSkin.FloatValue.ToString(new CultureInfo("en-US"))} " +
            $"\nCase: {caseResult.WeaponSkin.WeaponCase}")
            .WithColor(caseResult.Color);

            if (caseResult.WeaponSkin.IsRareItem())
            {
                builder.WithImageUrl(caseResult.WeaponSkin.ImageUrl);
            }
            else
            {
                builder.WithThumbnailUrl(caseResult.WeaponSkin.ImageUrl);
            }

            await ReplyAsync("", embed: builder.Build());
        }

        [Command("getskinsdata")]
        public async Task GetSkinsData()
        {
            if (Context.User.Id == Program.MyUserId)
            {
                var result = await new SkinInfoGetter().GetAsync();
                Console.WriteLine(result);
            }
        }
    }
}
