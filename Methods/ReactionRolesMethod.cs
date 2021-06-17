using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class ReactionRolesMethod {

        public async Task ApplyReactionRole(DiscordClient client, DiscordGuild guild, DiscordMessage message, DiscordMember member, MessageReactionAddEventArgs e) {

            if (e.User.IsBot) {
                return;
            }

            if (message.Embeds != null) {

                var pinEmo = DiscordEmoji.FromName(client, ":pushpin:");
                var bustsEmo = DiscordEmoji.FromName(client, ":busts_in_silhouette:");
                var gearEmo = DiscordEmoji.FromName(client, ":gear:");
                var gameDieEmo = DiscordEmoji.FromName(client, ":game_die:");
                var chartEmo = DiscordEmoji.FromName(client, ":bar_chart:");

                //Network Announcements, Staff log, Server change log, Event and games, Polls and Suggestions
                var pinRole = guild.Roles.FirstOrDefault(x => x.Value.Id == 854850812961095741).Value;
                var bustsRole = guild.Roles.FirstOrDefault(x => x.Value.Id == 822718895780790283).Value;
                var gearRole = guild.Roles.FirstOrDefault(x => x.Value.Id == 822718844623781888).Value;
                var gameDieRole = guild.Roles.FirstOrDefault(x => x.Value.Id == 854850737154424872).Value;
                var chartRole = guild.Roles.FirstOrDefault(x => x.Value.Id == 854850595733897246).Value;

                if (message.Reactions.Count > 0) {

                    if (message.Reactions.Any(x => x.Emoji == bustsEmo || x.Emoji == gearEmo)) {

                        if (e.Emoji == pinEmo) {
                            await e.Message.DeleteReactionAsync(pinEmo, member).ConfigureAwait(false);
                            if (member.Roles.Contains(pinRole)) {
                                await member.RevokeRoleAsync(pinRole).ConfigureAwait(false);
                            }
                            else {
                                await member.GrantRoleAsync(pinRole).ConfigureAwait(false);
                            }
                        }
                        else if (e.Emoji == bustsEmo) {
                            await e.Message.DeleteReactionAsync(bustsEmo, member).ConfigureAwait(false);
                            if (member.Roles.Contains(bustsRole)) {
                                await member.RevokeRoleAsync(bustsRole).ConfigureAwait(false);
                            }
                            else {
                                await member.GrantRoleAsync(bustsRole).ConfigureAwait(false);
                            }
                        }
                        else if (e.Emoji == gearEmo) {
                            await e.Message.DeleteReactionAsync(gearEmo, member).ConfigureAwait(false);
                            if (member.Roles.Contains(gearRole)) {
                                await member.RevokeRoleAsync(gearRole).ConfigureAwait(false);
                            }
                            else {
                                await member.GrantRoleAsync(gearRole).ConfigureAwait(false);
                            }
                        }
                        else if (e.Emoji == gameDieEmo) {
                            await e.Message.DeleteReactionAsync(gameDieEmo, member).ConfigureAwait(false);
                            if (member.Roles.Contains(gameDieRole)) {
                                await member.RevokeRoleAsync(gameDieRole).ConfigureAwait(false);
                            }
                            else {
                                await member.GrantRoleAsync(gameDieRole).ConfigureAwait(false);
                            }
                        }
                        else if (e.Emoji == chartEmo) {
                            await e.Message.DeleteReactionAsync(chartEmo, member).ConfigureAwait(false);
                            if (member.Roles.Contains(chartRole)) {
                                await member.RevokeRoleAsync(chartRole).ConfigureAwait(false);
                            }
                            else {
                                await member.GrantRoleAsync(chartRole).ConfigureAwait(false);
                            }
                        }
                    }
                }

            }

        }

    }
}
