using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgManager.Models
{
	public class BaseResource
	{
		public BaseResource()
		{
			// TODO: configurable route root rather than using model type name convention
			Context = HttpContext.Current;
			// this is to persist type name through downcasts for later redirection
			ModelType = GetType();
			Loaded = Created = Updated = Deleted = false; // initialize action result properties
		}

		public HttpContext Context { get; set; }
		public int Id { get; set; } // TODO: should Id be string or int?  thinking string.
		// TODO: turn Id to int?
		//public string Name { get; set; }

		// these properties are intended to be set by subclasses
		// to indicate success or failure of update/delete prior to calling base controller method
		public bool Created { get; set; }
		public bool Deleted { get; set; }
		public bool Updated { get; set; }
		public bool Loaded { get; set; }

		// this is instead of GetType to allow retaining type info during a model downcast.
		public Type ModelType { get; set; } // used for redirection to Model controller (uses convention of shared name)
		public IEnumerable<object> DummyModels { get; set; }
	}
}