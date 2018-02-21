//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("CuentasPorCobrar");
    });
    
    ObtenerFormaFiltrosCuentasPorCobrar();
    
    //-----Funcion del Grid
    $("#gbox_grdCuentasPorCobrar").livequery(function() {
        $("#grdCuentasPorCobrar").jqGrid('navButtonAdd', '#pagCuentasPorCobrar', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {
                var pRazonSocial = "";
                var pFolio = "";
                var pAI = 0;
                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;

                if ($('#gs_Folio').val() != null) { pFolio = $("#gs_Folio").val(); }

                if ($('#gs_RazonSocial').val() != null) { pRazonSocial = $("#gs_RazonSocial").val(); }

                if ($('#gs_AI').val() != null) { pAI = $("#gs_AI").val(); }

                if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

                    if ($("#chkPorFecha").is(':checked')) {
                        pPorFecha = 1;
                    }
                    else {
                        pPorFecha = 0;
                    }

                    pFechaInicial = $("#txtFechaInicial").val();
                    pFechaInicial = ConvertirFecha(pFechaInicial, 'aaaammdd');
                }
                if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                    pFechaFinal = $("#txtFechaFinal").val();
                    pFechaFinal = ConvertirFecha(pFechaFinal, 'aaaammdd');
                }

                $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                    IsExportExcel: true,
                    pRazonSocial: pRazonSocial,
                    pFolio: pFolio,
                    pAI: pAI,
                    pFechaInicial: pFechaInicial,
                    pFechaFinal: pFechaFinal,
                    pPorFecha: pPorFecha

                }, downloadType: 'Normal'
                });

            }
        });
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCuentasPorCobrar", function() {
        ObtenerFormaAgregarCuentasPorCobrar();
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaConciliarCuentasPorCobrar", function() {
        ObtenerFormaConciliarCuentasPorCobrar();
    });

    $("#dialogEditarCuentasPorCobrar, #dialogAgregarCuentasPorCobrar").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var CuentasPorCobrar = new Object();

        if ($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar") != null && $("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar") != "") {
            CuentasPorCobrar.pIdCuentasPorCobrar = parseInt($("#divFormaEditarCuentasPorCobrar,#divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar"));
            CuentasPorCobrar.pIdCliente = parseInt($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCliente"));
            CuentasPorCobrar.IdTipoMoneda = $("#cmbTipoMoneda").val();
            CuentasPorCobrar.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
                if(CuentasPorCobrar.IdTipoMoneda == 2)
                {
                    CuentasPorCobrar.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    CuentasPorCobrar.TipoCambio = "1";
                }

                if (CuentasPorCobrar.TipoCambio.replace(" ", "") == "") {
                    CuentasPorCobrar.TipoCambio = 0;
                }
                
                if (CuentasPorCobrar.TipoCambioDOF.replace(" ", "") == "") {
                    CuentasPorCobrar.TipoCambioDOF = 0;
                }
            
            var validacion = ValidaAsociacionDocumentos(CuentasPorCobrar);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.pCuentasPorCobrar = CuentasPorCobrar;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
        }
        else {
            AgregarCuentasPorCobrarEdicion();
        }
    });

    $("#grdCuentasPorCobrar").on("click", ".imgFormaConsultarCuentasPorCobrar", function() {
        var registro = $(this).parents("tr");
        var CuentasPorCobrar = new Object();
        CuentasPorCobrar.pIdCuentasPorCobrar = parseInt($(registro).children("td[aria-describedby='grdCuentasPorCobrar_IdCuentasPorCobrar']").html());
        ObtenerFormaConsultarCuentasPorCobrar(JSON.stringify(CuentasPorCobrar));
    });

    $("#grdCuentaBancaria").on("click", ".imgSeleccionarCuentaBancaria", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");

    });

    $("#grdMovimientosCobros").on("click", ".imgEliminarMovimiento", function() {

        var registro = $(this).parents("tr");
        var pCuentasPorCobrarEncabezadoFactura = new Object();
        pCuentasPorCobrarEncabezadoFactura.pIdCuentasPorCobrarEncabezadoFactura = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdCuentasPorCobrarEncabezadoFactura']").html());
        var oRequest = new Object();
        oRequest.pCuentasPorCobrarEncabezadoFactura = pCuentasPorCobrarEncabezadoFactura;

        console.log(pCuentasPorCobrarEncabezadoFactura);
        SetEliminarCuentasPorCobrarEncabezadoFactura(JSON.stringify(oRequest));
    });

    $("#grdCuentaBancaria").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");
    });

    $('#grdCuentasPorCobrar').one('click', '.div_grdCuentasPorCobrar_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCuentasPorCobrar_AI']").children().attr("baja")
        var idCuentasPorCobrar = $(registro).children("td[aria-describedby='grdCuentasPorCobrar_IdCuentasPorCobrar']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCuentasPorCobrar, baja);
    });

    $('#dialogAgregarCuentasPorCobrar, #dialogEditarCuentasPorCobrar').on('keypress', '#txtTipoCambio', function(event) {
        if (!ValidarFlotante(event, $(this).val())) {
            return false;
        }
    });

    $("#dialogAgregarCuentasPorCobrar, #dialogEditarCuentasPorCobrar").on("click", "#divBuscarCuentasContables", function() {
        $("#divFormaCuentaBancaria").obtenerVista({
            nombreTemplate: "tmplMuestraCuentasContables.html",
            despuesDeCompilar: function() {
                FiltroCuentaBancaria();
                $("#dialogMuestraCuentasBancarias").dialog("open");
            }
        });
    });

    $("#dialogAgregarCuentasPorCobrar, #dialogEditarCuentasPorCobrar").on("click", "#chkConciliado", function() {
        $("#txtFechaConciliacion").prop('disabled', function() {
            return !$(this).prop('disabled');
        });
    });

    $('#dialogAgregarCuentasPorCobrar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCuentasPorCobrar").remove();
        },
        buttons: {
            "Guardar": function() {
                if ($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar") == null || $("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar") == "") {
                    AgregarCuentasPorCobrar();
                }
                else {
                    EditarCuentasPorCobrar();
                }
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConciliarCuentasPorCobrar').dialog({
        autoOpen: false,
        height: '620',
        width: '1000',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConciliarCuentasPorCobrar").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarCuentasPorCobrar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCuentasPorCobrar").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCuentasPorCobrar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCuentasPorCobrar").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCuentasPorCobrar();
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraCuentasBancarias').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaMuestraCuentasBancarias").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogMuestraAsociarDocumentos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            HabilitaMonto();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

});

function HabilitaMonto(){
    var CuentasPorCobrar = new Object();       
    CuentasPorCobrar.pIdCuentasPorCobrar = parseInt($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar"));
    ObtenerHabilitaMonto(JSON.stringify(CuentasPorCobrar));    
}

function ObtenerHabilitaMonto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/ObtenerHabilitaMonto",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                if (parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares))
                {
                    $("#txtImporte").attr("disabled", "disabled");
                }
                else
                {
                    $("#txtImporte").removeAttr("disabled");
                }
                
                if (respuesta.Modelo.puedeEditarTipoCambioIngresos == 0 || parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares))
                {
                    $("#txtTipoCambio").attr("disabled", "disabled");
                }
                else
                {
                    $("#txtTipoCambio").removeAttr("disabled");
                }
                
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

function ObtenerFormaFiltrosCuentasPorCobrar() {
    $("#divFiltrosCuentasPorCobrar").obtenerVista({
        nombreTemplate: "tmplFiltrosCuentasPorCobrar.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaFiltroCuentasPorCobrar",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroCuentasPorCobrar();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroCuentasPorCobrar();
                    }
                });
            }

            $('#divFiltrosCuentasPorCobrar').on('click', '#chkPorFecha', function(event) {
                FiltroCuentasPorCobrar();
            });

        }
    });
}

function ConciliarIngreso(IdCuentasPorCobrar) {

    var pCuentasPorCobrar = new Object();
    pCuentasPorCobrar.pIdCuentasPorCobrar = IdCuentasPorCobrar
    var oRequest = new Object();
    oRequest.pCuentasPorCobrar = pCuentasPorCobrar;
    SetConciliarIngreso(JSON.stringify(oRequest));
}

function SetConciliarIngreso(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/ConciliarIngreso",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentasPorCobrarConciliar").trigger("reloadGrid");
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCuentasPorCobrar").dialog("close");
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCuentasPorCobrar() {
    $("#dialogAgregarCuentasPorCobrar").obtenerVista({
        nombreTemplate: "tmplAgregarCuentasPorCobrar.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaAgregarCuentasPorCobrar",
        despuesDeCompilar: function(pRespuesta) {
            $("#txtFecha").datepicker({
                maxDate: new Date()
            });
            $("#txtFechaAplicacion").datepicker({
                maxDate: new Date(),
                onSelect: function() {
                    var pRequest = new Object();
                    pRequest.pIdTipoMonedaDestino = 2;
                    pRequest.pFecha = $("#txtFechaAplicacion").val(); ;
                    ObtenerTipoCambioDiarioOficial(JSON.stringify(pRequest));
                }
            });
            $("#txtFechaConciliacion").datepicker({
                maxDate: new Date()
            });
            AutocompletarCliente();
            
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambioIngresos == 1)
            {
                $("#txtTipoCambio").removeAttr("disabled");
                
            }
            else
            {
                $("#txtTipoCambio").attr("disabled", "disabled");
            }
            
            $('#dialogAgregarCuentasPorCobrar').on('focusout', '#txtImporte', function(event) {
                $("#txtImporte").val(formato.moneda(QuitarFormatoNumero($("#txtImporte").val()), "$"));
            });
            
            $("#tabAsignarDocumentos").tabs();
            $("#dialogAgregarCuentasPorCobrar").dialog("open");
        }
    });
}

function ObtenerFormaConciliarCuentasPorCobrar() {
    $("#dialogConciliarCuentasPorCobrar").obtenerVista({
        nombreTemplate: "tmplConciliarCuentasPorCobrar.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaConciliarCuentasPorCobrar",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogConciliarCuentasPorCobrar").dialog("open");
            Inicializar_grdCuentasPorCobrarConciliar();
        }
    });
}

function ObtenerFormaAsociarDocumentos(pCuentasPorCobrar) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentos.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaAsociarDocumentos",
        parametros: pCuentasPorCobrar,
        despuesDeCompilar: function(pRespuesta) {
            FiltroFacturas();
            FiltroMovimientosCobros();
            $("#dialogMuestraAsociarDocumentos").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCuentasPorCobrar(pIdCuentasPorCobrar) {
    $("#dialogConsultarCuentasPorCobrar").obtenerVista({
        nombreTemplate: "tmplConsultarCuentasPorCobrar.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaCuentasPorCobrar",
        parametros: pIdCuentasPorCobrar,
        despuesDeCompilar: function (pRespuesta) {
            console.log("2");
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarCuentasPorCobrar == 1) {
                $("#dialogConsultarCuentasPorCobrar").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var CuentasPorCobrar = new Object();
                        CuentasPorCobrar.IdCuentasPorCobrar = parseInt($("#divFormaConsultarCuentasPorCobrar").attr("IdCuentasPorCobrar"));
                        ObtenerFormaEditarCuentasPorCobrar(JSON.stringify(CuentasPorCobrar))
                    }
                });
                $("#dialogConsultarCuentasPorCobrar").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCuentasPorCobrar").dialog("option", "buttons", {});
                $("#dialogConsultarCuentasPorCobrar").dialog("option", "height", "auto");
            }
            $("#dialogConsultarCuentasPorCobrar").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaEditarCuentasPorCobrar(IdCuentasPorCobrar) {
    $("#dialogEditarCuentasPorCobrar").obtenerVista({
        nombreTemplate: "tmplEditarCuentasPorCobrar.html",
        url: "CuentasPorCobrar.aspx/ObtenerFormaEditarCuentasPorCobrar",
        parametros: IdCuentasPorCobrar,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosEditar();
            $("#dialogEditarCuentasPorCobrar").dialog("option", "height", "auto");
            $("#txtFecha").datepicker({
            	maxDate: new Date()
            });
            $("#txtFechaAplicacion").datepicker({
            	maxDate: new Date()
            });
            $("#txtFechaConciliacion").datepicker({
            	maxDate: new Date()
            });
            AutocompletarCliente();
            $("#tabAsignarDocumentosEditar").tabs();
            $("#dialogEditarCuentasPorCobrar").dialog("open");
        }
    });
}

function ObtenerDatosCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/ObtenerDatosCuentaBancaria",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                var Proyecto = new Object();
                $("#txtCuenta").val(respuesta.Modelo.CuentaBancaria);
                $("#spanBanco").text(respuesta.Modelo.Descripcion + ' (' + respuesta.Modelo.CuentaBancaria + ') ' + respuesta.Modelo.Banco + ' ' + respuesta.Modelo.TipoMoneda);
                $("#spanSaldo").text(formato.moneda(respuesta.Modelo.Saldo, "$"));
                $("#txtTipoCambio").val(respuesta.Modelo.TipoCambio);
                $("#divFormaAgregarCuentasPorCobrar, #divFormaEditarCuentasPorCobrar").attr("idCuentaBancaria", respuesta.Modelo.IdCuentaBancaria);
                $("#cmbTipoMoneda option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFechaAplicacion").val(respuesta.Modelo.Fecha);
                
                if (parseInt(respuesta.Modelo.Permisos.puedeEditarTipoCambioIngresos) == 0)
                {
                    $("#txtTipoCambio").attr("disabled", "disabled");
                }
                else
                {
                    $("#txtTipoCambio").removeAttr("disabled");
                }
                
                if(respuesta.Modelo.PuedeVerSaldo == 1)
                {
                    $("#PuedeVerSaldoEtiqueda").css("display", "block");
                    $("#spanSaldo").css("display", "block");
                }
                else
                {
                    $("#PuedeVerSaldoEtiqueda").css("display", "none");
                    $("#spanSaldo").css("display", "none");
                }
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

function ObtenerTipoCambioDiarioOficial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/ObtenerTipoCambioDiarioOficial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtTipoCambio").val(respuesta.TipoCambioDiarioOficial);
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

function FiltroCuentasPorCobrar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCuentasPorCobrar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCuentasPorCobrar').getGridParam('page');
    request.pColumnaOrden = $('#grdCuentasPorCobrar').getGridParam('sortname');
    request.pTipoOrden = $('#grdCuentasPorCobrar').getGridParam('sortorder');
    request.pFolio = "";
    request.pAI = 0;
    request.pRazonSocial = "";
    request.pGestor = "";

    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    
    request.pAsociado = "NO";

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }
    
    if ($('#gs_Gestor').val() != null) { request.pGestor = $("#gs_Gestor").val(); }
    
    if ($('#gs_AI').val() != null) { request.pAI = $("#gs_AI").val(); }

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

        if ($("#chkPorFecha").is(':checked')) {
            request.pPorFecha = 1;
        }
        else {
            request.pPorFecha = 0;
        }

        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }
    
    if ($('#gs_Asociado').val() != null) { request.pAsociado = $("#gs_Asociado").val(); }


    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerCuentasPorCobrar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdCuentasPorCobrar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroCuentasPorCobrarConciliar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCuentasPorCobrarConciliar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCuentasPorCobrarConciliar').getGridParam('page');
    request.pColumnaOrden = $('#grdCuentasPorCobrarConciliar').getGridParam('sortname');
    request.pTipoOrden = $('#grdCuentasPorCobrarConciliar').getGridParam('sortorder');
    request.pFolio = "";
    request.pAI = 0;
    request.pRazonSocial = "";    

    if ($('#gs_FolioConciliar').val() != null) { request.pFolio = $("#gs_FolioConciliar").val(); }

    if ($('#gs_RazonSocialConciliar').val() != null) { request.pRazonSocial = $("#gs_RazonSocialConciliar").val(); }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerCuentasPorCobrarConciliar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdCuentasPorCobrarConciliar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroCuentaBancaria() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCuentaBancaria').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCuentaBancaria').getGridParam('page');
    request.pColumnaOrden = $('#grdCuentaBancaria').getGridParam('sortname');
    request.pTipoOrden = $('#grdCuentaBancaria').getGridParam('sortorder');
    request.pCuentaBancaria= "";
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerCuentaBancaria',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdCuentaBancaria')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturas() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturas').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturas').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturas').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturas').getGridParam('sortorder');
    request.pSerie = "";
    request.pNumeroFactura = 0;
    request.pIdCliente = 0;

    if ($('#gs_NumeroFactura').val() != null) {
        request.pNumeroFactura = $("#gs_NumeroFactura").val();
    }
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCliente") != null) {
        request.pIdCliente = $("#divFormaEditarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCliente");
        if ($('#divContGridAsociarDocumento').find(gs_Serie).existe()) {
            request.pSerie = $('#divContGridAsociarDocumento').find(gs_Serie).val();
        }        
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerFacturas',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturas')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroMovimientosCobros() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobros').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobros').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobros').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobros').getGridParam('sortorder');
    request.pIdCuentasPorCobrar = 0;
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar") != null) {
        request.pIdCuentasPorCobrar = $("#divFormaEditarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerMovimientosCobros',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobros')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroMovimientosCobrosConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosConsultar').getGridParam('sortorder');
    request.pIdCuentasPorCobrar = 0;
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaConsultarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar") != null) {
        request.pIdCuentasPorCobrar = $("#divFormaEditarCuentasPorCobrar, #divFormaConsultarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerMovimientosCobrosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            console.log(jsondata);
            if (stat == 'success')
            { $('#grdMovimientosCobrosConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroMovimientosCobrosEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdMovimientosCobrosEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdMovimientosCobrosEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdMovimientosCobrosEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdMovimientosCobrosEditar').getGridParam('sortorder');
    request.pIdCuentasPorCobrar = 0;
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaConsultarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar") != null) {
        request.pIdCuentasPorCobrar = $("#divFormaEditarCuentasPorCobrar, #divFormaConsultarCuentasPorCobrar, #divFormaAsociarDocumentos").attr("IdCuentasPorCobrar");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'CuentasPorCobrar.aspx/ObtenerMovimientosCobrosEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdMovimientosCobrosEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCuentasPorCobrar() {
    var pCuentasPorCobrar = new Object();
    pCuentasPorCobrar.IdCuentaBancaria = $("#divFormaAgregarCuentasPorCobrar").attr("idCuentaBancaria");
    if ($("#divFormaAgregarCuentasPorCobrar").attr("idCliente") == "") {
        pCuentasPorCobrar.IdCliente = 0;
    }
    else {
        pCuentasPorCobrar.IdCliente = $("#divFormaAgregarCuentasPorCobrar").attr("idCliente");
    }
    pCuentasPorCobrar.CuentaBancaria = $("#txtCuenta").val();
    pCuentasPorCobrar.IdMetodoPago = $("#cmbMetodoPago").val();
    pCuentasPorCobrar.Fecha = $("#txtFecha").val();
    pCuentasPorCobrar.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pCuentasPorCobrar.Referencia = $("#txtReferencia").val();
    pCuentasPorCobrar.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pCuentasPorCobrar.FechaAplicacion = $("#txtFechaAplicacion").val();
    pCuentasPorCobrar.FechaConciliacion = $("#txtFechaConciliacion").val();
    pCuentasPorCobrar.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCuentasPorCobrar.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pCuentasPorCobrar.IdTipoMoneda == 2)
    {
        pCuentasPorCobrar.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pCuentasPorCobrar.TipoCambio = "1";
    }

    if (pCuentasPorCobrar.TipoCambio.replace(" ", "") == "") {
        pCuentasPorCobrar.TipoCambio = 0;
    }
    
    if (pCuentasPorCobrar.TipoCambioDOF.replace(" ", "") == "") {
        pCuentasPorCobrar.TipoCambioDOF = 0;
    }
    
    if ($("#chkConciliado").is(':checked')) {
        pCuentasPorCobrar.Conciliado = 1;
    }
    else {
        pCuentasPorCobrar.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pCuentasPorCobrar.Asociado = 1;
    }
    else {
        pCuentasPorCobrar.Asociado = 0;
    }
    
    var validacion = ValidaCuentasPorCobrar(pCuentasPorCobrar);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentasPorCobrar = pCuentasPorCobrar;
    SetAgregarCuentasPorCobrar(JSON.stringify(oRequest));
}

function AgregarCuentasPorCobrarEdicion() {
    var pCuentasPorCobrar = new Object();
    pCuentasPorCobrar.IdCuentaBancaria = $("#divFormaAgregarCuentasPorCobrar").attr("idCuentaBancaria");
    if ($("#divFormaAgregarCuentasPorCobrar").attr("idCliente") == "") {
        pCuentasPorCobrar.IdCliente = 0;
    }
    else {
        pCuentasPorCobrar.IdCliente = $("#divFormaAgregarCuentasPorCobrar").attr("idCliente");
    }
    pCuentasPorCobrar.CuentaBancaria = $("#txtCuenta").val();
    pCuentasPorCobrar.IdMetodoPago = $("#cmbMetodoPago").val();
    pCuentasPorCobrar.Fecha = $("#txtFecha").val();
    pCuentasPorCobrar.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pCuentasPorCobrar.Referencia = $("#txtReferencia").val();
    pCuentasPorCobrar.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pCuentasPorCobrar.FechaAplicacion = $("#txtFechaAplicacion").val();
    pCuentasPorCobrar.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCuentasPorCobrar.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());

    if(pCuentasPorCobrar.IdTipoMoneda == 2)
    {
        pCuentasPorCobrar.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pCuentasPorCobrar.TipoCambio = "1";
    }

    if (pCuentasPorCobrar.TipoCambio.replace(" ", "") == "") {
        pCuentasPorCobrar.TipoCambio = 0;
    }
    
    if (pCuentasPorCobrar.TipoCambioDOF.replace(" ", "") == "") {
        pCuentasPorCobrar.TipoCambioDOF = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pCuentasPorCobrar.Conciliado = 1;
    }
    else {
        pCuentasPorCobrar.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pCuentasPorCobrar.Asociado = 1;
    }
    else {
        pCuentasPorCobrar.Asociado = 0;
    }

    var validacion = ValidaCuentasPorCobrarEdicion(pCuentasPorCobrar);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentasPorCobrar = pCuentasPorCobrar;
    SetAgregarCuentasPorCobrarEdicion(JSON.stringify(oRequest));
}

function SetAgregarCuentasPorCobrar(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/AgregarCuentasPorCobrar",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCuentasPorCobrar").dialog("close");
        }
    });
}

function SetAgregarCuentasPorCobrarEdicion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/AgregarCuentasPorCobrarEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarCuentasPorCobrar").attr("idCuentasPorCobrar", respuesta.IdCuentasPorCobrar);
                $("#txtCuenta").attr("disabled", "true");
                $("#cmbMetodoPago").attr("disabled", "true");
                $("#txtFecha").attr("disabled", "true");
                $("#txtRazonSocial").attr("disabled", "true");
                $("#txtFechaAplicacion").attr("disabled", "true");
                $("#chkAsociado").attr("disabled", "true");
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFolio").val(respuesta.Folio);
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
                var CuentasPorCobrar = new Object();
                CuentasPorCobrar.pIdCuentasPorCobrar = parseInt($("#divFormaEditarCuentasPorCobrar,#divFormaAgregarCuentasPorCobrar").attr("IdCuentasPorCobrar"));
                CuentasPorCobrar.pIdCliente = parseInt($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("IdCliente"));
                CuentasPorCobrar.IdTipoMoneda = $("#cmbTipoMoneda").val();
                CuentasPorCobrar.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
                if(CuentasPorCobrar.IdTipoMoneda == 2)
                {
                    CuentasPorCobrar.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    CuentasPorCobrar.TipoCambio = "1";
                }

                if (CuentasPorCobrar.TipoCambio.replace(" ", "") == "") {
                    CuentasPorCobrar.TipoCambio = 0;
                }
                
                if (CuentasPorCobrar.TipoCambioDOF.replace(" ", "") == "") {
                    CuentasPorCobrar.TipoCambioDOF = 0;
                }
                
                var validacion = ValidaAsociacionDocumentos(CuentasPorCobrar);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.pCuentasPorCobrar = CuentasPorCobrar;
                ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
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

function SetCambiarEstatus(pIdCuentasPorCobrar, pBaja) {
    var pRequest = "{'pIdCuentasPorCobrar':" + pIdCuentasPorCobrar + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }            
        },
        complete: function() {
            $('#grdCuentasPorCobrar').one('click', '.div_grdCuentasPorCobrar_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCuentasPorCobrar_AI']").children().attr("baja")
                var idCuentasPorCobrar = $(registro).children("td[aria-describedby='grdCuentasPorCobrar_IdCuentasPorCobrar']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCuentasPorCobrar, baja);
            });
        }
    });
}

function EditarCuentasPorCobrar() {
    var pCuentasPorCobrar = new Object();
    pCuentasPorCobrar.IdCuentasPorCobrar = $("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("idCuentasPorCobrar");
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("idCliente") != null) {
        pCuentasPorCobrar.IdCliente = $("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("idCliente");
    }
    else {
        pCuentasPorCobrar.IdCliente = 0;
    }

    pCuentasPorCobrar.CuentaBancaria = $("#txtCuenta").val();
    pCuentasPorCobrar.IdMetodoPago = $("#cmbMetodoPago").val();
    pCuentasPorCobrar.Fecha = $("#txtFecha").val();
    pCuentasPorCobrar.Folio = $("#txtFolio").val();
    pCuentasPorCobrar.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pCuentasPorCobrar.Referencia = $("#txtReferencia").val();
    pCuentasPorCobrar.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pCuentasPorCobrar.FechaAplicacion = $("#txtFechaAplicacion").val();
    pCuentasPorCobrar.FechaConciliacion = $("#txtFechaConciliacion").val();
    pCuentasPorCobrar.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCuentasPorCobrar.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pCuentasPorCobrar.IdTipoMoneda == 2)
    {
        pCuentasPorCobrar.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pCuentasPorCobrar.TipoCambio = "1";
    }

    if (pCuentasPorCobrar.TipoCambio.replace(" ", "") == "") {
        pCuentasPorCobrar.TipoCambio = 0;
    }
    
    if (pCuentasPorCobrar.TipoCambioDOF.replace(" ", "") == "") {
        pCuentasPorCobrar.TipoCambioDOF = 0;
    }
    

    if ($("#chkConciliado").is(':checked')) {
        pCuentasPorCobrar.Conciliado = 1;
    }
    else {
        pCuentasPorCobrar.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pCuentasPorCobrar.Asociado = 1;
    }
    else {
        pCuentasPorCobrar.Asociado = 0;
    }
    
    var validacion = ValidaCuentasPorCobrar(pCuentasPorCobrar);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentasPorCobrar = pCuentasPorCobrar;
    SetEditarCuentasPorCobrar(JSON.stringify(oRequest));
}

function EditarCuentasPorCobrarCliente() {
    var pCuentasPorCobrar = new Object();
    pCuentasPorCobrar.IdCuentasPorCobrar = $("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("idCuentasPorCobrar");
    if ($("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("idCliente") != null) {
        pCuentasPorCobrar.IdCliente = $("#divFormaEditarCuentasPorCobrar, #divFormaAgregarCuentasPorCobrar").attr("idCliente");
    }
    else {
        pCuentasPorCobrar.IdCliente = 0;
    }
    
    var oRequest = new Object();
    oRequest.pCuentasPorCobrar = pCuentasPorCobrar;
    SetEditarCuentasPorCobrarCliente(JSON.stringify(oRequest));
}
function SetEditarCuentasPorCobrar(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/EditarCuentasPorCobrar",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
            	$("#grdCuentasPorCobrar").trigger("reloadGrid");
            	$("#dialogEditarCuentasPorCobrar, #dialogAgregarCuentasPorCobrar").dialog("close");
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

function SetEditarCuentasPorCobrarCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/EditarCuentasPorCobrarCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCuentasPorCobrar").trigger("reloadGrid");
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function EdicionFacturas(valor, id, rowid, iCol) {
    var TipoMoneda = $('#grdFacturas').getCell(rowid, 'TipoMoneda');
    var Saldo = $('#grdFacturas').getCell(rowid, 'Saldo');
    var EsParcialidad = $('#grdFacturas').getCell(rowid, 'EsParcialidad');
    
    var CuentasPorCobrar = new Object();

    if (EsParcialidad == true || EsParcialidad == "True") {
        CuentasPorCobrar.EsParcialidad = 1;
    }
    else {
        CuentasPorCobrar.EsParcialidad = 0;
    } 

    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        CuentasPorCobrar.IdTipoMoneda = 1;
        CuentasPorCobrar.TipoCambio = 1;
        CuentasPorCobrar.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        CuentasPorCobrar.IdTipoMoneda = 2;
        CuentasPorCobrar.TipoCambio = $("#spanTipoCambioDolares").text();
        CuentasPorCobrar.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    CuentasPorCobrar.TipoMoneda = TipoMoneda;

    CuentasPorCobrar.Monto = QuitarFormatoNumero(valor);
    CuentasPorCobrar.Saldo = QuitarFormatoNumero(Saldo);
    CuentasPorCobrar.IdEncabezadoFactura = id;
    CuentasPorCobrar.rowid = rowid;
    CuentasPorCobrar.IdCuentasPorCobrar = $("#divFormaAsociarDocumentos").attr("idCuentasPorCobrar");
    var validacion = ValidarMontos(CuentasPorCobrar);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCuentasPorCobrar = CuentasPorCobrar;
    SetEditarMontos(JSON.stringify(oRequest));

    //Nueva Forma de Timbrar Pago
    //console.log(oRequest);
    //var oRequest = new Object();
    //oRequest.IdCuentasPorCobrar = CuentasPorCobrar.IdCuentasPorCobrar;
    //oRequest.IdEncabezadoFactura = CuentasPorCobrar.IdEncabezadoFactura;
    //oRequest.EsParcialidad = CuentasPorCobrar.EsParcialidad;
    //oRequest.Monto = CuentasPorCobrar.Monto;
    //oRequest.Saldo = CuentasPorCobrar.Saldo;
    //oRequest.IdTipoMoneda = CuentasPorCobrar.IdTipoMoneda;
    //oRequest.TipoCambio = CuentasPorCobrar.TipoCambio;
    //ObtenerPagoATimbrar(JSON.stringify(oRequest));

}

function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);

            console.log(respuesta);
            if (respuesta.Error == 0) {
                if (respuesta.EsParcialidad == 1) {
                    //MostrarMensajeError(respuesta.Descripcion);
                }
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosCuentasPorCobrar;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosCuentasPorCobrar / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));
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

function SetEliminarCuentasPorCobrarEncabezadoFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "CuentasPorCobrar.aspx/EliminarCuentasPorCobrarEncabezadoFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.EsParcialidad == 1) {
                    MostrarMensajeError(respuesta.Descripcion);
                }
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");
                $("#grdCuentasPorCobrar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosCuentasPorCobrar;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosCuentasPorCobrar / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));
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

function AutocompletarCliente() {

    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtRazonSocial').val();
            $.ajax({
                type: 'POST',
                url: 'CuentasPorCobrar.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarCuentasPorCobrar, #divFormaEditarCuentasPorCobrar").attr("idCliente", "0");
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
            $("#divFormaAgregarCuentasPorCobrar, #divFormaEditarCuentasPorCobrar").attr("idCliente", pIdCliente);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

//-----Validaciones------------------------------------------------------

function ValidaCuentasPorCobrar(pCuentasPorCobrar) {
    var errores = "";

    if (pCuentasPorCobrar.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pCuentasPorCobrar.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pCuentasPorCobrar.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pCuentasPorCobrar.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pCuentasPorCobrar.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pCuentasPorCobrar.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pCuentasPorCobrar.FechaAplicacion, 'aaaammdd') > ConvertirFecha(pCuentasPorCobrar.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser mayor a la fecha de emisión.<br />"; }

    if (pCuentasPorCobrar.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (parseFloat(pCuentasPorCobrar.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaCuentasPorCobrarEdicion(pCuentasPorCobrar) {
    var errores = "";

    if (pCuentasPorCobrar.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pCuentasPorCobrar.IdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pCuentasPorCobrar.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pCuentasPorCobrar.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pCuentasPorCobrar.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pCuentasPorCobrar.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pCuentasPorCobrar.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pCuentasPorCobrar.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pCuentasPorCobrar.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pCuentasPorCobrar.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(CuentasPorCobrar) {
    var errores = "";

    if (CuentasPorCobrar.pIdCuentasPorCobrar == 0)
    { errores = errores + "<span>*</span> No hay cobro por asociar, favor de elegir alguno.<br />"; }
    
    if (CuentasPorCobrar.pIdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(CuentasPorCobrar) {
    var errores = "";

    if (CuentasPorCobrar.IdEncabezadoFactura == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(CuentasPorCobrar.Monto) > parseFloat(CuentasPorCobrar.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al disponible.<br />"; }

    if (parseFloat(CuentasPorCobrar.Monto) > parseFloat(CuentasPorCobrar.Saldo))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(CuentasPorCobrar.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(CuentasPorCobrar.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

////////////////////////  Nueva forma de guardar Complementos de Pago /////////////////////////////////////

/* Timbrar */
function ObtenerPagoATimbrar(Request) {
    MostrarBloqueo();
    $.ajax({
        url: "CuentasPorCobrar.aspx/ObtenerDatosTimbradoPago",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                //TimbrarPago(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function TimbrarPago(json) {
    var Comprobante = new Object();
    Comprobante.Comprobante = json.Comprobante;
    Comprobante.Id = json.Id;
    Comprobante.Token = json.Token;
    Comprobante.RFC = json.RFC;
    Comprobante.RefID = json.RefID;
    Comprobante.Formato = json.Formato;
    Comprobante.NoCertificado = json.NoCertificado;
    Comprobante.Correos = json.Correos;
    Comprobante.ActualizarMontos = json.ActualizarMontos;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "http://" + window.location.hostname +"/WebServiceDiverza/Pagos.aspx/TimbrarPago",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                GuardarFacturaPago(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function GuardarFacturaPago(json) {
    var Comprobante = new Object();
    Comprobante.UUId = json.uuid;
    Comprobante.RefId = json.ref_id;
    Comprobante.Contenido = json.content;
    Comprobante.Certificado = json.certificado;
    Comprobante.RFC = json.rfc;
    Comprobante.Serie = json.serie;
    Comprobante.Folio = json.folio;
    Comprobante.ActualizarMontos = json.ActualizarMontos;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "CuentasPorCobrar.aspx/GuardarTimbradoPago",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdCuentasPorCobrar").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - json.AbonosCuentasPorCobrar;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (json.AbonosCuentasPorCobrar / $("#spanTipoCambioDolares").text());
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
                $("#spanDisponibleDolares").text(formato.moneda(DisponibleDolares, "$"));

                MostrarMensajeError(json.Descripcion);
            }
            else {
                MostrarMensajeError(json.Descripcion);

            }
            OcultarBloqueo();
        }
    });
}


/* Cancelar */
function ObtenerFacturaACancelar(Request) {
    console.log("Cancelar");
    MostrarBloqueo();
    $.ajax({
        url: "FacturaCliente.aspx/ObtenerDatosCancelacion",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#dialogMotivoCancelacionFactura").dialog("close");
                CancelarFactura(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function CancelarFactura(json) {
    console.log("Cancelando");
    var Comprobante = new Object();
    Comprobante.UUID = json.Comprobante.UUID;
    Comprobante.RefID = json.Comprobante.ref_id;
    Comprobante.Id = json.Id;
    Comprobante.Token = json.Token;
    Comprobante.RFC = json.RFC;
    Comprobante.NoCertificado = json.NoCertificado;
    Comprobante.MotivoCancelacion = json.MotivoCancelacion;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "http://localhost/WebServiceDiverza/Facturacion.aspx/CancelarFactura",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                EditarFacturaCancelada(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function EditarFacturaCancelada(json) {
    console.log("Cancelado");
    var Comprobante = new Object();
    Comprobante.UUId = json.uuid;
    Comprobante.RefId = json.ref_id;
    Comprobante.Date = json.date;
    Comprobante.Contenido = json.content;
    Comprobante.MotivoCancelacion = json.motivoCancelacion;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "FacturaCliente.aspx/EditarFactura",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                MostrarMensajeError(json.Cancelado);
            }
            else {
                MostrarMensajeError(json.Descripcion);

            }
            OcultarBloqueo();
        }
    });
}

