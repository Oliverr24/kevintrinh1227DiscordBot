using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using kevintrinh1227.Commands;
using kevintrinh1227.Methods;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kevintrinh1227 {
    public class Bot {

        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }

        public static Dictionary<ulong, DateTime> memberCountRefresh = new Dictionary<ulong, DateTime>();

        public async Task RunAsync() {

            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            //Config setup
            var config = new DiscordConfiguration {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All
            };

            //Client setup for events/methods
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.GuildAvailable += OnGuildAvailable;
            Client.GuildMemberAdded += OnMemberJoin;
            Client.MessageReactionAdded += OnReactionAdded;
            Client.Heartbeated += OnHeartBeat;

            //Setup for interactivity
            Client.UseInteractivity(new InteractivityConfiguration {
                Timeout = TimeSpan.FromMinutes(10)
            });

            //Setup for commands
            var commandsConfig = new CommandsNextConfiguration {

                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true,
                DmHelp = false,

            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<AnnounementCommand>();
            Commands.RegisterCommands<TicketCommands>();
            Commands.RegisterCommands<PurgeCommand>();
            Commands.RegisterCommands<MigrateCommand>();
            Commands.RegisterCommands<WarnCommands>();
            Commands.RegisterCommands<MuteCommands>();
            Commands.RegisterCommands<BanCommand>();
            Commands.RegisterCommands<UnbanCommand>();
            Commands.RegisterCommands<KickCommand>();
            Commands.RegisterCommands<InspectCommand>();
            Commands.RegisterCommands<PostReactionCommand>();

            //Connect the bot to Discord
            await Client.ConnectAsync();

            //Keep the bot running
            await Task.Delay(-1);

        }

        //Client ready event
        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e) {
            return Task.CompletedTask;
        }

        //GuildAvailableEvent
        private async Task OnGuildAvailable(DiscordClient sender, GuildCreateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var status = new DiscordActivity(type: ActivityType.Playing, name: $"play.asmodeus.club");

            await sender.UpdateStatusAsync(status).ConfigureAwait(false);

        }

        //Heartbeat event
        private async Task OnHeartBeat(DiscordClient sender, HeartbeatEventArgs e) {

            if (sender.Guilds.Count == 0) {
                return;
            }

            foreach (var g in sender.Guilds.Values) {

                var allMembers = g.MemberCount;

                var memberCountChannel = g.Channels.FirstOrDefault(x => x.Value.Name.StartsWith("Members:")).Value;

                if (memberCountChannel == null) {
                    continue;
                }

                if (memberCountRefresh.Count > 0) {
                    if (memberCountRefresh.ContainsKey(memberCountChannel.Id)) {
                        if (DateTime.Compare(DateTime.Now, memberCountRefresh[memberCountChannel.Id]) == 1) {
                            memberCountRefresh.Remove(memberCountChannel.Id);

                            await memberCountChannel.ModifyAsync(x => x.Name = $"Members: {allMembers}").ConfigureAwait(false);

                            memberCountRefresh.Add(memberCountChannel.Id, DateTime.Now.AddMinutes(5));
                        }
                    }
                }
                else {
                    await memberCountChannel.ModifyAsync(x => x.Name = $"Members: {allMembers}").ConfigureAwait(false);

                    memberCountRefresh.Add(memberCountChannel.Id, DateTime.Now.AddMinutes(5));
                }

            }

        }

        //Member join event
        private async Task OnMemberJoin(DiscordClient sender, GuildMemberAddEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var welcomeMessageInstance = new WelcomeMethod();

            _ = Task.Run(() => welcomeMessageInstance.WelcomeSendMethod(sender, e.Guild, e.Member));

        }

        //Reaction Added Event
        private async Task OnReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var reactionMessageInstance = new ReactionRolesMethod();

            _ = Task.Run(() => reactionMessageInstance.ApplyReactionRole(sender, e.Guild, e.Message, (DiscordMember) e.User, e));

        }


    }
}
