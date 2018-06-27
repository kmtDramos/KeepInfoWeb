/**/

$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	$("#tabs").tabs({
		select: function(event, ui){
			switch (ui.index) {
				case 0:
					ObtenerReporteProspeccion();
					break;
				case 1:
					ObtenerReporteOportunidad();
					break;
				case 2:
					ObtenerReporteFacturacion();
					break;
				case 3:
					ObtenerReporteFamilias();
					break;
				case 4:
					ObtenerReporteVentas();
					break;
				case 5:
					ObtenerReporteNoAutorizados();
					break;
				case 6:
					ObtenerReporteCartera();
					break;
				case 7:
					ObtenerReporteCobranza();
					break;
			}
		}
	});

	MostrarBloqueo();

	ObtenerReporteProspeccion();

});

function CrearTablaMontos(data) {
	var tabla = $("<table></table>");
	var thead = $("<thead></thead>");
	var tr = $("<tr></tr>");
	var tbody = $("<tbody></tbody>");

	for (x in data[0]) {
		$(tr).append($("<th>" + x + "</th>"));
	}

	for (x in data) {
		var row = $("<tr></tr>");
		for (y in data[x]) {
			var td = $("<td></td>");
			var div = $("<div></div>");
			if (y == "Agente" || y == "Familia") {
				$(div).text(data[x][y]).css("width", "200");
				$(td).append(div);
			}
			else {
				$(div).text(formato.moneda(data[x][y], '$').replace('.00', '')).css("width", "80");
				$(td).append(div).prop("align", "right");
			}
			$(row).append(td);
		}
		$(tbody).append(row);
	}

	$(thead).append(tr);
	$(tabla).append(thead);
	$(tabla).append(tbody);
	$(tabla).prop("border", 1).prop("cellpadding", 0).prop("cellspacing", 0);
	return tabla;
}

function CrearTabla(data) {
	var tabla = $("<table></table>");
	var thead = $("<thead></thead>");
	var tr = $("<tr></tr>");
	var tbody = $("<tbody></tbody>");

	for (x in data[0]) {
		$(tr).append($("<th>" + x + "</th>"));
	}

	for (x in data) {
		var row = $("<tr></tr>");
		for (y in data[x]) {
			var td = $("<td></td>");
			var div = $("<div></div>");
			if (y == "Agente") {
				$(div).text(data[x][y]).css("width", "200");
				$(td).append(div);
			}
			else {
				$(div).text(data[x][y]).css("width", "80");
				$(td).append(div).prop("align", "center");
			}
			$(row).append(td);
		}
		$(tbody).append(row);
	}

	$(thead).append(tr);
	$(tabla).append(thead);
	$(tabla).append(tbody);
	$(tabla).prop("border", 1).prop("cellpadding", 0).prop("cellspacing", 0);
	return tabla;
}

function ObtenerReporteFacturacion() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteFacturacion",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTablaMontos(json.Modelo.Facturacion);
				$("#tabFacturacion").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteFamilias() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteFamilias",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTablaMontos(json.Modelo.Familias);
				$("#tabFamilias").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteProspeccion() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteProspeccion",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTabla(json.Modelo.Prospeccion);
				$("#tabProspeccion").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteOportunidad() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteOportunidades",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTabla(json.Modelo.Oportunidades);
				$("#tabOportunidad").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteVentas() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerAutorizadosVendedores",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTablaMontos(json.Modelo.Autorizados);
				$("#tabVentas").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteNoAutorizados() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteNoAutorizados",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTablaMontos(json.Modelo.NoAutorizados);
				$("#tabNoAutorizado").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteCartera() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteCartera",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTablaMontos(json.Modelo.Cartera);
				$(tabla).click(function (event) {
					var Agente = $(event.target).parent("td").parent("tr").children("td:eq(0)").text();
					ObtenerReporteFacturasCartera(Agente);
				});
				$("#tabCartera").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteFacturasCartera(Agente) {
	MostrarBloqueo();
	var Reporte = new Object();
	Reporte.Agente = Agente;
	var Request = JSON.stringify(Reporte);
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteFacturasCartera",
		data: Request,
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTabla(json.Modelo.Facturas);
				var ventana = $('<div></div>');
				$(ventana).append(tabla).dialog({
					modal: true,
					width: 'auto',
					resizable: false,
					draggable: false,
					title: 'Facturas pendientes'
				})
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerReporteCobranza() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteDiarioVendedores.aspx/ObtenerReporteCobranza",
		type: "POST",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var tabla = CrearTablaMontos(json.Modelo.Cobranza);
				$("#tabCobranza").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}