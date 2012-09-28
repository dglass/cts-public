using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MvcCrudDemo.Filters;
using MvcCrudDemo.Models;

namespace MvcCrudDemo.Controllers
{
    // applying Authorize attribute at class-level here:
    [BaseResourceAuthorize]
    public class BaseResourceController : Controller
    {
        //
        // GET: /baseResource/({Id})
        // TODO: separate Resource Read/WriteAuthorize using HTTP 403 substatus codes?
       // [BaseResourceAuthorize] <-- this attribute can be applied at class or method levels.
        public ActionResult Index(BaseResource brm)
        {
            // initialize BaseResources cache if necessary:
            HttpContext.Cache["BaseResources"] = HttpContext.Cache["BaseResources"] ?? new Dictionary<Guid, BaseResource>();
            var br = (Dictionary<Guid, BaseResource>)HttpContext.Cache["BaseResources"];
            if (brm.Id == Guid.Empty)
            {
                if (br.Count < 1)
                {
                    // model list is now empty...this populates it with 3 dummy records:
                    var mlist = BaseResource.ListResources();
                    foreach (var model in mlist)
                    {
                        br[model.Id] = model;
                    }
                }
                ViewData.Model = br;
            }
            else
            {
                // note, this assumes access has already been authorized and user is not attempting to access a non-existent resource.
                // TODO: return 404 if authorized but non-existent.
                ViewData.Model = br[brm.Id];
            }
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Edit(BaseResource brm)
        {
            return View(((Dictionary<Guid, BaseResource>)HttpContext.Cache["BaseResources"])[brm.Id]);
        }

        [HttpDelete]
        [ActionName("Index")]
        //[AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(BaseResource brm)
        {
            if (((Dictionary<Guid, BaseResource>)HttpContext.Cache["BaseResources"]).Remove(brm.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                RouteData.Values.Remove("Id"); // must manually remove Id for proper redirect:
                return new RedirectToRouteResult("BaseResource", null);
            }
            // todo: more robust exception handling, friendly error page.
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed to delete resource from cache");
        }

        [HttpPost]
        [ActionName("Index")]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(BaseResource brm)
        {
            // TODO: ModelCache or similar?  or is that just a trivial wrapper layer?
            const string modelkey = "BaseResources";
            if (HttpContext.Cache[modelkey] == null)
            {
                HttpContext.Cache[modelkey] = new Dictionary<Guid, BaseResource>();
            }
            var resources = (Dictionary<Guid, BaseResource>)HttpContext.Cache[modelkey];
            var newId = Guid.NewGuid();
            resources[newId] = new BaseResource
            {
                Id = newId,
                Name = brm.Name
            };

            Response.StatusCode = (int)HttpStatusCode.Created;
            return new RedirectToRouteResult("BaseResource",
                new System.Web.Routing.RouteValueDictionary(
                      new { Id = newId.ToString() }
                )
            );
        }

        [HttpPut]
        [ActionName("Index")]
        //[AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Update(BaseResource brm)
        {
            // TODO: return 404 if record not found...
            ((Dictionary<Guid, BaseResource>)HttpContext.Cache["BaseResources"])[brm.Id] = brm;
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new RedirectToRouteResult("BaseResource", null); // original RouteData.Values will be retained.
        }

    }
}
