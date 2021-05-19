using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class ChannelModificationMethod {

        public async Task OnChannelCreate(DiscordGuild guild, ChannelCreateEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var latestCreation = guild.GetAuditLogsAsync(1).Result.FirstOrDefault(x => x.ActionType == AuditLogActionType.ChannelCreate);

            if (latestCreation == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var createdEmbed = new DiscordEmbedBuilder {
                Title = "Channel Created",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            createdEmbed.Description = $"» {e.Channel.Mention} was created by {latestCreation.UserResponsible.Mention}. \n\n » Channel Type: {e.Channel.Type}";

            await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

        }

        public async Task OnChannelDelete(DiscordGuild guild, ChannelDeleteEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var latestDeletion = guild.GetAuditLogsAsync(1).Result.FirstOrDefault(x => x.ActionType == AuditLogActionType.ChannelDelete);

            if (latestDeletion == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var createdEmbed = new DiscordEmbedBuilder {
                Title = "Channel Deleted",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            createdEmbed.Description = $"» {e.Channel.Name} was deleted by {latestDeletion.UserResponsible.Mention}.";

            await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

        }

        public async Task OnChannelModified(DiscordGuild guild, ChannelUpdateEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var latestUpdate = guild.GetAuditLogsAsync(1).Result.FirstOrDefault(x => x.ActionType == AuditLogActionType.ChannelUpdate);

            if (latestUpdate == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var createdEmbed = new DiscordEmbedBuilder {
                Title = "Channel Edited",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            if (e.ChannelBefore.Name != e.ChannelAfter.Name) {

                createdEmbed.Description = $"» {e.ChannelAfter.Mention} was updated by {latestUpdate.UserResponsible.Mention}. \n » New name: {e.ChannelAfter.Mention}";

                await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

                return;
            }


        }

    }
}
