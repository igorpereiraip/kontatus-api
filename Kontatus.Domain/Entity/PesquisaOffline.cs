using System.Collections.Generic;


namespace ConsigIntegra.Domain.Entity
{
    public class PesquisaOffline
    {
        public string CPF { get; set; }
        public IList<Beneficio> Beneficios { get; set; }
        public IList<ResultadoOffline> Resultado { get; set; }

    }
}