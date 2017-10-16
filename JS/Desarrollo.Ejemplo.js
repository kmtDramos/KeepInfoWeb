//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(".divAreaBotonesDialog").on("click", "#btnObtenerResultadosOracle", function() {
        ObtenerFormaObtenerResultadosOracle();
    });

    $(".divAreaBotonesDialog").on("click", "#btnCortaCadenas", function() {
        CortarPalabra();
    });
});

function CortarPalabra() {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Ejemplo.aspx/CortarPalabra",
        //data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
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


function ObtenerFormaObtenerResultadosOracle() {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Ejemplo.aspx/ObtenerResultadosOracle",
        //data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
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
