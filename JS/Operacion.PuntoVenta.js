//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaBotonesPuntoVenta();
});


//-----------AJAX-----------//
//-Funciones de Acciones-//


//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaBotonesPuntoVenta() {
    $("#divAreaBotonesPuntoVenta").obtenerVista({
        nombreTemplate: "tmplBotonesPuntoVenta.html",
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

//----------Validaciones----------//
//--------------------------//
