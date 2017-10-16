//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("Municipio");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarMunicipio", function() {
        ObtenerFormaAgregarMunicipio();
    });

    $("#grdMunicipio").on("click", ".imgFormaConsultarMunicipio", function() {
        var registro = $(this).parents("tr");
        var Municipio = new Object();
        Municipio.pIdMunicipio = parseInt($(registro).children("td[aria-describedby='grdMunicipio_IdMunicipio']").html());
        ObtenerFormaConsultarMunicipio(JSON.stringify(Municipio));
    });

    $('#dialogAgregarMunicipio, #dialogEditarMunicipio').on('change', '#cmbPais', function(event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstados(JSON.stringify(request));
    }); 
    
    $('#grdMunicipio').one('click', '.div_grdMunicipio_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdMunicipio_AI']").children().attr("baja")
        var idMunicipio = $(registro).children("td[aria-describedby='grdMunicipio_IdMunicipio']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idMunicipio, baja);
    });

    $('#dialogAgregarMunicipio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarMunicipio").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarMunicipio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarMunicipio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarMunicipio").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });


    $('#dialogEditarMunicipio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarMunicipio").remove();
        },
        buttons: {
            "Editar": function() {
                EditarMunicipio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarMunicipio() {
    $("#dialogAgregarMunicipio").obtenerVista({
        nombreTemplate: "tmplAgregarMunicipio.html",
        url: "Municipio.aspx/ObtenerFormaAgregarMunicipio",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarMunicipio").dialog("open");
            $("#cmbTipoVenta option[value=1]").attr("selected", true);
            $("#cmbTipoMoneda option[value=1]").attr("selected", true); 
        }
    });
}

function ObtenerFormaConsultarMunicipio(pIdMunicipio) {
    $("#dialogConsultarMunicipio").obtenerVista({
        nombreTemplate: "tmplConsultarMunicipio.html",
        url: "Municipio.aspx/ObtenerFormaMunicipio",
        parametros: pIdMunicipio,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarMunicipio == 1) {
                $("#dialogConsultarMunicipio").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Municipio = new Object();
                        Municipio.IdMunicipio = parseInt($("#divFormaConsultarMunicipio").attr("IdMunicipio"));
                        ObtenerFormaEditarMunicipio(JSON.stringify(Municipio))
                    }
                });
                $("#dialogConsultarMunicipio").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarMunicipio").dialog("option", "buttons", {});
                $("#dialogConsultarMunicipio").dialog("option", "height", "250");
            }
            $("#dialogConsultarMunicipio").dialog("open");
        }
    });
}

function ObtenerFormaEditarMunicipio(IdMunicipio) {
    $("#dialogEditarMunicipio").obtenerVista({
        nombreTemplate: "tmplEditarMunicipio.html",
        url: "Municipio.aspx/ObtenerFormaEditarMunicipio",
        parametros: IdMunicipio,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarMunicipio").dialog("open");
        }
    });
}

function ObtenerListaEstados(pRequest) {
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Municipio.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) { 
        }
    });
}


//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarMunicipio() {
    var pMunicipio = new Object();
    pMunicipio.Municipio = $("#txtMunicipio").val();
    pMunicipio.Clave = $("#txtClave").val();
    pMunicipio.IdEstado = $("#cmbEstado").val();
    
    var validacion = ValidaMunicipio(pMunicipio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pMunicipio = pMunicipio;
    SetAgregarMunicipio(JSON.stringify(oRequest));
}

function SetAgregarMunicipio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Municipio.aspx/AgregarMunicipio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdMunicipio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarMunicipio").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdMunicipio, pBaja) {
    var pRequest = "{'pIdMunicipio':" + pIdMunicipio + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Municipio.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdMunicipio").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdMunicipio').one('click', '.div_grdMunicipio_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdMunicipio_AI']").children().attr("baja")
                var idMunicipio = $(registro).children("td[aria-describedby='grdMunicipio_IdMunicipio']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idMunicipio, baja);
            });
        }
    });
}

function EditarMunicipio() {
    var pMunicipio = new Object();
    pMunicipio.IdMunicipio = $("#divFormaEditarMunicipio").attr("idMunicipio");
    pMunicipio.Municipio = $("#txtMunicipio").val();
    pMunicipio.Clave = $("#txtClave").val();
    pMunicipio.IdEstado = $("#cmbEstado").val();
    
    var validacion = ValidaMunicipio(pMunicipio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pMunicipio = pMunicipio;
    SetEditarMunicipio(JSON.stringify(oRequest));
}

function SetEditarMunicipio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Municipio.aspx/EditarMunicipio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdMunicipio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarMunicipio").dialog("close");
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaMunicipio(pMunicipio) {
    var errores = "";

    if (pMunicipio.Municipio == "")
    { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pMunicipio.Abreviatura == "")
    { errores = errores + "<span>*</span> El campo nacionalidad esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pMunicipio.Clave == "")
    { errores = errores + "<span>*</span> El campo clave esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pMunicipio.IdPais == 0)
    { errores = errores + "<span>*</span> El campo país esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
