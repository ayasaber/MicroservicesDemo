using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Logging.Models
{
    public static class TPSClaimNames
    {
        public static readonly string Username = "Username";
        public static readonly string SessionId = "SessionId";
        public static readonly string LoginDate = "LoginDate";

        public static readonly string CompanyId = "CompanyId";
        public static readonly string BranchId = "BranchId";
    }

    public class TPSClaims
    {
        public string? UserName { get; set; }
        public string? SessionId { get; set; }
        public DateTime? LoginDate { get; set; }
        public long? CompanyId { get; set; }
        public long? BranchId { get; set; }
        public string Token { get; set; }
    }

    public static class TPSClaimsExtensions
    {
        public static TPSClaims GetTPSClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var claims = ((JwtSecurityToken)handler.ReadToken(token)).Claims;

            _ = DateTime.TryParse(claims.GetClaimValue(TPSClaimNames.LoginDate), out var lastLogin);
            _ = long.TryParse(claims.GetClaimValue(TPSClaimNames.CompanyId), out var companyId);
            _ = long.TryParse(claims.GetClaimValue(TPSClaimNames.BranchId), out var branchId);

            var result = new TPSClaims()
            {
                UserName = claims.GetClaimValue(TPSClaimNames.Username),
                SessionId = claims.GetClaimValue(TPSClaimNames.SessionId),
                LoginDate = lastLogin,
                CompanyId = companyId,
                BranchId = branchId,
                Token = token
            };

            return result;
        }
        public static string GetClaimValue(this IEnumerable<Claim> claims, string claimType)
        {
            string claimValue = string.Empty;
            var claim = claims.FirstOrDefault(item => item.Type == claimType);
            if (claim != null)
            {
                claimValue = claim.Value;
            }
            return claimValue;
        }

        public static string GetClaimValue(this HttpContext httpContext, string claimType)
        {
            string claimValue = string.Empty;
            if (httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity is ClaimsIdentity claimsIdentity)
            {
                Claim claim = claimsIdentity.FindFirst(claimType);
                if (claim != null)
                {
                    claimValue = claim.Value;
                }
            }
            return claimValue;
        }
    }
}
