using System;
using System.Web.Mvc;

namespace Infrastructure.Extentions
{
    public sealed class AuthenticationFilter : AuthorizeAttribute
    {
        private UserManager _userManager { get; set; }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            return _userManager != null && _userManager.IsAuthenticated;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_userManager != null && !(_userManager.IsAuthenticated))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }

    public class UserManager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAuthenticated { get; set; }

    }
}
