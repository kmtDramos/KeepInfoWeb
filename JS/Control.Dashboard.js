/**/

$(function () {

	setInterval(MantenerSesion, 1000 * 60 * 2.5);

	ObtenerControlesUsuario();

});

function ObtenerControlesUsuario() {
	$.ajax({
		url: "Dashboard.aspx/ObtenerControles",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			var Controles = json.Modelo.Controles;
			CargarControles(Controles);
		}
	});
}

function CargarControles(Controles) {
	for (x in Controles) {
		var Nombre = Controles[x].Nombre;
		var Template = Controles[x].Template;
		var Metodo = Controles[x].Metodo;
		var Identificador = Controles[x].Identificador;

		var Div = $('<div id="'+ Identificador +'"></div>');

		var Tab = $('<li><a href="#' + Identificador + '">' + Nombre + '</a></li>');

		$("#ulControlesUsuario").append(Tab);
		$("#tabsControlesUsuario").append(Div);

		AgregarControl(Identificador, Metodo, Template);
	}
	InitControles();
}

function AgregarControl(Identificador, Metodo, Template) {
	$("#"+Identificador).obtenerVista({
		url: Metodo,
		nombreTemplate: Template
	});
}

function InitControles() {
	$("#tabsControlesUsuario").tabs();
}

function AbrirMetas() {
	var ventana = window.open();
	var url = window.location.toString();
	ventana.location = url.replace("Dashboard.aspx","Metas_X_Consultor_2018.pdf");
}