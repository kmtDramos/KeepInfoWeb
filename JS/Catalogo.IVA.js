//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("IVA");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarIVA", function() {
        ObtenerFormaAgregarIVA();
    });

    $("#grdIVA").on("click", ".imgFormaConsultarIVA", function() {
        var registro = $(this).parents("tr");
        var IVA = new Object();
        IVA.pIdIVA = parseInt($(registro).children("td[aria-describedby='grdIVA_IdIVA']").html());
        ObtenerFormaConsultarIVA(JSON.stringify(IVA));
    });

    $('#grdIVA').on('click', '.div_grdIVA_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdIVA_AI']").children().attr("baja")
        var idIVA = $(registro).children("td[aria-describedby='grdIVA_IdIVA']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idIVA, baja);
    });

    $('#dialogAgregarIVA').dialog({
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
            $("#divFormaAgregarIVA").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarIVA();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarIVA').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarIVA").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarIVA').dialog({
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
            $("#divFormaEditarIVA").remove();
        },
        buttons: {
            "Editar": function() {
                EditarIVA();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarIVA() {
    $("#dialogAgregarIVA").obtenerVista({
        nombreTemplate: "tmplAgregarIVA.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarIVA").dialog("open");
        }
    });
}

function ObtenerFormaConsultarIVA(pIdIVA) {
    $("#dialogConsultarIVA").obtenerVista({
        nombreTemplate: "tmplConsultarIVA.html",
        url: "IVA.aspx/ObtenerFormaIVA",
        parametros: pIdIVA,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarIVA == 1) {
                $("#dialogConsultarIVA").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var IVA = new Object();
                        IVA.IdIVA = parseInt($("#divFormaConsultarIVA").attr("IdIVA"));
                        ObtenerFormaEditarIVA(JSON.stringify(IVA))
                    }
                });
                $("#dialogConsultarIVA").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarIVA").dialog("option", "buttons", {});
                $("#dialogConsultarIVA").dialog("option", "height", "100");
            }
            $("#dialogConsultarIVA").dialog("open");
        }
    });
}

function ObtenerFormaEditarIVA(IdIVA) {
    $("#dialogEditarIVA").obtenerVista({
        nombreTemplate: "tmplEditarIVA.html",
        url: "IVA.aspx/ObtenerFormaEditarIVA",
        parametros: IdIVA,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarIVA").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarIVA() {
    var pIVA = new Object();
    pIVA.IVA = $("#txtIVA").val();
    pIVA.DescripcionIVA = $("#txtDescripcionIVA").val();
    pIVA.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    pIVA.CuentaContableTrasladado = $("#txtCuentaContableTrasladado").val();
    pIVA.CuentaContableAcreditablePagado = $("#txtCCAcreditablePagado").val();
    pIVA.CuentaContableTrasladadoPagado = $("#txtCCTrasladadoPagado").val();
    var validacion = ValidaIVA(pIVA);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pIVA = pIVA;
    SetAgregarIVA(JSON.stringify(oRequest));
}

function SetAgregarIVA(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IVA.aspx/AgregarIVA",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdIVA").trigger("reloadGrid");
                $("#dialogAgregarIVA").dialog("close");
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

function SetCambiarEstatus(pIdIVA, pBaja) {
    var pRequest = "{'pIdIVA':" + pIdIVA + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "IVA.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdIVA").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdIVA').one('click', '.div_grdIVA_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdIVA_AI']").children().attr("baja")
                var idIVA = $(registro).children("td[aria-describedby='grdIVA_IdIVA']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idIVA, baja);
            });
        }
    });
}

function EditarIVA() {
    var pIVA = new Object();
    pIVA.IdIVA = $("#divFormaEditarIVA").attr("idIVA");
    pIVA.IVA = $("#txtIVA").val();
    pIVA.DescripcionIVA = $("#txtDescripcionIVA").val();
    pIVA.ClaveCuentaContable = $("#txtClaveCuentaContable").val();
    pIVA.CuentaContableTrasladado = $("#txtCuentaContableTrasladado").val();
    pIVA.CuentaContableAcreditablePagado = $("#txtCCAcreditablePagado").val();
    pIVA.CuentaContableTrasladadoPagado = $("#txtCCTrasladadoPagado").val();
    var validacion = ValidaIVA(pIVA);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pIVA = pIVA;
    SetEditarIVA(JSON.stringify(oRequest));
}

function SetEditarIVA(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IVA.aspx/EditarIVA",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdIVA").trigger("reloadGrid");
                $("#dialogEditarIVA").dialog("close");
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
function ValidaIVA(pIVA) {
    var errores = "";

    if (pIVA.IVA == 0)
    { errores = errores + "<span>*</span> El campo IVA esta vacío, favor de capturarlo.<br/>"; }

    if (pIVA.DescripcionIVA == "")
    { errores = errores + "<span>*</span> El campo Descripción de IVA esta vacío, favor de capturarlo.<br/>"; }
    
    if (pIVA.ClaveCuentaContable == "")
    { errores = errores + "<span>*</span> El campo de la clave de cuenta contable esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
