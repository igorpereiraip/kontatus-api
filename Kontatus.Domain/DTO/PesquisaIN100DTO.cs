
using Kontatus.Domain.Entity;


namespace Kontatus.Domain.DTO
{
    public class PesquisaIN100DTO
    {
        public string CPF { get; set; }
        public string NumeroBeneficio { get; set; }
        public string Situacao { get; set; }
        public string ErrorID { get; set; }
        public DadosIN100 Resultado { get; set; }

    }
}
