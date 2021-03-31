using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    class BanCommand : BaseCommandModule {

        [Command("ban")]
        public async Task BanCmd(CommandContext ctx, DiscordMember member, [RemainingText] string reason) {

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

            var banEmbed = new DiscordEmbedBuilder {
                Title = $"Infractions | Ban",
                Description = $"» A user has been banned from the server.\n\n**User** » {member.Username}#{member.Discriminator} \n **ID** » {member.Id} \n **Reason** » {reason}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            try {

                await member.BanAsync(0, reason).ConfigureAwait(false);

            }
            catch {

            }

            await staffLogCh.SendMessageAsync(embed: banEmbed).ConfigureAwait(false);

        }


    }
}
