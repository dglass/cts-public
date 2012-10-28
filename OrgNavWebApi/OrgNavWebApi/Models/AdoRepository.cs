using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace OrgNavWebApi.Models
{
	// TODO: abstract base class
	// see CommonLibrary.NET RepositoryBase:
	// http://commonlibrarynet.codeplex.com/SourceControl/changeset/view/71149#478467
	public class AdoRepository<T> : IRepository<T> where T : class
	{
		//public ViewModels.PositionNodeVm GetAll()
		public List<object> GetAll()
		{
			return new List<object> { Get(0) }; // TODO: get all actual roots from db...
		}

		//public ViewModels.PositionNodeVm Get(object id)
		public object Get(object id)
		{
			var model = this.GetType().GenericTypeArguments[0];
			if (model == typeof(ViewModels.PositionNodeVm))
			{
				return GetPositionTree((int)id);
			}
			else
			{
				return null;
			}
		}

		private ViewModels.PositionNodeVm GetPositionTree(int id)
		{
			int uid = 1; // TODO: look this up from authenticated windows user...
			var root = new Models.PositionNode
			{
				Id = 1,
				ParentId = null,
				Name = "Root Node",
				IsExpanded = true
			};
			var connstr = WebConfigurationManager.ConnectionStrings["OrganizationNavigatorContext"].ConnectionString;
			using (SqlConnection c = new SqlConnection(connstr))
			{
				c.Open();
				// returns Id, Name, Depth
				SqlCommand cmd = new SqlCommand("GetPositionTree", c);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add(new SqlParameter("@RootPosId", id));
				cmd.Parameters.Add(new SqlParameter("@AppUserId", uid));
				var r = cmd.ExecuteReader(CommandBehavior.SingleResult);
				// TODO: direct population of tree structure without pre-loading to list?
				var st = r.GetSchemaTable();
				var rs = new DataTable(); // resultset
				foreach (DataRow colinfo in st.Rows)
				{
					rs.Columns.Add(
						new DataColumn(colinfo["ColumnName"].ToString(),
						(Type)colinfo["DataType"])
					);
				}
				while (r.Read())
				{
					var nr = rs.NewRow();
					foreach (DataColumn col in rs.Columns)
					{
						nr[col] = r.GetValue(rs.Columns.IndexOf(col));
					}
					rs.Rows.Add(nr);
					//NodeList.Add(new OrgUnit()
					//{
						//Id = r.GetInt32(0),
						//Name = r.GetString(1),
						//Level = r.GetInt32(2) // "depth" column name
					//});
				}
				//root.SubNodes = DataTableToObjectList<PositionNode>(rs);
				root.SubNodes = TreesFromList(DataTableToObjectList<PositionNode>(rs));
			}
			// TODO: move treebuilding, wrapping into Vm:
			return new Models.ViewModels.PositionNodeVm(root);
		}

		// TODO: make this List<NodeBase>...
		private List<PositionNode> TreesFromList(List<PositionNode> l)
		{
			var tl = new List<PositionNode>(); // holds tree-sorted list of root nodes
			var nt = new List<PositionNode>(); // nesting tracker

			tl.Add(l[0]); // first element is always root
			nt.Add(l[0]);
			int rootlevel = l[0].depth;
			int level = rootlevel;

			for (int i = 1; i < l.Count; i++)
			{
				var n = l[i];
				var depth = n.depth;
				if (depth > level)
				{
					if (nt.Count >= depth - rootlevel + 1)
					{
						nt[depth - rootlevel] = n;
					}
					else
					{
						nt.Add(n);
					}
					nt[level - rootlevel].SubNodes.Add(nt[depth - rootlevel]);
				}
				else if (depth <= level && depth > rootlevel)
				{
					nt[depth - rootlevel] = n;
					nt[depth - rootlevel - 1].SubNodes.Add(n);
				}
				else if (depth == rootlevel)
				{
					// this should only happen if multiple roots are allowed.
					tl.Add(n);
					nt[0] = n;
				}
				level = depth;
			}
			return tl;
		}

		private List<T> DataTableToObjectList<T>(DataTable dt)
		{
			var l = new List<T>();
			foreach (DataRow dr in dt.Rows)
			{
				var obj = Activator.CreateInstance<T>();
				var t = obj.GetType();
				foreach (DataColumn c in dt.Columns)
				{
					// TODO: convention that object property names, types match sproc result column names, types?
					var p = t.GetProperty(c.ColumnName);
					if (p != null)
					{
						// TODO: type map to convert from dr[c] SqlDbType to .NET type? (seems to auto-convert ok)
						p.SetValue(obj, dr[c]);
					}
				}
				l.Add(obj);
			}
			return l;
		}
	}
}