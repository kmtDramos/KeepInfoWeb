/**/

$(function () {
	InitTicketsServicio();
});

function InitTicketsServicio ()
{
	var ventana = $('<div id="divTicketsServicio">Tickets</div>');
	$("body").append(ventana);
	$(ventana).css({
		"position": "absolute",
		"padding": "10px",
		"min-width": "150px",
		"bottom": "10px",
		"right": "10px",
		"background-color": "#EEE","border":"1px solid #333",
		"border-radius": "4px"
	});
}