using Kontatus.Domain.Entity;
using Kontatus.Service;
using Kontatus.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Kontatus.Helper.Utilitarios;
using System;

namespace Kontatus.API.Controllers
{
    public class PessoaController : Controller<Pessoa>
    {
        public PessoaController(IPessoaService service)
            : base(service)
        {
        }

        [HttpPost("IncluirTeste")]
        [AllowAnonymous]
        public virtual async Task<Result<int>> IncluirTeste()
        {
            try
            {
                var number = await (service as PessoaService).Teste();

                return Result<int>.Ok(number);
            }
            catch (Exception ex)
            {
                return Result<int>.Err(ex.Message);
            }
        }

    }
}
