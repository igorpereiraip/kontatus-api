using System.Collections.Generic;


namespace ConsigIntegra.Domain.Entity
{
    public class PesquisaBeneficio
    {
        public string CPF { get; set; }
        public IList<Beneficio> Beneficios { get; set; }
        public IList<Resultado> Resultado { get; set; }

    }
}