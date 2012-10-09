using System;
using System.Web.Mvc;
using MvcCrudDemo.Filters;
using MvcCrudDemo.Models;

namespace MvcCrudDemo.Controllers
{
    // applying Authorize attribute at class level here, can also be applied per-method.
    [BaseResourceAuthorize]
    public class PersonController : CachedResourceController
    {
        //
        // GET: /Person/

        // *note*, method name must be unique among public class hierarchy.
        // MVC does not match signatures when resolving methods.
        // might want to use "PersonDetail" here.
        public ActionResult PersonDetail(Person p)
        {
            // TODO: EditMode property in BaseResource?
            p.Load();
            return View(p);
        }

        public ActionResult PersonEdit(Person p)
        {
            p.Load();
            return View(p);
        }

        public ActionResult PersonNew(Person p)
        {
            return View(p);
        }
        
        public ActionResult PersonList(Person p)
        {
			p.LoadDummyModels(); // will 
            return View(p.List());
        }

        [HttpPut]
        [ActionName("PersonDetail")]
        public ActionResult Update(Person p)
        {
            ViewData.Model = p;
            p.Update();
            return Update();
        }

        [HttpPost]
        [ActionName("PersonList")]
        public ActionResult Create(Person p)
        {
            ViewData.Model = p;
            p.Create();
            return Create();
        }

        [HttpDelete]
        [ActionName("PersonDetail")]
        public ActionResult Delete(Person p)
        {
            ViewData.Model = p;
            p.Delete();
            return Delete();
        }

    }
}
