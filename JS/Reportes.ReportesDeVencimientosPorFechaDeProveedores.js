//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosReporteVencimientosPorFechaDeProveedores();

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'ReporteDeFacturasDeVencimientoPorProveedor');
    });

    $("#divFiltrosReporteVencimientosPorFechaDeProveedores").on("change", "#cmbSucursal", function() {
        FiltroEstadoCuentaProveedores();
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

function ObtenerFormaFiltrosReporteVencimientosPorFechaDeProveedores() {
    $("#divFiltrosReporteVencimientosPorFechaDeProveedores").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteVencimientosPorFechaDeProveedores.html",
        url: "ReporteDeVencimientosPorFechaDeProveedores.aspx/ObtenerFormaFiltroReporteEstadoCuentaProveedores",
        despuesDeCompilar: function(pRespuesta) {
            autocompletarProveedor();
            var FechaFinal = $("#txtFechaFinal").val();
            if ($("#txtFechaInicial").length) {

                $("#txtFechaInicial").datepicker({
                    onSelect: function() {

                        var fullDate = new Date()
                        var month = '' + (fullDate.getMonth() + 1);
                        var day = '' + fullDate.getDate();
                        var year = fullDate.getFullYear();

                        if (month.length < 2) month = '0' + month;
                        if (day.length < 2) day = '0' + day;

                        var Fecha = [day, month, year].join('/');
                        if (ConvertirFecha($("#txtFechaInicial").val(), 'aaaammdd') < ConvertirFecha(Fecha, 'aaaammdd') || ConvertirFecha($("#txtFechaInicial").val(), 'aaaammdd') > ConvertirFecha($("#txtFechaFinal").val(), 'aaaammdd')) {
                            $("#txtFechaInicial").val(Fecha);

                        }

                        FiltroEstadoCuentaProveedores();
                    }
                });
            }
            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {

                        if (ConvertirFecha($("#txtFechaFinal").val(), 'aaaammdd') < ConvertirFecha($("#txtFechaInicial").val(), 'aaaammdd')) {
                            $("#txtFechaFinal").val(FechaFinal);
                        }

                        FiltroEstadoCuentaProveedores();
                    }
                });
            }


        }
    });
}
function FiltroEstadoCuentaProveedores() {

    var pIdProveedor = 0;
    if ($("#divFiltrosReporteVencimientosPorFechaDeProveedores").attr("idProveedor") != null && $("#divFiltrosReporteVencimientosPorFechaDeProveedores").attr("idProveedor") != "") {
        pIdProveedor = $("#divFiltrosReporteVencimientosPorFechaDeProveedores").attr("idProveedor");
    }
   
    var Proveedor = new Object();
    Proveedor.pIdProveedor = pIdProveedor;
    Proveedor.pFechaInicial = "";
    Proveedor.pFechaFinal = "";

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        Proveedor.pFechaInicial = $("#txtFechaInicial").val();
        Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        Proveedor.pFechaFinal = $("#txtFechaFinal").val();
        Proveedor.pFechaFinal = ConvertirFecha(Proveedor.pFechaFinal, 'aaaammdd');
    }
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
    Proveedor.pFechaIni = $("#txtFechaInicial").val();
    Proveedor.pFechaF = $("#txtFechaFinal").val();
    ObtieneReporteProveedorVencimientosPorProveedor(JSON.stringify(Proveedor));

}

function ObtieneReporteProveedorVencimientosPorProveedor(pRequest) {
    $("#divReporteDeProveedoresDeVencimientosPorFecha").obtenerVista({
    nombreTemplate: "tmplReporteVencimientosPorFechaProveedor.html",                   
        parametros: pRequest,
        url: "ReporteDeVencimientosPorFechaDeProveedores.aspx/ObtieneReporteVencimientosPorProveedor",
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
                url: 'ReporteDeVencimientosPorFechaDeProveedores.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divFiltrosReporteVencimientosPorFechaDeProveedores").attr("idProveedor", "0");
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
            $("#divFiltrosReporteVencimientosPorFechaDeProveedores").attr("idProveedor", pIdProveedor);

            var Proveedor = new Object();
            Proveedor.pIdProveedor = pIdProveedor;
            Proveedor.pFechaFinal = "";
            if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
                Proveedor.pFechaInicial = $("#txtFechaInicial").val();
                Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
            }

            if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                Proveedor.pFechaFinal = $("#txtFechaFinal").val();
                Proveedor.pFechaFinal = ConvertirFecha(Proveedor.pFechaFinal, 'aaaammdd');
            }

            Proveedor.pIdSucursal = $("#cmbSucursal").val();
            Proveedor.pSucursal = $("#cmbSucursal option:selected").html();
            Proveedor.pFechaIni = $("#txtFechaInicial").val();
            Proveedor.pFechaF = $("#txtFechaFinal").val();
            ObtieneReporteProveedorVencimientosPorProveedor(JSON.stringify(Proveedor));

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
    
}