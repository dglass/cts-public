using System.Web.Mvc;
using System.Web.Routing;

namespace MvcCrudDemo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // /new gets the *form* to create a new resource.
            // the actual Create occurs via a POST to [ModelName]List route.
            // [ModelName]New = form for creating new record
            routes.MapRoute(
                name: "PersonNew",
                url: "person/new", // *NOTE*, /new must precede /{Id}
                defaults: new { controller = "Person", action = "PersonNew" }
            );

            // [ModelName]List = list existing Models
            routes.MapRoute(
                name: "PersonList",
                url: "persons",
                defaults: new { controller = "Person", action = "PersonList" }
            );

            // [ModelName]Detail = single record
            routes.MapRoute(
                name: "PersonDetail",
                url: "person/{Id}",
                defaults: new {controller = "Person", action = "PersonDetail" }
            );

            // [ModelName]Edit = edit single record
            routes.MapRoute(
                name: "PersonEdit",
                url: "person/{Id}/edit",
                defaults: new { controller = "Person", action = "PersonEdit" }
            );

            // for demonstrating behavior under different permission settings:
            routes.MapRoute(
                name: "UserPermissions",
                url: "permissions",
                defaults: new { controller = "UserPermissions", action = "SetPermissions" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}