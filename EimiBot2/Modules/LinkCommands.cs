using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EimiBot2.Modules
{

    // Contains the class related to the link command

    public class LinkCommands : ModuleBase<SocketCommandContext>
    {

        private readonly Logger log = new Logger();

        private void DownloadFile(WebClient wc, string url, string location, string filename) 
        {
            wc.DownloadFile(url, location + filename);
            log.Info("Download successful.");
        }

        private void DeleteFile(string location)
        {
            if (File.Exists(location))
            {
                File.Delete(location);
                log.Info("File deleted successfully at " + location);
            }
            else
            {
                log.Error("File at " + location + " failed to be deleted.");
            }
        }

        [Command("link")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task MessageLink(params string[] args)
        {
            if (args.Length != 0)
            {
                try
                {
                    // 6 = message, 5 = channel, 4 = guild
                    string[] input = args[0].Split('/');

                    var message = input[6];
                    var channel = input[5];
                    var guild = input[4];

                    var messageContent = await Context.Client.GetGuild(Convert.ToUInt64(guild)).GetTextChannel(Convert.ToUInt64(channel)).GetMessageAsync(Convert.ToUInt64(message));

                    var guildname = Context.Client.GetGuild(Convert.ToUInt64(guild)).Name;
                    var channelname = Context.Client.GetGuild(Convert.ToUInt64(guild)).GetTextChannel(Convert.ToUInt64(channel)).Name;

                    //var messageContent = await Context.Guild.GetTextChannel(Convert.ToUInt64(channel)).GetMessageAsync(Convert.ToUInt64(message));

                    if (messageContent.Content != null) 
                    {
                        log.Info("Message: " + messageContent.Content);
                    }

                    log.Info("Number of Attachments: " + messageContent.Attachments.Count);

                    var attachments = messageContent.Attachments;
                    string[] filenames = new string[attachments.Count];

                    if (attachments.Count > 0)
                    {
                        WebClient myWebClient = new WebClient();
                        string location = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\temp\";

                        int count = 0;

                        foreach(var i in attachments)
                        {
                            string file = i.Filename;
                            string url = i.Url;

                            filenames[count] = file;

                            // Download the resource
                            DownloadFile(myWebClient, url, location, file);
                            count++;
                        }
                    }

                    var chatLocation = guildname + " / " + channelname + "\n";

                    if (messageContent.Content != null && attachments.Count == 1)
                    {
                        // If there's only one attachment, combine everything into one SendFileAsync
                        string location = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\temp\" + filenames[0];
                        string content = "**" + messageContent.Author + " (" + messageContent.Author.Id + ")**: \n" + messageContent.Content;
                        await Context.Channel.SendFileAsync(location, chatLocation + content);

                        DeleteFile(location);

                    }
                    else if (messageContent.Content != null)
                    {
                        // If there's a message with multiple attachments
                        await ReplyAsync(chatLocation + "**" + messageContent.Author + " (" + messageContent.Author.Id + ")**: \n" + messageContent.Content);
                    }
                    else 
                    {
                        // If there's no message with multiple attachments
                        await ReplyAsync(chatLocation +     "**" + messageContent.Author + " (" + messageContent.Author.Id + ")**: ");
                    }

                    if (attachments.Count > 1)
                    {
                        foreach (var a in filenames)
                        {
                            string location = @"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\temp\" + a;

                            await Context.Channel.SendFileAsync(location);

                            DeleteFile(location);
                        }
                    }

                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    await ReplyAsync("Invalid message ID!");
                    return;
                }
            }
            else if (args.Length == 0)
            {
                await ReplyAsync("Link command accepts a Message Link argument and repeats the message sent.");
            }
        }
    }
}
