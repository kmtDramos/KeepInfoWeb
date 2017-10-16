//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosReporteEstadoCrediticioClientes();
    //EXPORTAR PAGOS
    $("#gbox_grdMovimientosCobrosConsultar").livequery(function() {
        $("#grdMovimientosCobrosConsultar").jqGrid('navButtonAdd', '#pagMovimientosCobrosConsultar', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pIdCliente = 0;
                var pIdFuncionalidad = 1;
                var pEstatusProyecto = 0;
                var pIdSucursal = 0;

                pRazonSocial = $("#divFiltrosReporteEstadoCrediticioClientes").attr("Cliente");
                pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
                pIdSucursal = $("#cmbSucursal").val();

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pIdFuncionalidad: pIdFuncionalidad,
                    pRazonSocial: pRazonSocial,
                    pIdCliente: pIdCliente,
                    pEstatusProyecto: pEstatusProyecto,
                    pIdSucursal: pIdSucursal
                }, downloadType: 'Normal'
                });

            }
        });
    });
    //EXPORTAR FACTURAS PENDIENTES
    $("#gbox_grdFacturasPendientesConsultar").livequery(function() {
        $("#grdFacturasPendientesConsultar").jqGrid('navButtonAdd', '#pagFacturasPendientesConsultar', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pIdCliente = 0;
                var pIdFuncionalidad = 2;
                var pEstatusProyecto = 0;
                var pIdSucursal = 0;

                pRazonSocial = $("#divFiltrosReporteEstadoCrediticioClientes").attr("Cliente");
                pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
                pIdSucursal = $("#cmbSucursal").val();

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pIdFuncionalidad: pIdFuncionalidad,
                    pRazonSocial: pRazonSocial,
                    pIdCliente: pIdCliente,
                    pEstatusProyecto: pEstatusProyecto,
                    pIdSucursal: pIdSucursal
                }, downloadType: 'Normal'
                });

            }
        });
    });
    //EXPORTAR PROYECTOS
    $("#gbox_grdProyectosConsultar").livequery(function() {
        $("#grdProyectosConsultar").jqGrid('navButtonAdd', '#pagProyectosConsultar', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pIdCliente = 0;
                var pIdFuncionalidad = 3;
                var pEstatusProyecto = 0;
                var pIdSucursal = 0;

                if ($("#chkPorEstatus").is(':checked')) {
                    pEstatusProyecto = 1;
                }
                else {
                    pEstatusProyecto = 0;
                }

                pRazonSocial = $("#divFiltrosReporteEstadoCrediticioClientes").attr("Cliente");
                pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
                pIdSucursal = $("#cmbSucursal").val();

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pIdFuncionalidad: pIdFuncionalidad,
                    pRazonSocial: pRazonSocial,
                    pIdCliente: pIdCliente,
                    pEstatusProyecto: pEstatusProyecto,
                    pIdSucursal: pIdSucursal
                }, downloadType: 'Normal'
                });

            }
        });
    });

    //EXPORTAR FACTURACION
    $("#gbox_grdFacturacionConsultar").livequery(function() {
        $("#grdFacturacionConsultar").jqGrid('navButtonAdd', '#pagFacturacionConsultar', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pIdCliente = 0;
                var pIdFuncionalidad = 4;
                var pEstatusProyecto = 0;
                var pIdSucursal = 0;

                pRazonSocial = $("#divFiltrosReporteEstadoCrediticioClientes").attr("Cliente");
                pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
                pIdSucursal = $("#cmbSucursal").val();

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pIdFuncionalidad: pIdFuncionalidad,
                    pRazonSocial: pRazonSocial,
                    pIdCliente: pIdCliente,
                    pEstatusProyecto: pEstatusProyecto,
                    pIdSucursal: pIdSucursal
                }, downloadType: 'Normal'
                });

            }
        });
    });

    $("#divContenido").on("click", "#btnExportar", function() {
        tableToExcel('tblReporte2', 'ReporteCrediticioInformacionGeneral', 'prueba.xls');
    });

    $("#divContenido").on("click", "#btnMostarSaldos", function() {
        ObtenerSaldosDeReporteEstadoCrediticioClientes();

    });

    $("#divContenido").on("click", "#btnExportarSaldos", function() {
        tableToExcelSaldos('tblReporteSaldos', 'ReporteCrediticioRelacionSaldos');
    });

    $("#divContenido").on("click", "#btnExportarSaldosGeneral", function() {
        tableToExcelSaldosPesosGeneral('tblReporteSaldosPesosGeneral', 'ReporteCrediticioRelacionSaldosPesosGeneral');
    });

    $("#divContenido").on("click", "#btnExportarSaldosGeneralTCD", function() {
        tableToExcelSaldosPesosGeneralTCD('tblReporteSaldosPesosGeneralTCD', 'ReporteCrediticioRelacionSaldosPesosGeneralTCD');
    });

    $("#divContenido").on("click", "#btnMostarSaldosD", function() {
        ObtenerSaldosDolaresDeReporteEstadoCrediticioClientes();

    });

    $("#divContenido").on("click", "#btnMostarSaldosGeneral", function() {
        ObtenerSaldosPesosGeneralDeReporteEstadoCrediticioClientes();

    });

    $("#divContenido").on("click", "#btnMostarSaldosGeneralTCD", function() {
        ObtenerSaldosPesosGeneralTCDDeReporteEstadoCrediticioClientes();

    });

    $("#divContenido").on("click", "#btnExportarSaldosD", function() {
        tableToExcelSaldosDolares('tblReporteSaldosDolares', 'ReporteCrediticioRelacionSaldosDolares');
    });

    var tableToExcel = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">.Datos{font-size:16px;}</style><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name, pr) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))
        }
    })()

    var tableToExcelSaldos = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">.Datos{font-size:16px;}</style><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))

        }
    })()

    var tableToExcelSaldosPesosGeneral = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">.Datos{font-size:16px;}</style><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))

        }
    })()

    var tableToExcelSaldosPesosGeneralTCD = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">.Datos{font-size:16px;}</style><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))

        }
    })()

    var tableToExcelSaldosDolares = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><meta http-equiv="Content-Type" content="text/html; charset=utf-8" /><style type="text/css">.Datos{font-size:16px;}</style><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))
        }
    })()

    $("#dialogConsultarFacturaEncabezado").on("click", "#divImprimir", function() {
        var IdFacturaEncabezado = $("#divFormaConsultarFacturaEncabezado").attr("idfacturaencabezado");
        Imprimir(IdFacturaEncabezado);
    });
    
});


//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaFiltrosReporteEstadoCrediticioClientes() {
    $("#divFiltrosReporteEstadoCrediticioClientes").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteEstadoCrediticioClientesAsignados.html",
        url: "ReporteEstadoCrediticioClientesAsignados.aspx/ObtenerFormaFiltroReporteEstadoCrediticioClientes",
        despuesDeCompilar: function(pRespuesta) {
            var Cliente = new Object();
            Cliente.pIdFiltroClientesAsignados = $("#cmbFiltroClientesAsignados").val();
            ObtieneEstadoCrediticioCliente(JSON.stringify(Cliente));
        }
    });
}

function ObtieneEstadoCrediticioCliente(pRequest) {
    $("#divEstadoCrediticioClientes").obtenerVista({
        nombreTemplate: "tmplReporteEstadoCrediticioClienteAsignados.html",
        parametros: pRequest,
        url: "ReporteEstadoCrediticioClientesAsignados.aspx/ObtieneEstadoCrediticioCliente",
        despuesDeCompilar: function(pRespuesta) {
            $("#tabConsultarEstadoCrediticioCliente").tabs();
        }
    });
}

function ObtenerSaldosDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    Cliente.pIdFiltroClientesAsignados = $("#cmbFiltroClientesAsignados").val();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldos").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldos.html",
        url: "ReporteEstadoCrediticioClientesAsignados.aspx/ObtieneRelacionSaldos",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldos").show();
        }
    });
}

function ObtenerSaldosDolaresDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    Cliente.pIdFiltroClientesAsignados = $("#cmbFiltroClientesAsignados").val();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldosDolares").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldosDolares.html",
        url: "ReporteEstadoCrediticioClientesAsignados.aspx/ObtieneRelacionSaldosDolares",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldosD").show();
        }
    });
}

function ObtenerSaldosPesosGeneralDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    Cliente.pIdFiltroClientesAsignados = $("#cmbFiltroClientesAsignados").val();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldosPesosGeneral").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldosPesosGeneral.html",
        url: "ReporteEstadoCrediticioClientesAsignados.aspx/ObtieneRelacionSaldosPesosGeneral",
        despuesDeCompilar: function(pRespuesta) {
            $("#btnExportarSaldosGeneral").show();
        }
    });
}

function ObtenerSaldosPesosGeneralTCDDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    Cliente.pIdFiltroClientesAsignados = $("#cmbFiltroClientesAsignados").val();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldosPesosGeneralTCD").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldosPesosGeneralTCD.html",
        url: "ReporteEstadoCrediticioClientesAsignados.aspx/ObtieneRelacionSaldosPesosGeneralTCD",
        despuesDeCompilar: function(pRespuesta) {
            $("#btnExportarSaldosGeneralTCD").show();
        }
    });
}
function ObtenerFormaConsultarFacturaEncabezado(pIdFacturaEncabezado) {
    $("#dialogConsultarFacturaEncabezado").obtenerVista({
        nombreTemplate: "tmplConsultarFacturaEncabezado.html",
        url: "FacturaCliente.aspx/ObtenerFormaConsultarFacturaEncabezado",
        parametros: pIdFacturaEncabezado,
        despuesDeCompilar: function(pRespuesta) {
        $("#divDireccionesTabs").tabs();
        Inicializar_grdFacturaDetalleConsultar();
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
    if ($("#divFormaConsultarFacturaEncabezado").attr("IdFacturaEncabezado") != null && $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != "") {
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
function Imprimir(pIdFacturaEncabezado) {
    MostrarBloqueo();

    var pRequest = new Object();
    pRequest.pTemplate = 'notaVenta';
    pRequest.pIdFacturaEncabezado = pIdFacturaEncabezado;

    $.ajax({
        type: "POST",
        url: "ReporteEstadoCrediticioClientes.aspx/ImprimirDoc",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == "0") {
                var ruta = respuesta.Modelo.Archivo;
                var imp = ruta.split('|');
                if (imp[0] == '0') { $("[id*=btnDescarga]").click(); } else { MostrarMensajeError(imp[1]); }
            } else { MostrarMensajeError(respuesta.Descripcion); }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function ObtenerFormaConsultarProyecto(pIdProyecto) {
    $("#dialogConsultarProyecto").obtenerVista({
        nombreTemplate: "tmplConsultarProyecto.html",
        url: "Proyecto.aspx/ObtenerFormaConsultarProyecto",
        parametros: pIdProyecto,
        despuesDeCompilar: function(pRespuesta) {

            $("#dialogConsultarProyecto").dialog("open");
            var progreso = parseInt(pRespuesta.modelo.Progreso);
            barraColores(progreso);

        }
    });
}

function barraColores(valor) {
    if (valor >= 100) {
        $("#barColor").css({ "background": "Red", "width": valor + '%' });
    } else if (valor > 80 && valor < 99) {
        $("#barColor").css({ "background": "Orange", "width": valor + '%' });
    } else if (valor > 50 && valor < 79) {
        $("#barColor").css({ "background": "Yellow", "width": valor + '%' });
    } else if (valor > 1 && valor < 49) {
        $("#barColor").css({ "background": "LightGreen", "width": valor + '%' });
    } else {
        $("#barColor").css({ "background": "white", "width": valor + '%' });
    }

    valor = valor > 100 ? 100 : valor;
    $("#progresoEnTiempo").progressbar({
        value: valor
    });

    $("#spanPorcientoProgreso").html(valor);

    //regresar el formato
    new Date($("#txtFechaInicio, #txtFechaTermino").datepicker("option", "dateFormat", 'dd/mm/yy').val());
}