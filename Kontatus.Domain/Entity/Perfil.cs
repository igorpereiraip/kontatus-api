using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kontatus.Domain.Entity
{
    [Table("Perfil")]
    public class Perfil : Base
    {
        [StringLength(150, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "Nome do Usuario é Obrigatório")]
        public string Nome { get; set; }

        public bool Administrador { get; set; }

    }
}
