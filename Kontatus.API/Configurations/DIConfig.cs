using Kontatus.Data.Repository;
using Kontatus.Domain.DTO;
using Kontatus.Service;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kontatus.API.Configurations
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
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IArquivoService, ArquivoService>();
            services.AddScoped<IBlobStorageService, BlobStorageService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPessoaService, PessoaService>();
            services.AddScoped<ITelefoneService, TelefoneService>();
            services.AddScoped<IEnderecoService, EnderecoService>();
        }

        private static void ResolveRepositoriesDependencies(IServiceCollection services)
        {
            services.AddScoped<IPerfilRepository, PerfilRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<ILogUsuarioRepository, LogUsuarioRepository>();
            services.AddScoped<IArquivoRepository, ArquivoRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ITelefoneRepository, TelefoneRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        }

        private static void ResolveFiltersDependencies(IServiceCollection services)
        {
            services.AddScoped<LogUsuarioDTO>();
        }

    }
}
