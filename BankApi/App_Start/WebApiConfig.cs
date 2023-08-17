using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace BankApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "ActionApi",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id = RouteParameter.Optional }
            );

            EnableCorsAttribute Cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(Cors);

            
          
                // Enable CORS globally
                var cors = new EnableCorsAttribute("https://localhost:44362", "*", "*");
                config.EnableCors(cors);

            // Other configuration settings
           // config.Filters.Add(new AuthorizeAttribute());
        }
    }
}
