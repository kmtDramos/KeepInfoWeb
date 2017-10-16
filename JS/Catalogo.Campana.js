//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Campana");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCampana", function() {
        ObtenerFormaAgregarCampana();
    });

    $("#grdCampana").on("click", ".imgFormaConsultarCampana", function() {
        var registro = $(this).parents("tr");
        var Campana = new Object();
        Campana.pIdCampana = parseInt($(registro).children("td[aria-describedby='grdCampana_IdCampana']").html());
        ObtenerFormaConsultarCampana(JSON.stringify(Campana));
    });

    $('#grdCampana').one('click', '.div_grdCampana_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCampana_AI']").children().attr("baja")
        var idCampana = $(registro).children("td[aria-describedby='grdCampana_IdCampana']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCampana, baja);
    });

    $('#dialogAgregarCampana').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCampana").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCampana();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCampana').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCampana").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCampana').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCampana").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCampana();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCampana()
{
    $("#dialogAgregarCampana").obtenerVista({
        nombreTemplate: "tmplAgregarCampana.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCampana").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCampana(pIdCampana) {
    $("#dialogConsultarCampana").obtenerVista({
        nombreTemplate: "tmplConsultarCampana.html",
        url: "Campana.aspx/ObtenerFormaCampana",
        parametros: pIdCampana,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCampana == 1) {
                $("#dialogConsultarCampana").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Campana = new Object();
                        Campana.IdCampana = parseInt($("#divFormaConsultarCampana").attr("IdCampana"));
                        ObtenerFormaEditarCampana(JSON.stringify(Campana))
                    }
                });
                $("#dialogConsultarCampana").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCampana").dialog("option", "buttons", {});
                $("#dialogConsultarCampana").dialog("option", "height", "100");
            }
            $("#dialogConsultarCampana").dialog("open");
        }
    });
}

function ObtenerFormaEditarCampana(IdCampana) {
    $("#dialogEditarCampana").obtenerVista({
        nombreTemplate: "tmplEditarCampana.html",
        url: "Campana.aspx/ObtenerFormaEditarCampana",
        parametros: IdCampana,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCampana").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCampana() {
    var pCampana = new Object();
    pCampana.Campana = $("#txtCampana").val();
    var validacion = ValidaCampana(pCampana);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCampana = pCampana;
    SetAgregarCampana(JSON.stringify(oRequest)); 
}

function SetAgregarCampana(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Campana.aspx/AgregarCampana",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCampana").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCampana").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdCampana, pBaja) {
    var pRequest = "{'pIdCampana':" + pIdCampana + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Campana.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCampana").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCampana').one('click', '.div_grdCampana_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCampana_AI']").children().attr("baja")
                var idCampana = $(registro).children("td[aria-describedby='grdCampana_IdCampana']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCampana, baja);
            });
        }
    });
}

function EditarCampana() {
    var pCampana = new Object();
    pCampana.IdCampana = $("#divFormaEditarCampana").attr("idCampana");
    pCampana.Campana = $("#txtCampana").val();
    var validacion = ValidaCampana(pCampana);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCampana = pCampana;
    SetEditarCampana(JSON.stringify(oRequest));
}

function SetEditarCampana(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Campana.aspx/EditarCampana",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCampana").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCampana").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaCampana(pCampana) {
    var errores = "";

    if (pCampana.Campana == "")
    { errores = errores + "<span>*</span> El campo Campana esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}