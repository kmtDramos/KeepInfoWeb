//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("UnidadCaracteristica");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarUnidadCaracteristica", function() {
        ObtenerFormaAgregarUnidadCaracteristica();
    });
    
    $("#grdUnidadCaracteristica").on("click", ".imgFormaConsultarUnidadCaracteristica", function() {
        var registro = $(this).parents("tr");
        var UnidadCaracteristica = new Object();
        UnidadCaracteristica.pIdUnidadCaracteristica = parseInt($(registro).children("td[aria-describedby='grdUnidadCaracteristica_IdUnidadCaracteristica']").html());
        ObtenerFormaConsultarUnidadCaracteristica(JSON.stringify(UnidadCaracteristica));
    });
    
    $('#grdUnidadCaracteristica').one('click', '.div_grdUnidadCaracteristica_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdUnidadCaracteristica_AI']").children().attr("baja")
        var idUnidadCaracteristica = $(registro).children("td[aria-describedby='grdUnidadCaracteristica_IdUnidadCaracteristica']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idUnidadCaracteristica, baja);
    });
    
    $('#dialogAgregarUnidadCaracteristica').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarUnidadCaracteristica").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarUnidadCaracteristica();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarUnidadCaracteristica').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarUnidadCaracteristica").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarUnidadCaracteristica').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarUnidadCaracteristica").remove();
        },
        buttons: {
            "Editar": function() {
                EditarUnidadCaracteristica();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarUnidadCaracteristica()
{
    $("#dialogAgregarUnidadCaracteristica").obtenerVista({
        nombreTemplate: "tmplAgregarUnidadCaracteristica.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarUnidadCaracteristica").dialog("open");
        }
    });
}

function ObtenerFormaConsultarUnidadCaracteristica(pIdUnidadCaracteristica) {
    $("#dialogConsultarUnidadCaracteristica").obtenerVista({
        nombreTemplate: "tmplConsultarUnidadCaracteristica.html",
        url: "UnidadCaracteristica.aspx/ObtenerFormaUnidadCaracteristica",
        parametros: pIdUnidadCaracteristica,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarUnidadCaracteristica == 1) {
                $("#dialogConsultarUnidadCaracteristica").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var UnidadCaracteristica = new Object();
                        UnidadCaracteristica.IdUnidadCaracteristica = parseInt($("#divFormaConsultarUnidadCaracteristica").attr("IdUnidadCaracteristica"));
                        ObtenerFormaEditarUnidadCaracteristica(JSON.stringify(UnidadCaracteristica))
                    }
                });
                $("#dialogConsultarUnidadCaracteristica").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarUnidadCaracteristica").dialog("option", "buttons", {});
                $("#dialogConsultarUnidadCaracteristica").dialog("option", "height", "100");
            }
            $("#dialogConsultarUnidadCaracteristica").dialog("open");
        }
    });
}

function ObtenerFormaEditarUnidadCaracteristica(IdUnidadCaracteristica) {
    $("#dialogEditarUnidadCaracteristica").obtenerVista({
        nombreTemplate: "tmplEditarUnidadCaracteristica.html",
        url: "UnidadCaracteristica.aspx/ObtenerFormaEditarUnidadCaracteristica",
        parametros: IdUnidadCaracteristica,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarUnidadCaracteristica").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarUnidadCaracteristica() {
    var pUnidadCaracteristica = new Object();
    pUnidadCaracteristica.UnidadCaracteristica = $("#txtUnidadCaracteristica").val();
    var validacion = ValidaUnidadCaracteristica(pUnidadCaracteristica);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pUnidadCaracteristica = pUnidadCaracteristica;
    SetAgregarUnidadCaracteristica(JSON.stringify(oRequest)); 
}

function SetAgregarUnidadCaracteristica(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "UnidadCaracteristica.aspx/AgregarUnidadCaracteristica",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdUnidadCaracteristica").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarUnidadCaracteristica").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdUnidadCaracteristica, pBaja) {
    var pRequest = "{'pIdUnidadCaracteristica':" + pIdUnidadCaracteristica + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "UnidadCaracteristica.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdUnidadCaracteristica").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdUnidadCaracteristica').one('click', '.div_grdUnidadCaracteristica_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdUnidadCaracteristica_AI']").children().attr("baja")
                var idUnidadCaracteristica = $(registro).children("td[aria-describedby='grdUnidadCaracteristica_IdUnidadCaracteristica']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idUnidadCaracteristica, baja);
            });
        }
    });
}

function EditarUnidadCaracteristica() {
    var pUnidadCaracteristica = new Object();
    pUnidadCaracteristica.IdUnidadCaracteristica = $("#divFormaEditarUnidadCaracteristica").attr("idUnidadCaracteristica");
    pUnidadCaracteristica.UnidadCaracteristica = $("#txtUnidadCaracteristica").val();
    var validacion = ValidaUnidadCaracteristica(pUnidadCaracteristica);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pUnidadCaracteristica = pUnidadCaracteristica;
    SetEditarUnidadCaracteristica(JSON.stringify(oRequest));
}

function SetEditarUnidadCaracteristica(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "UnidadCaracteristica.aspx/EditarUnidadCaracteristica",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdUnidadCaracteristica").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarUnidadCaracteristica").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaUnidadCaracteristica(pUnidadCaracteristica) {
    var errores = "";

    if (pUnidadCaracteristica.UnidadCaracteristica == "")
    { errores = errores + "<span>*</span> El campo UnidadCaracteristica esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
