using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OrgManager
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "OrgUnit",
                url: "orgUnit/{Id}",
                defaults: new { controller="OrgUnit", action="DeptOp", id = UrlParameter.Optional }
            );

			routes.MapRoute(
				name: "OrgUnitNode",
				url: "orgUnitNode/{Id}",
				defaults: new { controller = "OrgUnit", action = "NodeOp", id = UrlParameter.Optional }
			);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}