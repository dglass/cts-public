using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace OrgNavWebApi.Models.Util
{
	// TODO: consider <P,T> to pass ParamCollectionTypes...
	public class SmartProc<T> where T : class
	{
		public string ConnStr { get; set; }
		public string ProcName { get; set; }
	//	public System.Data.IDataParameterCollection Params { get; private set; }
		public SqlParameterCollection Params { get; private set; }
		private SqlCommand _cmd;
		public void Init()
		{
			//var sw = new Stopwatch();
			//sw.Start();
			using (SqlConnection c = new SqlConnection(ConnStr))
			{
				c.Open();
				_cmd = new SqlCommand(ProcName, c);
				_cmd.CommandType = CommandType.StoredProcedure;
				Params = _cmd.Parameters;
				FillSchema(c);
			}
			//var et = sw.ElapsedMilliseconds; // ~ 8 ms after connection pooled...
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

		private void FillSchema(SqlConnection c)
		{
			//using (SqlConnection c = new SqlConnection(ConnStr))
			//{
				//c.Open();
				var sch = c.GetSchema("ProcedureParameters", new string[]{
					c.Database,
					null,
					ProcName});
				foreach (DataRow pr in sch.Rows)
				{
					Params.Add(new SqlParameter(pr["PARAMETER_NAME"].ToString(),
						GetSqlDbType(pr["DATA_TYPE"].ToString())));
				}
			//}
		}

		private SqlDbType GetSqlDbType(string type) {
			switch (type)
			{
				case "bigint": return SqlDbType.BigInt;
				case "binary": return SqlDbType.Binary;
				case "bit": return SqlDbType.Bit;
				case "char": return SqlDbType.Char;
				case "date": return SqlDbType.Date;
				case "datetime": return SqlDbType.DateTime;
				case "datetime2": return SqlDbType.DateTime2;
				case "datetimeoffset": return SqlDbType.DateTimeOffset;
				case "decimal": return SqlDbType.Decimal;
				case "float": return SqlDbType.Float;
				case "image": return SqlDbType.Image;
				case "int": return SqlDbType.Int;
				case "money": return SqlDbType.Money;
				case "nchar": return SqlDbType.NChar;
				case "ntext": return SqlDbType.NText;
				case "nvarchar": return SqlDbType.NVarChar;
				case "real": return SqlDbType.Real;
				case "smalldatetime": return SqlDbType.SmallDateTime;
				case "smallint": return SqlDbType.SmallInt;
				case "smallmoney": return SqlDbType.SmallMoney;
				case "structured": return SqlDbType.Structured;
				case "text": return SqlDbType.Text;
				case "time": return SqlDbType.Time;
				case "timestamp": return SqlDbType.Timestamp;
				case "tinyint": return SqlDbType.TinyInt;
				case "udt": return SqlDbType.Udt;
				case "uniqueidentifier": return SqlDbType.UniqueIdentifier;
				case "varbinary": return SqlDbType.VarBinary;
				case "varchar": return SqlDbType.VarChar;
				case "variant": return SqlDbType.Variant;
				case "xml": return SqlDbType.Xml;
				default: throw new Exception(String.Format("{0} db type not found.", type));
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