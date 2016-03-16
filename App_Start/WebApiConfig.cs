using LowStockApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LowStockDashboard
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
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new CustomAuthorizeAttribute());
            config.Filters.Add(new ExceptionHandler());

        }
    }
}
