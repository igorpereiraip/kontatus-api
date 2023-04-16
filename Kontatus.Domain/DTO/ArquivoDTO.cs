using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Kontatus.Domain.DTO
{
    public class ArquivoDTO
    {
        public string Mes { get; set; }
        public string Ano { get; set; }
        public string Competencia { get; set; }
        public string Descricao { get; set; }
        public int UsuarioID { get; set; }
        public IList<IFormFile> Arquivos { get; set; }
    }
}
