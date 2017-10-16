//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("CondicionPago");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCondicionPago", function() {
        ObtenerFormaAgregarCondicionPago();
    });

    $("#grdCondicionPago").on("click", ".imgFormaConsultarCondicionPago", function() {
        var registro = $(this).parents("tr");
        var CondicionPago = new Object();
        CondicionPago.pIdCondicionPago = parseInt($(registro).children("td[aria-describedby='grdCondicionPago_IdCondicionPago']").html());
        ObtenerFormaConsultarCondicionPago(JSON.stringify(CondicionPago));
    });

    $('#grdCondicionPago').one('click', '.div_grdCondicionPago_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCondicionPago_AI']").children().attr("baja")
        var idCondicionPago = $(registro).children("td[aria-describedby='grdCondicionPago_IdCondicionPago']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCondicionPago, baja);
    });

    $('#dialogAgregarCondicionPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCondicionPago").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCondicionPago();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCondicionPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCondicionPago").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCondicionPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCondicionPago").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCondicionPago();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCondicionPago() {
    $("#dialogAgregarCondicionPago").obtenerVista({
        nombreTemplate: "tmplAgregarCondicionPago.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCondicionPago").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCondicionPago(pIdCondicionPago) {
    $("#dialogConsultarCondicionPago").obtenerVista({
        nombreTemplate: "tmplConsultarCondicionPago.html",
        url: "CondicionPago.aspx/ObtenerFormaCondicionPago",
        parametros: pIdCondicionPago,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCondicionPago == 1) {
                $("#dialogConsultarCondicionPago").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var CondicionPago = new Object();
                        CondicionPago.IdCondicionPago = parseInt($("#divFormaConsultarCondicionPago").attr("IdCondicionPago"));
                        ObtenerFormaEditarCondicionPago(JSON.stringify(CondicionPago))
                    }
                });
                $("#dialogConsultarCondicionPago").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCondicionPago").dialog("option", "buttons", {});
                $("#dialogConsultarCondicionPago").dialog("option", "height", "100");
            }
            $("#dialogConsultarCondicionPago").dialog("open");
        }
    });
}

function ObtenerFormaEditarCondicionPago(IdCondicionPago) {
    $("#dialogEditarCondicionPago").obtenerVista({
        nombreTemplate: "tmplEditarCondicionPago.html",
        url: "CondicionPago.aspx/ObtenerFormaEditarCondicionPago",
        parametros: IdCondicionPago,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCondicionPago").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCondicionPago() {
    var pCondicionPago = new Object();
    pCondicionPago.CondicionPago = $("#txtCondicionPago").val();
    pCondicionPago.NumeroDias = $("#txtNumeroDias").val();
    var validacion = ValidaCondicionPago(pCondicionPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCondicionPago = pCondicionPago;
    SetAgregarCondicionPago(JSON.stringify(oRequest));
}

function SetAgregarCondicionPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CondicionPago.aspx/AgregarCondicionPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCondicionPago").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCondicionPago").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdCondicionPago, pBaja) {
    var pRequest = "{'pIdCondicionPago':" + pIdCondicionPago + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "CondicionPago.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCondicionPago").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCondicionPago').one('click', '.div_grdCondicionPago_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCondicionPago_AI']").children().attr("baja")
                var idCondicionPago = $(registro).children("td[aria-describedby='grdCondicionPago_IdCondicionPago']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCondicionPago, baja);
            });
        }
    });
}

function EditarCondicionPago() {
    var pCondicionPago = new Object();
    pCondicionPago.IdCondicionPago = $("#divFormaEditarCondicionPago").attr("idCondicionPago");
    pCondicionPago.CondicionPago = $("#txtCondicionPago").val();
    pCondicionPago.NumeroDias = $("#txtNumeroDias").val();
    var validacion = ValidaCondicionPago(pCondicionPago);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCondicionPago = pCondicionPago;
    SetEditarCondicionPago(JSON.stringify(oRequest));
}
function SetEditarCondicionPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CondicionPago.aspx/EditarCondicionPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCondicionPago").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCondicionPago").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaCondicionPago(pCondicionPago) {
    var errores = "";

    if (pCondicionPago.CondicionPago == "")
    { errores = errores + "<span>*</span> El campo de condición de pago esta vacio, favor de capturarlo.<br />"; }
    if (pCondicionPago.NumeroDias == 0)
    { errores = errores + "<span>*</span> El campo de número de dias esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
