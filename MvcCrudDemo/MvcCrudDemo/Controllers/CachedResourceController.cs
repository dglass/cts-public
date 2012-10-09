using System;
using System.Web.Mvc;
using MvcCrudDemo.Filters;
using MvcCrudDemo.Models;

namespace MvcCrudDemo.Controllers
{
    public class CachedResourceController : BaseResourceController
    {
        //
        // GET: /resource

		// this is not an override since that would require a parameter downcast.
		// base method is no-arg now anyway.
		// *NOTE*, consider replacing Index with Get() and List()...
		// TODO: check redirect to ModelType.Name Route before doing this.
        public ActionResult Index(CachedResource cr)
        {
			// TODO: set different View for List and single-item detail...current view changes on model type.
			// then call base.List() or base.Get()

			if (cr.Id == Guid.Empty)
			{
				ViewData.Model = cr.List();
			}
			else
			{
				ViewData.Model = cr;
				cr.Load();
			}
            //return Index(); // base.Index()
            return View();
        }

		[HttpPost]
		[ActionName("Index")]
		public ActionResult Create(CachedResource cr)
		{
			ViewData.Model = cr;
			cr.Create();
			return Create();
		}

        [HttpPut]
        [ActionName("Index")]
        public ActionResult Update(CachedResource cr)
        {
            ViewData.Model = cr;
            cr.Update();
            return Update();
        }

		[HttpDelete]
		[ActionName("Index")]
		public ActionResult Delete(CachedResource cr)
		{
			ViewData.Model = cr;
			cr.Delete();
			return Delete();
		}

		public ActionResult Edit(CachedResource cr)
		{
			ViewData.Model = cr;
			cr.Load();
			//return Edit();
		    return View();
		}

        public ActionResult New()
        {
            //return base.New();
            return View();
        }
    }
}
