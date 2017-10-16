//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoCuentaContable");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoCuentaContable", function() {
        ObtenerFormaAgregarTipoCuentaContable();
    });

    $("#grdTipoCuentaContable").on("click", ".imgFormaConsultarTipoCuentaContable", function() {
        var registro = $(this).parents("tr");
        var TipoCuentaContable = new Object();
        TipoCuentaContable.pIdTipoCuentaContable = parseInt($(registro).children("td[aria-describedby='grdTipoCuentaContable_IdTipoCuentaContable']").html());
        ObtenerFormaConsultarTipoCuentaContable(JSON.stringify(TipoCuentaContable));
    });

    $('#grdTipoCuentaContable').one('click', '.div_grdTipoCuentaContable_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCuentaContable_AI']").children().attr("baja")
        var idTipoCuentaContable = $(registro).children("td[aria-describedby='grdTipoCuentaContable_IdTipoCuentaContable']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoCuentaContable, baja);
    });

    $('#dialogAgregarTipoCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoCuentaContable").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoCuentaContable();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoCuentaContable").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoCuentaContable").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoCuentaContable();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoCuentaContable()
{
    $("#dialogAgregarTipoCuentaContable").obtenerVista({
        nombreTemplate: "tmplAgregarTipoCuentaContable.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoCuentaContable(pIdTipoCuentaContable) {
    $("#dialogConsultarTipoCuentaContable").obtenerVista({
        nombreTemplate: "tmplConsultarTipoCuentaContable.html",
        url: "TipoCuentaContable.aspx/ObtenerFormaTipoCuentaContable",
        parametros: pIdTipoCuentaContable,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCuentaContable == 1) {
                $("#dialogConsultarTipoCuentaContable").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoCuentaContable = new Object();
                        TipoCuentaContable.IdTipoCuentaContable = parseInt($("#divFormaConsultarTipoCuentaContable").attr("IdTipoCuentaContable"));
                        ObtenerFormaEditarTipoCuentaContable(JSON.stringify(TipoCuentaContable))
                    }
                });
                $("#dialogConsultarTipoCuentaContable").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoCuentaContable").dialog("option", "buttons", {});
                $("#dialogConsultarTipoCuentaContable").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoCuentaContable(IdTipoCuentaContable) {
    $("#dialogEditarTipoCuentaContable").obtenerVista({
        nombreTemplate: "tmplEditarTipoCuentaContable.html",
        url: "TipoCuentaContable.aspx/ObtenerFormaEditarTipoCuentaContable",
        parametros: IdTipoCuentaContable,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoCuentaContable").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoCuentaContable() {
    var pTipoCuentaContable = new Object();
    pTipoCuentaContable.TipoCuentaContable = $("#txtTipoCuentaContable").val();
    var validacion = ValidaTipoCuentaContable(pTipoCuentaContable);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCuentaContable = pTipoCuentaContable;
    SetAgregarTipoCuentaContable(JSON.stringify(oRequest)); 
}

function SetAgregarTipoCuentaContable(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCuentaContable.aspx/AgregarTipoCuentaContable",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCuentaContable").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoCuentaContable").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoCuentaContable, pBaja) {
    var pRequest = "{'pIdTipoCuentaContable':" + pIdTipoCuentaContable + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoCuentaContable.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoCuentaContable").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoCuentaContable').one('click', '.div_grdTipoCuentaContable_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCuentaContable_AI']").children().attr("baja")
                var idTipoCuentaContable = $(registro).children("td[aria-describedby='grdTipoCuentaContable_IdTipoCuentaContable']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoCuentaContable, baja);
            });
        }
    });
}

function EditarTipoCuentaContable() {
    var pTipoCuentaContable = new Object();
    pTipoCuentaContable.IdTipoCuentaContable = $("#divFormaEditarTipoCuentaContable").attr("idTipoCuentaContable");
    pTipoCuentaContable.TipoCuentaContable = $("#txtTipoCuentaContable").val();
    var validacion = ValidaTipoCuentaContable(pTipoCuentaContable);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCuentaContable = pTipoCuentaContable;
    SetEditarTipoCuentaContable(JSON.stringify(oRequest));
}

function SetEditarTipoCuentaContable(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCuentaContable.aspx/EditarTipoCuentaContable",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCuentaContable").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoCuentaContable").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoCuentaContable(pTipoCuentaContable) {
    var errores = "";

    if (pTipoCuentaContable.TipoCuentaContable == "")
    { errores = errores + "<span>*</span> El campo tipo de cuenta contable esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}