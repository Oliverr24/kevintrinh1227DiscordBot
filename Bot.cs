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
            Client.MessageCreated += OnMessageSent;
            Client.GuildMemberRemoved += OnMemberLeave;
            Client.ChannelCreated += OnChannelCreated;
            Client.ChannelDeleted += OnChannelDeleted;
            Client.ChannelUpdated += OnChannelUpdated;
            Client.GuildMemberUpdated += OnMemberUpdated;
            Client.GuildRoleCreated += OnRoleCreated;
            Client.GuildRoleDeleted += OnRoleDeleted;
            Client.GuildRoleUpdated += OnRoleUpdated;
            Client.MessageDeleted += OnMessageDeleted;
            Client.MessagesBulkDeleted += OnMessageBulkDeleted;
            Client.MessageUpdated += OnMessageUpdated;

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
            Commands.RegisterCommands<ApplyCommand>();

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
            Console.WriteLine("event");
            var welcomeMessageInstance = new WelcomeMethod();

            _ = Task.Run(() => welcomeMessageInstance.WelcomeSendMethod(sender, e.Guild, e.Member));
            Console.WriteLine("sent");
        }

        //Member leave event
        private async Task OnMemberLeave(DiscordClient sender, GuildMemberRemoveEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var memberLeaveInstance = new MemberLeavingMethod();

            _ = Task.Run(() => memberLeaveInstance.OnMemberLeave(e.Guild, e.Member));

            await Task.CompletedTask;

        }

        //Reaction Added Event
        private async Task OnReactionAdded(DiscordClient sender, MessageReactionAddEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var reactionMessageInstance = new ReactionRolesMethod();

            _ = Task.Run(() => reactionMessageInstance.ApplyReactionRole(sender, e.Guild, e.Message, (DiscordMember)e.User, e));

        }

        //Messages sent event
        private async Task OnMessageSent(DiscordClient sender, MessageCreateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var bannedMessageInstance = new BannedWordMethod();

            _ = Task.Run(() => bannedMessageInstance.OnBannedWords(e.Guild, e.Message));

            await Task.CompletedTask;
        }

        //Channel Created
        private async Task OnChannelCreated(DiscordClient sender, ChannelCreateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var createdChInstance = new ChannelModificationMethod();

            _ = Task.Run(() => createdChInstance.OnChannelCreate(e.Guild, e));

            await Task.CompletedTask;
        }

        //Channel Deleted
        private async Task OnChannelDeleted(DiscordClient sender, ChannelDeleteEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var deletedChInstance = new ChannelModificationMethod();

            _ = Task.Run(() => deletedChInstance.OnChannelDelete(e.Guild, e));

            await Task.CompletedTask;
        }

        //Channel Edited
        private async Task OnChannelUpdated(DiscordClient sender, ChannelUpdateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var editedChInstance = new ChannelModificationMethod();

            _ = Task.Run(() => editedChInstance.OnChannelModified(e.Guild, e));

            await Task.CompletedTask;
        }

        private async Task OnMemberUpdated(DiscordClient sender, GuildMemberUpdateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var roleChangeInstance = new RoleChangeMethod();

            _ = Task.Run(() => roleChangeInstance.OnRoleChange(e.Guild, e));

            await Task.CompletedTask;
        }

        private async Task OnRoleCreated(DiscordClient sender, GuildRoleCreateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var roleCreateInstance = new RoleChangeMethod();

            _ = Task.Run(() => roleCreateInstance.OnRoleCreate(e.Guild, e));

            await Task.CompletedTask;
        }
        
        private async Task OnRoleDeleted(DiscordClient sender, GuildRoleDeleteEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var roleDeleteInstance = new RoleChangeMethod();

            _ = Task.Run(() => roleDeleteInstance.OnRoleDelete(e.Guild, e));

            await Task.CompletedTask;
        }
        
        private async Task OnRoleUpdated(DiscordClient sender, GuildRoleUpdateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var roleUpdatedInstance = new RoleChangeMethod();

            _ = Task.Run(() => roleUpdatedInstance.OnRoleModified(e.Guild, e));

            await Task.CompletedTask;
        }

        private async Task OnMessageDeleted(DiscordClient sender, MessageDeleteEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var messageDeleteInstance = new DeletedMessagesMethod();

            _ = Task.Run(() => messageDeleteInstance.OnMsgDelete(e.Guild, e));

            await Task.CompletedTask;
        }

        private async Task OnMessageBulkDeleted(DiscordClient sender, MessageBulkDeleteEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var messageBulkDeleteInstance = new DeletedMessagesMethod();

            _ = Task.Run(() => messageBulkDeleteInstance.OnMsgBulkDelete(e.Guild, e));

            await Task.CompletedTask;
        }

        private async Task OnMessageUpdated(DiscordClient sender, MessageUpdateEventArgs e) {

            if (e.Guild == null) {
                return;
            }

            var messageUpdatedInstance = new DeletedMessagesMethod();

            _ = Task.Run(() => messageUpdatedInstance.OnMsgUpdated(e.Guild, e));

            await Task.CompletedTask;
        }


    }
}
