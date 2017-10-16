//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosReporteEstadoCuentaClientes();

    $("#divContenido").on("click", "#btnImprimirEstadoCuenta, #btnImprimirEstadoCuentaArriba", function() {
        var IdCuentaBancaria = 0;
        if ($("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria") != null && $("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria") != "") {
            IdCuentaBancaria = $("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria");
        }
        if (IdCuentaBancaria != 0) {
            ImprimirEstadoCuentaBancaria(IdCuentaBancaria);
        }
        else {
            MostrarMensajeError("Seleccione una cuenta bancaria");
        }
    });

    $("#divFiltrosReporteCuentasBancariasPorRangoFechas").on("change", "#cmbSucursal", function() {
        FiltroReporteCuentasBancarias();
    });

    $("#divFiltrosReporteCuentasBancariasPorRangoFechas").on("change", "#cmbTipoCuenta", function() {
        FiltroReporteCuentasBancarias();
    });

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'ReporteEstadoCuenta');
    });

    var tableToExcel = (function() {
        var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
        return function(table, name) {
            if (!table.nodeType) table = document.getElementById(table)
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
            window.location.href = uri + base64(format(template, ctx))
        }
    })()


});

function ImprimirEstadoCuentaBancaria(pIdCuentaBancaria) {
    MostrarBloqueo();
    
    var pRequest = new Object();
    pRequest.pTemplate = 'EstadoCuentasBancarias';
    pRequest.pIdCuentaBancaria = pIdCuentaBancaria;

    pRequest.pFechaInicial = "";
    pRequest.pFechaFinal = "";

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        pRequest.pFechaInicial = $("#txtFechaInicial").val();
        pRequest.pFechaInicial = ConvertirFecha(pRequest.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        pRequest.pFechaFinal = $("#txtFechaFinal").val();
        pRequest.pFechaFinal = ConvertirFecha(pRequest.pFechaFinal, 'aaaammdd');
    }

    pRequest.pIdSucursal = $("#cmbSucursal").val();
    pRequest.pIdTipoCuenta = $("#cmbTipoCuenta").val();
    pRequest.pFechaIni = $("#txtFechaInicial").val();
    pRequest.pFechaF = $("#txtFechaFinal").val();
    pRequest.pSucursal = $("#cmbSucursal option:selected").html();
    pRequest.pTipoImpresion = $("#cmbTipoImpresion").val();

    $.ajax({
        type: "POST",
        url: "ReporteCuentasBancariasPorRangoFechas.aspx/ImprimirEstadoCuentaBancaria",
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
//--------Validaciones-------//
//--------------------------//

//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaFiltrosReporteEstadoCuentaClientes() {
    $("#divFiltrosReporteCuentasBancariasPorRangoFechas").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteCuentasBancariasPorRangoFechas.html",
        url: "ReporteCuentasBancariasPorRangoFechas.aspx/ObtenerFormaFiltroReporteCuentasBancarias",
        despuesDeCompilar: function(pRespuesta) {
            autocompletarCuentaBancaria();
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroReporteCuentasBancarias();
                    }
                });
            }
            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroReporteCuentasBancarias();
                    }
                });
            }
        }
    });
}
function FiltroReporteCuentasBancarias() {

    var pIdCuentaBancaria = 0;
    if ($("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria") != null && $("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria") != "") {
        pIdCuentaBancaria = $("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria");
    }

    var CuentaBancaria = new Object();
    CuentaBancaria.pIdCuentaBancaria = pIdCuentaBancaria;
    CuentaBancaria.pFechaInicial = "";
    CuentaBancaria.pFechaFinal = "";

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        CuentaBancaria.pFechaInicial = $("#txtFechaInicial").val();
        CuentaBancaria.pFechaInicial = ConvertirFecha(CuentaBancaria.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        CuentaBancaria.pFechaFinal = $("#txtFechaFinal").val();
        CuentaBancaria.pFechaFinal = ConvertirFecha(CuentaBancaria.pFechaFinal, 'aaaammdd');
    }
    CuentaBancaria.pIdSucursal = $("#cmbSucursal").val();

    CuentaBancaria.pIdTipoCuenta = $("#cmbTipoCuenta").val();
    CuentaBancaria.pFechaIni = $("#txtFechaInicial").val();
    CuentaBancaria.pFechaF = $("#txtFechaFinal").val();
    CuentaBancaria.pSucursal = $("#cmbSucursal option:selected").html();
    ObtieneReporteCuentasBancarias(JSON.stringify(CuentaBancaria));

}

function ObtieneReporteCuentasBancarias(pRequest) {
    $("#divReporteCuentasBancariasPorRangoFechas").obtenerVista({
    nombreTemplate: "tmplReporteCuentasBancariasPorRangoFecha.html",                   
        parametros: pRequest,
        url: "ReporteCuentasBancariasPorRangoFechas.aspx/ObtieneReporteCuentasBancarias",
        despuesDeCompilar: function(pRespuesta) {

        }
    });
}

function autocompletarCuentaBancaria() {
    $('#txtCuentaBancaria').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pCuentaBancaria = $("#txtCuentaBancaria").val();
            pRequest.pIdSucursal = $("#cmbSucursal").val();
            $.ajax({
                type: 'POST',
                url: 'ReporteCuentasBancariasPorRangoFechas.aspx/BuscarCuentaBancaria',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFiltrosReporteEstadoCuentaClientes").attr("idCuentaBancaria", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.CuentaBancaria, value: item.CuentaBancaria, id: item.IdCuentaBancaria }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdCuentaBancaria = ui.item.id;
            $("#divFiltrosReporteCuentasBancariasPorRangoFechas").attr("idCuentaBancaria", pIdCuentaBancaria);

            var CuentaBancaria = new Object();
            CuentaBancaria.pIdCuentaBancaria = pIdCuentaBancaria;
            CuentaBancaria.pFechaInicial = "";
            CuentaBancaria.pFechaFinal = "";

            if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
                CuentaBancaria.pFechaInicial = $("#txtFechaInicial").val();
                CuentaBancaria.pFechaInicial = ConvertirFecha(CuentaBancaria.pFechaInicial, 'aaaammdd');
            }
            if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                CuentaBancaria.pFechaFinal = $("#txtFechaFinal").val();
                CuentaBancaria.pFechaFinal = ConvertirFecha(CuentaBancaria.pFechaFinal, 'aaaammdd');
            }
            CuentaBancaria.pIdSucursal = $("#cmbSucursal").val();
            CuentaBancaria.pIdTipoCuenta = $("#cmbTipoCuenta").val();
            CuentaBancaria.pFechaIni = $("#txtFechaInicial").val();
            CuentaBancaria.pFechaF = $("#txtFechaFinal").val();
            CuentaBancaria.pSucursal = $("#cmbSucursal option:selected").html();
            ObtieneReporteCuentasBancarias(JSON.stringify(CuentaBancaria));

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
    
}