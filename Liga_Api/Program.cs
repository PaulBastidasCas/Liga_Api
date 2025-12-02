using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Liga_Api.Data;

namespace Liga_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<Liga_ApiContext>(options =>
            // options.UseNpgsql(builder.Configuration.GetConnectionString("Liga_ApiContext") ?? throw new InvalidOperationException("Connection string 'Liga_ApiContext' not found.")));
            options.UseNpgsql(builder.Configuration.GetConnectionString("Liga_ApiContext.postgresql") ?? throw new InvalidOperationException("Connection string 'Liga_ApiContext' not found."))
            );

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddControllers()
                .AddNewtonsoftJson(
                    options =>
                    options.SerializerSettings.ReferenceLoopHandling
                    = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
