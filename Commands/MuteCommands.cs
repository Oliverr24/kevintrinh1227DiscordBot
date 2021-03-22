using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class MuteCommands : BaseCommandModule {

        [Command("mute")]
        public async Task MuteCmd(CommandContext ctx, DiscordMember member, [RemainingText] string reason) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "*" || x.Name.ToLower() == "owner");

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

            var muteRole = ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("muted")).Value;

            var botUseageCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("bot-usage")).Value;

            if (member.Roles.Contains(muteRole)) {

                var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

                var muteEmbed = new DiscordEmbedBuilder {
                    Title = $"Infraction | Mute",
                    Description = $"» This user is already muted.\n\n**User** » {member.Mention} \n **ID** » {member.Id} \n **Appeal link** » https://www.asmodeus.club/appeal",
                    Color = new DiscordColor(0xFF5555),
                    Timestamp = DateTime.Now,
                    Footer = new DiscordEmbedBuilder.EmbedFooter {
                        IconUrl = houndLogo.Url,
                        Text = $"© Asmodeus Club | Network"
                    }
                };

                await botUseageCh.SendMessageAsync(embed: muteEmbed).ConfigureAwait(false);

                return;

            } else {

                var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

                var muteEmbed = new DiscordEmbedBuilder {
                    Title = $"Infraction | Mute",
                    Description = $"» A mute has been issued.\n\n**User** » {member.Mention} \n **ID** » {member.Id} \n **Appeal link** » https://www.asmodeus.club/appeal \n\n **Reason** » {reason}",
                    Color = new DiscordColor(0xFF5555),
                    Timestamp = DateTime.Now,
                    Footer = new DiscordEmbedBuilder.EmbedFooter {
                        IconUrl = houndLogo.Url,
                        Text = $"© Asmodeus Club | Network"
                    }
                };

                await member.GrantRoleAsync(muteRole).ConfigureAwait(false);

                await botUseageCh.SendMessageAsync(embed: muteEmbed).ConfigureAwait(false);

                return;

            }

        }

        [Command("unmute")]
        public async Task UnmuteCmd(CommandContext ctx, DiscordMember member) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "*" || x.Name.ToLower() == "owner");

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

            var muteRole = ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("muted")).Value;

            var botUseageCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("bot-usage")).Value;

            if (member.Roles.Contains(muteRole)) {

                var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

                var muteEmbed = new DiscordEmbedBuilder {
                    Title = $"Infraction | Unmute",
                    Description = $"» User has been unmuted.\n\n**User** » {member.Mention} \n **ID** » {member.Id}",
                    Color = new DiscordColor(0xFF5555),
                    Timestamp = DateTime.Now,
                    Footer = new DiscordEmbedBuilder.EmbedFooter {
                        IconUrl = houndLogo.Url,
                        Text = $"© Asmodeus Club | Network"
                    }
                };

                await member.RevokeRoleAsync(muteRole).ConfigureAwait(false);

                await botUseageCh.SendMessageAsync(embed: muteEmbed).ConfigureAwait(false);

                return;

            }
            else {

                var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

                var muteEmbed = new DiscordEmbedBuilder {
                    Title = $"Infraction | Unmute",
                    Description = $"» This user is not muted.\n\n**User** » {member.Mention} \n **ID** » {member.Id}",
                    Color = new DiscordColor(0xFF5555),
                    Timestamp = DateTime.Now,
                    Footer = new DiscordEmbedBuilder.EmbedFooter {
                        IconUrl = houndLogo.Url,
                        Text = $"© Asmodeus Club | Network"
                    }
                };

                await botUseageCh.SendMessageAsync(embed: muteEmbed).ConfigureAwait(false);

                return;

            }

        }

    }
}
