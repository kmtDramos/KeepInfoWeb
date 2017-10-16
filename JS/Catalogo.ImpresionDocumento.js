//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("ImpresionDocumento");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarImpresionDocumento", function() {
        ObtenerFormaAgregarImpresionDocumento();
    });

    $("#grdImpresionDocumento").on("click", ".imgFormaConsultarImpresionDocumento", function() {
        var registro = $(this).parents("tr");
        var ImpresionDocumento = new Object();
        ImpresionDocumento.pIdImpresionDocumento = parseInt($(registro).children("td[aria-describedby='grdImpresionDocumento_IdImpresionDocumento']").html());
        ObtenerFormaConsultarImpresionDocumento(JSON.stringify(ImpresionDocumento));
    });

    $('#grdImpresionDocumento').on('click', '.div_grdImpresionDocumento_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdImpresionDocumento_AI']").children().attr("baja")
        var idImpresionDocumento = $(registro).children("td[aria-describedby='grdImpresionDocumento_IdImpresionDocumento']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idImpresionDocumento, baja);
    });

    $('#dialogAgregarImpresionDocumento').dialog({
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
            $("#divFormaAgregarImpresionDocumento").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarImpresionDocumento();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarImpresionDocumento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarImpresionDocumento").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarImpresionDocumento').dialog({
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
            $("#divFormaEditarImpresionDocumento").remove();
        },
        buttons: {
            "Editar": function() {
                EditarImpresionDocumento();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarImpresionDocumento() {
    $("#dialogAgregarImpresionDocumento").obtenerVista({
        nombreTemplate: "tmplAgregarImpresionDocumento.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarImpresionDocumento").dialog("open");
        }
    });
}

function ObtenerFormaConsultarImpresionDocumento(pIdImpresionDocumento) {
    $("#dialogConsultarImpresionDocumento").obtenerVista({
        nombreTemplate: "tmplConsultarImpresionDocumento.html",
        url: "ImpresionDocumento.aspx/ObtenerFormaImpresionDocumento",
        parametros: pIdImpresionDocumento,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarImpresionDocumento == 1) {
                $("#dialogConsultarImpresionDocumento").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var ImpresionDocumento = new Object();
                        ImpresionDocumento.IdImpresionDocumento = parseInt($("#divFormaConsultarImpresionDocumento").attr("IdImpresionDocumento"));
                        ObtenerFormaEditarImpresionDocumento(JSON.stringify(ImpresionDocumento))
                    }
                });
                $("#dialogConsultarImpresionDocumento").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarImpresionDocumento").dialog("option", "buttons", {});
                $("#dialogConsultarImpresionDocumento").dialog("option", "height", "100");
            }
            $("#dialogConsultarImpresionDocumento").dialog("open");
        }
    });
}

function ObtenerFormaEditarImpresionDocumento(IdImpresionDocumento) {
    $("#dialogEditarImpresionDocumento").obtenerVista({
        nombreTemplate: "tmplEditarImpresionDocumento.html",
        url: "ImpresionDocumento.aspx/ObtenerFormaEditarImpresionDocumento",
        parametros: IdImpresionDocumento,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarImpresionDocumento").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarImpresionDocumento() {
    var pImpresionDocumento = new Object();
    pImpresionDocumento.ImpresionDocumento = $("#txtImpresionDocumento").val();
    pImpresionDocumento.Procedimiento = $("#txtProcedimiento").val();
    var validacion = ValidaImpresionDocumento(pImpresionDocumento);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pImpresionDocumento = pImpresionDocumento;
    SetAgregarImpresionDocumento(JSON.stringify(oRequest));
}

function SetAgregarImpresionDocumento(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "ImpresionDocumento.aspx/AgregarImpresionDocumento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdImpresionDocumento").trigger("reloadGrid");
                $("#dialogAgregarImpresionDocumento").dialog("close");
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

function SetCambiarEstatus(pIdImpresionDocumento, pBaja) {
    var pRequest = "{'pIdImpresionDocumento':" + pIdImpresionDocumento + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "ImpresionDocumento.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdImpresionDocumento").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdImpresionDocumento').one('click', '.div_grdImpresionDocumento_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdImpresionDocumento_AI']").children().attr("baja")
                var idImpresionDocumento = $(registro).children("td[aria-describedby='grdImpresionDocumento_IdImpresionDocumento']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idImpresionDocumento, baja);
            });
        }
    });
}

function EditarImpresionDocumento() {
    var pImpresionDocumento = new Object();
    pImpresionDocumento.IdImpresionDocumento = $("#divFormaEditarImpresionDocumento").attr("idImpresionDocumento");
    pImpresionDocumento.ImpresionDocumento = $("#txtImpresionDocumento").val();
    pImpresionDocumento.Procedimiento = $("#txtProcedimiento").val();
    var validacion = ValidaImpresionDocumento(pImpresionDocumento);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pImpresionDocumento = pImpresionDocumento;
    SetEditarImpresionDocumento(JSON.stringify(oRequest));
}
function SetEditarImpresionDocumento(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "ImpresionDocumento.aspx/EditarImpresionDocumento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdImpresionDocumento").trigger("reloadGrid");
                $("#dialogEditarImpresionDocumento").dialog("close");
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
function ValidaImpresionDocumento(pImpresionDocumento) {
    var errores = "";

    if (pImpresionDocumento.ImpresionDocumento == "")
    { errores = errores + "<span>*</span> El campo impresión de documento esta vacío, favor de capturarlo.<br />"; }

    if (pImpresionDocumento.Procedimiento == "")
    { errores = errores + "<span>*</span> El campo de procedimiento esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
