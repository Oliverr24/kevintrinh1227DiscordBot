using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class PurgeCommand : BaseCommandModule {

        [Command("purge")]
        public async Task PurgeCmd(CommandContext ctx, int amount = 0) {

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

            if (amount == 0) {
                await ctx.Channel.SendMessageAsync($"{ctx.Member.Mention} please specify the number of messages to clear. Max int 100.").ConfigureAwait(false);
                return;
            }

            if (amount > 100) {
                await ctx.Channel.SendMessageAsync($"{ctx.Member.Mention} please specify a number no greater than 100.").ConfigureAwait(false);
                return;
            }

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var staffLogCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            var clearEmbed = new DiscordEmbedBuilder {
                Title = $"Asmodeus Club | Chat Purge",
                Description = $"» Messages have been purged.\n\n**Amount** » {amount} \n **Channel** » {ctx.Channel.Mention}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            IReadOnlyList<DiscordMessage> msgs = await ctx.Channel.GetMessagesAsync(amount).ConfigureAwait(false);

            await ctx.Channel.DeleteMessagesAsync(msgs).ConfigureAwait(false);

            await staffLogCh.SendMessageAsync(embed: clearEmbed).ConfigureAwait(false);

        }

    }
}
