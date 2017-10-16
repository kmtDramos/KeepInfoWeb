//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("AutorizacionTipoCambio");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarAutorizacionTipoCambio", function() {
        ObtenerFormaAgregarAutorizacionTipoCambio();
    });
    
    $("#grdAutorizacionTipoCambio").on("click", ".imgFormaConsultarAutorizacionTipoCambio", function() {
        var registro = $(this).parents("tr");
        var AutorizacionTipoCambio = new Object();
        AutorizacionTipoCambio.pIdAutorizacionTipoCambio = parseInt($(registro).children("td[aria-describedby='grdAutorizacionTipoCambio_IdAutorizacionTipoCambio']").html());
        ObtenerFormaConsultarAutorizacionTipoCambio(JSON.stringify(AutorizacionTipoCambio));
    });
    
    $('#grdAutorizacionTipoCambio').on('click', '.div_grdAutorizacionTipoCambio_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdAutorizacionTipoCambio_AI']").children().attr("baja")
        var idAutorizacionTipoCambio = $(registro).children("td[aria-describedby='grdAutorizacionTipoCambio_IdAutorizacionTipoCambio']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idAutorizacionTipoCambio, baja);
    });
    
    $('#dialogAgregarAutorizacionTipoCambio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarAutorizacionTipoCambio").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarAutorizacionTipoCambio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarAutorizacionTipoCambio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarAutorizacionTipoCambio").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarAutorizacionTipoCambio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarAutorizacionTipoCambio").remove();
        },
        buttons: {
            "Editar": function() {
                EditarAutorizacionTipoCambio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarAutorizacionTipoCambio()
{
    $("#dialogAgregarAutorizacionTipoCambio").obtenerVista({
        nombreTemplate: "tmplAgregarAutorizacionTipoCambio.html",
        url: "AutorizacionTipoCambio.aspx/ObtenerFormaAgregarAutorizacionTipoCambio",
        despuesDeCompilar: function(pRespuesta) {
            $("#txtFechaVigencia").datepicker();
            $("#dialogAgregarAutorizacionTipoCambio").dialog("open");
        }
    });
}

function ObtenerFormaConsultarAutorizacionTipoCambio(pIdAutorizacionTipoCambio) {
    $("#dialogConsultarAutorizacionTipoCambio").obtenerVista({
        nombreTemplate: "tmplConsultarAutorizacionTipoCambio.html",
        url: "AutorizacionTipoCambio.aspx/ObtenerFormaAutorizacionTipoCambio",
        parametros: pIdAutorizacionTipoCambio,
        despuesDeCompilar: function(pRespuesta) {        
            if (pRespuesta.modelo.Permisos.puedeEditarAutorizacionTipoCambio == 1) {
                $("#dialogConsultarAutorizacionTipoCambio").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var AutorizacionTipoCambio = new Object();
                        AutorizacionTipoCambio.pIdAutorizacionTipoCambio = parseInt($("#divFormaConsultarAutorizacionTipoCambio").attr("IdAutorizacionTipoCambio"));
                        ObtenerFormaEditarAutorizacionTipoCambio(JSON.stringify(AutorizacionTipoCambio))
                    }
                });
                $("#dialogConsultarAutorizacionTipoCambio").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarAutorizacionTipoCambio").dialog("option", "buttons", {});
                $("#dialogConsultarAutorizacionTipoCambio").dialog("option", "height", "100");
            }
            $("#dialogConsultarAutorizacionTipoCambio").dialog("open");
        }
    });
}

function ObtenerFormaEditarAutorizacionTipoCambio(pIdAutorizacionTipoCambio) {
    $("#dialogEditarAutorizacionTipoCambio").obtenerVista({
        nombreTemplate: "tmplEditarAutorizacionTipoCambio.html",
        url: "AutorizacionTipoCambio.aspx/ObtenerFormaEditarAutorizacionTipoCambio",
        parametros: pIdAutorizacionTipoCambio,
        despuesDeCompilar: function(pRespuesta) {
            $("#txtFechaVigencia").datepicker();
            $("#dialogEditarAutorizacionTipoCambio").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarAutorizacionTipoCambio() {
    var pAutorizacionTipoCambio = new Object();
    pAutorizacionTipoCambio.IdUsuarioAutorizo = $("#cmbUsuarioAutoriza").val();
    pAutorizacionTipoCambio.IdUsuarioSolicito = $("#cmbUsuarioSolicita").val();
    pAutorizacionTipoCambio.IdTipoMonedaOrigen  = $("#cmbTipoMonedaOrigen").val();
    pAutorizacionTipoCambio.IdTipoMonedaDestino = $("#cmbTipoMonedaDestino").val();
    pAutorizacionTipoCambio.IdTipoDocumento = $("#cmbTipoDocumento").val();
    pAutorizacionTipoCambio.TipoCambio          = $("#txtTipoCambio").val();
    pAutorizacionTipoCambio.FechaVigencia       = $("#txtFechaVigencia").val();
    pAutorizacionTipoCambio.ClaveAutorizacion = $("#txtClaveAutorizacion").val();
        
    var validacion = ValidaAutorizacionTipoCambio(pAutorizacionTipoCambio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAutorizacionTipoCambio = pAutorizacionTipoCambio;
    SetAgregarAutorizacionTipoCambio(JSON.stringify(oRequest)); 
}

function SetAgregarAutorizacionTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "AutorizacionTipoCambio.aspx/AgregarAutorizacionTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAutorizacionTipoCambio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarAutorizacionTipoCambio").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdAutorizacionTipoCambio, pBaja) {
    var pRequest = "{'pIdAutorizacionTipoCambio':" + pIdAutorizacionTipoCambio + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "AutorizacionTipoCambio.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdAutorizacionTipoCambio").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdAutorizacionTipoCambio').one('click', '.div_grdAutorizacionTipoCambio_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdAutorizacionTipoCambio_AI']").children().attr("baja")
                var idAutorizacionTipoCambio = $(registro).children("td[aria-describedby='grdAutorizacionTipoCambio_IdAutorizacionTipoCambio']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idAutorizacionTipoCambio, baja);
            });
        }
    });
}

function EditarAutorizacionTipoCambio() {
    var pAutorizacionTipoCambio = new Object();
    pAutorizacionTipoCambio.IdAutorizacionTipoCambio = parseInt($("#divFormaEditarAutorizacionTipoCambio").attr("IdAutorizacionTipoCambio"));
    pAutorizacionTipoCambio.IdUsuarioAutorizo = $("#cmbUsuarioAutoriza").val();
    pAutorizacionTipoCambio.IdUsuarioSolicito = $("#cmbUsuarioSolicita").val();
    pAutorizacionTipoCambio.IdTipoMonedaOrigen = $("#cmbTipoMonedaOrigen").val();
    pAutorizacionTipoCambio.IdTipoMonedaDestino = $("#cmbTipoMonedaDestino").val();
    pAutorizacionTipoCambio.IdTipoDocumento = $("#cmbTipoDocumento").val();
    pAutorizacionTipoCambio.TipoCambio = $("#txtTipoCambio").val();
    pAutorizacionTipoCambio.FechaVigencia = $("#txtFechaVigencia").val();
    pAutorizacionTipoCambio.ClaveAutorizacion = $("#txtClaveAutorizacion").val();
    
    var validacion = ValidaAutorizacionTipoCambio(pAutorizacionTipoCambio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAutorizacionTipoCambio = pAutorizacionTipoCambio;
    SetEditarAutorizacionTipoCambio(JSON.stringify(oRequest));
}

function SetEditarAutorizacionTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "AutorizacionTipoCambio.aspx/EditarAutorizacionTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAutorizacionTipoCambio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarAutorizacionTipoCambio").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaAutorizacionTipoCambio(pAutorizacionTipoCambio) {
    var errores = "";

    if (pAutorizacionTipoCambio.IdUsuarioAutorizo == "0")
    { errores = errores + "<span>*</span> El campo usuario autorizó esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionTipoCambio.IdUsuarioSolicito == "0")
    { errores = errores + "<span>*</span> El campo usuario solicitó esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionTipoCambio.IdTipoMonedaOrigen == "0")
    { errores = errores + "<span>*</span> El campo tipo moneda origen esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionTipoCambio.IdTipoMonedaDestino == "0")
    { errores = errores + "<span>*</span> El campo tipo moneda destino esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionTipoCambio.FechaVigencia == "")
    { errores = errores + "<span>*</span> El campo fecha vigencia esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionTipoCambio.ClaveAutorizacion == "")
    { errores = errores + "<span>*</span> El campo clave de autorización esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionTipoCambio.TipoCambio == "0")
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function MostrarCaracteres(obj) {
    name = $('#txtClaveAutorizacion').attr('name');
    value = $('#txtClaveAutorizacion').attr('value');

    if ($(obj).attr('checked')) {
        html = '<input type="text" name="' + name + '" value="' + value + '" id="txtClaveAutorizacion"/>';
        $('#txtClaveAutorizacion').after(html).remove();
    }
    else {
        html = '<input type="password" name="' + name + '" value="' + value + '" id="txtClaveAutorizacion"/>';
        $('#txtClaveAutorizacion').after(html).remove();
    }
}