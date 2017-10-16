//----------DHTMLX----------//
//--------------------------//
function tonclick(id) {
    var idPerfil = tree.getSelectedItemId();
    if (idPerfil != "Perfiles")
    {
        SetFormaConsultarPerfil(idPerfil)
    }
};

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos   
    ObtenerJsonArbolPerfiles();
    SetFormaAltaPerfil();

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
                var idPerfil = $("#divFormulario").attr("idPerfil");
                SetEliminarPerfil(idPerfil);
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
});

//---------Funciones--------//
//--------------------------//
function AgregarPerfil()
{
    var perfil = $("#txtPerfil").val();
    var idPagina = $("#cmbPagina").val();
    if($("#chkEsPerfilSucursal").is(':checked'))
	{var esPerfilSucursal = "true";}
	else
	{var esPerfilSucursal = "false";}
    var validacion = ValidaPerfil(perfil,idPagina);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetAgregarPerfil(perfil,idPagina,esPerfilSucursal);
}

function EditarPerfil()
{
    var idPerfil = $("#divFormulario").attr("idperfil");
    var perfil = $("#txtPerfil").val();
    var idPagina = $("#cmbPagina").val();
    if($("#chkEsPerfilSucursal").is(':checked'))
	{var esPerfilSucursal = "true";}
	else
	{var esPerfilSucursal = "false";}
    var validacion = ValidaPerfil(perfil,idPagina);
    if(validacion != "")
    {MostrarMensajeError(validacion);return false;}
    SetEditarPerfil(idPerfil,perfil,idPagina,esPerfilSucursal);
}

function EliminarPerfil()
{
    var perfil = $("#divFormulario").attr("perfil");
    MostrarMensajeEliminar("Esta seguro de eliminar el perfil: "+perfil);
}

//--------Validaciones-------//
//--------------------------//
function ValidaPerfil(pPerfil,pIdPagina)
{
    var errores = "";
    if(pPerfil == "")
    {errores=errores+"<span>*</span> El campo perfil esta vac&iacute;o, favor de capturarlo.<br />";}
    if(pIdPagina == "0")
    {errores=errores+"<span>*</span> Es necesario eligir una pagina, favor de capturarla.<br />";}
    
    if(errores != "")
    {errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores;}
    
    return errores;
}

//-----------AJAX-----------//
//--------------------------//
function ObtenerJsonArbolPerfiles() 
{
    $.ajax({
        type: "POST",
        url: "Perfiles.aspx/ObtenerJsonArbolPerfiles",
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
function SetAgregarPerfil(pPerfil, pIdPagina, pEsPerfilSucursal)
{   
    var pRequest = "{'pPerfil':'"+pPerfil+"','pIdPagina':'"+pIdPagina+"','pEsPerfilSucursal':"+pEsPerfilSucursal+"}";
    $.ajax({
        type: "POST",
        url: "Perfiles.aspx/AgregarPerfil",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta){
            $("#txtPerfil").val("");
            $("#cmbPagina").val("0");
            $("#chkEsPerfilSucursal").attr('checked', false);
            var respuesta = pRespuesta.d.split("|");
            var json = jQuery.parseJSON(respuesta[1]);
            IniciarArbol(json.Modelo);
        }
    });
}

function SetEditarPerfil(pIdPerfil,pPerfil,pIdPagina,pEsPerfilSucursal)
{
    var pRequest = "{'pIdPerfil':"+pIdPerfil+",'pPerfil':'"+pPerfil+"','pIdPagina':"+pIdPagina+",'pEsPerfilSucursal':"+pEsPerfilSucursal+"}";
    $.ajax({
        type: "POST",
        url: "Perfiles.aspx/EditarPerfil",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = pRespuesta.d.split("|");
            if (respuesta[0] == 0) 
            {
                var json = jQuery.parseJSON(respuesta[1]);
                IniciarArbol(json.Modelo);
                SetFormaAltaPerfil();
            }
            else {
                MostrarMensajeError(respuesta[1]);
            }
        }
    });
}

function SetEliminarPerfil(pIdPerfil)
{
    var pRequest = "{'pIdPerfil':"+pIdPerfil+"}";
    $.ajax({
        type: "POST",
        url: "Perfiles.aspx/EliminarPerfil",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = pRespuesta.d.split("|");
            var json = jQuery.parseJSON(respuesta[1]);
            IniciarArbol(json);
            SetFormaAltaPerfil();
            $("#dialogMensajeEliminar").dialog("close");
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function SetFormaAltaPerfil()
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaAltaPerfil.aspx",
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

function SetFormaConsultarPerfil(pIdPerfil)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaConsultarPerfil.aspx",
        data: {
            IdPerfil: pIdPerfil
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

function SetFormaEditarPerfil(pIdPerfil)
{   
    $.ajax({
        type: "POST",
        url: "../Formas/FormaEditarPerfil.aspx",
        data: {
            IdPerfil: pIdPerfil
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