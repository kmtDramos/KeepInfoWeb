/**/

$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

	$("#tabs").tabs({
		select: function(event, ui){
			switch (ui.index) {
				case 0:
					ObtenerReporteFacturacion();
					break;
				case 1:
					ObtenerReporteProspeccion();
					break;
			}
		}
	});

	ObtenerReporteFacturacion();

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
			if (y == "Agente") {
				$(td).text(data[x][y]);
			}
			else {
				$(td).text(formato.moneda(data[x][y],'$')).prop("align", "right");
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
			if (y == "Agente") {
				$(td).text(data[x][y]);
			}
			else {
				$(td).text(data[x][y]).prop("align", "center");
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
				console.log(tabla);
				$("#tabFacturacion").html('').append(tabla);
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
				console.log(tabla);
				$("#tabProspeccion").html('').append(tabla);
				OcultarBloqueo();
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}