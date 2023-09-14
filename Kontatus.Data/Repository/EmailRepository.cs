using Kontatus.Data.Context;
using Kontatus.Domain.DTO;
using Kontatus.Domain.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kontatus.Data.Repository
{
    public interface IEmailRepository : IRepository<Email>
    {
        Task<bool> CreateRange(List<Email> emails);
    }
    public class EmailRepository : Repository<Email>, IEmailRepository
    {
        public EmailRepository(KontatusContext context, LogUsuarioDTO logUsuarioDTO) : base(context, logUsuarioDTO)
        {
        }
        public async Task<bool> CreateRange(List<Email> emails)
        {
            await context.Emails.AddRangeAsync(emails);

            await context.SaveChangesAsync();

            return true;
        }

    }
}
