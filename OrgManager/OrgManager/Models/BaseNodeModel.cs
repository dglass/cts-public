using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgManager.Models
{
	public class BaseNodeModel : BaseDbModel
	{
		public int Level { get; set; } // TODO: remove? should probably be implicit.
		//public int ParentId { get; set; } // UNNEEDED, implicit in Subnodes.
		public string Name { get; set; }
		public List<BaseNodeModel> NodeList = new List<BaseNodeModel>(); // holds tree-sorted resultsets
		private List<BaseNodeModel> _subnodes = new List<BaseNodeModel>();
		public List<BaseNodeModel> SubNodes
		{
			get { return _subnodes; }
		}

		/// <summary>
		/// builds hierarchical structure from sorted tree list.
		/// by convention, first element of list will be root (Level 0)
		/// </summary>
		/// <param name="nodelist">sorted list of tree nodes, first item is root</param>
		public void BuildTree(List<BaseNodeModel> nodelist)
		{
			// Id is set automatically by framework.
			Name = nodelist[0].Name;
			Level = nodelist[0].Level; // root is Level 1, not 0.
			var nt = new List<BaseNodeModel>(); // Nesting Tracker
			nt.Add(this);

			// setting these to 1 because we've already added the root node above...
			int rootlevel = Level; // 1-based root i think.
			int level = rootlevel;

			for (int i = 1; i < nodelist.Count; i++)
			{
				var n = nodelist[i];
				var depth = n.Level;
				//if (depth < level) // outdent (higher ancestor) encountered...
				//{
				//	////prevnode.state += " jstree-last";
				//	//// also must mark any intermediate-level last nodes:
				//	//for (int d = depth; d < level; d++)
				//	//{
				//	//	nt[d + 1].state += " jstree-last";
				//	//}
				//}
				//if (i == nodeList.Rows.Count - 1) // final record; set this and any ancestors to last:
				//{
				//	n.state += " jstree-last";
				//	for (int j = rootlevel; j < depth; j++)
				//	{
				//		nt[j].state += " jstree-last";
				//	}
				//}
				if (depth > level) // indentation encountered...
				{
					//if (jtree.Count > depth - rootlevel)

					//var jtree = this[0]; // first root node
					// this makes no sense; you only want to determine whether the nt[index] exists yet!
					//if (jtree.children != null && jtree.children.Count > depth - rootlevel)
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
					// root-level node has already been added
					//					nt.Add(n);
					//if (nt.Count == 0) // changed to 1 to accommodate manually-added root.
					//{
					//	nt.Add(n); // add only first root node to nodetracker, else replace.
					//}
					//else
					//{
						nt[0] = n; // this should only happen if multiple roots are allowed.
					//}
				}
				level = depth;

			//	if (nodelist[i].Level == Level + 1)
			//	{
			//		Subnodes.Add(nodelist[i]);
			//	}
			}
		}
	}
}