using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Infocursos
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}",
    defaults: new { controller = "Home", action = "Index" }
);

            routes.MapRoute(
                name: "Formador",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Formador", action = "Formador", id = UrlParameter.Optional }
            );



            routes.MapRoute(
                name: "Alumno",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Alumno", action = "Alumno", id = UrlParameter.Optional }
            );




        }
    }
}
