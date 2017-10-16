//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("Estado");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarEstado", function() {
        ObtenerFormaAgregarEstado();
    });

    $("#grdEstado").on("click", ".imgFormaConsultarEstado", function() {
        var registro = $(this).parents("tr");
        var Estado = new Object();
        Estado.pIdEstado = parseInt($(registro).children("td[aria-describedby='grdEstado_IdEstado']").html());
        ObtenerFormaConsultarEstado(JSON.stringify(Estado));
    });

    $('#grdEstado').one('click', '.div_grdEstado_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdEstado_AI']").children().attr("baja")
        var idEstado = $(registro).children("td[aria-describedby='grdEstado_IdEstado']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idEstado, baja);
    });

    $('#dialogAgregarEstado').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarEstado").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarEstado();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarEstado').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEstado").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogEditarEstado').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarEstado").remove();
        },
        buttons: {
            "Editar": function() {
                EditarEstado();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarEstado() {
    $("#dialogAgregarEstado").obtenerVista({
        nombreTemplate: "tmplAgregarEstado.html",
        url: "Estado.aspx/ObtenerFormaAgregarEstado",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarEstado").dialog("open");
            $("#cmbTipoVenta option[value=1]").attr("selected", true);
            $("#cmbTipoMoneda option[value=1]").attr("selected", true); 
        }
    });
}

function ObtenerFormaConsultarEstado(pIdEstado) {
    $("#dialogConsultarEstado").obtenerVista({
        nombreTemplate: "tmplConsultarEstado.html",
        url: "Estado.aspx/ObtenerFormaEstado",
        parametros: pIdEstado,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarEstado == 1) {
                $("#dialogConsultarEstado").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Estado = new Object();
                        Estado.IdEstado = parseInt($("#divFormaConsultarEstado").attr("IdEstado"));
                        ObtenerFormaEditarEstado(JSON.stringify(Estado))
                    }
                });
                $("#dialogConsultarEstado").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarEstado").dialog("option", "buttons", {});
                $("#dialogConsultarEstado").dialog("option", "height", "250");
            }
            $("#dialogConsultarEstado").dialog("open");
        }
    });
}

function ObtenerFormaEditarEstado(IdEstado) {
    $("#dialogEditarEstado").obtenerVista({
        nombreTemplate: "tmplEditarEstado.html",
        url: "Estado.aspx/ObtenerFormaEditarEstado",
        parametros: IdEstado,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarEstado").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarEstado() {
    var pEstado = new Object();
    pEstado.Estado = $("#txtEstado").val();
    pEstado.Abreviatura = $("#txtAbreviatura").val();
    pEstado.Clave = $("#txtClave").val();
    pEstado.IdPais = $("#cmbPais").val();
    
    var validacion = ValidaEstado(pEstado);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEstado = pEstado;
    SetAgregarEstado(JSON.stringify(oRequest));
}

function SetAgregarEstado(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Estado.aspx/AgregarEstado",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEstado").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarEstado").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdEstado, pBaja) {
    var pRequest = "{'pIdEstado':" + pIdEstado + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Estado.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdEstado").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdEstado').one('click', '.div_grdEstado_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEstado_AI']").children().attr("baja")
                var idEstado = $(registro).children("td[aria-describedby='grdEstado_IdEstado']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idEstado, baja);
            });
        }
    });
}

function EditarEstado() {
    var pEstado = new Object();
    pEstado.IdEstado = $("#divFormaEditarEstado").attr("idEstado");
    pEstado.Estado = $("#txtEstado").val();
    pEstado.Abreviatura = $("#txtAbreviatura").val();
    pEstado.Clave = $("#txtClave").val();
    pEstado.IdPais = $("#cmbPais").val();
    
    var validacion = ValidaEstado(pEstado);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEstado = pEstado;
    SetEditarEstado(JSON.stringify(oRequest));
}

function SetEditarEstado(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Estado.aspx/EditarEstado",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEstado").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarEstado").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaEstado(pEstado) {
    var errores = "";

    if (pEstado.Estado == "")
    { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pEstado.Abreviatura == "")
    { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pEstado.Clave == "")
    { errores = errores + "<span>*</span> El campo clave esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pEstado.IdPais == 0)
    { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
