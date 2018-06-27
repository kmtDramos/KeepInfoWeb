//----------DHTMLX----------//
//--------------------------//
var MetaMensual = 0;
var ResultadoMensual = 0;
//----------JQuery----------//
//--------------------------//
$(function() {
    
    setInterval(MantenerSesion, 150000); //2.5 minutos

    Inicializar_grdOportunidad();
    ObtenerDatosNivelInteres();

    $(window).unload(function() {
        ActualizarPanelControles("Oportunidad");
    });

    RefrescarBotonDesactivar();

    $("#pieChartMesActual").click(function() {
        ObtenerGraficaMesActual();
    });

    $("#pieChartMesAnterior").click(function() {
        ObtenerGraficaMesPasado();
    });

    $("#btnReporteSeisMeses").click(function() {
        ResutadoUltimosMeses();
    });

    //########################################\\
    //#   Muestra explicacion de cantidades  #\\
    //########################################\\

    $("#Dudas").click(function() {
        ObtenerConceptosdeTabla();
    });
    
    $("#btnObtenerFormaAgregarOportunidad").click(function() {
        ObtenerFormaAgregarOportunidad();
    });

    $("#btnObtenerFormaReporteComisiones").click(function() {
        ObtenerFormaFiltrosReporteComisiones();
    });

    $("#dialogGraficaMesPasado").dialog({
        autoOpen: false,
        height: 'auto',
        width: '900px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divResultadoVentasDivision").remove();
        },
        buttons: {
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogGraficaMesActual").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            var titulo = $(this).dialog("option", "title") + " - " + $("#lblResultado").text();
            $(this).dialog("option", "title", titulo);
        },
        close: function() {
            var titulo = $(this).dialog("option", "title").replace(" - " + $("#lblResultado").text(),"");
            $(this).dialog("option", "title", titulo);
            $("#divResultadoVentasDivision").remove();
        },
        buttons: {
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogGraficaResultadosUltimosMeses").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            // $("#divResultadoVentasDivision").remove();
        },
        buttons: {
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogAgregarOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarOportunidad").remove();
        },
        buttons: {
            "Agregar": function() {
                AgregarOportunidad();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    // ############## Diseño dialog Filtro Reporte Comision #################
    $("#dialogFiltroReporteComision").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaReporteComisiones", "#dialogFiltroReporteComision").remove();
        },
        open: function() {
            $("#txtFechaInicialComisiones").datepicker();
            $("#txtFechaFinalComisiones").datepicker();
        },
        buttons: {
            "Generar reporte": function() {
                ObtenerFormaReporteComisiones();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    // ############## Diseño dialog Reporte Comision #################
    $("#dialogReporteComision").dialog({
        autoOpen: false,
        height: 'auto',
        width: '1000px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaReporteComisiones", "#dialogReporteComision").remove();
        },
        buttons: {
            "Exportar": function() {
                var Reporte = new Object();
                Reporte.pIdSucursal = $("#divReporteComision").attr("IdSucusal");
                Reporte.pFechaInicio = $("#divReporteComision").attr("FechaInicial");
                Reporte.pFechaFinal = $("#divReporteComision").attr("FechaFinal");
                Reporte.pDivision = $("#divReporteComision").attr("IdDivision");
                Reporte.pNotaCredito = $("#divReporteComision").attr("NotaCredito");
                Reporte.pAgente = $("#divReporteComision").attr("IdAgente");
                ExportarReporteComisiones(Reporte);
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#dialogComentariosOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: '700px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaComentariosOportunidad").remove();
        }
    });

    $("#dialogArchivoOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: '350px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaArchivoOportunidad").remove();
        }
    });

    $("#dialogConsultarOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarTiempoEntrega").remove();
        }
    });

    $("#dialogEditarOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarOportunidad").remove();
        }
    });

    $("#dialogAgregarMotivoCancelacionOportunidad").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            RefrescarBotonDesactivar();
        }
    });

    $("#divGridOportunidad").on("click", ".imgFormaConsultarOportunidad", function() {
        var registro = $(this).parents("tr");
        var Oportunidad = new Object();
        var idOportunidad = $(registro).children("td[aria-describedby='grdOportunidad_IdOportunidad']").html();
        Oportunidad.pIdOportunidad = idOportunidad;
        //ObtenerFormaConsultarOportunidad(JSON.stringify(Oportunidad));
        ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
    });

    $("#divGridOportunidad").on("click", ".imgFormaConsultarOportunidad", function() {
        var registro = $(this).parents("tr");
        var Oportunidad = new Object();
        var idOportunidad = $(registro).children("td[aria-describedby='grdOportunidad_IdOportunidad']").html();
        Oportunidad.pIdOportunidad = idOportunidad;
        //ObtenerFormaConsultarOportunidad(JSON.stringify(Oportunidad));
        ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
    });

    $("#divGridOportunidad").on("click", ".imgFormaComentariosOportunidad", function() {
        var registro = $(this).parents("tr");
        var Oportunidad = new Object();
        var idOportunidad = $(registro).children("td[aria-describedby='grdOportunidad_IdOportunidad']").html();
        Oportunidad.pIdOportunidad = idOportunidad;
        ObtenerFormaComentariosOportunidad(JSON.stringify(Oportunidad));
    });

    $("#divGridOportunidad").on("click", ".imgFormaArchivoOportunidad", function() {
        var registro = $(this).parents("tr");
        var Oportunidad = new Object();
        var idOportunidad = $(registro).children("td[aria-describedby='grdOportunidad_IdOportunidad']").html();
        Oportunidad.pIdOportunidad = idOportunidad;
        ObtenerFormaArchivoOportunidad(JSON.stringify(Oportunidad));
    });

    $("#lblOportunidadesClientes").click(function() {

    });

    //##############################################################################
    //////funcion del grid//////
    $("#gbox_grdOportunidad").livequery(function() {// MODIFICAR
        $("#grdOportunidad").jqGrid('navButtonAdd', '#pagOportunidad', {// MODIFICAR
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                pIdOportunidad = "";
                pOportunidad = "";
                pAgente = "";
                pCliente = "";
                pNivelInteres = -1;
                pIdSucursal = -1;
                pMonto = 0;
                pClasificacion = -1;
                pIdDivision = -1;
                pCerrado = 1;
                pAI = 1;

                if ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "")
                { pIdOportunidad = $("#gs_IdOportunidad").val(); }

                if ($('#gs_Oportunidad').val() != null)
                { pOportunidad = $("#gs_Oportunidad").val(); }

                if ($('#gs_Agente').val() != null)
                { pAgente = $("#gs_Agente").val(); }

                if ($('#gs_Cliente').val() != null)
                { pCliente = $("#gs_Cliente").val(); }

                if ($('#gs_NivelInteres').val() != null)
                { pNivelInteres = $("#gs_NivelInteres").val(); }

                if ($('#gs_Sucursal').val() != null)
                { pIdSucursal = $("#gs_Sucursal").val(); }

                if ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "")
                { pMonto = $("#gs_Monto").val(); }

                if ($('#gs_Clasificacion').val() != null)
                { pClasificacion = $("#gs_Clasificacion").val(); }

                if ($('#gs_Division').val() != null)
                { pIdDivision = $("#gs_Division").val(); }

                if ($('#gs_Cerrado').val() != null)
                { pCerrado = $("#gs_Cerrado").val(); }

                if ($('#gs_AI').val() != null)
                { pAI = $("#gs_AI").val(); }

                $.UnifiedExportFile({
                    action: '../ExportacionesExcel/ExportarExcelOportunidades.aspx',
                    data: {
                        IsExportExcel: true,
                        pIdOportunidad: pIdOportunidad,
                        pOportunidad: pOportunidad,
                        pAgente: pAgente,
                        pCliente: pCliente,
                        pNivelInteres: pNivelInteres,
                        pIdSucursal: pIdSucursal,
                        pMonto: pMonto,
                        pClasificacion: pClasificacion,
                        pIdDivision: pIdDivision,
                        pCerrado: pCerrado,
                        pAI: pAI
                    },
                    downloadType: 'Normal'
                });

            }
        });
    });

    InicializarDialogGraficasOportunidades();
    //#################################################################################

    $("#grdOportunidad").on("click", "td[aria-describedby=grdOportunidad_Actividades]", function() {
        var registro = $(this).parents("tr");
        var pIdOportunidad = $(registro).children("td[aria-describedby='grdOportunidad_IdOportunidad']").html();
        $("#dialog_grdActividadesClienteOportunidad").attr("idOportunidad", pIdOportunidad);
        $("#dialog_grdActividadesClienteOportunidad").dialog("option", "title", "Actividades")
        $("#dialog_grdActividadesClienteOportunidad").dialog("open");
    });
    
});

//-----------AJAX-----------//
//-Funciones Obtener Formas-//

function CalculoUtilidad() {
	var Monto = parseFloat($("#txtMontoOportunidad").val().replace("$", "").replace(/,/g, ""));
	var Margen = parseInt($("#txtMargen").val());
	var Costo = parseFloat($("#txtCosto").val().replace("$", "").replace(",", ""));

	console.log($("#txtMontoOportunidad").val().replace("$", "").replace(",", ""));
	console.log(Monto);
	console.log(Margen);
	console.log(Costo);

	Monto = (!isNaN(Monto) && isFinite(Monto)) ? Monto : 0;
	Margen = (!isNaN(Margen) && isFinite(Margen)) ? Margen : 0;
	Costo = (!isNaN(Costo) && isFinite(Costo)) ? Costo : 0;

	Monto = (Monto == 0 && Margen > 0 && Costo > 0) ? Costo / ((100 - Margen) / 100) : Monto;
	Margen = (Margen == 0 && Costo > 0 && Monto > 0) ? Math.round((Monto - Costo) * 100 / Monto) : Margen;
	Costo = (Costo == 0 && Monto > 0 && Margen > 0) ? Monto * ((100 - Margen) / 100) : Costo;

	$("#txtMontoOportunidad").val(formato.moneda(Monto,'$')); 
	$("#txtMargen").val(Margen);
	$("#txtCosto").val(formato.moneda(Costo,'$'));
}

function AutocompletarClienteOportunidad() {
    $('#txtClienteOportunidad').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pCliente = $("#txtClienteOportunidad").val();
            $.ajax({
                type: 'POST',
                url: 'Oportunidad.aspx/BuscarCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Cliente, value: item.Cliente, id: item.IdCliente, Saldo: item.Saldo }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
        	var pIdCliente = ui.item.id;
        	var Saldo = ui.item.Saldo;
            $("#divFormaAgregarOportunidad, #divFormaEditarOportunidad").attr("idCliente", pIdCliente);
            $("#lvlSaldo").text(formato.moneda(Saldo, '$'));
        },
        change: function(event, ui) {  },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    }).change(function () { $("#lvlSaldo").text(''); });
}

//################## Autocompletar Agente dialog #####################
function AutocompletarAgenteOportunidad() {
    $("#txtUsuario").autocomplete({
        source: function(request, response) {
            var Usuario = new Object();
            Usuario.pUsuario = request.term;
            var pRequest = JSON.stringify(Usuario);
            $.ajax({
                type: "POST",
                url: "Oportunidad.aspx/ObtenerUsuariosAsignar",
                data: pRequest,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function(oRespuesta) {
                    var json = jQuery.parseJSON(oRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Usuario, value: item.Usuario, id: item.IdUsuario }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var IdUsuario = ui.item.id;
            $("#divFormaReporteComisiones").attr("idUsuario", IdUsuario);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

// Agregar
function ObtenerFormaAgregarOportunidad() {
    $("#dialogAgregarOportunidad").obtenerVista({
        nombreTemplate: "tmplAgregarOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaAgregarOportunidad",
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarClienteOportunidad();
            $("#dialogAgregarOportunidad").dialog("open");
            $("#txtProveedores").on("keypress keyup keydown", function () {
            	var lb = $(this).val().split("\n").length;
            	var l = $(this).val().length
            	var r = Math.floor(l / 43)+lb;
            	$(this).attr("rows", r);
            });
            $("#txtFechaCierre").datepicker({
            	dateFormat: "dd/mm/yy",
				minDate: new Date()
            });
        }
    });
}

// Consultar reporte comisiones
function ObtenerFormaFiltrosReporteComisiones() {
    $("#dialogFiltroReporteComision").obtenerVista({
        nombreTemplate: "tmplFormaReporteComisiones.html",
        url: "Oportunidad.aspx/ObtenerFormaReporteComisiones",
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarAgenteOportunidad();
            $("#dialogFiltroReporteComision").dialog("open");
        }
    });
}

function ObtenerFormaReporteComisiones() {
    var Reporte = new Object();
    Reporte.pTamanoPaginacion = ($('#grdReporteComision').getGridParam('rowNum') != null)? $('#grdReporteComision').getGridParam('rowNum') : 10;
    Reporte.pPaginaActual = ($('#grdReporteComision').getGridParam('page') != null) ? $('#grdReporteComision').getGridParam('page') : 1 ;
    Reporte.pColumnaOrden = ($('#grdReporteComision').getGridParam('sortname') != null) ? $('#grdReporteComision').getGridParam('sortname') : "IdFacturaEncabezado";
    Reporte.pTipoOrden = ($('#grdReporteComision').getGridParam('sortorder') != null) ? $('#grdReporteComision').getGridParam('sortorder') : "ASC";
    Reporte.pIdSucursal = ($("#cmbSucursal").val() != -1 && $("#cmbSucursal").val() != null)? $("#cmbSucursal").val() : -1;
    Reporte.pFechaInicio = ($("#txtFechaInicialComisiones").val() != -1 && $("#txtFechaInicialComisiones").val() != null)? $("#txtFechaInicialComisiones").val() : -1;
    Reporte.pFechaFinal = ($("#txtFechaFinalComisiones").val() != -1 && $("#txtFechaFinalComisiones").val() != null)? $("#txtFechaFinalComisiones").val() : -1;
    Reporte.pDivision = ($("#cmbDivision").val() != -1)? $("#cmbDivision").val() : -1;
    Reporte.pNotaCredito = ($("#cmbPagoNotaCredito").val() != -1)? $("#cmbPagoNotaCredito").val() : -1;
    var IdUsuario = $("#divFormaReporteComisiones").attr("idUsuario");
    Reporte.pAgente = (IdUsuario != "")? IdUsuario : -1;
    
    var validacion = ValidarReporte(Reporte);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    
    $("#dialogReporteComision").obtenerVista({
        nombreTemplate: "tmplReporteComisiones.html",
        url: "Oportunidad.aspx/ObtenerFormaGridReporteComisiones",
        parametros: JSON.stringify(Reporte),
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogReporteComision").dialog("open");
            Inicializar_grdReporteComision();
            $("#dialogReporteComision").dialog("option", "position", "center");
        }
    });
}

function FiltroReporteComision() {
    var Reporte = new Object();
    Reporte.pTamanoPaginacion = ($('#grdReporteComision').getGridParam('rowNum') != null)? $('#grdReporteComision').getGridParam('rowNum') : 10;
    Reporte.pPaginaActual = ($('#grdReporteComision').getGridParam('page') != null) ? $('#grdReporteComision').getGridParam('page') : 1 ;
    Reporte.pColumnaOrden = ($('#grdReporteComision').getGridParam('sortname') != null) ? $('#grdReporteComision').getGridParam('sortname') : "IdFacturaEncabezado";
    Reporte.pTipoOrden = ($('#grdReporteComision').getGridParam('sortorder') != null) ? $('#grdReporteComision').getGridParam('sortorder') : "ASC";
    Reporte.pIdSucursal = ($("#cmbSucursal").val() != -1 && $("#cmbSucursal").val() != null)? parseInt($("#cmbSucursal").val()) : -1;
    Reporte.pFechaInicio = ($("#txtFechaInicialComisiones").val() != -1 && $("#txtFechaInicialComisiones").val() != null)? $("#txtFechaInicialComisiones").val() : -1;
    Reporte.pFechaFinal = ($("#txtFechaFinalComisiones").val() != -1 && $("#txtFechaFinalComisiones").val() != null)? $("#txtFechaFinalComisiones").val() : -1;
    Reporte.pDivision = ($("#cmbDivision").val() != -1)? parseInt($("#cmbDivision").val()) : -1;
    Reporte.pNotaCredito = ($("#cmbPagoNotaCredito").val() != -1)? parseInt($("#cmbPagoNotaCredito").val()) : -1;
    var IdUsuario = $("#divFormaReporteComisiones").attr("idUsuario");
    Reporte.pAgente = (IdUsuario != "")? parseInt(IdUsuario) : -1;
    
    var validacion = ValidarReporte(Reporte);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    var pRequest = JSON.stringify(Reporte);
    $.ajax({
        url: 'Oportunidad.aspx/ObtenerReporteComisiones',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdReporteComision')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { /*alert(JSON.parse(jsondata.responseText).Message);*/ }
             //$("#dialogFiltroReporteComision").dialog("close");
        }
    });
}

//################ Exportar Reporte Comisiones a Excel
function ExportarReporteComisiones(pRequest) {
    try {
        MostrarBloqueo();
        pIdSucursal = $("#divReporteComision").attr("IdSucusal");
        pFechaInicio = $("#divReporteComision").attr("FechaInicial");
        pFechaFinal = $("#divReporteComision").attr("FechaFinal");
        pDivision = $("#divReporteComision").attr("IdDivision");
        pNotaCredito = $("#divReporteComision").attr("NotaCredito");
        pAgente = $("#divReporteComision").attr("IdAgente");
        
        $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
            IsExportExcel: true,
            pIdSucursal: pIdSucursal,
            pFechaInicio: pFechaInicio,
            pFechaFinal: pFechaFinal,
            pDivision: pDivision,
            pNotaCredito: pNotaCredito,
            pAgente: pAgente
        }, downloadType: 'Normal'
        });
        OcultarBloqueo();
    } catch (e) {
        /*alert(e);*/
    }
}

function AgregarOportunidad() {
    var pOportunidad = new Object();
    pOportunidad.pOportunidad = $("#txtOportunidad").val();
    pOportunidad.pIdCliente = $("#divFormaAgregarOportunidad").attr("idCliente");
    pOportunidad.pMonto = $("#txtMontoOportunidad").val().replace("$", "").replace(",", "");
    pOportunidad.pFechaCierre = $("#txtFechaCierre").val();
    pOportunidad.IdNivelInteresOportunidad = parseInt($("#cmbNivelInteresOportunidad").val());
    pOportunidad.pIdDivision = parseInt($("#cmbDivisionOportunidad").val());
    pOportunidad.pEsProyecto = parseInt($("#cmbEsProyecto").val());
    pOportunidad.pUrgente = parseInt($("#cmbUrgente").val());
    pOportunidad.pIdCampana = parseInt($("#cmbCampana").val());
    pOportunidad.pProveedores = $("#txtProveedores").val();
    pOportunidad.pUtilidad = parseInt($("#txtMargen").val());
    pOportunidad.pCosto = parseFloat($("#txtCosto").val().replace("$", "").replace(",", ""));
    var validacion = ValidarOportunidad(pOportunidad);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    var oRequest = new Object();
    oRequest.pOportunidad = pOportunidad;
    SetAgregarOportunidad(JSON.stringify(oRequest));
}

function SetAgregarOportunidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/AgregarOportunidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOportunidad").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarOportunidad").dialog("close");
        }
    });
}

// Editar
function ObtenerFormaEditarOportunidad(request) {
    $("#dialogEditarOportunidad").obtenerVista({
        nombreTemplate: "tmplEditarOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaEditarOportunidad",
        parametros: request,
        despuesDeCompilar: function(pRespuesta) {
            AutocompletarUsuario();
            AutocompletarClienteOportunidad();
            $("#tabOportunidad").tabs();
            $("#dialogEditarOportunidad").dialog("option", "buttons", {
                //"Editar": function () {
                //	EditarOportunidad();
                //},
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            });
            $("#dialogEditarOportunidad").dialog("open");
            $("#dialogEditarOportunidad").dialog("option", "title", $("#divFormaEditarOportunidad").attr("title"));
            $("#txtProveedores").on("keypress keyup keydown", function () {
            	var lb = $(this).val().split("\n").length;
            	var ll = $(this).val().split("\n")[$(this).val().split("\n").length-1];
            	var r = lb + Math.floor(ll.length / 43);
            	$(this).attr("rows", r);
            }).keypress();
        	$("#txtFechaCierre").datepicker({
        		dateFormat: "dd/mm/yy",
        		minDate: new Date()
        	});
            $("#tblContactoCliente", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" }
            });
            $("#tablaOrdenCompra", "#dialogEditarOportunidad").DataTable({
            	"oLanguage": { "sUrl": "../JS/Spanish.json" },
            	"scrollCollapse": false
            });
            $("#tblProyectos", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" },
                "scrollCollapse": false
            });
            $("#tblFacturas", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" },
                "scrollCollapse": false
            });
            $("#tblCompras", "#dialogEditarOportunidad").DataTable({
                "oLanguage": { "sUrl": "../JS/Spanish.json" },
                "scrollCollapse": false
            });
        	$("#cmbDivisionOportunidad").change(function () {
        		var Division = new Object();
        		Division.IdDivision = parseInt($(this).val());
        		var Request = JSON.stringify(Division);
        		$("#iDivisionDescripcion").attr("title", '');
        		ObtenerDescripcionDivision(Request);
            }).change();
            $('#tabOportunidad').bind('tabsshow', function (event, ui) {
                switch (ui.index) {
                    case 1:
                        $("#commit").scrollTop($("#commit")[0].scrollHeight);
                        break;
                }
            });
        }
    });
}

function ObtenerDescripcionDivision(Division) {
	$.ajax({
		url: "Oportunidad.aspx/ObtenerDescripcionDivision",
		type: "post",
		data: Division,
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				$("#iDivisionDescripcion").attr("title", json.Modelo.Descripcion);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

function AutocompletarUsuario() {
    $("#txtUsuarioOportunidad").autocomplete({
        source: function(request, response) {
            var Usuario = new Object();
            Usuario.pUsuario = request.term;
            var pRequest = JSON.stringify(Usuario);
            $.ajax({
                type: "POST",
                url: "Oportunidad.aspx/ObtenerUsuariosAsignar",
                data: pRequest,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function(oRespuesta) {
                    var json = jQuery.parseJSON(oRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Usuario, value: item.Usuario, id: item.IdUsuario }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var IdUsuario = ui.item.id;
            $("#divFormaEditarOportunidad").attr("idUsuario", IdUsuario);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function EditarOportunidad() {
    var pOportunidad = new Object();
    pOportunidad.pIdOportunidad = $("#divFormaEditarOportunidad").attr("idOportunidad");
    pOportunidad.pOportunidad = $("#txtOportunidad").val();
    pOportunidad.pIdCliente = $("#divFormaEditarOportunidad").attr("idCliente");
    pOportunidad.pIdUsuario = $("#divFormaEditarOportunidad").attr("idUsuario");
    pOportunidad.pMonto = $("#txtMontoOportunidad").val().replace("$", "").replace(",", "");
    pOportunidad.pFechaCierre = $("#txtFechaCierre").val();
    pOportunidad.IdNivelInteresOportunidad = $("#cmbNivelInteresOportunidad").val();
    pOportunidad.pClasificacion = parseInt($("#cmbClasificacionOportunidad").val());
    pOportunidad.pDivision = parseInt($("#cmbDivisionOportunidad").val());
    pOportunidad.pCampana = parseInt($("#cmbCampana").val());
    pOportunidad.pCerrada = parseInt($("#cmbCerradaOportunidad").val());
    pOportunidad.pEsProyecto = parseInt($("#cmbEsProyectoOportunidad").val());
    pOportunidad.pUrgente = parseInt($("#cmbUrgenteOportunidad").val());
    pOportunidad.pProveedores = $("#txtProveedores").val();
    pOportunidad.pUtilidad = parseInt($("#txtMargen").val());
    pOportunidad.pCosto = parseFloat($("#txtCosto").val().replace("$", "").replace(",", ""));
    var validacion = ValidarOportunidad(pOportunidad);
    if (validacion != "") {
        MostrarMensajeError(validacion);
        return false;
    }
    var oRequest = new Object();
    oRequest.pOportunidad = pOportunidad;
    SetEditarOportunidad(JSON.stringify(oRequest));
    //$("#dialogEditarOportunidad").dialog("close");
}

function SetEditarOportunidad(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/EditarOportunidad",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdOportunidad").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            MostrarMensajeError("Se ha guardado con éxito.");
            //$("#dialogEditarOportunidad").dialog("close"); 
        }
    });
}

// Add and Read Commit
function GuardarComentario() {

    if ($("#addComentario").val() == "") {
        MostrarMensajeError("Favor de poner comentario previamente.");
    } else {
        MostrarBloqueo();
        var pComentario = new Object();
        pComentario.pComentario = $("#addComentario").val();
        pComentario.pIdOportunidad = parseInt($("#divFormaEditarOportunidad").attr("idOportunidad"));
        var pRequest = JSON.stringify(pComentario);

        $.ajax({
            type: "POST",
            url: "Oportunidad.aspx/AgregarComentarioOportunidad",
            data: pRequest,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (pRespuesta) {
                var json = JSON.parse(pRespuesta.d);
                if (json.Error == 0) {
                    $("#commit").empty();
                    $("#addComentario").val("");
                    var str = '';
                    $.each(json, function (k, v) {
                        if (k == 'Modelo')
                            $.each(v, function (a, b) {
                                $.each(b, function (x, y) {
                                    str += '<div class="container">'
                                        + '   <h3>' + b[x].Usuario + ' - ' + b[x].Area + '</h3>'
                                        + '   <p>' + b[x].Comentario + '</p>'
                                        + '   <span class="time-right">' + b[x].Fecha + '</span>'
                                        + ' </div>';
                                });
                            });
                    });
                    $("#commit").append(str);
                    $("#commit").scrollTop($("#commit")[0].scrollHeight);
                }
                else {
                    MostrarMensajeError(json.Descripcion);
                }
            },
            complete: function () {
                OcultarBloqueo();
            }
        });
    }
}

function ValidarOportunidad(pOportunidad) {

    var error = "";
    if (pOportunidad.pOportunidad == "") {
        error += '<span>*</span> El campo de oportunidad esta vacio, favor de completarlo.<br/>';
    }
    if (pOportunidad.pIdCliente == "" || pOportunidad.pIdCliente == 0 || pOportunidad.pIdCliente == null || pOportunidad.pIdCliente == undefined) {
        error += '<span>*</span> Favor de selecionar el cliente de la oportunidad.<br/>';
    }
    if (pOportunidad.pMonto == "") {
        error += '<span>*</span> El campo del monto de la oprotunidad esta vacio, favor de completarlo.<br/>';
    }
    if (isNaN(pOportunidad.pMonto)) {
        error += '<span>*</span> El monto debe ser numerico y no puede llevar signos ni comas.<br/>';
    }
    if (pOportunidad.pIdNivelInteresOportunidad == "") {
        error += '<span>*</span> Favor de selecionar el nivel de interés de la oportunidad.<br/>';
    }
    if (pOportunidad.pUrgente == 1 && pOportunidad.IdNivelInteresOportunidad != 1) {
        error += '<span>*</span> Únicamente las oportunidades con nivel de interes alto pueden ser urgentes.<br/>';
    }
    if (error != "") {
        error = '<p>Favor de completar los siguientes requisitos:</p>' + error;
    }
    return error;
}

function ValidarReporte(pReporte) {
    var error = "";
    if (pReporte.pIdSucursal == -1) {
        error += '<span>*</span> Favor de selecionar la sucursal.<br/>';
    }
    if (pReporte.pFechaInicial == "") {
        error += '<span>*</span> Favor de selecionar la fecha inicial.<br/>';
    }
    if (pReporte.pFechaFinal == "") {
        error += '<span>*</span> Favor de selecionar la fecha final.<br/>';
    }
    if (error != "") {
        error = '<p>Favor de completar los siguientes requisitos:</p>' + error;
    }
    return error;
}

function SetCambiarEstatus(pIdOportunidad, pMotivoCancelacion, pBaja) {
    var pRequest = "{'pIdOportunidad':" + pIdOportunidad + ", 'pMotivoCancelacion':'"+ pMotivoCancelacion +"', 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdOportunidad").trigger("reloadGrid");
        },
        complete: function() {
            RefrescarBotonDesactivar();
        }
    });
}

// Consultar
function ObtenerFormaConsultarOportunidad(pOportunidad) {
// <<<<<<< .mine
    $("#dialogConsultarOportunidad").obtenerVista({
        nombreTemplate: "tmplConsultarOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaOportunidad",
        parametros: pOportunidad,
        despuesDeCompilar: function(pRespuesta) {
        	if (pRespuesta.modelo.Permisos.puedeEditarOportunidad == 1) {
        		$("#tabsControlesOportunidad").tabs();
        		$("#dialogConsultarOportunidad").dialog("option", "buttons", {
        			"Archivo": function () {
        				var Oportunidad = new Object();
        				Oportunidad.pIdOportunidad = parseInt($("#divFormaConsultarOportunidad").attr("idOportunidad"));
        				ObtenerFormaArchivoOportunidad(JSON.stringify(Oportunidad))
        			},
                    "Editar": function() {
                        $(this).dialog("close");
                        var Oportunidad = new Object();
                        Oportunidad.pIdOportunidad = parseInt($("#divFormaConsultarOportunidad").attr("idOportunidad"));
                        ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
                    }
                });
                $("#dialogConsultarOportunidad").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarOportunidad").dialog("option", "buttons", {});
                $("#dialogConsultarOportunidad").dialog("option", "height", "100");
            }
            $("#dialogConsultarOportunidad").dialog("open");
        }
    });
	//initObjetos(formaActividad);
	FiltroActividadesClienteOportunidad();
}

function VerVentanaComentariosOportunidad(pIdOportunidad) {
    var request = new Object();
    request.pIdOportunidad = pIdOportunidad;
    ObtenerFormaComentariosOportunidad(JSON.stringify(request));
}

function ObtenerFormaComentariosOportunidad(request) {
    $("#dialogComentariosOportunidad").obtenerVista({
        nombreTemplate: "tmplFormaComentariosOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaComentariosOportunidad",
        parametros: request,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogComentariosOportunidad").dialog("option", "buttons", {
                "Agregar": function() {
                    var Nota = new Object();
                    Nota.pIdOportunidad = $("#divFormaComentariosOportunidad").attr("idOportunidad");
                    Nota.pComentario = $("#txtComentarioOportunidad").val();
                    var pNota = JSON.stringify(Nota);
                    AgregarComentarioOportunidad(pNota);
                    $("#divComentariosOportunidad").trigger("reloadGrid");
                },
                "Cancelar": function() {
                    $(this).dialog("close");
                }
            });
            $("#dialogComentariosOportunidad").dialog("open");
        }
    });
}

function AgregarComentarioOportunidad(request) {
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/AgregarComentarioOportunidad",
        data: request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdOportunidad").trigger("reloadGrid");
        },
        complete: function() {
            var oRequest = JSON.parse(request);
            var Oportunidad = new Object();
            Oportunidad.pIdOportunidad = oRequest.pIdOportunidad;
            ObtenerFormaComentariosOportunidad(JSON.stringify(Oportunidad));
        }
    });
}

function VerVentanaArchivoOportunidad(pIdOportunidad) {
    var request = new Object();
    request.pIdOportunidad = pIdOportunidad;
    ObtenerFormaArchivoOportunidad(JSON.stringify(request));
}

function ObtenerFormaArchivoOportunidad (request) {
    $("#dialogArchivoOportunidad").obtenerVista({
        nombreTemplate: "tmplFormaArchivoOportunidad.html",
        url: "Oportunidad.aspx/ObtenerFormaArchivoOportunidad",
        parametros: request,
        despuesDeCompilar: function(pRespuesta) {
            var idOportunidad = $("#divFormaArchivoOportunidad").attr("idOportunidad");
            var idUsuario = $("#divFormaArchivoOportunidad").attr("idUsuario");
            $("#divSubirArchivo").livequery(function() {
                var ctrlSubirLogo = new qq.FileUploader({
                    element: document.getElementById('divSubirArchivo'),
                    params: { pIdOportunidad: idOportunidad, IdUsuario: idUsuario },
                    action: '../ControladoresSubirArchivos/SubirArchivoOportunidad.ashx',
                    allowedExtensions: ["xlsx", "xls", "doc", "docx", "pdf", "txt", "jpg", "jpeg"],
                    template: '<div class="qq-uploader">' +
                    '<div class="qq-upload-drop-area"></div>' +
                    '<div class="qq-upload-container-list">' +
                    '<ul class="qq-upload-list"><li><span class="qq-upload-file"></span></li></ul></div>' +
                    '<div class="qq-upload-container-buttons">' +
                    '<div class="qq-upload-button qq-divBotonSubir">+ Subir...</div></div>' +
                    '</div>',
                    onSubmit: function(id, fileName) {
                        $(".qq-upload-list").empty();
                    },
                    onComplete: function(id, file, responseJSON) {
                        $("#dialogArchivoOportunidad").dialog("close");
                        setTimeout(function() {
                            var newRequest = "{\"pIdOportunidad\":\"" + idOportunidad + "\"}";
                            ObtenerFormaArchivoOportunidad(newRequest);
                        }, 500);
                        OcultarBloqueo();
                    }
                });
            });
            $("#dialogArchivoOportunidad").dialog("open");
        }
    });
}

function FiltroOportunidad() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pCampana = -1;
    request.pOportunidad = "";
    request.pMonto = "";

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Cotizacion.aspx/ObtenerCotizacion',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success') {
                ObtenerTotalesEstatusCotizacion();
                $('#grdCotizacion')[0].addJSONData(JSON.parse(jsondata.responseText).d);
            }
        }
    });
}

function RefrescarBotonDesactivar() {
    $('#divGridOportunidad').one('click', '.div_grdOportunidad_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdOportunidad_AI']").children().attr("baja");
        var idOportunidad = $(registro).children("td[aria-describedby='grdOportunidad_IdOportunidad']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        if (baja != "true") {
            var pMotivoCancelacion = "";
            SetCambiarEstatus(idOportunidad, pMotivoCancelacion, baja);
        } else {
            var Oportunidad = new Object();
            Oportunidad.pIdOportunidad = idOportunidad;
            var request = JSON.stringify(Oportunidad);
            $("#dialogAgregarMotivoCancelacionOportunidad").obtenerVista({
                nombreTemplate: "tmplFormaAgregarMotivoCancelacionOportunidad.html",
                despuesDeCompilar: function() {
                    $("#dialogAgregarMotivoCancelacionOportunidad").dialog("option", "buttons", {
                        "Cambiar": function() {
                            if ($("#txtMotivoCancelacionOportunidad").val() == "") {
                                MostrarMensajeError("Favor de agregar un motivo de cancelacion");
                            }
                            else {
                                var pMotivoCancelacion = $("#txtMotivoCancelacionOportunidad").val();
                                SetCambiarEstatus(idOportunidad, pMotivoCancelacion, baja);
                                $(this).dialog("close");
                            }
                        }
                    });
                    $("#dialogAgregarMotivoCancelacionOportunidad").dialog("open");
                }
            });
        }
    });
}

function ObtenerTotalesOportunidad() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pAgente = "";
    request.pOportunidad = "";
    request.pIdOportunidad = 0;
    request.pNivelInteres = -1;
    request.pSucursal = -1;
    request.pMonto = 0;
    request.pClasificacion = -1;
    request.pDivision = -1;
    request.pCerrado = 0;
    request.pEsProyecto = -1;
    request.pUrgente = -1;

    if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") {
        request.pAgente = $("#gs_Agente").val();
    }

    if ($('#gs_Cliente').val() != null && $('#gs_Cliente').val() != "") {
        request.pCliente = $("#gs_Cliente").val();
    }

    if ($('#gs_Oportunidad').val() != null && $('#gs_Oportunidad').val() != "") {
        request.pOportunidad = $("#gs_Oportunidad").val();
    }

    if ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "") {
        request.pIdOportunidad = $("#gs_IdOportunidad").val();
    }

    if ($('#gs_NivelInteres').val() != null && $('#gs_NivelInteres').val() != "") {
        request.pNivelInteres = $("#gs_NivelInteres").val();
    }

    if ($('#gs_Sucursal').val() != null && $('#gs_Sucursal').val() != "") {
        request.pSucursal = $("#gs_Sucursal").val();
    }

    if ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "") {
        request.pMonto = parseFloat($("#gs_Monto").val());
    }

    if ($('#gs_Clasificacion').val() != null && $('#gs_Clasificacion').val() != "") {
        request.pClasificacion = parseInt($("#gs_Clasificacion").val());
    }

    if ($('#gs_Division').val() != null && $('#gs_Division').val() != "") {
        request.pDivision = parseInt($("#gs_Division").val());
    }

    if ($('#gs_Cerrado').val() != null && $('#gs_Cerrado').val() != "") {
        request.pCerrado = parseInt($("#gs_Cerrado").val());
    }

    if ($('#gs_EsProyecto').val() != null && $('#gs_EsProyecto').val() != "") {
        request.pEsProyecto = parseInt($("#gs_EsProyecto").val());
    }

    if ($('#gs_Urgente').val() != null && $('#gs_Urgente').val() != "") {
        request.pUrgente = parseInt($("#gs_Urgente").val());
    }

    if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") {
        request.pAI = $("#gs_AI").val();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerTotalesOportunidad",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#sp-total-clientes").text(respuesta.Modelo.TotalClientes);
                $("#sp-total-actividades").text(respuesta.Modelo.Actividades);
                $("#sp-total").text(respuesta.Modelo.TotalOportunidades);
                $("#sp-monto").text(formato.moneda(respuesta.Modelo.MontoTotal, '$'));
                $("#sp-facturado").text(formato.moneda(respuesta.Modelo.Facturado, '$'));
                $("#sp-real").text(formato.moneda(respuesta.Modelo.MontoReal, '$'));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function ObtenerMetricasUsuario() {
    var request = new Object();
    request.pFechaInicio = "";
    request.pFechaFin = "";
    request.pUsuario = "";

    if ($('#txtFechaInicioAlcance').val() != null && $('#txtFechaInicioAlcance').val() != "") {
        request.pFechaInicio = $('#txtFechaInicioAlcance').val();
    }
    
    if ($('#txtFechaFinAlcance').val() != null && $('#txtFechaFinAlcance').val() != "") {
        request.pFechaFin  = $('#txtFechaFinAlcance').val();
    }
    
    if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") {
        request.pUsuario = $("#gs_Agente").val();
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerMetricasUsuario",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#lblAlcance1").text(formato.moneda(respuesta.Modelo.Alcance1, '$'));
                $("#lblAlcance2").text(formato.moneda(respuesta.Modelo.Alcance2, '$'));
                $("#lblMeta").text(formato.moneda(respuesta.Modelo.Meta, '$'));
                $("#lblClientesNuevos").text(respuesta.Modelo.ClienteNuevos);
                MetaMensual = respuesta.Modelo.Meta;
                ActualizarAvance();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function ObtenerClientesOportunidades() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pAgente = "";
    request.pOportunidad = "";
    request.pIdOportunidad = 0;
    request.pNivelInteres = -1;
    request.pSucursal = -1;
    request.pMonto = 0;
    request.pClasificacion = -1;
    request.pDivision = -1;
    request.pCerrado = 0;
    request.pEsProyecto = -1;
    request.pUrgente = -1;

    request.pAgente = ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") ? $("#gs_Agente").val() : '';
    request.pCliente = ($('#gs_Cliente').val() != null && $('#gs_Cliente').val() != "") ? $("#gs_Cliente").val() : '';
    request.pOportunidad = ($('#gs_Oportunidad').val() != null && $('#gs_Oportunidad').val() != "") ? $("#gs_Oportunidad").val() : '';
    request.pIdOportunidad = ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "") ? $("#gs_IdOportunidad").val() : '';
    request.pNivelInteres = ($('#gs_NivelInteres').val() != null && $('#gs_NivelInteres').val() != "") ? $("#gs_NivelInteres").val() : -1;
    request.pSucursal = ($('#gs_Sucursal').val() != null && $('#gs_Sucursal').val() != "") ? $("#gs_Sucursal").val() : -1;
    request.pMonto = ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "") ? parseFloat($("#gs_Monto").val()) : '0';
    request.pClasificacion = ($('#gs_Clasificacion').val() != null && $('#gs_Clasificacion').val() != "") ? parseInt($("#gs_Clasificacion").val()) : -1;
    request.pDivision = ($('#gs_Division').val() != null && $('#gs_Division').val() != "") ? parseInt($("#gs_Division").val()) : -1;
    request.pCerrado = ($('#gs_Cerrado').val() != null && $('#gs_Cerrado').val() != "") ? parseInt($("#gs_Cerrado").val()) : -1;
    request.pEsProyecto = ($('#gs_EsProyecto').val() != null && $('#gs_EsProyecto').val() != "") ? parseInt($("#gs_EsProyecto").val()) : -1;
    request.pUrgente = ($('#gs_Urgente').val() != null && $('#gs_Urgente').val() != "") ? parseInt($("#gs_Urgente").val()) : -1;
    request.pAI = ($('#gs_AI').val() != null && $('#gs_AI').val() != "") ? $("#gs_AI").val() : 0 ;

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerClientesOportunidades",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#sp-total-clientes").text(respuesta.Modelo.TotalClientes);
                $("#sp-total").text(respuesta.Modelo.TotalOportunidades);
                $("#sp-monto").text(formato.moneda(respuesta.Modelo.MontoTotal, '$'));
                $("#sp-facturado").text(formato.moneda(respuesta.Modelo.Facturado, '$'));
                $("#sp-real").text(formato.moneda(respuesta.Modelo.MontoReal, '$'));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function ObtenerResultadoVentas() {
    var request = new Object();
    request.pUsuario = ($('#gs_Agente').val() != null || $("#gs_Agente").val() != "") ? $("#gs_Agente").val() : "";
    request.pIdSucursal = ($('#gs_Sucursal').val() != null || $("#gs_Sucursal").val() != "") ? parseInt($("#gs_Sucursal").val()) : -1;
    request.pFechaInicio = ($('#txtFechaInicio').val() != null || $("#txtFechaInicio").val() != "") ? $("#txtFechaInicio").val() : "";
    request.pFechaFinal = ($('#txtFechaFinal').val() != null || $("#txtFechaFinal").val() != "") ? $("#txtFechaFinal").val() : "";

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerResultadoVentas",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var resultado = $.parseJSON(pRespuesta.responseText)
            var d = $.parseJSON(resultado.d);
            $("#lblResultado").text(formato.moneda(d.Table[0].Resultado, '$'));
            $("#lblMetaNecesario").text(formato.moneda(d.Table[0].Metas * d.Table[0].Avance, '$'));
            $("#lblAvanceNecesario").text((d.Table[0].Avance * 100).toFixed(2)+"%");
            $("#lblClientes").text(d.Table[0].Clientes);
            ResultadoMensual = d.Table[0].Resultado;
			
            ActualizarAvance();
        }
    });
}

function ActualizarAvance() {
    if (ResultadoMensual != 0 && MetaMensual != 0) {
        avance = ResultadoMensual / MetaMensual;
        $("#lblAvanceVenta").text((avance * 100).toFixed(2) + "%");
    }
}

function ObtenerResultadoVentasMesAnterior() {
    var request = new Object();
    request.pUsuario = ($('#gs_Agente').val() != null || $("#gs_Agente").val() != "") ? $("#gs_Agente").val() : "";
    request.pIdSucursal = ($('#gs_Sucursal').val() != null || $("#gs_Sucursal").val() != "") ? parseInt($("#gs_Sucursal").val()) : -1;
    request.pFechaInicio = ($('#txtFechaInicio').val() != null || $("#txtFechaInicio").val() != "") ? $("#txtFechaInicio").val() : "";
    request.pFechaFinal = ($('#txtFechaFinal').val() != null || $("#txtFechaFinal").val() != "") ? $("#txtFechaFinal").val() : "";

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerResultadoVentasMesAnterior",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var resultado = $.parseJSON(pRespuesta.responseText)
            var d = $.parseJSON(resultado.d);
            $("#lblResAnterior").text(formato.moneda(d.Table[0].Resultado, '$'));
            $("#lblCliAnterior").text(d.Table[0].Clientes);
        }
    });
}

//###########################################################################################
// Funciones para graficas de oportunidades
//###########################################################################################
function InicializarDialogGraficasOportunidades() {

    $("#dialogGraficasOportunidades").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function(event, ui) {
            $("#dialogGraficasOportunidades").empty();
        },
        open: function(event, ui) {
            $("#divTabsGraficas", this).tabs();
            ObtenerDatosGrafica();
            ObtenerGraficaNivelInteres();
        }
    });
    
    $("#btnVerGraficasOportunidad").click(function(event) {
        ObtenerFormaGraficas();
    });
}

function ObtenerFormaGraficas() {
    $("#dialogGraficasOportunidades").obtenerVista({
        nombreTemplate: "tmplGraficasOportunidades.html",
        url: "Oportunidad.aspx/ObtenerFormaGraficasOportunidades",
        parametros: Filtros(),
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogGraficasOportunidades").dialog("open");     
        }
    });
}

function ObtenerDatosGrafica() {
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerDatosGrafica",
        data: Filtros(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                CargarDatosGraficas(respuesta.Modelo);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function CargarDatosGraficas(pData) {
    var oportunidades = pData.Oportunidades;
    var cotizaciones = pData.Cotizaciones;
    var converciones = pData.Conversiones;
    var facturas = pData.Facturas;
    var datos = [oportunidades, cotizaciones, converciones, facturas];
    var ticks = ['Oportunidades', 'Cotizaciones', 'Pedidos/Proyectos', 'Facturado'];
    var plot1 = $.jqplot('cvsFunel', [datos], {
        seriesColors: ['#99CCFF', '#FFFF99', '#FFDD66', '#99FF99'],
        seriesDefaults: { renderer: $.jqplot.BarRenderer, rendererOptions: { fillToZero: true, varyBarColor: true }, pointLabels: { show: true} },
        axes: { xaxis: { renderer: $.jqplot.CategoryAxisRenderer, ticks: ticks }, yaxis: { pad: 1.2, tickOptions: { formatString: "$%'.2f"}} }
    });
}

function ObtenerGraficaNivelInteres() {
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerGraficaNivelInteres",
        data: Filtros(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#cvsNivelesInteres").empty();
                CargarGraficaNivelInteres(respuesta.Modelo);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function ObtenerDatosNivelInteres() {
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerDatosNivelInteres",
        data: Filtros(),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                var Alto = respuesta.Modelo.Alto;
                var Medio = respuesta.Modelo.Medio;
                var Bajo = respuesta.Modelo.Bajo;
                var Pronostico = (Bajo * 0.02) + (Medio * 0.2) + (Alto * 0.75);
                $("#lblNivelInteresBajo").text(formato.moneda(Bajo, '$'));
                $("#lblNivelInteresMedio").text(formato.moneda(Medio, '$'));
                $("#lblNivelInteresAlto").text(formato.moneda(Alto, '$'));
                $("#lblPronostico").text(formato.moneda(Pronostico, '$'));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function CargarGraficaNivelInteres(pData) {
    var Alto = pData.Alto;
    var Alto2 = pData.Alto2;
    var Medio = pData.Medio;
    var Medio2 = pData.Medio2;
    var Bajo = pData.Bajo;
    var Bajo2 = pData.Bajo2;
    var datos = [Bajo, Medio, Alto];
    var ticks = ['Bajo (?w)','Medio (4w)','Alto (2w)'];
    var plot1 = $.jqplot('cvsNivelesInteres', [datos], {
        gridDimensions: {
            height: 250,
            width: 800
        },
        seriesColors: ['#99CCFF', '#99CCFF', '#99CCFF'],
        seriesDefaults: {
            renderer: $.jqplot.BarRenderer,
            rendererOptions: {
                fillToZero: true,
                varyBarColor: true
            },
            pointLabels: {
                show: true
            }
        },
        axes: {
            xaxis: {
                renderer: $.jqplot.CategoryAxisRenderer,
                ticks: ticks
            },
            yaxis: {
                pad: 1.2,
                tickOptions: {
                    formatString: "$%'.2f"
                }
            }
        }
    });
}

function Filtros() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pAgente = "";
    request.pOportunidad = "";
    request.pIdOportunidad = 0;
    request.pNivelInteres = -1;
    request.pSucursal = -1;
    request.pMonto = 0;
    request.pClasificacion = -1;
    request.pDivision = -1;
    request.pCerrado = 0;
    request.pEsProyecto = -1;
    request.pUrgente = -1;

    if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") { request.pAgente = $("#gs_Agente").val(); }

    if ($('#gs_Cliente').val() != null && $('#gs_Cliente').val() != "") { request.pCliente = $("#gs_Cliente").val(); }

    if ($('#gs_Oportunidad').val() != null && $('#gs_Oportunidad').val() != "") { request.pOportunidad = $("#gs_Oportunidad").val(); }

    if ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "") { request.pIdOportunidad = $("#gs_IdOportunidad").val(); }

    if ($('#gs_NivelInteres').val() != null && $('#gs_NivelInteres').val() != "") { request.pNivelInteres = $("#gs_NivelInteres").val(); }

    if ($('#gs_Sucursal').val() != null && $('#gs_Sucursal').val() != "") { request.pSucursal = $("#gs_Sucursal").val(); }

    if ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "") { request.pMonto = parseFloat($("#gs_Monto").val()); }

    if ($('#gs_Clasificacion').val() != null && $('#gs_Clasificacion').val() != "") { request.pClasificacion = parseInt($("#gs_Clasificacion").val()); }

    if ($('#gs_Division').val() != null && $('#gs_Division').val() != "") { request.pDivision = parseInt($("#gs_Division").val()); }

    if ($('#gs_Cerrado').val() != null && $('#gs_Cerrado').val() != "") { request.pCerrado = parseInt($("#gs_Cerrado").val()); }

    if ($('#gs_EsProyecto').val() != null && $('#gs_EsProyecto').val() != "") { request.pEsProyecto = parseInt($("#gs_EsProyecto").val()); }

    if ($('#gs_Urgente').val() != null && $('#gs_Urgente').val() != "") { request.pUrgente = parseInt($("#gs_Urgente").val()); }

    if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") { request.pAI = $("#gs_AI").val(); }

    var pRequest = JSON.stringify(request);
    return pRequest;
}

//###########################################################################################
// 
//###########################################################################################
function ObtenerTotalCotizaciones() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pAgente = "";
    request.pOportunidad = "";
    request.pIdOportunidad = 0;
    request.pNivelInteres = -1;
    request.pSucursal = -1;
    request.pMonto = 0;
    request.pClasificacion = -1;
    request.pDivision = -1;
    request.pCerrado = 0;
    request.pEsProyecto = -1;
    request.pUrgente = -1;

    if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") { request.pAgente = $("#gs_Agente").val(); }

    if ($('#gs_Cliente').val() != null && $('#gs_Cliente').val() != "") { request.pCliente = $("#gs_Cliente").val(); }

    if ($('#gs_Oportunidad').val() != null && $('#gs_Oportunidad').val() != "") { request.pOportunidad = $("#gs_Oportunidad").val(); }

    if ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "") { request.pIdOportunidad = $("#gs_IdOportunidad").val(); }

    if ($('#gs_NivelInteres').val() != null && $('#gs_NivelInteres').val() != "") { request.pNivelInteres = $("#gs_NivelInteres").val(); }

    if ($('#gs_Sucursal').val() != null && $('#gs_Sucursal').val() != "") { request.pSucursal = $("#gs_Sucursal").val(); }

    if ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "") { request.pMonto = parseFloat($("#gs_Monto").val()); }

    if ($('#gs_Clasificacion').val() != null && $('#gs_Clasificacion').val() != "") { request.pClasificacion = parseInt($("#gs_Clasificacion").val()); }

    if ($('#gs_Division').val() != null && $('#gs_Division').val() != "") { request.pDivision = parseInt($("#gs_Division").val()); }

    if ($('#gs_Cerrado').val() != null && $('#gs_Cerrado').val() != "") { request.pCerrado = parseInt($("#gs_Cerrado").val()); }

    if ($('#gs_EsProyecto').val() != null && $('#gs_EsProyecto').val() != "") { request.pEsProyecto = parseInt($('#gs_EsProyecto').val()) }

    if ($('#gs_Urgente').val() != null && $('#gs_Urgente').val() != "") { request.pUrgente = parseInt($('#gs_Urgente').val()) }

    if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") { request.pAI = $("#gs_AI").val(); }

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerTotalCotizaciones",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#sp-cotizciones").text(formato.moneda(respuesta.Modelo.Cotizaciones, '$'));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function ObtenerTotalPedidos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pAgente = "";
    request.pOportunidad = "";
    request.pIdOportunidad = 0;
    request.pNivelInteres = -1;
    request.pSucursal = -1;
    request.pMonto = 0;
    request.pClasificacion = -1;
    request.pDivision = -1;
    request.pCerrado = 0;
    request.pEsProyecto = -1;
    request.pUrgente = -1;

    if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") { request.pAgente = $("#gs_Agente").val(); }

    if ($('#gs_Cliente').val() != null && $('#gs_Cliente').val() != "") { request.pCliente = $("#gs_Cliente").val(); }

    if ($('#gs_Oportunidad').val() != null && $('#gs_Oportunidad').val() != "") { request.pOportunidad = $("#gs_Oportunidad").val(); }

    if ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "") { request.pIdOportunidad = $("#gs_IdOportunidad").val(); }

    if ($('#gs_NivelInteres').val() != null && $('#gs_NivelInteres').val() != "") { request.pNivelInteres = $("#gs_NivelInteres").val(); }

    if ($('#gs_Sucursal').val() != null && $('#gs_Sucursal').val() != "") { request.pSucursal = $("#gs_Sucursal").val(); }

    if ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "") { request.pMonto = parseFloat($("#gs_Monto").val()); }

    if ($('#gs_Clasificacion').val() != null && $('#gs_Clasificacion').val() != "") { request.pClasificacion = parseInt($("#gs_Clasificacion").val()); }

    if ($('#gs_Division').val() != null && $('#gs_Division').val() != "") { request.pDivision = parseInt($("#gs_Division").val()); }

    if ($('#gs_Cerrado').val() != null && $('#gs_Cerrado').val() != "") { request.pCerrado = parseInt($("#gs_Cerrado").val()); }

    if ($('#gs_EsProyecto').val() != null && $('#gs_EsProyecto').val() != "") { request.pEsProyecto = parseInt($('#gs_EsProyecto').val()) }

    if ($('#gs_Urgente').val() != null && $('#gs_Urgente').val() != "") { request.pUrgente = parseInt($('#gs_Urgente').val()) }

    if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") { request.pAI = $("#gs_AI").val(); }

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerTotalPedidos",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#sp-pedidos").text(formato.moneda(respuesta.Modelo.Pedidos, '$'));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function ObtenerTotalProyectos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum');
    request.pPaginaActual = $('#grdOportunidad').getGridParam('page');
    request.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname');
    request.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder');
    request.pAI = 0;
    request.pCliente = "";
    request.pAgente = "";
    request.pOportunidad = "";
    request.pIdOportunidad = 0;
    request.pNivelInteres = -1;
    request.pSucursal = -1;
    request.pMonto = 0;
    request.pClasificacion = -1;
    request.pDivision = -1;
    request.pCerrado = 0;
    request.pEsProyecto = -1;
    request.pUrgente = -1;

    if ($('#gs_Agente').val() != null && $('#gs_Agente').val() != "") { request.pAgente = $("#gs_Agente").val(); }

    if ($('#gs_Cliente').val() != null && $('#gs_Cliente').val() != "") { request.pCliente = $("#gs_Cliente").val(); }

    if ($('#gs_Oportunidad').val() != null && $('#gs_Oportunidad').val() != "") { request.pOportunidad = $("#gs_Oportunidad").val(); }

    if ($('#gs_IdOportunidad').val() != null && $('#gs_IdOportunidad').val() != "") { request.pIdOportunidad = $("#gs_IdOportunidad").val(); }

    if ($('#gs_NivelInteres').val() != null && $('#gs_NivelInteres').val() != "") { request.pNivelInteres = $("#gs_NivelInteres").val(); }

    if ($('#gs_Sucursal').val() != null && $('#gs_Sucursal').val() != "") { request.pSucursal = $("#gs_Sucursal").val(); }

    if ($('#gs_Monto').val() != null && $('#gs_Monto').val() != "") { request.pMonto = parseFloat($("#gs_Monto").val()); }

    if ($('#gs_Clasificacion').val() != null && $('#gs_Clasificacion').val() != "") { request.pClasificacion = parseInt($("#gs_Clasificacion").val()); }

    if ($('#gs_Division').val() != null && $('#gs_Division').val() != "") { request.pDivision = parseInt($("#gs_Division").val()); }

    if ($('#gs_Cerrado').val() != null && $('#gs_Cerrado').val() != "") { request.pCerrado = parseInt($("#gs_Cerrado").val()); }

    if ($('#gs_EsProyecto').val() != null && $('#gs_EsProyecto').val() != "") { request.pEsProyecto = parseInt($('#gs_EsProyecto').val()) }

    if ($('#gs_Urgente').val() != null && $('#gs_Urgente').val() != "") { request.pUrgente = parseInt($('#gs_Urgente').val()) }

    if ($('#gs_AI').val() != null && $('#gs_AI').val() != "") { request.pAI = $("#gs_AI").val(); }

    var pRequest = JSON.stringify(request);
    $.ajax({
        type: "POST",
        url: "Oportunidad.aspx/ObtenerTotalProyectos",
        data: pRequest,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        complete: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.responseText);
            respuesta = $.parseJSON(respuesta.d);
            if (respuesta.Error == 0) {
                $("#sp-proyectos").text(formato.moneda(respuesta.Modelo.Proyectos, '$'));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        }
    });
}

function FiltroOportunidad() {
    var oRequest = new Object();
    oRequest.pTamanoPaginacion = 10;
    oRequest.pPaginaActual = 1;
    oRequest.pColumnaOrden = "IdOportunidad";
    oRequest.pTipoOrden = "DESC";
    oRequest.pIdOportunidad = "";
    oRequest.pOportunidad = "";
    oRequest.pAgente = "";
    oRequest.pCliente = "";
    oRequest.pNivelInteres = -1;
    oRequest.pSucursal = 1;
    oRequest.pMonto = "";
    oRequest.pClasificacion = -1;
    oRequest.pCampana = -1;
    oRequest.pDivision = -1;
    oRequest.pCerrado = 0;
    oRequest.pEsProyecto = -1;
    oRequest.pUrgente = -1;
    oRequest.pAI = 0;

    if ($('#grdOportunidad').getGridParam('rowNum') != null) { oRequest.pTamanoPaginacion = $('#grdOportunidad').getGridParam('rowNum') }
    if ($('#grdOportunidad').getGridParam('page') != null) { oRequest.pPaginaActual = $('#grdOportunidad').getGridParam('page') }
    if ($('#grdOportunidad').getGridParam('sortname') != null) { oRequest.pColumnaOrden = $('#grdOportunidad').getGridParam('sortname') }
    if ($('#grdOportunidad').getGridParam('sortorder') != null) { oRequest.pTipoOrden = $('#grdOportunidad').getGridParam('sortorder') }
    if ($('#gs_IdOportunidad').val() != null) { oRequest.pIdOportunidad = $('#gs_IdOportunidad').val() }
    if ($('#gs_Oportunidad').val() != null) { oRequest.pOportunidad = $('#gs_Oportunidad').val() }
    if ($('#gs_Agente').val() != null) { oRequest.pAgente = $('#gs_Agente').val() }
    if ($('#gs_Cliente').val() != null) { oRequest.pCliente = $('#gs_Cliente').val() }
    if ($('#gs_NivelInteres').val() != null) { oRequest.pNivelInteres = parseInt($('#gs_NivelInteres').val()) }
    if ($('#gs_Sucursal').val() != null) { oRequest.pSucursal = parseInt($('#gs_Sucursal').val()) }
    if ($('#gs_Monto').val() != null) { oRequest.pMonto = $('#gs_Monto').val() }
    if ($('#gs_Clasificacion').val() != null) { oRequest.pClasificacion = parseInt($('#gs_Clasificacion').val()) }
    if ($('#gs_Campana').val() != null) { oRequest.pCampana = parseInt($('#gs_Campana').val()) }
    if ($('#gs_Division').val() != null) { oRequest.pDivision = parseInt($('#gs_Division').val()) }
    if ($('#gs_Cerrado').val() != null) { oRequest.pCerrado = parseInt($('#gs_Cerrado').val()) }
    if ($('#gs_EsProyecto').val() != null) { oRequest.pEsProyecto = parseInt($('#gs_EsProyecto').val()) }
    if ($('#gs_Urgente').val() != null) { oRequest.pUrgente = parseInt($('#gs_Urgente').val()) }
    if ($('#gs_AI').val() != null) { oRequest.pAI = parseInt($('#gs_AI').val()) }
    
    $.ajax({
        url: 'Oportunidad.aspx/ObtenerOportunidad',
        data: JSON.stringify(oRequest),
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdOportunidad')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function Cambiar_TituloFecha() {
    $("td[aria-describedby='grdOportunidad_FechaNota']").mouseover(function() {
        var nota = $(this).parent("tr").children("td[aria-describedby='grdOportunidad_MensajeNota']").text();
        if (nota.length > 0) {
            $(this).attr("title", nota)
            $(this).tooltip();
        }
    });
}

function Termino_grdOportunidad() {
    Cambiar_TituloFecha();
    ObtenerTotalesOportunidad();
    ObtenerTotalCotizaciones();
    ObtenerTotalPedidos();
    ObtenerTotalProyectos();
    ObtenerMetricasUsuario();
    ObtenerResultadoVentas();
    ObtenerResultadoVentasMesAnterior();
    ObtenerDatosNivelInteres();
    ValidarUtilidad();
    ObtenerMetasSucursales();
}

function BotonesConsultarOportunidad() {

	var Grid = $("#grdOportunidad");
	$("td[aria-describedby=grdOportunidad_IdOportunidad]", Grid).each(function (indice, elemento) {
		$(elemento).css({ "cursor": "pointer", "text-decoration": "underline" });
		$(elemento).click(function () {
			var Oportunidad = new Object();
			Oportunidad.pIdOportunidad = parseInt($(elemento).text());
            //ObtenerFormaConsultarOportunidad(JSON.stringify(Oportunidad));
            ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
		});
	});

	$("td[aria-describedby=grdOportunidad_Oportunidad]", Grid).each(function (indice, elemento) {
		$(elemento).css({ "cursor": "pointer", "text-decoration": "underline" });
		$(elemento).click(function () {
			var Oportunidad = new Object();
			Oportunidad.pIdOportunidad = parseInt($(elemento).prev("td[aria-describedby=grdOportunidad_IdOportunidad]").text());
            //ObtenerFormaConsultarOportunidad(JSON.stringify(Oportunidad));
            ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
		});
	});

}

function ValidarUtilidad()
{
    $("td[aria-describedby=grdOportunidad_Utilidad]", "#grdOportunidad").each(function (index, elemento) {
        var costo = parseFloat($(elemento).parent("tr").children("td[aria-describedby=grdOportunidad_Costo]").text().replace("$", "").replace(",", ""));
        var facturado = parseFloat($(elemento).parent("tr").children("td[aria-describedby=grdOportunidad_Facturas]").text().replace("$", "").replace(",", ""));
        if ((costo == 0 && facturado > 0) || facturado - costo < 0)
            $(elemento).css({ "color": "red" });
    });
}

function BotonesConsultarOportunidad() {

	var Grid = $("#grdOportunidad");
	$("td[aria-describedby=grdOportunidad_IdOportunidad]", Grid).each(function (indice, elemento) {
		$(elemento).css({ "cursor": "pointer", "text-decoration": "underline" });
		$(elemento).click(function () {
			var Oportunidad = new Object();
			Oportunidad.pIdOportunidad = parseInt($(elemento).text());
            //ObtenerFormaConsultarOportunidad(JSON.stringify(Oportunidad));
            ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
		});
	});

	$("td[aria-describedby=grdOportunidad_Oportunidad]", Grid).each(function (indice, elemento) {
		$(elemento).css({ "cursor": "pointer", "text-decoration": "underline" });
		$(elemento).click(function () {
			var Oportunidad = new Object();
			Oportunidad.pIdOportunidad = parseInt($(elemento).prev("td[aria-describedby=grdOportunidad_IdOportunidad]").text());
			//ObtenerFormaConsultarOportunidad(JSON.stringify(Oportunidad));
            ObtenerFormaEditarOportunidad(JSON.stringify(Oportunidad))
		});
	});

}

function ObtenerGraficaMesPasado() {
    var request = new Object();
    request.pFechaInicio = ($('#txtFechaInicio3').val() != null || $("#txtFechaInicio3").val() != "") ? $("#txtFechaInicio3").val() : "";
    request.pFechaFinal = ($('#txtFechaFinal3').val() != null || $("#txtFechaFinal3").val() != "") ? $("#txtFechaFinal3").val() : "";

    var pRequest = JSON.stringify(request);
    $("#dialogGraficaMesPasado").obtenerVista({
        nombreTemplate: "tmplGraficaResultadoVentasDivision2.html",
        url: "Oportunidad.aspx/ObtenerGraficaResultadoVentaDivision",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
        	$("#dialogGraficaMesPasado").dialog("open");
        	var Divisiones = pRespuesta.modelo.Divisiones;
        	var Sucursales = pRespuesta.modelo.Sucursales;
        	var Agentes = pRespuesta.modelo.Agentes;
        	var Actividades = pRespuesta.modelo.Actividades;
        	var AvanceAgentes = pRespuesta.modelo.AvanceAgentes;
        	var VentasCliente = pRespuesta.modelo.VentasCliente;
        	CargarGraficaMesActual(Divisiones);
        	CargarGraficaMesSucursalesActual(Sucursales);
        	CargarGraficaMesAgentesActual(Agentes);
        	CargarGraficaActividades(Actividades);
        	CargarTablaAvanceAgentes(AvanceAgentes);
        	CargarTablaVentasCliente(VentasCliente);
        	$("#tabsGraficasVentas").tabs();
        	$("#ExportarReporteVentaCliente").click(function () {
        		ReporteVentasCliente();
        	});
        }
    });
}

function ObtenerGraficaMesActual() {
    var request = new Object();
    request.pFechaInicio = ($('#txtFechaInicio').val() != null || $("#txtFechaInicio").val() != "") ? $("#txtFechaInicio").val() : "";
    request.pFechaFinal = ($('#txtFechaFinal').val() != null || $("#txtFechaFinal").val() != "") ? $("#txtFechaFinal").val() : "";

    var pRequest = JSON.stringify(request);
    $("#dialogGraficaMesActual").obtenerVista({
        nombreTemplate: "tmplGraficaResultadoVentasDivision.html",
        url: "Oportunidad.aspx/ObtenerGraficaResultadoVentaDivision",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {

            $("#dialogGraficaMesActual").dialog("open");

            $("#txtFechaInicialIRMA", "#dialogGraficaMesActual").datepicker({
                dateFormat: "dd/mm/yy"
            });

            $("#txtFechaFinalIRMA", "#dialogGraficaMesActual").datepicker({
                dateFormat: "dd/mm/yy"
            });

            $("#btnCargarTablaVentasCliente", "#dialogGraficaMesActual").click(function() {
                RecargarTablaVentasCliente()
            });

            var Divisiones = pRespuesta.modelo.Divisiones;
            var Sucursales = pRespuesta.modelo.Sucursales;
            var Oportunidades = pRespuesta.modelo.Oportunidades;
            console.log(Oportunidades);
            var OportunidadesAlta = pRespuesta.modelo.OportunidadesAlta;
            var OportunidadesMedia = pRespuesta.modelo.OportunidadesMedia;
            var OportunidadesBaja = pRespuesta.modelo.OportunidadesBaja;
            var Agentes = pRespuesta.modelo.Agentes;
            var Actividades = pRespuesta.modelo.Actividades;
            var AvanceAgentes = pRespuesta.modelo.AvanceAgentes;
            var VentasCliente = pRespuesta.modelo.VentasCliente;
            var AgentesClientes = pRespuesta.modelo.AgentesClientes;

            CargarGraficaMesActual(Divisiones);
            CargarGraficaMesSucursalesActual(Sucursales);
            CargarGraficaMesActualOportunidades(Oportunidades);
            CargarGraficaMesActualOportunidadesAlta(OportunidadesAlta);
            CargarGraficaMesActualOportunidadesMedia(OportunidadesMedia);
            CargarGraficaMesActualOportunidadesBaja(OportunidadesBaja);
            CargarGraficaMesAgentesActual(Agentes);
            CargarGraficaActividades(Actividades);
            CargarTablaAvanceAgentes(AvanceAgentes);
            CargarTablaVentasCliente(VentasCliente);
        	//CargarTablaAtencionClientes(AgentesClientes);

            $("#rptAnalisisAtencionCliente").obtenerVista({
				nombreTemplate: "tmplReporteAtencionClientes.html",
            	modelo: pRespuesta.modelo
            });

            $(".btnJqueryUI").button();

            $("#tabsGraficasVentas").tabs();

            $("#ExportarReporteVentaCliente").click(function () {
                ReporteVentasCliente();
            });

            // Sabana de clientes
            $("#txtFechaInicialSabana", "#divSabanaClientes").datepicker({
                dateFormat: "dd/mm/yy"
            }).change(CalcularIncidentes);

            $("#txtFechaFinalSabana", "#divSabanaClientes").datepicker({
                dateFormat: "dd/mm/yy"
            }).change(CalcularIncidentes);

            $("#btnCargarSabanaClientes").click(SabanaClientes).click();

            $("#txtClienteSabana", "#divSabanaClientes").autocomplete({
                source: function (request, response) {
                    var pRequest = new Object();
                    pRequest.pCliente = $("#txtClienteSabana", "#divSabanaClientes").val();
                    $.ajax({
                        type: 'POST',
                        url: 'Oportunidad.aspx/BuscarCliente',
                        data: JSON.stringify(pRequest),
                        dataType: 'json',
                        contentType: 'application/json; charset=utf-8',
                        success: function (pRespuesta) {
                            var json = jQuery.parseJSON(pRespuesta.d);
                            response($.map(json.Table, function (item) {
                                return { label: item.Cliente, value: item.Cliente, id: item.IdCliente, Saldo: item.Saldo }
                            }));
                        }
                    });
                },
                minLength: 2,
                select: function (event, ui) {
                    var pIdCliente = ui.item.id;
                    var Saldo = ui.item.Saldo;
                    $("#txtClienteSabana", "#divSabanaClientes").attr("IdCliente", pIdCliente);
                },
                change: function (event, ui) { if ($(this).val() == "") { $(this).attr("IdCliente",0)}},
                open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
                close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
            });

            $("#divTabsGraficasOportunidades").tabs({ orientation: "vertical" });

        }
    });
}

function SabanaClientes() {
    MostrarBloqueo();
    var Sabana = new Object();
    Sabana.FechaInicial = $("#txtFechaInicialSabana").val();
    Sabana.FechaFinal = $("#txtFechaFinalSabana").val();
    Sabana.IdUsuario = parseInt($("#cmbIdUsuarioAgenteSabana").val());
    Sabana.IdCliente = parseInt($("#txtClienteSabana").attr("IdCliente"));
    Sabana.Incidentes = parseInt($("#cmbIncidentesSabana").val());
    var Request = JSON.stringify(Sabana);
    $.ajax({
        url: "Oportunidad.aspx/ObtenerSabanaClientes",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            if (json.Error == 0)
            {
                GridSabanaClientes(json.Modelo.Clientes);
            }
            else
            {
                MostrarMensajeError(json.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function CalcularIncidentes() {
    $("#cmbIncidentesSabana").html('');
    var FechaInicial = $("#txtFechaInicialSabana").val();
    var FechaFinal = $("#txtFechaFinalSabana").val();
    var f1 = FechaInicial.split("/");
    var f2 = FechaFinal.split("/");
    var fecha1 = new Date(f1[2], parseInt(f1[1])-1, f1[0]);
    var fecha2 = new Date(f2[2], parseInt(f2[1])-1, f2[0]);
    var meses = Meses(fecha1, fecha2);
    $("#cmbIncidentesSabana").append($('<option value="-1">Todos</option>'));
    for (var i = 0; i < meses; i++) {
        $("#cmbIncidentesSabana").append($('<option value="' + (meses - i) + '">' + (meses - i) + '</option>'));
    }
}

function Meses(fecha1, fecha2) {
    var meses = 1;
    meses += (fecha2.getFullYear() - fecha1.getFullYear()) * 12;
    meses -= fecha1.getMonth() + 1;
    meses += fecha2.getMonth() + 1;
    return meses;
}

function GridSabanaClientes(Clientes) {
    $("#divTablaSabanaCliente").html('');
    if (Clientes.length > 0) {
        var Sabana = document.createElement("table");
        var Encabezado = document.createElement("thead");
        var Cuerpo = document.createElement("tbody");
        var Columnas = document.createElement("tr");
        for (col in Clientes[0]) {
            var th = document.createElement("th");
            th.innerHTML = col;
            Columnas.appendChild(th);
        }
        Encabezado.appendChild(Columnas);
        Sabana.appendChild(Encabezado);
        for(x in Clientes){
            var tr = document.createElement("tr");
            var width = 960 / Clientes[x].length;
            for (y in Clientes[x]) {
                var td = document.createElement("td");
                var valor = (!isNaN(parseFloat(Clientes[x][y])) && y != 'No.' && y != 'Frequencia') ? formato.moneda(parseFloat(Clientes[x][y]), '$') : Clientes[x][y];
                var alineacion = (!isNaN(parseFloat(Clientes[x][y]))) ? "right" : "left";
                td.innerHTML = valor;
                td.width = width;
                td.align = alineacion;
                tr.appendChild(td);
            }
            Cuerpo.appendChild(tr);
        }
        Sabana.appendChild(Cuerpo);
        Sabana.border = 1;
        Sabana.cellPadding = 0;
        Sabana.cellSpacing = 0;
        Sabana.width = 960;
        $("#divTablaSabanaCliente").append(Sabana);
    }
}

function RecargarTablaVentasCliente() {
    var Reporte = new Object();
    Reporte.FechaInicial = $("#txtFechaInicialIRMA", "#dialogGraficaMesActual").val();
    Reporte.FechaFinal = $("#txtFechaFinalIRMA", "#dialogGraficaMesActual").val();
    Reporte.IdSucursal = parseInt($("#CmbSucursal", "#dialogGraficaMesActual").val());
    
    var Request = JSON.stringify(Reporte);

    $.ajax({
        url: 'Oportunidad.aspx/RecargarTablaVentasCliente',
        data: Request,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        success: function(Respuesta) {
            var Json = JSON.parse(Respuesta.d);
            if (Json.Error == 0) {
                var VentasCliente = Json.Modelo.VentasCliente;
                CargarTablaVentasCliente(VentasCliente);
            }
            else {
                MostrarMensajeError(Json.Descripcion);
            }
        }
    });
    
}

function ReporteVentasCliente() {
    var FechaInicio = $("#txtFechaInicialIRMA", "#dialogGraficaMesActual").val();
    var FechaFin = $("#txtFechaFinalIRMA", "#dialogGraficaMesActual").val();
    var IdSucursal = $("#CmbSucursal", "#dialogGraficaMesActual").val();
	var Url = window.location.toString();
	Url = Url.replace("/Paginas/Oportunidad.aspx", "") + "/ExportacionesExcel/ExportarExcelReporteVentaCliente.aspx";

	var Forma = document.createElement("form");
	Forma.action = Url;
	Forma.method = "post";

	var InicioFecha = document.createElement("input");
	InicioFecha.name = "pFechaInicio";
	InicioFecha.type = "hidden";
	InicioFecha.value = FechaInicio;

	var FinFecha = document.createElement("input");
	FinFecha.name = "pFechaFinal";
	FinFecha.type = "hidden";
	FinFecha.value = FechaFin;

	var Sucursal = document.createElement("input");
	Sucursal.name = "pIdSucursal";
	Sucursal.type = "hidden";
	Sucursal.value = IdSucursal;

	$(Forma).append(InicioFecha).append(FinFecha).append(Sucursal);
	
	var Ventana = window.open("");
	$(Ventana.document.body).append(Forma);

	Forma.submit();
}

function CargarGraficaMesActual(Divisiones) {
    var plot1 = $.jqplot('cnvResultadoVentasDivision', Divisiones, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Divisiones[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });

}

function CargarGraficaMesSucursalesActual(Sucursales) {
    var plot1 = $.jqplot('cnvResultadoVentasSucursal', Sucursales, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Sucursales[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });
}

function CargarGraficaMesActualOportunidades(Oportunidades) {
    var plot1 = $.jqplot('cnvResultadoVentasOportunidades', Oportunidades, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Oportunidades[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });

}

function CargarGraficaMesActualOportunidadesAlta(Oportunidades) {
    var plot1 = $.jqplot('cnvResultadoVentasOportunidadesAlta', Oportunidades, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Oportunidades[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });

}

function CargarGraficaMesActualOportunidadesMedia(Oportunidades) {
    var plot1 = $.jqplot('cnvResultadoVentasOportunidadesMedia', Oportunidades, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Oportunidades[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });

}

function CargarGraficaMesActualOportunidadesBaja(Oportunidades) {
    var plot1 = $.jqplot('cnvResultadoVentasOportunidadesBaja', Oportunidades, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Oportunidades[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });

}

function CargarGraficaMesAgentesActual(Agentes) {
    var plot1 = $.jqplot('cnvResultadoVentasAgente', Agentes, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'inside',
            rendererOptions: {
                numberRows: Agentes[0].length
            },
            location: 'ne',
            marginTop: '10px',
            mark: 'outside'
        }
    });
}

function CargarGraficaActividades(Datos) {
    $("#cnvResultadoActividades").html('');
    var tabla = document.createElement("table");
    tabla.width = "100%";
    $(tabla).css("font-size", "9px");
    var head = document.createElement("thead");
    var body = document.createElement("tbody");
    var colors = [];
    var totales = [];
    for (x in Datos) {
        var i = 0;
        if (x == 0) {
            var tr1 = document.createElement("tr");
            var skip = 0;
            for (y in Datos[x]) {
                var d = y.split('-');
                var th = document.createElement("th");
                th.innerHTML = d[0];
                totales.push(0);
                $(th).css({"background-color":"#3D4C5E","color":"#FFF"});
                tr1.appendChild(th);
            }
            head.appendChild(tr1);
        }
        i = 0;
        var tr = document.createElement("tr");
        //$(tr).mouseover(function() { $(this).css("font-weight", "bold"); }).mouseout(function() { $(this).css("font-weight", "normal"); });
        for (y in Datos[x]) {
            var d = y.split('-');
            var td = document.createElement("td");
            var ca = (!isNaN(parseInt(Datos[x][y]))) ? parseInt(Datos[x][y]) : Datos[x][y];
            td.innerHTML = ca;
            $(td).css("background-color", (d[1] != "undefined")?d[1]:"#DEDEDE");
            td.style.textAlign = (d[0] == "Agente") ? "left" : "right";
            td.width = (d[0] == "Agente") ? "200px" : "";
            if ((d[0] != "Agente")) {
                valor = (!isNaN(parseInt(Datos[x][y]))) ? parseInt(Datos[x][y]) : parseFloat(Datos[x][y].replace("$", "").replace(",", ""))
                totales[i] += parseFloat(valor.toFixed(0));
            }
            i++;
            tr.appendChild(td);
        }
        body.appendChild(tr);
    }
    var trTotales = document.createElement("tr");
    var tdTotales = document.createElement("td");
    tdTotales.innerHTML = "Totales";
    trTotales.appendChild(tdTotales);
    for (i in totales) {
        if (i > 0) {
        	var tdIndicador = document.createElement("td");
        	tdIndicador.style.textAlign = "right";
            tdIndicador.innerHTML = (i == 13) ? '$' + totales[i].toFixed(2) : ((i == 11) ? Math.floor(totales[i] / Datos.length) + '%' : totales[i]);
            trTotales.appendChild(tdIndicador);
        }
    }
    body.appendChild(trTotales);
    tabla.appendChild(head);
    tabla.appendChild(body);
    tabla.border = 1;
    tabla.cellSpacing = 0;
    tabla.cellPadding = 4;
    $("#cnvResultadoActividades").append(tabla);
    $("tr", body).each(function (index, element) {
    	$("td:eq(11)", element).text($("td:eq(11)", element).text()+'%');
    });
}

function CargarTablaAvanceAgentes(AvanceAgentes) {
    var tabla = document.createElement("table");
    var thead = document.createElement("thead");
    var tbody = document.createElement("tbody");
    var encabezados = document.createElement("tr");
    
    for (x in AvanceAgentes[0]) {
        if (x != "EnMeta"){
            var th = document.createElement("th");
            th.innerHTML = x;
            encabezados.appendChild(th);
        }
    }
    
    thead.appendChild(encabezados);
    tabla.appendChild(thead);

    for (x in AvanceAgentes) {
        var tr = document.createElement("tr");
        for (y in AvanceAgentes[x]) {
            if (y != "EnMeta") {
                var td = document.createElement("td");
                td.innerHTML = AvanceAgentes[x][y];
                td.align = (y=="Agente") ? "left" : "right";
                tr.appendChild(td);
            }
            tr.className = (AvanceAgentes[x]["EnMeta"] == 1) ? "verde": "rojo";
        }
        tbody.appendChild(tr);
    }
    
    tabla.appendChild(tbody);
    
    tabla.border = 1;
    tabla.cellPadding = 4;
    tabla.cellSpacing = 0;
    
    $("#divReporteAvanceMeta").append(tabla);
}

function CargarTablaVentasCliente(VentaCliente) {
    var tabla = document.createElement("table");
    var thead = document.createElement("thead");
    var tbody = document.createElement("tbody");
    var encabezados = document.createElement("tr");

    for (x in VentaCliente[0]) {
        if (x != "EnMeta") {
            var th = document.createElement("th");
            th.innerHTML = x;
            encabezados.appendChild(th);
        }
    }

    thead.appendChild(encabezados);
    tabla.appendChild(thead);

    for (x in VentaCliente) {
        var tr = document.createElement("tr");
        for (y in VentaCliente[x]) {
            var td = document.createElement("td");
            td.innerHTML = VentaCliente[x][y];
            td.align = (y == "Cliente") ? "left" : "right";
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }

    tabla.appendChild(tbody);

    tabla.border = 1;
    tabla.cellPadding = 4;
    tabla.cellSpacing = 0;

    $("#rptReporteVentasPorCliente").html(tabla);
}

function CargarTablaAtencionClientes(AgentesClientes) {
    var tabla = document.createElement("table");
    var thead = document.createElement("thead");
    var tbody = document.createElement("tbody");
    var encabezados = document.createElement("tr");

    for (x in AgentesClientes[0]) {
        if (x != "EnMeta") {
            var th = document.createElement("th");
            th.innerHTML = x;
            $(th).css({border:"1px solid #000"});
            if (x != "Agente")
            	th.width = 50;
            if (x == "$1" || x == "$2" || x == "$3" || x == "$4") {
            	th.innerHTML = "";
            	$(th).css({ border: "none", width: 20 });
            }
            encabezados.appendChild(th);
        }
    }

    thead.appendChild(encabezados);
    tabla.appendChild(thead);

    for (x in AgentesClientes) {
        var tr = document.createElement("tr");
        for (y in AgentesClientes[x]) {
            var td = document.createElement("td");
            var Negritas = (x == 0) ? 'bold' : 'normal';
            $(td).css('font-weight',Negritas);
            td.innerHTML = AgentesClientes[x][y];
            td.align = (y == "Agente") ? "left" : "center";
            tr.appendChild(td);
            $(td).css({ border: "1px solid #000" });
            if (y == "$1" || y == "$2" || y == "$3" || y == "$4") {
            	td.innerHTML = "";
            	$(td).css({ border: "none"});
            }
        }
        tbody.appendChild(tr);
    }

    tabla.appendChild(tbody);

    tabla.cellPadding = 1;
    tabla.cellSpacing = 0;
    $(tabla).css({"font-size":"10px"});

    $("#rptAnalisisAtencionCliente").html(tabla);
}

function ResutadoUltimosMeses() {
    var request = new Object();
    request.pFechaInicio = ($('#txtFechaInicio').val() != null || $("#txtFechaInicio").val() != "") ? $("#txtFechaInicio").val() : "";
    request.pFechaFinal = ($('#txtFechaFinal').val() != null || $("#txtFechaFinal").val() != "") ? $("#txtFechaFinal").val() : "";

    var pRequest = JSON.stringify(request);
    $("#dialogGraficaMesActual").obtenerVista({
        nombreTemplate: "tmplGraficaResultadoVentasDivision.html",
        url: "Oportunidad.aspx/ObtenerGraficaResultadoVentaDivision",
        parametros: pRequest,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogGraficaMesActual").dialog("open");
            var Divisiones = pRespuesta.modelo.Divisiones;
            CargarGraficaMesActual(Divisiones);
        }
    });
}

function CargarGraficaUltimosSeisMeses(Divisiones) {

    var plot1 = $.jqplot('cnvResultadoVentasDivision', Divisiones, {
        gridPadding: { top: 5, bottom: 5, left: 0, right: 0 },
        seriesDefaults: {
            renderer: $.jqplot.PieRenderer,
            trendline: { show: false },
            rendererOptions: { padding: 0, sliceMargin: 2, showDataLabels: true }
        },
        legend: {
            show: true,
            placement: 'outside',
            rendererOptions: {
                numberRows: Divisiones[0].length
            },
            location: 'ne',
            marginTop: '0px',
            mark: 'outside'
        }
    });

}

function ObtenerMetasSucursales() {
	$.ajax({
		url: "Oportunidad.aspx/ObtenerMetasSucursales",
		type: "post",
		dataType: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				$("#metaMty").text(json.Modelo.Monterrey);
				$("#metaMex").text(json.Modelo.Mexico);
				$("#metaGdl").text(json.Modelo.Guadalajara);
				$("#logroMty").text(json.Modelo.LogroMty);
				$("#logroMex").text(json.Modelo.LogroMex);
				$("#logroGdl").text(json.Modelo.LogroGdl);
				$("#hoyMty").text(json.Modelo.hoyMty);
				$("#hoyMex").text(json.Modelo.hoyMex);
				$("#hoyGdl").text(json.Modelo.hoyGdl);
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}

