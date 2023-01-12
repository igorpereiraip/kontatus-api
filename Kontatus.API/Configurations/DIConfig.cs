using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Service;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConsigIntegra.API.Configurations
{
    public static class DIConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            ResolveDomainServicesDependencies(services);
            ResolveServicesDependencies(services);
            ResolveRepositoriesDependencies(services);
            ResolveFiltersDependencies(services);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            return services;
        }
        
        private static void ResolveDomainServicesDependencies(IServiceCollection services)
        {
            services.AddScoped<LoginDomainService>();
        }

        private static void ResolveServicesDependencies(IServiceCollection services)
        {
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILogUsuarioService, LogUsuarioService>();
            services.AddScoped<IPesquisaService, PesquisaService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IArquivoService, ArquivoService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<ISolicitacaoIN100Service, SolicitacaoIN100Service>();
            services.AddScoped<IDadosIN100Service, DadosIN100Service>();
            services.AddScoped<ISolicitacaoOfflineService, SolicitacaoOfflineService>();
            services.AddScoped<ITokenService, TokenService>();
        }

        private static void ResolveRepositoriesDependencies(IServiceCollection services)
        {
            services.AddScoped<IPerfilRepository, PerfilRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILogUsuarioRepository, LogUsuarioRepository>();
            services.AddScoped<IArquivoRepository, ArquivoRepository>();
            services.AddScoped<ISolicitacaoIN100Repository, SolicitacaoIN100Repository>();
            services.AddScoped<IDadosIN100Repository, DadosIN100Repository>();
            services.AddScoped<ISolicitacaoOfflineRepository, SolicitacaoOfflineRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
        }

        private static void ResolveFiltersDependencies(IServiceCollection services)
        {
            services.AddScoped<LogUsuarioDTO>();
        }

    }
}
