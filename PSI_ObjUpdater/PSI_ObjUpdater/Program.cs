using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PsObjectSync;
using System.Diagnostics;
using PsEventHandlers;

namespace PSI_ObjUpdater
{
	class Program
	{
		const string pwaUri = "http://ctsdev-pj10/pstest/_vti_bin/PSI/ProjectServer.svc";
		static void Main(string[] args)
		{
			//var ru = new ResourceUpdater(pwaUri)
			//{
			//	OrgConnStr = "Data Source=db-dev.cts.wa.gov;Initial Catalog=Organization;Integrated Security=True",
			//	OrgNavConnStr = "Data Source=db-dev.cts.wa.gov;Initial Catalog=OrganizationNavigator;Integrated Security=True"
			//};
			//var sw = new Stopwatch();
			//sw.Start();
			//ru.UpdateWorkResources();
			//var et = sw.ElapsedMilliseconds; // takes 3-6 seconds.
			//sw.Stop();
			var ah = new CtsAdminEventHandler();
			ah.UpdateWorkResourceMetadata();
		}
	}
}
