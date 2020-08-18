using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ITS.UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                  name: "Main_Deafult",
                  url: "Home/Index",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

                );
            routes.MapRoute(
               name: "Error_Deafult",
               url: "Home/AccessRightsError/{CName}/{AName}",
               defaults: new { controller = "Home", action = "AccessRightsError", CName = UrlParameter.Optional, AName = UrlParameter.Optional }

             );
        }
    }
}
