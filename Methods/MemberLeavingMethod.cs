using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class MemberLeavingMethod {

        public async Task OnMemberLeave(DiscordGuild guild, DiscordMember member) {

            var latestKick = guild.GetAuditLogsAsync(1).Result.FirstOrDefault(x => x.ActionType == AuditLogActionType.Kick);
            
            var latestBan = guild.GetAuditLogsAsync(1).Result.FirstOrDefault(x => x.ActionType == AuditLogActionType.Ban);

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var leftEmbed = new DiscordEmbedBuilder {
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            if (latestKick == null && latestBan == null) {

                leftEmbed.Title = "Member Disconnected | Left";

                leftEmbed.Description = $"» {member.Username}#{member.Discriminator} left the server.";

                await staffLogCh.SendMessageAsync(embed: leftEmbed).ConfigureAwait(false);

            } else if (latestBan != null && latestKick == null) {

                leftEmbed.Title = "Member Disconnected | Ban";

                leftEmbed.Description = $"» {member.Username}#{member.Discriminator} was banned from the server by {latestBan.UserResponsible.Username}#{latestBan.UserResponsible.Discriminator}.";

                await staffLogCh.SendMessageAsync(embed: leftEmbed).ConfigureAwait(false);

            } else if (latestKick != null && latestBan == null) {

                leftEmbed.Title = "Member Disconnected | Kick";

                leftEmbed.Description = $"» {member.Username}#{member.Discriminator} was kicked from the server by {latestKick.UserResponsible.Username}#{latestKick.UserResponsible.Discriminator}.";

                await staffLogCh.SendMessageAsync(embed: leftEmbed).ConfigureAwait(false);

            }

        }

    }
}
