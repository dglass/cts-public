using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Configuration;

namespace OrgNavWebApi.Models.Util
{
	public class SmartProc
	{
		public string ConnStr { get; set; }
		public string ProcName { get; set; }
		public SqlParameterCollection Params { get; private set; }
		private SqlCommand _cmd;
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

		public SmartProc(string procName, string connKey, dynamic parameters)
		{
			var t = parameters.GetType();
			ConnStr = WebConfigurationManager.ConnectionStrings[connKey].ConnectionString;
			_cmd = new SqlCommand(procName);
			_cmd.CommandType = CommandType.StoredProcedure;
			// this sets param values directly by name; doesn't validate against proc metadata
			// will throw runtime error if params are missing / wrong type
			foreach (PropertyInfo prop in t.GetProperties())
			{
				var val = prop.GetValue(parameters);
				_cmd.Parameters.Add(new SqlParameter("@" + prop.Name, _netToSqlType[val.GetType()]));
				_cmd.Parameters["@" + prop.Name].Value = val;
			}
		}

		public bool Exec() // aka "non-query" - could be Create, Update, Insert, etc.
		{
			using (SqlConnection c = new SqlConnection(ConnStr)) {
				_cmd.Connection = c;
				c.Open();
				var result = _cmd.ExecuteNonQuery();
				return (result > 0);
			}
		}

		public List<T> All<T>() 
		{
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				_cmd.Connection = c;
				c.Open();

				// TODO: consider SqlDataAdapter here?
				var r = _cmd.ExecuteReader(CommandBehavior.SingleResult);
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
					// http://msdn.microsoft.com/en-us/library/kyaxdd3x.aspx BindingFlags usage
					var p = t.GetProperty(c.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
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