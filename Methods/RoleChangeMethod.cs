using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace kevintrinh1227.Methods {
    public class RoleChangeMethod {

        public async Task OnRoleChange(DiscordGuild guild, GuildMemberUpdateEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var roleEmbed = new DiscordEmbedBuilder {
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            var rolesB = e.RolesBefore.ToList();

            var rolesA = e.RolesAfter.ToList();

            if (rolesB.Count < 1) {
                return;
            }

            if (rolesB.Count < rolesA.Count) {
                roleEmbed.Title = "Roles Added";

                foreach (var r in rolesB) {
                    if (rolesA.Contains(r)) {
                        rolesA.Remove(r);
                    }
                }

                List<string> updatedList = new List<string>();

                foreach (var a in rolesA) {
                    updatedList.Add(a.Name);
                }

                var allRoles = string.Join(", ", updatedList);

                roleEmbed.Description = $"» Roles were added to {e.Member.Username}#{e.Member.Discriminator} \n\n » Roles Added: {allRoles}";

                await staffLogCh.SendMessageAsync(embed: roleEmbed).ConfigureAwait(false);

            }
            else if (rolesB.Count > rolesA.Count) {
                roleEmbed.Title = "Roles Removed";

                foreach (var r in rolesA) {
                    if (rolesB.Contains(r)) {
                        rolesB.Remove(r);
                    }
                }

                List<string> updatedList = new List<string>();

                foreach (var b in rolesB) {
                    updatedList.Add(b.Name);
                }

                var allRoles = string.Join(", ", updatedList);

                roleEmbed.Description = $"» Roles were removed from {e.Member.Username}#{e.Member.Discriminator} \n\n » Roles Removed: {allRoles}";

                await staffLogCh.SendMessageAsync(embed: roleEmbed).ConfigureAwait(false);

            }

        }

        public async Task OnRoleCreate(DiscordGuild guild, GuildRoleCreateEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var createdEmbed = new DiscordEmbedBuilder {
                Title = "Role Created",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            createdEmbed.Description = $"» {e.Role.Mention} was created.";

            await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

        }

        public async Task OnRoleDelete(DiscordGuild guild, GuildRoleDeleteEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var createdEmbed = new DiscordEmbedBuilder {
                Title = "Role Deleted",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            createdEmbed.Description = $"» {e.Role.Name} was deleted.";

            await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

        }

        public async Task OnRoleModified(DiscordGuild guild, GuildRoleUpdateEventArgs e) {

            var staffLogCh = guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            if (staffLogCh == null) {
                return;
            }

            var houndLogo = guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            var createdEmbed = new DiscordEmbedBuilder {
                Title = "Role Edited",
                Color = new DiscordColor(0xFF5555),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            if (e.RoleBefore.Name != e.RoleAfter.Name && e.RoleBefore.Color.Value != e.RoleAfter.Color.Value) {

                createdEmbed.Description = $"» {e.RoleAfter.Mention} was updated. \n\n » New Name: {e.RoleAfter.Name} \n » Old Name: {e.RoleBefore.Name} \n » New Color: {e.RoleAfter.Color} \n » Old Color: {e.RoleBefore.Color}";

                await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

            }
            else if (e.RoleBefore.Name != e.RoleAfter.Name) {

                createdEmbed.Description = $"» {e.RoleAfter.Name} was updated. \n\n » New name: {e.RoleAfter.Name} \n » Old name: {e.RoleBefore.Mention}";

                await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

            }
            else if (e.RoleBefore.Color.Value != e.RoleAfter.Color.Value) {

                createdEmbed.Description = $"» {e.RoleAfter.Mention} was updated. \n\n » New Color: {e.RoleAfter.Color} \n » Old Color: {e.RoleBefore.Color}";

                await staffLogCh.SendMessageAsync(embed: createdEmbed).ConfigureAwait(false);

            }


        }

    }
}
