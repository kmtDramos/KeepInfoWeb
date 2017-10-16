//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoCompra");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoCompra", function() {
        ObtenerFormaAgregarTipoCompra();
    });

    $("#grdTipoCompra").on("click", ".imgFormaConsultarTipoCompra", function() {
        var registro = $(this).parents("tr");
        var TipoCompra = new Object();
        TipoCompra.pIdTipoCompra = parseInt($(registro).children("td[aria-describedby='grdTipoCompra_IdTipoCompra']").html());
        ObtenerFormaConsultarTipoCompra(JSON.stringify(TipoCompra));
    });

    $('#grdTipoCompra').on('click', '.div_grdTipoCompra_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCompra_AI']").children().attr("baja")
        var idTipoCompra = $(registro).children("td[aria-describedby='grdTipoCompra_IdTipoCompra']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoCompra, baja);
    });

    $('#dialogAgregarTipoCompra').dialog({
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
            $("#divFormaAgregarTipoCompra").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoCompra();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarTipoCompra').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoCompra").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarTipoCompra').dialog({
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
            $("#divFormaEditarTipoCompra").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoCompra();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoCompra() {
    $("#dialogAgregarTipoCompra").obtenerVista({
        nombreTemplate: "tmplAgregarTipoCompra.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoCompra").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoCompra(pIdTipoCompra) {
    $("#dialogConsultarTipoCompra").obtenerVista({
        nombreTemplate: "tmplConsultarTipoCompra.html",
        url: "TipoCompra.aspx/ObtenerFormaTipoCompra",
        parametros: pIdTipoCompra,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCompra == 1) {
                $("#dialogConsultarTipoCompra").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoCompra = new Object();
                        TipoCompra.IdTipoCompra = parseInt($("#divFormaConsultarTipoCompra").attr("IdTipoCompra"));
                        ObtenerFormaEditarTipoCompra(JSON.stringify(TipoCompra))
                    }
                });
                $("#dialogConsultarTipoCompra").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoCompra").dialog("option", "buttons", {});
                $("#dialogConsultarTipoCompra").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoCompra").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoCompra(IdTipoCompra) {
    $("#dialogEditarTipoCompra").obtenerVista({
        nombreTemplate: "tmplEditarTipoCompra.html",
        url: "TipoCompra.aspx/ObtenerFormaEditarTipoCompra",
        parametros: IdTipoCompra,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoCompra").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoCompra() {
    var pTipoCompra = new Object();
    pTipoCompra.TipoCompra = $("#txtTipoCompra").val();
    pTipoCompra.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    var validacion = ValidaTipoCompra(pTipoCompra);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCompra = pTipoCompra;
    SetAgregarTipoCompra(JSON.stringify(oRequest));
}

function SetAgregarTipoCompra(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCompra.aspx/AgregarTipoCompra",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCompra").trigger("reloadGrid");
                $("#dialogAgregarTipoCompra").dialog("close");
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

function SetCambiarEstatus(pIdTipoCompra, pBaja) {
    var pRequest = "{'pIdTipoCompra':" + pIdTipoCompra + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoCompra.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoCompra").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoCompra').one('click', '.div_grdTipoCompra_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCompra_AI']").children().attr("baja")
                var idTipoCompra = $(registro).children("td[aria-describedby='grdTipoCompra_IdTipoCompra']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoCompra, baja);
            });
        }
    });
}

function EditarTipoCompra() {
    var pTipoCompra = new Object();
    pTipoCompra.IdTipoCompra = $("#divFormaEditarTipoCompra").attr("idTipoCompra");
    pTipoCompra.TipoCompra = $("#txtTipoCompra").val();
    pTipoCompra.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    var validacion = ValidaTipoCompra(pTipoCompra);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCompra = pTipoCompra;
    SetEditarTipoCompra(JSON.stringify(oRequest));
}
function SetEditarTipoCompra(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCompra.aspx/EditarTipoCompra",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCompra").trigger("reloadGrid");
                $("#dialogEditarTipoCompra").dialog("close");
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
function ValidaTipoCompra(pTipoCompra) {
    var errores = "";

    if (pTipoCompra.TipoCompra == "")
    { errores = errores + "<span>*</span> El campo tipo de compra esta vacío, favor de capturarlo.<br />"; }

    if (pTipoCompra.ClaveCuentaContable == "")
    { errores = errores + "<span>*</span> El campo de la clave de cuenta contable esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
