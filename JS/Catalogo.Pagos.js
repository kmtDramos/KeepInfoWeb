//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function () {
        ActualizarPanelControles("Pagos");
    });

    ObtenerFormaFiltrosPagos();

    //-----Funcion del Grid

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarPagos", function () {
        ObtenerFormaAgregarPago();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaConciliarPagos", function () {
        ObtenerFormaConciliarPagos();
    });

    $("#dialogEditarPago, #dialogAgregarPago").on("click", "#btnObtenerFormaAsociarDocumentos", function () {
        var Pago = new Object();
       
        if ($("#divFormaEditarPago, #divFormaAgregarPago").attr("IdPago") != null && $("#divFormaEditarPago, #divFormaAgregarPago").attr("IdPago") != "") {
            Pago.pIdPago = parseInt($("#divFormaEditarPago,#divFormaAgregarPago").attr("IdPago"));
            Pago.pIdCliente = parseInt($("#divFormaEditarPago, #divFormaAgregarPago").attr("IdCliente"));
            Pago.IdTipoMoneda = $("#cmbTipoMoneda").val();
            Pago.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());

            if (Pago.IdTipoMoneda == 2) {
                Pago.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
            }
            else {
                Pago.TipoCambio = "1";
            }

            if (Pago.TipoCambio.replace(" ", "") == "") {
                Pago.TipoCambio = 0;
            }

            if (Pago.TipoCambioDOF.replace(" ", "") == "") {
                Pago.TipoCambioDOF = 0;
            }

            var validacion = ValidaAsociacionDocumentos(Pago);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.pPago = Pago;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
        }
        else {
            AgregarPagosEdicion();
        }
    });
    
    $("#grdPagos").on("click", ".imgFormaConsultarPago", function () {
        var registro = $(this).parents("tr");
        var Pago = new Object();
        Pago.pIdPago = parseInt($(registro).children("td[aria-describedby='grdPagos_IdCuentasPorCobrar']").html());
        ObtenerFormaConsultarPago(JSON.stringify(Pago));
    });

    $("#grdCuentaBancaria").on("dblclick", "td", function () {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");
    });

    $("#grdCuentaBancaria").on("click", ".imgSeleccionarCuentaBancaria", function () {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");

    });
    
    $("#grdMovimientosCobros").on("click", ".imgEliminarMovimiento", function () {

        var registro = $(this).parents("tr");
        var pPagoEncabezadoFactura = new Object();
        pPagoEncabezadoFactura.pIdPagoEncabezadoFactura = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdCuentasPorCobrarEncabezadoFactura']").html());
        var oRequest = new Object();
        oRequest.pPagoEncabezadoFactura = pPagoEncabezadoFactura;
        
        SetEliminarPagoEncabezadoFactura(JSON.stringify(oRequest));
    });
    
    $('#dialogAgregarPago, #dialogEditarPago').on('keypress', '#txtTipoCambio', function (event) {
        if (!ValidarFlotante(event, $(this).val())) {
            return false;
        }
    });

    $("#dialogAgregarPago, #dialogEditarPago").on("click", "#chkConciliado", function () {
        $("#txtFechaConciliacion").prop('disabled', function () {
            return !$(this).prop('disabled');
        });
    });

    $("#dialogAgregarPago, #dialogEditarPago").on("click", "#divBuscarCuentasContables", function () {
        $("#divFormaCuentaBancaria").obtenerVista({
            nombreTemplate: "tmplMuestraCuentasContables.html",
            despuesDeCompilar: function () {
                FiltroCuentaBancaria();
                $("#dialogMuestraCuentasBancarias").dialog("open");
            }
        });
    });

    $('#dialogAgregarPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaAgregarPago").remove();
        },
        buttons: {
            "Guardar": function () {
                if ($("#divFormaEditarPago, #divFormaAgregarPago").attr("IdPago") == null || $("#divFormaEditarPago, #divFormaAgregarPago").attr("IdPago") == "") {
                    AgregarPago();
                }
                else {
                    EditarPago();
                }
            },
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConsultarPago").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaEditarPago").remove();
        },
        buttons: {
            "Timbrar": function () {
                TimbrarPago();
            },
            "Editar": function () {
                EditarPago();
            },
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConciliarPagos').dialog({
        autoOpen: false,
        height: '620',
        width: '1000',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConciliarPagos").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraCuentasBancarias').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaMuestraCuentasBancarias").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
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
        close: function () {
            HabilitaMonto();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });
    
});

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarPago() {
    var pPago = new Object();
    pPago.IdCuentaBancaria = $("#divFormaAgregarPago").attr("idcuentabancaria");
    if ($("#divFormaAgregarPago").attr("idCliente") == "") {
        pPago.IdCliente = 0;
    }
    else {
        pPago.IdCliente = $("#divFormaAgregarPago").attr("idCliente");
    }
    pPago.CuentaBancaria = $("#txtCuenta").val();
    pPago.IdMetodoPago = $("#cmbMetodoPago").val();
    pPago.Fecha = $("#txtFecha").val();
    pPago.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pPago.Referencia = $("#txtReferencia").val();
    pPago.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pPago.FechaAplicacion = $("#txtFechaAplicacion").val();
    pPago.FechaConciliacion = $("#txtFechaConciliacion").val();
    pPago.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pPago.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());

    if (pPago.IdTipoMoneda == 2) {
        pPago.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else {
        pPago.TipoCambio = "1";
    }

    if (pPago.TipoCambio.replace(" ", "") == "") {
        pPago.TipoCambio = 0;
    }

    if (pPago.TipoCambioDOF.replace(" ", "") == "") {
        pPago.TipoCambioDOF = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pPago.Conciliado = 1;
    }
    else {
        pPago.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pPago.Asociado = 1;
    }
    else {
        pPago.Asociado = 0;
    }

    var validacion = ValidaPago(pPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pPago = pPago;
    SetAgregarPago(JSON.stringify(oRequest));
}

function SetAgregarPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/AgregarPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdPagos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogAgregarPago").dialog("close");
        }
    });
}

function AgregarPagosEdicion() {
    var pPago = new Object();
    pPago.IdCuentaBancaria = $("#divFormaAgregarPago").attr("idCuentaBancaria");
    if ($("#divFormaAgregarPago").attr("idCliente") == "") {
        pPago.IdCliente = 0;
    }
    else {
        pPago.IdCliente = $("#divFormaAgregarPago").attr("idCliente");
    }
    pPago.CuentaBancaria = $("#txtCuenta").val();
    pPago.IdMetodoPago = $("#cmbMetodoPago").val();
    pPago.Fecha = $("#txtFecha").val();
    pPago.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pPago.Referencia = $("#txtReferencia").val();
    pPago.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pPago.FechaAplicacion = $("#txtFechaAplicacion").val();
    pPago.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pPago.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());

    if (pPago.IdTipoMoneda == 2) {
        pPago.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else {
        pPago.TipoCambio = "1";
    }

    if (pPago.TipoCambio.replace(" ", "") == "") {
        pPago.TipoCambio = 0;
    }

    if (pPago.TipoCambioDOF.replace(" ", "") == "") {
        pPago.TipoCambioDOF = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pPago.Conciliado = 1;
    }
    else {
        pPago.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pPago.Asociado = 1;
    }
    else {
        pPago.Asociado = 0;
    }

    var validacion = ValidaPagoEdicion(pPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pPago = pPago;
    SetAgregarPagoEdicion(JSON.stringify(oRequest));
}

function SetAgregarPagoEdicion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/AgregarPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarPago").attr("idPago", respuesta.IdCuentasPorCobrar);
                $("#txtCuenta").attr("disabled", "true");
                $("#cmbMetodoPago").attr("disabled", "true");
                $("#txtFecha").attr("disabled", "true");
                $("#txtRazonSocial").attr("disabled", "true");
                $("#txtFechaAplicacion").attr("disabled", "true");
                $("#chkAsociado").attr("disabled", "true");
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFolio").val(respuesta.Folio);
                $("#grdPagos").trigger("reloadGrid");
                var Pago = new Object();
                Pago.pIdPago = parseInt($("#divFormaEditarPago,#divFormaAgregaPago").attr("IdPago"));
                Pago.pIdCliente = parseInt($("#divFormaEditarPago, #divFormaAgregarPago").attr("IdCliente"));
                Pago.IdTipoMoneda = $("#cmbTipoMoneda").val();
                Pago.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());

                if (Pago.IdTipoMoneda == 2) {
                    Pago.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else {
                    Pago.TipoCambio = "1";
                }

                if (Pago.TipoCambio.replace(" ", "") == "") {
                    Pago.TipoCambio = 0;
                }

                if (Pago.TipoCambioDOF.replace(" ", "") == "") {
                    Pago.TipoCambioDOF = 0;
                }

                var validacion = ValidaAsociacionDocumentos(Pago);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.pPago = Pago;
                ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function EditarPago() {
    var pPago = new Object();
    pPago.IdPago = $("#divFormaEditarPago, #divFormaAgregarPago").attr("idPago");
    if ($("#divFormaEditarPago, #divFormaAgregarPago").attr("idCliente") != null) {
        pPago.IdCliente = $("#divFormaEditarPago, #divFormaAgregarPago").attr("idCliente");
    }
    else {
        pPago.IdCliente = 0;
    }

    pPago.CuentaBancaria = $("#txtCuenta").val();
    pPago.IdMetodoPago = $("#cmbMetodoPago").val();
    pPago.Fecha = $("#txtFecha").val();
    pPago.Folio = $("#txtFolio").val();
    pPago.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pPago.Referencia = $("#txtReferencia").val();
    pPago.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pPago.FechaAplicacion = $("#txtFechaAplicacion").val();
    pPago.FechaConciliacion = $("#txtFechaConciliacion").val();
    pPago.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pPago.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());

    if (pPago.IdTipoMoneda == 2) {
        pPago.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else {
        pPago.TipoCambio = "1";
    }

    if (pPago.TipoCambio.replace(" ", "") == "") {
        pPago.TipoCambio = 0;
    }

    if (pPago.TipoCambioDOF.replace(" ", "") == "") {
        pPago.TipoCambioDOF = 0;
    }


    if ($("#chkConciliado").is(':checked')) {
        pPago.Conciliado = 1;
    }
    else {
        pPago.Conciliado = 0;
    }

    var validacion = ValidaPago(pPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pPago = pPago;
    SetEditarPago(JSON.stringify(oRequest));
}

function SetEditarPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/EditarPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdPagos").trigger("reloadGrid");
                $("#dialogEditarPago, #dialogAgregarPago").dialog("close");
                
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function EdicionFacturas(valor, id, rowid, iCol) {
    var TipoMoneda = $('#grdFacturas').getCell(rowid, 'TipoMoneda');
    var Saldo = $('#grdFacturas').getCell(rowid, 'Saldo');
    var EsParcialidad = $('#grdFacturas').getCell(rowid, 'EsParcialidad');
    var Folio = $('#grdFacturas').getCell(rowid, 'NumeroFactura');
    if (Folio == "0")
    {
        MostrarMensajeError("La Factura aun no se encuentra Timbrada."); return false;
    }

    var Pago = new Object();

    if (EsParcialidad == true || EsParcialidad == "True") {
        Pago.EsParcialidad = 1;
    }
    else {
        Pago.EsParcialidad = 0;
    }

    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        Pago.IdTipoMoneda = 1;
        Pago.TipoCambio = 1;
        Pago.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        Pago.IdTipoMoneda = 2;
        Pago.TipoCambio = $("#spanTipoCambioDolares").text();
        Pago.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    Pago.TipoMoneda = TipoMoneda;

    Pago.Monto = QuitarFormatoNumero(valor);
    Pago.Saldo = QuitarFormatoNumero(Saldo);
    Pago.IdEncabezadoFactura = id;
    Pago.rowid = rowid;
    Pago.IdPago = $("#divFormaAsociarDocumentos").attr("idCuentasPorCobrar");
    var validacion = ValidarMontos(Pago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pPago = Pago;
    SetEditarMontos(JSON.stringify(oRequest));
}

function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);

            console.log(respuesta);
            if (respuesta.Error == 0) {
                if (respuesta.EsParcialidad == 1) {
                    //MostrarMensajeError(respuesta.Descripcion);
                }
                $("#chkAsociado").attr("checked", "checked");

                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosCuentasPorCobrar;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosCuentasPorCobrar / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetEliminarPagoEncabezadoFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/EliminarPagoEncabezadoFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.EsParcialidad == 1) {
                    //MostrarMensajeError(respuesta.Descripcion);
                }

                $("#chkAsociado").attr("checked", "");

                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");
                $("#grdCuentasPorCobrar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosCuentasPorCobrar;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosCuentasPorCobrar / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function AutocompletarCliente() {

    $('#txtRazonSocial').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtRazonSocial').val();
            $.ajax({
                type: 'POST',
                url: 'Pagos.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarPago, #divFormaEditarPago").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarPago, #divFormaEditarPago").attr("idCliente", pIdCliente);
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function HabilitaMonto() {
    var Pago = new Object();
    Pago.pIdPago = parseInt($("#divFormaEditarPago, #divFormaAgregarPago").attr("IdPago"));
    ObtenerHabilitaMonto(JSON.stringify(Pago));
}

function ConciliarIngreso(IdPago) {

    var pPago = new Object();
    pPago.pIdPago = IdPago
    var oRequest = new Object();
    oRequest.pPago = pPago;
    SetConciliarIngreso(JSON.stringify(oRequest));
}

function SetConciliarIngreso(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/ConciliarIngreso",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdPagosConciliar").trigger("reloadGrid");
                $("#grdPagos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogAgregarPago").dialog("close");
        }
    });
}

function TimbrarPago() {
    var pPago = new Object();
    if ($("#divFormaConsultarPago, #divFormaEditarPago").attr("idPago") == "" || $("#divFormaConsultarPago, #divFormaEditarPago").attr("idPago") == null) {
        MostrarMensajeError("No hay Complemento de Pago para timbrar"); return false;
    }
    else {
        pPago.IdPago = $("#divFormaConsultarPago, #divFormaEditarPago").attr("idPago");
        
        //Nueva Forma de Timbrar Pagos
        //console.log(oRequest);
        var oRequest = new Object();
        oRequest.IdPago = pPago.IdPago;
        ObtenerPagoATimbrar(JSON.stringify(oRequest));

    }
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarPago() {
    $("#dialogAgregarPago").obtenerVista({
        nombreTemplate: "tmplAgregarPago.html",
        url: "Pagos.aspx/ObtenerFormaAgregarPago",
        despuesDeCompilar: function (pRespuesta) {
            $("#txtFecha").datepicker({
                maxDate: new Date()
            });
            $("#txtFechaAplicacion").datepicker({
                maxDate: new Date(),
                onSelect: function () {
                    var pRequest = new Object();
                    pRequest.pIdTipoMonedaDestino = 2;
                    pRequest.pFecha = $("#txtFechaAplicacion").val();;
                    ObtenerTipoCambioDiarioOficial(JSON.stringify(pRequest));
                }
            });
            $("#txtFechaConciliacion").datepicker({
                maxDate: new Date()
            });
            AutocompletarCliente();

            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambioIngresos == 1) {
                $("#txtTipoCambio").removeAttr("disabled");

            }
            else {
                $("#txtTipoCambio").attr("disabled", "disabled");
            }

            $('#dialogAgregarPago').on('focusout', '#txtImporte', function (event) {
                $("#txtImporte").val(formato.moneda(QuitarFormatoNumero($("#txtImporte").val()), "$"));
            });

            $("#tabAsignarDocumentos").tabs();
            $("#dialogAgregarPago").dialog("open");
        }
    });
}

function ObtenerFormaConsultarPago(pIdPago) {
    $("#dialogConsultarPago").obtenerVista({
        nombreTemplate: "tmplConsultarPago.html",
        url: "Pagos.aspx/ObtenerFormaPago",
        parametros: pIdPago,
        despuesDeCompilar: function (pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarPago == 1) {
                $("#dialogConsultarPago").dialog("option", "buttons", {
                    "Timbrar": function () {
                        TimbrarPago();
                    },
                    "Editar": function () {
                        $(this).dialog("close");
                        var Pago = new Object();
                        Pago.IdPago = parseInt($("#divFormaConsultarPago").attr("IdPago"));
                        ObtenerFormaEditarPago(JSON.stringify(Pago))
                    }
                });
                $("#dialogConsultarPago").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarPago").dialog("option", "buttons", {});
                $("#dialogConsultarPago").dialog("option", "height", "auto");
            }
            $("#dialogConsultarPago").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaEditarPago(IdPago) {
    $("#dialogEditarPago").obtenerVista({
        nombreTemplate: "tmplEditarPago.html",
        url: "Pagos.aspx/ObtenerFormaEditarPago",
        parametros: IdPago,
        despuesDeCompilar: function (pRespuesta) {
            Inicializar_grdMovimientosCobrosEditar();
            $("#dialogEditarPago").dialog("option", "height", "auto");
            $("#txtFecha").datepicker({
                maxDate: new Date()
            });
            $("#txtFechaAplicacion").datepicker({
                maxDate: new Date()
            });
            $("#txtFechaConciliacion").datepicker({
                maxDate: new Date()
            });
            AutocompletarCliente();
            $("#tabAsignarDocumentosEditar").tabs();
            $("#dialogEditarPago").dialog("open");
        }
    });
}

function ObtenerFormaConciliarPagos() {
    $("#dialogConciliarPagos").obtenerVista({
        nombreTemplate: "tmplConciliarPagos.html",
        url: "Pagos.aspx/ObtenerFormaConciliarPagos",
        despuesDeCompilar: function (pRespuesta) {
            $("#dialogConciliarPagos").dialog("open");
            Inicializar_grdPagosConciliar();
        }
    });
}

function ObtenerTipoCambioDiarioOficial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/ObtenerTipoCambioDiarioOficial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtTipoCambio").val(respuesta.TipoCambioDiarioOficial);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerFormaFiltrosPagos() {
    $("#divFiltrosPagos").obtenerVista({
        nombreTemplate: "tmplFiltrosPagos.html",
        url: "Pagos.aspx/ObtenerFormaFiltroPagos",
        despuesDeCompilar: function (pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function () {
                        FiltroPagos();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function () {
                        FiltroPagos();
                    }
                });
            }

            $('#divFiltrosPagos').on('click', '#chkPorFecha', function (event) {
                FiltroPagos();
            });

        }
    });
}

function ObtenerDatosCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/ObtenerDatosCuentaBancaria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                var Proyecto = new Object();
                $("#txtCuenta").val(respuesta.Modelo.CuentaBancaria);
                $("#spanBanco").text(respuesta.Modelo.Descripcion + ' (' + respuesta.Modelo.CuentaBancaria + ') ' + respuesta.Modelo.Banco + ' ' + respuesta.Modelo.TipoMoneda);
                $("#spanSaldo").text(formato.moneda(respuesta.Modelo.Saldo, "$"));
                $("#txtTipoCambio").val(respuesta.Modelo.TipoCambio);
                $("#divFormaAgregarPago, #divFormaEditarPago").attr("idCuentaBancaria", respuesta.Modelo.IdCuentaBancaria);
                $("#cmbTipoMoneda option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFechaAplicacion").val(respuesta.Modelo.Fecha);

                if (parseInt(respuesta.Modelo.Permisos.puedeEditarTipoCambioIngresos) == 0) {
                    $("#txtTipoCambio").attr("disabled", "disabled");
                }
                else {
                    $("#txtTipoCambio").removeAttr("disabled");
                }

                if (respuesta.Modelo.PuedeVerSaldo == 1) {
                    $("#PuedeVerSaldoEtiqueda").css("display", "block");
                    $("#spanSaldo").css("display", "block");
                }
                else {
                    $("#PuedeVerSaldoEtiqueda").css("display", "none");
                    $("#spanSaldo").css("display", "none");
                }
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerFormaAsociarDocumentos(pPago) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentos.html",
        url: "Pagos.aspx/ObtenerFormaAsociarDocumentos",
        parametros: pPago,
        despuesDeCompilar: function (pRespuesta) {
            FiltroFacturas();
            FiltroMovimientosCobros();
            $("#dialogMuestraAsociarDocumentos").dialog("open");
        }
    });
}

function ObtenerHabilitaMonto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Pagos.aspx/ObtenerHabilitaMonto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                if (parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares)) {
                    $("#txtImporte").attr("disabled", "disabled");
                }
                else {
                    $("#txtImporte").removeAttr("disabled");
                }

                if (respuesta.Modelo.puedeEditarTipoCambioIngresos == 0 || parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares)) {
                    $("#txtTipoCambio").attr("disabled", "disabled");
                }
                else {
                    $("#txtTipoCambio").removeAttr("disabled");
                }

            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });

}

function ObtenerFormaDatosFiscales(pRequest) {
    $("#dialogDatosFiscales").obtenerVista({
        nombreTemplate: "tmplDatosFiscales.html",
        parametros: pRequest,
        url: "Pagos.aspx/LlenaDatosFiscales",
        despuesDeCompilar: function () {
            $("#dialogDatosFiscales").dialog("open");
        }
    });
}

function FiltroMovimientosCobros() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobros').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobros').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobros').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobros').getGridParam('sortorder');
    request.pIdPago = 0;
    if ($("#divFormaEditarPago, #divFormaAsociarDocumentos").attr("IdPago") != null) {
        request.pIdPago = $("#divFormaEditarPago, #divFormaAsociarDocumentos").attr("IdPago");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerMovimientosCobros',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
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
    request.pIdPago = 0;
    if ($("#divFormaEditarPago, #divFormaConsultarPago, #divFormaAsociarDocumentos").attr("IdPago") != null) {
        request.pIdPago = $("#divFormaEditarPago, #divFormaConsultarPago, #divFormaAsociarDocumentos").attr("IdPago");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerMovimientosCobrosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
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
    request.pIdPago = 0;
    if ($("#divFormaEditarPago, #divFormaConsultarPago, #divFormaAsociarDocumentos").attr("IdPago") != null) {
        request.pIdPago = $("#divFormaEditarPago, #divFormaConsultarPago, #divFormaAsociarDocumentos").attr("IdPago");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerMovimientosCobrosEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroCuentaBancaria() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCuentaBancaria').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCuentaBancaria').getGridParam('page');
    request.pColumnaOrden = $('#grdCuentaBancaria').getGridParam('sortname');
    request.pTipoOrden = $('#grdCuentaBancaria').getGridParam('sortorder');
    request.pCuentaBancaria = "";
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerCuentaBancaria',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdCuentaBancaria')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroPagos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPagos').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPagos').getGridParam('page');
    request.pColumnaOrden = $('#grdPagos').getGridParam('sortname');
    request.pTipoOrden = $('#grdPagos').getGridParam('sortorder');
    request.pFolio = "";
    request.pAI = 0;
    request.pRazonSocial = "";
    request.pGestor = "";

    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;

    request.pAsociado = "NO";

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }

    if ($('#gs_Gestor').val() != null) { request.pGestor = $("#gs_Gestor").val(); }

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

    if ($('#gs_Asociado').val() != null) { request.pAsociado = $("#gs_Asociado").val(); }


    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerPagos',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdPagos')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroPagosConciliar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPagosConciliar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPagosConciliar').getGridParam('page');
    request.pColumnaOrden = $('#grdPagosConciliar').getGridParam('sortname');
    request.pTipoOrden = $('#grdPagosConciliar').getGridParam('sortorder');
    request.pFolio = "";
    request.pAI = 0;
    request.pRazonSocial = "";

    if ($('#gs_FolioConciliar').val() != null) { request.pFolio = $("#gs_FolioConciliar").val(); }

    if ($('#gs_RazonSocialConciliar').val() != null) { request.pRazonSocial = $("#gs_RazonSocialConciliar").val(); }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerPagosConciliar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdPagosConciliar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturas() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturas').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturas').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturas').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturas').getGridParam('sortorder');
    request.pSerie = "";
    request.pNumeroFactura = 0;
    request.pIdCliente = 0;

    if ($('#gs_NumeroFactura').val() != null) {
        request.pNumeroFactura = $("#gs_NumeroFactura").val();
    }
    if ($("#divFormaEditarPago, #divFormaAsociarDocumentos").attr("IdCliente") != null) {
        request.pIdCliente = $("#divFormaEditarPago, #divFormaAsociarDocumentos").attr("IdCliente");
        if ($('#divContGridAsociarDocumento').find(gs_Serie).existe()) {
            request.pSerie = $('#divContGridAsociarDocumento').find(gs_Serie).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Pagos.aspx/ObtenerFacturas',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturas')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//-----Validaciones------------------------------------------------------

function ValidaPago(pPago) {
    var errores = "";

    if (pPago.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pPago.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pPago.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pPago.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pPago.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pPago.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pPago.FechaAplicacion, 'aaaammdd') > ConvertirFecha(pPago.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser mayor a la fecha de emisión.<br />"; }

    if (pPago.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (parseFloat(pPago.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(pPago) {
    var errores = "";

    if (pPago.pIdPago== 0)
    { errores = errores + "<span>*</span> No hay cobro por asociar, favor de elegir alguno.<br />"; }

    if (pPago.pIdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaPagoEdicion(pPago) {
    var errores = "";

    if (pPago.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pPago.IdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pPago.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pPago.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pPago.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pPago.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pPago.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pPago.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pPago.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pPago.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(Pago) {
    var errores = "";

    if (Pago.IdEncabezadoFactura == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(Pago.Monto) > parseFloat(Pago.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al disponible.<br />"; }

    if (parseFloat(Pago.Monto) > parseFloat(Pago.Saldo))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(Pago.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(Pago.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

////////////////////////  Nueva forma de guardar Complementos de Pago /////////////////////////////////////

/* Timbrar */
function ObtenerPagoATimbrar(Request) {
    MostrarBloqueo();
    $.ajax({
        url: "Pagos.aspx/ObtenerDatosTimbradoPago",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                TimbPago(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function TimbPago(json) {
    var Comprobante = new Object();
    Comprobante.Comprobante = json.Comprobante;
    Comprobante.Id = json.Id;
    Comprobante.Token = json.Token;
    Comprobante.RFC = json.RFC;
    Comprobante.RefID = json.RefID;
    Comprobante.Formato = json.Formato;
    Comprobante.NoCertificado = json.NoCertificado;
    Comprobante.Correos = json.Correos;
    Comprobante.RutaCFDI = json.RutaCFDI;
    //Comprobante.ActualizarMontos = json.ActualizarMontos;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "http://" + window.location.hostname + "/WebServiceDiverza/Pagos.aspx/TimbrarPago",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            console.log(Respuesta);
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                GuardarFacturaPago(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function GuardarFacturaPago(json) {
    var Comprobante = new Object();
    Comprobante.UUId = json.uuid;
    Comprobante.RefId = json.ref_id;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "Pagos.aspx/GuardarTimbradoPago",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            MostrarMensajeError(json.Descripcion);
            OcultarBloqueo();
    });
}


/* Cancelar */
function ObtenerFacturaACancelar(Request) {
    console.log("Cancelar");
    MostrarBloqueo();
    $.ajax({
        url: "FacturaCliente.aspx/ObtenerDatosCancelacion",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#dialogMotivoCancelacionFactura").dialog("close");
                CancelarFactura(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function CancelarFactura(json) {
    console.log("Cancelando");
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
        url: "http://localhost/WebServiceDiverza/Facturacion.aspx/CancelarFactura",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                EditarFacturaCancelada(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function EditarFacturaCancelada(json) {
    console.log("Cancelado");
    var Comprobante = new Object();
    Comprobante.UUId = json.uuid;
    Comprobante.RefId = json.ref_id;
    Comprobante.Date = json.date;
    Comprobante.Contenido = json.content;
    Comprobante.MotivoCancelacion = json.motivoCancelacion;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "FacturaCliente.aspx/EditarFactura",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                MostrarMensajeError(json.Cancelado);
            }
            else {
                MostrarMensajeError(json.Descripcion);

            }
            OcultarBloqueo();
        }
    });
}

