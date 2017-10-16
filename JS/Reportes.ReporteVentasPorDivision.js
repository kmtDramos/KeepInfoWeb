

$(function () {

	$("form").submit(function (event) {
		event.preventDefault();
	});

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 2.5);

	$("#txtFechaInicial").datepicker();

	$("#txtFechaFinal").datepicker();

	$("#tabsVentasPorDivision").tabs();

	MostrarBloqueo();

	ObtenerSucursales();

	ObtenerDivisiones();

	$("#btnVentasPorDivision").click(function (event) {
		ObtenerReporteVentasPorDivision();
		ObtenerReporteVentasPorDivisionMes();
	});

	$("#txtCliente").autocomplete({
		source: function (request, response) {
			var pRequest = new Object();
			pRequest.pCliente = $("#txtCliente").val();
			$.ajax({
				type: 'POST',
				url: 'Oportunidad.aspx/BuscarCliente',
				data: JSON.stringify(pRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						return { label: item.Cliente, value: item.Cliente, id: item.IdCliente }
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdCliente = ui.item.id;
			$(this).attr("IdCliente", pIdCliente);
		},
		change: function (event, ui) {
			if ($("#txtCliente").val() == '')
				$("#txtCliente").attr('IdCliente', -1);
		},
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	})
	.blur(function () {
		if ($(this).val() == '')
			$(this).attr("IdCliente", -1);
	});

});

function ObtenerReporteVentasPorDivisionMes() {

	var Reporte = new Object();
	Reporte.FechaInicial = $("#txtFechaInicial").val();
	Reporte.FechaFinal = $("#txtFechaFinal").val();
	Reporte.IdSucursal = parseInt($("#cmbSucursal").val());
	Reporte.IdUsuario = parseInt($("#cmbAgente").val());
	Reporte.IdCliente = parseInt($("#txtCliente").attr('IdCliente'));
	Reporte.IdDivision = parseInt($("#cmbDivision").val());

	var Request = JSON.stringify(Reporte);
	$.ajax({
		url: "ReporteVentasPorDivision.aspx/ObtenerReporteVentasPorDivisionMes",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				TablaVentasPorDivisionMes(json.Modelo.Divisiones);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function TablaVentasPorDivisionMes(Divisiones) {
	$("#tabDivisionPorMeses").html('');
	var Tabla = $("<table cellpadding\"2\" cellspacing=\"0\" style=\"border:1px solid #BBB;\"></table>");
	var thead = $("<thead></thead>");
	var tbody = $("<tbody></tbody>");
	var trh = $("<tr></tr>");
	var llenado = false
	for (x in Divisiones)
	{
		for (y in Divisiones[x])
		{
			if (x == 0)
			{
				$(trh).append($("<th class=\"celda\">" + y + "</th>"));
			}
		}
	}
	$(thead).append(trh);
	for (x in Divisiones)
	{
		var tr = $("<tr></tr>");
		for (y in Divisiones[x])
		{
			$(tr).append($("<td class=\"celda\">" + Divisiones[x][y] + "</td>"));
		}
		$(tbody).append(tr);
	}
	$(Tabla).append(thead);
	$(Tabla).append(tbody);
	$("#tabDivisionPorMeses").append(Tabla);
}

function ObtenerDivisiones() {
	$.ajax({
		url: "ReporteVentasPorDivision.aspx/ObtenerDivisiones",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Divisiones = json.Modelo.Divisiones;

				for(x in Divisiones)
				{
					$("#cmbDivision").append($("<option value='" + Divisiones[x].Valor + "'>" + Divisiones[x].Descripcion + "</option>"));
				}
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function ObtenerSucursales() {
	MostrarBloqueo();
	$.ajax({
		url: "ReporteVentasPorDivision.aspx/ObtenerSucursales",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				$("#cmbSucursal").html('');
				var Sucursales = json.Modelo.Sucursales;
				for (var i = 0; i < Sucursales.length; i++) {
					var opcion = document.createElement("option");
					opcion.value = Sucursales[i].Valor;
					opcion.innerHTML = Sucursales[i].Descripcion;
					$("#cmbSucursal").append(opcion);
				}
				$("#cmbSucursal").change(ObtenerAgentes).change();
			} else {
				MostrarMensajeError(json.Descripcion);
			}
			OcultarBloqueo();
		}
	});
}

function ObtenerAgentes() {
	MostrarBloqueo();
	var Sucursal = new Object();
	Sucursal.IdSucursal = parseInt($("#cmbSucursal").val());
	var Request = JSON.stringify(Sucursal);
	$.ajax({
		url: "ReporteVentasPorDivision.aspx/ObtenerAgentes",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				$("#cmbAgente").html('');
				var Agentes = json.Modelo.Agentes;
				for (var i = 0; i < Agentes.length; i++) {
					var opcion = document.createElement("option");
					opcion.value = Agentes[i].Valor;
					opcion.innerHTML = Agentes[i].Descripcion;
					$("#cmbAgente").append(opcion);
				}
			} else {
				MostrarMensajeError(json.Descripcion);
			}
			OcultarBloqueo();
		}
	});
}

function ObtenerReporteVentasPorDivision() {
	MostrarBloqueo();

	var Reporte = new Object();
	Reporte.FechaInicial = $("#txtFechaInicial").val();
	Reporte.FechaFinal = $("#txtFechaFinal").val();
	Reporte.IdSucursal = parseInt($("#cmbSucursal").val());
	Reporte.IdUsuario = parseInt($("#cmbAgente").val());
	Reporte.IdCliente = parseInt($("#txtCliente").attr('IdCliente'));
	Reporte.IdDivision = parseInt($("#cmbDivision").val());

	var Request = JSON.stringify(Reporte);

	$("#tabVentasPorDivision").obtenerVista({
		url: "ReporteVentasPorDivision.aspx/ObtenerReporteVentasPorDivision",
		parametros: Request,
		nombreTemplate: "tmplReporteVentasPorDivision.html",
		despuesDeCompilar: function (Respuesta) {
			OcultarBloqueo();
		}
	});

}