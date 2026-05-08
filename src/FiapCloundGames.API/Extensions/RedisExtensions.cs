using FiapCloudGames.Infrastructure.Caching;
using StackExchange.Redis;

namespace FiapCloudGames.API.Extensions
{
    public static class RedisExtensions
    {
        public static WebApplicationBuilder AddRedis(this WebApplicationBuilder builder)
        {
            var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisOptions>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(redisConfig.Configuration, true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfig.Configuration;
                options.InstanceName = redisConfig.InstanceName;
            });

            return builder;
        }
    }
}
