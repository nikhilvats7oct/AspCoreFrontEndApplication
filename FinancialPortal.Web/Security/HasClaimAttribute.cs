using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinancialPortal.Web.Security
{
    public class HasClaimAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _claimType;
        private readonly string _claimValue;

        public HasClaimAttribute(string claimType, string claimValue)
        {
            _claimType = claimType;
            _claimValue = claimValue;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isOk = false;

            if (string.IsNullOrWhiteSpace(_claimType) || string.IsNullOrWhiteSpace(_claimValue))
            {
                isOk = context.HttpContext.User is ClaimsPrincipal u && u.Claims.Any(x => x.Type == "sub");
            }
            else
            {
                isOk = context.HttpContext.User is ClaimsPrincipal user && user.HasClaim(_claimType, _claimValue);
            }

            if (isOk)
            {
                // don't have to do anything.
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}