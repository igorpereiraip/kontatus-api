using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Helper.Utilitarios;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
{
    public interface IEmailService
    {
        Task EnviarEmailNovoUsuario(string nome, string email, string senha);
    }

    public class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly ILogUsuarioService _logUsuarioService;

        public EmailService(IEmailSender emailSender, IConfiguration config, ILogUsuarioService logUsuarioService)
        {
            _emailSender = emailSender;
            _config = config;
            _logUsuarioService = logUsuarioService;
        }

        public async Task EnviarEmailNovoUsuario(string nome, string email, string senha)
        {
            var enderecoBlob = _config.GetSection("GeralConfiguration")["BlobStorage"];

            var webRequest = WebRequest.Create(enderecoBlob + @"templates/novo-usuario.html");

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            {
                var body = reader.ReadToEnd();

                body = body.Replace("{{nome}}", nome);
                body = body.Replace("{{email}}", email);
                body = body.Replace("{{senha}}", senha);

                await _emailSender.SendEmailAsync("ConsigIntegra - DADOS DE ACESSO", body, new[] { email });
                var logUsuario = new LogUsuarioDTO();
                logUsuario.Ativo = true;
                logUsuario.Controle = "E-mail novo Usuário";
                logUsuario.Metodo = "POST";
                logUsuario.UrlAcionada = email;
                _logUsuarioService.Create(logUsuario, 65, "");
            }
        } 
    }
}
