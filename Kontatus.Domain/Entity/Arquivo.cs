using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace ConsigIntegra.Domain.Entity
{
    [Table("Arquivo")]
    public class Arquivo : Base
    {

        [StringLength(300, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "CaminhoImagem é Obrigatório")]
        public string CaminhoImagem { get; set; }

        [StringLength(300, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "NomeArquivo é Obrigatório")]
        public string NomeBlob { get; set; }

        public string NomeCompleto { get; set; }

        public DateTime DataUpload { get; set; }

        public string Beneficio { get; set; }

        public double Tamanho { get; set; }

        public int UsuarioID { get; set; }

        [NotMapped]
        public virtual string CaminhoVisualizacao
        {
            get
            {
                return $"{this.CaminhoImagem}";
            }
        }

        [NotMapped]
        public virtual string Extensao { get { return CaminhoImagem.Split('.').ToList().Last(); } }

        
        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }
    }
}