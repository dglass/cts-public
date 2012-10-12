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
		public string ShortName { get; set; } // *note*, Name is in base class since all Nodes should have names.
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

		public void LoadDetail()
		{
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				// returns Id, Name, Depth
				SqlCommand cmd = new SqlCommand("select ShortName from OrgUnitCTS where Id = @Id", c);
				cmd.Parameters.Add(new SqlParameter("@Id", Id));
				var r = cmd.ExecuteReader(CommandBehavior.SingleResult);
				// TODO: direct population of tree structure without pre-loading tolist?
				if (r.Read())
				{
					ShortName = r.IsDBNull(0) ? "" : r.GetString(0);
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
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				var cmd = new SqlCommand();
				cmd.Connection = c;
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@Id", Id));
				if (ShortName == null)
				{
					cmd.CommandText = "RenameOrgUnitCTS";
					// TODO: dynamic parameterization and/or dynamic update query...
					cmd.Parameters.Add(new SqlParameter("@Name", Name));
				}
				else
				{
					cmd.CommandText = "UpdateOrgUnitCTS";
					cmd.Parameters.Add(new SqlParameter("@ShortName", ShortName));
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