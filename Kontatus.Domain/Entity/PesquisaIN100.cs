namespace ConsigIntegra.Domain.Entity
{
    public class PesquisaIN100
    {
        public string CPF { get; set; }
        public string NumeroBeneficio { get; set; }
        public string Situacao { get; set; }
        public string ErrorID { get; set; }
        public ResultadoIN100 Resultado { get; set; }

    }
}
