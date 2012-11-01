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
	// TODO: abstract base class?
	// see CommonLibrary.NET RepositoryBase:
	// http://commonlibrarynet.codeplex.com/SourceControl/changeset/view/71149#478467
	//public class AdoRepository<T> : IRepository<T> where T : class
	public class AdoRepository : IRepository
	{
		private string _connKey;
		public AdoRepository(string connKey)
		{
			_connKey = connKey;
		}

		public bool Exec(string procName, dynamic parameters)
		{
			//return new Util.SmartProc<T>(procName, parameters).Exec();
			return new Util.SmartProc(procName, _connKey, parameters).Exec();
		}

		public List<T> GetFromProc<T>(string procName, dynamic parameters)
		{
			//var sp = new Util.SmartProc<T>(procName, parameters);
			var sp = new Util.SmartProc(procName, _connKey, parameters);
			List<T> result = sp.All<T>();
			return result;
		}

		public T GetSingleFromProc<T>(string procName, dynamic parameters)
		{
			var r = GetFromProc<T>(procName, parameters);
			if (r.Count != 1) {
				throw new Exception("non-single result");
			}
			return r[0];
		}
	}
}