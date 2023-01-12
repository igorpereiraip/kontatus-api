using AutoMapper;
using Kontatus.Data.Repository;

namespace Kontatus.Service
{
    public interface ITokenService
    {
    }

    public class TokenService : ITokenService
    {
        private readonly ITokenRepository repository;
        private readonly IMapper _mapper;

        public TokenService(ITokenRepository repository, IMapper mapper)
        {
            this.repository = repository;
            _mapper = mapper;
        }

    }
}
