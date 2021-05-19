using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class AnnounementCommand : BaseCommandModule {

        [Command("announce")]
        [Aliases("announcement")]
        [Description("Send an announcement out")]
        public async Task AnnounceMessage(CommandContext ctx) {

            if (ctx.Guild == null) {

                return;
            }

            await ctx.Channel.DeleteMessageAsync(ctx.Message).ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower() == "bot.use");

            if (!roleCheck) {
                var failEmbed = new DiscordEmbedBuilder {
                    Title = "Invalid Permissions",
                    Description = "You do not have permission to use this command!",
                    Color = DiscordColor.DarkRed,
                };

                try {
                    await ctx.Member.SendMessageAsync(embed: failEmbed).ConfigureAwait(false);
                }
                catch {

                }

                return;
            }

            var interactivity = ctx.Client.GetInteractivity();

            List<DiscordMessage> delMsg = new List<DiscordMessage>();

            var houndLogo = ctx.Guild.Emojis.FirstOrDefault(x => x.Value.Name.ToLower() == "houndslogo").Value;

            string title = string.Empty;
            string description = string.Empty;
            string picUrl = string.Empty;
            DiscordRole role;

            var titleMessage = await ctx.Channel.SendMessageAsync("Please type out the \'Title\'.").ConfigureAwait(false);

            delMsg.Add(titleMessage);

            var titleResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel == ctx.Channel).ConfigureAwait(false);

            delMsg.Add(titleResponse.Result);

            if (titleResponse.Result.Content.StartsWith("!cancel")) {
                await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);
                return;
            }

            title = titleResponse.Result.Content;

            var descriptionMessage = await ctx.Channel.SendMessageAsync("Please type out your \'Description\'.").ConfigureAwait(false);

            delMsg.Add(descriptionMessage);

            var descriptionResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel == ctx.Channel).ConfigureAwait(false);

            delMsg.Add(descriptionResponse.Result);

            if (descriptionResponse.Result.Content.StartsWith("!cancel")) {
                await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);
                return;
            }

            description = descriptionResponse.Result.Content;

            var urlMessage = await ctx.Channel.SendMessageAsync("Please give us the URL to the image you would like included. If you do not wish for one type NA").ConfigureAwait(false);

            delMsg.Add(urlMessage);

            var urlResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel == ctx.Channel).ConfigureAwait(false);

            delMsg.Add(urlResponse.Result);

            var sendEmbed = new DiscordEmbedBuilder {
                Title = title,
                Description = description,
                Color = new DiscordColor(0xFF0000),
                Timestamp = DateTime.UtcNow,
                Footer = new DiscordEmbedBuilder.EmbedFooter {
                    IconUrl = houndLogo.Url,
                    Text = $"© Asmodeus Club | Network"
                }
            };

            try {
                if (urlResponse.Result.Content.ToLower().Contains("NA")) {

                } else {
                    sendEmbed.WithImageUrl(urlResponse.Result.Content.Trim());
                }

            }
            catch {

            }

            var channelMessage = await ctx.Channel.SendMessageAsync("Please choose the \'Channel\' to post to.").ConfigureAwait(false);

            delMsg.Add(channelMessage);

            var channelResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel == ctx.Channel).ConfigureAwait(false);

            delMsg.Add(channelResponse.Result);

            if (channelResponse.Result.Content.StartsWith("!cancel")) {
                await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);
                return;
            }

            var roleMessage = await ctx.Channel.SendMessageAsync("Please choose the \'Role\' you wish to @. Type *NA* to not @ a role.").ConfigureAwait(false);

            delMsg.Add(roleMessage);

            var roleResponse = await interactivity.WaitForMessageAsync(x => x.Author.Id == ctx.User.Id && x.Channel == ctx.Channel).ConfigureAwait(false);

            delMsg.Add(roleResponse.Result);

            try {

                if (channelResponse.Result.MentionedChannels.Count == 0) {
                    var chan = ctx.Guild.Channels.FirstOrDefault(x => x.Value.Name.ToLower().Contains(channelResponse.Result.Content.ToLower().ToString().Trim())).Value;

                    if (roleResponse.Result.MentionedRoles.Count == 0 || roleResponse.Result.Content.ToLower().Trim().Contains("na")) {

                        var noRole = await ctx.Channel.SendMessageAsync("Role was not found or NA").ConfigureAwait(false);

                        delMsg.Add(noRole);

                    }
                    else {

                        if (roleResponse.Result.MentionedRoles[0] != null) {

                            role = roleResponse.Result.MentionedRoles[0];

                            await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                            await chan.SendMessageAsync(role.Mention, embed: sendEmbed).ConfigureAwait(false);

                            return;

                        }

                    }

                    await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                    if (chan == null) {
                        await ctx.Channel.SendMessageAsync(embed: sendEmbed).ConfigureAwait(false);
                    }

                    return;
                }
                else {

                    var chan = channelResponse.Result.MentionedChannels[0];

                    if (roleResponse.Result.MentionedRoles.Count == 0 || roleResponse.Result.Content.ToLower().Trim().Contains("na")) {

                        if (roleResponse.Result.Content.ToLower().Contains("@everyone")) {

                            await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                            await chan.SendMessageAsync("@everyone", embed: sendEmbed).ConfigureAwait(false);

                        }
                        else if (roleResponse.Result.Content.ToLower().Contains("@here")) {

                            await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                            await chan.SendMessageAsync("@here", embed: sendEmbed).ConfigureAwait(false);

                        }
                        else {

                            var noRole = await ctx.Channel.SendMessageAsync("Role was not found or NA").ConfigureAwait(false);

                            delMsg.Add(noRole);

                            await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                            await chan.SendMessageAsync(embed: sendEmbed).ConfigureAwait(false);

                        }

                    }
                    else {

                        if (roleResponse.Result.MentionedRoles[0] != null) {

                            role = roleResponse.Result.MentionedRoles[0];

                            await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                            if (roleResponse.Result.Content.ToLower().Contains("@everyone")) {

                                await chan.SendMessageAsync("@everyone", embed: sendEmbed).ConfigureAwait(false);

                            }
                            else if (roleResponse.Result.Content.ToLower().Contains("@here")) {

                                await chan.SendMessageAsync("@here", embed: sendEmbed).ConfigureAwait(false);

                            }
                            else {

                                await chan.SendMessageAsync(role.Mention, embed: sendEmbed).ConfigureAwait(false);

                            }

                            return;

                        }

                    }

                    return;
                }


            }
            catch {
                var nullChannelMessage = await ctx.Channel.SendMessageAsync("Channel does not exist, defaulted to current channel.").ConfigureAwait(false);

                delMsg.Add(nullChannelMessage);

                if (roleResponse.Result.MentionedRoles.Count == 0 || roleResponse.Result.Content.ToLower().Trim().Contains("na")) {

                    var noRole = await ctx.Channel.SendMessageAsync("Role was not found or NA").ConfigureAwait(false);

                    delMsg.Add(noRole);

                }
                else {

                    if (roleResponse.Result.MentionedRoles[0] != null) {

                        role = roleResponse.Result.MentionedRoles[0];

                        await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                        if (roleResponse.Result.Content.ToLower().Contains("@everyone")) {

                            await ctx.Channel.SendMessageAsync("@everyone", embed: sendEmbed).ConfigureAwait(false);

                        }
                        else if (roleResponse.Result.Content.ToLower().Contains("@here")) {

                            await ctx.Channel.SendMessageAsync("@here", embed: sendEmbed).ConfigureAwait(false);

                        }
                        else {

                            await ctx.Channel.SendMessageAsync(role.Mention, embed: sendEmbed).ConfigureAwait(false);

                        }

                        return;

                    }

                }

                await ctx.Channel.DeleteMessagesAsync(delMsg).ConfigureAwait(false);

                await ctx.Channel.SendMessageAsync(embed: sendEmbed).ConfigureAwait(false);

                return;
            }
        }


    }
}
