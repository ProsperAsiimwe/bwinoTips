using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BwinoTips.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Enable Attribute Routing
            routes.LowercaseUrls = true;
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(name: "", url: "ExclusiveTip/{ExclusiveTipId}/{action}", defaults: new { controller = "ExclusiveTip" }, constraints: new { ExclusiveTipId = @"\d+" });
            routes.MapRoute(name: "", url: "Admin/{roleName}/{id}/{action}", defaults: new { controller = "Users", action = "Show" }, constraints: new { id = @"\d+", roleName = @"^(admin|developer|member)s$" });           

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
