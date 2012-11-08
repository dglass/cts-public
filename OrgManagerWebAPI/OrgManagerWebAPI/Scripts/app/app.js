// app-specific JS code goes here.
var theTree;

// main:
$(function () {
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
	$.ajax({
		type: 'GET',
		url: 'api/orgunit/2', // 2 is the CTS (root) OrgUnit.
		success: function (d) {
			bindTree(d);
		}
	});
	bindTree(rootNode);
});

function bindTree(rootNode) {
	theTree = $('#OrgTree').jstree({
		core: { animation: 80 },
		json_data: { data: rootNode },
		plugins: ["json_data", "themes", "ui"]
	});
	//jst = $.jstree._reference(theTree);
	//setupEventBindings();
}