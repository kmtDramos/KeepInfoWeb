//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoGarantia");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoGarantia", function() {
        ObtenerFormaAgregarTipoGarantia();
    });

    $("#grdTipoGarantia").on("click", ".imgFormaConsultarTipoGarantia", function() {
        var registro = $(this).parents("tr");
        var TipoGarantia = new Object();
        TipoGarantia.pIdTipoGarantia = parseInt($(registro).children("td[aria-describedby='grdTipoGarantia_IdTipoGarantia']").html());
        ObtenerFormaConsultarTipoGarantia(JSON.stringify(TipoGarantia));
    });

    $('#grdTipoGarantia').one('click', '.div_grdTipoGarantia_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoGarantia_AI']").children().attr("baja")
        var idTipoGarantia = $(registro).children("td[aria-describedby='grdTipoGarantia_IdTipoGarantia']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoGarantia, baja);
    });

    $('#dialogAgregarTipoGarantia').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoGarantia").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoGarantia();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoGarantia').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoGarantia").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoGarantia').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoGarantia").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoGarantia();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoGarantia()
{
    $("#dialogAgregarTipoGarantia").obtenerVista({
        nombreTemplate: "tmplAgregarTipoGarantia.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoGarantia").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoGarantia(pIdTipoGarantia) {
    $("#dialogConsultarTipoGarantia").obtenerVista({
        nombreTemplate: "tmplConsultarTipoGarantia.html",
        url: "TipoGarantia.aspx/ObtenerFormaTipoGarantia",
        parametros: pIdTipoGarantia,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoGarantia == 1) {
                $("#dialogConsultarTipoGarantia").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoGarantia = new Object();
                        TipoGarantia.IdTipoGarantia = parseInt($("#divFormaConsultarTipoGarantia").attr("IdTipoGarantia"));
                        ObtenerFormaEditarTipoGarantia(JSON.stringify(TipoGarantia))
                    }
                });
                $("#dialogConsultarTipoGarantia").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoGarantia").dialog("option", "buttons", {});
                $("#dialogConsultarTipoGarantia").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoGarantia").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoGarantia(IdTipoGarantia) {
    $("#dialogEditarTipoGarantia").obtenerVista({
        nombreTemplate: "tmplEditarTipoGarantia.html",
        url: "TipoGarantia.aspx/ObtenerFormaEditarTipoGarantia",
        parametros: IdTipoGarantia,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoGarantia").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoGarantia() {
    var pTipoGarantia = new Object();
    pTipoGarantia.TipoGarantia = $("#txtTipoGarantia").val();
    var validacion = ValidaTipoGarantia(pTipoGarantia);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoGarantia = pTipoGarantia;
    SetAgregarTipoGarantia(JSON.stringify(oRequest)); 
}

function SetAgregarTipoGarantia(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoGarantia.aspx/AgregarTipoGarantia",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoGarantia").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoGarantia").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoGarantia, pBaja) {
    var pRequest = "{'pIdTipoGarantia':" + pIdTipoGarantia + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoGarantia.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoGarantia").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoGarantia').one('click', '.div_grdTipoGarantia_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoGarantia_AI']").children().attr("baja")
                var idTipoGarantia = $(registro).children("td[aria-describedby='grdTipoGarantia_IdTipoGarantia']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoGarantia, baja);
            });
        }
    });
}

function EditarTipoGarantia() {
    var pTipoGarantia = new Object();
    pTipoGarantia.IdTipoGarantia = $("#divFormaEditarTipoGarantia").attr("idTipoGarantia");
    pTipoGarantia.TipoGarantia = $("#txtTipoGarantia").val();
    var validacion = ValidaTipoGarantia(pTipoGarantia);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoGarantia = pTipoGarantia;
    SetEditarTipoGarantia(JSON.stringify(oRequest));
}

function SetEditarTipoGarantia(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoGarantia.aspx/EditarTipoGarantia",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoGarantia").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoGarantia").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoGarantia(pTipoGarantia) {
    var errores = "";

    if (pTipoGarantia.TipoGarantia == "")
    { errores = errores + "<span>*</span> El campo TipoGarantia esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}