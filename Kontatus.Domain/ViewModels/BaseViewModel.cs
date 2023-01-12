using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kontatus.Domain.ViewModels
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            DataCadastro = DateTime.UtcNow;
            Ativo = true;
        }

        public int ID { get; set; }

        [JsonIgnore]
        public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public DateTime? DataAlteracao { get; set; }

        public bool Ativo { get; set; }
    }
}
