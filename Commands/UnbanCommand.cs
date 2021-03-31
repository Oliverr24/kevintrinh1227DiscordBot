using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class UnbanCommand : BaseCommandModule {

        [Command("unban")]
        public async Task UnbanCmd(CommandContext ctx, DiscordUser user, [RemainingText] string reason) {

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

            var staffLogCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            var unbanEmbed = new DiscordEmbedBuilder {
                Title = $"Infractions | Unban",
                Description = $"» A user has been unbanned from the server.\n\n**User** » {user.Username}#{user.Discriminator} \n **ID** » {user.Id} \n **Reason** » {reason}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            try {

                await ctx.Guild.UnbanMemberAsync(user, reason).ConfigureAwait(false);

            }
            catch {

            }

            await staffLogCh.SendMessageAsync(embed: unbanEmbed).ConfigureAwait(false);

        }


    }
}
