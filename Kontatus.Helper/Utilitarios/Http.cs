using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Kontatus.Helper.Utilitarios
{
    public static class Http
    {
        public static JwtSecurityToken ExtractJwtFromHeader(this IHeaderDictionary headers)
        {
            if (headers.TryGetValue("Authorization", out var authorization))
            {
                var jwt = authorization[0];
                if (jwt is null)
                {
                    return null;
                }
                return new JwtSecurityTokenHandler().ReadToken(jwt.Substring(7)) as JwtSecurityToken;
            }
            return null;
        }

        public static string GetClaimFromJWT(this JwtSecurityToken token, string claim)
        {
            return token.Claims.Where(c => c.Type == claim).Select(c => c.Value).FirstOrDefault();
        }
    }
}
