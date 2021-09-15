using DSharpPlus;
using DSharpPlus.CommandsNext;
using PervySageBot.Commands;
using PervySageBot.Content.Fetcher;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PervySageBot
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }
        static async Task MainAsync()
        {
            string token;
            if(File.Exists(@"C:\MyTestKeyPair\Tokens\PSTok.txt"))
            {
                token = File.ReadAllText(@"C:\MyTestKeyPair\Tokens\PSTok.txt");
            }
            else
            {
                token = "error";
            }
            var disClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged
                
            });
            var commands = disClient.UseCommandsNext(new CommandsNextConfiguration() 
            {
                StringPrefixes = new[] {"Pervy Sage "},
                
            });
            commands.RegisterCommands<SearchCommands>();
            await disClient.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
