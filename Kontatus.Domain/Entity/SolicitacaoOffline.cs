namespace ConsigIntegra.Domain.Entity
{
    public class SolicitacaoOffline : Base
    {
        public string CPF { get; set; }
        public string NumeroBeneficio { get; set; }
        public int? UsuarioID { get; set; }
        public virtual Usuario Usuario { get; set; }

    }
}
