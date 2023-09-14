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

    }
}
