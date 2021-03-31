using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class ApplyCommand : BaseCommandModule {

        [Command("apply")]
        public async Task ApplyCmd(CommandContext ctx, [RemainingText] string applicationName) {

            if (ctx.Guild == null) {

                return;
            }

            await ctx.Channel.DeleteMessageAsync(ctx.Message).ConfigureAwait(false);

            if(applicationName.ToLower() == "builder") {

                await ctx.RespondAsync("Builder Application Link:  https://asmodeus.club/builder-application").ConfigureAwait(false);

            } else if (applicationName.ToLower() == "staff") {

                await ctx.RespondAsync("Staff Application Link: https://asmodeus.club/staff-application").ConfigureAwait(false);

            } else {

                await ctx.RespondAsync("There is no application recognized. Please do `.apply builder` or `.apply staff`.").ConfigureAwait(false);

            }

        }


    }
}
