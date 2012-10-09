using System;
using System.Collections.Generic;

namespace MvcCrudDemo.Models
{
	public class CachedResource : BaseResource
	{
		public Dictionary<Guid, object> List()
		{
			// initialize cache if necessary:
			Context.Cache[ModelType.Name] = Context.Cache[ModelType.Name] ?? new Dictionary<Guid, object>();
			var ccr = (Dictionary<Guid, object>)Context.Cache[ModelType.Name];
			if (ccr.Count < 1)
			{
				// model list is now empty...this populates it with DummyModels, loaded by controller if necessary:
				foreach (var model in DummyModels)
				{
					ccr[((BaseResource)model).Id] = model;
				}
			}
			return ccr;
		}

		// TODO: LoadedProperties collection? rename Load to LoadProperties ?
		// convert this to interface?
		public virtual void Load()
		{
		}

		public bool Create()
		{
			if (Context.Cache[ModelType.Name] == null)
			{
				Context.Cache[ModelType.Name] = new Dictionary<Guid, CachedResource>();
			}
			var resources = (Dictionary<Guid, object>)Context.Cache[ModelType.Name];
			Id = Guid.NewGuid();
			resources[Id] = this;
			Created = true;
		    return Created;
		}

		public bool Update()
		{
			// TODO: consider fully-qualified type name as key to avoid namespace conflicts:
			((Dictionary<Guid, object>)Context.Cache[ModelType.Name])[Id] = this;
			Updated = true;
			return Updated;
		}

		// NOTE, CRD actions return bool in addition to setting property for BaseResourceController.
		public bool Delete()
		{
			// remove self from cache:
			Deleted = ((Dictionary<Guid, object>)Context.Cache[ModelType.Name]).Remove(Id);
			return Deleted;
		}
	}
}