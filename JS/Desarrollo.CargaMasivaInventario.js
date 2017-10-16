//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $("#divPanelIntegrador").tabs();
    ObtenerFormaAgregarCargaMasivaInventario();
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCargaMasivaInventario() {
    $("#divAgregarCargaMasivaInventario").obtenerVista({
        nombreTemplate: "tmplAgregarCargaMasivaInventario.html",
        //url: "CargaMasivaInventario.aspx/ObtenerFormaAgregarCargaMasivaInventario",
        despuesDeCompilar: function(pRespuesta) {
            $("#txtFechaFactura").datepicker();
            $("#txtFechaPago").datepicker();
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCargaMasivaInventario() {
    $(this).dialog("close");
}

//-----Validaciones----------