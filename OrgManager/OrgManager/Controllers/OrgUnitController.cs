using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using OrgManager.Models;
using System.Net;

namespace OrgManager.Controllers
{
	// TODO: create, implement an ICrudController.
	// e.g. https://github.com/pollingj/Membrane-CMS/blob/master/Membrane.Commons/Plugin/Controllers/ICRUDController.cs
    public class OrgUnitController : Controller
    {
		//
		// GET: /OrgUnit/
		// this is intended only for XHR consumption:
		[HttpGet]
		[ActionName("NodeOp")]
		public ActionResult GetNodeJson(OrgUnit ou)
		{
			//var sw = new Stopwatch();
			//sw.Start();
			ou.LoadFromDb();
			//var et = sw.ElapsedMilliseconds; // 48, 11, 8, 6, 6 in 5 subsequent refreshes
			var jr = new JsonResult()
			{
				// *note*, this returns a List<JsTreeNode> of one root node.  multi-roots are also possible.
				//Data = Models.JsTreeNode.GetDummyTree()
				Data = new Models.JsTreeNode(ou),
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
			return jr;
		}

		[HttpGet]
		[ActionName("DeptOp")]
		public ActionResult GetDetailJson(OrgUnit ou)
		{
			ou.LoadDetail();
			// trying returning self without specifying Json...
			// TODO: AjaxAttribute ? to tag properties for Json serialization.
			var jr = new JsonResult()
			{
				Data = ou,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
			return jr;
		}

		// TODO: add authorization code from MvcCrudDemo...
		[HttpPut]
		[ActionName("DeptOp")]
		[ValidateInput(false)] // to allow HTML input
		public ActionResult Update(OrgUnit ou)
		{
			if (ou.Update())
			{
				// return only status on success.
				Response.StatusCode = (int)HttpStatusCode.OK;
				return new EmptyResult();
			}
			// TODO: this should return JsonError somehow...
			return new ViewResult() { ViewName = "Error" };
		}

		[HttpPost]
		[ActionName("DeptOp")]
		public ActionResult Create(OrgUnit ou)
		{
			var jr = new JsonResult
			{
				Data = new JsTreeNode(ou.Create())
			};
			return jr;
		}

		[HttpDelete]
		[ActionName("DeptOp")]
		public ActionResult Delete(OrgUnit ou)
		{
			if (ou.Delete())
			{
				Response.StatusCode = (int)HttpStatusCode.OK;
				return new EmptyResult();
			}
			// TODO: this should return JsonError somehow...
			return new ViewResult() { ViewName = "Error" };
		}

    }
}
