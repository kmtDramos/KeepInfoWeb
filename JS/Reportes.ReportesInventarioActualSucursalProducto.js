//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltroReporteInventarioActualSucursalProducto();
    FiltroInventarioActualSucursalProducto();
    $("#divContenido").on("click", "#btnImprimirInventarioActualSucursalProducto, #btnImprimirInventarioActualSucursalProductoArriba", function() {
        ImprimirInventarioActualSucursalProducto();
    });

    $("#divFiltrosReporteInventarioActualSucursalProducto").on("change", "#cmbSucursal", function() {
        $("#divFiltrosReporteInventarioActualSucursalProducto").attr("idSucursal", $("#cmbSucursal").val());
        FiltroInventarioActualSucursalProducto();
    });

    //    $("#divFiltrosReporteEstadoCuentaClientes").on("change", "#cmbTipoCambio", function() {
    //        FiltroEstadoCuentaClientes();
    //    });

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'InventarioActualSucursalProducto');
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

function ImprimirInventarioActualSucursalProducto() {
    MostrarBloqueo();
    var Sucursal = new Object();
    Sucursal.pIdProducto = $("#txtClaveProducto").attr('IdProducto');
    Sucursal.pFechaInicial = "";
    Sucursal.pFechaFinal = "";
    Sucursal.pTemplate = 'InventarioActualSucursalProducto';
    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        Sucursal.pFechaInicial = $("#txtFechaInicial").val();
        Sucursal.pFechaInicial = ConvertirFecha(Sucursal.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        Sucursal.pFechaFinal = $("#txtFechaFinal").val();
        Sucursal.pFechaFinal = ConvertirFecha(Sucursal.pFechaFinal, 'aaaammdd');
    }
    Sucursal.pIdSucursal = $("#cmbSucursal").val();
    Sucursal.pTipoImpresion = $("#cmbTipoImpresion").val();

    $.ajax({
        type: "POST",
        url: "ReporteInventarioActualSucursalProducto.aspx/ImprimirInventarioActualSucursalProducto",
        data: JSON.stringify(Sucursal),
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

function ObtenerFormaFiltroReporteInventarioActualSucursalProducto() {
    $("#divFiltrosReporteInventarioActualSucursalProducto").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteInventarioActualSucursalProducto.html",
        url: "ReporteInventarioActualSucursalProducto.aspx/ObtenerFormaFiltroReporteInventarioActualSucursalProducto",
        despuesDeCompilar: function(pRespuesta) {
            FiltroInventarioActualSucursalProducto();
            autocompletarClaveProducto();
        }
    });
}

function autocompletarClaveProducto() {
    $('#txtClaveProducto').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pClave = $("#txtClaveProducto").val();
            $.ajax({
                type: 'POST',
                url: 'ReporteInventarioActualSucursalProducto.aspx/BuscarClave',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divGridAlmacenProductoResumen").attr("IdProducto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Producto, value: item.Producto, id: item.IdProducto }
                        //return { label: item.NombreProyecto, value: item.NombreProyecto, id: item.IdProyecto }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            $("#txtClaveProducto").attr('IdProducto', pIdProducto);
            FiltroInventarioActualSucursalProducto();
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function FiltroInventarioActualSucursalProducto() {
    var Sucursal = new Object();
    Sucursal.pFechaInicial = "";
    Sucursal.pFechaFinal = "";
    
    Sucursal.pIdSucursal = $("#cmbSucursal").val();
    Sucursal.pIdProducto = $("#txtClaveProducto").attr('IdProducto');
    ObtieneInventarioActualSucursalProducto(JSON.stringify(Sucursal));
}

function ObtieneInventarioActualSucursalProducto(pRequest) {
    $("#divInventarioActualSucursalProducto").obtenerVista({
    nombreTemplate: "tmplReporteInventarioActualSucursalProducto.html",                   
        parametros: pRequest,
        url: "ReporteInventarioActualSucursalProducto.aspx/ObtieneInventarioActualSucursalProducto",
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