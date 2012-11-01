using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNavWebApi.Models
{
	public interface IRepository
	{
		// Exec could be made a static Repository method, but there's not much point.
		// http://www.jagregory.com/writings/static-method-abuse/
		bool Exec(string procName, dynamic parameters);
		List<T> GetFromProc<T>(string procName, dynamic parameters);
		T GetSingleFromProc<T>(string procName, dynamic parameters);
	}
}
