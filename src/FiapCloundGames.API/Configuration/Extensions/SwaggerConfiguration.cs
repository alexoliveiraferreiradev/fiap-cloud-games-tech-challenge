using FiapCloundGames.API.Filters;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FiapCloundGames.API.Configuration.Extensions
{
    public static class SwaggerConfiguration
    {
        public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FIAP Cloud Games API",
                    Version = "v1",
                    Description = "API para gestão de catálogo de jogos e processamento de pedidos.",
                    Contact = new OpenApiContact
                    {
                        Name = "Alex Oliveira Ferreira",
                        Email = "alexoliveiraferreiradev@gmail.com"
                    }
                });

                options.SchemaFilter<EnumSchemaFilter>();

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT: Bearer {Seu Token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });
            return builder;
        }
    }
}
