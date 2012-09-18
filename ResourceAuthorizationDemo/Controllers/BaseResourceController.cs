using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResourceAuthorizationDemo.Models;
using ResourceAuthorizationDemo.Filters;

namespace ResourceAuthorizationDemo.Controllers
{
    public class BaseResourceController : Controller
    {
        //
        // GET: /baseResource/{Id}
        // TODO: separate Resource Read/WriteAuthorize using HTTP 403 substatus codes?
        [BaseResourceAuthorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index(BaseResourceModel brm)
        {
            //var u = new AppUserModel();
            //if (u.CanRead(brm))
            //{
            // TODO: probably better separate method (List()) and View for model listing...
            // this way requires type branching logic within View:
            if (brm.Id == null)
            {
                ViewData.Model = BaseResourceModel.ListResources();
            }
            else
            {
                ViewData.Model = brm;
            }
            return View();
            //}
            //else
            //{
            //    return new HttpUnauthorizedResult("you are not authorized to view the requested record.");
            //}
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create()
        {
            Response.StatusCode = (int)System.Net.HttpStatusCode.Created;
            return new RedirectToRouteResult("BaseResource",
                new System.Web.Routing.RouteValueDictionary(
                    // *NOTE*, this will map unmatched public properties to query string args (probably unwanted)
//                    new BaseResourceModel() { Id = "7777" }
                      new { Id = "7777" }
                )
            );
        }

    }
}
