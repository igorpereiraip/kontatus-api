using Kontatus.Data.Repository;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Service
{
    public interface IUsuarioService : IService<Usuario>
    {
        Task<Usuario> Create(UsuarioDTO usuario);
        Task<Usuario> Update(int id, UsuarioDTO usuario);
        Task<Usuario> GetByLoginID(int loginID);
        Task UpdatePassword(string email);
        Task AlterarSenha(string email, string senha, int usuarioID);
        Task<List<Usuario>> GetUsuarios();
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

                return novoUsuario;
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
