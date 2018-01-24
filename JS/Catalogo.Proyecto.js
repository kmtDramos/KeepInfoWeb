//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {

	setInterval(MantenerSesion, 150000); //2.5 minutos
	
    $(window).unload(function() {
        ActualizarPanelControles("ConceptoProyecto");
        Termino_grdProyecto();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarProyecto", function() {
        ObtenerFormaAgregarProyecto();
    });

    //
    $("#dialogGraficasProyectos").dialog({ autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $("#divTabsGraficas").tabs();
        },
        close: function() {

        }
    });

    $("#btnGraficasProyectos").click(function() {
        var oRequest = new Object();
        oRequest.pIdProyecto = 0;
        oRequest.pNombreProyecto = "";
        oRequest.pCliente = "";
        oRequest.pResponsable = "";
        oRequest.pIdDivision = -1;
        oRequest.pIdEstatusProyecto = -1;
        oRequest.pAI = 0;

        oRequest.pIdProyecto = ($("#gs_IdProyecto").val() != null && !isNaN($("#gs_IdProyecto").val()) && $("#gs_IdProyecto").val() != "") ? parseInt($("#gs_IdProyecto").val()) : 0;
        oRequest.pNobreProyecto = ($("#gs_NombreProyecto").val() != null) ? $("#gs_NombreProyecto").val() : "";
        oRequest.pCliente = ($("#gs_RazonSocial").val() != null) ? $("#gs_RazonSocial").val() : "";
        oRequest.pResponsable = ($("#gs_Responsable").val() != null) ? $("#gs_Responsable").val() : "";
        oRequest.pIdDivision = ($("#gs_IdDivision").val() != null) ? parseInt($("#gs_IdDivision").val()) : -1;
        oRequest.pIdEstatusProyecto = ($("#gs_EstatusProyecto").val() != null) ? parseInt($("#gs_EstatusProyecto").val()) : -1;
        oRequest.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : -1;

        var pRequest = JSON.stringify(oRequest);

        $("#dialogGraficasProyectos").obtenerVista({
            nombreTemplate: "tmplGraficasProyectos.html",
            url: "Proyecto.aspx/ObtenerDatosGrafica",
            parametros: pRequest,
            despuesDeCompilar: function(pRespuesta) {
                $("#dialogGraficasProyectos").dialog("open");
                var pData = pRespuesta.Modelo;
                var Programado = pData.Programado;
                var Facturado = pData.Facturado;
                var Cobrado = pData.Cobrado;
                var PorCobrar = pData.PorCobrar;
                var CostoTeorico = pData.CostoTeorico;
                var CostoReal = pData.CostoReal;
                var Diferencia = pData.Diferencia;
                var datos = [Programado, Facturado, Cobrado, PorCobrar, CostoTeorico, CostoReal, Diferencia];
                var ticks = ['Programado', 'Facturado', 'Cobrado', 'Por cobrar', 'Costo teorico', 'Costo real', 'Diferencia'];
                var plot1 = $.jqplot('cvsPie', [datos], {
                    seriesColors: ['#99CCFF', '#FFFF99', '#FFFF99', '#FFFF99', '#FFDD66', '#FFDD66', '#99FF99'],
                    seriesDefaults: { renderer: $.jqplot.BarRenderer, rendererOptions: { fillToZero: true, varyBarColor: true }, pointLabels: { show: true} },
                    axes: { xaxis: { renderer: $.jqplot.CategoryAxisRenderer, ticks: ticks }, yaxis: { pad: 1.2, tickOptions: { formatString: "$%'.2f"}} }
                });
            }
        });
    });

    $("#grdProyecto").on("click", ".imgFormaConsultarProyecto", function() {
        var registro = $(this).parents("tr");
        var Proyecto = new Object();
        Proyecto.pIdProyecto = parseInt($(registro).children("td[aria-describedby='grdProyecto_IdProyecto']").html());
        ObtenerFormaConsultarProyecto(JSON.stringify(Proyecto));
    });

    $("#grdProyecto").on("click", ".imgFormaListaSolicitudesFacturas", function() {
        var registro = $(this).parents("tr");
        var Proyecto = new Object();
        Proyecto.pIdProyecto = parseInt($(registro).children("td[aria-describedby='grdProyecto_IdProyecto']").html());
        ObtenerFormaListaSolicitudesFacturas(JSON.stringify(Proyecto));
    });

    $('#grdProyecto').one('click', '.div_grdProyecto_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdProyecto_AI']").children().attr("baja")
        var idProyecto = $(registro).children("td[aria-describedby='grdProyecto_IdProyecto']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idProyecto, baja);
    });

    $("#dialogConsultarConcepto").on("click", "#btnAgregarConceptoFactura", function() {
        var Proyecto = new Object();
        Proyecto.pIdProyecto = parseInt($(this).attr("idProyecto"));
        Proyecto.IdCliente = $("#divFormaConsultarConcepto").attr("idCliente");
        Proyecto.IdSolicitudFactura = $("#divFormaConsultarConcepto").attr("idSolicitudFactura");
        Proyecto.Solicitud = $("#txtSolicitud").val();
        var validacion = ValidaSolicitud(Proyecto);
        if (validacion != "") {
            MostrarMensajeError(validacion);
            return false;
        }
        ObtenerFormaAgregarConceptoSolicitudFactura(JSON.stringify(Proyecto));
    });

    $("#dialogEditarSolicitudFactura").on("click", "#btnAgregarConceptoFactura", function() {
        var Proyecto = new Object();
        Proyecto.pIdProyecto = parseInt($(this).attr("idProyecto"));
        Proyecto.IdCliente = $("#divFormaEditarSolicitudFactura").attr("idCliente");
        Proyecto.IdSolicitudFactura = $("#divFormaEditarSolicitudFactura").attr("idSolicitudFactura");
        Proyecto.Solicitud = $("#txtSolicitud").val();
        var validacion = ValidaSolicitud(Proyecto);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        ObtenerFormaAgregarConceptoSolicitudFactura(JSON.stringify(Proyecto));
    });

    $("#dialogConsultarSolicitudFactura").on("click", "#btnObtenerFormaAgregarSolicitudFactura", function() {
        var Proyecto = new Object();
        Proyecto.pIdProyecto = parseInt($(this).attr("idProyecto"));
        ObtenerFormaAgregarSolicitudFactura(JSON.stringify(Proyecto));
    });

    $("#dialogConsultarConcepto, #dialogEditarSolicitudFactura").on("click", ".divEliminar", function() {
        var Concepto = new Object();
        Concepto.pIdConcepto = $(this).parents("li").attr("idConcepto");
        SetEliminarConcepto(JSON.stringify(Concepto));
    });

    $("#dialogConsultarSolicitudFactura").on("click", ".divEliminarSolicitud", function() {
        var Solicitud = new Object();
        Solicitud.pIdSolicitud = $(this).parents("li").attr("idSolicitudFactura");
        SetEliminarSolicitud(JSON.stringify(Solicitud));
    });

    $("#dialogConsultarConcepto, #dialogEditarSolicitudFactura").on("click", ".lblEditarConcepto", function() {
        var Concepto = new Object();
        Concepto.pIdConcepto = $(this).parents("li").attr("idConcepto");
        ObtenerFormaEditarConceptoSolicitudProyecto(JSON.stringify(Concepto));
    });

    $("#dialogConsultarSolicitudFactura").on("click", ".lblEditarSolicitud", function() {
        var Solicitud = new Object();
        Solicitud.pIdSolicitud = $(this).parents("li").attr("idSolicitudFactura");
        ObtenerFormaEditarSolicitudFactura(JSON.stringify(Solicitud));

    });

    $("#dialogConsultarConcepto, #dialogEditarSolicitudFactura").on("dblclick", "#ulListadoConceptos li", function() {
        if ($(this).hasClass("ordenarConcepto")) {
            $(this).removeClass("ordenarConcepto");
        }
        else {
            if ($("#ulListadoConceptos li.ordenarConcepto").existe()) {
                $(this).before($("#ulListadoConceptos li.ordenarConcepto"));
                $("#ulListadoConceptos li.ordenarConcepto").removeClass("ordenarConcepto");
                ActualizarOrdenamiento();
                var Proyecto = new Object();
                Proyecto.IdSolicitudFactura = $("#divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idSolicitudFactura");
                ActualizarFormaConsultarConcepto(JSON.stringify(Proyecto));
            }
            else {
                $(this).addClass("ordenarConcepto");
            }
        }
    });

    $("#dialogConsultarSolicitudFactura").on("dblclick", "#ulListadoSolicitudFacturas li", function() {
        if ($(this).hasClass("ordenarSolicitudFactura")) {
            $(this).removeClass("ordenarSolicitudFactura");
        }
        else {
            if ($("#ulListadoSolicitudFacturas li.ordenarSolicitudFactura").existe()) {
                $(this).before($("#ulListadoSolicitudFacturas li.ordenarSolicitudFactura"));
                $("#ulListadoSolicitudFacturas li.ordenarSolicitudFactura").removeClass("ordenarSolicitudFactura");
                ActualizarOrdenamientoSolicitudFacturas();
            }
            else {
                $(this).addClass("ordenarSolicitudFactura");
            }
        }
    });

    $('#dialogAgregarConceptoSolicitudFactura').on('focusin', '#txtMonto', function(event) {
        $(this).quitarValorPredeterminado("Moneda");
    });

    $('#dialogAgregarConceptoSolicitudFactura').on('focusout', '#txtMonto', function(event) {
        $(this).valorPredeterminado("Moneda");
    });

    $('#dialogAgregarConceptoSolicitudFactura').on('keypress', '#txtMonto', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarConceptoSolicitudFactura').on('focusin', '#txtCantidadPorcentaje', function(event) {
        $(this).quitarValorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarConceptoSolicitudFactura').on('focusout', '#txtCantidadPorcentaje', function(event) {
        $(this).valorPredeterminado("Porcentaje");
    });

    $('#dialogAgregarConceptoSolicitudFactura').on('keypress', '#txtCantidadPorcentaje', function(event) {
        if (!ValidarNumeroPunto(event, $(this).val())) {
            return false;
        }
    });

    $('#dialogAgregarProyecto, #dialogEditarProyecto').on('change', '#cmbOportunidad', function(event) {
        var request = new Object();
        request.pIdOportunidad = $(this).val();
        ObtenerListaNivelInteres(JSON.stringify(request));
    });

    $('#dialogAgregarProyecto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarProyecto").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarProyecto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogAgregarConceptoSolicitudFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $('#divFormaAgregarConceptoProyecto').on('keyup', '#txtCantidadPorcentaje', function(pEvento) {
                var key = pEvento.which || pEvento.keyCode;
                if (key == 13) {
                    if (parseInt($(this).val()) > 100) {
                        $(this).val("100");
                    }
                    CalculaMontoProcentaje();
                }
            });

            if ($("#chkConversionMoneda").is(':checked')) {
                MuestraObjetos(1);
            }
            else {
                MuestraObjetos(0);
            }

            $('#divFormaAgregarConceptoProyecto').on('click', '#chkConversionMoneda', function(event) {
                if ($("#chkConversionMoneda").is(':checked')) {
                    MuestraObjetos(1);
                }
                else {
                    MuestraObjetos(0);
                }
            });
        },
        close: function() {
            $("#divFormaAgregarConceptoProyecto").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarConceptoProyecto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarProyecto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarProyecto").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogConsultarConcepto").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarConcepto").remove();
        },
        buttons: {
            "Guardar": function() {
                AgregarSolicitud();
            },
            "Cancelar": function() {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogConsultarSolicitudFactura").dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarSolicitudFactura").remove();
            $("#grdProyecto").trigger("reloadGrid");
        },
        buttons: {
            "Cerrar": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarProyecto').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarProyecto").remove();
        },
        buttons: {
            "Guardar": function() {
                EditarProyecto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        },
        open: function() {
            $('#divFormaEditarProyecto').on('change', '#txtFechaInicio, #txtFechaTermino', function(event) {
                var progreso = 0;
                var fecha = FechaCampo();
                var Hoy = new Date(fecha);
                var FechaInicio = new Date($("#txtFechaInicio").datepicker("option", "dateFormat", 'yy/mm/dd').val());
                var FechaTermino = new Date($("#txtFechaTermino").datepicker("option", "dateFormat", 'yy/mm/dd').val());

                var transcurridos = new Date(Hoy - FechaInicio);
                var periodoTotal = new Date(FechaTermino - FechaInicio);
                var diastranscurridos = transcurridos / 1000 / 60 / 60 / 24;
                var periodoTotaltranscurrido = periodoTotal / 1000 / 60 / 60 / 24;
                progreso = ((diastranscurridos * 100) / periodoTotaltranscurrido);

                var progreso = parseInt(progreso);
                progreso = FechaInicio > Hoy ? 0 : progreso;
                barraColores(progreso);

            });
        }
    });

    $('#dialogEditarConceptoSolicitudFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function() {
            $('#divFormaEditarConceptoProyecto').on('focusout', '#txtCantidadPorcentaje', function(event) {
                if (parseInt($(this).val()) > 100) {
                    $(this).val("100");
                }
                CalculaMontoProcentaje();
            });

            if ($("#chkConversionMoneda").is(':checked')) {
                MuestraObjetos(1);
            }
            else {
                MuestraObjetos(0);
            }

            $('#divFormaEditarConceptoProyecto').on('click', '#chkConversionMoneda', function(event) {
                if ($("#chkConversionMoneda").is(':checked')) {
                    MuestraObjetos(1);
                }
                else {
                    MuestraObjetos(0);
                }
            });
        },
        close: function() {
            $("#divFormaEditarConceptoProyecto").remove();
        },
        buttons: {
            "Guardar": function() {
                EditarConcepto();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogEditarSolicitudFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarSolicitudFactura").remove();
        },
        buttons: {
            "Guardar": function() {
                EditarSolicitudFactura();
            },
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });

    $("#btnSabanaProyectos").click(ObtenerSabanaProyectos);
	
    ObtenerEstatusProyecto();

});

function ObtenerSabanaProyectos() {

}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarProyecto() {
    $("#dialogAgregarProyecto").obtenerVista({
        nombreTemplate: "tmplAgregarProyecto.html",
        url: "Proyecto.aspx/ObtenerFormaAgregarProyecto",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarProyecto").dialog("open");
            $("#txtFechaInicio").datepicker();
            $("#txtFechaTermino").datepicker();
            AutocompletarCliente();
            var progreso = 0;
            $( "#progresoEnTiempo" ).progressbar({
              value: progreso
          });
          $('#divFormaAgregarProyecto').on('change', '#cmbTipoMoneda', function(event) {

          var pTipoCambio = new Object();
          pTipoCambio.IdTipoCambioOrigen = parseInt($(this).val());
          pTipoCambio.IdTipoCambioDestino = parseInt(1);
          var oRequest = new Object();
          oRequest.pTipoCambio = pTipoCambio;
          ObtenerTipoCambio(JSON.stringify(oRequest))

          });
          
        }
    });
}

function ObtenerFormaConsultarProyecto(pIdProyecto) {
    $("#dialogConsultarProyecto").obtenerVista({
        nombreTemplate: "tmplConsultarProyecto.html",
        url: "Proyecto.aspx/ObtenerFormaConsultarProyecto",
        parametros: pIdProyecto,
        despuesDeCompilar: function(pRespuesta) {
            if (pRespuesta.modelo.Permisos.puedeEditarProyecto == 1) {
                $("#dialogConsultarProyecto").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Proyecto = new Object();
                        Proyecto.IdProyecto = parseInt($("#divFormaConsultarProyecto").attr("IdProyecto"));
                        ObtenerFormaEditarProyecto(JSON.stringify(Proyecto))
                    }
                });
                $("#dialogConsultarProyecto").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarProyecto").dialog("option", "buttons", {});
                $("#dialogConsultarProyecto").dialog("option", "height", "350");
            }
            $("#dialogConsultarProyecto").dialog("open");
            
            var progreso = parseInt(pRespuesta.modelo.Progreso);
            
            barraColores(progreso);            
              
        }
    });
}

function ObtenerFormaAgregarConcepto(pRequest) {
    $("#dialogConsultarConcepto").obtenerVista({
        nombreTemplate: "tmplAgregarSolicitudFactura.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaAgregarConcepto",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarConcepto").dialog("open");
        }
    });
}

function ObtenerFormaAgregarSolicitudFactura(pRequest) {
    $("#dialogConsultarConcepto").obtenerVista({
        nombreTemplate: "tmplAgregarSolicitudFactura.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaAgregarSolicitudFactura",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarConcepto").dialog("open");
            AutocompletarClienteEmpresarial();
        }
    });
}

function ObtenerFormaListaSolicitudesFacturas(pRequest) {
    $("#dialogConsultarSolicitudFactura").obtenerVista({
        nombreTemplate: "tmplConsultarListaSolicitudesFacturas.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaListaSolicitudesFacturas",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConsultarSolicitudFactura").dialog("open");
            AutocompletarClienteEmpresarial();
        }
    });
}

function ActualizarFormaConsultarConcepto(pRequest) {
        $("#divListaConceptos").obtenerVista({
        nombreTemplate: "tmplConsultarConceptoSolicitud.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaAgregarConcepto",
        despuesDeCompilar: function (Resultado) {
        	$("li.liConcepto", "#ulListadoConceptos").click(function () {
        		var Concepto = new Object();
        		Concepto.pIdConcepto = parseInt($(this).attr("idConcepto"));
        		ObtenerFormaEditarConceptoSolicitudProyecto(JSON.stringify(Concepto));
        	});
        }
    });
}

function ActualizarFormaConsultarSolicitudFacturaProyecto(pRequest) {
    $("#divListaSolicitudFacturas").obtenerVista({
        nombreTemplate: "tmplConsultarSolicitudFacturaProyecto.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaListaSolicitudesFacturas",
        despuesDeCompilar: function () {
        	$("#txtMonto", "#divListaSolicitudFacturas").valorPredeterminado("Moneda");
        }
    });
}

function ActualizarFormaConsultarSolicitudFactura(pRequest) {
    $("#dialogConsultarSolicitudFactura").obtenerVista({
        nombreTemplate: "tmplConsultarSolicitudFactura.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaAgregarSolicitudFactura"
    });
}

function ObtenerFormaEditarConceptoSolicitudProyecto(pConcepto) {
    $("#dialogEditarConceptoSolicitudFactura").obtenerVista({
    	nombreTemplate: "tmplEditarConceptoProyecto.html",
        parametros: pConcepto,
        url: "Proyecto.aspx/ObtenerFormaEditarConceptoSolicitudFactura",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarConceptoSolicitudFactura").dialog("open");
        }       
    });
}

function ObtenerFormaEditarSolicitudFactura(pSolicitud) {
    $("#dialogEditarSolicitudFactura").obtenerVista({
        nombreTemplate: "tmplEditarSolicitudFactura.html",
        parametros: pSolicitud,
        url: "Proyecto.aspx/ObtenerFormaEditarSolicitudFactura",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarSolicitudFactura").dialog("open");
            AutocompletarClienteEmpresarial();
        }
    });
}

function ObtenerFormaAgregarConceptoSolicitudFactura(pRequest) {
    $("#dialogAgregarConceptoSolicitudFactura").obtenerVista({
        nombreTemplate: "tmplAgregarConceptoSolicitudProyecto.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerFormaAgregarConceptoSolicitudFactura",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarConceptoSolicitudFactura").dialog("open");
            $("#cmbTipoVenta option[value=4]").attr("selected", true);
            $("#cmbTipoMoneda option[value=1]").attr("selected", true);  
        }
    });
}

function ObtenerFormaEditarProyecto(IdProyecto) {
    $("#dialogEditarProyecto").obtenerVista({
        nombreTemplate: "tmplEditarProyecto.html",
        url: "Proyecto.aspx/ObtenerFormaEditarProyecto",
        parametros: IdProyecto,
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogEditarProyecto").dialog("open");
            $("#txtFechaInicio").datepicker();
            $("#txtFechaTermino").datepicker();
            AutocompletarCliente();
            var progreso = parseInt(pRespuesta.modelo.Progreso);
            barraColores(progreso);

            $('#divFormaEditarProyecto').on('change', '#cmbTipoMoneda', function(event) {
                var pTipoCambio = new Object();
                pTipoCambio.IdTipoCambioOrigen = parseInt($(this).val());
                pTipoCambio.IdTipoCambioDestino = parseInt(1);
                var oRequest = new Object();
                oRequest.pTipoCambio = pTipoCambio;
                ObtenerTipoCambio(JSON.stringify(oRequest))
            });
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarProyecto() {
    var pProyecto = new Object();
    pProyecto.NombreProyecto = $("#txtNombreProyecto").val();
    pProyecto.IdCliente = $("#divFormaAgregarProyecto").attr("idCliente");
    pProyecto.FechaInicio = $("#txtFechaInicio").val();
    pProyecto.FechaTermino = $("#txtFechaTermino").val();
    pProyecto.PrecioTeorico = $("#txtPrecioTeorico").val();
    pProyecto.CostoTeorico = $("#txtCostoTeorico").val();
    pProyecto.Notas = $("#txtNotas").val();
    pProyecto.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProyecto.TipoCambio = $("#txtTipoCambio").val();
    pProyecto.IdUsuarioResponsable = $("#cmbUsuarioResponsable").val();
    pProyecto.IdDivision = $("#cmbDivision").val();
    pProyecto.IdOportunidad = $("#cmbOportunidad").val();
    pProyecto.IdNivelInteres = $("#cmbNivelInteres").val();
    pProyecto.CostoProducto = $("#CostoProducto").val();
    pProyecto.CostoManoObra = $("#CostoManoObra").val();
    
    var validacion = ValidaProyecto(pProyecto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProyecto = pProyecto;
    SetAgregarProyecto(JSON.stringify(oRequest));
}

function AgregarConceptoProyecto() {
    var pProyecto = new Object();
    pProyecto.IdProyecto = $("#divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idProyecto");
    pProyecto.IdCliente = $("#divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idCliente");
    pProyecto.Solicitud = $("#txtSolicitud").val();
    pProyecto.IdSolicitudFactura = $("#divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idSolicitudFactura");
    pProyecto.NombreConcepto = $("#txtNombreConcepto").val();
    pProyecto.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    pProyecto.IdTipoVenta = $("#cmbTipoVenta").val();
    pProyecto.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProyecto.Monto = $("#txtMonto").val();
    pProyecto.Cantidad = parseFloat($("#txtCantidad").val());
    pProyecto.IdTipoIVA = $("#cmbTipoIVA").val();
    pProyecto.ClaveProdServ = $("#txtClaveProdServ").val();

    pProyecto.Monto = pProyecto.Monto.replace("$", "");
    pProyecto.Monto = pProyecto.Monto.split(",").join("");
    pProyecto.Monto = parseFloat(pProyecto.Monto);
    
    if ($("#chkConversionMoneda").is(':checked')) {
        pProyecto.ConversionMoneda = 1;
        pProyecto.TipoCambioConversion = $("#txtTipoCambioConversion").val();
        pProyecto.IdTipoMonedaConversion = $("#cmbTipoMonedaConversion").val();
    }
    else {
        pProyecto.ConversionMoneda = 0;
    }
    
    var validacion = ValidaConceptoProyecto(pProyecto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProyecto = pProyecto;
    SetAgregarConceptoProyecto(JSON.stringify(oRequest));
}

function ActualizarOrdenamiento() {
    var pConcepto = new Object();
    pConcepto.IdSolicitudFactura = $("#divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idSolicitudFactura");

    pConcepto.Conceptos = new Array();
    $("#ulListadoConceptos li").each(function() {
        pConcepto.Conceptos.push($(this).attr("idConcepto"));
    });
    
    var oRequest = new Object();
    oRequest.pConcepto = pConcepto;
    SetActualizarOrdenamiento(JSON.stringify(oRequest));
}

function ActualizarOrdenamientoSolicitudFacturas() {
    var pSolicitud = new Object();
    pSolicitud.IdProyecto = $("#divFormaConsultarSolicitudFactura").attr("idProyecto");

    pSolicitud.Solicitudes = new Array();
    $("#ulListadoSolicitudFacturas li").each(function() {
        pSolicitud.Solicitudes.push($(this).attr("idSolicitudFactura"));
    });

    var oRequest = new Object();
    oRequest.pSolicitud = pSolicitud;
    SetActualizarOrdenamientoSolicitudFacturas(JSON.stringify(oRequest));
}

function AgregarSolicitud() {
    var pProyecto = new Object();
    pProyecto.IdProyecto = $("#divFormaConsultarConcepto").attr("idProyecto");
    pProyecto.IdCliente = $("#divFormaConsultarConcepto").attr("idCliente");
    pProyecto.Solicitud = $("#txtSolicitud").val();
    pProyecto.IdSolicitudFactura = $("#divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idSolicitudFactura");
    var validacion = ValidaSolicitud(pProyecto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProyecto = pProyecto;
    SetAgregarSolicitud(JSON.stringify(oRequest));
}

function SetAgregarProyecto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/AgregarProyecto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProyecto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarProyecto").dialog("close");
        }
    });
}

function SetAgregarConceptoProyecto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/AgregarConceptoProyecto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                var Proyecto = new Object();
                Proyecto.IdSolicitudFactura = respuesta.IdSolicitudFactura;
                ActualizarFormaConsultarConcepto(JSON.stringify(Proyecto));
                $("#divFormaConsultarConcepto").attr("idsolicitudfactura", respuesta.IdSolicitudFactura);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarConceptoSolicitudFactura").dialog("close");
        }
    });
}

function SetActualizarOrdenamiento(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/ActualizarOrdenamiento",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetActualizarOrdenamientoSolicitudFacturas(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/ActualizarOrdenamientoSolicitudFacturas",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            var Proyecto = new Object();
            Proyecto.pIdProyecto = $("#divFormaConsultarSolicitudFactura").attr("idProyecto");
            ActualizarFormaConsultarSolicitudFacturaProyecto(JSON.stringify(Proyecto));
        }
    });
}

function SetAgregarSolicitud(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/AgregarSolicitud",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                var Proyecto = new Object();
                Proyecto.pIdProyecto = respuesta.IdProyecto
                ObtenerFormaListaSolicitudesFacturas(JSON.stringify(Proyecto));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
        OcultarBloqueo();
            $("#dialogConsultarConcepto").dialog("close");
        }
    });
}

function SetCambiarEstatus(pIdProyecto, pBaja) {
    var pRequest = "{'pIdProyecto':" + pIdProyecto + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdProyecto").trigger("reloadGrid");
        },
        complete: function() {
            $('#grdProyecto').one('click', '.div_grdProyecto_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdProyecto_AI']").children().attr("baja")
                var idProyecto = $(registro).children("td[aria-describedby='grdProyecto_IdProyecto']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idProyecto, baja);
            });
        }
    });
}

function EditarProyecto() {
    var pProyecto = new Object();
    pProyecto.IdProyecto = $("#divFormaEditarProyecto").attr("idProyecto");
    pProyecto.NombreProyecto = $("#txtNombreProyecto").val();
    pProyecto.IdCliente = $("#divFormaEditarProyecto").attr("idCliente");
    pProyecto.FechaInicio = $("#txtFechaInicio").val();
    pProyecto.FechaTermino = $("#txtFechaTermino").val();
    pProyecto.PrecioTeorico = $("#txtPrecioTeorico").val();
    pProyecto.CostoTeorico = $("#txtCostoTeorico").val();
    pProyecto.Notas = $("#txtNotas").val();
    pProyecto.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pProyecto.TipoCambio = $("#txtTipoCambio").val();
    pProyecto.IdUsuarioResponsable = $("#cmbUsuarioResponsable").val();
    pProyecto.IdOportunidad = $("#cmbOportunidad").val();
    pProyecto.IdNivelInteres = $("#cmbNivelInteres").val();
    pProyecto.IdEstatusProyecto = $("#cmbEstatus").val();
    pProyecto.IdDivision = $("#cmbDivision").val();
    pProyecto.CostoProducto = $("#CostoProducto").val();
    pProyecto.CostoManoObra = $("#CostoManoObra").val();
    
    var validacion = ValidaProyecto(pProyecto);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pProyecto = pProyecto;
    SetEditarProyecto(JSON.stringify(oRequest));
}

function EditarConcepto() {
    var Concepto = new Object();
    Concepto.IdConcepto = $("#divFormaEditarConceptoProyecto").attr("idConcepto");
    Concepto.NombreConcepto = $("#divFormaEditarConceptoProyecto #txtNombreConcepto").val();
    Concepto.IdProyecto = $("#divFormaEditarConceptoProyecto").attr("idProyecto");
    Concepto.IdUnidadCompraVenta = $("#cmbUnidadCompraVenta").val();
    Concepto.IdTipoVenta = $("#cmbTipoVenta").val();
    Concepto.IdTipoMoneda = $("#cmbTipoMoneda").val();
    Concepto.Monto = $("#txtMonto").val();
    Concepto.Cantidad = $("#txtCantidad").val();
    Concepto.IdTipoIVA = $("#cmbTipoIVA").val();
    Concepto.ClaveProdServ = $("#txtClaveProdServ").val();
    

    if ($("#chkConversionMoneda").is(':checked')) {
        Concepto.ConversionMoneda = 1;
        Concepto.TipoCambioConversion = $("#txtTipoCambioConversion").val();
        Concepto.IdTipoMonedaConversion = $("#cmbTipoMonedaConversion").val();
    }
    else {
        Concepto.ConversionMoneda = 0;
    }

    var validacion = ValidaConceptoProyecto(Concepto);
    if (validacion != "") {
        MostrarMensajeError(validacion); return false;
    }
    var oRequest = new Object();
    oRequest.pConcepto = Concepto;

    SetEditarConceptoProyecto(JSON.stringify(oRequest));

}

function EditarSolicitudFactura() {

    var pSolicitud = new Object();
    pSolicitud.IdSolicitudFactura = $("#divFormaEditarSolicitudFactura").attr("idSolicitudFactura");
    pSolicitud.IdCliente = $("#divFormaEditarSolicitudFactura").attr("idCliente");
    pSolicitud.IdProyecto = $("#divFormaEditarSolicitudFactura").attr("idProyecto");
    pSolicitud.Solicitud = $("#txtSolicitud").val();
    var validacion = ValidaSolicitud(pSolicitud);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pSolicitud = pSolicitud;
    SetEditarSolicitudFactura(JSON.stringify(oRequest));
}

function SetEditarProyecto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/EditarProyecto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdProyecto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarProyecto").dialog("close");
        }
    });
}

function SetEditarConceptoProyecto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/EditarConceptoProyecto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            var Proyecto = new Object();
            Proyecto.IdSolicitudFactura = respuesta.IdSolicitudFactura;
            ActualizarFormaConsultarConcepto(JSON.stringify(Proyecto));
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarConceptoSolicitudFactura").dialog("close");
        }
    });
}

function SetEditarSolicitudFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/EditarSolicitudFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Proyecto = new Object();
                Proyecto.pIdProyecto = respuesta.IdProyecto
                ActualizarFormaConsultarSolicitudFacturaProyecto(JSON.stringify(Proyecto));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarSolicitudFactura").dialog("close");
        }
    });
}

function SetEliminarConcepto(pConcepto) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/EliminarConcepto",
        data: pConcepto,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            var Proyecto = new Object();
            Proyecto.IdSolicitudFactura = respuesta.IdSolicitudFactura;
            ActualizarFormaConsultarConcepto(JSON.stringify(Proyecto));
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function SetEliminarSolicitud(pSolicitud) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/EliminarSolicitud",
        data: pSolicitud,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Proyecto = new Object();
                Proyecto.pIdProyecto = respuesta.IdProyecto;
                ActualizarFormaConsultarSolicitudFacturaProyecto(JSON.stringify(Proyecto));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
                
            },
            complete: function() {
                OcultarBloqueo();
            }
    });
}

function CalculaMontoProcentaje() {
    var pProyecto = new Object();
    pProyecto.IdProyecto = $("#divFormaAgregarConceptoProyecto, #divFormaEditarConceptoProyecto").attr("idProyecto");
    pProyecto.CantidadProcentaje = $("#txtCantidadPorcentaje").val();
    if (pProyecto.CantidadProcentaje == "") {
        pProyecto.CantidadProcentaje = 0;
    }
    var oRequest = new Object();
    oRequest.pProyecto = pProyecto;
    SetCalculaMontoProcentaje(JSON.stringify(oRequest));
}

function SetCalculaMontoProcentaje(pProyecto) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/CalculaMontoPorcentaje",
        data: pProyecto,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            $("#txtMonto").val(respuesta.CantidadProcentaje);
            $("#txtMonto").valorPredeterminado("Moneda");
            $("#cmbTipoMoneda option[value=" + respuesta.IdTipoMoneda + "]").attr("selected", true);            
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function MuestraObjetos(opcion) {
    if (opcion == 1) {
        $("#TipoCambioConversion").css("display", "block");
        $("#TipoMonedaConversion").css("display", "block");
        $("#txtTipoCambioConversion").css("display", "block");
        $("#cmbTipoMonedaConversion").css("display", "block");
    }
    else {
        $("#TipoCambioConversion").css("display", "none");
        $("#TipoMonedaConversion").css("display", "none");
        $("#txtTipoCambioConversion").css("display", "none");
        $("#cmbTipoMonedaConversion").css("display", "none");
    }
}

function AutocompletarCliente() {
    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtRazonSocial').val();
            $.ajax({
                type: 'POST',
                url: 'Proyecto.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarProyecto, #divFormaEditarProyecto, #divFormaAgregarConceptoProyecto").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarProyecto, #divFormaEditarProyecto, #divFormaAgregarConceptoProyecto").attr("idCliente", pIdCliente);
            var request = new Object();
            request.pIdCliente = pIdCliente;
            ObtenerListaOportunidades(JSON.stringify(request));
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarClienteEmpresarial() {

    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {

            var pProyecto = new Object();
            pProyecto.pRazonSocial = $('#txtRazonSocial').val();
            pProyecto.pIdCliente = $("#divFormaAgregarConceptoProyecto, #divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idCliente");
            pProyecto.pIdClienteProyecto = $("#divFormaAgregarConceptoProyecto, #divFormaConsultarConcepto, #divFormaEditarSolicitudFactura").attr("idClienteProyecto");
            var pRequest = new Object();
            pRequest.pProyecto = pProyecto;
            $.ajax({
                type: 'POST',
                url: 'Proyecto.aspx/BuscarRazonSocialClienteEmpresarial',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarConceptoProyecto, #divFormaConsultarConcepto, #divFormaConsultarSolicitudFactura").attr("idCliente", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarConceptoProyecto, #divFormaConsultarConcepto, #divFormaConsultarSolicitudFactura").attr("idCliente", pIdCliente);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

//-----Validaciones------------------------------------------------------
function ValidaProyecto(pProyecto) {
    var errores = "";

    if (pProyecto.NombreProyecto == "")
    { errores = errores + "<span>*</span> El campo nombre del proyecto esta vacío, favor de capturarlo.<br />"; }

    if (pProyecto.IdCliente == 0)
    { errores = errores + "<span>*</span> Debe de seleccionar un cliente de la lista, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdOportunidad == 0)
    { errores = errores + "<span>*</span> Debe de seleccionar una oportunidad, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdNivelInteres == 0)
    { errores = errores + "<span>*</span> Debe de seleccionar un nivel interes, favor de seleccionarlo.<br />"; }

    if (pProyecto.FechaInicio == "")
    { errores = errores + "<span>*</span> El campo fecha de inicio esta vacío, favor de capturarlo.<br />"; }

    if (pProyecto.FechaTermino == "")
    { errores = errores + "<span>*</span> El campo fecha de termino esta vacío, favor de capturarlo.<br />"; }

    if (pProyecto.CostoTeorico == 0)
    { errores = errores + "<span>*</span> El campo costo teorico esta vacío, favor de capturarlo.<br />"; }

    if (pProyecto.PrecioTeorico == 0)
    { errores = errores + "<span>*</span> El campo precio teorico esta vacío, favor de capturarlo.<br />"; }

    if (parseFloat(pProyecto.PrecioTeorico) <= parseFloat(pProyecto.CostoTeorico))
    { errores = errores + "<span>*</span> El costo del proyecto no puede ser mayor o igual al precio, debe tener una ganancia.<br />"; }

    if (pProyecto.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacío, favor de seleccionarlo.<br />"; }

    if (pProyecto.TipoCambio == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacío, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdTipoMoneda == 1 && pProyecto.TipoCambio != 1)
    { errores = errores + "<span>*</span> La moneda pesos no puede tener un tipo de cambio distinto de 1.<br />"; }

    if (pProyecto.IdUsuarioResponsable == 0)
    { errores = errores + "<span>*</span> El campo usuario responsable esta vacío, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdDivision == 0 || pProyecto.IdDivision ==  "")
    { errores = errores + "<span>*</span> La especialidad esta vacia, favor de seleccionarla.<br />"; }

    if (pProyecto.Notas == "")
    { errores = errores + "<span>*</span> El campo notas esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaConceptoProyecto(pProyecto) {
    var errores = "";

    if (pProyecto.IdProyecto == 0)
    { errores = errores + "<span>*</span> El campo proyecto esta vacio, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdCliente == 0)
    { errores = errores + "<span>*</span> El campo cliente esta vacio, favor de seleccionarlo.<br />"; }

    if (pProyecto.NombreConcepto == "")
    { errores = errores + "<span>*</span> El campo nombre del concepto esta vacío, favor de capturarlo.<br />"; }

    if (pProyecto.ClaveProdServ == "")
    { errores = errores + "<span>*</span> La Clave (SAT) esta vacío, favor de capturarlo.<br />"; }

    if (pProyecto.IdUnidadCompraVenta == 0)
    { errores = errores + "<span>*</span> El campo unidad de compra venta esta vacio, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdTipoVenta == 0)
    { errores = errores + "<span>*</span> El campo tipo de venta esta vacio, favor de seleccionarlo.<br />"; }

    if (pProyecto.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (pProyecto.Cantidad == 0)
    { errores = errores + "<span>*</span> El campo cantidad esta vacio, favor de seleccionarlo.<br />"; }

    if (pProyecto.ConversionMoneda == 1) {
        if (pProyecto.TipoCambioConversion == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio de la conversion esta vacio, favor de capturarlo.<br />"; }

        if (pProyecto.IdTipoMonedaConversion == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda de conversion esta vacio, favor de seleccionarlo.<br />"; }
    }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;   
}

function ValidaSolicitud(pProyecto) {
    var errores = "";

    if (pProyecto.IdProyecto == 0)
    { errores = errores + "<span>*</span> Debe de seleccionar un proyecto, elegirlo de la lista.<br />"; }

    if (pProyecto.IdCliente == 0)
    { errores = errores + "<span>*</span> Debe de seleccionar un cliente, elegirlo de la lista.<br />"; }

    if (pProyecto.Solicitud == "")
    { errores = errores + "<span>*</span> El campo solicitud esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ObtenerListaUnidadCompraVenta() {
    $("#cmbUnidadCompraVenta").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Proyecto.aspx/ObtenerListaUnidadCompraVenta"
    });
}

function ObtenerListaOportunidades(pRequest) {
    $("#cmbOportunidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerListaOportunidad"
    });
}

function ObtenerListaOportunidad() {
    var request = new Object();
    request.pIdCliente = $("#divFormaAgregarProyecto, #divFormaEditarProyecto").attr("idCliente");
    if (request.pIdCliente == "") {
        request.pIdCliente = "0";
    }

    $("#cmbOportunidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        parametros: JSON.stringify(request),
        url: "Proyecto.aspx/ObtenerListaOportunidad",
        despuesDeCompilar: function() {
            var request = new Object();
            request.pIdOportunidad = 0;
            ObtenerListaNivelInteres(JSON.stringify(request));
        }
    });
}

function ObtenerListaNivelInteres(pRequest) {
    $("#cmbNivelInteres").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        parametros: pRequest,
        url: "Proyecto.aspx/ObtenerListaNivelInteres",
        despuesDeCompilar: function (Respuesta) {
        	console.log(Respuesta);
        }
    });
}

function barraColores(valor){           
    if (valor >= 100){
        $("#barColor").css({ "background": "Red", "width": valor+'%'});
    } else if (valor > 80 && valor < 99){
        $("#barColor").css({ "background": "Orange", "width": valor+'%' });
    } else if (valor > 50 && valor < 79){
        $("#barColor").css({ "background": "Yellow", "width": valor+'%' });
    } else if (valor > 1 && valor < 49){
        $("#barColor").css({ "background": "LightGreen", "width": valor+'%' });
    } else {
        $("#barColor").css({ "background": "white", "width": valor+'%' });
    }
    
    valor = valor > 100 ? 100 : valor;
    $( "#progresoEnTiempo" ).progressbar({
    value: valor
    });   

    $("#spanPorcientoProgreso").html(valor);
    
    //regresar el formato
    new Date ($("#txtFechaInicio, #txtFechaTermino" ).datepicker( "option", "dateFormat", 'dd/mm/yy' ).val());
}

function ObtenerTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Proyecto.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.Modelo.TipoCambioActual == 0) {
                    $("#txtTipoCambio").val(1);
                }
                else {
                    $("#txtTipoCambio").val(respuesta.Modelo.TipoCambioActual);
                }
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function FiltroProyecto() {
    var oRequest = new Object();
    oRequest.pTamanoPaginacion = 10;
    oRequest.pPaginaActual = 1;
    oRequest.pColumnaOrden = "IdProyecto";
    oRequest.pTipoOrden = "DESC";
    oRequest.pIdProyecto = "";
    oRequest.pIdOportunidad = "";
    oRequest.pIdTipoMoneda = -1;
    oRequest.pNombreProyecto = "";
    oRequest.pRazonSocial = "";
    oRequest.pResponsable = "";
    oRequest.pIdDivision = -1;
    oRequest.pEstatusProyecto = "1";
    oRequest.pAI = 0;

    oRequest.pTamanoPaginacion = ($('#grdProyecto').getGridParam('rowNum') != null) ? $('#grdProyecto').getGridParam('rowNum') : 10;
    oRequest.pPaginaActual = ($('#grdProyecto').getGridParam('page') != null) ? $('#grdProyecto').getGridParam('page') : 1;
    oRequest.pColumnaOrden = ($('#grdProyecto').getGridParam('sortname') != null) ? $('#grdProyecto').getGridParam('sortname') : "IdProyecto";
    oRequest.pTipoOrden = ($('#grdProyecto').getGridParam('sortorder') != null) ? $('#grdProyecto').getGridParam('sortorder') : "desc";
    oRequest.pIdProyecto = ($("#gs_IdProyecto").val() != null) ? $("#gs_IdProyecto").val() : "";
    oRequest.pIdOportunidad = ($("#gs_IdOportunidad").val() != null) ? $("#gs_IdOportunidad").val() : "";
    oRequest.pIdTipoMoneda = ($("#gs_Moneda").val() != null) ? $("#gs_Moneda").val() : -1;
    oRequest.pNombreProyecto = ($("#gs_NombreProyecto").val() != null) ? $("#gs_NombreProyecto").val() : "";
    oRequest.pRazonSocial = ($("#gs_RazonSocial").val() != null) ? $("#gs_RazonSocial").val() : "";
    oRequest.pResponsable = ($("#gs_Responsable").val() != null) ? $("#gs_Responsable").val() : "";
    oRequest.pIdDivision = ($("#gs_IdDivision").val() != null) ? parseInt($("#gs_IdDivision").val()) : -1;
    oRequest.pEstatusProyecto = ($("#cmbEstatsuProyecto").multipleSelect('getSelects') + "" != null) ? $("#cmbEstatsuProyecto").multipleSelect('getSelects') + "" : "1";
    oRequest.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : 0;

    var pRequest = JSON.stringify(oRequest);
    
    $.ajax({
        url: "Proyecto.aspx/ObtenerProyecto",
        type: "post",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdProyecto')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function Termino_grdProyecto() {
    var oRequest = new Object();
    oRequest.pIdProyecto = 0;
    oRequest.pNombreProyecto = "";
    oRequest.pCliente = "";
    oRequest.pResponsable = "";
    oRequest.pIdDivision = -1;
    oRequest.pIdEstatusProyecto = -1;
    oRequest.pAI = 0;

    oRequest.pIdProyecto = ($("#gs_IdProyecto").val() != null && !isNaN($("#gs_IdProyecto").val()) && $("#gs_IdProyecto").val()!="") ? parseInt($("#gs_IdProyecto").val()) : 0;
    oRequest.pNobreProyecto = ($("#gs_NombreProyecto").val() != null) ? $("#gs_NombreProyecto").val() : "";
    oRequest.pCliente = ($("#gs_RazonSocial").val() != null) ? $("#gs_RazonSocial").val() : "";
    oRequest.pResponsable = ($("#gs_Responsable").val() != null) ? $("#gs_Responsable").val() : "";
    oRequest.pIdDivision = ($("#gs_IdDivision").val() != null) ? parseInt($("#gs_IdDivision").val()) : -1;
    oRequest.pIdEstatusProyecto = ($("#gs_EstatusProyecto").val() != null) ? parseInt($("#gs_EstatusProyecto").val()) : -1;
    oRequest.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : -1;
    
    var pRequest = JSON.stringify(oRequest);

    $.ajax({
        url: "Proyecto.aspx/ObtenerTotalProyectos",
        type: "post",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            //
            var Totales = JSON.parse(pRespuesta.d);
            $("#lblTotalProyecto").text(Totales.Modelo.Proyectos);
            $("#lblProgramado").text(Totales.Modelo.Programado);
            $("#lblFacturado").text(Totales.Modelo.Facturado);
            $("#lblPorCobrar").text(Totales.Modelo.PorCobrar);
            $("#lblCobrado").text(Totales.Modelo.Cobrado);
            $("#lblCostoTeorico").text(Totales.Modelo.CostoTeorico);
            $("#lblCostoReal").text(Totales.Modelo.CostoReal);
            $("#lblDiferencia").text(Totales.Modelo.Diferencia);
        }
    });
    
}

function VerFacturas(IdProyecto) {
	var Facturas = new Object();
	Facturas.IdProyecto = parseInt(IdProyecto);

	var Request = JSON.stringify(Facturas);
	

}

function Filtro_FacturasProyecto() {

}

function ObtenerEstatusProyecto() {
	$.ajax({
		url: "Proyecto.aspx/ObtenerEstatusProyecto",
		type: "post",
		datatype: "json",
		contentType: "application/json; charset=utf-8",
		success: function (Respuesta) {
			var json = JSON.parse(Respuesta.d);
			if (json.Error == 0)
			{
				var Estatus = json.Modelo.Estatus
				for(x in Estatus)
				{
					$("#cmbEstatsuProyecto").append($('<option value="' + Estatus[x].Valor + '">' + Estatus[x].Descripcion + '</option>'));
				}
				$("#cmbEstatsuProyecto").multipleSelect().change(FiltroProyecto).multipleSelect("setSelects", JSON.parse("[" + json.Modelo.EstatusUsuario + "]"));
				Inicializar_grdProyecto();
				FiltroProyecto();
			}
			else
			{
				MostrarMensajeError(json.Descripcion);
			}
		}
	});
}