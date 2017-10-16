/*
*/

$(function () {
	
	setTimeout(MantenerSesion, 10000);
	setInterval(MantenerSesion, 1000 * 60 * 2.5);

	$("#txtFechaInicial").datepicker({
		dateFormat:"dd/mm/yy"
	}).change(FiltroAnalisisCartera);
	$("#txtFechaFinal").datepicker({
		dateFormat: "dd/mm/yy"
	}).change(FiltroAnalisisCartera);

});

function Termino_grdAnalisisCartera() {
	$("td[aria-describedby=grdAnalisisCartera_Vendido]", "#grdAnalisisCartera tbody").each(function (index, element) {
		$(element).text(formato.moneda($(element).text(), '$'));
	});

	$("td[aria-describedby=grdAnalisisCartera_Ingreso]", "#grdAnalisisCartera tbody").each(function (index, element) {
		$(element).text(formato.moneda($(element).text(), '$'));
	});

	$("td[aria-describedby=grdAnalisisCartera_Saldo]", "#grdAnalisisCartera tbody").each(function (index, element) {
		$(element).text(formato.moneda($(element).text(), '$'));
	});

	$("td[aria-describedby=grdAnalisisCartera_VentasDivision]", "#grdAnalisisCartera tbody").each(function (index, element) {
		$(element).click(function () {
			var IdCliente = parseInt($(element).parent("tr").children("td[aria-describedby=grdAnalisisCartera_IdCliente]").text());
			AbrirTablaVentasFamilia(IdCliente);
		});
		$(element).html('<img src="../Images/view.png" height="18"/>');
	});
}

function FiltroAnalisisCartera() {
	MostrarBloqueo();
	var Cartera = new Object();
	Cartera.pTamanoPaginacion = ($('#grdAnalisisCartera').getGridParam('rowNum') != null) ? $('#grdAnalisisCartera').getGridParam('rowNum') : "50";
	Cartera.pPaginaActual = ($('#grdAnalisisCartera').getGridParam('page') != null) ? $('#grdAnalisisCartera').getGridParam('page') : "1";
	Cartera.pColumnaOrden = ($('#grdAnalisisCartera').getGridParam('sortname') != null) ? $('#grdAnalisisCartera').getGridParam('sortname') : "Cliente";
	Cartera.pTipoOrden = ($('#grdAnalisisCartera').getGridParam('sortorder') != null) ? $('#grdAnalisisCartera').getGridParam('sortorder') : "ASC";
	Cartera.pCliente = ($("#gs_Cliente").val() != null) ? $("#gs_Cliente").val() : "";
	Cartera.pIdTipoCliente = ($("#gs_IdTipoCliente").val() != null) ? parseInt($("#gs_IdTipoCliente").val()) : -1;
	Cartera.pAgente = ($("#gs_Agente").val()) ? $("#gs_Agente").val() : "";
	Cartera.pFechaInicial = ($("#txtFechaInicial").val() != null) ? $("#txtFechaInicial").val() : "";
	Cartera.pFechaFinal = ($("#txtFechaFinal").val() != null) ? $("#txtFechaFinal").val() : "";

	var Request = JSON.stringify(Cartera);

	$.ajax({
		url: 'AnalisisCartera.aspx/ObtenerAnalisisCartera',
		data: Request,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') $('#grdAnalisisCartera')[0].addJSONData(JSON.parse(jsondata.responseText).d);
			else alert(JSON.parse(jsondata.responseText).Message);
			OcultarBloqueo();
		}
	});
}

function AbrirTablaVentasFamilia(IdCliente) {
	var Cliente = new Object();
	Cliente.IdCliente = IdCliente;

	var Request = JSON.stringify(Cliente);

	var ventana = $("<div></div>");
	$(ventana).obtenerVista({
		url: "AnalisisCartera.aspx/ObtenerTablaVentasClienteDivisiones",
		parametros: Request,
		nombreTemplate: "tmplTablaVentasClienteDivisiones.html",
		despuesDeCompilar: function () {
			$(ventana).dialog({
				modal: true,
				draggable: false,
				resizable: false,
				width: 'auto',
				height: 'auto',
				close: function () {
					$(this).remove();
				},
				buttons: {
					"Cerrar": function () {
						$(this).dialog("close");
					}
				}
			});
		}
	});
}