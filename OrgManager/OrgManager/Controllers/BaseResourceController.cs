using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OrgManager.Models;

namespace OrgManager.Controllers
{
	// TODO: convert this to interface, ICrudController
    public class BaseResourceController : Controller
    {
		// TODO: separate Resource Read/WriteAuthorize using HTTP 403 substatus codes?
		// NOTE: if subclasses will use same method name, base methods must be protected to avoid ambiguity.
		// if subclasses are extended more than one level, CRUD methods will need unique names,
		// since MVC does not respect signatures when attempting to resolve methods.

		protected ActionResult Create()
		{
			var brm = (BaseResource)ViewData.Model;
			if (brm.Created)
			{
				Response.StatusCode = (int)HttpStatusCode.Created;
				// redirect assumes a route matching the name of the model type:
				return new RedirectToRouteResult(brm.ModelType.Name + "Detail",
					new System.Web.Routing.RouteValueDictionary(
						new { brm.Id }
					)
				);
			}
			return new ViewResult { ViewName = "Error" }; // TODO: more detailed / friendly error possibly.
		}

		protected ActionResult Update()
		{
			var brm = (BaseResource)ViewData.Model;
			if (brm.Updated)
			{
				Response.StatusCode = (int)HttpStatusCode.OK;
				// redirect assumes a route matching the name of the model type:
				return new RedirectToRouteResult(brm.ModelType.Name + "Detail", null); // original RouteData.Values will be retained.
			}
			return new ViewResult { ViewName = "Error" }; // TODO: more detailed / friendly error possibly.
		}

		protected ActionResult Delete()
		{
			var brm = (BaseResource)ViewData.Model;
			if (brm.Deleted)
			{
				Response.StatusCode = (int)HttpStatusCode.OK;
				RouteData.Values.Remove("Id"); // must manually remove Id for proper redirect:
				// redirect assumes a route matching the name of the model type:
				return new RedirectToRouteResult(brm.ModelType.Name + "List", null);
			}
			// todo: more robust exception handling, friendly error page.
			return new ViewResult { ViewName = "Error" }; // TODO: more detailed / friendly error possibly.
		}

    }
}
