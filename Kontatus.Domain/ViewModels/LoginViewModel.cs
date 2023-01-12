using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsigIntegra.Domain.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel()
        {
            TipoLoginID = TipoLoginViewModel.ConsigIntegra;
            Principal = true;
        }

        public int UsuarioID { get; set; }

        public string Email { get; set; }

        [JsonIgnore]
        public string Senha { get; set; }

        public TipoLoginViewModel TipoLoginID { get; set; }

        public bool Principal { get; set; }

        public int TimezoneID { get; set; }

        public string TipoLoginNome { get { return TipoLoginID.ToString(); } }

        [JsonIgnore]
        public virtual UsuarioViewModel Usuario { get; set; }

    }

    public enum TipoLoginViewModel { ConsigIntegra = 1, Google = 2, Facebook = 3 }
}
