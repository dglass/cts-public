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
				name: "LazyLoad",
				routeTemplate: "api/{controller}/{id}/subnodes"
				//routeTemplate: "api/{controller}/{id}/subnodes/{all}"
				//defaults: new {
					//all = RouteParameter.Optional,
					//action = "GetLazyExpand"
				//}
			);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
