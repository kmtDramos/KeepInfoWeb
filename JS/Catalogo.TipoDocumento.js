//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoDocumento");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoDocumento", function() {
        ObtenerFormaAgregarTipoDocumento();
    });

    $("#grdTipoDocumento").on("click", ".imgFormaConsultarTipoDocumento", function() {
        var registro = $(this).parents("tr");
        var TipoDocumento = new Object();
        TipoDocumento.pIdTipoDocumento = parseInt($(registro).children("td[aria-describedby='grdTipoDocumento_IdTipoDocumento']").html());
        ObtenerFormaConsultarTipoDocumento(JSON.stringify(TipoDocumento));
    });

    $('#grdTipoDocumento').one('click', '.div_grdTipoDocumento_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoDocumento_AI']").children().attr("baja")
        var idTipoDocumento = $(registro).children("td[aria-describedby='grdTipoDocumento_IdTipoDocumento']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoDocumento, baja);
    });

    $('#dialogAgregarTipoDocumento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoDocumento").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoDocumento();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoDocumento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoDocumento").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoDocumento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoDocumento").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoDocumento();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoDocumento()
{
    $("#dialogAgregarTipoDocumento").obtenerVista({
        nombreTemplate: "tmplAgregarTipoDocumento.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoDocumento").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoDocumento(pIdTipoDocumento) {
    $("#dialogConsultarTipoDocumento").obtenerVista({
        nombreTemplate: "tmplConsultarTipoDocumento.html",
        url: "TipoDocumento.aspx/ObtenerFormaTipoDocumento",
        parametros: pIdTipoDocumento,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoDocumento == 1) {
                $("#dialogConsultarTipoDocumento").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoDocumento = new Object();
                        TipoDocumento.IdTipoDocumento = parseInt($("#divFormaConsultarTipoDocumento").attr("IdTipoDocumento"));
                        ObtenerFormaEditarTipoDocumento(JSON.stringify(TipoDocumento))
                    }
                });
                $("#dialogConsultarTipoDocumento").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoDocumento").dialog("option", "buttons", {});
                $("#dialogConsultarTipoDocumento").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoDocumento").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoDocumento(IdTipoDocumento) {
    $("#dialogEditarTipoDocumento").obtenerVista({
        nombreTemplate: "tmplEditarTipoDocumento.html",
        url: "TipoDocumento.aspx/ObtenerFormaEditarTipoDocumento",
        parametros: IdTipoDocumento,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoDocumento").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoDocumento() {
    var pTipoDocumento = new Object();
    pTipoDocumento.TipoDocumento = $("#txtTipoDocumento").val();
    pTipoDocumento.Comando = $("#txtComando").val();
    var validacion = ValidaTipoDocumento(pTipoDocumento);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoDocumento = pTipoDocumento;
    SetAgregarTipoDocumento(JSON.stringify(oRequest)); 
}

function SetAgregarTipoDocumento(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoDocumento.aspx/AgregarTipoDocumento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoDocumento").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoDocumento").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoDocumento, pBaja) {
    var pRequest = "{'pIdTipoDocumento':" + pIdTipoDocumento + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoDocumento.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoDocumento").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoDocumento').one('click', '.div_grdTipoDocumento_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoDocumento_AI']").children().attr("baja")
                var idTipoDocumento = $(registro).children("td[aria-describedby='grdTipoDocumento_IdTipoDocumento']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoDocumento, baja);
            });
        }
    });
}

function EditarTipoDocumento() {
    var pTipoDocumento = new Object();
    pTipoDocumento.IdTipoDocumento = $("#divFormaEditarTipoDocumento").attr("idTipoDocumento");
    pTipoDocumento.TipoDocumento = $("#txtTipoDocumento").val();
    pTipoDocumento.Comando = $("#txtComando").val();
    var validacion = ValidaTipoDocumento(pTipoDocumento);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoDocumento = pTipoDocumento;
    SetEditarTipoDocumento(JSON.stringify(oRequest));
}

function SetEditarTipoDocumento(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoDocumento.aspx/EditarTipoDocumento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoDocumento").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoDocumento").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoDocumento(pTipoDocumento) {
    var errores = "";

    if (pTipoDocumento.TipoDocumento == "")
    { errores = errores + "<span>*</span> El campo tipo documento esta vacío, favor de capturarlo.<br />"; }
    
    if (pTipoDocumento.Comando == "")
    { errores = errores + "<span>*</span> El campo Comando esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}