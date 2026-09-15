using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using VgAutoDrill.Admin.WebApi.Middlewares;

namespace VgAutoDrill.Admin.WebApi.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VgAutoDrill.Admin Project",
                    Description = "VgAutoDrill.Admin.WebApi Swagger",
                    Contact = new OpenApiContact { Name = "txc xmq", Email = "asp.net@live.com" }
                });

                var basePath = AppContext.BaseDirectory;
                var webApiXmlPath = Path.Combine(basePath, "VgAutoDrill.Admin.WebApi.xml");
                s.IncludeXmlComments(webApiXmlPath, true);

                var webApiModelXmlPath = Path.Combine(basePath, "VgAutoDrill.Admin.Model.xml");
                s.IncludeXmlComments(webApiModelXmlPath, true);

                var applicationXmlPath = Path.Combine(basePath, "VgAutoDrill.Admin.Application.xml");
                s.IncludeXmlComments(applicationXmlPath);

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                s.SchemaFilter<EnumSchemaFilter>();

            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
