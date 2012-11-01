using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Diagnostics;

namespace OrgNavWebApi.Models
{
	// TODO: abstract base class
	// see CommonLibrary.NET RepositoryBase:
	// http://commonlibrarynet.codeplex.com/SourceControl/changeset/view/71149#478467
	public class AdoRepository<T> : IRepository<T> where T : class
	{
		public List<T> GetFromProc(string procName, Dictionary<string, object> paramHash)
		{
			var sp = new Util.SmartProc<T>(procName, paramHash);
			List<T> result = sp.All();
			return result;
		}

		public static bool Exec(string procName, Dictionary<string, object> paramHash)
		{
			return new Util.SmartProc<T>(procName, paramHash).Exec();
		}
	}
}