/**/

$(function () {

	$("form").submit(function (event) {
		event.preventDefault();
	});

	MantenerSesion();
	setInterval(MantenerSesion, 1000 * 60 * 2.5);

	$("#txtFechaInicial").datepicker();
	$("#txtFechaFinal").datepicker();

	$("#tabsGraficasVentaMeta").tabs();

	ObtenerSucursales();

	$("#btnGraficaVentaMeta").click(ObtenerDatosGraficaVentasMeta);

});

function ObtenerSucursales() {
	$.ajax({
		url: "GraficaAvanceVenta.aspx/ObtenerSucursales",
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
		}
	});
}

function ObtenerAgentes() {
	var Sucursal = new Object();
	Sucursal.IdSucursal = parseInt($("#cmbSucursal").val());
	var Request = JSON.stringify(Sucursal);
	$.ajax({
		url: "GraficaAvanceVenta.aspx/ObtenerAgentes",
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

			}
		}
	});
}

function ObtenerDatosGraficaVentasMeta() {
	var Grafica = new Object();
	Grafica.MostrarPor = parseInt($("#cmbMostrarPor").val());
	Grafica.FechaInicial = $("#txtFechaInicial").val();
	Grafica.FechaFinal = $("#txtFechaFinal").val();
	Grafica.IdSucursal = parseInt($("#cmbSucursal").val());
	Grafica.IdUsuario = parseInt($("#cmbAgente").val());

	var Request = JSON.stringify(Grafica);

	$.ajax({
		url: "GraficaAvanceVenta.aspx/ObtenerDatosGraficaVentasMeta",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				var Titulo = json.Modelo.Titulo;
				var Ventas = json.Modelo.Ventas;
				var Metas = json.Modelo.Metas;
				var Ticks = json.Modelo.Ticks;
				GenerarGraficaVentasMeta(Titulo, Ventas, Metas, Ticks);
			} else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});

}

function GenerarGraficaVentasMeta(Titulo, Ventas, Metas, Ticks) {
	$("#divGraficaVentaMeta").html('');

	var Unidad = $("#cmbMostrarPor option:selected").text();

	$.jqplot('divGraficaVentaMeta', [Ventas,Metas], {
		title: Titulo,
		seriesColors: ["#0ac92a", "#1133FF"],
		series: [
			{
				label: "Ventas",
				pointLabels: {  show: true, formatString: "$%'.2f" }
			},
			{ label: "Meta"}
		],
		legend: {
			show: true,
			placement: 'outside'
		},
		seriesDefaults: {
			labelRenderer: $.jqplot.CanvasAxisLabelRenderer
		},
		axes: {
			xaxis: {
				numberTicks: Ticks.length,
				min: 1,
				max: Ticks.length,
				renderer: $.jqplot.DateAxisRenderer,
				label: Unidad,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer,
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickOptions: { angle: 0 }
			},
			yaxis: {
				label: 'Montos',
				labelRenderer: $.jqplot.CanvasAxisLabelRenderer,
				tickRenderer: $.jqplot.CanvasAxisTickRenderer,
				tickOptions:{  formatString: "$%'.2f"}
			}
		}
	});
}