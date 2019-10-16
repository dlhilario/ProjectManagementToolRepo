using System.Web.Http;

namespace ProjectManagementTool.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.EnableCors();
            config.Routes.MapHttpRoute(
             name: "API Default",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = RouteParameter.Optional }
             );


        }
    }
}