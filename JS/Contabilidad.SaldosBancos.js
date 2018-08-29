/**/

$(function () {

    $("#btnBuscarSaldos").click(ObtenerSaldosBancos);

    ObtenerBancos();

    ObtenerSaldosBancos();

    $("#cmbBancos").change(CargarCuentaBancaria);

    $("#txtFechaInicio").datepicker();

});

function ObtenerSaldosBancos() {
    var Saldos = new Object();
    Saldos.IdBanco = parseInt($("#cmbBanco").val());
    Saldos.IdCuentaBancaria = parseInt($("#cmbCuentaBancaria").val());
    Saldos.FechaInicio = $("#txtFechaInicio").val();

    var Request = JSON.stringify(Saldos);

    $("#SaldosBancos").obtenerVista({
        url: "SaldosBancos.aspx/ObtenerSaldosBancos",
        parametros: Request,
        nombreTemplate: "tmplSaldosBancos.html",
        despuesDeCompilar: function () {

        }
    });
}

function ObtenerBancos() {
    $.ajax({
        url: "SaldosBancos.aspx/ObtenerBancos",
        type: "post",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0) {
                var ListaBancos = json.Modelo.ListaBancos;
                for (x in ListaBancos) {
                    $("#cmbBanco").append($('<option value="' + ListaBancos[x].Valor + '">' + ListaBancos[x].Descripcion + '</option>'));
                }
            }
            else {
                MostrarMensajeError(json.Descripcion);
            }

        }
    });
}

function CargarCuentaBancaria() {
    var CuentaBancaria = new Object();
    CuentaBancaria.IdBanco = parseInt($("#cmbBanco", "#divAgregarMovimiento").val());

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