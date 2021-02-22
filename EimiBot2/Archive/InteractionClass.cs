using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Addons.Interactive;
using Renci.SshNet.Messages.Transport;

namespace EimiBot2.Modules
{
    public class InteractionClass : InteractiveBase
    {
        private Logger log = new Logger();

        public async Task GetInput()
        {
            TimeSpan delay = TimeSpan.FromSeconds(5);
            //var response = await NextMessageAsync();
            var response = "no";
            log.Info(response.ToString());

            if (response != null || response.ToString() != "")
            {
                AdminCommands ad = new AdminCommands
                {
                    Prf = response.ToString(),
                    ReplyStatus = true
                };
            }
            else
            {
                _ = new AdminCommands { ReplyStatus = false };
            }
        }

        public async Task GetConfirmation()
        {
            string replyMessage = "test";
            int next = 0;
            while (next < 5)
            {
                await ReplyAsync(replyMessage);
                //var response = await NextMessageAsync();
                var response = "n";
                
                log.Info(response.ToString());

                if (response != null || response.ToString() != "")
                {
                    if (response.ToString().ToLower() == "yes" || response.ToString().ToLower() == "ye" || response.ToString().ToLower() == "y")
                    {
                        _ = new AdminCommands { ReplyStatus = true };
                        next = 10;

                    }
                    else if (response.ToString().ToLower() == "nah" || response.ToString().ToLower() == "no" || response.ToString().ToLower() == "n")
                    {
                        _ = new AdminCommands { ReplyStatus = false };
                        next = 10;
                    }
                    else
                    {
                        await ReplyAsync("Invalid input. Input one more time.");
                        next++;
                    }
                }
            }

            if (next < 10)
            {
                await ReplyAsync("Invalid inputs. Will close the function.");
                _ = new AdminCommands { ReplyStatus = false };
            }
        }
    }
}
