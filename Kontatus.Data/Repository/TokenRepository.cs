﻿using ConsigIntegra.Data.Context;
using ConsigIntegra.Domain.DTO;
using ConsigIntegra.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsigIntegra.Data.Repository
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<List<Token>> GetTokensAtivosPorUsuario(int usuarioID);
        Task<Token> GetJWT(string jwt);
    }
    public class TokenRepository : Repository<Token>, ITokenRepository
    {
        public TokenRepository(ConsigIntegraContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }

        public async Task<List<Token>> GetTokensAtivosPorUsuario(int usuarioID)
        {
            var tokens = await context.Tokens.Include("Usuario").Where(x=> x.Ativo && x.UsuarioID == usuarioID && x.DataExpiracao > DateTime.Now).OrderByDescending(x=> x.ID).ToListAsync();

            return tokens;
        }
        public async Task<Token> GetJWT(string jwt)
        {
            var token = await context.Tokens.Where(x => x.JWT == jwt).FirstOrDefaultAsync();

            return token;
        }

    }
}
