/**/

$(function () {

	MantenerSesion();
	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	$("#tabs").tabs();

});