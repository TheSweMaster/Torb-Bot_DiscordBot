using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotTest.Modules
{
    public class TextToEmojis : ModuleBase<SocketCommandContext>
    {
        [Command("emojis")]
        public async Task TextToEmojisCommand([Remainder]string message)
        {
            await SendMessage(message);
        }

        [Command("icons")]
        public async Task TextToIconsCommand([Remainder]string message)
        {
            await SendMessage(message);
        }

        private async Task SendMessage(string message)
        {
            var sb = new StringBuilder();
            foreach (var chr in message.ToLower())
            {
                sb.Append(GetSymbolCode(chr));
            }

            await ReplyAsync(sb.ToString());
        }

        private static string GetSymbolCode(char chr)
        {
            switch (chr)
            {
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                    return $":regional_indicator_{chr}:";
                case '0':
                    return ":zero:";
                case '1':
                    return ":one:";
                case '2':
                    return ":two:";
                case '3':
                    return ":three:";
                case '4':
                    return ":four:";
                case '5':
                    return ":five:";
                case '6':
                    return ":six:";
                case '7':
                    return ":seven:";
                case '8':
                    return ":eight:";
                case '9':
                    return ":nine:";
                case '#':
                    return ":hash:";
                case '*':
                    return ":asterisk:";
                case '<':
                    return ":arrow_backward:";
                case '>':
                    return ":arrow_forward:";
                case '!':
                    return ":exclamation:";
                case '?':
                    return ":question:";
                case '-':
                    return ":heavy_minus_sign:";
                case '+':
                    return ":heavy_plus_sign:";
                case '$':
                    return ":heavy_dollar_sign:";
                default:
                    return chr.ToString();
            }
        }
    }
}
