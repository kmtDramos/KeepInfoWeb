//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoCambioDiarioOficial");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoCambioDiarioOficial", function() {
        ObtenerFormaAgregarTipoCambioDiarioOficial();
    });
    
    $("#grdTipoCambioDiarioOficial").on("click", ".imgFormaConsultarTipoCambioDiarioOficial", function() {
        var registro = $(this).parents("tr");
        var TipoCambioDiarioOficial = new Object();
        TipoCambioDiarioOficial.pIdTipoCambioDiarioOficial = parseInt($(registro).children("td[aria-describedby='grdTipoCambioDiarioOficial_IdTipoCambioDiarioOficial']").html());
        ObtenerFormaConsultarTipoCambioDiarioOficial(JSON.stringify(TipoCambioDiarioOficial));
    });
    
    $('#grdTipoCambioDiarioOficial').one('click', '.div_grdTipoCambioDiarioOficial_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCambioDiarioOficial_AI']").children().attr("baja")
        var idTipoCambioDiarioOficial = $(registro).children("td[aria-describedby='grdTipoCambioDiarioOficial_IdTipoCambioDiarioOficial']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoCambioDiarioOficial, baja);
    });
    
    $('#dialogAgregarTipoCambioDiarioOficial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoCambioDiarioOficial").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoCambioDiarioOficial();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarTipoCambioDiarioOficial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoCambioDiarioOficial").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarTipoCambioDiarioOficial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoCambioDiarioOficial").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoCambioDiarioOficial();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAgregarTipoCambioDiarioOficial, #dialogEditarTipoCambioDiarioOficial').on('focusin', '#txtTipoCambioDiarioOficial', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarTipoCambioDiarioOficial, #dialogEditarTipoCambioDiarioOficial').on('focusout', '#txtTipoCambioDiarioOficial', function(event) {
        $(this).val(parseFloat($(this).val()));
    });

    $('#dialogAgregarTipoCambioDiarioOficial, #dialogEditarTipoCambioDiarioOficial').on('keypress', '#txtTipoCambioDiarioOficial', function(event) {
        if (!ValidarFlotante(event, $(this).val())) {
            return false;
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoCambioDiarioOficial() {

    $("#dialogAgregarTipoCambioDiarioOficial").obtenerVista({
        url: "TipoCambioDiarioOficial.aspx/ObtenerFormaAgregarTipoCambioDiarioOficial",
        nombreTemplate: "tmplAgregarTipoCambioDiarioOficial.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoCambioDiarioOficial").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoCambioDiarioOficial(pIdTipoCambioDiarioOficial) {
    $("#dialogConsultarTipoCambioDiarioOficial").obtenerVista({
        nombreTemplate: "tmplConsultarTipoCambioDiarioOficial.html",
        url: "TipoCambioDiarioOficial.aspx/ObtenerFormaTipoCambioDiarioOficial",
        parametros: pIdTipoCambioDiarioOficial,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambioDiarioOficial == 1) {
                $("#dialogConsultarTipoCambioDiarioOficial").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoCambioDiarioOficial = new Object();
                        TipoCambioDiarioOficial.IdTipoCambioDiarioOficial = parseInt($("#divFormaConsultarTipoCambioDiarioOficial").attr("IdTipoCambioDiarioOficial"));
                        ObtenerFormaEditarTipoCambioDiarioOficial(JSON.stringify(TipoCambioDiarioOficial))
                    }
                });
                $("#dialogConsultarTipoCambioDiarioOficial").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoCambioDiarioOficial").dialog("option", "buttons", {});
                $("#dialogConsultarTipoCambioDiarioOficial").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoCambioDiarioOficial").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoCambioDiarioOficial(IdTipoCambioDiarioOficial) {
    $("#dialogEditarTipoCambioDiarioOficial").obtenerVista({
        nombreTemplate: "tmplEditarTipoCambioDiarioOficial.html",
        url: "TipoCambioDiarioOficial.aspx/ObtenerFormaEditarTipoCambioDiarioOficial",
        parametros: IdTipoCambioDiarioOficial,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoCambioDiarioOficial").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoCambioDiarioOficial() {
    var pTipoCambioDiarioOficial = new Object();
    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = $("#txtTipoCambioDiarioOficial").val();
    pTipoCambioDiarioOficial.IdTipoMonedaOrigen = $("#cmbMonedaOrigen").val();
    pTipoCambioDiarioOficial.IdTipoMonedaDestino = $("#cmbMonedaDestino").val();
    pTipoCambioDiarioOficial.Fecha = $("#lblFecha").html();

    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = pTipoCambioDiarioOficial.TipoCambioDiarioOficial.replace("$", "");
    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = pTipoCambioDiarioOficial.TipoCambioDiarioOficial.split(",").join("");
    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = parseFloat(pTipoCambioDiarioOficial.TipoCambioDiarioOficial);
    
    var validacion = ValidaTipoCambioDiarioOficial(pTipoCambioDiarioOficial);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCambioDiarioOficial = pTipoCambioDiarioOficial;
    SetAgregarTipoCambioDiarioOficial(JSON.stringify(oRequest)); 
}

function SetAgregarTipoCambioDiarioOficial(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCambioDiarioOficial.aspx/AgregarTipoCambioDiarioOficial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCambioDiarioOficial").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoCambioDiarioOficial").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoCambioDiarioOficial, pBaja) {
    var pRequest = "{'pIdTipoCambioDiarioOficial':" + pIdTipoCambioDiarioOficial + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoCambioDiarioOficial.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoCambioDiarioOficial").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoCambioDiarioOficial').one('click', '.div_grdTipoCambioDiarioOficial_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCambioDiarioOficial_AI']").children().attr("baja")
                var idTipoCambioDiarioOficial = $(registro).children("td[aria-describedby='grdTipoCambioDiarioOficial_IdTipoCambioDiarioOficial']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoCambioDiarioOficial, baja);
            });
        }
    });
}

function EditarTipoCambioDiarioOficial() {
    var pTipoCambioDiarioOficial = new Object();
    pTipoCambioDiarioOficial.IdTipoCambioDiarioOficial = $("#divFormaEditarTipoCambioDiarioOficial").attr("idTipoCambioDiarioOficial");
    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = $("#txtTipoCambioDiarioOficial").val();
    pTipoCambioDiarioOficial.IdTipoMonedaOrigen = $("#cmbMonedaOrigen").val();
    pTipoCambioDiarioOficial.IdTipoMonedaDestino = $("#cmbMonedaDestino").val();
    pTipoCambioDiarioOficial.Fecha = $("#lblFecha").html();

    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = pTipoCambioDiarioOficial.TipoCambioDiarioOficial.replace("$", "");
    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = pTipoCambioDiarioOficial.TipoCambioDiarioOficial.split(",").join("");
    pTipoCambioDiarioOficial.TipoCambioDiarioOficial = parseFloat(pTipoCambioDiarioOficial.TipoCambioDiarioOficial);
    
    var validacion = ValidaTipoCambioDiarioOficial(pTipoCambioDiarioOficial);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCambioDiarioOficial = pTipoCambioDiarioOficial;
    SetEditarTipoCambioDiarioOficial(JSON.stringify(oRequest));
}

function SetEditarTipoCambioDiarioOficial(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCambioDiarioOficial.aspx/EditarTipoCambioDiarioOficial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCambioDiarioOficial").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoCambioDiarioOficial").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoCambioDiarioOficial(pTipoCambioDiarioOficial) {
    var errores = "";

    if (pTipoCambioDiarioOficial.TipoCambioDiarioOficial == "")
    { errores = errores + "<span>*</span> El campo TipoCambioDiarioOficial esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
