//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoServicio");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoServicio", function() {
        ObtenerFormaAgregarTipoServicio();
    });

    $("#grdTipoServicio").on("click", ".imgFormaConsultarTipoServicio", function() {
        var registro = $(this).parents("tr");
        var TipoServicio = new Object();
        TipoServicio.pIdTipoServicio = parseInt($(registro).children("td[aria-describedby='grdTipoServicio_IdTipoServicio']").html());
        ObtenerFormaConsultarTipoServicio(JSON.stringify(TipoServicio));
    });

    $('#grdTipoServicio').one('click', '.div_grdTipoServicio_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoServicio_AI']").children().attr("baja")
        var idTipoServicio = $(registro).children("td[aria-describedby='grdTipoServicio_IdTipoServicio']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoServicio, baja);
    });

    $('#dialogAgregarTipoServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoServicio").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoServicio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoServicio").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoServicio").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoServicio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoServicio() {
    $("#dialogAgregarTipoServicio").obtenerVista({
        nombreTemplate: "tmplAgregarTipoServicio.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoServicio").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoServicio(pIdTipoServicio) {
    $("#dialogConsultarTipoServicio").obtenerVista({
        nombreTemplate: "tmplConsultarTipoServicio.html",
        url: "TipoServicio.aspx/ObtenerFormaTipoServicio",
        parametros: pIdTipoServicio,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoServicio == 1) {
                $("#dialogConsultarTipoServicio").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoServicio = new Object();
                        TipoServicio.IdTipoServicio = parseInt($("#divFormaConsultarTipoServicio").attr("IdTipoServicio"));
                        ObtenerFormaEditarTipoServicio(JSON.stringify(TipoServicio))
                    }
                });
                $("#dialogConsultarTipoServicio").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoServicio").dialog("option", "buttons", {});
                $("#dialogConsultarTipoServicio").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoServicio").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoServicio(IdTipoServicio) {
    $("#dialogEditarTipoServicio").obtenerVista({
        nombreTemplate: "tmplEditarTipoServicio.html",
        url: "TipoServicio.aspx/ObtenerFormaEditarTipoServicio",
        parametros: IdTipoServicio,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoServicio").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoServicio() {
    var pTipoServicio = new Object();
    pTipoServicio.TipoServicio = $("#txtTipoServicio").val();
    var validacion = ValidaTipoServicio(pTipoServicio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoServicio = pTipoServicio;
    SetAgregarTipoServicio(JSON.stringify(oRequest));
}

function SetAgregarTipoServicio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoServicio.aspx/AgregarTipoServicio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoServicio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoServicio").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoServicio, pBaja) {
    var pRequest = "{'pIdTipoServicio':" + pIdTipoServicio + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoServicio.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoServicio").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoServicio').one('click', '.div_grdTipoServicio_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoServicio_AI']").children().attr("baja")
                var idTipoServicio = $(registro).children("td[aria-describedby='grdTipoServicio_IdTipoServicio']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoServicio, baja);
            });
        }
    });
}

function EditarTipoServicio() {
    var pTipoServicio = new Object();
    pTipoServicio.IdTipoServicio = $("#divFormaEditarTipoServicio").attr("idTipoServicio");
    pTipoServicio.TipoServicio = $("#txtTipoServicio").val();
    var validacion = ValidaTipoServicio(pTipoServicio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoServicio = pTipoServicio;
    SetEditarTipoServicio(JSON.stringify(oRequest));
}
function SetEditarTipoServicio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoServicio.aspx/EditarTipoServicio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoServicio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoServicio").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoServicio(pTipoServicio) {
    var errores = "";

    if (pTipoServicio.TipoServicio == "")
    { errores = errores + "<span>*</span> El nombre del tipo de servicio esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
