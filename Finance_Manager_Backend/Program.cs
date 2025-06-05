using Finance_Manager_Backend.BusinessLogic.Models;
using Finance_Manager_Backend.BusinessLogic.Services;
using Finance_Manager_Backend.BusinessLogic.Services.AuthServices;
using Finance_Manager_Backend.DataBase;
using Finance_Manager_Backend.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Reflection;
using System.Text;
using FluentValidation;
using System.Globalization;
// using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

public class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();

        var cultureInfo = new CultureInfo("en-GB");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

        // DI
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddHttpClient<CurrencyConverterService>();

        builder.Services.AddHostedService<RefreshTokenCleanupService>();

        builder.Services.AddTransient<TokenGenerator>();

        builder.Services.AddScoped<DbTransactionTemplate>();

        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<GoogleAuthService>();
        builder.Services.AddScoped<TransactionsService>();
        builder.Services.AddScoped<SavingsService>();
        builder.Services.AddScoped<AnalyticsService>();
        builder.Services.AddScoped<UsersService>();
        builder.Services.AddScoped<CategoriesService>();

        builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
            // options.SuppressModelStateInvalidFilter = true;
        })
        .ConfigureApiBehaviorOptions(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        })
        .AddNewtonsoftJson();
        // Auto Validation without async filter:
        /*builder.Services.AddControllers();
        builder.Services.AddFluentValidationAutoValidation();*/
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.EnableAnnotations();
            options.SchemaFilter<JsonRequiredSchemaFilter>();
            options.UseAllOfToExtendReferenceSchemas();

            // Jwt token support
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter JWT token in format: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        // JwtAuthentication
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            });

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"))
            .AddPolicy("UserPolicy", policy => policy.RequireRole("User", "Admin"));

        // Cache
        builder.Services.AddMemoryCache();

        // Logger
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        builder.Host.UseSerilog();

        /*builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.Listen(System.Net.IPAddress.Any, 18090);       // http://0.0.0.0:18090
        });*/

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            DataSeeder.ClearDb(app.Services).Wait();
            DataSeeder.SeedData(app.Services).Wait();
            // Delete later
            DataSeeder.SeedAdmin(app.Services, app.Configuration).Wait();
        }
        else if (app.Environment.IsProduction())
        {
            DataSeeder.SeedAdmin(app.Services, app.Configuration).Wait();
        }

        // Middleware
        app.UseRouting();
        app.UseMiddleware<ExceptionsHandler>();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();

        Console.WriteLine("It's all good man.");
    }
}
