using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Microsoft.Office.Project.Server;
using Microsoft.Office.Project.Server.Events;
using Microsoft.Office.Project.Server.Library;
using PsObjectSync;
using System.Diagnostics;

namespace PsEventHandlers
{
    public class CtsAdminEventHandler : AdminEventReceiver
    {
		// TODO: investigate dynamic current PWA Uri or use config file
		// see PSContextInfo.SiteGuid:
		// http://msdn.microsoft.com/en-us/library/microsoft.office.project.server.library.pscontextinfo.siteguid_di_pj14mref.aspx
		const string pwaUri = "http://ctsdev-pj10/pstest/_vti_bin/PSI/ProjectServer.svc";

		EventLog l = new EventLog();
		public override void OnAdSyncERPSynchronized(PSContextInfo contextInfo, AdSyncERPSynchronizedEventArgs e)
		{
			l.Source = "Application";
			try
			{
				// should base call come first or last?
				base.OnAdSyncERPSynchronized(contextInfo, e);
				l.WriteEntry("CtsAdminEventHandler trying update!");
				UpdateWorkResourceMetadata();
			}
			catch (Exception xcp)
			{
				l.WriteEntry("UpdateWorkResourceMetadata failed! " + xcp.InnerException.GetType().ToString() + ":" + xcp.Message);
			}
		}

		public void UpdateWorkResourceMetadata()
		{
			var ru = new ResourceUpdater(pwaUri)
			{
				// TODO: get firewall opened to Hyper-V server client.
				//OrgConnStr = "Data Source=db-dev.cts.wa.gov;Initial Catalog=Organization;Integrated Security=True",
				//OrgNavConnStr = "Data Source=db-dev.cts.wa.gov;Initial Catalog=OrganizationNavigator;Integrated Security=True"
				OrgConnStr = "Data Source=ctsl7f303059.dis.wa.lcl;Initial Catalog=Organization;Integrated Security=True",
				OrgNavConnStr = "Data Source=ctsl7f303059.dis.wa.lcl;Initial Catalog=OrganizationNavigator;Integrated Security=True"
			};
			l.WriteEntry("instantiated ResourceUpdater.");
			try
			{
				ru.UpdateWorkResources();
				l.WriteEntry("ResourceUpdater.UpdateWorkResources succeeded!");
			}
			catch (Exception xcp)
			{
				l.WriteEntry("ResourceUpdater.UpdateWorkResources failed! " + xcp.InnerException.GetType().ToString() + ":" + xcp.Message);
			}
		}
    }
}
