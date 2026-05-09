using Microsoft.EntityFrameworkCore;
using SkyRoute_Infrastructure.Context;

namespace SkyRoute_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //logging
            builder.Services.AddControllers();
            builder.Services.AddOpenApi(); //or swagger
            //JWT
            builder.Services.AddAuthorization();
            //Rate limit
            builder.Services.AddApplicationServices();
            builder.Services.AddCustomCors();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            //app.UseIpRateLimiting();
            app.UseCors("AllowCredentials");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
