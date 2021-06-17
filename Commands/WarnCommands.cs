using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using kevintrinh1227.Context;
using kevintrinh1227.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class WarnCommands : BaseCommandModule {

        [Command("warn")]
        public async Task WarnMember(CommandContext ctx, DiscordMember member, [RemainingText] string reason) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Id == 826660041498558464);

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

            var staffLogCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            using SqliteContext cont = new SqliteContext();

            if (cont == null) {

                await ctx.RespondAsync($"{ctx.Member.Mention} there was an issue with the database.").ConfigureAwait(false);

                return;
            }

            var newWarn = new WarnUsers();

            if (cont.WarnedUsers.Count() < 1) {
                newWarn.warningNumber = 1;
                newWarn.memberId = member.Id;
                newWarn.reason = reason;
            }
            else {
                var lastWarning = cont.WarnedUsers.OrderByDescending(x => x.warningNumber).Select(r => r.warningNumber).FirstOrDefault();
                newWarn.warningNumber = lastWarning + 1;
                newWarn.memberId = member.Id;
                newWarn.reason = reason;
            }

            cont.WarnedUsers.Add(newWarn);

            await cont.SaveChangesAsync().ConfigureAwait(false);

            var warnEmbed = new DiscordEmbedBuilder {
                Title = $"Infraction | Warning issued",
                Description = $"» A warning has been issued.\n\n**User** » {member.Mention} \n **ID** » {member.Id} \n\n **Reason** » {reason}",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            await staffLogCh.SendMessageAsync(embed: warnEmbed).ConfigureAwait(false);

        }

        [Command("delwarn")]
        public async Task DeleteWarning(CommandContext ctx, int warnNumber) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Id == 826660041498558464);

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

            var staffLogCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            using SqliteContext cont = new SqliteContext();

            if (cont == null) {

                await ctx.RespondAsync($"{ctx.Member.Mention} there was an issue with the database.").ConfigureAwait(false);

                return;
            }

            foreach (var warning in cont.WarnedUsers) {
                if (warning.warningNumber == warnNumber) {

                    var member = ctx.Guild.Members.FirstOrDefault(x => x.Value.Id == warning.memberId).Value;

                    if (member != null) {

                        var warnEmbed = new DiscordEmbedBuilder {
                            Title = $"Infraction | Warning Removed",
                            Description = $"» Warning has been removed.\n\n**User** » {member.Mention}",
                            Color = new DiscordColor(0xFF0000),
                            Timestamp = DateTime.Now,
                            Footer = new DiscordEmbedBuilder.EmbedFooter {
                                IconUrl = houndLogo.Url,
                                Text = $"© Asmodeus Club | Network"
                            }
                        };

                        await staffLogCh.SendMessageAsync(embed: warnEmbed).ConfigureAwait(false);

                    }
                    else {

                        var warnEmbed = new DiscordEmbedBuilder {
                            Title = $"Infraction | Warning issued",
                            Description = $"» Warning has been removed from a member no longer within the community. \n\n**ID** » {member.Id}",
                            Color = new DiscordColor(0xFF0000),
                            Timestamp = DateTime.Now,
                            Footer = new DiscordEmbedBuilder.EmbedFooter {
                                IconUrl = houndLogo.Url,
                                Text = $"© Asmodeus Club | Network"
                            }
                        };

                        await staffLogCh.SendMessageAsync(embed: warnEmbed).ConfigureAwait(false);

                    }

                    cont.WarnedUsers.Remove(warning);

                    await cont.SaveChangesAsync().ConfigureAwait(false);

                    return;

                }
            }

        }

        [Command("warnings")]
        public async Task GetWarnings(CommandContext ctx, DiscordMember member) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Id == 826660041498558464);

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

            var staffLogCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-logs")).Value;

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            using SqliteContext cont = new SqliteContext();

            if (cont == null) {

                await ctx.RespondAsync($"{ctx.Member.Mention} there was an issue with the database.").ConfigureAwait(false);

                return;
            }

            var warningsEmbed = new DiscordEmbedBuilder {
                Title = $"Infractions | Warning List",
                Description = $"» List of all warnings for " +
                $"{member.Username}#{member.Discriminator}",
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.Now,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            int count = 0;

            foreach (var warning in cont.WarnedUsers) {

                if (warning.memberId == member.Id) {

                    count++;

                    warningsEmbed.AddField($"Warning {count}", $"Warning ID: {warning.warningNumber}\n{warning.reason}");

                }

            }

            await staffLogCh.SendMessageAsync(embed: warningsEmbed).ConfigureAwait(false);

        }


    }
}
