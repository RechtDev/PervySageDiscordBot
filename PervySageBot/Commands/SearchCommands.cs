using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using PervySageBot.Content.Fetcher;
using PervySageBot.ContentFetcher.ContentManipulation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PervySageBot.Commands
{
    public class SearchCommands: BaseCommandModule
    {
        [Command("Search")]
        [Description("Finds you porn based off your entry")]
        [RequireRoles(RoleCheckMode.Any,"pervert")]
        public async Task SearchCommandWithsQuery(CommandContext ctx, 
            [Description("The result you want")] string query)
        {

            var messageEmbed = new DiscordEmbedBuilder()
            {
                Title = $"It's Demon Time",
                Color = DiscordColor.DarkRed,
                Description = $"Crafting {ctx.Member.DisplayName} A Delicate Package\n The Demons Are Hard at Work",
            };
            var message = await ctx.Channel.SendMessageAsync(embed: messageEmbed);
            WebsiteScraper websiteScraper = new WebsiteScraper(query);
            websiteScraper.GetWebsite(out string[] imgUrls, out string RefererUrl);
            Dictionary<string, Stream> streams = ContentIO.GetDataStreams(await websiteScraper.DownloadAttachments(RefererUrl, imgUrls));
            string[] FileLocations = new String[streams.Count];
            int count = 0;
            for (int i = 0; i < streams.Count; i++)
            {
                FileLocations[i] = $"{ContentIO.FilePrefix}{i}.jpeg";
            }
            var responseEmbed = new DiscordEmbedBuilder()
            {
                Title = "Package Incoming",
                Color = DiscordColor.DarkRed,
                Description = "look at these fire pics",
            };
            responseEmbed.WithImageUrl(@$"attachment://{ContentIO.FilePrefix}{count}.jpeg");
            var responseMessage = new DiscordMessageBuilder();
            var leftArrowEmoji = DiscordEmoji.FromName(ctx.Client, $":arrow_left:");
            var rightArrowEmoji = DiscordEmoji.FromName(ctx.Client, $":arrow_right:");
            responseMessage.WithFile(fileName: $"{ContentIO.FilePrefix}{count}.jpeg", streams[$"{ContentIO.FilePrefix}{count}.jpeg"]);
            responseMessage.WithEmbed(responseEmbed.Build());
            responseMessage.AddComponents(new DiscordComponent[]
                {
                    new DiscordButtonComponent(ButtonStyle.Danger,"_prev", null, false, new DiscordComponentEmoji(leftArrowEmoji)),
                    new DiscordButtonComponent(ButtonStyle.Danger,"_next", null, false, new DiscordComponentEmoji(rightArrowEmoji))
                });
            await ctx.RespondAsync(responseMessage).ConfigureAwait(false);
            while (streams.Count > 1 && count <= streams.Count)
            {
                count = count + 1;
                ctx.Client.ComponentInteractionCreated += async (s, e)  =>
                {
                    if (e.Id == "_next")
                    {
                        responseEmbed.WithImageUrl(@$"attachment://{ContentIO.FilePrefix}{count}.jpeg");
                        responseMessage.WithFile(fileName: $"{ContentIO.FilePrefix}{count}.jpeg", streams[$"{ContentIO.FilePrefix}{count}.jpeg"]);
                        await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().AddEmbed(responseEmbed));
                    }
                };
            }
        }
    }
}
