using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgNavWebApi.Models.ViewModels
{
	// ViewModel wrapper for PositionNode model:
	public class PositionNodeVm {
		private Dictionary<string, object> _attr = new Dictionary<string, object>();
		private Models.PositionNode _model;
		public PositionNodeVm(Models.PositionNode n)
		{
			_model = n;
			_attr["id"] = n.Id;
			// lazy-loadability:
			_attr["z"] = (n.ChildCount > 0 && n.IsExpanded == false);
		}

		public Dictionary<string, object> attr
		{
			get { return _attr; }
		}

		public static List<PositionNodeVm> List(List<PositionNode> pnl)
		{
			var vml = new List<PositionNodeVm>();
			for (int i = 0; i < pnl.Count; i++)
			{
				var pnv = new PositionNodeVm(pnl[i]);
				if (i == pnl.Count - 1)
				{
					// *note*, state is lazy-populated property
					pnv.state += " jstree-last"; // this only works for first tier of lazy-loaded subnodes.
				}
				vml.Add(pnv);
			}
			return vml;
		}

		public static List<PositionNodeVm> VmTreesFromList(List<PositionNode> l) {
			var tl = new List<PositionNodeVm>(); // holds tree-sorted list of root nodes
			var nt = new List<PositionNodeVm>(); // nesting tracker

			var root = new PositionNodeVm(l[0]);
			tl.Add(root); // first element is always root
			nt.Add(root);
			int rootlevel = l[0].depth;
			int level = rootlevel;

			for (int i = 1; i < l.Count; i++)
			{
				var n = new PositionNodeVm(l[i]);
				var depth = n._model.depth;
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
					//nt[level - rootlevel].SubNodes.Add(nt[depth - rootlevel]);
					nt[level - rootlevel].children.Add(nt[depth - rootlevel]);
				}
				else if (depth <= level && depth > rootlevel)
				{
					nt[depth - rootlevel] = n;
//					nt[depth - rootlevel - 1].SubNodes.Add(n);
					nt[depth - rootlevel - 1].children.Add(n);
				}
				else if (depth == rootlevel)
				{
					// this should only happen if multiple roots are allowed.
					tl.Add(n);
					nt[0] = n;
				}
				// attach children of root:
				//if (depth == rootlevel + 1)
				//{
					//nt[0].children.Add(n);
				//}

				// outdenting...set preceding node last-sibling states if applicable:
				if (depth < level) {
					for (int d = depth; d < level; d++)
					{
						nt[d].state += " jstree-last";
					}
				}
				if (i == l.Count - 1) // final record; set this and any ancestors to last:
				{
					n.state += " jstree-last";
					for (int j = rootlevel; j < depth; j++)
					{
						nt[j].state += " jstree-last";
					}
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

		private string _state;
		public string state
		{
			get
			{
				if (String.IsNullOrEmpty(_state)) {
					_state = 
					IsLeaf && _model.ChildCount == 0 ? "leaf" : _model.IsExpanded ? "open" : "closed";
				}
				return _state;
			}
			set
			{
				_state = value;
			}
		}
	}
}