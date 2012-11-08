using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using OrgManagerWebApi.Models;

namespace OrgManagerWebApi.Models.ViewModels
{
	/// <summary>
	/// NodeVm is a ViewModel wrapper for the Node model.
	/// It provides JSON in the expected format for populating a JSTree
	/// </summary>
	public class NodeVm
	{
		private Dictionary<string, object> _attr = new Dictionary<string, object>();
		private Node _model;
		public NodeVm(Node n)
		{
			_model = n;
			_attr["id"] = n.Id;
			// lazy-loadability:
			//_attr["z"] = (n.ChildCount > 0 && n.IsExpanded == false);
		}

		public Dictionary<string, object> attr
		{
			get { return _attr; }
		}

		public static List<NodeVm> List(List<Node> nl)
		{
			var vml = new List<NodeVm>();
			for (int i = 0; i < nl.Count; i++)
			{
				var pnv = new NodeVm(nl[i]);
				if (i == nl.Count - 1)
				{
					// *note*, state is lazy-populated property
					pnv.state += " jstree-last"; // this only works for first tier of lazy-loaded subnodes.
				}
				vml.Add(pnv);
			}
			return vml;
		}

		public static List<NodeVm> VmTreesFromList(List<Node> l) {
			var tl = new List<NodeVm>(); // holds tree-sorted list of root nodes
			var nt = new List<NodeVm>(); // nesting tracker

			var root = new NodeVm(l[0]);
			tl.Add(root); // first element is always root
			nt.Add(root);
			int rootlevel = l[0].Depth;
			int level = rootlevel;

			for (int i = 1; i < l.Count; i++)
			{
				var n = new NodeVm(l[i]);
				var depth = n._model.Depth;
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
					nt[level - rootlevel].children.Add(nt[depth - rootlevel]);
				}
				else if (depth <= level && depth > rootlevel)
				{
					nt[depth - rootlevel] = n;
					nt[depth - rootlevel - 1].children.Add(n);
				}
				else if (depth == rootlevel)
				{
					// this should only happen if multiple roots are allowed.
					tl.Add(n);
					nt[0] = n;
				}

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

		private List<NodeVm> _children = new List<NodeVm>();
		public List<NodeVm> children
		{
			get
			{
				// need to wrap each _model.PositionNode with ViewModel:
				if (_children.Count == 0 && _model.SubNodes.Count > 0)
				{
					foreach (var m in _model.SubNodes)
					{
						_children.Add(new NodeVm(m));
					}
				}
				return _children;
			}
		}

		private bool IsLeaf {
			get
			{
				//return _model.SubNodes.Count == 0;
				return children.Count == 0;
			}
		}

		private string _state;
		public string state
		{
			get
			{
				if (String.IsNullOrEmpty(_state)) {
					_state =
						// this version only applies when lazy-loading is enabled:
						//IsLeaf && _model.ChildCount == 0 ? "leaf" : _model.IsExpanded ? "open" : "closed";
					IsLeaf ? "jstree-leaf" : "jstree-open";
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