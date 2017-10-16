//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltroReporteInventarioActualAlmacenProducto();

    $("#divContenido").on("click", "#btnImprimirInventarioActualAlmacenProducto, #btnImprimirInventarioActualAlmacenProductoArriba", function() {
        ImprimirInventarioActualAlmacenProducto();
    });

    $("#divFiltrosReporteInventarioActualAlmacenProducto").on("change", "#cmbAlmacen", function() {
        $("#divFiltrosReporteInventarioActualAlmacenProducto").attr("idAlmacen", $("#cmbAlmacen").val());
        FiltroInventarioActualAlmacenProducto();
    });

    //    $("#divFiltrosReporteEstadoCuentaClientes").on("change", "#cmbTipoCambio", function() {
    //        FiltroEstadoCuentaClientes();
    //    });

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'InventarioActualAlmacenProducto');
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

function ImprimirInventarioActualAlmacenProducto() {
    MostrarBloqueo();


    var Almacen = new Object();
    Almacen.pFechaInicial = "";
    Almacen.pFechaFinal = "";
    Almacen.pTemplate = 'InventarioActualAlmacenProducto';
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

    $.ajax({
        type: "POST",
        url: "ReporteInventarioActualAlmacenProducto.aspx/ImprimirInventarioActualAlmacenProducto",
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

function ObtenerFormaFiltroReporteInventarioActualAlmacenProducto() {
    $("#divFiltrosReporteInventarioActualAlmacenProducto").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteInventarioActualAlmacenProducto.html",
        url: "ReporteInventarioActualAlmacenProducto.aspx/ObtenerFormaFiltroReporteInventarioActualAlmacenProducto",
        despuesDeCompilar: function(pRespuesta) {
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroInventarioActualAlmacenProducto();
                    }
                });
            }
            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroInventarioActualAlmacenProducto();
                    }
                });
            }
        }
    });
}

function FiltroInventarioActualAlmacenProducto() {
    var Almacen = new Object();
    Almacen.pFechaInicial = "";
    Almacen.pFechaFinal = "";

    if ($("#divFiltrosReporteInventarioActualAlmacenProducto").attr("idAlmacen") != 0) {

        if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
            Almacen.pFechaInicial = $("#txtFechaInicial").val();
            Almacen.pFechaInicial = ConvertirFecha(Almacen.pFechaInicial, 'aaaammdd');
        }
        if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
            Almacen.pFechaFinal = $("#txtFechaFinal").val();
            Almacen.pFechaFinal = ConvertirFecha(Almacen.pFechaFinal, 'aaaammdd');
        }
        Almacen.pIdAlmacen = $("#cmbAlmacen").val();

        ObtieneInventarioActualAlmacenProducto(JSON.stringify(Almacen));
    }
    else {
        $("#tblReporte tbody").html("");
    }

}

function ObtieneInventarioActualAlmacenProducto(pRequest) {
    $("#divInventarioActualAlmacenProducto").obtenerVista({
    nombreTemplate: "tmplReporteInventarioActualAlmacenProducto.html",                   
        parametros: pRequest,
        url: "ReporteInventarioActualAlmacenProducto.aspx/ObtieneInventarioActualAlmacenProducto",
        despuesDeCompilar: function(pRespuesta) {

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
                url: 'ReporteEstadoCuentaClientes.aspx/BuscarRazonSocial',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFiltrosReporteEstadoCuentaClientes").attr("idCliente", "0");
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
            $("#divFiltrosReporteEstadoCuentaClientes").attr("idCliente", pIdCliente);

            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            Cliente.pFechaInicial = "";
            Cliente.pFechaFinal = "";

            if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
                Cliente.pFechaInicial = $("#txtFechaInicial").val();
                Cliente.pFechaInicial = ConvertirFecha(Cliente.pFechaInicial, 'aaaammdd');
            }
            if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                Cliente.pFechaFinal = $("#txtFechaFinal").val();
                Cliente.pFechaFinal = ConvertirFecha(Cliente.pFechaFinal, 'aaaammdd');
            }
            Cliente.pIdSucursal = $("#cmbSucursal").val();
            Cliente.pIdTipoCambio = $("#cmbTipoCambio").val();

            ObtieneEstadoCuentaCliente(JSON.stringify(Cliente));

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
    
}