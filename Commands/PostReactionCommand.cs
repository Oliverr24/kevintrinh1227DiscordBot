using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class PostReactionCommand : BaseCommandModule {

        [Command("postreaction")]
        [Aliases("pr")]
        public async Task PostReactionCmd(CommandContext ctx) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Id == 826660041498558464);

            if (!roleCheck) {
                var failEmbed = new DiscordEmbedBuilder {
                    Title = "Invalid Permissions",
                    Description = "You do not have permission to use this command. If this is incorrect, please contact an Administrator.",
                    Color = DiscordColor.DarkRed,
                };

                try {
                    await ctx.Member.SendMessageAsync(embed: failEmbed).ConfigureAwait(false);
                }
                catch {

                }

                return;
            }

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var pinEmo = DiscordEmoji.FromName(ctx.Client, ":pushpin:");
            var bustsEmo = DiscordEmoji.FromName(ctx.Client, ":busts_in_silhouette:");
            var gearEmo = DiscordEmoji.FromName(ctx.Client, ":gear:");
            var gameDieEmo = DiscordEmoji.FromName(ctx.Client, ":game_die:");
            var chartEmo = DiscordEmoji.FromName(ctx.Client, ":bar_chart:");
            var bellEmo = DiscordEmoji.FromName(ctx.Client, ":bell:");

            var reactionEmbed = new DiscordEmbedBuilder {
                Title = $"{bellEmo} | Notification Roles",
                Description = $"React to the following roles in order to get pings regarding network updates and events.\nReact again to remove the selected role. \n\n ➜ {pinEmo} Reaction for important network announcements. \n ➜ {bustsEmo} Reaction for Staff Log updates. \n ➜ {gearEmo} Reaction for server change logs and updates. \n ➜ {gameDieEmo} Reaction for community events and games. \n ➜ {chartEmo} Reaction for polls and suggestions. \n",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            var reactMsg = await ctx.Channel.SendMessageAsync(embed: reactionEmbed).ConfigureAwait(false);

            await reactMsg.CreateReactionAsync(pinEmo).ConfigureAwait(false);
            await reactMsg.CreateReactionAsync(bustsEmo).ConfigureAwait(false);
            await reactMsg.CreateReactionAsync(gearEmo).ConfigureAwait(false);
            await reactMsg.CreateReactionAsync(gameDieEmo).ConfigureAwait(false);
            await reactMsg.CreateReactionAsync(chartEmo).ConfigureAwait(false);

        }

    }
}
