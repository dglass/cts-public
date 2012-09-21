using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ResourceAuthorizationDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // this gets the form to create a new resource.
            // the actual Create occurs via a POST.
            // *NOTE*, this must precede /{Id} map since it will capture every string by default.
            routes.MapRoute(
                name: "BaseResourceNew",
                url: "resource/new",
                defaults: new { controller = "BaseResource", action = "New" }
            );

            // this gets the form to edit a resource.
            // the actual Update occurs via a PUT.
            routes.MapRoute(
                name: "BaseResourceEdit",
                url: "resource/{Id}/edit",
                defaults: new { controller = "BaseResource", action = "Edit" }
            );

            // this returns a single record; overload returns list...
            routes.MapRoute(
                name: "BaseResource",
                url: "resource/{Id}",
                defaults: new { controller = "BaseResource", action = "Index", Id = UrlParameter.Optional}
            );

            //// this returns list of available records...
            //routes.MapRoute(
            //    name: "BaseResourceListRestRoute",
            //    url: "baseResource",
            //    defaults: new { controller = "BaseResource", action = "List" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}