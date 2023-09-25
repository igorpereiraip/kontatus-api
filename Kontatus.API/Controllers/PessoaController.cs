using Kontatus.Domain.Entity;
using Kontatus.Service;
using Kontatus.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Kontatus.Helper.Utilitarios;
using System;
using Kontatus.Domain.DTO;
using System.Linq;
using System.Collections.Generic;

namespace Kontatus.API.Controllers
{
    public class PessoaController : Controller<Pessoa>
    {
        public PessoaController(IPessoaService service)
            : base(service)
        {
        }

        [HttpGet("GetPersonByName/{name}/{uf}/{year}")]
        public async Task<Result<List<Pessoa>>> GetPersonByName(string name, string uf, string year)
        {
            try
            {
                var personList = await (service as PessoaService).GetPersonByName(name, uf, year);

                return Result<List<Pessoa>>.Ok(personList);
            }
            catch (Exception ex)
            {
                return Result<List<Pessoa>>.Err(ex.Message);
            }
        }

    }
}
