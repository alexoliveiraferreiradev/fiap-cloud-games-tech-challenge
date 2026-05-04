using FiapCloundGames.API.Infrastructure.Caching;
using FiapCloundGames.API.Infrastructure.Persistance;
using FiapCloundGames.API.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace FiapCloundGames.API.Configuration.Extensions
{
    public static class ApiConfiguration
    {
        private static string connectionString;
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();
            AddRedisConfiguration(builder); 
            AddDbContextConfig(builder);
            AddControllConfiguration(builder);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            AddJwtBearerConfiguration(builder);
            AddAuthorizationConfiguration(builder);
            builder.Services.AddEndpointsApiExplorer();
            return builder;
        }

        private static void AddRedisConfiguration(WebApplicationBuilder builder)
        {
            var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfiguration>();
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig.Configuration;
                options.InstanceName = redisConfig.InstanceName;
            });
        }
        private static void AddDbContextConfig(WebApplicationBuilder builder)
        {
            connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        private static void AddControllConfiguration(WebApplicationBuilder builder)
        {

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorEntry = context.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new
                        {
                            Campo = e.Key,
                            Mensagem = e.Value.Errors.First().ErrorMessage
                        })
                        .FirstOrDefault();

                    return new BadRequestObjectResult(new
                    {
                        field = errorEntry?.Campo,
                        message = errorEntry?.Mensagem
                    });
                };
            })
             .AddJsonOptions(options =>
             {
                 options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
             }); ;

        }

        private static void AddJwtBearerConfiguration(WebApplicationBuilder builder)
        {
            var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
            builder.Services.Configure<JwtSettings>(jwtSettingsSection);

            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            //builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Emissor,
                    ValidAudience = jwtSettings.ValidoEm
                };
            });
        }

        private static void AddAuthorizationConfiguration(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AcessoGeral", policy =>
                    policy.RequireRole("AdminRole", "JogadorRole"));
            });
        }

    }
}
