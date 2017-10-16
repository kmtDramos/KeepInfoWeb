//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("TipoCambio");
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarTipoCambio", function() {
        ObtenerFormaAgregarTipoCambio();
    });
    
    $("#grdTipoCambio").on("click", ".imgFormaConsultarTipoCambio", function() {
        var registro = $(this).parents("tr");
        var TipoCambio = new Object();
        TipoCambio.pIdTipoCambio = parseInt($(registro).children("td[aria-describedby='grdTipoCambio_IdTipoCambio']").html());
        ObtenerFormaConsultarTipoCambio(JSON.stringify(TipoCambio));
    });
    
    $('#grdTipoCambio').one('click', '.div_grdTipoCambio_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCambio_AI']").children().attr("baja")
        var idTipoCambio = $(registro).children("td[aria-describedby='grdTipoCambio_IdTipoCambio']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idTipoCambio, baja);
    });
    
    $('#dialogAgregarTipoCambio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarTipoCambio").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarTipoCambio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
    $('#dialogConsultarTipoCambio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarTipoCambio").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarTipoCambio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTipoCambio").remove();
        },
        buttons: {
            "Editar": function() {
                EditarTipoCambio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAgregarTipoCambio, #dialogEditarTipoCambio').on('focusin', '#txtTipoCambio', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarTipoCambio, #dialogEditarTipoCambio').on('focusout', '#txtTipoCambio', function(event) {
        $(this).val(parseFloat($(this).val()).currency());
    });

    $('#dialogAgregarTipoCambio, #dialogEditarTipoCambio').on('keypress', '#txtTipoCambio', function(event) {
        if (!ValidarFlotante(event, $(this).val())) {
            return false;
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarTipoCambio() {

    $("#dialogAgregarTipoCambio").obtenerVista({
        url: "TipoCambio.aspx/ObtenerFormaAgregarTipoCambio",
        nombreTemplate: "tmplAgregarTipoCambio.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarTipoCambio").dialog("open");
        }
    });
}

function ObtenerFormaConsultarTipoCambio(pIdTipoCambio) {
    $("#dialogConsultarTipoCambio").obtenerVista({
        nombreTemplate: "tmplConsultarTipoCambio.html",
        url: "TipoCambio.aspx/ObtenerFormaTipoCambio",
        parametros: pIdTipoCambio,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambio == 1) {
                $("#dialogConsultarTipoCambio").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var TipoCambio = new Object();
                        TipoCambio.IdTipoCambio = parseInt($("#divFormaConsultarTipoCambio").attr("IdTipoCambio"));
                        ObtenerFormaEditarTipoCambio(JSON.stringify(TipoCambio))
                    }
                });
                $("#dialogConsultarTipoCambio").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarTipoCambio").dialog("option", "buttons", {});
                $("#dialogConsultarTipoCambio").dialog("option", "height", "100");
            }
            $("#dialogConsultarTipoCambio").dialog("open");
        }
    });
}

function ObtenerFormaEditarTipoCambio(IdTipoCambio) {
    $("#dialogEditarTipoCambio").obtenerVista({
        nombreTemplate: "tmplEditarTipoCambio.html",
        url: "TipoCambio.aspx/ObtenerFormaEditarTipoCambio",
        parametros: IdTipoCambio,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarTipoCambio").dialog("open");
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarTipoCambio() {
    var pTipoCambio = new Object();
    pTipoCambio.TipoCambio = $("#txtTipoCambio").val();
    pTipoCambio.IdTipoMonedaOrigen = $("#cmbMonedaOrigen").val();
    pTipoCambio.IdTipoMonedaDestino = $("#cmbMonedaDestino").val();
    pTipoCambio.Fecha = $("#lblFecha").html();

    pTipoCambio.TipoCambio = pTipoCambio.TipoCambio.replace("$", "");
    pTipoCambio.TipoCambio = pTipoCambio.TipoCambio.split(",").join("");
    pTipoCambio.TipoCambio = parseFloat(pTipoCambio.TipoCambio);
    
    var validacion = ValidaTipoCambio(pTipoCambio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCambio = pTipoCambio;
    SetAgregarTipoCambio(JSON.stringify(oRequest)); 
}

function SetAgregarTipoCambio(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCambio.aspx/AgregarTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCambio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarTipoCambio").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdTipoCambio, pBaja) {
    var pRequest = "{'pIdTipoCambio':" + pIdTipoCambio + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "TipoCambio.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdTipoCambio").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdTipoCambio').one('click', '.div_grdTipoCambio_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdTipoCambio_AI']").children().attr("baja")
                var idTipoCambio = $(registro).children("td[aria-describedby='grdTipoCambio_IdTipoCambio']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idTipoCambio, baja);
            });
        }
    });
}

function EditarTipoCambio() {
    var pTipoCambio = new Object();
    pTipoCambio.IdTipoCambio = $("#divFormaEditarTipoCambio").attr("idTipoCambio");
    pTipoCambio.TipoCambio = $("#txtTipoCambio").val();
    pTipoCambio.IdTipoMonedaOrigen = $("#cmbMonedaOrigen").val();
    pTipoCambio.IdTipoMonedaDestino = $("#cmbMonedaDestino").val();
    pTipoCambio.Fecha = $("#lblFecha").html();

    pTipoCambio.TipoCambio = pTipoCambio.TipoCambio.replace("$", "");
    pTipoCambio.TipoCambio = pTipoCambio.TipoCambio.split(",").join("");
    pTipoCambio.TipoCambio = parseFloat(pTipoCambio.TipoCambio);
    
    var validacion = ValidaTipoCambio(pTipoCambio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pTipoCambio = pTipoCambio;
    SetEditarTipoCambio(JSON.stringify(oRequest));
}

function SetEditarTipoCambio(pRequest) {    
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "TipoCambio.aspx/EditarTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdTipoCambio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarTipoCambio").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaTipoCambio(pTipoCambio) {
    var errores = "";

    if (pTipoCambio.TipoCambio == "")
    { errores = errores + "<span>*</span> El campo TipoCambio esta vacío, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
