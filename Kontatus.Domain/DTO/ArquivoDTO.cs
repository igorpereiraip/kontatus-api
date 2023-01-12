using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ConsigIntegra.Domain.DTO
{
    public class ArquivoDTO
    {
        public string Mes { get; set; }
        public string Ano { get; set; }
        public string Beneficio { get; set; }
        public int UsuarioID { get; set; }
        public IList<IFormFile> Arquivos { get; set; }
    }
}
