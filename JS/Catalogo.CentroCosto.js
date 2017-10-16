//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("CentroCosto");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCentroCosto", function() {
        ObtenerFormaAgregarCentroCosto();
    });

    $("#grdCentroCosto").on("click", ".imgFormaConsultarCentroCosto", function() {
        var registro = $(this).parents("tr");
        var CentroCosto = new Object();
        CentroCosto.pIdCentroCosto = parseInt($(registro).children("td[aria-describedby='grdCentroCosto_IdCentroCosto']").html());
        ObtenerFormaConsultarCentroCosto(JSON.stringify(CentroCosto));
    });

    $('#grdCentroCosto').on('click', '.div_grdCentroCosto_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCentroCosto_AI']").children().attr("baja")
        var idCentroCosto = $(registro).children("td[aria-describedby='grdCentroCosto_IdCentroCosto']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCentroCosto, baja);
    });

    $('#dialogAgregarCentroCosto').dialog({
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
            $("#divFormaAgregarCentroCosto").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCentroCosto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCentroCosto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCentroCosto").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCentroCosto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCentroCosto").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCentroCosto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCentroCosto() {
    $("#dialogAgregarCentroCosto").obtenerVista({
        nombreTemplate: "tmplAgregarCentroCosto.html",
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarCuentaContable();
            $("#dialogAgregarCentroCosto").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCentroCosto(pIdCentroCosto) {
    $("#dialogConsultarCentroCosto").obtenerVista({
        nombreTemplate: "tmplConsultarCentroCosto.html",
        url: "CentroCosto.aspx/ObtenerFormaCentroCosto",
        parametros: pIdCentroCosto,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCentroCosto == 1) {
                $("#dialogConsultarCentroCosto").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var CentroCosto = new Object();
                        CentroCosto.IdCentroCosto = parseInt($("#divFormaConsultarCentroCosto").attr("IdCentroCosto"));
                        ObtenerFormaEditarCentroCosto(JSON.stringify(CentroCosto))
                    }
                });
                $("#dialogConsultarCentroCosto").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCentroCosto").dialog("option", "buttons", {});
                $("#dialogConsultarCentroCosto").dialog("option", "height", "100");
            }
            $("#dialogConsultarCentroCosto").dialog("open");
        }
    });
}

function ObtenerFormaEditarCentroCosto(IdCentroCosto) {
    $("#dialogEditarCentroCosto").obtenerVista({
        nombreTemplate: "tmplEditarCentroCosto.html",
        url: "CentroCosto.aspx/ObtenerFormaEditarCentroCosto",
        parametros: IdCentroCosto,
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarCuentaContable();
            $("#dialogEditarCentroCosto").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCentroCosto() {
    var pCentroCosto = new Object();
    pCentroCosto.CentroCosto = $("#txtCentroCosto").val();
    pCentroCosto.Monto = $("#txtMonto").val();
    pCentroCosto.Descripcion = $("#txtDescripcion").val();
    pCentroCosto.IdCuentaContable = $("#divFormaAgregarCentroCosto").attr("idCuentaContable");
    pCentroCosto.CuentaContable = $("#txtCuentaContable").val();
    var validacion = ValidaCentroCosto(pCentroCosto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCentroCosto = pCentroCosto;
    SetAgregarCentroCosto(JSON.stringify(oRequest));
}

function SetAgregarCentroCosto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CentroCosto.aspx/AgregarCentroCosto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCentroCosto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCentroCosto").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdCentroCosto, pBaja) {
    var pRequest = "{'pIdCentroCosto':" + pIdCentroCosto + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "CentroCosto.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCentroCosto").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCentroCosto').one('click', '.div_grdCentroCosto_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCentroCosto_AI']").children().attr("baja")
                var idCentroCosto = $(registro).children("td[aria-describedby='grdCentroCosto_IdCentroCosto']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCentroCosto, baja);
            });
        }
    });
}

function EditarCentroCosto() {
    var pCentroCosto = new Object();
    pCentroCosto.IdCentroCosto = $("#divFormaEditarCentroCosto").attr("idCentroCosto");
    pCentroCosto.CentroCosto = $("#txtCentroCosto").val();
    pCentroCosto.Monto = $("#txtMonto").val();
    pCentroCosto.Descripcion = $("#txtDescripcion").val();
    pCentroCosto.IdCuentaContable = $("#divFormaEditarCentroCosto").attr("idCuentaContable");
    pCentroCosto.CuentaContable = $("#txtCuentaContable").val();
    var validacion = ValidaCentroCosto(pCentroCosto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCentroCosto = pCentroCosto;
    SetEditarCentroCosto(JSON.stringify(oRequest));
}
function SetEditarCentroCosto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CentroCosto.aspx/EditarCentroCosto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCentroCosto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCentroCosto").dialog("close");
        }
    });
}

function AutocompletarCuentaContable() {

    $('#txtCuentaContable').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pCuentaContable = $('#txtCuentaContable').val();
            $.ajax({
                type: 'POST',
                url: 'CentroCosto.aspx/BuscarCuentaContable',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarCentroCosto, #divFormaEditarCentroCosto").attr("idCuentaContable", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.CuentaContable, value: item.CuentaContable, id: item.IdCuentaContable }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdCuentaContable = ui.item.id;
            $("#divFormaAgregarCentroCosto, #divFormaEditarCentroCosto").attr("idCuentaContable", pIdCuentaContable);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaCentroCosto(pCentroCosto) {
    var errores = "";

    if (pCentroCosto.CentroCosto == "")
    { errores = errores + "<span>*</span> El nombre del centro de costo esta vacío, favor de capturarlo.<br />"; }

    if (pCentroCosto.Monto == 0)
    { errores = errores + "<span>*</span> El monto esta vacío, favor de capturarlo.<br />"; }

    if (pCentroCosto.IdCuentaContable == 0)
    { errores = errores + "<span>*</span> La cuenta contable esta vacia, favor de capturarla.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
