//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Depositos");
    });

    ObtenerFormaFiltrosDepositos();

    //////funcion del grid//////
    $("#gbox_grdDepositos").livequery(function() {
        $("#grdDepositos").jqGrid('navButtonAdd', '#pagDepositos', {
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
    /////////////////////////// 

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarDepositos", function() {
        ObtenerFormaAgregarDepositos();
    });

    $("#dialogEditarDepositos, #dialogAgregarDepositos").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var Depositos = new Object();

        if ($("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("IdDepositos") != null && $("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("IdDepositos") != "") {
            Depositos.pIdDepositos = parseInt($("#divFormaEditarDepositos,#divFormaAgregarDepositos").attr("IdDepositos"));
            Depositos.pIdCliente = parseInt($("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("IdCliente"));
            var validacion = ValidaAsociacionDocumentos(Depositos);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.Depositos = Depositos;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
        }
        else {
            AgregarDepositosEdicion();
        }
    });

    $("#grdDepositos").on("click", ".imgFormaConsultarDepositos", function() {
        var registro = $(this).parents("tr");
        var Depositos = new Object();
        Depositos.pIdDepositos = parseInt($(registro).children("td[aria-describedby='grdDepositos_IdDepositos']").html());
        ObtenerFormaConsultarDepositos(JSON.stringify(Depositos));
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
        var pDepositosEncabezadoFactura = new Object();
        pDepositosEncabezadoFactura.pIdDepositosEncabezadoFactura = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdDepositosEncabezadoFactura']").html());
        var oRequest = new Object();
        oRequest.pDepositosEncabezadoFactura = pDepositosEncabezadoFactura;
        SetEliminarDepositosEncabezadoFactura(JSON.stringify(oRequest));
    });

    $("#grdCuentaBancaria").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");
    });

    $('#grdDepositos').one('click', '.div_grdDepositos_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdDepositos_AI']").children().attr("baja")
        var idDepositos = $(registro).children("td[aria-describedby='grdDepositos_IdDepositos']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idDepositos, baja);
    });

    $("#dialogAgregarDepositos, #dialogEditarDepositos").on("click", "#divBuscarCuentasContables", function() {
        $("#divFormaCuentaBancaria").obtenerVista({
            nombreTemplate: "tmplMuestraCuentasContables.html",
            despuesDeCompilar: function() {
                FiltroCuentaBancaria();
                $("#dialogMuestraCuentasBancarias").dialog("open");
            }
        });
    });

    $('#dialogAgregarDepositos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarDepositos").remove();
        },
        buttons: {
            "Guardar": function() {
                if ($("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("IdDepositos") == null || $("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("IdDepositos") == "") {
                    AgregarDepositos();
                }
                else {
                    EditarDepositos();
                }
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarDepositos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarDepositos").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarDepositos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarDepositos").remove();
        },
        buttons: {
            "Editar": function() {
                EditarDepositos();
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
            //$("#divFormaAsociarDocumentosF").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

});

function ObtenerFormaFiltrosDepositos() {
    $("#divFiltrosDepositos").obtenerVista({
        nombreTemplate: "tmplFiltrosDepositos.html",
        url: "Depositos.aspx/ObtenerFormaFiltroDepositos",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroDepositos();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroDepositos();
                    }
                });
            }

            $('#divFiltrosDepositos').on('click', '#chkPorFecha', function(event) {
                FiltroDepositos();
            });

        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarDepositos() {
    $("#dialogAgregarDepositos").obtenerVista({
        nombreTemplate: "tmplAgregarDepositos.html",
        url: "Depositos.aspx/ObtenerFormaAgregarDepositos",
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdIngresosNoDepositados();
            $("#dialogAgregarDepositos").dialog("open");
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
            $("#tabAsignarDocumentos").tabs();

        }
    });
}


function ObtenerFormaAsociarDocumentos(Depositos) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentos.html",
        url: "Depositos.aspx/ObtenerFormaAsociarDocumentos",
        parametros: Depositos,
        despuesDeCompilar: function(pRespuesta) {
            FiltroFacturas();
            FiltroMovimientosCobros();
            $("#dialogMuestraAsociarDocumentos").dialog("open");
        }
    });
}

function ObtenerFormaConsultarDepositos(pIdDepositos) {
    $("#dialogConsultarDepositos").obtenerVista({
        nombreTemplate: "tmplConsultarDepositos.html",
        url: "Depositos.aspx/ObtenerFormaDepositos",
        parametros: pIdDepositos,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdIngresosNoDepositadosConsultar();
            FiltroIngresosNoDepositadosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarDepositos == 1) {
                $("#dialogConsultarDepositos").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Depositos = new Object();
                        Depositos.IdDepositos = parseInt($("#divFormaConsultarDepositos").attr("IdDepositos"));
                        ObtenerFormaEditarDepositos(JSON.stringify(Depositos))
                    }
                });
                $("#dialogConsultarDepositos").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarDepositos").dialog("option", "buttons", {});
                $("#dialogConsultarDepositos").dialog("option", "height", "auto");
            }
            $("#dialogConsultarDepositos").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaEditarDepositos(IdDepositos) {
    $("#dialogEditarDepositos").obtenerVista({
        nombreTemplate: "tmplEditarDepositos.html",
        url: "Depositos.aspx/ObtenerFormaEditarDepositos",
        parametros: IdDepositos,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdIngresosNoDepositadosEditar();
            $("#dialogEditarDepositos").dialog("option", "height", "auto");
            FiltroIngresosNoDepositadosEditar();
            $("#dialogEditarDepositos").dialog("open");
            $("#tabAsignarDocumentosEditar").tabs();
            
        }
    });
}

function ObtenerDatosCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/ObtenerDatosCuentaBancaria",
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
                $("#spanTipoCambio").text(respuesta.Modelo.TipoCambio);
                $("#divFormaAgregarDepositos, #divFormaEditarDepositos").attr("idCuentaBancaria", respuesta.Modelo.IdCuentaBancaria);
                $("#cmbTipoMoneda option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFechaAplicacion").val(respuesta.Modelo.Fecha);
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
                
                FiltroIngresosNoDepositados();
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
        url: "Depositos.aspx/ObtenerTipoCambioDiarioOficial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#spanTipoCambio").text(respuesta.TipoCambioDiarioOficial);
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

function FiltroDepositos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDepositos').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDepositos').getGridParam('page');
    request.pColumnaOrden = $('#grdDepositos').getGridParam('sortname');
    request.pTipoOrden = $('#grdDepositos').getGridParam('sortorder');
    request.pFolio = "";
    request.pAI = 0;
    request.pRazonSocial = "";

    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }

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


    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerDepositos',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDepositos')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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
        url: 'Depositos.aspx/ObtenerCuentaBancaria',
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
    
    request.pIdCliente = 0;
    if ($("#divFormaEditarDepositos, #divFormaAsociarDocumentos").attr("IdCliente") != null) {
        request.pIdCliente = $("#divFormaEditarDepositos, #divFormaAsociarDocumentos").attr("IdCliente");
        if ($('#divContGridAsociarDocumento').find(gs_Serie).existe()) {
            request.pSerie = $('#divContGridAsociarDocumento').find(gs_Serie).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerFacturas',
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
    request.pIdDepositos = 0;
    if ($("#divFormaEditarDepositos, #divFormaAsociarDocumentos").attr("IdDepositos") != null) {
        request.pIdDepositos = $("#divFormaEditarDepositos, #divFormaAsociarDocumentos").attr("IdDepositos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerMovimientosCobros',
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


function ElegirMonto(IdIngresoNoDepositado) {

    var pIngresosNoDepositados = new Object();
    pIngresosNoDepositados.pIdIngresoNoDepositado = IdIngresoNoDepositado    
    var oRequest = new Object();
    oRequest.pIngresosNoDepositados = pIngresosNoDepositados;   
    SetObtenerMontoIngresoNoDepositados(JSON.stringify(oRequest));    
}
function FiltroIngresosNoDepositados() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdIngresosNoDepositados').getGridParam('rowNum');
    request.pPaginaActual = $('#grdIngresosNoDepositados').getGridParam('page');
    request.pColumnaOrden = $('#grdIngresosNoDepositados').getGridParam('sortname');
    request.pTipoOrden = $('#grdIngresosNoDepositados').getGridParam('sortorder');
    request.pIdCuentaBancaria = 0;
    if ($("#divFormaAgregarDepositos, #divFormaEditarDepositos").attr("idCuentaBancaria") != null && $("#divFormaAgregarDepositos, #divFormaEditarDepositos").attr("idCuentaBancaria") !="") {
        request.pIdCuentaBancaria = $("#divFormaAgregarDepositos, #divFormaEditarDepositos").attr("idCuentaBancaria");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerIngresosNoDepositados',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdIngresosNoDepositados')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroIngresosNoDepositadosConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdIngresosNoDepositadosConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdIngresosNoDepositadosConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdIngresosNoDepositadosConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdIngresosNoDepositadosConsultar').getGridParam('sortorder');
    request.pIdDepositos = 0;
    if ($("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaConsultarDepositos").attr("idDepositos") != null && $("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaConsultarDepositos").attr("idDepositos") != "") {
        request.pIdDepositos = $("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaConsultarDepositos").attr("idDepositos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerIngresosNoDepositadosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdIngresosNoDepositadosConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroIngresosNoDepositadosEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdIngresosNoDepositadosEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdIngresosNoDepositadosEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdIngresosNoDepositadosEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdIngresosNoDepositadosEditar').getGridParam('sortorder');
    request.pIdDepositos = 0;
    if ($("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaEditarDepositos").attr("idDepositos") != null && $("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaEditarDepositos").attr("idDepositos") != "") {
        request.pIdDepositos = $("#divFormaAgregarDepositos, #divFormaEditarDepositos, #divFormaEditarDepositos").attr("idDepositos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Depositos.aspx/ObtenerIngresosNoDepositadosEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdIngresosNoDepositadosEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarDepositos() {
    var pDepositos = new Object();
    pDepositos.IdCuentaBancaria = $("#divFormaAgregarDepositos").attr("idCuentaBancaria");
    if ($("#divFormaAgregarDepositos").attr("idCliente") == "") {
        pDepositos.IdCliente = 0;
    }
    else {
        pDepositos.IdCliente = $("#divFormaAgregarDepositos").attr("idCliente");
    }
    pDepositos.CuentaBancaria = $("#txtCuenta").val();
    pDepositos.IdMetodoPago = $("#cmbMetodoPago").val();
    pDepositos.Fecha = $("#txtFecha").val();
    pDepositos.Importe = $("#txtImporte").val();
    pDepositos.Referencia = $("#txtReferencia").val();
    pDepositos.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pDepositos.FechaAplicacion = $("#txtFechaAplicacion").val();
    pDepositos.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pDepositos.TipoCambio = QuitarFormatoNumero($("#spanTipoCambio").text());

    if (pDepositos.TipoCambio.replace(" ", "") == "") {
        pDepositos.TipoCambio = 0;
    }
    
    if ($("#chkConciliado").is(':checked')) {
        pDepositos.Conciliado = 1;
    }
    else {
        pDepositos.Conciliado = 0;
    }

    var values = "";
    $('#grdIngresosNoDepositados tr').each(function(index) {
        $(this).children("td").each(function(index2) {
            switch (index2) {
                case 0:
                    if ($(this).text() != null && $(this).text() != "") {
                            values = $('input:checkbox:checked.chkElegirMonto').map(function() {
                            return this.value;
                        }).get();
                    }
                    break;
            }
        });
    });

    pDepositos.IdsIngresosNoDepositados = new Array();
    
    $(".chkElegirMonto:checked").each(function(index, object) {
        var registro = $(this).parents("tr");
        pDepositos.IdsIngresosNoDepositados.push($(registro).children("td[aria-describedby='grdIngresosNoDepositados_IdIngresosNoDepositados']").text());
    });
    
    pDepositos.Asociado = 0;
    
    var validacion = ValidaDepositos(pDepositos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDepositos = pDepositos;
    SetAgregarDepositos(JSON.stringify(oRequest));
}

function AgregarDepositosEdicion() {
    var pDepositos = new Object();
    pDepositos.IdCuentaBancaria = $("#divFormaAgregarDepositos").attr("idCuentaBancaria");
    if ($("#divFormaAgregarDepositos").attr("idCliente") == "") {
        pDepositos.IdCliente = 0;
    }
    else {
        pDepositos.IdCliente = $("#divFormaAgregarDepositos").attr("idCliente");
    }
    pDepositos.CuentaBancaria = $("#txtCuenta").val();
    pDepositos.IdMetodoPago = $("#cmbMetodoPago").val();
    pDepositos.Fecha = $("#txtFecha").val();
    pDepositos.Importe = $("#txtImporte").val();
    pDepositos.Referencia = $("#txtReferencia").val();
    pDepositos.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pDepositos.FechaAplicacion = $("#txtFechaAplicacion").val();
    pDepositos.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pDepositos.TipoCambio = QuitarFormatoNumero($("#spanTipoCambio").text())
    if ($("#chkConciliado").is(':checked')) {
        pDepositos.Conciliado = 1;
    }
    else {
        pDepositos.Conciliado = 0;
    }

    pDepositos.Asociado = 0;

    var validacion = ValidaDepositosEdicion(pDepositos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDepositos = pDepositos;
    SetAgregarDepositosEdicion(JSON.stringify(oRequest));
}

function SetAgregarDepositos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/AgregarDepositos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDepositos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarDepositos").dialog("close");
        }
    });
}

function SetAgregarDepositosEdicion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/AgregarDepositosEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarDepositos").attr("idDepositos", respuesta.IdDepositos);
                $("#txtCuenta").attr("disabled", "true");
                $("#cmbMetodoPago").attr("disabled", "true");
                $("#txtFecha").attr("disabled", "true");
                $("#txtImporte").attr("disabled", "true");
                $("#txtFechaAplicacion").attr("disabled", "true");
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFolio").val(respuesta.Folio);
                $("#grdDepositos").trigger("reloadGrid");
                var Depositos = new Object();
                Depositos.pIdDepositos = parseInt($("#divFormaEditarDepositos,#divFormaAgregarDepositos").attr("IdDepositos"));
                Depositos.pIdCliente = parseInt($("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("IdCliente"));
                var validacion = ValidaAsociacionDocumentos(Depositos);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.Depositos = Depositos;
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

function SetCambiarEstatus(pIdDepositos, pBaja) {
    var pRequest = "{'pIdDepositos':" + pIdDepositos + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDepositos").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            $('#grdDepositos').one('click', '.div_grdDepositos_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdDepositos_AI']").children().attr("baja")
                var idDepositos = $(registro).children("td[aria-describedby='grdDepositos_IdDepositos']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idDepositos, baja);
            });
        }
    });
}

function EditarDepositos() {
    var pDepositos = new Object();
    pDepositos.IdDepositos = $("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("idDepositos");
    if ($("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("idCliente") != null) {
        pDepositos.IdCliente = $("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("idCliente");
    }
    else {
        pDepositos.IdCliente = 0;
    }

    pDepositos.CuentaBancaria = $("#txtCuenta").val();
    pDepositos.IdMetodoPago = $("#cmbMetodoPago").val();
    pDepositos.Fecha = $("#txtFecha").val();
    pDepositos.Folio = $("#txtFolio").val();
    pDepositos.Importe = $("#txtImporte").val();
    pDepositos.Referencia = $("#txtReferencia").val();
    pDepositos.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pDepositos.FechaAplicacion = $("#txtFechaAplicacion").val();
    pDepositos.TipoCambio = QuitarFormatoNumero($("#spanTipoCambio").text());

    if (pDepositos.TipoCambio.replace(" ", "") == "") {
        pDepositos.TipoCambio = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pDepositos.Conciliado = 1;
    }
    else {
        pDepositos.Conciliado = 0;
    }

    pDepositos.Asociado = 0;
    
    var validacion = ValidaDepositos(pDepositos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDepositos = pDepositos;
    SetEditarDepositos(JSON.stringify(oRequest));
}
function EditarDepositosCliente() {
    var pDepositos = new Object();
    pDepositos.IdDepositos = $("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("idDepositos");
    if ($("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("idCliente") != null) {
        pDepositos.IdCliente = $("#divFormaEditarDepositos, #divFormaAgregarDepositos").attr("idCliente");
    }
    else {
        pDepositos.IdCliente = 0;
    }
    
    var oRequest = new Object();
    oRequest.pDepositos = pDepositos;
    SetEditarDepositosCliente(JSON.stringify(oRequest));
}

function SetEditarDepositos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/EditarDepositos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdDepositos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarDepositos, #dialogAgregarDepositos").dialog("close");
        }
    });
}

function SetEditarDepositosCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/EditarDepositosCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdDepositos").trigger("reloadGrid");
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function EdicionFacturas(valor, id,rowid, iCol) {
    var Depositos = new Object();
    Depositos.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    Depositos.Monto = QuitarFormatoNumero(valor);
    Depositos.IdEncabezadoFactura = id;
    Depositos.rowid = rowid;
    Depositos.IdDepositos = $("#divFormaAsociarDocumentos").attr("idDepositos");
    var validacion = ValidarMontos(Depositos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pDepositos = Depositos;
    SetEditarMontos(JSON.stringify(oRequest));
    
}

function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdDepositos").trigger("reloadGrid");
                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                Disponible = Importe - respuesta.AbonosDepositos;
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
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

function SetObtenerMontoIngresoNoDepositados(pRequest) {
    var Suma = 0;
    $(".chkElegirMonto:checked").each(function(index, object) {
        var registro = $(this).parents("tr");
        Suma = Suma + parseFloat(QuitarFormatoNumero($(registro).children("td[aria-describedby='grdIngresosNoDepositados_Importe']").text()));
    });
    $("#txtImporte").val(Suma);
}

function SetEliminarDepositosEncabezadoFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Depositos.aspx/EliminarDepositosEncabezadoFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdDepositos").trigger("reloadGrid");
                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                Disponible = Importe - respuesta.AbonosDepositos;
                $("#spanDisponible").text(formato.moneda(Disponible, "$"));
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

//-----Validaciones------------------------------------------------------
function ValidaDepositos(pDepositos) {
    var errores = "";

    if (pDepositos.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pDepositos.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pDepositos.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pDepositos.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pDepositos.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pDepositos.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pDepositos.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pDepositos.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pDepositos.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (parseFloat(pDepositos.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (pDepositos.IdsIngresosNoDepositados == 0)
    { errores = errores + "<span>*</span> Debe de elegir un ingreso no depositado para realizar el deposito.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDepositosEdicion(pDepositos) {
    var errores = "";

    if (pDepositos.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pDepositos.IdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pDepositos.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pDepositos.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pDepositos.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pDepositos.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pDepositos.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pDepositos.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pDepositos.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pDepositos.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(Depositos) {
    var errores = "";

    if (Depositos.pIdDepositos == 0)
    { errores = errores + "<span>*</span> No hay cobro por asociar, favor de elegir alguno.<br />"; }
    
    if (Depositos.pIdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(Depositos) {
    var errores = "";

    if (Depositos.IdEncabezadoFactura == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(Depositos.Monto) > parseFloat(Depositos.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al dispoible.<br />"; }

    if (parseFloat(Depositos.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(Depositos.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

