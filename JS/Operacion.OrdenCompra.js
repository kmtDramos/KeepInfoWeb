//----------Variables----------//
var vClienteProyecto = "1";
var buttonsEdit = {
    "Editar": function() {
        EditarOrdenCompra(this);
    }
                        ,
    "Generar orden": function() {
        ConsolidarOrdenCompra(this);
        $(this).dialog("close");
    }
                        ,
    "Salir": function() {
        $(this).dialog("close");
    }
};

var buttonSalir = {
    "Salir": function() {
        $(this).dialog("close");
    }
};

//--------------------------//

//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("OrdenCompra");
    });

    ObtenerOrdenCompraIndicadores();
    ObtenerFormaFiltrosOrdenCompraEncabezado();

    //////funcion del grid//////
    $("#gbox_grdOrdenCompra").livequery(function() {
        $("#grdOrdenCompra").jqGrid('navButtonAdd', '#pagOrdenCompra', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pFolio = "";
                var pIdEstatusRecepcion = -1;
                var pAI = 0;

                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;

                if ($("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != null && $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != "") {
                    pIdEstatusRecepcion = $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado");
                }

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

                pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
                pIdTipoArchivo = ($("#cmbBusquedaDocumento").val() != "") ? parseInt($("#cmbBusquedaDocumento").val()) : 0;

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pRazonSocial: pRazonSocial,
                    pFolio: pFolio,
                    pIdEstatusRecepcion: pIdEstatusRecepcion,
                    pAI: pAI,
                    pFechaInicial: pFechaInicial,
                    pFechaFinal: pFechaFinal,
                    pPorFecha: pPorFecha,
                    pNumeroPedido: pNumeroPedido,
                    pIdTipoArchivo: pIdTipoArchivo
                }, downloadType: 'Normal'
                });

            }
        });
    });
    ///////////////////////////

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarOrdenCompra", function() {
        ObtenerFormaAgregarOrdenCompra();
    });

    $("#grdOrdenCompra").on("click", ".imgFormaConsultarOrdenCompra", function() {
        var registro = $(this).parents("tr");
        var OrdenCompra = new Object();
        OrdenCompra.pIdOrdenCompra = parseInt($(registro).children("td[aria-describedby='grdOrdenCompra_IdOrdenCompraEncabezado']").html());
        ObtenerFormaConsultarOrdenCompra(JSON.stringify(OrdenCompra));
    });

    $("#grdOrdenCompra").on("click", ".div_grdOrdenCompra_AI", function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdOrdenCompra_AI']").children().attr("baja")
        var IdOrdenCompra = $(registro).children("td[aria-describedby='grdOrdenCompra_IdOrdenCompraEncabezado']").html();
        var baja = (estatusBaja == "0" || estatusBaja.toLowerCase() == "false") ? "true" : "false";

        SetCambiarEstatus(IdOrdenCompra, baja);
    });

    $("#dialogAgregarOrdenCompra, #dialogEditarOrdenCompra").on("click", "#divBuscarPedidosCliente", function() {

        var comboPedido = $("#cmbPedido").val();
        if (comboPedido == '0') {
            MostrarMensajeError("Por favor seleccione un pedido del combo");
            return false;
        }

        $("#divFormaMuestraDetallePedido").obtenerVista({
            nombreTemplate: "tmplAgregarDetallePedido.html",
            despuesDeCompilar: function() {
                FiltroDetallePedido();
                $("#dialogMuestraDetallePedido").dialog("open");
            }
        });
    });


    $("#dialogAgregarOrdenCompra, #dialogEditarOrdenCompra, #dialogConsultarOrdenCompra").on("click", "#divImprimir", function() {
        var IdOrdenCompraI = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra, #divFormaConsultarOrdenCompra").attr("IdOrdenCompra");
        Imprimir(IdOrdenCompraI);
    });

    $("#dialogAgregarOrdenCompra, #dialogEditarOrdenCompra").on("click", "#divTipoCambio", function() {
        $("#dialogProporcionarClaveAutorizacion").obtenerVista({
            nombreTemplate: "tmplObtenerAutorizacionTipoCambio.html",
            despuesDeCompilar: function() {
                $("#dialogProporcionarClaveAutorizacion").dialog("open");
            }
        });
    });

    $("#grdDetallePedido").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var DetallePedido = new Object();
        var cantidad = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_Restante']").html());

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
        var cantidad = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_Restante']").html());

        if (cantidad > 0) {
            DetallePedido.pIdDetallePedido = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdCotizacionDetalle']").html());
            ObtenerDatosDetallePedido(JSON.stringify(DetallePedido));
            $("#dialogMuestraDetallePedido").dialog("close");
        } else {
            MostrarMensajeError("El saldo restante es igual a 0, no se puede bajar la partida.");
        }

    });

    $(".spanFiltroTotal").click(function() {
        var idEstatusRecepcion = $(this).attr("IdEstatusRecepcion");
        $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado", idEstatusRecepcion);
        $('#gs_Folio').val(null);
        $('#gs_RazonSocial').val(null);
        $('#gs_AI').val(null);
        $("#chkPorFecha").attr('checked', false);
        FiltroOrdenCompra();
    });

    $('#dialogAgregarOrdenCompra').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: true,
        resizable: true,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("input[name=ProductoServicio]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetos(1);
                } else {
                    MuestraObjetos(0);
                }
            });

            $("#divImprimir").hide();
            $("#txtCosto").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtDescuento").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtCostoDescuento").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtCantidad").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtTotal").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });

            $('#divFormaAgregarOrdenCompra').on('focusout', '#txtCosto', function(event) {

                RecalculaDatosCosto();
            });
            $('#divFormaAgregarOrdenCompra').on('focusout', '#txtDescuento', function(event) {
                if (parseInt($(this).val(), 10) > 100) {
                    $(this).val("100");
                }
                RecalculaDatosCosto();
            });
            $('#divFormaAgregarOrdenCompra').on('focusout', '#txtCostoDescuento', function(event) {
                if (parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())) > parseFloat(QuitaFormatoMoneda($("#txtCosto").val()))) {
                    $(this).val($("#txtCosto").val());
                }
                RecalculaDatosCostoDescuento();
            });
            $('#divFormaAgregarOrdenCompra').on('focusout', '#txtCantidad', function(event) {
                //$(this).val($(this).val() > $(this).attr("OrdenCompraCantidad") ? $(this).attr("OrdenCompraCantidad") : $(this).val());
                RecalculaDatosCantidad();
            });
            $("#divFormaAgregarOrdenCompra").on("click", "#btnAgregarPartida", function() {
                AgregarOrdenCompraDetalle();
            });

            $('#divFormaAgregarOrdenCompra').on('change', '#cmbTipoMoneda', function(event) {
                $("#txtMonedaProducto").val($("#cmbTipoMoneda option:selected").text());
                $("#txtMonedaServicio").val($("#cmbTipoMoneda option:selected").text());

                CambiarPrecio();

                $("#txtMonedaProducto").attr("tipomonedaid", $("#cmbTipoMoneda").val());
                $("#txtMonedaServicio").attr("tipomonedaid", $("#cmbTipoMoneda").val());
            });

            $('#divFormaAgregarOrdenCompra').on('change', '#cmbPedido', function(event) {
                LimpiarDatosOrdenCompraDetalle();
            });

            $("input[name=ClienteProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                    $("#chkCSPedido").attr("checked", false);
                    MuestraObjetosSinPedido();
                    vClienteProyecto = "1";
                } else {
                    MuestraObjetosClienteProyecto(0);
                    $("#chkCSPedido").attr("checked", true);
                    MuestraObjetosSinPedido();
                    vClienteProyecto = "2";
                }
            });

            $("#grdOrdenCompraDetalle").on("click", ".imgEliminarPartida", function() {
                var registro = $(this).parents("tr");
                var pOrdenCompraDetalle = new Object();
                pOrdenCompraDetalle.pIdOrdenCompraDetalle = parseInt($(registro).children("td[aria-describedby='grdOrdenCompraDetalle_IdOrdenCompraDetalle']").html());
                var oRequest = new Object();
                oRequest.pOrdenCompraDetalle = pOrdenCompraDetalle;

                $("#dialogConfirmaAccion").dialog("option", "buttons", {
                    "Aceptar": function() {
                        SetEliminarOrdenCompraDetalle(JSON.stringify(oRequest));
                        $(this).dialog("close")
                    },
                    "Cancelar": function() {
                        $(this).dialog("close")
                    }
                });
                $("#textoConfirmacion").html("¿Está seguro que desea eliminar la partida?");
                $("#dialogConfirmaAccion").dialog("open");
            });

            MuestraObjetosSinPedido();
        },
        close: function() {
            $("#divFormaAgregarOrdenCompra").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarOrdenCompra();
            },
            "Generar orden": function() {
                ConsolidarOrdenCompra(this);
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarOrdenCompra').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarOrdenCompra").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarOrdenCompra').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: true,
        resizable: true,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("input[name=ProductoServicio]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetos(1);
                } else {
                    MuestraObjetos(0);
                }
            });

            $("#txtCosto").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtDescuento").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtCostoDescuento").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtCantidad").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });
            $("#txtTotal").keypress(function(event) { return ValidarNumeroPunto(event, this.value); });

            $('#divFormaEditarOrdenCompra').on('focusout', '#txtCosto', function(event) {
                RecalculaDatosCosto();
            });

            $('#divFormaEditarOrdenCompra').on('focusout', '#txtDescuento', function(event) {
                if (parseInt($(this).val(), 10) > 100) {
                    $(this).val("100");
                }
                RecalculaDatosCosto();
            });
            $('#divFormaEditarOrdenCompra').on('focusout', '#txtCostoDescuento', function(event) {
                if (parseFloat(QuitaFormatoMoneda($("#txtCostoDescuento").val())) > parseFloat(QuitaFormatoMoneda($("#txtCosto").val()))) {
                    $(this).val($("#txtCosto").val());
                }
                RecalculaDatosCostoDescuento();
            });
            $('#divFormaEditarOrdenCompra').on('focusout', '#txtCantidad', function(event) {
                //$(this).val($(this).val() > $(this).attr("OrdenCompraCantidad") ? $(this).attr("OrdenCompraCantidad") : $(this).val());
                RecalculaDatosCantidad();
            });
            $("#divFormaEditarOrdenCompra").on("click", "#btnAgregarPartida", function() {
                AgregarOrdenCompraDetalle();
            });

            $("#grdOrdenCompraDetalleEditar").on("click", ".imgEliminarPartidaEditar", function() {
                var registro = $(this).parents("tr");
                var pOrdenCompraDetalle = new Object();
                pOrdenCompraDetalle.pIdOrdenCompraDetalle = parseInt($(registro).children("td[aria-describedby='grdOrdenCompraDetalleEditar_IdOrdenCompraDetalle']").html());
                var oRequest = new Object();
                oRequest.pOrdenCompraDetalle = pOrdenCompraDetalle;

                $("#dialogConfirmaAccion").dialog("option", "buttons", {
                    "Aceptar": function() {
                        SetEliminarOrdenCompraDetalle(JSON.stringify(oRequest));
                        $(this).dialog("close")
                    },
                    "Cancelar": function() {
                        $(this).dialog("close")
                    }
                });
                $("#textoConfirmacion").html("¿Está seguro que desea eliminar la partida?");
                $("#dialogConfirmaAccion").dialog("open");
            });

            MuestraObjetosSinPedido();
        },
        close: function() {
            $("#divFormaEditarOrdenCompra").remove();
        },
        buttons: buttonsEdit
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

    $('#dialogProporcionarClaveAutorizacion').dialog({
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
                ProporcionarClaveAutorizacion();
            },
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogConfirmaAccion").dialog({
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
                $(this).dialog("close");
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });




    $('#dialogAgregarOrdenCompra, #dialogEditarOrdenCompra').on('focusout', '#txtDescuento', function(event) {
        $(this).valorPredeterminado("Porcentaje");
        if (parseInt($(this).val(), 10) > 100) {
            $(this).val("100");
        }
    });


    $("#divFiltrosOrdenCompraEncabezado").on("keypress", "#txtNumeroPedidoBuscador", function(event) {


        var key = (document.all) ? event.keyCode : event.which;
        if (key == 13) {
            FiltroOrdenCompra();
        }
    });
});

function MuestraObjetos(opcion) {
    if (opcion == 1) {
        $("#txtClaveProducto").css("display", "block");
        $("#txtDescripcionProducto").css("display", "block");
        $("#txtMonedaProducto").css("display", "block");
                
        $("#txtClaveServicio").css("display", "none");
        $("#txtDescripcionServicio").css("display", "none");
        $("#txtMonedaServicio").css("display", "none");        
                
        $("#txtClaveServicio").val("");
        $("#txtDescripcionServicio").val("");
        $("#txtDescripcionDetallePartida").val("");
                
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
    }
    else {
        $("#txtClaveProducto").css("display", "none");
        $("#txtDescripcionProducto").css("display", "none");
        $("#txtMonedaProducto").css("display", "none");
                
        $("#txtClaveServicio").css("display", "block");
        $("#txtDescripcionServicio").css("display", "block");
        $("#txtMonedaServicio").css("display", "block");
                
        $("#txtClaveProducto").val("");
        $("#txtDescripcionProducto").val("");
        $("#txtDescripcionDetallePartida").val("");

        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
    }
}

function MuestraObjetosClienteProyecto(opcion) {
    if (opcion == 1) {
        $("#txtCliente").css("display", "block");
        $("#txtProyecto").css("display", "none");
        $("#txtProyecto").val("");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente", "0");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto", "0");
    }
    else {
        $("#txtCliente").css("display", "none");
        $("#txtProyecto").css("display", "block");
        $("#txtCliente").val("");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente", "0");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto", "0");
    }
}

function ObtenerFormaFiltrosOrdenCompraEncabezado() {
    $("#divFiltrosOrdenCompraEncabezado").obtenerVista({
        nombreTemplate: "tmplFiltrosOrdenCompraEncabezado.html",
        url: "OrdenCompra.aspx/ObtenerFormaFiltroOrdenCompraEncabezado",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroOrdenCompra();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroOrdenCompra();
                    }
                });
            }

            $('#divFiltrosOrdenCompraEncabezado').on('click', '#chkPorFecha', function(event) {
                FiltroOrdenCompra();
            });

        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarOrdenCompra(){
    $("#dialogAgregarOrdenCompra").obtenerVista({
        url: "OrdenCompra.aspx/ObtenerFormaAgregarOrdenCompra",
        dataType: "json",
        nombreTemplate: "tmplAgregarOrdenCompra.html",
        despuesDeCompilar: function(pRespuesta) {
        
                       
            $("#dialogAgregarOrdenCompra").dialog("open");            
            $("#txtFechaEntrega").datepicker();
            $("#txtMonedaProducto").val($("#cmbTipoMoneda option:selected").html());
            $("#txtMonedaServicio").val($("#cmbTipoMoneda option:selected").html());
            
            
            AutocompletarProveedor();
            AutocompletarProductoClave();
            AutocompletarProductoDescripcion();
            AutocompletarServicioClave();
            AutocompletarServicioDescripcion();
            AutocompletarCliente();
            AutocompletarProyecto();
            Inicializar_grdOrdenCompraDetalle();

            BloquearPantalla("#dialogAgregarOrdenCompra", "color:Red; font-size:large; font-weight:bold;", "Normal");         
        }
    });
}

function ObtenerFormaConsultarOrdenCompra(pIdOrdenCompra) {
    $("#dialogConsultarOrdenCompra").obtenerVista({
        nombreTemplate: "tmplConsultarOrdenCompra.html",
        url: "OrdenCompra.aspx/ObtenerFormaOrdenCompra",
        parametros: pIdOrdenCompra,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdOrdenCompraDetalleConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarOrdenCompra == 1) {
                if (pRespuesta.modelo.Consolidado == 1) {
                    $("#dialogConsultarOrdenCompra").dialog("option", "buttons", buttonSalir);
                }
                else {
                    $("#dialogConsultarOrdenCompra").dialog("option", "buttons", {
                        "Editar": function() {
                            $(this).dialog("close");
                            var OrdenCompra = new Object();
                            OrdenCompra.pIdOrdenCompra = parseInt($("#divFormaConsultarOrdenCompra").attr("IdOrdenCompra"));
                            ObtenerFormaEditarOrdenCompra(JSON.stringify(OrdenCompra))
                        },
                         "Salir": function() {
                            $(this).dialog("close");
                        }
                    });
                    $("#dialogConsultarOrdenCompra").dialog("option", "height", "auto");
                }
            }
            else {
                $("#dialogConsultarOrdenCompra").dialog("option", "buttons", {});
                $("#dialogConsultarOrdenCompra").dialog("option", "height", "auto");
            }

            $("#dialogConsultarOrdenCompra").dialog("open");
        }
    });
}

function ObtenerFormaEditarOrdenCompra(pIdOrdenCompra) {
    $("#dialogEditarOrdenCompra").obtenerVista({
        url: "OrdenCompra.aspx/ObtenerFormaEditarOrdenCompra",
        parametros: pIdOrdenCompra,
        dataType: "json",
        nombreTemplate: "tmplEditarOrdenCompra.html",
        despuesDeCompilar: function(pRespuesta) {

            $("#dialogEditarOrdenCompra").dialog("open");
            $("#txtFechaEntrega").datepicker();
            $("#txtMonedaProducto").val($("#cmbTipoMoneda option:selected").html());
            $("#txtMonedaservicio").val($("#cmbTipoMoneda option:selected").html());

            AutocompletarProveedor();
            AutocompletarProductoClave();
            AutocompletarProductoDescripcion();
            AutocompletarServicioClave();
            AutocompletarServicioDescripcion();
            AutocompletarCliente();
            AutocompletarProyecto();

            Inicializar_grdOrdenCompraDetalleEditar();
            LimpiarDatosOrdenCompraDetalle();
            DeshabilitaCamposEncabezado();

            //$("#chkCSPedido").attr("disabled", true);
            //$('input[name=ClienteProyecto]').attr("disabled", true);
            //$('input[name=ProductoServicio]').attr("disabled", true);

            $("input[name=ClienteProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                    $("#chkCSPedido").attr("checked", false);
                    MuestraObjetosSinPedido();
                    vClienteProyecto = "1";
                } else {
                    MuestraObjetosClienteProyecto(0);
                    $("#chkCSPedido").attr("checked", true);
                    MuestraObjetosSinPedido();
                    vClienteProyecto = "2";
                }
            });

            var idcliente = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente");
            var idproyecto = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto");

            $('input[name=ClienteProyecto]').each(function() {
                var valor = $(this).val();
                if (parseInt(valor, 10) == 1 && parseInt(idcliente,10) != 0) {
                    $(this).attr("checked", 'checked');
                    $("#txtCliente").show();
                    $("#txtProyecto").hide();
                } else if (parseInt(valor, 10) == 2 && parseInt(idproyecto, 10) != 0) {
                    $(this).attr("checked", 'checked');
                    $("#txtCliente").hide();
                    $("#txtProyecto").show();
                }
            });

            if (pRespuesta.modelo.Permisos.puedeEditarOrdenCompra != 1) {
                $("#dialogEditarOrdenCompra").dialog("option", "buttons", buttonSalir);
                $("#dialogEditarOrdenCompra").dialog("option", "height", "auto");
            } else if (pRespuesta.modelo.Consolidado == 1) {
                $("#dialogEditarOrdenCompra").dialog("option", "buttons", buttonSalir);
                $("#dialogEditarOrdenCompra").dialog("option", "height", "auto");

                BloquearPantalla("#divFormaEditarOrdenCompra", "color:Lime; font-size:large; font-weight:bold;", "Consolidado");
            } else if (pRespuesta.modelo.Baja == 1) {
                $("#dialogEditarOrdenCompra").dialog("option", "buttons", buttonSalir);
                $("#dialogEditarOrdenCompra").dialog("option", "height", "auto");

                BloquearPantalla("#divFormaEditarOrdenCompra", "color:Red; font-size:large; font-weight:bold;", "Cancelado");
            } else {
                $("#dialogEditarOrdenCompra").dialog("option", "buttons", buttonsEdit);
                $("#dialogEditarOrdenCompra").dialog("option", "height", "auto");
                BloquearPantalla("#divFormaEditarOrdenCompra", "color:Red; font-size:large; font-weight:bold;", "Normal");
            }

            var Precio = parseFloat(pRespuesta.modelo.Total);
            $("#spanCantidadLetra").text(covertirNumLetras(Precio.toString(), $("#cmbTipoMoneda option:selected").text()));
        }
    });
}

function ObtenerTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#TipoCambioActual").html("$" + respuesta.Modelo.TipoCambioActual);

                var tipocambio = QuitaFormatoMoneda(respuesta.Modelo.TipoCambioActual);
                var valor = formato.moneda((QuitaFormatoMoneda($("#txtCosto").val()) / tipocambio), '$');

                $("#txtCosto").val(valor);

                RecalculaDatosCosto();
                RecalculaDatosCostoDescuento();
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

function ObtenerOrdenCompraIndicadores() {

    var request = new Object();
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pFolio = 0;
    request.pRazonSocial = "";
    request.pAI = 0;

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
        type: "POST",
        url: "OrdenCompra.aspx/ObtenerOrdenCompraIndicadores",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
                $("#Pendientes").text(respuesta.Table[0].Pendientes);
                $("#Facturados").text(respuesta.Table[0].Facturado);
                $("#Canceladas").text(respuesta.Table[0].Canceladas);
                $("#Total").text(respuesta.Table[0].Total);            
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerDatosDetallePedido(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/ObtenerDatosDetallePedido",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCotizacionDetalle", respuesta.Modelo.IdCotizacionDetalle);
                if (respuesta.Modelo.IdProducto != 0) {

                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", respuesta.Modelo.IdProducto);
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
                    $("#txtClaveProducto").val(respuesta.Modelo.Clave);
                    $("#txtDescripcionProducto").val(respuesta.Modelo.Descripcion);
                    
                    var Producto = new Object();
                    Producto.IdProducto = respuesta.Modelo.IdProducto;
                    obtenerProducto(Producto);

                    $("#txtCantidad").val(respuesta.Modelo.OrdenCompraCantidad);
                    $("#txtCantidad").attr("OrdenCompraCantidad", respuesta.Modelo.OrdenCompraCantidad);
                }
                else {

                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", respuesta.Modelo.IdServicio);
                    $("#txtClaveServicio").val(respuesta.Modelo.Clave);
                    $("#txtDescripcionServicio").val(respuesta.Modelo.Descripcion);

                    var Servicio = new Object();
                    Servicio.IdServicio = respuesta.Modelo.IdServicio;
                    obtenerServicio(Servicio);

                    $("#txtCantidad").val(respuesta.Modelo.OrdenCompraCantidad);
                    $("#txtCantidad").attr("OrdenCompraCantidad", respuesta.Modelo.OrdenCompraCantidad);
                }
                $("#txtClaveProdServ").val(respuesta.Modelo.ClaveProdServ);
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


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarOrdenCompra() {
    var pOrdenCompra = new Object();

    var idordencompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    pOrdenCompra.IdOrdenCompra = validaNumero(idordencompra) ? idordencompra : 0;
                      
    var idproveedor = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProveedor");
    pOrdenCompra.IdProveedor = validaNumero(idproveedor) ? idproveedor : 0;

    var idautorizaciontipocambio = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdAutorizacionTipoCambio");
    pOrdenCompra.IdAutorizacionTipoCambio = validaNumero(idautorizaciontipocambio) ? idautorizaciontipocambio : 0;

    var idcliente = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente");
    pOrdenCompra.IdCliente = validaNumero(idcliente) ? idcliente : 0;
    
    pOrdenCompra.FechaRequerida = $("#txtFechaEntrega").val();
    pOrdenCompra.Subtotal       = QuitaFormatoMoneda($("#txtSubtotalDetalle").text());
    pOrdenCompra.IVA            = QuitaFormatoMoneda($("#txtIVADetalle").text());
    pOrdenCompra.Total          = QuitaFormatoMoneda($("#txtTotalDetalle").text());
    pOrdenCompra.Saldo          = QuitaFormatoMoneda($("#txtTotalDetalle").text());
    pOrdenCompra.IdTipoMoneda   = $("#cmbTipoMoneda").val();
    pOrdenCompra.IdDivision     = $("#cmbDivision").val();
    pOrdenCompra.FechaAlta      = $("#txtFecha").text();
    pOrdenCompra.DireccionEntrega = $("#txtEntrega").val();
    pOrdenCompra.Nota           = $("#txtNota").val();
    pOrdenCompra.TipoCambio     = QuitaFormatoMoneda($("#TipoCambioActual").html());
    pOrdenCompra.Consolidado    = 0;
    pOrdenCompra.SinPedido      = $("#chkCSPedido").attr("checked") ? 1 : 0;
    pOrdenCompra.CantidadTotalLetra = $("#spanCantidadLetra").html();
    pOrdenCompra.IdPedido       = $("#cmbPedido").val();

    var validacion = ValidaOrdenCompra(pOrdenCompra);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pOrdenCompra = pOrdenCompra;
    SetAgregarOrdenCompra(JSON.stringify(oRequest));
}

function EditarOrdenCompra() {
    var pOrdenCompra = new Object();
    
    var idordencompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    pOrdenCompra.IdOrdenCompra = validaNumero(idordencompra) ? idordencompra : 0;

    var idproveedor = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProveedor");
    pOrdenCompra.IdProveedor = validaNumero(idproveedor) ? idproveedor : 0;

    var idautorizaciontipocambio = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdAutorizacionTipoCambio");
    pOrdenCompra.IdAutorizacionTipoCambio = validaNumero(idautorizaciontipocambio) ? idautorizaciontipocambio : 0;

    var idcliente = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente");
    pOrdenCompra.IdCliente = validaNumero(idcliente) ? idcliente : 0;
    
    pOrdenCompra.FechaRequerida = $("#txtFechaEntrega").val();
    pOrdenCompra.Subtotal       = QuitaFormatoMoneda($('#txtSubtotalDetalle').text());
    pOrdenCompra.IVA            = QuitaFormatoMoneda($('#txtIVADetalle').text());
    pOrdenCompra.Total          = QuitaFormatoMoneda($('#txtTotalDetalle').text());
    pOrdenCompra.Saldo          = QuitaFormatoMoneda($('#txtTotalDetalle').text());
    pOrdenCompra.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pOrdenCompra.ClaveProdServ = $("#txtClaveProdServ").val(); 
    pOrdenCompra.IdDivision     = $("#cmbDivision").val();
    pOrdenCompra.IdProyecto     = $("#divFormaEditarOrdenCompra", "#dialogEditarOrdenCompra").attr("idproyecto");
    pOrdenCompra.FechaAlta      = $("#txtFecha").text();
    pOrdenCompra.DireccionEntrega = $("#txtEntrega").val();
    pOrdenCompra.Nota           = $("#txtNota").val();
    pOrdenCompra.TipoCambio     = QuitaFormatoMoneda($("#TipoCambioActual").html());
    pOrdenCompra.Consolidado    = 0;
    pOrdenCompra.SinPedido      = $("#chkCSPedido").attr("checked") ? 1 : 0;
    pOrdenCompra.CantidadTotalLetra = $("#spanCantidadLetra").html();
    pOrdenCompra.IdPedido       = $("#cmbPedido").val();

    var validacion = ValidaOrdenCompra(pOrdenCompra);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pOrdenCompra = pOrdenCompra;
    SetEditarOrdenCompra(JSON.stringify(oRequest));
}

function ConsolidarOrdenCompra(dialog) {
    var pOrdenCompra = new Object();

    var idordencompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    pOrdenCompra.IdOrdenCompra = validaNumero(idordencompra) ? idordencompra : 0;

    if (pOrdenCompra.IdOrdenCompra != 0) {
        var oRequest = new Object();
        oRequest.pOrdenCompra = pOrdenCompra;
        SetConsolidarOrdenCompra(JSON.stringify(oRequest), dialog);
    } else {
        MostrarMensajeError("¡No puede procesar la orden de compra sin haberla guardado!");
    }
}


function AgregarOrdenCompraDetalle() {
    var totalCompra = QuitaFormatoMoneda($("#txtTotal").val());
    var ivaProveedor = parseInt($("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IVAProveedor"));

    var pOrdenCompra = new Object();

    pOrdenCompra.totalCompra = totalCompra; 
    var iva = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("iVA");
    pOrdenCompra.IVA = validaNumero(iva) ? iva : 0;

    var IVATotal = (totalCompra * pOrdenCompra.IVA) / 100;
    var subtotalActual  = QuitaFormatoMoneda($('#txtSubtotalDetalle').text());
    var totalActual     = QuitaFormatoMoneda($('#txtTotalDetalle').text());
    var ivaActual = QuitaFormatoMoneda($('#txtIVADetalle').text());

    pOrdenCompra.subtotalActual = subtotalActual;
    pOrdenCompra.totalActual = totalActual;
    pOrdenCompra.ivaActual = ivaActual;    
    
    
    var idordencompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    pOrdenCompra.IdOrdenCompra = validaNumero(idordencompra) ? idordencompra : 0;

    var idproveedor = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProveedor");
    pOrdenCompra.IdProveedor = validaNumero(idproveedor) ? idproveedor : 0;
    
    var idautorizaciontipocambio = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdAutorizacionTipoCambio");
    pOrdenCompra.IdAutorizacionTipoCambio = validaNumero(idautorizaciontipocambio) ? idautorizaciontipocambio : 0;

    pOrdenCompra.FechaRequerida = $("#txtFechaEntrega").val();
    pOrdenCompra.SubtotalDet    = QuitaFormatoMoneda(subtotalActual + totalCompra);
    pOrdenCompra.IVADet         = QuitaFormatoMoneda(ivaActual + IVATotal);
    pOrdenCompra.TotalDet       = QuitaFormatoMoneda(totalActual + (totalCompra + IVATotal));
    pOrdenCompra.SaldoDet       = QuitaFormatoMoneda(totalActual + (totalCompra + IVATotal));
    pOrdenCompra.IdTipoMoneda   = $("#cmbTipoMoneda").val();
    pOrdenCompra.IdDivision     = $("#cmbDivision").val();
    pOrdenCompra.FechaAlta      = $("#txtFecha").text();
    pOrdenCompra.DireccionEntrega = $("#txtEntrega").val();
    pOrdenCompra.Nota           = $("#txtNota").val();
    pOrdenCompra.TipoCambio     = QuitaFormatoMoneda($("#TipoCambioActual").html());
    pOrdenCompra.Consolidado    = 0;
    pOrdenCompra.SinPedido = $("#chkCSPedido").attr("checked") ? 1 : 0;
    
    var Precio = parseFloat(pOrdenCompra.TotalDet);
    pOrdenCompra.CantidadTotalLetra = covertirNumLetras(Precio.toString(), $("#cmbTipoMoneda option:selected").text());

    var idproducto= $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto");
    pOrdenCompra.IdProducto = validaNumero(idproducto) ? idproducto : 0;

    var idservicio = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio");
    pOrdenCompra.IdServicio = validaNumero(idservicio) ? idservicio : 0;
    
    var idpedidodetalle = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCotizacionDetalle");
    pOrdenCompra.IdPedidoDetalle = validaNumero(idpedidodetalle) ? idpedidodetalle : 0;

    var idtipoiva = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("idTipoIVA");
    pOrdenCompra.IdTipoIVA = validaNumero(idtipoiva) ? idtipoiva : 0;
    
    var Total = $("#txtTotal").val();
    Total = Total.replace("$", "");
    Total = Total.replace(",", "");
    Total = parseFloat(Total);
    if (isNaN(Total)) {
        Total = 0.00;
    }
    
    pOrdenCompra.Costo      = QuitaFormatoMoneda($("#txtCosto").val());
    pOrdenCompra.Descuento  = $("#txtDescuento").val();
    pOrdenCompra.CostoDescuento = QuitaFormatoMoneda($("#txtCostoDescuento").val());
    pOrdenCompra.Cantidad       = $("#txtCantidad").val();
    pOrdenCompra.Total = Total;
    pOrdenCompra.ClaveProdServ = $("#txtClaveProdServ").val(); 
    pOrdenCompra.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pOrdenCompra.IdTipoCompra        = $("#cmbTipoCompra").val();
    pOrdenCompra.FechaAltaDetalle    = $("#txtFecha").text();
    pOrdenCompra.IdPedidoEncabezado  = $("#cmbPedido").val();
   
    var idcliente = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente");
    pOrdenCompra.IdCliente = validaNumero(idcliente) ? idcliente : 0;

    var idproyecto = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto");
    pOrdenCompra.IdProyecto = validaNumero(idproyecto) ? idproyecto : 0;

    pOrdenCompra.DescripcionDetallePartida = $("#txtDescripcionDetallePartida").val();

    var validacion = ValidaOrdenCompraDetalle(pOrdenCompra);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }

    var oRequest = new Object();
    oRequest.pOrdenCompra = pOrdenCompra;
    SetAgregarOrdenCompraDetalle(JSON.stringify(oRequest));
}

function ProporcionarClaveAutorizacion() {
    var IdTipoMonedaProducto = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdTipoMonedaProducto");
    var IdTipoMonedaServicio = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdTipoMonedaServicio");
    var IdTipoMonedaDestino = IdTipoMonedaServicio == "0" ? IdTipoMonedaProducto : IdTipoMonedaServicio;
    var pRequest = new Object();

    pRequest.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
    pRequest.IdTipoMonedaDestino = IdTipoMonedaDestino == undefined ? "0" : IdTipoMonedaDestino;
    pRequest.ClaveAutorizacion = $("#txtClaveAutorizacion").val();
    
    var iddocumento = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    pRequest.IdDocumento = validaNumero(iddocumento) ? iddocumento : 0;

    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/ObtenerAutorizacionTipoCambio",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Table[0] != undefined) {
                $("#TipoCambioActual").html(formato.moneda(respuesta.Table[0].TipoCambio, '$', 4));
                $('#divPrecio').html(formato.moneda(Modelo.Costo, '$'));
                
                $("#txtCosto").val(formato.moneda(Modelo.Costo / respuesta.Table[0].TipoCambio, '$'));               
                RecalculaDatosCosto(); 
                RecalculaDatosCostoDescuento();
                RecalculaDatosCantidad();
               
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdAutorizacionTipoCambio", respuesta.Table[0].IdAutorizacionTipoCambio);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("ClaveAutorizacion", respuesta.Table[0].ClaveAutorizacion);
                //                DesactivaClaveAutorizacion(JSON.stringify(pRequest));
            }
        },
        complete: function() {
            $("#dialogProporcionarClaveAutorizacion").dialog("close");
            //$("#divTipoCambio").hide();
        }
    });
}

function DesactivaClaveAutorizacion(pRequest) {
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/DesactivaAutorizacionTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
        },
        complete: function() {            
        }
    });
}

function CancelarOrdenCompra(dialog) {
    var IdOrdenCompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    var baja = "true";
    if (IdOrdenCompra != 0 && IdOrdenCompra != "0") {
        SetCambiarEstatus(IdOrdenCompra, baja);
        $(dialog).dialog("close")
    } else {
        MostrarMensajeError("No se puede cancelar la orden de compra.");
    }
}

function Imprimir(IdOrdenCompra) {
	var OrdenCompra = new Object();
	OrdenCompra.IdOrdenCompra = IdOrdenCompra;
	var Request = JSON.stringify(OrdenCompra);
	var contenedor = $("<div></div>");
	$(contenedor).obtenerVista({
		url: "OrdenCompra.aspx/ImprimirOrdenCompra",
		parametros: Request,
		nombreTemplate: "tmplImprimirOrdenCompra.html",
		despuesDeCompilar: function (Respuesta) {
			var plantilla = $(contenedor).html();
			var Impresion = window.open("", "_blank");
			Impresion.document.write(plantilla);
			Impresion.print();
			Impresion.close();
			$(contenedor).remove();
		}
	});
}

function CambiarPrecio() {
    var pDato = new Object();

    pDato.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();

    if (pDato.IdTipoMonedaOrigen != 0) {
        var IdTipoMonedaProducto = $("#txtMonedaProducto").attr("tipomonedaid");
        var IdTipoMonedaServicio = $("#txtMonedaServicio").attr("tipomonedaid");
        var IdTipoMonedaOrigen = IdTipoMonedaServicio == "0" ? IdTipoMonedaProducto : IdTipoMonedaServicio;

        pDato.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
        pDato.IdTipoMonedaDestino = IdTipoMonedaOrigen == undefined ? "0" : IdTipoMonedaOrigen;

        pDato.Precio = QuitaFormatoMoneda($("#txtCosto").val()) != undefined && QuitaFormatoMoneda($("#txtCosto").val()) != null && QuitaFormatoMoneda($("#txtCosto").val()) != "" ? QuitaFormatoMoneda($("#txtCosto").val()) : 0;
         
        var oRequest = new Object();
        oRequest.pDato = pDato;
        SetPrecioPorMoneda(JSON.stringify(oRequest));
    }
}

function SetPrecioPorMoneda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/ObtenerPrecioPorMoneda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            Modelo = respuesta.Modelo;
            if (respuesta.Error == 0) {
                $("#txtCosto").val(formato.moneda(Modelo.MonedaPrecio, '$'));
                $("#TipoCambioActual").html(formato.moneda(Modelo.TipoCambioActual,'$', 4));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            RecalculaDatosCosto();
            RecalculaDatosCostoDescuento();
            RecalculaDatosCantidad();

            OcultarBloqueo();
        }
    });
}

function SetAgregarOrdenCompra(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/AgregarOrdenCompra",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                
                $("#txtFolio").val(respuesta.Folio);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra", respuesta.IdOrdenCompraEncabezado);
                $("#grdOrdenCompra").trigger("reloadGrid");
                $("#grdOrdenCompraDetalle").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleConsultar").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleEditar").trigger("reloadGrid");

                LimpiarDatosOrdenCompraDetalle();
                DeshabilitaCamposEncabezado();
               
                ObtenerOrdenCompraIndicadores();

                MostrarMensajeError("Se ha agregado la orden de compra con folio: " + respuesta.Folio);
                $("#dialogAgregarOrdenCompra").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();            
            $("#grdOrdenCompra").trigger("reloadGrid");
        }
    });
}

function SetEditarOrdenCompra(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/EditarOrdenCompra",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {

                $("#txtFolio").val(respuesta.Folio);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra", respuesta.IdOrdenCompraEncabezado);
                $("#grdOrdenCompra").trigger("reloadGrid");
                $("#grdOrdenCompraDetalle").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleConsultar").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleEditar").trigger("reloadGrid");

                LimpiarDatosOrdenCompraDetalle();
                DeshabilitaCamposEncabezado();

                MostrarMensajeError("Se ha editado la orden de compra con folio: " + respuesta.Folio);
                $("#dialogEditarOrdenCompra").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#grdOrdenCompra").trigger("reloadGrid");
        }
    });
}

function SetConsolidarOrdenCompra(pRequest, dialog) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/ConsolidarOrdenCompra",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var IdOrdenCompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");

                if (IdOrdenCompra != "0" && IdOrdenCompra != "") {
                    BloquearPantalla(dialog, "color:Lime; font-size:large; font-weight:bold;", "Consolidado");
                    $(dialog).dialog("close");
                }
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#grdOrdenCompra").trigger("reloadGrid");
            $("#grdOrdenCompraDetalle").trigger("reloadGrid");
            $("#grdOrdenCompraDetalleConsultar").trigger("reloadGrid");
            $("#grdOrdenCompraDetalleEditar").trigger("reloadGrid");
        }
    });
}

function SetAgregarOrdenCompraDetalle(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/AgregarOrdenCompraDetalle",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtFolio").val(respuesta.Folio);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra", respuesta.IdOrdenCompraEncabezado);
                $("#grdOrdenCompra").trigger("reloadGrid");
                $("#grdOrdenCompraDetalle").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleConsultar").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleEditar").trigger("reloadGrid");

                $("#txtSubtotalDetalle").text(formato.moneda(respuesta.SubtotalDetalle,'$'));
                $("#txtIVADetalle").text(formato.moneda(respuesta.IVADetalle,'$'));
                $("#txtTotalDetalle").text(formato.moneda(respuesta.TotalDetalle, '$'));

                $("#txtSubtotalDetalleEditar").text(formato.moneda(respuesta.SubtotalDetalle, '$'));
                $("#txtIVADetalleEditar").text(formato.moneda(respuesta.IVADetalle, '$'));
                $("#txtTotalDetalleEditar").text(formato.moneda(respuesta.TotalDetalle, '$'));
                $("#txtDescripcionDetallePartida").val("");
               
                ObtenerOrdenCompraIndicadores();

                LimpiarDatosOrdenCompraDetalle();
                DeshabilitaCamposEncabezado();

                $("#txtCliente").attr("disabled", true);
                $("#txtProyecto").attr("disabled", true);                
                $("#cmbPedido").attr("disabled", true);
                $("#chkCSPedido").attr("disabled", true);
                $('input[name=ClienteProyecto]').attr("disabled", true);
//                $('input[name=ProductoServicio]').attr("disabled", true);

                var Precio = parseFloat(respuesta.TotalDetalle);
                $("#spanCantidadLetra").text(covertirNumLetras(Precio.toString(), $("#cmbTipoMoneda option:selected").text()));
                $("#spanCantidadLetraEditar").text(covertirNumLetras(Precio.toString(), $("#cmbTipoMoneda option:selected").text()));
                $("#divTipoCambio").hide();
    
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

function SetEliminarOrdenCompraDetalle(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/EliminarOrdenCompraDetalle",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOrdenCompraDetalleEditar").trigger("reloadGrid");
                $("#grdOrdenCompraDetalleConsultar").trigger("reloadGrid");
                $("#grdOrdenCompraDetalle").trigger("reloadGrid");
                $("#grdOrdenCompra").trigger("reloadGrid");
                var detalle = $("#grdOrdenCompraDetalle, #grdOrdenCompraDetalleConsultar, #grdOrdenCompraDetalleEditar").jqGrid('getGridParam', 'records');
                if (detalle == 1) {
                    $("#divTipoCambio").show();
                }

                $("#txtSubtotalDetalle").text(formato.moneda(respuesta.SubtotalDetalle, '$'));
                $("#txtIVADetalle").text(formato.moneda(respuesta.IVADetalle, '$'));
                $("#txtTotalDetalle").text(formato.moneda(respuesta.TotalDetalle, '$'));

                $("#txtSubtotalDetalleEditar").text(formato.moneda(respuesta.SubtotalDetalle, '$'));
                $("#txtIVADetalleEditar").text(formato.moneda(respuesta.IVADetalle, '$'));
                $("#txtTotalDetalleEditar").text(formato.moneda(respuesta.TotalDetalle, '$'));

                var Precio = parseFloat(respuesta.TotalDetalle);
                $("#spanCantidadLetra").text(covertirNumLetras(Precio.toString(), $("#cmbTipoMoneda option:selected").text()));
                $("#spanCantidadLetraEditar").text(covertirNumLetras(Precio.toString(), $("#cmbTipoMoneda option:selected").text()));

                LimpiarDatosOrdenCompraDetalle();
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

function SetCambiarEstatus(pIdOrdenCompra, pBaja) {
    var pRequest = "{'pIdOrdenCompraEncabezado':" + pIdOrdenCompra + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);

            if (respuesta.Error == 1) {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {            
            ObtenerOrdenCompraIndicadores();
            $("#grdOrdenCompra").trigger("reloadGrid");
        }
    });
}

function obtenerProducto(pRequest){
    $.ajax({
        type: "POST",
        url: "OrdenCompra.aspx/obtenerProducto",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Modelo = respuesta.Modelo;

                $("#txtDescripcionDetallePartida").val(Modelo.Descripcion);
                $("#txtClaveProdServ").val(Modelo.ClaveProdServ);

                var pTipoCambio = new Object();
                pTipoCambio.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
                pTipoCambio.IdTipoMonedaDestino = Modelo.IdTipoMonedaProducto;

                var idordencompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
                pTipoCambio.IdOrdenCompra = validaNumero(idordencompra) ? idordencompra : 0;

                var claveautorizacion = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("ClaveAutorizacion");
                pTipoCambio.ClaveAutorizacion = claveautorizacion == undefined ? "" : claveautorizacion;

                $("#cmbUnidadCompraVenta option[value=" + Modelo.IdUnidadCompraVenta + "]").attr("selected", true);
                $("#cmbUnidadCompraVenta").attr("disabled", "true");

                $('#divPrecio').html('Precio: ' + formato.moneda(Modelo.Costo, '$') + ' ' + validaPluralMoneda(Modelo.TipoMonedaProducto));
                ObtenerTipoCambio(JSON.stringify(pTipoCambio));

                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("idTipoIVA", Modelo.IdTipoIVA);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("iVA", Modelo.IVA);

                $("#txtCosto").val(formato.moneda(Modelo.Costo, '$'));
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdTipoMonedaProducto", Modelo.IdTipoMonedaProducto);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdTipoMonedaServicio", "0");
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
        url: "OrdenCompra.aspx/obtenerServicio",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Modelo = respuesta.Modelo;

                $("#txtDescripcionDetallePartida").val(Modelo.Descripcion);
                $("#txtClaveProdServ").val(Modelo.ClaveProdServ);

                var pTipoCambio = new Object();
                pTipoCambio.IdTipoMonedaOrigen = $("#cmbTipoMoneda").val();
                pTipoCambio.IdTipoMonedaDestino = Modelo.IdTipoMonedaServicio;

                var idordencompra = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
                pTipoCambio.IdOrdenCompra = validaNumero(idordencompra) ? idordencompra : 0;

                var claveautorizacion = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("ClaveAutorizacion");
                pTipoCambio.ClaveAutorizacion = claveautorizacion == undefined ? "" : claveautorizacion;

                $("#cmbUnidadCompraVenta option[value=" + Modelo.IdUnidadCompraVenta + "]").attr("selected", true);
                $("#cmbUnidadCompraVenta").attr("disabled", "true");

                $('#divPrecio').html('Precio: ' + formato.moneda(Modelo.Costo, '$') + ' ' + validaPluralMoneda(Modelo.TipoMonedaServicio));
                ObtenerTipoCambio(JSON.stringify(pTipoCambio));

                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("idTipoIVA", Modelo.IdTipoIVA);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("iVA", Modelo.IVA);

                $("#txtCosto").val(formato.moneda(Modelo.Costo, '$'));
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdTipoMonedaServicio", Modelo.IdTipoMonedaServicio);
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdTipoMonedaProducto", "0");

                RecalculaDatosCosto();
                RecalculaDatosCostoDescuento();
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

function obtenerPedidosCliente(pRequest) {
    $("#cmbPedido").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "OrdenCompra.aspx/obtenerPedidosCliente",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}



function AutocompletarProveedor() {

    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtRazonSocial').val();
            $.ajax({
                type: 'POST',
                url: 'OrdenCompra.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProveedor", "0");
                    
                    var json = jQuery.parseJSON(pRespuesta.d);

                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdProveedor, IVAProveedor: item.IVAActual }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdProveedor = ui.item.id;
            var pIVAProveedor = ui.item.IVAProveedor;
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProveedor", pIdProveedor);
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IVAProveedor", pIVAProveedor);          
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
                url: 'OrdenCompra.aspx/BuscarProducto',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
                    $("#txtDescripcionProducto").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
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
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            var pProducto   = ui.item.descripcion;
            
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", pIdProducto);
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
            $("#txtDescripcionProducto").val(pProducto);
            
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
                url: 'OrdenCompra.aspx/BuscarProducto',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
                    $("#txtClaveProducto").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        if (pProducto.TipoBusqueda == 'P') {
                            return { label: item.Producto, value: item.Producto, id: item.IdProducto, clave: item.Clave }
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
            var pProducto   = ui.item.value;
            var pClave      = ui.item.clave;
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", pIdProducto);
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
            $("#txtClaveProducto").val(pClave);
            $("#txtDescripcionProducto").val(pProducto);
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
                url: 'OrdenCompra.aspx/BuscarServicio',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
                    $("#txtDescripcionServicio").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
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
        select: function(event, ui) {
            var pIdServicio = ui.item.id;
            var pServicio   = ui.item.descripcion;
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", pIdServicio);

            $("#txtDescripcionServicio").val(pServicio);

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
                url: 'OrdenCompra.aspx/BuscarServicio',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
                $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
                    $("#txtClaveServicio").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
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
        select: function(event, ui) {
            var pIdServicio = ui.item.id;
            var pClave    = ui.item.clave;
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", pIdServicio);

            $("#txtClaveServicio").val(pClave);

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
                url: 'OrdenCompra.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente", "0");
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto", "0");
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
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente", pIdCliente);
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto", "0");

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
                url: 'OrdenCompra.aspx/BuscarProyectoCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente", "0");
                    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto", "0");
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
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdCliente", "0");
            $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProyecto", pIdProyecto);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function RecalculaDatosCosto() {
    var Costo = 0;
    var CostoDescuento = 0;
    var Descuento = 0;
    var Total = 0;
    var Cantidad = 0;
    Costo       = QuitaFormatoMoneda($("#txtCosto").val());
    Descuento   = $("#txtDescuento").val();
    Cantidad    = $("#txtCantidad").val();
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
    $("#txtCostoDescuento").val(formato.moneda(CostoDescuento, '$'));
    Total = parseFloat(Cantidad) * parseFloat(CostoDescuento);
    $("#txtTotal").val(formato.moneda(Total,'$'));
}

function RecalculaDatosCostoDescuento() {
    var Costo = 0;
    var CostoDescuento = 0;
    var Descuento = 0;
    var Total = 0;
    var Cantidad = 0;
    Costo           = QuitaFormatoMoneda($("#txtCosto").val());
    Cantidad        = $("#txtCantidad").val();
    CostoDescuento  = QuitaFormatoMoneda($("#txtCostoDescuento").val());
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
//    $("#txtDescuento").val(Descuento);
    Total = parseFloat(Cantidad) * parseFloat(CostoDescuento);
    $("#txtTotal").val(formato.moneda(Total, '$'));
}

function RecalculaDatosCantidad() {
    var Costo = 0;
    var CostoDescuento = 0;
    var Descuento = 0;
    var Total = 0;
    var Cantidad = 0;
    Costo           = QuitaFormatoMoneda($("#txtCosto").val());
    Cantidad        = $("#txtCantidad").val();
    CostoDescuento  = QuitaFormatoMoneda($("#txtCostoDescuento").val());
    if (Cantidad == "") {
        Cantidad = 0;
    }
    if (CostoDescuento == "") {
        CostoDescuento = 0;
    }
    Total = parseFloat(Cantidad) * parseFloat(CostoDescuento);
    $("#txtTotal").val(formato.moneda(Total, '$'));
}


function FiltroOrdenCompra() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOrdenCompra').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOrdenCompra').getGridParam('page');
    request.pColumnaOrden = $('#grdOrdenCompra').getGridParam('sortname');
    request.pTipoOrden = $('#grdOrdenCompra').getGridParam('sortorder');
    request.pRazonSocial = "";
    request.pAgente = "";
    request.pFolio = "";
    request.pIdEstatusRecepcion = -1;
    request.pAI = 0;
    request.pAsociado = -1;
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pFolioPedido = 0;
    request.pNoProyecto = 0;

    if ($("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != null && $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado") != "") {
        request.pIdEstatusRecepcion = $("#tblTotalesEstatus").attr("idEstatusRecepcionSeleccionado");
    }

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }

    if ($('#gs_Agente').val() != null) { request.pAgente = $("#gs_Agente").val(); }

    if ($("#gs_Asociada").val() != null) { request.pAsociado = $("#gs_Asociada").val() }
    
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
    var filtroCombo = parseInt($("#cmbBusquedaDocumento").val());
    switch(filtroCombo){
        case 1:
            request.pFolioPedido = ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null)? parseInt($("#txtNumeroPedidoBuscador").val()) : 0;
        break;
        case 2:
            request.pNoProyecto = ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null)? parseInt($("#txtNumeroPedidoBuscador").val()) : 0;
        break;
        default:
        break;
    }


    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'OrdenCompra.aspx/ObtenerOrdenCompra',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdOrdenCompra')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroOrdenCompraDetalle() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOrdenCompraDetalle').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOrdenCompraDetalle').getGridParam('page');
    request.pColumnaOrden = $('#grdOrdenCompraDetalle').getGridParam('sortname');
    request.pTipoOrden = $('#grdOrdenCompraDetalle').getGridParam('sortorder');
    request.pIdOrdenCompraEncabezado = 0;
    if ($("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra") != null && $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra") != "") {
        request.pIdOrdenCompraEncabezado = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'OrdenCompra.aspx/ObtenerOrdenCompraDetalle',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdOrdenCompraDetalle')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroOrdenCompraDetalleConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOrdenCompraDetalleConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOrdenCompraDetalleConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdOrdenCompraDetalleConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdOrdenCompraDetalleConsultar').getGridParam('sortorder');
    request.pIdOrdenCompraEncabezado = 0;
    if ($("#divFormaAgregarOrdenCompra, #divFormaConsultarOrdenCompra").attr("IdOrdenCompra") != null && $("#divFormaAgregarOrdenCompra, #divFormaConsultarOrdenCompra").attr("IdOrdenCompra") != "") {
        request.pIdOrdenCompraEncabezado = $("#divFormaAgregarOrdenCompra, #divFormaConsultarOrdenCompra").attr("IdOrdenCompra");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'OrdenCompra.aspx/ObtenerOrdenCompraDetalle',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdOrdenCompraDetalleConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroOrdenCompraDetalleEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOrdenCompraDetalleEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOrdenCompraDetalleEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdOrdenCompraDetalleEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdOrdenCompraDetalleEditar').getGridParam('sortorder');
    request.pIdOrdenCompraEncabezado = 0;
    if ($("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra") != null && $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra") != "") {
        request.pIdOrdenCompraEncabezado = $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdOrdenCompra");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'OrdenCompra.aspx/ObtenerOrdenCompraDetalle',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdOrdenCompraDetalleEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
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

    if ($("#cmbPedido").val() != null && $("#cmbPedido").val() != "") {
        request.pIdPedido = $("#cmbPedido").val();
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'OrdenCompra.aspx/ObtenerDetallePedido',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetallePedido')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//----------Validaciones----------//
//--------------------------//

function ValidaOrdenCompra(pOrdenCompra) {
    var errores = "";

    if (pOrdenCompra.IdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

    if (pOrdenCompra.IdDivision == 0)
    { errores = errores + "<span>*</span> No hay una división seleccionda, favor de elegir alguno.<br />"; }

    if (pOrdenCompra.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; }

    if (pOrdenCompra.FechaRequerida == "")
    { errores = errores + "<span>*</span> El campo fecha requerida esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaOrdenCompraDetalle(pOrdenCompraDetalle) {
    var errores = "";

    if (pOrdenCompraDetalle.IdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

    if (pOrdenCompraDetalle.IdDivision == 0)
    { errores = errores + "<span>*</span> No hay una división seleccionda, favor de elegir alguno.<br />"; }

    if (pOrdenCompraDetalle.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; }

    if (pOrdenCompraDetalle.FechaRequerida == "")
    { errores = errores + "<span>*</span> El campo fecha requerida esta vacío, favor de capturarlo.<br />"; }

    if ($("#chkCSPedido").attr("checked") == false && pOrdenCompraDetalle.IdPedidoDetalle == "0")
    { errores = errores + "<span>*</span> Es necesario seleccionar una partida del pedido, favor de capturarlo.<br />"; }

    if (vClienteProyecto == "2" && pOrdenCompraDetalle.IdProyecto == "0")
    { errores = errores + "<span>*</span> El proyecto esta vacío, favor de capturarlo.<br />"; }

    if (pOrdenCompraDetalle.IdUnidadCompraVenta == "0")
    { errores = errores + "<span>*</span> El unidad de compra venta esta vacío, favor de capturarlo.<br />"; }

    if (pOrdenCompraDetalle.IdTipoCompra == "0")
    { errores = errores + "<span>*</span> El tipo de compra venta esta vacío, favor de capturarlo.<br />"; }

    if (pOrdenCompraDetalle.Cantidad == "")
    { errores = errores + "<span>*</span> El cantidad esta vacío, favor de capturarlo.<br />"; }

    if (pOrdenCompraDetalle.ClaveProdServ == "")
    { errores = errores + "<span>*</span> La Clave (SAT) esta vacío, favor de capturarlo.<br />"; }

    if (pOrdenCompraDetalle.IdProducto == "0" && pOrdenCompraDetalle.IdServicio == "0")
    { errores = errores + "<span>*</span> El producto ó servicio esta vacío, favor de capturarlo.<br />"; }

    //if (pOrdenCompraDetalle.Total == "0")
    //{ errores = errores + "<span>*</span> El total esta vacío, favor de capturarlo.<br />"; }

    if (pOrdenCompraDetalle.DescripcionDetallePartida == "")
    { errores = errores + "<span>*</span> La descripción de la partida esta vacía, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

//---------- Utilerias ---------//

function DeshabilitaCamposEncabezado() {
    $("#txtRazonSocial").attr("disabled", true);
    $("#cmbDivision").attr("disabled", true);
    $("#cmbTipoMoneda").attr("disabled", true);
    $("#txtFechaEntrega").attr("disabled", true);
    $("#txtEntrega").attr("disabled", true);
    $("#txtNota").attr("disabled", true);
    $("#txtNumeroGuia").attr("disabled", true);
    //$('input[name=ClienteProyecto]').attr("disabled", true);
    $("#divImprimir").show();
}

function LimpiarDatosOrdenCompraDetalle() {
    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
    $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");
    $("#txtCosto").val("$0.00");
    $("#txtClaveProducto").val("");
    $("#txtClaveServicio").val("");
    $("#txtDescripcionProducto").val("");
    $("#txtDescripcionServicio").val("");
    $("#txtDescuento").val("0");
    $("#txtCostoDescuento").val("$0.00");
    $("#txtCantidad").val("1");
    $("#txtTotal").val("$0.00");
    $("#divPrecio").html("");
    $("#txtClaveProdServ").val("");
}

function Enter(evento) {
    var key = evento.which || evento.keyCode;
    if ((key == 13)) {
        ProporcionarClaveAutorizacion();
        return false;
    }
}

function MuestraObjetosSinPedido() {
    if ($("#chkCSPedido").attr("checked")) 
    {
        $("#txtClaveProducto").attr("disabled", false);
        $("#txtDescripcionProducto").attr("disabled", false);
        

        $("#txtClaveServicio").attr("disabled", false);
        $("#txtDescripcionServicio").attr("disabled", false);

        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");

        $("#cmbPedido").attr("disabled", true);
        
        $("#divBuscarPedidosCliente").css("display", "none");

        $("#chkCSPedido").attr("disabled", true)

        $('input[name=ProductoServicio]').attr("disabled", false);
    }
    else {
        $("#txtClaveProducto").attr("disabled", true);
        $("#txtDescripcionProducto").attr("disabled", true);
        
        
        $("#txtClaveServicio").attr("disabled", true);
        $("#txtDescripcionServicio").attr("disabled", true);

        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdProducto", "0");
        $("#divFormaAgregarOrdenCompra, #divFormaEditarOrdenCompra").attr("IdServicio", "0");

        $("#cmbPedido").attr("disabled", false);
        
        $("#divBuscarPedidosCliente").css("display", "block");

        $("#chkCSPedido").attr("disabled", false)
        
        $('input[name=ProductoServicio]').attr("disabled", true);
    }
}

function BloquearPantalla(div, estiloAd, contenido) {
    if (contenido != "Normal") {
        $(div).fadeTo('slow', .6);
        $(div).append('<div style="position: absolute;top:0;left:0;width: 100%;height:90%;z-index:2;opacity:0.4;filter: alpha(opacity = 50); text-align: center; vertical-align: middle;' + estiloAd + '">' + contenido + '</div>');
    }
    else {
        $(div).removeAttr("style");
    }
}

function ObtenerListaTipoCompra() {
    $("#cmbTipoCompra").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "OrdenCompra.aspx/ObtenerListaTipoCompra"
    });
}
