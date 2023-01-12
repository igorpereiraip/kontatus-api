
using Kontatus.Domain.Entity;
using System.Collections.Generic;

namespace Kontatus.Domain.DTO
{
    public class BancoEmprestimoDTO
    {
        public string BancoEmprestimo { get; set; }
        public List<EmprestimoDTO> EmprestimosDTO { get; set; }

    }
}
