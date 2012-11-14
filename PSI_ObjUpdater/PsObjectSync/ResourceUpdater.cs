using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SvcCustomFields;
using SvcLookupTable;
using SvcResource;
using Microsoft.Office.Project.Server.Library;
using System.Xml;

namespace PsObjectSync
{
	public class ResourceUpdater : BasePsObjectSync
	{
		private ResourceClient _rc;
		private CustomFieldsClient _cfc;
		private Guid _resourceDeptsCfId; // custom field ID for Resource Departments
		private Dictionary<string, int> _empHash; // LanId to PersonId
		private Dictionary<string, int> _orgUnitHash; // PersonId to OrgUnitId
		private Dictionary<string, Guid> _deptHash; // (d[orgUnitId] = deptGuid) ouid is string, not int

		/// <summary>
		/// Updates Enterprise (Work) Resource ECFs from HRMS, etc.  Note, these exist only in Published db.
		/// </summary>
		/// <param name="pwaUri">Uri to ProjectServer.svc</param>
		public ResourceUpdater(string pwaUri) : base(pwaUri)
		{
			Init();
		}

		private void Init()
		{
			_rc = new ResourceClient(b, a);
			SetCredentials(_rc.ClientCredentials);
			_cfc = new CustomFieldsClient(b, a);
			SetCredentials(_cfc.ClientCredentials);
			_resourceDeptsCfId = GetCfIdByName("Resource Departments");
		}

		private void InitEmpHash()
		{
			// initialize empHash (LanId to HRMS ID dict):
			_empHash = new Dictionary<string, int>();

			using (var c = new SqlConnection(OrgConnStr))
			{
				const string q = @"select WindowsDomain + '\' + WindowsUserName as LanId,
					Id from Employee where IsActive=1";
				c.Open();
				var cmd = new SqlCommand
				{
					CommandText = q,
					CommandType = CommandType.Text,
					Connection = c
				};
				var sdr = cmd.ExecuteReader();
				while (sdr.Read())
				{
					// ToLower() IMPORTANT!
					_empHash[sdr.GetString(0).ToLower()] = sdr.GetInt32(1);
				}
			}
			// TO (NOT) DO: create OrgUnitId CF for Resource entity if not exists (used for linking / re-linking Department after changes)
			// no need since can look up current OrgUnit from HRMS (Organization) data.
		}

		// TODO: move this to static method of CfUtil?
		private Guid GetCfIdByName(string cfName)
		{
			var f = new Filter
			{
				Criteria = new Filter.FieldOperator(Filter.FieldOperationType.Equal, "MD_PROP_NAME", cfName),
				FilterTableName = "CustomFields"
			};
			f.Fields.Add(new Filter.Field("MD_PROP_NAME"));
			var fx = f.GetXml();
			var cfcds = _cfc.ReadCustomFields(fx, false);
			return cfcds.CustomFields[0].MD_PROP_UID;
		}

		public void UpdateWorkResources()
		{
			InitEmpHash();
			UpdateExternalIds();
		}

		// TODO: exclude Resources where RES_EXTERNAL_ID is already defined? (with force override option?)
		private void UpdateExternalIds()
		{
			// NOTE, built-in fields (e.g. ResourcesRow.RES_EXTERNAL_ID) can be bulk-updated in a single ResourceDataSet.
			// ResourceCustomFields, however, can apparently not.  They must be updated individually per-resource,
			// with a complete checkout / invoke web service / check-in cycle per Resource.

			// ReadResources: http://msdn.microsoft.com/en-us/library/office/gg221042(v=office.15).aspx
			//var xf = ResourceFilter.ActiveUsersOnly;
//			var xf = new Filter();
//			var rt = new ResourceDataSet.ResourcesDataTable(); // for metadata only
//			// *NOTE*, specifying FilterTableName appears to make returned dataset read-only due to null associated tables.
////			xf.FilterTableName = "Resources";
//			xf.FilterTableName = rt.TableName;
			// this should return only Resources with null RES_EXTERNAL_IDs:
			//xf.Criteria = new Filter.FieldOperator(Filter.FieldOperationType.IsNull,
				//rt.RES_EXTERNAL_IDColumn.ColumnName);

			// this should only process records with Empty (but not null) RES_EXTERNAL_IDs
			//xf.Criteria = new Filter.FieldOperator(Filter.FieldOperationType.Equal,
			//	rt.RES_EXTERNAL_IDColumn.ColumnName, String.Empty);
			//// add all columns:
			////foreach (DataColumn c in rt.Columns)
			////{
			//	//xf.Fields.Add(new Filter.Field(c.ColumnName));
			////}
			//xf.Fields.Add(new Filter.Field(rt.RES_EXTERNAL_IDColumn.ColumnName));
			//xf.Fields.Add(new Filter.Field(rt.WRES_ACCOUNTColumn.ColumnName));
			//xf.Fields.Add(new Filter.Field(rt.RES_CHECKOUTBYColumn.ColumnName));
			//var xfs = xf.GetXml();
			//var rds = _rc.ReadResources(xfs, false);
			var rds = _rc.ReadResources(String.Empty, false);
			var rcount = rds.Resources.Count;
			var empUids = new List<Guid>();
			var checkedOutUids = new List<Guid>();
			foreach (ResourceDataSet.ResourcesRow r in rds.Resources)
			{
				if (!r.IsWRES_ACCOUNTNull() && _empHash.ContainsKey(r.WRES_ACCOUNT.ToLower()))
				{
					empUids.Add(r.RES_UID);
					if (!r.IsRES_CHECKOUTBYNull())
					{
						checkedOutUids.Add(r.RES_UID);
					}
					r.RES_EXTERNAL_ID = _empHash[r.WRES_ACCOUNT.ToLower()].ToString(CultureInfo.InvariantCulture);
				}
			}
			// force check-in for any checked-out resources before checking all out to self:
			_rc.CheckInResources(checkedOutUids.ToArray(), true);
			var empids = empUids.ToArray();
			_rc.CheckOutResources(empids);
			if (rds.HasChanges())
			{
				try
				{
					// rds fails to update with GeneralDataTableCannotBeNull exception
					// when using filtered query. (apparently due to missing CalendarExceptions, etc.)
					_rc.UpdateResources(rds, false, true); // auto check-in
				}
				catch (System.ServiceModel.FaultException fx)
				{
					PSClientError psClientError;
					var ft = fx.CreateMessageFault();
					if (ft.HasDetail)
					{
						using (var xmlReader = ft.GetReaderAtDetailContents())
						{
							var xml = new XmlDocument();
							xml.Load(xmlReader);
							psClientError = new PSClientError(xml["ServerExecutionFault"]["ExceptionDetails"].InnerXml);
						}
					}
				}
			}
			// *NOTE*, auto check-in is being used.
			//_rc.CheckInResources(empUids.ToArray(), true);
		}


	}
}