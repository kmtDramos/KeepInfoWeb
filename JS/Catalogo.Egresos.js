//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Egresos");
    });

    ObtenerFormaFiltrosEgresos();

    //////funcion del grid//////
    $("#gbox_grdEgresos").livequery(function() {
        $("#grdEgresos").jqGrid('navButtonAdd', '#pagEgresos', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function() {

                var pRazonSocial = "";
                var pCuentaBancaria = "";
                var pFolio = "";
                var pAI = 0;

                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;

                if ($('#gs_Folio').val() != null) { pFolio = $("#gs_Folio").val(); }

                if ($('#gs_NombreCuentaBancaria').val() != null) { pCuentaBancaria = $("#gs_NombreCuentaBancaria").val(); }

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
                    pCuentaBancaria: pCuentaBancaria,
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

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarEgresos", function() {
        ObtenerFormaAgregarEgresos();
    });

    $("#dialogEditarEgresos, #dialogAgregarEgresos").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        var Egresos = new Object();

        if ($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("IdEgresos") != null && $("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("IdEgresos") != "") {
            Egresos.pIdEgresos = parseInt($("#divFormaEditarEgresos,#divFormaAgregarEgresos").attr("IdEgresos"));
            Egresos.pIdProveedor = parseInt($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("IdProveedor"));
            
            Egresos.IdTipoMoneda = $("#cmbTipoMoneda").val();
            Egresos.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
                if(Egresos.IdTipoMoneda == 2)
                {
                    Egresos.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    Egresos.TipoCambio = "1";
                }

                if (Egresos.TipoCambio.replace(" ", "") == "") {
                    Egresos.TipoCambio = 0;
                }
                
                if (Egresos.TipoCambioDOF.replace(" ", "") == "") {
                    Egresos.TipoCambioDOF = 0;
                }
            
            
            var validacion = ValidaAsociacionDocumentos(Egresos);
            if (validacion != "")
            { MostrarMensajeError(validacion); return false; }
            var oRequest = new Object();
            oRequest.Egresos = Egresos;
            ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
        }
        else {
            AgregarEgresosEdicion();
        }
    });

    $("#grdEgresos").on("click", ".imgFormaConsultarEgresos", function() {
        var registro = $(this).parents("tr");
        var Egresos = new Object();
        Egresos.pIdEgresos = parseInt($(registro).children("td[aria-describedby='grdEgresos_IdEgresos']").html());
        ObtenerFormaConsultarEgresos(JSON.stringify(Egresos));
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
        var pEgresosEncabezadoFacturaProveedor = new Object();
        pEgresosEncabezadoFacturaProveedor.pIdEgresosEncabezadoFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdEgresosEncabezadoFacturaProveedor']").html());
        var oRequest = new Object();
        oRequest.pEgresosEncabezadoFacturaProveedor = pEgresosEncabezadoFacturaProveedor;
        SetEliminarEgresosEncabezadoFacturaProveedor(JSON.stringify(oRequest));
    });

    $("#grdCuentaBancaria").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");
    });

    $('#grdEgresos').one('click', '.div_grdEgresos_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdEgresos_AI']").children().attr("baja")
        var idEgresos = $(registro).children("td[aria-describedby='grdEgresos_IdEgresos']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idEgresos, baja);
    });

    $("#dialogAgregarEgresos, #dialogEditarEgresos").on("click", "#divBuscarCuentasContables", function() {
        $("#divFormaCuentaBancaria").obtenerVista({
            nombreTemplate: "tmplMuestraCuentasContables.html",
            despuesDeCompilar: function() {
                FiltroCuentaBancaria();
                $("#dialogMuestraCuentasBancarias").dialog("open");
            }
        });
    });

    $("#dialogAgregarEgresos, #dialogEditarEgresos").on("click", "#chkConciliado", function() {
        $("#txtFechaConciliacion").prop('disabled', function () {
           return ! $(this).prop('disabled');
        });
    });

    $('#dialogAgregarEgresos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarEgresos").remove();
        },
        buttons: {
            "Guardar": function() {
                if ($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("IdEgresos") == null || $("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("IdEgresos") == "") {
                    AgregarEgresos();
                }
                else {
                    EditarEgresos();
                }
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarEgresos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarEgresos").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarEgresos').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarEgresos").remove();
        },
        buttons: {
            "Editar": function() {
                EditarEgresos();
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
    var Egresos = new Object();       
    Egresos.pIdEgresos= parseInt($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idEgresos"));
    ObtenerHabilitaMonto(JSON.stringify(Egresos));    
}

function ObtenerHabilitaMonto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/ObtenerHabilitaMonto",
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
                
                if (respuesta.Modelo.puedeEditarTipoCambioEgresos == 0 || parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares))
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

function ObtenerFormaFiltrosEgresos() {
    $("#divFiltrosEgresos").obtenerVista({
        nombreTemplate: "tmplFiltrosEgresos.html",
        url: "Egresos.aspx/ObtenerFormaFiltroEgresos",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroEgresos();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroEgresos();
                    }
                });
            }

            $('#divFiltrosEgresos').on('click', '#chkPorFecha', function(event) {
                FiltroEgresos();
            });

        }
    });
}


//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarEgresos() {
    $("#dialogAgregarEgresos").obtenerVista({
        nombreTemplate: "tmplAgregarEgresos.html",
        url: "Egresos.aspx/ObtenerFormaAgregarEgresos",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarEgresos").dialog("open");
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
            AutocompletarProveedor();
            
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambioEgresos == 1)
            {
                $("#txtTipoCambio").removeAttr("disabled");
                
            }
            else
            {
                $("#txtTipoCambio").attr("disabled", "disabled");
            }
            
            $('#dialogAgregarEgresos').on('focusout', '#txtImporte', function(event) {
                $("#txtImporte").val(formato.moneda(QuitarFormatoNumero($("#txtImporte").val()), "$"));
            });
            
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaAsociarDocumentos(Egresos) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentosProveedor.html",
        url: "Egresos.aspx/ObtenerFormaAsociarDocumentos",
        parametros: Egresos,
        despuesDeCompilar: function(pRespuesta) {
            FiltroFacturas();
            FiltroMovimientosCobros();
            $("#dialogMuestraAsociarDocumentos").dialog("open");
        }
    });
}

function ObtenerFormaConsultarEgresos(pIdEgresos) {
    $("#dialogConsultarEgresos").obtenerVista({
        nombreTemplate: "tmplConsultarEgresos.html",
        url: "Egresos.aspx/ObtenerFormaEgresos",
        parametros: pIdEgresos,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarEgresos == 1) {
                $("#dialogConsultarEgresos").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Egresos = new Object();
                        Egresos.IdEgresos = parseInt($("#divFormaConsultarEgresos").attr("IdEgresos"));
                        ObtenerFormaEditarEgresos(JSON.stringify(Egresos))
                    }
                });
                $("#dialogConsultarEgresos").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarEgresos").dialog("option", "buttons", {});
                $("#dialogConsultarEgresos").dialog("option", "height", "auto");
            }
            $("#dialogConsultarEgresos").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaEditarEgresos(IdEgresos) {
    $("#dialogEditarEgresos").obtenerVista({
        nombreTemplate: "tmplEditarEgresos.html",
        url: "Egresos.aspx/ObtenerFormaEditarEgresos",
        parametros: IdEgresos,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosEditar();
            $("#dialogEditarEgresos").dialog("option", "height", "auto");
            $("#dialogEditarEgresos").dialog("open");
            AutocompletarProveedor();
            $("#tabAsignarDocumentosEditar").tabs();
            $("#txtFechaConciliacion").datepicker();
        }
    });
}

function ObtenerDatosCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/ObtenerDatosCuentaBancaria",
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
                $("#divFormaAgregarEgresos, #divFormaEditarEgresos").attr("idCuentaBancaria", respuesta.Modelo.IdCuentaBancaria);
                $("#cmbTipoMoneda option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFechaAplicacion").val(respuesta.Modelo.Fecha);
                
                if (parseInt(respuesta.Modelo.Permisos.puedeEditarTipoCambioEgresos) == 0)
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

function FiltroEgresos() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdEgresos').getGridParam('rowNum');
    request.pPaginaActual = $('#grdEgresos').getGridParam('page');
    request.pColumnaOrden = $('#grdEgresos').getGridParam('sortname');
    request.pTipoOrden = $('#grdEgresos').getGridParam('sortorder');
    request.pFolio = "";
    request.pAI = 0;
    request.pRazonSocial = "";
    request.pCuentaBancaria = "";

    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;
    request.pReferencia = "";
    request.pImporte = 0;

    if ($('#gs_Folio').val() != null) { request.pFolio = $("#gs_Folio").val(); }

    if ($('#gs_NombreCuentaBancaria').val() != null) { request.pCuentaBancaria = $("#gs_NombreCuentaBancaria").val(); }

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
    
    if ($('#gs_Referencia').val() != null) { request.pReferencia = $("#gs_Referencia").val(); }
    
    if ($('#gs_Importe').val() != null && $('#gs_Importe').val() != "") { 
        var Importe = $("#gs_Importe").val().replace("$", "").replace(",", "");
        request.pImporte = parseFloat(Importe);  
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Egresos.aspx/ObtenerEgresos',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdEgresos')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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
        url: 'Egresos.aspx/ObtenerCuentaBancaria',
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
    request.pNumeroFactura = "";
    
    request.pIdProveedor = 0;
    if ($("#divFormaEditarEgresos, #divFormaAsociarDocumentos").attr("IdProveedor") != null) {
        request.pIdProveedor = $("#divFormaEditarEgresos, #divFormaAsociarDocumentos").attr("IdProveedor");
        if ($('#divContGridAsociarDocumento').find(gs_NumeroFactura).existe()) {
            request.pNumeroFactura = $('#divContGridAsociarDocumento').find(gs_NumeroFactura).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Egresos.aspx/ObtenerFacturas',
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
    request.pIdEgresos = 0;
    if ($("#divFormaEditarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos") != null) {
        request.pIdEgresos = $("#divFormaEditarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Egresos.aspx/ObtenerMovimientosCobros',
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
    request.pIdEgresos = 0;
    if ($("#divFormaEditarEgresos, #divFormaConsultarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos") != null) {
        request.pIdEgresos = $("#divFormaEditarEgresos, #divFormaConsultarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Egresos.aspx/ObtenerMovimientosCobrosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
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
    request.pIdEgresos = 0;
    if ($("#divFormaEditarEgresos, #divFormaConsultarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos") != null) {
        request.pIdEgresos = $("#divFormaEditarEgresos, #divFormaConsultarEgresos, #divFormaAsociarDocumentos").attr("IdEgresos");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Egresos.aspx/ObtenerMovimientosCobrosEditar',
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

function ObtenerTipoCambioDiarioOficial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/ObtenerTipoCambioDiarioOficial",
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

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarEgresos() {
    var pEgresos = new Object();
    pEgresos.IdEgresos = 0;
    pEgresos.IdCuentaBancaria = $("#divFormaAgregarEgresos").attr("idCuentaBancaria");
    if ($("#divFormaAgregarEgresos").attr("idProveedor") == "") {
        pEgresos.IdProveedor = 0;
    }
    else {
        pEgresos.IdProveedor = $("#divFormaAgregarEgresos").attr("idProveedor");
    }
    pEgresos.CuentaBancaria = $("#txtCuenta").val();
    pEgresos.IdMetodoPago = $("#cmbMetodoPago").val();
    pEgresos.Fecha = $("#txtFecha").val();
    pEgresos.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pEgresos.Referencia = $("#txtReferencia").val();
    pEgresos.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pEgresos.FechaAplicacion = $("#txtFechaAplicacion").val();
    pEgresos.FechaConciliacion = $("#txtFechaConciliacion").val();
    pEgresos.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pEgresos.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pEgresos.IdTipoMoneda == 2)
    {
        pEgresos.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pEgresos.TipoCambio = "1";
    }

    if (pEgresos.TipoCambio.replace(" ", "") == "") {
        pEgresos.TipoCambio = 0;
    }
    
    if (pEgresos.TipoCambioDOF.replace(" ", "") == "") {
        pEgresos.TipoCambioDOF = 0;
    }
    
    if ($("#chkConciliado").is(':checked')) {
        pEgresos.Conciliado = 1;
    }
    else {
        pEgresos.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pEgresos.Asociado = 1;
    }
    else {
        pEgresos.Asociado = 0;
    }
    
    var validacion = ValidaEgresos(pEgresos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEgresos = pEgresos;
    SetAgregarEgresos(JSON.stringify(oRequest));
}

function AgregarEgresosEdicion() {
    var pEgresos = new Object();
    pEgresos.IdCuentaBancaria = $("#divFormaAgregarEgresos").attr("idCuentaBancaria");
    if ($("#divFormaAgregarEgresos").attr("idProveedor") == "") {
        pEgresos.IdProveedor = 0;
    }
    else {
        pEgresos.IdProveedor = $("#divFormaAgregarEgresos").attr("idProveedor");
    }
    pEgresos.CuentaBancaria = $("#txtCuenta").val();
    pEgresos.IdMetodoPago = $("#cmbMetodoPago").val();
    pEgresos.Fecha = $("#txtFecha").val();
    pEgresos.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pEgresos.Referencia = $("#txtReferencia").val();
    pEgresos.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pEgresos.FechaAplicacion = $("#txtFechaAplicacion").val();
    pEgresos.FechaConciliacion = $("#txtFechaConciliacion").val();
    pEgresos.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pEgresos.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pEgresos.IdTipoMoneda == 2)
    {
        pEgresos.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pEgresos.TipoCambio = "1";
    }

    if (pEgresos.TipoCambio.replace(" ", "") == "") {
        pEgresos.TipoCambio = 0;
    }
    
    if (pEgresos.TipoCambioDOF.replace(" ", "") == "") {
        pEgresos.TipoCambioDOF = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pEgresos.Conciliado = 1;
    }
    else {
        pEgresos.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pEgresos.Asociado = 1;
    }
    else {
        pEgresos.Asociado = 0;
    }

    var validacion = ValidaEgresosEdicion(pEgresos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEgresos = pEgresos;
    SetAgregarEgresosEdicion(JSON.stringify(oRequest));
}

function SetAgregarEgresos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/AgregarEgresos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEgresos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarEgresos").dialog("close");
        }
    });
}

function SetAgregarEgresosEdicion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/AgregarEgresosEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarEgresos").attr("idEgresos", respuesta.IdEgresos);
                $("#txtCuenta").attr("disabled", "true");
                $("#cmbMetodoPago").attr("disabled", "true");
                $("#txtFecha").attr("disabled", "true");
                $("#txtRazonSocial").attr("disabled", "true");
                $("#txtFechaAplicacion").attr("disabled", "true");
                $("#chkAsociado").attr("disabled", "true");
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFolio").val(respuesta.Folio);
                $("#grdEgresos").trigger("reloadGrid");
                var Egresos = new Object();
                Egresos.pIdEgresos = parseInt($("#divFormaEditarEgresos,#divFormaAgregarEgresos").attr("IdEgresos"));
                Egresos.pIdProveedor = parseInt($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("IdProveedor"));
                Egresos.IdTipoMoneda = $("#cmbTipoMoneda").val();
                Egresos.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
                
                if(Egresos.IdTipoMoneda == 2)
                {
                    Egresos.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    Egresos.TipoCambio = "1";
                }

                if (Egresos.TipoCambio.replace(" ", "") == "") {
                    Egresos.TipoCambio = 0;
                }
                
                if (Egresos.TipoCambioDOF.replace(" ", "") == "") {
                    Egresos.TipoCambioDOF = 0;
                }
                
                var validacion = ValidaAsociacionDocumentos(Egresos);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.Egresos = Egresos;
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

function SetCambiarEstatus(pIdEgresos, pBaja) {
    var pRequest = "{'pIdEgresos':" + pIdEgresos + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEgresos").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            $('#grdEgresos').one('click', '.div_grdEgresos_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdEgresos_AI']").children().attr("baja")
                var idEgresos = $(registro).children("td[aria-describedby='grdEgresos_IdEgresos']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idEgresos, baja);
            });
        }
    });
}

function EditarEgresos() {
    var pEgresos = new Object();
    pEgresos.IdEgresos = $("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idEgresos");
    if ($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idProveedor") != null) {
        pEgresos.IdProveedor = $("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idProveedor");
    }
    else {
        pEgresos.IdProveedor = 0;
    }

    pEgresos.CuentaBancaria = $("#txtCuenta").val();
    pEgresos.IdMetodoPago = $("#cmbMetodoPago").val();
    pEgresos.Fecha = $("#txtFecha").val();
    pEgresos.Folio = $("#txtFolio").val();
    pEgresos.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pEgresos.Referencia = $("#txtReferencia").val();
    pEgresos.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pEgresos.FechaAplicacion = $("#txtFechaAplicacion").val();
    pEgresos.FechaConciliacion = $("#txtFechaConciliacion").val();
    pEgresos.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pEgresos.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pEgresos.IdTipoMoneda == 2)
    {
        pEgresos.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pEgresos.TipoCambio = "1";
    }

    if (pEgresos.TipoCambio.replace(" ", "") == "") {
        pEgresos.TipoCambio = 0;
    }
    
    if (pEgresos.TipoCambioDOF.replace(" ", "") == "") {
        pEgresos.TipoCambioDOF = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pEgresos.Conciliado = 1;
    }
    else {
        pEgresos.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pEgresos.Asociado = 1;
    }
    else {
        pEgresos.Asociado = 0;
    }
    
    var validacion = ValidaEgresos(pEgresos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEgresos = pEgresos;
    SetEditarEgresos(JSON.stringify(oRequest));
}
function EditarEgresosProveedor() {
    var pEgresos = new Object();
    pEgresos.IdEgresos = $("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idEgresos");
    if ($("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idProveedor") != null) {
        pEgresos.IdProveedor = $("#divFormaEditarEgresos, #divFormaAgregarEgresos").attr("idProveedor");
    }
    else {
        pEgresos.IdProveedor = 0;
    }
    
    var oRequest = new Object();
    oRequest.pEgresos = pEgresos;
    SetEditarEgresosProveedor(JSON.stringify(oRequest));
}
function SetEditarEgresos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/EditarEgresos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdEgresos").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarEgresos, #dialogAgregarEgresos").dialog("close");
        }
    });
}

function SetEditarEgresosProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/EditarEgresosProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdEgresos").trigger("reloadGrid");
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function EdicionFacturas(valor, id, rowid, iCol) {

    var TipoMoneda = $('#grdFacturas').getCell(rowid, 'TipoMoneda');
    var Saldo = $('#grdFacturas').getCell(rowid, 'Saldo');
    var Egresos = new Object();
    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        Egresos.IdTipoMoneda = 1;
        Egresos.TipoCambio = 1;
        Egresos.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        Egresos.IdTipoMoneda = 2;
        Egresos.TipoCambio = $("#spanTipoCambioDolares").text();
        Egresos.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    Egresos.TipoMoneda = TipoMoneda;
    Egresos.Monto = QuitarFormatoNumero(valor);
    Egresos.Saldo = QuitarFormatoNumero(Saldo);
    Egresos.IdEncabezadoFacturaProveedor = id;
    Egresos.rowid = rowid;
    Egresos.IdEgresos = $("#divFormaAsociarDocumentos").attr("idEgresos");
    var validacion = ValidarMontos(Egresos);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pEgresos = Egresos;
    SetEditarMontos(JSON.stringify(oRequest));
    
}
function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdEgresos").trigger("reloadGrid");
               
                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosEgresos;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosEgresos / $("#spanTipoCambioDolares").text());
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

function SetEliminarEgresosEncabezadoFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Egresos.aspx/EliminarEgresosEncabezadoFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");
                $("#grdEgresos").trigger("reloadGrid");          

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosEgresos;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosEgresos / $("#spanTipoCambioDolares").text());
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

function AutocompletarProveedor() {

    $('#txtRazonSocial').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtRazonSocial').val();
            $.ajax({
                type: 'POST',
                url: 'Egresos.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarEgresos, #divFormaEditarEgresos").attr("idProveedor", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdProveedor }
                    }));
                }
            });
        },
        minLength: 1,
        select: function(event, ui) {
            var pIdProveedor = ui.item.id;
            $("#divFormaAgregarEgresos, #divFormaEditarEgresos").attr("idProveedor", pIdProveedor);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

//-----Validaciones------------------------------------------------------

function ValidaEgresos(pEgresos) {
    var errores = "";

    if (pEgresos.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pEgresos.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pEgresos.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pEgresos.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pEgresos.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pEgresos.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pEgresos.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pEgresos.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pEgresos.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }
    
    if(pEgresos.IdEgresos == 0)
    {
        if (parseFloat(pEgresos.TipoCambio, 10) == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }
    }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaEgresosEdicion(pEgresos) {
    var errores = "";

    if (pEgresos.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pEgresos.IdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

    if (pEgresos.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pEgresos.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pEgresos.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pEgresos.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pEgresos.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pEgresos.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pEgresos.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pEgresos.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }
    
    if (parseFloat(pEgresos.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(Egresos) {
    var errores = "";

    if (Egresos.pIdEgresos == 0)
    { errores = errores + "<span>*</span> No hay cobro por asociar, favor de elegir alguno.<br />"; }
    
    if (Egresos.pIdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(Egresos) {
    var errores = "";

    if (Egresos.IdEncabezadoFacturaProveedor == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(Egresos.Monto) > parseFloat(Egresos.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al disponible.<br />"; }

    if (parseFloat(Egresos.Monto) > parseFloat(Egresos.Saldo))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(Egresos.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(Egresos.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

