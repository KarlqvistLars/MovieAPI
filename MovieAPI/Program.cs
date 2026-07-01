using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MovieAPI.Data;
using MovieAPI.Data.Seed;
using MovieAPI.Interfaces;
using MovieAPI.Login;
using MovieAPI.Services;
using System.Text;

namespace MovieAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Koppla upp mot SQL Server-databasen
            var connectionString = builder.Configuration.GetConnectionString("MovieAPIContext") ?? throw new InvalidOperationException("Connection string 'MovieAPIContext' not found.");

            builder.Services
                .AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MovieAPIContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<MovieAPIContext>(options => options.UseSqlServer(connectionString));
            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);


            builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            // Registrera Interfacen, Service och DbContext för att kunna mocca för xUnit-tester
            builder.Services.AddScoped<IMovieAPIContext, MovieAPIContext>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IActorService, ActorService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option => {
                option.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo {
                    Title = "Movie API",
                    Version = "v1",
                    Description = "API för hantering av filmer, skådespelare och recensioner.",
                    Contact = new OpenApiContact {
                        Name = "Lars Karlqvist",
                        Email = "example@email.se",
                        Url = new Uri("https://www.larskarlqvist.se")
                    },
                    License = new OpenApiLicense {
                        Name = "Apache 2.0",
                        Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                    }
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Skriv in JWT-token"
                });
                option.AddSecurityRequirement(document => new OpenApiSecurityRequirement {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            builder.Services.AddAuthorization();



            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles = ["Admin", "User", "Moderator"];

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
