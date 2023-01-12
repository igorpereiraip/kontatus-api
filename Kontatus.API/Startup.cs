using AutoMapper;
using Kontatus.API.Configurations;
using Kontatus.Data.Context;
using Kontatus.Helper.Utilitarios;
using GED.API.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Globalization;

namespace Kontatus.API
{
    public class Startup
    {
        //readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KontatusContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlServer(Configuration.GetConnectionString("KontatusDB"), b => { b.MigrationsAssembly("Kontatus.Data"); b.CommandTimeout(600); });
            });

            services.ResolveDependencies(Configuration);
            
            services.AddControllers();
            services.SwaggerConfigure();
            services.ConfigureAuth(Configuration);


            //services.AddApplicationInsightsTelemetry();
            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });


            ConfigureEmail(services);

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kontatus.API", Version = "v1" });
            //});

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperConfig());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("Development");
            }
            else
            {
                app.UseCors();
            }
                
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/docs";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kontatus API V1");
                c.DefaultModelExpandDepth(-1);
                c.DefaultModelsExpandDepth(-1);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            ContextConfig.Configure(app);
        }

        private void ConfigureEmail(IServiceCollection services)
        {
            var configuracoes = Configuration.GetSection("EmailConfiguration");
            var emailSender = new EmailSender(
                host: configuracoes["Host"],
                port: Convert.ToInt32(configuracoes["Port"]),
                from: configuracoes["From"],
                username: configuracoes["Username"],
                password: configuracoes["Password"]);
            services.AddSingleton<IEmailSender>(emailSender);
        }
    }
}
