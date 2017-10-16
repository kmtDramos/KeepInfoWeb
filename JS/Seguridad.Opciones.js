//----------DHTMLX----------//
//--------------------------//
function tonclick(id) {
    var idOpcion = tree.getSelectedItemId();
    if (idOpcion != "Opciones")
    {
        SetFormaConsultarOpcion(idOpcion)
    }
};

//----------JQuery----------//
//--------------------------//
$(document).ready(function(){
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerJsonArbolOpciones();
    SetFormaAltaOpcion();

    //Dialogo Eliminar
    $('#dialogMensajeEliminar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Eliminar": function() {
                var idOpcion = $("#divFormulario").attr("idOpcion");
                SetEliminarOpcion(idOpcion);
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
});

//---------Funciones--------//
//--------------------------//
function AgregarOpcion()
{
    var opcion = $("#txtOpcion").val();
    var comando = $("#txtComando").val();
    var validacion = ValidaOpcion(opcion,comando);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetAgregarOpcion(opcion,comando);
}

function EditarOpcion()
{
    var idOpcion = $("#divFormulario").attr("idOpcion");
    var opcion = $("#txtOpcion").val();
    var comando = $("#txtComando").val();
    var validacion = ValidaOpcion(opcion,comando);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetEditarOpcion(idOpcion,opcion,comando);
}

function EliminarOpcion()
{
    var opcion = $("#divFormulario").attr("opcion");
    MostrarMensajeEliminar("Esta seguro de eliminar la opcion: "+opcion);
}

//--------Validaciones-------//
//--------------------------//
function ValidaOpcion(pOpcion,pComando)
{
    var errores = "";
    if(pOpcion == "")
    {errores=errores+"<span>*</span> El campo opcion esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pComando == "")
    {errores=errores+"<span>*</span> El campo comando esta vac&iacute;o, favor de capturarlo.<br />";}
    
    if(errores != "")
    {errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores;}
    
    return errores;
}

//-----------AJAX-----------//
//--------------------------//
function ObtenerJsonArbolOpciones() 
{
    $.ajax({
        type: "POST",
        url: "Opciones.aspx/ObtenerJsonArbolOpciones",
        data: "",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var json = jQuery.parseJSON(pRespuesta.d);
            IniciarArbol(json.Modelo);
        }
    });
}

//-----------AJAX------------//
//----Funciones de Accion----//
function SetAgregarOpcion(pOpcion, pComando)
{   
    var pRequest = "{'pOpcion':'"+pOpcion+"','pComando':'"+pComando+"'}";
    $.ajax({
        type: "POST",
        url: "Opciones.aspx/AgregarOpcion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == "1")
            { MostrarMensajeError(respuesta[1]); }
            else 
            {
                $("#txtOpcion").val("");
                $("#txtComando").val("");
                var json = jQuery.parseJSON(respuesta[1]);
                IniciarArbol(json.Modelo);
            }
        }
    });
}

function SetEditarOpcion(pIdOpcion,pOpcion,pComando)
{
    var pRequest = "{'pIdOpcion':"+pIdOpcion+",'pOpcion':'"+pOpcion+"','pComando':'"+pComando+"'}";
    $.ajax({
        type: "POST",
        url: "Opciones.aspx/EditarOpcion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            var respuesta = pRespuesta.d.split("|");
            if(respuesta[0] == 0)
            {
                var json = jQuery.parseJSON(respuesta[1]);
                IniciarArbol(json.Modelo);
                SetFormaAltaOpcion();
            }
            else
            {
                MostrarMensajeError(respuesta[1]);
            }  
        }
    });
}

function SetEliminarOpcion(pIdOpcion)
{
    var pRequest = "{'pIdOpcion':"+pIdOpcion+"}";
    $.ajax({
        type: "POST",
        url: "Opciones.aspx/EliminarOpcion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            var respuesta = pRespuesta.d.split("|");
            var json = jQuery.parseJSON(respuesta[1]);
            IniciarArbol(json.Modelo);
            SetFormaAltaOpcion();
            $("#dialogMensajeEliminar").dialog("close");
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaAltaOpcion()
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaOpcion.aspx",
        data: {},
        dataType: "html",
        success: function(pRespuesta){
            if (!$("#divFormulario").get(0)){
				$('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
			}
			else{
                $('#divFormulario').slideUp('slow', function() {
                    $('#formPrincipal').remove();
                    $('#col-2').append(pRespuesta);
                    $("#divFormulario").slideDown('slow');
                });
            }
        }
    });
}

function SetFormaConsultarOpcion(pIdOpcion)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarOpcion.aspx",
        data: {
            IdOpcion: pIdOpcion
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').slideUp('slow', function() {
                $('#formPrincipal').remove();
                $('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
            });
        }
    });
}

function SetFormaEditarOpcion(pIdOpcion)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaEditarOpcion.aspx",
        data: {
            IdOpcion: pIdOpcion
        },
        dataType: "html",
        success: function(pRespuesta){
            $('#divFormulario').slideUp('slow', function() {
                $('#formPrincipal').remove();
                $('#col-2').append(pRespuesta);
                $("#divFormulario").slideDown('slow');
            });
        }
    });
}