using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class InspectCommand : BaseCommandModule {

        [Command("inspect")]
        [Description("Inspect a member of your server and get information for them.")]
        public async Task InspectCmd(CommandContext ctx, DiscordMember member = null) {

            if (ctx.Guild == null) {

                return;
            }

            await ctx.Channel.DeleteMessageAsync(ctx.Message).ConfigureAwait(false);

            if (member == null) {
                var incorrectEmbed = new DiscordEmbedBuilder {
                    Title = "Incorrect Syntax",
                    Description = "Syntax: !inspect <name>",
                    Color = DiscordColor.DarkRed,
                };

                return;
            }

            var roleCheck = ctx.Member.Roles.Any(x => x.Id == 822711382373433364);

            if (!roleCheck) {
                var failEmbed = new DiscordEmbedBuilder {
                    Title = "Invalid Permissions",
                    Description = "You do not have permission to use this command!",
                    Color = DiscordColor.DarkRed,
                    Timestamp = DateTime.UtcNow,
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

            List<string> rString = new List<string>();

            foreach (var r in member.Roles) {
                if (member.Roles == null) {
                    break;
                }
                rString.Add(r.Name);
            }

            string memRoles = string.Join(", ", rString.ToArray());

            var joinedDate = member.JoinedAt;

            var diff = (DateTime.UtcNow - joinedDate).TotalDays;

            var days = Math.Round(diff, 2);

            var inspectEmbed = new DiscordEmbedBuilder {
                Title = $"Asmodeus Club | Inspect",
                Description = $"» A user has been inspected.\n\n**User** » {member.Mention}#{member.Discriminator} \n **ID** » {member.Id} \n **Day's In The Server** » {days} \n **Account Created** » {member.CreationTimestamp.UtcDateTime} \n **Member Roles** » {memRoles}",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            inspectEmbed.WithThumbnail(member.AvatarUrl);

            await staffLogCh.SendMessageAsync(embed: inspectEmbed).ConfigureAwait(false);

        }

    }
}
