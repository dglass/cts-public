using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrgManager.Models
{
    public class JsTreeNode
    {
        public string data { get; set; } // lowercase "data" required for JSTree JSON serialization...
        public string state { get; set; } // "open" or "closed"
        public List<JsTreeNode> children { get; set; }
        public static List<JsTreeNode> GetDummyTree()
        {
            // this returns a single root node, but multi-roots are also possible:
            var r = new JsTreeNode()
            {
                data = "CTS",
                state = "open"
            };
            r.children = new List<JsTreeNode>(); // thinking this needs to be null unless childnodes exist...
            r.children.Add(new JsTreeNode()
            {
                data = "Administrative Services"
            });
            var msd = new JsTreeNode()
            {
                data = "Management Services",
                state = "open"
            };
            msd.children = new List<JsTreeNode>();
            msd.children.Add(new JsTreeNode()
            {
                data = "Finance"
            });
            msd.children.Add(new JsTreeNode()
            {
                data = "HR"
            });
            msd.children.Add(new JsTreeNode()
            {
                data = "Legal"
            });
            r.children.Add(msd);
            var ops = new JsTreeNode() { data = "Operations", state = "open" };
            ops.children = new List<JsTreeNode>();
            ops.children.Add(new JsTreeNode() { data = "Application Services" });
            ops.children.Add(new JsTreeNode() { data = "Capital Projects" });
            ops.children.Add(new JsTreeNode() { data = "Computing" });
            ops.children.Add(new JsTreeNode() { data = "Customer Relations" });
            ops.children.Add(new JsTreeNode() { data = "Enterprise Projects" });
            ops.children.Add(new JsTreeNode() { data = "SDC Program" });
            ops.children.Add(new JsTreeNode() { data = "Security" });
            ops.children.Add(new JsTreeNode() { data = "Telecommunication" });
            r.children.Add(ops);
            return new List<JsTreeNode>()
            {
                r
            };
        }
    }
}