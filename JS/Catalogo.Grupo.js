//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Grupo");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarGrupo", function() {
        ObtenerFormaAgregarGrupo();
    });
    
    $("#grdGrupo").on("click", ".imgFormaConsultarGrupo", function() {
        var registro = $(this).parents("tr");
        var Grupo = new Object();
        Grupo.pIdGrupo = parseInt($(registro).children("td[aria-describedby='grdGrupo_IdGrupo']").html());
        ObtenerFormaConsultarGrupo(JSON.stringify(Grupo));
    });
    
    $('#grdGrupo').one('click', '.div_grdGrupo_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdGrupo_AI']").children().attr("baja")
        var idGrupo = $(registro).children("td[aria-describedby='grdGrupo_IdGrupo']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idGrupo, baja);
    });
    
    $('#dialogAgregarGrupo').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarGrupo").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarGrupo();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarGrupo').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarGrupo").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarGrupo').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarGrupo").remove();
        },
        buttons: {
            "Editar": function() {
                EditarGrupo();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarGrupo()
{
    $("#dialogAgregarGrupo").obtenerVista({
        nombreTemplate: "tmplAgregarGrupo.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarGrupo").dialog("open");
        }
    });
}

function ObtenerFormaConsultarGrupo(pIdGrupo) {
    $("#dialogConsultarGrupo").obtenerVista({
        nombreTemplate: "tmplConsultarGrupo.html",
        url: "Grupo.aspx/ObtenerFormaGrupo",
        parametros: pIdGrupo,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarGrupo == 1) {
                $("#dialogConsultarGrupo").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Grupo = new Object();
                        Grupo.IdGrupo = parseInt($("#divFormaConsultarGrupo").attr("IdGrupo"));
                        ObtenerFormaEditarGrupo(JSON.stringify(Grupo))
                    }
                });
                $("#dialogConsultarGrupo").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarGrupo").dialog("option", "buttons", {});
                $("#dialogConsultarGrupo").dialog("option", "height", "100");
            }
            $("#dialogConsultarGrupo").dialog("open");
        }
    });
}

function ObtenerFormaEditarGrupo(IdGrupo) {
    $("#dialogEditarGrupo").obtenerVista({
        nombreTemplate: "tmplEditarGrupo.html",
        url: "Grupo.aspx/ObtenerFormaEditarGrupo",
        parametros: IdGrupo,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarGrupo").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarGrupo() {
    var pGrupo = new Object();
    pGrupo.Grupo = $("#txtGrupo").val();
    var validacion = ValidaGrupo(pGrupo);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pGrupo = pGrupo;
    SetAgregarGrupo(JSON.stringify(oRequest)); 
}

function SetAgregarGrupo(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Grupo.aspx/AgregarGrupo",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdGrupo").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarGrupo").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdGrupo, pBaja) {
    var pRequest = "{'pIdGrupo':" + pIdGrupo + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Grupo.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdGrupo").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdGrupo').one('click', '.div_grdGrupo_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdGrupo_AI']").children().attr("baja")
                var idGrupo = $(registro).children("td[aria-describedby='grdGrupo_IdGrupo']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idGrupo, baja);
            });
        }
    });
}

function EditarGrupo() {
    var pGrupo = new Object();
    pGrupo.IdGrupo = $("#divFormaEditarGrupo").attr("idGrupo");
    pGrupo.Grupo = $("#txtGrupo").val();
    var validacion = ValidaGrupo(pGrupo);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pGrupo = pGrupo;
    SetEditarGrupo(JSON.stringify(oRequest));
}

function SetEditarGrupo(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Grupo.aspx/EditarGrupo",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdGrupo").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarGrupo").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaGrupo(pGrupo) {
    var errores = "";

    if (pGrupo.Grupo == "")
    { errores = errores + "<span>*</span> El campo Grupo esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
