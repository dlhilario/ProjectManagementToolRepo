using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ProjectManagementTool.Models;

namespace ProjectManagementTool.Models
{
    public class RoutingPageUtils
    {
        public static Func<HttpSessionState> RecoverSessionState = () => null;
        public static ApplicationUser CurrentUserInfo {
            
            get{
                var current = HttpContext.Current;
                if (current ==null)
                {
                    throw new NullReferenceException("Current HttpContext is not set for this thread.");
                }
                var Session = current.Session ?? RecoverSessionState();
                if (Session == null)
                {
                    throw new NullReferenceException("A Session state is not stablished for the Current HttpContext."); 
                }
                return Session["UserProfile"] as ApplicationUser;
            }
            set{
                var current = HttpContext.Current;
                if (current == null)
                {
                    throw new NullReferenceException("Current HttpContext is not set for this thread.");
                }
                var Session = current.Session ?? RecoverSessionState();
                if (Session == null)
                {
                    throw new NullReferenceException("A Session state is not stablished for the Current HttpContext.");
                }
                Session["UserProfile"] = value;
            }
            }
    }
}