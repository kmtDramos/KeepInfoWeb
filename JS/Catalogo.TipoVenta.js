//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoVenta");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoVenta", function() {
        ObtenerFormaAgregarTipoVenta();
    });

    $("#grdTipoVenta").on("click", ".imgFormaConsultarTipoVenta", function() {
        var registro = $(this).parents("tr");
        var TipoVenta = new Object();
        TipoVenta.pIdTipoVenta = parseInt($(registro).children("td[aria-describedby='grdTipoVenta_IdTipoVenta']").html());
        ObtenerFormaConsultarTipoVenta(JSON.stringify(TipoVenta));
    });

    $('#grdTipoVenta').on('click', '.div_grdTipoVenta_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoVenta_AI']").children().attr("baja")
        var idTipoVenta = $(registro).children("td[aria-describedby='grdTipoVenta_IdTipoVenta']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoVenta, baja);
    });

    $('#dialogAgregarTipoVenta').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoVenta").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoVenta();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoVenta').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoVenta").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoVenta').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoVenta").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoVenta();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoVenta() {
    $("#dialogAgregarTipoVenta").obtenerVista({
        nombreTemplate: "tmplAgregarTipoVenta.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoVenta").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoVenta(pIdTipoVenta) {
    $("#dialogConsultarTipoVenta").obtenerVista({
        nombreTemplate: "tmplConsultarTipoVenta.html",
        url: "TipoVenta.aspx/ObtenerFormaTipoVenta",
        parametros: pIdTipoVenta,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoVenta == 1) {
                $("#dialogConsultarTipoVenta").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoVenta = new Object();
                        TipoVenta.IdTipoVenta = parseInt($("#divFormaConsultarTipoVenta").attr("IdTipoVenta"));
                        ObtenerFormaEditarTipoVenta(JSON.stringify(TipoVenta))
                    }
                });
                $("#dialogConsultarTipoVenta").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoVenta").dialog("option", "buttons", {});
                $("#dialogConsultarTipoVenta").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoVenta").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoVenta(IdTipoVenta) {
    $("#dialogEditarTipoVenta").obtenerVista({
        nombreTemplate: "tmplEditarTipoVenta.html",
        url: "TipoVenta.aspx/ObtenerFormaEditarTipoVenta",
        parametros: IdTipoVenta,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoVenta").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoVenta() {
    var pTipoVenta = new Object();
    pTipoVenta.TipoVenta = $("#txtTipoVenta").val();
    var validacion = ValidaTipoVenta(pTipoVenta);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoVenta = pTipoVenta;
    SetAgregarTipoVenta(JSON.stringify(oRequest));
}

function SetAgregarTipoVenta(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoVenta.aspx/AgregarTipoVenta",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoVenta").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoVenta").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoVenta, pBaja) {
    var pRequest = "{'pIdTipoVenta':" + pIdTipoVenta + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoVenta.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoVenta").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoVenta').one('click', '.div_grdTipoVenta_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoVenta_AI']").children().attr("baja")
                var idTipoVenta = $(registro).children("td[aria-describedby='grdTipoVenta_IdTipoVenta']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoVenta, baja);
            });
        }
    });
}

function EditarTipoVenta() {
    var pTipoVenta = new Object();
    pTipoVenta.IdTipoVenta = $("#divFormaEditarTipoVenta").attr("idTipoVenta");
    pTipoVenta.TipoVenta = $("#txtTipoVenta").val();
    var validacion = ValidaTipoVenta(pTipoVenta);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoVenta = pTipoVenta;
    SetEditarTipoVenta(JSON.stringify(oRequest));
}
function SetEditarTipoVenta(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoVenta.aspx/EditarTipoVenta",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoVenta").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoVenta").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoVenta(pTipoVenta) {
    var errores = "";

    if (pTipoVenta.TipoVenta == "")
    { errores = errores + "<span>*</span> El nombre del tipo de venta esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
