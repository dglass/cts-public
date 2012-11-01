using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OrgNavWebApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "NodeOp",
				routeTemplate: "api/node/{id}",
				defaults: new
				{
					action = "NodeOp",
					controller = "Node"
				}
			);

			config.Routes.MapHttpRoute(
				name: "SubNodes",
				routeTemplate: "api/{controller}/{id}/subnodes",
				defaults: new {
					action = "GetLazyExpand"
				}
			);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
