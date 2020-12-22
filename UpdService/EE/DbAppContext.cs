using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UpdService.Objects;

namespace UpdService.EE
{
    public class DbAppContext : DbContext
    {
        public const string Fn = "apps.db";

        public DbSet<App> Apps { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Revision> Revisions { get; set; }

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddFile("log.txt"); });

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlite("Data Source=" + Fn);

        object o = new object();
        public DbAppContext()
        {
            lock (o)
            {
                var b = Database.EnsureCreated();
            }
        }
    }
}
