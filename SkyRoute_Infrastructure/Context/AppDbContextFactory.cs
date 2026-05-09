using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_Infrastructure;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // Use a dummy connection string or your actual one. 
        // This is only used to generate the migration files, not to run the app.
        optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SkyRouteDb;Trusted_Connection=True;");

        return new AppDbContext(optionsBuilder.Options);
    }
}