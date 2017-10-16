//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("ImpresionEtiquetas");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarImpresionEtiquetas", function() {
        ObtenerFormaAgregarImpresionEtiquetas();
    });

    $("#grdImpresionEtiquetas").on("click", ".imgFormaConsultarImpresionEtiquetas", function() {
        var registro = $(this).parents("tr");
        var ImpresionEtiquetas = new Object();
        ImpresionEtiquetas.pIdImpresionEtiquetas = parseInt($(registro).children("td[aria-describedby='grdImpresionEtiquetas_IdImpresionEtiquetas']").html());
        ObtenerFormaConsultarImpresionEtiquetas(JSON.stringify(ImpresionEtiquetas));
    });

    $('#grdImpresionEtiquetas').on('click', '.div_grdImpresionEtiquetas_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdImpresionEtiquetas_AI']").children().attr("baja")
        var idImpresionEtiquetas = $(registro).children("td[aria-describedby='grdImpresionEtiquetas_IdImpresionEtiquetas']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idImpresionEtiquetas, baja);
    });

    $('#dialogAgregarImpresionEtiquetas').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("#txtEtiqueta").regexMask(/^[a-zA-Z]+$/);
            $("#txtEtiqueta").on('keyup', function() {
                $("#txtEtiqueta").val($("#txtEtiqueta").val().toUpperCase());
            });
        },
        close: function() {
            $("#divFormaAgregarImpresionEtiquetas").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarImpresionEtiquetas();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarImpresionEtiquetas').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarImpresionEtiquetas").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarImpresionEtiquetas').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("#txtEtiqueta").regexMask(/^[a-zA-Z]+$/);
            $("#txtEtiqueta").on('keyup', function() {
                $("#txtEtiqueta").val($("#txtEtiqueta").val().toUpperCase());
            });
        },
        close: function() {
            $("#divFormaEditarImpresionEtiquetas").remove();
        },
        buttons: {
            "Editar": function() {
                EditarImpresionEtiquetas();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

    
//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarImpresionEtiquetas() {

    $("#dialogAgregarImpresionEtiquetas").obtenerVista({
        url: "ImpresionEtiquetas.aspx/ObtenerFormaAgregarImpresionEtiquetas",
        nombreTemplate: "tmplAgregarImpresionEtiquetas.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarImpresionEtiquetas").dialog("open");
        }
    });
}

function ObtenerFormaConsultarImpresionEtiquetas(pIdImpresionEtiquetas) {
    $("#dialogConsultarImpresionEtiquetas").obtenerVista({
        nombreTemplate: "tmplConsultarImpresionEtiquetas.html",
        url: "ImpresionEtiquetas.aspx/ObtenerFormaImpresionEtiquetas",
        parametros: pIdImpresionEtiquetas,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarImpresionEtiquetas == 1) {
                $("#dialogConsultarImpresionEtiquetas").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var ImpresionEtiquetas = new Object();
                        ImpresionEtiquetas.IdImpresionEtiquetas = parseInt($("#divFormaConsultarImpresionEtiquetas").attr("IdImpresionEtiquetas"));
                        ObtenerFormaEditarImpresionEtiquetas(JSON.stringify(ImpresionEtiquetas))
                    }
                });
                $("#dialogConsultarImpresionEtiquetas").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarImpresionEtiquetas").dialog("option", "buttons", {});
                $("#dialogConsultarImpresionEtiquetas").dialog("option", "height", "100");
            }
            $("#dialogConsultarImpresionEtiquetas").dialog("open");
        }
    });
}

function ObtenerFormaEditarImpresionEtiquetas(IdImpresionEtiquetas) {
    $("#dialogEditarImpresionEtiquetas").obtenerVista({
        nombreTemplate: "tmplEditarImpresionEtiquetas.html",
        url: "ImpresionEtiquetas.aspx/ObtenerFormaEditarImpresionEtiquetas",
        parametros: IdImpresionEtiquetas,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarImpresionEtiquetas").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarImpresionEtiquetas() {
    var pImpresionEtiquetas = new Object();
    pImpresionEtiquetas.IdImpresionTemplate = $("#cmbTemplate").val();
    pImpresionEtiquetas.Campo = $("#txtCampo").val();
    pImpresionEtiquetas.Etiqueta = "{"+$("#txtEtiqueta").val()+"}";

    var validacion = ValidaImpresionEtiquetas(pImpresionEtiquetas);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pImpresionEtiquetas = pImpresionEtiquetas;
    SetAgregarImpresionEtiquetas(JSON.stringify(oRequest)); 
}

function SetAgregarImpresionEtiquetas(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "ImpresionEtiquetas.aspx/AgregarImpresionEtiquetas",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdImpresionEtiquetas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarImpresionEtiquetas").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdImpresionEtiquetas, pBaja) {
    var pRequest = "{'pIdImpresionEtiquetas':" + pIdImpresionEtiquetas + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "ImpresionEtiquetas.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdImpresionEtiquetas").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdImpresionEtiquetas').one('click', '.div_grdImpresionEtiquetas_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdImpresionEtiquetas_AI']").children().attr("baja")
                var idImpresionEtiquetas = $(registro).children("td[aria-describedby='grdImpresionEtiquetas_IdImpresionEtiquetas']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idImpresionEtiquetas, baja);
            });
        }
    });
}

function EditarImpresionEtiquetas() {
    var pImpresionEtiquetas = new Object();
    pImpresionEtiquetas.IdImpresionEtiquetas = $("#divFormaEditarImpresionEtiquetas").attr("IdImpresionEtiquetas");
    pImpresionEtiquetas.IdImpresionTemplate = $("#cmbTemplate").val();
    pImpresionEtiquetas.Campo = $("#txtCampo").val();
    pImpresionEtiquetas.Etiqueta = "{" + $("#txtEtiqueta").val() + "}";
    
    var validacion = ValidaImpresionEtiquetas(pImpresionEtiquetas);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pImpresionEtiquetas = pImpresionEtiquetas;
    SetEditarImpresionEtiquetas(JSON.stringify(oRequest));
}

function SetEditarImpresionEtiquetas(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "ImpresionEtiquetas.aspx/EditarImpresionEtiquetas",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdImpresionEtiquetas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarImpresionEtiquetas").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaImpresionEtiquetas(pImpresionEtiquetas) {
    var errores = "";
    
    if (pImpresionEtiquetas.IdImpresionTemplate == "0")
    { errores = errores + "<span>*</span> El campo impresión template esta vacío, favor de capturarlo.<br />"; }

    if (pImpresionEtiquetas.Campo == "")
    { errores = errores + "<span>*</span> El campo esta vacío, favor de capturarlo.<br />"; }

    if (pImpresionEtiquetas.Etiqueta == "")
    { errores = errores + "<span>*</span> El campo etiqueta esta vacío, favor de capturarlo.<br />"; }
               
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
