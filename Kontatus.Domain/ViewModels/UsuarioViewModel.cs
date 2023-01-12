using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsigIntegra.Domain.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        public string Nome { get; set; }

        public bool Administrador { get; set; }
        public virtual ICollection<LoginViewModel> Logins { get; set; }


    }

}
