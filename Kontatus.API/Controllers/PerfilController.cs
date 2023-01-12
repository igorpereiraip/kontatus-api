using Kontatus.Domain.Entity;
using Kontatus.Service;
using Kontatus.API.Controllers;

namespace Kontatus.API.Controllers
{
    public class PerfilController : Controller<Perfil>
    {
        public PerfilController(IPerfilService service)
            : base(service)
        {
        }

    }
}
