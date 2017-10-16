//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosReporteEstadoCrediticioProveedores();

    $("#divContenido").on("click", "#btnExportarSaldos", function() {
        tableToExcel('tblReporteSaldos', 'ReporteCrediticioRelacionSaldos');
    });

    $("#divContenido").on("click", "#btnExportarSaldosGeneral", function() {
        tableToExcel('tblReporteSaldosPesosGeneral', 'ReporteCrediticioRelacionSaldosPesosGeneral');
    });

    $("#divContenido").on("click", "#btnExportarSaldosGeneralTCD", function() {
        tableToExcel('tblReporteSaldosPesosGeneralTCD', 'ReporteCrediticioRelacionSaldosPesosGeneralTCD');
    });

    $("#divContenido").on("click", "#btnExportarSaldosD", function() {
        tableToExcel('tblReporteSaldosDolares', 'ReporteCrediticioRelacionSaldosDolares');
    });
    
    $("#divContenido").on("click", "#btnExportarSaldosTotales", function() {
        tableToExcel('tblReporteSaldosTotales', 'ReporteCrediticioRelacionSaldosPesosGeneralConTCD');
    });
    
    $("#divContenido").on("click", "#btnExportarSaldosTotalesTCDelDia", function() {
        tableToExcel('tblReporteSaldosTotalesTCDelDia', 'ReporteCrediticioRelacionSaldosPesosGeneralConTCDDia');
    });
    
    $("#divContenido").on("click", "#btnMostarSaldos", function() {
        ObtenerSaldosDeReporteEstadoCrediticioProveedores();

    });

    $("#divContenido").on("click", "#btnMostarSaldosD", function() {
        ObtenerSaldosDolaresDeReporteEstadoCrediticioProveedores();

    });

    $("#divContenido").on("click", "#btnMostarSaldosGeneral", function() {
        ObtenerSaldosPesosGeneralDeReporteEstadoCrediticioProveedores();

    });

    $("#divContenido").on("click", "#btnMostarSaldosGeneralTCD", function() {
        ObtenerSaldosPesosGeneralTCDDeReporteEstadoCrediticioProveedores();

    });

    $("#divContenido").on("click", "#btnMostarSaldosTotales", function() {
        ObtenerSaldosTotalesDeReporteEstadoCrediticioProveedores();

    });
    $("#divContenido").on("click", "#btnMostarSaldosTotalesTCDelDia", function() {
        ObtenerSaldosTotalesTCDelDiaDeReporteEstadoCrediticioProveedores();

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

    $("#divFiltrosReporteEstadoCrediticioProveedores").on("change", "#cmbSucursal", function() {
        $("#ReporteRelacionSaldos").empty();
        $("#btnExportarSaldos").hide();
        $("#ReporteRelacionSaldosDolares").empty();
        $("#btnExportarSaldosD").hide();
        $("#ReporteRelacionSaldosPesosGeneral").empty();
        $("#btnExportarSaldosGeneral").hide();
        $("#ReporteRelacionSaldosPesosGeneralTCD").empty();
        $("#btnExportarSaldosGeneralTCD").hide();
    });

});



//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaFiltrosReporteEstadoCrediticioProveedores() {
    $("#divFiltrosReporteEstadoCrediticioProveedores").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteEstadoCrediticioProveedores.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtenerFormaFiltroReporteEstadoCrediticioProveedores",
        despuesDeCompilar: function(pRespuesta) {
            ObtieneFormaReporteCrediticioProveedor();
            $("#txtFecha").datepicker();
            
        }
    });
}

function ObtieneFormaReporteCrediticioProveedor() {
    $("#divEstadoCrediticioProveedores").obtenerVista({
        nombreTemplate: "tmplReporteEstadoCrediticioProveedor.html",
        despuesDeCompilar: function(pRespuesta) {
            $("#tabConsultarEstadoCrediticioProveedor").tabs();
        }
    });
}

function ObtenerSaldosDeReporteEstadoCrediticioProveedores() {
    var Proveedor = new Object();
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFecha = $("#txtFecha").val();
    var pRequest = JSON.stringify(Proveedor);
    $("#ReporteRelacionSaldos").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioProveedorSaldos.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtieneRelacionSaldos",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldos").show();
        }
    });
}

function ObtenerSaldosDolaresDeReporteEstadoCrediticioProveedores() {
    var Proveedor = new Object();
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFecha = $("#txtFecha").val();
    var pRequest = JSON.stringify(Proveedor);
    $("#ReporteRelacionSaldosDolares").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioProveedorSaldosDolares.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtieneRelacionSaldosDolares",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldosD").show();
        }
    });
}

function ObtenerSaldosPesosGeneralDeReporteEstadoCrediticioProveedores() {
    var Proveedor = new Object();
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFecha = $("#txtFecha").val();
    var pRequest = JSON.stringify(Proveedor);
    $("#ReporteRelacionSaldosPesosGeneral").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioProveedorSaldosPesosGeneral.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtieneRelacionSaldosPesosGeneral",
        despuesDeCompilar: function(pRespuesta) {
            $("#btnExportarSaldosGeneral").show();
        }
    });
}

function ObtenerSaldosPesosGeneralTCDDeReporteEstadoCrediticioProveedores() {
    var Proveedor = new Object();
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFecha = $("#txtFecha").val();
    var pRequest = JSON.stringify(Proveedor);
    $("#ReporteRelacionSaldosPesosGeneralTCD").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioProveedorSaldosPesosGeneralTCD.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtieneRelacionSaldosPesosGeneralTCD",
        despuesDeCompilar: function(pRespuesta) {
            $("#btnExportarSaldosGeneralTCD").show();
        }
    });
}

function ObtenerSaldosTotalesDeReporteEstadoCrediticioProveedores() {
    var Proveedor = new Object();
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFecha = $("#txtFecha").val();
    var pRequest = JSON.stringify(Proveedor);
    $("#ReporteRelacionSaldosTotales").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioProveedorSaldosTotales.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtieneRelacionSaldosTotales",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldosTotales").show();
        }
    });
}
function ObtenerSaldosTotalesTCDelDiaDeReporteEstadoCrediticioProveedores() {
    var Proveedor = new Object();
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFecha = $("#txtFecha").val();
    var pRequest = JSON.stringify(Proveedor);
    $("#ReporteRelacionSaldosTotalesTCDelDia").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioProveedorSaldosTotalesTCDelDia.html",
        url: "ReporteEstadoCrediticioProveedores.aspx/ObtieneRelacionSaldosTotalesTCDelDia",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldosTotalesTCDelDia").show();
        }
    });
}
