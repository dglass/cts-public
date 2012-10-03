using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgManager.Models
{
	/// <summary>
	/// provides node attributes for consumption by jsTree.
	/// property names should not be changed;
	/// they are intentionally lower case to match expected JSON.
	/// </summary>

	public class NodeAttr
	{
		// DRG 2011-12-26: commented out unused attributes for this particular use case...
		public int id { get; set; }
		//public bool complete { get; set; }
		public bool z { get; set; } // formerly known as 'lazy'
		public string e { get; set; } // email, for "rollover business card"
		public string i { get; set; } // image source, for "rollover business card"
		public string t { get; set; } // can't use 'title' since it is a reserved attribute (e.g. tooltip)...also, all attrs get lower-cased, so best start that way.
		public string n { get; set; } // employeename, to facilitate generating "rollover business card"
		public string p { get; set; } // phone num, to facilitate generating "rollover business card"
		//public string uri { get; set; }
		//public int typeid { get; set; }
		//public string note { get; set; }
	}
}