//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(window).unload(function() {
        ActualizarPanelControles("Servicio");
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarServicio", function() {
        ObtenerFormaAgregarServicio();
    });

    $("#grdServicio").on("click", ".imgFormaConsultarServicio", function() {
        var registro = $(this).parents("tr");
        var Servicio = new Object();
        Servicio.pIdServicio = parseInt($(registro).children("td[aria-describedby='grdServicio_IdServicio']").html());
        ObtenerFormaConsultarServicio(JSON.stringify(Servicio));
    });

    $("#grdServicio").on("click", ".imgFormaConsultarDescuento", function() {
        var registro = $(this).parents("tr");
        var Servicio = new Object();
        Servicio.pIdServicio = parseInt($(registro).children("td[aria-describedby='grdServicio_IdServicio']").html());
        $("#dialogConsultarDescuentoServicio").attr("idServicio", Servicio.pIdServicio);
        FiltroDescuentoServicio();
        $("#dialogConsultarDescuentoServicio").dialog("open");
    });

    $('#grdServicio').one('click', '.div_grdServicio_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdServicio_AI']").children().attr("baja")
        var idServicio = $(registro).children("td[aria-describedby='grdServicio_IdServicio']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idServicio, baja);
    });

    $('#grdDescuentoServicio').one('click', '.div_grdDescuentoServicio_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoServicio_AI']").children().attr("baja")
        var idDescuentoServicio = $(registro).children("td[aria-describedby='grdDescuentoServicio_IdDescuentoServicio']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatusDescuento(idDescuentoServicio, baja);
    });

    $('#dialogAgregarServicio, #dialogEditarServicio').on('focusin', '#txtPrecio', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarServicio, #dialogEditarServicio').on('focusout', '#txtPrecio', function(event) {
        $(this).valorPredeterminado("Moneda");
    });

    $("#dialogConsultarDescuentoServicio").on("click", "#btnAgregarDescuentoServicio", function() {
        ObtenerFormaAgregarDescuentoServicio();
    });

    $('#dialogAgregarDescuentoServicio').on('focusin', '#txtDescuentoServicio', function(event) {
        $(this).quitarValorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarServicio").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarServicio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarServicio").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarServicioAEnrolar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarServicioAEnrolar").remove();
        },
        buttons: {
            "Enrolar": function() {
                EnrolarServicio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });


    $('#dialogConsultarDescuentoServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarDescuento").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarServicio").remove();
        },
        buttons: {
            "Guardar": function() {
                EditarServicio();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAgregarDescuentoServicio').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarDescuentoServicio").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarDescuentoServicio();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarServicio() {
    $("#dialogAgregarServicio").obtenerVista({
        nombreTemplate: "tmplAgregarServicio.html",
        url: "Servicio.aspx/ObtenerFormaAgregarServicio",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarServicio").dialog("open");
            $("#cmbTipoVenta option[value=1]").attr("selected", true);
            $("#cmbTipoMoneda option[value=1]").attr("selected", true); 
        }
    });
}

function ObtenerFormaConsultarServicio(pIdServicio) {
    $("#dialogConsultarServicio").obtenerVista({
        nombreTemplate: "tmplConsultarServicio.html",
        url: "Servicio.aspx/ObtenerFormaServicio",
        parametros: pIdServicio,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarServicio == 1) {
                $("#dialogConsultarServicio").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Servicio = new Object();
                        Servicio.IdServicio = parseInt($("#divFormaConsultarServicio").attr("IdServicio"));
                        ObtenerFormaEditarServicio(JSON.stringify(Servicio))
                    }
                });
                $("#dialogConsultarServicio").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarServicio").dialog("option", "buttons", {});
                $("#dialogConsultarServicio").dialog("option", "height", "280");
            }
            $("#dialogConsultarServicio").dialog("open");
        }
    });
}

function ObtenerFormaEditarServicio(IdServicio) {
    $("#dialogEditarServicio").obtenerVista({
        nombreTemplate: "tmplEditarServicio.html",
        url: "Servicio.aspx/ObtenerFormaEditarServicio",
        parametros: IdServicio,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarServicio").dialog("open");
        }
    });
}

function ObtenerFormaConsultarServicioAEnrolar(pIdServicio) {
    $("#dialogConsultarServicioAEnrolar").obtenerVista({
        nombreTemplate: "tmplConsultarServicioAEnrolar.html",
        url: "Servicio.aspx/ObtenerFormaServicioAEnrolar",
        parametros: pIdServicio,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarServicioAEnrolar").dialog("open");
        }
    });
}

function ObtenerFormaAgregarDescuentoServicio() {
    $("#dialogAgregarDescuentoServicio").obtenerVista({
        nombreTemplate: "tmplAgregarDescuentoServicio.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarDescuentoServicio").dialog("open");
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarServicio() {
    var pServicio = new Object();
    pServicio.ClaveProdServ = $("#txtClaveProdServ").val();
    pServicio.Servicio = $("#txtServicio").val();
    pServicio.IdTipoServicio = $("#cmbTipoServicio").val();
    pServicio.Clave = $("#txtClave").val();
    pServicio.IdTipoVenta = $("#cmbTipoVenta").val();
    pServicio.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pServicio.Precio = $("#txtPrecio").val();
    pServicio.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pServicio.IdTipoIVA = $("#cmbTipoIVA").val();
    pServicio.IdDivision = $("#cmbDivision").val();

    pServicio.Precio = pServicio.Precio.replace("$", "");
    pServicio.Precio = pServicio.Precio.split(",").join("");
    pServicio.Precio = parseFloat(pServicio.Precio);
    
    var validacion = ValidaServicio(pServicio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pServicio = pServicio;
    SetAgregarServicio(JSON.stringify(oRequest));
}

function SetAgregarServicio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Servicio.aspx/AgregarServicio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdServicio").trigger("reloadGrid");
            }
            else if (respuesta.Descripcion == "valida") {
                ObtenerFormaConsultarServicioAEnrolar(JSON.stringify(respuesta.Modelo));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarServicio").dialog("close");
        }
    });
}

function AgregarDescuentoServicio() {
    var pDescuento = new Object();
    pDescuento.IdServicio = $("#dialogConsultarDescuentoServicio").attr("idServicio");
    pDescuento.Descuento = $("#txtDescuentoServicio").val();
    pDescuento.MotivoDescuento = $("#txtDescripcion").val();
    var validacion = ValidaDescuento(pDescuento);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDescuento = pDescuento;
    SetAgregarDescuentoServicio(JSON.stringify(oRequest));
}

function SetAgregarDescuentoServicio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Servicio.aspx/AgregarDescuento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDescuentoServicio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarDescuentoServicio").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdServicio, pBaja) {
    var pRequest = "{'pIdServicio':" + pIdServicio + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Servicio.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdServicio").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdServicio').one('click', '.div_grdServicio_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdServicio_AI']").children().attr("baja")
                var idServicio = $(registro).children("td[aria-describedby='grdServicio_IdServicio']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idServicio, baja);
            });
        }
    });
}

function SetCambiarEstatusDescuento(pIdDescuentoServicio, pBaja) {
    var pRequest = "{'pIdDescuentoServicio':" + pIdDescuentoServicio + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Servicio.aspx/CambiarEstatusDescuento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdDescuentoServicio").trigger("reloadGrid");
        },
        complete: function() {
                $('#grdDescuentoServicio').one('click', '.div_grdDescuentoServicio_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdDescuentoServicio_AI']").children().attr("baja")
                var idDescuentoServicio = $(registro).children("td[aria-describedby='grdDescuentoServicio_IdDescuentoServicio']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatusDescuento(idDescuentoServicio, baja);
            });
        }
    });
}

function EditarServicio() {
    var pServicio = new Object();
    pServicio.IdServicio = $("#divFormaEditarServicio").attr("idServicio");
    pServicio.ClaveProdServ = $("#txtClaveProdServ").val();
    pServicio.Servicio = $("#txtServicio").val();
    pServicio.IdTipoServicio = $("#cmbTipoServicio").val();
    pServicio.Clave = $("#txtClave").val();
    pServicio.IdTipoVenta = $("#cmbTipoVenta").val();
    pServicio.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pServicio.Precio = $("#txtPrecio").val();
    pServicio.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pServicio.IdTipoIVA = $("#cmbTipoIVA").val();
    pServicio.IdDivision = $("#cmbDivision").val();

    pServicio.Precio = pServicio.Precio.replace("$", "");
    pServicio.Precio = pServicio.Precio.split(",").join("");
    pServicio.Precio = parseFloat(pServicio.Precio);
    
    var validacion = ValidaServicio(pServicio);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pServicio = pServicio;
    SetEditarServicio(JSON.stringify(oRequest));
}

function SetEditarServicio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Servicio.aspx/EditarServicio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdServicio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarServicio").dialog("close");
        }
    });
}

function EnrolarServicio() {
    var pServicio = new Object();
    pServicio.IdServicio = $("#divFormaConsultarServicioAEnrolar").attr("idServicio");
    var oRequest = new Object();
    oRequest.pServicio = pServicio;
    SetEnrolarServicio(JSON.stringify(oRequest));
}

function SetEnrolarServicio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Servicio.aspx/EnrolarServicio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdServicio").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogConsultarServicioAEnrolar").dialog("close");
        }
    });
}

function FiltroDescuentoServicio() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDescuentoServicio').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDescuentoServicio').getGridParam('page');
    request.pColumnaOrden = $('#grdDescuentoServicio').getGridParam('sortname');
    request.pTipoOrden = $('#grdDescuentoServicio').getGridParam('sortorder');
    request.pIdServicio = 0;
    if ($("#dialogConsultarDescuentoServicio").attr("idServicio") != null) {
        request.pIdServicio = $("#dialogConsultarDescuentoServicio").attr("idServicio");
    }
    
    var pRequest = JSON.stringify(request);
    $.ajax({
    url: 'Servicio.aspx/ObtenerDescuentoServicio',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success') {
                $('#grdDescuentoServicio')[0].addJSONData(JSON.parse(jsondata.responseText).d);                
            }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}


//-----Validaciones------------------------------------------------------
function ValidaServicio(pServicio) {
    var errores = "";

    if (pServicio.IdTipoServicio == 0)
    { errores = errores + "<span>*</span> El campo tipo de servicio esta vac&iacute;o, favor de seleccionarlo.<br />"; }

    if (pServicio.Clave == "")
    { errores = errores + "<span>*</span> El campo clave esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pServicio.IdUnidadCompraVenta == 0)
    { errores = errores + "<span>*</span> El campo unidad de media esta vac&iacute;o, favor de seleccionarlo.<br />"; }

    if (pServicio.IdDivision == 0)
    { errores = errores + "<span>*</span> El campo división esta vac&iacute;o, favor de seleccionarlo.<br />"; }

    if (pServicio.Servicio == "")
    { errores = errores + "<span>*</span> El campo descripción esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pServicio.Precio == 0)
    { errores = errores + "<span>*</span> El campo precio esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pServicio.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vac&iacute;o, favor de seleccionarlo.<br />"; }

    if (pServicio.IdTipoVenta == 0)
    { errores = errores + "<span>*</span> El campo tipo de venta esta vac&iacute;o, favor de seleccionarlo.<br />"; }    

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDescuento(pDescuento) {
    var errores = "";

    if (pDescuento.Descuento == 0)
    { errores = errores + "<span>*</span> El campo descuento esta vac&iacute;o, favor de capturarlo.<br />"; }

    if (pDescuento.MotivoDescuento == "")
    { errores = errores + "<span>*</span> El campo motivo de descuento esta vac&iacute;o, favor de capturarlo.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ObtenerListaTipoServicio() {
    $("#cmbTipoServicio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Servicio.aspx/ObtenerListaTipoServicio"
    });
}
function ObtenerListaTipoVenta() {
    $("#cmbTipoVenta").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Servicio.aspx/ObtenerListaTipoVenta"
    });
}

function ObtenerListaUnidadCompraVenta() {
    $("#cmbUnidadCompraVenta").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Servicio.aspx/ObtenerListaUnidadCompraVenta"
    });
}

function ObtenerListaDivision() {
    $("#cmbDivision").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Servicio.aspx/ObtenerListaDivision"
    });
}