/**/

$(function () {

    MantenerSesion();
    setInterval(MantenerSesion, 1000 * 60 * 1.5);

    ObtenerBancos();

    $("#cmbBanco").change(FiltroMovimientos);

    $("#btnAgregarMovimiento").click(ObtenerFormaAgregarMovimiento);

    $("#btnTraspaso").click(ObtenerFormaTraspaso);

});

function FiltroMovimientos() {
    var Movimientos = new Object();
    Movimientos.pTamanoPaginacion = $('#grdMovimientos').getGridParam('rowNum');
    Movimientos.pPaginaActual = $('#grdMovimientos').getGridParam('page');
    Movimientos.pColumnaOrden = $('#grdMovimientos').getGridParam('sortname');
    Movimientos.pTipoOrden = $('#grdMovimientos').getGridParam('sortorder');
    Movimientos.pIdBanco = parseInt($("#cmbBanco").val());
    var Request = JSON.stringify(Movimientos);
    $.ajax({
        url: "MovimeintosBancarios.aspx/ObtenerMovimientos",
        data: Request,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdMovimientos')[0].addJSONData(JSON.parse(jsondata.responseText).d); OcultarBloqueo(); }
            else { console.log(JSON.parse(jsondata.responseText).Message); }
            TerminoMovimientos();
        }
    });
}

function TerminoMovimientos() {

}

function ObtenerFormaAgregarMovimiento() {
    var ventana = $("<div id='divAgregarMovimiento'></div>");

    $(ventana).dialog({
        modal: true,
        title: "Agregar movimiento",
        resizable: false,
        draggable: false,
        autoOpen: false,
        width: "auto",
        close: function () {
            $(this).remove();
        },
        buttons: {
            "Guardar": function () { AgregarMovimiento(); },
            "Cancelar": function () { $(this).dialog("close"); }
        }
    });

    $(ventana).obtenerVista({
        url: "MovimeintosBancarios.aspx/ObtenerFormaAgregarMovimiento",
        nombreTemplate: "tmplAgregarMovimiento.html",
        despuesDeCompilar: function () {

            $(ventana).dialog("open");

            $("#cmbBanco", "#divAgregarMovimiento").change(function () {
                CargarCuentaBancaria();
            }).val($("#cmbBanco").val()).change();

            $("#txtFechaMovimiento").datetimepicker({
                maxDate: new Date()
            });

            $("#txtMonto").focus(function () {
                $(this).val(QuitaFormatoMoneda($(this).val()));
                $(this).select();
            }).blur(function () {
                var valor = (isNaN(parseFloat($(this).val()))) ? 0 : parseFloat($(this).val());
                $(this).val(formato.moneda(valor,'$'));
            }).keypress(function (event) {
                    return ValidarNumeroPunto(event, this.value);
            });

            $("#txtRazonSocial").autocomplete({
                source: function (request, response) {
                    var pRequest = new Object();
                    pRequest.pRazonSocial = $("#txtRazonSocial").val();
                    $.ajax({
                        type: 'POST',
                        url: 'MovimeintosBancarios.aspx/BuscarRazonSocial',
                        data: JSON.stringify(pRequest),
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (pRespuesta) {
                            var json = jQuery.parseJSON(pRespuesta.d);
                            response($.map(json.Table, function (item) {
                                return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdOrganizacion }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var IdOrganizacion = ui.item.id;
                    $(this).attr("IdOrganizacion", IdOrganizacion);
                },
                change: function (event, ui) { },
                open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
            });

            $("#cmbTipoMoneda").change(function () {
                var TipoCambio = $("option:checked", this).attr("TipoCambio");
                $("#txtTipoCambio").val(TipoCambio);
            });

        }
    });
}

function ObtenerFormaTraspaso() {
    var ventana = $("<div id='divTraspaso'></div>");

    $(ventana).dialog({
        modal: true,
        title: "Crear traspaso",
        resizable: false,
        draggable: false,
        autoOpen: false,
        width: "auto",
        close: function () {
            $(this).remove();
        },
        buttons: {
            "Generar traspaso": function () { GenerarTraspaso(); },
            "Cancelar": function () { $(this).dialog("close"); }
        }
    });

    $(ventana).obtenerVista({
        url: "MovimeintosBancarios.aspx/ObtenerFormaAgregarMovimiento",
        nombreTemplate: "tmplTraspasoEntreCuentas.html",
        despuesDeCompilar: function () {

            $(ventana).dialog("open");

            $("#cmbBancoOrigen").change(function () {
                CargarCuentaBancariaOrigen();
            }).val($("#cmbBanco").val()).change();

            $("#cmbBancoDestino").change(function () {
                CargarCuentaBancariaDestino();
            }).val($("#cmbBanco").val()).change();

            $("#txtMontoOrigen").focus(function () {
                $(this).val(QuitaFormatoMoneda($(this).val()));
                $(this).select();
            }).blur(function () {
                var valor = (isNaN(parseFloat($(this).val()))) ? 0 : parseFloat($(this).val());
                $(this).val(formato.moneda(valor, '$'));
            }).keypress(function (event) {
                return ValidarNumeroPunto(event, this.value);
                });

            $("#txtMontoDestino").focus(function () {
                if ($(this).val() == "")
                    $(this).val(0);
                $(this).val(QuitaFormatoMoneda($(this).val()));
                $(this).select();
            }).blur(function () {
                var valor = (isNaN(parseFloat($(this).val()))) ? 0 : parseFloat($(this).val());
                $(this).val(formato.moneda(valor, '$'));
            }).keypress(function (event) {
                return ValidarNumeroPunto(event, this.value);
            });

        }
    });
}

function CargarCuentaBancaria() {
    var CuentaBancaria = new Object();
    CuentaBancaria.IdBanco = parseInt($("#cmbBanco","#divAgregarMovimiento").val());

    var Request = JSON.stringify(CuentaBancaria);

    $.ajax({
        url: "MovimeintosBancarios.aspx/ObtenerCuentaBancaria",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            $("#cmbCuentaBancaria").html('<option value="">-Seleccionar-</option>');
            var CuentasBancarias = json.Modelo.CuentasBancarias;
            for (x in CuentasBancarias) {
                $("#cmbCuentaBancaria").append($('<option value="' + CuentasBancarias[x].Valor + '">' + CuentasBancarias[x].Descripcion + '</option>'));
            }
        }
    });

}

function CargarCuentaBancariaOrigen() {
    var CuentaBancaria = new Object();
    CuentaBancaria.IdBanco = parseInt($("#cmbBancoOrigen").val());

    var Request = JSON.stringify(CuentaBancaria);

    $.ajax({
        url: "MovimeintosBancarios.aspx/ObtenerCuentaBancaria",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            $("#cmbCuentaBancariaOrigen").html('<option value="">-Seleccionar-</option>');
            var CuentasBancarias = json.Modelo.CuentasBancarias;
            for (x in CuentasBancarias) {
                $("#cmbCuentaBancariaOrigen").append($('<option value="' + CuentasBancarias[x].Valor + '">' + CuentasBancarias[x].Descripcion + '</option>'));
            }
        }
    });

}

function CargarCuentaBancariaDestino() {
    var CuentaBancaria = new Object();
    CuentaBancaria.IdBanco = parseInt($("#cmbBancoDestino").val());

    var Request = JSON.stringify(CuentaBancaria);

    $.ajax({
        url: "MovimeintosBancarios.aspx/ObtenerCuentaBancaria",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            $("#cmbCuentaBancariaDestino").html('<option value="">-Seleccionar-</option>');
            var CuentasBancarias = json.Modelo.CuentasBancarias;
            for (x in CuentasBancarias) {
                $("#cmbCuentaBancariaDestino").append($('<option value="' + CuentasBancarias[x].Valor + '">' + CuentasBancarias[x].Descripcion + '</option>'));
            }
        }
    });

}

function AgregarMovimiento() {
    var Movimiento = new Object();
    Movimiento.IdCuentaBancaria = parseInt($("#cmbCuentaBancaria").val());
    Movimiento.IdTipoMovimiento = parseInt($("#cmbTipoMovimiento").val());
    Movimiento.IdTipoMoneda = parseInt($("#cmbTipoMoneda").val());
    Movimiento.TipoCambio = parseFloat($("#txtTipoCambio").val());
    Movimiento.FechaMovimiento = $("#txtFechaMovimiento").val();
    Movimiento.IdOrganizacion = parseInt($("#txtRazonSocial").attr("IdOrganizacion"));
    Movimiento.IdFlujoCaja = parseInt($("#cmbFlujoCaja").val());
    Movimiento.Monto = parseFloat(QuitaFormatoMoneda($("#txtMonto").val()));
    Movimiento.Referencia = $("#txtReferencia").val();

    var validacion = ValidarMovimiento(Movimiento);

    var Request = JSON.stringify(Movimiento);

    if (validacion == "")
    {
        $.ajax({
            url: "MovimeintosBancarios.aspx/AgregarMovimiento",
            type: "post",
            data: Request,
            dataType: "json",
            contentType: "application/json;chartset=utf-8",
            success: function (Respuesta) {
                var json = JSON.parse(Respuesta.d);

                if (json.Error == 0) {
                    $("#divAgregarMovimiento").dialog("close");
                    FiltroMovimientos();
                }
                else
                {
                    MostrarMensajeError(json.Descripcion);
                }
            }
        });
    }
    else
    {
        MostrarMensajeError(validacion);
    }

}

function GenerarTraspaso() {
    var Traspaso = new Object();
    Traspaso.IdCuentaBancariaOrigen = parseInt($("#cmbCuentaBancariaOrigen").val());
    Traspaso.IdTipoMonedaOrigen = parseInt($("#cmbTipoMonedaOrigen").val());
    Traspaso.TipoCambioOrigen = parseFloat($("#txtTipoCambioOrigen").val());
    Traspaso.MontoOrigen = parseFloat(QuitaFormatoMoneda($("#txtMontoOrigen").val()));
    Traspaso.ReferenciaOrigen = $("#txtReferenciaOrigen").val();
    Traspaso.IdCuentaBancariaDestino = parseInt($("#cmbCuentaBancariaDestino").val());
    Traspaso.IdTipoMonedaDestino = parseInt($("#cmbTipoMonedaDestino").val());
    Traspaso.TipoCambioDestino = parseFloat($("#txtTipoCambioDestino").val());
    Traspaso.MontoDestino = parseFloat(QuitaFormatoMoneda($("#txtMontoDestino").val()));
    Traspaso.ReferenciaDestino = $("#txtReferenciaDestino").val();

    var Request = JSON.stringify(Traspaso);

    $.ajax({
        url: "MovimeintosBancarios.aspx/GenerarTraspaso",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (Error == 0) {
                $("#divTraspaso").dialog("close");
            } else {
                MostrarMensajeError(json.Descripcion);
            }
        }
    });

}

function ValidarMovimiento(Movimiento) {

    var valido = "";

    valido += (Movimiento.IdCuentaBancaria != 0) ? valido : "<li>Favor de seleccionar una cuenta bancaria.</li>";
    valido += (Movimiento.IdTipoMovimiento != 0) ? valido : "<li>Favor de seleccionar un tipo de movimiento.</li>";
    valido += (Movimiento.IdTipoMoneda != 0) ? valido : "<li>Favor de seleccionar una moneda.</li>";
    valido += (Movimiento.FechaMovimiento != "") ? valido : "<li>Favor de seleccionar una fecha de movimiento.</li>";
    valido += (Movimiento.IdOrganizacion != 0) ? valido : "<li>Favor de seleccionar una razón social.</li>";
    valido += (Movimiento.IdFlujoCaja != 0) ? valido : "<li>Favor de seleccionar el flujo de caja.</li>";
    valido += (Movimiento.Monto > 0) ? valido : "<li>Favor de ingresar un monto.</li>";
    valido += (Movimiento.Referencia != "") ? valido : "<li>Favor de ingresar una referencia.</li>";

    valido += (valido != "") ? "Favor de completar los siguientes campos:": valido;

    return valido;
}

function ObtenerBancos() {
    $.ajax({
        url: "MovimeintosBancarios.aspx/ObtenerBancos",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0)
            {
                var ListaBancos = json.Modelo.ListaBancos;
                for (x in ListaBancos)
                {
                    $("#cmbBanco").append($('<option value="' + ListaBancos[x].Valor + '">' + ListaBancos[x].Descripcion + '</option>'));
                }
            }
            else
            {
                MostrarMensajeError(json.Descripcion);
            }

        }
    });
}