//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoMoneda");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoMoneda", function() {
        ObtenerFormaAgregarTipoMoneda();
    });
    
    $("#grdTipoMoneda").on("click", ".imgFormaConsultarTipoMoneda", function() {
        var registro = $(this).parents("tr");
        var TipoMoneda = new Object();
        TipoMoneda.pIdTipoMoneda = parseInt($(registro).children("td[aria-describedby='grdTipoMoneda_IdTipoMoneda']").html());
        ObtenerFormaConsultarTipoMoneda(JSON.stringify(TipoMoneda));
    });
    
    $('#grdTipoMoneda').one('click', '.div_grdTipoMoneda_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoMoneda_AI']").children().attr("baja")
        var idTipoMoneda = $(registro).children("td[aria-describedby='grdTipoMoneda_IdTipoMoneda']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoMoneda, baja);
    });
    
    $('#dialogAgregarTipoMoneda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoMoneda").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoMoneda();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarTipoMoneda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoMoneda").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarTipoMoneda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoMoneda").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoMoneda();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoMoneda()
{
    $("#dialogAgregarTipoMoneda").obtenerVista({
        nombreTemplate: "tmplAgregarTipoMoneda.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoMoneda").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoMoneda(pIdTipoMoneda) {
    $("#dialogConsultarTipoMoneda").obtenerVista({
        nombreTemplate: "tmplConsultarTipoMoneda.html",
        url: "TipoMoneda.aspx/ObtenerFormaTipoMoneda",
        parametros: pIdTipoMoneda,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoMoneda == 1) {
                $("#dialogConsultarTipoMoneda").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoMoneda = new Object();
                        TipoMoneda.IdTipoMoneda = parseInt($("#divFormaConsultarTipoMoneda").attr("IdTipoMoneda"));
                        ObtenerFormaEditarTipoMoneda(JSON.stringify(TipoMoneda))
                    }
                });
                $("#dialogConsultarTipoMoneda").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoMoneda").dialog("option", "buttons", {});
                $("#dialogConsultarTipoMoneda").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoMoneda").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoMoneda(IdTipoMoneda) {
    $("#dialogEditarTipoMoneda").obtenerVista({
        nombreTemplate: "tmplEditarTipoMoneda.html",
        url: "TipoMoneda.aspx/ObtenerFormaEditarTipoMoneda",
        parametros: IdTipoMoneda,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoMoneda").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoMoneda() {
    var pTipoMoneda = new Object();
    pTipoMoneda.TipoMoneda = $("#txtTipoMoneda").val();
    pTipoMoneda.Simbolo    = $("#txtSimbolo").val();
    
    var validacion = ValidaTipoMoneda(pTipoMoneda);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoMoneda = pTipoMoneda;    
    SetAgregarTipoMoneda(JSON.stringify(oRequest)); 
}

function SetAgregarTipoMoneda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoMoneda.aspx/AgregarTipoMoneda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoMoneda").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoMoneda").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoMoneda, pBaja) {
    var pRequest = "{'pIdTipoMoneda':" + pIdTipoMoneda + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoMoneda.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoMoneda").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoMoneda').one('click', '.div_grdTipoMoneda_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoMoneda_AI']").children().attr("baja")
                var idTipoMoneda = $(registro).children("td[aria-describedby='grdTipoMoneda_IdTipoMoneda']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoMoneda, baja);
            });
        }
    });
}

function EditarTipoMoneda() {
    var pTipoMoneda = new Object();
    pTipoMoneda.IdTipoMoneda = $("#divFormaEditarTipoMoneda").attr("idTipoMoneda");
    pTipoMoneda.TipoMoneda = $("#txtTipoMoneda").val();
    pTipoMoneda.Simbolo    = $("#txtSimbolo").val();
    
    var validacion = ValidaTipoMoneda(pTipoMoneda);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoMoneda = pTipoMoneda;
    SetEditarTipoMoneda(JSON.stringify(oRequest));
}

function SetEditarTipoMoneda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoMoneda.aspx/EditarTipoMoneda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoMoneda").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoMoneda").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoMoneda(pTipoMoneda) {
    var errores = "";

    if (pTipoMoneda.TipoMoneda == "")
    { errores = errores + "<span>*</span> El campo TipoMoneda esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
