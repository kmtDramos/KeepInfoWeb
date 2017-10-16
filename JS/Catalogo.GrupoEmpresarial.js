//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("GrupoEmpresarial");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarGrupoEmpresarial", function() {
        ObtenerFormaAgregarGrupoEmpresarial();
    });

    $("#grdGrupoEmpresarial").on("click", ".imgFormaConsultarGrupoEmpresarial", function() {
        var registro = $(this).parents("tr");
        var GrupoEmpresarial = new Object();
        GrupoEmpresarial.pIdGrupoEmpresarial = parseInt($(registro).children("td[aria-describedby='grdGrupoEmpresarial_IdGrupoEmpresarial']").html());
        ObtenerFormaConsultarGrupoEmpresarial(JSON.stringify(GrupoEmpresarial));
    });

    $('#grdGrupoEmpresarial').on('click', '.div_grdGrupoEmpresarial_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdGrupoEmpresarial_AI']").children().attr("baja")
        var idGrupoEmpresarial = $(registro).children("td[aria-describedby='grdGrupoEmpresarial_IdGrupoEmpresarial']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idGrupoEmpresarial, baja);
    });

    $('#dialogAgregarGrupoEmpresarial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
        },
        close: function() {
            $("#divFormaAgregarGrupoEmpresarial").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarGrupoEmpresarial();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarGrupoEmpresarial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarGrupoEmpresarial").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarGrupoEmpresarial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
        },
        close: function() {
            $("#divFormaEditarGrupoEmpresarial").remove();
        },
        buttons: {
            "Guardar": function() {
                EditarGrupoEmpresarial();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarGrupoEmpresarial() {
    $("#dialogAgregarGrupoEmpresarial").obtenerVista({
        nombreTemplate: "tmplAgregarGrupoEmpresarial.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarGrupoEmpresarial").dialog("open");
        }
    });
}

function ObtenerFormaConsultarGrupoEmpresarial(pIdGrupoEmpresarial) {
    $("#dialogConsultarGrupoEmpresarial").obtenerVista({
        nombreTemplate: "tmplConsultarGrupoEmpresarial.html",
        url: "GrupoEmpresarial.aspx/ObtenerFormaGrupoEmpresarial",
        parametros: pIdGrupoEmpresarial,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarGrupoEmpresarial == 1) {
                $("#dialogConsultarGrupoEmpresarial").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var GrupoEmpresarial = new Object();
                        GrupoEmpresarial.IdGrupoEmpresarial = parseInt($("#divFormaConsultarGrupoEmpresarial").attr("IdGrupoEmpresarial"));
                        ObtenerFormaEditarGrupoEmpresarial(JSON.stringify(GrupoEmpresarial))
                    }
                });
                $("#dialogConsultarGrupoEmpresarial").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarGrupoEmpresarial").dialog("option", "buttons", {});
                $("#dialogConsultarGrupoEmpresarial").dialog("option", "height", "100");
            }
            $("#dialogConsultarGrupoEmpresarial").dialog("open");
        }
    });
}

function ObtenerFormaEditarGrupoEmpresarial(IdGrupoEmpresarial) {
    $("#dialogEditarGrupoEmpresarial").obtenerVista({
        nombreTemplate: "tmplEditarGrupoEmpresarial.html",
        url: "GrupoEmpresarial.aspx/ObtenerFormaEditarGrupoEmpresarial",
        parametros: IdGrupoEmpresarial,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarGrupoEmpresarial").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarGrupoEmpresarial() {
    var pGrupoEmpresarial = new Object();
    pGrupoEmpresarial.GrupoEmpresarial = $("#txtGrupoEmpresarial").val();
    var validacion = ValidaGrupoEmpresarial(pGrupoEmpresarial);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pGrupoEmpresarial = pGrupoEmpresarial;
    SetAgregarGrupoEmpresarial(JSON.stringify(oRequest));
}

function SetAgregarGrupoEmpresarial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "GrupoEmpresarial.aspx/AgregarGrupoEmpresarial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdGrupoEmpresarial").trigger("reloadGrid");
                $("#dialogAgregarGrupoEmpresarial").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetCambiarEstatus(pIdGrupoEmpresarial, pBaja) {
    var pRequest = "{'pIdGrupoEmpresarial':" + pIdGrupoEmpresarial + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "GrupoEmpresarial.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdGrupoEmpresarial").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdGrupoEmpresarial').one('click', '.div_grdGrupoEmpresarial_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdGrupoEmpresarial_AI']").children().attr("baja")
                var idGrupoEmpresarial = $(registro).children("td[aria-describedby='grdGrupoEmpresarial_IdGrupoEmpresarial']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idGrupoEmpresarial, baja);
            });
        }
    });
}

function EditarGrupoEmpresarial() {
    var pGrupoEmpresarial = new Object();
    pGrupoEmpresarial.IdGrupoEmpresarial = $("#divFormaEditarGrupoEmpresarial").attr("idGrupoEmpresarial");
    pGrupoEmpresarial.GrupoEmpresarial = $("#txtGrupoEmpresarial").val();
    var validacion = ValidaGrupoEmpresarial(pGrupoEmpresarial);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pGrupoEmpresarial = pGrupoEmpresarial;
    SetEditarGrupoEmpresarial(JSON.stringify(oRequest));
}
function SetEditarGrupoEmpresarial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "GrupoEmpresarial.aspx/EditarGrupoEmpresarial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdGrupoEmpresarial").trigger("reloadGrid");
                $("#dialogEditarGrupoEmpresarial").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaGrupoEmpresarial(pGrupoEmpresarial) {
    var errores = "";

    if (pGrupoEmpresarial.GrupoEmpresarial == "")
    { errores = errores + "<span>*</span> El campo grupo empresarial esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}