//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    Inicializar_grdDevolucion();
    //Inicializar_grdAlmacenProductoDetalle();

    $("#grdDevolucion").on("click", ".imgGenerarDevolucion", function() {
        var registro = $(this).parents("tr");
        var IdDevolucion = parseInt($(registro).children("td[aria-describedby='grdDevolucion_IdDevolucion']").html());
        var pDevolucion = new Object();
        pDevolucion.pIdDevolucion = IdDevolucion;
        var oRequest = new Object();
        oRequest.pDevolucion = pDevolucion;
        //SetGenerarDevolucion(JSON.stringify(oRequest));
   

        $("#dialogConfirmaAccion").dialog("option", "buttons", {
            "Aceptar": function() {
            SetGenerarDevolucion(JSON.stringify(oRequest));
                $(this).dialog("close")
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        });
        $("#textoConfirmacion").html("¿Está seguro que desea realizar la devolución?");
        $("#dialogConfirmaAccion").dialog("open");
        
    });

    function SetGenerarDevolucion(pRequest) {
        $.ajax({
            type: "POST",
            url: "Devolucion.aspx/GenerarDevolucion",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function(pRespuesta) {
                respuesta = jQuery.parseJSON(pRespuesta.d);
                if (respuesta.Error == 0) {
                    $("#grdDevolucion").trigger("reloadGrid");
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
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });




});

