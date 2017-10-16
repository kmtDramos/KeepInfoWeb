//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoIndustria");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoIndustria", function() {
        ObtenerFormaAgregarTipoIndustria();
    });

    $("#grdTipoIndustria").on("click", ".imgFormaConsultarTipoIndustria", function() {
        var registro = $(this).parents("tr");
        var TipoIndustria = new Object();
        TipoIndustria.pIdTipoIndustria = parseInt($(registro).children("td[aria-describedby='grdTipoIndustria_IdTipoIndustria']").html());
        ObtenerFormaConsultarTipoIndustria(JSON.stringify(TipoIndustria));
    });

    $('#grdTipoIndustria').one('click', '.div_grdTipoIndustria_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoIndustria_AI']").children().attr("baja")
        var idTipoIndustria = $(registro).children("td[aria-describedby='grdTipoIndustria_IdTipoIndustria']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoIndustria, baja);
    });

    $('#dialogAgregarTipoIndustria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoIndustria").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoIndustria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoIndustria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoIndustria").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoIndustria').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoIndustria").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoIndustria();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoIndustria()
{
    $("#dialogAgregarTipoIndustria").obtenerVista({
        nombreTemplate: "tmplAgregarTipoIndustria.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoIndustria").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoIndustria(pIdTipoIndustria) {
    $("#dialogConsultarTipoIndustria").obtenerVista({
        nombreTemplate: "tmplConsultarTipoIndustria.html",
        url: "TipoIndustria.aspx/ObtenerFormaTipoIndustria",
        parametros: pIdTipoIndustria,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoIndustria == 1) {
                $("#dialogConsultarTipoIndustria").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoIndustria = new Object();
                        TipoIndustria.IdTipoIndustria = parseInt($("#divFormaConsultarTipoIndustria").attr("IdTipoIndustria"));
                        ObtenerFormaEditarTipoIndustria(JSON.stringify(TipoIndustria))
                    }
                });
                $("#dialogConsultarTipoIndustria").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoIndustria").dialog("option", "buttons", {});
                $("#dialogConsultarTipoIndustria").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoIndustria").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoIndustria(IdTipoIndustria) {
    $("#dialogEditarTipoIndustria").obtenerVista({
        nombreTemplate: "tmplEditarTipoIndustria.html",
        url: "TipoIndustria.aspx/ObtenerFormaEditarTipoIndustria",
        parametros: IdTipoIndustria,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoIndustria").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoIndustria() {
    var pTipoIndustria = new Object();
    pTipoIndustria.TipoIndustria = $("#txtTipoIndustria").val();
    var validacion = ValidaTipoIndustria(pTipoIndustria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoIndustria = pTipoIndustria;
    SetAgregarTipoIndustria(JSON.stringify(oRequest)); 
}

function SetAgregarTipoIndustria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoIndustria.aspx/AgregarTipoIndustria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoIndustria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoIndustria").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoIndustria, pBaja) {
    var pRequest = "{'pIdTipoIndustria':" + pIdTipoIndustria + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoIndustria.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoIndustria").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoIndustria').one('click', '.div_grdTipoIndustria_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoIndustria_AI']").children().attr("baja")
                var idTipoIndustria = $(registro).children("td[aria-describedby='grdTipoIndustria_IdTipoIndustria']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoIndustria, baja);
            });
        }
    });
}

function EditarTipoIndustria() {
    var pTipoIndustria = new Object();
    pTipoIndustria.IdTipoIndustria = $("#divFormaEditarTipoIndustria").attr("idTipoIndustria");
    pTipoIndustria.TipoIndustria = $("#txtTipoIndustria").val();
    var validacion = ValidaTipoIndustria(pTipoIndustria);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoIndustria = pTipoIndustria;
    SetEditarTipoIndustria(JSON.stringify(oRequest));
}

function SetEditarTipoIndustria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoIndustria.aspx/EditarTipoIndustria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoIndustria").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoIndustria").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoIndustria(pTipoIndustria) {
    var errores = "";

    if (pTipoIndustria.TipoIndustria == "")
    { errores = errores + "<span>*</span> El campo TipoIndustria esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}