using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgNavWebApi.Models.ViewModels
{
	// ViewModel wrapper for PositionNode model:
	public class PositionNodeVm {
		private Models.PositionNode _model;
		public PositionNodeVm(Models.PositionNode n)
		{
			_model = n;
		}

		public static List<PositionNode> TreesFromList(List<PositionNode> l)
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
		
		// TODO: complex type options for data property.
		// public properties are intentionally lowercase for consumption by jsTree:
		public string data
		{
			get
			{
				return _model.Name;
			}
		}

		private List<PositionNodeVm> _children = new List<PositionNodeVm>();
		public List<PositionNodeVm> children
		{
			get
			{
				// need to wrap each _model.PositionNode with ViewModel:
				if (_children.Count == 0 && _model.SubNodes.Count > 0)
				{
					foreach (var m in _model.SubNodes)
					{
						_children.Add(new PositionNodeVm(m));
					}
				}
				return _children;
			}
		}

		// TODO: set attr[id,complete,lazy, etc.]..., state: "leaf jstree-last", etc.
		private bool IsLeaf {
			get
			{
				return _model.SubNodes.Count == 0;
			}
		}

		public string state
		{
			get
			{
				return IsLeaf ? " leaf" : "open"; // TODO: construct state from IsLeaf, IsLazy, IsExpanded, IsLastSibling, ('jstree-last') etc...
			}
		}
	}
}