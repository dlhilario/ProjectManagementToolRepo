using System.Web;
using System.Web.Mvc;
using ProjectManagementTool.PMTWebService;

namespace ProjectManagementTool
{
    public class AuthenticatedAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {        
            HttpSessionStateBase Session = httpContext.Session;
            UserProfile userProfile = (UserProfile)Session["UserProfile"];

            if (userProfile != null  && !userProfile.LoggedIn)
            {
                return false;
            }

            return true;
        }

        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
         
           
        //}

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Account/Login");
        }
    }
}