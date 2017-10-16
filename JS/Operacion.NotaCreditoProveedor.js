//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
var Productos = new Object();
Productos.idsFacturasDetalle = new Array();

$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("NotaCreditoProveedor");
    });
    ObtenerFormaFiltrosNotaCreditoProveedor();


    //////funcion del grid//////
    $("#gbox_grdNotaCreditoProveedor").livequery(function() {
        $("#grdNotaCreditoProveedor").jqGrid('navButtonAdd', '#pagNotaCreditoProveedor', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pFolioNotaCredito = "";
                var pSerieNotaCredito = "";
                var pAI = -1;
                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;
                var pFiltroTimbrado = 0;

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
                    pFiltroTimbrado: pFiltroTimbrado

                }, downloadType: 'Normal'
                });

            }
        });
    });
    ///////////////////////////


    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarNotaCreditoProveedor", function() {
        ObtenerFormaComboTipoNotaCreditoProveedor();
    });

    //////////    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarNotaCreditoProveedorDevolucionCancelacion", function() {
    //////////        ObtenerFormaComboTipoNotaCreditoProveedorDevolucionCancelacion();
    //////////    });

    $("#grdNotaCreditoProveedor").on("click", ".imgFormaConsultarNotaCreditoProveedor", function() {
        var registro = $(this).parents("tr");
        var NotaCreditoProveedor = new Object();
        NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($(registro).children("td[aria-describedby='grdNotaCreditoProveedor_IdNotaCreditoProveedor']").html());
        ObtenerFormaConsultarNotaCreditoProveedor(JSON.stringify(NotaCreditoProveedor));
    });

    $("#grdNotaCreditoProveedor").on("click", ".imgFormaConsultarFacturaFormato", function() {
        var registro = $(this).parents("tr");
        var NotaCreditoProveedor = new Object();
        NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($(registro).children("td[aria-describedby='grdNotaCreditoProveedor_IdNotaCreditoProveedor']").html());
        ObtenerFormaConsultarNotaCreditoProveedorFormato(JSON.stringify(NotaCreditoProveedor));
    });

    $('#grdNotaCreditoProveedor').on('click', '.div_grdNotaCreditoProveedor_AI', function(event) {

        var registro = $(this).parents("tr");
        var NotaCreditoProveedor = new Object();
        var estatusBaja = $(registro).children("td[aria-describedby='grdNotaCreditoProveedor_AI']").children().attr("baja")
        NotaCreditoProveedor.IdNotaCreditoProveedor = $(registro).children("td[aria-describedby='grdNotaCreditoProveedor_IdNotaCreditoProveedor']").html();
        if (estatusBaja == "0" || estatusBaja == "False") {
            ObtenerFormaMotivoCancelacion(JSON.stringify(NotaCreditoProveedor));
        }
        else {
            MostrarMensajeAviso("Esta nota de crédito, ya esta cancelada");
        }

    });

    //Nota de creditodevolucion
    $('#dialogAgregarNotaCreditoProveedorDevolucionCancelacion, #dialogEditarNotaCreditoProveedorDevolucionCancelacion').on('change', '#cmbSerieNotaCreditoProveedor', function(event) {
        var pNotaCreditoProveedorDevolucion = new Object();
        pNotaCreditoProveedorDevolucion.IdSerieNotaCreditoProveedor = parseInt($(this).val());
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedorDevolucion;
        ObtenerNumeroNotaCreditoProveedorDevolucion(JSON.stringify(oRequest))
    });

    $('#dialogAgregarNotaCreditoProveedorDevolucionCancelacion, #dialogEditarNotaCreditoProveedorDevolucionCancelacion').on('change', '#cmbTipoMoneda', function(event) {
        var pTipoCambioDevolucion = new Object();
        pTipoCambioDevolucion.IdTipoCambioOrigen = parseInt($(this).val());
        pTipoCambioDevolucion.IdTipoCambioDestino = parseInt(1);
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambioDevolucion;
        ObtenerTipoCambioDevolucion(JSON.stringify(oRequest))
    });

    $("#grdProductosNotaCreditoProveedorDevolucionCancelacion").on("click", ".checkAsignarVarios", function() {
        if ($(this).is(':checked')) {
            var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
            var registro = $(this).parents("tr");
            facturasSeleccionadas.push(parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoProveedorDevolucionCancelacion_IdDetalleFacturaProveedor']").html()));
            var cantidad = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoProveedorDevolucionCancelacion_Cantidad']").html());
            var cantidadDisponible = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoProveedorDevolucionCancelacion_Disponible']").html());
            if (cantidadDisponible != 0) {
                var IdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoProveedorDevolucionCancelacion_IdDetalleFacturaProveedor']").html());
                $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
                $("#txtCantidadDevolver").val("");
                $("#Cantidad").empty().append(cantidad);
                $("#CantidadDisponible").empty().append(cantidadDisponible); ;
                $("#dialogAgregarProductosCantidades").attr("idDetalleFacturaProveedor", IdDetalleFacturaProveedor);
                $("#dialogAgregarProductosCantidades").dialog("open");
            }
            else {
                MostrarMensajeError("Este producto no tiene cantidad disponible");
            }
        }
        else {
            var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
            var registro = $(this).parents("tr");
            var idFactura = parseInt($(registro).children("td[aria-describedby='grdProductosNotaCreditoProveedorDevolucionCancelacion_IdDetalleFacturaProveedor']").html());
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

    $("#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion").on("click", ".checkAsignarVarios", function() {
        if ($(this).is(':checked')) {
            var ProductosSeleccionados = JSON.parse("[" + $("#txtProductosSeleccionados").val() + "]");
            var registro = $(this).parents("tr");
            ProductosSeleccionados.push(parseInt($(registro).children("td[aria-describedby='grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion_IdDevolucionProveedor']").html()));
            $("#txtProductosSeleccionados").val(ProductosSeleccionados);
        }
        else {
            var ProductosSeleccionados = JSON.parse("[" + $("#txtProductosSeleccionados").val() + "]");
            var registro = $(this).parents("tr");
            var idDevolucion = parseInt($(registro).children("td[aria-describedby='grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion_IdDevolucionProveedor']").html());
            $.each(ProductosSeleccionados, function(pIndex, pIdDevolucion) {
                if (pIdDevolucion == idDevolucion) {
                    ProductosSeleccionados.splice(pIndex, 1);
                    return (pIdDevolucion !== idDevolucion);
                }
            });
            $("#txtProductosSeleccionados").val(ProductosSeleccionados);
        }
    });

    //    $('#dialogAgregarNotaCreditoProveedor, #dialogEditarNotaCreditoProveedor').on('change', '#cmbSerieNotaCreditoProveedor', function(event) {
    //        var pNotaCreditoProveedor = new Object();
    //        pNotaCreditoProveedor.IdSerieNotaCreditoProveedor = parseInt($(this).val());
    //        var oRequest = new Object();
    //        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
    //        ObtenerNumeroNotaCreditoProveedor(JSON.stringify(oRequest))
    //    });
    $('#dialogAgregarNotaCreditoProveedor, #dialogEditarNotaCreditoProveedor').on('change', '#cmbTipoMoneda', function(event) {
        var pTipoCambio = new Object();
        pTipoCambio.IdTipoCambioOrigen = parseInt($(this).val());
        pTipoCambio.IdTipoCambioDestino = parseInt(1);
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambio;
        ObtenerTipoCambio(JSON.stringify(oRequest))
    });

    $('#dialogAgregarNotaCreditoProveedor, #dialogEditarNotaCreditoProveedor').on('focusout', '#txtMonto', function(event) {
        RecalculaDatosMonto();
    });

    $('#dialogAgregarNotaCreditoProveedor, #dialogEditarNotaCreditoProveedor').on('focusout', '#txtTotal', function(event) {
        RecalculaDatosTotal();
    });

    $('#dialogAgregarNotaCreditoProveedorDevolucionCancelacion, #dialogEditarNotaCreditoProveedorDevolucionCancelacion').on('focusout', '#txtMonto', function(event) {
        RecalculaDatosMonto();
    });

    $('#dialogAgregarNotaCreditoProveedorDevolucionCancelacion, #dialogEditarNotaCreditoProveedorDevolucionCancelacion ').on('focusout', '#txtTotal', function(event) {
        RecalculaDatosTotal();
    });

    $("#grdMovimientosCobros").on("click", ".imgEliminarMovimiento", function() {

        var registro = $(this).parents("tr");
        var pNotaCreditoProveedorEncabezadoFacturaProveedor = new Object();
        pNotaCreditoProveedorEncabezadoFacturaProveedor.pIdNotaCreditoProveedorEncabezadoFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdNotaCreditoProveedorEncabezadoFacturaProveedor']").html());
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedorEncabezadoFacturaProveedor = pNotaCreditoProveedorEncabezadoFacturaProveedor;
        SetEliminarNotaCreditoProveedorEncabezadoFacturaProveedor(JSON.stringify(oRequest));
    });

    $("#dialogEditarNotaCreditoProveedor, #dialogAgregarNotaCreditoProveedor").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var NotaCreditoProveedor = new Object();
        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedor").attr("IdTipoNotaCreditoProveedor"));
        if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedor").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedor").attr("IdNotaCreditoProveedor") != "") {
            NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedor").attr("IdNotaCreditoProveedor"));
            NotaCreditoProveedor.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedor").attr("IdProveedor"));
            NotaCreditoProveedor.IdTipoNotaCreditoProveedor = IdTipoNotaCreditoProveedor;
            var validacion = ValidaAsociacionDocumentos(NotaCreditoProveedor);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), IdTipoNotaCreditoProveedor);
        }
        else {
            AgregarNotaCreditoProveedorEdicion(IdTipoNotaCreditoProveedor);
        }
    });

    ////////    //NotaCreditoProveedorDevolucionCancelacion
    ////////    $("#dialogEditarNotaCreditoProveedor").on("click", "#btnObtenerFormaAsociarDocumentosProducto", function() {
    ////////        var NotaCreditoProveedor = new Object();
    ////////        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdTipoNotaCreditoProveedor"));
    ////////        var Timbrada = $("#divFormaEditarNotaCreditoProveedor").attr("timbrada");
    ////////        var Cancelada = $("#divFormaEditarNotaCreditoProveedor").attr("baja");
    ////////        if (Cancelada == 1) {
    ////////            MostrarMensajeError("No puede asociar documentos por que la nota de credito esta daba de baja");
    ////////            return false;
    ////////        }
    ////////        if (Timbrada != 0) {
    ////////            if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
    ////////                NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
    ////////                NotaCreditoProveedor.pIdCliente = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idCliente"));
    ////////                NotaCreditoProveedor.IdTipoNotaCreditoProveedor = IdTipoNotaCreditoProveedor;
    ////////                var validacion = ValidaAsociacionDocumentos(NotaCreditoProveedor);
    ////////                if (validacion != "")
    ////////                { MostrarMensajeError(validacion); return false; }
    ////////                var oRequest = new Object();
    ////////                oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
    ////////                ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), IdTipoNotaCreditoProveedor);

    ////////            }
    ////////            else {
    ////////                AgregarNotaCreditoProveedorEdicion(IdTipoNotaCreditoProveedor);
    ////////            }
    ////////        }
    ////////        else {
    ////////            MostrarMensajeError("Debe timbrar la nota de credito para poder asociar documentos");
    ////////            return false;
    ////////        }
    ////////    });

    $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var NotaCreditoProveedor = new Object();
        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdTipoNotaCreditoProveedor"));
        if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
            NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
            NotaCreditoProveedor.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idProveedor"));
            NotaCreditoProveedor.IdTipoNotaCreditoProveedor = IdTipoNotaCreditoProveedor;
            var validacion = ValidaAsociacionDocumentos(NotaCreditoProveedor);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), IdTipoNotaCreditoProveedor);

        }
        else {
            AgregarNotaCreditoProveedorEdicion(IdTipoNotaCreditoProveedor);
        }
    });

    $("#dialogEditarNotaCreditoProveedor").on("click", "#btnObtenerFormaAsociarProductos", function() {
        var NotaCreditoProveedor = new Object();
        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("IdTipoNotaCreditoProveedor"));
        if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
            // var Timbrada = $("#divFormaEditarNotaCreditoProveedor").attr("timbrada");
            var Cancelada = $("#divFormaEditarNotaCreditoProveedor").attr("baja");
            if (Cancelada == 1) {
                MostrarMensajeError("No puede asociar productos por que la nota de credito esta cancelada");
                return false;
            }
            else {
                NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
                NotaCreditoProveedor.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor"));
                NotaCreditoProveedor.pIdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdTipoNotaCreditoProveedor"));
                var oRequest = new Object();
                oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
                $("#cmbTipoMoneda").attr("disabled", "true");
                FiltroProductosNotaCreditoProveedorDevolucionCancelacion();
                $("#txtFacturasSeleccionadas").val("");
                $("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
            }

        }
        else {
            AgregarNotaCreditoProveedorEdicionDevolucionCancelacion(IdTipoNotaCreditoProveedor);
        }

    });

    $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").on("click", "#btnObtenerFormaAsociarProductos", function() {
        var NotaCreditoProveedor = new Object();
        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("IdTipoNotaCreditoProveedor"));
        if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
            NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
            NotaCreditoProveedor.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor"));
            NotaCreditoProveedor.pIdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdTipoNotaCreditoProveedor"));
            var oRequest = new Object();
            oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
            $("#cmbTipoMoneda").attr("disabled", "true");
            FiltroProductosNotaCreditoProveedorDevolucionCancelacion();
            $("#txtFacturasSeleccionadas").val("");
            $("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
        }
        else {
            AgregarNotaCreditoProveedorEdicionDevolucionCancelacion(IdTipoNotaCreditoProveedor);
        }

    });

    $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").on("click", "#btnObtenerProductosAsociados", function() {
        var NotaCreditoProveedor = new Object();
        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("IdTipoNotaCreditoProveedor"));

        if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
            NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
            var oRequest = new Object();
            oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
            FiltroProductosAsociadosNotaCreditoProveedorDevolucionCancelacion(JSON.stringify(oRequest));
            Productos.idsFacturasDetalle.length = 0;
            $("#dialogMuestraProductosAsociados").dialog("open");
        }
        else {
            //Valida Nota
            var validacion = ValidaSiExisteNotaCreditoProveedor();
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }

        }

    });

    $("#dialogEditarNotaCreditoProveedor").on("click", "#btnObtenerProductosAsociados", function() {
        var NotaCreditoProveedor = new Object();
        var IdTipoNotaCreditoProveedor = parseInt($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("IdTipoNotaCreditoProveedor"));
        var Cancelada = $("#divFormaEditarNotaCreditoProveedor").attr("baja");
        if (Cancelada == 1) {
            MostrarMensajeError("No puede asociar documentos por que la nota de crédito esta cancelada");
            return false;
        }
        else {
            if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
                NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
                var oRequest = new Object();
                oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
                FiltroProductosAsociadosNotaCreditoProveedorDevolucionCancelacion(JSON.stringify(oRequest));
                $("#dialogMuestraProductosAsociados").dialog("open");
            }
            else {
                //Valida Nota
                var validacion = ValidaSiExisteNotaCreditoProveedor();
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }

            }
            
        }
        

    });

    $('#dialogAgregarNotaCreditoProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarNotaCreditoProveedor").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarNotaCreditoProveedor();

            },
            "Salir": function() {
                $(this).dialog("close");

            }
        }
    });

    $('#dialogAgregarNotaCreditoProveedorDevolucionCancelacion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").remove();

        },
        buttons: {
            "Guardar": function() {
                if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != null && $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") != "") {
                    if ($("#txtTotal").val() == "0") {
                        MostrarMensajeError("Favor de asociar productos y guardar la nota de crédito");
                        return;
                    }
                    else {
                        AgregarNotaCreditoProveedorDevolucionCancelacion();
                    }
                }
                else {
                    MostrarMensajeError("Favor de crear una nota de crédito de proveedor");
                }

            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarNotaCreditoProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarNotaCreditoProveedor").remove();
        },
        buttons: {
            "Timbrar": function() {
                var NotaCreditoProveedor = new Object();
                NotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaConsultarNotaCreditoProveedor").attr("IdNotaCreditoProveedor");

                if (NotaCreditoProveedor.IdNotaCreditoProveedor != "0" && NotaCreditoProveedor.IdNotaCreditoProveedor != "" && NotaCreditoProveedor.IdNotaCreditoProveedor != null) {
                    ObtenerFormaDatosFiscales(JSON.stringify(NotaCreditoProveedor));
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

    $('#dialogEditarNotaCreditoProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarNotaCreditoProveedor").remove();
        },
        buttons: {
            "Timbrar": function() {
                var NotaCreditoProveedor = new Object();
                NotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaEditarNotaCreditoProveedor").attr("IdNotaCreditoProveedor");

                if (NotaCreditoProveedor.IdNotaCreditoProveedor != "0" && NotaCreditoProveedor.IdNotaCreditoProveedor != "" && NotaCreditoProveedor.IdNotaCreditoProveedor != null) {
                    ObtenerFormaDatosFiscales(JSON.stringify(NotaCreditoProveedor));
                }
                else {
                    MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                }
            },
            "Editar": function() {
                EditarNotaCreditoProveedor();
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
                pRequest.pIdNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idNotaCreditoProveedor");

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
                TimbrarNotaCreditoProveedor();
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
                CancelarNotaCreditoProveedor();
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogSeleccionarTipoNotaCreditoProveedor').dialog({
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
                request.pIdTipoNotaCreditoProveedor = $("#cmbTipoNotaCredito").val();
                if (request.pIdTipoNotaCreditoProveedor != 0) {
                    ObtenerFormaAgregarNotaCreditoProveedor(request);
                    $("#dialogSeleccionarTipoNotaCreditoProveedor").dialog("close");
                }
                else {
                    MostrarMensajeError("Debe de seleccionar un tipo de nota");
                }
            },
            "Salir": function() {
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
                var idFactura = $("#dialogAgregarProductosCantidades").attr("idDetalleFacturaProveedor");
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
function ObtenerFormaAsociarDocumentos(NotaCreditoProveedor) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentosNotaCreditoProveedor.html",
        url: "NotaCreditoProveedor.aspx/ObtenerFormaAsociarDocumentos",
        parametros: NotaCreditoProveedor,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.IdTipoNotaCreditoProveedor == 1) {

                FiltroFacturas();
                FiltroMovimientosCobros();
                $("#dialogMuestraAsociarDocumentos").dialog("open");

            }
            else {
                FiltroFacturas();
                FiltroMovimientosCobros();
                $("#dialogMuestraAsociarDocumentos").dialog("open");
            }
        }
    });
}
function ObtenerFormaAgregarNotaCreditoProveedor(pRequest) {
    if (pRequest.pIdTipoNotaCreditoProveedor == 1) {
        $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").obtenerVista({
            url: "NotaCreditoProveedor.aspx/ObtenerFormaAgregarNotaCreditoProveedorDevolucionCancelacion",
            parametros: JSON.stringify(pRequest),
            nombreTemplate: "tmplAgregarNotaCreditoProveedorDevolucionCancelacion.html",
            despuesDeCompilar: function(pRespuesta) {
                $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").dialog("open");
                $("#txtFecha").datepicker();
                AutocompletarProveedor(pRequest.pIdTipoNotaCreditoProveedor);
            }
        });
    }
    else {
        $("#dialogAgregarNotaCreditoProveedor").obtenerVista({
            url: "NotaCreditoProveedor.aspx/ObtenerFormaAgregarNotaCreditoProveedor",
            parametros: JSON.stringify(pRequest),
            nombreTemplate: "tmplAgregarNotaCreditoProveedor.html",
            despuesDeCompilar: function(pRespuesta) {
                $("#dialogAgregarNotaCreditoProveedor").dialog("open");
                $("#txtFecha").datepicker();
                AutocompletarProveedor(pRequest.pIdTipoNotaCreditoProveedor);
            }
        });
        
    }
}

function ObtenerFormaFiltrosNotaCreditoProveedor() {
    $("#divFiltrosNotaCreditoProveedor").obtenerVista({
        nombreTemplate: "tmplFiltrosNotaCreditoProveedor.html",
        url: "NotaCreditoProveedor.aspx/ObtenerFormaFiltroNotaCreditoProveedor",
        despuesDeCompilar: function(pRespuesta) {
        
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroNotaCreditoProveedor();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                    FiltroNotaCreditoProveedor();
                    }
                });
            }

            $('#divFiltrosNotaCreditoProveedor').on('click', '#chkPorFecha', function(event) {
                FiltroNotaCreditoProveedor();
            });

            $('#divFiltrosNotaCreditoProveedor').on('change', '#cmbFiltroTimbrado', function(event) {
                FiltroNotaCreditoProveedor();
            });

        }
    });
}

function EliminarProductosAsociados() {
    var pRequest = new Object();
    pRequest.pIdDevoluciones = $("#txtProductosSeleccionados").val();
    pRequest.pIdNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idNotaCreditoProveedor");
    $("#txtProductosSeleccionados").val("");
    SetEliminarProductosAsociados(JSON.stringify(pRequest));
}
function SetEliminarProductosAsociados(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/EliminarProductosAsociados",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                $("#txtMonto").val(respuesta.TotalMonto);
                $("#txtIVA").val(respuesta.IVA);
                $("#txtTotal").val(respuesta.Total);
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
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

function ObtenerFormaComboTipoNotaCreditoProveedor() {
    $("#dialogSeleccionarTipoNotaCreditoProveedor").obtenerVista({
        nombreTemplate: "tmplFiltroComboTipoNotaCreditoProveedor.html",
        url: "NotaCreditoProveedor.aspx/LlenaComboTipoNotaCreditoProveedor",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogSeleccionarTipoNotaCreditoProveedor").dialog("open");
        }
    });
}



function ObtenerFormaDatosFiscales(pRequest) {
    $("#dialogDatosFiscales").obtenerVista({
        nombreTemplate: "tmplDatosFiscales.html",
        parametros: pRequest,
        url: "NotaCreditoProveedor.aspx/LlenaDatosFiscales",
        despuesDeCompilar: function() {
            $("#dialogDatosFiscales").dialog("open");
        }
    });
}

function ObtenerFormaMotivoCancelacion(pRequest) {
    $("#dialogMotivoCancelacion").obtenerVista({
        nombreTemplate: "tmplMotivoCancelacionProveedor.html",
        parametros: pRequest,
        url: "NotaCreditoProveedor.aspx/LlenaMotivoCancelacion",
        despuesDeCompilar: function() {
            $("#dialogMotivoCancelacion").dialog("open");
        }
    });
}

function ObtenerFormaConsultarNotaCreditoProveedorFormato(pRequest) {
    $("#dialogFacturaFormato").obtenerVista({
        nombreTemplate: "tmplFacturaFormato.html",
        parametros: pRequest,
        url: "NotaCreditoProveedor.aspx/ObtieneFacturaFormato",
        despuesDeCompilar: function(pRespuesta) {
            jQuery("#dialogFacturaFormato").empty();
            jQuery("#dialogFacturaFormato").append('<iframe src="' + pRespuesta.modelo.Ruta + '" style="width:750px; height:550px;"></iframe>');
            $("#dialogFacturaFormato").dialog("open");
        }
    });
}

function ObtenerFormaConsultarNotaCreditoProveedor(pIdNotaCreditoProveedor) {
    $("#dialogConsultarNotaCreditoProveedor").obtenerVista({
        nombreTemplate: "tmplConsultarNotaCreditoProveedor.html",
        url: "NotaCreditoProveedor.aspx/ObtenerFormaNotaCreditoProveedor",
        parametros: pIdNotaCreditoProveedor,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarNotaCreditoProveedor == 1) {              
                $("#dialogConsultarNotaCreditoProveedor").dialog("option", "buttons", {                        
                    "Editar": function() {
                        $(this).dialog("close");
                        var NotaCreditoProveedor = new Object();
                        NotaCreditoProveedor.IdNotaCreditoProveedor = parseInt($("#divFormaConsultarNotaCreditoProveedor").attr("IdNotaCreditoProveedor"));
                        ObtenerFormaEditarNotaCreditoProveedor(JSON.stringify(NotaCreditoProveedor))
                    }
                });
                
                $("#dialogConsultarNotaCreditoProveedor").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarNotaCreditoProveedor").dialog("option", "buttons", {});
                $("#dialogConsultarNotaCreditoProveedor").dialog("option", "height", "100");
            }
            $("#dialogConsultarNotaCreditoProveedor").dialog("open");
        }
    });
}

function ObtenerFormaEditarNotaCreditoProveedor(IdNotaCreditoProveedor) {
    $("#dialogEditarNotaCreditoProveedor").obtenerVista({
        nombreTemplate: "tmplEditarNotaCreditoProveedor.html",
        url: "NotaCreditoProveedor.aspx/ObtenerFormaEditarNotaCreditoProveedor",
        parametros: IdNotaCreditoProveedor,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosEditar();
            $("#dialogEditarNotaCreditoProveedor").dialog("open");
            $("#txtFecha").datepicker();
            AutocompletarProveedor(2);          
            if (pRespuesta.modelo.Permisos.puedeEditarNotaCreditoProveedor == 1) {
                $("#dialogEditarNotaCreditoProveedor").dialog("option", "buttons", {                    
                    "Editar": function() {
                        EditarNotaCreditoProveedor();
                    },
                    "Salir": function() {
                        $(this).dialog("close")
                    }
                });
                $("#dialogEditarNotaCreditoProveedor").dialog("option", "height", "auto");
            }
            else {
                $("#dialogEditarNotaCreditoProveedor").dialog("option", "buttons", {});
                $("#dialogEditarNotaCreditoProveedor").dialog("option", "height", "100");
            }

        }
    });
}

function ObtenerNumeroNotaCreditoProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/ObtenerNumeroNotaCreditoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {                
                $("#txtFolioNotaCreditoProveedor").val(respuesta.Modelo.NumeroNotaCreditoProveedor);               
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
        url: "NotaCreditoProveedor.aspx/ObtenerTipoCambio",
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
function AgregarNotaCreditoProveedor() {
    var pNotaCreditoProveedor = new Object();
    if ($("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
        pNotaCreditoProveedor.IdProveedor = 0;
    }
    else {
        pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
    }
    
    if ($("#divFormaAgregarNotaCreditoProveedor").attr("IdNotaCreditoProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedor").attr("IdNotaCreditoProveedor") == null) {

        pNotaCreditoProveedor.SerieNotaCredito = $("#txtSerieNotaCredito").val();
        pNotaCreditoProveedor.FolioNotaCredito = $("#txtFolioNotaCredito").val();
        pNotaCreditoProveedor.Descripcion = $("#txtDescripcion").val();
        pNotaCreditoProveedor.Fecha = $("#txtFecha").val();
        pNotaCreditoProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pNotaCreditoProveedor.TipoCambio = $("#txtTipoCambio").val();
        pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
        pNotaCreditoProveedor.Monto = $("#txtMonto").val();
        pNotaCreditoProveedor.PorcentajeIVA = $("#txtPorcentajeIVA").val();
        pNotaCreditoProveedor.IVA = $("#txtIVA").val();
        pNotaCreditoProveedor.Total = $("#txtTotal").val();
        pNotaCreditoProveedor.IdTipoNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idTipoNotaCreditoProveedor");
        var validacion = ValidaNotaCreditoProveedor(pNotaCreditoProveedor);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
        SetAgregarNotaCreditoProveedor(JSON.stringify(oRequest), pNotaCreditoProveedor.IdTipoNotaCreditoProveedor);
    }
    else {
        $("#dialogAgregarNotaCreditoProveedor").dialog("close");
    }
}

function AgregarNotaCreditoProveedorDevolucionCancelacion() {
    var pNotaCreditoProveedor = new Object();
    if ($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
        pNotaCreditoProveedor.IdProveedor = 0;
    }
    else {
        pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
    }
    if ($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") == null) {
        //
        pNotaCreditoProveedor.SerieNotaCredito = $("#txtSerieNotaCredito").val();
        pNotaCreditoProveedor.FolioNotaCredito = $("#txtFolioNotaCredito").val();
        pNotaCreditoProveedor.Descripcion = $("#txtDescripcion").val();
        pNotaCreditoProveedor.Fecha = $("#txtFecha").val();
        pNotaCreditoProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pNotaCreditoProveedor.TipoCambio = $("#txtTipoCambio").val();
        pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
        pNotaCreditoProveedor.Monto = $("#txtMonto").val();
        pNotaCreditoProveedor.PorcentajeIVA = $("#txtPorcentajeIVA").val();
        pNotaCreditoProveedor.IVA = $("#txtIVA").val();
        pNotaCreditoProveedor.Total = $("#txtTotal").val();
        pNotaCreditoProveedor.IdTipoNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor");
        //Valida Nota
        var validacion = ValidaNotaCreditoProveedorDevolucionCancelacion(pNotaCreditoProveedor);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
        SetAgregarNotaCreditoProveedor(JSON.stringify(oRequest), pNotaCreditoProveedor.IdTipoNotaCreditoProveedor);
    }
    else {
        //
        var pNotaCreditoProveedor = new Object();
        pNotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idNotaCreditoProveedor");
        if ($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
            pNotaCreditoProveedor.IdProveedor = 0;
        }
        else {
            pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
        }
        pNotaCreditoProveedor.SerieNotaCredito = $("#txtSerieNotaCredito").val();
        pNotaCreditoProveedor.FolioNotaCredito = $("#txtFolioNotaCredito").val();
        pNotaCreditoProveedor.Descripcion = $("#txtDescripcion").val();
        pNotaCreditoProveedor.Fecha = $("#txtFecha").val();
        pNotaCreditoProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
        pNotaCreditoProveedor.TipoCambio = $("#txtTipoCambio").val();
        pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
        pNotaCreditoProveedor.Monto = $("#txtMonto").val();
        pNotaCreditoProveedor.PorcentajeIVA = $("#txtPorcentajeIVA").val();
        pNotaCreditoProveedor.IVA = $("#txtIVA").val();
        pNotaCreditoProveedor.Total = $("#txtTotal").val();
        pNotaCreditoProveedor.IdTipoNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor");
        //Valida Nota
        var validacion = ValidaNotaCreditoProveedorDevolucionCancelacion(pNotaCreditoProveedor);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
        SetEditarNotaCreditoProveedorDevolucionCancelacion(JSON.stringify(oRequest), pNotaCreditoProveedor.IdTipoNotaCreditoProveedor);
    }
}

function AgregarNotaCreditoProveedorEdicion(IdTipoNotaCreditoProveedor) {
    var pNotaCreditoProveedor = new Object();
    if (IdTipoNotaCreditoProveedor == 1) {
        if ($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
            pNotaCreditoProveedor.IdProveedor = 0;
        }
        else {
            pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
        }
        
    }
    else {
        if ($("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
            pNotaCreditoProveedor.IdProveedor = 0;
        }
        else {
            pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
        }
    }
    pNotaCreditoProveedor.SerieNotaCredito = $("#txtSerieNotaCredito").val();
    pNotaCreditoProveedor.FolioNotaCredito = $("#txtFolioNotaCredito").val();
    pNotaCreditoProveedor.Descripcion = $("#txtDescripcion").val();
    pNotaCreditoProveedor.Fecha = $("#txtFecha").val();
    pNotaCreditoProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pNotaCreditoProveedor.TipoCambio = $("#txtTipoCambio").val();
    pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
    pNotaCreditoProveedor.Monto = $("#txtMonto").val();
    pNotaCreditoProveedor.PorcentajeIVA = $("#txtPorcentajeIVA").val();
    pNotaCreditoProveedor.IVA = $("#txtIVA").val();
    pNotaCreditoProveedor.Total = $("#txtTotal").val();
    pNotaCreditoProveedor.IdTipoNotaCreditoProveedor = IdTipoNotaCreditoProveedor;
    var validacion = "";
    if (IdTipoNotaCreditoProveedor == 1) {
        validacion = ValidaNotaCreditoProveedorEdicionDevolucionCancelacion(pNotaCreditoProveedor);
        
    }
    else {
        validacion = ValidaNotaCreditoProveedorEdicion(pNotaCreditoProveedor);
    }
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
    SetAgregarNotaCreditoProveedorEdicion(JSON.stringify(oRequest), IdTipoNotaCreditoProveedor);
}

function AgregarNotaCreditoProveedorEdicionDevolucionCancelacion(IdTipoNotaCreditoProveedor) {
    var pNotaCreditoProveedor = new Object();
    if ($("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
        pNotaCreditoProveedor.IdProveedor = 0;
    }
    else {
        pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
    }

    pNotaCreditoProveedor.SerieNotaCredito = $("#txtSerieNotaCredito").val();
    pNotaCreditoProveedor.FolioNotaCredito = $("#txtFolioNotaCredito").val();
    pNotaCreditoProveedor.Descripcion = $("#txtDescripcion").val();
    pNotaCreditoProveedor.Fecha = $("#txtFecha").val();
    pNotaCreditoProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pNotaCreditoProveedor.TipoCambio = $("#txtTipoCambio").val();
    pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
    pNotaCreditoProveedor.Monto = $("#txtMonto").val();
    pNotaCreditoProveedor.PorcentajeIVA = $("#txtPorcentajeIVA").val();
    pNotaCreditoProveedor.IVA = $("#txtIVA").val();
    pNotaCreditoProveedor.Total = $("#txtTotal").val();
    pNotaCreditoProveedor.IdTipoNotaCreditoProveedor = IdTipoNotaCreditoProveedor;
    var validacion = "";
    validacion = ValidaNotaCreditoProveedorEdicionDevolucionCancelacion(pNotaCreditoProveedor);
     if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
    SetAgregarNotaCreditoProveedorEdicionDevolucionCancelacion(JSON.stringify(oRequest), IdTipoNotaCreditoProveedor);
}

function TimbrarNotaCreditoProveedor() {
    var pNotaCreditoProveedor = new Object();
    if ($("#divFormaAgregarNotaCreditoProveedor,#divFormaConsultarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idNotaCreditoProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedor,#divFormaConsultarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idNotaCreditoProveedor") == null) {
        MostrarMensajeError("No hay nota de crédito para timbrar"); return false;
    }
    else {
        pNotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedor,#divFormaConsultarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idNotaCreditoProveedor");
        pNotaCreditoProveedor.LugarExpedicion = $("#txtLugarExpedicion").val();
        pNotaCreditoProveedor.IdMetodoPago = $("#cmbMetodoPago").val();
        pNotaCreditoProveedor.IdFormaPago = $("#cmbFormaPago").val();
        pNotaCreditoProveedor.IdCondicionPago = $("#cmbCondicionPago").val();
        pNotaCreditoProveedor.IdCuentaBancariaCliente = $("#cmbCuentaBancariaCliente").val();
        pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
        pNotaCreditoProveedor.Observaciones = $("#txtObservaciones").val();

        var validacion = ValidaDatosFiscales(pNotaCreditoProveedor);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }

        pNotaCreditoProveedor.MetodoPago = $("#cmbMetodoPago option:selected").html();

        if (pNotaCreditoProveedor.IdFormaPago == 0) {
            pNotaCreditoProveedor.FormaPago = "";
        }
        else {
            pNotaCreditoProveedor.FormaPago = $("#cmbFormaPago option:selected").html();
        }

        if (pNotaCreditoProveedor.IdCondicionPago == 0) {
            pNotaCreditoProveedor.CondicionPago = "";
        }
        else {
            pNotaCreditoProveedor.CondicionPago = $("#cmbCondicionPago option:selected").html();
        }

        if (pNotaCreditoProveedor.IdCuentaBancariaCliente == 0) {
            pNotaCreditoProveedor.CuentaBancariaCliente = "No identificado";
        }
        else {
            pNotaCreditoProveedor.CuentaBancariaCliente = $("#cmbCuentaBancariaCliente option:selected").html();
        }
        
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
        SetTimbrarNotaCreditoProveedor(JSON.stringify(oRequest));
    }
}

function CancelarNotaCreditoProveedor() {
    var pNotaCreditoProveedor = new Object();
    if ($("#divFormaAgregarMotivoCancelacion").attr("idNotaCreditoProveedor") == "" || $("#divFormaAgregarMotivoCancelacion").attr("idNotaCreditoProveedor") == null) {
        MostrarMensajeError("No hay nota de crédito para cancelar"); return false;
    }
    else {
        pNotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaAgregarMotivoCancelacion").attr("idNotaCreditoProveedor");
        pNotaCreditoProveedor.MotivoCancelacion = $("#txtMotivoCancelacion").val();
        var validacion = ValidaMotivoCancelacion(pNotaCreditoProveedor);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        var oRequest = new Object();
        oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
        SetCancelarNotaCreditoProveedor(JSON.stringify(oRequest));
    }
}

function SetAgregarNotaCreditoProveedor(pRequest, pIdTipoNotaCreditoProveedor) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/AgregarNotaCreditoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                if (pIdTipoNotaCreditoProveedor == 2) {
                    $("#dialogAgregarNotaCreditoProveedor").dialog("close");
                }
                if (pIdTipoNotaCreditoProveedor == 1) {
                    $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").dialog("close");
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
function SetEditarNotaCreditoProveedorDevolucionCancelacion(pRequest, pIdTipoNotaCreditoProveedor) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/EditarNotaCreditoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
//            $("#dialogAgregarNotaCreditoProveedorDevolucionCancelacion").dialog("close");
            OcultarBloqueo();
        }
    });
}


function SetTimbrarNotaCreditoProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/TimbrarNotaCreditoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                $("#dialogAgregarNotaCreditoProveedor, #dialogConsultarNotaCreditoProveedor, #dialogEditarNotaCreditoProveedor, #dialogDatosFiscales").dialog("close");                
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

function SetCancelarNotaCreditoProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/CancelarNotaCreditoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
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

function SetAgregarNotaCreditoProveedorEdicion(pRequest, pIdTipoNotaCreditoProveedor) {
    if (pIdTipoNotaCreditoProveedor == 2) {
        MostrarBloqueo();
        $.ajax({
            type: "POST",
            url: "NotaCreditoProveedor.aspx/AgregarNotaCreditoProveedorEdicion",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#divFormaAgregarNotaCreditoProveedor").attr("idNotaCreditoProveedor", respuesta.IdNotaCreditoProveedor);
                    $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                    var NotaCreditoProveedor = new Object();
                    NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedor").attr("IdNotaCreditoProveedor"));
                    NotaCreditoProveedor.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedor").attr("IdProveedor"));
                    NotaCreditoProveedor.IdTipoNotaCreditoProveedor = pIdTipoNotaCreditoProveedor;
                    var validacion = ValidaAsociacionDocumentos(NotaCreditoProveedor);
                    if (validacion != "")
                    { MostrarMensajeError(validacion); return false; }
                    var oRequest = new Object();
                    oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
                    ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), pIdTipoNotaCreditoProveedor);
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
            url: "NotaCreditoProveedor.aspx/AgregarNotaCreditoProveedorEdicion",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idNotaCreditoProveedor", respuesta.IdNotaCreditoProveedor);
                    $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                    var NotaCreditoProveedor = new Object();
                    NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
                    NotaCreditoProveedor.pIdCliente = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdCliente"));
                    NotaCreditoProveedor.IdTipoNotaCreditoProveedor = pIdTipoNotaCreditoProveedor;
                    var validacion = ValidaAsociacionDocumentos(NotaCreditoProveedor);
                    if (validacion != "")
                    { MostrarMensajeError(validacion); return false; }
                    var oRequest = new Object();
                    oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
                    if (respuesta.TotalNotaCreditoProveedorTimbrada > 0) {
                       //  FiltroProductosNotaCreditoProveedorDevolucionCancelacion();
                        //$("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
                        ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest), pIdTipoNotaCreditoProveedor);
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
function SetAgregarNotaCreditoProveedorEdicionDevolucionCancelacion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/AgregarNotaCreditoProveedorEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idNotaCreditoProveedor", respuesta.IdNotaCreditoProveedor);
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                var NotaCreditoProveedor = new Object();
                NotaCreditoProveedor.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
                NotaCreditoProveedor.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor"));
                var validacion = ValidaAsociacionDocumentos(NotaCreditoProveedor);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.NotaCreditoProveedor = NotaCreditoProveedor;
                $("#cmbTipoMoneda").attr("disabled", "true");
                FiltroProductosNotaCreditoProveedorDevolucionCancelacion();
                $("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");              
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



function SetCambiarEstatus(pIdNotaCreditoProveedor, pBaja) {
    var pRequest = "{'pIdNotaCreditoProveedor':" + pIdNotaCreditoProveedor + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdNotaCreditoProveedor").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdNotaCreditoProveedor').one('click', '.div_grdNotaCreditoProveedor_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdNotaCreditoProveedor_AI']").children().attr("baja")
                var idNotaCreditoProveedor = $(registro).children("td[aria-describedby='grdNotaCreditoProveedor_IdNotaCreditoProveedor']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idNotaCreditoProveedor, baja);
            });
        }
    });
}

function EditarNotaCreditoProveedor() {
    var pNotaCreditoProveedor = new Object();
    pNotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaEditarNotaCreditoProveedor").attr("idNotaCreditoProveedor");
    if ($("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == "" || $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor") == null) {
        pNotaCreditoProveedor.IdProveedor = 0;
    }
    else {
        pNotaCreditoProveedor.IdProveedor = $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor");
    }
    pNotaCreditoProveedor.SerieNotaCredito = $("#txtSerieNotaCredito").val();
    pNotaCreditoProveedor.FolioNotaCredito = $("#txtFolioNotaCredito").val();
    pNotaCreditoProveedor.Descripcion = $("#txtDescripcion").val();
    pNotaCreditoProveedor.Fecha = $("#txtFecha").val();
    pNotaCreditoProveedor.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pNotaCreditoProveedor.TipoCambio = $("#txtTipoCambio").val();
    pNotaCreditoProveedor.Referencia = $("#txtReferencia").val();
    pNotaCreditoProveedor.Monto = $("#txtMonto").val();
    pNotaCreditoProveedor.PorcentajeIVA = $("#txtPorcentajeIVA").val();
    pNotaCreditoProveedor.IVA = $("#txtIVA").val();
    pNotaCreditoProveedor.Total = $("#txtTotal").val();
    pNotaCreditoProveedor.IdTipoNotaCreditoProveedor = $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idTipoNotaCreditoProveedor");
    var validacion = ValidaNotaCreditoProveedor(pNotaCreditoProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCreditoProveedor = pNotaCreditoProveedor;
    SetEditarNotaCreditoProveedor(JSON.stringify(oRequest));
}
function SetEditarNotaCreditoProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/EditarNotaCreditoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarNotaCreditoProveedor").dialog("close");
        }
    });
}

function EdicionFacturas(valor, id, rowid, iCol) {
    var TipoMoneda = $('#grdFacturas').getCell(rowid, 'TipoMoneda');
    var Saldo = $('#grdFacturas').getCell(rowid, 'Saldo');
    var NotaCreditoProveedor = new Object();    

    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        NotaCreditoProveedor.IdTipoMoneda = 1;
        NotaCreditoProveedor.TipoCambio = 1;
        NotaCreditoProveedor.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        NotaCreditoProveedor.IdTipoMoneda = 2;
        NotaCreditoProveedor.TipoCambio = $("#spanTipoCambioDolares").text();
        NotaCreditoProveedor.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    NotaCreditoProveedor.TipoMoneda = TipoMoneda;

    NotaCreditoProveedor.Monto = QuitarFormatoNumero(valor);
    NotaCreditoProveedor.Saldo = QuitarFormatoNumero(Saldo);
    NotaCreditoProveedor.IdEncabezadoFacturaProveedor = id;
    NotaCreditoProveedor.rowid = rowid;
    NotaCreditoProveedor.IdNotaCreditoProveedor = $("#divFormaAsociarDocumentos").attr("idNotaCreditoProveedor");
    var validacion = ValidarMontos(NotaCreditoProveedor);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pNotaCreditoProveedor = NotaCreditoProveedor;
    SetEditarMontos(JSON.stringify(oRequest));

}

function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {

                if (respuesta.EsParcialidad == 1) {
                    MostrarMensajeError(respuesta.Descripcion);
                }
                
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosNotaCreditoProveedor;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosNotaCreditoProveedor / $("#spanTipoCambioDolares").text());
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

function SetEliminarNotaCreditoProveedorEncabezadoFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/EliminarNotaCreditoProveedorEncabezadoFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {               
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosNotaCreditoProveedor;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosNotaCreditoProveedor / $("#spanTipoCambioDolares").text());
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

function AutocompletarProveedor(pIdTipoNotaCreditoProveedor) {

    if (pIdTipoNotaCreditoProveedor == 2) {
        $('#txtRazonSocial').autocomplete({
            source: function(request, response) {
                var pRequest = new Object();
                pRequest.pRazonSocial = $('#txtRazonSocial').val();
                $.ajax({
                    type: 'POST',
                    url: 'NotaCreditoProveedor.aspx/BuscarRazonSocialProveedor',
                    data: JSON.stringify(pRequest),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    success: function(pRespuesta) {
                        $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor", "0");
                        var json = jQuery.parseJSON(pRespuesta.d);
                        response($.map(json.Table, function(item) {
                            return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdProveedor }
                        }));
                    }
                });
            },
            minLength: 1,
            select: function(event, ui) {
                var pIdProveedor = ui.item.id;
                $("#divFormaAgregarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor").attr("idProveedor", pIdProveedor);
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
                    url: 'NotaCreditoProveedor.aspx/BuscarRazonSocialProveedor',
                    data: JSON.stringify(pRequest),
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8',
                    success: function(pRespuesta) {
                    $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor", "0");
                        var json = jQuery.parseJSON(pRespuesta.d);
                        response($.map(json.Table, function(item) {
                            return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdProveedor }
                        }));
                    }
                });
            },
            minLength: 1,
            select: function(event, ui) {
                var pIdProveedor = ui.item.id;
                $("#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion, #divFormaEditarNotaCreditoProveedor").attr("idProveedor", pIdProveedor);
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
    request.pNumeroFactura = "";
    request.pIdProveedor = 0;
    if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdProveedor") != null) {
        request.pIdProveedor = $("#divFormaEditarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdProveedor");
        if ($('#divContGridAsociarDocumento').find(gs_NumeroFactura).existe()) {
            request.pNumeroFactura = $('#divContGridAsociarDocumento').find(gs_NumeroFactura).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerFacturas',
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

function FiltroNotaCreditoProveedor() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdNotaCreditoProveedor').getGridParam('rowNum');
    request.pPaginaActual = $('#grdNotaCreditoProveedor').getGridParam('page');
    request.pColumnaOrden = $('#grdNotaCreditoProveedor').getGridParam('sortname');
    request.pTipoOrden = $('#grdNotaCreditoProveedor').getGridParam('sortorder');
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pSerieNotaCredito = "";
    request.pFolioNotaCredito = "";
    request.pPorFecha = 0;
    request.pAI = -1
    request.pRazonSocial = "";

    if ($('#gs_SerieNotaCredito').val() != null) {
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
        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerNotaCreditoProveedor',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdNotaCreditoProveedor')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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
    request.pIdNotaCreditoProveedor = 0;
    if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdNotaCreditoProveedor") != null) {
        request.pIdNotaCreditoProveedor = $("#divFormaEditarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdNotaCreditoProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerMovimientosCobros',
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
    request.pIdNotaCreditoProveedor = 0;
    if ($("#divFormaEditarNotaCreditoProveedor, #divFormaConsultarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdNotaCreditoProveedor") != null) {
        request.pIdNotaCreditoProveedor = $("#divFormaEditarNotaCreditoProveedor, #divFormaConsultarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdNotaCreditoProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerMovimientosCobrosConsultar',
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
    request.pIdNotaCreditoProveedor = 0;
    if ($("#divFormaEditarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdNotaCreditoProveedor") != null) {
        request.pIdNotaCreditoProveedor = $("#divFormaEditarNotaCreditoProveedor, #divFormaEditarNotaCreditoProveedor, #divFormaAsociarDocumentos").attr("IdNotaCreditoProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerMovimientosCobrosEditar',
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

function FiltroProductosNotaCreditoProveedorDevolucionCancelacion() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdProductosNotaCreditoProveedorDevolucionCancelacion').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProductosNotaCreditoProveedorDevolucionCancelacion').getGridParam('page');
    request.pColumnaOrden = $('#grdProductosNotaCreditoProveedorDevolucionCancelacion').getGridParam('sortname');
    request.pTipoOrden = $('#grdProductosNotaCreditoProveedorDevolucionCancelacion').getGridParam('sortorder');
    request.pIdTipoMoneda = 0;
    request.pTipoCambio = 0;
    request.pDescripcion = "";
    request.pNumeroSerie = "";
    
    if ($("#cmbTipoMoneda").val() != null) {
        request.pIdTipoMoneda = $("#cmbTipoMoneda").val();
    }
    if ($("#txtTipoCambio").val() != null) {
        request.pTipoCambio = $("#txtTipoCambio").val();
    }
    if ($('#gs_Descripcion').val() != null) {
        request.pDescripcion = $("#gs_Descripcion").val();
    }

    if ($('#gs_NumeroSerie').val() != null) {
        request.pNumeroSerie = $("#gs_NumeroSerie").val();
    }
    

    if ($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") == null || $("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") == "")
        request.pIdNotaCreditoProveedor = 0;
    else
        request.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
    if ($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor") == null || $("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor") == "")
        request.pIdTipoNotaCreditoProveedor = 0;
    else
        request.pIdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor"));

    if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor") == null || $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor") == "")
        request.pIdProveedor = 0;
    else
        request.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor"));

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerProductosNotaCreditoProveedorDevolucionCancelacion',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success') {
                $('#grdProductosNotaCreditoProveedorDevolucionCancelacion').val("");
                $('#grdProductosNotaCreditoProveedorDevolucionCancelacion')[0].addJSONData(JSON.parse(jsondata.responseText).d);
                //$("#dialogMuestraAsociarProductosDevolucionCancelacion").dialog("open");
            }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//NotaCreditoProveedorDevolucion
function ObtenerNumeroNotaCreditoProveedorDevolucion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/ObtenerNumeroNotaCreditoProveedorDevolucion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtFolioNotaCreditoProveedor").val(respuesta.Modelo.NumeroNotaCreditoProveedor);
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
        url: "NotaCreditoProveedor.aspx/ObtenerTipoCambioDevolucion",
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

function Termino_grdProductosNotaCreditoProveedorDevolucionCancelacion() {
    var ids = $('#grdProductosNotaCreditoProveedorDevolucionCancelacion').jqGrid('getDataIDs');
    var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]"); 

    for (var i = 0; i < ids.length; i++) {
        idFactura = $('#grdProductosNotaCreditoProveedorDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosNotaCreditoProveedorDevolucionCancelacion_IdDetalleFacturaProveedor"]').html();
        $.each(facturasSeleccionadas, function(pIndex, pIdFactura) {
            if (pIdFactura == idFactura) {
                $('#grdProductosNotaCreditoProveedorDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosNotaCreditoProveedorDevolucionCancelacion_Sel"] input').prop('checked', true);
            }
            return (pIdFactura !== idFactura);
        });
        $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
    }
}

function Termino_grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion() {
    var ids = $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion').jqGrid('getDataIDs');
    var productosSeleccionados = JSON.parse("[" + $("#txtProductosSeleccionados").val() + "]");

    for (var i = 0; i < ids.length; i++) {
        idDevolucion = $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion_IdDevolucion"]').html();
        $.each(productosSeleccionados, function(pIndex, pIdDevolucion) {
        if (pIdDevolucion == idDevolucion) {
                $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion #' + ids[i] + ' td[aria-describedby="grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion_Sel"] input').prop('checked', true);
            }
            return (pIdDevolucion !== idDevolucion);
        });
        $("#txtProductosSeleccionados").val(productosSeleccionados);
    }
}

//
function FiltroProductosAsociadosNotaCreditoProveedorDevolucionCancelacion() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion').getGridParam('page');
    request.pColumnaOrden = $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion').getGridParam('sortname');
    request.pTipoOrden = $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion').getGridParam('sortorder');
    request.pDescripcion = "";

    if ($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") == null || $("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor") == "")
        request.pIdNotaCreditoProveedor = 0;
    else
        request.pIdNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdNotaCreditoProveedor"));
    if ($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor") == null || $("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor") == "")
        request.pIdTipoNotaCreditoProveedor = 0;
    else
        request.pIdTipoNotaCreditoProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor,#divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("idTipoNotaCreditoProveedor"));

    if ($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor") == null || $("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor") == "")
        request.pIdProveedor = 0;
    else
        request.pIdProveedor = parseInt($("#divFormaEditarNotaCreditoProveedor, #divFormaAgregarNotaCreditoProveedorDevolucionCancelacion").attr("IdProveedor"));

    if ($('#gs_DescripcionProducto').val() != null) {
        request.pDescripcion = $("#gs_DescripcionProducto").val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'NotaCreditoProveedor.aspx/ObtenerProductosAsociadosNotaCreditoProveedorDevolucionCancelacion',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success') {
                $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion').val("");
                $('#grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion')[0].addJSONData(JSON.parse(jsondata.responseText).d);
                
            }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}
function AgregarProductosPorCantidad() {
    var Valores = new Object();
    Valores.Cantidad = $("#txtCantidadDevolver").val();
    var CantidadDisponible = $("#CantidadDisponible").text();
    if (parseInt(Valores.Cantidad) > parseInt(CantidadDisponible) || parseInt(Valores.Cantidad) <= 0) {
        MostrarMensajeError("No puede devolver cantidad mayor a la disponible");
    }
    else {
        Valores.idFacturaDetalle = $("#dialogAgregarProductosCantidades").attr("idDetalleFacturaProveedor");
        Valores.Cantidad = $("#txtCantidadDevolver").val();
        Productos.idsFacturasDetalle.push(Valores);
        $("#dialogAgregarProductosCantidades").dialog("close");
        $("#txtCantidadDevolver").val("");
        Termino_grdProductosNotaCreditoProveedorDevolucionCancelacion();
    }
}

function EdicionProductosNotaCreditoProveedorDevolucionCancelacion(valor, id, rowid, iCol) {

}
 
//-----Validaciones------------------------------------------------------
function ValidaNotaCreditoProveedor(pNotaCreditoProveedor) {
    var errores = "";

    if (pNotaCreditoProveedor.IdProveedor == 0)
    { errores = errores + "<span>*</span> El campo proveedor esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCreditoProveedor.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCreditoProveedor.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCreditoProveedor.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCreditoProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCreditoProveedor.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pNotaCreditoProveedor.Monto == 0)
    { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCreditoProveedor.PorcentajeIVA == 0)
    { errores = errores + "<span>*</span> El campo porcentaje IVA esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCreditoProveedor.IVA == 0)
    { errores = errores + "<span>*</span> El campo IVA esta vacio, favor de capturarlo.<br />"; }
    
    if (pNotaCreditoProveedor.Total == 0)
    { errores = errores + "<span>*</span> El campo total esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaNotaCreditoProveedorEdicion(pNotaCreditoProveedor) {
    var errores = "";

    if (pNotaCreditoProveedor.IdProveedor == 0)
    { errores = errores + "<span>*</span> El campo proveedor esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.Monto == 0)
    { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.PorcentajeIVA == 0)
    { errores = errores + "<span>*</span> El campo porcentaje IVA esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.IVA == 0)
    { errores = errores + "<span>*</span> El campo IVA esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.Total == 0)
    { errores = errores + "<span>*</span> El campo total esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaNotaCreditoProveedorEdicionDevolucionCancelacion(pNotaCreditoProveedor) {
    var errores = "";

    if (pNotaCreditoProveedor.IdProveedor == 0)
    { errores = errores + "<span>*</span> El campo proveedor esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(NotaCreditoProveedor) {
    var errores = "";

    if (NotaCreditoProveedor.pIdNotaCreditoProveedor == 0)
    { errores = errores + "<span>*</span> No hay nota de crédito por asociar, favor de elegir alguno.<br />"; }

    if (NotaCreditoProveedor.pIdProveedor == 0)
    { errores = errores + "<span>*</span> No hay proveedor por asociar, favor de elegir alguno.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDatosFiscales(pNotaCreditoProveedor) {
    var errores = "";

    if (pNotaCreditoProveedor.pIdNotaCreditoProveedor == 0)
    { errores = errores + "<span>*</span> No hay nota de crédito por asociar, favor de elegir alguno.<br />"; }

    if (pNotaCreditoProveedor.LugarExpedicion == "")
    { errores = errores + "<span>*</span> El campo lugar de expedición esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> No hay método de pago por asociar, favor de elegir alguno.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaMotivoCancelacion(pNotaCreditoProveedor) {
    var errores = "";

    if (pNotaCreditoProveedor.pIdNotaCreditoProveedor == 0)
    { errores = errores + "<span>*</span> No hay nota de crédito por asociar, favor de elegir alguno.<br />"; }

    if (pNotaCreditoProveedor.MotivoCancelacion == "")
    { errores = errores + "<span>*</span> El campo motivo de cancelación esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(NotaCreditoProveedor) {
    var errores = "";

    if (NotaCreditoProveedor.IdEncabezadoFactura == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(NotaCreditoProveedor.Monto) > parseFloat(NotaCreditoProveedor.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al disponible.<br />"; }

    if (parseFloat(NotaCreditoProveedor.Monto) > parseFloat(NotaCreditoProveedor.Saldo))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(NotaCreditoProveedor.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(NotaCreditoProveedor.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
function ValidaNotaCreditoProveedorDevolucionCancelacion(pNotaCreditoProveedor) {
    var errores = "";

    if (pNotaCreditoProveedor.IdProveedor == 0)
    { errores = errores + "<span>*</span> El campo proveedor esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.SerieNotaCredito == "")
    { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.FolioNotaCredito == 0)
    { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.Descripcion == "")
    { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de capturarlo.<br />"; }

    if (pNotaCreditoProveedor.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pNotaCreditoProveedor.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;

}
function ValidaSiExisteNotaCreditoProveedor() {
    var errores = "";
    errores = errores + "<span>*</span> Favor de crear una nota de crédito.<br />"; 

    return errores;
}

function DevolucionProductos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "NotaCreditoProveedor.aspx/AgregarDevoluciones",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtMonto").val(respuesta.TotalMonto);
                $("#txtIVA").val(respuesta.IVA);
                $("#txtTotal").val(respuesta.Total);
                $("#grdNotaCreditoProveedor").trigger("reloadGrid");
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