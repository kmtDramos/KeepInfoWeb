/**/

$(function () {

    MantenerSesion();
    setInterval(MantenerSesion, 1000 * 60 * 1.5);

    ObtenerBancos();

    $("#btnAgregarMovimiento").click(ObtenerFormaAgregarMovimiento);

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

            $("#cmbBanco").change(function () {
                CargarCuentaBancaria();
            });

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
                    pRequest.pCliente = $("#txtRazonSocial").val();
                    $.ajax({
                        type: 'POST',
                        url: 'MovimeintosBancarios.aspx/BuscarRazonSocial',
                        data: JSON.stringify(pRequest),
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (pRespuesta) {
                            var json = jQuery.parseJSON(pRespuesta.d);
                            response($.map(json.Table, function (item) {
                                return { label: item.RazonSocial, value: item.Cliente, id: item.IdCliente, Saldo: item.Saldo }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var pIdCliente = ui.item.id;
                    var Saldo = ui.item.Saldo;
                    $("#divFormaAgregarOportunidad, #divFormaEditarOportunidad").attr("idCliente", pIdCliente);
                    $("#lvlSaldo").text(formato.moneda(Saldo, '$'));
                },
                change: function (event, ui) { },
                open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
            }).change(function () { $("#lvlSaldo").text(''); });

        }
    });
}

function CargarCuentaBancaria() {
    var CuentaBancaria = new Object();
    CuentaBancaria.IdBanco = parseInt($("#cmbBanco").val());

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

function AgregarMovimiento() {
    var Movimiento = new Object();
    Movimiento.IdCuentaBancaria = parseInt($("#cmbCuentaBancaria").val());
    Movimiento.IdTipoMovimiento = parseInt($("#cmbTipoMovimiento").val());
    Movimiento.FechaMovimiento = $("#txtFechaMovimiento").val();
    Movimiento.IdOrganizacion = parseInt($("#txtRazonSocial").attr());
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

function ValidarMovimiento(Movimiento) {

    var valido = "";

    valido += (Movimiento.IdCuentaBancaria != 0) ? valido : "<li>Favor de seleccionar una cuenta bancaria.</li>";
    valido += (Movimiento.IdTipoMovimiento != 0) ? valido : "<li>Favor de seleccionar un tipo de movimiento.</li>";
    valido += (Movimiento.FechaMovimiento != "") ? valido : "<li>Favor de seleccionar una fecha de movimiento.</li>";
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