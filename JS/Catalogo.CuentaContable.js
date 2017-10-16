//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("CuentaContable");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCuentaContable", function() {
        ObtenerFormaTipoCuentaContable();
    });

    $("#grdCuentaContable").on("click", ".imgFormaConsultarCuentaContable", function() {
        var registro = $(this).parents("tr");
        var CuentaContable = new Object();
        CuentaContable.pIdCuentaContable = parseInt($(registro).children("td[aria-describedby='grdCuentaContable_IdCuentaContable']").html());
        ObtenerFormaConsultarCuentaContable(JSON.stringify(CuentaContable));
    });

    $('#grdCuentaContable').on('click', '.div_grdCuentaContable_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaContable_AI']").children().attr("baja")
        var idCuentaContable = $(registro).children("td[aria-describedby='grdCuentaContable_IdCuentaContable']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCuentaContable, baja);
    });

    $('#dialogAgregarCuentaContable').on('change', '#cmbSucursal, #cmbDivision, #cmbTipoCompra', function(event) {
        var pCuentaContable = new Object();
        pCuentaContable.IdSucursal = parseInt($("#cmbSucursal").val());
        pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
        pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
        var oRequest = new Object();
        oRequest.pCuentaContable = pCuentaContable;
        ObtenerCuentaContableGenerada(JSON.stringify(oRequest));
    });

    $('#dialogEditarCuentaContable').on('change', '#cmbSucursal, #cmbDivision, #cmbTipoCompra', function(event) {
        var pCuentaContable = new Object();
        pCuentaContable.IdSucursal = parseInt($("#cmbSucursal").val());
        pCuentaContable.IdDivision = parseInt($("#cmbDivision").val());
        pCuentaContable.IdTipoCompra = parseInt($("#cmbTipoCompra").val());
        var oRequest = new Object();
        oRequest.pCuentaContable = pCuentaContable;
        ObtenerCuentaContableGenerada(JSON.stringify(oRequest));
    });

    $('#dialogAgregarCuentaContable').dialog({
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
            $("#divFormaAgregarCuentaContable").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCuentaContable();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCuentaContable").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCuentaContable').dialog({
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
            $("#divFormaEditarCuentaContable").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCuentaContable();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogTipoCuentaContable').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaTipoCuentaContable").remove();
        },
        buttons: {
            "Aceptar": function() {
                if ($("#cmbTipoCuentaContable").val() == 1) {
                    ObtenerFormaAgregarCuentaContable();
                }
                if ($("#cmbTipoCuentaContable").val() > 1) {
                    var request = new Object();
                    request.pIdTipoCuentaContable = $("#cmbTipoCuentaContable").val();
                    ObtenerFormaAgregarCuentaContableComplementos(JSON.stringify(request));
                }
                $(this).dialog("close");
            }
        }
    });

    $('#dialogAgregarCuentaContableComplementos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCuentaContableComplementos").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCuentaContableComplementos();
            }
        }
    });
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCuentaContable() {
    $("#dialogAgregarCuentaContable").obtenerVista({
        nombreTemplate: "tmplAgregarCuentaContable.html",
        url: "CuentaContable.aspx/ObtenerFormaAgregarCuentaContable",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCuentaContable").dialog("open");
        }
    });
}

function ObtenerCuentaContableGenerada(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaContable.aspx/ObtenerCuentaContableGenerada",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtCuentaContableSeg-1").val(respuesta.Modelo.SegmentoSucursal);
                $("#txtCuentaContableSeg-2").val(respuesta.Modelo.SegmentoDivision);
                $("#txtCuentaContableSeg-3").val(respuesta.Modelo.SegmentoTipoCompra);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerFormaConsultarCuentaContable(pIdCuentaContable) {
    $("#dialogConsultarCuentaContable").obtenerVista({
        nombreTemplate: "tmplConsultarCuentaContable.html",
        url: "CuentaContable.aspx/ObtenerFormaCuentaContable",
        parametros: pIdCuentaContable,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCuentaContable == 1) {
                $("#dialogConsultarCuentaContable").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var CuentaContable = new Object();
                        CuentaContable.IdCuentaContable = parseInt($("#divFormaConsultarCuentaContable").attr("IdCuentaContable"));
                        ObtenerFormaEditarCuentaContable(JSON.stringify(CuentaContable))
                    }
                });
                $("#dialogConsultarCuentaContable").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCuentaContable").dialog("option", "buttons", {});
                $("#dialogConsultarCuentaContable").dialog("option", "height", "100");
            }
            $("#dialogConsultarCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaEditarCuentaContable(IdCuentaContable) {
    $("#dialogEditarCuentaContable").obtenerVista({
        nombreTemplate: "tmplEditarCuentaContable.html",
        url: "CuentaContable.aspx/ObtenerFormaEditarCuentaContable",
        parametros: IdCuentaContable,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaTipoCuentaContable() {
    $("#dialogTipoCuentaContable").obtenerVista({
        nombreTemplate: "tmplFormaTipoCuentaContable.html",
        url: "CuentaContable.aspx/ObtenerFormaTipoCuentaContable",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogTipoCuentaContable").dialog("open");
        }
    });
}

function ObtenerFormaAgregarCuentaContableComplementos(pRequest) {
    $("#dialogAgregarCuentaContableComplementos").obtenerVista({
        nombreTemplate: "tmplAgregarCuentaContableComplementos.html",
        url: "CuentaContable.aspx/ObtenerFormaAgregarCuentaContableComplementos",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCuentaContableComplementos").dialog("open");
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCuentaContable() {
    var pCuentaContable = new Object();
    pCuentaContable.CuentaContable = $("#txtCuentaContableSeg-1").val() + "-" + $("#txtCuentaContableSeg-2").val() + "-" + $("#txtCuentaContableSeg-3").val() + "-" + $("#txtCuentaContableSeg-4").val();
    pCuentaContable.IdSucursal = $("#cmbSucursal").val();
    pCuentaContable.IdDivision = $("#cmbDivision").val();
    pCuentaContable.IdTipoCompra = $("#cmbTipoCompra").val();
    pCuentaContable.Descripcion = $("#txtDescripcion").val();
    var validacion = ValidaCuentaContable(pCuentaContable);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentaContable = pCuentaContable;
    SetAgregarCuentaContable(JSON.stringify(oRequest));
}

function SetAgregarCuentaContable(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaContable.aspx/AgregarCuentaContable",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentaContable").trigger("reloadGrid");
                $("#dialogAgregarCuentaContable").dialog("close");
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

function SetCambiarEstatus(pIdCuentaContable, pBaja) {
    var pRequest = "{'pIdCuentaContable':" + pIdCuentaContable + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "CuentaContable.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCuentaContable").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCuentaContable').one('click', '.div_grdCuentaContable_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCuentaContable_AI']").children().attr("baja")
                var idCuentaContable = $(registro).children("td[aria-describedby='grdCuentaContable_IdCuentaContable']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCuentaContable, baja);
            });
        }
    });
}

function EditarCuentaContable() {
    var pCuentaContable = new Object();
    pCuentaContable.IdCuentaContable = $("#divFormaEditarCuentaContable").attr("idCuentaContable");
    pCuentaContable.CuentaContable = $("#txtCuentaContableSeg-1").val() + "-" + $("#txtCuentaContableSeg-2").val() + "-" + $("#txtCuentaContableSeg-3").val() + "-" + $("#txtCuentaContableSeg-4").val();
    pCuentaContable.IdSucursal = $("#cmbSucursal").val();
    pCuentaContable.IdDivision = $("#cmbDivision").val();
    pCuentaContable.IdTipoCompra = $("#cmbTipoCompra").val();
    pCuentaContable.Descripcion = $("#txtDescripcion").val();
    var validacion = ValidaCuentaContable(pCuentaContable);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentaContable = pCuentaContable;
    SetEditarCuentaContable(JSON.stringify(oRequest));
}

function SetEditarCuentaContable(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaContable.aspx/EditarCuentaContable",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentaContable").trigger("reloadGrid");
                $("#dialogEditarCuentaContable").dialog("close");

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

function AgregarCuentaContableComplementos() {
    var pCuentaContable = new Object();
    pCuentaContable.IdTipoCuentaContable = $("#divFormaAgregarCuentaContableComplementos").attr("idTipoCuentaContable");
    pCuentaContable.CuentaClienteComplemento = $("#txtCuentaClienteComplemento").val();
    pCuentaContable.DescripcionCuentaClienteComplemento = $("#txtDescripcionCuentaClienteComplemento").val();
    var oRequest = new Object();
    oRequest.pCuentaContable = pCuentaContable;
    SetAgregarCuentaContableComplementos(JSON.stringify(oRequest));
}

function SetAgregarCuentaContableComplementos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentaContable.aspx/AgregarCuentaContableComplementos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentaContable").trigger("reloadGrid");
                $("#dialogAgregarCuentaContableComplementos").dialog("close");
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
function ValidaCuentaContable(pCuentaContable) {
    var errores = "";

    if (pCuentaContable.CuentaContable == "")
    { errores = errores + "<span>*</span> El campo de la cuenta contable esta vacío, favor de capturarlo.<br />"; }

    if (pCuentaContable.IdDivision == 0)
    { errores = errores + "<span>*</span> El campo división esta vacío, favor de seleccionarlo.<br />"; }

    if (pCuentaContable.IdTipoCompra == 0)
    { errores = errores + "<span>*</span> El campo tipo de compra esta vacío, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
