/**/

$(function () {

	MantenerSesion();

	setInterval(MantenerSesion, 1000 * 60 * 1.5);

});

function FiltroInventario() {
	var Inventario = new Object();
	Inventario.pTamanoPaginacion = $('#grdInventario').getGridParam('rowNum');
	Inventario.pPaginaActual = $('#grdInventario').getGridParam('page');
	Inventario.pColumnaOrden = $('#grdInventario').getGridParam('sortname');
	Inventario.pTipoOrden = $('#grdInventario').getGridParam('sortorder');
    Inventario.pClaveInterna = ($("#gs_ClaveInterna").val() == null) ? "" : $("#gs_ClaveInterna").val();
	Inventario.pClave = ($("#gs_Clave").val() == null) ? "" : $("#gs_Clave").val();
	Inventario.pDescripcion = ($("#gs_Descripcion").val() == null) ? "" : $("#gs_Descripcion").val();
    Inventario.pDivision = ($("#gs_Division").val() == null) ? "" : $("#gs_Division").val();
	var Request = JSON.stringify(Inventario);
	$.ajax({
		url: "InventarioReal.aspx/ObtenerInventario",
		data: Request,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') { $('#grdInventario')[0].addJSONData(JSON.parse(jsondata.responseText).d); OcultarBloqueo(); }
			else { console.log(JSON.parse(jsondata.responseText).Message); }
			TerminoInventario();
		}
	});
}

function TerminoInventario() {
	$("td[aria-describedby=grdInventario_Existencia]", "#grdInventario tbody").each(function (index, element) {
		var IdExperienciaReal = $(element).parent("tr").children("td[aria-describedby=grdInventario_IdExperienciaReal]").text();
		var input = $('<input type="text" value="' + $(element).text() + '" style="width:96%;text-align:right;"/>');
		//$(element).html(input);
		//$(input).change(function () { ActualizarExistenciaProducto(IdExperienciaReal, $(this).val()); });
	});
}

function ActualizarExistenciaProducto(IdExperienciaReal, Existencia) {
	var ExistenciaReal = new Object();
	ExistenciaReal.IdExperienciaReal = parseInt(IdExperienciaReal);
	ExistenciaReal.Existencia = parseInt(Existencia);
	var Request = JSON.stringify(ExistenciaReal);
	$.ajax({
		url: "InventarioReal.aspx/ActualizarExistenciaProducto",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			FiltroInventario();
		}
	});
}