//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltroReporteExistenciaActualAlmacenProducto();

    $("#divContenido").on("click", "#btnImprimirInventarioActualGlobalUnidades, #btnImprimirInventarioActualGlobalUnidadesArriba", function() {
        ImprimirInventarioExistenciaActual();
    });

    $("#divFiltrosReporteExistencias").on("change", "#cmbAlmacen", function() {
        $("#divFiltrosReporteExistencias").attr("idAlmacen", $("#cmbAlmacen").val());
        FiltroExistencias();
    });

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'InventarioActualGlobalEnUnidades');
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

function ImprimirInventarioExistenciaActual() {
    MostrarBloqueo();

 
    var Almacen = new Object();
    Almacen.pFechaInicial = "";
    Almacen.pTemplate = 'InventarioExistencias';
    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        Almacen.pFechaInicial = $("#txtFechaInicial").val();
        Almacen.pFechaInicial = ConvertirFecha(Almacen.pFechaInicial, 'aaaammdd');
    }
    Almacen.pIdAlmacen = $("#cmbAlmacen").val();
    Almacen.pTipoImpresion = $("#cmbTipoImpresion").val();
    Almacen.pFechaIni = $("#txtFechaInicial").val();

    $.ajax({
        type: "POST",
        url: "ReporteExistencias.aspx/ImprimirInventarioExistenciaActual",
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

function ObtenerFormaFiltroReporteExistenciaActualAlmacenProducto() {
    $("#divFiltrosReporteExistencias").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteGlobalExistenciaUnidades.html",
        url: "ReporteExistencias.aspx/ObtenerFormaFiltroReporteExistenciaActualAlmacenProducto",
        despuesDeCompilar: function(pRespuesta) {
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroExistencias();
                    }
                });
            }            
            FiltroExistencias();
        }
    });
}

function FiltroExistencias() {

    var Almacen = new Object();
    Almacen.pIdAlmacen = $("#cmbAlmacen").val();
    Almacen.pFechaInicial = "";
    Almacen.pFechaFinal = "";

        if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
            Almacen.pFechaInicial = $("#txtFechaInicial").val();
            Almacen.pFechaInicial = ConvertirFecha(Almacen.pFechaInicial, 'aaaammdd');
        }

        Almacen.pFechaIni = $("#txtFechaInicial").val();
        ObtieneExistencias(JSON.stringify(Almacen));  

}

function ObtieneExistencias(pRequest) {
    $("#divInventarioExistencias").obtenerVista({
    nombreTemplate: "tmplReporteExistencias.html",                   
        parametros: pRequest,
        url: "ReporteExistencias.aspx/ObtieneExistencias",
        despuesDeCompilar: function(pRespuesta) {

        }
    });
}

