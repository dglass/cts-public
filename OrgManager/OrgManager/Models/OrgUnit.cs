using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace OrgManager.Models
{
	public class OrgUnit : BaseNodeModel
	{
		public string ShortName { get; set; } // *note*, Name is in base class since all Nodes should have names.
		public string Code { get; set; } //  2-5 character Code for constructing RBS tree, etc.
		public int? HrmsOrgUnit { get; set; }

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

		public static List<KeyValuePair<int,string>> GetHrmsOrgUnits() {
			var units = new List<KeyValuePair<int, string>>();
			// using second conn string for Org vs. OrgNav db:
			using (SqlConnection c = new SqlConnection(WebConfigurationManager.ConnectionStrings[1].ConnectionString))
			{
				c.Open();
				SqlCommand cmd = new SqlCommand("select Id, OrgUnitDescription from OrgUnit order by OrgUnitDescription", c);
				var rs = cmd.ExecuteReader(CommandBehavior.SingleResult);
				while (rs.Read())
				{
					units.Add(new KeyValuePair<int, string>(rs.GetInt32(0), rs.GetString(1)));
				}
				return units;
			}
		}

		public void LoadDetail()
		{
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				SqlCommand cmd = new SqlCommand("select ShortName, Code, HrmsOrgUnitId, Name from OrgUnitCTS where Id = @Id", c);
				cmd.Parameters.Add(new SqlParameter("@Id", Id));
				var r = cmd.ExecuteReader(CommandBehavior.SingleRow);
				// TODO: direct population of tree structure without pre-loading tolist?
				if (r.Read())
				{
					ShortName = r.IsDBNull(0) ? "" : r.GetString(0);
					Code = r.IsDBNull(1) ? "" : r.GetString(1);
					if (!r.IsDBNull(2))
					{
						HrmsOrgUnit = r.GetInt32(2);
					}
					Name = r.IsDBNull(3) ? "" : r.GetString(3);
				}
			}
		}

		/// <summary>
		///  this Creates a subnode of the executing model...
		/// </summary>
		/// <returns>newly created OrgUnit</returns>
		public OrgUnit Create()
		{
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				// TODO: write this proc...need triggers on OrgUnitNodeCTS too...
				SqlCommand cmd = new SqlCommand("AddOrgUnitCTS", c);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@ParentId", Id));
				var r = cmd.ExecuteReader(CommandBehavior.SingleResult);
				if (r.Read())
				{
					return new OrgUnit
					{
						Id = r.GetInt32(0),
						Name = r.GetString(1)
					};
				}
				else
				{
					throw new Exception("procedure failed...");
				}
			}
		}

		public bool Update()
		{
			// TODO: only update fields from incoming model,
			// *or* include FieldsToUpdate property...?  or match certain properties with certain update code...
			// decided to add Action property to model to support multiple update methods.
			// is that cheating on keeping it RESTful?  It still requires an HTTP PUT, at least.
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				var cmd = new SqlCommand();
				cmd.Connection = c;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@Id", Id));
				if (UpdateAction == UpdateActions.Move) // treat as move request...
				{
					cmd.CommandText = "MoveOrgUnitCTS";
					cmd.Parameters.Add(new SqlParameter("@ParentId", ParentId));
					cmd.Parameters.Add(new SqlParameter("@SibIndex", SibIndex));
				}
				else if (UpdateAction == UpdateActions.Rename) // treat as rename only
				{
					cmd.CommandText = "RenameOrgUnitCTS";
					// TODO: dynamic parameterization and/or dynamic update query...
					cmd.Parameters.Add(new SqlParameter("@Name", Name));
				}
				else if (UpdateAction == UpdateActions.Update) // update all OrgUnitCTS fields (detail form submission) but *not* node structure
				{
					cmd.CommandText = "UpdateOrgUnitCTS";
					// TODO: Reflect properties, params...
					cmd.Parameters.Add(new SqlParameter("@ShortName", ShortName));
					cmd.Parameters.Add(new SqlParameter("@Code", Code));
					cmd.Parameters.Add(new SqlParameter("@HrmsOrgUnitId", HrmsOrgUnit));
				}
				return (cmd.ExecuteNonQuery() == 1); // == successful update
			}
		}

		public bool Delete()
		{
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				var cmd = new SqlCommand("DeleteOrgUnitCTS", c);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@Id", Id));
				return (cmd.ExecuteNonQuery() == 1); // == successful update
			}
		}
	}
}