using Kontatus.Domain.Entity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kontatus.Domain.Entity
{
    [Table("Login")]
    public class Login : Base
    {
        public Login()
        {
            TipoLoginID = TipoLogin.Kontatus;
            Principal = true;
        }

        [Required(ErrorMessage = "UsuarioID é Obrigatório"), Range(1, int.MaxValue, ErrorMessage = "UsuarioID é Obrigatório")]
        public int UsuarioID { get; set; }
        
        [StringLength(250, ErrorMessage = "O campo {0} não pode exceder {1} caracteres"), Required(ErrorMessage = "Email é Obrigatório")]
        public string Email { get; set; }
        
        [JsonIgnore]
        [Required(ErrorMessage = "Senha é Obrigatório")]
        public string Senha { get; set; }
        
        [Required(ErrorMessage = "TipoLoginID é Obrigatório"), Range(1, 3, ErrorMessage = "Tipo de Login deve está entre 1 e 3")]
        public TipoLogin TipoLoginID { get; set; }

        [Required(ErrorMessage = "Principal é Obrigatório")]
        public bool Principal { get; set; }

        [NotMapped]
        public string TipoLoginNome { get { return TipoLoginID.ToString(); } }


        [JsonIgnore]
        public virtual Usuario Usuario { get; set; }   
        
    }

    public enum TipoLogin { Kontatus = 1, Google = 2, Facebook = 3 }
}