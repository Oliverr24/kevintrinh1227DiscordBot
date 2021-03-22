using kevintrinh1227.Models;
using Microsoft.EntityFrameworkCore;

namespace kevintrinh1227.Context {
    public class SqliteContext : DbContext {

        public DbSet<WarnUsers> WarnedUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=/SqliteDB.db");

    }
}
