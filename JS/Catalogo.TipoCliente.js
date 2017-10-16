//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoCliente");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoCliente", function() {
        ObtenerFormaAgregarTipoCliente();
    });

    $("#grdTipoCliente").on("click", ".imgFormaConsultarTipoCliente", function() {
        var registro = $(this).parents("tr");
        var TipoCliente = new Object();
        TipoCliente.pIdTipoCliente = parseInt($(registro).children("td[aria-describedby='grdTipoCliente_IdTipoCliente']").html());
        ObtenerFormaConsultarTipoCliente(JSON.stringify(TipoCliente));
    });

    $('#grdTipoCliente').one('click', '.div_grdTipoCliente_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCliente_AI']").children().attr("baja")
        var idTipoCliente = $(registro).children("td[aria-describedby='grdTipoCliente_IdTipoCliente']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoCliente, baja);
    });

    $('#dialogAgregarTipoCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoCliente").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoCliente();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoCliente").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoCliente").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoCliente();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoCliente()
{
    $("#dialogAgregarTipoCliente").obtenerVista({
        nombreTemplate: "tmplAgregarTipoCliente.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoCliente").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoCliente(pIdTipoCliente) {
    $("#dialogConsultarTipoCliente").obtenerVista({
        nombreTemplate: "tmplConsultarTipoCliente.html",
        url: "TipoCliente.aspx/ObtenerFormaTipoCliente",
        parametros: pIdTipoCliente,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCliente == 1) {
                $("#dialogConsultarTipoCliente").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoCliente = new Object();
                        TipoCliente.IdTipoCliente = parseInt($("#divFormaConsultarTipoCliente").attr("IdTipoCliente"));
                        ObtenerFormaEditarTipoCliente(JSON.stringify(TipoCliente))
                    }
                });
                $("#dialogConsultarTipoCliente").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoCliente").dialog("option", "buttons", {});
                $("#dialogConsultarTipoCliente").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoCliente").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoCliente(IdTipoCliente) {
    $("#dialogEditarTipoCliente").obtenerVista({
        nombreTemplate: "tmplEditarTipoCliente.html",
        url: "TipoCliente.aspx/ObtenerFormaEditarTipoCliente",
        parametros: IdTipoCliente,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoCliente").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoCliente() {
    var pTipoCliente = new Object();
    pTipoCliente.TipoCliente = $("#txtTipoCliente").val();
    var validacion = ValidaTipoCliente(pTipoCliente);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCliente = pTipoCliente;
    SetAgregarTipoCliente(JSON.stringify(oRequest)); 
}

function SetAgregarTipoCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCliente.aspx/AgregarTipoCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCliente").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoCliente").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoCliente, pBaja) {
    var pRequest = "{'pIdTipoCliente':" + pIdTipoCliente + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoCliente.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoCliente").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoCliente').one('click', '.div_grdTipoCliente_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCliente_AI']").children().attr("baja")
                var idTipoCliente = $(registro).children("td[aria-describedby='grdTipoCliente_IdTipoCliente']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoCliente, baja);
            });
        }
    });
}

function EditarTipoCliente() {
    var pTipoCliente = new Object();
    pTipoCliente.IdTipoCliente = $("#divFormaEditarTipoCliente").attr("idTipoCliente");
    pTipoCliente.TipoCliente = $("#txtTipoCliente").val();
    var validacion = ValidaTipoCliente(pTipoCliente);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCliente = pTipoCliente;
    SetEditarTipoCliente(JSON.stringify(oRequest));
}

function SetEditarTipoCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCliente.aspx/EditarTipoCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCliente").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoCliente").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoCliente(pTipoCliente) {
    var errores = "";

    if (pTipoCliente.TipoCliente == "")
    { errores = errores + "<span>*</span> El campo TipoCliente esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}