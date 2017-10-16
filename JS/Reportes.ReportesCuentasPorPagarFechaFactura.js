//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosReporteCuentasPorPagarPorFechaFactura();

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'ReporteSaldosProveedor');
    });

    $("#divFiltrosReporteCuentasPorPagarPorFechaFactura").on("change", "#cmbSucursal", function() {
        $("#txtRazonSocial").val('');
        FiltroReporteCuentasPorCobrarProveedores();

    });

   $('#divFiltrosReporteCuentasPorPagarPorFechaFactura').on('change', '#txtRazonSocial', function(event) {
        if ($("#txtRazonSocial").val() == '') {
            var pIdProveedor = 0;
            var Proveedor = new Object();
            Proveedor.pIdProveedor = pIdProveedor;
            Proveedor.pFechaInicial = "";

            if ($("#txtFechaInicial").val() != "") {
                Proveedor.pFechaInicial = $("#txtFechaInicial").val();
                Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
            }
            Proveedor.pIdSucursal = $("#cmbSucursal").val();
            Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
            ObtieneEstadoReporteCuentasPorPagarProveedor(JSON.stringify(Proveedor));
        }

    });

    var tableToExcel = (function() {
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

});


//---------Funciones--------//


//--------------------------//

//--------Validaciones-------//
//--------------------------//

//-----------AJAX------------//
//----Funciones de Accion----//

//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaFiltrosReporteCuentasPorPagarPorFechaFactura() {
    $("#divFiltrosReporteCuentasPorPagarPorFechaFactura").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteCuentasPorPagarPorFechaFactura.html",
        url: "ReporteCuentasPorPagarPorFechaFactura.aspx/ObtenerFormaFiltroReporteCuentasPorPagarPorFechaFactura",
        despuesDeCompilar: function(pRespuesta) {
            autocompletarProveedor();
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroReporteCuentasPorCobrarProveedores();
                    }
                });
                FiltroReporteCuentasPorCobrarProveedores();
            }

        }
    });
}
function FiltroReporteCuentasPorCobrarProveedores() {

    var pIdProveedor = 0;

    if ($("#txtRazonSocial").val() == "") {
        pIdProveedor = 0;
    }
    else {
        if ($("#divFiltrosReporteCuentasPorPagarPorFechaFactura").attr("idProveedor") != null && $("#divFiltrosReporteCuentasPorPagarPorFechaFactura").attr("idProveedor") != "") {
            pIdProveedor = $("#divFiltrosReporteCuentasPorPagarPorFechaFactura").attr("idProveedor");
        }
    }
    var Proveedor = new Object();
    Proveedor.pIdProveedor = pIdProveedor;
    Proveedor.pFechaInicial = "";
    
    if ($("#txtFechaInicial").val() != "") {
        Proveedor.pFechaInicial = $("#txtFechaInicial").val();
        Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
    }
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    ObtieneEstadoReporteCuentasPorPagarProveedor(JSON.stringify(Proveedor));

}

function ObtieneEstadoReporteCuentasPorPagarProveedor(pRequest) {
    $("#divEstadoCuentaProveedor").obtenerVista({
    nombreTemplate: "tmplReporteCuentasPorPagarPorFechaFactura.html",                   
        parametros: pRequest,
        url: "ReporteCuentasPorPagarPorFechaFactura.aspx/ObtieneEstadoCuentasPorCobrar",
        despuesDeCompilar: function(pRespuesta) {

        }
    });
}

function autocompletarProveedor() {
    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $("#txtRazonSocial").val();
            pRequest.pIdSucursal = $("#cmbSucursal").val();
            $.ajax({
                type: 'POST',
                url: 'ReporteCuentasPorPagarPorFechaFactura.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divFiltrosReporteCuentasPorPagarPorFechaFactura").attr("idProveedor", "0");
                    
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdProveedor }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProveedor = ui.item.id;
            $("#divFiltrosReporteCuentasPorPagarPorFechaFactura").attr("idProveedor", pIdProveedor);

            var Proveedor = new Object();
            Proveedor.pIdProveedor = pIdProveedor;
            if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
                Proveedor.pFechaInicial = $("#txtFechaInicial").val();
                Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
            }

            Proveedor.pIdSucursal = $("#cmbSucursal").val();
            Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
            //ObtieneEstadoCuentaProveedor(JSON.stringify(Proveedor));
            ObtieneEstadoReporteCuentasPorPagarProveedor(JSON.stringify(Proveedor));

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });

}
