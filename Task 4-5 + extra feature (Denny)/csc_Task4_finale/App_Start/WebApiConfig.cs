using EmployeeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace csc_Task4_finale
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new RequireHttpsAttribute());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new RequireHttpsAttribute());
        }
        
    }
}
