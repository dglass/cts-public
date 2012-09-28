using System.Web.Mvc;
using System.Web.Routing;

namespace MvcCrudDemo
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
                defaults: new { controller = "BaseResource", action = "Index", Id = UrlParameter.Optional }
            );

            // for demonstrating behavior under different permission settings:
            routes.MapRoute(
                name: "UserPermissions",
                url: "permissions",
                defaults: new { controller = "UserPermissions", action = "Index" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}