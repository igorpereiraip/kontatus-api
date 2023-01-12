using ConsigIntegra.Domain.Entity;
using ConsigIntegra.Service;
using GED.API.Controllers;

namespace ConsigIntegra.API.Controllers
{
    public class PerfilController : Controller<Perfil>
    {
        public PerfilController(IPerfilService service)
            : base(service)
        {
        }

    }
}
