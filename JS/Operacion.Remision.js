//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    //SetFormaFiltrosRemision();
    //obtenerComboAlmacenRemision();
    ObtenerTotalesEstatusRemision();

    ObtenerFormaFiltrosEncabezadoRemision();

    //////funcion del grid//////
    $("#gbox_grdEncabezadoRemision").livequery(function() {
        $("#grdEncabezadoRemision").jqGrid('navButtonAdd', '#pagEncabezadoRemision', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pFolio = "";
                var pIdEstatusRemision = -1;
                var pAI = 0;

                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;
                var pNumeroSerie = "";
                var pNumeroPedido = 0;
                var pClave = "";
                
                if ($("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado") != null && $("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado") != "") {
                    pIdEstatusRemision = $("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado");
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

                if ($("#txtNumeroSerieBuscador").val() != "" && $("#txtNumeroSerieBuscador").val() != null) {
                    pNumeroSerie = $("#txtNumeroSerieBuscador").val();
                }

                if ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null) {
                    pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
                }

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pRazonSocial: pRazonSocial,
                    pFolio: pFolio,
                    pIdEstatusRemision: pIdEstatusRemision,
                    pAI: pAI,
                    pFechaInicial: pFechaInicial,
                    pFechaFinal: pFechaFinal,
                    pPorFecha: pPorFecha,
                    pNumeroSerie: pNumeroSerie,
                    pNumeroPedido: pNumeroPedido

                }, downloadType: 'Normal'
                });

            }
        });
    });
    ///////////////////////////

    $("#btnObtenerFormaAgregarRemision").livequery('click', function() {
        ObtenerFormaSeleccionarAlmacen();
    });

    $('#grdEncabezadoRemision').one('click', '.div_grdEncabezadoRemision_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdEncabezadoRemision_AI']").children().attr("baja")
        var idRemision = $(registro).children("td[aria-describedby='grdEncabezadoRemision_IdEncabezadoRemision']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        if (baja == "true") {
            SetCambiarEstatus(idRemision, baja);
        }
        else {
            SetActivarEstatus(idRemision, baja);
        }
    });

    $("#grdEncabezadoRemision").on("click", ".imgFormaConsultarEncabezadoRemision", function() {
        var registro = $(this).parents("tr");
        var EncabezadoRemision = new Object();
        EncabezadoRemision.pIdEncabezadoRemision = parseInt($(registro).children("td[aria-describedby='grdEncabezadoRemision_IdEncabezadoRemision']").html());
        ObtenerFormaConsultarEncabezadoRemision(JSON.stringify(EncabezadoRemision));
    });

    $("#btnRecargarGrdDetalleProducto").livequery('click', function() {
        limpiaFiltrosGridDetalleProductoGeneral();
        $("#grdDetalleProducto").trigger("reloadGrid");
    });

    $(".imgRemisionarProducto").livequery('click', function() {
        var registro = $(this).parents("tr");
        var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
        $("#chkTodos").attr('checked', false);
        if (IdAlmacenProducto != 0) {
            var registro = $(this).parents("tr");
            var Producto = new Object();
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html()));
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdDetalleFacturaProveedor', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html()));
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdAlmacen', IdAlmacenProducto);
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdExistenciaDistribuida', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html()));
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdPedidoDetalle', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html()));
            $("#txtCantidad").val(parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html()));
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("cantidadPedido", parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html()));
            $("#txtCosto").val($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
            $("#txtPrecio").val($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
            $("#cmbTipoMonedaOrigen option[value=" + parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html()) + "]").attr("selected", true);

            var pTipoCambio = new Object();
            pTipoCambio.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
            pTipoCambio.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html());

            var oRequest = new Object();
            oRequest.pTipoCambio = pTipoCambio;
            ObtenerTipoCambioDetalle(JSON.stringify(oRequest))

        } else {
            MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
        }
    });

    $("#btnVerRemision").livequery('click', function() {
        obtenerFormaDetallePedido();
    });

    $("#grdDetallePedido").on("click", ".imgSeleccionarDetallePedido", function() {
        var registro = $(this).parents("tr");
        var pIdProducto = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdProducto']").html());
        var pIdPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdCotizacionDetalle']").html());
        var pSaldo = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_Saldo']").html());
        if (pSaldo != 0) {
            LlenarDetalleProducto(pIdProducto, pIdPedidoDetalle);
            MuestraObjetosElegirTodos();
        } else {
        }

    });

    $("#grdDetallePedido").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var pIdProducto = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdProducto']").html());
        var pIdPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_IdCotizacionDetalle']").html());
        var pSaldo = parseInt($(registro).children("td[aria-describedby='grdDetallePedido_Saldo']").html());
        if (pSaldo != 0) {
            LlenarDetalleProducto(pIdProducto, pIdPedidoDetalle);
            MuestraObjetosElegirTodos();
        } else {

        }
    });

    $("#btnAgregarItem").livequery('click', function() {
        AgregarDetalleProductoRemision();
    });

    $('#dialogAgregarRemision, #dialogEditarEncabezadoRemision').on('change', '#cmbTipoMoneda', function(event) {
        //$("#cmbTipoMonedaOrigen").attr("disabled", "true");
        var pTipoCambio = new Object();
        pTipoCambio.IdTipoCambioOrigen = parseInt($("#cmbTipoMoneda").val());
        pTipoCambio.IdTipoCambioDestino = parseInt($("#cmbTipoMonedaOrigen").val());
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambio;
        ObtenerTipoCambioDetalle(JSON.stringify(oRequest))

    });

    $('#dialogAgregarRemision, #dialogEditarEncabezadoRemision').on('focusout', '#txtPrecio', function(event) {
        var pTipoCambio = new Object();
        pTipoCambio.IdTipoCambioOrigen = parseInt($("#cmbTipoMoneda").val());
        pTipoCambio.IdTipoCambioDestino = parseInt($("#cmbTipoMonedaOrigen").val());
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambio;
        ObtenerTipoCambioDetalle(JSON.stringify(oRequest))
    });

    $("#dialogAgregarRemision, #dialogConsultarEncabezadoRemision").on("click", "#divImprimir", function() {

        var pIdEncabezadoRemision = $("#divAgregarRemision, #divFormaConsultarEncabezadoRemision, #divFormaEditarEncabezadoRemision").attr("idEncabezadoRemision");
        if ($("#chkSinPrecio").is(':checked')) {
            pSinPrecio = 1;
        }
        else {
            pSinPrecio = 0;
        }
        Imprimir(pIdEncabezadoRemision, pSinPrecio);
    });

    $("#dialogEditarEncabezadoRemision").on("click", "#divImprimir", function() {

        var pIdEncabezadoRemision = $("#divFormaEditarEncabezadoRemision").attr("idEncabezadoRemision");
        if ($("#chkSinPrecioEditar").is(':checked')) {
            pSinPrecio = 1;
        }
        else {
            pSinPrecio = 0;
        }
        Imprimir(pIdEncabezadoRemision, pSinPrecio);
    });

    $(".spanFiltroTotal").click(function() {
        var idEstatusRemision = $(this).attr("IdEstatusRemision");
        $("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado", idEstatusRemision);

        $('#gs_Folio').val(null);
        $('#gs_NombreComercial').val(null);
        $('#gs_AI').val(null);
        $("#chkPorFecha").attr('checked', false);
        $('#txtNumeroSerieBuscador').val(null);
        $('#txtNumeroPedidoBuscador').val(null);
        
        FiltroEncabezadoRemision();
    });

    $('#dialogAgregarRemision').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: true,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("input[name=ClienteProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                } else {
                    MuestraObjetosClienteProyecto(0);
                }
            });

            $("#grdDetalleRemision").on("click", ".imgEliminarConceptoEditar", function() {
                var registro = $(this).parents("tr");
                var pDetalleRemision = new Object();
                pDetalleRemision.pIdDetalleRemision = parseInt($(registro).children("td[aria-describedby='grdDetalleRemision_IdDetalleRemision']").html());
                var oRequest = new Object();
                oRequest.pDetalleRemision = pDetalleRemision;
                SetEliminarDetalleRemision(JSON.stringify(oRequest));
            });

            $("#grdDetalleProducto").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
                $("#chkTodos").attr('checked', false);
                if (IdAlmacenProducto != 0) {
                    var registro = $(this).parents("tr");
                    var Producto = new Object();
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdDetalleFacturaProveedor', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdAlmacen', IdAlmacenProducto);
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdExistenciaDistribuida', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdPedidoDetalle', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html()));
                    $("#txtCantidad").val(parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("cantidadPedido", parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html()));
                    $("#txtCosto").val($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
                    $("#txtPrecio").val($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
                    $("#cmbTipoMonedaOrigen option[value=" + parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html()) + "]").attr("selected", true);

                    var pTipoCambio = new Object();
                    pTipoCambio.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
                    pTipoCambio.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html());

                    var oRequest = new Object();
                    oRequest.pTipoCambio = pTipoCambio;
                    ObtenerTipoCambioDetalle(JSON.stringify(oRequest))

                } else {
                    MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
                }
            });

        },
        close: function() {
            $("#divAgregarRemision").remove();
            $("#grdEncabezadoRemision").trigger("reloadGrid");
            ObtenerTotalesEstatusRemision();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogDetallePedido').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: true,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarEncabezadoRemision').dialog({
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
            $("#divFormaConsultarEncabezadoRemision").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarEncabezadoRemision').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("input[name=ClienteProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                } else {
                    MuestraObjetosClienteProyecto(0);
                }
            });
            $("#grdDetalleRemisionEditar").on("click", ".imgEliminarConceptoEditar", function() {

                var registro = $(this).parents("tr");
                var pDetalleRemision = new Object();
                pDetalleRemision.pIdDetalleRemision = parseInt($(registro).children("td[aria-describedby='grdDetalleRemisionEditar_IdDetalleRemision']").html());
                var oRequest = new Object();
                oRequest.pDetalleRemision = pDetalleRemision;
                SetEliminarDetalleRemision(JSON.stringify(oRequest));
            });

            $("#grdDetalleProducto").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
                if (IdAlmacenProducto != 0) {
                    var registro = $(this).parents("tr");
                    var Producto = new Object();
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdDetalleFacturaProveedor', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdAlmacen', IdAlmacenProducto);
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdExistenciaDistribuida', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdPedidoDetalle', parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html()));
                    $("#txtCantidad").val(parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html()));
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("cantidadPedido", parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html()));
                    $("#txtCosto").val($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
                    $("#txtPrecio").val($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
                    $("#cmbTipoMonedaOrigen option[value=" + parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html()) + "]").attr("selected", true);

                    var pTipoCambio = new Object();
                    pTipoCambio.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
                    pTipoCambio.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html());

                    var oRequest = new Object();
                    oRequest.pTipoCambio = pTipoCambio;
                    ObtenerTipoCambioDetalle(JSON.stringify(oRequest))

                } else {
                    MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
                }
            });
        },
        close: function() {
            $("#divFormaEditarEncabezadoRemision").remove();
        },
        buttons: {
            "Editar": function() {
                EditarEncabezadoRemision();
            },
            "Salir": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogCancelarRemision").dialog({
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
                CancelarEncabezadoRemision();
                $(this).dialog("close");
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogSeleccionarAlmacenRemision').dialog({
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
                    obtenerFormaAgregarRemision(JSON.stringify(request));
                    $("#dialogSeleccionarAlmacenRemision").dialog("close");
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
});

function ObtenerTipoCambioDetalle(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Remision.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtTipoCambioDetalle").val(respuesta.Modelo.TipoCambioActual);
                $("#txtTipoCambioRemision").val(respuesta.Modelo.TipoCambioRemision);
                RecalculaMontoConTipoCambio();
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

function RecalculaMontoConTipoCambio() {

    var TotalTipoCambio;
    var TipoCambioFactura;
    TipoCambioFactura = $("#txtTipoCambioRemision").val();
    if (TipoCambioFactura == 0) {
        TipoCambioFactura = 1;
    }
    if (respuesta.Modelo.TipoCambioActual == 0) {
        respuesta.Modelo.TipoCambioActual = 1;
    }
    TotalTipoCambio = (QuitarFormatoNumero($("#txtPrecio").val()) / $("#txtTipoCambioDetalle").val());
    $("#txtPrecioConTipoCambio").val(formato.moneda(TotalTipoCambio.toFixed(2), '$'));
    
}

function SetPrecioPorMoneda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Remision.aspx/ObtenerPrecioPorMoneda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            Modelo = respuesta.Modelo;
            if (respuesta.Error == 0) {

                $("#txtPrecio").val(formato.moneda(Modelo.MonedaPrecio));
                $("#TipoCambioActual").html(Modelo.TipoCambioActual)
                var request = new Object();
                request.ListaCombo = Modelo.ListaTipoMoneda;
                ObtenerListaTipoMoneda(request);
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
        url: "Remision.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#TipoCambioActual").html(respuesta.Modelo.TipoCambioActual);
                var tipocambio = parseFloat(respuesta.Modelo.TipoCambioActual);
                var valor = formato.moneda((parseFloat($("#txtPrecio").val()) / tipocambio), '');
                $("#txtPrecio").val(valor);
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

function MuestraObjetosClienteProyecto(opcion) {
    if (opcion == 1) {
        $("#txtNombreComercial").css("display", "block");
        $("#txtProyecto").css("display", "none");
        $("#txtProyecto").val("");       

        $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProyecto", "0");
        $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idCliente", "0");
    }
    else {
        $("#txtNombreComercial").css("display", "none");
        $("#txtProyecto").css("display", "block");
        $("#txtNombreComercial").val("");
        $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProyecto", "0");
        $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idCliente", "0");
    }
}

function MuestraObjetosElegirTodos() {
    $("#EtiquetaTodos").css("display", "block");
    $("#chkTodos").css("display", "block");
}

function OcultaObjetosElegirTodos() {
    $("#EtiquetaTodos").css("display", "none");
    $("#chkTodos").css("display", "none");
}

function SetCambiarEstatus(pIdRemision, pBaja) {
    var pRequest = "{'pIdRemision':" + pIdRemision + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Remision.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoRemision").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
                ObtenerTotalesEstatusRemision();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            $('#grdEncabezadoRemision').one('click', '.div_grdEncabezadoRemision_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEncabezadoRemision_AI']").children().attr("baja")
                var idRemision = $(registro).children("td[aria-describedby='grdEncabezadoRemision_IdEncabezadoRemision']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                if (baja == "true") {
                    SetCambiarEstatus(idRemision, baja);
                }
                else {
                    SetActivarEstatus(idRemision, baja);
                }
            });
        }
    });
}

function SetActivarEstatus(pIdRemision, pBaja) {
    var pRequest = "{'pIdRemision':" + pIdRemision + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Remision.aspx/ActivarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {

            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoRemision").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
                ObtenerTotalesEstatusRemision();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }            
            
        },
        complete: function() {
            $('#grdEncabezadoRemision').one('click', '.div_grdEncabezadoRemision_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEncabezadoRemision_AI']").children().attr("baja")
                var idRemision = $(registro).children("td[aria-describedby='grdEncabezadoRemision_IdEncabezadoRemision']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                if (baja == "true") {
                    SetCambiarEstatus(idRemision, baja);
                }
                else {
                    SetActivarEstatus(idRemision, baja);
                }
            });
        }
    });
}

function SetEliminarDetalleRemision(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Remision.aspx/EliminarDetalleRemision",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtTotal").text(formato.moneda(parseFloat(respuesta.Total), "$"));
                $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotal").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                $("#txtTotalEditar").text(formato.moneda(parseFloat(respuesta.Total), "$"));
                $("#spanCantidadLetraEditar").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalEditar").text()).toString(), $("#cmbTipoMoneda option:selected").text()));
                
                limpiaFiltrosGridDetalleProducto();
                $("#grdDetalleProducto").trigger("reloadGrid");
                $("#grdDetalleRemision").trigger("reloadGrid");
                $("#grdDetalleRemisionEditar").trigger("reloadGrid");
                $("#grdEncabezadoRemision").trigger("reloadGrid");
                ObtenerTotalesEstatusRemision();
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

function Imprimir(pIdEncabezadoRemision, pSinPrecio) {
    MostrarBloqueo();

    var pRequest = new Object();
    pRequest.pTemplate = 'Remision';
    pRequest.pIdEncabezadoRemision = pIdEncabezadoRemision;
    pRequest.pFolio = $("#txtFolio").text();
    pRequest.pSinPrecio = pSinPrecio;  
    
    $.ajax({
        type: "POST",
        url: "Remision.aspx/Imprimir",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == "0") {
                var ruta = respuesta.Modelo.Archivo;
                var imp = ruta.split('|');
                if (imp[0] == '0') { $("[id*=btnDescarga]").click(); } else { MostrarMensajeError(imp[1]); }
            } else { MostrarMensajeError(respuesta.Descripcion); }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerFormaFiltrosEncabezadoRemision() {
    $("#divFiltrosEncabezadoRemision").obtenerVista({
        nombreTemplate: "tmplFiltrosEncabezadoRemision.html",
        url: "Remision.aspx/ObtenerFormaFiltroEncabezadoRemision",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroEncabezadoRemision();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroEncabezadoRemision();
                    }
                });
            }

            $('#divFiltrosEncabezadoRemision').on('click', '#chkPorFecha', function(event) {
                FiltroEncabezadoRemision();
            });

            $('#divFiltrosEncabezadoRemision').on('focusout', '#txtNumeroSerieBuscador', function(event) {
                FiltroEncabezadoRemision();
            });

            $('#divFiltrosEncabezadoRemision').on('focusout', '#txtNumeroPedidoBuscador', function(event) {
                FiltroEncabezadoRemision();
            });

        }
    });
}

function BuscarNumeroSerie(evento) {
    var key = evento.which || evento.keyCode;
    if ((key == 13)) {
        FiltroEncabezadoRemision();
        return false;
    }
}

function BuscarNumeroPedido(evento) {
    var key = evento.which || evento.keyCode;
    if ((key == 13)) {
        FiltroEncabezadoRemision();
        return false;
    }
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaFiltrosRemision() {
    $("#divVistaFormas").obtenerVista({
        nombreTemplate: "tmplFiltrosRemision.html",        
        efecto: "slide",
        despuesDeCompilar: function() {                        
            $("#txtFechaDe").datepicker();
            $("#txtFechaA").datepicker();
        }
    });
}

function ObtenerFormaSeleccionarAlmacen() {
    $("#dialogSeleccionarAlmacenRemision").obtenerVista({
        nombreTemplate: "tmplSeleccionarAlmacenRemision.html",
        url: "Remision.aspx/LlenaComboAlmacen",
        despuesDeCompilar: function() {
            $("#dialogSeleccionarAlmacenRemision").dialog("open");
        }
    });
}

function ObtenerListaTipoMoneda(pRequest) {
    $("#cmbTipoMonedaOrigen").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        modelo: pRequest.ListaCombo
    });
}

function obtenerComboAlmacenRemision() {
    $("#divAreaBotonesDialog").obtenerVista({
        url: "Remision.aspx/LlenaComboAlmacen",
        nombreTemplate: "tmplFiltroComboRemision.html",
        efecto: "slide",
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function obtenerFormaAgregarRemision(pRequest) {
    MostrarBloqueo();
    $("#dialogAgregarRemision").obtenerVista({
        url: "Remision.aspx/ObtenerFormaAgregarRemision",
        parametros: pRequest,
        nombreTemplate: "tmplAgregarRemision.html",
        despuesDeCompilar: function(pRespuesta) {            
            Inicializar_grdDetalleProducto();
            Inicializar_grdDetalleRemision();
            AutocompletarCliente();
            AutocompletarProyecto();
            autocompletarClaveProducto();
            autocompletarNumeroSerieProducto();
            $("#dialogAgregarRemision").dialog("open");
            $("#txtFechaRemision").datepicker();
            OcultarBloqueo();
        }
    });                
}

function obtenerFormaDetallePedido() {
    $("#divFormaMuestraDetallePedido").obtenerVista({
        nombreTemplate: "tmplAgregarDetallePedido.html",
        despuesDeCompilar: function() {
            FiltroDetallePedido();
            $("#dialogDetallePedido").dialog("open");
        }
    });
}

function ObtenerFormaConsultarEncabezadoRemision(pIdEncabezadoRemision) {
    $("#dialogConsultarEncabezadoRemision").obtenerVista({
        nombreTemplate: "tmplConsultarEncabezadoRemision.html",
        url: "Remision.aspx/ObtenerFormaEncabezadoRemision",
        parametros: pIdEncabezadoRemision,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdDetalleRemisionConsultar();            
            if (pRespuesta.modelo.Permisos.puedeEditarEncabezadoRemision == 1) {
                $("#dialogConsultarEncabezadoRemision").dialog("option", "buttons", {                    
                    "Editar": function() {
                        var IdEstatus = 0;
                        IdEstatus = parseInt($("#divFormaConsultarEncabezadoRemision").attr("idEstatus"));
                        if (IdEstatus == 0) {
                            $(this).dialog("close");
                            var EncabezadoRemision = new Object();
                            EncabezadoRemision.IdEncabezadoRemision = parseInt($("#divFormaConsultarEncabezadoRemision").attr("IdEncabezadoRemision"));
                            ObtenerFormaEditarEncabezadoRemision(JSON.stringify(EncabezadoRemision))
                        }
                        else {
                            MostrarMensajeError("No se puede editar esta factura porque esta cancelada");
                        }
                    },
                    "Salir": function() {
                        $(this).dialog("close")
                    }
                });
                $("#dialogConsultarEncabezadoRemision").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarEncabezadoRemision").dialog("option", "buttons", {});
                $("#dialogConsultarEncabezadoRemision").dialog("option", "height", "auto");
            }
            $("#dialogConsultarEncabezadoRemision").dialog("open");
        }
    });
}

function ObtenerFormaEditarEncabezadoRemision(IdEncabezadoRemision) {
    $("#dialogEditarEncabezadoRemision").obtenerVista({
        nombreTemplate: "tmplEditarEncabezadoRemision.html",
        url: "Remision.aspx/ObtenerFormaEditarEncabezadoRemision",
        parametros: IdEncabezadoRemision,
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarCliente();
            AutocompletarProyecto();
            Inicializar_grdDetalleProducto();
            Inicializar_grdDetalleRemisionEditar();
            autocompletarClaveProducto();
            autocompletarNumeroSerieProducto();
            $("#dialogEditarEncabezadoRemision").dialog("option", "height", "auto");
            $("#dialogEditarEncabezadoRemision").dialog("open");
            AutocompletarCliente();
        }
    });
}

function AutocompletarCliente() {

    $('#txtNombreComercial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtNombreComercial').val();
            $.ajax({
                type: 'POST',
                url: 'Remision.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idCliente", "0");
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProyecto", "0");
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
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idCliente", pIdCliente);
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProyecto", "0");
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
                url: 'Remision.aspx/BuscarProyectoCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idCliente", "0");
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProyecto", "0");
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
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idCliente", "0");
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProyecto", pIdProyecto);
            $("#grdDetalleProducto").trigger("reloadGrid");
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function autocompletarClaveProducto() {
    $('#txtClaveProducto').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pClave = $("#txtClaveProducto").val();
            $.ajax({
                type: 'POST',
                url: 'Remision.aspx/BuscarClave',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idProducto", "0");
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("idTipoMonedaProducto", "0");
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdPedidoDetalle", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Clave + ' - ' + item.Producto, value: item.Clave + ' - ' + item.Producto, id: item.IdProducto }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', pIdProducto);
            $("#grdDetalleProducto").trigger("reloadGrid");
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function autocompletarNumeroSerieProducto() {
    $('#txtNS').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pNumeroSerie = $("#txtNS").val();
            $.ajax({
                type: 'POST',
                url: 'Remision.aspx/BuscarNumeroSerie',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdPedidoDetalle", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.NumeroSerie + ' - ' + item.Producto, value: item.NumeroSerie + ' - ' + item.Producto, id: item.IdProducto, ns: item.NumeroSerie }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            var pNumeroSerie = ui.item.ns;
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', pIdProducto);
            $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('NumeroSerie', pNumeroSerie);
            $("#grdDetalleProducto").trigger("reloadGrid");
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function limpiaFiltrosGridDetalleProducto() {
    $("#txtClaveProducto").val('');
    $("#txtNS").val('');    
    $("#txtCantidad").val('1');
    $("#txtCosto").val('$0.00');
    $("#txtPesos").val('');
    $("#txtTC").val('');
    $("#txtPrecio").val('$0.00');
    $("#txtPrecioConTipoCambio").val('$0.00');
    $("#txtTipoCambioDetalle").val('1');
    $("#txtOtroPesos").val('');
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdExistenciaDistribuida', "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdDetalleFacturaProveedor', "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("cantidadPedido", "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("NumeroSerie", "");
}

function limpiaFiltrosGridDetalleProductoGeneral() {
    $("#txtClaveProducto").val('');
    $("#txtNS").val('');
    $("#txtCantidad").val('1');
    $("#txtCosto").val('$0.00');
    $("#txtPesos").val('');
    $("#txtTC").val('');
    $("#txtPrecio").val('$0.00');
    $("#txtPrecioConTipoCambio").val('$0.00');
    $("#txtTipoCambioDetalle").val('1');
    $("#txtOtroPesos").val('');
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdExistenciaDistribuida', "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdDetalleFacturaProveedor', "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdPedidoDetalle', "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("cantidadPedido", "0");
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("NumeroSerie", "");
    OcultaObjetosElegirTodos();
}

function FiltroDetalleProducto() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleProducto').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleProducto').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleProducto').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleProducto').getGridParam('sortorder');
    request.pIdProducto = 0;
    request.pIdPedidoDetalle = 0;
    request.pNumeroSerie = "";
    request.pIdProyecto = 0;
    request.pIdAlmacen = 0;
    request.pFolioPedido = "";
    request.pClave = "";
    
    if ($("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdProducto") != null && $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdProducto") != "") {
        request.pIdProducto = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdProducto");
    }
    if ($("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdPedidoDetalle") != null && $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdPedidoDetalle") != "") {
        request.pIdPedidoDetalle = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdPedidoDetalle");
    }
    if ($("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdProyecto") != null && $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdProyecto") != "") {
        request.pIdProyecto = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdProyecto");
    }
    if ($("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdAlmacen") != null && $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdAlmacen") != "") {
        request.pIdAlmacen = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr("IdAlmacen");
    }
    
    if ($("#gs_NumeroSerie").val() != null) { request.pNumeroSerie = $("#gs_NumeroSerie").val(); }
    if ($("#gs_Clave").val() != null) { request.pClave= $("#gs_Clave").val(); }
    if ($("#gs_NumeroPedido").val() != null) { request.pFolioPedido = $("#gs_NumeroPedido").val(); }


    var pRequest = JSON.stringify(request);
    //alert(pRequest);
    $.ajax({
        url: 'Remision.aspx/ObtenerDetalleProducto',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleProducto')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleRemision() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleRemision').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleRemision').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleRemision').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleRemision').getGridParam('sortorder');
    request.pIdEncabezadoRemision = 0;
    if ($("#divAgregarRemision").attr("IdEncabezadoRemision") != null && $("#divAgregarRemision").attr("IdEncabezadoRemision") != "") {
        request.pIdEncabezadoRemision = $("#divAgregarRemision").attr("IdEncabezadoRemision");
    }    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Remision.aspx/ObtenerDetalleRemision',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleRemision')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleRemisionConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleRemisionConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleRemisionConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleRemisionConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleRemisionConsultar').getGridParam('sortorder');
    request.pIdEncabezadoRemisionConsultar = 0;
    if ($("#divFormaConsultarEncabezadoRemision").attr("IdEncabezadoRemision") != null && $("#divFormaConsultarEncabezadoRemision").attr("IdEncabezadoRemision") != "") {
        request.pIdEncabezadoRemision = $("#divFormaConsultarEncabezadoRemision").attr("IdEncabezadoRemision");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Remision.aspx/ObtenerDetalleRemisionConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleRemisionConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleRemisionEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleRemisionEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleRemisionEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleRemisionEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleRemisionEditar').getGridParam('sortorder');
    request.pIdEncabezadoRemisionEditar = 0;
    if ($("#divFormaEditarEncabezadoRemision").attr("IdEncabezadoRemision") != null && $("#divFormaEditarEncabezadoRemision").attr("IdEncabezadoRemision") != "") {
        request.pIdEncabezadoRemision = $("#divFormaEditarEncabezadoRemision").attr("IdEncabezadoRemision");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Remision.aspx/ObtenerDetalleRemisionEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleRemisionEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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
        url: 'Remision.aspx/ObtenerDetallePedido',
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

function FiltroEncabezadoRemision() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdEncabezadoRemision').getGridParam('rowNum');
    request.pPaginaActual = $('#grdEncabezadoRemision').getGridParam('page');
    request.pColumnaOrden = $('#grdEncabezadoRemision').getGridParam('sortname');
    request.pTipoOrden = $('#grdEncabezadoRemision').getGridParam('sortorder');
    request.pRazonSocial = "";
    request.pFolio = "";
    request.pIdEstatusRemision = -1;
    request.pAI = 0;
    request.pClave = "";
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pNumeroSerie = "";
    request.pNumeroPedido = "";

    if ($("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado") != null && $("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado") != "") {
        request.pIdEstatusRemision = $("#tblTotalesEstatus").attr("idEstatusRemisionSeleccionado");
    }

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }
    
    if ($("#gs_Clave").val() != null) { request.pClave = $("#gs_Clave").val(); }
    
    if ($("#gs_NumeroPedido").val() != null) { request.pNumeroPedido= $("#gs_NumeroPedido").val(); }

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

    if ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null) {
        request.pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Remision.aspx/ObtenerRemision',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdEncabezadoRemision')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function obtenerPedidosCliente(pRequest) {
    $("#cmbCotizacion").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Remision.aspx/obtenerPedidosCliente",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function ObtenerTotalesEstatusRemision() {
    $.ajax({
        url: 'Remision.aspx/ObtenerTotalesEstatusRemision',
        data: {},
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $.each(respuesta.Modelo.TotalesEstatusRemision, function(index, oEstatusRemision) {
                    $('#span-E' + oEstatusRemision.IdEstatusRemision).text(oEstatusRemision.Contador);
                });
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        }
    });
}


function LlenarDetalleProducto(pIdProducto, pIdPedidoDetalle) {
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto', pIdProducto);
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProductoAnterior', pIdProducto);
    $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdPedidoDetalle', pIdPedidoDetalle);
    $("#grdDetalleProducto").trigger("reloadGrid");
    $("#dialogDetallePedido").dialog('close');
}

function AgregarDetalleProductoRemision(oRegistros) {
    var request = new Object();
    var oRegistros = new Object();
    oRegistros.pIdProducto = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProducto');
    oRegistros.pIdCliente = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdCliente');
    oRegistros.pIdProyecto = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdProyecto');
    oRegistros.pIdDetFacProveedor = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdDetalleFacturaProveedor');
    oRegistros.pIdDetPedido = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdPedidoDetalle');
    oRegistros.pIdAlmacen = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdAlmacen');
    oRegistros.pIdExistenciaDistribuida = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdExistenciaDistribuida');
    oRegistros.pIdTipoMoneda = $("#cmbTipoMoneda").val();
    oRegistros.pFechaRemision = $("#txtFechaRemision").val();
    oRegistros.pCantidad = $("#txtCantidad").val();
    oRegistros.pCantidadPedido = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('cantidadPedido');
    oRegistros.pCostoUnitario = QuitarFormatoNumero($("#txtCosto").val());
    oRegistros.pPrecioUnitario = QuitarFormatoNumero($("#txtPrecio").val());
    oRegistros.pTipoCambio = $("#txtTipoCambioRemision").val();
    oRegistros.pPrecioConTipoCambio = QuitarFormatoNumero($("#txtPrecioConTipoCambio").val());
    oRegistros.pNota = $("#txtNota").val();
    oRegistros.pTodos = 0;
    
    if (oRegistros.pTodos == 0) {
        ValidaEncabezadoRemision(oRegistros);
    }
    else {
        ValidaEncabezadoRemisionTodos(oRegistros);
    }
}

function EditarEncabezadoRemision() {
    var pEncabezadoRemision = new Object();
    pEncabezadoRemision.IdEncabezadoRemision = $("#divFormaEditarEncabezadoRemision").attr("idEncabezadoRemision");
    pEncabezadoRemision.Nota = $("#txtNota").val();
    var oRequest = new Object();
    oRequest.pEncabezadoRemision = pEncabezadoRemision;
    SetEditarEncabezadoRemision(JSON.stringify(oRequest));
}

function SetEditarEncabezadoRemision(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Remision.aspx/EditarEncabezadoRemision",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoRemision").trigger("reloadGrid");
                ObtenerTotalesEstatusRemision();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarEncabezadoRemision").dialog("close");
        }
    });
}

function ConsolidarRemision() {   
    var request = new Object();
    request.pIdCotizacion = $("#divAgregarRemision,#divFormaEditarEncabezadoRemision").attr('IdCotizacion');
    request.pIdEncabezadoRemision = $("#divAgregarRemision,#divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision');
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Remision.aspx/ConsolidarRemision',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogAgregarRemision").dialog("close");
            } else {
                MostrarMensajeError("Todavia Existen productos en el pedido no remisionados.");
            }
        },
        complete: function(pResupuesta) {
        }
    });
}

function CancelarEncabezadoRemision() {
    var request = new Object();
    request.pIdEncabezadoRemision = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision');
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Remision.aspx/CancelarEncabezadoRemision',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEncabezadoRemision").trigger("reloadGrid");
                ObtenerTotalesEstatusRemision();
                $("#dialogEditarEncabezadoRemision").dialog("close");
            } else {
                MostrarMensajeError("No se puede cancelar la remisión");
            }
        },
        complete: function(jsondata, stat) {
        }
    });
}

//-----Validaciones------------------------------------------------------
function ValidaEncabezadoRemision(oRegistros) {
    var errores = "";
    var request = new Object();
    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Remision.aspx/ValidarGenerarRemisionSinCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 1) 
            {
                oRegistros.pIdCliente=0;
                oRegistros.pIdProyecto =0;
                //alert(1);
                if (oRegistros.pFechaRemision == "")
                { 
                    errores = errores + "<span>*</span> El campo fecha de remisión esta vacío, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pIdTipoMoneda == 0)
                { 
                    errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; 
                }

                if (oRegistros.pIdAlmacen == 0)
                { 
                    errores = errores + "<span>*</span> No hay almacen asociado, favor de elegir alguno.<br />"; 
                }

                if (oRegistros.pIdProducto == 0)
                { 
                    errores = errores + "<span>*</span> No hay producto asociado, favor de elegir alguno.<br />"; 
                }

                if (oRegistros.pCantidad == 0)
                {
                    errores = errores + "<span>*</span> El campo cantidad esta vacio, favor de capturarlo.<br />"; 
                }

                if (parseInt(oRegistros.pCantidad) > parseInt(oRegistros.pCantidadPedido))
                { 
                    errores = errores + "<span>*</span> El campo cantidad no puede ser mayor a la cantidad de la partida seleccionada.<br />"; 
                } 

                if (oRegistros.pCostoUnitario == 0)
                { 
                    errores = errores + "<span>*</span> El campo costo esta vacio, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pPrecioUnitario == 0)
                { 
                    errores = errores + "<span>*</span> El precio unitario esta vacio, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pTipoCambio == 0)
                {
                    errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pIdDetFacProveedor == 0 || oRegistros.pIdDetFacProveedor=="")
                { 
                    errores = errores + "<span>*</span> No hay recepcion de este producto, favor de recepcionarlo.<br />"; 
                }
                 
                if (errores != "")
                { 
                    errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; 
                }
               
            } 
            if (respuesta.Error == 0) 
            {
                //alert(0);
                if ((oRegistros.pIdCliente == 0 || oRegistros.pIdCliente == "") && (oRegistros.pIdProyecto == 0 || oRegistros.pIdProyecto == ""))
                { 
                    errores = errores + "<span>*</span> No hay cliente o proyecto seleccionado, favor de elegir alguno.<br />"; 
                }

                if (oRegistros.pFechaRemision == "")
                { 
                    errores = errores + "<span>*</span> El campo fecha de remisión esta vacío, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pIdTipoMoneda == 0)
                { 
                    errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; 
                }

                if (oRegistros.pIdAlmacen == 0)
                { errores = errores + "<span>*</span> No hay almacen asociado, favor de elegir alguno.<br />"; }

                if (oRegistros.pIdProducto == 0)
                { 
                    errores = errores + "<span>*</span> No hay producto asociado, favor de elegir alguno.<br />"; 
                }

                if (oRegistros.pCantidad == 0)
                { errores = errores + "<span>*</span> El campo cantidad esta vacio, favor de capturarlo.<br />"; }

                if (parseInt(oRegistros.pCantidad) > parseInt(oRegistros.pCantidadPedido))
                { 
                    errores = errores + "<span>*</span> El campo cantidad no puede ser mayor a la cantidad de la partida seleccionada.<br />"; 
                } 

                if (oRegistros.pCostoUnitario == 0)
                { 
                    errores = errores + "<span>*</span> El campo costo esta vacio, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pPrecioUnitario == 0)
                { 
                    errores = errores + "<span>*</span> El precio unitario esta vacio, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pTipoCambio == 0)
                { 
                    errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturarlo.<br />"; 
                }

                if (oRegistros.pIdDetFacProveedor == 0 || oRegistros.pIdDetFacProveedor=="")
                { 
                    errores = errores + "<span>*</span> No hay recepcion de este producto, favor de recepcionarlo.<br />"; 
                }
                
                if (errores != "") 
                { 
                    errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; 
                }
                //alert(5);
            }
            if(errores != ""){
                MostrarMensajeError(errores); return false;
            }
            var urlMetodo = "";
            if ($("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision') == 0 || $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision') == "") {
				
                if ($("#chkTodos").is(':checked')) {
                    oRegistros.pTodos = 1;       
                    oRegistros.DetallePartidas = new Array();
                    $(".chkElegir:checked").each(function(index, object) {
                        var registro = $(this).parents("tr");
                        var pDetalle = new Object();
                        var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
                        if (IdAlmacenProducto != 0) {
                            var registro = $(this).parents("tr");
                            var pDetalle = new Object();
                            pDetalle.IdProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html());
                            pDetalle.IdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html());
                            pDetalle.IdAlmacen = IdAlmacenProducto;
                            pDetalle.IdExistenciaDistribuida = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html());
                            pDetalle.IdPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html());
                            pDetalle.Cantidad = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
                            pDetalle.CantidadPedido = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
                            pDetalle.Costo = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
                            pDetalle.Precio = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
                            pDetalle.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
                            pDetalle.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html());
                            oRegistros.DetallePartidas.push(pDetalle);

                        } else {
                            MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
                        }
                    });
                }
					urlMetodo = 'Remision.aspx/AgregarProductoDetalleRemisionEncabezado';
					
            }else {
					if ($("#chkTodos").is(':checked')) {
						oRegistros.pTodos = 1;
						oRegistros.DetallePartidas = new Array();
						$(".chkElegir:checked").each(function(index, object) {
							var registro = $(this).parents("tr");
							var pDetalle = new Object();
							var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
							if (IdAlmacenProducto != 0) {
								var registro = $(this).parents("tr");
								var pDetalle = new Object();
								pDetalle.IdProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html());
								pDetalle.IdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html());
								pDetalle.IdAlmacen = IdAlmacenProducto;
								pDetalle.IdExistenciaDistribuida = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html());
								pDetalle.IdPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html());
								pDetalle.Cantidad = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
								pDetalle.CantidadPedido = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
								pDetalle.Costo = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
								pDetalle.Precio = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
								pDetalle.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
								pDetalle.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html()); 
								oRegistros.DetallePartidas.push(pDetalle);

							} else {
								MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
							}
						});
					}        
				urlMetodo = 'Remision.aspx/AgregarDetalleProductoRemision';        
				oRegistros.pIdEncabezadoRemision = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision');
				
            }    
            request.pRegistros = oRegistros;
            var pRequest = JSON.stringify(request);
            MostrarBloqueo();
            $.ajax({
                url: urlMetodo,
                data: pRequest,
                dataType: 'json',
                type: 'post',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    respuesta = jQuery.parseJSON(pRespuesta.d);
                    if (respuesta.Error == 0) {
                        $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision', respuesta.IdEncabezadoRemision);

                        $("#txtTotal").text(formato.moneda(parseFloat(respuesta.Total), "$"));
                        $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotal").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                        $("#txtTotalEditar").text(formato.moneda(parseFloat(respuesta.Total), "$"));
                        $("#spanCantidadLetraEditar").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalEditar").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                        $("#txtNumeroRemision").val(respuesta.NumeroRemision);

                        $("#txtFechaRemision").attr("disabled", "true");
                        $("#cmbTipoMoneda").attr("disabled", "true");
                        $("#txtNota").attr("disabled", "true");

                        limpiaFiltrosGridDetalleProducto();
                        $("#grdDetalleProducto").trigger("reloadGrid");
                        $("#grdDetalleRemision").trigger("reloadGrid");
                        $("#grdDetalleRemisionEditar").trigger("reloadGrid");
                        $("#grdEncabezadoRemision").trigger("reloadGrid");
                        ObtenerTotalesEstatusRemision();
                    }
                },
                complete: function(jsondata, stat) {
                    OcultarBloqueo();
                }
            });
        }
    });
}

function ValidaEncabezadoRemisionTodos(oRegistros) {
    var errores = "";
    var request = new Object();
    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Remision.aspx/ValidarGenerarRemisionSinCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) 
            {
                if (oRegistros.pFechaRemision == "")
                { 
					errores = errores + "<span>*</span> El campo fecha de remisión esta vacío, favor de capturarlo.<br />"; 
				}

                if (oRegistros.pIdTipoMoneda == 0)
                {
					errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; 
				}

                if (oRegistros.pIdAlmacen == 0)
                { 
					errores = errores + "<span>*</span> No hay almacen asociado, favor de elegir alguno.<br />"; 
				}
                errores = errores + "<span>*</span> "+ respuesta.Descripcion +".<br />";

                if (errores != "")
                { 
					errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; 
				}
            }
            if (respuesta.Error == 1) 
            {
                if ((oRegistros.pIdCliente == 0 || oRegistros.pIdCliente == "") && (oRegistros.pIdProyecto == 0 || oRegistros.pIdProyecto == ""))
                { 
					errores = errores + "<span>*</span> No hay cliente o proyecto seleccionado, favor de elegir alguno.<br />" ;
				}
                if (oRegistros.pFechaRemision == "")
                { 
					errores = errores + "<span>*</span> El campo fecha de remisión esta vacío, favor de capturarlo.<br />"; 
				}
                if (oRegistros.pIdTipoMoneda == 0)
                { 
					errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; 
				}
                if (oRegistros.pIdAlmacen == 0)
                { 
					errores = errores + "<span>*</span> No hay almacen asociado, favor de elegir alguno.<br />"; 
				}

                if (errores != "")
                { 
					errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; 
				}
            }
            if(errores != ""){
                MostrarMensajeError(errores); return false;
            }
            var urlMetodo = "";
            if ($("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision') == 0 || $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision') == "") {
                    if ($("#chkTodos").is(':checked')) {
                        oRegistros.pTodos = 1;       
                        oRegistros.DetallePartidas = new Array();
                        $(".chkElegir:checked").each(function(index, object) {
                            var registro = $(this).parents("tr");
                            var pDetalle = new Object();
                            var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
                            if (IdAlmacenProducto != 0) {
                                var registro = $(this).parents("tr");
                                var pDetalle = new Object();
                                pDetalle.IdProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html());
                                pDetalle.IdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html());
                                pDetalle.IdAlmacen = IdAlmacenProducto;
                                pDetalle.IdExistenciaDistribuida = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html());
                                pDetalle.IdPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html());
                                pDetalle.Cantidad = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
                                pDetalle.CantidadPedido = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
                                pDetalle.Costo = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
                                pDetalle.Precio = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
                                pDetalle.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
                                pDetalle.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html());
                                oRegistros.DetallePartidas.push(pDetalle);

                            } else {
                                MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
                            }
                        });
                    }
                    urlMetodo = 'Remision.aspx/AgregarProductoDetalleRemisionEncabezado';
                    //alert(6);
                }else {
					if ($("#chkTodos").is(':checked')) {
						oRegistros.pTodos = 1;
						oRegistros.DetallePartidas = new Array();
						$(".chkElegir:checked").each(function(index, object) {
							var registro = $(this).parents("tr");
							var pDetalle = new Object();
							var IdAlmacenProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdAlmacen']").html());
							if (IdAlmacenProducto != 0) {
								var registro = $(this).parents("tr");
								var pDetalle = new Object();
								pDetalle.IdProducto = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdProducto']").html());
								pDetalle.IdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdDetalleFacturaProveedor']").html());
								pDetalle.IdAlmacen = IdAlmacenProducto;
								pDetalle.IdExistenciaDistribuida = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdExistenciaDistribuida']").html());
								pDetalle.IdPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdCotizacionDetalle']").html());
								pDetalle.Cantidad = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
								pDetalle.CantidadPedido = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_Existencia']").html());
								pDetalle.Costo = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Costo']").text());
								pDetalle.Precio = QuitarFormatoNumero($(registro).children("td[aria-describedby='grdDetalleProducto_Precio']").text());
								pDetalle.IdTipoCambioOrigen = $("#cmbTipoMoneda").val();
								pDetalle.IdTipoCambioDestino = parseInt($(registro).children("td[aria-describedby='grdDetalleProducto_IdTipoMoneda']").html()); 
								oRegistros.DetallePartidas.push(pDetalle);

							} else {
								MostrarMensajeError("No se puede remisionar este producto ya que no pertenece a ningún almacen");
							}
						});
					}        
				urlMetodo = 'Remision.aspx/AgregarDetalleProductoRemision';        
				oRegistros.pIdEncabezadoRemision = $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision');
            }    
            request.pRegistros = oRegistros;
            var pRequest = JSON.stringify(request);
            MostrarBloqueo();
            $.ajax({
                url: urlMetodo,
                data: pRequest,
                dataType: 'json',
                type: 'post',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    respuesta = jQuery.parseJSON(pRespuesta.d);
                    if (respuesta.Error == 0) {
                        $("#divAgregarRemision, #divFormaEditarEncabezadoRemision").attr('IdEncabezadoRemision', respuesta.IdEncabezadoRemision);

                        $("#txtTotal").text(formato.moneda(parseFloat(respuesta.Total), "$"));
                        $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotal").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                        $("#txtTotalEditar").text(formato.moneda(parseFloat(respuesta.Total), "$"));
                        $("#spanCantidadLetraEditar").text(covertirNumLetras(QuitarFormatoNumero($("#txtTotalEditar").text()).toString(), $("#cmbTipoMoneda option:selected").text()));

                        $("#txtNumeroRemision").val(respuesta.NumeroRemision);

                        $("#txtFechaRemision").attr("disabled", "true");
                        $("#cmbTipoMoneda").attr("disabled", "true");
                        $("#txtNota").attr("disabled", "true");

                        limpiaFiltrosGridDetalleProducto();
                        $("#grdDetalleProducto").trigger("reloadGrid");
                        $("#grdDetalleRemision").trigger("reloadGrid");
                        $("#grdDetalleRemisionEditar").trigger("reloadGrid");
                        $("#grdEncabezadoRemision").trigger("reloadGrid");
                        ObtenerTotalesEstatusRemision();
                    }
                },
                complete: function(jsondata, stat) {
                    OcultarBloqueo();
                }
            });
        }
	});
}