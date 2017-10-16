//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaRevisarAsientosPendientes", function() {
        ObtenerFormaRevisarAsientosPendientes();
    });

    $("#dialogRevisarAsientosPendientes").on("click", "#divActualizarAsiento", function() {
        var Paginador = new Object();
        Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
        Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
        Paginador.pColumnaOrden = "Prueba";
        Paginador.pTipoOrden = "DESC";
        ObtenerFormaAsientos(JSON.stringify(Paginador));
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgConsultarFacturaCliente", function() {
        var Factura = new Object();
        Factura.pIdFacturaEncabezado = $(this).attr("idFacturaEncabezado")
        ObtenerFormaConsultarFacturaEncabezado(JSON.stringify(Factura));
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgConsultarFacturaProveedor", function() {
        var Factura = new Object();
        Factura.pIdEncabezadoFacturaProveedor = $(this).attr("idFacturaProveedor")
        ObtenerFormaConsultarEncabezadoFacturaProveedor(JSON.stringify(Factura));
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgConsultarCobroCliente", function() {
        if ($(this).attr("TipoEntrada") == 1) {
            var CobroCliente = new Object();
            CobroCliente.pIdCuentasPorCobrar = $(this).attr("idIngreso");
            ObtenerFormaConsultarCuentasPorCobrar(JSON.stringify(CobroCliente));
        }
        else {
            var Deposito = new Object();
            Deposito.pIdDepositos = $(this).attr("idIngreso");
            ObtenerFormaConsultarDepositos(JSON.stringify(Deposito));
        }
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgConsultarPagoProveedor", function() {
        if ($(this).attr("TipoSalida") == 1) {
            var Egreso = new Object();
            Egreso.pIdEgresos = $(this).attr("idEgreso");
            ObtenerFormaConsultarEgresos(JSON.stringify(Egreso));
        }
        else {
            var Cheques = new Object();
            Cheques.pIdCheques = $(this).attr("idEgreso");
            ObtenerFormaConsultarCheques(JSON.stringify(Cheques));
        }
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgEditarCuentaContableMovimientosFacturaCliente", function() {
        var FacturaEncabezado = new Object();
        FacturaEncabezado.pIdFacturaEncabezado = $(this).attr("idFacturaEncabezado");
        ObtenerFormaEditarCuentaMovimientosFacturaCliente(JSON.stringify(FacturaEncabezado));
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgEditarCuentaContableMovimientosFacturaProveedor", function() {
        var FacturaProveedor = new Object();
        FacturaProveedor.pIdFacturaProveedor = $(this).attr("idFacturaProveedor");
        ObtenerFormaEditarCuentaMovimientosFacturaProveedor(JSON.stringify(FacturaProveedor));
    });

    $("#dialogEditarCuentaMovimientosFacturaProveedor").on("click", ".imgFormaEditarDetalleFacturaProveedor", function() {
        var registro = $(this).parents("tr");
        var DetalleFacturaProveedor = new Object();
        DetalleFacturaProveedor.pIdDetalleFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdDetalleFacturaProveedorEditar_IdDetalleFacturaProveedor']").html());
        ObtenerFormaEditarDetalleFacturaProveedor(JSON.stringify(DetalleFacturaProveedor));
    });

    $("#dialogEditarCuentaMovimientosDetalleFacturaProveedor").on("change", "#cmbTipoCompra", function() {
        var Request = new Object();
        Request.pIdTipoCompra = $(this).val();
        Request.pIdDetalleFacturaProveedor = $("#divEditarCuentaMovimientosDetalleFacturaProveedor").attr("IdDetalleFacturaProveedor");
        ObtenerComboCuentasContables(JSON.stringify(Request));
    });

    $("#dialogRevisarAsientosPendientes").on("click", ".imgConfirmarAsiento", function() {
        $("#dialogAgregarAsiento").attr("idDocumento", $(this).attr("idDocumento"));
        $("#dialogAgregarAsiento").attr("tipoAsiento", $(this).attr("tipoAsiento"));
        $("#dialogAgregarAsiento").attr("tipoSalida", $(this).attr("tipoSalida"));
        $("#dialogAgregarAsiento").attr("tipoEntrada", $(this).attr("tipoEntrada"));
        $("#dialogAgregarAsiento").attr("idDocumentoCobroPago", $(this).attr("idDocumentoCobroPago"));
        $("#dialogAgregarAsiento").dialog("open");
    });

    $("#dialogRevisarAsientosPendientes").on("change", "#cmbTipoAsientoContable", function() {
        var Paginador = new Object();
        Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
        Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
        Paginador.pColumnaOrden = "Prueba";
        Paginador.pTipoOrden = "DESC";
        ObtenerFormaAsientos(JSON.stringify(Paginador));
    });

    $("#dialogRevisar").on("change", "#cmbTipoAsientoContable", function() {
        var Paginador = new Object();
        Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
        Paginador.pPaginaActual = 1;
        Paginador.pColumnaOrden = "Prueba";
        Paginador.pTipoOrden = "DESC";
        ObtenerFormaAsientos(JSON.stringify(Paginador));
    });

    $("#dialogRevisarAsientosPendientes").on("click", "#tblPaginador .btnPrimeraPagina, #tblPaginador .btnPaginaAnterior, #tblPaginador .btnSiguientePagina, #tblPaginador .btnUltimaPagina", function() {
        var pagina = 0;
        switch ($(this).attr("rol")) {
            case "primeraPagina":
                pagina = 0;
                break;
            case "paginaAnterior":
                pagina = parseInt($("#tblPaginador .txtPaginaActual").val(), 10) - 1;
                break;
            case "siguientePagina":
                pagina = parseInt($("#tblPaginador .txtPaginaActual").val(), 10) + 1;
                break;
            case "ultimaPagina":
                pagina = parseInt($("#tblPaginador .spanNoPaginas").text(), 10);
                break;
            default:
        }
        
        var Paginador = new Object();
        Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
        Paginador.pPaginaActual = pagina;
        Paginador.pColumnaOrden = "Prueba";
        Paginador.pTipoOrden = "DESC";
        ObtenerFormaAsientos(JSON.stringify(Paginador));
    });

    $("#dialogRevisarAsientosPendientes").on("change", "#divPaginador .cmbTamanoPaginacion", function() {
        var Paginador = new Object();
        Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
        Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
        Paginador.pColumnaOrden = "Prueba";
        Paginador.pTipoOrden = "DESC";
        ObtenerFormaAsientos(JSON.stringify(Paginador));
    });

    $('#dialogRevisarAsientosPendientes').dialog({
        autoOpen: false,
        height: '625',
        width: '750',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divRevisarAsientosPendientes").empty();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarFacturaEncabezado').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarFacturaEncabezado").remove();
        }
    });

    $('#dialogConsultarEncabezadoFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEncabezadoFacturaProveedor").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarCuentasPorCobrar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCuentasPorCobrar").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarDepositos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarDepositos").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarEgresos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEgresos").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarCheques').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCheques").remove();
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCuentaMovimientosFacturaCliente').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divEditarCuentaMovimientosFacturaCliente").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCuentaMovimientosFacturaCliente();
            }
        }
    });

    $('#dialogEditarCuentaMovimientosFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divEditarCuentaMovimientosFacturaProveedor").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCuentaMovimientosFacturaProveedor();
            }
        }
    });

    $('#dialogEditarCuentaMovimientosDetalleFacturaProveedor').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divEditarCuentaMovimientosDetalleFacturaProveedor").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCuentaMovimientosDetalleFacturaProveedor();
            }
        }
    });

    $('#dialogAgregarAsiento').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
        },
        buttons: {
            "Aceptar": function() {
                AgregarAsiento();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAsientos(pPaginador) {
    switch(parseInt($("#cmbTipoAsientoContable").val())) {
        case 1:
            ObtenerFormaAsientosFacturaCliente(pPaginador);
            break;
        case 2:
            ObtenerFormaAsientosFacturaProveedor(pPaginador);
            break;
        case 3:
            ObtenerFormaAsientosCobroCliente(pPaginador);
            break;
        case 4:
            ObtenerFormaAsientosPagoProveedor(pPaginador);
            break;
        default:
    }
}

function ObtenerFormaRevisarAsientosPendientes() {
    var Paginador = new Object();
    Paginador.pTamanoPaginacion = 10;
    Paginador.pPaginaActual = 0;
    Paginador.pColumnaOrden = "Prueba";
    Paginador.pTipoOrden = "DESC";
    ObtenerFormaAsientos(JSON.stringify(Paginador));
    $("#dialogRevisarAsientosPendientes").obtenerVista({
        nombreTemplate: "tmplFormaAgregarAsiento.html",
        url: "Asientos.aspx/ObtenerFormaRevisarAsientosPendientes",
        parametros: JSON.stringify(Paginador),
        despuesDeCompilar: function(pRespuesta) {
            $("#tblPaginador").generarBotonesPaginador();
            var Paginador = new Object();
            Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
            Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
            Paginador.pColumnaOrden = "Prueba";
            Paginador.pTipoOrden = "DESC";
            ObtenerFormaAsientos(JSON.stringify(Paginador));
            $("#dialogRevisarAsientosPendientes").dialog("open");
        }
    });
}

function ObtenerFormaAsientosFacturaCliente(pPaginador){
    $("#divAsientos").obtenerVista({
        nombreTemplate: "tmplFormaAsientosFacturaCliente.html",
        url: "Asientos.aspx/ObtenerFormaRevisarAsientosPendientes",
        parametros: pPaginador,
        despuesDeCompilar: function(pRespuesta) {
            $("#divPaginador .txtPaginaActual").val(pRespuesta.modelo.Paginador.PaginaActual);
            $("#divPaginador .spanNoPaginas").text(pRespuesta.modelo.Paginador.NoPaginas);
            $("#divPaginador .spanNoRegistros").text(pRespuesta.modelo.Paginador.NoRegistros);
        }
    });
}

function ObtenerFormaAsientosFacturaProveedor(pPaginador){
    $("#divAsientos").obtenerVista({
        nombreTemplate: "tmplFormaAsientosFacturaProveedor.html",
        url: "Asientos.aspx/ObtenerFormaAsientosFacturaProveedor",
        parametros: pPaginador,
        despuesDeCompilar: function(pRespuesta) {
            $("#divPaginador .txtPaginaActual").val(pRespuesta.modelo.Paginador.PaginaActual);
            $("#divPaginador .spanNoPaginas").text(pRespuesta.modelo.Paginador.NoPaginas);
            $("#divPaginador .spanNoRegistros").text(pRespuesta.modelo.Paginador.NoRegistros);
        }
    });
}

function ObtenerFormaAsientosCobroCliente(pPaginador){
    $("#divAsientos").obtenerVista({
        nombreTemplate: "tmplFormaAsientosCobroCliente.html",
        url: "Asientos.aspx/ObtenerFormaAsientosCobroCliente",
        parametros: pPaginador,
        despuesDeCompilar: function(pRespuesta) {
            $("#divPaginador .txtPaginaActual").val(pRespuesta.modelo.Paginador.PaginaActual);
            $("#divPaginador .spanNoPaginas").text(pRespuesta.modelo.Paginador.NoPaginas);
            $("#divPaginador .spanNoRegistros").text(pRespuesta.modelo.Paginador.NoRegistros);
        }
    });
}

function ObtenerFormaAsientosPagoProveedor(pPaginador){
    $("#divAsientos").obtenerVista({
        nombreTemplate: "tmplFormaAsientosPagoProveedor.html",
        url: "Asientos.aspx/ObtenerFormaAsientosPagoProveedor",
        parametros: pPaginador,
        despuesDeCompilar: function(pRespuesta) {
            $("#divPaginador .txtPaginaActual").val(pRespuesta.modelo.Paginador.PaginaActual);
            $("#divPaginador .spanNoPaginas").text(pRespuesta.modelo.Paginador.NoPaginas);
            $("#divPaginador .spanNoRegistros").text(pRespuesta.modelo.Paginador.NoRegistros);
        }
    });
}

function ObtenerFormaConsultarFacturaEncabezado(pIdFacturaEncabezado) {
    $("#dialogConsultarFacturaEncabezado").obtenerVista({
        nombreTemplate: "tmplConsultarAsientosFacturaEncabezado.html",
        url: "FacturaCliente.aspx/ObtenerFormaConsultarFacturaEncabezado",
        parametros: pIdFacturaEncabezado,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdFacturaDetalleConsultar();            
            $("#divDireccionesTabs").tabs();
            $("#dialogConsultarFacturaEncabezado").dialog("open");
        }
    });
}

function FiltroFacturaDetalleConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaDetalleConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaDetalleConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaDetalleConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaDetalleConsultar').getGridParam('sortorder');
    request.pIdFacturaEncabezado = 0;
    if ($("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != null && $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != "") {
        request.pIdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado")
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerFacturaDetalleConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaDetalleConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaConsultarEncabezadoFacturaProveedor(pIdEncabezadoFacturaProveedor) {
    $("#dialogConsultarEncabezadoFacturaProveedor").obtenerVista({
        nombreTemplate: "tmplConsultarAsientosEncabezadoFacturaProveedor.html",
        url: "EncabezadoFacturaProveedor.aspx/ObtenerFormaEncabezadoFacturaProveedor",
        parametros: pIdEncabezadoFacturaProveedor,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdDetalleFacturaProveedorConsultar();
            $("#dialogConsultarEncabezadoFacturaProveedor").dialog("open");
        }
    });
}

function FiltroDetalleFacturaProveedorConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleFacturaProveedorConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleFacturaProveedorConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleFacturaProveedorConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleFacturaProveedorConsultar').getGridParam('sortorder');
    request.pIdEncabezadoFacturaProveedor = 0;
    if ($("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaConsultarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != null && $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor") != "") {
        request.pIdEncabezadoFacturaProveedor = $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaConsultarEncabezadoFacturaProveedor").attr("IdEncabezadoFacturaProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'EncabezadoFacturaProveedor.aspx/ObtenerDetalleFacturaProveedorConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleFacturaProveedorConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaConsultarCuentasPorCobrar(pIdCuentasPorCobrar) {
    $("#dialogConsultarCuentasPorCobrar").obtenerVista({
        nombreTemplate: "tmplConsultarAsientosCuentasPorCobrar.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaCuentasPorCobrar",
        parametros: pIdCuentasPorCobrar,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            $("#dialogConsultarCuentasPorCobrar").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function FiltroMovimientosCobrosConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortorder');
    request.pIdCuentasPorCobrar = 0;
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaConsultarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar") != null) {
        request.pIdCuentasPorCobrar = $("#divFormaEditarCuentasPorCobrar, #divFormaConsultarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerMovimientosCobrosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaConsultarDepositos(pIdDepositos) {
    $("#dialogConsultarAsientosDepositos").obtenerVista({
        nombreTemplate: "tmplConsultarDepositos.html",
        url: "Depositos.aspx/ObtenerFormaDepositos",
        parametros: pIdDepositos,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdIngresosNoDepositadosConsultar();
            FiltroIngresosNoDepositadosConsultar();
            $("#dialogConsultarDepositos").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function FiltroIngresosNoDepositadosConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdIngresosNoDepositadosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdIngresosNoDepositadosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdIngresosNoDepositadosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdIngresosNoDepositadosConsultar').getGridParam('sortorder');
    request.pIdDepositos = 0;
    if ($("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaConsultarDepositos").attr("idDepositos") != null && $("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaConsultarDepositos").attr("idDepositos") != "") {
        request.pIdDepositos = $("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaConsultarDepositos").attr("idDepositos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerIngresosNoDepositadosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdIngresosNoDepositadosConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaConsultarEgresos(pIdEgresos) {
    $("#dialogConsultarEgresos").obtenerVista({
        nombreTemplate: "tmplConsultarAsientosEgresos.html",
        url: "Egresos.aspx/ObtenerFormaEgresos",
        parametros: pIdEgresos,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultarEgresos();
            $("#dialogConsultarEgresos").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function FiltroMovimientosCobrosConsultarEgresos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosConsultarEgresos').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosConsultarEgresos').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosConsultarEgresos').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosConsultarEgresos').getGridParam('sortorder');
    request.pIdEgresos = 0;
    if ($("#divFormaEditarEgresos, #divFormaConsultarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos") != null) {
        request.pIdEgresos = $("#divFormaEditarEgresos, #divFormaConsultarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Egresos.aspx/ObtenerMovimientosCobrosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosConsultarEgresos')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaConsultarCheques(pIdCheques) {
    $("#dialogConsultarCheques").obtenerVista({
        nombreTemplate: "tmplConsultarAsientosCheques.html",
        url: "Cheques.aspx/ObtenerFormaCheques",
        parametros: pIdCheques,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultarCheques();
            $("#dialogConsultarCheques").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function FiltroMovimientosCobrosCheques() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosCheques').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosCheques').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosCheques').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosCheques').getGridParam('sortorder');
    request.pIdCheques = 0;
    if ($("#divFormaEditarCheques, #divFormaAsociarDocumentos").attr("IdCheques") != null) {
        request.pIdCheques = $("#divFormaEditarCheques, #divFormaAsociarDocumentos").attr("IdCheques");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Cheques.aspx/ObtenerMovimientosCobros',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosCheques')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaEditarCuentaMovimientosFacturaCliente(pFacturaEncabezado){
    $("#dialogEditarCuentaMovimientosFacturaCliente").obtenerVista({
        nombreTemplate: "tmplEditarCuentaMovimientosFacturaCliente.html",
        url: "Asientos.aspx/ObtenerFormaEditarCuentaMovimientosFacturaCliente",
        parametros: pFacturaEncabezado,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCuentaMovimientosFacturaCliente").dialog("open");
        }
    });
}

function ObtenerFormaEditarCuentaMovimientosFacturaProveedor(pFacturaProveedor){
    $("#dialogEditarCuentaMovimientosFacturaProveedor").obtenerVista({
        nombreTemplate: "tmplEditarCuentaMovimientosFacturaProveedor.html",
        url: "Asientos.aspx/ObtenerFormaEditarCuentaMovimientosFacturaProveedor",
        parametros: pFacturaProveedor,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdDetalleFacturaProveedorEditar();
            $("#dialogEditarCuentaMovimientosFacturaProveedor").dialog("open");
        }
    });
}

function FiltroDetalleFacturaProveedorEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleFacturaProveedorEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleFacturaProveedorEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleFacturaProveedorEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleFacturaProveedorEditar').getGridParam('sortorder');
    request.pIdEncabezadoFacturaProveedor = 0;
    if ($("#divEditarCuentaMovimientosFacturaProveedor").attr("idFacturaProveedor") != null && $("#divEditarCuentaMovimientosFacturaProveedor").attr("idFacturaProveedor") != "") {
        request.pIdEncabezadoFacturaProveedor = $("#divEditarCuentaMovimientosFacturaProveedor").attr("idFacturaProveedor");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Asientos.aspx/ObtenerDetalleFacturaProveedorEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleFacturaProveedorEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaEditarDetalleFacturaProveedor(pRequest) {
    $("#dialogEditarCuentaMovimientosDetalleFacturaProveedor").obtenerVista({
        nombreTemplate: "tmplEditarAsientoDetalleFacturaProveedor.html",
        url: "Asientos.aspx/ObtenerEditarAsientoDetalleFacturaProveedor",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarCuentaMovimientosDetalleFacturaProveedor").dialog("open");
        }
    });
}

function ObtenerComboCuentasContables(pRequest){
    $("#cmbCuentaContable").obtenerVista({
        nombreTemplate: "tmplComboCuentaContable.html",
        url: "Asientos.aspx/ObtenerComboCuentasContables",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function EditarCuentaMovimientosFacturaCliente(){
    var CuentaMovimientos = new Object();
    CuentaMovimientos.pIdFacturaEncabezado = $("#divEditarCuentaMovimientosFacturaCliente").attr("idFacturaEncabezado");
    CuentaMovimientos.pIdSucursal = $("#cmbSucursal").val();
    CuentaMovimientos.pIdDivision = $("#cmbDivision").val();
    SetEditarCuentaMovimientosFacturaCliente(JSON.stringify(CuentaMovimientos));
}

function SetEditarCuentaMovimientosFacturaCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/EditarCuentaMovimientosFacturaCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#dialogEditarCuentaMovimientosFacturaCliente").dialog("close");
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

function EditarCuentaMovimientosFacturaProveedor(){
    var CuentaMovimientos = new Object();
    CuentaMovimientos.pIdFacturaProveedor = $("#divEditarCuentaMovimientosFacturaProveedor").attr("idFacturaProveedor");
    CuentaMovimientos.pIdSucursal = $("#cmbSucursal").val();
    CuentaMovimientos.pIdDivision = $("#cmbDivision").val();
    SetEditarCuentaMovimientosFacturaProveedor(JSON.stringify(CuentaMovimientos));
}

function SetEditarCuentaMovimientosFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/EditarCuentaMovimientosFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#dialogEditarCuentaMovimientosFacturaProveedor").dialog("close");
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

function EditarCuentaMovimientosDetalleFacturaProveedor(){
    var CuentaMovimientos = new Object();
    CuentaMovimientos.pIdDetalleFacturaProveedor = $("#divEditarCuentaMovimientosDetalleFacturaProveedor").attr("idDetalleFacturaProveedor");
    CuentaMovimientos.pIdTipoCompra = $("#cmbTipoCompra").val();
    CuentaMovimientos.pIdCuentaContable = $("#cmbCuentaContable").val();
    SetEditarCuentaMovimientosDetalleFacturaProveedor(JSON.stringify(CuentaMovimientos));
}

function SetEditarCuentaMovimientosDetalleFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/EditarCuentaMovimientosDetalleFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#dialogEditarCuentaMovimientosDetalleFacturaProveedor").dialog("close");
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

function AgregarAsiento(){
    switch($("#dialogAgregarAsiento").attr("tipoAsiento")) {
        case "facturaCliente":
            var FacturaCliente = new Object();
            FacturaCliente.pIdFacturaCliente = $("#dialogAgregarAsiento").attr("idDocumento");
            AgregarAsientoFacturaCliente(JSON.stringify(FacturaCliente));
            break;
        case "facturaProveedor":
            var FacturaProveedor = new Object();
            FacturaProveedor.pIdFacturaProveedor = $("#dialogAgregarAsiento").attr("idDocumento");
            AgregarAsientoFacturaProveedor(JSON.stringify(FacturaProveedor));
            break;
        case "cobroCliente":
            var CobroCliente = new Object();
            CobroCliente.pIdCobroCliente = $("#dialogAgregarAsiento").attr("idDocumento");
            CobroCliente.pTipoEntrada = $("#dialogAgregarAsiento").attr("tipoEntrada");
            CobroCliente.pIdFacturaEncabezado = $("#dialogAgregarAsiento").attr("idDocumentoCobroPago");
            AgregarAsientoCobroCliente(JSON.stringify(CobroCliente));
            break;
        case "pagoProveedor":
            var PagoProveedor = new Object();
            PagoProveedor.pIdPagoProveedor = $("#dialogAgregarAsiento").attr("idDocumento");
            PagoProveedor.pTipoSalida = $("#dialogAgregarAsiento").attr("tipoSalida");
            PagoProveedor.pIdEncabezadoFactura = $("#dialogAgregarAsiento").attr("idDocumentoCobroPago");
            AgregarAsientoPagoProveedor(JSON.stringify(PagoProveedor));
            break;
        default:
    }
}

function AgregarAsientoFacturaCliente(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/AgregarAsientoFacturaCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }         
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function AgregarAsientoFacturaProveedor(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/AgregarAsientoFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function AgregarAsientoCobroCliente(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/AgregarAsientoCobroCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }           
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function AgregarAsientoPagoProveedor(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Asientos.aspx/AgregarAsientoPagoProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Paginador = new Object();
                Paginador.pTamanoPaginacion = $("#divPaginador .cmbTamanoPaginacion").val();
                Paginador.pPaginaActual = $("#divPaginador .txtPaginaActual").val();
                Paginador.pColumnaOrden = "Prueba";
                Paginador.pTipoOrden = "DESC";
                ObtenerFormaAsientos(JSON.stringify(Paginador));
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdAsientosContables").trigger("reloadGrid");
                $("#dialogAgregarAsiento").dialog("close");
            }           
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}