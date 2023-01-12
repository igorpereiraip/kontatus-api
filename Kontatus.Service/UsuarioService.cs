using ConsigIntegra.Data.Repository;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsigIntegra.Service
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<Usuario> Create(UsuarioDTO usuario);
        Task<Usuario> Update(int id, UsuarioDTO usuario);
        Task<Usuario> GetByLoginID(int loginID);
        Task UpdatePassword(string email);
        Task AlterarSenha(string email, string senha, int usuarioID);
        Task<List<Usuario>> GetUsuarios();
        Task<Usuario> AdicionarSaldoIN100(int loginId, int id, int valor);
        Task<Usuario> AdicionarSaldoExtrato(int loginId, int id, int valor);
        Task<Usuario> AdicionarSaldoOffline(int loginId, int id, int valor);
        Task<Usuario> ConsumirSaldoIN100(int loginId, int id, string tipo, string beneficio);
        Task<Usuario> EstornarSaldoIN100(int loginId, int id, string tipo, string beneficio);
        Task<Usuario> EstornarSaldoIN100Extrato(int loginId, int id, string tipo, string beneficio);
        Task<Usuario> ConsumirSaldoExtrato(int loginId, int id, string tipo, string beneficio);
        Task<Usuario> ConsumirSaldoIN100Extrato(int loginId, int id, string tipo, string beneficio);
        Task<bool> ConsumirSaldoOffline(int loginId, int id, string tipo, string beneficio, bool consultaDiariaBeneficio);
        Task<bool> ConsumirSaldoOfflineIlimitado(int loginId, int id, string tipo, string beneficio);
    }

    public class UsuarioService : Service<Usuario>, IUsuarioService
    {
        private readonly ILoginRepository loginRepository;
        private readonly IEmailService _emailService;
        private readonly ILogUsuarioService _logUsuarioService;

        public UsuarioService(IUsuarioRepository repository, ILoginRepository loginRepository, IEmailService emailService, ILogUsuarioService logUsuarioService)
            : base(repository)
        {
            this.loginRepository = loginRepository;
            _emailService = emailService;
            _logUsuarioService = logUsuarioService;
        }

        public async Task<List<Usuario>> GetUsuarios()
        {
            return await (repository as UsuarioRepository).GetUsuarios();
        }

        public async Task<Usuario> GetByLoginID(int loginID)
        {
            var login = await loginRepository.Get(loginID);

            if (login == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            var usuario = await (repository as IUsuarioRepository).Obter(login.UsuarioID);

            return usuario;
        }

        public async Task<Usuario> Create(UsuarioDTO usuario)
        {
            var novoUsuario = await (repository as UsuarioRepository).Create(usuario);

            //await EnviarEmail(novoUsuario.ID, usuario.Nome, usuario.Email, usuario.Senha);

            return novoUsuario;
        }

        public async Task<Usuario> AdicionarSaldoIN100(int loginID, int id, int valor)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoIN100(id, valor);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = valor > 0 ? "+" + valor.ToString() : valor.ToString();
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = "Adição Saldo IN100";
            logUsuario.RegistroAfetadoID = id;

            _logUsuarioService.Create(logUsuario, loginID, "");

            return usuario;
        }

        public async Task<Usuario> AdicionarSaldoExtrato(int loginID, int id, int valor)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoExtrato(id, valor);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = valor > 0 ? "+" + valor.ToString() : valor.ToString();
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = "Adição Saldo Extrato";
            logUsuario.RegistroAfetadoID = id;

            _logUsuarioService.Create(logUsuario, loginID, "");

            return usuario;
        }

        public async Task<Usuario> AdicionarSaldoOffline(int loginID, int id, int valor)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoOffline(id, valor);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = valor > 0 ? "+" + valor.ToString() : valor.ToString();
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = "Adição Saldo Offline";
            logUsuario.RegistroAfetadoID = id;

            _logUsuarioService.Create(logUsuario, loginID, "");

            return usuario;
        }

        public async Task<Usuario> ConsumirSaldoExtrato(int loginId, int id, string tipo, string beneficio)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoExtrato(id, -1);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "-1";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return usuario;
        }

        public async Task<Usuario> ConsumirSaldoIN100(int loginId, int id, string tipo, string beneficio)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoIN100(id, -1);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "-1";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return usuario;
        }

        public async Task<Usuario> ConsumirSaldoIN100Extrato(int loginId, int id, string tipo, string beneficio)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoExtrato(id, -1);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "-1";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return usuario;
        }

        public async Task<bool> ConsumirSaldoOffline(int loginId, int id, string tipo, string beneficio, bool consultaDiariaBeneficio)
        {
            if(consultaDiariaBeneficio)
                await (repository as UsuarioRepository).AdicionarSaldoOffline(id, -1);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "-1";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return true;
        }

        public async Task<bool> ConsumirSaldoOfflineIlimitado(int loginId, int id, string tipo, string beneficio)
        {
            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "0";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return true;
        }

        public async Task<Usuario> EstornarSaldoIN100(int loginId, int id, string tipo, string beneficio)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoIN100(id, 1);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "1";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return usuario;
        }

        public async Task<Usuario> EstornarSaldoIN100Extrato(int loginId, int id, string tipo, string beneficio)
        {
            var usuario = await (repository as UsuarioRepository).AdicionarSaldoExtrato(id, 1);

            var logUsuario = new LogUsuarioDTO();
            logUsuario.Ativo = true;
            logUsuario.Controle = "1";
            logUsuario.Metodo = "PUT";
            logUsuario.UrlAcionada = tipo;
            logUsuario.RegistroAfetadoID = id;
            logUsuario.RegistroNovo = beneficio;

            _logUsuarioService.Create(logUsuario, loginId, "");

            return usuario;
        }

        private async Task EnviarEmail(int userID, string nome, string email, string senha)
        {
            await _emailService.EnviarEmailNovoUsuario(nome, email, senha);
        }

        public override async Task<Usuario> GetById(int id)
        {
            return await (repository as UsuarioRepository).Get(id);
        }

        public async Task<Usuario> Update(int id, UsuarioDTO usuario)
        {
            return await (repository as UsuarioRepository).Update(id, usuario);
        }

        public async Task UpdatePassword(string email)
        {
            var usuario = await (repository as UsuarioRepository).UpdatePassword(email);

            await EnviarEmail(usuario.ID, usuario.Nome, usuario.Email, usuario.Senha);
        }

        public async Task AlterarSenha(string email, string senha, int usuarioID)
        {
            await (repository as UsuarioRepository).AlterarSenha(email, senha, usuarioID);
        }
    }
}
