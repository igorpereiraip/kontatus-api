using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kontatus.Domain.Entity
{
    public class Base
    {
        public Base()
        {
            DataCadastro = DateTime.UtcNow;
            Ativo = true;
        }

        [Key]
        public int ID { get; set; }

        public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }

    }
}
