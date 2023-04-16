using Kontatus.Domain.Entity;
using Kontatus.Service;

namespace Kontatus.API.Controllers
{
    public class TelefoneController : Controller<Telefone>
    {
        public TelefoneController(ITelefoneService service)
            : base(service)
        {
        }

        //[HttpPost("IncluirTeste")]
        //[AllowAnonymous]
        //public virtual async Task<Result<int>> IncluirTeste()
        //{
        //    try
        //    {
        //        var number = await (service as PessoaService).Teste();

        //        return Result<int>.Ok(number);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Result<int>.Err(ex.Message);
        //    }
        //}

    }
}
