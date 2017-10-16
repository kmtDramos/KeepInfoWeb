//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function() {
        ActualizarPanelControles("Cheques");
    });

    ObtenerFormaFiltrosCheques();

    //////funcion del grid//////
    $("#gbox_grdCheques").livequery(function() {
        $("#grdCheques").jqGrid('navButtonAdd', '#pagCheques', {
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
                    pRazonSocial: pRazonSocial,
                    pCuentaBancaria: pCuentaBancaria,
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

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarCheques", function() {
        ObtenerFormaAgregarCheques();
    });

    $("#dialogEditarCheques, #dialogAgregarCheques").on("click", "#btnObtenerFormaAsociarDocumentos", function() {
        if ($("#chkConciliado").is(':checked')  || true)
        {
            var Cheques = new Object();
            if ($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("IdCheques") != null && $("#divFormaEditarCheques, #divFormaAgregarCheques").attr("IdCheques") != "") {
                Cheques.pIdCheques = parseInt($("#divFormaEditarCheques,#divFormaAgregarCheques").attr("IdCheques"));
                Cheques.pIdProveedor = parseInt($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("IdProveedor"));
                
                Cheques.IdTipoMoneda = $("#cmbTipoMoneda").val();
                Cheques.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
                
                if ($("#chkConciliado").is(':checked')) {
                    Cheques.Conciliado = 1;
                }
                else {
                    Cheques.Conciliado = 0;
                }
                Cheques.FechaConciliacion = $("#txtFechaConciliacion").val();
                
                if(Cheques.IdTipoMoneda == 2)
                {
                    Cheques.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    Cheques.TipoCambio = "1";
                }

                if (Cheques.TipoCambio.replace(" ", "") == "") {
                    Cheques.TipoCambio = 0;
                }
                
                if (Cheques.TipoCambioDOF.replace(" ", "") == "") {
                    Cheques.TipoCambioDOF = 0;
                }
                
                var validacion = ValidaAsociacionDocumentos(Cheques);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.Cheques = Cheques;
                ObtenerFormaAsociarDocumentos(JSON.stringify(oRequest));
            }
            else {
                AgregarChequesEdicion();
            }
        }
        else
        {
            MostrarMensajeError("No se pueden asociar documentos hasta que el cheque este conciliado"); return false; 
        }
    });

    $("#grdCheques").on("click", ".imgFormaConsultarCheques", function() {
        var registro = $(this).parents("tr");
        var Cheques = new Object();
        Cheques.pIdCheques = parseInt($(registro).children("td[aria-describedby='grdCheques_IdCheques']").html());
        ObtenerFormaConsultarCheques(JSON.stringify(Cheques));
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
        var pChequesEncabezadoFacturaProveedor = new Object();
        pChequesEncabezadoFacturaProveedor.pIdChequesEncabezadoFacturaProveedor = parseInt($(registro).children("td[aria-describedby='grdMovimientosCobros_IdChequesEncabezadoFacturaProveedor']").html());
        var oRequest = new Object();
        oRequest.pChequesEncabezadoFacturaProveedor = pChequesEncabezadoFacturaProveedor;
        SetEliminarChequesEncabezadoFacturaProveedor(JSON.stringify(oRequest));
    });

    $("#grdCuentaBancaria").on("dblclick", "td", function() {
        var registro = $(this).parents("tr");
        var CuentaBancaria = new Object();
        CuentaBancaria.pIdCuentaBancaria = parseInt($(registro).children("td[aria-describedby='grdCuentaBancaria_IdCuentaBancaria']").html());
        ObtenerDatosCuentaBancaria(JSON.stringify(CuentaBancaria));
        $("#dialogMuestraCuentasBancarias").dialog("close");
    });

    $('#dialogAgregarCheques').on('change', '#txtImporte', function(event) {
        $("#spanCantidadLetra").text(covertirNumLetras(QuitarFormatoNumero($("#txtImporte").val()), $("#cmbTipoMoneda option:selected").text()));
    });

    $('#grdCheques').one('click', '.div_grdCheques_AI', function(event) {
        var registro = $(this).parents("tr");
        var estatusBaja = $(registro).children("td[aria-describedby='grdCheques_AI']").children().attr("baja")
        var idCheques = $(registro).children("td[aria-describedby='grdCheques_IdCheques']").html();
        var baja = "false";
        if (estatusBaja == "0" || estatusBaja == "False") {
            baja = "true";
        }
        SetCambiarEstatus(idCheques, baja);
    });

    $("#dialogAgregarCheques, #dialogEditarCheques").on("click", "#divBuscarCuentasContables", function() {
        $("#divFormaCuentaBancaria").obtenerVista({
            nombreTemplate: "tmplMuestraCuentasContables.html",
            despuesDeCompilar: function() {
                FiltroCuentaBancaria();
                $("#dialogMuestraCuentasBancarias").dialog("open");
            }
        });
    });

    
    $("#dialogAgregarCheques, #dialogEditarCheques").on("click", "#chkConciliado", function() {
        HabilitaTipoCambio();
    });

    $('#dialogAgregarCheques').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarCheques").remove();
        },
        buttons: {
            "Guardar": function() {
                if ($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("IdCheques") == null || $("#divFormaEditarCheques, #divFormaAgregarCheques").attr("IdCheques") == "") {
                    AgregarCheques();
                }
                else {
                    EditarCheques();
                }
            },
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogConsultarCheques').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaConsultarCheques").remove();
        },
        buttons: {
            "Salir": function() {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarCheques').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaEditarCheques").remove();
        },
        buttons: {
            "Editar": function() {
                EditarCheques();
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

function HabilitaTipoCambio()
{
    if ($("#chkConciliado").is(':checked')) {
        $("#txtFechaConciliacion").removeAttr("disabled");
        if($("#divFormaAgregarCheques, #divFormaEditarCheques").attr("puedeEditarTipoCambioCheques") != "" && $("#divFormaAgregarCheques, #divFormaEditarCheques").attr("puedeEditarTipoCambioCheques") != "0");
        {
            $("#txtTipoCambio").removeAttr("disabled");
        }
    }
    else {
        $("#txtFechaConciliacion").val("");
        $("#txtFechaConciliacion").attr("disabled", "disabled");
        $("#txtTipoCambio").attr("disabled", "disabled");
    }
}

function HabilitaTipoCambioEdicion()
{
    if ($("#chkConciliado").is(':checked')) {
        if($("#divFormaAgregarCheques, #divFormaEditarCheques").attr("puedeEditarTipoCambioCheques") != "" && $("#divFormaAgregarCheques, #divFormaEditarCheques").attr("puedeEditarTipoCambioCheques") != "0");
        {
            $("#txtTipoCambio").removeAttr("disabled");
        }
    }
    else {
        $("#txtFechaConciliacion").val("");
        $("#txtFechaConciliacion").attr("disabled", "disabled");
        $("#txtTipoCambio").attr("disabled", "disabled");
    }
}

function HabilitaMonto(){
    var Cheques = new Object();       
    Cheques.pIdCheques= parseInt($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idCheques"));
    ObtenerHabilitaMonto(JSON.stringify(Cheques));    
}

function ObtenerHabilitaMonto(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/ObtenerHabilitaMonto",
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
                if (respuesta.Modelo.puedeEditarTipoCambioCheques == 0 || parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares))
                {
                    $("#txtTipoCambio").attr("disabled", "disabled");
                    
                }
                else
                {
                    $("#txtTipoCambio").removeAttr("disabled");
                }
                
                if (parseFloat(respuesta.Modelo.Importe) != parseFloat(respuesta.Modelo.Disponible) || parseFloat(respuesta.Modelo.ImporteDolares) != parseFloat(respuesta.Modelo.DisponibleDolares))
                {
                    $("#chkConciliado").attr("disabled", "disabled");
                    
                }
                else
                {
                    $("#chkConciliado").removeAttr("disabled");
                }
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobrosConsultar").trigger("reloadGrid");
                $("#grdMovimientosCobrosEditar").trigger("reloadGrid");
                $("#grdCheques").trigger("reloadGrid");  
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

function ObtenerFormaFiltrosCheques() {
    $("#divFiltrosCheques").obtenerVista({
        nombreTemplate: "tmplFiltrosCheques.html",
        url: "Cheques.aspx/ObtenerFormaFiltroCheques",
        despuesDeCompilar: function(pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function() {
                        FiltroCheques();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function() {
                        FiltroCheques();
                    }
                });
            }

            $('#divFiltrosCheques').on('click', '#chkPorFecha', function(event) {
                FiltroCheques();
            });

        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarCheques() {
    $("#dialogAgregarCheques").obtenerVista({
        nombreTemplate: "tmplAgregarCheques.html",
        url: "Cheques.aspx/ObtenerFormaAgregarCheques",
        despuesDeCompilar: function(pRespuesta) {
            $("#dialogAgregarCheques").dialog("open");
            $("#txtFecha").datepicker({
                maxDate: new Date()
            });
            $("#txtFechaAplicacion").datepicker();
            $("#txtFechaConciliacion").datepicker({
                maxDate: new Date(),
                onSelect: function() {
                    var pRequest = new Object();
                    pRequest.pIdTipoMonedaDestino = 2;
                    pRequest.pFecha = $("#txtFechaConciliacion").val(); ;
                    ObtenerTipoCambioDiarioOficial(JSON.stringify(pRequest));
                }
            });
            AutocompletarProveedor();
            
            if (pRespuesta.modelo.Permisos.puedeEditarTipoCambioCheques == 1)
            {
                $("#txtTipoCambio").removeAttr("disabled");
                
            }
            else
            {
                $("#txtTipoCambio").attr("disabled", "disabled");
            }
            
            $("#divFormaAgregarCheques, #divFormaEditarCheques").attr("puedeEditarTipoCambioCheques", pRespuesta.modelo.Permisos.puedeEditarTipoCambioCheques);
            
            HabilitaTipoCambio();
            
            $('#dialogAgregarCheques').on('focusout', '#txtImporte', function(event) {
                $("#txtImporte").val(formato.moneda(QuitarFormatoNumero($("#txtImporte").val()), "$"));
            });
            
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaAsociarDocumentos(Cheques) {
    $("#divFormaAsociarDocumentosF").obtenerVista({
        nombreTemplate: "tmplConsultarDocumentosProveedorCheques.html",
        url: "Cheques.aspx/ObtenerFormaAsociarDocumentos",
        parametros: Cheques,
        despuesDeCompilar: function(pRespuesta) {
        
            if ($("#chkConciliado").is(':checked')) {
                $("#chkConciliado").attr("disabled", "disabled");
                $("#txtFechaConciliacion").attr("disabled", "disabled");
            }
           
            FiltroFacturas();
            FiltroMovimientosCobros();
            $("#dialogMuestraAsociarDocumentos").dialog("open");
        }
    });
}

function ObtenerFormaConsultarCheques(pIdCheques) {
    $("#dialogConsultarCheques").obtenerVista({
        nombreTemplate: "tmplConsultarCheques.html",
        url: "Cheques.aspx/ObtenerFormaCheques",
        parametros: pIdCheques,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarCheques == 1) {
                $("#dialogConsultarCheques").dialog("option", "buttons", {
                    "Editar": function() {
                        $(this).dialog("close");
                        var Cheques = new Object();
                        Cheques.IdCheques = parseInt($("#divFormaConsultarCheques").attr("IdCheques"));
                        ObtenerFormaEditarCheques(JSON.stringify(Cheques))
                    }
                });
                $("#dialogConsultarCheques").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarCheques").dialog("option", "buttons", {});
                $("#dialogConsultarCheques").dialog("option", "height", "auto");
            }
            $("#dialogConsultarCheques").dialog("open");
            $("#tabAsignarDocumentos").tabs();
        }
    });
}

function ObtenerFormaEditarCheques(IdCheques) {
    $("#dialogEditarCheques").obtenerVista({
        nombreTemplate: "tmplEditarCheques.html",
        url: "Cheques.aspx/ObtenerFormaEditarCheques",
        parametros: IdCheques,
        despuesDeCompilar: function(pRespuesta) {
            Inicializar_grdMovimientosCobrosEditar();
            $("#txtFechaConciliacion").datepicker({
                maxDate: new Date(),
                onSelect: function() {
                    var pRequest = new Object();
                    pRequest.pIdTipoMonedaDestino = 2;
                    pRequest.pFecha = $("#txtFechaConciliacion").val(); ;
                    ObtenerTipoCambioDiarioOficial(JSON.stringify(pRequest));
                }
            });
            HabilitaTipoCambioEdicion();
            $("#dialogEditarCheques").dialog("open");
            AutocompletarProveedor();
            $("#tabAsignarDocumentosEditar").tabs();
        }
    });
}

function ObtenerDatosCuentaBancaria(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/ObtenerDatosCuentaBancaria",
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
                $("#divFormaAgregarCheques, #divFormaEditarCheques").attr("idCuentaBancaria", respuesta.Modelo.IdCuentaBancaria);
                $("#cmbTipoMoneda option[value=" + respuesta.Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFechaAplicacion").val(respuesta.Modelo.Fecha);
                
                if (parseInt(respuesta.Modelo.Permisos.puedeEditarTipoCambioCheques) == 0)
                {
                    $("#txtTipoCambio").attr("disabled", "disabled");
                }
                else
                {
                    $("#txtTipoCambio").removeAttr("disabled");
                }
                
                HabilitaTipoCambio();
                
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

function FiltroCheques() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdCheques').getGridParam('rowNum');
    request.pPaginaActual = $('#grdCheques').getGridParam('page');
    request.pColumnaOrden = $('#grdCheques').getGridParam('sortname');
    request.pTipoOrden = $('#grdCheques').getGridParam('sortorder');
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
        url: 'Cheques.aspx/ObtenerCheques',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdCheques')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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
        url: 'Cheques.aspx/ObtenerCuentaBancaria',
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
    if ($("#divFormaEditarCheques, #divFormaAsociarDocumentos").attr("IdProveedor") != null) {
        request.pIdProveedor = $("#divFormaEditarCheques, #divFormaAsociarDocumentos").attr("IdProveedor");
        if ($('#divContGridAsociarDocumento').find(gs_NumeroFactura).existe()) {
            request.pNumeroFactura = $('#divContGridAsociarDocumento').find(gs_NumeroFactura).val();
        }
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Cheques.aspx/ObtenerFacturas',
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
    request.pIdCheques = 0;
    if ($("#divFormaEditarCheques, #divFormaAsociarDocumentos").attr("IdCheques") != null) {
        request.pIdCheques = $("#divFormaEditarCheques, #divFormaAsociarDocumentos").attr("IdCheques");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Cheques.aspx/ObtenerMovimientosCobros',
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
    request.pIdCheques = 0;
    if ($("#divFormaEditarCheques, #divFormaConsultarCheques, #divFormaAsociarDocumentos").attr("IdCheques") != null) {
        request.pIdCheques = $("#divFormaEditarCheques, #divFormaConsultarCheques, #divFormaAsociarDocumentos").attr("IdCheques");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Cheques.aspx/ObtenerMovimientosCobrosConsultar',
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
    request.pIdCheques = 0;
    if ($("#divFormaEditarCheques, #divFormaConsultarCheques, #divFormaAsociarDocumentos").attr("IdCheques") != null) {
        request.pIdCheques = $("#divFormaEditarCheques, #divFormaConsultarCheques, #divFormaAsociarDocumentos").attr("IdCheques");
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Cheques.aspx/ObtenerMovimientosCobrosEditar',
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

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarCheques() {
    var pCheques = new Object();
    pCheques.IdCuentaBancaria = $("#divFormaAgregarCheques").attr("idCuentaBancaria");
    if ($("#divFormaAgregarCheques").attr("idProveedor") == "") {
        pCheques.IdProveedor = 0;
    }
    else {
        pCheques.IdProveedor = $("#divFormaAgregarCheques").attr("idProveedor");
    }
    pCheques.CuentaBancaria = $("#txtCuenta").val();
    pCheques.IdMetodoPago = $("#cmbMetodoPago").val();
    pCheques.Fecha = $("#txtFecha").val();
    pCheques.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pCheques.Referencia = $("#txtReferencia").val();
    pCheques.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pCheques.FechaAplicacion = $("#txtFechaAplicacion").val();
    pCheques.FechaConciliacion = $("#txtFechaConciliacion").val();
    pCheques.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCheques.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pCheques.IdTipoMoneda == 2)
    {
        pCheques.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pCheques.TipoCambio = "1";
    }

    if (pCheques.TipoCambio.replace(" ", "") == "") {
        pCheques.TipoCambio = 0;
    }
    
    if (pCheques.TipoCambioDOF.replace(" ", "") == "") {
        pCheques.TipoCambioDOF = 0;
    }

    if (pCheques.TipoCambio.replace(" ", "") == "") {
        pCheques.TipoCambio = 0;
    }
    
    if ($("#chkConciliado").is(':checked')) {
        pCheques.Conciliado = 1;
    }
    else {
        pCheques.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pCheques.Asociado = 1;
    }
    else {
        pCheques.Asociado = 0;
    }

    if ($("#chkImpreso").is(':checked')) {
        pCheques.Impreso = 1;
    }
    else {
        pCheques.Impreso = 0;
    }
    
    var validacion = ValidaCheques(pCheques);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCheques = pCheques;
    SetAgregarCheques(JSON.stringify(oRequest));
}

function AgregarChequesEdicion() {
    var pCheques = new Object();
    pCheques.IdCuentaBancaria = $("#divFormaAgregarCheques").attr("idCuentaBancaria");
    if ($("#divFormaAgregarCheques").attr("idProveedor") == "") {
        pCheques.IdProveedor = 0;
    }
    else {
        pCheques.IdProveedor = $("#divFormaAgregarCheques").attr("idProveedor");
    }
    pCheques.CuentaBancaria = $("#txtCuenta").val();
    pCheques.IdMetodoPago = $("#cmbMetodoPago").val();
    pCheques.Fecha = $("#txtFecha").val();
    pCheques.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pCheques.Referencia = $("#txtReferencia").val();
    pCheques.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pCheques.FechaAplicacion = $("#txtFechaAplicacion").val();
    pCheques.FechaConciliacion = $("#txtFechaConciliacion").val();
    pCheques.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCheques.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pCheques.IdTipoMoneda == 2)
    {
        pCheques.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pCheques.TipoCambio = "1";
    }

    if (pCheques.TipoCambio.replace(" ", "") == "") {
        pCheques.TipoCambio = 0;
    }
    
    if (pCheques.TipoCambioDOF.replace(" ", "") == "") {
        pCheques.TipoCambioDOF = 0;
    }
    
    if ($("#chkConciliado").is(':checked')) {
        pCheques.Conciliado = 1;
    }
    else {
        pCheques.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pCheques.Asociado = 1;
    }
    else {
        pCheques.Asociado = 0;
    }

    if ($("#chkImpreso").is(':checked')) {
        pCheques.Impreso = 1;
    }
    else {
        pCheques.Impreso = 0;
    }

    var validacion = ValidaChequesEdicion(pCheques);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCheques = pCheques;
    SetAgregarChequesEdicion(JSON.stringify(oRequest));
}

function SetAgregarCheques(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/AgregarCheques",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCheques").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogAgregarCheques").dialog("close");
        }
    });
}

function SetAgregarChequesEdicion(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/AgregarChequesEdicion",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarCheques").attr("idCheques", respuesta.IdCheques);
                $("#txtCuenta").attr("disabled", "true");
                $("#cmbMetodoPago").attr("disabled", "true");
                $("#txtFecha").attr("disabled", "true");
                $("#txtRazonSocial").attr("disabled", "true");
                $("#txtFechaAplicacion").attr("disabled", "true");
                $("#chkAsociado").attr("disabled", "true");
                $("#cmbTipoMoneda").attr("disabled", "true");
                $("#txtFolio").val(respuesta.Folio);
                $("#grdCheques").trigger("reloadGrid");
                var Cheques = new Object();
                Cheques.pIdCheques = parseInt($("#divFormaEditarCheques,#divFormaAgregarCheques").attr("IdCheques"));
                Cheques.pIdProveedor = parseInt($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("IdProveedor"));
                Cheques.IdTipoMoneda = $("#cmbTipoMoneda").val();
                Cheques.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
                
                if ($("#chkConciliado").is(':checked')) {
                    Cheques.Conciliado = 1;
                }
                else {
                    Cheques.Conciliado = 0;
                }
                Cheques.FechaConciliacion = $("#txtFechaConciliacion").val();
                
                if(Cheques.IdTipoMoneda == 2)
                {
                    Cheques.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
                }
                else
                {
                    Cheques.TipoCambio = "1";
                }

                if (Cheques.TipoCambio.replace(" ", "") == "") {
                    Cheques.TipoCambio = 0;
                }
                
                if (Cheques.TipoCambioDOF.replace(" ", "") == "") {
                    Cheques.TipoCambioDOF = 0;
                }
                
                var validacion = ValidaAsociacionDocumentos(Cheques);
                if (validacion != "")
                { MostrarMensajeError(validacion); return false; }
                var oRequest = new Object();
                oRequest.Cheques = Cheques;
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

function SetCambiarEstatus(pIdCheques, pBaja) {
    var pRequest = "{'pIdCheques':" + pIdCheques + ", 'pBaja':" + pBaja + "}";
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/CambiarEstatus",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCheques").trigger("reloadGrid");
                MostrarMensajeError(respuesta.Descripcion);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
            $('#grdCheques').one('click', '.div_grdCheques_AI', function(event) {
                var registro = $(this).parents("tr");
                var estatusBaja = $(registro).children("td[aria-describedby='grdCheques_AI']").children().attr("baja")
                var idCheques = $(registro).children("td[aria-describedby='grdCheques_IdCheques']").html();
                var baja = "false";
                if (estatusBaja == "0" || estatusBaja == "False") {
                    baja = "true";
                }
                SetCambiarEstatus(idCheques, baja);
            });
        }
    });
}

function EditarCheques() {
    var pCheques = new Object();
    pCheques.IdCheques = $("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idCheques");
    if ($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idProveedor") != null) {
        pCheques.IdProveedor = $("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idProveedor");
    }
    else {
        pCheques.IdProveedor = 0;
    }

    pCheques.CuentaBancaria = $("#txtCuenta").val();
    pCheques.IdMetodoPago = $("#cmbMetodoPago").val();
    pCheques.Fecha = $("#txtFecha").val();
    pCheques.Folio = $("#txtFolio").val();
    pCheques.Importe = QuitarFormatoNumero($("#txtImporte").val());
    pCheques.Referencia = $("#txtReferencia").val();
    pCheques.ConceptoGeneral = $("#txtConceptoGeneral").val();
    pCheques.FechaAplicacion = $("#txtFechaAplicacion").val();
    pCheques.FechaConciliacion = $("#txtFechaConciliacion").val();
    pCheques.IdTipoMoneda = $("#cmbTipoMoneda").val();
    pCheques.TipoCambioDOF = QuitarFormatoNumero($("#txtTipoCambio").val());
    
    if(pCheques.IdTipoMoneda == 2)
    {
        pCheques.TipoCambio = QuitarFormatoNumero($("#txtTipoCambio").val());
    }
    else
    {
        pCheques.TipoCambio = "1";
    }

    if (pCheques.TipoCambio.replace(" ", "") == "") {
        pCheques.TipoCambio = 0;
    }
    
    if (pCheques.TipoCambioDOF.replace(" ", "") == "") {
        pCheques.TipoCambioDOF = 0;
    }

    if (pCheques.TipoCambio.replace(" ", "") == "") {
        pCheques.TipoCambio = 0;
    }

    if ($("#chkConciliado").is(':checked')) {
        pCheques.Conciliado = 1;
    }
    else {
        pCheques.Conciliado = 0;
    }

    if ($("#chkAsociado").is(':checked')) {
        pCheques.Asociado = 1;
    }
    else {
        pCheques.Asociado = 0;
    }

    if ($("#chkImpreso").is(':checked')) {
        pCheques.Impreso = 1;
    }
    else {
        pCheques.Impreso = 0;
    }
    
    var validacion = ValidaCheques(pCheques);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCheques = pCheques;
    SetEditarCheques(JSON.stringify(oRequest));
}
function EditarChequesProveedor() {
    var pCheques = new Object();
    pCheques.IdCheques = $("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idCheques");
    if ($("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idProveedor") != null) {
        pCheques.IdProveedor = $("#divFormaEditarCheques, #divFormaAgregarCheques").attr("idProveedor");
    }
    else {
        pCheques.IdProveedor = 0;
    }
    
    var oRequest = new Object();
    oRequest.pCheques = pCheques;
    SetEditarChequesProveedor(JSON.stringify(oRequest));
}
function SetEditarCheques(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/EditarCheques",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdCheques").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function() {
            OcultarBloqueo();
            $("#dialogEditarCheques, #dialogAgregarCheques").dialog("close");
        }
    });
}

function SetEditarChequesProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/EditarChequesProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            $("#grdCheques").trigger("reloadGrid");
        },
        complete: function() {
            OcultarBloqueo();
        }
    });
}

function EdicionFacturas(valor, id, rowid, iCol) {
    var TipoMoneda = $('#grdFacturas').getCell(rowid, 'TipoMoneda');
    var Saldo = $('#grdFacturas').getCell(rowid, 'Saldo');
    var Cheques = new Object();
    if (TipoMoneda == "Pesos" || TipoMoneda == "Peso") {
        Cheques.IdTipoMoneda = 1;
        Cheques.TipoCambio = 1;
        Cheques.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    }
    else {
        Cheques.IdTipoMoneda = 2;
        Cheques.TipoCambio = $("#spanTipoCambioDolares").text();
        Cheques.Disponible = QuitarFormatoNumero($("#spanDisponibleDolares").text());
    }
    Cheques.TipoMoneda = TipoMoneda;
    Cheques.Disponible = QuitarFormatoNumero($("#spanDisponible").text());
    Cheques.Monto = QuitarFormatoNumero(valor);
    Cheques.Saldo = QuitarFormatoNumero(Saldo);
    Cheques.IdEncabezadoFacturaProveedor = id;
    Cheques.rowid = rowid;
    Cheques.IdCheques = $("#divFormaAsociarDocumentos").attr("idCheques");
    var validacion = ValidarMontos(Cheques);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCheques = Cheques;
    SetEditarMontos(JSON.stringify(oRequest));
    
}
function SetEditarMontos(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/EditarMontos",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdCheques").trigger("reloadGrid");                

                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosCheques;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosCheques / $("#spanTipoCambioDolares").text());
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

function SetEliminarChequesEncabezadoFacturaProveedor(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "Cheques.aspx/EliminarChequesEncabezadoFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdMovimientosCobros").trigger("reloadGrid");
                $("#grdCheques").trigger("reloadGrid");
                var Importe = QuitarFormatoNumero($("#spanImporte").text());
                var Disponible = 0;
                var DisponibleDolares = 0;
                Disponible = Importe - respuesta.AbonosCheques;
                DisponibleDolares = (QuitarFormatoNumero($("#spanImporteDolares").text())) - (respuesta.AbonosCheques / $("#spanTipoCambioDolares").text());
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
                url: 'Cheques.aspx/BuscarRazonSocialProveedor',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    $("#divFormaAgregarCheques, #divFormaEditarCheques").attr("idProveedor", "0");
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
            $("#divFormaAgregarCheques, #divFormaEditarCheques").attr("idProveedor", pIdProveedor);
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

//-----Validaciones------------------------------------------------------

function ValidaCheques(pCheques) {
    var errores = "";

    if (pCheques.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pCheques.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pCheques.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pCheques.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pCheques.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pCheques.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pCheques.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pCheques.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pCheques.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (parseFloat(pCheques.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaChequesEdicion(pCheques) {
    var errores = "";

    if (pCheques.IdCuentaBancaria == 0)
    { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

    if (pCheques.IdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

    if (pCheques.IdMetodoPago == 0)
    { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

    if (pCheques.Fecha == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacio, favor de seleccionarlo.<br />"; }

    if (pCheques.Importe == 0)
    { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

    if (pCheques.Referencia == "")
    { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

    if (pCheques.FechaAplicacion == "")
    { errores = errores + "<span>*</span> El campo fecha de aplicacion esta vacio, favor de seleccionarlo.<br />"; }

    if (ConvertirFecha(pCheques.FechaAplicacion, 'aaaammdd') < ConvertirFecha(pCheques.Fecha, 'aaaammdd'))
    { errores = errores + "<span>*</span> La fecha de aplicación no debe ser menor a la fecha de emisión.<br />"; }

    if (pCheques.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

    if (parseFloat(pCheques.TipoCambio, 10) == 0)
    { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de capturar el tipo de cambio del día acreditado.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaAsociacionDocumentos(Cheques) {
    var errores = "";

    if (Cheques.pIdCheques == 0)
    { errores = errores + "<span>*</span> No hay cheque por asociar, favor de elegir alguno.<br />"; }
    
    if (Cheques.pIdProveedor == 0)
    { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidarMontos(Cheques) {
    var errores = "";

    if (Cheques.IdEncabezadoFacturaProveedor == 0)
    { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

    if (parseFloat(Cheques.Monto) > parseFloat(Cheques.Disponible))
    { errores = errores + "<span>*</span> El monto no puede ser mayor al disponible.<br />"; }

    if (parseFloat(Cheques.Monto) > parseFloat(Cheques.Saldo)+1)
    { errores = errores + "<span>*</span> El monto no puede ser mayor al saldo de la factura.<br />"; }

    if (parseFloat(Cheques.Monto) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }

    if (parseFloat(Cheques.Disponible) <= 0)
    { errores = errores + "<span>*</span> El monto no puede ser menor o igual a 0.<br />"; }
    
    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}



