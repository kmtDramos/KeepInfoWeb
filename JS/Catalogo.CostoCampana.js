//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("CostoCampana");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCostoCampana", function() {
        ObtenerFormaAgregarCostoCampana();
    });

    $("#grdCostoCampana").on("click", ".imgFormaConsultarCostoCampana", function() {
        var registro = $(this).parents("tr");
        var CostoCampana = new Object();
        CostoCampana.pIdCostoCampana = parseInt($(registro).children("td[aria-describedby='grdCostoCampana_IdCostoCampana']").html());
        ObtenerFormaConsultarCostoCampana(JSON.stringify(CostoCampana));
    });

    $('#grdCostoCampana').one('click', '.div_grdCostoCampana_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCostoCampana_AI']").children().attr("baja")
        var idCostoCampana = $(registro).children("td[aria-describedby='grdCostoCampana_IdCostoCampana']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCostoCampana, baja);
    });
    
    $('#dialogAgregarCostoCampana').on('focusin', '#txtMonto', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });
    
    $('#dialogAgregarCostoCampana').on('focusout', '#txtMonto', function(event) {
        $(this).valorPredeterminado("Moneda");
    });
    
    $('#dialogAgregarCostoCampana').on('keypress', '#txtMonto', function(event) {
        if(!ValidarNumeroPunto(event,$(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarCostoCampana').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCostoCampana").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarCostoCampana();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarCostoCampana').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCostoCampana").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCostoCampana').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCostoCampana").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCostoCampana();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCostoCampana()
{
    $("#dialogAgregarCostoCampana").obtenerVista({
        nombreTemplate: "tmplAgregarCostoCampana.html",
        url: "CostoCampana.aspx/ObtenerFormaAgregarCostoCampana",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCostoCampana").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCostoCampana(pIdCostoCampana) {
    $("#dialogConsultarCostoCampana").obtenerVista({
        nombreTemplate: "tmplConsultarCostoCampana.html",
        url: "CostoCampana.aspx/ObtenerFormaCostoCampana",
        parametros: pIdCostoCampana,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarCostoCampana == 1) {
                $("#dialogConsultarCostoCampana").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var CostoCampana = new Object();
                        CostoCampana.IdCostoCampana = parseInt($("#divFormaConsultarCostoCampana").attr("IdCostoCampana"));
                        ObtenerFormaEditarCostoCampana(JSON.stringify(CostoCampana))
                    }
                });
                $("#dialogConsultarCostoCampana").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCostoCampana").dialog("option", "buttons", {});
                $("#dialogConsultarCostoCampana").dialog("option", "height", "100");
            }
            $("#dialogConsultarCostoCampana").dialog("open");
        }
    });
}

function ObtenerFormaEditarCostoCampana(IdCostoCampana) {
    $("#dialogEditarCostoCampana").obtenerVista({
        nombreTemplate: "tmplEditarCostoCampana.html",
        url: "CostoCampana.aspx/ObtenerFormaEditarCostoCampana",
        parametros: IdCostoCampana,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCostoCampana").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCostoCampana() {
    var pCostoCampana = new Object();
    
    var monto = $("#txtMonto").val();
    if(monto == "")
    { monto = "0";}
    monto = monto.replace("$","");
    monto = monto.split(",").join("");
    monto = parseFloat(monto);
    
    
    pCostoCampana.CostoCampana = $("#txtCostoCampana").val();
    pCostoCampana.IdCampana = $("#cmbCampana").val();
    pCostoCampana.Monto = monto;
    var validacion = ValidaCostoCampana(pCostoCampana);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCostoCampana = pCostoCampana;
    SetAgregarCostoCampana(JSON.stringify(oRequest)); 
}

function SetAgregarCostoCampana(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CostoCampana.aspx/AgregarCostoCampana",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCostoCampana").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCostoCampana").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdCostoCampana, pBaja) {
    var pRequest = "{'pIdCostoCampana':" + pIdCostoCampana + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "CostoCampana.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCostoCampana").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdCostoCampana').one('click', '.div_grdCostoCampana_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCostoCampana_AI']").children().attr("baja")
                var idCostoCampana = $(registro).children("td[aria-describedby='grdCostoCampana_IdCostoCampana']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCostoCampana, baja);
            });
        }
    });
}

function EditarCostoCampana() {
    var pCostoCampana = new Object();
    pCostoCampana.IdCostoCampana = $("#divFormaEditarCostoCampana").attr("idCostoCampana");
    pCostoCampana.CostoCampana = $("#txtCostoCampana").val();
    var validacion = ValidaCostoCampana(pCostoCampana);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCostoCampana = pCostoCampana;
    SetEditarCostoCampana(JSON.stringify(oRequest));
}

function SetEditarCostoCampana(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CostoCampana.aspx/EditarCostoCampana",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCostoCampana").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCostoCampana").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaCostoCampana(pCostoCampana) {
    var errores = "";

    if (pCostoCampana.CostoCampana == "")
    { errores = errores + "<span>*</span> El campo costo campana esta vacío, favor de capturarlo.<br />"; }
    
    if (pCostoCampana.IdCampana == "0")
    { errores = errores + "<span>*</span> El campo campaña esta vacio, favor de capturarlo.<br />"; }
    
    if (pCostoCampana.Monto == "")
    { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}