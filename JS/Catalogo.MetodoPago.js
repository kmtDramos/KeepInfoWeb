//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("MetodoPago");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarMetodoPago", function() {
        ObtenerFormaAgregarMetodoPago();
    });

    $("#grdMetodoPago").on("click", ".imgFormaConsultarMetodoPago", function() {
        var registro = $(this).parents("tr");
        var MetodoPago = new Object();
        MetodoPago.pIdMetodoPago = parseInt($(registro).children("td[aria-describedby='grdMetodoPago_IdMetodoPago']").html());
        ObtenerFormaConsultarMetodoPago(JSON.stringify(MetodoPago));
    });

    $('#grdMetodoPago').one('click', '.div_grdMetodoPago_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdMetodoPago_AI']").children().attr("baja")
        var idMetodoPago = $(registro).children("td[aria-describedby='grdMetodoPago_IdMetodoPago']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idMetodoPago, baja);
    });

    $('#dialogAgregarMetodoPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarMetodoPago").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarMetodoPago();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarMetodoPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarMetodoPago").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarMetodoPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarMetodoPago").remove();
        },
        buttons: {
            "Editar": function() {
                EditarMetodoPago();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarMetodoPago()
{
    $("#dialogAgregarMetodoPago").obtenerVista({
        nombreTemplate: "tmplAgregarMetodoPago.html",
        url: "MetodoPago.aspx/ObtenerFormaAgregarMetodoPago",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarMetodoPago").dialog("open");
        }
    });
}

function ObtenerFormaConsultarMetodoPago(pIdMetodoPago) {
    $("#dialogConsultarMetodoPago").obtenerVista({
        nombreTemplate: "tmplConsultarMetodoPago.html",
        url: "MetodoPago.aspx/ObtenerFormaMetodoPago",
        parametros: pIdMetodoPago,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarMetodoPago == 1) {
                $("#dialogConsultarMetodoPago").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var MetodoPago = new Object();
                        MetodoPago.IdMetodoPago = parseInt($("#divFormaConsultarMetodoPago").attr("IdMetodoPago"));
                        ObtenerFormaEditarMetodoPago(JSON.stringify(MetodoPago))
                    }
                });
                $("#dialogConsultarMetodoPago").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarMetodoPago").dialog("option", "buttons", {});
                $("#dialogConsultarMetodoPago").dialog("option", "height", "100");
            }
            $("#dialogConsultarMetodoPago").dialog("open");
        }
    });
}

function ObtenerFormaEditarMetodoPago(IdMetodoPago) {
    $("#dialogEditarMetodoPago").obtenerVista({
        nombreTemplate: "tmplEditarMetodoPago.html",
        url: "MetodoPago.aspx/ObtenerFormaEditarMetodoPago",
        parametros: IdMetodoPago,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarMetodoPago").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarMetodoPago() {
    var pMetodoPago = new Object();
    pMetodoPago.MetodoPago = $("#txtMetodoPago").val();
    pMetodoPago.IdTipoMovimiento = $("#cmbTipoMovimiento").val();
    pMetodoPago.Clave = $("#txtClave").val();
    var validacion = ValidaMetodoPago(pMetodoPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pMetodoPago = pMetodoPago;
    SetAgregarMetodoPago(JSON.stringify(oRequest)); 
}

function SetAgregarMetodoPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "MetodoPago.aspx/AgregarMetodoPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdMetodoPago").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarMetodoPago").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdMetodoPago, pBaja) {
    var pRequest = "{'pIdMetodoPago':" + pIdMetodoPago + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "MetodoPago.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdMetodoPago").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdMetodoPago').one('click', '.div_grdMetodoPago_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdMetodoPago_AI']").children().attr("baja")
                var idMetodoPago = $(registro).children("td[aria-describedby='grdMetodoPago_IdMetodoPago']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idMetodoPago, baja);
            });
        }
    });
}

function EditarMetodoPago() {
    var pMetodoPago = new Object();
    pMetodoPago.IdMetodoPago = $("#divFormaEditarMetodoPago").attr("idMetodoPago");
    pMetodoPago.MetodoPago = $("#txtMetodoPago").val();
    pMetodoPago.IdTipoMovimiento = $("#cmbTipoMovimiento").val();
    pMetodoPago.Clave = $("#txtClave").val();
    var validacion = ValidaMetodoPago(pMetodoPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pMetodoPago = pMetodoPago;
    SetEditarMetodoPago(JSON.stringify(oRequest));
}

function SetEditarMetodoPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "MetodoPago.aspx/EditarMetodoPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdMetodoPago").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarMetodoPago").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaMetodoPago(pMetodoPago) {
    var errores = "";

    if (pMetodoPago.MetodoPago == "")
    { errores = errores + "<span>*</span> El campo MetodoPago esta vacío, favor de capturarlo.<br />"; }

    if (pMetodoPago.IdTipoMovimiento == 0)
    { errores = errores + "<span>*</span> El campo tipo de movimiento esta vacío, favor de seleccionarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}