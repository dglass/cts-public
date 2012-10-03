using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace OrgManager.Controllers
{
    public class OrgUnitController : Controller
    {
        //
        // GET: /OrgUnit/
        // this is intended only for XHR consumption:
        public ActionResult GetTree(Models.OrgUnit ou)
        {
			//var sw = new Stopwatch();
			//sw.Start();
			ou.LoadFromDb();
			//var et = sw.ElapsedMilliseconds; // 48, 11, 8, 6, 6 in 5 subsequent refreshes
            var jr = new JsonResult()
            {
                // *note*, this returns a List<JsTreeNode> of one root node.  multi-roots are also possible.
                //Data = Models.JsTreeNode.GetDummyTree()
				Data = new Models.JsTreeNode(ou)
            };
			return jr;
        }

    }
}
