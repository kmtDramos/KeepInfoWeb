//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
var Productos = new Object();
Productos.idsFacturasDetalle = new Array();

$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("NotaCredito");
    });
    ObtenerFormaFiltrosNotaCredito();


    //////funcion del grid//////
    $("#gbox_grdNotaCredito").livequery(function() {
        $("#grdNotaCredito").jqGrid('navButtonAdd', '#pagNotaCredito', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pFolioNotaCredito = "";
                var pSerieNotaCredito = "";
                var pAI = 0;
                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;
                var pFiltroTimbrado = 0;
                var pIdTipoFiltro = 0;
                var pValorFiltro = "";

                if ($('#gs_SerieNotaCredito').val() != null) {
                    pSerieNotaCredito = $("#gs_SerieNotaCredito").val();
                }
                if ($('#gs_FolioNotaCredito').val() != null) {
                    pFolioNotaCredito = $("#gs_FolioNotaCredito").val();
                }

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
                    pFiltroTimbrado = $("#cmbFiltroTimbrado").val();
                }

                if ($("#cmbTipoFiltro").val() != null) {
                    pIdTipoFiltro = $("#cmbTipoFiltro").val();
                }

                if ($("#txtValorFiltro").val() != null) {
                    pValorFiltro = $("#txtValorFiltro").val();
                }

                if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                    pFechaFinal = $("#txtFechaFinal").val();
                    pFechaFinal = ConvertirFecha(pFechaFinal, 'aaaammdd');
                }

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pRazonSocial: pRazonSocial,
                    pFolioNotaCredito: pFolioNotaCredito,
                    pSerieNotaCredito: pSerieNotaCredito,
                    pAI: pAI,
                    pFechaInicial: pFechaInicial,
                    pFechaFinal: pFechaFinal,
                    pPorFecha: pPorFecha,
                    pFiltroTimbrado: pFiltroTimbrado,
                    pIdTipoFiltro: pIdTipoFiltro,
                    pValorFiltro: pValorFiltro
                }, downloadType: 'Normal'
                });

            }
        });
    });
    ///////////////////////////


    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarNotaCredito", function() {
        ObtenerFormaComboTipoNotaCredito();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarNotaCreditoDevolucionCancelacion", function() {
        ObtenerFormaComboTipoNotaCreditoDevolucionCancelacion();
    });

    $("#grdNotaCredito").on("click", ".imgFormaConsultarNotaCredito", function() {
        var registro = $(this).parents("tr");
        var NotaCredito = new Object();
        NotaCredito.pIdNotaCredito = parseInt($(registro).children("td[aria-describedby='grdNotaCredito_IdNotaCredito']").html());
        ObtenerFormaConsultarNotaCredito(JSON.stringify(NotaCredito));
    });

    $("#grdNotaCredito").on("click", ".imgFormaConsultarFacturaFormato", function() {
        var registro = $(this).parents("tr");
        var NotaCredito = new Object();
        NotaCredito.pIdNotaCredito = parseInt($(registro).children("td[aria-describedby='grdNotaCredito_IdNotaCredito']").html());
        ObtenerFormaConsultarNotaCreditoFormato(JSON.stringify(NotaCredito));
    });

    $("#grdNotaCredito").on("click", ".imgFormaConsultarFacturaXML", function () {
        var registro = $(this).parents("tr");
        var NotaCredito = new Object();
        NotaCredito.pIdNotaCredito = parseInt($(registro).children("td[aria-describedby='grdNotaCredito_IdNotaCredito']").html());
        ObtenerFormaConsultarNotaCreditoXML(JSON.stringify(NotaCredito));
    });

    $('#grdNotaCredito').on('click', '.div_grdNotaCredito_AI', function(event) {

        var registro = $(this).parents("tr");
        var NotaCredito = new Object();
        var estatusBaja = $(registro).children("td[aria-describedby='grdNotaCredito_AI']").children().attr("baja")
        NotaCredito.IdNotaCredito = $(registro).children("td[aria-describedby='grdNotaCredito_IdNotaCredito']").html();
        if (estatusBaja == "0" || estatusBaja == "False") {
            ObtenerFormaMotivoCancelacion(JSON.stringify(NotaCredito));
        }
        else {
            MostrarMensajeAviso("Esta nota de crédito, ya esta cancelada");
        }

    });

    //Nota de creditodevolucion
    $('#dialogAgregarNotaCreditoDevolucionCancelacion, #dialogEditarNotaCreditoDevolucionCancelacion').on('change', '#cmbSerieNotaCredito', function(event) {
        var pNotaCreditoDevolucion = new Object();
        pNotaCreditoDevolucion.IdSerieNotaCredito = parseInt($(this).val());
        var oRequest = new Object();
        oRequest.pNotaCredito = pNotaCreditoDevolucion;
        ObtenerNumeroNotaCreditoDevolucion(JSON.stringify(oRequest))
    });

    $('#dialogAgregarNotaCreditoDevolucionCancelacion, #dialogEditarNotaCreditoDevolucionCancelacion').on('change', '#cmbTipoMoneda', function(event) {
        var pTipoCambioDevolucion = new Object();
        pTipoCambioDevolucion.IdTipoCambioOrigen = parseInt($(this).val());
        pTipoCambioDevolucion.IdTipoCambioDestino = parseInt(1);
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambioDevolucion;
        ObtenerTipoCambioDevolucion(JSON.stringify(oRequest))
    });

    $("#grdProductosNotaCreditoDevolucionCancelacion").on("click", ".checkAsignarVarios", function() {
        if ($(this).is(':checked')) {
            var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
            var registro = $(this).parents("tr");
            facturasSeleccionadas.push(parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoDevolucionCancelacion_IdFacturaDetalle']").html()));
            var cantidad = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoDevolucionCancelacion_Cantidad']").html());
            var cantidadDisponible = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoDevolucionCancelacion_Disponible']").html());
            if (cantidadDisponible != 0) {
                var IdFacturaDetalle = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoDevolucionCancelacion_IdFacturaDetalle']").html());
                $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
                $("#txtCantidadDevolver").val("");
                $("#Cantidad").empty().append(cantidad);
                $("#CantidadDisponible").empty().append(cantidadDisponible); ;
                $("#dialogAgregarProductosCantidades").attr("idFacturaDetalle", IdFacturaDetalle);
                $("#dialogAgregarProductosCantidades").dialog("open");
            }
            else {
                MostrarMensajeError("Este producto no tiene cantidad disponible");
            }
        }
        else {
            var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
            var registro = $(this).parents("tr");
            var idFactura = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoDevolucionCancelacion_IdFacturaDetalle']").html());
            $.each(facturasSeleccionadas, function(pIndex, pIdFactura) {
                if (pIdFactura == idFactura) {
                    facturasSeleccionadas.splice(pIndex, 1);
                    $.each(Productos.idsFacturasDetalle, function(pIndex, oProducto) {
                        if (idFactura == oProducto.idFacturaDetalle) {
                            Productos.idsFacturasDetalle.splice(pIndex, 1);
                        }
                    });
                }
            });
            $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
        }
    });

    $("#grdProductosAsociadosNotaCreditoDevolucionCancelacion").on("click", ".checkAsignarVarios", function() {
        if ($(this).is(':checked')) {
            var ProductosSeleccionados = JSON.parse("[" + $("#txtProductosSeleccionados").val() + "]");
            var registro = $(this).parents("tr");
            ProductosSeleccionados.push(parseInt($(registro).children("td[aria-describedby='grdProductosAsociadosNotaCreditoDevolucionCancelacion_IdDevolucion']").html()));
            $("#txtProductosSeleccionados").val(ProductosSeleccionados);
        }
        else {
            var ProductosSeleccionados = JSON.parse("[" + $("#txtProductosSeleccionados").val() + "]");
            var registro = $(this).parents("tr");
            var idDevolucion = parseInt($(registro).children("td[aria-describedby='grdProductosAsociadosNotaCreditoDevolucionCancelacion_IdDevolucion']").html());
            $.each(ProductosSeleccionados, function(pIndex, pIdDevolucion) {
                if (pIdDevolucion == idDevolucion) {
                    ProductosSeleccionados.splice(pIndex, 1);
                    return (pIdDevolucion !== idDevolucion);
                }
            });
            $("#txtProductosSeleccionados").val(ProductosSeleccionados);
        }
    });

    $('#dialogAgregarNotaCredito, #dialogEditarNotaCredito').on('change', '#cmbSerieNotaCredito', function(event) {
        var pNotaCredito = new Object();
        pNotaCredito.IdSerieNotaCredito = parseInt($(this).val());
        var oRequest = new Object();
        oRequest.pNotaCredito = pNotaCredito;
        ObtenerNumeroNotaCredito(JSON.stringify(oRequest))
    });
    $('#dialogAgregarNotaCredito, #dialogEditarNotaCredito').on('change', '#cmbTipoMoneda', function(event) {
        var pTipoCambio = new Object();
        pTipoCambio.IdTipoCambioOrigen = parseInt($(this).val());
        pTipoCambio.IdTipoCambioDestino = parseInt(1);
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambio;
        ObtenerTipoCambio(JSON.stringify(oRequest))
    });

    $('#dialogAgregarNotaCredito, #dialogEditarNotaCredito').on('focusout', '#txtMonto', function(event) {
        RecalculaDatosMonto();
    });

    $('#dialogAgregarNotaCredito, #dialogEditarNotaCredito').on('focusout', '#txtTotal', function(event) {
        RecalculaDatosTotal();
    });

    $('#dialogAgregarNotaCreditoDevolucionCancelacion, #dialogEditarNotaCreditoDevolucionCancelacion').on('focusout', '#txtMonto', function(event) {
        RecalculaDatosMonto();
    });

    $('#dialogAgregarNotaCreditoDevolucionCancelacion, #dialogEditarNotaCreditoDevolucionCancelacion ').on('focusout', '#txtTotal', function(event) {
        RecalculaDatosTotal();
    });

    $("#grdMovimientosCobros").on("click", ".imgEliminarMovimiento", function() {

        var registro = $(this).parents("tr");
        var pNotaCreditoEncabezadoFactura = new Object();
        pNotaCreditoEncabezadoFactura.pIdNotaCreditoEncabezadoFactura = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdNotaCreditoEncabezadoFactura']").html());
        var oRequest = new Object();
        oRequest.pNotaCreditoEncabezadoFactura = pNotaCreditoEncabezadoFactura;
        SetEliminarNotaCreditoEncabezadoFactura(JSON.stringify(oRequest));
    });

    $("#dialogEditarNotaCredito, #dialogAgregarNotaCredito").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCredito").attr("IdTipoNotaCredito"));
        if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCredito").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCredito").attr("IdNotaCredito") != "") {
            NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCredito").attr("IdNotaCredito"));
            NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCredito").attr("IdCliente"));
            NotaCredito.IdTipoNotaCredito = IdTipoNotaCredito;
            var validacion = ValidaAsociacionDocumentos(NotaCredito);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.NotaCredito = NotaCredito;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), IdTipoNotaCredito);
        }
        else {
            AgregarNotaCreditoEdicion(IdTipoNotaCredito);
        }
    });

    //NotaCreditoDevolucionCancelacion
    $("#dialogEditarNotaCredito").on("click", "#btnObtenerFormaAsociarDocumentosProducto", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdTipoNotaCredito"));
        var Timbrada = $("#divFormaEditarNotaCredito").attr("timbrada");
        var Cancelada = $("#divFormaEditarNotaCredito").attr("baja");
        /*
        if (Cancelada == 1) {
            MostrarMensajeError("No puede asociar documentos por que la nota de credito esta daba de baja");
            return false;
        }
        if (Timbrada != 0) {
        */
                if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
                NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
                NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idCliente"));
                NotaCredito.IdTipoNotaCredito = IdTipoNotaCredito;
                var validacion = ValidaAsociacionDocumentos(NotaCredito);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.NotaCredito = NotaCredito;
                ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), IdTipoNotaCredito);

            }
            else {
                AgregarNotaCreditoEdicion(IdTipoNotaCredito);
            }
        /*}
        else {
            MostrarMensajeError("Debe timbrar la nota de credito para poder asociar documentos");
            return false;
        }*/
    });

    $("#dialogAgregarNotaCreditoDevolucionCancelacion").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdTipoNotaCredito"));
        if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
            NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
            NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idCliente"));
            NotaCredito.IdTipoNotaCredito = IdTipoNotaCredito;
            var validacion = ValidaAsociacionDocumentos(NotaCredito);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.NotaCredito = NotaCredito;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), IdTipoNotaCredito);

        }
        else {
            AgregarNotaCreditoEdicion(IdTipoNotaCredito);
        }
    });

    $("#dialogEditarNotaCredito").on("click", "#btnObtenerFormaAsociarProductos", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("IdTipoNotaCredito"));
        if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
            var Timbrada = $("#divFormaEditarNotaCredito").attr("timbrada");
            var Cancelada = $("#divFormaEditarNotaCredito").attr("baja");
            if (Cancelada == 1) {
                MostrarMensajeError("No puede asociar productos por que la nota de credito esta daba de baja");
                return false;
            }
            if (Timbrada == 0) {
                NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
                NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente"));
                NotaCredito.pIdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdTipoNotaCredito"));
                var oRequest = new Object();
                oRequest.NotaCredito = NotaCredito;
                $("#cmbTipoMoneda").attr("disabled", "true");
                FiltroProductosNotaCreditoDevolucionCancelacion();
                $("#txtFacturasSeleccionadas").val("");
                $("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
            }
            else {
                MostrarMensajeError("La nota de crédito se encuentra timbrada");
                return false;
            }
        }
        else {
            AgregarNotaCreditoEdicionDevolucionCancelacion(IdTipoNotaCredito);
        }

    });

    $("#dialogAgregarNotaCreditoDevolucionCancelacion").on("click", "#btnObtenerFormaAsociarProductos", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("IdTipoNotaCredito"));
        if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
            NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
            NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente"));
            NotaCredito.pIdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdTipoNotaCredito"));
            var oRequest = new Object();
            oRequest.NotaCredito = NotaCredito;
            $("#cmbTipoMoneda").attr("disabled", "true");
            FiltroProductosNotaCreditoDevolucionCancelacion();
            $("#txtFacturasSeleccionadas").val("");
            $("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
        }
        else {
            AgregarNotaCreditoEdicionDevolucionCancelacion(IdTipoNotaCredito);
        }

    });

    $("#dialogAgregarNotaCreditoDevolucionCancelacion").on("click", "#btnObtenerProductosAsociados", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("IdTipoNotaCredito"));

        if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
            NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
            var oRequest = new Object();
            oRequest.NotaCredito = NotaCredito;
            FiltroProductosAsociadosNotaCreditoDevolucionCancelacion(JSON.stringify(oRequest));
            //console.log(384);
            Productos.idsFacturasDetalle.length = 0;
            $("#dialogMuestraProductosAsociados").dialog("open");
        }
        else {
            //Valida Nota
            var validacion = ValidaSiExisteNotaCredito();
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }

        }

    });

    $("#dialogEditarNotaCredito").on("click", "#btnObtenerProductosAsociados", function() {
        var NotaCredito = new Object();
        var IdTipoNotaCredito = parseInt($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("IdTipoNotaCredito"));
        var Cancelada = $("#divFormaEditarNotaCredito").attr("baja");
        if (Cancelada == 1) {
            MostrarMensajeError("No puede asociar documentos por que la nota de credito esta daba de baja");
            return false;
        }
        if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
            var Timbrada = $("#divFormaEditarNotaCredito").attr("timbrada");
            if (Timbrada == 0) {
                NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
                var oRequest = new Object();
                oRequest.NotaCredito = NotaCredito;
                FiltroProductosAsociadosNotaCreditoDevolucionCancelacion(JSON.stringify(oRequest));
                //console.log(412);
                $("#dialogMuestraProductosAsociados").dialog("open");
            }
            else {
                MostrarMensajeError("La nota de crédito se encuentra timbrada");
                return false;
            }
        }
        else {
            //Valida Nota
            var validacion = ValidaSiExisteNotaCredito();
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }

        }

    });

    $('#dialogAgregarNotaCredito').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarNotaCredito").remove();
        },
        buttons: {
            "Timbrar": function() {
                var NotaCredito = new Object();
                NotaCredito.IdNotaCredito = $("#divFormaAgregarNotaCredito").attr("IdNotaCredito");

                if (NotaCredito.IdNotaCredito != "0" && NotaCredito.IdNotaCredito != "" && NotaCredito.IdNotaCredito != null) {
                    ObtenerFormaDatosFiscales(JSON.stringify(NotaCredito));
                }
                else {
                    MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                }

            },
            "Guardar": function() {
                AgregarNotaCredito();

            },
            "Salir": function() {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogAgregarNotaCreditoDevolucionCancelacion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarNotaCreditoDevolucionCancelacion").remove();

        },
        buttons: {
            //            "Timbrar": function() {
            //                var NotaCredito = new Object();
            //                NotaCredito.IdNotaCredito = $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito");

            //                if (NotaCredito.IdNotaCredito != "0" && NotaCredito.IdNotaCredito != "" && NotaCredito.IdNotaCredito != null) {
            //                    ObtenerFormaDatosFiscales(JSON.stringify(NotaCredito));
            //                }
            //                else {
            //                    MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
            //                }

            //            },
            "Guardar": function() {
                if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
                    if ($("#txtTotal").val() == "0") {
                        MostrarMensajeError("Favor de asociar productos y guardar la nota de crédito");
                        return;
                    }
                    else {
                        AgregarNotaCreditoDevolucionCancelacion();
                    }
                }
                else {
                    MostrarMensajeError("Favor de crear una nota de crédito");
                }

            },
            "Salir": function() {
                //                if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != null && $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") != "") {
                //                    if ($("#txtTotal").val() == "0") {
                //                        MostrarMensajeError("Favor de asociar productos y guardar la nota de crédito");
                //                        return;
                //                    }
                //                    else {
                //                        $(this).dialog("close");
                //                    }
                //                }
                //                else {
                //                    $(this).dialog("close");
                //                }
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarNotaCredito').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarNotaCredito").remove();
        },
        buttons: {
            "Timbrar": function() {
                var NotaCredito = new Object();
                NotaCredito.IdNotaCredito = $("#divFormaConsultarNotaCredito").attr("IdNotaCredito");

                if (NotaCredito.IdNotaCredito != "0" && NotaCredito.IdNotaCredito != "" && NotaCredito.IdNotaCredito != null) {
                    ObtenerFormaDatosFiscales(JSON.stringify(NotaCredito));
                }
                else {
                    MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                }
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarNotaCredito').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarNotaCredito").remove();
        },
        buttons: {
            "Timbrar": function() {
                var NotaCredito = new Object();
                NotaCredito.IdNotaCredito = $("#divFormaEditarNotaCredito").attr("IdNotaCredito");

                if (NotaCredito.IdNotaCredito != "0" && NotaCredito.IdNotaCredito != "" && NotaCredito.IdNotaCredito != null) {
                    ObtenerFormaDatosFiscales(JSON.stringify(NotaCredito));
                }
                else {
                    MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                }
            },
            "Editar": function() {
                EditarNotaCredito();
            },
            "Salir": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogMuestraAsociarDocumentos').dialog({
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
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraAsociarProductosDevolucionCancelacion').dialog({
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
            "Guardar": function() {
                if ($("#txtFacturasSeleccionadas").val() == "") {
                    MostrarMensajeError("Seleccionar productos ha devolucion");
                    return;
                }

                var pRequest = new Object();
                pRequest.pDevoluciones = $("#txtFacturasSeleccionadas").val();
                pRequest.pIdNotaCredito = $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idNotaCredito");

                pRequest.IdsFacturas = new Array();
                $.each(Productos.idsFacturasDetalle, function(pIndex, oProducto) {
                    var DetalleFactura = new Object();
                    DetalleFactura.IdDetalleFactura = oProducto.idFacturaDetalle;
                    DetalleFactura.Cantidad = oProducto.Cantidad;
                    pRequest.IdsFacturas.push(DetalleFactura);


                });

                var oRequest = new Object();
                oRequest.pRequest = pRequest;
                Productos.idsFacturasDetalle.length = 0;
                DevolucionProductos(JSON.stringify(oRequest))
                $(this).dialog("close");
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraProductosAsociados').dialog({
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
            "Eliminar": function() {
                if ($("#txtProductosSeleccionados").val() == "") {
                    MostrarMensajeError("Seleccionar productos ha eliminar");
                    return;
                }
                EliminarProductosAsociados();
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogDatosFiscales').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Timbrar": function() {
                TimbrarNotaCredito();
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMotivoCancelacion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Cancelar": function() {
                CancelarNotaCredito();
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogSeleccionarTipoNotaCredito').dialog({
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
                request.pIdTipoNotaCredito = $("#cmbTipoNotaCredito").val();
                if (request.pIdTipoNotaCredito != 0) {
                    ObtenerFormaAgregarNotaCredito(request);
                    $("#dialogSeleccionarTipoNotaCredito").dialog("close");
                }
                else {
                    MostrarMensajeError("Debe de seleccionar un tipo de nota");
                }
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogFacturaFormato').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarProductosCantidades').dialog({
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
                AgregarProductosPorCantidad();
            },
            "Cancelar": function() {
                var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
                var registro = $(this).parents("tr");
                var idFactura = $("#dialogAgregarProductosCantidades").attr("idFacturaDetalle");
                $.each(facturasSeleccionadas, function(pIndex, pIdFactura) {
                    if (pIdFactura == idFactura) {
                        facturasSeleccionadas.splice(pIndex, 1);
                        $.each(Productos.idsFacturasDetalle, function(pIndex, oProducto) {
                            if (idFactura == oProducto.idFacturaDetalle) {
                                Productos.idsFacturasDetalle.splice(pIndex, 1);
                            }
                        });
                    }
                });
                $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
                $(this).dialog("close");
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAsociarDocumentos(NotaCredito) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentosNotaCredito.html",
        url: "NotaCredito.aspx/ObtenerFormaAsociarDocumentos",
        parametros: NotaCredito,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.IdTipoNotaCredito == 1) {
                
                if (pRespuesta.modelo.Permisos.TotalNotaCreditoTimbrada > 0) {
                    FiltroFacturas();
                    FiltroMovimientosCobros();
                    $("#dialogMuestraAsociarDocumentos").dialog("open");
                }
                else {

                    MostrarMensajeError("La nota de crédito no esta timbrada");
                }

            }
            else {
                FiltroFacturas();
                FiltroMovimientosCobros();
                $("#dialogMuestraAsociarDocumentos").dialog("open");
            }
        }
    });
}
function ObtenerFormaAgregarNotaCredito(pRequest) {
    if (pRequest.pIdTipoNotaCredito == 1) {
        $("#dialogAgregarNotaCreditoDevolucionCancelacion").obtenerVista({
            url: "NotaCredito.aspx/ObtenerFormaAgregarNotaCreditoDevolucionCancelacion",
            parametros: JSON.stringify(pRequest),
            nombreTemplate: "tmplAgregarNotaCreditoDevolucionCancelacion.html",
            despuesDeCompilar: function(pRespuesta) {
                $("#dialogAgregarNotaCreditoDevolucionCancelacion").dialog("open");
                $("#txtFecha").datepicker();
                AutocompletarCliente(pRequest.pIdTipoNotaCredito);
            }
        });
    }
    else {
        
        $("#dialogAgregarNotaCredito").obtenerVista({
            url: "NotaCredito.aspx/ObtenerFormaAgregarNotaCredito",
            parametros: JSON.stringify(pRequest),
            nombreTemplate: "tmplAgregarNotaCredito.html",
            despuesDeCompilar: function(pRespuesta) {
                $("#dialogAgregarNotaCredito").dialog("open");
                $("#txtFecha").datepicker();
                AutocompletarCliente(pRequest.pIdTipoNotaCredito);
            }
        });
    }
}

function ObtenerFormaFiltrosNotaCredito() {
    $("#divFiltrosNotaCredito").obtenerVista({
        nombreTemplate: "tmplFiltrosNotaCredito.html",
        url: "NotaCredito.aspx/ObtenerFormaFiltroNotaCredito",
        despuesDeCompilar: function(pRespuesta) {
        
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroNotaCredito();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                    FiltroNotaCredito();
                    }
                });
            }

            $('#divFiltrosNotaCredito').on('click', '#chkPorFecha', function(event) {
                FiltroNotaCredito();
            });

            $('#divFiltrosNotaCredito').on('change', '#cmbFiltroTimbrado', function(event) {
                FiltroNotaCredito();
            });

        }
    });
}

function EliminarProductosAsociados() {
    var pRequest = new Object();
    pRequest.pIdDevoluciones = $("#txtProductosSeleccionados").val();
    pRequest.pIdNotaCredito = $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idNotaCredito");
    $("#txtProductosSeleccionados").val("");
    SetEliminarProductosAsociados(JSON.stringify(pRequest));
}
function SetEliminarProductosAsociados(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/EliminarProductosAsociados",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCredito").trigger("reloadGrid");
                $("#txtMonto").val(respuesta.TotalMonto);
                $("#txtIVA").val(respuesta.IVA);
                $("#txtTotal").val(respuesta.Total);
                $("#grdNotaCredito").trigger("reloadGrid");
                $("#dialogMuestraProductosAsociados").dialog("close");
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

function ObtenerFormaComboTipoNotaCredito() {
    $("#dialogSeleccionarTipoNotaCredito").obtenerVista({
        nombreTemplate: "tmplFiltroComboTipoNotaCredito.html",
        url: "NotaCredito.aspx/LlenaComboTipoNotaCredito",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogSeleccionarTipoNotaCredito").dialog("open");
        }
    });
}



function ObtenerFormaDatosFiscales(pRequest) {
    $("#dialogDatosFiscales").obtenerVista({
        nombreTemplate: "tmplDatosFiscales.html",
        parametros: pRequest,
        url: "NotaCredito.aspx/LlenaDatosFiscales",
        despuesDeCompilar: function() {
            $("#dialogDatosFiscales").dialog("open");
        }
    });
}

function ObtenerFormaMotivoCancelacion(pRequest) {
    $("#dialogMotivoCancelacion").obtenerVista({
        nombreTemplate: "tmplMotivoCancelacion.html",
        parametros: pRequest,
        url: "NotaCredito.aspx/LlenaMotivoCancelacion",
        despuesDeCompilar: function() {
            $("#dialogMotivoCancelacion").dialog("open");
        }
    });
}

function ObtenerFormaConsultarNotaCreditoFormato(pRequest) {
    $("#dialogFacturaFormato").obtenerVista({
        nombreTemplate: "tmplFacturaFormato.html",
        parametros: pRequest,
        url: "NotaCredito.aspx/ObtieneFacturaFormato",
        despuesDeCompilar: function(pRespuesta) {
            jQuery("#dialogFacturaFormato").empty();
            jQuery("#dialogFacturaFormato").append('<iframe src="' + pRespuesta.modelo.Ruta + '" style="width:750px; height:550px;"></iframe>');
            $("#dialogFacturaFormato").dialog("open");
        }
    });
}

function ObtenerFormaConsultarNotaCreditoXML(pRequest) {
    $.ajax({
        url: "NotaCredito.aspx/ObtieneFacturaXML",
        data: pRequest,
        type: "post",
        contentType: 'application/json; charset=utf-8',
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                downloadURI(json.xml, json.name);

            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function downloadURI(uri, name) {
    var link = document.createElement("a");
    link.download = name;
    link.href = uri;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}

function ObtenerFormaConsultarNotaCredito(pIdNotaCredito) {
    $("#dialogConsultarNotaCredito").obtenerVista({
        nombreTemplate: "tmplConsultarNotaCredito.html",
        url: "NotaCredito.aspx/ObtenerFormaNotaCredito",
        parametros: pIdNotaCredito,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarNotaCredito == 1) {

                if (pRespuesta.modelo.IdTxtTimbradosNotaCredito == 0) {

                    $("#dialogConsultarNotaCredito").dialog("option", "buttons", {
                        "Timbrar": function() {
                            var NotaCredito = new Object();
                            NotaCredito.IdNotaCredito = $("#divFormaConsultarNotaCredito").attr("IdNotaCredito");

                            if (NotaCredito.IdNotaCredito != "0" && NotaCredito.IdNotaCredito != "" && NotaCredito.IdNotaCredito != null) {
                                ObtenerFormaDatosFiscales(JSON.stringify(NotaCredito));
                            }
                            else {
                                MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                            }
                        },
                        "Editar": function() {
                            $(this).dialog("close");
                            var NotaCredito = new Object();
                            NotaCredito.IdNotaCredito = parseInt($("#divFormaConsultarNotaCredito").attr("IdNotaCredito"));
                            ObtenerFormaEditarNotaCredito(JSON.stringify(NotaCredito))
                        }
                    });
                }
                else {
                    $("#dialogConsultarNotaCredito").dialog("option", "buttons", {                        
                        "Editar": function() {
                            $(this).dialog("close");
                            var NotaCredito = new Object();
                            NotaCredito.IdNotaCredito = parseInt($("#divFormaConsultarNotaCredito").attr("IdNotaCredito"));
                            ObtenerFormaEditarNotaCredito(JSON.stringify(NotaCredito))
                        }
                    });                
                }
                $("#dialogConsultarNotaCredito").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarNotaCredito").dialog("option", "buttons", {});
                $("#dialogConsultarNotaCredito").dialog("option", "height", "100");
            }
            $("#dialogConsultarNotaCredito").dialog("open");
        }
    });
}

function ObtenerFormaEditarNotaCredito(IdNotaCredito) {
    $("#dialogEditarNotaCredito").obtenerVista({
        nombreTemplate: "tmplEditarNotaCredito.html",
        url: "NotaCredito.aspx/ObtenerFormaEditarNotaCredito",
        parametros: IdNotaCredito,
        despuesDeCompilar: function(pRespuesta) {
        $("#divFormaEditarNotaCredito").attr("timbrada", pRespuesta.modelo.IdTxtTimbradosNotaCredito);
            Inicializar_grdMovimientosCobrosEditar();
            $("#dialogEditarNotaCredito").dialog("open");
            $("#txtFecha").datepicker();
            AutocompletarCliente(2);
            if (pRespuesta.modelo.Permisos.puedeEditarNotaCredito == 1) {

                if (pRespuesta.modelo.IdTxtTimbradosNotaCredito == 0) {

                    $("#dialogEditarNotaCredito").dialog("option", "buttons", {
                        "Timbrar": function() {
                            var NotaCredito = new Object();
                            NotaCredito.IdNotaCredito = $("#divFormaEditarNotaCredito").attr("IdNotaCredito");

                            if (NotaCredito.IdNotaCredito != "0" && NotaCredito.IdNotaCredito != "" && NotaCredito.IdNotaCredito != null) {
                                ObtenerFormaDatosFiscales(JSON.stringify(NotaCredito));
                            }
                            else {
                                MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                            }
                        },
                        "Editar": function() {
                            EditarNotaCredito();
                        },
                        "Salir": function() {
                            $(this).dialog("close")
                        }
                    });
                }
                else {
                    $("#dialogEditarNotaCredito").dialog("option", "buttons", {
                        "Salir": function() {
                            $(this).dialog("close")
                        }
                    });
                }
                $("#dialogEditarNotaCredito").dialog("option", "height", "auto");
            }
            else {
                $("#dialogEditarNotaCredito").dialog("option", "buttons", {});
                $("#dialogEditarNotaCredito").dialog("option", "height", "100");
            }

        }
    });
}

function ObtenerNumeroNotaCredito(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Notacredito.aspx/ObtenerNumeroNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {                
                $("#txtFolioNotaCredito").val(respuesta.Modelo.NumeroNotaCredito);               
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
function ObtenerFormaProductosCantidades() {
    $("#dialogAgregarProductosCantidades").obtenerVista({
        //nombreTemplate: "tmplFacturaFormato.html",
        //parametros: pRequest,
        //url: "FacturaCliente.aspx/ObtieneFacturaFormato",
        despuesDeCompilar: function(pRespuesta) {
        $("#dialogAgregarProductosCantidades").dialog("open");
        }
    });
}

function ObtenerTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.Modelo.TipoCambioActual == 0) {
                    $("#txtTipoCambio").val(1);
                }
                else {
                    $("#txtTipoCambio").val(respuesta.Modelo.TipoCambioActual);
                }
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

function RecalculaDatosMonto() {
    var Monto = 0;
    var PorcentajeIVA = 0;
    var IVA = 0;
    var Total = 0;
    Monto = $("#txtMonto").val();
    IVA = $("#txtPorcentajeIVA").val();

    if (Monto == "") {
        Monto = 0;
    }
    if (IVA == "") {
        IVA = 0;
    }
    IVA = ((parseFloat(Monto) * parseFloat(IVA)) / parseFloat(100));
    Total = ((parseFloat(Monto)) + (parseFloat(IVA)));    
    $("#txtIVA").val(IVA);
    $("#txtTotal").val(Total);
    
}

function RecalculaDatosTotal() {
    var Monto = 0;
    var PorcentajeIVA = 0;
    var IVA = 0;
    var Total = 0;
    Total = $("#txtTotal").val();
    Monto = $("#txtMonto").val();
    IVA = $("#txtPorcentajeIVA").val();

    if (Total == "") {
        Total = 0;
    }
    if (Monto == "") {
        Monto = 0;
    }
    if (IVA == "") {
        IVA = 0;
    }

    Monto = ((parseFloat(Total) * parseFloat(100)) / (parseFloat(100) + parseFloat(IVA)));    
    IVA = ((parseFloat(Monto) * parseFloat(IVA)) / parseFloat(100));
    Total = ((parseFloat(Monto)) + (parseFloat(IVA)));
    $("#txtIVA").val(IVA);
    $("#txtMonto").val(Monto);
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarNotaCredito() {
    var pNotaCredito = new Object();
    if ($("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente") == null) {
        pNotaCredito.IdCliente = 0;
    }
    else {
        pNotaCredito.IdCliente = $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente");
    }
    
    if ($("#divFormaAgregarNotaCredito").attr("IdNotaCredito") == "" || $("#divFormaAgregarNotaCredito").attr("IdNotaCredito") == null) {

        pNotaCredito.SerieNotaCredito = $("#cmbSerieNotaCredito option:selected").html();
        pNotaCredito.FolioNotaCredito = $("#txtFolioNotaCredito").val();
        pNotaCredito.Descripcion = $("#txtDescripcion").val();
        pNotaCredito.Fecha = $("#txtFecha").val();
        pNotaCredito.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pNotaCredito.TipoCambio = $("#txtTipoCambio").val();
        pNotaCredito.Referencia = $("#txtReferencia").val();
        pNotaCredito.Monto = $("#txtMonto").val();
        pNotaCredito.PorcentajeIVA = $("#txtPorcentajeIVA").val();
        pNotaCredito.IVA = $("#txtIVA").val();
        pNotaCredito.Total = $("#txtTotal").val();
        pNotaCredito.IdTipoNotaCredito = $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idTipoNotaCredito");
        var validacion = ValidaNotaCredito(pNotaCredito);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCredito = pNotaCredito;
        SetAgregarNotaCredito(JSON.stringify(oRequest), pNotaCredito.IdTipoNotaCredito);
    }
    else {
        $("#dialogAgregarNotaCredito").dialog("close");
    }
}

function AgregarNotaCreditoDevolucionCancelacion() {
    var pNotaCredito = new Object();
    if ($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == null) {
        pNotaCredito.IdCliente = 0;
    }
    else {
        pNotaCredito.IdCliente = $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente");
    }
    if ($("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") == "" || $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") == null) {
        //
        pNotaCredito.SerieNotaCredito = $("#cmbSerieNotaCredito option:selected").html();
        pNotaCredito.FolioNotaCredito = $("#txtFolioNotaCredito").val();
        pNotaCredito.Descripcion = $("#txtDescripcion").val();
        pNotaCredito.Fecha = $("#txtFecha").val();
        pNotaCredito.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pNotaCredito.TipoCambio = $("#txtTipoCambio").val();
        pNotaCredito.Referencia = $("#txtReferencia").val();
        pNotaCredito.Monto = $("#txtMonto").val();
        pNotaCredito.PorcentajeIVA = $("#txtPorcentajeIVA").val();
        pNotaCredito.IVA = $("#txtIVA").val();
        pNotaCredito.Total = $("#txtTotal").val();
        pNotaCredito.IdTipoNotaCredito = $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito");
        //Valida Nota
        var validacion = ValidaNotaCreditoDevolucionCancelacion(pNotaCredito);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCredito = pNotaCredito;
        SetAgregarNotaCredito(JSON.stringify(oRequest), pNotaCredito.IdTipoNotaCredito);
    }
    else {
        //
        var pNotaCredito = new Object();
        pNotaCredito.IdNotaCredito = $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idNotaCredito");
        if ($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == null) {
            pNotaCredito.IdCliente = 0;
        }
        else {
            pNotaCredito.IdCliente = $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente");
        }
        pNotaCredito.SerieNotaCredito = $("#cmbSerieNotaCredito option:selected").html();
        pNotaCredito.FolioNotaCredito = $("#txtFolioNotaCredito").val();
        pNotaCredito.Descripcion = $("#txtDescripcion").val();
        pNotaCredito.Fecha = $("#txtFecha").val();
        pNotaCredito.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pNotaCredito.TipoCambio = $("#txtTipoCambio").val();
        pNotaCredito.Referencia = $("#txtReferencia").val();
        pNotaCredito.Monto = $("#txtMonto").val();
        pNotaCredito.PorcentajeIVA = $("#txtPorcentajeIVA").val();
        pNotaCredito.IVA = $("#txtIVA").val();
        pNotaCredito.Total = $("#txtTotal").val();
        pNotaCredito.IdTipoNotaCredito = $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito");
        //Valida Nota
        var validacion = ValidaNotaCreditoDevolucionCancelacion(pNotaCredito);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCredito = pNotaCredito;
        SetEditarNotaCreditoDevolucionCancelacion(JSON.stringify(oRequest), pNotaCredito.IdTipoNotaCredito);
    }
}

function AgregarNotaCreditoEdicion(IdTipoNotaCredito) {
    var pNotaCredito = new Object();
    if (IdTipoNotaCredito == 1) {
        if ($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == null) {
            pNotaCredito.IdCliente = 0;
        }
        else {
            pNotaCredito.IdCliente = $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente");
        }
        
    }
    else {
        if ($("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente") == null) {
            pNotaCredito.IdCliente = 0;
        }
        else {
            pNotaCredito.IdCliente = $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente");
        }
    }
    pNotaCredito.SerieNotaCredito = $("#cmbSerieNotaCredito option:selected").html();
    pNotaCredito.FolioNotaCredito = $("#txtFolioNotaCredito").val();
    pNotaCredito.Descripcion = $("#txtDescripcion").val();
    pNotaCredito.Fecha = $("#txtFecha").val();
    pNotaCredito.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pNotaCredito.TipoCambio = $("#txtTipoCambio").val();
    pNotaCredito.Referencia = $("#txtReferencia").val();
    pNotaCredito.Monto = $("#txtMonto").val();
    pNotaCredito.PorcentajeIVA = $("#txtPorcentajeIVA").val();
    pNotaCredito.IVA = $("#txtIVA").val();
    pNotaCredito.Total = $("#txtTotal").val();
    pNotaCredito.IdTipoNotaCredito = IdTipoNotaCredito;
    var validacion = "";
    if (IdTipoNotaCredito == 1) {
        validacion = ValidaNotaCreditoEdicionDevolucionCancelacion(pNotaCredito);
        
    }
    else {
        validacion = ValidaNotaCreditoEdicion(pNotaCredito);
    }
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCredito = pNotaCredito;
    SetAgregarNotaCreditoEdicion(JSON.stringify(oRequest), IdTipoNotaCredito);
}

//

function AgregarNotaCreditoEdicionDevolucionCancelacion(IdTipoNotaCredito) {
    var pNotaCredito = new Object();
    if ($("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente") == null) {
        pNotaCredito.IdCliente = 0;
    }
    else {
        pNotaCredito.IdCliente = $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente");
    }
    
    pNotaCredito.SerieNotaCredito = $("#cmbSerieNotaCredito option:selected").html();
    pNotaCredito.FolioNotaCredito = $("#txtFolioNotaCredito").val();
    pNotaCredito.Descripcion = $("#txtDescripcion").val();
    pNotaCredito.Fecha = $("#txtFecha").val();
    pNotaCredito.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pNotaCredito.TipoCambio = $("#txtTipoCambio").val();
    pNotaCredito.Referencia = $("#txtReferencia").val();
    pNotaCredito.Monto = $("#txtMonto").val();
    pNotaCredito.PorcentajeIVA = $("#txtPorcentajeIVA").val();
    pNotaCredito.IVA = $("#txtIVA").val();
    pNotaCredito.Total = $("#txtTotal").val();
    pNotaCredito.IdTipoNotaCredito = IdTipoNotaCredito;
    var validacion = "";
    validacion = ValidaNotaCreditoEdicionDevolucionCancelacion(pNotaCredito);
     if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCredito = pNotaCredito;
    SetAgregarNotaCreditoEdicionDevolucionCancelacion(JSON.stringify(oRequest), IdTipoNotaCredito);
}

function TimbrarNotaCredito() {
    var pNotaCredito = new Object();
    if ($("#divFormaAgregarNotaCredito,#divFormaConsultarNotaCredito, #divFormaEditarNotaCredito").attr("idNotaCredito") == "" || $("#divFormaAgregarNotaCredito,#divFormaConsultarNotaCredito, #divFormaEditarNotaCredito").attr("idNotaCredito") == null) {
        MostrarMensajeError("No hay nota de crédito para timbrar"); return false;
    }
    else {
        pNotaCredito.IdNotaCredito = $("#divFormaAgregarNotaCredito,#divFormaConsultarNotaCredito, #divFormaEditarNotaCredito").attr("idNotaCredito");
        pNotaCredito.LugarExpedicion = $("#txtLugarExpedicion").val();
        pNotaCredito.IdMetodoPago = $("#cmbMetodoPago").val();
        pNotaCredito.IdFormaPago = $("#cmbFormaPago").val();
        pNotaCredito.IdCondicionPago = $("#cmbCondicionPago").val();
        pNotaCredito.IdCuentaBancariaCliente = $("#cmbCuentaBancariaCliente").val();
        pNotaCredito.Referencia = $("#txtReferencia").val();
        pNotaCredito.Observaciones = $("#txtObservaciones").val();
        pNotaCredito.IdTipoRelacion = $("#cmbTipoRelacion").val();
        pNotaCredito.IdUsoCFDI = $("#cmbUsoCFDI").val();

        var validacion = ValidaDatosFiscales(pNotaCredito);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }

        pNotaCredito.MetodoPago = $("#cmbMetodoPago option:selected").html();

        if (pNotaCredito.IdFormaPago == 0) {
            pNotaCredito.FormaPago = "";
        }
        else {
            pNotaCredito.FormaPago = $("#cmbFormaPago option:selected").html();
        }

        if (pNotaCredito.IdCondicionPago == 0) {
            pNotaCredito.CondicionPago = "";
        }
        else {
            pNotaCredito.CondicionPago = $("#cmbCondicionPago option:selected").html();
        }

        if (pNotaCredito.IdCuentaBancariaCliente == 0) {
            pNotaCredito.CuentaBancariaCliente = "No identificado";
        }
        else {
            pNotaCredito.CuentaBancariaCliente = $("#cmbCuentaBancariaCliente option:selected").html();
        }
        //var oRequest = new Object();
        //oRequest.pNotaCredito = pNotaCredito;
        //SetTimbrarNotaCredito(JSON.stringify(oRequest));

        //Nueva Forma de Timbrar NC
        //console.log(oRequest);
        oRequest = new Object();
        oRequest.IdNotaCredito = pNotaCredito.IdNotaCredito; 
        oRequest.IdMetodoPago = pNotaCredito.IdMetodoPago;
        oRequest.IdFormaPago = pNotaCredito.IdFormaPago;
        oRequest.Observaciones = pNotaCredito.Observaciones;
        oRequest.CondicionPago = pNotaCredito.CondicionPago;
        oRequest.IdUsoCFDI = pNotaCredito.IdUsoCFDI;
        oRequest.IdTipoRelacion = pNotaCredito.IdTipoRelacion;
        ObtenerNotaCreditoCATimbrar(JSON.stringify(oRequest));

    }
}

function CancelarNotaCredito() {
    var pNotaCredito = new Object();
    if ($("#divFormaAgregarMotivoCancelacion").attr("idNotaCredito") == "" || $("#divFormaAgregarMotivoCancelacion").attr("idNotaCredito") == null) {
        MostrarMensajeError("No hay nota de crédito para cancelar"); return false;
    }
    else {
        pNotaCredito.IdNotaCredito = $("#divFormaAgregarMotivoCancelacion").attr("idNotaCredito");
        pNotaCredito.MotivoCancelacion = $("#txtMotivoCancelacion").val();
        var validacion = ValidaMotivoCancelacion(pNotaCredito);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        //var oRequest = new Object();
        //oRequest.pNotaCredito = pNotaCredito;
        //SetCancelarNotaCredito(JSON.stringify(oRequest));

        // Nueva Forma de Cancelar
        //console.log(pNotaCredito);
        var oRequest = new Object();
        oRequest.IdNotaCredito = parseInt(pNotaCredito.IdNotaCredito);
        oRequest.MotivoCancelacion = pNotaCredito.MotivoCancelacion;
        ObtenerNotaCreditoACancelar(JSON.stringify(oRequest));
    }
}

function SetAgregarNotaCredito(pRequest, pIdTipoNotaCredito) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/AgregarNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCredito").trigger("reloadGrid");
                if (pIdTipoNotaCredito == 2 || pIdTipoNotaCredito == 3) {
                    $("#dialogAgregarNotaCredito").dialog("close");
                }
                if (pIdTipoNotaCredito == 1) {
                    $("#dialogAgregarNotaCreditoDevolucionCancelacion").dialog("close");
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

//
function SetEditarNotaCreditoDevolucionCancelacion(pRequest, pIdTipoNotaCredito) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/EditarNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCredito").trigger("reloadGrid");
                $("#dialogAgregarNotaCreditoDevolucionCancelacion").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
//            $("#dialogAgregarNotaCreditoDevolucionCancelacion").dialog("close");
            OcultarBloqueo();
        }
    });
}


function SetTimbrarNotaCredito(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/TimbrarNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdNotaCredito").trigger("reloadGrid");
                $("#dialogAgregarNotaCredito, #dialogConsultarNotaCredito, #dialogEditarNotaCredito, #dialogDatosFiscales").dialog("close");                
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

function SetCancelarNotaCredito(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/CancelarNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdNotaCredito").trigger("reloadGrid");
                $("#dialogMotivoCancelacion").dialog("close");
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

function SetAgregarNotaCreditoEdicion(pRequest, pIdTipoNotaCredito) {
    if (pIdTipoNotaCredito == 2 || pIdTipoNotaCredito == 3) {
        MostrarBloqueo();
        $.ajax({
            type: "POST",
            url: "NotaCredito.aspx/AgregarNotaCreditoEdicion",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#divFormaAgregarNotaCredito").attr("idNotaCredito", respuesta.IdNotaCredito);
                    $("#grdNotaCredito").trigger("reloadGrid");
                    var NotaCredito = new Object();
                    NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCredito").attr("IdNotaCredito"));
                    NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCredito").attr("IdCliente"));
                    NotaCredito.IdTipoNotaCredito = pIdTipoNotaCredito;
                    var validacion = ValidaAsociacionDocumentos(NotaCredito);
                    if (validacion != "")
                    { MostrarMensajeError(validacion); return false; }
                    var oRequest = new Object();
                    oRequest.NotaCredito = NotaCredito;
                    ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), pIdTipoNotaCredito);
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
    else {

        MostrarBloqueo();
        $.ajax({
            type: "POST",
            url: "NotaCredito.aspx/AgregarNotaCreditoEdicion",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idNotaCredito", respuesta.IdNotaCredito);
                    $("#grdNotaCredito").trigger("reloadGrid");
                    var NotaCredito = new Object();
                    NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
                    NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente"));
                    NotaCredito.IdTipoNotaCredito = pIdTipoNotaCredito;
                    var validacion = ValidaAsociacionDocumentos(NotaCredito);
                    if (validacion != "")
                    { MostrarMensajeError(validacion); return false; }
                    var oRequest = new Object();
                    oRequest.NotaCredito = NotaCredito;
                    if (respuesta.TotalNotaCreditoTimbrada > 0) {
                       //  FiltroProductosNotaCreditoDevolucionCancelacion();
                        //$("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
                        ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), pIdTipoNotaCredito);
                    }
                    else {

                        MostrarMensajeError("La nota de crédito no esta timbrada");
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
}
//AgregarNotaDeCreditoEdicion cuando es por asociar documentos
function SetAgregarNotaCreditoEdicionDevolucionCancelacion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/AgregarNotaCreditoEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idNotaCredito", respuesta.IdNotaCredito);
                $("#grdNotaCredito").trigger("reloadGrid");
                var NotaCredito = new Object();
                NotaCredito.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
                NotaCredito.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente"));
                var validacion = ValidaAsociacionDocumentos(NotaCredito);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.NotaCredito = NotaCredito;
                if (respuesta.TotalNotaCreditoTimbrada == 0) {
                    $("#cmbTipoMoneda").attr("disabled", "true");
                    FiltroProductosNotaCreditoDevolucionCancelacion();
                    $("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
                }
                else {

                    MostrarMensajeError("La nota de crédito ya esta timbrada");
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



function SetCambiarEstatus(pIdNotaCredito, pBaja) {
    var pRequest = "{'pIdNotaCredito':" + pIdNotaCredito + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdNotaCredito").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdNotaCredito').one('click', '.div_grdNotaCredito_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdNotaCredito_AI']").children().attr("baja")
                var idNotaCredito = $(registro).children("td[aria-describedby='grdNotaCredito_IdNotaCredito']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idNotaCredito, baja);
            });
        }
    });
}

function EditarNotaCredito() {
    var pNotaCredito = new Object();
    pNotaCredito.IdNotaCredito = $("#divFormaEditarNotaCredito").attr("idNotaCredito");
    if ($("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente") == "" || $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente") == null) {
        pNotaCredito.IdCliente = 0;
    }
    else {
        pNotaCredito.IdCliente = $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente");
    }
    pNotaCredito.SerieNotaCredito = $("#txtSerieNotaCredito").val();
    pNotaCredito.FolioNotaCredito = $("#txtFolioNotaCredito").val();
    pNotaCredito.Descripcion = $("#txtDescripcion").val();
    pNotaCredito.Fecha = $("#txtFecha").val();
    pNotaCredito.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pNotaCredito.TipoCambio = $("#txtTipoCambio").val();
    pNotaCredito.Referencia = $("#txtReferencia").val();
    pNotaCredito.Monto = $("#txtMonto").val();
    pNotaCredito.PorcentajeIVA = $("#txtPorcentajeIVA").val();
    pNotaCredito.IVA = $("#txtIVA").val();
    pNotaCredito.Total = $("#txtTotal").val();
    pNotaCredito.IdTipoNotaCredito = $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idTipoNotaCredito");
    var validacion = ValidaNotaCredito(pNotaCredito);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCredito = pNotaCredito;
    SetEditarNotaCredito(JSON.stringify(oRequest));
}
function SetEditarNotaCredito(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/EditarNotaCredito",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCredito").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarNotaCredito").dialog("close");
        }
    });
}

function EdicionFacturas(valor, id, rowid, iCol) {
    var TipoMoneda = $('#grdFacturas').getCell(rowid, 'TipoMoneda');
    var Saldo = $('#grdFacturas').getCell(rowid, 'Saldo');
    var EsParcialidad = $('#grdFacturas').getCell(rowid, 'EsParcialidad');

    var NotaCredito = new Object();

    if (EsParcialidad == true || EsParcialidad == "True") {
        NotaCredito.EsParcialidad = 1;
    }
    else {
        NotaCredito.EsParcialidad = 0;
    } 

    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        NotaCredito.IdTipoMoneda = 1;
        NotaCredito.TipoCambio = 1;
        NotaCredito.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        NotaCredito.IdTipoMoneda = 2;
        NotaCredito.TipoCambio = $("#spanTipoCambioDolares").text();
        NotaCredito.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    NotaCredito.TipoMoneda = TipoMoneda;

    NotaCredito.Monto = QuitarFormatoNumero(valor);
    NotaCredito.Saldo = QuitarFormatoNumero(Saldo);
    NotaCredito.IdEncabezadoFactura = id;
    NotaCredito.rowid = rowid;
    NotaCredito.IdNotaCredito = $("#divFormaAsociarDocumentos").attr("idNotaCredito");
    var validacion = ValidarMontos(NotaCredito);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCredito = NotaCredito;
    SetEditarMontos(JSON.stringify(oRequest));
    
}

function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {

                if (respuesta.EsParcialidad == 1) {
                    //MostrarMensajeError(respuesta.Descripcion);
                }
                
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdNotaCredito").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosNotaCredito;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosNotaCredito / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));
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

function SetEliminarNotaCreditoEncabezadoFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/EliminarNotaCreditoEncabezadoFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.EsParcialidad == 1) {
                    //MostrarMensajeError(respuesta.Descripcion);
                }
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");
                $("#grdNotaCredito").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosNotaCredito;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosNotaCredito / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));
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

function AutocompletarCliente(pIdTipoNotaCredito) {

    if (pIdTipoNotaCredito == 2 || pIdTipoNotaCredito == 3) {
        $('#txtRazonSocial').autocomplete({
            source: function(request, response) {
                var pRequest = new Object();
                pRequest.pRazonSocial = $('#txtRazonSocial').val();
                $.ajax({
                    type: 'POST',
                    url: 'NotaCredito.aspx/BuscarRazonSocialCliente',
                    data: JSON.stringify(pRequest),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    success: function(pRespuesta) {
                        $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente", "0");
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
                $("#divFormaAgregarNotaCredito, #divFormaEditarNotaCredito").attr("idCliente", pIdCliente);
            },
            change: function(event, ui) { },
            open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
            close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
        });
    }
    else {
        $('#txtRazonSocial').autocomplete({
            source: function(request, response) {
                var pRequest = new Object();
                pRequest.pRazonSocial = $('#txtRazonSocial').val();
                $.ajax({
                    type: 'POST',
                    url: 'NotaCredito.aspx/BuscarRazonSocialCliente',
                    data: JSON.stringify(pRequest),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    success: function(pRespuesta) {
                    $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente", "0");
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
                $("#divFormaAgregarNotaCreditoDevolucionCancelacion, #divFormaEditarNotaCredito").attr("idCliente", pIdCliente);
            },
            change: function(event, ui) { },
            open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
            close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
        });
    }
}


function FiltroFacturas() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturas').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturas').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturas').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturas').getGridParam('sortorder');
    request.pSerie = "";
    request.pIdCliente = 0;
    if ($("#divFormaEditarNotaCredito, #divFormaAsociarDocumentos").attr("IdCliente") != null) {
        request.pIdCliente = $("#divFormaEditarNotaCredito, #divFormaAsociarDocumentos").attr("IdCliente");
        if ($('#divContGridAsociarDocumento').find(gs_Serie).existe()) {
            request.pSerie = $('#divContGridAsociarDocumento').find(gs_Serie).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCredito.aspx/ObtenerFacturas',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturas')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroNotaCredito() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdNotaCredito').getGridParam('rowNum');
    request.pPaginaActual = $('#grdNotaCredito').getGridParam('page');
    request.pColumnaOrden = $('#grdNotaCredito').getGridParam('sortname');
    request.pTipoOrden = $('#grdNotaCredito').getGridParam('sortorder');
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pSerieNotaCredito = "";
    request.pFolioNotaCredito = "";
    request.pPorFecha = 0;
    request.pAI = 0;
    request.pFiltroTimbrado = 0;
    request.pRazonSocial = "";
    request.pAsociado = "NO";
    request.pIdPedido = 0;
    request.pIdFactura = 0;
    request.pIdOrdenCompra = 0;

    var pIdTipoFiltro = ($("#cmbTipoFiltro").val() != null) ? parseInt($("#cmbTipoFiltro").val()) : 0;
    var pValorFiltro = ($("#txtValorFiltro").val() != null && !isNaN(parseInt($("#txtValorFiltro").val()))) ? parseInt($("#txtValorFiltro").val()) : -1;

    switch (pIdTipoFiltro) {
        case 1:
            request.pIdPedido = pValorFiltro;
            break;
        case 2:
            request.pIdFactura = pValorFiltro;
            break;
        case 3:
            request.pIdOrdenCompra = pValorFiltro;
            break;
    }

    if ($('#gs_SerieNotaCredito').val() != null && $('#gs_SerieNotaCredito').val() != "") {
        request.pSerieNotaCredito = $("#gs_SerieNotaCredito").val();
    }
    if ($('#gs_FolioNotaCredito').val() != null) {
        request.pFolioNotaCredito = $("#gs_FolioNotaCredito").val();
    }

    if ($('#gs_RazonSocial').val() != null) {
        request.pRazonSocial = $("#gs_RazonSocial").val();
    }

    if ($('#gs_AI').val() != null) { request.pAI = $("#gs_AI").val(); }

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

        if ($("#chkPorFecha").is(':checked')) {
            request.pPorFecha = 1;
        }
        else {
            request.pPorFecha = 0;
        }
        request.pFiltroTimbrado = $("#cmbFiltroTimbrado").val();

        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }
    if ($("#cmbTipoFiltro").val() != null) { request.pIdTipoFiltro = $("#cmbTipoFiltro").val(); }
    if ($("#txtValorFiltro").val() != null) { request.pValorFiltro = $("#txtValorFiltro").val(); }
    if ($('#gs_Asociado').val() != null) { request.pAsociado = $("#gs_Asociado").val(); }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCredito.aspx/ObtenerNotaCredito',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdNotaCredito')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroMovimientosCobros() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobros').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobros').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobros').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobros').getGridParam('sortorder');
    request.pIdNotaCredito = 0;
    if ($("#divFormaEditarNotaCredito, #divFormaAsociarDocumentos").attr("IdNotaCredito") != null) {
        request.pIdNotaCredito = $("#divFormaEditarNotaCredito, #divFormaAsociarDocumentos").attr("IdNotaCredito");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCredito.aspx/ObtenerMovimientosCobros',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobros')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroMovimientosCobrosConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortorder');
    request.pIdNotaCredito = 0;
    if ($("#divFormaEditarNotaCredito, #divFormaConsultarNotaCredito, #divFormaAsociarDocumentos").attr("IdNotaCredito") != null) {
        request.pIdNotaCredito = $("#divFormaEditarNotaCredito, #divFormaConsultarNotaCredito, #divFormaAsociarDocumentos").attr("IdNotaCredito");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCredito.aspx/ObtenerMovimientosCobrosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroMovimientosCobrosEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosEditar').getGridParam('sortorder');
    request.pIdNotaCredito = 0;
    if ($("#divFormaEditarNotaCredito, #divFormaEditarNotaCredito, #divFormaAsociarDocumentos").attr("IdNotaCredito") != null) {
        request.pIdNotaCredito = $("#divFormaEditarNotaCredito, #divFormaEditarNotaCredito, #divFormaAsociarDocumentos").attr("IdNotaCredito");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCredito.aspx/ObtenerMovimientosCobrosEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroProductosNotaCreditoDevolucionCancelacion() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdProductosNotaCreditoDevolucionCancelacion').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProductosNotaCreditoDevolucionCancelacion').getGridParam('page');
    request.pColumnaOrden = $('#grdProductosNotaCreditoDevolucionCancelacion').getGridParam('sortname');
    request.pTipoOrden = $('#grdProductosNotaCreditoDevolucionCancelacion').getGridParam('sortorder');
    request.pIdTipoMoneda = 0;
    request.pTipoCambio = 0;
    request.pDescripcion = "";
    
    if ($("#cmbTipoMoneda").val() != null) {
        request.pIdTipoMoneda = $("#cmbTipoMoneda").val();
    }
    if ($("#txtTipoCambio").val() != null) {
        request.pTipoCambio = $("#txtTipoCambio").val();
    }
    if ($('#gs_Descripcion').val() != null) {
        request.pDescripcion = $("#gs_Descripcion").val();
    }
    

    if ($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") == null || $("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") == "")
        request.pIdNotaCredito = 0;
    else
        request.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
    if ($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito") == null || $("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito") == "")
        request.pIdTipoNotaCredito = 0;
    else
        request.pIdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito"));

    if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente") == null || $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente") == "")
        request.pIdCliente = 0;
    else
        request.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente"));

    var pRequest = JSON.stringify(request);
    if (request.pIdNotaCredito != 0) {
        $.ajax({
            url: 'NotaCredito.aspx/ObtenerProductosNotaCreditoDevolucionCancelacion',
            data: pRequest,
            dataType: 'json',
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            complete: function(jsondata, stat) {
                if (stat == 'success') {
                    $('#grdProductosNotaCreditoDevolucionCancelacion').val("");
                    $('#grdProductosNotaCreditoDevolucionCancelacion')[0].addJSONData(JSON.parse(jsondata.responseText).d);
                    //$("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
                }
                else
                { alert(JSON.parse(jsondata.responseText).Message); }
            }
        });
    }
}

//NotaCreditoDevolucion
function ObtenerNumeroNotaCreditoDevolucion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Notacredito.aspx/ObtenerNumeroNotaCreditoDevolucion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtFolioNotaCredito").val(respuesta.Modelo.NumeroNotaCredito);
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

function ObtenerTipoCambioDevolucion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCredito.aspx/ObtenerTipoCambioDevolucion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.Modelo.TipoCambioActual == 0) {
                    $("#txtTipoCambio").val(1);
                }
                else {
                    $("#txtTipoCambio").val(respuesta.Modelo.TipoCambioActual);
                }
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
//

function Termino_grdProductosNotaCreditoDevolucionCancelacion() {
    var ids = $('#grdProductosNotaCreditoDevolucionCancelacion').jqGrid('getDataIDs');
    var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]"); 

    for (var i = 0; i < ids.length; i++) {
        idFactura = $('#grdProductosNotaCreditoDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosNotaCreditoDevolucionCancelacion_IdFacturaDetalle"]').html();
        $.each(facturasSeleccionadas, function(pIndex, pIdFactura) {
            if (pIdFactura == idFactura) {
                $('#grdProductosNotaCreditoDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosNotaCreditoDevolucionCancelacion_Sel"] input').prop('checked', true);
            }
            return (pIdFactura !== idFactura);
        });
        $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
    }
}

function Termino_grdProductosAsociadosNotaCreditoDevolucionCancelacion() {
    var ids = $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion').jqGrid('getDataIDs');
    var productosSeleccionados = JSON.parse("[" + $("#txtProductosSeleccionados").val() + "]");

    for (var i = 0; i < ids.length; i++) {
        idDevolucion = $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosAsociadosNotaCreditoDevolucionCancelacion_IdDevolucion"]').html();
        $.each(productosSeleccionados, function(pIndex, pIdDevolucion) {
        if (pIdDevolucion == idDevolucion) {
                $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosAsociadosNotaCreditoDevolucionCancelacion_Sel"] input').prop('checked', true);
            }
            return (pIdDevolucion !== idDevolucion);
        });
        $("#txtProductosSeleccionados").val(productosSeleccionados);
    }
}

//
function FiltroProductosAsociadosNotaCreditoDevolucionCancelacion() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion').getGridParam('page');
    request.pColumnaOrden = $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion').getGridParam('sortname');
    request.pTipoOrden = $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion').getGridParam('sortorder');
    request.pDescripcion = "";

    if ($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") == null || $("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito") == "")
        request.pIdNotaCredito = 0;
    else
        request.pIdNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdNotaCredito"));
    if ($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito") == null || $("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito") == "")
        request.pIdTipoNotaCredito = 0;
    else
        request.pIdTipoNotaCredito = parseInt($("#divFormaEditarNotaCredito,#divFormaAgregarNotaCreditoDevolucionCancelacion").attr("idTipoNotaCredito"));

    if ($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente") == null || $("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente") == "")
        request.pIdCliente = 0;
    else
        request.pIdCliente = parseInt($("#divFormaEditarNotaCredito, #divFormaAgregarNotaCreditoDevolucionCancelacion").attr("IdCliente"));

    if ($('#gs_DescripcionProducto').val() != null) {
        request.pDescripcion = $("#gs_DescripcionProducto").val();
    }

    var pRequest = JSON.stringify(request);
    if (request.pIdNotaCredito != 0) {
        $.ajax({
            url: 'NotaCredito.aspx/ObtenerProductosAsociadosNotaCreditoDevolucionCancelacion',
            data: pRequest,
            dataType: 'json',
            type: 'post',
            contentType: 'application/json; charset=utf-8',
            complete: function(jsondata, stat) {
                if (stat == 'success') {
                    $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion').val("");
                    $('#grdProductosAsociadosNotaCreditoDevolucionCancelacion')[0].addJSONData(JSON.parse(jsondata.responseText).d);
                } else {
                    alert(JSON.parse(jsondata.responseText).Message);
                }
            }
        });
    }
}
function AgregarProductosPorCantidad() {
    var Valores = new Object();
    Valores.Cantidad = $("#txtCantidadDevolver").val();
    var CantidadDisponible = $("#CantidadDisponible").text();
    if (parseInt(Valores.Cantidad) > parseInt(CantidadDisponible) || parseInt(Valores.Cantidad) <= 0) {
        MostrarMensajeError("No puede devolver cantidad mayor a la disponible");
    }
    else {
        Valores.idFacturaDetalle = $("#dialogAgregarProductosCantidades").attr("idFacturaDetalle");
        Valores.Cantidad = $("#txtCantidadDevolver").val();
        Productos.idsFacturasDetalle.push(Valores);
        $("#dialogAgregarProductosCantidades").dialog("close");
        $("#txtCantidadDevolver").val("");
        Termino_grdProductosNotaCreditoDevolucionCancelacion();
    }
}

function EdicionProductosNotaCreditoDevolucionCancelacion(valor, id, rowid, iCol) {

}
 
//-----Validaciones------------------------------------------------------
function ValidaNotaCredito(pNotaCredito) {
    var errores = "";

    if (pNotaCredito.IdCliente == 0)
    { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCredito.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCredito.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCredito.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCredito.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCredito.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCredito.Monto == 0)
    { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCredito.PorcentajeIVA == 0)
    { errores = errores + "<span>*</span> El campo porcentaje IVA esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCredito.IVA == 0)
    { errores = errores + "<span>*</span> El campo IVA esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCredito.Total == 0)
    { errores = errores + "<span>*</span> El campo total esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaNotaCreditoEdicion(pNotaCredito) {
    var errores = "";

    if (pNotaCredito.IdCliente == 0)
    { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.Monto == 0)
    { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.PorcentajeIVA == 0)
    { errores = errores + "<span>*</span> El campo porcentaje IVA esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.IVA == 0)
    { errores = errores + "<span>*</span> El campo IVA esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.Total == 0)
    { errores = errores + "<span>*</span> El campo total esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaNotaCreditoEdicionDevolucionCancelacion(pNotaCredito) {
    var errores = "";

    if (pNotaCredito.IdCliente == 0)
    { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(NotaCredito) {
    var errores = "";

    if (NotaCredito.pIdNotaCredito == 0)
    { errores = errores + "<span>*</span> No hay nota de crédito por asociar, favor de elegir alguno.<br />"; }

    if (NotaCredito.pIdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDatosFiscales(pNotaCredito) {
    var errores = "";

    if (pNotaCredito.pIdNotaCredito == 0)
    { errores = errores + "<span>*</span> No hay nota de crédito por asociar, favor de elegir alguno.<br />"; }

    if (pNotaCredito.LugarExpedicion == "")
    { errores = errores + "<span>*</span> El campo lugar de expedición esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> No hay método de pago por asociar, favor de elegir alguno.<br />"; }
    
    if (pNotaCredito.IdTipoRelacion == 0)
    { errores = errores + "<span>*</span> No hay Tipo de Relacion por asociar, favor de elegir alguno.<br />"; }

    if (pNotaCredito.IdUsoCFDI == 0)
    { errores = errores + "<span>*</span> No hay Uso de CFDI por asociar, favor de elegir alguno.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaMotivoCancelacion(pNotaCredito) {
    var errores = "";

    if (pNotaCredito.pIdNotaCredito == 0)
    { errores = errores + "<span>*</span> No hay nota de crédito por asociar, favor de elegir alguno.<br />"; }

    if (pNotaCredito.MotivoCancelacion == "")
    { errores = errores + "<span>*</span> El campo motivo de cancelación esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(NotaCredito) {
    var errores = "";

    if (NotaCredito.IdEncabezadoFactura == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(NotaCredito.Monto) > parseFloat(NotaCredito.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al disponible.<br />"; }

    if (parseFloat(NotaCredito.Monto) > parseFloat(NotaCredito.Saldo))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(NotaCredito.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(NotaCredito.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
function ValidaNotaCreditoDevolucionCancelacion(pNotaCredito) {
    var errores = "";

    if (pNotaCredito.IdCliente == 0)
    { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCredito.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCredito.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;

}
function ValidaSiExisteNotaCredito() {
    var errores = "";
    errores = errores + "<span>*</span> Favor de crear una nota de credito.<br />"; 

    return errores;
}

function DevolucionProductos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Notacredito.aspx/AgregarDevoluciones",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtMonto").val(respuesta.TotalMonto);
                $("#txtIVA").val(respuesta.IVA);
                $("#txtTotal").val(respuesta.Total);
                $("#grdNotaCredito").trigger("reloadGrid");
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

////////////////////////  Nueva forma de guardar Complementos de Pago /////////////////////////////////////

/* Timbrar */
function ObtenerNotaCreditoCATimbrar(Request) {
    MostrarBloqueo();
    $.ajax({
        url: "NotaCredito.aspx/ObtenerDatosNotaCredito",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                TimbrarNC(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function TimbrarNC(json) {
    var NotaCredito = new Object();
    NotaCredito.NotaCredito = json.Comprobante;
    NotaCredito.Id = json.Id;
    NotaCredito.Token = json.Token;
    NotaCredito.RFC = json.RFC;
    NotaCredito.RefID = json.RefID;
    NotaCredito.Formato = json.Formato;
    NotaCredito.NoCertificado = json.NoCertificado;
    NotaCredito.Correos = json.Correos;
    NotaCredito.RutaCFDI = json.RutaCFDI;
    var Request = JSON.stringify(NotaCredito);
    $.ajax({
        url: "http://" + window.location.hostname + "/WebServiceDiverza/NotaCredito.aspx/TimbrarNotaCredito",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            console.log(Respuesta);
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                GuardarNotaCreditoTimbrada(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function GuardarNotaCreditoTimbrada(json) {
    var Comprobante = new Object();
    Comprobante.UUId = json.uuid;
    Comprobante.RefId = json.ref_id;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "NotaCredito.aspx/GuardarNotaCredito",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            MostrarMensajeError(json.Descripcion);
            OcultarBloqueo();
        }
    });
}

/* Cancelar */
function ObtenerNotaCreditoACancelar(Request) {
    MostrarBloqueo();
    $.ajax({
        url: "NotaCredito.aspx/ObtenerDatosCancelacion",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                //$("#grdFacturas").trigger("reloadGrid");
                //$("#dialogMotivoCancelacionFactura").dialog("close");
                CancelarNotaCred(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function CancelarNotaCred(json) {
    var Comprobante = new Object();
    Comprobante.UUID = json.Comprobante.UUID;
    Comprobante.RefID = json.Comprobante.ref_id;
    Comprobante.Id = json.Id;
    Comprobante.Token = json.Token;
    Comprobante.RFC = json.RFC;
    Comprobante.NoCertificado = json.NoCertificado;
    Comprobante.MotivoCancelacion = json.MotivoCancelacion;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "http://" + window.location.hostname + "/WebServiceDiverza/NotaCredito.aspx/CancelarNotaCredito",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
               EditarNotaCreditoACancelar(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function EditarNotaCreditoACancelar(json) {
    var Comprobante = new Object();
    Comprobante.RefId = json.ref_id;
    Comprobante.Date = json.date;
    Comprobante.message = json.message;
    Comprobante.MotivoCancelacion = json.motivoCancelacion;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "NotaCredito.aspx/EditarNotaCreditoCancelado",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            MostrarMensajeError(json.Descripcion);
            OcultarBloqueo();
        }
    });
}

