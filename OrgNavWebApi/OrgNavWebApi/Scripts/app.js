$(function () {
	$.ajax({
		type: 'GET',
		url: 'api/positiontree/71027130', // this is the DIRECTOR (root) position.
		success: function (d) {
			bindTree(d);
		}
	});
});

function bindTree(rootNode) {
	$('#PositionTree').jstree({
		core: { animation: 80 },
		json_data: { data: rootNode },
		plugins: ["json_data", "themes"]
	});
}