using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgNavWebApi.Models
{
	public class PositionNode
	{
		public int Id { get; set; }
		// int? is shorthand for Nullable<Int32>
		// http://stackoverflow.com/questions/4028830/c-sharp-nullableint-vs-int-is-there-any-difference
		public int? ParentId { get; set; }
		public string Name { get; set; } // TODO: PositionName, PersonName, OrgUnitName...
		public bool IsExpanded { get; set; }
		public int depth { get; set; } // used for non-recursive tree construction

		public List<PositionNode> SubNodes { get; set; }

		public PositionNode()
		{
			SubNodes = new List<PositionNode>();
		}
	}
}