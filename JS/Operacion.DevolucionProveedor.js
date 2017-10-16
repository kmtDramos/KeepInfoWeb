//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    Inicializar_grdDevolucionProveedor();
    //Inicializar_grdAlmacenProductoDetalle();

    $("#grdDevolucionProveedor").on("click", ".imgGenerarDevolucionProveedor", function() {
        var registro = $(this).parents("tr");
        var IdDevolucionProveedor = parseInt($(registro).children("td[aria-describedby='grdDevolucionProveedor_IdDevolucionProveedor']").html());
        var pDevolucionProveedor = new Object();
        pDevolucionProveedor.pIdDevolucionProveedor = IdDevolucionProveedor;
        var oRequest = new Object();
        oRequest.pDevolucionProveedor = pDevolucionProveedor;
        //SetGenerarDevolucionProveedor(JSON.stringify(oRequest));
   

        $("#dialogConfirmaAccion").dialog("option", "buttons", {
            "Aceptar": function() {
                SetGenerarDevolucionProveedor(JSON.stringify(oRequest));
                $(this).dialog("close")
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        });
        $("#textoConfirmacion").html("¿Está seguro que desea realizar la devolución?");
        $("#dialogConfirmaAccion").dialog("open");
       
    });

    function SetGenerarDevolucionProveedor(pRequest) {
        $.ajax({
            type: "POST",
            url: "DevolucionProveedor.aspx/GenerarDevolucionProveedor",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#grdDevolucionProveedor").trigger("reloadGrid");
                }
                else if (respuesta.Descripcion == "valida") {

                }
                else {
                    MostrarMensajeError(respuesta.Descripcion);
                    return false;
                }
            },
            complete: function() {
            }
        });
    }

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
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

});

