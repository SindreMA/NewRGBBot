using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace NewRGBBot
{
    class Program
    {
        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
        private CommandHandler _handler;
        private DiscordSocketClient _client;
        public async Task StartAsync()
        {
            await Log("Setting up the bot", ConsoleColor.Green);
            _client = new DiscordSocketClient();
            new CommandHandler(_client);
            await Log("Logging in...", ConsoleColor.Green);
            await _client.LoginAsync(TokenType.Bot, "###################################");
            await Log("Connecting...", ConsoleColor.Green);
            await _client.StartAsync();
            _client.GuildAvailable += _client_GuildAvailable;
            await Task.Delay(-1);
            _handler = new CommandHandler(_client);

        }


        private async Task _client_GuildAvailable(SocketGuild arg)
        {

            await Log(arg.Name + " Connected!", ConsoleColor.Green);
        }
        public static async Task Log(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.Now + " : " + message, color);
            Console.ResetColor();
        }
    }
    public class Roles

    {
        public IRole Role { get; set; }
        public List<IGuildUser> Users { get; set; }
    }
    public class CommandHandler
    {
        public System.Threading.Timer _timer;


        private DiscordSocketClient _client;
        private CommandService _service;
        public CommandHandler(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            _service.AddModulesAsync(Assembly.GetEntryAssembly());
            _client.MessageReceived += _client_MessageReceived;
            _timer = new System.Threading.Timer(Callback, true, 1000, System.Threading.Timeout.Infinite);
        }

        public List<SocketGuild> Paused = new List<SocketGuild>();
        private void Callback(Object state)
        {

            CheckGame();
            // Long running operation
            _timer.Change(500, Timeout.Infinite);
        }
        public async Task CheckGame()
        {
            Color Red = new Color(255, 0, 0);
            Color Pink = new Color(255, 0, 222);
            Color Blue = new Color(0, 0, 255);
            Color Green = new Color(0, 255, 0);
            Color Purple = new Color(239, 0, 255);
            Color Black = new Color(0, 0, 0);
            Color White = new Color(255, 255, 255);
            Color Gray = new Color(146, 146, 146);
            Color Yellow = new Color(239, 255, 0);
            Color Brown = new Color(112, 85, 11);
            Color Orange = new Color(195, 158, 58);
            List<Color> Liste = new List<Color>();
            Liste.Add(Red);
            Liste.Add(Pink);
            Liste.Add(Blue);
            Liste.Add(Green);
            Liste.Add(Purple);
            Liste.Add(Black);
            Liste.Add(White);
            Liste.Add(Gray);
            Liste.Add(Yellow);
            Liste.Add(Brown);
            Liste.Add(Orange);
            foreach (var item in _client.Guilds)
            {
                if (!Paused.Contains(item))
                {


                    foreach (var role in item.Roles)
                    {
                        if (role.Name.Contains("RGB"))
                        {
                            Random sds = new Random();
                            await role.ModifyAsync(t => t.Color = Liste[sds.Next(0, Liste.Count)]);


                        }
                    }
                }
            }
        }
        private async Task _client_MessageReceived(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (context.Guild.GetUser(context.User.Id).GuildPermissions.Administrator || context.User.Id == 170605899189190656)
            {
                if (arg.Content == "_pause")
                {
                    Paused.Add(context.Guild);
                }
                if (arg.Content == "_unpause")
                {
                    Paused.Remove(context.Guild);
                }
            }
        }
    }
}
