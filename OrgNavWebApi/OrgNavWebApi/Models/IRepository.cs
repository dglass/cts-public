using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNavWebApi.Models
{
	public interface IRepository<T>
	{
		// use these if subclassing specific repository interfaces:
		//T Get(object id);
		//T GetAll();
		object Get(object id);
		List<object> GetAll();
	}
}
