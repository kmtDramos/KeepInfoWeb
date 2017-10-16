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

    $("#divFiltrosReporteEstadoCrediticioClientes").on("change", "#cmbSucursal", function() {
        $("#ReporteRelacionSaldos").empty();
        $("#btnExportarSaldos").hide();
        $("#ReporteRelacionSaldosDolares").empty();
        $("#btnExportarSaldosD").hide();
        var Cliente = new Object();
        Cliente.pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
        Cliente.pNombreCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("Cliente");
        Cliente.pIdSucursal = $("#cmbSucursal").val();
        Cliente.pSucursal = $("#cmbSucursal option:selected").html();
        ObtieneEstadoCrediticioCliente(JSON.stringify(Cliente));
    });

    $("#dialogConsultarFacturaEncabezado").on("click", "#divImprimir", function() {
        var IdFacturaEncabezado = $("#divFormaConsultarFacturaEncabezado").attr("idfacturaencabezado");
        Imprimir(IdFacturaEncabezado);
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
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });
    $('#dialogConsultarProyecto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarProyecto").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });
});


//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaFiltrosReporteEstadoCrediticioClientes() {
    $("#divFiltrosReporteEstadoCrediticioClientes").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteEstadoCrediticioClientes.html",
        url: "ReporteEstadoCrediticioClientes.aspx/ObtenerFormaFiltroReporteEstadoCrediticioClientes",
        despuesDeCompilar: function(pRespuesta) {
            autocompletarCliente();
            
            var Cliente = new Object();
            Cliente.pIdCliente = 0;
            Cliente.pNombreCliente = "";
            Cliente.pIdSucursal = $("#cmbSucursal").val();
            Cliente.pSucursal = $("#cmbSucursal option:selected").html();
            ObtieneEstadoCrediticioCliente(JSON.stringify(Cliente));
            
        }
    });
}

function ObtieneEstadoCrediticioCliente(pRequest) {
    $("#divEstadoCrediticioClientes").obtenerVista({
        nombreTemplate: "tmplReporteEstadoCrediticioCliente.html",
        parametros: pRequest,
        url: "ReporteEstadoCrediticioClientes.aspx/ObtieneEstadoCrediticioCliente",
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            Inicializar_grdFacturasPendientesConsultar();
            Inicializar_grdProyectosConsultar();
            Inicializar_grdFacturacionConsultar();

            $("#grdMovimientosCobrosConsultar").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var Factura = new Object();

                Factura.pIdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobrosConsultar_IdFacturaEncabezado']").html());
                ObtenerFormaConsultarFacturaEncabezado(JSON.stringify(Factura));
            });

            $("#grdFacturasPendientesConsultar").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var Factura = new Object();

                Factura.pIdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFacturasPendientesConsultar_IdFacturaEncabezado']").html());
                ObtenerFormaConsultarFacturaEncabezado(JSON.stringify(Factura));
            });

            $("#grdFacturacionConsultar").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var Factura = new Object();

                Factura.pIdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFacturacionConsultar_IdFacturaEncabezado']").html());
                ObtenerFormaConsultarFacturaEncabezado(JSON.stringify(Factura));
            });

            $("#grdProyectosConsultar").on("dblclick", "td", function() {
                var registro = $(this).parents("tr");
                var Proyecto = new Object();
                Proyecto.pIdProyecto = parseInt($(registro).children("td[aria-describedby='grdProyectosConsultar_IdProyecto']").html());
                ObtenerFormaConsultarProyecto(JSON.stringify(Proyecto));
            });



            $("#tabConsultarEstadoCrediticioCliente").tabs();
            $('#Proyectos').on('click', '#chkPorEstatus', function(event) {
                FiltroProyectosConsultar();
            });
        }
    });
}

function autocompletarCliente() {
    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $("#txtRazonSocial").val();
            pRequest.pIdSucursal = $("#cmbSucursal").val();
            $.ajax({
                type: 'POST',
                url: 'ReporteEstadoCrediticioClientes.aspx/BuscarRazonSocial',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdCliente = ui.item.id;
            var RazonSocial = ui.item.value;
            $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente", pIdCliente);
            $("#divFiltrosReporteEstadoCrediticioClientes").attr("Cliente", RazonSocial);
            $("#ReporteRelacionSaldos").empty();
            $("#btnExportarSaldos").hide();
            $("#btnExportarSaldosD").hide();

            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            Cliente.pNombreCliente = RazonSocial;
            Cliente.pIdSucursal = $("#cmbSucursal").val();
            Cliente.pSucursal = $("#cmbSucursal option:selected").html();
            ObtieneEstadoCrediticioCliente(JSON.stringify(Cliente));
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });

}

function FiltroMovimientosCobrosConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortorder');
    request.pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
    request.pIdSucursal = $("#cmbSucursal").val();
    var pRequest = JSON.stringify(request);
    $.ajax({
    url: 'ReporteEstadoCrediticioClientes.aspx/ObtenerMovimientosCobrosConsultar',
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

function FiltroFacturasPendientesConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturasPendientesConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturasPendientesConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturasPendientesConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturasPendientesConsultar').getGridParam('sortorder');
    request.pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
    request.pIdSucursal = $("#cmbSucursal").val();
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ReporteEstadoCrediticioClientes.aspx/ObtenerFacturasPendientesConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturasPendientesConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroProyectosConsultar() {
    var request = new Object();
    
    request.pTamanoPaginacion = $('#grdProyectosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProyectosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdProyectosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdProyectosConsultar').getGridParam('sortorder');
    request.pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
    if ($("#chkPorEstatus").is(':checked')) {
        request.pEstatusProyecto = 1;
    }
    else {
        request.pEstatusProyecto = 0;
    }
    request.pIdSucursal = $("#cmbSucursal").val();
    var pRequest = JSON.stringify(request);
    $.ajax({
    url: 'ReporteEstadoCrediticioClientes.aspx/ObtenerProyectosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdProyectosConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturacionConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturacionConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturacionConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturacionConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturacionConsultar').getGridParam('sortorder');
    request.pIdCliente = $("#divFiltrosReporteEstadoCrediticioClientes").attr("idCliente");
    request.pIdSucursal = $("#cmbSucursal").val();
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ReporteEstadoCrediticioClientes.aspx/ObtenerFacturacionConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturacionConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerSaldosDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldos").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldos.html",
        url: "ReporteEstadoCrediticioClientes.aspx/ObtieneRelacionSaldos",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldos").show();
        }
    });
}

function ObtenerSaldosDolaresDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldosDolares").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldosDolares.html",
        url: "ReporteEstadoCrediticioClientes.aspx/ObtieneRelacionSaldosDolares",
        despuesDeCompilar: function(pRespuesta) {
        $("#btnExportarSaldosD").show();
        }
    });
}

function ObtenerSaldosPesosGeneralDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldosPesosGeneral").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldosPesosGeneral.html",
        url: "ReporteEstadoCrediticioClientes.aspx/ObtieneRelacionSaldosPesosGeneral",
        despuesDeCompilar: function(pRespuesta) {
            $("#btnExportarSaldosGeneral").show();
        }
    });
}

function ObtenerSaldosPesosGeneralTCDDeReporteEstadoCrediticioClientes() {
    var Cliente = new Object();
    Cliente.pIdSucursal = $("#cmbSucursal").val();
    Cliente.pSucursal = $("#cmbSucursal option:selected").html();
    var pRequest = JSON.stringify(Cliente);
    $("#ReporteRelacionSaldosPesosGeneralTCD").obtenerVista({
        parametros: pRequest,
        nombreTemplate: "tmplReporteEstadoCrediticioClienteSaldosPesosGeneralTCD.html",
        url: "ReporteEstadoCrediticioClientes.aspx/ObtieneRelacionSaldosPesosGeneralTCD",
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