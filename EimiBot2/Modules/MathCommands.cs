using Discord;
using Discord.Commands;
using Discord.Webhook;
using Discord.WebSocket;
using System;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EimiBot2.Modules
{
    public class MathCommands : ModuleBase<SocketCommandContext>
    {
        [Command("sc")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task SimpleCalculator(params string[] args)
        {
            // Combine all the inputs into one
            var input = "";

            foreach (var i in args)
            {
                input += i;
            }

            input.Replace(" ", "");

            // 9 + 9 * 10 / 20

            await ReplyAsync("Simple: " + input); 
        }
    }
}
