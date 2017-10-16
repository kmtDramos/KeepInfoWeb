//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("SubCuentaContable");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSubCuentaContable", function() {
        ObtenerFormaAgregarSubCuentaContable();
    });

    $("#grdSubCuentaContable").on("click", ".imgFormaConsultarSubCuentaContable", function() {
        var registro = $(this).parents("tr");
        var SubCuentaContable = new Object();
        SubCuentaContable.pIdSubCuentaContable = parseInt($(registro).children("td[aria-describedby='grdSubCuentaContable_IdSubCuentaContable']").html());
        ObtenerFormaConsultarSubCuentaContable(JSON.stringify(SubCuentaContable));
    });

    $('#grdSubCuentaContable').on('click', '.div_grdSubCuentaContable_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdSubCuentaContable_AI']").children().attr("baja")
        var idSubCuentaContable = $(registro).children("td[aria-describedby='grdSubCuentaContable_IdSubCuentaContable']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idSubCuentaContable, baja);
    });

    $('#dialogAgregarSubCuentaContable').dialog({
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
            $("#divFormaAgregarSubCuentaContable").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarSubCuentaContable();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarSubCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarSubCuentaContable").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarSubCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarSubCuentaContable").remove();
        },
        buttons: {
            "Editar": function() {
                EditarSubCuentaContable();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarSubCuentaContable() {
    $("#dialogAgregarSubCuentaContable").obtenerVista({
        nombreTemplate: "tmplAgregarSubCuentaContable.html",
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarCuentaContable();
            $("#dialogAgregarSubCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaConsultarSubCuentaContable(pIdSubCuentaContable) {
    $("#dialogConsultarSubCuentaContable").obtenerVista({
        nombreTemplate: "tmplConsultarSubCuentaContable.html",
        url: "SubCuentaContable.aspx/ObtenerFormaSubCuentaContable",
        parametros: pIdSubCuentaContable,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarSubCuentaContable == 1) {
                $("#dialogConsultarSubCuentaContable").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var SubCuentaContable = new Object();
                        SubCuentaContable.IdSubCuentaContable = parseInt($("#divFormaConsultarSubCuentaContable").attr("IdSubCuentaContable"));
                        ObtenerFormaEditarSubCuentaContable(JSON.stringify(SubCuentaContable))
                    }
                });
                $("#dialogConsultarSubCuentaContable").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarSubCuentaContable").dialog("option", "buttons", {});
                $("#dialogConsultarSubCuentaContable").dialog("option", "height", "100");
            }
            $("#dialogConsultarSubCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaEditarSubCuentaContable(IdSubCuentaContable) {
    $("#dialogEditarSubCuentaContable").obtenerVista({
        nombreTemplate: "tmplEditarSubCuentaContable.html",
        url: "SubCuentaContable.aspx/ObtenerFormaEditarSubCuentaContable",
        parametros: IdSubCuentaContable,
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarCuentaContable();
            $("#dialogEditarSubCuentaContable").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarSubCuentaContable() {
    var pSubCuentaContable = new Object();
    pSubCuentaContable.SubCuentaContable = $("#txtSubCuentaContable").val();
    pSubCuentaContable.Descripcion = $("#txtDescripcion").val();
    pSubCuentaContable.IdCuentaContable = $("#divFormaAgregarSubCuentaContable").attr("idCuentaContable");
    pSubCuentaContable.CuentaContable = $("#txtCuentaContable").val();
    var validacion = ValidaSubCuentaContable(pSubCuentaContable);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSubCuentaContable = pSubCuentaContable;
    SetAgregarSubCuentaContable(JSON.stringify(oRequest));
}

function SetAgregarSubCuentaContable(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SubCuentaContable.aspx/AgregarSubCuentaContable",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSubCuentaContable").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarSubCuentaContable").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdSubCuentaContable, pBaja) {
    var pRequest = "{'pIdSubCuentaContable':" + pIdSubCuentaContable + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "SubCuentaContable.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdSubCuentaContable").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdSubCuentaContable').one('click', '.div_grdSubCuentaContable_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdSubCuentaContable_AI']").children().attr("baja")
                var idSubCuentaContable = $(registro).children("td[aria-describedby='grdSubCuentaContable_IdSubCuentaContable']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idSubCuentaContable, baja);
            });
        }
    });
}

function EditarSubCuentaContable() {
    var pSubCuentaContable = new Object();
    pSubCuentaContable.IdSubCuentaContable = $("#divFormaEditarSubCuentaContable").attr("idSubCuentaContable");
    pSubCuentaContable.SubCuentaContable = $("#txtSubCuentaContable").val();
    pSubCuentaContable.Descripcion = $("#txtDescripcion").val();
    pSubCuentaContable.IdCuentaContable = $("#divFormaEditarSubCuentaContable").attr("idCuentaContable");
    pSubCuentaContable.CuentaContable = $("#txtCuentaContable").val();
    var validacion = ValidaSubCuentaContable(pSubCuentaContable);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSubCuentaContable = pSubCuentaContable;
    SetEditarSubCuentaContable(JSON.stringify(oRequest));
}
function SetEditarSubCuentaContable(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SubCuentaContable.aspx/EditarSubCuentaContable",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdSubCuentaContable").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarSubCuentaContable").dialog("close");
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
                url: 'SubCuentaContable.aspx/BuscarCuentaContable',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarSubCuentaContable, #divFormaEditarSubCuentaContable").attr("idCuentaContable", "0");
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
            $("#divFormaAgregarSubCuentaContable, #divFormaEditarSubCuentaContable").attr("idCuentaContable", pIdCuentaContable);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaSubCuentaContable(pSubCuentaContable) {
    var errores = "";

    if (pSubCuentaContable.SubCuentaContable == "")
    { errores = errores + "<span>*</span> El nombre de la subcuenta contable esta vacío, favor de capturarlo.<br />"; }
    
    if (pSubCuentaContable.IdCuentaContable == 0)
    { errores = errores + "<span>*</span> La cuenta contable esta vacia, favor de capturarla.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
