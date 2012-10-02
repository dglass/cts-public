using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgManager.Controllers
{
    public class OrgUnitController : Controller
    {
        //
        // GET: /OrgUnit/
        // this is intended only for XHR consumption:
        public ActionResult GetTree()
        {
            return new JsonResult()
            {
                // *note*, this returns a List<JsTreeNode> of one root node.  multi-roots are also possible.
                Data = Models.JsTreeNode.GetDummyTree()
            };
        }

    }
}
