using System.Web.Mvc;

namespace MvcCrudDemo.Controllers
{
    public class UserPermissionsController : Controller
    {
        //
        // GET: /UserPermissions/

        public ActionResult Index()
        {
            HttpContext.Cache["UserPermissions"] = HttpContext.Cache["UserPermissions"] ?? new Models.UserPermissions();
            return View(HttpContext.Cache["UserPermissions"]);
        }

        [AcceptVerbs(HttpVerbs.Put)]
        [ActionName("Index")]
        public ActionResult Update(Models.UserPermissions up)
        {
            HttpContext.Cache["UserPermissions"] = up; // TODO: only do this for PUT(Update)
            return View(up);
        }

    }
}
