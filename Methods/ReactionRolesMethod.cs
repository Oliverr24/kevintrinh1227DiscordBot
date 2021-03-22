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

                var bustsEmo = DiscordEmoji.FromName(client, ":busts_in_silhouette:");
                var gearEmo = DiscordEmoji.FromName(client, ":gear:");

                var bustsRole = guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("stafflogs")).Value;
                var gearRole = guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("changelogs")).Value;

                if (message.Reactions.Count > 0) {

                    if (message.Reactions.Any(x => x.Emoji == bustsEmo || x.Emoji == gearEmo)) {

                        if (e.Emoji == bustsEmo) {
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
                    }
                }

            }

        }

    }
}
