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
            var staffLogsCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;
            var selfRoleCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("self-roles")).Value;
            var announcementsCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("announcements")).Value;
            var serverlogsCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("server-logs")).Value;
            var rulesCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("rules")).Value;
            var informationCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("information")).Value;
            var nitroCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("nitro-boost")).Value;

            var welcomeRole = guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("» default")).Value;

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var pinEmo = DiscordEmoji.FromName(sender, ":pushpin:");
            var warnEmo = DiscordEmoji.FromName(sender, ":warning:");

            if (welcomeChannel == null || welcomeRole == null || botUseageCh == null || staffLogsCh == null || selfRoleCh == null || announcementsCh == null || serverlogsCh == null || rulesCh == null || informationCh == null) {
                return;
            }

            var welcomeEmbed = new DiscordEmbedBuilder() {
                Title = $"Welcome {member.Username}#{member.Discriminator} To **Asmodeus Club** (#{guild.MemberCount})",
                Description = $"\n{pinEmo}**Server Information**{pinEmo}\n\n**Server IP** » `play.asmodeus.club`\n**Server Store** » https://asmodeus.club/store \n**Forums** » https://asmodeus.club/",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.UtcNow,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"©️ Asmodeus Club | Network"
                }
            };

            var welcomeDMEmbed = new DiscordEmbedBuilder() {
                Title = $"{pinEmo} Welcome to Asmodeus Club's discord server! {pinEmo}",
                Description = $"**Please do the following...** \n ➜ View our latest server updates and news at {announcementsCh.Mention}. \n ➜ View important {informationCh.Mention} from our team." +
                $"\n ➜ View our latest staff / network change logs at {serverlogsCh.Mention}. \n ➜ Read our discord server {rulesCh.Mention}. \n ➜ Select {selfRoleCh.Mention} based on desired notifications." +
                $"\n \n {warnEmo} **Account Linking** {warnEmo} \n ➜ Most importantly be sure to link your discord account with your Minecraft account by running `/discord sync` in lobby/hub and sending your code here. Perks for server boosting can be found here {nitroCh.Mention}.",
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

            try {
                await member.SendMessageAsync(embed: welcomeDMEmbed).ConfigureAwait(false);
            }
            catch {
                await staffLogsCh.SendMessageAsync($"{member.Mention} did not receive join message - private messages are off.").ConfigureAwait(false);
            }

            await staffLogsCh.SendMessageAsync($"{member.Username} joined the server.").ConfigureAwait(false);

        }

    }
}
