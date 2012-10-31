using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Configuration;

namespace OrgNavWebApi.Models.Util
{
	public class SmartProc<T>
	{
		public string ConnStr { get; set; }
		public string ProcName { get; set; }
		public SqlParameterCollection Params { get; private set; }
		private SqlCommand _cmd;
		private Dictionary<string,object> _paramHash;
		// TODO: consider this utility class:
		// https://gist.github.com/858392#file_sql_db_type2_db_type.cs
		private Dictionary<Type, Type> _netToSqlType = new Dictionary<Type, Type>()
		{
			{typeof(bool), SqlDbType.Bit.GetType()},
			{typeof(byte), SqlDbType.TinyInt.GetType()},
			{typeof(byte[]), SqlDbType.Image.GetType()},
			{typeof(DateTime), SqlDbType.DateTime.GetType()},
			{typeof(Decimal), SqlDbType.Decimal.GetType()},
			{typeof(double), SqlDbType.Float.GetType()},
			{typeof(Guid), SqlDbType.UniqueIdentifier.GetType()},
			{typeof(Int16), SqlDbType.SmallInt.GetType()},
			{typeof(Int32), SqlDbType.Int.GetType()},
			{typeof(Int64), SqlDbType.BigInt.GetType()},
			{typeof(object), SqlDbType.Variant.GetType()},
			{typeof(string), SqlDbType.VarChar.GetType()}
		};

		public SmartProc (string procName, Dictionary<string,object> paramHash) {
			// TODO: use ObjectContext for this?
			ConnStr = WebConfigurationManager.ConnectionStrings["OrganizationNavigatorContext"].ConnectionString;

			_cmd = new SqlCommand(procName);
			_cmd.CommandType = CommandType.StoredProcedure;
			_paramHash = paramHash;
			// this sets param values directly by name; doesn't pre-query for proc param metadata
			foreach (var k in paramHash.Keys)
			{
				_cmd.Parameters.Add(new SqlParameter("@" + k, _netToSqlType[paramHash[k].GetType()]));
				_cmd.Parameters["@" + k].Value = paramHash[k];
			}
		}

		public List<T> All() 
		{
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				_cmd.Connection = c;
				c.Open();
				var r = _cmd.ExecuteReader(CommandBehavior.SingleResult);
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
				}
				return DataTableToObjectList<T>(rs);
			}
		}

		private List<TPriv> DataTableToObjectList<TPriv>(DataTable dt)
		{
			var l = new List<TPriv>();
			foreach (DataRow dr in dt.Rows)
			{
				var obj = Activator.CreateInstance<TPriv>();
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