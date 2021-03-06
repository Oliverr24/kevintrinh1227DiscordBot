using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using kevintrinh1227.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace kevintrinh1227.Commands {
    public class MigrateCommand : BaseCommandModule {

        [Command("migratelite")]
        public async Task MigrateLite(CommandContext ctx) {

            await ctx.Channel.DeleteMessageAsync(ctx.Message).ConfigureAwait(false);

            var roleCheck = ctx.Member.Roles.Any(x => x.Name.ToLower().Contains("owner"));


            if (!roleCheck) {
                return;
            }

            await ctx.RespondAsync("*** migrating... ***").ConfigureAwait(false);

            await using SqliteContext lite = new SqliteContext();

            if (lite.Database.GetPendingMigrationsAsync().Result.Any()) {
                await lite.Database.MigrateAsync();
            }

            await ctx.RespondAsync("Sqlite Migration Compelte").ConfigureAwait(false);

        }

    }
}
