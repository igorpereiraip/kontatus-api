using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsigIntegra.Domain.ViewModels
{
    public class LogUsuarioViewModel : BaseViewModel
    {

        public int UsuarioID { get; set; }

        public string Metodo { get; set; }

        public string Controle { get; set; }

        public string UrlAcionada { get; set; }

        public int? RegistroAfetadoID { get; set; }
        public string RegistroNovo { get; set; }
        public string RegistroAntigo { get; set; }
        public string RegistroAfetadoNome { get; set; }

        public string DataLog
        {
            get
            {
                return string.Format("{0:yyyy-MM-ddTHH:mm}", DataCadastro);
            }
        }

        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual UsuarioViewModel RegistroAfetado { get; set; }

    }
}
