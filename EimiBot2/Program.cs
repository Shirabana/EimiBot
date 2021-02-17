using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace EimiBot2
{
    public class Program
    {

        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();

        private readonly Logger log = new Logger();
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public async Task RunBotAsync()
        {
            log.Info("Bot starting up");
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();

            var token = File.ReadAllText(@"C:\Users\Amagi\source\repos\EimiBot\EimiBot2\token.config");

            _client.Log += _client_Log;

            await RegisterCommandAsync();

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
            await _client.SetActivityAsync(new Game("Coding a new world"));

            log.Info("Bot successfully started up.");

            // Wait
            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg)
        {
            log.Info(arg.ToString());
            
            return Task.CompletedTask;
        }

        public async Task RegisterCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message == null) return;

            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            int argPos = 1;

            if (message.HasStringPrefix("!e ", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) log.Error(result.ErrorReason);
            }
        }
    }
}
