//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("UnidadCompraVenta");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarUnidadCompraVenta", function() {
        ObtenerFormaAgregarUnidadCompraVenta();
    });

    $("#grdUnidadCompraVenta").on("click", ".imgFormaConsultarUnidadCompraVenta", function() {
        var registro = $(this).parents("tr");
        var UnidadCompraVenta = new Object();
        UnidadCompraVenta.pIdUnidadCompraVenta = parseInt($(registro).children("td[aria-describedby='grdUnidadCompraVenta_IdUnidadCompraVenta']").html());
        ObtenerFormaConsultarUnidadCompraVenta(JSON.stringify(UnidadCompraVenta));
    });

    $('#grdUnidadCompraVenta').one('click', '.div_grdUnidadCompraVenta_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdUnidadCompraVenta_AI']").children().attr("baja")
        var idUnidadCompraVenta = $(registro).children("td[aria-describedby='grdUnidadCompraVenta_IdUnidadCompraVenta']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idUnidadCompraVenta, baja);
    });

    $('#dialogAgregarUnidadCompraVenta').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarUnidadCompraVenta").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarUnidadCompraVenta();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarUnidadCompraVenta').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarUnidadCompraVenta").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarUnidadCompraVenta').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarUnidadCompraVenta").remove();
        },
        buttons: {
            "Editar": function() {
                EditarUnidadCompraVenta();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarUnidadCompraVenta() {
    $("#dialogAgregarUnidadCompraVenta").obtenerVista({
        nombreTemplate: "tmplAgregarUnidadCompraVenta.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarUnidadCompraVenta").dialog("open");
        }
    });
}

function ObtenerFormaConsultarUnidadCompraVenta(pIdUnidadCompraVenta) {
    $("#dialogConsultarUnidadCompraVenta").obtenerVista({
        nombreTemplate: "tmplConsultarUnidadCompraVenta.html",
        url: "UnidadCompraVenta.aspx/ObtenerFormaUnidadCompraVenta",
        parametros: pIdUnidadCompraVenta,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarUnidadCompraVenta == 1) {
                $("#dialogConsultarUnidadCompraVenta").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var UnidadCompraVenta = new Object();
                        UnidadCompraVenta.IdUnidadCompraVenta = parseInt($("#divFormaConsultarUnidadCompraVenta").attr("IdUnidadCompraVenta"));
                        ObtenerFormaEditarUnidadCompraVenta(JSON.stringify(UnidadCompraVenta))
                    }
                });
                $("#dialogConsultarUnidadCompraVenta").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarUnidadCompraVenta").dialog("option", "buttons", {});
                $("#dialogConsultarUnidadCompraVenta").dialog("option", "height", "100");
            }
            $("#dialogConsultarUnidadCompraVenta").dialog("open");
        }
    });
}

function ObtenerFormaEditarUnidadCompraVenta(IdUnidadCompraVenta) {
    $("#dialogEditarUnidadCompraVenta").obtenerVista({
        nombreTemplate: "tmplEditarUnidadCompraVenta.html",
        url: "UnidadCompraVenta.aspx/ObtenerFormaEditarUnidadCompraVenta",
        parametros: IdUnidadCompraVenta,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarUnidadCompraVenta").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarUnidadCompraVenta() {
    var pUnidadCompraVenta = new Object();
    pUnidadCompraVenta.UnidadCompraVenta = $("#txtUnidadCompraVenta").val();
    var validacion = ValidaUnidadCompraVenta(pUnidadCompraVenta);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pUnidadCompraVenta = pUnidadCompraVenta;
    SetAgregarUnidadCompraVenta(JSON.stringify(oRequest));
}

function SetAgregarUnidadCompraVenta(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "UnidadCompraVenta.aspx/AgregarUnidadCompraVenta",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdUnidadCompraVenta").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarUnidadCompraVenta").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdUnidadCompraVenta, pBaja) {
    var pRequest = "{'pIdUnidadCompraVenta':" + pIdUnidadCompraVenta + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "UnidadCompraVenta.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdUnidadCompraVenta").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdUnidadCompraVenta').one('click', '.div_grdUnidadCompraVenta_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdUnidadCompraVenta_AI']").children().attr("baja")
                var idUnidadCompraVenta = $(registro).children("td[aria-describedby='grdUnidadCompraVenta_IdUnidadCompraVenta']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idUnidadCompraVenta, baja);
            });
        }
    });
}

function EditarUnidadCompraVenta() {
    var pUnidadCompraVenta = new Object();
    pUnidadCompraVenta.IdUnidadCompraVenta = $("#divFormaEditarUnidadCompraVenta").attr("idUnidadCompraVenta");
    pUnidadCompraVenta.UnidadCompraVenta = $("#txtUnidadCompraVenta").val();
    var validacion = ValidaUnidadCompraVenta(pUnidadCompraVenta);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pUnidadCompraVenta = pUnidadCompraVenta;
    SetEditarUnidadCompraVenta(JSON.stringify(oRequest));
}
function SetEditarUnidadCompraVenta(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "UnidadCompraVenta.aspx/EditarUnidadCompraVenta",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdUnidadCompraVenta").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarUnidadCompraVenta").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaUnidadCompraVenta(pUnidadCompraVenta) {
    var errores = "";

    if (pUnidadCompraVenta.UnidadCompraVenta == "")
    { errores = errores + "<span>*</span> El nombre del campo unidad de compra venta esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
