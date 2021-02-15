using Discord.Addons.Interactive;
using System.Threading.Tasks;

namespace EimiBot2.Modules
{
    public class InteractionClass : InteractiveBase
    {
        public async Task GetInput()
        {
            var response = NextMessageAsync();

            await Task.Delay(10000);

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
    }
}
