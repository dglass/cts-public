var theTree; // global handle
var shiftPressed = false;
var approot = 'api/';
var jst; // "needle" pointer to tree

$(function () {
	// pre-initialization before tree construction:
	$(document).keydown(function (e) {
		shiftPressed = e.shiftKey; // track shift state for expand(all)/collapse(all) 
	});
	$(document).keyup(function (e) {
		shiftPressed = e.shiftKey; // track shift state for expand(all)/collapse(all) 
	});
	// end pre-init

	$.ajax({
		type: 'GET',
		url: 'api/positiontree/71027130', // this is the DIRECTOR (root) position.
		success: function (d) {
			bindTree(d);
		}
	});
});

function bindTree(rootNode) {
	theTree = $('#PositionTree').jstree({
		core: { animation: 80 },
		json_data: { data: rootNode },
		plugins: ["json_data", "themes", "ui"]
	});
	jst = $.jstree._reference(theTree);
	setupEventBindings();
}

function setupEventBindings() {
	theTree.bind("close_node.jstree", asyncCollapseNode);
	theTree.bind("open_node.jstree", asyncExpandNode);
}

// async event handlers
// async methods:
function asyncCollapseNode(event, data) {
	var all = shiftPressed;
	var node = data.args[0]; // args[0] is origin node...
	var atag = node[0].childNodes[1];
	var jtag = $(atag);
	//jtag.toggleClass('jstree-clicked', false); // need to turn this off?
	// *NOTE* the async-op toggle may happen too quickly to be visible locally.
	jtag.toggleClass('async-op');
	$.ajax({
		type: 'POST',
		// TODO: populate node.attr (currently empty)
		url: approot + 'node/' + node.attr("id"),
		//url: approot + 'node/' + 1234,
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Http-Method-Override", "PUT");
		},
		data: {
			action: all ? 'collapse_all' : 'collapse'
		},
		success: function () {
			if (all) {
				// remove all subnodes and revert self to lazy...collapsing all subnodes without firing collapse event = too much work.
				node.children('ul').remove();
				node.attr('z', 'true'); // reset to lazy (sic 'true' string)
			}
			jtag.toggleClass('async-op');
		},
		error: function (xrq, txtStatus, errThrown) {
			alert(xrq['responseText']);
			alert(txtStatus);
		}
	});
}

function asyncExpandNode(event, data) {
	var node = data.args[0];
	var l = node["length"];
	// ignore expand events triggered by drop onto leaf (in which case node arg contains no nodes)
	// TODO: distinguish between drop onto leaf and lazy-load expand.
	if (l == undefined) {
		return;
	}

	// node.attr("lazy") is coming back as "false" string, which evaluates to true!  TODO: fix.
	var lazy = node.attr("z");
	if (lazy == "true" || shiftPressed) { // always lazy-load all when shiftPressed...
		// TODO: don't forget to set node.attr("lazy") to undefined after lazy-load completes...
		if (shiftPressed) {
			node.children('ul').remove();
			asyncLazyLoad(node, '/all');
		}
		else {
			asyncLazyLoad(node, '');
		}
		node.removeAttr("z");

		return false;
	}

	// return if node has no childnodes.
	// caused by attempted leaf expansion during drag-drop or programmatic node move.
	// *note*, the new parent probably *should* be expanded, but only *after* the child is added.
	//		if (! node.hasChildNodes()) {
	//			return;
	//		}
	//		// first set text orange to indicate async op:
	var atag = node[0].childNodes[1];
	var jtag = $(atag);
	//jtag.toggleClass('jstree-clicked', false); // need to turn this off?
	// *NOTE* the async-op toggle happens too fast to be visible locally.
	jtag.toggleClass('async-op');

	$.ajax({
		url: approot + 'node/' + node.attr("id"),
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Http-Method-Override", "PUT");
		},
		success: function () {
			jtag.toggleClass('async-op');
			//				jtag.toggleClass('jstree-clicked'); // need to turn this off
		},
		data: {
			action: 'expand'
		}
	});
}

function asyncLazyLoad(n, all) {
	var atag = n[0].childNodes[1];
	var jtag = $(atag);
	jtag.toggleClass('async-op');
	$.ajax({
		//type: 'PUT','GET' 'POST' 'DELETE', etc.
		//            url: '/NodeService/node/' + n.attr("id") + '/subnodes',
		//			url: '/nodes/' + n.attr("id") + '/subnodes',
		//		url: '/Node/SubNodes/' + n.attr("id"), // MVC unReSTful path...
		url: approot + 'node/' + n.attr("id") + '/subnodes' + all,
		// *NOTE*, d result is coerced into an array.
		success: function (d) {
			//alert("REST call took " + (new Date().getTime() - start) + " ms.");
			//d["data"] = "<input type='checkbox' onclick=alert('hello')></input> " + d["data"];
			// TODO: return JSON array from server (currently returns root node, as for main data)
			jtag.toggleClass('async-op');
			var domList = jst._parse_json(d);
			n.append(domList);
			// TODO: more granular version of this, only gray completed items that were just lazy-loaded.
			//grayCompletedItems(mytree);
			//styleSubItems(n);
			n.removeAttr("z"); //aka 'lazy'
		},
		// *NOTE*, Method-Override only works with POST.
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Http-Method-Override", "GET");
		}
	});
}
