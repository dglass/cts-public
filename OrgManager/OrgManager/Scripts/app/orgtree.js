function getData() {
    // this is generic ajaxSetup...TODO: find more appropriate location.
    $.ajaxSetup({
        type: 'POST',
        timeout: 20000,
        error: function (xrq, txtStatus, errThrown) {
            alert(xrq['responseText']);
            alert(txtStatus);
        }
    });
    $.ajax({
        // *note* trailing slash here prevents auto-redirect to append it.
        // (more efficient)
        //			url: '/Node/Index/' + rootNodeId,
        //			url: '/orgunit/' + rootNodeId,
        url: approot + 'orgunit/2', // *NOTE*, global approot is set in Index.cshtml.  OrgUnitId #2 is all CTS (aka "DIRECTOR").
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
    var orgtree = $('#OrgTree').bind("loaded.jstree", function (e, d) {
        //styleSubItems(mytree); // replaces grayCompletedItems...
    }).jstree({
        core: { animation: 80 }, // milliseconds
	    json_data: { data: rootNode },
	    plugins: ["crrm", "json_data", "themes", "ui"] // removed hotkeys for this app
    });
}