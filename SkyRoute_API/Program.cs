using AspNetCoreRateLimit;
using Microsoft.EntityFrameworkCore;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLogging(logging => logging.AddConsole());
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddAuthorization();
            builder.Services.AddRateLimiting();
            builder.Services.AddApplicationServices();
            builder.Services.AddCustomCors();
            builder.Services.AddPooledDbContextFactory<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHttpClient();
            builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStopping.Register(async () => {await Task.CompletedTask;});
            app.UseHttpsRedirection();
            app.UseIpRateLimiting();
            app.UseCors("AllowCredentials");
            app.UseCors("AngularDevPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}