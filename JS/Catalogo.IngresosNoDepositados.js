//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("IngresosNoDepositados");
    });

    ObtenerFormaFiltrosIngresosNoDepositados();

    //////funcion del grid//////
    $("#gbox_grdIngresosNoDepositados").livequery(function() {
        $("#grdIngresosNoDepositados").jqGrid('navButtonAdd', '#pagIngresosNoDepositados', {
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

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarIngresosNoDepositados", function() {
        ObtenerFormaAgregarIngresosNoDepositados();
    });

    $("#dialogEditarIngresosNoDepositados, #dialogAgregarIngresosNoDepositados").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
    
        if ($("#chkDepositado").is(':checked')) 
        {
            var IngresosNoDepositados = new Object();

            if ($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("IdIngresosNoDepositados") != null && $("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("IdIngresosNoDepositados") != "") {
                IngresosNoDepositados.pIdIngresosNoDepositados = parseInt($("#divFormaEditarIngresosNoDepositados,#divFormaAgregarIngresosNoDepositados").attr("IdIngresosNoDepositados"));
                IngresosNoDepositados.pIdCliente = parseInt($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("IdCliente"));
                
                IngresosNoDepositados.IdTipoMoneda = $("#cmbTipoMoneda").val();
                IngresosNoDepositados.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
        
                    if(IngresosNoDepositados.IdTipoMoneda == 2)
                    {
                        IngresosNoDepositados.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                    }
                    else
                    {
                        IngresosNoDepositados.TipoCambio = "1";
                    }

                    if (IngresosNoDepositados.TipoCambio.replace(" ", "") == "") {
                        IngresosNoDepositados.TipoCambio = 0;
                    }
                    
                    if (IngresosNoDepositados.TipoCambioDOF.replace(" ", "") == "") {
                        IngresosNoDepositados.TipoCambioDOF = 0;
                    }
                
                var validacion = ValidaAsociacionDocumentos(IngresosNoDepositados);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.IngresosNoDepositados = IngresosNoDepositados;
                ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
            }
            else {
                AgregarIngresosNoDepositadosEdicion();
            }
        }
        else
        {
            MostrarMensajeError("No se pueden asociar documentos hasta que se realice el deposito"); return false; 
        }
    });

    $("#grdIngresosNoDepositados").on("click", ".imgFormaConsultarIngresosNoDepositados", function() {
        var registro = $(this).parents("tr");
        var IngresosNoDepositados = new Object();
        IngresosNoDepositados.pIdIngresosNoDepositados = parseInt($(registro).children("td[aria-describedby='grdIngresosNoDepositados_IdIngresosNoDepositados']").html());
        ObtenerFormaConsultarIngresosNoDepositados(JSON.stringify(IngresosNoDepositados));
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
        var pIngresosNoDepositadosEncabezadoFactura = new Object();
        pIngresosNoDepositadosEncabezadoFactura.pIdIngresosNoDepositadosEncabezadoFactura = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdIngresosNoDepositadosEncabezadoFactura']").html());
        var oRequest = new Object();
        oRequest.pIngresosNoDepositadosEncabezadoFactura = pIngresosNoDepositadosEncabezadoFactura;
        SetEliminarIngresosNoDepositadosEncabezadoFactura(JSON.stringify(oRequest));
    });

    $("#grdCuentaBancaria").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");
    });

    $('#grdIngresosNoDepositados').one('click', '.div_grdIngresosNoDepositados_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdIngresosNoDepositados_AI']").children().attr("baja")
        var idIngresosNoDepositados = $(registro).children("td[aria-describedby='grdIngresosNoDepositados_IdIngresosNoDepositados']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idIngresosNoDepositados, baja);
    });

    $("#dialogAgregarIngresosNoDepositados, #dialogEditarIngresosNoDepositados").on("click", "#divBuscarCuentasContables", function() {
        $("#divFormaCuentaBancaria").obtenerVista({
            nombreTemplate: "tmplMuestraCuentasContables.html",
            despuesDeCompilar: function() {
                FiltroCuentaBancaria();
                $("#dialogMuestraCuentasBancarias").dialog("open");
            }
        });
    });

    $('#dialogAgregarIngresosNoDepositados').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarIngresosNoDepositados").remove();
        },
        buttons: {
            "Guardar": function() {
                if ($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("IdIngresosNoDepositados") == null || $("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("IdIngresosNoDepositados") == "") {
                    AgregarIngresosNoDepositados();
                }
                else {
                    EditarIngresosNoDepositados();
                }
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarIngresosNoDepositados').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarIngresosNoDepositados").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarIngresosNoDepositados').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarIngresosNoDepositados").remove();
        },
        buttons: {
            "Editar": function() {
                EditarIngresosNoDepositados();
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
    var IngresosNoDepositados = new Object();       
    IngresosNoDepositados.pIdIngresosNoDepositados = parseInt($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idIngresosNoDepositados"));
    ObtenerHabilitaMonto(JSON.stringify(IngresosNoDepositados));    
}

function ObtenerHabilitaMonto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/ObtenerHabilitaMonto",
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
                
                if (respuesta.Modelo.puedeEditarTipoCambioIngresosNoDepositados == 0 || parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares))
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

function ObtenerFormaFiltrosIngresosNoDepositados() {
    $("#divFiltrosIngresosNoDepositados").obtenerVista({
        nombreTemplate: "tmplFiltrosIngresosNoDepositados.html",
        url: "IngresosNoDepositados.aspx/ObtenerFormaFiltroIngresosNoDepositados",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroIngresosNoDepositados();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroIngresosNoDepositados();
                    }
                });
            }

            $('#divFiltrosIngresosNoDepositados').on('click', '#chkPorFecha', function(event) {
                FiltroIngresosNoDepositados();
            });

        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarIngresosNoDepositados() {
    $("#dialogAgregarIngresosNoDepositados").obtenerVista({
        nombreTemplate: "tmplAgregarIngresosNoDepositados.html",
        url: "IngresosNoDepositados.aspx/ObtenerFormaAgregarIngresosNoDepositados",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarIngresosNoDepositados").dialog("open");
            $("#txtFecha").datepicker({
                maxDate: new Date()
            });
            $("#txtFechaPago").datepicker({
                maxDate: new Date(),
                onSelect: function() {
                var pRequest = new Object();
                pRequest.pIdTipoMonedaDestino = 2;
                pRequest.pFecha = $("#txtFechaPago").val(); ;
                ObtenerTipoCambioDiarioOficial(JSON.stringify(pRequest));
                }
            });
            $("#txtFechaDeposito").datepicker();
            AutocompletarCliente();
            
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambioIngresosNoDepositados == 1)
            {
                $("#txtTipoCambio").removeAttr("disabled");
                
            }
            else
            {
                $("#txtTipoCambio").attr("disabled", "disabled");
            }
            
            $('#dialogAgregarIngresosNoDepositados').on('focusout', '#txtImporte', function(event) {
                $("#txtImporte").val(formato.moneda(QuitarFormatoNumero($("#txtImporte").val()), "$"));
            });
            
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaAsociarDocumentos(IngresosNoDepositados) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentosIngresosNoDepositados.html",
        url: "IngresosNoDepositados.aspx/ObtenerFormaAsociarDocumentos",
        parametros: IngresosNoDepositados,
        despuesDeCompilar: function(pRespuesta) {
            FiltroFacturas();
            FiltroMovimientosCobros();
            $("#dialogMuestraAsociarDocumentos").dialog("open");
        }
    });
}

function ObtenerFormaConsultarIngresosNoDepositados(pIdIngresosNoDepositados) {
    $("#dialogConsultarIngresosNoDepositados").obtenerVista({
        nombreTemplate: "tmplConsultarIngresosNoDepositados.html",
        url: "IngresosNoDepositados.aspx/ObtenerFormaIngresosNoDepositados",
        parametros: pIdIngresosNoDepositados,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarIngresosNoDepositados == 1) {
                $("#dialogConsultarIngresosNoDepositados").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var IngresosNoDepositados = new Object();
                        IngresosNoDepositados.IdIngresosNoDepositados = parseInt($("#divFormaConsultarIngresosNoDepositados").attr("IdIngresosNoDepositados"));
                        ObtenerFormaEditarIngresosNoDepositados(JSON.stringify(IngresosNoDepositados))
                    }
                });
                $("#dialogConsultarIngresosNoDepositados").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarIngresosNoDepositados").dialog("option", "buttons", {});
                $("#dialogConsultarIngresosNoDepositados").dialog("option", "height", "auto");
            }
            $("#dialogConsultarIngresosNoDepositados").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaEditarIngresosNoDepositados(IdIngresosNoDepositados) {
    $("#dialogEditarIngresosNoDepositados").obtenerVista({
        nombreTemplate: "tmplEditarIngresosNoDepositados.html",
        url: "IngresosNoDepositados.aspx/ObtenerFormaEditarIngresosNoDepositados",
        parametros: IdIngresosNoDepositados,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosEditar();
            $("#dialogEditarIngresosNoDepositados").dialog("option", "height", "auto");
            $("#dialogEditarIngresosNoDepositados").dialog("open");
            AutocompletarCliente();
            $("#tabAsignarDocumentosEditar").tabs();
        }
    });
}

function ObtenerDatosCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/ObtenerDatosCuentaBancaria",
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
                $("#divFormaAgregarIngresosNoDepositados, #divFormaEditarIngresosNoDepositados").attr("idCuentaBancaria", respuesta.Modelo.IdCuentaBancaria);
                $("#cmbTipoMoneda option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFechaDeposito").val(respuesta.Modelo.Fecha);
                
                if (parseInt(respuesta.Modelo.Permisos.puedeEditarTipoCambioIngresosNoDepositados) == 0)
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
        url: "IngresosNoDepositados.aspx/ObtenerTipoCambioDiarioOficial",
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

function FiltroIngresosNoDepositados() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdIngresosNoDepositados').getGridParam('rowNum');
    request.pPaginaActual = $('#grdIngresosNoDepositados').getGridParam('page');
    request.pColumnaOrden = $('#grdIngresosNoDepositados').getGridParam('sortname');
    request.pTipoOrden = $('#grdIngresosNoDepositados').getGridParam('sortorder');
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
        url: 'IngresosNoDepositados.aspx/ObtenerIngresosNoDepositados',
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

function FiltroCuentaBancaria() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCuentaBancaria').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCuentaBancaria').getGridParam('page');
    request.pColumnaOrden = $('#grdCuentaBancaria').getGridParam('sortname');
    request.pTipoOrden = $('#grdCuentaBancaria').getGridParam('sortorder');
    request.pCuentaBancaria= "";
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'IngresosNoDepositados.aspx/ObtenerCuentaBancaria',
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
    if ($("#divFormaEditarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdCliente") != null) {
        request.pIdCliente = $("#divFormaEditarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdCliente");
        if ($('#divContGridAsociarDocumento').find(gs_Serie).existe()) {
            request.pSerie = $('#divContGridAsociarDocumento').find(gs_Serie).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'IngresosNoDepositados.aspx/ObtenerFacturas',
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
    request.pIdIngresosNoDepositados = 0;
    if ($("#divFormaEditarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdIngresosNoDepositados") != null) {
        request.pIdIngresosNoDepositados = $("#divFormaEditarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdIngresosNoDepositados");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'IngresosNoDepositados.aspx/ObtenerMovimientosCobros',
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
    request.pIdIngresosNoDepositados = 0;
    if ($("#divFormaEditarIngresosNoDepositados, #divFormaConsultarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdIngresosNoDepositados") != null) {
        request.pIdIngresosNoDepositados = $("#divFormaEditarIngresosNoDepositados, #divFormaConsultarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdIngresosNoDepositados");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'IngresosNoDepositados.aspx/ObtenerMovimientosCobrosConsultar',
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
    request.pIdIngresosNoDepositados = 0;
    if ($("#divFormaEditarIngresosNoDepositados, #divFormaConsultarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdIngresosNoDepositados") != null) {
        request.pIdIngresosNoDepositados = $("#divFormaEditarIngresosNoDepositados, #divFormaConsultarIngresosNoDepositados, #divFormaAsociarDocumentos").attr("IdIngresosNoDepositados");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'IngresosNoDepositados.aspx/ObtenerMovimientosCobrosEditar',
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
function AgregarIngresosNoDepositados() {
    var pIngresosNoDepositados = new Object();
    pIngresosNoDepositados.IdCuentaBancaria = $("#divFormaAgregarIngresosNoDepositados").attr("idCuentaBancaria");
    if ($("#divFormaAgregarIngresosNoDepositados").attr("idCliente") == "") {
        pIngresosNoDepositados.IdCliente = 0;
    }
    else {
        pIngresosNoDepositados.IdCliente = $("#divFormaAgregarIngresosNoDepositados").attr("idCliente");
    }
    pIngresosNoDepositados.CuentaBancaria = $("#txtCuenta").val();
    pIngresosNoDepositados.IdMetodoPago = $("#cmbMetodoPago").val();
    pIngresosNoDepositados.Fecha = $("#txtFecha").val();
    pIngresosNoDepositados.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pIngresosNoDepositados.Referencia = $("#txtReferencia").val();
    pIngresosNoDepositados.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pIngresosNoDepositados.FechaDeposito = $("#txtFechaDeposito").val();
    pIngresosNoDepositados.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pIngresosNoDepositados.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    pIngresosNoDepositados.FechaPago = $("#txtFechaPago").val();
    
   if(pIngresosNoDepositados.IdTipoMoneda == 2)
    {
        pIngresosNoDepositados.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pIngresosNoDepositados.TipoCambio = "1";
    }

    if (pIngresosNoDepositados.TipoCambio.replace(" ", "") == "") {
        pIngresosNoDepositados.TipoCambio = 0;
    }
    
    if (pIngresosNoDepositados.TipoCambioDOF.replace(" ", "") == "") {
        pIngresosNoDepositados.TipoCambioDOF = 0;
    }

    if ($("#chkDepositado").is(':checked')) {
        pIngresosNoDepositados.Depositado = 1;
    }
    else {
        pIngresosNoDepositados.Depositado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pIngresosNoDepositados.Asociado = 1;
    }
    else {
        pIngresosNoDepositados.Asociado = 0;
    }
    
    var validacion = ValidaIngresosNoDepositados(pIngresosNoDepositados);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pIngresosNoDepositados = pIngresosNoDepositados;
    SetAgregarIngresosNoDepositados(JSON.stringify(oRequest));
}

function AgregarIngresosNoDepositadosEdicion() {
    var pIngresosNoDepositados = new Object();
    pIngresosNoDepositados.IdCuentaBancaria = $("#divFormaAgregarIngresosNoDepositados").attr("idCuentaBancaria");
    if ($("#divFormaAgregarIngresosNoDepositados").attr("idCliente") == "") {
        pIngresosNoDepositados.IdCliente = 0;
    }
    else {
        pIngresosNoDepositados.IdCliente = $("#divFormaAgregarIngresosNoDepositados").attr("idCliente");
    }
    pIngresosNoDepositados.CuentaBancaria = $("#txtCuenta").val();
    pIngresosNoDepositados.IdMetodoPago = $("#cmbMetodoPago").val();
    pIngresosNoDepositados.Fecha = $("#txtFecha").val();
    pIngresosNoDepositados.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pIngresosNoDepositados.Referencia = $("#txtReferencia").val();
    pIngresosNoDepositados.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pIngresosNoDepositados.FechaDeposito = $("#txtFechaDeposito").val();
    pIngresosNoDepositados.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pIngresosNoDepositados.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    pIngresosNoDepositados.FechaPago = $("#txtFechaPago").val();
    
    if(pIngresosNoDepositados.IdTipoMoneda == 2)
    {
        pIngresosNoDepositados.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pIngresosNoDepositados.TipoCambio = "1";
    }

    if (pIngresosNoDepositados.TipoCambio.replace(" ", "") == "") {
        pIngresosNoDepositados.TipoCambio = 0;
    }
    
    if (pIngresosNoDepositados.TipoCambioDOF.replace(" ", "") == "") {
        pIngresosNoDepositados.TipoCambioDOF = 0;
    }

    if ($("#chkDepositado").is(':checked')) {
        pIngresosNoDepositados.Depositado = 1;
    }
    else {
        pIngresosNoDepositados.Depositado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pIngresosNoDepositados.Asociado = 1;
    }
    else {
        pIngresosNoDepositados.Asociado = 0;
    }

    var validacion = ValidaIngresosNoDepositadosEdicion(pIngresosNoDepositados);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pIngresosNoDepositados = pIngresosNoDepositados;
    SetAgregarIngresosNoDepositadosEdicion(JSON.stringify(oRequest));
}

function SetAgregarIngresosNoDepositados(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/AgregarIngresosNoDepositados",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdIngresosNoDepositados").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarIngresosNoDepositados").dialog("close");
        }
    });
}

function SetAgregarIngresosNoDepositadosEdicion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/AgregarIngresosNoDepositadosEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarIngresosNoDepositados").attr("idIngresosNoDepositados", respuesta.IdIngresosNoDepositados);
                $("#txtCuenta").attr("disabled", "true");
                $("#cmbMetodoPago").attr("disabled", "true");
                $("#txtFecha").attr("disabled", "true");
                $("#txtRazonSocial").attr("disabled", "true");
                $("#txtFechaDeposito").attr("disabled", "true");
                $("#chkAsociado").attr("disabled", "true");
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFolio").val(respuesta.Folio);
                $("#grdIngresosNoDepositados").trigger("reloadGrid");
                var IngresosNoDepositados = new Object();
                IngresosNoDepositados.pIdIngresosNoDepositados = parseInt($("#divFormaEditarIngresosNoDepositados,#divFormaAgregarIngresosNoDepositados").attr("IdIngresosNoDepositados"));
                IngresosNoDepositados.pIdCliente = parseInt($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("IdCliente"));
                IngresosNoDepositados.IdTipoMoneda = $("#cmbTipoMoneda").val();
                IngresosNoDepositados.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
                
                if(IngresosNoDepositados.IdTipoMoneda == 2)
                {
                    IngresosNoDepositados.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    IngresosNoDepositados.TipoCambio = "1";
                }

                if (IngresosNoDepositados.TipoCambio.replace(" ", "") == "") {
                    IngresosNoDepositados.TipoCambio = 0;
                }
                
                if (IngresosNoDepositados.TipoCambioDOF.replace(" ", "") == "") {
                    IngresosNoDepositados.TipoCambioDOF = 0;
                }
                
                var validacion = ValidaAsociacionDocumentos(IngresosNoDepositados);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.IngresosNoDepositados = IngresosNoDepositados;
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

function SetCambiarEstatus(pIdIngresosNoDepositados, pBaja) {
    var pRequest = "{'pIdIngresosNoDepositados':" + pIdIngresosNoDepositados + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdIngresosNoDepositados").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            $('#grdIngresosNoDepositados').one('click', '.div_grdIngresosNoDepositados_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdIngresosNoDepositados_AI']").children().attr("baja")
                var idIngresosNoDepositados = $(registro).children("td[aria-describedby='grdIngresosNoDepositados_IdIngresosNoDepositados']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idIngresosNoDepositados, baja);
            });
        }
    });
}

function EditarIngresosNoDepositados() {
    var pIngresosNoDepositados = new Object();
    pIngresosNoDepositados.IdIngresosNoDepositados = $("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idIngresosNoDepositados");
    if ($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idCliente") != null) {
        pIngresosNoDepositados.IdCliente = $("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idCliente");
    }
    else {
        pIngresosNoDepositados.IdCliente = 0;
    }

    pIngresosNoDepositados.CuentaBancaria = $("#txtCuenta").val();
    pIngresosNoDepositados.IdMetodoPago = $("#cmbMetodoPago").val();
    pIngresosNoDepositados.Fecha = $("#txtFecha").val();
    pIngresosNoDepositados.Folio = $("#txtFolio").val();
    pIngresosNoDepositados.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pIngresosNoDepositados.Referencia = $("#txtReferencia").val();
    pIngresosNoDepositados.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pIngresosNoDepositados.FechaDeposito = $("#txtFechaDeposito").val();
    pIngresosNoDepositados.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pIngresosNoDepositados.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    pIngresosNoDepositados.FechaPago = $("#txtFechaPago").val();
    
    if(pIngresosNoDepositados.IdTipoMoneda == 2)
    {
        pIngresosNoDepositados.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pIngresosNoDepositados.TipoCambio = "1";
    }

    if (pIngresosNoDepositados.TipoCambio.replace(" ", "") == "") {
        pIngresosNoDepositados.TipoCambio = 0;
    }
    
    if (pIngresosNoDepositados.TipoCambioDOF.replace(" ", "") == "") {
        pIngresosNoDepositados.TipoCambioDOF = 0;
    }
    
    if ($("#chkDepositado").is(':checked')) {
        pIngresosNoDepositados.Depositado = 1;
    }
    else {
        pIngresosNoDepositados.Depositado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pIngresosNoDepositados.Asociado = 1;
    }
    else {
        pIngresosNoDepositados.Asociado = 0;
    }
    
    if(pIngresosNoDepositados.Depositado == 1)
    {
        MostrarMensajeError("No se puede editar este Ingreso no depositado, ya que se encuentra en estatus Depositado"); return false;
    }
    
    var validacion = ValidaIngresosNoDepositados(pIngresosNoDepositados);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pIngresosNoDepositados = pIngresosNoDepositados;
    SetEditarIngresosNoDepositados(JSON.stringify(oRequest));
}
function EditarIngresosNoDepositadosCliente() {
    var pIngresosNoDepositados = new Object();
    pIngresosNoDepositados.IdIngresosNoDepositados = $("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idIngresosNoDepositados");
    if ($("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idCliente") != null) {
        pIngresosNoDepositados.IdCliente = $("#divFormaEditarIngresosNoDepositados, #divFormaAgregarIngresosNoDepositados").attr("idCliente");
    }
    else {
        pIngresosNoDepositados.IdCliente = 0;
    }
    
    var oRequest = new Object();
    oRequest.pIngresosNoDepositados = pIngresosNoDepositados;
    SetEditarIngresosNoDepositadosCliente(JSON.stringify(oRequest));
}
function SetEditarIngresosNoDepositados(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/EditarIngresosNoDepositados",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdIngresosNoDepositados").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarIngresosNoDepositados, #dialogAgregarIngresosNoDepositados").dialog("close");
        }
    });
}

function SetEditarIngresosNoDepositadosCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/EditarIngresosNoDepositadosCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdIngresosNoDepositados").trigger("reloadGrid");
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

    var IngresosNoDepositados = new Object();

    if (EsParcialidad == true || EsParcialidad == "True") {
        IngresosNoDepositados.EsParcialidad = 1;
    }
    else {
        IngresosNoDepositados.EsParcialidad = 0;
    } 

    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        IngresosNoDepositados.IdTipoMoneda = 1;
        IngresosNoDepositados.TipoCambio = 1;
        IngresosNoDepositados.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        IngresosNoDepositados.IdTipoMoneda = 2;
        IngresosNoDepositados.TipoCambio = $("#spanTipoCambioDolares").text();
        IngresosNoDepositados.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    IngresosNoDepositados.TipoMoneda = TipoMoneda;

    IngresosNoDepositados.Monto = QuitarFormatoNumero(valor);
    IngresosNoDepositados.Saldo = QuitarFormatoNumero(Saldo);
    IngresosNoDepositados.IdEncabezadoFactura = id;
    IngresosNoDepositados.rowid = rowid;
    IngresosNoDepositados.IdIngresosNoDepositados = $("#divFormaAsociarDocumentos").attr("idIngresosNoDepositados");
    var validacion = ValidarMontos(IngresosNoDepositados);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pIngresosNoDepositados = IngresosNoDepositados;
    SetEditarMontos(JSON.stringify(oRequest));
    
}
function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/EditarMontos",
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
                $("#grdIngresosNoDepositados").trigger("reloadGrid");
                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosIngresosNoDepositados;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosIngresosNoDepositados / $("#spanTipoCambioDolares").text());
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

function SetEliminarIngresosNoDepositadosEncabezadoFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "IngresosNoDepositados.aspx/EliminarIngresosNoDepositadosEncabezadoFactura",
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
                $("#grdIngresosNoDepositados").trigger("reloadGrid");
                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosIngresosNoDepositados;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosIngresosNoDepositados / $("#spanTipoCambioDolares").text());
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
                url: 'IngresosNoDepositados.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarIngresosNoDepositados, #divFormaEditarIngresosNoDepositados").attr("idCliente", "0");
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
            $("#divFormaAgregarIngresosNoDepositados, #divFormaEditarIngresosNoDepositados").attr("idCliente", pIdCliente);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

//-----Validaciones------------------------------------------------------
function ValidaIngresosNoDepositados(pIngresosNoDepositados) {
    var errores = "";

    if (pIngresosNoDepositados.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pIngresosNoDepositados.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pIngresosNoDepositados.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pIngresosNoDepositados.FechaPago == "")
    { errores = errores + "<span>*</span> El campo fecha de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pIngresosNoDepositados.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pIngresosNoDepositados.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pIngresosNoDepositados.FechaDeposito == "")
    { errores = errores + "<span>*</span> El campo fecha de deposito esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pIngresosNoDepositados.FechaDeposito, 'aaaammdd') < ConvertirFecha(pIngresosNoDepositados.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de deposito no debe ser menor a la fecha de emisión.<br />"; }

    if (pIngresosNoDepositados.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (parseFloat(pIngresosNoDepositados.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaIngresosNoDepositadosEdicion(pIngresosNoDepositados) {
    var errores = "";

    if (pIngresosNoDepositados.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pIngresosNoDepositados.IdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pIngresosNoDepositados.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pIngresosNoDepositados.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }
    
    if (pIngresosNoDepositados.FechaPago == "")
    { errores = errores + "<span>*</span> El campo fecha de pago esta vacio, favor de seleccionarlo.<br />"; }


    if (pIngresosNoDepositados.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pIngresosNoDepositados.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pIngresosNoDepositados.FechaDeposito == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pIngresosNoDepositados.FechaDeposito, 'aaaammdd') < ConvertirFecha(pIngresosNoDepositados.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de deposito no debe ser menor a la fecha de emisión.<br />"; }

    if (pIngresosNoDepositados.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(IngresosNoDepositados) {
    var errores = "";

    if (IngresosNoDepositados.pIdIngresosNoDepositados == 0)
    { errores = errores + "<span>*</span> No hay cobro por asociar, favor de elegir alguno.<br />"; }
    
    if (IngresosNoDepositados.pIdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(IngresosNoDepositados) {
    var errores = "";

    if (IngresosNoDepositados.IdEncabezadoFactura == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(IngresosNoDepositados.Monto) > parseFloat(IngresosNoDepositados.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al dispoible.<br />"; }

    if (parseFloat(IngresosNoDepositados.Monto) > parseFloat(IngresosNoDepositados.Saldo))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(IngresosNoDepositados.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(IngresosNoDepositados.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

