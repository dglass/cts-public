using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OrgManagerWebApi
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "OuDetail",
				routeTemplate: "api/orgunit/{id}/detail",
				defaults: new
				{
					controller = "OrgUnit",
					action = "GetDetail"
				}
			);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional,
				action = "NodeOp"}
			);
		}
	}
}
