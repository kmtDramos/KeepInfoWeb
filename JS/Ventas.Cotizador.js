/**/

$(function () {

	// Mantener Sesion
	MantenerSesion();
	setInterval(MantenerSesion, 1000 * 60 * 2.5);

	ObtenerSucursalesFiltro();
	Inicializar_grdCotizador();

	$("#btnAgregarCotizacion").click(function () { ObtenerFormaAgregarPropuesta(0); });
	
});

//
function ObtenerSucursalesFiltro() {
	$.ajax({
		url: "Cotizador.aspx/ObtenerSucursales",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Sucursales = json.Modelo.Sucursales
				$("#cmbSucursal").html('');
				$("#cmbSucursal").append($('<option value="-1">-Todos-</option>'));
				for (x in Sucursales)
					$("#cmbSucursal").append($('<option value="' + Sucursales[x].Valor + '">' + Sucursales[x].Descripcion + '</option>'));

			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});

}

//
function FiltroCotizador() {

	var Cotizador = new Object();
	Cotizador.pTamanoPaginacion = $('#grdCotizador').getGridParam('rowNum');
	Cotizador.pPaginaActual = $('#grdCotizador').getGridParam('page');
	Cotizador.pColumnaOrden = $('#grdCotizador').getGridParam('sortname');
	Cotizador.pTipoOrden = $('#grdCotizador').getGridParam('sortorder');
	Cotizador.pFolio = ($("#gs_Folio").val() != null || !isNaN(parseInt($("#gs_Folio").val()))) ? parseInt($("#gs_Folio").val()) : 0;
	Cotizador.pCliente = ($("#gs_Cliente").val() != null) ? $("#gs_Cliente").val() : "";
	Cotizador.pIdOportunidad = ($("#gs_Folio").val() != null || !isNaN(parseInt($("#gs_IdOportunidad").val()))) ? $("#gs_IdOportunidad").val() : "";
	Cotizador.pAgente = ($("#gs_Folio").val() != null) ? $("#gs_Agente").val() : "";
	Cotizador.pIdTipoMoneda = $("#gs_TipoMoneda").val();
	Cotizador.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : 0;

	var pRequest = JSON.stringify(Cotizador);

	$.ajax({
		url: 'Cotizador.aspx/ObtenerPresupuesto',
		data: pRequest,
		dataType: 'json',
		type: 'post',
		contentType: 'application/json; charset=utf-8',
		success: function (Respuesta) {
			var json = Respuesta.d;
			$('#grdCotizador')[0].addJSONData(json);
		},
		complete: function () {
			InicializarComponentesGrid();
		}
	});

}

//
function InicializarComponentesGrid() {
	$("td[aria-describedby=grdCotizador_Consultar]", "#grdCotizador").each(function (index, elemento) {
		$(elemento).click(function () {
			var IdPresupuesto = parseInt($(this).parent("tr").children("td[aria-describedby=grdCotizador_IdPresupuesto]").html());
			ObtenerFormaAgregarPropuesta(IdPresupuesto);
		});
	});
	$("td[aria-describedby=grdCotizador_AI]", "#grdCotizador").each(function (index, elemento) {
		$(elemento).click(function () {
			var IdPresupuesto = parseInt($(this).parent("tr").children("td[aria-describedby=grdCotizador_IdPresupuesto]").html());
			var Ventana = $('<div><p><b>Motivo cancelación:</b></p><textarea id="txtMotivoCancelacion" style="width:400px;height:100px;resize;none;"></textarea/></div>');
			$(Ventana).dialog({
				modal: true,
				draggable: false,
				resizable: false,
				close: function () { $(Ventana).remove(); },
				buttons: {
					"Cambiar": function () {
						$(Ventana).dialog("close");
						var Motivo = $("#txtMotivoCancelacion",Ventana).val();
						CambiarEstatusPresupuesto(IdPresupuesto, Motivo);
					},
					"Cancelar": function () { $(Ventana).dialog("close"); }
				}
			});
		});
	});
}

//
function CambiarEstatusPresupuesto(IdPresupuesto, Motivo) {
	var Presupuesto = new Object();
	Presupuesto.IdPresupuesto = IdPresupuesto;
	Presupuesto.Motivo = Motivo;
	var Request = JSON.stringify(Presupuesto);
	$.ajax({
		url: "Cotizador.aspx/CambiarEstatusPresupuesto",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				FiltroCotizador();
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

//
function ObtenerFormaAgregarPropuesta(IdPresupuesto) {
	MostrarBloqueo();
	var ventana = $('<div id="divDialogCotizador"></div>');
	var guardado = false;
	var Presupuesto = new Object();
	Presupuesto.IdPresupuesto = IdPresupuesto;
	var Request = JSON.stringify(Presupuesto);
	$(ventana).attr("Guardado", 0);
	$(ventana).obtenerVista({
		url: "Cotizador.aspx/ObtenerFormaPresupuesto",
		parametros: Request,
		nombreTemplate: "tmplAgregarPresupuesto.html",
		despuesDeCompilar: function () {

			window.close = function () { if (!guardado) return confirm("¿Desea salir sin guardar?"); }
			window.onbeforeunload = function () { if (!guardado) return confirm("¿Desea salir sin guardar?"); }

			var IdPresupuesto = parseInt($("#cotizador").attr("IdPresupuesto"));

			$("#cmbEstatusPresupuesto", "#cotizador").val(1);

			$(ventana).dialog({
				modal: true,
				draggable: false,
				resizable: false,
				title: "Cotizador",
				width: 1150,
				height: 650,
				beforeClose: function (evente, ui) { if (!guardado) return confirm("¿Desea salir sin guardar la cotización?"); },
				close: function (event, ui) { $(ventana).remove(); window.close = null; window.onbeforeunload = null; },
				buttons: {
					"Guardar": function () {
						guardado = true;
						GuardarPresupuesto();
						$(this).dialog("close");
					},
					"Cerrar": function () { $(ventana).dialog("close"); }
				}
			});

			$("#txtFechaExpiracion").datepicker().prop("readonly", true);

			$("#btnAgregarConcepto").click(AgregarConcepto);

			$("#btnLimpiarConceptos").click(function () {
				var modal = $("<div><p>¿Desea borrar todos las partidas de la cotización?</p></div>");
				$(modal).dialog({
					modal: true,
					draggable: false,
					resizable: false,
					title: "Borrar conceptos",
					close: function () {
						$(modal).remove();
					},
					buttons: {
						"Borrar conceptos": function () {
							$("tbody", "#conceptos").html('');
							BajaConceptos(IdPresupuesto);
							$(modal).dialog("close");
						},
						"Cancelar": function () {
							$(modal).dialog("close");
						}
					}
				});
			});
			
			$("#btnDescargarPlantilla").click(function () {
				if (!($("#contractFileDownload").length)) {
                    $('body').append('<form id="contractFileDownload" method="post" enctype="multipart/form-data" action="downloadFile.aspx" target="dummyContractFileDownload"><input type="hidden" name="formContractFileDownload" id="formContractFileDownload"/></form>');
                    $(function() {
                        $('#contractFileDownload').iframer({
                            onComplete: function(data) {
                            }
                        });
                    });
                }
                $("#formContractFileDownload").val('Plantilla');
                $("#contractFileDownload").submit();
			});
			
			$("tr", "#conceptos tbody").each(function (index, elemento) {
				InitCoponentesConecpto(elemento);
			});

			$("#cmbTipoMoneda").change(CalcularPresupuesto);

			$("#cmbTipoMoneda").change(ObtenerTipoCambio);

			AutocompletarClienteCotizador();

			CalcularPresupuesto();

			NumerarConceptos();

			OcultarBloqueo();

			$("#btnSubirExcel").click(SubirExcel);

		}
	});
	
	
}

// Funcion para enviar archivo de Excel
function SubirExcel() {
	var ventana = $('<div>'+
						'<div id="divSubirExcel">'+
							'<noscript>'+
								'<p>Favor de habilitar JavaScript para poder subir la imagen.</p>'+
								'<!-- or put a simple form for upload here -->'+
							'</noscript>'+
						'</div>' +
					'</div>');
	$(ventana).dialog({
		modal: true,
		width:310,
		draggable: false,
		resizable: false,
		close: function () { $(ventana).remove(); },
		open: function(){
			var ctrlSubirLogo = new qq.FileUploader({
				element: document.getElementById('divSubirExcel'),
				action: '../ControladoresSubirArchivos/SubirXLSPresupuesto.ashx',
				allowedExtensions: ["xls", "xlsx"],
				template: '<div class="qq-uploader">' +
					'<div class="qq-upload-drop-area"></div>' +
					'<div class="qq-upload-container-list"><ul class="qq-upload-list"><li><span class="qq-upload-file">Favor de subir una imagen.</span></li></ul></div>' +
					'<div class="qq-upload-container-buttons"><div class="qq-upload-button qq-divBotonSubir">+ Agregar</div></div>' +
					'</div>',
				onSubmit: function (id, fileName) {
					$(".qq-upload-list").empty();
				},
				onComplete: function (id, file, responseJSON) {
					OcultarBloqueo();
				}
			});
		},
		buttons: {
			"Cerrar": function () {
				$(ventana).dialog("close");
			}
		}
	});
}

// Funcion para autocompletar 
function AutocompletarClienteCotizador() {
	$('#txtCliente').autocomplete({
		source: function (request, response) {
			var Cliente = new Object();
			Cliente.pCliente = $("#txtCliente").val();

			var Request = JSON.stringify(Cliente);
			$.ajax({
				url: 'Cotizador.aspx/BuscarCliente',
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
			var pIdCliente = ui.item.id;
			$("#txtCliente").attr("IdCliente", pIdCliente);
			ObtenerOportunidadesCliente();
			ObtenerDireccionesCliente();
			ObtenerContactosCliente();
		},
		focus: function (event, ui) {
			var pIdCliente = ui.item.id;
			$("#txtCliente").attr("IdCliente", pIdCliente);
			ObtenerOportunidadesCliente();
			ObtenerDireccionesCliente();
			ObtenerContactosCliente();
		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});
}

//
function ObtenerContactosCliente() {
	var Contactos = new Object();
	Contactos.IdCliente = parseInt($("#txtCliente").attr("IdCliente"));
	var Request = JSON.stringify(Contactos);
	$.ajax({
		url: "Cotizador.aspx/ObtenerContactosCliente",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				LlenarContactosCliente(json.Modelo.Contactos);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

//
function LlenarContactosCliente(Contactos) {
	var IdContactoOrganizacion = parseInt($("#cmbContacto").val());
	$("#cmbContacto").html('');
	$("#cmbContacto").append($('<option value="-1">Elegir una opción...</option>'));
	for (x in Contactos)
		$("#cmbContacto").append($('<option value="' + Contactos[x].IdContactoOrganizacion + '">' + Contactos[x].Nombre + '</option>'));
	$("#cmbContacto").val(IdContactoOrganizacion);
}

//
function ObtenerDireccionesCliente() {
	var Direccion = new Object();
	Direccion.IdCliente = parseInt($("#txtCliente").attr("IdCliente"));
	var Request = JSON.stringify(Direccion);
	$.ajax({
		url: "Cotizador.aspx/ObtenerDireccionesCliente",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				LlenarDireccionesCliente(json.Modelo.Direcciones);
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

//
function LlenarDireccionesCliente(Direcciones) {
	var IdDireccionOrganizacion = parseInt($("#cmbDireccionCliente").val());
	$("#cmbDireccionCliente").html('');
	$("#cmbDireccionCliente").append($('<option value="-1">Elegir una opción...</option>'));
	for (x in Direcciones)
		$("#cmbDireccionCliente").append($('<option value="' + Direcciones[x].IdDireccionOrganizacion + '">' + Direcciones[x].Descripcion + '</option>'));
	$("#cmbDireccionCliente").val(IdDireccionOrganizacion);
}

//
function ObtenerOportunidadesCliente() {
	var Oportunidades = new Object();
	Oportunidades.IdCliente = parseInt($("#txtCliente").attr("IdCliente"));
	var Request = JSON.stringify(Oportunidades);
	$.ajax({
		url: "Cotizador.aspx/ObtenerOportunidadesCliente",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0) {
				LlenarOportunidadesCliente(json.Modelo.Oportunidades);
			}
			else {
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

//
function ObtenerTipoCambio() {
	var Presupuesto = ObtenerDatosPresupuesto();
	var Request = JSON.stringify(Presupuesto);
	$.ajax({
		url: "Cotizador.aspx/ObtenerTipoCambio",
		type: "post",
		data: Request,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				if ($(".costounitario").length > 0)
				{
					var TipoCambio = $("#txtTipoCambio").val();
					TipoCambio = (json.Modelo.TipoCambio > TipoCambio) ? 1 / json.Modelo.TipoCambio : TipoCambio;
					$(".costounitario").each(function (index, element)
					{
						var Costo = parseFloat($(element).val().replace('$', '').replace(/,/gi, ''));
						$(".costounitario").val(Costo * TipoCambio).focus().blur();
						CalcularPresupuesto();
					});
				}
				$("#txtTipoCambio").val(json.Modelo.TipoCambio);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

//
function LlenarOportunidadesCliente(Oportunidades) {
	var IdOportunidad = parseInt($("#cmbIdOportunidad").val());
	$("#cmbIdOportunidad").html('');
	$("#cmbIdOportunidad").append($('<option value="-1">Elegir una opción...</option>'));
	for (x in Oportunidades)
		$("#cmbIdOportunidad").append($('<option value="' + Oportunidades[x].IdOportunidad + '" margen="' + Oportunidades[x].Margen + '">' + Oportunidades[x].Oportunidad + '</option>'));
	$("#cmbIdOportunidad").val(IdOportunidad);
}

//
function AgregarConcepto()
{
	var valido = ValidarCotizacion()
	if (valido == "") {
		var plantilla = '<tr class="concepto" IdConcepto="0">' +
                            '<td align="center" style="border:1px solid #bbb;" class="numConcepto"></td>' +
                            '<td><select class="wInherit cmbDivision division"></select></td>' +
                            '<td><input type="text" class="wInherit upperCase proveedor" IdProveedor="0" placeholder="Proveedor" /></td>' +
                            '<td><input type="text" class="wInherit upperCase clave" placeholder="Clave" /></td>' +
                            '<td><input type="text" class="wInherit upperCase descripcion" placeholder="Descripción" /></td>' +
                            '<td><input type="text" class="wInherit txtMonto costounitario" placeholder="$0.00" value="$0.00"/></td>' +
							'<td><input type="text" class="wInherit txtMonto manoobra" placeholder="$0.00" value="$0.00"/></td>'+
                            '<td><input type="text" class="wInherit txtPorcentaje margen" value="25%"/></td>' +
                            '<td><input type="text" class="wInherit txtPorcentaje descuento" placeholder="0%" value="0%"/></td>' +
                            '<td><input type="text" class="wInherit txtPorcentaje margenNeto" placeholder="0%" value="0%" readonly/></td>' +
                            '<td><input type="text" class="wInherit txtNumero cantidad" placeholder="0" value="0"/></td>' +
                            '<td><input type="text" class="wInherit txtMonto preciounitario" placeholder="$0.00" value="$0.00" readonly/></td>' +
                            '<td><input type="text" class="wInherit txtMonto costototal" value="$0.00" readonly/></td>' +
                            '<td><input type="text" class="wInherit txtMonto preciototal"  value="$0.00" readonly/></td>' +
                            '<td><input type="text" class="wInherit txtMonto utilidad" value="$0.00" readonly/></td>' +
                            '<td align="center" style="border:1px solid #bbb;"><img class="btnEliminarConcepto" src="../Images/eliminar.png" height="12"></td>' +
                        '</tr>';

		var concepto = $(plantilla);

		$("tbody", "#conceptos").append(concepto);
		InitCoponentesConecpto(concepto);
		NumerarConceptos();
		$(".clave", concepto).focus();
		LlenarComboDivision($(".division", concepto));
	}
	else
	{
		MostrarMensajeError(valido);
	}
}

//
function LlenarComboDivision(elemento)
{
	$.ajax({
		url: "Cotizador.aspx/ObtenerDivisiones",
		type: "post",
		dataType: "json",
		contentType: "application/json;charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Divisiones = json.Modelo.Divisiones;
				$(elemento).append($('<option value="0">Elegir una opción...</option>'))
				for(x in Divisiones){
					$(elemento)
						.append(
							$('<option value="' + Divisiones[x].Valor + '" LimiteDescuento="' + Divisiones[x].LimiteDescuento + '" LimiteMargen="' + Divisiones[x].LimiteMargen + '">' + Divisiones[x].Descripcion + '</option>')
						);
				}
			}
			else
			{
				MostrarError(json.Descripcion);
			}
		}
	});
}

//
function InitCoponentesConecpto(concepto) {

	$(".utilidad,.preciototal,.costototal,.preciounitario", concepto).focus(function () { $(this).prop("readonly", true) });

	$("th", "#trEncabezados").each(function (index, th) { $("td:eq(" + index + ")", concepto).width($(th).attr("width")-2) });

	var inputProveedor = $(".proveedor", concepto);
	$(inputProveedor).autocomplete({
		source: function (request, response) {
			var Proveedor = new Object();
			Proveedor.pProveedor = $(inputProveedor).val();

			var Request = JSON.stringify(Proveedor);
			$.ajax({
				url: 'Cotizador.aspx/BuscarProveedor',
				type: 'POST',
				data: Request,
				dataType: 'json',
				contentType: 'application/json; charset=utf-8',
				success: function (pRespuesta) {
					var json = jQuery.parseJSON(pRespuesta.d);
					response($.map(json.Modelo.Proveedores, function (item) {
						return { label: item.Proveedor, value: item.Proveedor, id: item.IdProveedor }
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			$(inputProveedor).val(ui.item.Proveedor);
			$(inputProveedor).attr("IdProveedor", ui.item.IdProveedor);
		},
		focus: function (event, ui) {
			$(inputProveedor).val(ui.item.Proveedor);
			$(inputProveedor).attr("IdProveedor", ui.item.IdProveedor);
		},
		open: function () { $(inputProveedor).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(inputProveedor).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});

	$(".btnEliminarConcepto", concepto).click(function () { EliminarConcepto(concepto); });

	$(".txtMonto, .txtNumero", concepto).keypress(function (event) { return ((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 190); });

	$(".margen", concepto).blur(function () {
		var LimiteMargen = parseInt($(".division option:selected", concepto).attr("LimiteMargen"));
		if (parseInt($(this).val()) < LimiteMargen)
		{
			$(this).val(LimiteMargen);
			MostrarMensajeError("El margen debe ser mayor o igual a "+ LimiteMargen +"%");
		}
	});

	$(".descuento", concepto).blur(function () {
		var LimiteDescuento = parseInt($(".division option:selected", concepto).attr("LimiteDescuento"));
		if (parseInt($(this).val()) > LimiteDescuento)
		{
			$(this).val(LimiteDescuento);
			MostrarMensajeError("El descuento debe ser menor o igual a " + LimiteDescuento + "%");
		}
	});

	$(".txtPorcentaje", concepto)
        .focus(function () {
        	var porcentaje = $(this).val().replace('%', '');
        	$(this).val(porcentaje);
        	this.select();
        })
        .blur(function () {
        	var porcentaje = $(this).val().replace('%', '');
        	if (isNaN(parseFloat(porcentaje)))
        		porcentaje = 0;
        	porcentaje = Math.abs(porcentaje);
        	$(this).val(porcentaje.toFixed(0) + '%');
        })
        .keypress(function () {
        	return ((event.keyCode >= 48 && event.keyCode <= 57) || (this.value >= 0 && this.value <= 100));
        });

	$(".txtMonto", concepto)
        .focus(function () {
        	var monto = $(this).val().replace('$', '').replace(/,/g, '');
        	$(this).val(monto);
        	this.select();
        })
        .blur(function () {
        	var monto = $(this).val().replace(/$/g, '').replace(/,/g, '');
        	if (isNaN(parseFloat(monto)))
        		monto = 0;
        	$(this).val(formato.moneda(monto, '$'));
        });

	$(".preciounitario, .cantidad, .descuento, .siniva, .manoobra, .margen", concepto).change(CalcularPresupuesto);

	$(".clave", concepto).autocomplete({
		source: function (request, response) {
			var Concepto = new Object();
			Concepto.Clave = $(".clave", concepto).val();
			Concepto.IdTipoMoneda = parseInt($("#cmbTipoMoneda").val());
			var Request = JSON.stringify(Concepto);
			$.ajax({
				url: "Cotizador.aspx/ObtenerConceptoClave",
				type: "post",
				data: Request,
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (Respuesta) {
					var json = JSON.parse(Respuesta.d);
					response($.map(json.Modelo.Conceptos, function (item) {
						return { label: item.Descripcion+' ('+ item.Clave +')', value: item.Clave, Descripcion: item.Descripcion, Costo: item.Costo, IdProducto: item.IdProducto, IdServicio: item.IdServicio}
					}));
				}
			});
		},
		minLength: 2,
		select: function (event, ui) {
			$(".descripcion", concepto).val(ui.item.Descripcion);
            $(".costounitario", concepto).val(ui.item.Costo);
            $(".clave", concepto).attr("IdProducto", ui.item.IdProducto);
            $(".clave", concepto).attr("IdServicio", ui.item.IdServicio);
		},
		focus: function (event, ui) {
			$(".descripcion", concepto).val(ui.item.Descripcion);
			$(".costo", concepto).val(ui.item.Costo);
		},
		change: function (event, ui) { },
		open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
		close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
	});

}

//
function EliminarConcepto(concepto) {
	var clave = $(".clave", concepto).val();
	var descripcion = $(".descripcion", concepto).val();
	var mensaje = (clave.length > 0 && descripcion.length > 0) ? ": \"" + descripcion.substring(0, 30) + "(" + clave + ")\"" : "";
	var ventana = $("<div><p>¿Desea eliminar el concepto?</p></div>");
	var idconcepto = parseInt($(concepto).attr("IdConcepto"));
	$(ventana).dialog({
		modal: true,
		draggable: false,
		resizable: false,
		title: "Eliminar concepto",
		close: function () {
			$(ventana).remove();
			NumerarConceptos();
		},
		buttons: {
			"Eliminar": function () {
				$(concepto).remove();
				BajaConcepto(idconcepto);
				$(ventana).dialog("close");
			},
			"Cancelar": function () {
				$(ventana).dialog("close");
			}
		}
	});
}

//
function BajaConcepto(IdConcepto) {
	var Concepto = new Object();
	if (IdConcepto != 0) {
		Concepto.IdConcepto = IdConcepto;
		var Request = JSON.stringify(Concepto);
		$.ajax({
			url: "Cotizador.aspx/EliminarConcepto",
			type: "post",
			data: Request,
			dataType: "json",
			contentType: "application/json;charset=utf-8",
			success: function (Respuesta) {
				var json = JSON.parse(Respuesta.d);
				if (json.Error != 0) {
					MostrarMensajeError(json.Descripcion);
				}
				$("#divDialogCotizador").attr("guardado", 1);
				CalcularPresupuesto();
				GuardarPresupuesto();
			}
		});
	}
}

//
function BajaConceptos(IdPresupuesto) {
	var Presupuesto = new Object();
	if (IdPresupuesto != 0) {
		Presupuesto.IdPresupuesto = IdPresupuesto;
		var Request = JSON.stringify(Presupuesto);
		$.ajax({
			url: "Cotizador.aspx/EliminarConceptos",
			type: "post",
			data: Request,
			dataType: "json",
			contentType: "application/json;charset=utf-8",
			success: function (Respuesta) {
				var json = JSON.parse(Respuesta.d);
				if (json.Error != 0) {
					MostrarMensajeError(json.Descripcion);
				}
				$("#divDialogCotizador").attr("guardado", 1);
				CalcularPresupuesto();
				GuardarPresupuesto();
			}
		});
	}
}

// 
function NumerarConceptos() {
	$(".concepto").each(function (index, concepto) {
		$(".numConcepto", concepto).text(index + 1);
	});
}

//
function CalcularPresupuesto() {
	var Presupuesto = new Object();
	Presupuesto.Subtotal = 0;
	Presupuesto.Descuento = 0;
	Presupuesto.SubtotalDescuento = 0;
	Presupuesto.IVA = 0;
	Presupuesto.Total = 0;

	$(".concepto").each(function (index, elemento) {

		var CostoUnitario = parseFloat($(".costounitario", elemento).val().replace('$', '').replace(/,/g, ''));
		var ManoObra = parseFloat($(".manoobra", elemento).val().replace('$', '').replace(/,/g, ''));
		var PrecioUnitario = parseFloat($(".preciounitario", elemento).val().replace('$', '').replace(/,/g, ''));
		var Descuento = parseFloat($(".descuento", elemento).val().replace('%', ''));
		var Cantidad = parseFloat($(".cantidad", elemento).val().replace('$', '').replace(/,/g, ''));
		var Margen = parseFloat($(".margen", elemento).val().replace('%', ''));
		var IVA = (!$(".siniva", elemento).is(":checked")) ? 0.16 : 0;

		CostoUnitario = (!isNaN(CostoUnitario)) ? CostoUnitario : 0;
		ManoObra = (!isNaN(ManoObra)) ? ManoObra : 0;
		PrecioUnitario = (!isNaN(PrecioUnitario)) ? PrecioUnitario : 0;
		Descuento = (!isNaN(Descuento)) ? Descuento : 0;
		Cantidad = (!isNaN(Cantidad)) ? Cantidad : 0;
		Margen = (!isNaN(Margen)) ? Margen : 0;

		PrecioUnitario = (CostoUnitario + ManoObra) / ((100 - (Margen - Descuento)) / 100);

		Presupuesto.Subtotal += (PrecioUnitario * Cantidad);
		Presupuesto.Descuento += (PrecioUnitario * Cantidad) * (Descuento / 100);
		Presupuesto.SubtotalDescuento += ((PrecioUnitario - (PrecioUnitario * Cantidad) * (Descuento / 100)) * Cantidad);
		Presupuesto.IVA += ((PrecioUnitario - (PrecioUnitario * Cantidad) * (Descuento / 100)) * Cantidad) * IVA;
		Presupuesto.Total += ((PrecioUnitario - (PrecioUnitario * Cantidad) * (Descuento / 100)) * Cantidad) * (1 + IVA);

		$(".preciounitario", elemento).val(formato.moneda(PrecioUnitario, '$'));
		$(".costototal", elemento).val(formato.moneda((CostoUnitario + ManoObra) * Cantidad, '$'));
		$(".preciototal", elemento).val(formato.moneda(((PrecioUnitario - Descuento) * Cantidad), '$'));
		$(".margen", elemento).val(Margen + '%');
		$(".margenNeto", elemento).val((Margen -Descuento)+"%");
		$(".utilidad", elemento).val(formato.moneda(((PrecioUnitario - (PrecioUnitario * Cantidad) * (Descuento / 100)) * Cantidad) * (Margen / 100), '$'));

	});

	$("#txtSubtotal").text(formato.moneda(Presupuesto.Subtotal, '$'));
	$("#txtDescuento").text(formato.moneda(Presupuesto.Descuento, '$'));
	$("#txtSubtotalDescuento").text(formato.moneda(Presupuesto.SubtotalDescuento, '$'));
	$("#txtIVA").text(formato.moneda(Presupuesto.IVA, '$'));
	$("#txtTotal").text(formato.moneda(Presupuesto.Total, '$'));
	$("#txtTotalLetra").text(covertirNumLetras(Presupuesto.Total.toFixed(2) + '', $("option:selected", "#cmbTipoMoneda").text()));

}

//
function ValidarCotizacion() {
	var valido = '';

	if ($("#txtCliente").attr("IdCliente") == "0")
		valido = "<li>Favor de elegir un cliente.</li>";

	if ($("#cmbIdOportunidad").val() == -1)
		valido += "<li>Favor de elegir una oportunidad.</li>";

	if ($("#cmbDireccionCliente").val() == -1)
		valido += "<li>Favor de elegir una dirección.</li>";

	if ($("#cmbTipoMoneda").val() == null)
		valido += "<li>Favor de elegir una moneda.</li>";

	if ($("#txtFechaExpiracion").val() == '')
		valido += "<li>Favor de elegir una fecha de vigencia.</li>";

	if ($("#txtNota").val() == '')
		valido += "<li>Favor de completar la nota de la cotización</li>";

	if (valido != '')
		valido = "<p>Faltan campos por completar:</p><ul>" + valido + "</ul>";

	return valido;
}

//
function GuardarPresupuesto() {

	var valido = ValidarCotizacion();

	if (valido == '') {
		var Presupuesto = ObtenerDatosPresupuesto();
		console.log(Presupuesto);
		if (true) {
			var Datos = {pPresupuesto: Presupuesto}
			var Request = JSON.stringify(Datos);
			$.ajax({
				url: "Cotizador.aspx/GurardarPresupuesto",
				type: "post",
				data: Request,
				dataType: "json",
				contentType: "application/json; charset=utf-8",
				success: function (Respuesta) {
					var json = JSON.parse(Respuesta.d);
					if (json.Error == 0)
					{
						FiltroCotizador();
						$("#cotizador").attr("IdPresupuesto", json.Modelo.IdPresupuesto);
						MostrarMensajeError("Guardado con éxito.");
					}
					else {
						MostrarMensajeError(json.Descripcion);
					}
				}
			});
		}
	}
	else
	{
		MostrarMensajeError(valido);
	}

}

//
function ObtenerDatosPresupuesto() {

	var Presupuesto = new Object();
	Presupuesto.IdPresupuesto = parseInt($("#cotizador").attr("IdPresupuesto"));
	Presupuesto.IdCliente = parseInt($("#txtCliente").attr("IdCliente"));
	Presupuesto.IdContactoOrganizacion = parseInt($("#cmbContacto").val());
	Presupuesto.IdDireccionOrganizacion = parseInt($("#cmbDireccionCliente").val());
	Presupuesto.IdOportunidad = parseInt($("#cmbIdOportunidad").val());
	Presupuesto.IdTipoMoneda = parseInt($("#cmbTipoMoneda").val());
	Presupuesto.TipoCambio = parseFloat($("#txtTipoCambio").val());
	Presupuesto.IdEstatusPresupuesto = parseInt($("#cmbEstatusPresupuesto").val());
	Presupuesto.FechaExpiracion = $("#txtFechaExpiracion").val();
	Presupuesto.Nota = $("#txtNota").val();
	Presupuesto.MontoLetra = $("#txtTotalLetra").text();
	Presupuesto.Subtotal = parseFloat($("#txtSubtotal").text().replace('$', '').replace(/,/g, ''));
	Presupuesto.Descuento = parseFloat($("#txtDescuento").text().replace('$', '').replace(/,/g, ''));
	Presupuesto.IVA = parseFloat($("#txtIVA").text().replace('$', '').replace(/,/g, ''));
	Presupuesto.Total = parseFloat($("#txtTotal").text().replace('$', '').replace(/,/g, ''));
	Presupuesto.Costo = 0;
	Presupuesto.ManoObra = 0;
	Presupuesto.Utilidad = 0;
	Presupuesto.Conceptos = [];

	$(".concepto").each(function (index, elemento) {

		var Concepto = new Object();
		Concepto.IdPropuestaConcepto = parseInt($(elemento).attr("IdConcepto"));
		Concepto.Orden = $(".numConcepto", elemento).text();
        Concepto.Clave = $(".clave", elemento).val();
        Concepto.IdProducto = parseInt($(".clave", elemento).attr("IdProducto"));
        Concepto.IdServicio = parseInt($(".clave", elemento).attr("IdServicio"));
		Concepto.Descripcion = $(".descripcion", elemento).val();
		Concepto.Proveedor = $(".proveedor", elemento).val();
		Concepto.CostoUnitario = parseFloat($(".costounitario", elemento).val().replace('$', '').replace(/,/g, ''));
		Concepto.ManoObra = parseFloat($(".manoobra", elemento).val().replace('$', '').replace(/,/g, ''));
		Concepto.PrecioUnitario = parseFloat($(".preciounitario", elemento).val().replace('$', '').replace(/,/g, ''));
		Concepto.Descuento = parseFloat($(".descuento", elemento).val().replace('%', ''));
		Concepto.Cantidad = parseFloat($(".cantidad", elemento).val().replace('$', '').replace(/,/g, ''));
		Concepto.Margen = parseFloat($(".margen", elemento).val().replace('%', ''));
		Concepto.IVA = (!$(".siniva", elemento).is(":checked")) ? 0.16 : 0;
		Concepto.Total = parseFloat($(".preciototal", elemento).val().replace('$', '').replace(/,/g, ''));
		Concepto.Utilidad = parseFloat($(".utilidad", elemento).val().replace('$', '').replace(/,/g, ''));
		Concepto.IdDivision = parseInt($(".division", elemento).val());

		Presupuesto.Costo += parseFloat($(".costototal").val().replace('$', '').replace(/,/g, ''));
		Presupuesto.ManoObra += parseFloat($(".manoobra", elemento).val().replace('$', '').replace(/,/g, ''));
		Presupuesto.Utilidad += parseFloat($(".utilidad").val().replace('$', '').replace(/,/g, ''));

		Presupuesto.Conceptos.push(Concepto);

	});

	return Presupuesto;

}

//
function ImprimirPresupuesto(IdPresupuesto)
{
	MostrarBloqueo();

	var Presupuesto = new Object();
	Presupuesto.IdPresupuesto = parseInt(IdPresupuesto);
	var Request = JSON.stringify(Presupuesto);

	var formato = $("<div></div>");
	$(formato).obtenerVista({
		url: "Cotizador.aspx/Imprimir",
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
