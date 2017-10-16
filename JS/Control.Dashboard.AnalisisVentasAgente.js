/**/

$(function () {
	Inicializar_grdVentasAgente();
});


function FiltroVentasAgente() {
	var idoportunidad = '';
	if ($('#gbox_grdVentasAgente #gs_IdOportunidad').val() != null) {
		idoportunidad = $('#gs_IdOportunidad').val();
	}

	var cliente = '';
	if ($('#gbox_grdVentasAgente #gs_Cliente').val() != null) {
		cliente = $('#gs_Cliente').val();
	}

	var formacontacto = '';
	if ($('#gbox_grdVentasAgente #gs_FormaContacto').val() != null) {
		formacontacto = $('#gs_FormaContacto').val();
	}

	var oportunidad = '';
	if ($('#gbox_grdVentasAgente #gs_Oportunidad').val() != null) {
		oportunidad = $('#gs_Oportunidad').val();
	}

	var oportunidadbaja = '';
	if ($('#gbox_grdVentasAgente #gs_OportunidadBaja').val() != null) {
		oportunidadbaja = $('#gs_OportunidadBaja').val();
	}

	var oportunidadmedia = '';
	if ($('#gbox_grdVentasAgente #gs_OportunidadMedia').val() != null) {
		oportunidadmedia = $('#gs_OportunidadMedia').val();
	}

	var oportunidadalta = '';
	if ($('#gbox_grdVentasAgente #gs_OportunidadAlta').val() != null) {
		oportunidadalta = $('#gs_OportunidadAlta').val();
	}

	var facturaado = '';
	if ($('#gbox_grdVentasAgente #gs_Facturaado').val() != null) {
		facturaado = $('#gs_Facturaado').val();
	}

	var fechacreacion = '';
	if ($('#gbox_grdVentasAgente #gs_FechaCreacion').val() != null) {
		fechacreacion = $('#gs_FechaCreacion').val();
	}

	var fechacierre = '';
	if ($('#gbox_grdVentasAgente #gs_FechaCierre').val() != null) {
		fechacierre = $('#gs_FechaCierre').val();
	}

	var idtipocliente = -1;
	if ($("#gs_TipoCliente").val() != null) {
		idtipocliente = $("#gs_TipoCliente").val();
	}

	var iddivision = -1;
	if ($("#gs_Division").val() != null) {
		iddivision = $("#gs_Division").val();
	}

	var actividadesafuturo = -1;
	if ($("#gs_ActividadesAFuturo").val() != null) {
		actividadesafuturo = $("#gs_ActividadesAFuturo").val();
	}

	var idusuario = parseInt($("#divContenedorAnalisisVentasAgente").attr("IdUsuario"));

	$.ajax({
		url: 'Dashboard.aspx/ObtenerOportunidadesClienteAgente',
		data: "{'pTamanoPaginacion':" + $('#grdVentasAgente').getGridParam('rowNum') + ",'pPaginaActual':" + $('#grdVentasAgente').getGridParam('page') + ",'pColumnaOrden':'" + $('#grdVentasAgente').getGridParam('sortname') + "','pTipoOrden':'" + $('#grdVentasAgente').getGridParam('sortorder') + "','pIdOportunidad':'" + idoportunidad + "','pCliente':'" + cliente + "','pFormaContacto':'" + formacontacto + "','pOportunidad':'" + oportunidad + "','pOportunidadBaja':'" + oportunidadbaja + "','pOportunidadMedia':'" + oportunidadmedia + "','pOportunidadAlta':'" + oportunidadalta + "','pFacturaado':'" + facturaado + "','pFechaCreacion':'" + fechacreacion + "','pFechaCierre':'" + fechacierre + "','IdUsuario':" + idusuario + ",'IdTipoCliente':"+ idtipocliente +",'IdDivision':"+ iddivision +",'Seguimiento':"+ actividadesafuturo +"}",
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') {
				$('#grdVentasAgente')[0].addJSONData(JSON.parse(jsondata.responseText).d);
				InitColumnasGridOportunidades();
			}
			else {
				alert(JSON.parse(jsondata.responseText).Message);
			}
		}
	});
}

function InitColumnasGridOportunidades() {

	$("td[aria-describedby=grdVentasAgente_OportunidadAlta]", "#grdVentasAgente tbody").each(function (index, elemento) {
		var valor = parseFloat($(elemento).text().replace("$", "").replace(",", "").replace(",", "").replace(",", ""));
		if (valor >= 100000) {
			$(elemento).css("font-weight","bold");
		}
	});

	$("td[aria-describedby=grdVentasAgente_OportunidadMedia]", "#grdVentasAgente tbody").each(function (index, elemento) {
		var valor = parseFloat($(elemento).text().replace("$", "").replace(",", "").replace(",", "").replace(",", ""));
		if (valor >= 100000) {
			$(elemento).css("font-weight", "bold");
		}
	});

	$("td[aria-describedby=grdVentasAgente_OportunidadBaja]", "#grdVentasAgente tbody").each(function (index, elemento) {
		var valor = parseFloat($(elemento).text().replace("$", "").replace(",", "").replace(",", "").replace(",", ""));
		if (valor >= 100000) {
			$(elemento).css("font-weight", "bold");
		}
	});

	$("td[aria-describedby=grdVentasAgente_FechaCreacion]", "#grdVentasAgente tbody").each(function (index, elemento) {
		var dias = parseInt($(elemento).text());
		if (dias <= 30) {
			$(elemento).parent("tr").children("td").css("color", "#090");
		} else if (dias > 30 && dias < 60) {
			$(elemento).parent("tr").children("td").css("color", "#D90");
		} else if (dias >= 60) {
			$(elemento).parent("tr").children("td").css("color", "#D00");
		}
	});

	$("td[aria-describedby=grdVentasAgente_ActividadesAFuturo]", "#grdVentasAgente tbody").each(function (index, elemento) {
		var actividad = parseInt($(elemento).text());
		if (actividad == 1) {
			$(elemento).html('<img src="../Images/true.png" height="22"/>');
		} else {
			$(elemento).html('');
		}
		$(elemento).click(function () {
			var IdOportunidad = parseInt($("td[aria-describedby=grdVentasAgente_IdOportunidad]", $(elemento).parent("tr")).text());
			ObtenerFormaAgregarActividad(0, IdOportunidad);
		});
	});

}