using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class AnnounementCommand : BaseCommandModule {

        [Command("announcement")]
        [Aliases("announce", "alert")]
        public async Task AnnouncementCmd(CommandContext ctx, DiscordChannel channel, [RemainingText] string announcement) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "*" || x.Name.ToLower() == "owner");

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

            var ch = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Id == channel.Id).Value;

            if (ch == null) {

                var failEmbed = new DiscordEmbedBuilder {
                    Title = "Command Failed",
                    Description = "The channel you chose has not been found. Please try again!",
                    Color = DiscordColor.DarkRed,
                };

                await ctx.RespondAsync(embed: failEmbed).ConfigureAwait(false);

                return;

            }

            if (string.IsNullOrEmpty(announcement)) {

                var failEmbed = new DiscordEmbedBuilder {
                    Title = "Command Failed",
                    Description = "The announcement message is invalid. Please try again!",
                    Color = DiscordColor.DarkRed,
                };

                await ctx.RespondAsync(embed: failEmbed).ConfigureAwait(false);

                return;

            }

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var announcementEmbed = new DiscordEmbedBuilder() {
                Title = $"{ctx.Guild.Name}",
                Description = $"{announcement}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.UtcNow,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            await ch.SendMessageAsync(embed: announcementEmbed).ConfigureAwait(false);

        }

    }
}
