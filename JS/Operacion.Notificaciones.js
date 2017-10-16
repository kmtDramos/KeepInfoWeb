/**/

var Notificaciones = [];
var Publicadas = [];
var Elementos = [];
var Avisos = [];

$(function () {

	var VentanaNotificaciones = $('<div id="divNotificaciones" class="oculto"></div>');

	$("body").append(VentanaNotificaciones);

	$(VentanaNotificaciones).click(function () {
		if ($(VentanaNotificaciones).hasClass("oculto"))
			$(VentanaNotificaciones).toggleClass("visible oculto", 200)
	});

	InitNotificaciones();
	ActualizarNotificaciones();
	
});

function InitNotificaciones() {
	$("#divNotificaciones").obtenerVista({
		nombreTemplate: "tmplNotificaciones.html",
		despuesDeCompilar: function () {
			$("#btnCerrarNotificaciones", "#divNotificaciones").click(function () {
				if ($("#divNotificaciones").hasClass("visible"))
					$("#divNotificaciones").toggleClass("visible oculto", 200)
			});
			$("#btnRefrescarNotificaciones", "#divNotificaciones").click(function () {
				Publicadas = [];
				$("#listaNotificacionesActividades").html('');
				PublicarNotificaciones();
				MostrarBloqueo();
				setTimeout(function () {
					OcultarBloqueo();
				}, 2000);
			});
			setInterval(PublicarNotificaciones, 1000);
			setInterval(ActualizarElementos, 1000);
		}
	});
}

function ActualizarNotificaciones() {
	$("#divNotificaciones").addClass("process");
	$.ajax({
		url: "Notificacion.aspx/ObtenerNotificaciones",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			$("#divNotificaciones").removeClass("process");
			var json = JSON.parse(Respuesta.d);
			Notificaciones = json.Modelo.Notificaciones;
			setTimeout(ActualizarNotificaciones, 1000 * 60);
			$("#lblEncabezadoNotificaciones").text("Recordatorios ("+ Notificaciones.length +")");
		}
	});
}

function PublicarNotificaciones() {
	$("#divNotificaciones").addClass("process");
	for (x in Notificaciones) {
		var Notificacion = Notificaciones[x];
		var IdActividad = Notificacion.IdActividad;

		if (Publicadas.indexOf(IdActividad) === -1) {
			var Actividad = Notificacion.Actividad;
			var Cliente = Notificacion.Cliente;
			var FechaActividad = Notificacion.FechaActividad;
			var IdOportunidad = Notificacion.IdOportunidad;
			var Oportunidad = Notificacion.Oportunidad;
			var TipoActividad = Notificacion.TipoActividad;

			var date = FechaDateObject(FechaActividad);

			if (new Date().valueOf() < date.valueOf()) {
				var Aviso = new Object();
				Aviso.Fecha = date;
				Aviso.Id = "#Actividad" + IdActividad;
				Avisos.push(Aviso);
			}

			var Plantilla = '<table id="Actividad' + IdActividad + '" onclick="ObtenerFormaEditarActividad(' + IdActividad + ');" class="Notificacion"' +
								' IdActividad="' + IdActividad + '" FechaActividad="'+ date.valueOf() +'" IdOportunidad="'+ IdOportunidad +'" Index="'+ x +'">' +
								'<tbody>' +
									'<tr>' +
										'<td>Actividad ' + TipoActividad + ' ' + Cliente + ' - ' + FechaActividad + '</td>' +
									'<tr>' +
									'<tr>' +
										'<td>' + Actividad + '</td>' +
									'<tr>' +
									'<tr>' +
										'<td>#' + IdOportunidad + ': ' + Oportunidad + ' </td>' +
									'<tr>' +
								'</tbody>' +
							'</table>';
			var Publicacion = $(Plantilla);
			Elementos.push(Publicacion);
			$("#listaNotificacionesActividades").append(Publicacion);
			Publicadas.push(IdActividad);
		}
	}
	$("#divNotificaciones").removeClass("process");
}

function FechaDateObject(Fecha) {
	var Datos = Fecha.split(" ");
	var Dato1 = Datos[0];
	var Dato2 = Datos[1];

	var date = Dato1.split("/");
	var time = Dato2.split(":");

	var dd = parseInt(date[0]);
	var mm = parseInt(date[1])-1;
	var yy = parseInt(date[2]);
	var hh = parseInt(time[0]);
	var nn = parseInt(time[1]);

	return new Date(yy, mm, dd, hh, nn);
}

function ActualizarElementos() {
	$(Elementos).each(function (index, elemento) {
		var x = $(elemento).attr("Index");
		var date = new Date(parseInt($(elemento).attr("FechaActividad")));
		if (date.valueOf() > new Date().valueOf() + 1000 * 60 * 15)
			$(elemento).css("background-color", "#6F6");
		else if (date.valueOf() > new Date().valueOf() && date.valueOf() < new Date().valueOf() + 1000 * 60 * 15)
			$(elemento).css("background-color", "#FF6");
		else
			$(elemento).css("background-color", "#F66");
		if (date.valueOf() - new Date().valueOf() > 0)
			console.log(date.valueOf() - new Date().valueOf());
	});
}

function Notificar() {
	for (x in Avisos) {
		var Aviso = Avisos[x];
		Avisos.splice(x, 1);
		if (new Date().valueOf() < Aviso.Fecha.valueOf() && new Date().valueOf() + 1000 * 60 * 15 > Aviso.valueOf()){
			$("#divNotificaciones").removeClass("oculto");
			$("#divNotificaciones").addClass("visible");
			$("#listaNotificacionesActividades").scrollTop($(Aviso.Id).offset().top);
		}
	}
	setTimeout(Notificar, 1000 * 60);
}