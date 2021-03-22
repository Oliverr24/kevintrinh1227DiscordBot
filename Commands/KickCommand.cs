using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class KickCommand : BaseCommandModule {

        [Command("kick")]
        public async Task KickCmd(CommandContext ctx, DiscordMember member, [RemainingText] string reason) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "*");

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

            var botUseageCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("bot-usage")).Value;

            var kickEmbed = new DiscordEmbedBuilder {
                Title = $"Infractions | Kick",
                Description = $"» A user has been kicked from the server.\n\n**User** » {member.Username}#{member.Discriminator} \n **ID** » {member.Id} \n **Reason** » {reason}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            try {

                await member.RemoveAsync(reason).ConfigureAwait(false);

            }
            catch {

            }

            await botUseageCh.SendMessageAsync(embed: kickEmbed).ConfigureAwait(false);


        }

    }
}
