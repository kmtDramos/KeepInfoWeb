//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//
//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos

    $("#grdReportesKeep").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var ReportesKeep = new Object();
        ReportesKeep.pIdReportesKeep = parseInt($(registro).children("td[aria-describedby='grdReportesKeep_IdReportesKeep']").html());
        ObtenerFormaFiltrosReportesKeep(JSON.stringify(ReportesKeep));
    });

    $('#dialogFiltrosReportesKeep').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Generar reporte": function() {
                GenerarReporte();
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogReporteKeep').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1000px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Exportar": function() {
                Exportar();
            },            
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });
    
});

//---------Funciones--------//
function GenerarReporte() {
    var ReporteKeep = new Object();
    ReporteKeep.pIdReportesKeep = $("#divFormaFiltrosReportesKeep").attr("idReportesKeep");
    ObtenerFormaReporteKeep(JSON.stringify(ReporteKeep));
}

function DataModelJQ() { }
DataModelJQ.prototype.name;
DataModelJQ.prototype.index;
DataModelJQ.prototype.width;
DataModelJQ.prototype.sortable;

function generaDataModel(header) {
    arrDataModel = new Array;
    Titulo = "";
    var noRegs;
    var regs = header;
    noRegs = regs.length;
    if (noRegs > 0) {
        for (i = 0; i < noRegs; i++) {
            arrDataModel[i] = new DataModelJQ();

            arrDataModel[i].name = regs[i];
            arrDataModel[i].index = regs[i];
            arrDataModel[i].width = '150';
            arrDataModel[i].sortable = true;
        }
        Titulo = "Datos";
    }
    else {
        arrDataModel[0] = new DataModelJQ();

        arrDataModel[0].name = "";
        arrDataModel[0].index = "";
        arrDataModel[0].width = '450';
        arrDataModel[0].sortable = true;
        Titulo = "No se encontraron registros para esta búsqueda";
        
    }
    llenaTablaReportes(header, arrDataModel);
}

function llenaTablaReportes(header, datamodel) {
    jQuery("#grdReporteKeep").jqGrid({
        datatype: function() {
            GeneraGrid();
        },
        jsonReader: {
            root: 'Elementos',
            page: 'PaginaActual',
            total: 'NoPaginas',
            records: 'NoRegistros',
            repeatitems: true,
            cell: 'Row',
            id: 'RowNumber'
        },
        colNames: header,
        colModel: datamodel,
        loadtext: 'Cargando datos...',
        recordtext: '{0} - {1} de {2} elementos',
        emptyrecords: 'No hay resultados',
        pgtext: 'Pág: {0} de {1}',
        viewrecords: true,
        sortname: "RowNumber",
        sortorder: "DESC",
        height: "auto",
        rowNum: 10,
        rowList: [1, 10, 15, 30],
        pager: "#pagReporteKeep",
        rownumbers: true,
        caption: Titulo,
        height: '100%',
        width: 1200
    });

    $("#grdReporteKeep").jqGrid('navGrid', '#pagReporteKeep', { edit: false, add: false, del: false, search: false });
    $("#grdReporteKeep").jqGrid('hideCol', ["RowNumber"]);
}

function LllenaGridReporte(pRequest) {
    MostrarBloqueo();
    var Names = [];
    $.ajax({
        url: 'ReportesKeep.aspx/ObtenerNombres',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Names = respuesta.Columnas;
                generaDataModel(Names)
            }
            else {

            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function GeneraGrid() {

    var ReporteKeep = new Object();

    if ($('#grdReporteKeep').getGridParam('rowNum') != null) {
        ReporteKeep.pTamanoPaginacion = $('#grdReporteKeep').getGridParam('rowNum');
        ReporteKeep.pPaginaActual = $('#grdReporteKeep').getGridParam('page');
        ReporteKeep.pColumnaOrden = $('#grdReporteKeep').getGridParam('sortname');
        ReporteKeep.pTipoOrden = $('#grdReporteKeep').getGridParam('sortorder');
    }
    else {
        ReporteKeep.pTamanoPaginacion = 10;
        ReporteKeep.pPaginaActual = 1;
        ReporteKeep.pColumnaOrden = "";
        ReporteKeep.pTipoOrden = "asc";
    }


    ReporteKeep.pIdReportesKeep = $("#divFormaReporteKeep").attr("idReportesKeep");
    ReporteKeep.pIdSucursal = $("#divFormaReporteKeep").attr("idSucursal");
    ReporteKeep.pFechaInicial = $("#divFormaReporteKeep").attr("fechaInicial");
    ReporteKeep.pFechaFinal = $("#divFormaReporteKeep").attr("fechaFinal");   

    var pRequest = JSON.stringify(ReporteKeep);    
    $.ajax({
        url: 'ReportesKeep.aspx/ObtenerReporte',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success') {

                $('#grdReporteKeep')[0].addJSONData(JSON.parse(jsondata.responseText).d);
                $("#dialogReporteKeep").dialog("open");
                $("#dialogFiltrosReportesKeep").dialog("close");
            }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }

    });
}

function Exportar() {

    var pIdReportesKeep = $("#divFormaReporteKeep").attr("idReportesKeep");
    var pIdSucursal = $("#divFormaReporteKeep").attr("idSucursal");
    var pFechaInicial = $("#divFormaReporteKeep").attr("fechaInicial");
    var pFechaFinal = $("#divFormaReporteKeep").attr("fechaFinal");

    $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
        IsExportExcel: true,
        pIdReportesKeep: pIdReportesKeep,
        pIdSucursal: pIdSucursal,
        pFechaInicial: pFechaInicial,
        pFechaFinal: pFechaFinal     

    }, downloadType: 'Normal'
    });
}
//--------------------------//

//--------Validaciones-------//
//--------------------------//

//-----------AJAX------------//
//----Funciones de Accion----//

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaFiltrosReportesKeep(pRequest) {
    $("#dialogFiltrosReportesKeep").obtenerVista({
        nombreTemplate: "tmplFiltrosReportesKeep.html",
        parametros: pRequest,
        url: "ReportesKeep.aspx/ObtenerFormaFiltrosReportesKeep",
        despuesDeCompilar: function() {

            $("#txtFechaInicial").datepicker();
            $("#txtFechaFinal").datepicker();
            //$("cmbSucursal").multiselect();
            $("#dialogFiltrosReportesKeep").dialog("open");
        }
    });
}


function ObtenerFormaReporteKeep(pRequest) {
    $("#dialogReporteKeep").obtenerVista({
        nombreTemplate: "tmplReporteKeep.html",
        parametros: pRequest,
        url: "ReportesKeep.aspx/ObtenerFormaReporteKeep",
        despuesDeCompilar: function() {
            var ReporteKeep = new Object();
            ReporteKeep.pIdReportesKeep = $("#divFormaFiltrosReportesKeep").attr("idReportesKeep");

            ReporteKeep.pIdSucursal = $("#cmbSucursal").val();

            if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {
                ReporteKeep.pFechaInicial = $("#txtFechaInicial").val();
                ReporteKeep.pFechaInicial = ConvertirFecha(ReporteKeep.pFechaInicial, 'aaaammdd');
            }
            if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                ReporteKeep.pFechaFinal = $("#txtFechaFinal").val();
                ReporteKeep.pFechaFinal = ConvertirFecha(ReporteKeep.pFechaFinal, 'aaaammdd');
            }

            if ($('#grdReporteKeep').getGridParam('rowNum') != null) {
                ReporteKeep.pTamanoPaginacion = $('#grdReporteKeep').getGridParam('rowNum');
                ReporteKeep.pPaginaActual = $('#grdReporteKeep').getGridParam('page');
                ReporteKeep.pColumnaOrden = $('#grdReporteKeep').getGridParam('sortname');
                ReporteKeep.pTipoOrden = $('#grdReporteKeep').getGridParam('sortorder');
            }
            else {
                ReporteKeep.pTamanoPaginacion = 10;
                ReporteKeep.pPaginaActual = 1;
                ReporteKeep.pColumnaOrden = "";
                ReporteKeep.pTipoOrden = "asc";
            }

            $("#divFormaReporteKeep").attr("idReportesKeep", ReporteKeep.pIdReportesKeep);
            $("#divFormaReporteKeep").attr("idSucursal", ReporteKeep.pIdSucursal);
            $("#divFormaReporteKeep").attr("fechaInicial", ReporteKeep.pFechaInicial);
            $("#divFormaReporteKeep").attr("fechaFinal", ReporteKeep.pFechaFinal);
            $("#spanNombreReporte").text($("#divFormaFiltrosReportesKeep").attr("nombreReporte"));

            var oRequest = new Object();
            oRequest.ReporteKeep = ReporteKeep;
            LllenaGridReporte(JSON.stringify(oRequest));

            

        }
    });
}
