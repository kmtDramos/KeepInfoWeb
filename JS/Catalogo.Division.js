//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Division");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarDivision", function() {
        ObtenerFormaAgregarDivision();
    });

    $("#grdDivision").on("click", ".imgFormaConsultarDivision", function() {
        var registro = $(this).parents("tr");
        var Division = new Object();
        Division.pIdDivision = parseInt($(registro).children("td[aria-describedby='grdDivision_IdDivision']").html());
        ObtenerFormaConsultarDivision(JSON.stringify(Division));
    });

    $('#grdDivision').on('click', '.div_grdDivision_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDivision_AI']").children().attr("baja")
        var idDivision = $(registro).children("td[aria-describedby='grdDivision_IdDivision']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idDivision, baja);
    });

    $('#dialogAgregarDivision').dialog({
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
            $("#divFormaAgregarDivision").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarDivision();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarDivision').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarDivision").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarDivision').dialog({
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
            $("#divFormaEditarDivision").remove();
        },
        buttons: {
            "Editar": function() {
                EditarDivision();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarDivision() {
    $("#dialogAgregarDivision").obtenerVista({
        nombreTemplate: "tmplAgregarDivision.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarDivision").dialog("open");
        }
    });
}

function ObtenerFormaConsultarDivision(pIdDivision) {
    $("#dialogConsultarDivision").obtenerVista({
        nombreTemplate: "tmplConsultarDivision.html",
        url: "Division.aspx/ObtenerFormaDivision",
        parametros: pIdDivision,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarDivision == 1) {
                $("#dialogConsultarDivision").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Division = new Object();
                        Division.IdDivision = parseInt($("#divFormaConsultarDivision").attr("IdDivision"));
                        ObtenerFormaEditarDivision(JSON.stringify(Division))
                    }
                });
                $("#dialogConsultarDivision").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarDivision").dialog("option", "buttons", {});
                $("#dialogConsultarDivision").dialog("option", "height", "100");
            }
            $("#dialogConsultarDivision").dialog("open");
        }
    });
}

function ObtenerFormaEditarDivision(IdDivision) {
    $("#dialogEditarDivision").obtenerVista({
        nombreTemplate: "tmplEditarDivision.html",
        url: "Division.aspx/ObtenerFormaEditarDivision",
        parametros: IdDivision,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarDivision").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarDivision() {
    var pDivision = new Object();
    pDivision.Division = $("#txtDivision").val();
    pDivision.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    pDivision.LimiteDescuento = parseInt($("#txtLimiteDescuento").val());
    pDivision.LimiteMargen = parseInt($("#txtLimiteMargen").val());
    pDivision.Descripcion = $("#txtDescripcion").val();
    
    if ($("#chkEsVenta").is(':checked')) {
        pDivision.EsVenta = 1;
    }
    else {
        pDivision.EsVenta = 0;
    }
    
    var validacion = ValidaDivision(pDivision);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDivision = pDivision;
    SetAgregarDivision(JSON.stringify(oRequest));
}

function SetAgregarDivision(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Division.aspx/AgregarDivision",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDivision").trigger("reloadGrid");
                $("#dialogAgregarDivision").dialog("close");
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

function SetCambiarEstatus(pIdDivision, pBaja) {
    var pRequest = "{'pIdDivision':" + pIdDivision + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Division.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdDivision").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdDivision').one('click', '.div_grdDivision_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdDivision_AI']").children().attr("baja")
                var idDivision = $(registro).children("td[aria-describedby='grdDivision_IdDivision']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idDivision, baja);
            });
        }
    });
}

function EditarDivision() {
    var pDivision = new Object();
    pDivision.IdDivision = $("#divFormaEditarDivision").attr("idDivision");
    pDivision.Division = $("#txtDivision").val();
    pDivision.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    pDivision.LimiteDescuento = parseInt($("#txtLimiteDescuento").val());
    pDivision.LimiteMargen = parseInt($("#txtLimiteMargen").val());
    pDivision.Descripcion = $("#txtDescripcion").val();
    
    if ($("#chkEsVenta").is(':checked')) {
        pDivision.EsVenta = 1;
    }
    else {
        pDivision.EsVenta = 0;
    }
    
    var validacion = ValidaDivision(pDivision);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDivision = pDivision;
    SetEditarDivision(JSON.stringify(oRequest));
}

function SetEditarDivision(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Division.aspx/EditarDivision",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDivision").trigger("reloadGrid");
                $("#dialogEditarDivision").dialog("close");
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
function ValidaDivision(pDivision) {
    var errores = "";

    if (pDivision.Division == "")
    { errores = errores + "<span>*</span> El campo división esta vacío, favor de capturarlo.<br />"; }

    if (pDivision.ClaveCuentaContable == "")
    { errores = errores + "<span>*</span> El campo de la clave de cuenta contable esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
