//----------DHTMLX----------//
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function () {
	setInterval(MantenerSesion, 150000); //2.5 minutos

	$(window).unload(function () {
		ActualizarPanelControles("Cotizacion");
	});

	ObtenerTotalesEstatusCotizacion();
	ObtenerFormaFiltrosCotizacion();

	//////funcion del grid//////
	$("#gbox_grdCotizacion").livequery(function () {
		$("#grdCotizacion").jqGrid('navButtonAdd', '#pagCotizacion', {
			caption: "Exportar",
			title: "Exportar",
			buttonicon: 'ui-icon-newwin',
			onClickButton: function () {

				var pRazonSocial = "";
				var pFolio = "";
				var pIdEstatusCotizacion = -1;
				var pAI = 0;

				var pFechaInicial = "";
				var pFechaFinal = "";
				var pPorFecha = 0;

				var idestatuscotizacion = $("#tblCotizacionTotalesEstatus").attr("idEstatusCotizacionSeleccionado");
				pIdEstatusCotizacion = validaNumero(idestatuscotizacion) ? idestatuscotizacion : -1;

				if ($('#gs_Folio').val() != null) { pFolio = $("#gs_Folio").val(); }

				if ($('#gs_RazonSocial').val() != null) { pRazonSocial = $("#gs_RazonSocial").val(); }

				if ($('#gs_AI').val() != null) { pAI = $("#gs_AI").val(); }

				if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

					if ($("#chkPorFecha").is(':checked')) {
						pPorFecha = 1;
					}
					else {
						pPorFecha = 0;
					}

					pFechaInicial = $("#txtFechaInicial").val();
					pFechaInicial = ConvertirFecha(pFechaInicial, 'aaaammdd');
				}
				if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
					pFechaFinal = $("#txtFechaFinal").val();
					pFechaFinal = ConvertirFecha(pFechaFinal, 'aaaammdd');
				}

				$.UnifiedExportFile({
					action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
						IsExportExcel: true,
						pRazonSocial: pRazonSocial,
						pFolio: pFolio,
						pIdEstatusCotizacion: pIdEstatusCotizacion,
						pAI: pAI,
						pFechaInicial: pFechaInicial,
						pFechaFinal: pFechaFinal,
						pPorFecha: pPorFecha

					}, downloadType: 'Normal'
				});

			}
		});
	});

	$('#dialogAgregarCotizacion, #dialogEditarCotizacion').on('change', '#cmbTipoMoneda', function (event) {
		CambiarPrecio();
	});

	$(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCotizacion", function () {
		ObtenerFormaAgregarCotizacion();
	});

	$('#dialogAgregarCotizacion, #dialogEditarCotizacion').on('change', '#cmbOportunidad', function (event) {
		var request = new Object();
		request.pIdOportunidad = $(this).val();
		ObtenerListaNivelInteres(JSON.stringify(request));
		ObtenerListaCampanaOportunidad(JSON.stringify(request));
	});

	$('#dialogAgregarCotizacion').dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		open: function () {

			$("input[name=ProductoServicio]:radio").click(function (evento) {
				if (this.value == 1) {
					MuestraObjetos(1);
				} else {
					MuestraObjetos(0);
				}
				LimpiarDatosBusqueda();
			});
			eliminarConceptoEditar();

			$("#txtPrecio").keypress(function (event) { return ValidarNumeroPunto(event, this.value); });
			$("#txtCantidad").keypress(function (event) { return ValidarNumeroPunto(event, this.value); });
			$("#txtCantidad").keypress(function (event) { return ValidarNumeroPunto(event, this.value); });
			$("#divImprimir").hide();
		},
		close: function () {
			$("#divFormaAgregarCotizacion").remove();
		},
		buttons: {
			"Guardar como borrador": function () {
				AgregarCotizacion();
			},
			"Generar cotización": function () {
				$("#dialogGenerarCotizacion").dialog("open");
			},
			"Salir": function () {
				$(this).dialog("close")
			}
		}
	});

	$('#grdCotizacion').one('click', '.div_grdCotizacion_AI', function (event) {
		var registro = $(this).parents("tr");
		var estatusBaja = $(registro).children("td[aria-describedby='grdCotizacion_AI']").children().attr("baja")
		var idCotizacion = $(registro).children("td[aria-describedby='grdCotizacion_IdCotizacion']").html();
		var baja = "false";
		idEstatusCotizacion = 1;
		if (estatusBaja == "0" || estatusBaja == "False") {
			baja = "true";
			idEstatusCotizacion = 4;
		}

		SetCambiarEstatus(idCotizacion, baja, idEstatusCotizacion);
	});

	$("#dialogAgregarCotizacion, #dialogEditarCotizacion").on("click", "#btnBajarPartida", function () {
		AgregarCotizacionDetalle();
	});

	$("#grdCotizacion").on("click", ".imgFormaConsultarCotizacion", function () {
		var registro = $(this).parents("tr");
		var Cotizacion = new Object();
		Cotizacion.pIdCotizacion = parseInt($(registro).children("td[aria-describedby='grdCotizacion_IdCotizacion']").html());
		ObtenerFormaConsultarCotizacion(JSON.stringify(Cotizacion));
	});

	$('#dialogConsultarCotizacion').dialog({
		autoOpen: false,
		height: 'auto',
		width: '920',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		buttons: {
			"Aceptar": function () {
				$(this).dialog("close");
			}
		},
		close: function () {
			$("#divFormaConsultarCotizacion").remove();
		}
	});

	$('#dialogEditarCotizacion').dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		open: function () {
			$("input[name=ProductoServicio]:radio").click(function (evento) {
				if (this.value == 1) {
					MuestraObjetos(1);
				} else {
					MuestraObjetos(0);
				}
				LimpiarDatosBusqueda();
			});

			$('#divFormaEditarCotizacion').on('change', '#cmbTipoMoneda', function (event) {
				$("#cmbTipoMoneda option[value=" + $(this).val() + "]").attr("selected", true);
			});



			$("#dialogEditarCotizacion").on("click", "#btnAgregarPartida", function () {
				AgregarCotizacionDetalle();
			});

			$("#grdCotizacionDetalleEditar").on("click", ".imgEliminarConceptoEditar", function () {
				var registro = $(this).parents("tr");
				var pCotizacionDetalle = new Object();
				pCotizacionDetalle.pIdCotizacionDetalle = parseInt($(registro).children("td[aria-describedby='grdCotizacionDetalleEditar_IdCotizacionDetalle']").html());
				pCotizacionDetalle.pIva = $("#IVAActual").html();
				var oRequest = new Object();
				oRequest.pCotizacionDetalle = pCotizacionDetalle;
				SetEliminarCotizacionDetalle(JSON.stringify(oRequest));
			});

			$("#txtPrecio").keypress(function (event) { return ValidarNumeroPunto(event, this.value); });
			$("#txtCantidad").keypress(function (event) { return ValidarNumeroPunto(event, this.value); });
			$("#txtCantidad").keypress(function (event) { return ValidarNumeroPunto(event, this.value); });
		},
		close: function () {
			$("#divFormaEditarCotizacion").remove();
		}
	});

	$(".spanFiltroTotal").click(function () {
		var idEstatusCotizacion = $(this).attr("idEstatusCotizacion");
		$("#tblCotizacionTotalesEstatus").attr("idEstatusCotizacionSeleccionado", idEstatusCotizacion);
		FiltroCotizacion();
	});

	$('#grdCotizacion').on('click', '.div_grdCotizacion_IdEstatusCotizacion ', function (event) {
		var estatusPedido = $(this).attr("idEstatusCotizacion");
		var registro = $(this).parents("tr");
		var idCotizacion = $(registro).children("td[aria-describedby='grdCotizacion_IdCotizacion']").html();
		var puedeEditarVigenciaCotizacion = $("#tblCotizacionTotalesEstatus").attr("puedeEditarVigenciaCotizacion");
		var puedePasarPedidoACotizado = $("#tblCotizacionTotalesEstatus").attr("puedePasarPedidoACotizado");

		var valor = estatusPedido;
		switch (valor) {
			case '2': //cotizacion
				$("#dialogGenerarPedido").attr("idCotizacion", idCotizacion);
				$("#dialogGenerarPedido").dialog("open");
				break;
			case '3': //pedido 
				$("#dialogRegresarACotizacion").attr("idCotizacion", idCotizacion);
				$("#dialogRegresarACotizacion").dialog("open");
				break;
			case '5': //vencido
				if (puedeEditarVigenciaCotizacion == 1) {
					$("#dialogActivarCotizacionVencida").attr("idCotizacion", idCotizacion);
					$("#dialogActivarCotizacionVencida").dialog("open");
				}

				break;
			default:
				return false;
		}

	});

	$("#dialogGenerarPedido").dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Aceptar": function () {
				SetGenerarPedido($("#dialogGenerarPedido").attr("idCotizacion"), true);
			},
			"Cancelar": function () {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
				$(this).dialog("close");
			}
		}
	});

	$("#dialogRegresarACotizacion").dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Aceptar": function () {
				SetRegresarACotizacion($("#dialogRegresarACotizacion").attr("idCotizacion"), true);
			},
			"Cancelar": function () {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
				$(this).dialog("close");
			}
		}
	});

	$("#dialogGenerarCotizacion").dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Generar cotización": function () {
				GenerarCotizacion();
				$(this).dialog("close");
			},
			"Cancelar": function () {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
				$(this).dialog("close");
			}
		}
	});

	$("#dialogAgregarCotizacion, #dialogEditarCotizacion").on("click", "#divTipoCambio", function () {
		var validacion = validarExisteMonedaDestino();
		if (validacion != "")
		{ MostrarMensajeError(validacion); return false; }
		$("#dialogProporcionarClaveAutorizacion").obtenerVista({
			nombreTemplate: "tmplObtenerAutorizacionTipoCambio.html",
			despuesDeCompilar: function () {
				$("#dialogProporcionarClaveAutorizacion").dialog("open");
			}
		});
	});

	$('#dialogProporcionarClaveAutorizacion').dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Aceptar": function () {
				ProporcionarClaveAutorizacion();
			},
			"Cerrar": function () {
				$(this).dialog("close");
			}
		}
	});

	$("#dialogAgregarCotizacion, #dialogEditarCotizacion").on("click", "#divIVA", function () {
		$("#dialogProporcionarClaveAutorizacionIVA").obtenerVista({
			nombreTemplate: "tmplObtenerAutorizacionIVA.html",
			despuesDeCompilar: function () {
				$("#dialogProporcionarClaveAutorizacionIVA").dialog("open");
			}
		});
	});

	$('#dialogProporcionarClaveAutorizacionIVA').dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Aceptar": function () {
				ProporcionarClaveAutorizacionIVA();
			},
			"Cerrar": function () {
				$(this).dialog("close");
			}
		}
	});

	$("#dialogRegresarABorrador").dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Aceptar": function () {
				$("#dialogConsultarCotizacion").dialog("close");
				var Cotizacion = new Object();
				Cotizacion.IdCotizacion = parseInt($("#divFormaConsultarCotizacion").attr("idCotizacion"));
				Cotizacion.IdEstatusCotizacion = parseInt($("#divFormaConsultarCotizacion").attr("idEstatusCotizacion"));
				ObtenerFormaEditarCotizacion(JSON.stringify(Cotizacion))
				$(this).dialog("close");
				$("#grdCotizacion").trigger("reloadGrid");
			},
			"Cancelar": function () {
				$(this).dialog("close");
			}
		}
	});

	$("#dialogAgregarCotizacion, #dialogEditarCotizacion, #dialogConsultarCotizacion").on("click", "#divImprimir", function () {
		var IdCotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion, #divFormaConsultarCotizacion").attr("idCotizacion");
		Imprimir(IdCotizacion);
	});

	$("#dialogActivarCotizacionVencida").dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
		},
		buttons: {
			"Aceptar": function () {

				var pCotizacion = new Object();
				pCotizacion.IdCotizacion = parseInt($("#dialogActivarCotizacionVencida").attr("idCotizacion"));
				pCotizacion.ValidoHasta = $("#txtNuevaVigencia").val();

				var validacion = validaNuevaVigencia(pCotizacion);
				if (validacion != "")
				{ MostrarMensajeError(validacion); return false; }

				var oRequest = new Object();
				oRequest.pCotizacion = pCotizacion;
				ActivarCotizacionVencida(JSON.stringify(oRequest));

			},
			"Cancelar": function () {
				//$("#grdCotizacion").trigger("reloadGrid");
				//ObtenerTotalesEstatusCotizacion();
				$(this).dialog("close");
			}
		},
		open: function () {
			$("#txtNuevaVigencia").datepicker();
		}
	});

	$('#dialogDeclinarCotizacion').dialog({
		autoOpen: false,
		height: 'auto',
		width: 'auto',
		modal: true,
		draggable: false,
		resizable: false,
		show: 'fade',
		hide: 'fade',
		close: function () {
			$("#divDeclinarCotizacion").remove();
		}
	});

});


//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCotizacion() {
	$("#dialogAgregarCotizacion").obtenerVista({
		nombreTemplate: "tmplAgregarCotizacion.html",
		url: "Cotizacion.aspx/ObtenerFormaAgregarCotizacion",
		despuesDeCompilar: function (pRespuesta) {
			Modelo = pRespuesta.modelo;
			if (Modelo.Permisos.puedeAgregarCotizacion == 1) {
				$("#txtNota").val("LIBRE A BORDO: MEXICO D.F./ MTY TODA CANCELACIÓN,  DEVOLUCIÓN DE PRODUCTO EN CONDICIONES DE VENTA  O CHEQUE DEVUELTO GENERA UN CARGO DEL 20% MAS IVA CONDICIONES DE PAGO. LOS PRECIOS PUEDEN SER SUJETOS A CAMBIOS SIN PREVIO AVISO.");
				$("#txtMonedaProducto").html($("#cmbTipoMoneda option:selected").text());
				$("#txtMonedaServicio").html($("#cmbTipoMoneda option:selected").text());
				Inicializar_grdCotizacionDetalle();
				autocompletarCliente();
				autocompletarProductoClave();
				autocompletarProductoDescripcion();
				autocompletarServicioClave();
				autocompletarServicioDescripcion();
				$("#txtValidoHasta").datepicker();
				$("#dialogAgregarCotizacion").dialog("open");
				$("#grdCotizacionDetalle").jqGrid('sortableRows', {
					update: function (e, ui) {
						OrdenaPartidas();
					}
				});

				ocultaAlAbrir();
			} else {
				MostrarMensajeError("No tiene permiso para agregar cotizaciones");
				return false;
			}

		}
	});

}

function ObtenerListaContactoOrganizacion(pRequest) {
	$("#cmbContactoOrganizacion").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		modelo: pRequest.ListaCombo
	});
}

function ObtenerListaAgente(pRequest) {
	$("#cmbUsuarioAgente").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		modelo: pRequest.ListaCombo
	});
}

function ObtenerFormaConsultarCotizacion(pIdCotizacion) {
	$("#dialogConsultarCotizacion").obtenerVista({
		nombreTemplate: "tmplConsultarCotizacion.html",
		url: "Cotizacion.aspx/ObtenerFormaCotizacion",
		parametros: pIdCotizacion,
		despuesDeCompilar: function (pRespuesta) {
			Modelo = pRespuesta.modelo;
			if (Modelo.Permisos.puedeConsultarCotizacion == 1) {

				Inicializar_grdCotizacionDetalleConsultar();

				$('#txtIVADetalle').text(formato.moneda(Modelo.IVA, '$'));
				$('#txtTotalDetalle').text(formato.moneda(Modelo.Total, '$'));

				if (Modelo.IdEstatusCotizacion == 2) {
					$("#dialogConsultarCotizacion").dialog("option", "buttons", {
						"Editar": function () {
							$("#dialogRegresarABorrador").dialog("open");
						}
					});
				}
				else if (Modelo.IdEstatusCotizacion == 1) {
					$("#dialogConsultarCotizacion").dialog("option", "buttons", {
						"Editar": function () {
							var Cotizacion = new Object();
							Cotizacion.IdCotizacion = parseInt($("#divFormaConsultarCotizacion").attr("idCotizacion"));
							Cotizacion.IdEstatusCotizacion = parseInt(Modelo.IdEstatusCotizacion);
							ObtenerFormaEditarCotizacion(JSON.stringify(Cotizacion))
							$(this).dialog("close");
						}
					});
				}
				else
				{
					$("#dialogConsultarCotizacion").dialog("option", "buttons", {
						"Salir": function () {
							$(this).dialog("close");
						}
					});
				}
				$("#dialogConsultarCotizacion").dialog("open");
			}
			else
			{
				MostrarMensajeError("No tiene permiso para consultar cotizaciones");
				return false;
			}
		}
	});
	
	
}

function ObtenerListaDescuento(pRequest) {
	$("#cmbDescuento").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		modelo: pRequest.ListaCombo
	});
}

function ObtenerListaTipoMoneda(pRequest) {
	$("#cmbTipoMonedaOrigen").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		modelo: pRequest.ListaCombo
	});
}

function ProporcionarClaveAutorizacion() {
	var IdTipoMonedaProducto = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdTipoMonedaProducto");
	var IdTipoMonedaServicio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdTipoMonedaServicio");
	var IdTipoMonedaDestino = IdTipoMonedaServicio == "0" ? IdTipoMonedaProducto : IdTipoMonedaServicio;
	var pRequest = new Object();

	pRequest.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
	pRequest.IdTipoMonedaDestino = IdTipoMonedaDestino == undefined ? "0" : IdTipoMonedaDestino;
	pRequest.ClaveAutorizacion = $("#txtClaveAutorizacion").val();

	var iddocumento = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion");
	pRequest.IdDocumento = validaNumero(iddocumento) ? iddocumento : 0;

	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ObtenerAutorizacionTipoCambio",
		data: JSON.stringify(pRequest),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Table[0] != undefined) {
				var precioorigen = QuitarFormatoNumero($("#spanPrecioOrigen").text());
				precioorigen = parseFloat(precioorigen);
				var recalcula = precioorigen / respuesta.Table[0].TipoCambio;
				$("#txtPrecio").val(recalcula);
				$("#TipoCambioActual").html(formato.moneda(respuesta.Table[0].TipoCambio, "$", 4));
				$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionTipoCambio", respuesta.Table[0].IdAutorizacionTipoCambio);
				$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("claveAutorizacion", respuesta.Table[0].ClaveAutorizacion);
				DesactivaClaveAutorizacion(JSON.stringify(pRequest));
				$("#divTipoCambio").hide();
			}
		},
		complete: function () {
			$("#dialogProporcionarClaveAutorizacion").dialog("close");

		}
	});
}

function DesactivaClaveAutorizacion(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/DesactivaAutorizacionTipoCambio",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
		},
		complete: function () {
		}
	});
}

function ProporcionarClaveAutorizacionIVA() {

	var pRequest = new Object();
	pRequest.ClaveAutorizacion = $("#txtClaveAutorizacionIVA").val();
	var iddocumento = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion");
	pRequest.IdDocumento = validaNumero(iddocumento) ? iddocumento : 0;

	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ObtenerAutorizacionIVA",
		data: JSON.stringify(pRequest),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Table[0] != undefined) {
				$("#IVAActual").html(respuesta.Table[0].IVA);
				$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionIVA", respuesta.Table[0].IdAutorizacionIVA);
				$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("claveAutorizacionIVA", respuesta.Table[0].ClaveAutorizacion);
				DesactivaClaveAutorizacionIVA(JSON.stringify(pRequest));
				$("#divIVA").hide();
			}
		},
		complete: function () {
			$("#dialogProporcionarClaveAutorizacionIVA").dialog("close");
		}
	});
}

function DesactivaClaveAutorizacionIVA(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/DesactivaAutorizacionIVA",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
		},
		complete: function () {
		}
	});
}

//-----------AJAX-----------//
//-Funciones de acciones-//
function autocompletarCliente() {
	$('#txtRazonSocial').autocomplete({
		source: function (request, response) {
			var pRequest = new Object();
			pRequest.pRazonSocial = $("#txtRazonSocial").val();
			$.ajax({
				type: 'POST',
				url: 'Cotizacion.aspx/BuscarRazonSocial',
				data: JSON.stringify(pRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente", "0");
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdCliente = ui.item.id;
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente", pIdCliente);

			var request = new Object();
			request.pIdCliente = pIdCliente;
			ObtenerListaOportunidades(JSON.stringify(request));

			var Cliente = new Object();
			Cliente.IdCliente = pIdCliente;
			ObtenerDatosCliente(JSON.stringify(Cliente));
		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

function autocompletarProductoClave() {
	$('#txtClaveProducto').autocomplete({
		source: function (request, response) {
			var pProducto = new Object();
			pProducto.Producto = $("#txtClaveProducto").val();
			pProducto.TipoBusqueda = "C";

			var oRequest = new Object();
			oRequest.pProducto = pProducto;
			$.ajax({
				type: 'POST',
				url: 'Cotizacion.aspx/BuscarProducto',
				data: JSON.stringify(oRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
					$("#txtDescripcionProducto").val("");
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						if (pProducto.TipoBusqueda == 'C') {
							return { label: item.Clave, value: item.Clave, id: item.IdProducto, descripcion: item.Producto }
						}
						else {
							return { label: item.Clave, value: item.Clave, id: item.IdProducto }
						}
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdProducto = ui.item.id;
			var pProducto = ui.item.descripcion;
			var pClave = ui.item.value;
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", pIdProducto);
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");

			$("#txtClaveProducto").val(pClave);
			$("#txtDescripcionProducto").val(pProducto);
			var Producto = new Object();
			Producto.IdProducto = pIdProducto;
			obtenerProducto(Producto);
		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

function autocompletarProductoDescripcion() {
	$('#txtDescripcionProducto').autocomplete({
		source: function (request, response) {
			var pProducto = new Object();

			pProducto.Producto = $("#txtDescripcionProducto").val();
			pProducto.TipoBusqueda = "P";

			var oRequest = new Object();
			oRequest.pProducto = pProducto;
			$.ajax({
				type: 'POST',
				url: 'Cotizacion.aspx/BuscarProducto',
				data: JSON.stringify(oRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
					$("#txtClaveProducto").val("");
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						if (pProducto.TipoBusqueda == 'P') {
							return { label: item.Producto, value: item.Producto, id: item.IdProducto, clave: item.Clave }
						}
						else {
							return { label: item.Producto, value: item.Clave, id: item.IdProducto }
						}
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdProducto = ui.item.id;
			var pProducto = ui.item.value;
			var pClave = ui.item.clave;
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", pIdProducto);
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");

			$("#txtClaveProducto").val(pClave);
			$("#txtDescripcionProducto").val(pProducto);
			var Producto = new Object();
			Producto.IdProducto = pIdProducto;
			obtenerProducto(Producto);

		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

function autocompletarServicioClave() {
	$('#txtClaveServicio').autocomplete({
		source: function (request, response) {
			var pServicio = new Object();
			pServicio.Servicio = $("#txtClaveServicio").val();
			pServicio.TipoBusqueda = "C";

			var oRequest = new Object();
			oRequest.pServicio = pServicio;
			$.ajax({
				type: 'POST',
				url: 'Cotizacion.aspx/BuscarServicio',
				data: JSON.stringify(oRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
					$("#txtDescripcionServicio").val("");
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						if (pServicio.TipoBusqueda == 'C') {
							return { label: item.Clave, value: item.Clave, id: item.IdServicio, descripcion: item.Servicio }
						}
						else {
							return { label: item.Clave, value: item.Clave, id: item.IdServicio }
						}
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdServicio = ui.item.id;
			var pServicio = ui.item.descripcion;
			var pClave = ui.item.value;
			var pIdTipoMonedaDestino = ($("#cmbTipoMoneda").val() != "" && $("#cmbTipoMoneda").val() != null) ? $("#cmbTipoMoneda").val() : 0;
			var pIdCotizacion = ($("#divFormaAgregarCotizacion").attr("idcotizacion") != "" && $("#divFormaAgregarCotizacion").attr("idcotizacion") != null) ? $("#divFormaAgregarCotizacion").attr("idcotizacion") : 0;

			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", pIdServicio);

			$("#txtDescripcionServicio").val(pServicio);
			$("#txtClaveServicio").val(pClave);

			var Servicio = new Object();
			Servicio.pIdServicio = pIdServicio;
			Servicio.pIdCotizacion = pIdCotizacion;
			Servicio.pIdTipoMonedaDestino = pIdTipoMonedaDestino;
			obtenerServicio(Servicio);

		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

function autocompletarServicioDescripcion() {
	$('#txtDescripcionServicio').autocomplete({
		source: function (request, response) {
			var pServicio = new Object();
			pServicio.Servicio = $("#txtDescripcionServicio").val();
			pServicio.TipoBusqueda = "P";

			var oRequest = new Object();
			oRequest.pServicio = pServicio;
			$.ajax({
				type: 'POST',
				url: 'Cotizacion.aspx/BuscarServicio',
				data: JSON.stringify(oRequest),
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
					$("#txtClaveServicio").val("");
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Table, function (item) {
						if (pServicio.TipoBusqueda == 'P') {
							return { label: item.Servicio, value: item.Servicio, id: item.IdServicio, clave: item.Clave }
						}
						else {
							return { label: item.Clave, value: item.Clave, id: item.IdServicio }
						}
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			var pIdServicio = ui.item.id;
			var pServicio = ui.item.value;
			var pClave = ui.item.clave;
			var pIdTipoMonedaDestino = ($("#cmbTipoMoneda").val() != "" && $("#cmbTipoMoneda").val() != null) ? $("#cmbTipoMoneda").val() : 0;
			var pIdCotizacion = ($("#divFormaAgregarCotizacion").attr("idcotizacion") != "" && $("#divFormaAgregarCotizacion").attr("idcotizacion") != null) ? $("#divFormaAgregarCotizacion").attr("idcotizacion") : 0;

			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
			$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", pIdServicio);

			$("#txtClaveServicio").val(pClave);
			$("#txtDescripcionServicio").val(pServicio);

			var Servicio = new Object();
			Servicio.pIdServicio = pIdServicio;
			Servicio.pIdCotizacion = pIdCotizacion;
			Servicio.pIdTipoMonedaDestino = pIdTipoMonedaDestino;
			obtenerServicio(Servicio);

		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

function ObtenerDatosCliente(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ObtenerDatosCliente",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				Modelo = respuesta.Modelo;
				var ContactoOrganizacion = new Object();
				var Agente = new Object();
				ContactoOrganizacion.ListaCombo = Modelo.ListaContactoOrganizacion;
				Agente.ListaCombo = Modelo.ListaAgente
				ObtenerListaContactoOrganizacion(ContactoOrganizacion);
				ObtenerListaAgente(Agente);
				$("#txtNota").val("LIBRE A BORDO: MEXICO D.F./ MTY TODA CANCELACIÓN,  DEVOLUCIÓN DE PRODUCTO EN CONDICIONES DE VENTA  O CHEQUE DEVUELTO GENERA UN CARGO DEL 20% MAS IVA CONDICIONES DE PAGO " + Modelo.CondicionPago + " ").addClass('uppercase');
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function AgregarCotizacion() {
	var pCotizacion = new Object();

	var idcotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion");
	pCotizacion.IdCotizacion = validaNumero(idcotizacion) ? idcotizacion : 0;

	var idcliente = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente");
	pCotizacion.IdCliente = validaNumero(idcliente) ? idcliente : 0;

	var idautorizaciontipocambio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionTipoCambio");
	pCotizacion.IdAutorizacionTipoCambio = validaNumero(idautorizaciontipocambio) ? idautorizaciontipocambio : 0;

	var idautorizacionIVA = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionIVA");
	pCotizacion.IdAutorizacionIVA = validaNumero(idautorizacionIVA) ? idautorizacionIVA : 0;

	var idsucursalejecuta = $("#cmbSucursales").val();
	pCotizacion.IdSucursalEjecutaServicio = validaNumero(idsucursalejecuta) ? idsucursalejecuta : 0;

	pCotizacion.Nota = $("#txtNota").val();
	pCotizacion.ValidoHasta = $("#txtValidoHasta").val();
	pCotizacion.IdContactoOrganizacion = $("#cmbContactoOrganizacion").val();
	pCotizacion.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
	pCotizacion.IdUsuarioAgente = $("#cmbUsuarioAgente").val();
	pCotizacion.IdCampana = $("#cmbCampana").val();

	var TipoCambioActual = QuitarFormatoNumero($("#TipoCambioActual").html());
	pCotizacion.TipoCambio = TipoCambioActual;
	pCotizacion.AutorizacionIVA = $("#IVAActual").html();
	pCotizacion.Proyecto = ""; //$("#txtProyecto").val();
	pCotizacion.IdNivelInteresCotizacion = $("#cmbNivelInteresCotizacion").val();
	pCotizacion.IdDivision = $("#cmbDivision").val();
	pCotizacion.Oportunidad = $("#txtOportunidad").val();
	pCotizacion.IdOportunidad = $("#cmbOportunidad").val();
	pCotizacion.IdEstatusCotizacion = 1;

	var validacion = ValidaCotizacion(pCotizacion);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	console.log(pCotizacion);

	var oRequest = new Object();
	oRequest.pCotizacion = pCotizacion;
	SetAgregarCotizacion(JSON.stringify(oRequest));
}

function SetAgregarCotizacion(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/AgregarCotizacion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
				MostrarMensajeError("Datos guardados");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogAgregarCotizacion").dialog("close");
		}
	});
}

function SetCambiarEstatus(pIdCotizacion, pBaja, pIdEstatusCotizacion) {
	var pRequest = "{'pIdCotizacion':" + pIdCotizacion + ", 'pBaja':" + pBaja + ", 'pIdEstatusCotizacion':" + pIdEstatusCotizacion + "}";
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/CambiarEstatus",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			Modelo = respuesta.Modelo;
			if (respuesta.Error == 0) {
				if (Modelo.Permisos.puedeEliminarCotizacion == 1) {
					$("#grdCotizacion").trigger("reloadGrid");
					ObtenerTotalesEstatusCotizacion();
				} else {
					MostrarMensajeError("No tiene permiso para eliminar cotizaciones");
					return false;
				}

			} else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			$('#grdCotizacion').one('click', '.div_grdCotizacion_AI', function (event) {
				var registro = $(this).parents("tr");
				var estatusBaja = $(registro).children("td[aria-describedby='grdCotizacion_AI']").children().attr("baja")
				var idCotizacion = $(registro).children("td[aria-describedby='grdCotizacion_IdCotizacion']").html();
				var idEstatusCotizacion = 1;

				var baja = "false";
				if (estatusBaja == "0" || estatusBaja == "False") {
					baja = "true";
					idEstatusCotizacion = 4;
				}

				SetCambiarEstatus(idCotizacion, baja, idEstatusCotizacion);
			});
		}
	});
}

function obtenerProducto(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/obtenerProducto",
		data: JSON.stringify(pRequest),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				Modelo = respuesta.Modelo;

				var pTipoCambio = new Object();
				pTipoCambio.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
				pTipoCambio.IdTipoMonedaDestino = Modelo.IdTipoMonedaProducto;

				var idcotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion");
				pTipoCambio.IdCotizacion = validaNumero(idcotizacion) ? idcotizacion : 0;

				var claveautorizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("claveAutorizacion");
				pTipoCambio.ClaveAutorizacion = validaNumero(claveautorizacion) ? claveautorizacion : 0;

				ObtenerTipoCambio(JSON.stringify(pTipoCambio));
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function obtenerServicio(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/obtenerServicio",
		data: JSON.stringify(pRequest),
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {

				Modelo = respuesta.Modelo;

				var pTipoCambio = new Object();
				pTipoCambio.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
				pTipoCambio.IdTipoMonedaDestino = Modelo.IdTipoMonedaServicio;

				if ($("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion") == "" || $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion") == null) { pTipoCambio.IdCotizacion = 0; }
				else { pTipoCambio.IdCotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion"); }

				if ($("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("ClaveAutorizacion") == "" || $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("claveAutorizacion") == null) { pTipoCambio.ClaveAutorizacion = 0; }
				else { pTipoCambio.ClaveAutorizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("claveAutorizacion"); }

				ObtenerTipoCambio(JSON.stringify(pTipoCambio), "Servicio");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function AgregarCotizacionDetalle() {
	var pCotizacion = new Object();

	var idproducto = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto");
	pCotizacion.IdProducto = validaNumero(idproducto) ? idproducto : 0;

	var idservicio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio");
	pCotizacion.IdServicio = validaNumero(idservicio) ? idservicio : 0;

	var cantidad = 0;
	var descuento = 0;
	var totalPartida = 0;
	var precio = 0;
	var DetalleIVAGravado = 0;
	var iddescuento = $("#cmbDescuento").val();
	var idtiempoentrega = $("#cmbTiempoEntrega").val();
	var descripcion = $("#txtDescripcion").val();

	if (pCotizacion.IdProducto != 0) {
		descripcion += idtiempoentrega == 0 ? "" : ' Tiempo de entrega ' + $("#cmbTiempoEntrega option:selected").text();
	}

	descripcion += iddescuento == 0 ? "" : ' ' + $("#cmbDescuento option:selected").text();




	cantidad = $("#txtCantidad").val();
	descuento = $("#txtValorDescuento").val();
	descuento = descuento.replace("%", "");
	descuento = descuento.split(",").join("");
	descuento = validaNumero(descuento) ? descuento : 0;


	pCotizacion.Descripcion = descripcion;
	pCotizacion.Cantidad = cantidad


	var SubTPartida = 0;
	var preciounitario = QuitaFormatoMoneda($("#txtPrecio").val());
	SubTPartida = parseFloat(cantidad) * parseFloat(preciounitario);


	if (descuento == 0) {
		totalPartida = parseFloat(SubTPartida);
	}
	else {
		totalPartida = (parseFloat(cantidad) * parseFloat(preciounitario) - ((parseFloat(cantidad) * parseFloat(preciounitario)) * parseFloat(descuento / 100)));
	}

	if (descuento == 0) {
		preciounitario = parseFloat(preciounitario);
	}
	else {
		preciounitario = parseFloat(preciounitario) - (parseFloat(preciounitario) * parseFloat(descuento / 100));
	}

	pCotizacion.PrecioUnitario = QuitaFormatoMoneda($("#txtPrecio").val());
	pCotizacion.Total = parseFloat(totalPartida);
	pCotizacion.Descuento = descuento;
	pCotizacion.OrdenDeCompraCantidad = cantidad;
	pCotizacion.RecepcionCantidad = cantidad;
	pCotizacion.RemisionCantidad = cantidad;
	pCotizacion.FacturacionCantidad = cantidad;

	var idcotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion");
	pCotizacion.IdCotizacion = validaNumero(idcotizacion) ? idcotizacion : 0;

	pCotizacion.IdTiempoEntrega = validaNumero(idtiempoentrega) ? idtiempoentrega : 0;

	var idcliente = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente");
	pCotizacion.IdCliente = validaNumero(idcliente) ? idcliente : 0;

	var idsucursalejecuta = $("#cmbSucursales").val();
	pCotizacion.IdSucursalEjecutaServicio = validaNumero(idsucursalejecuta) ? idsucursalejecuta : 0;


	/*Para la cotizacion encabezado*/

	pCotizacion.Nota = $("#txtNota").val();
	pCotizacion.ValidoHasta = $("#txtValidoHasta").val();
	pCotizacion.IdContactoOrganizacion = $("#cmbContactoOrganizacion").val();
	pCotizacion.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
	pCotizacion.IdTipoMonedaProducto = $("#divFormaAgregarCotizacion").attr("idTipoMonedaProducto");
	pCotizacion.IdTiempoEntrega = $("#cmbTiempoEntrega").val();

	pCotizacion.IdUsuarioAgente = $("#cmbUsuarioAgente").val();
	pCotizacion.IdCampana = $("#cmbCampana").val();
	pCotizacion.TipoCambio = QuitaFormatoMoneda($("#TipoCambioActual").html());
	pCotizacion.Proyecto = ""; //$("#txtProyecto").val();
	pCotizacion.Oportunidad = $("#txtOportunidad").val();
	pCotizacion.IdOportunidad = $("#cmbOportunidad").val();
	pCotizacion.IdNivelInteresCotizacion = $("#cmbNivelInteresCotizacion").val();
	pCotizacion.IdDivision = $("#cmbDivision").val();

	var idautorizacionIVA = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionIVA");
	pCotizacion.IdAutorizacionIVA = validaNumero(idautorizacionIVA) ? idautorizacionIVA : 0;

	var ivapantalla = $("#IVAActual").html();
	pCotizacion.AutorizacionIVA = ivapantalla;

	var idtipoivaProducto = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoIVAProducto");
	idtipoivaProducto = validaNumero(idtipoivaProducto) ? idtipoivaProducto : 0;

	var idtipoivaServicio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoIVAServicio");
	idtipoivaServicio = validaNumero(idtipoivaServicio) ? idtipoivaServicio : 0;

	if (idtipoivaProducto != 0) {
		var idtipoiva = idtipoivaProducto;
		var IVADetalle = ivapantalla;

	}
	else {
		var idtipoiva = idtipoivaServicio;
		var IVADetalle = ivapantalla;
	}

	if (idtipoiva == 1) {
		DetalleIVAGravado = ((totalPartida * ivapantalla) / 100);
		IVADetalle = ivapantalla;
	}
	else {
		DetalleIVAGravado = 0;
		IVADetalle = 0;
	}

	pCotizacion.IdTipoIVA = idtipoiva;
	pCotizacion.IVADetalle = IVADetalle;

	var subtotalActual = QuitaFormatoMoneda($('#txtSubtotalDetalle').text());
	var subtotalConDescuentoActual = QuitaFormatoMoneda($('#txtSubtotalDescuentoDetalle').text());
	var descCantAct = QuitaFormatoMoneda($('#txtDescuentoDetalle').text());
	var totalActual = QuitaFormatoMoneda($('#txtTotalDetalle').text());
	var ivaActual = QuitaFormatoMoneda($('#txtIVADetalle').text());

	pCotizacion.SubtotalCot = QuitaFormatoMoneda(parseFloat(SubTPartida) + parseFloat(subtotalConDescuentoActual));
	pCotizacion.DescuentoCantidad = QuitaFormatoMoneda((parseFloat(SubTPartida) * parseFloat(descuento / 100)) + parseFloat(descCantAct));
	pCotizacion.SubtotalConDescuento = ((parseFloat(subtotalActual) + parseFloat(SubTPartida)) - parseFloat(pCotizacion.DescuentoCantidad));
	pCotizacion.IVACot = QuitaFormatoMoneda(parseFloat(DetalleIVAGravado) + parseFloat(ivaActual));
	pCotizacion.TotalCot = QuitaFormatoMoneda((parseFloat(totalPartida) + parseFloat(DetalleIVAGravado)) + parseFloat(totalActual));

	precio = parseFloat(pCotizacion.TotalCot);
	pCotizacion.CantidadTotalLetra = covertirNumLetras(precio.toString(), $("#cmbTipoMoneda option:selected").text());

	var idautorizaciontipocambio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionTipoCambio");
	pCotizacion.IdAutorizacionTipoCambio = validaNumero(idautorizaciontipocambio) ? idautorizaciontipocambio : 0;

	var validacion = ValidaCotizacionDetalle(pCotizacion);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	var oRequest = new Object();
	oRequest.pCotizacion = pCotizacion;
	console.log(JSON.stringify(oRequest));
	SetAgregarCotizacionDetalle(JSON.stringify(oRequest));
}

function SetAgregarCotizacionDetalle(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/AgregarCotizacionDetalle",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion", respuesta.Modelo.IdCotizacion);
				$("#txtFolio").val(respuesta.Modelo.Folio);
				$("#txtSubtotalDetalle").text(formato.moneda(respuesta.Modelo.Subtotal + respuesta.Modelo.DescuentoCantidad, "$"));
				//$("#txtDescuentoDetalle").text(formato.moneda(respuesta.Modelo.DescuentoCantidad, "$"));
				//$("#txtSubtotalDescuentoDetalle").text(formato.moneda(respuesta.Modelo.SubtotalConDescuento, "$"));
				$("#txtIVADetalle").text(formato.moneda((parseFloat(respuesta.Modelo.IVA)), "$"));
				$("#txtTotalDetalle").text(formato.moneda((parseFloat(respuesta.Modelo.Total)), "$"));
				$("#txtTotalLetraDetalle").val(respuesta.Modelo.CantidadTotalLetra);
				$("#spanTextoIVA").text('IVA tasa ' + respuesta.Modelo.PorcentajeIVA + '%');
				$("#grdCotizacionDetalle").trigger("reloadGrid");
				$("#grdCotizacionDetalleConsultar").trigger("reloadGrid");
				$("#grdCotizacionDetalleEditar").trigger("reloadGrid");
				$("#grdCotizacion").trigger("reloadGrid");

				ObtenerTotalesEstatusCotizacion();
				DeshabilitaCamposEncabezado();
				OrdenaPartidas();
				LimpiarDatosBusqueda();
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function FiltroCotizacionDetalle() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdCotizacionDetalle').getGridParam('rowNum');
	request.pPaginaActual = $('#grdCotizacionDetalle').getGridParam('page');
	request.pColumnaOrden = $('#grdCotizacionDetalle').getGridParam('sortname');
	request.pTipoOrden = $('#grdCotizacionDetalle').getGridParam('sortorder');
	request.pIdCotizacion = 0;
	if ($("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion") != null && $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion") != "") {
		request.pIdCotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion");
	}
	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cotizacion.aspx/ObtenerCotizacionDetalle',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') {
				$('#grdCotizacionDetalle')[0].addJSONData(JSON.parse(jsondata.responseText).d);
			}
			else { alert(JSON.parse(jsondata.responseText).Message); }
		}
	});
}

function FiltroCotizacionDetalleConsultar() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdCotizacionDetalleConsultar').getGridParam('rowNum');
	request.pPaginaActual = $('#grdCotizacionDetalleConsultar').getGridParam('page');
	request.pColumnaOrden = $('#grdCotizacionDetalleConsultar').getGridParam('sortname');
	request.pTipoOrden = $('#grdCotizacionDetalleConsultar').getGridParam('sortorder');
	request.pIdCotizacion = 0;
	if ($("#divFormaAgregarCotizacion, #divFormaConsultarCotizacion").attr("idCotizacion") != null && $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion") != "") {
		request.pIdCotizacion = $("#divFormaAgregarCotizacion, #divFormaConsultarCotizacion").attr("idCotizacion");
	}
	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cotizacion.aspx/ObtenerCotizacionDetalleConsultar',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') {
				$('#grdCotizacionDetalleConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d);
			}
			else {
				alert(JSON.parse(jsondata.responseText).Message);
			}

			var descuento = 0;
			var subtotal = 0;

			$("tr.jqgrow", "#grdCotizacionDetalleConsultar").each(function (index, element) {
				var Cantidad = QuitarFormatoNumero($("td:eq(4)", element).text());
				var PrecioUnitario = QuitarFormatoNumero($("td:eq(5)", element).text());
				var Descuento = $("td:eq(9)", element).text().replace("%", "");
				var SubtotalPartida = QuitarFormatoNumero($("td:eq(6)", element).text());
				descuento = parseInt(SubtotalPartida) * (Descuento / 100);
				subtotal += parseInt(SubtotalPartida);
			});

			//Subtotal
			$("#txtSubtotalDetalle").text(formato.moneda(subtotal, '$'));
			//Descuento
			//$("#txtDescuentoDetalle").text(formato.moneda(descuento, '$'));
			//SubtotalDescuento
			//$("#txtSubtotalDescuentoDetalle").text(formato.moneda(subtotal - descuento, '$'));
		}
	});
}

function SetEliminarCotizacionDetalle(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/EliminarCotizacionDetalle",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#txtSubtotalDetalle").text(formato.moneda(respuesta.Modelo.Subtotal, "$"));
				$("#txtDescuentoDetalle").text(formato.moneda(respuesta.Modelo.Descuento, "$"));
				$("#txtSubtotalDescuentoDetalle").text(formato.moneda(respuesta.Modelo.SubtotalDescuento, "$"));
				$("#txtIVADetalle").text(formato.moneda((parseFloat(respuesta.Modelo.Iva)), "$"));
				$("#txtTotalDetalle").text(formato.moneda((parseFloat(respuesta.Modelo.Total)), "$"));

				var Precio = parseFloat(respuesta.Modelo.Total);
				var textoMoneda = $("#cmbTipoMoneda option:selected").text();
				$("#txtTotalLetraDetalle").val(covertirNumLetras(Precio.toString(), textoMoneda));

				$("#grdCotizacionDetalleEditar").trigger("reloadGrid");
				$("#grdCotizacionDetalleConsultar").trigger("reloadGrid");
				$("#grdCotizacionDetalle").trigger("reloadGrid");
				$("#grdCotizacion").trigger("reloadGrid");

				ObtenerTotalesEstatusCotizacion();
				OrdenaPartidas()
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function eliminarConceptoEditar() {
	$("#grdCotizacionDetalle").on("click", ".imgEliminarConceptoEditar", function () {
		var registro = $(this).parents("tr");
		var pCotizacionDetalle = new Object();
		pCotizacionDetalle.pIdCotizacionDetalle = parseInt($(registro).children("td[aria-describedby='grdCotizacionDetalle_IdCotizacionDetalle']").html());
		pCotizacionDetalle.pIva = $("#IVAActual").html();
		var oRequest = new Object();
		oRequest.pCotizacionDetalle = pCotizacionDetalle;
		SetEliminarCotizacionDetalle(JSON.stringify(oRequest));
	});
}

function obtieneDescuento() {
	$('#divFormaAgregarCotizacion, #divFormaEditarCotizacion').on('change', '#cmbDescuento', function (event) {
		var request = new Object();
		request.pIdDescuento = $(this).val();

		if ($("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto") == "" || $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto") == null) { request.IdProducto = 0; }
		else { request.pIdProducto = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto"); }

		if (request.pIdDescuento != 0) {
			ObtenerValorDescuento(JSON.stringify(request));
		}
		else {
			$("#txtValorDescuento").val('0%');
		}
	});
}

function ObtenerValorDescuento(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ObtenerValorDescuento",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				var p = respuesta.Modelo.Descuento + "%";
				$("#txtValorDescuento").val(p);
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function ObtenerFormaEditarCotizacion(IdCotizacion, IdEstatusCotizacion) {
	$("#dialogEditarCotizacion").obtenerVista({
		nombreTemplate: "tmplEditarCotizacion.html",
		url: "Cotizacion.aspx/ObtenerFormaEditarCotizacion",
		parametros: IdCotizacion,
		despuesDeCompilar: function (pRespuesta) {
			Modelo = pRespuesta.modelo;
			if (Modelo.Permisos.puedeEditarCotizacion == 1) {

				Inicializar_grdCotizacionDetalleEditar();
				$('#txtSubtotalDescuentoDetalle').text(formato.moneda(Modelo.Subtotal, '$'));
				$('#txtIVADetalle').text(formato.moneda(Modelo.IVA, '$'));
				$('#txtTotalDetalle').text(formato.moneda(Modelo.Total, '$'));

				autocompletarCliente();
				autocompletarProductoClave();
				autocompletarProductoDescripcion();
				autocompletarServicioClave();
				autocompletarServicioDescripcion();
				$("#txtValidoHasta").datepicker();
				$("#dialogEditarCotizacion").dialog("open");
				DeshabilitaCamposEncabezado();

				$("#grdCotizacionDetalleEditar").jqGrid('sortableRows', {
					update: function (e, ui) {
						OrdenaPartidas();
					}
				});

				if (Modelo.IdEstatusCotizacion == 1) {//es borrador
					$("#dialogEditarCotizacion").dialog("option", "buttons", {
						"Editar borrador": function () {
							EditarCotizacion();
						},
						"Generar cotización": function () {
							$("#dialogGenerarCotizacion").dialog("open");
						},
						"Salir": function () {
							$(this).dialog("close")
						}
					});
					$("#dialogConsultarCotizacion").dialog("option", "height", "auto");
				}
				else {// ya no se edita
					$("#dialogEditarCotizacion").dialog("option", "buttons", {
						"Salir": function () {
							$(this).dialog("close")
						}
					});
					$("#dialogConsultarCotizacion").dialog("option", "height", "auto");

				}

				var Precio = parseFloat(Modelo.Total);
				var textoMoneda = $("#cmbTipoMoneda option:selected").text();
				$("#txtTotalLetraDetalle").val(covertirNumLetras(Precio.toString(), textoMoneda));
                
			} else {
				MostrarMensajeError("No puedes editar cotizaciones");
				return false;
			}
			ocultaAlAbrir()

		}
	});
}

var cantidadT = 1000;
function cuenta() {
    $("#cantidadDescripcion").text(cantidadT - $("#txtDescripcion").val().length);
}

function BloquearCamposEditar() {
	$("#cmbTipoMoneda").attr('disabled', 'disabled');
	$("#btnBajarPartida").css('display', 'none');
	$("#grdCotizacionDetalleEditar .imgEliminarConceptoEditar").css('display', 'none');
}

function OrdenaPartidas() {
	var lista = $("#grdCotizacionDetalle, #grdCotizacionDetalleEditar").jqGrid('getDataIDs');
	var pCotizacionDetalle = new Object();
	pCotizacionDetalle.Partidas = new Array();
	for (var i = 0; i < lista.length; i++) {
		var Orden = i + 1;
		var rowData = $('#grdCotizacionDetalle, #grdCotizacionDetalleEditar').jqGrid('getRowData', lista[i]);
		var pPartida = new Object();
		pPartida.IdCotizacionDetalle = rowData.IdCotizacionDetalle;
		pPartida.Ordenacion = Orden;
		pPartida.Clave = Orden;
		pCotizacionDetalle.Partidas.push(pPartida);
	}
	var oRequest = new Object();
	oRequest.pCotizacionDetalle = pCotizacionDetalle;
	CotizacionDetalleReordenar(JSON.stringify(oRequest));
}

function CotizacionDetalleReordenar(pRequest) {
	$.ajax({
		url: 'Cotizacion.aspx/ActualizarCotizacionDetalleOrdenacion',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				//alert("Todo bien");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		}
	});
}

function FiltroCotizacionDetalleEditar() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdCotizacionDetalleEditar').getGridParam('rowNum');
	request.pPaginaActual = $('#grdCotizacionDetalleEditar').getGridParam('page');
	request.pColumnaOrden = $('#grdCotizacionDetalleEditar').getGridParam('sortname');
	request.pTipoOrden = $('#grdCotizacionDetalleEditar').getGridParam('sortorder');
	request.pIdCotizacion = 0;
	if ($("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion") != null && $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion") != "") {
		request.pIdCotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdCotizacion");
	}
	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cotizacion.aspx/ObtenerCotizacionDetalleEditar',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success')
			{ $('#grdCotizacionDetalleEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
			else
			{ alert(JSON.parse(jsondata.responseText).Message); }

			sumadetallePU = $("#grdCotizacionDetalleEditar").jqGrid('getCol', 'Subtotal', false, 'sum');
			subtotal = QuitarFormatoNumero($("#txtSubtotalDescuentoDetalle").text());
			$("#txtSubtotalDetalle").text(formato.moneda(sumadetallePU, '$'));
			$("#txtDescuentoDetalle").text(formato.moneda(sumadetallePU - subtotal, '$'));


		}
	});
}

function FiltroCotizacion() {
	var request = new Object();
	request.pTamanoPaginacion = $('#grdCotizacion').getGridParam('rowNum');
	request.pPaginaActual = $('#grdCotizacion').getGridParam('page');
	request.pColumnaOrden = $('#grdCotizacion').getGridParam('sortname');
	request.pTipoOrden = $('#grdCotizacion').getGridParam('sortorder');
	request.pAI = 0;
	request.pRazonSocial = "";
	request.pFolio = "";
	request.pIdOportunidad = "";
	request.pFechaInicial = "";
	request.pFechaFinal = "";
	request.pPorFecha = 0;
	request.pIdEstatusCotizacion = -1;

	var idestatuscotizacion = $("#tblCotizacionTotalesEstatus").attr("idEstatusCotizacionSeleccionado");
	request.pIdEstatusCotizacion = validaNumero(idestatuscotizacion) ? idestatuscotizacion : -1;

	if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

	if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }

	if ($('#gs_IdOportunidad').val() != null) { request.pIdOportunidad = $("#gs_IdOportunidad").val(); }

	if ($('#gs_AI').val() != null) { request.pAI = $("#gs_AI").val(); }

	if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

		if ($("#chkPorFecha").is(':checked')) {
			request.pPorFecha = 1;
		}
		else {
			request.pPorFecha = 0;
		}

		request.pFechaInicial = $("#txtFechaInicial").val();
		request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
	}

	if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
		request.pFechaFinal = $("#txtFechaFinal").val();
		request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
	}

	if ($("#cmbEstatusCotizacion").val() != null) {
		request.pIdEstatusCotizacion = parseInt($("#cmbEstatusCotizacion").val());
	}

	var pRequest = JSON.stringify(request);
	$.ajax({
		url: 'Cotizacion.aspx/ObtenerCotizacion',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		complete: function (jsondata, stat) {
			if (stat == 'success') {
				ObtenerTotalesEstatusCotizacion();
				$('#grdCotizacion')[0].addJSONData(JSON.parse(jsondata.responseText).d);
				var btnDeclinar = '<img src="../images/false.png" height="20" style="cursor:pointer;" onclick="DeclinarCotizacion(this);" />';
				$('td[aria-describedby="grdCotizacion_Declinar"]', '#grdCotizacion').html(btnDeclinar);
				$('td[aria-describedby="grdCotizacion_Utilidad"]', '#grdCotizacion').each(function (index,elemento) {
					var utilidad = parseFloat($(elemento).text().replace("$", "").replace(",", ""));
					if (utilidad < 0)
						$(elemento).css({ "color": "#F00" });
				});
			}
			else { alert(JSON.parse(jsondata.responseText).Message); }
		}
	});
}

function ObtenerTotalesEstatusCotizacion() {

	var request = new Object();
	request.pFechaInicial = "";
	request.pFechaFinal = "";
	request.pPorFecha = 0;
	request.pFolio = 0;
	request.pRazonSocial = "";
	request.pAI = 0;

	var idestatuscotizacion = $("#tblCotizacionTotalesEstatus").attr("idEstatusCotizacionSeleccionado");
	request.pIdEstatusCotizacion = validaNumero(idestatuscotizacion) ? idestatuscotizacion : -1;

	if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

		if ($("#chkPorFecha").is(':checked')) {
			request.pPorFecha = 1;
		}
		else {
			request.pPorFecha = 0;
		}

		request.pFechaInicial = $("#txtFechaInicial").val();
		request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
	}
	if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
		request.pFechaFinal = $("#txtFechaFinal").val();
		request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
	}

	if ($('#gs_Folio').val() != null && $('#gs_Folio').val() != "") {
		request.pFolio = $("#gs_Folio").val();
	}

	if ($('#gs_RazonSocial').val() != null && $('#gs_RazonSocial').val() != "") {
		request.pRazonSocial = $("#gs_RazonSocial").val();
	}

	if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") {
		request.pAI = $("#gs_AI").val();
	}

	var pRequest = JSON.stringify(request);

	$.ajax({
		url: 'Cotizacion.aspx/ObtenerTotalesEstatusCotizacion',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$.each(respuesta.Modelo.TotalesEstatusCotizacion, function (index, oEstatusCotizacion) {
					$('#span-E' + oEstatusCotizacion.IdEstatusCotizacion).text(oEstatusCotizacion.Contador);
				});
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		}
	});
}

function ObtenerFormaFiltrosCotizacion() {
	$("#divFiltrosCotizacion").obtenerVista({
		nombreTemplate: "tmplFiltrosCotizacion.html",
		url: "Cotizacion.aspx/ObtenerFormaFiltroCotizacion",
		despuesDeCompilar: function (pRespuesta) {

			if ($("#txtFechaInicial").length) {
				$("#txtFechaInicial").datepicker({
					onSelect: function () {
						FiltroCotizacion();
					}
				});
			}

			if ($("#txtFechaFinal").length) {
				$("#txtFechaFinal").datepicker({
					onSelect: function () {
						FiltroCotizacion();
					}
				});
			}

			$("#cmbEstatusCotizacion").change(function () {
				FiltroCotizacion();
			});

			//            $('#divFiltrosCotizacion').on('click', '#chkPorFecha', function(event) {
			//                FiltroCotizacion();
			//            });

		}
	});
}

function GenerarCotizacion() {
	var pCotizacion = new Object();

	var idcotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion");
	pCotizacion.IdCotizacion = validaNumero(idcotizacion) ? idcotizacion : 0;

	var idcliente = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente");
	pCotizacion.IdCliente = validaNumero(idcliente) ? idcliente : 0;

	var idautorizacionIVA = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionIVA");
	pCotizacion.IdAutorizacionIVA = validaNumero(idautorizacionIVA) ? idautorizacionIVA : 0

	pCotizacion.Nota = $("#txtNota").val();
	pCotizacion.ValidoHasta = $("#txtValidoHasta").val();
	pCotizacion.IdContactoOrganizacion = $("#cmbContactoOrganizacion").val();
	pCotizacion.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
	pCotizacion.IdUsuarioAgente = $("#cmbUsuarioAgente").val();
	pCotizacion.IdOportunidad = $("#cmbOportunidad").val();
	pCotizacion.IdEstatusCotizacion = parseInt(2);

	var validacion = ValidaCotizacion(pCotizacion);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	var oRequest = new Object();
	oRequest.pCotizacion = pCotizacion;
	SetGenerarCotizacion(JSON.stringify(oRequest));
}

function SetGenerarCotizacion(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/GenerarCotizacion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			Modelo = respuesta.Modelo
			if (respuesta.Error == 0) {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
				MostrarMensajeError("Se ha generado la cotización " + Modelo.Folio);
				$("#dialogAgregarCotizacion, #dialogEditarCotizacion").dialog("close");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogEditarCotizacion").dialog("close");
		}
	});
}

function CalcularTotales(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/CalculaSumaTotal",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#txtSubtotalDetalle").text(formato.moneda(respuesta.Modelo.Subtotal, "$"));
				$("#txtDescuentoDetalle").text(formato.moneda(respuesta.Modelo.Descuento, "$"));
				$("#txtSubtotalDescuentoDetalle").text(formato.moneda(respuesta.Modelo.SubtotalDescuento, "$"));
				$("#txtIVADetalle").text(formato.moneda((parseFloat(respuesta.Modelo.Iva)), "$"));
				$("#txtTotalDetalle").text(formato.moneda((parseFloat(respuesta.Modelo.Total)), "$"));
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function SetGenerarPedido(pIdCotizacion) {
	var pRequest = "{'pIdCotizacion':" + pIdCotizacion + "}";
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/GenerarPedido",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
			} else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogGenerarPedido").dialog("close");
		}
	});
}

function ObtenerTipoCambio(pRequest, pTipoBuscador) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ObtenerTipoCambio",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {

				if (pTipoBuscador == "Servicio") {
					var tipocambio = QuitarFormatoNumero($("#TipoCambioActual").html());

					$("#txtPrecio").val(parseFloat(Modelo.Precio) / parseFloat(tipocambio));
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdTipoMonedaServicio", Modelo.IdTipoMonedaServicio);
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdTipoMonedaProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoIVAProducto", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoIVAServicio", Modelo.IdTipoIVA);
					$("#txtDescripcion").val(Modelo.Servicio);
					$("#txtUnidadCompraVenta").val(Modelo.UnidadCompraVenta);
					$("#txtValorMedida").val("No aplica");
					$("#txtValorDescuento").val("0%");
					$("#spanPrecioOrigen").text(formato.moneda(parseFloat(Modelo.Precio), Modelo.SimboloMonedaServicio));
					$("#spanMonedaOrigen").text(Modelo.TipoMonedaServicio);

					var request = new Object();
					request.ListaCombo = Modelo.ListaDescuento;
					ObtenerListaDescuento(request);
					obtieneDescuento();
				}
				else {
					$("#txtPrecio").val(parseFloat(Modelo.Precio));
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdTipoMonedaProducto", Modelo.IdTipoMonedaProducto);
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("IdTipoMonedaServicio", "0");
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoIVAProducto", Modelo.IdTipoIVA);
					$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoIVAServicio", "0");
					$("#txtValorMedida").val(Modelo.ValorMedida);
					$("#txtUnidadCompraVenta").val(Modelo.UnidadCompraVenta);
					$("#txtExistencia").val(Modelo.Existencia);
					$("#txtDescripcion").val(Modelo.Descripcion);
					$("#txtValorDescuento").val("0%");
					$("#spanPrecioOrigen").text(formato.moneda(parseFloat(Modelo.Precio), Modelo.SimboloMonedaProducto));
					$("#spanMonedaOrigen").text(Modelo.TipoMonedaProducto);

					var request = new Object();
					request.ListaCombo = Modelo.ListaDescuento;
					ObtenerListaDescuento(request);
					obtieneDescuento();
				}
				$("#TipoCambioActual").html(respuesta.Modelo.TipoCambioActual);
				var tipocambio = parseFloat(respuesta.Modelo.TipoCambioActual);
				var valor = parseFloat($("#txtPrecio").val()) / parseFloat(tipocambio);
				$("#txtPrecio").val(valor.toFixed(2));
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function CambiarPrecio() {
	var Precio = $("#txtPrecio").val().replace(/,/gi,'');
	var IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();

	var IdTipoMonedaProducto = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto");
	var IdTipoMonedaServicio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio");
	var IdTipoMonedaDestino = IdTipoMonedaServicio == "0" ? IdTipoMonedaProducto : IdTipoMonedaServicio;

	var IdCotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion");

	if (IdTipoMonedaOrigen != '0') {
		var pDato = new Object();
		pDato.IdServicio = ($("#divFormaEditarCotizacion").attr("idServicio") == "") ? 0 : parseInt($("#divFormaEditarCotizacion").attr("idServicio"));
		pDato.IdProducto = ($("#divFormaEditarCotizacion").attr("idProducto") == "") ? 0 : parseInt($("#divFormaEditarCotizacion").attr("idProducto"));
		pDato.IdTipoMonedaOrigen = IdTipoMonedaOrigen;
		pDato.IdTipoMonedaDestino = $("#cmbTipoMonedaOrigen").val();
		pDato.Precio = Precio == '' ? 0 : Precio;

		SetPrecioPorMoneda(JSON.stringify(pDato));
	}

}

function SetPrecioPorMoneda(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ObtenerPrecio",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			Modelo = respuesta.Modelo;
			if (respuesta.Error == 0) {
				$("#txtPrecio").val(formato.moneda(Modelo.MonedaPrecio, '', 4));
				$("#TipoCambioActual").html(formato.moneda(Modelo.TipoCambioActual, "$", 4));
				var request = new Object();
				request.ListaCombo = Modelo.ListaTipoMoneda;
				ObtenerListaTipoMoneda(request);
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
		}
	});
}

function ValidaExisteTipoCambio() {
	$.ajax({
		url: 'Cotizacion.aspx/ValidaExisteTipoCambio',
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {

			}
			else {

			}
		}
	});
}

function EditarCotizacion() {
	var pCotizacion = new Object();

	var idcotizacion = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCotizacion");
	pCotizacion.IdCotizacion = validaNumero(idcotizacion) ? idcotizacion : 0;

	var idcliente = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente");
	pCotizacion.IdCliente = validaNumero(idcliente) ? idcliente : 0;

	var idautorizaciontipocambio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionTipoCambio");
	pCotizacion.IdAutorizacionTipoCambio = validaNumero(idautorizaciontipocambio) ? idautorizaciontipocambio : 0;

	var idautorizacionIVA = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idAutorizacionIVA");
	pCotizacion.IdAutorizacionIVA = validaNumero(idautorizacionIVA) ? idautorizacionIVA : 0;

	var idsucursalejecuta = $("#cmbSucursales").val();
	pCotizacion.IdSucursalEjecutaServicio = validaNumero(idsucursalejecuta) ? idsucursalejecuta : 0;

	pCotizacion.Nota = $("#txtNota").val();
	pCotizacion.ValidoHasta = $("#txtValidoHasta").val();
	pCotizacion.IdContactoOrganizacion = $("#cmbContactoOrganizacion").val();
	pCotizacion.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
	pCotizacion.IdUsuarioAgente = $("#cmbUsuarioAgente").val();
	pCotizacion.IdCampana = $("#cmbCampana").val();
	pCotizacion.IdEstatusCotizacion = parseInt($("#divFormaEditarCotizacion").attr("IdEstatusCotizacion"));


	var tipocambio = QuitarFormatoNumero($("#TipoCambioActual").html());
	pCotizacion.TipoCambio = tipocambio;
	pCotizacion.AutorizacionIVA = $("#IVAActual").html();
	pCotizacion.Proyecto = ""; //$("#txtProyecto").val();
	pCotizacion.IdNivelInteresCotizacion = $("#cmbNivelInteresCotizacion").val();
	pCotizacion.Oportunidad = $("#txtOportunidad").val();
	pCotizacion.IdOportunidad = $("#cmbOportunidad").val();
	pCotizacion.IdDivision = $("#cmbDivision").val();

	var validacion = ValidaCotizacion(pCotizacion);
	if (validacion != "")
	{ MostrarMensajeError(validacion); return false; }

	var oRequest = new Object();
	oRequest.pCotizacion = pCotizacion;
	SetEditarCotizacion(JSON.stringify(oRequest));

	//    view_dialog();

	ObtenerFormaEditarCotizacion(JSON.stringify(pCotizacion));

}

function SetEditarCotizacion(pRequest) {
	MostrarBloqueo();
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/AgregarCotizacion",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
				MostrarMensajeError("Datos editados");
			}
			else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogAgregarCotizacion").dialog("close");
		}
	});
}

function Imprimir(IdCotizacion) {
	MostrarBloqueo();

	var Cotizacion = new Object();
	Cotizacion.IdCotizacion = IdCotizacion;

	var Request = JSON.stringify(Cotizacion);

	var formato = $("<div></div>");

	$(formato).obtenerVista({
		url: "Cotizacion.aspx/ImpirmirCotizacion",
		parametros: Request,
		nombreTemplate: "tmplImprimirCotizacion.html",
		despuesDeCompilar: function (Respuesta) {
			var impresion = window.open("", "_blank");
			impresion.document.write($(formato).html());
			impresion.print();
			impresion.close();
		}
	});

}

function ActivarCotizacionVencida(pRequest) {
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/ActivarCotizacionVencida",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
			} else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogActivarCotizacionVencida").dialog("close");
		}
	});
}

function SetRegresarACotizacion(pIdCotizacion) {
	var pRequest = "{'pIdCotizacion':" + pIdCotizacion + "}";
	$.ajax({
		type: "POST",
		url: "Cotizacion.aspx/RegresarACotizado",
		data: pRequest,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (pRespuesta) {
			respuesta = jQuery.parseJSON(pRespuesta.d);
			if (respuesta.Error == 0) {
				$("#grdCotizacion").trigger("reloadGrid");
				ObtenerTotalesEstatusCotizacion();
			} else {
				MostrarMensajeError(respuesta.Descripcion);
				return false;
			}
		},
		complete: function () {
			OcultarBloqueo();
			$("#dialogRegresarACotizacion").dialog("close");
		}
	});
}

function MuestraObjetos(opcion) {
	if (opcion == 1) {
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
		$("#txtClaveProducto").css("display", "block");
		$("#txtDescripcionProducto").css("display", "block");
		$("#txtClaveServicio").css("display", "none");
		$("#txtDescripcionServicio").css("display", "none");
		$("#txtClaveServicio").val("");
		$("#txtDescripcionServicio").val("");
		$("#txtMonedaServicio").css("display", "none");
		$("#txtMonedaProducto").css("display", "block");
		$(".EsServicio").css("visibility", "hidden");
		$(".NoEsServicio").css("visibility", "visible");

	}
	else {
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
		$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
		$("#txtClaveProducto").css("display", "none");
		$("#txtDescripcionProducto").css("display", "none");
		$("#txtClaveServicio").css("display", "block");
		$("#txtDescripcionServicio").css("display", "block");
		$("#txtClaveProducto").val("");
		$("#txtDescripcionProducto").val("");
		$("#txtMonedaServicio").css("display", "block");
		$("#txtMonedaProducto").css("display", "none");
		$(".EsServicio").css("visibility", "visible");
		$(".NoEsServicio").css("visibility", "hidden");
	}
}

function ocultaAlAbrir() {
	$(".EsServicio").css("visibility", "hidden");
	$(".NoEsServicio").css("visibility", "visible");
}

//----------Limpiar----------//
//--------------------------//
function DeshabilitaCamposEncabezado() {
	//$("#txtRazonSocial").attr("disabled", "true");
	//$("#cmbOportunidad").attr("disabled", "true");
	//$("#cmbNivelInteresCotizacion").attr("disabled", "true");
	$("#cmbUsuarioAgente").attr("disabled", "true");
	//$("#cmbTipoMoneda").attr("disabled", "true");
	$("#txtValidoHasta").attr("disabled", "true");
	$("#cmbUsuarioSolicitante").attr("disabled", "true");
	//$("#cmbContactoOrganizacion").attr("disabled", "true");
	//$("#txtNota").attr("disabled", "true");
	//$("#cmbCampana").attr("disabled", "true");
	//$('#cmbDivision').prop("disabled", false);
	$("#divImprimir").show();
}

function LimpiarDatosBusqueda() {
	$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idProducto", "0");
	$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idServicio", "0");
	$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaProducto", "0");
	$("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idTipoMonedaServicio", "0");
	$("#txtClaveProducto").val("");
	$("#txtClaveServicio").val("");
	$("#txtDescripcionProducto").val("");
	$("#txtDescripcionServicio").val("");
	$("#txtPrecio").val("");
	$("#cmbDescuento").empty().append('<option value="0">Eligir una opción...</option>');
	$("#txtValorDescuento").val("0%");
	$("#txtCantidad").val("");
	$("#txtValorMedida").val("");
	$("#txtDescripcion").val("");
	$("#txtExistencia").val("");
	$("#txtTipoMoneda").val("");
	$("#txtUnidadCompraVenta").val("");
	$("#spanPrecioOrigen").html('$0.00');
	$("#spanMonedaOrigen").html('');
}

//----------Validaciones----------//
//--------------------------//
function ValidaCotizacion(pCotizacion) {
	var errores = "";
	var detalle = $("#grdCotizacionDetalle, #grdCotizacionDetalleEditar").jqGrid('getGridParam', 'records');

	if (pCotizacion.IdCliente == 0)
	{ errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

	if (pCotizacion.IdCampana == 0)
	{ errores = errores + "<span>*</span> No hay campaña asociada, favor de seleccionarla.<br />"; }

	if (pCotizacion.IdDivision == 0)
	{ errores = errores + "<span>*</span> No hay división asociada, favor de seleccionarla.<br />"; }

	if (pCotizacion.IdUsuarioAgente == "0")
	{ errores = errores + "<span>*</span> El agente esta vacío, favor de capturarlo.<br />"; }

	if (pCotizacion.IdTipoMonedaOrigen == "0")
	{ errores = errores + "<span>*</span> El tipo de cambio esta vacío, favor de capturarlo.<br />"; }

	//Revisar agregar permiso
	//if (pCotizacion.IdOportunidad == "0")
	//{ errores = errores + "<span>*</span>No se selecciono ninguna oportunidad, favor de seleccionar una.<br />"; }

	if (detalle == 0)
	{ errores = errores + "<span>*</span> No existen partidas, favor de capturarlas.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ValidaCotizacionDetalle(pCotizacion) {
	var errores = "";

	if (pCotizacion.IdCliente == 0)
	{ errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

	if (pCotizacion.IdCampana == 0)
	{ errores = errores + "<span>*</span> No hay campaña asociada, favor de seleccionarla.<br />"; }

	if (pCotizacion.IdDivision == 0)
	{ errores = errores + "<span>*</span> No hay división asociada, favor de seleccionarla.<br />"; }

	if (pCotizacion.Cantidad == "")
	{ errores = errores + "<span>*</span> La cantidad esta vacía, favor de capturarla.<br />"; }

	if (pCotizacion.IdProducto == 0 && pCotizacion.IdServicio == 0)
	{ errores = errores + "<span>*</span> Favor de elegir un producto o un servicio.<br />"; }

	if (pCotizacion.IdProducto != 0) {
		if (pCotizacion.IdTiempoEntrega == "0")
		{ errores = errores + "<span>*</span> El tiempo entrega esta vacío, favor de capturarlo.<br />"; }
	}

	if (pCotizacion.IdTipoMonedaOrigen == "0")
	{ errores = errores + "<span>*</span> El tipo de cambio esta vacío, favor de capturarlo.<br />"; }

	if (pCotizacion.ValidoHasta == "")
	{ errores = errores + "<span>*</span> El campo fecha válido hasta esta vacío, favor de capturarlo.<br />"; }

	if (pCotizacion.IdUsuarioAgente == "0")
	{ errores = errores + "<span>*</span> El agente esta vacío, favor de capturarlo.<br />"; }

	//if (pCotizacion.PrecioUnitario == "")
	//{ errores = errores + "<span>*</span> El precio esta vacío, favor de capturarlo.<br />"; } 

	if (pCotizacion.Descripcion == "")
	{ errores = errores + "<span>*</span> La descripcion esta vacía, favor de capturarla.<br />"; }


	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function validarExisteMonedaDestino() {
	var errores = "";

	var IdProducto = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion ").attr("idProducto");
	var IdServicio = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion ").attr("idServicio")

	if (IdProducto == 0 && IdServicio == 0)
	{ errores = errores + "<span>*</span> Seleccione un producto o servicio.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function validaNuevaVigencia(pCotizacion) {
	var errores = "";
	var fechaActual = FechaActual();

	if (pCotizacion.IdCotizacion == 0)
	{ errores = errores + "<span>*</span> No hay numero de cotización por asociar.<br />"; }

	if (pCotizacion.ValidoHasta == '')
	{ errores = errores + "<span>*</span> No hay fecha de vigencia, favor de seleccionarla.<br />"; }

	if (Date.parse($("#txtNuevaVigencia").val()) < Date.parse(fechaActual))
	{ errores = errores + "<span>*</span> La fecha seleccionada  debe ser mayor al día de hoy.<br />"; }

	if (errores != "")
	{ errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

	return errores;
}

function ObtenerListaTiempoEntrega() {
	var request = new Object();
	request.pIdTiempoEntrega = $("#cmbTiempoEntrega").val();
	$("#cmbTiempoEntrega").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cotizacion.aspx/ObtenerListaTiempoEntrega",
		parametros: JSON.stringify(request)
	});
}

function ObtenerListaCampana() {
	var request = new Object();
	request.pIdCampana = $("#cmbCampana").val();
	$("#cmbCampana").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		url: "Cotizacion.aspx/ObtenerListaCampana",
		parametros: JSON.stringify(request)
	});
}

function ObtenerListaOportunidades(pRequest) {
	$("#cmbOportunidad").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		parametros: pRequest,
		url: "Cotizacion.aspx/ObtenerListaOportunidad"
	});
}

function ObtenerListaOportunidad() {
	var request = new Object();
	request.pIdCliente = $("#divFormaAgregarCotizacion, #divFormaEditarCotizacion").attr("idCliente");
	if (request.pIdCliente == "") {
		request.pIdCliente = "0";
	}

	$("#cmbOportunidad").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		parametros: JSON.stringify(request),
		url: "Cotizacion.aspx/ObtenerListaOportunidad",
		despuesDeCompilar: function () {
			var request = new Object();
			request.pIdOportunidad = 0;
			ObtenerListaNivelInteres(JSON.stringify(request));
		}
	});
}

function ObtenerListaNivelInteres(pRequest) {
	$("#cmbNivelInteresCotizacion").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		parametros: pRequest,
		url: "Cotizacion.aspx/ObtenerListaNivelInteres"
	});
}

function ObtenerListaCampanaOportunidad(pRequest) {
	$("#cmbCampana").obtenerVista({
		nombreTemplate: "tmplComboGenerico.html",
		parametros: pRequest,
		url: "Cotizacion.aspx/ObtenerListaCampanaOportunidad"
	});
}

function DeclinarCotizacion(btn) {
	var IdEstatusCotizacion = parseInt($("div.div_grdCotizacion_IdEstatusCotizacion", $(btn).parent("td").parent("tr").children('td[aria-describedby="grdCotizacion_IdEstatusCotizacion"]')).attr("idEstatusCotizacion"));
	if (IdEstatusCotizacion != 7) {
		$("#dialogDeclinarCotizacion").obtenerVista({
			nombreTemplate: "tmplDeclinarCotizacion.html",
			despuesDeCompilar: function () {
				var buttons = {
					"Declinar": function () {
						var MotivoDeclinar = $("#txtMotivoDeclinar").val();
						if (MotivoDeclinar.length > 0) {
							var Cotizacion = new Object();
							var IdCotizacion = parseInt($('td[aria-describedby="grdCotizacion_IdCotizacion"]', $(btn).parent("td").parent("tr")).text());
							Cotizacion.IdCotizacion = IdCotizacion;
							Cotizacion.MotivoDeclinar = MotivoDeclinar;
							var Request = JSON.stringify(Cotizacion);
							setDeclinarCotizacion(Request);
							$("#dialogDeclinarCotizacion").dialog("close");
						} else {
							MostrarMensajeError("Favor de poner un motivo para declinar");
						}
					},
					"Cancelar": function () {
						$(this).dialog("close");
					}
				};
				$("#dialogDeclinarCotizacion").dialog("option", "buttons", buttons);
				$("#dialogDeclinarCotizacion").dialog("open");
			}
		});
	} else {
		var Cotizacion = new Object();
		var IdCotizacion = parseInt($('td[aria-describedby="grdCotizacion_IdCotizacion"]', $(btn).parent("td").parent("tr")).text());
		Cotizacion.IdCotizacion = IdCotizacion;
		Cotizacion.MotivoDeclinar = "";
		var Request = JSON.stringify(Cotizacion);
		setDeclinarCotizacion(Request);
		$("#dialogDeclinarCotizacion").dialog("close");
	}
}

function setDeclinarCotizacion(Request) {
	$.ajax({
		url: "Cotizacion.aspx/DeclinarCotizacion",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				FiltroCotizacion();
			}
			MostrarMensajeError(json.Descripcion);
		}
	});
}