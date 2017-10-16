/**/


$(function () {
	
	MantenerSesion();

	setTimeout(MantenerSesion, 1000 * 60);

	ObtenerTablaProspeccion();

	$("#btnAgregarFilaProspeccion").click(ObtenerAgregarFilaProspeccion);

});

function ObtenerTablaProspeccion()
{
	$("#divTablaProspeccion").obtenerVista({
		url: "Prospeccion.aspx/ObtenerTablaProspeccion",
		nombreTemplate: "tmplTablaProspeccion.html",
		despuesDeCompilar: function () {
			$("input", "#tblProspeccion").change(function () {
				var fila = $(this).parent("td").parent("tr");
				GuardarFila(fila);
			});
			$('input[type=text]', "#tblProspeccion").each(function (index, element) {
				$(element).autocomplete({
					source: function (request, response) {
						var Cliente = new Object();
						Cliente.pCliente = $(element).val();

						var Request = JSON.stringify(Cliente);
						$.ajax({
							url: 'Prospeccion.aspx/BuscarCliente',
							type: 'POST',
							data: Request,
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
					},
					focus: function (event, ui) {
					},
					change: function (event, ui) { },
					open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
					close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
				});
			});
		}
	});
}

function ObtenerAgregarFilaProspeccion()
{
	var tr = $('<tr class="fila" IdProspeccion="0"></tr>');
	$(tr).obtenerVista({
		url: "Prospeccion.aspx/ObtenerAgregarFilaProspeccion",
		nombreTemplate: "tmplFilaProspeccion.html",
		despuesDeCompilar: function () {
			$("tbody", "#tblProspeccion").append(tr);
			$("input", tr).change(function () {
				GuardarFila(tr);
			});

			$('input[type=text]', tr).autocomplete({
				source: function (request, response) {
					var Cliente = new Object();
					Cliente.pCliente = $('input[type=text]', tr).val();

					var Request = JSON.stringify(Cliente);
					$.ajax({
						url: 'Prospeccion.aspx/BuscarCliente',
						type: 'POST',
						data: Request,
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
				},
				focus: function (event, ui) {
				},
				change: function (event, ui) { },
				open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
				close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
			});
		}
	});
}

function GuardarFila(fila)
{
	var Prospeccion = new Object();
	Prospeccion.IdProspeccion = parseInt($(fila).attr("IdProspeccion"));
	Prospeccion.Cliente = $("input[name=Cliente]", fila).val();

	var EstatusProspeccion = [];
	$("input[type=checkbox]", fila).each(function (index, element) {
		var Estatus = new Object();
		Estatus.IdEstatusProspeccion = parseInt($(element).val());
		Estatus.Baja = !$(element).is(":checked");
		EstatusProspeccion.push(Estatus);
	});
	
	Prospeccion.EstatusProspeccion = EstatusProspeccion;

	var Request = JSON.stringify(Prospeccion);

	$.ajax({
		url: "Prospeccion.aspx/GuardarProspeccion",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				$(fila).attr("IdProspeccion", json.Modelo.IdProspeccion);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function EliminarFila(element)
{
	var ventana = $('<div><p>¿Deseas eliminar la prospección?</p></div>');
	$(ventana).dialog({
		modal: true,
		draggable: false,
		resizable: false,
		close: function () {
			$(ventana).remove();
		},
		buttons:{
			"Eliminar": function () {
				var Prospeccion = new Object();
				var fila = $(element).parent("td").parent("tr");

				Prospeccion.IdProspeccion = parseInt($(fila).attr("IdProspeccion"));

				var Request = JSON.stringify(Prospeccion);

				$.ajax({
					url: "Prospeccion.aspx/EliminarProspeccion",
					type: "post",
					data: Request,
					dataType: "json",
					contentType: "application/json; charset=utf-8",
					success: function (Respuesta) {
						var json = JSON.parse(Respuesta.d);
						if (json.Error == 0)
						{
							$(fila).remove();
						}
						else
						{
							MostrarMensajeError(json.Descripcion);
						}
					}
				});
				$(ventana).dialog("close");
			},
			"Cancelar": function () {
				$(ventana).dialog("close");
			}
		}
	});
}