using DSharpPlus;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class WelcomeMethod {

        public async Task WelcomeSendMethod(DiscordClient sender, DiscordGuild guild, DiscordMember member) {

            var welcomeChannel = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("welcome")).Value;
            var botUseageCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("bot-usage")).Value;

            var welcomeRole = guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("» default")).Value;

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var pinEmo = DiscordEmoji.FromName(sender, ":pushpin:");

            if (welcomeChannel == null || welcomeRole == null || botUseageCh == null) {
                return;
            }

            var welcomeEmbed = new DiscordEmbedBuilder() {
                Title = $"Welcome {member.Username}#{member.Discriminator} To **Asmodeous Club** (#{guild.MemberCount})",
                Description = $"\n{pinEmo}**Server Information**{pinEmo}\n\n**Server IP** » `play.asmodeus.club`\n**Server Store** » https://asmodeus.club/store \n**Forums** » https://asmodeus.club/",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.UtcNow,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"©️ Asmodeus Club | Network"
                }
            };

            welcomeEmbed.WithThumbnail(member.AvatarUrl);

            await welcomeChannel.SendMessageAsync(embed: welcomeEmbed).ConfigureAwait(false);

            try {
                await member.GrantRoleAsync(welcomeRole).ConfigureAwait(false);
            }
            catch {
                await botUseageCh.SendMessageAsync($"The role {welcomeRole.Name} was not applied to {member.Mention}.").ConfigureAwait(false);
            }

        }

    }
}
