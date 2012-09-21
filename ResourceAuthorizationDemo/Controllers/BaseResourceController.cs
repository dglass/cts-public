using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ResourceAuthorizationDemo.Models;
using ResourceAuthorizationDemo.Filters;
using System.Web.Routing;
using System.Net;

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
            // initialize BaseResources cache if necessary:
            HttpContext.Cache["BaseResources"] = HttpContext.Cache["BaseResources"] ?? new Dictionary<Guid, BaseResourceModel>();
            var br = (Dictionary<Guid, BaseResourceModel>)HttpContext.Cache["BaseResources"];
            if (brm.Id == Guid.Empty)
            {
                if (br.Count < 1)
                {
                    // model list is now empty...this populates it with 3 dummy records:
                    var mlist = BaseResourceModel.ListResources();
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

        public ActionResult Edit(BaseResourceModel brm)
        {
            return View(((Dictionary<Guid,BaseResourceModel>)HttpContext.Cache["BaseResources"])[brm.Id]);
        }

        [HttpDelete]
        [ActionName("Index")]
        [AcceptVerbs(HttpVerbs.Delete)]
        public ActionResult Delete(BaseResourceModel brm)
        {
            if (((Dictionary<Guid, BaseResourceModel>)HttpContext.Cache["BaseResources"]).Remove(brm.Id))
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                RouteData.Values.Remove("Id"); // must manually remove Id for proper redirect:
                return new RedirectToRouteResult("BaseResource", null);
            }
            else
            {
                // todo: more robust exception handling, friendly error page.
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Failed to delete resource from cache");
            }
        }

        [HttpPost]
        [ActionName("Index")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(BaseResourceModel brm)
        {
            // TODO: ModelCache or similar?  or is that just a trivial wrapper layer?
            var modelkey = "BaseResources";
            if (HttpContext.Cache[modelkey] == null)
            {
                HttpContext.Cache[modelkey] = new Dictionary<Guid,BaseResourceModel>();
            }
            var resources = (Dictionary<Guid,BaseResourceModel>)HttpContext.Cache[modelkey];
            var newId = Guid.NewGuid();
            resources[newId] = new BaseResourceModel()
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
        [AcceptVerbs(HttpVerbs.Put)]
        public ActionResult Update(BaseResourceModel brm)
        {
            // TODO: return 404 if record not found...
            ((Dictionary<Guid, BaseResourceModel>)HttpContext.Cache["BaseResources"])[brm.Id] = brm;
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new RedirectToRouteResult("BaseResource", null); // original RouteData.Values will be retained.
        }

    }
}
