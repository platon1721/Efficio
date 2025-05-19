using Asp.Versioning.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace Efficio.API.Configurations
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // Hangi versioonid
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
        
                // Lisa dokument iga versiooni jaoks
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(
                        description.GroupName,
                        new OpenApiInfo
                        {
                            Title = "Efficio API",
                            Version = description.ApiVersion.ToString()
                        });
                }
                
                // c.SwaggerDoc("v1", new OpenApiInfo { Title = "Efficio API", Version = "v1" });
                
                // Configure Swagger to use JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] {}
                    }
                });
            });

            return services;
        }
    }
}