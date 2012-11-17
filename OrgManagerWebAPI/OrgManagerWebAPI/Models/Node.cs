using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgManagerWebApi.Models
{
	// TODO: split out OrgUnitNode, OrgUnit.
	public class Node
	{
		public int Id { get; set; }
		public int HrmsOrgUnitId { get; set; }
		public string HrmsOrgUnitDescription { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }
		public int Depth { get; set; }
		public int FilledPositions { get; set; }
		public int PositionCount { get; set; }
		// SubNodes has been replaced by ViewModel children property.
		//public List<Node> SubNodes = new List<Node>();

		// these are update-only properties:
		// TODO: Action Enum
		public string UpdateAction { get; set; }
		public int? ParentId { get; set; }
		public int? SibIndex { get; set; }

	}
}