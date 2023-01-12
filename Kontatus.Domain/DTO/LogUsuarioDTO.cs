namespace Kontatus.Domain.DTO
{
    public class LogUsuarioDTO
    {
        public int ID { get; set; }
        public string UrlAcionada { get; set; }
        public int? RegistroAfetadoID { get; set; }
        public string RegistroNovo { get; set; }
        public string RegistroAntigo { get; set; }
        public int UsuarioID { get; set; }
        public string Metodo { get; set; }
        public string Controle { get; set; }
        public bool Ativo { get; set; }
    }
}
