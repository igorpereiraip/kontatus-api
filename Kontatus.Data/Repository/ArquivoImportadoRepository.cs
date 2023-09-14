using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IArquivoImportadoRepository: IRepository<ArquivoImportado>
    {

    }
        public class ArquivoImportadoRepository : Repository<ArquivoImportado>, IArquivoImportadoRepository
    {
            public ArquivoImportadoRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
            {

            }


    }
}
