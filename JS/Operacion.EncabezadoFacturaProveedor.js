//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("EncabezadoFacturaProveedor");
    });

    ObtenerTotalesEstatusRecepcion();
    ObtenerFormaFiltrosEncabezadoFacturaProveedor();

    $("#gbox_grdEncabezadoFacturaProveedor").livequery(function() {
        $("#grdEncabezadoFacturaProveedor").jqGrid('navButtonAdd', '#pagEncabezadoFacturaProveedor', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {
                var pRazonSocial = "";
                var pNumeroFactura = "";
                var pIdEstatusRecepcion = -1;
                var pDivision = "";
                var pAI = 0;

                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;
                var pNumeroSerie = "";
                var pNumeroPedido = "";
                var pIdTipoArchivo = 0;

                if ($("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != null && $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != "") {
                    pIdEstatusRecepcion = $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado");
                }

                if ($('#gs_NumeroFactura').val() != null) { pNumeroFactura = $("#gs_NumeroFactura").val(); }

                if ($('#gs_RazonSocial').val() != null) { pRazonSocial = $("#gs_RazonSocial").val(); }

                if ($('#gs_Division').val() != null) { pDivision = $("#gs_Division").val(); }

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

                if ($("#txtNumeroSerieBuscador").val() != "" && $("#txtNumeroSerieBuscador").val() != null) {
                	pNumeroSerie = $("#txtNumeroSerieBuscador").val();
                }

                if ($("#txtNumeroClaveBuscador").val() != "" && $("#txtNumeroClaveBuscador").val() != null) {
                	pClave = $("#txtNumeroClaveBuscador").val();
                }

                if ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null) {
                    pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
                }

                pIdTipoArchivo = $("#cmbBusquedaDocumento").val();

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pRazonSocial: pRazonSocial,
                    pNumeroFactura: pNumeroFactura,
                    pIdEstatusRecepcion: pIdEstatusRecepcion,
                    pDivision: pDivision,
                    pAI: pAI,
                    pFechaInicial: pFechaInicial,
                    pFechaFinal: pFechaFinal,
                    pPorFecha: pPorFecha,
                    pNumeroSerie: pNumeroSerie,
                    pNumeroPedido: pNumeroPedido,
                    pIdTipoArchivo: pIdTipoArchivo
                }, downloadType: 'Normal'
                });

            }
        });
    });

    $("#dialogAgregarEncabezadoFacturaProveedor, #dialogEditarEncabezadoFacturaProveedor, #dialogConsultarEncabezadoFacturaProveedor").on("click", "#divImprimir", function() {
        var IdFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor, #divFormaConsultarEncabezadoFacturaProveedor").attr("idencabezadofacturaproveedor");
        Imprimir(IdFacturaProveedor);
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarEncabezadoFacturaProveedor", function() {
        ObtenerFormaSeleccionarAlmacen();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaFacturasPendientesPorValidar", function() {
        ObtenerFormaFacturasPendientesPorValidar();
    });

    $("#grdEncabezadoFacturaProveedor").on("click", ".imgFormaConsultarEncabezadoFacturaProveedor", function() {
        var registro = $(this).parents("tr");
        var EncabezadoFacturaProveedor = new Object();
        EncabezadoFacturaProveedor.pIdEncabezadoFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_IdEncabezadoFacturaProveedor']").html());
        ObtenerFormaConsultarEncabezadoFacturaProveedor(JSON.stringify(EncabezadoFacturaProveedor));
    });

    $('#grdEncabezadoFacturaProveedor').one('click', '.div_grdEncabezadoFacturaProveedor_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_AI']").children().attr("baja")
        var idEncabezadoFacturaProveedor = $(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_IdEncabezadoFacturaProveedor']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        if (baja == "true") {
            SetCambiarEstatus(idEncabezadoFacturaProveedor, baja);
        }
        else {
            SetActivarEstatus(idEncabezadoFacturaProveedor, baja);
        }
    });

    $("#grdFacturasPendientesPorValidar").on("click", ".imgFormaConsultarFacturasPendientesPorValidar", function() {
        var registro = $(this).parents("tr");
        var FacturasPendientesPorValidar = new Object();
        FacturasPendientesPorValidar.pIdEncabezadoFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdFacturasPendientesPorValidar_IdEncabezadoFacturaProveedor']").html());
        ObtenerFormaConsultarEncabezadoFacturaProveedor(JSON.stringify(FacturasPendientesPorValidar));
    });

    $("#grdFacturasPendientesPorValidar").on("click", ".imgFormaMarcarComoRevisada", function() {
        var registro = $(this).parents("tr");
        var FacturasPendientesPorValidar = new Object();
        $("#dialogConfirmarRevision").attr("idEncabezadoFacturaProveedor", parseInt($(registro).children("td[aria-describedby='grdFacturasPendientesPorValidar_IdEncabezadoFacturaProveedor']").html()));
        ObtenerFormaConfirmarRevision();
    });

    $('#grdFacturasPendientesPorValidar').one('click', '.div_grdFacturasPendientesPorValidar_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdFacturasPendientesPorValidar_AI']").children().attr("baja")
        var idFacturasPendientesPorValidar = $(registro).children("td[aria-describedby='grdFacturasPendientesPorValidar_IdFacturasPendientesPorValidar']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        if (baja == "true") {
            SetCambiarEstatus(idFacturasPendientesPorValidar, baja);
        }
        else {
            SetActivarEstatus(idFacturasPendientesPorValidar, baja);
        }
    });

    $("#grdDetallePedido").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var DetallePedido = new Object();
        var cantidad = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_RecepcionCantidad']").html());
        if (cantidad > 0) {
            DetallePedido.pIdDetallePedido = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdCotizacionDetalle']").html());
            ObtenerDatosDetallePedido(JSON.stringify(DetallePedido));
            $("#dialogMuestraDetallePedido").dialog("close");
        } else {
            MostrarMensajeError("El saldo restante es igual a 0, no se puede bajar la partida.");
        }
    });

    $("#grdDetallePedido").on("click", ".imgSeleccionarDetallePedido", function() {
        var registro = $(this).parents("tr");
        var DetallePedido = new Object();
        var cantidad = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_RecepcionCantidad']").html());
        if (cantidad > 0) {
            DetallePedido.pIdDetallePedido = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdCotizacionDetalle']").html());
            ObtenerDatosDetallePedido(JSON.stringify(DetallePedido));
            $("#dialogMuestraDetallePedido").dialog("close");
        } else {
            MostrarMensajeError("El saldo restante es igual a 0, no se puede bajar la partida.");
        }

    });

    $("#grdDetalleOrdenCompra").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var DetalleOrdenCompra = new Object();
        var cantidad = parseInt($(registro).children("td[aria-describedby='grdDetalleOrdenCompra_RecepcionCantidad']").html());
        if (cantidad > 0) {
            DetalleOrdenCompra.pIdDetalleOrdenCompra = parseInt($(registro).children("td[aria-describedby='grdDetalleOrdenCompra_IdOrdenCompraDetalle']").html());
            ObtenerDatosDetalleOrdenCompra(JSON.stringify(DetalleOrdenCompra));
            $("#dialogMuestraDetalleOrdenCompra").dialog("close");
        } else {
            MostrarMensajeError("El saldo restante es igual a 0, no se puede bajar la partida.");
        }
    });

    $("#grdDetalleOrdenCompra").on("click", ".imgSeleccionarDetalleOrdenCompra", function() {
        var registro = $(this).parents("tr");
        var DetalleOrdenCompra = new Object();
        var cantidad = parseInt($(registro).children("td[aria-describedby='grdDetalleOrdenCompra_RecepcionCantidad']").html());
        if (cantidad > 0) {
            DetalleOrdenCompra.pIdDetalleOrdenCompra = parseInt($(registro).children("td[aria-describedby='grdDetalleOrdenCompra_IdOrdenCompraDetalle']").html());
            ObtenerDatosDetalleOrdenCompra(JSON.stringify(DetalleOrdenCompra));
            $("#dialogMuestraDetalleOrdenCompra").dialog("close");
        } else {
            MostrarMensajeError("El saldo restante es igual a 0, no se puede bajar la partida.");
        }

    });

    $("#dialogAgregarEncabezadoFacturaProveedor, #dialogEditarEncabezadoFacturaProveedor").on("click", "#divBuscarPedidosCliente", function() {
        $("#divFormaMuestraDetallePedido").obtenerVista({
            nombreTemplate: "tmplAgregarDetallePedido.html",
            despuesDeCompilar: function() {
                FiltroDetallePedido();
                $("#dialogMuestraDetallePedido").dialog("open");
            }
        });
    });

    $("#dialogAgregarEncabezadoFacturaProveedor, #dialogEditarEncabezadoFacturaProveedor").on("click", "#divBuscarOrdenCompra", function() {
        $("#divFormaMuestraDetalleOrdenCompra").obtenerVista({
            nombreTemplate: "tmplAgregarDetalleOrdenCompra.html",
            despuesDeCompilar: function() {
                FiltroDetalleOrdenCompra();
                $("#dialogMuestraDetalleOrdenCompra").dialog("open");
            }
        });
    });

    $(".spanFiltroTotal").click(function() {
        var idEstatusRecepcion = $(this).attr("IdEstatusRecepcion");
        $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado", idEstatusRecepcion);

        $('#gs_NumeroFactura').val(null);
        $('#gs_RazonSocial').val(null);
        $('#gs_Division').val(null);
        $('#gs_AI').val(null);
        $("#chkPorFecha").attr('checked', false);
        $('#txtNumeroSerieBuscador').val(null);
        $('#txtNumeroClaveBuscador').val(null);
        $('#txtNumeroPedidoBuscador').val(null);

        FiltroEncabezadoFacturaProveedor();
    });

    $('#dialogAgregarEncabezadoFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {

        },
        close: function() {
            $("#divFormaAgregarEncabezadoFacturaProveedor").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarEncabezadoFacturaProveedor();
            }
        }
    });

    $('#dialogConsultarEncabezadoFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEncabezadoFacturaProveedor").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarEncabezadoFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {

        },
        close: function() {
            $("#divFormaEditarEncabezadoFacturaProveedor").remove();
        },
        buttons: {
            "Editar": function() {
                EditarEncabezadoFacturaProveedor();
            }
        }
    });

    $('#dialogMuestraDetalleFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Aceptar": function() {
                AgregarPartidasDetalleFacturaProveedor();
            },
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraDetallePedido').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraDetalleOrdenCompra').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogCancelarFacturaProveedor").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Aceptar": function() {
                CancelarFactura();
                $(this).dialog("close");
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogSeleccionarAlmacen').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Aceptar": function() {
                var request = new Object();
                request.pIdAlmacen = $("#cmbAlmacen").val();
                if (request.pIdAlmacen != 0) {
                    ObtenerFormaAgregarEncabezadoFacturaProveedor(JSON.stringify(request));
                    $("#dialogSeleccionarAlmacen").dialog("close");
                }
                else {
                    MostrarMensajeError("Debe de seleccionar un almacén");
                }
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogFacturasPendientesPorValidar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {

        },
        close: function() {
            $("#divFormaFacturasPendientesPorValidar").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConfirmarRevision').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {

        },
        close: function() {
            $("#divFormaConfirmarRevision").remove();
        },
        buttons: {
            "Aceptar": function() {
                SetCambiarEstatusRevisada($("#dialogConfirmarRevision").attr("idEncabezadoFacturaProveedor"));
            },
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

});

function Imprimir(pIdFacturaEncabezado) {
    alert(pIdFacturaEncabezado);
}

function ObtenerFormaFiltrosEncabezadoFacturaProveedor() {
    $("#divFiltrosEncabezadoFacturaProveedor").obtenerVista({
        nombreTemplate: "tmplFiltrosEncabezadoFacturaProveedor.html",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerFormaFiltroEncabezadoFacturaProveedor",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroEncabezadoFacturaProveedor();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroEncabezadoFacturaProveedor();
                    }
                });
            }

            $('#divFiltrosEncabezadoFacturaProveedor').on('click', '#chkPorFecha', function(event) {
                FiltroEncabezadoFacturaProveedor();
            });

            $('#divFiltrosEncabezadoFacturaProveedor').on('focusout', '#txtNumeroSerieBuscador', function (event) {
            	FiltroEncabezadoFacturaProveedor();
            });

            $('#divFiltrosEncabezadoFacturaProveedor').on('focusout', '#txtNumeroClaveBuscador', function (event) {
            	FiltroEncabezadoFacturaProveedor();
            });

            $('#divFiltrosEncabezadoFacturaProveedor').on('focusout', '#txtNumeroPedidoBuscador', function(event) {
                FiltroEncabezadoFacturaProveedor();
            });

            $('#divFiltrosEncabezadoFacturaProveedor').on('change', '#cmbBusquedaDocumento', function(event) {
                FiltroEncabezadoFacturaProveedor();
            });
        }
    });
}

function BuscarNumeroSerie(evento) {
	var key = evento.which || evento.keyCode;
	if ((key == 13)) {
		FiltroEncabezadoFacturaProveedor();
		return false;
	}
}

function BuscarNumeroClave(evento) {
	var key = evento.which || evento.keyCode;
	if ((key == 13)) {
		FiltroEncabezadoFacturaProveedor();
		return false;
	}
}

function BuscarNumeroPedido(evento) {
    var key = evento.which || evento.keyCode;
    if ((key == 13)) {
        FiltroEncabezadoFacturaProveedor();
        return false;
    }
}

//-----------AJAX-----------//
function ObtenerListaSubCuentaContable(pRequest) {
    $("#cmbSubCuentaContable").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerListaSubCuentaContable",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function obtenerComboAlmacenRemision() {
    $("#divAreaBotonesDialog").obtenerVista({
        url: "EncabezadoFacturaProveedor.aspx/LlenaComboAlmacen",
        nombreTemplate: "tmplFiltroComboEncabezadoFacturaProveedor.html",
        efecto: "slide",
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

//-Funciones Obtener Formas-//
function ObtenerFormaAgregarEncabezadoFacturaProveedor(pRequest) {
    MostrarBloqueo();    
    $("#dialogAgregarEncabezadoFacturaProveedor").obtenerVista({
        url: "EncabezadoFacturaProveedor.aspx/ObtenerFormaAgregarEncabezadoFacturaProveedor",
        parametros: pRequest,
        nombreTemplate: "tmplAgregarEncabezadoFacturaProveedor.html",
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarProveedor();
            AutocompletarProductoClave();
            //AutocompletarProductoDescripcion();
            AutocompletarServicioClave();
            //AutocompletarServicioDescripcion();
            //AutocompletarCliente();
            AutocompletarProyecto();
            
            Inicializar_grdDetalleFacturaProveedor();
            $("#txtFechaFactura").datepicker();
            $("#txtFechaPago").datepicker();

            $('#divFormaAgregarEncabezadoFacturaProveedor').on('focusout', '#txtNumeroFactura', function(event) {
                ExisteNumeroFactura();
            });
            
            $("input[name=ProductoServicio]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetos(1);
                } else {
                    MuestraObjetos(0);
                }
                LimpiarDatosDetalleFactura();
            });
            $('#divFormaAgregarEncabezadoFacturaProveedor').on('focusout', '#txtCosto', function(event) {
                RecalculaDatosCosto();
            });
            $('#divFormaAgregarEncabezadoFacturaProveedor').on('focusout', '#txtDescuento', function(event) {
                if (parseInt($(this).val(), 10) > 100) {
                    $(this).val("100");
                }
                RecalculaDatosCosto();
            });
            $('#divFormaAgregarEncabezadoFacturaProveedor').on('focusout', '#txtCostoDescuento', function(event) {
                if (parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())) > parseFloat(QuitaFormatoMoneda($("#txtCosto").val()))) {
                    $(this).val($("#txtCosto").val());
                }
                RecalculaDatosCostoDescuento();
            });
            $('#divFormaAgregarEncabezadoFacturaProveedor').on('focusout', '#txtCantidad', function(event) {
                RecalculaDatosCantidad();
            });
            $('#divFormaAgregarEncabezadoFacturaProveedor').on('change', '#cmbTipoMoneda', function(event) {
                $("#cmbTipoMonedaConcepto").attr("disabled", "true");

                var pTipoCambio = new Object();
                pTipoCambio.IdTipoCambioOrigen = parseInt($("#cmbTipoMoneda").val());
                pTipoCambio.IdTipoCambioDestino = parseInt($("#cmbTipoMonedaConcepto").val());
                var oRequest = new Object();
                oRequest.pTipoCambio = pTipoCambio;
                ObtenerTipoCambioDetalle(JSON.stringify(oRequest))
                RecalculaDatosCantidad();

            });

            $('#divFormaAgregarEncabezadoFacturaProveedor').on('change', '#cmbCondicionPago', function(event) {
                var pCondicionPago = new Object();
                pCondicionPago.IdCondicionPago = parseInt($(this).val());
                pCondicionPago.FechaFactura = $("#txtFechaFactura").val();
                var oRequest = new Object();
                oRequest.pCondicionPago = pCondicionPago;
                ObtenerFechaPago(JSON.stringify(oRequest))
            });

            $("#divFormaAgregarEncabezadoFacturaProveedor").on("click", "#btnAgregarPartidaFacturaProveedor", function() {
                var pEncabezadoFacturaProveedor = new Object();
                if ($("#divFormaAgregarEncabezadoFacturaProveedor").attr("idProducto") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor").attr("idProducto") == null) {
                    pEncabezadoFacturaProveedor.IdProducto = 0;
                }
                else {
                    pEncabezadoFacturaProveedor.IdProducto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto");
                }
                if (pEncabezadoFacturaProveedor.IdProducto == 0) {
                    AgregarDetalleFacturaProveedor();
                }
                else {
                    pEncabezadoFacturaProveedor.Cantidad = parseInt($("#txtCantidad").val(), 10);
                    if (pEncabezadoFacturaProveedor.Cantidad > 1) {
                        if ($("#chkAplicaNumeroSerie").is(':checked')) {
                                MuestraDetalleFacturaProveedor();
                        }
                        else {
                            AgregarDetalleFacturaProveedor();
                        }
                    }
                    else {
                        AgregarDetalleFacturaProveedor();
                    }
                }
            });

            $("input[name=ClienteProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                } else {
                    MuestraObjetosClienteProyecto(0);
                }
            });

            $("#grdDetalleFacturaProveedor").on("click", ".imgEliminarConceptoEditar", function() {

                var registro = $(this).parents("tr");
                var pDetalleFacturaProveedor = new Object();
                pDetalleFacturaProveedor.pIdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleFacturaProveedor_IdDetalleFacturaProveedor']").html());
                var oRequest = new Object();
                oRequest.pDetalleFacturaProveedor = pDetalleFacturaProveedor;
                SetEliminarDetalleFacturaProveedor(JSON.stringify(oRequest));
            });

            $('#divFormaAgregarEncabezadoFacturaProveedor').on('change', '#cmbDivision, #cmbTipoCompra', function(event) {
                var pCuentaContable = new Object();
                pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
                pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
                pCuentaContable.IdSubCuentaContable = parseInt(0);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", "0");
                var oRequest = new Object();
                oRequest.pCuentaContable = pCuentaContable;
                ObtenerCuentaContableGenerada(JSON.stringify(oRequest));
            });

            $('#divFormaAgregarEncabezadoFacturaProveedor').on('change', '#cmbSubCuentaContable', function(event) {
                var pCuentaContable = new Object();
                pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
                pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
                pCuentaContable.IdSubCuentaContable = parseInt($("#cmbSubCuentaContable").val());
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", "0");
                var oRequest = new Object();
                oRequest.pCuentaContable = pCuentaContable;
                ObtenerCuentaContableGeneradaSubCuenta(JSON.stringify(oRequest));
            });

            $("#grdDetalleFacturaProveedor").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var DetalleFacturaProveedor = new Object();

                DetalleFacturaProveedor.pIdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleFacturaProveedor_IdDetalleFacturaProveedor']").html());
                ObtenerDatosDetalleFacturaProveedor(JSON.stringify(DetalleFacturaProveedor));
            });
            
            
            $("#dialogAgregarEncabezadoFacturaProveedor").dialog("open");
        }
    });
}

function ObtenerFormaFacturasPendientesPorValidar() {
    $("#divFormaFacturasPendientesPorValidar").obtenerVista({
        nombreTemplate: "tmplFormaFacturasPendientesPorValidar.html",
        despuesDeCompilar: function() {
            $("#grdFacturasPendientesPorValidar").trigger("reloadGrid");
            $("#dialogFacturasPendientesPorValidar").dialog("open");
        }
    });
}

function ObtenerFormaConfirmarRevision() {
    $("#dialogConfirmarRevision").dialog("open");
}

function ObtenerFormaSeleccionarAlmacen() {
    $("#dialogSeleccionarAlmacen").obtenerVista({
        nombreTemplate: "tmplSeleccionarAlmacen.html",
        url: "EncabezadoFacturaProveedor.aspx/LlenaComboAlmacen",
        despuesDeCompilar: function() {
            $("#dialogSeleccionarAlmacen").dialog("open");
        }
    });
}

function ObtenerDatosDetallePedido(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerDatosDetallePedido",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", respuesta.Modelo.IdCotizacionDetalle);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", 0);
                if (respuesta.Modelo.IdProducto != 0) {
                    MuestraObjetos(1);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", respuesta.Modelo.IdProducto);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", 0);
                    $('input:radio[name=ProductoServicio]')[0].checked = true;
                    $("#txtCantidad").val(respuesta.Modelo.Cantidad);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", respuesta.Modelo.Cantidad);
                    $("#cmbUsuarioSolicitante option[value=" + respuesta.Modelo.IdUsuarioSolicitante + "]").attr("selected", true);
                    $("#txtTotalSinTipoCambio").val(parseFloat(respuesta.Modelo.Cantidad) * parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())));
                    var Producto = new Object();
                    Producto.IdProducto = respuesta.Modelo.IdProducto;
                    obtenerProducto(Producto);                    
                }
                else {
                    MuestraObjetos(0);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", respuesta.Modelo.IdServicio);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", 0);
                    $('input:radio[name=ProductoServicio]')[1].checked = true;
                    $("#txtCantidad").val(respuesta.Modelo.Cantidad);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", respuesta.Modelo.Cantidad);
                    $("#cmbUsuarioSolicitante option[value=" + respuesta.Modelo.IdUsuarioSolicitante + "]").attr("selected", true);
                    $("#txtTotalSinTipoCambio").val(parseFloat(respuesta.Modelo.Cantidad) * parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())));
                    var Servicio = new Object();
                    Servicio.IdServicio = respuesta.Modelo.IdServicio;
                    obtenerServicio(Servicio);                    
                }
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerDatosDetalleFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerDatosDetalleFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idDetalleFacturaProveedor", respuesta.Modelo.IdDetalleFacturaProveedor);
                if (respuesta.Modelo.IdOrdenCompraDetalle != 0) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", respuesta.Modelo.IdOrdenCompraDetalle);
                }
                if (respuesta.Modelo.IdCotizacionDetalle != 0) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", respuesta.Modelo.IdCotizacionDetalle);
                }
                $("#txtNumeroSerie").val(respuesta.Modelo.NumeroSerie);

                if (respuesta.Modelo.IdCliente != 0) {
                    $('input:radio[name=ClienteProyecto]')[0].checked = true;
                    $("#txtCliente").val(respuesta.Modelo.RazonSocial);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", respuesta.Modelo.IdCliente);
                    var Cliente = new Object();
                    Cliente.IdCliente = respuesta.Modelo.IdCliente;
                    obtenerPedidosClienteOrdenCompra(JSON.stringify(Cliente), respuesta.Modelo.IdCotizacion);
                }
                else {
                    $('input:radio[name=ClienteProyecto]')[1].checked = true;
                    $("#txtProyecto").val(respuesta.Modelo.Proyecto);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", respuesta.Modelo.IdProyecto);
                }
                if (respuesta.Modelo.IdProducto != 0) {
                    MuestraObjetos(1);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", respuesta.Modelo.IdProducto);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", 0);
                    $('input:radio[name=ProductoServicio]')[0].checked = true;
                    $("#txtCantidad").val(respuesta.Modelo.Cantidad);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", respuesta.Modelo.Cantidad);
                    $("#cmbUsuarioSolicitante option[value=" + respuesta.Modelo.IdUsuarioSolicitante + "]").attr("selected", true);
                    $("#cmbTipoCompra option[value=" + respuesta.Modelo.IdTipoCompra + "]").attr("selected", true);
                    var Producto = new Object();
                    Producto.IdProducto = respuesta.Modelo.IdProducto;
                    obtenerProducto(Producto);

                    var pCuentaContable = new Object();
                    pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
                    pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
                    pCuentaContable.IdSubCuentaContable = parseInt(0);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", "0");
                    var oRequest = new Object();
                    oRequest.pCuentaContable = pCuentaContable;
                    ObtenerCuentaContableGenerada(JSON.stringify(oRequest));        
                    
                }
                else {
                    MuestraObjetos(1);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", respuesta.Modelo.IdServicio);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", 0);
                    $('input:radio[name=ProductoServicio]')[1].checked = true;
                    $("#txtCantidad").val(respuesta.Modelo.Cantidad);
                    
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", respuesta.Modelo.Cantidad);
                    
                    $("#cmbUsuarioSolicitante option[value=" + respuesta.Modelo.IdUsuarioSolicitante + "]").attr("selected", true);
                    $("#cmbTipoCompra option[value=" + respuesta.Modelo.IdTipoCompra + "]").attr("selected", true);
                    var Servicio = new Object();
                    Servicio.IdServicio = respuesta.Modelo.IdServicio;
                    obtenerServicio(Servicio);
                }

                var registro = $(this).parents("tr");
                var pDetalleFacturaProveedor = new Object();
                pDetalleFacturaProveedor.pIdDetalleFacturaProveedor = parseInt(respuesta.Modelo.IdDetalleFacturaProveedor);
                var oRequest = new Object();
                oRequest.pDetalleFacturaProveedor = pDetalleFacturaProveedor;
                SetEliminarDetalleFacturaProveedor(JSON.stringify(oRequest));

            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerDatosDetalleOrdenCompra(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerDatosDetalleOrdenCompra",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", 0);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", respuesta.Modelo.IdOrdenCompraDetalle);
                if (respuesta.Modelo.IdProducto != 0) {
                    MuestraObjetos(1);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", respuesta.Modelo.IdProducto);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", 0);
                    $('input:radio[name=ProductoServicio]')[0].checked = true;
                    $("#txtCantidad").val(respuesta.Modelo.Cantidad);
                    $("#txtCosto").val(formato.moneda(respuesta.Modelo.Costo, '$'));
                    $("#txtDescuento").val(0);
                    $("#txtCostoDescuento").val(formato.moneda(respuesta.Modelo.Costo, '$'));
                    $("#cmbTipoMonedaConcepto option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", respuesta.Modelo.Cantidad);
                    $("#txtTotalSinTipoCambio").val(parseFloat(respuesta.Modelo.Cantidad) * parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())));
                    $("#cmbUsuarioSolicitante option[value=" + respuesta.Modelo.IdUsuarioSolicitante + "]").attr("selected", true);
                    $("#cmbTipoCompra option[value=" + respuesta.Modelo.IdTipoCompra + "]").attr("selected", true);

                    if (respuesta.Modelo.IdCliente != 0) {

                        $('input:radio[name=ClienteProyecto]')[0].checked = true;
                        $("#txtCliente").val(respuesta.Modelo.RazonSocial);
                        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", respuesta.Modelo.IdCliente);
                        var Cliente = new Object();
                        Cliente.IdCliente = respuesta.Modelo.IdCliente;
                        obtenerPedidosClienteOrdenCompra(JSON.stringify(Cliente), respuesta.Modelo.IdCotizacion);
                        $("#txtCliente").show();
                        $("#txtProyecto").hide();
                    }
                    else {
                        $('input:radio[name=ClienteProyecto]')[1].checked = true;
                        $("#txtProyecto").val(respuesta.Modelo.Proyecto);
                        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", respuesta.Modelo.IdProyecto);
                        $("#txtCliente").hide();
                        $("#txtProyecto").show();
                    }



                    var Producto = new Object();
                    Producto.IdProducto = respuesta.Modelo.IdProducto;
                    obtenerProducto(Producto);
                }
                else {
                    MuestraObjetos(0);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", respuesta.Modelo.IdServicio);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", 0);
                    $('input:radio[name=ProductoServicio]')[1].checked = true;
                    $("#txtCantidad").val(respuesta.Modelo.Cantidad);
                    $("#txtCosto").val(formato.moneda(respuesta.Modelo.Costo, '$'));
                    $("#txtDescuento").val(0);
                    $("#txtCostoDescuento").val(formato.moneda(respuesta.Modelo.Costo, '$'));
                    $("#cmbTipoMonedaConcepto option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", respuesta.Modelo.Cantidad);
                    $("#txtTotalSinTipoCambio").val(parseFloat(respuesta.Modelo.Cantidad) * parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())));
                    $("#cmbUsuarioSolicitante option[value=" + respuesta.Modelo.IdUsuarioSolicitante + "]").attr("selected", true);

                    if (respuesta.Modelo.IdCliente != 0) {

                        $('input:radio[name=ClienteProyecto]')[0].checked = true;
                        $("#txtCliente").val(respuesta.Modelo.RazonSocial);
                        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", respuesta.Modelo.IdCliente);

                        var Cliente = new Object();
                        Cliente.IdCliente = respuesta.Modelo.IdCliente;
                        obtenerPedidosClienteOrdenCompra(JSON.stringify(Cliente), respuesta.Modelo.IdCotizacion);
                    }
                    else {
                        $('input:radio[name=ClienteProyecto]')[1].checked = true;
                        $("#txtProyecto").val(respuesta.Modelo.Proyecto);
                        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", respuesta.Modelo.IdProyecto);
                    }

                    var Servicio = new Object();
                    Servicio.IdServicio = respuesta.Modelo.IdServicio;
                    obtenerServicio(Servicio);
                }

            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function MuestraDetalleFacturaProveedor() {
    $("#divFormaMuestraDetalleFacturaProveedorF").obtenerVista({
        nombreTemplate: "tmplAgregarDetalleFacturaProveedor.html",
        despuesDeCompilar: function() {
            FiltroProductoNumeroSerie();
            $("#dialogMuestraDetalleFacturaProveedor").dialog("open");
        }
    });
}


function ObtenerFormaConsultarEncabezadoFacturaProveedor(pIdEncabezadoFacturaProveedor) {
    $("#dialogConsultarEncabezadoFacturaProveedor").obtenerVista({
        nombreTemplate: "tmplConsultarEncabezadoFacturaProveedor.html",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerFormaEncabezadoFacturaProveedor",
        parametros: pIdEncabezadoFacturaProveedor,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdDetalleFacturaProveedorConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarEncabezadoFacturaProveedor == 1) {
                var IdEstatus = 0;
                IdEstatus = parseInt($("#divFormaConsultarEncabezadoFacturaProveedor").attr("idEstatus"));
                if (IdEstatus == 4) {
                    $("#dialogConsultarEncabezadoFacturaProveedor").dialog("option", "buttons", {
                        "Editar": function() {                          
                            $(this).dialog("close");
                            var EncabezadoFacturaProveedor = new Object();
                            EncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = parseInt($("#divFormaConsultarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor"));
                            ObtenerFormaEditarEncabezadoFacturaProveedor(JSON.stringify(EncabezadoFacturaProveedor))
                        },
                        "Salir": function() {
                            $(this).dialog("close");
                        }
                    });
                }
                else {
                    $("#dialogConsultarEncabezadoFacturaProveedor").dialog("option", "buttons", {                        
                        "Salir": function() {
                            $(this).dialog("close");
                        }
                    });
                }


                $("#dialogConsultarEncabezadoFacturaProveedor").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarEncabezadoFacturaProveedor").dialog("option", "buttons", {});
                $("#dialogConsultarEncabezadoFacturaProveedor").dialog("option", "height", "auto");
            }
            $("#dialogConsultarEncabezadoFacturaProveedor").dialog("open");
        }
    });
}

function ObtenerFormaEditarEncabezadoFacturaProveedor(IdEncabezadoFacturaProveedor) {
    $("#dialogEditarEncabezadoFacturaProveedor").obtenerVista({
        nombreTemplate: "tmplEditarEncabezadoFacturaProveedor.html",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerFormaEditarEncabezadoFacturaProveedor",
        parametros: IdEncabezadoFacturaProveedor,
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarProveedor();
            AutocompletarProductoClave();
            AutocompletarProductoDescripcion();
            AutocompletarServicioClave();
            AutocompletarServicioDescripcion();
            //AutocompletarCliente();
            AutocompletarProyecto();
            Inicializar_grdDetalleFacturaProveedorEditar();
            $("#txtFechaFactura").datepicker();
            $("#txtFechaPago").datepicker();
            DeshabilitaCamposEncabezado();
            var Proveedor = new Object();
            Proveedor.IdProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor");
            obtenerOrdenCompraProveedor(JSON.stringify(Proveedor));

            $('#divFormaEditarEncabezadoFacturaProveedor').on('focusout', '#txtNumeroFactura', function(event) {
                ExisteNumeroFactura();
            });
            $("input[name=ProductoServicio]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetos(1);
                } else {
                    MuestraObjetos(0);
                }
                LimpiarDatosDetalleFactura();
            });
            $('#divFormaEditarEncabezadoFacturaProveedor').on('focusout', '#txtCosto', function(event) {
                RecalculaDatosCosto();
            });
            $('#divFormaEditarEncabezadoFacturaProveedor').on('focusout', '#txtDescuento', function(event) {
                if (parseInt($(this).val(), 10) > 100) {
                    $(this).val("100");
                }
                RecalculaDatosCosto();
            });
            $('#divFormaEditarEncabezadoFacturaProveedor').on('focusout', '#txtCostoDescuento', function(event) {
                if (parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())) > parseFloat(QuitaFormatoMoneda($("#txtCosto").val()))) {
                    $(this).val($("#txtCosto").val());
                }
                RecalculaDatosCostoDescuento();
            });
            $('#divFormaEditarEncabezadoFacturaProveedor').on('focusout', '#txtCantidad', function(event) {
                RecalculaDatosCantidad();
            });
            $('#divFormaEditarEncabezadoFacturaProveedor').on('change', '#cmbTipoMoneda', function(event) {
                $("#cmbTipoMonedaConcepto option[value=" + $(this).val() + "]").attr("selected", true);
            });

            $("#divFormaEditarEncabezadoFacturaProveedor").on("click", "#btnEditarPartidaFacturaProveedor", function() {
                var pEncabezadoFacturaProveedor = new Object();
                if ($("#divFormaEditarEncabezadoFacturaProveedor").attr("idProducto") == "" || $("#divFormaEditarEncabezadoFacturaProveedor").attr("idProducto") == null) {
                    pEncabezadoFacturaProveedor.IdProducto = 0;
                }
                else {
                    pEncabezadoFacturaProveedor.IdProducto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto");
                }
                if (pEncabezadoFacturaProveedor.IdProducto == 0) {
                    AgregarDetalleFacturaProveedor();
                }
                else {
                    pEncabezadoFacturaProveedor.Cantidad = $("#txtCantidad").val();
                    if (pEncabezadoFacturaProveedor.Cantidad > 1) {
                        if ($("#chkAplicaNumeroSerie").is(':checked')) {
                            MuestraDetalleFacturaProveedor();
                        }
                        else {
                            AgregarDetalleFacturaProveedor();
                        }
                    }
                    else {
                        AgregarDetalleFacturaProveedor();
                    }
                }
            });

            $('#divFormaEditarEncabezadoFacturaProveedor').on('change', '#cmbCondicionPago', function(event) {
                var pCondicionPago = new Object();
                pCondicionPago.IdCondicionPago = parseInt($(this).val());
                pCondicionPago.FechaFactura = $("#txtFechaFactura").val();
                var oRequest = new Object();
                oRequest.pCondicionPago = pCondicionPago;
                ObtenerFechaPago(JSON.stringify(oRequest))
            });


            $("input[name=ClienteProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                } else {
                    MuestraObjetosClienteProyecto(0);
                }
            });

            $("#grdDetalleFacturaProveedorEditar").on("click", ".imgEliminarConceptoEditar", function() {

                var registro = $(this).parents("tr");
                var pDetalleFacturaProveedor = new Object();
                pDetalleFacturaProveedor.pIdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleFacturaProveedorEditar_IdDetalleFacturaProveedor']").html());
                var oRequest = new Object();
                oRequest.pDetalleFacturaProveedor = pDetalleFacturaProveedor;
                SetEliminarDetalleFacturaProveedor(JSON.stringify(oRequest));
            });

            $('#divFormaEditarEncabezadoFacturaProveedor').on('change', '#cmbDivision, #cmbTipoCompra', function(event) {
                var pCuentaContable = new Object();
                pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
                pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
                pCuentaContable.IdSubCuentaContable = parseInt(0);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", "0");
                var oRequest = new Object();
                oRequest.pCuentaContable = pCuentaContable;
                ObtenerCuentaContableGenerada(JSON.stringify(oRequest));
            });

            $('#divFormaEditarEncabezadoFacturaProveedor').on('change', '#cmbSubCuentaContable', function(event) {
                var pCuentaContable = new Object();
                pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
                pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
                pCuentaContable.IdSubCuentaContable = parseInt($("#cmbSubCuentaContable").val());
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", "0");
                var oRequest = new Object();
                oRequest.pCuentaContable = pCuentaContable;
                ObtenerCuentaContableGeneradaSubCuenta(JSON.stringify(oRequest));
            });

            $("#grdDetalleFacturaProveedorEditar").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var DetalleFacturaProveedor = new Object();

                DetalleFacturaProveedor.pIdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleFacturaProveedorEditar_IdDetalleFacturaProveedor']").html());
                ObtenerDatosDetalleFacturaProveedor(JSON.stringify(DetalleFacturaProveedor));
            });
            $("#dialogEditarEncabezadoFacturaProveedor").dialog("open");
        }
    });
}

function DeshabilitaCamposEncabezado() {
    $("#txtRazonSocial").attr("disabled", "true");
    $("#cmbDivision").attr("disabled", "true");
    $("#cmbTipoMoneda").attr("disabled", "true");
    $("#txtFechaFactura").attr("disabled", "true");
    $("#txtFechaPago").attr("disabled", "true");
    $("#txtNumeroGuia").attr("disabled", "true");
}
function LimpiarDatosDetalleFactura() {
    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA", "0");
    $("#txtCosto").val("$0.00");
    $("#txtClaveProducto").val("");
    $("#txtClaveServicio").val("");
    $("#txtDescripcionProducto").val("");
    $("#txtDescripcionServicio").val("");
    $("#txtDescuento").val("0");
    $("#txtCostoDescuento").val("$0.00");
    $("#txtCantidad").val("1");
    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido", "0");
    $("#txtTotalSinTipoCambio").val("$0.00");
    $("#txtTotalConTipoCambio").val("$0.00");
    $("#txtNumeroSerie").val("");
    $("#txtIVA").val("");
    $("#cmbUnidadCompraVenta option[value=0]").attr("selected", true);
    $("#txtTipoCambioDetalle").val("");
    $('input[name=ClienteProyecto]').attr("disabled", true);    
}


function MuestraObjetos(opcion) {
    if (opcion == 1) {
        $("#txtClaveProducto").css("display", "block");
        $("#txtDescripcionProducto").css("display", "block");
        $("#txtClaveServicio").css("display", "none");
        $("#txtDescripcionServicio").css("display", "none");
        $("#txtClaveServicio").val("");
        $("#txtDescripcionServicio").val("");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
    }
    else {
        $("#txtClaveProducto").css("display", "none");
        $("#txtDescripcionProducto").css("display", "none");
        $("#txtClaveServicio").css("display", "block");
        $("#txtDescripcionServicio").css("display", "block");
        $("#txtClaveProducto").val("");
        $("#txtDescripcionProducto").val("");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
    }
}

function MuestraObjetosClienteProyecto(opcion) {
    if (opcion == 1) {
        $("#txtCliente").css("display", "block");
        $("#txtProyecto").css("display", "none");
        $("#txtProyecto").val("");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", "0");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", "0");
        $("#cmbTipoMonedaConcepto").attr("disabled", "readonly");
        $("#txtTipoCambioDetalle").attr("readonly","readonly");
    }
    else {
        $("#txtCliente").css("display", "none");
        $("#txtProyecto").css("display", "block");
        $("#txtCliente").val("");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", "0");
        $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", "0");

        AutocompletarProductoClave();
        AutocompletarProductoDescripcion();
        AutocompletarServicioClave();
        AutocompletarServicioDescripcion();
        $("#cmbTipoMonedaConcepto").removeAttr("disabled");
        $("#txtTipoCambioDetalle").removeAttr("readonly");
    }
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarEncabezadoFacturaProveedor() {
    var pEncabezadoFacturaProveedor = new Object();

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == null) {
        if ($("#divFormaAgregarEncabezadoFacturaProveedor").attr("idProveedor") == "") {
            pEncabezadoFacturaProveedor.IdProveedor = 0;
        }
        else {
            pEncabezadoFacturaProveedor.IdProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor").attr("idProveedor");
        }
        pEncabezadoFacturaProveedor.NumeroFactura = $("#txtNumeroFactura").val();
        pEncabezadoFacturaProveedor.IdDivision = $("#cmbDivision").val();
        pEncabezadoFacturaProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
        pEncabezadoFacturaProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pEncabezadoFacturaProveedor.FechaFactura = $("#txtFechaFactura").val();
        pEncabezadoFacturaProveedor.FechaPago = $("#txtFechaPago").val();
        pEncabezadoFacturaProveedor.NumeroGuia = $("#txtNumeroGuia").val();
        pEncabezadoFacturaProveedor.TipoCambioFactura = $("#txtTipoCambioFactura").val();
        pEncabezadoFacturaProveedor.IdAlmacen = $("#divFormaAgregarEncabezadoFacturaProveedor").attr("idAlmacen");

        var validacion = ValidaEncabezadoFacturaProveedor(pEncabezadoFacturaProveedor);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        //ExisteNumeroFactura();
        var oRequest = new Object();
        oRequest.pEncabezadoFacturaProveedor = pEncabezadoFacturaProveedor;
        SetAgregarEncabezadoFacturaProveedor(JSON.stringify(oRequest));
    }
    else {
        $("#dialogAgregarEncabezadoFacturaProveedor").dialog("close");
    }  
    
}

function AgregarDetalleFacturaProveedor() {

    var pEncabezadoFacturaProveedor = new Object();
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == null) {
        pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor") == null) {
        pEncabezadoFacturaProveedor.IdProveedor = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor");
    }
    pEncabezadoFacturaProveedor.NumeroFactura = $("#txtNumeroFactura").val();
    pEncabezadoFacturaProveedor.IdDivision = $("#cmbDivision").val();
    pEncabezadoFacturaProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
    pEncabezadoFacturaProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pEncabezadoFacturaProveedor.FechaFactura = $("#txtFechaFactura").val();
    pEncabezadoFacturaProveedor.FechaPago = $("#txtFechaPago").val();
    pEncabezadoFacturaProveedor.NumeroGuia = $("#txtNumeroGuia").val();
    pEncabezadoFacturaProveedor.TipoCambioFactura = $("#txtTipoCambioFactura").val();
    pEncabezadoFacturaProveedor.TipoCambioDetalle = $("#txtTipoCambioDetalle").val();

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto") == null) {
        pEncabezadoFacturaProveedor.IdProducto = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProducto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio") == null) {
        pEncabezadoFacturaProveedor.IdServicio = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdServicio = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle") == null) {
        pEncabezadoFacturaProveedor.IdCotizacionDetalle = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdCotizacionDetalle = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle");
    }
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle") == null) {
        pEncabezadoFacturaProveedor.IdOrdenCompraDetalle = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdOrdenCompraDetalle = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido") == null) {
        pEncabezadoFacturaProveedor.CantidadPedido = 0;
    }
    else {
        pEncabezadoFacturaProveedor.CantidadPedido = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("cantidadPedido");
    }

    pEncabezadoFacturaProveedor.Costo = parseFloat(QuitaFormatoMoneda($("#txtCosto").val()));
    pEncabezadoFacturaProveedor.Descuento = QuitaFormatoMoneda($("#txtDescuento").val());
    pEncabezadoFacturaProveedor.CostoDescuento = QuitaFormatoMoneda($("#txtCostoDescuento").val());
    pEncabezadoFacturaProveedor.Cantidad = $("#txtCantidad").val();
    pEncabezadoFacturaProveedor.Total = QuitaFormatoMoneda($("#txtTotalSinTipoCambio").val());
    pEncabezadoFacturaProveedor.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pEncabezadoFacturaProveedor.IdTipoCompra = $("#cmbTipoCompra").val();
    pEncabezadoFacturaProveedor.IdTipoMonedaConcepto = $("#cmbTipoMonedaConcepto").val();
    pEncabezadoFacturaProveedor.IdUsuarioSolicito = $("#cmbUsuarioSolicitante").val();
    pEncabezadoFacturaProveedor.IdAlmacen = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idAlmacen");
    pEncabezadoFacturaProveedor.IdSubCuentaContable = $("#cmbSubCuentaContable").val();

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente") == null) {
        pEncabezadoFacturaProveedor.IdCliente = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdCliente = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto") == null) {
        pEncabezadoFacturaProveedor.IdProyecto = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProyecto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto");
    }

    pEncabezadoFacturaProveedor.NumeroSerie = $("#txtNumeroSerie").val();

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA") == null) {
        pEncabezadoFacturaProveedor.IdTipoIVA = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdTipoIVA = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA");
    }
    pEncabezadoFacturaProveedor.IVA = $("#txtIVA").val();
    pEncabezadoFacturaProveedor.CuentaContable = $("#txtCuentaContable").val();
    
    var validacion = ValidaDetalleFacturaProveedor(pEncabezadoFacturaProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    
    var oRequest = new Object();
    oRequest.pEncabezadoFacturaProveedor = pEncabezadoFacturaProveedor;
    SetAgregarDetalleFacturaProveedor(JSON.stringify(oRequest));
}

function AgregarPartidasDetalleFacturaProveedor() {
    var pEncabezadoFacturaProveedor = new Object();
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == null) {
        pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor") == null) {
        pEncabezadoFacturaProveedor.IdProveedor = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor");
    }
    pEncabezadoFacturaProveedor.NumeroFactura = $("#txtNumeroFactura").val();
    pEncabezadoFacturaProveedor.IdDivision = $("#cmbDivision").val();
    pEncabezadoFacturaProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
    pEncabezadoFacturaProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pEncabezadoFacturaProveedor.FechaFactura = $("#txtFechaFactura").val();
    pEncabezadoFacturaProveedor.FechaPago = $("#txtFechaPago").val();
    pEncabezadoFacturaProveedor.NumeroGuia = $("#txtNumeroGuia").val();
    pEncabezadoFacturaProveedor.TipoCambioFactura = $("#txtTipoCambioFactura").val();
    pEncabezadoFacturaProveedor.TipoCambioDetalle = $("#txtTipoCambioDetalle").val();
    
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto") == null) {
        pEncabezadoFacturaProveedor.IdProducto = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProducto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio") == null) {
        pEncabezadoFacturaProveedor.IdServicio = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdServicio = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle") == null) {
        pEncabezadoFacturaProveedor.IdCotizacionDetalle = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdCotizacionDetalle = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle");
    }
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle") == null) {
        pEncabezadoFacturaProveedor.IdOrdenCompraDetalle = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdOrdenCompraDetalle = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle");
    }

    pEncabezadoFacturaProveedor.Costo = parseFloat(QuitaFormatoMoneda($("#txtCosto").val()) / parseFloat(pEncabezadoFacturaProveedor.TipoCambioDetalle));
    pEncabezadoFacturaProveedor.Descuento = QuitaFormatoMoneda($("#txtDescuento").val());
    pEncabezadoFacturaProveedor.CostoDescuento = QuitaFormatoMoneda($("#txtCostoDescuento").val());
    pEncabezadoFacturaProveedor.Cantidad = $("#txtCantidad").val();
    pEncabezadoFacturaProveedor.Total = QuitaFormatoMoneda($("#txtTotalConTipoCambio").val());
    pEncabezadoFacturaProveedor.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pEncabezadoFacturaProveedor.IdTipoCompra = $("#cmbTipoCompra").val();
    pEncabezadoFacturaProveedor.IdTipoMonedaConcepto = $("#cmbTipoMonedaConcepto").val();
    pEncabezadoFacturaProveedor.IdUsuarioSolicito = $("#cmbUsuarioSolicitante").val();
    pEncabezadoFacturaProveedor.IdAlmacen = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idAlmacen");
    pEncabezadoFacturaProveedor.IdSubCuentaContable = $("#cmbSubCuentaContable").val();

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente") == null) {
        pEncabezadoFacturaProveedor.IdCliente = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdCliente = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente");
    }

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto") == null) {
        pEncabezadoFacturaProveedor.IdProyecto = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProyecto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto");
    }

    pEncabezadoFacturaProveedor.NumeroSerie = $("#txtNumeroSerie").val();

    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA") == null) {
        pEncabezadoFacturaProveedor.IdTipoIVA = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdTipoIVA = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA");
    }
    pEncabezadoFacturaProveedor.IVA = $("#txtIVA").val();
    pEncabezadoFacturaProveedor.CuentaContable = $("#txtCuentaContable").val();

    var validacion = ValidaDetalleFacturaProveedor(pEncabezadoFacturaProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }

    var NumeroSerieVacio = 0;
    pEncabezadoFacturaProveedor.DetallePartidas = new Array();
    $(".chkElegir:checked").each(function(index, object) {
        var registro = $(this).parents("tr");
        var pDetalle = new Object();
        pDetalle.IdProducto = $(registro).children("td[aria-describedby='grdProductoNumeroSerie_IdProducto']").text();
        pDetalle.Precio = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdProductoNumeroSerie_Precio']").text());
        pDetalle.Descuento = $(registro).children("td[aria-describedby='grdProductoNumeroSerie_Descuento']").text().replace("%", "");
        pDetalle.Total = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdProductoNumeroSerie_Total']").text());
        pDetalle.NumeroSerie = $(registro).children("td[aria-describedby='grdProductoNumeroSerie_NumeroSerie']").text();
        if (pDetalle.NumeroSerie.trim() == "") {
            NumeroSerieVacio = 1;
        }
        pEncabezadoFacturaProveedor.DetallePartidas.push(pDetalle);
    });
    if (NumeroSerieVacio == 1) {
        MostrarMensajeError("Existen productos con numeros de series vacios");
    }

    var oRequest = new Object();
    oRequest.pEncabezadoFacturaProveedor = pEncabezadoFacturaProveedor;
    SetAgregarPartidasDetalleFacturaProveedor(JSON.stringify(oRequest));
}

function CancelarFactura() {
    var pEncabezadoFacturaProveedor = new Object();
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == "" || $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor") == null) {
        pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = 0;
        MostrarMensajeError("Debe seleccionar una factura para poder cancelarla");
        return false;
    }
    else {
        pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor");
        var oRequest = new Object();
        oRequest.pEncabezadoFacturaProveedor = pEncabezadoFacturaProveedor;
        SetCancelarFacturaProveedor(JSON.stringify(oRequest));
    }
}

function SetCancelarFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/CancelarFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {

                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
                $("#dialogEditarEncabezadoFacturaProveedor").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetAgregarPartidasDetalleFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/AgregarPartidasDetalleFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor", respuesta.IdEncabezadoFacturaProveedor);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor", respuesta.IdEncabezadoFacturaProveedor);
                $("#txtSubtotalFactura").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtIVAFactura").text(formato.moneda(parseFloat(respuesta.TotalIVA), "$"));
                $("#txtTotalFactura").text(formato.moneda(parseFloat(respuesta.SubtotalFactura) + (parseFloat(respuesta.TotalIVA)), "$"));
                $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalFactura").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                $("#txtSubtotalFacturaEditar").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtIVAFacturaEditar").text(formato.moneda(parseFloat(respuesta.TotalIVA), "$"));
                $("#txtTotalFacturaEditar").text(formato.moneda(parseFloat(respuesta.SubtotalFactura) + (parseFloat(respuesta.TotalIVA)), "$"));
                $("#spanCantidadLetraEditar").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalFacturaEditar").text()).toString(), $("#cmbTipoMoneda option:selected").text()));
                
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
                $("#grdDetalleFacturaProveedor").trigger("reloadGrid");
                $("#grdDetalleFacturaProveedorConsultar").trigger("reloadGrid");
                $("#grdDetalleFacturaProveedorEditar").trigger("reloadGrid");
                LimpiarDatosDetalleFactura();
                DeshabilitaCamposEncabezado();
                $("#dialogMuestraDetalleFacturaProveedor").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var TotalTipoCambio;
                var TipoCambioFactura;
                TipoCambioFactura = $("#txtTipoCambioFactura").val();
                if (TipoCambioFactura == 0) {
                    TipoCambioFactura = 1;
                }
                if (respuesta.Modelo.TipoCambioActual == 0) {
                    respuesta.Modelo.TipoCambioActual = 1;
                }
                $("#txtTipoCambioFactura").val(respuesta.Modelo.TipoCambioActual);
                TotalTipoCambio = ($("#txtTotalSinTipoCambio").val() / $("#txtTipoCambioDetalle").val());
                $("#txtTotalConTipoCambio").val(TotalTipoCambio.toFixed(2)); 
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerFechaPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerFechaPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtFechaPago").val(respuesta.Modelo.FechaPago);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerTipoCambioDetalle(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtTipoCambioDetalle").val(respuesta.Modelo.TipoCambioActual);
                $("#txtTipoCambioFactura").val(respuesta.Modelo.TipoCambioFactura);
                RecalculaDatosCantidad();
                
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerCuentaContableGenerada(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerCuentaContableGenerada",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtCuentaContable").val(respuesta.Modelo.CuentaContable);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", respuesta.Modelo.IdCuentaContable);
                var request = new Object();
                request.pIdCuentaContable = respuesta.Modelo.IdCuentaContable;
                request.pIdSucursal = respuesta.Modelo.IdSucursal;
                request.pIdDivision = respuesta.Modelo.IdDivision;
                request.pIdTipoCompra = respuesta.Modelo.IdTipoCompra;
                ObtenerListaSubCuentaContable(JSON.stringify(request));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerCuentaContableGeneradaSubCuenta(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerCuentaContableGenerada",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtCuentaContable").val(respuesta.Modelo.CuentaContable);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdCuentaContable", respuesta.Modelo.IdCuentaContable);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetAgregarEncabezadoFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/AgregarEncabezadoFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarEncabezadoFacturaProveedor").dialog("close");
        }
    });
}

function SetAgregarDetalleFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/AgregarDetalleFacturaProveedorNormal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor", respuesta.IdEncabezadoFacturaProveedor);
                $("#txtSubtotalFactura").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtIVAFactura").text(formato.moneda(parseFloat(respuesta.TotalIVA), "$"));
                $("#txtTotalFactura").text(formato.moneda(parseFloat(respuesta.SubtotalFactura) + (parseFloat(respuesta.TotalIVA)), "$"));
                $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalFactura").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                $("#txtSubtotalFacturaEditar").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtIVAFacturaEditar").text(formato.moneda(parseFloat(respuesta.TotalIVA), "$"));
                $("#txtTotalFacturaEditar").text(formato.moneda(parseFloat(respuesta.SubtotalFactura) + (parseFloat(respuesta.TotalIVA)), "$"));
                $("#spanCantidadLetraEditar").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalFacturaEditar").text()).toString(), $("#cmbTipoMoneda option:selected").text()));
                
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
                $("#grdDetalleFacturaProveedor").trigger("reloadGrid");
                $("#grdDetalleFacturaProveedorConsultar").trigger("reloadGrid");
                $("#grdDetalleFacturaProveedorEditar").trigger("reloadGrid");
                LimpiarDatosDetalleFactura();
                DeshabilitaCamposEncabezado();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetCambiarEstatus(pIdEncabezadoFacturaProveedor, pBaja) {
    var pRequest = "{'pIdEncabezadoFacturaProveedor':" + pIdEncabezadoFacturaProveedor + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {

            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }

        },
        complete: function() {
            $('#grdEncabezadoFacturaProveedor').one('click', '.div_grdEncabezadoFacturaProveedor_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_AI']").children().attr("baja")
                var idEncabezadoFacturaProveedor = $(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_IdEncabezadoFacturaProveedor']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                if (baja == "true") {
                    SetCambiarEstatus(idEncabezadoFacturaProveedor, baja);
                }
                else {
                    SetActivarEstatus(idEncabezadoFacturaProveedor, baja);
                }
            });
        }
    });
}

function SetCambiarEstatusRevisada(pIdEncabezadoFacturaProveedor) {
    var pRequest = "{'pIdEncabezadoFacturaProveedor':" + pIdEncabezadoFacturaProveedor + "}";
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/CambiarEstatusRevisada",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {

            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogConfirmarRevision").dialog("close");
                $("#grdFacturasPendientesPorValidar").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }

        },
        complete: function() {
            $('#grdFacturasPendientesPorValidar').one('click', '.div_grdFacturasPendientesPorValidar_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdFacturasPendientesPorValidar_AI']").children().attr("baja")
                var idEncabezadoFacturaProveedor = $(registro).children("td[aria-describedby='grdFacturasPendientesPorValidar_IdEncabezadoFacturaProveedor']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                if (baja == "true") {
                    SetCambiarEstatus(idEncabezadoFacturaProveedor, baja);
                }
                else {
                    SetActivarEstatus(idEncabezadoFacturaProveedor, baja);
                }
            });
        }
    });
}

function SetActivarEstatus(pIdEncabezadoFacturaProveedor, pBaja) {
    var pRequest = "{'pIdEncabezadoFacturaProveedor':" + pIdEncabezadoFacturaProveedor + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ActivarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {

            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }

        },
        complete: function() {
            $('#grdEncabezadoFacturaProveedor').one('click', '.div_grdEncabezadoFacturaProveedor_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_AI']").children().attr("baja")
                var idEncabezadoFacturaProveedor = $(registro).children("td[aria-describedby='grdEncabezadoFacturaProveedor_IdEncabezadoFacturaProveedor']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                if (baja == "true") {
                    SetCambiarEstatus(idEncabezadoFacturaProveedor, baja);
                }
                else {
                    SetActivarEstatus(idEncabezadoFacturaProveedor, baja);
                }
            });
        }
    });
}

function EditarEncabezadoFacturaProveedor() {
    var pEncabezadoFacturaProveedor = new Object();
    pEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = $("#divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor");
    if ($("#divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor") == "") {
        pEncabezadoFacturaProveedor.IdProveedor = 0;
    }
    else {
        pEncabezadoFacturaProveedor.IdProveedor = $("#divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor");
    }
    
    pEncabezadoFacturaProveedor.NumeroFactura = $("#txtNumeroFactura").val();
    pEncabezadoFacturaProveedor.IdDivision = $("#cmbDivision").val();
    pEncabezadoFacturaProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
    pEncabezadoFacturaProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pEncabezadoFacturaProveedor.FechaFactura = $("#txtFechaFactura").val();
    pEncabezadoFacturaProveedor.FechaPago = $("#txtFechaPago").val();
    pEncabezadoFacturaProveedor.NumeroGuia = $("#txtNumeroGuia").val();
    pEncabezadoFacturaProveedor.TipoCambioFactura = $("#txtTipoCambioFactura").val();
    
    var validacion = ValidaEncabezadoFacturaProveedor(pEncabezadoFacturaProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEncabezadoFacturaProveedor = pEncabezadoFacturaProveedor;
    SetEditarEncabezadoFacturaProveedor(JSON.stringify(oRequest));
}
function SetEditarEncabezadoFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/EditarEncabezadoFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
                ObtenerTotalesEstatusRecepcion();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarEncabezadoFacturaProveedor").dialog("close");
        }
    });
}

function SetEliminarDetalleFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/EliminarDetalleFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor", respuesta.IdEncabezadoFacturaProveedor);
                $("#txtSubtotalFactura").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtIVAFactura").text(formato.moneda(parseFloat(respuesta.TotalIVA), "$"));
                $("#txtTotalFactura").text(formato.moneda(parseFloat(respuesta.SubtotalFactura) + (parseFloat(respuesta.TotalIVA)), "$"));
                $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalFactura").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                $("#txtSubtotalFacturaEditar").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtIVAFacturaEditar").text(formato.moneda(parseFloat(respuesta.TotalIVA), "$"));
                $("#txtTotalFacturaEditar").text(formato.moneda(parseFloat(respuesta.SubtotalFactura) + (parseFloat(respuesta.TotalIVA)), "$"));
                $("#spanCantidadLetraEditar").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalFacturaEditar").text()).toString(), $("#cmbTipoMoneda option:selected").text()));
                
                $("#grdDetalleFacturaProveedorEditar").trigger("reloadGrid");
                $("#grdDetalleFacturaProveedorConsultar").trigger("reloadGrid");
                $("#grdDetalleFacturaProveedor").trigger("reloadGrid");
                $("#grdEncabezadoFacturaProveedor").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function EdicionProductoNumeroSerie(valor, id, rowid, iCol) {
    
}

function AutocompletarProveedor() {

    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtRazonSocial').val();
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor", "0");
                    $("#cmbCondicionPago option[value=0]").attr("selected", true);
                    var json = jQuery.parseJSON(pRespuesta.d);

                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdProveedor, idCondicionPago: item.IdCondicionPago }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdProveedor = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor", pIdProveedor);
            $("#cmbCondicionPago option[value=" + ui.item.idCondicionPago + "]").attr("selected", true);

            var Proveedor = new Object();
            Proveedor.IdProveedor = pIdProveedor;
            obtenerOrdenCompraProveedor(JSON.stringify(Proveedor));

            var pCondicionPago = new Object();
            pCondicionPago.IdCondicionPago = $("#cmbCondicionPago").val();
            pCondicionPago.FechaFactura = $("#txtFechaFactura").val();
            var oRequest = new Object();
            oRequest.pCondicionPago = pCondicionPago;
            ObtenerFechaPago(JSON.stringify(oRequest));

            ExisteNumeroFactura();

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarProductoClave() {
    
    $('#txtClaveProducto').autocomplete({
        source: function(request, response) {
            var pProducto = new Object();
            pProducto.Producto = $("#txtClaveProducto").val();
            pProducto.TipoBusqueda = "C";

            var oRequest = new Object();
            oRequest.pProducto = pProducto;
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarProducto',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");
                    $("#txtDescripcionProducto").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        if (pProducto.TipoBusqueda == 'C') {
                            return { label: item.Producto, value: item.Producto, id: item.IdProducto }
                        }
                        else {
                            return { label: item.Clave, value: item.Clave, id: item.IdProducto }
                        }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", pIdProducto);
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");
            
            var Producto = new Object();
            Producto.IdProducto = pIdProducto;
            obtenerProducto(Producto);

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarProductoDescripcion() {
    $('#txtDescripcionProducto').autocomplete({
        source: function(request, response) {
            var pProducto = new Object();
            pProducto.Producto = $("#txtDescripcionProducto").val();
            pProducto.TipoBusqueda = "P";

            var oRequest = new Object();
            oRequest.pProducto = pProducto;
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarProducto',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");
                    $("#txtClaveProducto").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        if (pProducto.TipoBusqueda == 'P') {
                            return { label: item.Producto, value: item.Producto, id: item.IdProducto }
                        }
                        else {
                            return { label: item.Clave, value: item.Clave, id: item.IdProducto }
                        }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", pIdProducto);
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");
            var Producto = new Object();
            Producto.IdProducto = pIdProducto;
            obtenerProducto(Producto);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarServicioClave() {

    $('#txtClaveServicio').autocomplete({
        source: function(request, response) {
            var pServicio = new Object();
            pServicio.Servicio = $("#txtClaveServicio").val();
            pServicio.TipoBusqueda = "C";

            var oRequest = new Object();
            oRequest.pServicio = pServicio;
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarServicio',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");
                    $("#txtDescripcionServicio").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        if (pServicio.TipoBusqueda == 'C') {
                            return { label: item.Servicio, value: item.Servicio, id: item.IdServicio }
                        }
                        else {
                            return { label: item.Clave, value: item.Clave, id: item.IdServicio }
                        }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdServicio = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", pIdServicio);
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");

            var Servicio = new Object();
            Servicio.IdServicio = pIdServicio;
            obtenerServicio(Servicio);

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarServicioDescripcion() {

    $('#txtDescripcionServicio').autocomplete({
        source: function(request, response) {
            var pServicio = new Object();
            pServicio.Servicio = $("#txtDescripcionServicio").val();
            pServicio.TipoBusqueda = "P";

            var oRequest = new Object();
            oRequest.pServicio = pServicio;
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarServicio',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");
                    $("#txtClaveServicio").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        if (pServicio.TipoBusqueda == 'P') {
                            return { label: item.Servicio, value: item.Servicio, id: item.IdServicio }
                        }
                        else {
                            return { label: item.Clave, value: item.Clave, id: item.IdServicio }
                        }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdServicio = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProducto", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idServicio", pIdServicio);
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCotizacionDetalle", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idOrdenCompraDetalle", "0");

            var Servicio = new Object();
            Servicio.IdServicio = pIdServicio;
            obtenerServicio(Servicio);

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarCliente() {

    $('#txtCliente').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtCliente').val();
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", pIdCliente);
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", "0");
            var Cliente = new Object();
            Cliente.IdCliente = pIdCliente;
            obtenerPedidosCliente(JSON.stringify(Cliente));            
            
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarProyecto() {

    $('#txtProyecto').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pNombreProyecto = $('#txtProyecto').val();
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarProyectoCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", "0");
                    $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.NombreProyecto, value: item.NombreProyecto, id: item.IdProyecto }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdProyecto = ui.item.id;
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idCliente", "0");
            $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProyecto", pIdProyecto);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function ExisteNumeroFactura() {
    var pEncabezadoFactura = new Object();
    pEncabezadoFactura.NumeroFactura = $("#txtNumeroFactura").val();
    pEncabezadoFactura.IdProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idProveedor");
    var oRequest = new Object();
    oRequest.pEncabezadoFactura = pEncabezadoFactura;
    SetExisteNumeroFactura(JSON.stringify(oRequest));
}

function RecalculaDatosCosto() {
    var Costo = 0;
    var CostoDescuento = 0;
    var Descuento = 0;
    var Total = 0;
    var Cantidad = 0;
    Costo = QuitaFormatoMoneda($("#txtCosto").val());
    Descuento = QuitaFormatoMoneda($("#txtDescuento").val());
    Cantidad = $("#txtCantidad").val();
    if (Cantidad == "") {
        Cantidad = 0;
    }
    if (Costo == "") {
        Costo = 0;
    }
    if (Descuento == "") {
        Descuento = 0;
    }
    CostoDescuento = parseFloat(Costo) * (parseFloat(1) - parseFloat((parseFloat(Descuento) / parseFloat(100))));
    $("#txtCostoDescuento").val(formato.moneda(CostoDescuento.toFixed(2), '$'));
    Total = parseFloat(Cantidad) * parseFloat(CostoDescuento);
    $("#txtTotalSinTipoCambio").val(formato.moneda(Total.toFixed(2), '$'));

    RecalculaMontoConTipoCambio();
}

function RecalculaDatosCostoDescuento() {
    var Costo = 0;
    var CostoDescuento = 0;
    var Descuento = 0;
    var Total = 0;
    var Cantidad = 0;
    Costo = QuitaFormatoMoneda($("#txtCosto").val());
    Cantidad = $("#txtCantidad").val();
    CostoDescuento = QuitaFormatoMoneda($("#txtCostoDescuento").val());
    if (Cantidad == "") {
        Cantidad = 0;
    }
    if (Costo == "") {
        Costo = 0;
    }
    if (CostoDescuento == "") {
        CostoDescuento = 0;
    }
    Descuento = ((parseFloat(Costo) - parseFloat(CostoDescuento)) / parseFloat(Costo)) * 100
    $("#txtDescuento").val(Descuento.toFixed(2));
    Total = parseFloat(Cantidad) * parseFloat(CostoDescuento);
    $("#txtTotalSinTipoCambio").val(formato.moneda(Total.toFixed(2), '$'));
    RecalculaMontoConTipoCambio();
}

function RecalculaDatosCantidad() {
    var Costo = 0;
    var CostoDescuento = 0;
    var Descuento = 0;
    var Total = 0;
    var Cantidad = 0;
    Costo = parseFloat(QuitaFormatoMoneda($("#txtCosto").val()));
    Costo = Costo * parseFloat($("#txtTipoCambioDetalle").val());
    Costo = Costo.toFixed(2);
    $("#txtCosto").val(formato.moneda(Costo, '$'));
    $("#txtCostoDescuento").val(formato.moneda(Costo, '$'));
    
    Cantidad = $("#txtCantidad").val();
    CostoDescuento = QuitaFormatoMoneda($("#txtCostoDescuento").val());
    if (Cantidad == "") {
        Cantidad = 0;
    }
    if (CostoDescuento == "") {
        CostoDescuento = 0;
    }
    
    Total = parseFloat(Cantidad) * parseFloat(CostoDescuento);
    $("#txtTotalSinTipoCambio").val(formato.moneda(Total.toFixed(2), '$'));
    RecalculaMontoConTipoCambio();
}

function RecalculaMontoConTipoCambio() {
    var TotalTipoCambio;
    var TipoCambioFactura;
    TipoCambioFactura = $("#txtTipoCambioFactura").val();
    if (TipoCambioFactura == 0) {
        TipoCambioFactura = 1;
    }
    if (respuesta.Modelo.TipoCambioActual == 0) {
        respuesta.Modelo.TipoCambioActual = 1;
    }
    TotalTipoCambio = (parseFloat(QuitaFormatoMoneda($("#txtTotalSinTipoCambio").val())) / parseFloat($("#txtTipoCambioDetalle").val()));
    $("#txtTotalConTipoCambio").val(formato.moneda(TotalTipoCambio.toFixed(2), '$'));

}

function SetExisteNumeroFactura(pEncabezadoFactura) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/ExisteNumeroFactura",
        data: pEncabezadoFactura,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            if (respuesta.Error != 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#txtNumeroFactura").val("0");
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function obtenerProducto(pRequest) {
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/obtenerProducto",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Modelo = respuesta.Modelo;
                $("#txtDescripcionProducto").val(Modelo.Producto);
                $("#txtClaveProducto").val(Modelo.Producto);
                //$("#txtCosto").val(formato.moneda(Modelo.Costo,'$'));
                //$("#txtDescuento").val(0);
                //$("#txtCostoDescuento").val(formato.moneda(Modelo.Costo,'$'));
                //$("#txtTotalSinTipoCambio").val(formato.moneda(Modelo.Costo,'$'));
                $("#cmbUnidadCompraVenta option[value=" + Modelo.IdUnidadCompraVenta + "]").attr("selected", true);
                $("#cmbUnidadCompraVenta").attr("disabled", "true");
                //$("#cmbTipoMonedaConcepto option[value=" + Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA", Modelo.IdTipoIVA);
                $("#txtIVA").val(Modelo.IVA);
                var pTipoCambio = new Object();
                pTipoCambio.IdTipoCambioOrigen = parseInt($("#cmbTipoMonedaConcepto").val()); ;
                pTipoCambio.IdTipoCambioDestino = parseInt($("#cmbTipoMoneda").val());
                var oRequest = new Object();
                oRequest.pTipoCambio = pTipoCambio;
                ObtenerTipoCambioDetalle(JSON.stringify(oRequest))
                
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function obtenerServicio(pRequest) {
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/obtenerServicio",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Modelo = respuesta.Modelo;
                $("#txtDescripcionServicio").val(Modelo.Servicio);
                $("#txtClaveServicio").val(Modelo.Servicio);
                //$("#txtCosto").val(formato.moneda(Modelo.Precio,'$'));
                //$("#txtDescuento").val(0);
                //$("#txtCostoDescuento").val(formato.moneda(Modelo.Precio,'$'));
                //$("#txtTotalSinTipoCambio").val(formato.moneda(Modelo.Precio,'$'));
                $("#cmbUnidadCompraVenta option[value=" + Modelo.IdUnidadCompraVenta + "]").attr("selected", true);
                $("#cmbUnidadCompraVenta").attr("disabled", "true");
                //$("#cmbTipoMonedaConcepto option[value=" + Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA", Modelo.IdTipoIVA);
                $("#txtIVA").val(Modelo.IVA);
                var pTipoCambio = new Object();
                pTipoCambio.IdTipoCambioOrigen = parseInt($("#cmbTipoMonedaConcepto").val()); ;
                pTipoCambio.IdTipoCambioDestino = parseInt($("#cmbTipoMoneda").val());
                var oRequest = new Object();
                oRequest.pTipoCambio = pTipoCambio;
                ObtenerTipoCambioDetalle(JSON.stringify(oRequest));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function obtenerPedidosClienteOrdenCompra(pRequest,IdCotizacion) {
    $("#cmbCotizacion").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "EncabezadoFacturaProveedor.aspx/obtenerPedidosCliente",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#cmbCotizacion option[value=" + IdCotizacion + "]").attr("selected", true);
        }
    });
}

function obtenerPedidosCliente(pRequest) {
    $("#cmbCotizacion").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "EncabezadoFacturaProveedor.aspx/obtenerPedidosCliente",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}


function ObtenerTotalesEstatusRecepcion() {

    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerTotalesEstatusRecepcion',
        data: {},
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $.each(respuesta.Modelo.TotalesEstatusRecepcion, function(index, oEstatusRecepcion) {
                    $('#span-E' + oEstatusRecepcion.IdEstatusRecepcion).text(oEstatusRecepcion.Contador);
                });
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        }
    });
}

function obtenerOrdenCompraProveedor(pRequest) {
    $("#cmbOrdenCompra").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "EncabezadoFacturaProveedor.aspx/obtenerOrdenCompraProveedor",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}


function FiltroEncabezadoFacturaProveedor() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdEncabezadoFacturaProveedor').getGridParam('rowNum');
    request.pPaginaActual = $('#grdEncabezadoFacturaProveedor').getGridParam('page');
    request.pColumnaOrden = $('#grdEncabezadoFacturaProveedor').getGridParam('sortname');
    request.pTipoOrden = $('#grdEncabezadoFacturaProveedor').getGridParam('sortorder');
    request.pRazonSocial = "";
    request.pNumeroFactura = "";
    request.pIdEstatusRecepcion = -1;
    request.pDivision = "";
    request.pAI = 0;
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pNumeroSerie = "";
    request.pClave = "";
    request.pNumeroPedido = "";
    request.pBusquedaDocumento = 0;

    if ($("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != null && $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != "") {
        request.pIdEstatusRecepcion = $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado");
    }

    if ($('#gs_NumeroFactura').val() != null) { request.pNumeroFactura = $("#gs_NumeroFactura").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }

    if ($('#gs_Division').val() != null) { request.pDivision = $("#gs_Division").val(); }

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

    if ($("#txtNumeroSerieBuscador").val() != "" && $("#txtNumeroSerieBuscador").val() != null) {
    	request.pNumeroSerie = $("#txtNumeroSerieBuscador").val();
    }

    if ($("#txtNumeroClaveBuscador").val() != "" && $("#txtNumeroClaveBuscador").val() != null) {
    	request.pClave = $("#txtNumeroClaveBuscador").val();
    }

    if ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null) {
        request.pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
    }

    if ($("#cmbBusquedaDocumento").val() != "" && $("#cmbBusquedaDocumento").val() != null) {
        request.pBusquedaDocumento = $("#cmbBusquedaDocumento").val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerEncabezadoFacturaProveedor',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdEncabezadoFacturaProveedor')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}



function FiltroDetalleFacturaProveedor() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleFacturaProveedor').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleFacturaProveedor').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleFacturaProveedor').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleFacturaProveedor').getGridParam('sortorder');
    request.pIdEncabezadoFacturaProveedor = 0;
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != null && $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != "") {
        request.pIdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
    url: 'EncabezadoFacturaProveedor.aspx/ObtenerDetalleFacturaProveedor',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleFacturaProveedor')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleFacturaProveedorConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleFacturaProveedorConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleFacturaProveedorConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleFacturaProveedorConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleFacturaProveedorConsultar').getGridParam('sortorder');
    request.pIdEncabezadoFacturaProveedor = 0;
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaConsultarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != null && $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != "") {
        request.pIdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaConsultarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerDetalleFacturaProveedorConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleFacturaProveedorConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleFacturaProveedorEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleFacturaProveedorEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleFacturaProveedorEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleFacturaProveedorEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleFacturaProveedorEditar').getGridParam('sortorder');
    request.pIdEncabezadoFacturaProveedor = 0;
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != null && $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != "") {
        request.pIdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerDetalleFacturaProveedorEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleFacturaProveedorEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroProductoNumeroSerie() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdProductoNumeroSerie').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProductoNumeroSerie').getGridParam('page');
    request.pColumnaOrden = $('#grdProductoNumeroSerie').getGridParam('sortname');
    request.pTipoOrden = $('#grdProductoNumeroSerie').getGridParam('sortorder');
    request.pIdProducto = 0;
    request.pCantidad = 0;
    request.pCosto = 0;
    request.pDescuento = 0;
    request.pCostoDescuento = 0;
    request.pTipoCambioFactura = 0;
    request.pTipoCambioDetalle = 0;
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdProducto") != null && $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdProducto") != "") {
        request.pIdProducto = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdProducto");
        request.pCantidad = $("#txtCantidad").val();
        request.pCosto = QuitaFormatoMoneda($("#txtCosto").val());
        request.pDescuento = QuitaFormatoMoneda($("#txtDescuento").val());
        request.pCostoDescuento = QuitaFormatoMoneda($("#txtCostoDescuento").val());
        request.pTipoCambioFactura = $("#txtTipoCambioFactura").val();
        request.pTipoCambioDetalle = $("#txtTipoCambioDetalle").val();
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerProductoNumeroSerie',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdProductoNumeroSerie')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetallePedido() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetallePedido').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetallePedido').getGridParam('page');
    request.pColumnaOrden = $('#grdDetallePedido').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetallePedido').getGridParam('sortorder');
    request.pIdPedido = 0;

    if ($("#cmbCotizacion").val() != null && $("#cmbCotizacion").val() != "") {
        request.pIdPedido = $("#cmbCotizacion").val();
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerDetallePedido',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetallePedido')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleOrdenCompra() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleOrdenCompra').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleOrdenCompra').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleOrdenCompra').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleOrdenCompra').getGridParam('sortorder');
    request.pIdOrdenCompra = 0;

    if ($("#cmbOrdenCompra").val() != null && $("#cmbOrdenCompra").val() != "") {
        request.pIdOrdenCompra = $("#cmbOrdenCompra").val();
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerDetalleOrdenCompra',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleOrdenCompra')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFacturasPendientesPorValidar() {
    var request = new Object();
    var grid = $("#grdFacturasPendientesPorValidar");
    request.pTamanoPaginacion = $(grid).getGridParam('rowNum');
    request.pPaginaActual = $(grid).getGridParam('page');
    request.pColumnaOrden = $(grid).getGridParam('sortname');
    request.pTipoOrden = $(grid).getGridParam('sortorder');
    request.pAI = 0;
    request.pIdOrdenCompra = 0;
    request.pRazonSocial = ($("#gs_RazonSocialValidar").val() != null) ? $("#gs_RazonSocialValidar").val() : "";
    request.pNumeroFactura = ($("#gs_NumeroFacturaValidar").val() != null) ? $("#gs_NumeroFacturaValidar").val() : "";
    request.pDivision = ($("#gs_DivisionValidar").val() != null) ? $("#gs_DivisionValidar").val() : "";

    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url:'EncabezadoFacturaProveedor.aspx/ObtenerFacturasPendientesPorValidar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturasPendientesPorValidar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturasPendientesPorValidar() {
    var request = new Object();
    var grid = $("#grdFacturasPendientesPorValidar");
    request.pTamanoPaginacion = $(grid).getGridParam('rowNum');
    request.pPaginaActual = $(grid).getGridParam('page');
    request.pColumnaOrden = $(grid).getGridParam('sortname');
    request.pTipoOrden = $(grid).getGridParam('sortorder');
    request.pAI = 0;
    request.pIdOrdenCompra = 0;
    request.pRazonSocial = ($("#gs_RazonSocialValidar").val() != null) ? $("#gs_RazonSocialValidar").val() : "";
    request.pNumeroFactura = ($("#gs_NumeroFacturaValidar").val() != null) ? $("#gs_NumeroFacturaValidar").val() : "";
    request.pDivision = ($("#gs_DivisionValidar").val() != null) ? $("#gs_DivisionValidar").val() : "";

    var pRequest = JSON.stringify(request);
    //alert(pRequest);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerFacturasPendientesPorValidar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturasPendientesPorValidar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//-----Validaciones------------------------------------------------------
function ValidaEncabezadoFacturaProveedor(pEncabezadoFacturaProveedor) {
    var errores = "";

    if (pEncabezadoFacturaProveedor.IdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.IdDivision == 0)
    { errores = errores + "<span>*</span> No hay división asociada, favor de elegir alguna.<br />"; }

    if (pEncabezadoFacturaProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.FechaFactura == "")
    { errores = errores + "<span>*</span> El campo fecha de factura esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.IdCondicionPago <= 0)
    { errores = errores + "<span>*</span> No hay condicion de pago seleccionada, favor de elegir alguna.<br/>"}

    if (pEncabezadoFacturaProveedor.FechaPago == "")
    { errores = errores + "<span>*</span> El campo fecha de factura esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDetalleFacturaProveedor(pEncabezadoFacturaProveedor) {
    var errores = "";
   
    if (pEncabezadoFacturaProveedor.IdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.NumeroFactura == "" || pEncabezadoFacturaProveedor.NumeroFactura == 0)
    { errores = errores + "<span>*</span> El campo número de factura esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.IdDivision == 0)
    { errores = errores + "<span>*</span> No hay división asociada, favor de elegir alguna.<br />"; }

    if (pEncabezadoFacturaProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.FechaFactura == "")
    { errores = errores + "<span>*</span> El campo fecha de factura esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.FechaPago == "")
    { errores = errores + "<span>*</span> El campo fecha de pago esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.IdProducto == 0 && pEncabezadoFacturaProveedor.IdServicio == 0)
    { errores = errores + "<span>*</span> Favor de elegir un producto o un servicio.<br />"; }

    if (pEncabezadoFacturaProveedor.Costo == 0)
    { errores = errores + "<span>*</span> El campo costo esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.CostoDescuento == 0)
    { errores = errores + "<span>*</span> El campo costodescuento esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.Cantidad <= 0)
    { errores = errores + "<span>*</span> El campo cantidad debe de ser mayor a 0.<br />"; }
    
    //if (pEncabezadoFacturaProveedor.Total == 0)
    //{ errores = errores + "<span>*</span> El campo total esta vacío, favor de capturarlo.<br />"; }

    if (pEncabezadoFacturaProveedor.IdUnidadCompraVenta == 0)
    { errores = errores + "<span>*</span> No hay unidad de compra venta asociado, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.IdTipoCompra == 0)
    { errores = errores + "<span>*</span> No hay tipo de compra asociado, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.IdTipoMonedaConcepto == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda del concepto asociado, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.IdUsuarioSolicito == 0)
    { errores = errores + "<span>*</span> No hay usuario solicitante asociado, favor de elegir alguno.<br />"; }

//    if (pEncabezadoFacturaProveedor.IdCliente == 0 && pEncabezadoFacturaProveedor.IdProyecto == 0)
//    { errores = errores + "<span>*</span> Favor de elegir un cliente o un proyecto.<br />"; }

    if (pEncabezadoFacturaProveedor.IdAlmacen == 0)
    { errores = errores + "<span>*</span> No hay almacén asociado, favor de elegir alguno.<br />"; }

    if (pEncabezadoFacturaProveedor.IdCotizacionDetalle != 0 || pEncabezadoFacturaProveedor.IdOrdenCompraDetalle != 0)
    {
        if (parseInt(pEncabezadoFacturaProveedor.Cantidad) > parseInt(pEncabezadoFacturaProveedor.CantidadPedido))
        { errores = errores + "<span>*</span> El campo cantidad no puede ser mayor a la cantidad de la partida seleccionada.<br />"; } 
    }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }
    
    return errores;
}

function ObtenerListaTipoCompra() {
    $("#cmbTipoCompra").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerListaTipoCompra"
    });
}