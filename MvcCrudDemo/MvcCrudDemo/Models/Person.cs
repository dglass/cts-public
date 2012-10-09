using System;
using System.Collections.Generic;
using MvcCrudDemo.Models;

namespace MvcCrudDemo.Models
{
    public class Person : CachedResource
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

		public void LoadDummyModels()
		{
			if (DummyModels == null)
			{
				DummyModels = new List<object> {
					new Person { Id = Guid.NewGuid(), FirstName = "Joe", LastName = "Blow" },
					new Person { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe" }
				};
			}
		}

        // TODO: LoadedProperties collection? rename Load to LoadProperties ?
		// TODO: populate non-Id Properties from cache using reflection?
		// http://msdn.microsoft.com/en-us/library/f7ykdhsy.aspx
		public override void Load() // populates record detail from cache
        {
            var cached = (Person)((Dictionary<Guid,object>)Context.Cache[ModelType.Name])[Id];
            FirstName = cached.FirstName;
            LastName = cached.LastName;
            Loaded = true;
        }
    }
}