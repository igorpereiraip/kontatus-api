using Kontatus.Data.Repository;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Kontatus.Helper.Utilitarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IEmailService
    {
        Task EnviarEmailNovoUsuario(string nome, string email, string senha);
        Task<bool> ValidateEmailAsync(Email email, List<Email> listEmail);
        Task<bool> CreateRange(List<Email> emails);
    }

    public class EmailService : Service<Email>, IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly ILogUsuarioService _logUsuarioService;
        private readonly IEmailRepository repository;
        public EmailService(IEmailSender emailSender, IConfiguration config, ILogUsuarioService logUsuarioService, IEmailRepository repository) : base()
        {
            _emailSender = emailSender;
            _config = config;
            _logUsuarioService = logUsuarioService;
            this.repository = repository;
        }

        public async Task<bool> CreateRange(List<Email> emails)
        {
            await repository.CreateRange(emails);
            return true;
        }

        public async Task<bool> ValidateEmailAsync(Email email, List<Email> listEmail)
        {
            return !String.IsNullOrEmpty(email.EnderecoEmail) && email.EnderecoEmail.Contains("@") &&
                listEmail.Find(z => z.PessoaId == email.PessoaId && z.EnderecoEmail == email.EnderecoEmail) == null &&
                !await repository.Find(x => x.PessoaId == email.PessoaId && x.EnderecoEmail == email.EnderecoEmail).AnyAsync();
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

                await _emailSender.SendEmailAsync("Kontatus - DADOS DE ACESSO", body, new[] { email });
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
