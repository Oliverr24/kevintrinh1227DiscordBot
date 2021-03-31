using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class DeletedMessagesMethod {

        public async Task OnMsgDelete(DiscordGuild guild, MessageDeleteEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var deletedEmbed = new DiscordEmbedBuilder {
                Title = "Message Deleted",
                Description = $"» A message was deleted. \n\n » Deleted From: {e.Channel.Mention} \n » Deleted Message: {e.Message.Content}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            await staffLogCh.SendMessageAsync(embed: deletedEmbed).ConfigureAwait(false);

        }

        public async Task OnMsgBulkDelete(DiscordGuild guild, MessageBulkDeleteEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var deletedEmbed = new DiscordEmbedBuilder {
                Title = "Bulk Messages Deleted",
                Description = $"» A bulk deletion was made. \n\n » Deleted From: {e.Channel.Mention} \n » Deleted Amount: {e.Messages.Count}",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            await staffLogCh.SendMessageAsync(embed: deletedEmbed).ConfigureAwait(false);

        }

        public async Task OnMsgUpdated(DiscordGuild guild, MessageUpdateEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            if (e.Message.Author.IsBot) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            if (e.MessageBefore != null) {

                var deletedEmbed = new DiscordEmbedBuilder {
                    Title = "Message Updated",
                    Description = $"» A message was updated. \n\n » Channel: {e.Channel.Mention} \n » Previouse Message: {e.MessageBefore.Content} \n » Updated Message: {e.Message.Content}",
                    Color = new DiscordColor(0xFF5555),
                    Timestamp = DateTime.Now,
                    Footer = new DiscordEmbedBuilder.EmbedFooter {
                        IconUrl = houndLogo.Url,
                        Text = $"© Asmodeus Club | Network"
                    }
                };

                await staffLogCh.SendMessageAsync(embed: deletedEmbed).ConfigureAwait(false);

            }

        }

    }
}
