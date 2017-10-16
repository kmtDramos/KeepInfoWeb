/**/

var TamanoPaginacion = 1000;
var PaginaActual = 1;
var ColumnaOrden = "";
var pIdEmpresa = 1;

$(function () {

	Inicializar_grdReporteVentasAgentes();

	$("#dialogAnalisisVentasAgentes").dialog({
		autoOpen: false,
		modal: true,
		draggable: false,
		resizable: false,
		width: "1100px",
		height: "auto",
		close: function () {
			$(this).dialog("close").html('');
			
		}
	});

	//##############################################################################
	//////funcion del grid//////
	$("#gbox_grdReporteVentasAgentes").livequery(function () {// MODIFICAR
		$("#grdReporteVentasAgentes").jqGrid('navButtonAdd', '#pagReporteVentasAgentes', {// MODIFICAR
			caption: "Exportar",
			title: "Exportar",
			buttonicon: 'ui-icon-newwin',
			onClickButton: function () {

				//
				var Stored = "sp_ReporteVentasAgente_Exportar";

				//
				var ReporteVentasAgentes = new Object();
				ReporteVentasAgentes.pIdEmpresa = $("#divReproteVentasAgentes").attr("IdEmpresa");
				var Request = JSON.stringify(ReporteVentasAgentes);

				//
				var Archivo = "Reporte_Ventas_Agentes.xls";

				ExportarExcel(Stored, Request, Archivo);
			}
		});
	});

});

function FiltroReporteVentasAgentes() {
	var agente = '';
	var sucursal = -1;
	if ($('#gbox_grdReporteVentasAgentes #gs_Agente').val() != null) {
		agente = $('#gs_Agente').val();
	}

	if ($('#gbox_grdReporteVentasAgentes #gs_Sucursal').val() != null) {
		sucursal = parseInt($('#gbox_grdReporteVentasAgentes #gs_Sucursal').val());
	}

	$.ajax({
		url: 'Dashboard.aspx/ObtenerVentasAgentes',
		data: "{'pTamanoPaginacion':" + $('#grdReporteVentasAgentes').getGridParam('rowNum') + ",'pPaginaActual':" + $('#grdReporteVentasAgentes').getGridParam('page') + ",'pColumnaOrden':'" + $('#grdReporteVentasAgentes').getGridParam('sortname') + "','pTipoOrden':'" + $('#grdReporteVentasAgentes').getGridParam('sortorder') + "','pAgente':'" + agente + "','pIdSucursal':"+sucursal+"}",
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success'){
				$('#grdReporteVentasAgentes')[0].addJSONData(JSON.parse(jsondata.responseText).d);
			} else {
				alert(JSON.parse(jsondata.responseText).Message);
			}
			ObtenerTotales();
			AplicarMascaras(JSON.parse(jsondata.responseText).d);
		}
	});
}

function AplicarMascaras(json) {

	$("td[aria-describedby=grdReporteVentasAgentes_VentasMesAnterior]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = $(element).text();
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_OportunidadBaja]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = $(element).text();
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_OportunidadMedia]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = $(element).text();
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_OportunidadAlta]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = $(element).text();
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_TotalOportunidades]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = parseFloat($(element).text());
		var cantidad = parseFloat(monto).currency();
		var meta = parseFloat($("td[aria-describedby=grdReporteVentasAgentes_Meta]", $(element).parent("tr")).text());
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
		if (monto < meta) {
			$(element).css("color","#f00");
		}
	});

	$("td[aria-describedby=grdReporteVentasAgentes_Pronostico]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = parseFloat($(element).text());
		var meta = parseFloat($("td[aria-describedby=grdReporteVentasAgentes_Meta]", $(element).parent("tr")).text());
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_Meta]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = $(element).text();
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_Ventas]", "#grdReporteVentasAgentes").each(function (index, element) {
		var monto = $(element).text();
		var cantidad = parseFloat(monto).currency();
		$(element).text(cantidad.split(".")[0]).attr("title", cantidad);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_AvanceMeta]", "#grdReporteVentasAgentes").each(function (index, element) {
		var porcentaje = $(element).text();
		var plan = $("td[aria-describedby=grdReporteVentasAgentes_AvanceTiempo]", $(element).parent("tr")).text();
		$(element).text(porcentaje + "%").attr("title", porcentaje + "%");
	});

	$("td[aria-describedby=grdReporteVentasAgentes_AvanceTiempo]", "#grdReporteVentasAgentes").each(function (index, element) {
		var porcentaje = $(element).text() + "%";
		$(element).text(porcentaje).attr("title", porcentaje);
	});

	$("td[aria-describedby=grdReporteVentasAgentes_Consultar]", "#grdReporteVentasAgentes").click(function () {
		var Usuario = new Object();
		TamanoPaginacion = json.NoRegistros;
		Usuario.IdUsuario = json.Elementos[$(this).parent("tr").attr("id") - 1].ID;
		var Request = JSON.stringify(Usuario);
		$("#dialogAnalisisVentasAgentes").obtenerVista({
			url: "Dashboard.aspx/ObtenerAnalisisVentasAgenteEspecifico",
			parametros: Request,
			nombreTemplate: "tmplDashboardAnalisisVentasAgente.html",
			despuesDeCompilar: function (Respuesta) {
				$("#dialogAnalisisVentasAgentes").dialog("open");
			}
		});
	});

	$("td[aria-describedby=grdReporteVentasAgentes_Actividades]", "#grdReporteVentasAgentes").click(function () {
		var Usuario = new Object();
		Usuario.IdUsuario = json.Elementos[$(this).parent("tr").attr("id") - 1].ID;
		var Request = JSON.stringify(Usuario);
		var contenedor = $("<div></div>");
		$(contenedor).obtenerVista({
			url: "Dashboard.aspx/TablaActividadesAFuturoAgente",
			parametros: Request,
			nombreTemplate: "tmplTablaActividadesAFuturoAgente.html",
			despuesDeCompilar: function (Respuesta) {
				$(contenedor).dialog({
					width: "auto",
					height: "auto",
					title: "Actividades a futuro de agente",
					resizable: false,
					draggable: false,
					modal: true,
					close: function () {
						$(contenedor).remove();
					},
					buttons: {
						"Cerrar": function () {
							$(this).dialog("close");
						}
					}
				});
			}
		});
	});

	$("td[aria-describedby=grdReporteVentasAgentes_Prospeciones]", "#grdReporteVentasAgentes").click(function () {
		var Usuario = new Object();
		Usuario.IdUsuario = json.Elementos[$(this).parent("tr").attr("id") - 1].ID;
		var Request = JSON.stringify(Usuario);
		var contenedor = $("<div></div>");
		$(contenedor).obtenerVista({
			url: "Dashboard.aspx/TablaActividadesAFuturoAgente",
			parametros: Request,
			nombreTemplate: "tmplTablaActividadesAFuturoAgente.html",
			despuesDeCompilar: function (Respuesta) {
				$(contenedor).dialog({
					width: "auto",
					height: "auto",
					title: "Actividades a futuro de agente",
					resizable: false,
					draggable: false,
					modal: true,
					close: function () {
						$(contenedor).remove();
					},
					buttons: {
						"Cerrar": function () {
							$(this).dialog("close");
						}
					}
				});
			}
		});
	});

	$("td[aria-describedby=grdReporteVentasAgentes_ClientesAtendidos]", "#grdReporteVentasAgentes").click(function () {
		var Agente = $("td[aria-describedby=grdReporteVentasAgentes_Agente]", $(this).parent("tr")).text();
		var url = window.location.toString();
		url = url.replace("Dashboard.aspx","Cliente.aspx");
		var cartera = window.open(url);
		cartera.onload = function () {
			setTimeout(function () {
				$("#gs_Agente", cartera.document).val(Agente);
				cartera.FiltroCliente();
			}, 1000);
		}
	});

	/*
	$("td[aria-describedby=grdReporteVentasAgentes_Grafica]", "#grdReporteVentasAgentes").each(function (index, element) {
		$(element).html('<img src="../Images/pie-chart.png"/>').css({ "cursor": "pointer" }).click(function () {

		});
	});
	*/
}

function ObtenerTotales() {
	var Reporte = new Object();
	Reporte.Agente = $("#gs_Agente").val();
	Reporte.IdSucursal = (!isNaN(parseInt($("#gs_Sucursal").val()))) ? parseInt($("#gs_Sucursal").val()) : -1;

	var Request = JSON.stringify(Reporte);

	$.ajax({
		url: "Dashboard.aspx/ObtenerTotales",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				$("#lblMeta").text(json.Modelo.Meta);
				$("#lblPronostico").text(json.Modelo.Pronostico);
				$("#lblDiferencia").text(json.Modelo.Diferencia);
				$("#lblVenta").text(json.Modelo.Venta);
			} else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}