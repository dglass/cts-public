using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ResourceAuthorizationDemo.Filters
{
    public class BaseResourceAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // first perform custom auth, if succeeds return base.AuthorizeCore:
            return IsAuthorized(httpContext) && base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new HttpUnauthorizedResult("unauthorized");
            // use Forbidden, not Unauthorized, to prevent login box:
            // EmptyResult() is required to prevent showing any content.
            // calling base.HandleUnauthorizedRequest(filterContext) will redirect to 401, cause login prompt.
            filterContext.Result = new EmptyResult();
            var rsp = filterContext.HttpContext.Response;
            //rsp.Status = "403 Forbidden"; // DEPRECATED
            rsp.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            // set substatus depending on REST request header? (no built-in enum, apparently):
            // http://support.microsoft.com/kb/943891
            // rsp.SubStatusCode = 2; // Read forbidden
            rsp.StatusDescription = "Forbidden Resource Request";
            // this changes Result to 401, ignoring 403 setting above:
            //base.HandleUnauthorizedRequest(filterContext);
        }

        private bool IsAuthorized(HttpContextBase c) {
            var h = (MvcHandler)c.CurrentHandler;
            // assumes any Resource access will include Id param (see BaseResourceModel)
            var id = h.RequestContext.RouteData.Values["Id"];
            //var controller = h.RequestContext.RouteData.Values["controller"];
            //var action = h.RequestContext.RouteData.Values["action"];
            var n = c.User.Identity.Name;
//            return (id != null && id.Equals("1234")); // dummy authorization condition
            //return (id == null || id.Equals("1234") || id.Equals("7777")); // dummy authorization condition, allows listing resources (empty id)
            return true; // until done with DemoAuthorizationProvider...
        }
    }
}