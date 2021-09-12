using DSharpPlus;
using PervySageBot.Content.Fetcher;
using System;
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
            var disClient = new DiscordClient(new DiscordConfiguration()
            {
                Token = "ODg1ODQ2MTM3NzMzNTQ2MDA1.YTs-gQ.iBztyDAXyZnt7K0xS21N7ALMB-A",
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
