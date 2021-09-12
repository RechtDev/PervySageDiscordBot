using DSharpPlus;
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
            disClient.MessageCreated += async (s, e) =>
            {
                if (e.Message.Content.ToLower().StartsWith("testing"))
                {
                    await e.Message.RespondAsync("Bot working! GJ");
                }
            };
            await disClient.ConnectAsync();
            await Task.Delay(-1);

            //WebsiteScraper websiteScraper = new WebsiteScraper("ass");
            //websiteScraper.GetWebsite(out string[] imgUrls);
            //await websiteScraper.DownloadaAttachments(websiteScraper.GetSesstionCookies(), imgUrls);
        }
    }
}
