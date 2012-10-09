using System.Web.Mvc;
using MvcCrudDemo.Models;

namespace MvcCrudDemo.Controllers
{
//    public class UserPermissionsController : Controller
    public class UserPermissionsController : CachedResourceController
    {
        //
        // GET: /UserPermissions/

//        public ActionResult Index()
        // renamed this to prevent ambiguity with base class Index() since MVC does not respect method signatures
        public ActionResult SetPermissions(UserPermissions up)
        {
//            HttpContext.Cache["UserPermissions"] = HttpContext.Cache["UserPermissions"] ?? new Models.UserPermissions();
            HttpContext.Cache[up.ModelType.Name] = HttpContext.Cache[up.ModelType.Name] ?? new UserPermissions();
            return View(HttpContext.Cache[up.ModelType.Name]);
        }

        [HttpPut]
        [ActionName("SetPermissions")]
        // *NOTE*, can't use Update() due to MVC failure to respect method signatures.
        public ActionResult UpdatePermissions(Models.UserPermissions up)
        {
            HttpContext.Cache[up.ModelType.Name] = up; // TODO: only do this for PUT(Update)
            return View(up);
        }

    }
}
