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
        // GET: /BaseResource/
        // TODO: separate Resource Read/WriteAuthorize using HTTP 403 substatus codes?
        [BaseResourceAuthorize]
        public ActionResult Index(Models.BaseResourceModel brm)
        {
            //var u = new AppUserModel();
            //if (u.CanRead(brm))
            //{
            return View(brm);
            //}
            //else
            //{
            //    return new HttpUnauthorizedResult("you are not authorized to view the requested record.");
            //}
        }

    }
}
