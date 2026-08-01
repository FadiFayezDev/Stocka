using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Contexts
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            // SQLite file وهمي للتصميم / Migrations
            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "armoryx_migrations.db");
            var sqlite = $"Data Source={dbPath}";
            var postgreSql = "Host=ep-bold-forest-a9ugoads-pooler.gwc.azure.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_HPQDd9gf3rAc; SSL Mode=VerifyFull; Channel Binding=Require;";
            var sqlServer = "Data Source=black-terminal.database.windows.net;Initial Catalog=stocka;User ID=fadyfayez.net@gmail.com;Connect Timeout=60;Encrypt=True;Trust Server Certificate=False;Authentication=ActiveDirectoryInteractive;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";

            optionsBuilder.UseNpgsql(postgreSql)
                .UseSnakeCaseNamingConvention();

            return new AppDbContext(optionsBuilder.Options);
        }
    }

}
