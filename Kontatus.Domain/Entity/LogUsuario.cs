using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ConsigIntegra.Domain.Entity
{
    [Table("LogUsuario")]
    public class LogUsuario : Base
    {

        [Required(ErrorMessage = "ID do Usuário é Obrigatório"), Range(1, int.MaxValue, ErrorMessage = "O ID do Usuário deve ser um inteiro válido e maior do que 0")]
        public int UsuarioID { get; set; }

        [StringLength(10, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "Método do Log é Obrigatório")]
        public string Metodo { get; set; }

        [StringLength(150, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "Controle do Log é Obrigatório")]
        public string Controle { get; set; }

        [StringLength(500, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "Descrição do Log é Obrigatória")]
        public string UrlAcionada { get; set; }

        public int? RegistroAfetadoID { get; set; }

        public string RegistroNovo { get; set; }

        public string RegistroAntigo { get; set; }

        [NotMapped]
        public string DataLog { get { return string.Format("{0:yyyy-MM-ddTHH:mmZ}", DataCadastro); } }

        public virtual Usuario Usuario { get; set; }
        public virtual Usuario RegistroAfetado { get; set; }

    }
}
