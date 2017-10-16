//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("AutorizacionIVA");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarAutorizacionIVA", function() {
        ObtenerFormaAgregarAutorizacionIVA();
    });
    
    $('#grdAutorizacionIVA').on('click', '.div_grdAutorizacionIVA_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdAutorizacionIVA_AI']").children().attr("baja")
        var idAutorizacionIVA = $(registro).children("td[aria-describedby='grdAutorizacionIVA_IdAutorizacionIVA']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idAutorizacionIVA, baja);
    });
    
    $('#dialogAgregarAutorizacionIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarAutorizacionIVA").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarAutorizacionIVA();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarAutorizacionIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarAutorizacionIVA").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarAutorizacionIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarAutorizacionIVA").remove();
        },
        buttons: {
            "Editar": function() {
                EditarAutorizacionIVA();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $("#grdAutorizacionIVA").on("click", ".imgFormaConsultarAutorizacionIVA", function() {
        var registro = $(this).parents("tr");
        var AutorizacionIVA = new Object();
        AutorizacionIVA.pIdAutorizacionIVA = parseInt($(registro).children("td[aria-describedby='grdAutorizacionIVA_IdAutorizacionIVA']").html());
        ObtenerFormaConsultarAutorizacionIVA(JSON.stringify(AutorizacionIVA));
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarAutorizacionIVA()
{
    $("#dialogAgregarAutorizacionIVA").obtenerVista({
        nombreTemplate: "tmplAgregarAutorizacionIVA.html",
        url: "AutorizacionIVA.aspx/ObtenerFormaAgregarAutorizacionIVA",
        despuesDeCompilar: function(pRespuesta) {
            $("#txtFechaVigencia").datepicker();
            $("#dialogAgregarAutorizacionIVA").dialog("open");
        }
    });
}

function ObtenerFormaConsultarAutorizacionIVA(pIdAutorizacionIVA) {
    $("#dialogConsultarAutorizacionIVA").obtenerVista({
        nombreTemplate: "tmplConsultarAutorizacionIVA.html",
        url: "AutorizacionIVA.aspx/ObtenerFormaAutorizacionIVA",
        parametros: pIdAutorizacionIVA,
        despuesDeCompilar: function(pRespuesta) {        
            if (pRespuesta.modelo.Permisos.puedeConsultarAutorizacionIVA == 1) {
                $("#dialogConsultarAutorizacionIVA").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var AutorizacionIVA = new Object();
                        AutorizacionIVA.pIdAutorizacionIVA = parseInt($("#divFormaConsultarAutorizacionIVA").attr("IdAutorizacionIVA"));
                        ObtenerFormaEditarAutorizacionIVA(JSON.stringify(AutorizacionIVA))
                    }
                });
                $("#dialogConsultarAutorizacionIVA").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarAutorizacionIVA").dialog("option", "buttons", {});
                $("#dialogConsultarAutorizacionIVA").dialog("option", "height", "100");
            }
            $("#dialogConsultarAutorizacionIVA").dialog("open");
        }
    });
}

function ObtenerFormaEditarAutorizacionIVA(pIdAutorizacionIVA) {
    $("#dialogEditarAutorizacionIVA").obtenerVista({
        nombreTemplate: "tmplEditarAutorizacionIVA.html",
        url: "AutorizacionIVA.aspx/ObtenerFormaEditarAutorizacionIVA",
        parametros: pIdAutorizacionIVA,
        despuesDeCompilar: function(pRespuesta) {
            $("#txtFechaVigencia").datepicker();
            $("#dialogEditarAutorizacionIVA").dialog("open");
        }
    });
}



//-----------AJAX-----------//
//-Funciones Ajax-//
function AgregarAutorizacionIVA() {
    var pAutorizacionIVA = new Object();
    pAutorizacionIVA.IdUsuarioAutorizo = $("#cmbUsuarioAutoriza").val();
    pAutorizacionIVA.IdUsuarioSolicito = $("#cmbUsuarioSolicita").val();
    pAutorizacionIVA.TipoDocumento = $("#cmbTipoDocumento").val();
    pAutorizacionIVA.IVA          = $("#txtIVA").val();
    pAutorizacionIVA.FechaVigencia       = $("#txtFechaVigencia").val();
    pAutorizacionIVA.ClaveAutorizacion = $("#txtClaveAutorizacion").val();
        
    var validacion = ValidaAutorizacionIVA(pAutorizacionIVA);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAutorizacionIVA = pAutorizacionIVA;
    SetAgregarAutorizacionIVA(JSON.stringify(oRequest)); 
}

function SetAgregarAutorizacionIVA(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "AutorizacionIVA.aspx/AgregarAutorizacionIVA",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAutorizacionIVA").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarAutorizacionIVA").dialog("close");
        }
    });
}

function EditarAutorizacionIVA() {
    var pAutorizacionIVA = new Object();
    pAutorizacionIVA.IdAutorizacionIVA = parseInt($("#divFormaEditarAutorizacionIVA").attr("IdAutorizacionIVA"));
    pAutorizacionIVA.IdUsuarioAutorizo = $("#cmbUsuarioAutoriza").val();
    pAutorizacionIVA.IdUsuarioSolicito = $("#cmbUsuarioSolicita").val();
    pAutorizacionIVA.TipoDocumento = $("#cmbTipoDocumento").val();
    pAutorizacionIVA.IVA = $("#txtIVA").val();
    pAutorizacionIVA.FechaVigencia = $("#txtFechaVigencia").val();
    pAutorizacionIVA.ClaveAutorizacion = $("#txtClaveAutorizacion").val();
    
    var validacion = ValidaAutorizacionIVA(pAutorizacionIVA);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pAutorizacionIVA = pAutorizacionIVA;
    SetEditarAutorizacionIVA(JSON.stringify(oRequest));
}

function SetEditarAutorizacionIVA(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "AutorizacionIVA.aspx/EditarAutorizacionIVA",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAutorizacionIVA").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarAutorizacionIVA").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdAutorizacionIVA, pBaja) {
    var pRequest = "{'pIdAutorizacionIVA':" + pIdAutorizacionIVA + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "AutorizacionIVA.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdAutorizacionIVA").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdAutorizacionIVA').one('click', '.div_grdAutorizacionIVA_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdAutorizacionIVA_AI']").children().attr("baja")
                var idAutorizacionIVA = $(registro).children("td[aria-describedby='grdAutorizacionIVA_IdAutorizacionIVA']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idAutorizacionIVA, baja);
            });
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaAutorizacionIVA(pAutorizacionIVA) {
    var errores = "";

    if (pAutorizacionIVA.IdUsuarioAutorizo == "0")
    { errores = errores + "<span>*</span> El campo usuario autorizó esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionIVA.IdUsuarioSolicito == "0")
    { errores = errores + "<span>*</span> El campo usuario solicitó esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionIVA.FechaVigencia == "")
    { errores = errores + "<span>*</span> El campo fecha vigencia esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionIVA.ClaveAutorizacion == "")
    { errores = errores + "<span>*</span> El campo clave de autorización esta vacío, favor de capturarlo.<br />"; }

    if (pAutorizacionIVA.IVA == "0")
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