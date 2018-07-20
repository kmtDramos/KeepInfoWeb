/**/

$(function () {

    MantenerSesion();
    setInterval(MantenerSesion, 1000 * 60 * 1.5);

    $("#btnAgregarMovimiento").click(AgregarMovimiento);

});

function FiltroMovimientos() {

}

function AgregarMovimiento() {
    var ventana = $("<div></div>");

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
            "Guardar": function () { $(this).dialog("close"); },
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