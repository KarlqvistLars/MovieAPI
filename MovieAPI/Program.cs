using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Data.Seed;
using MovieAPI.Interfaces;
using MovieAPI.Services;

namespace MovieAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Koppla upp mot SQL Server-databasen
            var connectionString = builder.Configuration.GetConnectionString("MovieAPIContext") ?? throw new InvalidOperationException("Connection string 'MovieAPIContext' not found.");
            builder.Services.AddDbContext<MovieAPIContext>(options => options.UseSqlServer(connectionString));
            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            // Registrera Interfacen, Service och DbContext för att kunna mocca för xUnit-tester
            builder.Services.AddScoped<IMovieAPIContext, MovieAPIContext>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IActorService, ActorService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }
            // Seed-logik
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<MovieAPIContext>();
                try
                {
                    // Säkerställer att databasen finns och är migrerad
                    context.Database.Migrate();
                } catch
                {
                    Console.WriteLine("Något fel uppstod vid kontroll ifall databasen existerar. \nSe till att SQL Server är igång och att anslutningssträngen är korrekt.");
                }
                // Anropar din seed-metod
                DbSeeder.Initialize(context);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
