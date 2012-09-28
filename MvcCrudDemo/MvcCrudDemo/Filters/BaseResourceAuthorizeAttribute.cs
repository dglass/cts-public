using System.Web;
using System.Web.Mvc;
using MvcCrudDemo.Models;

namespace MvcCrudDemo.Filters
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
            // use Forbidden, not Unauthorized, to prevent triggering login box:
            // TODO: redirect to friendly error page since unauthorized could also apply to update, delete, create.
            // calling base.HandleUnauthorizedRequest(filterContext) will redirect to 401, cause login prompt.
            //filterContext.Result = new EmptyResult();
            var rsp = filterContext.HttpContext.Response;
            //rsp.Status = "403 Forbidden"; // DEPRECATED
            rsp.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            // set substatus depending on REST request header? (no built-in enum, apparently):
            // http://support.microsoft.com/kb/943891
            // rsp.SubStatusCode = 2; // Read forbidden
            rsp.StatusDescription = "Forbidden Resource or Action";
            // this changes Result to 401, ignoring 403 setting above:
            //base.HandleUnauthorizedRequest(filterContext);
            filterContext.Result = new ViewResult { ViewName = "Error" }; // returning general error for now
        }

        protected bool IsAuthorized(HttpContextBase c)
        {
            var u = new BaseAppUser(new BaseAuthorizationProvider(c));
            return u.Can();
        }
    }
}