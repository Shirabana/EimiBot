using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace EimiBot2.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly Logger log = new Logger();

        [Command("help")]
        [Summary("Displays the commands of the bot")]
        public async Task Help()
        {
            //string avatarLocation = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\images\bot\ava.jpg";
            log.Info("Help booted up");
            string avatarUrl = "https://i.imgur.com/uhvitiQ.jpg";
            EmbedBuilder eb = new EmbedBuilder();

            string title = "Eimi Bot v0.01";

            var ab = new EmbedAuthorBuilder()
                .WithName(title)
                .WithIconUrl(avatarUrl);

            var fb = new EmbedFooterBuilder()
                .WithText("Powered by the programmer");

            var link = new EmbedFieldBuilder()
                .WithName("Message Link Repeater")
                .WithValue("・link");

            var fun = new EmbedFieldBuilder()
                .WithName("Fun")
                .WithValue("・bento / bentou\n・ping");

            var admin = new EmbedFieldBuilder()
                .WithName("Admin")
                .WithValue("・prefix\n・prune");

            eb.AddField(link);
            eb.AddField(fun);
            eb.AddField(admin);
            eb.WithTitle("List of Commands: (!e)");
            eb.WithAuthor(ab);
            eb.WithFooter(fb);

            await Context.Channel.SendMessageAsync("", false, eb.Build());
        }

        [Command("ping")]
        public async Task Ping()
        {
            log.Info("Ping!");

            await ReplyAsync("Pong!"); 
        }

        [Command("bento")]
        public async Task Bentou()
        {
            Random rand = new Random();
            int choice;
            choice = rand.Next(8);

            log.Info("bento " + choice);

            string location = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\images\bento\" + choice + ".jpg";

            await Context.Channel.SendFileAsync(location);
        }

        [Command("bentou")]
        public async Task BentouR()
        {
            await Bentou();
        }
    }
}
