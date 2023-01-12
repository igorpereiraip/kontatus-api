
using ConsigIntegra.Domain.Entity;
using System.Collections.Generic;

namespace ConsigIntegra.Domain.DTO
{
    public class BancoEmprestimoDTO
    {
        public string BancoEmprestimo { get; set; }
        public List<EmprestimoDTO> EmprestimosDTO { get; set; }

    }
}
