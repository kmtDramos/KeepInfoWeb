/**/

$(function () {

    MantenerSesion();
    setInterval(MantenerSesion, 1000 * 60 * 2.5);

    $("#btnAgregarSolicitudPago").click(ObtenerFormaAgregarSolicitudPago);

});

function FiltroSolicitudPago() {

    var SolicitudPago = new Object();
    SolicitudPago.pTamanoPaginacion = $('#grdSolicitudPago').getGridParam('rowNum');
    SolicitudPago.pPaginaActual = $('#grdSolicitudPago').getGridParam('page');
    SolicitudPago.pColumnaOrden = $('#grdSolicitudPago').getGridParam('sortname');
    SolicitudPago.pTipoOrden = $('#grdSolicitudPago').getGridParam('sortorder');

    var pRequest = JSON.stringify(SolicitudPago);

    $.ajax({
        url: 'SolicitudPago.aspx/ObtenerSolicitudPago',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function (Respuesta) {
            var json = Respuesta.d;
            $('#grdSolicitudPago')[0].addJSONData(json);
        },
        complete: function () {

        }
    });

}

function ObtenerFormaAgregarSolicitudPago() {

    var ventana = $('<div id="divAgregarSolicitudPago"></div>');

    $(ventana).dialog({
        modal: true,
        draggable: false,
        resizable: false,
        title: "Agregar solicitud de pago",
        close: function () {
            $(this).remove();
        },
        buttons: {
            "Guardar": function () { },
            "Cancelar": function () { $(this).dialog("close"); }
        }
    });

    $(ventana).obtenerVista({
        nombreTemplate: "htmlAgregarSolicitudPago.html",
        despuesDeCompilar: function () {

            $("#txtFechaRequerida").datepicker();
            
            $("#txtProveedor").autocomplete({
                source: function (request, response) {
                    var pRequest = new Object();
                    pRequest.Proveedor = $("#txtProveedor").val();
                    $.ajax({
                        type: 'POST',
                        url: 'SolicitudPago.aspx/BuscarProveedor',
                        data: JSON.stringify(pRequest),
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (pRespuesta) {
                            var json = jQuery.parseJSON(pRespuesta.d);
                            response($.map(json.Modelo.Table, function (item) {
                                return { label: item.Proveedor, value: item.Proveedor, id: item.IdProveedor }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var IdProveedor = ui.item.id;
                    $(this).attr("IdProveedor", IdProveedor);
                },
                change: function (event, ui) { },
                open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
            });

        }
    });

}

function GuardarSolicitudPago() {
    var SolicitudPago = new Object();
    SolicitudPago.IdProveedor = parseInt($("#txtProveedor").attr("IdProveedro"))
    SolicitudPago.Monto = parseFloat(QuitarFormatoNumero($("#txtMonto").val()));
    SolicitudPago.FechaPago = $("#txtFechaRequerida").val();

    var Request = JSON.stringify(SolicitudPago);

    $.ajax({
        url: "SolicitudPago.aspx/GuardarSolicitudPago",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json;charset",
        success: function (Respuesta) {

        }
    });
}