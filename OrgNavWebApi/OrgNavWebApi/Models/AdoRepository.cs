using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Diagnostics;

namespace OrgNavWebApi.Models
{
	// TODO: abstract base class
	// see CommonLibrary.NET RepositoryBase:
	// http://commonlibrarynet.codeplex.com/SourceControl/changeset/view/71149#478467
	public class AdoRepository<T> : IRepository<T> where T : class
	{
		//public ViewModels.PositionNodeVm GetAll()
		public List<object> GetAll()
		{
			return new List<object> { Get(0) }; // TODO: get all actual roots from db...
		}

		//public ViewModels.PositionNodeVm Get(object id)
		public object Get(object id)
		{
			var model = this.GetType().GenericTypeArguments[0];
			if (model == typeof(ViewModels.PositionNodeVm))
			{
				return GetPositionTree((int)id);
			}
			else
			{
				return null;
			}
		}

		private ViewModels.PositionNodeVm GetPositionTree(int id)
		{
			int uid = 1; // TODO: look this up from authenticated windows user...

			var sp = new Util.SmartProc<PositionNode>() {
				ConnStr = WebConfigurationManager.ConnectionStrings["OrganizationNavigatorContext"].ConnectionString,
				ProcName = "GetPositionTree"
			};
			sp.Init(); // initializes parameters.  TODO: move this to constructor.
			sp.Params["@RootPosId"].Value = id;
			sp.Params["@AppUserId"].Value = uid;

			//var sw = new Stopwatch();
			//sw.Start();
			var plist = sp.All();
			//var et = sw.ElapsedMilliseconds; // ~ 20ms on local laptop
			//sw.Stop(); 

			var root = new Models.PositionNode
			{
				Id = 1,
				ParentId = null,
				Name = "Root Node",
				IsExpanded = true
			};

			root.SubNodes = plist;
			// TODO: move treebuilding, wrapping into Vm:
			return new Models.ViewModels.PositionNodeVm(root);
		}

		// TODO: make this List<NodeBase>...
		private List<PositionNode> TreesFromList(List<PositionNode> l)
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
	}
}