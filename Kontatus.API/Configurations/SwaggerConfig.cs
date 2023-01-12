using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace GED.API.Configurations
{
    public static class SwaggerConfig
    {
        public static void SwaggerConfigure(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GED API", Version = "v1" });
                
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, "GED.API.xml");
                
                //c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "Autorização JWT usa-se Bearer token no cabeçalho." +
                            " \r\n\r\n Entre com 'Bearer' [espaço] e o token do seu usuario." +
                            "\r\n\r\nExemplo: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                        new List<string> ()
                    }
                });
            });
        }
    }
}
