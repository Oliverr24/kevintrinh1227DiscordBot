using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class TicketCommands : BaseCommandModule {

        [Command("ticket")]
        public async Task CreateTicket(CommandContext ctx) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var channel = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains($"{ctx.Member.Username.ToLower()}-ticket")).Value;

            var ticketCat = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Type == ChannelType.Category && x.Value.Name.ToLower().Contains("tickets")).Value;

            var supportRole = ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("support team")).Value;

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            if (channel == null) {

                var everyoneBuilder = new DiscordOverwriteBuilder();

                everyoneBuilder.For(ctx.Guild.EveryoneRole);

                everyoneBuilder.Deny(Permissions.AccessChannels);
                everyoneBuilder.Deny(Permissions.SendMessages);

                var everyoneList = new List<DiscordOverwriteBuilder> { everyoneBuilder };

                var tempChannel = await ctx.Guild.CreateChannelAsync($"{ctx.Member.Username}-ticket", ChannelType.Text, ticketCat, null, null, null, everyoneList).ConfigureAwait(false);

                var modBuilder = new DiscordOverwriteBuilder();

                modBuilder.For(ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("support team")).Value);

                modBuilder.Allow(Permissions.AccessChannels);
                modBuilder.Allow(Permissions.SendMessages);

                var modList = new List<DiscordOverwriteBuilder> { modBuilder };

                var ticketOwnerBuilder = new DiscordOverwriteBuilder();

                ticketOwnerBuilder.For(ctx.Member);

                ticketOwnerBuilder.Allow(Permissions.AccessChannels);
                ticketOwnerBuilder.Allow(Permissions.SendMessages);

                var ticketOwnerList = new List<DiscordOverwriteBuilder> { ticketOwnerBuilder };

                await tempChannel.AddOverwriteAsync(ctx.Guild.Roles.FirstOrDefault(x => x.Value.Name.ToLower().Contains("*")).Value, modBuilder.Allowed, modBuilder.Denied);
                await tempChannel.AddOverwriteAsync(ctx.Member, ticketOwnerBuilder.Allowed, ticketOwnerBuilder.Denied);

                var ticketEmbed = new DiscordEmbedBuilder {
                    Title = $"Asmodeus Club | Ticket",
                    Description = $"» You have made a support ticket.\n\n**User** » {ctx.Member.Mention} \n **Location** » {tempChannel.Mention} \n\n» Please click on the channel location and follow\n» the example ticket format in #support channel. \n» A {supportRole.Mention} member will assist you shortly.",
                    Color = new DiscordColor(0xFF5555),
                    Timestamp = DateTime.Now,
                    Footer = new DiscordEmbedBuilder.EmbedFooter {
                        IconUrl = houndLogo.Url,
                        Text = $"© Asmodeus Club | Network"
                    }
                };

                await ctx.RespondAsync(embed: ticketEmbed).ConfigureAwait(false);

            }
            else {

                if (channel.Parent.Name.ToLower().Contains("ticket")) {

                    var ticketEmbed = new DiscordEmbedBuilder {
                        Title = $"Asmodeus Club | Ticket",
                        Description = $"» You already have an open support ticket.\n\n**User** » {ctx.Member.Mention} \n **Location** » {channel.Mention} \n\n» Please click on the channel location and follow\n» the example ticket format in #support channel. \n» A {supportRole.Mention} member will assist you shortly.",
                        Color = new DiscordColor(0xFF5555),
                        Timestamp = DateTime.Now,
                        Footer = new DiscordEmbedBuilder.EmbedFooter {
                            IconUrl = houndLogo.Url,
                            Text = $"© Asmodeus Club | Network"
                        }
                    };

                    await ctx.RespondAsync(embed: ticketEmbed).ConfigureAwait(false);
                    return;

                }

            }


        }

        [Command("closeticket")]
        public async Task CloseTicketChannel(CommandContext ctx) {

            if (ctx.Guild == null || ctx.Channel == null) {
                return;
            }

            await ctx.Message.DeleteAsync().ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "*" || x.Name.ToLower() == "support team");

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

            var staffLogCh = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains("staff-log")).Value;

            if (ctx.Channel != null) {

                if (ctx.Channel.Parent.Name.ToLower().Contains("ticket")) {

                    var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

                    var ticketCloseEmbed = new DiscordEmbedBuilder {
                        Title = $"Asmodeus Club | Ticket Closed",
                        Description = $"»  Ticket has been closed.\n\n**User** » {ctx.Member.Mention} \n **Ticket Name** » {ctx.Channel.Name}",
                        Color = new DiscordColor(0xFF5555),
                        Timestamp = DateTime.Now,
                        Footer = new DiscordEmbedBuilder.EmbedFooter {
                            IconUrl = houndLogo.Url,
                            Text = $"© Asmodeus Club | Network"
                        }
                    };

                    await staffLogCh.SendMessageAsync(embed: ticketCloseEmbed).ConfigureAwait(false);

                    await ctx.Channel.DeleteAsync().ConfigureAwait(false);

                    return;

                }
                else {

                    var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

                    var ticketCloseEmbed = new DiscordEmbedBuilder {
                        Title = $"Asmodeus Club | Ticket Closed",
                        Description = $"» This is not a ticket, it cannot be closed..\n\n**User** » {ctx.Member.Mention} \n **Ticket Name** » {ctx.Channel.Mention}",
                        Color = new DiscordColor(0xFF5555),
                        Timestamp = DateTime.Now,
                        Footer = new DiscordEmbedBuilder.EmbedFooter {
                            IconUrl = houndLogo.Url,
                            Text = $"© Asmodeus Club | Network"
                        }
                    };

                    await ctx.RespondAsync(embed: ticketCloseEmbed).ConfigureAwait(false);

                    return;

                }

            }


        }


    }
}
