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

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "bot.use");

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

            var bustsEmo = DiscordEmoji.FromName(ctx.Client, ":busts_in_silhouette:");
            var gearEmo = DiscordEmoji.FromName(ctx.Client, ":gear:");

            var reactionEmbed = new DiscordEmbedBuilder {
                Title = $"Self Roles",
                Description = $"» Select a role to get server change updates, \n » React again to remove the selected role. \n\n » React with {bustsEmo}  to get the @stafflogs  role! \n » React with {gearEmo} to get the @changelogs role!",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            var reactMsg = await ctx.Channel.SendMessageAsync(embed: reactionEmbed).ConfigureAwait(false);

            await reactMsg.CreateReactionAsync(bustsEmo).ConfigureAwait(false);
            await reactMsg.CreateReactionAsync(gearEmo).ConfigureAwait(false);

        }

    }
}
