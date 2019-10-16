using System.Web;
using System.Web.Mvc;
using ProjectManagementTool.PMTWebService;
using System.Threading;
using System.Security.Principal;
using System.Security.Claims;

namespace ProjectManagementTool
{
    public class AuthenticatedAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthenticated = httpContext.Request.IsAuthenticated;
            
            if (isAuthenticated)
            {
                return true;
            }

            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {


        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Account/Login");
        }
    }
}