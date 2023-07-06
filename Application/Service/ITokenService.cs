using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public interface ITokenService
    {
        public string GenerateJWTToken((int userId, string userName, string roles) userDetails);

        /// <summary>
        /// using to extract claim from token from header.Authorization
        /// example use: ValidateToken(tokenString)?.FindFirst("ClaimName")?.Value
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public ClaimsPrincipal? ValidateToken(string jwtToken);
    }
}
