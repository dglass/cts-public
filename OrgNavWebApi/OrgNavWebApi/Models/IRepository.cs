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
		//List<T> Get(object id);
		List<T> GetFromProc(string procName, Dictionary<string,object> paramHash);
	}
}
