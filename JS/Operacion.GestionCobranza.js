//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosGestionCobranza();
    
    $("#grdGestionCobranza").on("click", ".imgFormaAgregarProximaGestion", function() {
        var registro = $(this).parents("tr");
        var GestionCobranza = new Object();
        GestionCobranza.pFacturasSeleccionadas = $("#txtFacturasSeleccionadas").val();
        GestionCobranza.pIdGestionCobranza = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdGestionCobranza']").html());
        GestionCobranza.pIdFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
        ObtenerFormaAgregarProximaGestion(JSON.stringify(GestionCobranza)); 
    });
    
    $("#grdGestionCobranza").on("click", ".imgFormaAgregarFechaPago", function() {
        var registro = $(this).parents("tr");
        var GestionCobranza = new Object();
        GestionCobranza.pIdGestionCobranza = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdGestionCobranza']").html());
        GestionCobranza.pIdFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
        GestionCobranza.pFacturasSeleccionadas = $("#txtFacturasSeleccionadas").val();
        ObtenerFormaAgregarFechaPago(JSON.stringify(GestionCobranza));
    });
    
    $("#divFiltrosGestionCobranza").on("change", "#cmbFiltroTipoGestion", function() {
        FiltroGestionCobranza();
    });
    
    $("#divFiltrosGestionCobranza").on("change", "#cmbFiltroClientesAsignados", function() {
        FiltroGestionCobranza();
    });
    
    $("#grdGestionCobranza").on("click", ".divEditarProximaGestion", function() {
        var registro = $(this).parents("tr");
        var GestionCobranza = new Object();
        GestionCobranza.pFacturasSeleccionadas = $("#txtFacturasSeleccionadas").val();
        GestionCobranza.pIdGestionCobranza = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdGestionCobranza']").html());
        GestionCobranza.pIdFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
        ObtenerFormaAgregarProximaGestion(JSON.stringify(GestionCobranza));    
    });
    
    $("#grdGestionCobranza").on("click", ".divEditarFechaPago", function() {
        var registro = $(this).parents("tr");
        var GestionCobranza = new Object();
        GestionCobranza.pFacturasSeleccionadas = $("#txtFacturasSeleccionadas").val();
        GestionCobranza.pIdGestionCobranza = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdGestionCobranza']").html());
        GestionCobranza.pIdFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
        ObtenerFormaAgregarFechaPago(JSON.stringify(GestionCobranza));
    });
    
    $("#divFiltrosGestionCobranza").on("change", "#cmbFiltroFecha", function() {
        if ($(this).val() == 0) {
            $("#txtFechaInicio, #txtFechaCorte").attr("disabled", true);
            $("#txtFechaInicio, #txtFechaCorte").datepicker("setDate", new Date());
        }
        else if ($(this).val() == 1) {
            $("#txtFechaInicio").attr("disabled", false);
            $("#txtFechaCorte").attr("disabled", true);
            $("#txtFechaInicio").datepicker("setDate", new Date());
            $("#txtFechaCorte").datepicker("setDate", new Date());
        }
        else if ($(this).val() == 2) {
            $("#txtFechaInicio").attr("disabled", false);
            $("#txtFechaCorte").attr("disabled", false);
            var fecha = new Date();
            fecha.setDate(fecha.getDate() + 31);
            $("#txtFechaInicio").datepicker("setDate", new Date());
            $("#txtFechaCorte").datepicker("setDate", fecha);
        }
        FiltroGestionCobranza();
    });
    
    $("#grdGestionCobranza").on("click", ".div_grdGestionCobranza_Comentario", function() {
        var registro = $(this).parents("tr");
        var Factura = new Object();
        Factura.pIdFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
        ObtenerFormaGestionCobranzaSeguimientos(JSON.stringify(Factura));
    });
    
    $("#grdGestionCobranza").on("click", "#btnObtenerFormaExportarSeguiemientoCliente", function() {
        var registro = $(this).parents("tr");
        var Factura = new Object();
        Factura.pIdFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
        ObtenerFormaGestionCobranzaSeguimientos(JSON.stringify(Factura));
    });
    
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaExportarSeguiemientoCliente", function() {
        ObtenerFormaExportarSeguiemientosCliente();
    });
    
    $("#grdGestionCobranza").on("click", ".checkAsignarVarios", function() {
        if ($(this).is(':checked')) {
            var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
            var registro = $(this).parents("tr");
            facturasSeleccionadas.push(parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html()));
            $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
        }
        else{
            var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
            var registro = $(this).parents("tr");
            var idFactura = parseInt($(registro).children("td[aria-describedby='grdGestionCobranza_IdFactura']").html());
            $.each(facturasSeleccionadas, function(pIndex, pIdFactura) {
                if(pIdFactura == idFactura){
                    facturasSeleccionadas.splice(pIndex, 1);
                    return ( pIdFactura !== idFactura );
                }
            });
            $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
        }
    });
    
    $('#dialogAgregarProximaGestion').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarProximaGestion").remove();
        },
        buttons: {
            "Programar": function() {
                AgregarProximaGestion();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogAgregarFechaPago').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarFechaPago").remove();
        },
        buttons: {
            "Programar": function() {
                AgregarFechaPago();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogConsultarGestionCobranzaSeguimientos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarGestionCobranzaSeguimientos").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");   
            },
            "Exportar": function() {
                 $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: { IsExportExcel: true, pIdGestionCobranza: $("#divFormaConsultarGestionCobranzaSeguimientos").attr("IdGestionCobranza") }, downloadType: 'Normal' });
            }
        }
    });
    
    $('#dialogExportarSeguimientosCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divExportarSeguimientosCliente").remove();
        },
        buttons: {
            "Exportar": function() {
                ExportarSeguiemientosCliente();
            }
        }
    });
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarProximaGestion(pRequest) {
    $("#dialogAgregarProximaGestion").obtenerVista({
        nombreTemplate: "tmplFormaAgregarProximaGestion.html",
        parametros: pRequest,
        url: "GestionCobranza.aspx/ObtenerFormaAgregarProximaGestion",
        despuesDeCompilar: function(pRespuesta) {
            $("#divFechaProximaGestion").datepicker();
            $("#dialogAgregarProximaGestion").dialog("open");
        }
    });
}

function ObtenerFormaAgregarFechaPago(pRequest) {
    $("#dialogAgregarFechaPago").obtenerVista({
        nombreTemplate: "tmplFormaAgregarFechaPago.html",
        parametros: pRequest,
        url: "GestionCobranza.aspx/ObtenerFormaAgregarProximaGestion",
        despuesDeCompilar: function(pRespuesta) {
            $("#divFechaPago").datepicker();
            $("#dialogAgregarFechaPago").dialog("open");
        }
    }); 
}

function ObtenerFormaFiltrosGestionCobranza() {
    $("#divFiltrosGestionCobranza").obtenerVista({
        nombreTemplate: "tmplFiltrosGestionCobranza.html",
        url: "GestionCobranza.aspx/ObtenerFormaFiltroGestionCobranza",
        despuesDeCompilar: function(pRespuesta) {
            if ($("#txtFechaInicio").length) {
                $("#txtFechaInicio").datepicker({
                    onSelect: function() {
                        FiltroGestionCobranza();
                    }
                });
            }
            if ($("#txtFechaCorte").length) {
                $("#txtFechaCorte").datepicker({
                    onSelect: function() {
                        FiltroGestionCobranza();
                    }
                });
            }
        }
    });
}

function ObtenerFormaGestionCobranzaSeguimientos(pFactura) {
    $("#dialogConsultarGestionCobranzaSeguimientos").obtenerVista({
        nombreTemplate: "tmplFormaConsultarGestionCobranzaSeguimientos.html",
        url: "GestionCobranza.aspx/ObtenerFormaGestionCobranzaSeguimientos",
        parametros: pFactura,
        despuesDeCompilar: function(pRespuesta) {
            Modelo = pRespuesta.modelo;     
            if (Modelo.Seguimientos.length > 1) {
                $("#dialogConsultarGestionCobranzaSeguimientos").dialog("option", "buttons", {
                    "Aceptar": function() {
                        $(this).dialog("close");   
                    },
                    "Exportar": function() {
                        $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: { IsExportExcel: true, pIdGestionCobranza: $("#divFormaConsultarGestionCobranzaSeguimientos").attr("IdGestionCobranza") }, downloadType: 'Normal' });
                    }
                });				
            }
            else{
                $("#dialogConsultarGestionCobranzaSeguimientos").dialog("option", "buttons", {
                    "Aceptar": function() {
                        $(this).dialog("close");   
                    }
                });
            }
            $("#dialogConsultarGestionCobranzaSeguimientos").dialog("open");
        }
    }); 
}

function ObtenerFormaExportarSeguiemientosCliente(){
    $("#dialogExportarSeguimientosCliente").obtenerVista({
        nombreTemplate: "tmplFormaExportarSeguimientosCliente.html",
        despuesDeCompilar: function(pRespuesta) {
            CrearAutocompletarCliente();
            $("#dialogExportarSeguimientosCliente").dialog("open");
        }
    });
}

function FiltroGestionCobranza() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdGestionCobranza').getGridParam('rowNum');
    request.pPaginaActual = $('#grdGestionCobranza').getGridParam('page');
    request.pColumnaOrden = $('#grdGestionCobranza').getGridParam('sortname');
    request.pTipoOrden = $('#grdGestionCobranza').getGridParam('sortorder');
    request.pRazonSocial = "";
    request.pSerieFactura = "";
    request.pNumeroFactura = "";
    request.pIdTipoGestion = 0;
    request.pFiltroFecha = 0;
    request.pFechaInicio = "";
    request.pFechaCorte = "";
    request.pIdFiltroClientesAsignados = 1;
   
    if ($('#gs_RazonSocial').val() != null) {
        request.pRazonSocial = $("#gs_RazonSocial").val();
    }
    if ($('#gs_SerieFactura').val() != null) {
        request.pSerieFactura = $("#gs_SerieFactura").val();
    }
    if ($('#gs_NumeroFactura').val() != null) {
        request.pNumeroFactura = $("#gs_NumeroFactura").val();
    }   
    if ($('#cmbFiltroTipoGestion').val() != null) {
        request.pIdTipoGestion = $("#cmbFiltroTipoGestion").val();
    }
    if ($('#cmbFiltroClientesAsignados').val() != null) {
        request.pIdFiltroClientesAsignados = $("#cmbFiltroClientesAsignados").val();
    }    
    if ($('#cmbFiltroFecha').val() != null) {
        request.pFiltroFecha = $("#cmbFiltroFecha").val();
    }
    else {
        request.pFiltroFecha = "1";
    }
    if ($('#txtFechaInicio').val() != null) {
        request.pFechaInicio = $("#txtFechaInicio").val();
    }
    request.pFechaInicio = ConvertirFecha(request.pFechaInicio,'aaaammdd');
    
    if ($('#txtFechaCorte').val() != null) {
        request.pFechaCorte = $("#txtFechaCorte").val();
    }
    request.pFechaCorte = ConvertirFecha(request.pFechaCorte,'aaaammdd');
        
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'GestionCobranza.aspx/ObtenerGestionCobranza',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdGestionCobranza')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function Termino_grdGestionCobranza() {
    var ids = $('#grdGestionCobranza').jqGrid('getDataIDs');
    var facturasSeleccionadas = JSON.parse("[" + $("#txtFacturasSeleccionadas").val() + "]");
    
    for (var i = 0; i < ids.length; i++) {
        var etiquetaComentario = '<div class="div_grdGestionCobranza_Comentario cursorPointer"><img src="../images/comment.png" /></div>';
        $('#grdGestionCobranza').jqGrid('setRowData', ids[i], { Comentario: etiquetaComentario });
        idFactura = $('#grdGestionCobranza #' + ids[i] + ' td[aria-describedby="grdGestionCobranza_IdFactura"]').html();
        $.each(facturasSeleccionadas, function(pIndex, pIdFactura) {
            if(pIdFactura == idFactura) {
                $('#grdGestionCobranza #' + ids[i] + ' td[aria-describedby="grdGestionCobranza_Sel"] input').prop('checked', true);
            }
            return (pIdFactura !== idFactura);
        });
        $("#txtFacturasSeleccionadas").val(facturasSeleccionadas);
    }
}

function CrearAutocompletarCliente() {
    $('#txtBuscadorCliente').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtBuscadorCliente').val();
            $.ajax({
                type: 'POST',
                url: 'GestionCobranza.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divExportarSeguimientosCliente").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaExportarSeguimientosCliente").attr("idCliente", pIdCliente);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarProximaGestion(){
    var pRequest = new Object();
    pRequest.pIdGestionCobranza = $("#divFormaAgregarProximaGestion").attr("idGestionCobranza");
    pRequest.pIdFactura = $("#divFormaAgregarProximaGestion").attr("idFactura");
    pRequest.pFechaProximaGestion = $("#divFechaProximaGestion").val();
    pRequest.pComentarios = $("#txtComentarios").val();
    pRequest.pFacturasSeleccionadas = $("#txtFacturasSeleccionadas").val();
    $("#txtFacturasSeleccionadas").val("");
    SetAgregarProximaGestion(JSON.stringify(pRequest));
}

function SetAgregarProximaGestion(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "GestionCobranza.aspx/AgregarProximaGestion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdGestionCobranza").trigger("reloadGrid");
                $("#dialogAgregarProximaGestion").dialog("close");
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

function AgregarFechaPago(){
    var pRequest = new Object();
    pRequest.pIdGestionCobranza = $("#divFormaAgregarFechaPago").attr("idGestionCobranza");
    pRequest.pIdFactura = $("#divFormaAgregarFechaPago").attr("idFactura");
    pRequest.pFechaPago = $("#divFechaPago").val();
    pRequest.pComentarios = $("#txtComentarios").val();
    pRequest.pFacturasSeleccionadas = $("#txtFacturasSeleccionadas").val();
    $("#txtFacturasSeleccionadas").val("");
    SetAgregarFechaPago(JSON.stringify(pRequest));
}

function SetAgregarFechaPago(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "GestionCobranza.aspx/AgregarFechaPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdGestionCobranza").trigger("reloadGrid");
                $("#dialogAgregarFechaPago").dialog("close");
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

function ExportarSeguiemientosCliente(){
    var request = new Object();
    request.pIdCliente = $("#divFormaExportarSeguimientosCliente").attr("idCliente");
    
    var validacion = ValidarExportarSeguimientosCliente(request);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    
    $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarSeguimientosCliente.aspx', data: { IsExportExcel: true, pIdCliente: request.pIdCliente }, downloadType: 'Normal' });
}

//-----Validaciones-------------
function ValidarExportarSeguimientosCliente(pCliente) {
    var errores = "";

    if (pCliente.pIdCliente == 0)
    { errores = errores + "<span>*</span> Favor de seleccionar a un cliente.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}
