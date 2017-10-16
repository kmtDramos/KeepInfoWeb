//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TiempoEntrega");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTiempoEntrega", function() {
        ObtenerFormaAgregarTiempoEntrega();
    });

    $("#grdTiempoEntrega").on("click", ".imgFormaConsultarTiempoEntrega", function() {
        var registro = $(this).parents("tr");
        var TiempoEntrega = new Object();
        TiempoEntrega.pIdTiempoEntrega = parseInt($(registro).children("td[aria-describedby='grdTiempoEntrega_IdTiempoEntrega']").html());
        ObtenerFormaConsultarTiempoEntrega(JSON.stringify(TiempoEntrega));
    });

    $('#grdTiempoEntrega').one('click', '.div_grdTiempoEntrega_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTiempoEntrega_AI']").children().attr("baja")
        var idTiempoEntrega = $(registro).children("td[aria-describedby='grdTiempoEntrega_IdTiempoEntrega']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTiempoEntrega, baja);
    });

    $('#dialogAgregarTiempoEntrega').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTiempoEntrega").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTiempoEntrega();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTiempoEntrega').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTiempoEntrega").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTiempoEntrega').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTiempoEntrega").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTiempoEntrega();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTiempoEntrega()
{
    $("#dialogAgregarTiempoEntrega").obtenerVista({
        nombreTemplate: "tmplAgregarTiempoEntrega.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTiempoEntrega").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTiempoEntrega(pIdTiempoEntrega) {
    $("#dialogConsultarTiempoEntrega").obtenerVista({
        nombreTemplate: "tmplConsultarTiempoEntrega.html",
        url: "TiempoEntrega.aspx/ObtenerFormaTiempoEntrega",
        parametros: pIdTiempoEntrega,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTiempoEntrega == 1) {
                $("#dialogConsultarTiempoEntrega").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TiempoEntrega = new Object();
                        TiempoEntrega.IdTiempoEntrega = parseInt($("#divFormaConsultarTiempoEntrega").attr("IdTiempoEntrega"));
                        ObtenerFormaEditarTiempoEntrega(JSON.stringify(TiempoEntrega))
                    }
                });
                $("#dialogConsultarTiempoEntrega").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTiempoEntrega").dialog("option", "buttons", {});
                $("#dialogConsultarTiempoEntrega").dialog("option", "height", "100");
            }
            $("#dialogConsultarTiempoEntrega").dialog("open");
        }
    });
}

function ObtenerFormaEditarTiempoEntrega(IdTiempoEntrega) {
    $("#dialogEditarTiempoEntrega").obtenerVista({
        nombreTemplate: "tmplEditarTiempoEntrega.html",
        url: "TiempoEntrega.aspx/ObtenerFormaEditarTiempoEntrega",
        parametros: IdTiempoEntrega,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTiempoEntrega").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTiempoEntrega() {
    var pTiempoEntrega = new Object();
    pTiempoEntrega.TiempoEntrega = $("#txtTiempoEntrega").val();
    var validacion = ValidaTiempoEntrega(pTiempoEntrega);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTiempoEntrega = pTiempoEntrega;
    SetAgregarTiempoEntrega(JSON.stringify(oRequest)); 
}

function SetAgregarTiempoEntrega(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TiempoEntrega.aspx/AgregarTiempoEntrega",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTiempoEntrega").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTiempoEntrega").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTiempoEntrega, pBaja) {
    var pRequest = "{'pIdTiempoEntrega':" + pIdTiempoEntrega + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TiempoEntrega.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTiempoEntrega").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTiempoEntrega').one('click', '.div_grdTiempoEntrega_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTiempoEntrega_AI']").children().attr("baja")
                var idTiempoEntrega = $(registro).children("td[aria-describedby='grdTiempoEntrega_IdTiempoEntrega']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTiempoEntrega, baja);
            });
        }
    });
}

function EditarTiempoEntrega() {
    var pTiempoEntrega = new Object();
    pTiempoEntrega.IdTiempoEntrega = $("#divFormaEditarTiempoEntrega").attr("idTiempoEntrega");
    pTiempoEntrega.TiempoEntrega = $("#txtTiempoEntrega").val();
    var validacion = ValidaTiempoEntrega(pTiempoEntrega);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTiempoEntrega = pTiempoEntrega;
    SetEditarTiempoEntrega(JSON.stringify(oRequest));
}

function SetEditarTiempoEntrega(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TiempoEntrega.aspx/EditarTiempoEntrega",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTiempoEntrega").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTiempoEntrega").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTiempoEntrega(pTiempoEntrega) {
    var errores = "";

    if (pTiempoEntrega.TiempoEntrega == "")
    { errores = errores + "<span>*</span> El campo TiempoEntrega esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}