using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace OrgManager.Models
{
	public class OrgUnit : BaseNodeModel
	{
		public void LoadFromDb() {
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				// returns Id, Name, Depth
				SqlCommand cmd = new SqlCommand("GetOrgUnitTreeCTS", c);
				cmd.CommandType = CommandType.StoredProcedure;
				var r = cmd.ExecuteReader(CommandBehavior.SingleResult);
				// TODO: direct population of tree structure without pre-loading to list?
				while (r.Read())
				{
					NodeList.Add(new OrgUnit()
					{
						Id = r.GetInt32(0),
						Name = r.GetString(1),
						Level = r.GetInt32(2) // "depth" column name
					});
				}
			}
			if (NodeList.Count > 0)
			{
				BuildTree(NodeList);
			}
		}
	}
}