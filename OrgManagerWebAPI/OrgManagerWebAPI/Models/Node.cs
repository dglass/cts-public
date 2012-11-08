using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgManagerWebApi.Models
{
	public class Node
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Depth { get; set; }
		public List<Node> SubNodes = new List<Node>();
	}
}