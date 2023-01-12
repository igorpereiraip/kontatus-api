using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Kontatus.Domain.Entity
{
    public class Resultado
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public int? Idade { get; set; }
        public int? CodigoTipoBeneficio { get; set; }
        public string DescricaoTipoBeneficio { get; set; }
        public string NumeroBeneficio { get; set; }

    }
}
