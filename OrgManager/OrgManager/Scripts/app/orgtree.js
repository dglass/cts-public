var approot; // set in Home.Index View.
var ouPath = 'orgUnit/';
var ouNodePath = 'orgUnitNode/';
var jst; // reference to tree (needle), set at end of bindTree()

function createDialogs() {
	var dept = $('#DeptDetail');
	var dlg = $('#DeptDetail').dialog({
		autoOpen: false,
		buttons: {
			"Ok": function () { asyncUpdateDept(this); $(this).dialog('close'); },
			"Cancel": function () { $(this).dialog('close') }
		},
		closeOnEscape: false,
		dialogClass: "alert",
		modal: true, // this seems to fail in splitter, haven't tried it out of it yet.
		position: "center",
		resizable: false,
		width: 480,
		height: 360
	});
}

function getData() {
    // this is generic ajaxSetup...TODO: find more appropriate location.
    $.ajaxSetup({
    	type: 'POST', // default type
    	timeout: 20000,
        error: function (xrq, txtStatus, errThrown) {
            alert(xrq['responseText']);
            alert(txtStatus);
        }
    });
    $.ajax({
		type: 'GET',
        // *note* trailing slash here prevents auto-redirect to append it.
        // (more efficient)
        //			url: '/Node/Index/' + rootNodeId,
    	//			url: '/orgunit/' + rootNodeId,
		url: approot + ouNodePath + '2', // *NOTE*, global approot is set in Index.cshtml.  OrgUnitId #2 is all CTS (aka "DIRECTOR").
        // *NOTE*, d collection result is coerced into an array
        success: function (d) {
            bindTree(d);
        }
        // TODO: handle errors...
        // *NOTE*, Method-Override only works with POST.
        //, beforeSend: function (xhr) {
        //    xhr.setRequestHeader("X-Http-Method-Override", "GET");
        //}
    });
}

//function buildTree() {
function bindTree(rootNode) {
    // testing adding root-node icon at client before binding:
    //rootNode[0]['data'] = {
    //    title: rootNode[0]['data']
    //    //,icon: approot + 'Scripts/extras/slickgrid/images/comment_yellow.gif'
    //};
    /* DEMO DATA:
    var rootNode = new Array();
    rootNode[0] = { data: {
        title: "First Root Node"
    }};
    rootNode[1] = {
        data: {
            title: "Second Root Node"
        }
    };
    rootNode[0]['children'] = [
        {
            data: {
                title: "Subnode of First"
            }
        }
    ];
    END DEMO DATA */
    orgtree = $('#OrgTree').bind("loaded.jstree", function (e, d) {
        //styleSubItems(mytree); // replaces grayCompletedItems...
    }).jstree({
    	contextmenu: {
    		select_node: true,
    		// *note*, default (unconfigured) "items" includes CRUD and cut/copy/paste options.
    		// to clear them, assign false to them.
    		// https://groups.google.com/forum/?fromgroups=#!topic/jstree/AC8U-OEV14s
    		items: {
    			//rename: {
    			//	label: "Rename", // asyncRenameNode is triggered by post-rename event binding, no need to explicitly attach it here.
    			//	action: function (obj) { asyncRenameNode(obj); }
    			//},
    			//remove: false, // delete
    			create: {
    				label: "Create",
    				action: beginAppendNode
    			},
    			edit: {
    				label: "Edit Detail",
    				action: showEditForm
    			},
    			remove: {
    				label: "Delete",
    				action: asyncDeleteNode
    			},
				ccp: false // cut, copy, paste
			}
    	},
        core: { animation: 80 }, // milliseconds
	    json_data: { data: rootNode },
	    plugins: ["contextmenu", "crrm", "hotkeys", "json_data", "themes", "ui"] // removed hotkeys for this app
    });
	// now set remaining bindings:
    setupBindings();
    jst = $.jstree._reference($('#OrgTree'));
}

function setupBindings() {
	orgtree.bind("rename.jstree", asyncRenameNode); // fires after losing textbox edit focus
	//orgtree.bind("delete_node.jstree", asyncDeleteNode); // only fires *after* node is deleted.  must fire before delete.
	// binding create_node is nothing but trouble! must handle pre-create event; this is post-create.
	//orgtree.bind("create_node.jstree", beginAppendNode);  // need to suspend this binding or it will re-trigger itself recursively.
	//orgtree.bind("select_node.jstree", selectNode);
	//orgtree.bind("dblclick.jstree", selectNode);
}

function showEditForm(n) {
	$(n[0].childNodes[1]).toggleClass('async-op');
	// first retrieve details from server (todo: cache these for multiple edits in a session)
	$.ajax({
		type: 'GET', // override doesn't work for GET
		// this uses ouPath instead of ouNodePath, which pulls full tree:
		url: approot + ouPath + n.attr("id"),
		success: function (d) {
			var dv = eval(d); // this to convert JSON string into JS object.
			$(n[0].childNodes[1]).toggleClass('async-op');
			$('#ShortName').val(dv['ShortName']);
			$('#DeptDetail').dialog('open');
		}
	});
}

function asyncDeleteNode(node) {
	var atag = node[0].childNodes[1];
	var jtag = $(atag);
	jtag.toggleClass('async-op');
	$.ajax({
		url: approot + ouPath + node.attr("id"),
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Http-Method-Override", "DELETE");
		},
		error: function (xhr, x, e) {
			// TODO: more user-friendly reply besides "Forbidden", etc.
			alert(e);
		},
		success: function () {
			jst.remove(node);
			jtag.toggleClass('async-op');
		}
	});
}

function beginAppendNode(n) {
	asyncAppendNode(n, 'last'); // append to tail by default.  TODO: head/tail option
}

// BEGIN async ops:
function asyncAppendNode(n, pos) { // pos sets head ("first") or tail ("last")
	// *NOTE* this omits any action param or header override because it is a plain POST (create)
	var atag = n[0].childNodes[1];
	var jtag = $(atag);
	jtag.toggleClass('async-op');
	var id = n.attr("id"); // parent node id
	$.ajax({
		url: approot + ouPath + n.attr("id"),
		data: {
			pos: pos
		},
		success: function (d) {
			jtag.toggleClass('async-op'); // revert parent node async indicator
			//alert("d=" + d);
			// d is normally newly-created node passed back from server.
			//d = { data: 'New Item' };
			// TODO: return array of nodes, rather than single, newly inserted node
			// (to include previously unloaded siblings in lazy-load situation);a
			// this appears to be unnecessary now; d is coming back as an object?
			//eval("d=" + d); // this to convert JSON string into JS object.
			//alert('new node='+eval(d));
			// 2011.06.02: switched from first to last to match db insert to tail
			//var nn = jst.create_node(n, "first", d);
			// 2011.08.31: modified to enable specifying position:
			//                      var nn = jst.create_node(n, "last", d);

			// new node needs to be replaced with new server node, not re-created here:
			// or need to unbind create_node event to prevent endless recursion.

			var nn = jst.create_node(n, pos, d);

			// *note*, this needs to be supported server-side...
			// this should not be called if node is already open:
			if (n.attr("state") == 'closed' || n.attr("state") == 'leaf') {
				jst.open_node(n);
			}
			// now select newly inserted node and put into edit mode:
			jst.deselect_all();
			jst.select_node(nn);
			// rename fails for unopened node when animation is on...TODO: fix by waiting until animation is complete.
			jst.rename(nn);
		}
	});
}

function asyncRenameNode(event, data) {
	var node = data.args[0]; // args[0] is origin node...
	var atag = node[0].childNodes[1];
	var jtag = $(atag);
	jtag.toggleClass('async-op');
	$.ajax({
		url: approot + ouPath + node.attr("id"),
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Http-Method-Override", "PUT");
		},
		data: {
			action: 'rename',
			Name: data.rslt.new_name
		},
		error: function (xhr, x, e) {
			// TODO: more user-friendly reply besides "Forbidden", etc.
			alert(e);
		},
		success: function () {
			jtag.toggleClass('async-op');
		}
	});
}

function asyncUpdateDept(dlg) {
	var n = $(jst.get_selected());
	var nodetext = $(n[0].childNodes[1]);
	nodetext.toggleClass('async-op'); // only toggle class for link text...
	$.ajax({
		url: approot + ouPath + n.attr("id"),
		beforeSend: function (xhr) {
			xhr.setRequestHeader("X-Http-Method-Override", "PUT");
		},
		data: {
			ShortName: $('#ShortName').val()
			//action: 'update',
			//notes: $('#notes').val(),
			//due: $('#dueDatePicker').val()
		},
		success: function () {
			nodetext.toggleClass('async-op');
		}
	});
}