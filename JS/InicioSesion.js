//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    $("#ctl00_bodyMasterPageSeguridad_txtUsuario").focus();
});

//---------Funciones--------//
//--------------------------//
function IniciarSesion()
{
    MostrarBloqueoRaiz();
    var usuario = $("#ctl00_bodyMasterPageSeguridad_txtUsuario").val();
    var contrasena = $("#ctl00_bodyMasterPageSeguridad_txtContrasena").val();
    var validacion = ValidaUsuario(usuario,contrasena);
    if(validacion != "")
    {
        OcultarBloqueo();
        MostrarMensajeError(validacion);
        $("#dialogMensajeError").dialog("open");
    }
    else 
    {
        contrasena = CryptoJS.MD5(contrasena);
        SetIniciarSesion(usuario,contrasena);
    }
}

function IniciarSesionEnter(evento)
{
    var key = evento.which || evento.keyCode;
    if ((key == 13)){
        IniciarSesion();
        return false; 
    }
}

//-------Validaciones-------//
//--------------------------//
function ValidaUsuario(pUsuario,pContrasena)
{
    var errores = "";

    if (pUsuario == "")
    { errores = errores + "<span>*</span> El campo usuario esta vac&iacute;o, favor de capturarlo.<br />"; }
    if (pContrasena == "")
    { errores = errores + "<span>*</span> El campo contrase&ntilde;a esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }
    return errores;
}

//-----------AJAX-----------//
//--------------------------//
function SetIniciarSesion(pUsuario, pContrasena)
{   
    var pRequest = "{'pUsuario':'"+pUsuario+"','pContrasena':'"+pContrasena+"'}";
    $.ajax({
        type: "POST",
        url: "InicioSesion.aspx/IniciarSesion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = pRespuesta.d.split("|");            
            
            if (respuesta[0] == 1) {
                OcultarBloqueo();
                MostrarMensajeError(respuesta[1]);
                $("#dialogMensajeError").dialog("open");
            }
            else if (respuesta[0] == 2) {
                $("#divInicioSesion").attr("idUsuario", respuesta[1]);
                $("#dialogCambiarContrasena").dialog("open");
            }
            else {
                location.replace(respuesta[1]);
            }
        }
    });
}