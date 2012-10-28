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