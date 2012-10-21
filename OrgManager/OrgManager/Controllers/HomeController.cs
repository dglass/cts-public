using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrgManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
			//var hou = new List<KeyValuePair<int,string>>() {
			//	new KeyValuePair<int,string>(1234, "First Item"),
			//	new KeyValuePair<int,string>(2345, "Second Item"),
			//};
			//ViewBag.HrmsOrgUnits = hou;
			ViewBag.HrmsOrgUnits = Models.OrgUnit.GetHrmsOrgUnits();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
