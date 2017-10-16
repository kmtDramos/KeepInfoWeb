//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltroReporteInventarioImportesGlobal();

    $("#divContenido").on("click", "#btnImprimirInventarioImportesGlobal, #btnImprimirInventarioImportesGlobalArriba", function() {
        ImprimirInventarioImportesGlobal();
    });

    $("#divFiltrosReporteInventarioImportesGlobal").on("change", "#cmbAlmacen", function() {
        $("#divFiltrosReporteInventarioImportesGlobal").attr("idAlmacen", $("#cmbAlmacen").val());
        FiltroInventarioImportesGlobal();
    });

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'InventarioImportesGlobal');
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

function ImprimirInventarioImportesGlobal() {
    MostrarBloqueo();


    var Almacen = new Object();
    Almacen.pFechaInicial = "";
    Almacen.pFechaFinal = "";
    Almacen.pTemplate = 'InventarioImportesGlobal';
    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        Almacen.pFechaInicial = $("#txtFechaInicial").val();
        Almacen.pFechaInicial = ConvertirFecha(Almacen.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        Almacen.pFechaFinal = $("#txtFechaFinal").val();
        Almacen.pFechaFinal = ConvertirFecha(Almacen.pFechaFinal, 'aaaammdd');
    }
    Almacen.pIdAlmacen = $("#cmbAlmacen").val();
    Almacen.pTipoImpresion = $("#cmbTipoImpresion").val();
    Almacen.pFechaIni = $("#txtFechaInicial").val();
    Almacen.pFechaF = $("#txtFechaFinal").val();

    $.ajax({
        type: "POST",
        url: "ReporteInventarioImportesGlobalCD.aspx/ImprimirInventarioImportesGlobal",
        data: JSON.stringify(Almacen),
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

function ObtenerFormaFiltroReporteInventarioImportesGlobal() {
    $("#divFiltrosReporteInventarioImportesGlobal").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteInventarioImportesGlobal.html",
        url: "ReporteInventarioImportesGlobalCD.aspx/ObtenerFormaFiltroReporteInventarioImportesGlobal",
        despuesDeCompilar: function(pRespuesta) {
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroInventarioImportesGlobal();
                    }
                });
            }
            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroInventarioImportesGlobal();
                    }
                });
            }
            FiltroInventarioImportesGlobal();
        }
    });
}

function FiltroInventarioImportesGlobal() {

    var Almacen = new Object();
    Almacen.pIdAlmacen = $("#cmbAlmacen").val();
    Almacen.pFechaInicial = "";
    Almacen.pFechaFinal = "";

        if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
            Almacen.pFechaInicial = $("#txtFechaInicial").val();
            Almacen.pFechaInicial = ConvertirFecha(Almacen.pFechaInicial, 'aaaammdd');
        }
        if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
            Almacen.pFechaFinal = $("#txtFechaFinal").val();
            Almacen.pFechaFinal = ConvertirFecha(Almacen.pFechaFinal, 'aaaammdd');
        }

        Almacen.pFechaIni = $("#txtFechaInicial").val();
        Almacen.pFechaF = $("#txtFechaFinal").val();
        ObtieneInventarioImportesGlobal(JSON.stringify(Almacen));
  

}

function ObtieneInventarioImportesGlobal(pRequest) {
    $("#divInventarioImportesGlobal").obtenerVista({
    nombreTemplate: "tmplReporteInventarioImportesGlobal.html",                   
        parametros: pRequest,
        url: "ReporteInventarioImportesGlobalCD.aspx/ObtieneInventarioImportesGlobal",
        despuesDeCompilar: function(pRespuesta) {

        }
    });
}

