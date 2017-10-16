//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosReporteEstadoCuentaProveedores();

    $("#divContenido").on("click", "#btnImprimirEstadoCuenta, #btnImprimirEstadoCuentaArriba", function() {
        var IdProveedor = 0;
        if ($("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor") != null && $("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor") != "") {
            IdProveedor = $("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor");
        }
        if (IdProveedor != 0) {
            ImprimirEstadoCuentaProveedor(IdProveedor);
        }
        else {
            MostrarMensajeError("Seleccione un proveedor");
        }
    });

    $("#divContenido").on("click", "#btnExportar, #btnExportarArriba", function() {
        tableToExcel('tblReporte', 'ReporteEstadoCuenta');
    });

    $("#divFiltrosReporteEstadoCuentaProveedores").on("change", "#cmbSucursal", function() {
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

    $("#divFiltrosReporteEstadoCrediticioProveedores").on("change", "#cmbSucursal", function() {
        $("#ReporteRelacionSaldos").empty();
        $("#btnExportarSaldos").hide();
        $("#ReporteRelacionSaldosDolares").empty();
        $("#btnExportarSaldosD").hide();
        $("#RelacionSaldosPesosGeneral").empty();
        $("#btnExportarSaldosGeneral").hide();
        $("#RelacionSaldosPesosGeneralTCD").empty();
        $("#btnExportarSaldosGeneralTCD").hide();

    });

});

function ImprimirEstadoCuentaProveedor(pIdProveedor) {
    MostrarBloqueo();

   
    var pRequest = new Object();
    pRequest.pTemplate = 'EstadoCuentaProveedor';
    pRequest.pIdProveedor = pIdProveedor;

    pRequest.pFechaInicial = "";
    pRequest.pFechaFinal = "";

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        pRequest.pFechaInicial = $("#txtFechaInicial").val();
        pRequest.pFechaInicial = ConvertirFecha(pRequest.pFechaInicial, 'aaaammdd');   }    
    
        pRequest.pIdSucursal = $("#cmbSucursal").val();

        pRequest.pTipoImpresion = $("#cmbTipoImpresion").val();

    $.ajax({
        type: "POST",
        url: "ReporteEstadoCuentaProveedores.aspx/ImprimirEstadoCuentaProveedor",
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

//---------Funciones--------//


//--------------------------//

//--------Validaciones-------//
//--------------------------//

//-----------AJAX------------//
//----Funciones de Accion----//

//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function ObtenerFormaFiltrosReporteEstadoCuentaProveedores() {
    $("#divFiltrosReporteEstadoCuentaProveedores").obtenerVista({
        nombreTemplate: "tmplFiltrosReporteEstadoCuentaProveedores.html",
        url: "ReporteEstadoCuentaProveedores.aspx/ObtenerFormaFiltroReporteEstadoCuentaProveedores",
        despuesDeCompilar: function(pRespuesta) {
            autocompletarProveedor();
            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroEstadoCuentaProveedores();
                    }
                });
            }
            
        }
    });
}
function FiltroEstadoCuentaProveedores() {

    var pIdProveedor = 0;
    if ($("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor") != null && $("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor") != "") {
        pIdProveedor = $("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor");
    }

    var Proveedor = new Object();
    Proveedor.pIdProveedor = pIdProveedor;
    Proveedor.pFechaInicial = "";
    Proveedor.pFechaFinal = "";

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
        Proveedor.pFechaInicial = $("#txtFechaInicial").val();
        Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
    }
    Proveedor.pIdSucursal = $("#cmbSucursal").val();
    ObtieneEstadoCuentaProveedor(JSON.stringify(Proveedor));

}

function ObtieneEstadoCuentaProveedor(pRequest) {
    $("#divEstadoCuentaProveedor").obtenerVista({
    nombreTemplate: "tmplReporteEstadoCuentaProveedor.html",                   
        parametros: pRequest,
        url: "ReporteEstadoCuentaProveedores.aspx/ObtieneEstadoCuentaProveedor",
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
                url: 'ReporteEstadoCuentaProveedores.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor", "0");
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
            $("#divFiltrosReporteEstadoCuentaProveedores").attr("idProveedor", pIdProveedor);

            var Proveedor = new Object();
            Proveedor.pIdProveedor = pIdProveedor;
            Proveedor.pFechaFinal = "";
            if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
                Proveedor.pFechaInicial = $("#txtFechaInicial").val();
                Proveedor.pFechaInicial = ConvertirFecha(Proveedor.pFechaInicial, 'aaaammdd');
            }

            Proveedor.pIdSucursal = $("#cmbSucursal").val(); 
            ObtieneEstadoCuentaProveedor(JSON.stringify(Proveedor));

        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
    
}