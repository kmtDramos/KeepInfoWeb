//----------DHTMLX----------//
var arrDataModel = new Array;
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function () {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    $(window).unload(function () {
        ActualizarPanelControles("FacturaCliente");
    });
    ObtenerFormaFiltrosFactura();

    $("#gbox_grdFacturas").livequery(function () {
        $("#grdFacturas").jqGrid('navButtonAdd', '#pagFacturas', {
            caption: "Exportar",
            title: "Exportar",
            buttonicon: 'ui-icon-newwin',
            onClickButton: function () {
                var pRazonSocial = "";
                var pNumeroFactura = "";
                var pSerieFactura = "";
                var pAI = 0;
                var pFechaInicial = "";
                var pFechaFinal = "";
                var pPorFecha = 0;
                var pFiltroTimbrado = 0;
                var pEstatusFacturaEncabezado = "";

                if ($('#gs_SerieFactura').val() != null) {
                    pSerieFactura = $("#gs_SerieFactura").val();
                }

                if ($('#gs_NumeroFactura').val() != null) {
                    pNumeroFactura = $("#gs_NumeroFactura").val();
                }

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
                    pFiltroTimbrado = $("#cmbFiltroTimbrado").val();
                }

                if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
                    pFechaFinal = $("#txtFechaFinal").val();
                    pFechaFinal = ConvertirFecha(pFechaFinal, 'aaaammdd');
                }

                if ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null) {
                    pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
                }

                if ($('#gs_EstatusFacturaEncabezado').val() != null) {
                    pEstatusFacturaEncabezado = $("#gs_EstatusFacturaEncabezado").val();
                }

                $.UnifiedExportFile({
                    action: '../ExportacionesExcel/ExportarExcel.aspx', data: {
                        IsExportExcel: true,
                        pRazonSocial: pRazonSocial,
                        pNumeroFactura: pNumeroFactura,
                        pSerieFactura: pSerieFactura,
                        pAI: pAI,
                        pFechaInicial: pFechaInicial,
                        pFechaFinal: pFechaFinal,
                        pPorFecha: pPorFecha,
                        pFiltroTimbrado: pFiltroTimbrado,
                        pEstatusFacturaEncabezado: pEstatusFacturaEncabezado

                    }, downloadType: 'Normal'
                });

            }
        });
    });

    $("#btnVerDetalle").click(function (e) {
        e.preventDefault();
        var ventana = $("<div></div>");
        $(ventana).dialog({
            modal: true,
            autoOpen: false,
            draggable: false,
            resizable: false,
            width: 950,
            height: 390,
            close: function () {
                $(this).remove();
            },
            buttons: {
                "Cerrar": function () {
                    $(ventana).dialog("close");
                }
            }
        });
        $(ventana).obtenerVista({
            nombreTemplate: "tmplConsultarFacturas_Detalle.html",
            despuesDeCompilar: function () {
                Inicializar_grdFacturas_Detalle();
                ExportarFacturas_Detalle();
                $(ventana).dialog("open");
            }
        });
    });

    $("#grdFacturas").on("click", ".imgFormaConsultarFacturaEncabezado", function () {
        var registro = $(this).parents("tr");
        var Factura = new Object();
        Factura.pIdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFacturas_IdFacturaEncabezado']").html());
        ObtenerFormaConsultarFacturaEncabezado(JSON.stringify(Factura));
    });

    $("#grdFacturas").on("click", ".imgFormaConsultarFacturaFormato", function () {
        var registro = $(this).parents("tr");
        var Factura = new Object();
        Factura.IdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFacturas_IdFacturaEncabezado']").html());
        ObtenerFormaConsultarFacturaFormato(JSON.stringify(Factura));
    });

    $("#grdFacturas").on("click", ".imgFormaConsultarFacturaAddenda", function () {
        var registro = $(this).parents("tr");
        var Factura = new Object();
        Factura.IdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFacturas_IdFacturaEncabezado']").html());
        ObtenerFormaConsultarFacturaAddenda(JSON.stringify(Factura));
    });

    $("#grdFacturas").on("click", ".imgFormaConsultarFacturaXML", function () {
        var registro = $(this).parents("tr");
        var Factura = new Object();
        Factura.IdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFacturas_IdFacturaEncabezado']").html());
        ObtenerFormaConsultarFacturaXML(JSON.stringify(Factura));
    });

    $("#divSubirArchivoXML").livequery(function () {
        var ctrlSubirLogo = new qq.FileUploader({
            element: document.getElementById('divSubirArchivoXML'),
            action: '../ControladoresSubirArchivos/SubirArchivoXML.ashx',
            allowedExtensions: ["xml"],
            template: '<div class="qq-uploader">' +
            '<div class="qq-upload-drop-area"></div>' +
            '<div class="qq-upload-container-list"><ul class="qq-upload-list"><li><span class="qq-upload-file">Favor de elegir el XML.</span></li></ul></div>' +
            '<div class="qq-upload-container-buttons"><div id="divEliminarArchivoXML" class="qq-upload-button">- Borrar</div><div class="qq-upload-button qq-divBotonSubir">+ Agregar</div></div>' +
            '</div>',
            onSubmit: function (id, fileName) {
                $(".qq-upload-list").empty();
            },
            onComplete: function (id, file, responseJSON) {
                $("#divRutaArchivo").html(responseJSON.name);
                $("#divRutaArchivo").attr("archivo", responseJSON.name);

                var Archivo = new Object();
                Archivo.pArchivo = responseJSON.name;
                ObtenerDatosFacturaXML(JSON.stringify(Archivo));

                OcultarBloqueo();
            }
        });
    });

    $('#dialogFacturaAddenda').on('click', '#divEliminarArchivoXML', function (event) {
        if ($("#divRutaArchivo").attr("archivo") != "") {
            MostrarMensajeEliminar("¿Esta seguro de eliminar el archivo?");
        }
    });

    //Dialogo Eliminar
    $('#dialogMensajeEliminar').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Eliminar": function () {
                $(".qq-upload-list").html("<li><span class='qq-upload-file'>Favor de elegir el XML.</span></li>");
                $("#divRutaArchivo").html("");
                $("#divRutaArchivo").attr("archivo", "");
                $(this).dialog("close");
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarDescripcionPartida').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Guardar": function () {
                EditarDetallePartida();
            },
            "Cancelar": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#divFiltrosFacturaCliente").on("keypress", "#txtNumeroPedidoBuscador", function (event) {
        var key = (document.all) ? event.keyCode : event.which;
        if (key == 13) {
            FiltroFacturas();
        }
    });

    $('#dialogAgregarFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function () {

        },
        close: function () {
            $("#divFormaAgregarFactura").remove();
        },
        buttons: {
            "Timbrar": function () {
                var Factura = new Object();
                Factura.IdFacturaEncabezado = $("#divFormaAgregarFactura").attr("IdFacturaEncabezado");

                if (Factura.IdFacturaEncabezado != "0" && Factura.IdFacturaEncabezado != "" && Factura.IdFacturaEncabezado != null) {
                    if ($("#chkEsRefactura").is(':checked')) {
                        ObtenerFormaFacturasSustituye(JSON.stringify(Factura));
                    }
                    else {
                        TimbrarFactura();
                    }

                }
                else {
                    MostrarMensajeError("No se puede timbrar la factura hasta que se grave una partida");
                }

            },
            "Salir": function () {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogConsultarFacturaEncabezado').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConsultarFacturaEncabezado").remove();
        },
        buttons: {
            "Timbrar": function () {
                var Factura = new Object();
                Factura.IdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado").attr("IdFacturaEncabezado");

                if (Factura.IdFacturaEncabezado != "0" && Factura.IdFacturaEncabezado != "" && Factura.IdFacturaEncabezado != null) {
                    if ($("#chkEsRefactura").is(':checked')) {
                        ObtenerFormaFacturasSustituye(JSON.stringify(Factura));
                    }
                    else {
                        TimbrarFactura();
                    }
                }
                else {
                    MostrarMensajeError("No se puede timbrar la factura hasta que se grave una partida");
                }
            },
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogEditarFacturaEncabezado').dialog({
        autoOpen: false,
        height: 'auto',
        width: '1170px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function () {

        },
        close: function () {
            $("#divFormaEditarFacturaEncabezado").remove();
        },
        buttons: {
            "Timbrar": function () {
                var FacturaEncabezado = new Object();
                FacturaEncabezado.IdFacturaEncabezado = $("#divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado");

                if (FacturaEncabezado.IdFacturaEncabezado != "0" && FacturaEncabezado.IdFacturaEncabezado != "" && FacturaEncabezado.IdFacturaEncabezado != null) {
                    if ($("#chkEsRefactura").is(':checked')) {
                        ObtenerFormaFacturasSustituye(JSON.stringify(Factura));
                    }
                    else {
                        TimbrarFactura();
                    }
                }
                else {
                    MostrarMensajeError("No ha seleccionado ninguna nota de crédito");
                }
            },
            "Editar": function () {
                EditarFacturaEncabezado();
            },
            "Salir": function () {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogDatosFiscalesFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaDatosFiscalesCliente").remove();
        },
        buttons: {
            "Editar": function () {
                EditarDatosFiscalesCliente();
            },
            "Salir": function () {
                $(this).dialog("close")
            }
        }
    });

    $('#dialogMotivoCancelacionFactura').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Cancelar": function () {
                CancelarFacturaEncabezado();
            },
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogFacturasSustituye').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Timbrar": function () {
                var detalle = $("#grdFacturasSustituye").jqGrid('getGridParam', 'records');
                if (detalle != 0) {
                    TimbrarFactura();
                }
                else {
                    MostrarMensajeError("Debe de aginar al menos una factura, ya que la factura es refacturación");
                }

            },
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogFacturaFormato').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $('#dialogFacturaAddenda').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        buttons: {
            "GenerarAddenda": function () {
                GenerarAddenda();
            },
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarFactura", function () {
        ObtenerFormaAgregarFactura();
    });

    $("#divFiltrosFacturaCliente").on("change", "#cmbTipoBusqueda", function () {
        $("#txtBuscador").val("");
        $("#txtBuscador").attr("IdGenerico", "0");
    });

    $("#divFiltrosFacturaCliente").on("change", "#cmbTipoBusquedaAgregar", function () {
        $("#txtBuscador").val("");
        $("#txtBuscador").attr("IdGenerico", "0");
    });

    $("#dialogAgregarFactura, #dialogEditarFacturaEncabezado").on("change", "#cmbDescuento", function () {
        var request = new Object();
        request.pIdDescuentoCliente = $("#cmbDescuento").val();
        ObtenerPorcentaje(JSON.stringify(request));
    });

    $("#dialogAgregarFactura, #dialogEditarFacturaEncabezado").on("change", "#cmbDireccionCliente", function () {
        var request = new Object();
        request.pIdDireccionCliente = $("#cmbDireccionCliente").val();
        ObtenerDireccionCliente(JSON.stringify(request));
    });

    $("#dialogAgregarFactura, #dialogEditarFacturaEncabezado").on("change", "#cmbDireccionFiscal", function () {
        var request = new Object();
        request.pIdDireccionFiscal = $("#cmbDireccionFiscal").val();
        ObtenerDireccionFiscal(JSON.stringify(request));
    });

    $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('change', '#cmbTipoMoneda', function (event) {
        var pTipoCambio = new Object();
        pTipoCambio.IdTipoCambioOrigen = parseInt($(this).val());
        pTipoCambio.IdTipoCambioDestino = parseInt(1);
        var oRequest = new Object();
        oRequest.pTipoCambio = pTipoCambio;
        ObtenerTipoCambio(JSON.stringify(oRequest))

        if ($("#chkSinDocumentacion").is(':checked')) {
            MuestraPedidosSinDocumentacion(1);
        }
        else {
            MuestraPedidosSinDocumentacion(0);
        }
    });

    $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('change', '#cmbCondicionPago', function (event) {
        var pCondicionPago = new Object();
        pCondicionPago.IdCondicionPago = parseInt($(this).val());
        pCondicionPago.FechaFactura = $("#spanFechaActual").text();
        var oRequest = new Object();
        oRequest.pCondicionPago = pCondicionPago;
        ObtenerFechaPago(JSON.stringify(oRequest));
    });

    $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('change', '#cmbPedido', function (event) {
        $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idPedido", parseInt($(this).val()));
        var pFactura = new Object();
        pFactura.IdPedido = parseInt($(this).val());
        var oRequest = new Object();
        oRequest.pFactura = pFactura;
        ObtenerTipoCambioPedido(JSON.stringify(oRequest));
    });

    $('#grdFacturas').on('click', '.div_grdFacturas_AI', function (event) {

        var registro = $(this).parents("tr");
        var FacturaEncabezado = new Object();
        var estatusBaja = $(registro).children("td[aria-describedby='grdFacturas_AI']").children().attr("baja")
        FacturaEncabezado.IdFacturaEncabezado = $(registro).children("td[aria-describedby='grdFacturas_IdFacturaEncabezado']").html();
        if (estatusBaja == "0" || estatusBaja == "False") {
            ObtenerFormaMotivoCancelacionFactura(JSON.stringify(FacturaEncabezado));
        }
        else {
            MostrarMensajeAviso("Esta factura, ya esta cancelada");
        }

    });

    $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('click', '#chkSinDocumentacion,#chkNuevoCotizador', function (event) {
        if ($("#chkSinDocumentacion").is(':checked')) {
            MuestraPedidosSinDocumentacion(1);
        }
        else {
            MuestraPedidosSinDocumentacion(0);
        }
    });

    $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('click', '#chkSinFiltroTipoMoneda', function (event) {
        if ($("#chkSinDocumentacion").is(':checked')) {
            MuestraPedidosSinDocumentacion(1);
        }
        else {
            MuestraPedidosSinDocumentacion(0);
        }
    });

    $("#dialogAgregarFacturaEncabezado, #dialogEditarFacturaEncabezado, #dialogConsultarFacturaEncabezado").on("click", "#divImprimir", function () {
        var IdFacturaEncabezado = $("#divFormaAgregarFacturaEncabezado, #divFormaEditarFacturaEncabezado, #divFormaConsultarFacturaEncabezado").attr("idfacturaencabezado");
        Imprimir(IdFacturaEncabezado);
    });

    $('#dialogDatosFiscalesFactura').on('change', '#cmbPais', function (event) {
        var request = new Object();
        request.pIdPais = $(this).val();
        ObtenerListaEstados(JSON.stringify(request));

        request.pIdEstado = $(this).val();
        ObtenerListaMunicipios(JSON.stringify(request));

        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidades(JSON.stringify(request));

    });

    $('#dialogDatosFiscalesFactura').on('change', '#cmbEstado', function (event) {
        var request = new Object();
        request.pIdEstado = $(this).val();
        ObtenerListaMunicipios(JSON.stringify(request));
    });

    $('#dialogDatosFiscalesFactura').on('change', '#cmbMunicipio', function (event) {
        var request = new Object();
        request.pIdMunicipio = $(this).val();
        ObtenerListaLocalidades(JSON.stringify(request));
    });

});

function DataModelJQ() { }
DataModelJQ.prototype.name;
DataModelJQ.prototype.index;
DataModelJQ.prototype.width;
DataModelJQ.prototype.sortable;
DataModelJQ.prototype.editable;

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
            if (i == 0) {
                arrDataModel[i].width = '0';
                arrDataModel[i].sortable = false;
                arrDataModel[i].editable = false;
            }
            else {
                arrDataModel[i].width = '30';
                arrDataModel[i].sortable = false;
                arrDataModel[i].editable = true;
            }
        }
        Titulo = "Datos";
    }
    else {
        arrDataModel[0] = new DataModelJQ();

        arrDataModel[0].name = "";
        arrDataModel[0].index = "";
        arrDataModel[0].width = '30';
        arrDataModel[0].sortable = false;
        arrDataModel[i].editable = true;
        Titulo = "No se encontraron registros para esta búsqueda";

    }
    llenaTablaConceptos(header, arrDataModel);
}

function llenaTablaConceptos(header, datamodel) {
    jQuery("#grdConceptos").jqGrid({
        datatype: function () {
            GeneraGrid(header);
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
        pager: "#pagConceptos",
        rownumbers: true,
        forceFit: true,
        cellEdit: true,
        cellsubmit: 'clientArray',
        afterSaveCell: function (rowid, name, val, iRow, iCol) {
            var valor = jQuery('#grdConceptos').jqGrid('getCell', rowid, iCol);
            //alert(valor);
        },
        caption: Titulo,
        height: '100%',
        width: 850
    });

    $("#grdConceptos").jqGrid('navGrid', '#pagConceptos', { edit: false, add: false, del: false, search: false });
    $("#grdConceptos").jqGrid('hideCol', ["RowNumber"]);
}

function GeneraGrid(header) {

    var Conceptos = new Object();

    if ($('#grdConceptos').getGridParam('rowNum') != null) {
        Conceptos.pTamanoPaginacion = $('#grdConceptos').getGridParam('rowNum');
        Conceptos.pPaginaActual = $('#grdConceptos').getGridParam('page');
        Conceptos.pColumnaOrden = $('#grdConceptos').getGridParam('sortname');
        Conceptos.pTipoOrden = $('#grdConceptos').getGridParam('sortorder');
    }
    else {
        Conceptos.pTamanoPaginacion = 100;
        Conceptos.pPaginaActual = 1;
        Conceptos.pColumnaOrden = "Orden";
        Conceptos.pTipoOrden = "asc";
    }

    Conceptos.pIdAddenda = $("#divFormaAgregarFacturaAddenda").attr("idAddenda");
    $("#divFormaAgregarFacturaAddenda").attr("atributos", header);

    Conceptos.pEncabezados = header;
    var oRequest = new Object();
    oRequest.Conceptos = Conceptos;
    var pRequest = JSON.stringify(oRequest);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerConceptos',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') {

                $('#grdConceptos')[0].addJSONData(JSON.parse(jsondata.responseText).d);
            }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }

    });
}

function AutocompletarCliente() {
    $('#txtCliente').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtCliente').val();
            $.ajax({
                type: 'POST',
                url: 'FacturaCliente.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente", "0");
                    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente, idCondicionPago: item.IdCondicionPago }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente", pIdCliente);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
            $("#txtProyecto").val("");
            $("#grdConceptoProyecto").trigger("reloadGrid");
            $("#divDetalleTabs").tabs("option", "active", 0);
            $("#cmbCondicionPago option[value=" + ui.item.idCondicionPago + "]").attr("selected", true);

            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            Cliente.IdTipoMonedaFactura = $("#cmbTipoMoneda").val();
            if ($("#chkSinFiltroTipoMoneda").is(':checked')) {
                Cliente.PorFiltroTipoMoneda = 1;
            }
            else {
                Cliente.PorFiltroTipoMoneda = 0;
            }
            var Request = JSON.stringify(Cliente);
            ObtenerFormaDatosCliente(Request);

            var pCondicionPago = new Object();
            pCondicionPago.IdCondicionPago = $("#cmbCondicionPago").val();
            pCondicionPago.FechaFactura = $("#spanFechaActual").text();
            var oRequest = new Object();
            oRequest.pCondicionPago = pCondicionPago;
            ObtenerFechaPago(JSON.stringify(oRequest));

            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            ObtenerNumerosCuenta(JSON.stringify(Cliente));

            var Facturas = new Object();
            Facturas.pIdCliente = pIdCliente;
            ObtenerFacturasCliente(JSON.stringify(Cliente));

            var Contactos = new Object();
            Contactos.pIdCliente = pIdCliente
            ObtenerContactosOrganizacion(JSON.stringify(Contactos))
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarProyecto() {
    $('#txtProyecto').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pNombreProyecto = $('#txtProyecto').val();
            $.ajax({
                type: 'POST',
                url: 'FacturaCliente.aspx/BuscarProyectoCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente", "0");
                    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.NombreProyecto, value: item.NombreProyecto, id: item.IdProyecto, idCliente: item.IdCliente, idCondicionPago: item.IdCondicionPago, idTipoMoneda: item.IdTipoMoneda }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var pIdProyecto = ui.item.id;
            var pIdCliente = ui.item.idCliente;
            var pIdTipoMoneda = ui.item.idTipoMoneda;

            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente", pIdCliente);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", pIdProyecto);

            $("#cmbCondicionPago option[value=" + ui.item.idCondicionPago + "]").attr("selected", true);
            $("#cmbTipoMoneda option[value=" + pIdTipoMoneda + "]").attr("selected", true);

            var pTipoCambio = new Object();
            pTipoCambio.IdTipoCambioOrigen = parseInt(pIdTipoMoneda);
            pTipoCambio.IdTipoCambioDestino = parseInt(1);
            var oRequest = new Object();
            oRequest.pTipoCambio = pTipoCambio;
            ObtenerTipoCambio(JSON.stringify(oRequest));

            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            Cliente.IdTipoMonedaFactura = $("#cmbTipoMoneda").val();
            if ($("#chkSinFiltroTipoMoneda").is(':checked')) {
                Cliente.PorFiltroTipoMoneda = 1;
            }
            else {
                Cliente.PorFiltroTipoMoneda = 0;
            }
            var Request = JSON.stringify(Cliente);
            ObtenerFormaDatosCliente(Request);

            var pCondicionPago = new Object();
            pCondicionPago.IdCondicionPago = $("#cmbCondicionPago").val();
            pCondicionPago.FechaFactura = $("#spanFechaActual").text();
            var oRequest = new Object();
            oRequest.pCondicionPago = pCondicionPago;
            ObtenerFechaPago(JSON.stringify(oRequest));

            var Cliente = new Object();
            Cliente.pIdCliente = pIdCliente;
            ObtenerNumerosCuenta(JSON.stringify(Cliente));

            var Cliente = new Object();
            Facturas.pIdCliente = pIdCliente;
            ObtenerFacturasCliente(JSON.stringify(Facturas));

            $("#divDetalleTabs").tabs("option", "active", 1);
            $("#grdConceptoProyecto").trigger("reloadGrid");

        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function MuestraObjetosClienteProyecto(opcion) {
    if (opcion == 1) {
        $("#txtCliente").css("display", "block");
        $("#txtProyecto").css("display", "none");
        $("#txtProyecto").val("");
        $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente", "0");
        $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
    }
    else {
        $("#txtCliente").css("display", "none");
        $("#txtProyecto").css("display", "block");
        $("#txtCliente").val("");
        $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente", "0");
        $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
    }
}

function MuestraPedidosSinDocumentacion(opcion) {
    if (opcion == 1) {
        var pIdCliente = 0;
        if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") != "" && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") != null) {
            pIdCliente = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente");
        }
        var Cliente = new Object();
        Cliente.IdCliente = pIdCliente;
        Cliente.IdTipoMonedaFactura = $("#cmbTipoMoneda").val();
        Cliente.NuevoCotizador = (($("#chkNuevoCotizador").is(":checked")) ? 1 : 0);
        if ($("#chkSinFiltroTipoMoneda").is(':checked')) {
            Cliente.PorFiltroTipoMoneda = 1;
        }
        else {
            Cliente.PorFiltroTipoMoneda = 0;
        }
        obtenerPedidosClienteSinDocumentacion(JSON.stringify(Cliente));
    }
    else {
        var pIdCliente = 0;
        if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") != "" && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") != null) {
            pIdCliente = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente");
        }
        var Cliente = new Object();
        Cliente.IdCliente = pIdCliente;
        Cliente.IdTipoMonedaFactura = $("#cmbTipoMoneda").val();
        Cliente.NuevoCotizador = (($("#chkNuevoCotizador").is(":checked")) ? 1 : 0);
        if ($("#chkSinFiltroTipoMoneda").is(':checked')) {
            Cliente.PorFiltroTipoMoneda = 1;
        }
        else {
            Cliente.PorFiltroTipoMoneda = 0;
        }
        obtenerPedidosClienteConDocumentacion(JSON.stringify(Cliente));
    }
}

function obtenerPedidosClienteSinDocumentacion(pRequest) {
    $("#cmbPedido").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/obtenerPedidosClienteSinDocumentacion",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
            $("#divDetalleTabs").tabs("option", "active", 0);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idPedido", 0)
            $("#grdPedidoDetalle").trigger("reloadGrid");
        }
    });
}

function obtenerPedidosClienteConDocumentacion(pRequest) {
    $("#cmbPedido").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/obtenerPedidosClienteConDocumentacion",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
            $("#divDetalleTabs").tabs("option", "active", 0);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idPedido", 0)
            $("#grdPedidoDetalle").trigger("reloadGrid");
        }
    });
}

function ObtenerFormaMotivoCancelacionFactura(pRequest) {
    $("#dialogMotivoCancelacionFactura").obtenerVista({
        nombreTemplate: "tmplMotivoCancelacionFactura.html",
        parametros: pRequest,
        url: "FacturaCliente.aspx/LlenaMotivoCancelacionFactura",
        despuesDeCompilar: function () {
            $("#dialogMotivoCancelacionFactura").dialog("open");
        }
    });
}

function ObtenerListaEstados(pRequest) {
    $("#cmbEstado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/ObtenerListaEstados",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
        }
    });
}

function ObtenerListaMunicipios(pRequest) {
    $("#cmbMunicipio").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/ObtenerListaMunicipios",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
        }
    });
}

function ObtenerFormaEditarDetallePartida(pIdFacturaDetalle) {
    var request = new Object();
    request.pIdFacturaDetalle = pIdFacturaDetalle;
    var pRequest = JSON.stringify(request);
    $("#dialogEditarDescripcionPartida").obtenerVista({
        nombreTemplate: "tmplFormaFacturaClienteEditarDescripcionPartidas.html",
        parametros: pRequest,
        url: "FacturaCliente.aspx/ObtenerFormaEditarDetallePartida",
        despuesDeCompilar: function () {
            $("#dialogEditarDescripcionPartida").dialog("open");
        }
    });
}

function EditarDetallePartida() {
    var request = new Object();
    request.pIdFacturaDetalle = $("#divFormaDescripcionPartida").attr("idFacturaDetalle");
    request.pDescripcionAgregada = $("#txtDescripcionAgregada").val();
    var pRequest = JSON.stringify(request);
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/GuardarFacturaDetalleDescripcionAgregada",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#dialogEditarDescripcionPartida").dialog("close");
                MostrarMensajeError("Se edito todo correcto");
                $("#grdFacturaDetalleEditar").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerListaLocalidades(pRequest) {
    $("#cmbLocalidad").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/ObtenerListaLocalidades",
        parametros: pRequest,
        despuesDeCompilar: function (pRespuesta) {
        }
    });
}

function ObtenerTipoCambio(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/ObtenerTipoCambio",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                if (respuesta.Modelo.TipoCambioActual == 0) {
                    $("#spanTipoCambioFactura").text(1);
                }
                else {
                    $("#spanTipoCambioFactura").text(respuesta.Modelo.TipoCambioActual);
                }
                $("#grdPedidoDetalle").trigger("reloadGrid");
                $("#grdConceptoProyecto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerNumeroFactura(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/ObtenerNumeroFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtNumeroFactura").val(respuesta.Modelo.NumeroFactura);
                $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("serieTimbrado", respuesta.Modelo.SerieTimbrado)
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerTipoCambioPedido(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/ObtenerTipoCambioPedido",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#spanTipoCambioPedido").text(respuesta.Modelo.TipoCambio);
                $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdTipoMonedaPedido", respuesta.Modelo.IdTipoMonedaPedido);
                $("#grdPedidoDetalle").trigger("reloadGrid");
                $("#divDetalleTabs").tabs("option", "active", 0);
                $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
                $("#txtProyecto").val("");
                $("#grdConceptoProyecto").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerFechaPago(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/ObtenerFechaPago",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#txtFechaPago").val(respuesta.Modelo.FechaPago);
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarFactura() {
    $("#dialogAgregarFactura").obtenerVista({
        url: "FacturaCliente.aspx/ObtenerFormaAgregarFacturaPedidoCliente",
        nombreTemplate: "tmplAgregarFactura.html",
        despuesDeCompilar: function (pRespuesta) {
            $("#divDireccionesTabs").tabs();
            $("#divDetalleTabs").tabs();
            AutocompletarCliente();
            AutocompletarProyecto();
            Inicializar_grdPedidoDetalle();
            Inicializar_grdConceptoProyecto();
            $("#cmbTipoMoneda option[value=" + 1 + "]").attr("selected", true);
            $("#spanTipoCambioFactura").text(1);
            $("#cmbMetodoPago").val(51);
            Inicializar_grdFacturaDetalle();
            $("#txtFechaPago").datepicker();
            $("#txtFechaFacturar").datepicker();

            $("#grdPedidoDetalle").on("click", "td", function () {
                $("#divFormaAgregarFactura").attr("idProducto", "0");
                $("#divFormaAgregarFactura").attr("idServicio", "0");
                var registro = $(this).parents("tr");
                var DetallePedido = new Object();
                var cantidad = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_CantidadPendiente']").html());
                var IdCotizacionDetalle = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_IdCotizacionDetalle']").html());
                var IdProducto = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_IdProducto']").html());
                var IdServicio = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_IdServicio']").html());
                var PrecioUnitario = $(registro).children("td[aria-describedby='grdPedidoDetalle_Precio']").html();
                var IdTipoIVA = $(registro).children("td[aria-describedby='grdPedidoDetalle_IdTipoIVA']").html();
                var IVA = $(registro).children("td[aria-describedby='grdPedidoDetalle_IVA']").html();
                var Nota = $(registro).children("td[aria-describedby='grdPedidoDetalle_Nota']").html();
                if (cantidad > 0) {
                    $("#txtCantidadFacturar").val(cantidad);
                    $("#divFormaAgregarFactura").attr("cantidadDetallePedido", cantidad);
                    $("#divFormaAgregarFactura").attr("idCotizacionDetalle", IdCotizacionDetalle);
                    $("#divFormaAgregarFactura").attr("idProducto", IdProducto);
                    $("#divFormaAgregarFactura").attr("idServicio", IdServicio);
                    $("#divFormaAgregarFactura").attr("precioUnitario", PrecioUnitario);
                    $("#divFormaAgregarFactura").attr("idTipoIVA", IdTipoIVA);
                    $("#txtIVA").val(IVA);
                    $("#txtNotaFactura").val(Nota);
                    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
                    $("#txtProyecto").val("");
                    $("#grdConceptoProyecto").trigger("reloadGrid");

                } else {
                    MostrarMensajeError("La cantidad es igual a 0, no se puede bajar la partida.");
                }
            });

            $("#divFormaAgregarFactura").on("click", "#btnAgregarPartidaFactura", function () {
                AgregarDetalleFactura();
            });

            $("#grdFacturaDetalle").on("click", ".imgEliminarConcepto", function () {

                var registro = $(this).parents("tr");
                var pFacturaDetalle = new Object();
                pFacturaDetalle.pIdFacturaDetalle = parseInt($(registro).children("td[aria-describedby='grdFacturaDetalle_IdFacturaDetalle']").html());
                var oRequest = new Object();
                oRequest.pFacturaDetalle = pFacturaDetalle;
                SetEliminarFacturaDetalle(JSON.stringify(oRequest));
            });

            $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('click', '#chkParcialidades', function (event) {
                if ($("#chkParcialidades").is(':checked')) {
                    $("#txtNoParcialidades").val("0");
                    $("#txtNoParcialidades").css("display", "block");
                }
                else {

                    $("#txtNoParcialidades").val("0");
                    $("#txtNoParcialidades").css("display", "none");
                }
            });

            $('#dialogAgregarFactura, #dialogEditarFacturaEncabezado').on('click', '#chkSinIVA', function (event) {
                if ($("#chkSinIVA").is(':checked')) {
                    //MostrarMensajeAviso("Al seleccionar esta opcion la factura no aplicara el IVA");
                    //alert("si");

                }
                else {
                    //MostrarMensajeAviso("Se aplicara el IVA");
                }
            });

            $("#dialogAgregarFactura").dialog("open");
        }
    });
}

function ObtenerFormaConsultarFacturaEncabezado(pIdFacturaEncabezado) {
    $("#dialogConsultarFacturaEncabezado").obtenerVista({
        nombreTemplate: "tmplConsultarFacturaEncabezado.html",
        url: "FacturaCliente.aspx/ObtenerFormaConsultarFacturaEncabezado",
        parametros: pIdFacturaEncabezado,
        despuesDeCompilar: function (pRespuesta) {
            Inicializar_grdFacturaDetalleConsultar();
            $("#divDireccionesTabs").tabs();
            if (pRespuesta.modelo.Permisos.puedeEditarFacturaEncabezado == 1) {

                if (pRespuesta.modelo.IdTxtTimbradosFactura == 0) {

                    $("#dialogConsultarFacturaEncabezado").dialog("option", "buttons", {
                        "Timbrar": function () {
                            var Factura = new Object();
                            Factura.IdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado").attr("IdFacturaEncabezado");

                            if (Factura.IdFacturaEncabezado != "0" && Factura.IdFacturaEncabezado != "" && Factura.IdFacturaEncabezado != null) {
                                if ($("#chkEsRefactura").is(':checked')) {
                                    ObtenerFormaFacturasSustituye(JSON.stringify(Factura));
                                }
                                else {
                                    TimbrarFactura();
                                }
                            }
                            else {
                                MostrarMensajeError("No se puede timbrar la factura hasta que se grave una partida");
                            }
                        },
                        "Editar": function () {
                            $(this).dialog("close");
                            var FacturaEncabezado = new Object();
                            FacturaEncabezado.IdFacturaEncabezado = parseInt($("#divFormaConsultarFacturaEncabezado").attr("IdFacturaEncabezado"));
                            ObtenerFormaEditarFacturaEncabezado(JSON.stringify(FacturaEncabezado))
                        },
                        "Salir": function () {
                            $(this).dialog("close");
                        }/*,
                        "Pruebas": function () {
                            var json = JSON.parse(pIdFacturaEncabezado);
                            var Factura = new Object();
                            Factura.IdFacturaEncabezado = json.pIdFacturaEncabezado;
                            var Request = JSON.stringify(Factura);
                            ObtenerFacturaATimbrar(Request);
                        },*/
                    });
                }
                else {
                    $("#dialogConsultarFacturaEncabezado").dialog("option", "buttons", {
                		"Pruebas": function () {
                			var json = JSON.parse(pIdFacturaEncabezado);
                			var Factura = new Object();
                			Factura.IdFacturaEncabezado = json.pIdFacturaEncabezado;
                			var Request = JSON.stringify(Factura);
                			ObtenerFacturaATimbrar(Request);
                		},/*
                		"CancelarP": function () {
                			var json = JSON.parse(pIdFacturaEncabezado);
                			var Factura = new Object();
                			Factura.IdFacturaEncabezado = json.pIdFacturaEncabezado;
                			var Request = JSON.stringify(Factura);
                            //CancelacionWebService(Request);
                            ObtenerFacturaACancelar(Request);
                		},*/
                        "Salir": function () {
                            $(this).dialog("close");
                        }
                    });
                }
                $("#dialogConsultarFacturaEncabezado").dialog("option", "height", "auto");
            }
            else {
                $("#dialogConsultarFacturaEncabezado").dialog("option", "buttons", {});
                $("#dialogConsultarFacturaEncabezado").dialog("option", "height", "auto");
            }
            $("#dialogConsultarFacturaEncabezado").dialog("open");
        }
    });
}

function ObtenerFormaConsultarFacturaFormato(pRequest) {
    $("#dialogFacturaFormato").obtenerVista({
        nombreTemplate: "tmplFacturaFormato.html",
        parametros: pRequest,
        url: "FacturaCliente.aspx/ObtieneFacturaFormato",
        despuesDeCompilar: function (pRespuesta) {
            console.log(pRequest);
            jQuery("#dialogFacturaFormato").empty();
            jQuery("#dialogFacturaFormato").append('<iframe src="' + pRespuesta.modelo.Ruta + '" style="width:750px; height:550px;"></iframe>');
            $("#dialogFacturaFormato").dialog("open");
        }
    });
}

function ObtenerFormaConsultarFacturaAddenda(pRequest) {
    $("#dialogFacturaAddenda").obtenerVista({
        nombreTemplate: "tmplFacturaAddenda.html",
        parametros: pRequest,
        url: "FacturaCliente.aspx/ObtieneFacturaAddenda",
        despuesDeCompilar: function (pRespuesta) {
            $("#dialogFacturaAddenda").dialog("open");
        }
    });
}

function ObtenerFormaConsultarFacturaXML(pRequest) {
    $.ajax({
        url: "FacturaCliente.aspx/ObtieneFacturaXML",
        data: pRequest,
        type: "post",
        contentType: 'application/json; charset=utf-8',
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                //window.location.href = json.Ruta;
                window.open(json.Ruta);
                //window.location = 'FacturaCliente.aspx/DownloadFacturaXML?fileGuid=' + response.FileGuid+ '&filename=' + response.FileName;

            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function ObtenerFormaFacturasSustituye(pRequest) {
    $("#dialogFacturasSustituye").obtenerVista({
        nombreTemplate: "tmplFacturasSustituye.html",
        parametros: pRequest,
        url: "FacturaCliente.aspx/LlenaFacturasSustituye",
        despuesDeCompilar: function () {
            Inicializar_grdFacturasSustituye();
            $("#divFormaAgregarFacturasSustituye").on("click", "#btnAgregarFacturaSustituye", function () {
                AgregarFacturaSustituye();
            });

            $("#grdFacturasSustituye").on("click", ".imgEliminarFacturaEncabezadoSustituye", function () {

                var registro = $(this).parents("tr");
                var pFacturaEncabezadoSustituye = new Object();
                pFacturaEncabezadoSustituye.pIdFacturaEncabezadoSustituye = parseInt($(registro).children("td[aria-describedby='grdFacturasSustituye_IdFacturaEncabezadoSustituye']").html());
                var oRequest = new Object();
                oRequest.pFacturaEncabezadoSustituye = pFacturaEncabezadoSustituye;
                SetEliminarFacturaEncabezadoSustituye(JSON.stringify(oRequest));
            });

            $("#dialogFacturasSustituye").dialog("open");
        }
    });
}

function ObtenerFormaEditarFacturaEncabezado(IdFacturaEncabezado) {
    $("#dialogEditarFacturaEncabezado").obtenerVista({
        nombreTemplate: "tmplEditarFacturaEncabezado.html",
        url: "FacturaCliente.aspx/ObtenerFormaEditarFacturaEncabezado",
        parametros: IdFacturaEncabezado,
        despuesDeCompilar: function (pRespuesta) {

            $("#divDireccionesTabsEditar").tabs();
            $("#divDetalleTabs").tabs();
            AutocompletarCliente();
            AutocompletarProyecto();
            Inicializar_grdPedidoDetalle();
            Inicializar_grdFacturaDetalleEditar();
            Inicializar_grdConceptoProyecto();

            $("#grdPedidoDetalle").on("click", "td", function () {
                $("#divFormaEditarFacturaEncabezado").attr("idProducto", "0");
                $("#divFormaEditarFacturaEncabezado").attr("idServicio", "0");
                var registro = $(this).parents("tr");
                var DetallePedido = new Object();
                var cantidad = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_CantidadPendiente']").html());
                var IdCotizacionDetalle = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_IdCotizacionDetalle']").html());
                var IdProducto = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_IdProducto']").html());
                var IdServicio = parseInt($(registro).children("td[aria-describedby='grdPedidoDetalle_IdServicio']").html());
                var PrecioUnitario = $(registro).children("td[aria-describedby='grdPedidoDetalle_Precio']").html();
                var IdTipoIVA = $(registro).children("td[aria-describedby='grdPedidoDetalle_IdTipoIVA']").html();
                var IVA = $(registro).children("td[aria-describedby='grdPedidoDetalle_IVA']").html();
                if (cantidad > 0) {
                    $("#txtCantidadFacturar").val(cantidad);
                    $("#divFormaEditarFacturaEncabezado").attr("cantidadDetallePedido", cantidad);
                    $("#divFormaEditarFacturaEncabezado").attr("idCotizacionDetalle", IdCotizacionDetalle);
                    $("#divFormaEditarFacturaEncabezado").attr("idProducto", IdProducto);
                    $("#divFormaEditarFacturaEncabezado").attr("idServicio", IdServicio);
                    $("#divFormaEditarFacturaEncabezado").attr("precioUnitario", PrecioUnitario);
                    $("#divFormaEditarFacturaEncabezado").attr("idTipoIVA", IdTipoIVA);
                    $("#txtIVA").val(IVA);
                    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
                    $("#txtProyecto").val("");
                    $("#grdConceptoProyecto").trigger("reloadGrid");
                } else {
                    MostrarMensajeError("La cantidad es igual a 0, no se puede bajar la partida.");
                }
            });

            $("#divFormaEditarFacturaEncabezado").on("click", "#btnAgregarPartidaFactura", function () {
                AgregarDetalleFactura();
            });

            $("#grdFacturaDetalleEditar").on("click", ".imgEliminarConcepto", function () {
                var registro = $(this).parents("tr");
                var pFacturaDetalle = new Object();
                pFacturaDetalle.pIdFacturaDetalle = parseInt($(registro).children("td[aria-describedby='grdFacturaDetalleEditar_IdFacturaDetalle']").html());
                var oRequest = new Object();
                oRequest.pFacturaDetalle = pFacturaDetalle;
                SetEliminarFacturaDetalle(JSON.stringify(oRequest));
            });

            if ($("#chkSinDocumentacion").is(':checked')) {
                MuestraPedidosSinDocumentacion(1);
            }
            else {
                MuestraPedidosSinDocumentacion(0);
            }

            $("#grdFacturaDetalleEditar").on("click", "td", function () {
                if ($(this).index() == 4) {
                    var registro = $(this).parents("tr");
                    var pIdFacturaDetalle = parseInt($(registro).children("td[aria-describedby='grdFacturaDetalleEditar_IdFacturaDetalle']").html());
                    ObtenerFormaEditarDetallePartida(pIdFacturaDetalle);
                }
            });

            $("#dialogEditarFacturaEncabezado").dialog("open");
            $("#txtFechaPago").datepicker();
            $("#txtFechaFacturar").datepicker();
            if (pRespuesta.modelo.Permisos.puedeEditarFacturaEncabezado == 1) {

                if (pRespuesta.modelo.IdTxtTimbradosFactura == 0) {

                    $("#dialogEditarFacturaEncabezado").dialog("option", "buttons", {
                        "Timbrar": function () {
                            var Factura = new Object();
                            Factura.IdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado");

                            if (Factura.IdFacturaEncabezado != "0" && Factura.IdFacturaEncabezado != "" && Factura.IdFacturaEncabezado != null) {
                                if ($("#chkEsRefactura").is(':checked')) {
                                    ObtenerFormaFacturasSustituye(JSON.stringify(Factura));
                                }
                                else {
                                    TimbrarFactura();
                                }
                            }
                            else {
                                MostrarMensajeError("No se puede timbrar la factura hasta que se grave una partida");
                            }
                        },
                        "Editar": function () {
                            EditarFacturaEncabezado();
                        },
                        "Salir": function () {
                            $(this).dialog("close")
                        }
                    });
                }
                else {
                    $("#dialogEditarFacturaEncabezado").dialog("option", "buttons", {
                        "Salir": function () {
                            $(this).dialog("close")
                        }
                    });
                }
                $("#dialogEditarFacturaEncabezado").dialog("option", "height", "auto");
            }
            else {
                $("#dialogEditarFacturaEncabezado").dialog("option", "buttons", {});
                $("#dialogEditarFacturaEncabezado").dialog("option", "height", "100");
            }

        }
    });
}

function ObtenerFormaFiltrosFactura() {
    $("#divFiltrosFacturaCliente").obtenerVista({
        nombreTemplate: "tmplFiltrosFacturaCliente.html",
        url: "FacturaCliente.aspx/ObtenerFormaFiltrofacturaCliente",
        despuesDeCompilar: function (pRespuesta) {

            if ($("#txtFechaInicial").length) {
                $("#txtFechaInicial").datepicker({
                    onSelect: function () {
                        FiltroFacturas();
                    }
                });
            }

            if ($("#txtFechaFinal").length) {
                $("#txtFechaFinal").datepicker({
                    onSelect: function () {
                        FiltroFacturas();
                    }
                });
            }

            $('#divFiltrosFacturaCliente').on('click', '#chkPorFecha', function (event) {
                FiltroFacturas();
            });

            $('#divFiltrosFacturaCliente').on('change', '#cmbFiltroTimbrado', function (event) {
                FiltroFacturas();
            });

            $('#divFiltrosFacturaCliente').on('focusout', '#txtNumeroPedidoBuscador', function (event) {
                FiltroFacturas();
            });

            $('#divFiltrosFacturaCliente').on('change', '#cmbBusquedaDocumento', function (event) {
                FiltroFacturas();
            });
        }
    });
}

function ObtenerFormaDatosCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: 'POST',
        url: 'FacturaCliente.aspx/ObtenerFormaDatosCliente',
        data: pRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionOrganizacion", respuesta.Modelo.IdDireccionOrganizacion);
            $('#txtCliente').val(respuesta.Modelo.RazonSocial);
            $("#spanRFC").text(respuesta.Modelo.RFC);
            $("#spanNombreComercial").text(respuesta.Modelo.NombreComercial);
            $("#spanPorcentaje").text("0");
            $("#cmbUsuarioAgente option[value=" + respuesta.Modelo.IdUsuarioAgente + "]").attr("selected", true);

            $("#cmbDireccionFiscal").obtenerVista({
                modelo: respuesta.Modelo.DireccionesFiscales,
                nombreTemplate: "tmplComboGenerico.html",
                despuesDeCompilar: function (pRespuesta) {
                }
            });

            $("#cmbDireccionCliente").obtenerVista({
                modelo: respuesta.Modelo.DireccionesEntrega,
                nombreTemplate: "tmplComboGenerico.html",
                despuesDeCompilar: function (pRespuesta) {
                }
            });

            $("#cmbPedido").obtenerVista({
                modelo: respuesta.Modelo.Pedidos,
                nombreTemplate: "tmplComboGenerico.html",
                despuesDeCompilar: function (pRespuesta) {
                }
            });

            $("#cmbDescuento").obtenerVista({
                modelo: respuesta.Modelo.DescuentosCliente,
                nombreTemplate: "tmplComboGenerico.html",
                despuesDeCompilar: function (pRespuesta) {
                }
            });

            OcultarBloqueo();
        }
    });
}

function ObtenerFormaDatosClienteCambiosFiscales(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: 'POST',
        url: 'FacturaCliente.aspx/ObtenerFormaDatosCliente',
        data: pRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);

            $("#spanRFC").text(respuesta.Modelo.RFC);
            $("#spanNombreComercial").text(respuesta.Modelo.NombreComercial);
            $("#spanCalleFiscal").text(respuesta.Modelo.CalleFiscal);
            $("#spanNumeroExteriorFiscal").text(respuesta.Modelo.NumeroExteriorFiscal);
            $("#spanNumeroInteriorFiscal").text(respuesta.Modelo.NumeroInteriorFiscal);
            $("#spanColoniaFiscal").text(respuesta.Modelo.ColoniaFiscal);
            $("#spanCodigoPostalFiscal").text(respuesta.Modelo.CodigoPostalFiscal);
            $("#spanConmutadorFiscal").text(respuesta.Modelo.ConmutadorFiscal);
            $("#spanPaisFiscal").text(respuesta.Modelo.PaisFiscal);
            $("#spanEstadoFiscal").text(respuesta.Modelo.EstadoFiscal);
            $("#spanMunicipioFiscal").text(respuesta.Modelo.MunicipioFiscal);
            $("#spanLocalidadFiscal").text(respuesta.Modelo.LocalidadFiscal);
            $("#txtReferenciaFiscal").val(respuesta.Modelo.ReferenciaFiscal);
            OcultarBloqueo();
        }
    });
}

function ObtenerFormaDatosFiscalesCliente(pValidacion, pIdCliente) {
    $("#dialogDatosFiscalesFactura").obtenerVista({
        nombreTemplate: "tmplDatosFiscalesFactura.html",
        url: "FacturaCliente.aspx/ObtenerFormaDatosFiscalesCliente",
        parametros: pIdCliente,
        despuesDeCompilar: function (pRespuesta) {
            respuesta = pRespuesta;
            $("#dialogDatosFiscalesFactura").dialog("open");
            MostrarMensajeError(pValidacion);
        }
    });
}

function ObtenerNumerosCuenta(pIdCliente) {
    $("#cmbNumeroCuenta").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/ObtenerNumerosCuenta",
        parametros: pIdCliente,
        despuesDeCompilar: function (pRespuesta) {
        }
    });
}

function ObtenerFacturasCliente(pIdCliente) {
    $("#cmbFactruaRelacionado").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/ObtenerFacturasCliente",
        parametros: pIdCliente,
        despuesDeCompilar: function (pRespuesta) {
        }
    });
}

function ObtenerContactosOrganizacion(pRequest) {
    $("#cmbContactosOrganizacion").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "FacturaCliente.aspx/ObtenerContactosOrganizacion",
        parametros: pRequest,
        despuesDeComplilar: function (pRespuesta) {
            alert("hola");
        }
    });
}

function ObtenerPorcentaje(pRequest) {
    $.ajax({
        type: 'POST',
        url: 'FacturaCliente.aspx/ObtenerPorcentaje',
        data: pRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            $("#spanPorcentaje").text(respuesta.Modelo.Porcentaje);
        }
    });
}

function ObtenerDireccionCliente(pRequest) {
    $.ajax({
        type: 'POST',
        url: 'FacturaCliente.aspx/ObtenerDireccionCliente',
        data: pRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdDireccionEntrega", respuesta.Modelo.IdDireccionCliente);
            $("#spanCalleEntregaEntrega").text(respuesta.Modelo.Calle);
            $("#spanNumeroExteriorEntrega").text(respuesta.Modelo.NumeroExterior);
            $("#spanNumeroInteriorEntrega").text(respuesta.Modelo.NumeroInterior);
            $("#spanColoniaEntrega").text(respuesta.Modelo.Colonia);
            $("#spanPaisEntrega").text(respuesta.Modelo.Pais);
            $("#spanEstadoEntrega").text(respuesta.Modelo.Estado);
            $("#spanCodigoPostalEntrega").text(respuesta.Modelo.CodigoPostal);
            $("#spanTelefonoEntrega").text(respuesta.Modelo.ConmutadorTelefono);
            $("#spanMunicipioEntrega").text(respuesta.Modelo.Municipio);
            $("#spanLocalidadEntrega").text(respuesta.Modelo.Localidad);
            $("#txtReferenciaEntrega").val(respuesta.Modelo.Referencia);
        }
    });
}

function ObtenerDireccionFiscal(pRequest) {
    $.ajax({
        type: 'POST',
        url: 'FacturaCliente.aspx/ObtenerDireccionFiscal',
        data: pRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (pRespuesta) {
            var respuesta = $.parseJSON(pRespuesta.d);
            $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdDireccionFiscal", respuesta.Modelo.IdDireccionFiscal);
            $("#spanCalleFiscal").text(respuesta.Modelo.Calle);
            $("#spanNumeroExteriorFiscal").text(respuesta.Modelo.NumeroExterior);
            $("#spanNumeroInteriorFiscal").text(respuesta.Modelo.NumeroInterior);
            $("#spanColoniaFiscal").text(respuesta.Modelo.Colonia);
            $("#spanCodigoPostalFiscal").text(respuesta.Modelo.CodigoPostal);
            $("#spanConmutadorFiscal").text(respuesta.Modelo.Conmutador);
            $("#spanPaisFiscal").text(respuesta.Modelo.Pais);
            $("#spanEstadoFiscal").text(respuesta.Modelo.Estado);
            $("#spanMunicipioFiscal").text(respuesta.Modelo.Municipio);
            $("#spanLocalidadFiscal").text(respuesta.Modelo.Localidad);
            $("#txtReferenciaFiscal").val(respuesta.Modelo.Referencia);
        }
    });
}

function FiltroFacturaDetalle() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaDetalle').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaDetalle').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaDetalle').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaDetalle').getGridParam('sortorder');
    request.pIdFacturaEncabezado = 0;
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != null && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != "") {
        request.pIdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado")
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerFacturaDetalle',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaDetalle')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturaDetalleConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaDetalleConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaDetalleConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaDetalleConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaDetalleConsultar').getGridParam('sortorder');
    request.pIdFacturaEncabezado = 0;
    if ($("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != null && $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != "") {
        request.pIdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado")
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerFacturaDetalleConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaDetalleConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturaDetalleEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaDetalleEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaDetalleEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaDetalleEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaDetalleEditar').getGridParam('sortorder');
    request.pIdFacturaEncabezado = 0;
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != null && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado") != "") {
        request.pIdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdFacturaEncabezado")
    }
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerFacturaDetalleEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaDetalleEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroPedidoDetalle() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPedidoDetalle').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPedidoDetalle').getGridParam('page');
    request.pColumnaOrden = $('#grdPedidoDetalle').getGridParam('sortname');
    request.pTipoOrden = $('#grdPedidoDetalle').getGridParam('sortorder');
    request.pIdPedido = 0;
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdPedido") != null && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdPedido") != "") {
        request.pIdPedido = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdPedido")
    }

    request.pIdTipoMonedaFactura = 0;
    request.pTipoCambioFactura = 0;
    if ($("#cmbTipoMoneda").val() != null && $("#cmbTipoMoneda").val() != "") {
        request.pIdTipoMonedaFactura = $("#cmbTipoMoneda").val();
    }
    if ($("#spanTipoCambioFactura").text() != "") {
        request.pTipoCambioFactura = $("#spanTipoCambioFactura").text();
    }

    request.pIdTipoMonedaPedido = 0;
    request.pTipoCambioPedido = 0;
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idTipoMonedaPedido") != null && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idTipoMonedaPedido") != "") {
        request.pIdTipoMonedaPedido = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idTipoMonedaPedido");
    }

    if ($("#spanTipoCambioPedido").text() != "") {
        request.pTipoCambioPedido = $("#spanTipoCambioPedido").text();
    }
    request.NuevoCotizador = (($("#chkNuevoCotizador").is(":checked")) ? 1 : 0);

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerPedidoDetalle',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdPedidoDetalle')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroConceptoProyecto() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdConceptoProyecto').getGridParam('rowNum');
    request.pPaginaActual = $('#grdConceptoProyecto').getGridParam('page');
    request.pColumnaOrden = $('#grdConceptoProyecto').getGridParam('sortname');
    request.pTipoOrden = $('#grdConceptoProyecto').getGridParam('sortorder');
    request.pIdProyecto = 0;
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdProyecto") != null && $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdProyecto") != "") {
        request.pIdProyecto = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdProyecto")
    }

    request.pIdTipoMonedaFactura = 0;
    request.pTipoCambioFactura = 0;
    if ($("#cmbTipoMoneda").val() != null && $("#cmbTipoMoneda").val() != "") {
        request.pIdTipoMonedaFactura = $("#cmbTipoMoneda").val();
    }
    if ($("#spanTipoCambioFactura").text() != "") {
        request.pTipoCambioFactura = $("#spanTipoCambioFactura").text();
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerConceptoProyecto',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdConceptoProyecto')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroFacturasSustituye() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturasSustituye').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturasSustituye').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturasSustituye').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturasSustituye').getGridParam('sortorder');
    request.pIdFacturaEncabezado = 0;
    if ($("#divFormaAgregarFacturasSustituye").attr("IdFacturaEncabezado") != null && $("#divFormaAgregarFacturasSustituye").attr("IdFacturaEncabezado") != "") {
        request.pIdFacturaEncabezado = $("#divFormaAgregarFacturasSustituye").attr("IdFacturaEncabezado")
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerFacturasSustituye',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturasSustituye')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
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
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pSerieFactura = "";
    request.pNumeroFactura = "";
    request.pPorFecha = 0;
    request.pAI = 0;
    request.pFiltroTimbrado = 0;
    request.pIdDivision = -1;
    request.pFolio = 0;
    request.pIdProyecto;
    request.pRazonSocial = "";
    request.pAgente = "";
    request.pNumeroPedido = "";
    request.pBusquedaDocumento = 0;
    request.pEstatusFacturaEncabezado = "";

    if ($('#gs_SerieFactura').val() != null) { request.pSerieFactura = $("#gs_SerieFactura").val(); }
    if ($('#gs_NumeroFactura').val() != null) { request.pNumeroFactura = $("#gs_NumeroFactura").val(); }

    request.pIdDivision = ($("#gs_IdDivision").val() != null) ? parseInt($("#gs_IdDivision").val()) : -1;
    request.pFolio = ($("#gs_Folio").val() != null && $("#gs_Folio").val() != "") ? parseInt($("#gs_Folio").val()) : 0;
    request.pIdProyecto = ($("#gs_IdProyecto").val() != null && $("#gs_IdProyecto").val() != "") ? parseInt($("#gs_IdProyecto").val()) : 0;

    if ($('#gs_RazonSocial').val() != null) { request.pRazonSocial = $("#gs_RazonSocial").val(); }
    if ($('#gs_Agente').val() != null) { request.pAgente = $("#gs_Agente").val(); }
    if ($('#gs_AI').val() != null) { request.pAI = $("#gs_AI").val(); }

    if ($("#txtFechaInicial").val() != "" && $("#txtFechaInicial").val() != null) {

        if ($("#chkPorFecha").is(':checked')) { request.pPorFecha = 1; }
        else { request.pPorFecha = 0; }
        request.pFiltroTimbrado = $("#cmbFiltroTimbrado").val();

        request.pFechaInicial = $("#txtFechaInicial").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinal").val() != "" && $("#txtFechaFinal").val() != null) {
        request.pFechaFinal = $("#txtFechaFinal").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }

    if ($("#txtNumeroPedidoBuscador").val() != "" && $("#txtNumeroPedidoBuscador").val() != null) {
        request.pNumeroPedido = $("#txtNumeroPedidoBuscador").val();
    }

    if ($("#cmbBusquedaDocumento").val() != "" && $("#cmbBusquedaDocumento").val() != null) {
        request.pBusquedaDocumento = $("#cmbBusquedaDocumento").val();
    }

    if ($('#gs_EstatusFacturaEncabezado').val() != null) {
        request.pEstatusFacturaEncabezado = $("#gs_EstatusFacturaEncabezado").val();
    }


    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'FacturaCliente.aspx/ObtenerFacturas',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturas')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

//-----------AJAX-----------//
//-Funciones de Acciones-//
function AgregarDetalleFactura() {
    var pFactura = new Object();
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado") == null) {
        pFactura.IdFacturaEncabezado = 0;
    }
    else {
        pFactura.IdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado");
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") == null) {
        pFactura.IdCliente = 0;
    }
    else {
        pFactura.IdCliente = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente");
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto") == null) {
        pFactura.IdProyecto = 0;
    }
    else {
        pFactura.IdProyecto = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto");
    }

    if (pFactura.IdProyecto != 0) {

        var detalle = $("#grdConceptoProyecto").jqGrid('getGridParam', 'records');
        if (detalle != 0) {
            pFactura.IdsConceptosProyectos = new Array();
            $(".chkElegir:checked").each(function (index, object) {
                var registro = $(this).parents("tr");
                pFactura.IdsConceptosProyectos.push($(registro).children("td[aria-describedby='grdConceptoProyecto_IdConceptoProyecto']").text());
            });
            pFactura.NumeroPartidas = 1;
        }
        else {
            pFactura.NumeroPartidas = 0;
        }
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCotizacionDetalle") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCotizacionDetalle") == null) {
        pFactura.IdCotizacionDetalle = 0;
    }
    else {
        pFactura.IdCotizacionDetalle = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCotizacionDetalle");
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProducto") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProducto") == null) {
        pFactura.IdProducto = 0;
    }
    else {
        pFactura.IdProducto = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProducto");
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idServicio") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idServicio") == null) {
        pFactura.IdServicio = 0;
    }
    else {
        pFactura.IdServicio = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idServicio");
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idTipoIVA") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idTipoIVA") == null) {
        pFactura.IdTipoIVA = 0;
    }
    else {
        pFactura.IdTipoIVA = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idTipoIVA");
    }
    pFactura.IVA = $("#txtIVA").val();

    var RFCFinal = "";

    pFactura.RegimenFiscal = $("#spanRegimenFiscal").text();
    pFactura.LugarExpedicion = $("#spanLugarExpedicion").text();
    pFactura.FechaActual = $("#spanFechaActual").text();
    pFactura.NombreComercial = $("#spanNombreComercial").text();
    pFactura.RazonSocial = $("#txtCliente").val();
    pFactura.IdContactoCliente = $("#cmbContactosOrganizacion").val();

    RFCFinal = $("#spanRFC").text();
    pFactura.RFC = RFCFinal.replace(/([.*+?^=!:${}()|\[\]\/\\-])/gi, "");

    pFactura.IdCondicionPago = $("#cmbCondicionPago").val();
    pFactura.CondicionPago = $("#cmbCondicionPago option:selected").html();
    pFactura.IdMetodoPago = $("#cmbMetodoPago").val();
    pFactura.MetodoPago = $("#cmbMetodoPago option:selected").attr("clave");
    pFactura.FechaPago = $("#txtFechaPago").val();
    pFactura.IdNumeroCuenta = $("#cmbNumeroCuenta").val();
    if (pFactura.IdNumeroCuenta == "" || pFactura.IdNumeroCuenta == null) {
        pFactura.IdNumeroCuenta = 0;
    }
    pFactura.NumeroCuenta = $("#cmbNumeroCuenta option:selected").html();

    pFactura.IdDescuentoCliente = $("#cmbDescuento").val()
    pFactura.PorcentajeDescuento = $("#spanPorcentaje").text();

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdDireccionFiscal") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdDireccionFiscal") == null) {
        pFactura.IdDireccionOrganizacion = 0;
    }
    else {
        pFactura.IdDireccionOrganizacion = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("IdDireccionFiscal");
    }

    pFactura.CalleFiscal = $("#spanCalleFiscal").text();
    pFactura.NumeroExteriorFiscal = $("#spanNumeroExteriorFiscal").text();
    pFactura.NumeroInteriorFiscal = $("#spanNumeroInteriorFiscal").text();
    pFactura.ColoniaFiscal = $("#spanColoniaFiscal").text();
    pFactura.PaisFiscal = $("#spanPaisFiscal").text();
    pFactura.EstadoFiscal = $("#spanEstadoFiscal").text();
    pFactura.CodigoPostalFiscal = $("#spanCodigoPostalFiscal").text();
    pFactura.ConmutadorFiscal = $("#spanConmutadorFiscal").text();
    pFactura.MunicipioFiscal = $("#spanMunicipioFiscal").text();
    pFactura.LocalidadFiscal = $("#spanLocalidadFiscal").text();
    pFactura.ReferenciaFiscal = $("#txtReferenciaFiscal").val();

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionEntrega") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionEntrega") == null) {
        pFactura.IdDireccionEntrega = 0;
    }
    else {
        pFactura.IdDireccionEntrega = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionEntrega");
    }

    pFactura.FechaFacturar = $("#txtFechaFacturar").val();
    pFactura.IdSerieFactura = $("#cmbSerieFactura").val();
    pFactura.NumeroFactura = $("#txtNumeroFactura").val();
    pFactura.NumeroOrdenCompra = $("#txtNumeroOrdenCompra").val();

    if ($("#chkEsRefactura").is(':checked')) {
        pFactura.EsRefactura = 1;
    }
    else {
        pFactura.EsRefactura = 0;
    }
    pFactura.IdTipoMoneda = $("#cmbTipoMoneda").val();

    pFactura.TipoCambioFactura = $("#spanTipoCambioFactura").text();

    if ($("#chkParcialidades").is(':checked')) {
        pFactura.Parcialidades = 1;
    }
    else {
        pFactura.Parcialidades = 0;
    }

    if ($("#chkSinIVA").is(':checked')) {
        pFactura.SinIVA = 1;
    }
    else {
        pFactura.SinIVA = 0;
    }

    pFactura.IdUsoCFDI = $("#cmbUsoCFDI").val();
    pFactura.IdFacturaAnticipo = $("#cmbFacturaRelacionado").val();
    pFactura.IdTipoRelacion = $("#cmbTipoRelacion").val();
    if ($("#chkAnticipo").is(':checked')) {
        pFactura.Anticipo = 1;
    }
    else {
        pFactura.Anticipo = 0;
    }
    pFactura.NoParcialidades = $("#txtNoParcialidades").val();
    pFactura.IdUsuarioAgente = $("#cmbUsuarioAgente").val();
    pFactura.IdDivision = $("#cmbDivision").val();

    pFactura.NotaFactura = $("#txtNotaFactura").val();
    pFactura.IdPedido = $("#cmbPedido").val();
    pFactura.TipoCambioPedido = $("#spanTipoCambioPedido").text();
    pFactura.CantidadFacturar = $("#txtCantidadFacturar").val();
    pFactura.CantidadDetallePedido = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("cantidadDetallePedido");

    pFactura.PrecioUnitario = QuitaFormatoMoneda($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("precioUnitario"));
    pFactura.Total = parseInt(pFactura.CantidadFacturar) * parseFloat(pFactura.PrecioUnitario);
    pFactura.NuevoCotizador = (($("#chkNuevoCotizador").is(":checked")) ? 1 : 0);

    var validacion = ValidaDetalleFactura(pFactura);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }

    var validacionFiscal = ValidaDatosFiscales(pFactura);
    if (validacionFiscal != "") {
        var Cliente = new Object();
        Cliente.IdCliente = parseInt(pFactura.IdCliente);
        ObtenerFormaDatosFiscalesCliente(validacionFiscal, JSON.stringify(Cliente))
        return false;
    }

    var oRequest = new Object();
    oRequest.pFactura = pFactura;
    SetAgregarDetalleFactura(JSON.stringify(oRequest));
}

function AgregarFacturaSustituye() {
    var pFactura = new Object();
    if ($("#divFormaAgregarFacturasSustituye").attr("idFacturaEncabezado") == "" || $("#divFormaAgregarFacturasSustituye").attr("idFacturaEncabezado") == null) {
        pFactura.IdFacturaEncabezado = 0;
    }
    else {
        pFactura.IdFacturaEncabezado = $("#divFormaAgregarFacturasSustituye").attr("idFacturaEncabezado");
    }

    pFactura.IdSerieFactura = $("#cmbSerieFactura").val();
    pFactura.NumeroFactura = $("#txtNumeroFacturaSustituye").val();

    var validacion = ValidaFacturaSustituye(pFactura);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }

    var oRequest = new Object();
    oRequest.pFactura = pFactura;
    SetAgregarFacturaSustituye(JSON.stringify(oRequest));
}

function EditarFacturaEncabezado() {
    var pFactura = new Object();
    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado") == null) {
        pFactura.IdFacturaEncabezado = 0;
    }
    else {
        pFactura.IdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado");
    }

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente") == null) {
        pFactura.IdCliente = 0;
    }
    else {
        pFactura.IdCliente = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idCliente");
    }
    pFactura.RazonSocial = $("#txtCliente").val();
    pFactura.IdContactoCliente = $("#cmbContactosOrganizacion").val();

    pFactura.RegimenFiscal = $("#spanRegimenFiscal").text();
    pFactura.LugarExpedicion = $("#spanLugarExpedicion").text();
    pFactura.FechaActual = $("#spanFechaActual").text();
    pFactura.NombreComercial = $("#spanNombreComercial").text();
    pFactura.RazonSocial = $("#txtCliente").val();
    pFactura.RFC = $("#spanRFC").text();
    pFactura.IdCondicionPago = $("#cmbCondicionPago").val();
    pFactura.CondicionPago = $("#cmbCondicionPago option:selected").html();
    pFactura.IdMetodoPago = $("#cmbMetodoPago").val();
    //pFactura.MetodoPago = $("#cmbMetodoPago option:selected").attr("clave");
    pFactura.FechaPago = $("#txtFechaPago").val();
    pFactura.IdNumeroCuenta = $("#cmbNumeroCuenta").val();
    if (pFactura.IdNumeroCuenta == "" || pFactura.IdNumeroCuenta == null) {
        pFactura.IdNumeroCuenta = 0;
    }
    pFactura.NumeroCuenta = $("#cmbNumeroCuenta option:selected").html();

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionOrganizacion") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionOrganizacion") == null) {
        pFactura.IdDireccionOrganizacion = 0;
    }
    else {
        pFactura.IdDireccionOrganizacion = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionOrganizacion");
    }

    pFactura.IdDescuentoCliente = $("#cmbDescuento").val()
    pFactura.PorcentajeDescuento = $("#spanPorcentaje").text();

    pFactura.CalleFiscal = $("#spanCalleFiscal").text();
    pFactura.NumeroExteriorFiscal = $("#spanNumeroExteriorFiscal").text();
    pFactura.NumeroInteriorFiscal = $("#spanNumeroInteriorFiscal").text();
    pFactura.ColoniaFiscal = $("#spanColoniaFiscal").text();
    pFactura.PaisFiscal = $("#spanPaisFiscal").text();
    pFactura.EstadoFiscal = $("#spanEstadoFiscal").text();

    pFactura.CodigoPostalFiscal = $("#spanCodigoPostalFiscal").text();
    pFactura.ConmutadorFiscal = $("#spanConmutadorFiscal").text();
    pFactura.MunicipioFiscal = $("#spanMunicipioFiscal").text();
    pFactura.LocalidadFiscal = $("#spanLocalidadFiscal").text();
    pFactura.ReferenciaFiscal = $("#txtReferenciaFiscal").val();

    if ($("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionEntrega") == "" || $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionEntrega") == null) {
        pFactura.IdDireccionEntrega = 0;
    }
    else {
        pFactura.IdDireccionEntrega = $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idDireccionEntrega");
    }

    pFactura.FechaFacturar = $("#txtFechaFacturar").val();
    pFactura.IdSerieFactura = $("#cmbSerieFactura").val();
    pFactura.NumeroFactura = $("#txtNumeroFactura").val();
    pFactura.NumeroOrdenCompra = $("#txtNumeroOrdenCompra").val();

    if ($("#chkEsRefactura").is(':checked')) {
        pFactura.EsRefactura = 1;
    }
    else {
        pFactura.EsRefactura = 0;
    }
    pFactura.IdTipoMoneda = $("#cmbTipoMoneda").val();

    if ($("#chkSinIVA").is(':checked')) {
        pFactura.SinIVA = 1;
    }
    else {
        pFactura.SinIVA = 0;
    }

    pFactura.TipoCambioFactura = $("#spanTipoCambioFactura").text();

    if ($("#chkParcialidades").is(':checked')) {
        pFactura.Parcialidades = 1;
    }
    else {
        pFactura.Parcialidades = 0;
    }
    pFactura.IdUsoCFDI = $("#cmbUsoCFDI").val();
    pFactura.IdFacturaAnticipo = $("#cmbFacturaRelacionado").val();
    pFactura.IdTipoRelacion = $("#cmbTipoRelacion").val();
    if ($("#chkAnticipo").is(':checked')) {
        pFactura.Anticipo = 1;
    }
    else {
        pFactura.Anticipo = 0;
    }
    pFactura.NoParcialidades = $("#txtNoParcialidades").val();
    pFactura.IdUsuarioAgente = $("#cmbUsuarioAgente").val();
    pFactura.IdDivision = $("#cmbDivision").val();
    pFactura.NotaFactura = $("#txtNotaFactura").val();

    var validacion = ValidaFacturaEncabezado(pFactura);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }

    var validacionFiscal = ValidaDatosFiscales(pFactura);
    if (validacionFiscal != "") {
        var Cliente = new Object();
        Cliente.IdCliente = parseInt(pFactura.IdCliente);
        ObtenerFormaDatosFiscalesCliente(validacionFiscal, JSON.stringify(Cliente))
        return false;
    }

    var oRequest = new Object();
    oRequest.pFactura = pFactura;
    SetEditarFacturaEncabezado(JSON.stringify(oRequest));
}

function CancelarFacturaEncabezado() {
    var pFacturaEncabezado = new Object();
    if ($("#divFormaAgregarMotivoCancelacionFactura").attr("idFacturaEncabezado") == "" || $("#divFormaAgregarMotivoCancelacionFactura").attr("idFacturaEncabezado") == null) {
        MostrarMensajeError("No hay factura para cancelar"); return false;
    }
    else {
        pFacturaEncabezado.IdFacturaEncabezado = $("#divFormaAgregarMotivoCancelacionFactura").attr("idFacturaEncabezado");
        pFacturaEncabezado.MotivoCancelacion = $("#txtMotivoCancelacion").val();
        var validacion = ValidaMotivoCancelacionFactura(pFacturaEncabezado);
        if (validacion != "")
        { MostrarMensajeError(validacion); return false; }
        //var oRequest = new Object();
        //oRequest.pFacturaEncabezado = pFacturaEncabezado;
        //SetCancelarFacturaEncabezado(JSON.stringify(oRequest));

        // Nueva Forma de Cancelar
        //console.log(pFacturaEncabezado);
        var oRequest = new Object();
        oRequest.IdFacturaEncabezado = parseInt( pFacturaEncabezado.IdFacturaEncabezado );
        oRequest.MotivoCancelacion = pFacturaEncabezado.MotivoCancelacion;
        ObtenerFacturaACancelar(JSON.stringify(oRequest));

    }
}

function SetCancelarFacturaEncabezado(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/CancelarFacturaEncabezado",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdFacturas").trigger("reloadGrid");
                $("#dialogMotivoCancelacionFactura").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetAgregarDetalleFactura(pRequest) {
    console.log(pRequest);
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/AgregarDetalleFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado", respuesta.IdFacturaEncabezado);
                $("#txtSubtotalFactura").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtDescuento").text(formato.moneda(respuesta.DescuentoFactura, "$"));
                $("#txtSubtotalDescuento").text(formato.moneda(respuesta.SubtotalDescuentoFactura, "$"));
                $("#txtIVAFactura").text(formato.moneda(parseFloat(respuesta.IVAFactura), "$"));
                $("#txtTotalFactura").text(formato.moneda(parseFloat(respuesta.TotalFactura), "$"));
                $("#spanCantidadLetra").text(respuesta.TotalLetraFactura);
                $("#txtNumeroFactura").val(respuesta.NumeroFactura);
                $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("serieTimbrado", respuesta.SerieTimbrado);
                LimpiarDatosDetalleFactura();
                DeshabilitaCamposEncabezado();
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdPedidoDetalle").trigger("reloadGrid");
                $("#grdFacturaDetalle").trigger("reloadGrid");
                $("#grdFacturaDetalleEditar").trigger("reloadGrid");
                $("#grdConceptoProyecto").trigger("reloadGrid");

            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetAgregarFacturaSustituye(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/AgregarFacturaSustituye",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturasSustituye").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetEditarFacturaEncabezado(pRequest) {
    console.log(pRequest);
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/EditarFacturaEncabezado",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdFacturas").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogEditarFacturaEncabezado").dialog("close");
        }
    });
}

function SetEliminarFacturaDetalle(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/EliminarFacturaDetalle",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado", respuesta.IdFacturaEncabezado);
                $("#txtSubtotalFactura").text(formato.moneda(respuesta.SubtotalFactura, "$"));
                $("#txtDescuento").text(formato.moneda(respuesta.DescuentoFactura, "$"));
                $("#txtSubtotalDescuento").text(formato.moneda(respuesta.SubtotalDescuentoFactura, "$"));
                $("#txtIVAFactura").text(formato.moneda(parseFloat(respuesta.IVAFactura), "$"));
                $("#txtTotalFactura").text(formato.moneda(parseFloat(respuesta.TotalFactura), "$"));
                $("#spanCantidadLetra").text(respuesta.TotalLetraFactura);
                LimpiarDatosDetalleFactura();
                DeshabilitaCamposEncabezado();
                $("#grdFacturas").trigger("reloadGrid");
                $("#grdPedidoDetalle").trigger("reloadGrid");
                $("#grdFacturaDetalle").trigger("reloadGrid");
                $("#grdFacturaDetalleEditar").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function SetEliminarFacturaEncabezadoSustituye(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/EliminarFacturaEncabezadoSustituye",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {

                $("#grdFacturasSustituye").trigger("reloadGrid");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function TimbrarFactura() {
    var pFactura = new Object();
    if ($("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado") == "" || $("#divFormaAgregarFactura,#divFormaConsultarFacturaEncabezado,#divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado") == null) {
        MostrarMensajeError("No hay factura para timbrar"); return false;
    }
    else if ($("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("serieTimbrado") == 0) {
        MostrarMensajeError("Esta serie de factura no se puede timbrar."); return false;
    }
    else {
        pFactura.IdFacturaEncabezado = $("#divFormaAgregarFactura, #divFormaConsultarFacturaEncabezado, #divFormaEditarFacturaEncabezado").attr("idFacturaEncabezado");
        //var oRequest = new Object();
        //oRequest.pFactura = pFactura;
        //SetTimbrarFactura(JSON.stringify(oRequest));

        //Nueva Forma de Timbrar
        //console.log(pFactura);
        var oRequest = new Object();
        oRequest.IdFacturaEncabezado = parseInt(pFactura.IdFacturaEncabezado);
        ObtenerFacturaATimbrar(JSON.stringify(oRequest));

    }
}

function SetTimbrarFactura() {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/TimbrarFactura",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError(respuesta.Descripcion);
                $("#grdFacturas").trigger("reloadGrid");
                $("#dialogAgregarFactura, #dialogConsultarFacturaEncabezado, #dialogDatosFiscalesFactura, #dialogEditarFacturaEncabezado, #dialogFacturasSustituye").dialog("close");
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function GenerarAddenda() {

    var pAddenda = new Object();
    pAddenda.IdAddenda = 1;
    pAddenda.DetallePartidas = new Array();

    var cadena = $("#divFormaAgregarFacturaAddenda").attr("atributos");
    var arr = cadena.split(',');

    var data = {};
    var pPartidas = new Object();
    var grid = $('#grdConceptos');
    var rows = grid.jqGrid('getDataIDs');

    for (i = 0; i < rows.length; i++) {
        var rowData = grid.jqGrid('getRowData', rows[i]);
        //        for (a = 0; a < arr.length; a++)
        //        {
        //            data[arr[a]] = rowData[arr[a]];
        //        }
        pAddenda.DetallePartidas.push(rowData);
    }
    //pAddenda.DetallePartidas = data;
    pAddenda.pEncabezados = arr;

    var oRequest = new Object();
    oRequest.pAddenda = pAddenda;
    SetGeneraAddenda(JSON.stringify(oRequest));
}

function SetGeneraAddenda(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/GeneraAddenda",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                //$("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idEncabezadoFacturaProveedor", respuesta.IdEncabezadoFacturaProveedor);

            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function ObtenerDatosFacturaXML(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/ObtenerDatosFacturaXML",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var respuesta = $.parseJSON(pRespuesta.d);
                $("#folio").text(respuesta.Modelo.folio);
                $("#serie").text(respuesta.Modelo.serie);
                $("#tipoDeComprobante").text(respuesta.Modelo.tipoDeComprobante);
                $("#fecha").text(respuesta.Modelo.fecha);

                $("#subTotal").text(respuesta.Modelo.subTotal);
                $("#total").text(respuesta.Modelo.total);
                $("#Moneda").text(respuesta.Modelo.Moneda);
                $("#TipoCambio").text(respuesta.Modelo.TipoCambio);

                $("#nombreemisior").text(respuesta.Modelo.nombreemisior);
                $("#rfcemisior").text(respuesta.Modelo.rfcemisior);
                $("#nombrereceptor").text(respuesta.Modelo.nombrereceptor);
                $("#rfcreceptor").text(respuesta.Modelo.rfcreceptor);

                $("#tasa").text(respuesta.Modelo.tasa);
                $("#importe").text(respuesta.Modelo.importe);

                Names = respuesta.Modelo.Columnas;

                generaDataModel(Names)

            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
        }
    });
}

function EditarDatosFiscalesCliente() {

    var pCliente = new Object();
    pCliente.IdCliente = $("#divFormaDatosFiscalesCliente").attr("idCliente");
    //Datos organizacion
    pCliente.RazonSocial = $("#txtRazonSocial").val();
    pCliente.NombreComercial = $("#txtNombreComercial").val();
    pCliente.RFC = $("#txtRFC").val();
    //Datos direccion de organizacion
    pCliente.Calle = $("#txtCalle").val();
    pCliente.NumeroExterior = $("#txtNumeroExterior").val();
    pCliente.NumeroInterior = $("#txtNumeroInterior").val();
    pCliente.Colonia = $("#txtColonia").val();
    pCliente.CodigoPostal = $("#txtCodigoPostal").val();
    pCliente.Conmutador = $("#txtConmutador").val();
    pCliente.IdMunicipio = $("#cmbMunicipio").val();
    pCliente.Referencia = $("#txtReferencia").val();
    pCliente.IdLocalidad = $("#cmbLocalidad").val();

    var validacion = ValidaCliente(pCliente);
    if (validacion != "")
    { MostrarMensajeError(validacion); return false; }
    var oRequest = new Object();
    oRequest.pCliente = pCliente;
    SetEditarCliente(JSON.stringify(oRequest));
}

function SetEditarCliente(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "FacturaCliente.aspx/EditarCliente",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                var Cliente = new Object();
                Cliente.pIdCliente = parseInt(respuesta.IdCliente);
                var Request = JSON.stringify(Cliente);
                ObtenerFormaDatosClienteCambiosFiscales(Request);
                $("#cmbDireccionFiscal").change();
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
            }
        },
        complete: function () {
            OcultarBloqueo();
            $("#dialogDatosFiscalesFactura").dialog("close");
        }
    });
}

function Imprimir(pIdFacturaEncabezado) {
    MostrarBloqueo();

    var FacturaEncabezado = new Object();
    FacturaEncabezado.IdFacturaEncabezado = pIdFacturaEncabezado;

    var Request = JSON.stringify(FacturaEncabezado);

    var contenedor = $("<div></div>");

    $(contenedor).obtenerVista({
        url: "FacturaCliente.aspx/ImprimirFacturaEncabezado",
        parametros: Request,
        nombreTemplate: "tmplImprimirNotaVenta.html",
        despuesDeCompilar: function (Respuesta) {
            var plantilla = $(contenedor).html();
            var Impresion = window.open("", "");
            Impresion.document.write(plantilla);
            Impresion.print();
            Impresion.close();
        }
    });

}


function LimpiarDatosDetalleFactura() {
    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProducto", "0");
    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idServicio", "0");
    $("#txtCantidadFacturar").val("0");
    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("cantidadDetallePedido", "0");
    $("#spanTipoCambioPedido").val("1");

    $("#divFormaAgregarFactura, #divFormaEditarFacturaEncabezado").attr("idProyecto", "0");
    $("#txtProyecto").val("");
    $("#grdConceptoProyecto").trigger("reloadGrid");
}

function DeshabilitaCamposEncabezado() {

    $("#cmbCondicionPago").attr("disabled", "true");
    $("#txtFechaPago").attr("disabled", "true");
    $("#cmbMetodoPago").attr("disabled", "true");
    $("#cmbNumeroCuenta").attr("disabled", "true");
    $("#cmbDireccionCliente").attr("disabled", "true");
    $("#txtFechaFacturar").attr("disabled", "true");
    $("#cmbSerieFactura").attr("disabled", "true");
    $("#txtNumeroFactura").attr("disabled", "true");
    $("#chkEsRefactura").attr("disabled", "true");
    $("#cmbTipoMoneda").attr("disabled", "true");
    $("#chkParcialidades").attr("disabled", "true");
    $("#txtNoParcialidades").attr("disabled", "true");
    $("#txtNotaFactura").attr("disabled", "true");
}

function ValidaDetalleFactura(pFactura) {

    var errores = "";

    if (pFactura.IdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.FechaActual == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdCondicionPago == 0)
    { errores = errores + "<span>*</span> No hay condición de pago por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.FechaPago == "")
    { errores = errores + "<span>*</span> El campo fecha de pago esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdMetodoPago == "")
    { errores = errores + "<span>*</span> Favor de elegir un método de pago.<br />"; }

    if (pFactura.FechaFacturar == "")
    { errores = errores + "<span>*</span> El campo fecha de factura esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdSerieFactura == 0)
    { errores = errores + "<span>*</span> No hay serie por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdUsoCFDI == 0)
    { errores = errores + "<span>*</span> No hay Uso CFDI por asociar, favor de elegir alguno.<br />"; }

    //    if (pFactura.NumeroFactura == "" || pFactura.NumeroFactura == 0)
    //    { errores = errores + "<span>*</span> El campo número de factura esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.TipoCambioFactura == 0)
    { errores = errores + "<span>*</span> No hay tipo de cambio por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdUsuarioAgente == 0)
    { errores = errores + "<span>*</span> No hay agente por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdDivision == 0)
    { errores = errores + "<span>*</span> No hay división por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.NotaFactura == "")
    { errores = errores + "<span>*</span> El campo nota de factura esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.Parcialidades == 1) {
        if (pFactura.NoParcialidades == "" || pFactura.NoParcialidades == 0) {
            errores = errores + "<span>*</span> El campo número de parcialidades esta vacío, favor de capturarlo.<br />";
        }
        else {
            if (parseInt(pFactura.NoParcialidades) < 2) {
                errores = errores + "<span>*</span> Tienen que ser al menos 2 parcialidades.<br />";
            }
        }
    }


    if (pFactura.IdProyecto == 0) {
        if (pFactura.IdCotizacionDetalle == 0)
        { errores = errores + "<span>*</span> No hay partida de pedido por asociar, favor de elegir alguno.<br />"; }

        if (pFactura.CantidadFacturar == 0)
        { errores = errores + "<span>*</span> El campo cantidad esta vacio, favor de capturarlo.<br />"; }

        if (parseInt(pFactura.CantidadFacturar) > parseInt(pFactura.CantidadDetallePedido))
        { errores = errores + "<span>*</span> El campo cantidad no puede ser mayor a la cantidad de la partida seleccionada.<br />"; }
    }
    else {
        if (pFactura.NumeroPartidas == 0)
        { errores = errores + "<span>*</span> Para poder facturar por proyecto debe de tener al menos un concepto.<br />"; }
    }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaFacturaSustituye(pFactura) {

    var errores = "";

    if (pFactura.IdFacturaEncabezado == 0)
    { errores = errores + "<span>*</span> No hay factura por asociar, favor de elegir alguna.<br />"; }

    if (pFactura.IdSerieFactura == 0)
    { errores = errores + "<span>*</span> No hay serie por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.NumeroFactura == "" || pFactura.NumeroFactura == 0)
    { errores = errores + "<span>*</span> El campo número de factura esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaFacturaEncabezado(pFactura) {

    var errores = "";

    if (pFactura.IdCliente == 0)
    { errores = errores + "<span>*</span> No hay cliente por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.FechaActual == "")
    { errores = errores + "<span>*</span> El campo fecha esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdCondicionPago == 0)
    { errores = errores + "<span>*</span> No hay condición de pago por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.FechaPago == "")
    { errores = errores + "<span>*</span> El campo fecha de pago esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdMetodoPago == "")
    { errores = errores + "<span>*</span> Favor de elegir un método de pago.<br />"; }

    if (pFactura.FechaFacturar == "")
    { errores = errores + "<span>*</span> El campo fecha de factura esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdSerieFactura == 0)
    { errores = errores + "<span>*</span> No hay serie por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdUsoCFDI == 0)
    { errores = errores + "<span>*</span> No hay Uso CFDI por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdFacturaAnticipo != 0 && pFactura.IdTipoRelacion == 0)
    { errores = errores + "<span>*</span> Si relaciona una Factura debe indicar su Tipo de Relacion, favor de elegir alguno.<br />"; }

    if (pFactura.IdFacturaAnticipo == 0 && pFactura.IdTipoRelacion != 0)
    { errores = errores + "<span>*</span> Si tiene Tipo Relacion  debe indicar una Factura a Relacionar, favor de elegir alguno.<br />"; }

    //if (pFactura.NumeroFactura == "" || pFactura.NumeroFactura == 0)
    //{ errores = errores + "<span>*</span> El campo número de factura esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdTipoMoneda == 0)
    { errores = errores + "<span>*</span> No hay tipo de moneda por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.TipoCambioFactura == 0)
    { errores = errores + "<span>*</span> No hay tipo de cambio por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdUsuarioAgente == 0)
    { errores = errores + "<span>*</span> No hay agente por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.IdDivision == 0)
    { errores = errores + "<span>*</span> No hay división por asociar, favor de elegir alguno.<br />"; }

    if (pFactura.NotaFactura == "")
    { errores = errores + "<span>*</span> El campo nota de factura esta vacío, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaDatosFiscales(pFactura) {
    var erroresFiscales = "";

    if (pFactura.LugarExpedicion == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo lugar de expedición esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.RFC == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo RFC esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.IdMetodoPago == "" || pFactura.MetodoPago == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo método de pago esta vacío, favor de seleccioanrlo.<br />"; }

    if (pFactura.CalleFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.NumeroExteriorFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo numero exterior esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.ColoniaFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.PaisFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo país esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.EstadoFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo estado esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.CodigoPostalFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.MunicipioFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo municipio esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.LocalidadFiscal == "")
    { erroresFiscales = erroresFiscales + "<span>*</span> El campo localidad esta vacío, favor de capturarlo.<br />"; }

    if (pFactura.RFC != "") {
        if (RFCValidoEmpresarial(pFactura.RFC) == false)
        { erroresFiscales = erroresFiscales + "<span>*</span> El formato del RFC no es valido, favor de capturar un RFC valido.<br />"; }
    }

    if (pFactura.CodigoPostalFiscal != "") {
        if (ValidaCodigoPostalFiscal(pFactura.CodigoPostalFiscal))
        { erroresFiscales = erroresFiscales + "<span>*</span> El código postal debe de tener 5 números<br />"; }
    }

    if (erroresFiscales != "")
    { erroresFiscales = "<p>Favor de completar los siguientes requisitos:</p>" + erroresFiscales; }

    return erroresFiscales;
}

function ValidaCliente(pCliente) {
    var errores = "";

    if (pCliente.RazonSocial == "")
    { errores = errores + "<span>*</span> La razón social esta vacía, favor de capturarla.<br />"; }

    if (ValidarRazonSocial(pCliente.RazonSocial))
    { errores = errores + "<span>*</span> La razón tiene caracteres invalidos, favor de revisarlo.<br />"; }

    if (pCliente.NombreComercial == "")
    { errores = errores + "<span>*</span> El nombre comercial del cliente esta vacío, favor de capturarlo.<br />"; }

    if (pCliente.RFC == "")
    { errores = errores + "<span>*</span> El RFC esta vacío, favor de capturarlo.<br />"; }

    //////    if (pCliente.RFC != "") {
    //////        if (RFCValidoEmpresarial(pCliente.RFC) == false)
    //////        { errores = errores + "<span>*</span> El formato del RFC no es valido, favor de capturar un RFC valido.<br />"; }
    //////    }

    if (pCliente.Calle == "")
    { errores = errores + "<span>*</span> El campo calle esta vacío, favor de capturarlo.<br />"; }

    if (pCliente.NumeroExterior == "")
    { errores = errores + "<span>*</span> El campo numéro exterior esta vacío, favor de capturarlo.<br />"; }

    if (pCliente.Colonia == "")
    { errores = errores + "<span>*</span> El campo colonia esta vacío, favor de capturarlo.<br />"; }

    if (pCliente.CodigoPostal == "")
    { errores = errores + "<span>*</span> El campo código postal esta vacío, favor de capturarlo.<br />"; }

    if (pCliente.CodigoPostal != "") {
        if (ValidaCodigoPostalFiscal(pCliente.CodigoPostal))
        { errores = errores + "<span>*</span> El código postal debe de tener 5 números<br />"; }
    }
    if (pCliente.IdPais == 0)
    { errores = errores + "<span>*</span> El campo país esta vacío, favor de seleccionarlo.<br />"; }

    if (pCliente.IdEstado == 0)
    { errores = errores + "<span>*</span> El campo estado esta vacío, favor de seleccionarlo.<br />"; }

    if (pCliente.IdMunicipio == 0)
    { errores = errores + "<span>*</span> El campo municipio esta vacío, favor de seleccionarlo.<br />"; }

    if (pCliente.IdLocalidad == 0)
    { errores = errores + "<span>*</span> El campo localidad esta vacío, favor de seleccionarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function ValidaMotivoCancelacionFactura(pFacturaEncabezado) {
    var errores = "";

    if (pFacturaEncabezado.pIdFacturaEncabezado == 0)
    { errores = errores + "<span>*</span> No hay factura por asociar, favor de elegir alguno.<br />"; }

    if (pFacturaEncabezado.MotivoCancelacion == "")
    { errores = errores + "<span>*</span> El campo motivo de cancelación esta vacio, favor de capturarlo.<br />"; }

    if (errores != "")
    { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

    return errores;
}

function FiltroFacturas_Detalle() {
    var Factura = new Object();
    Factura.pTamanoPaginacion = 10;
    Factura.pPaginaActual = 1;
    Factura.pColumnaOrden = "IdFacturaEncabezado";
    Factura.pTipoOrden = "DESC";
    Factura.pSerieFactura = "";
    Factura.pNumeroFactura = "";
    Factura.pFechaInicial = "";
    Factura.pFechaFinal = "";
    Factura.pPorFecha = 0;
    Factura.pAI = -1;
    Factura.pFiltroTimbrado = 0;
    Factura.pIdDivision = -1;
    Factura.pRazonSocial = "";
    Factura.pAgente = "";
    Factura.pNumeroPedido = "";
    Factura.pBusquedaDocumento = 0;
    Factura.pEstatusFacturaEncabezado = "";
    Factura.pFolio = 0;
    Factura.pIdProyecto = 0;
    Factura.pDescripcion = "";
    Factura.pClave = "";

    Factura.pTamanoPaginacion = parseInt($('#grdFacturas_Detalle').getGridParam('rowNum'));
    Factura.pPaginaActual = parseInt($('#grdFacturas_Detalle').getGridParam('page'));
    Factura.pColumnaOrden = $('#grdFacturas_Detalle').getGridParam('sortname');
    Factura.pTipoOrden = $('#grdFacturas_Detalle').getGridParam('sortorder');
    Factura.pSerieFactura = ($("#gs_SerieFactura").val() != null) ? $("#gs_SerieFactura").val() : "";
    Factura.pNumeroFactura = ($("#gs_NumeroFactura").val() != null) ? $("#gs_NumeroFactura").val() : "";
    Factura.pFechaInicial = ($("#txtFechaInicial").val() != null) ? $("#txtFechaInicial").val() : "";
    Factura.pFechaFinal = ($("#txtFechaFinal").val() != null) ? $("#txtFechaFinal").val() : "";
    Factura.pPorFecha = ($("#chkPorFecha").val() != null && $("#chkPorFecha").is(':checked')) ? 1 : 0;
    Factura.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : -1;
    Factura.pFiltroTimbrado = ($("#cmbFiltroTimbrado").val() != null) ? parseInt($("#cmbFiltroTimbrado").val()) : 0;
    Factura.pIdDivision = ($("#gs_IdDivision").val() != null) ? parseInt($("#gs_IdDivision").val()) : -1;
    Factura.pRazonSocial = ($("#gs_RazonSocial").val() != null) ? $("#gs_RazonSocial").val() : "";
    Factura.pAgente = ($("#gs_Agente").val() != null) ? $("#gs_Agente").val() : "";
    Factura.pNumeroPedido = ($("#txtNumeroPedidoBuscador").val() != null) ? $("#txtNumeroPedidoBuscador").val() : "";
    Factura.pBusquedaDocumento = ($("#cmbBusquedaDocumento").val() != null) ? parseInt($("#cmbBusquedaDocumento").val()) : 0;
    Factura.pDescripcion = ($("#gs_Descripcion").val() != null) ? $("#gs_Descripcion").val() : "";
    Factura.pClave = ($("#gs_Clave").val() != null) ? $("#gs_Clave").val() : "";

    var Request = JSON.stringify(Factura);

    $.ajax({
        url: "FacturaCliente.aspx/ObtenerFacturas_Detalle",
        type: "post",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        complete: function (jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturas_Detalle')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { MostrarMensajeError(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ExportarFacturas_Detalle() {
    $("#grdFacturas_Detalle").jqGrid('navButtonAdd', '#pagFacturas_Detalle', {
        caption: "Exportar",
        title: "Exportar",
        buttonicon: 'ui-icon-newwin',
        onClickButton: function () {
            var Factura = new Object();
            Factura.pTamanoPaginacion = 10;
            Factura.pPaginaActual = 1;
            Factura.pColumnaOrden = "IdFacturaEncabezado";
            Factura.pTipoOrden = "DESC";
            Factura.pSerieFactura = "";
            Factura.pNumeroFactura = "";
            Factura.pFechaInicial = "";
            Factura.pFechaFinal = "";
            Factura.pPorFecha = 0;
            Factura.pAI = -1;
            Factura.pFiltroTimbrado = 0;
            Factura.pIdDivision = -1;
            Factura.pRazonSocial = "";
            Factura.pAgente = "";
            Factura.pNumeroPedido = "";
            Factura.pBusquedaDocumento = 0;
            Factura.pEstatusFacturaEncabezado = "";
            Factura.pFolio = 0;
            Factura.pIdProyecto = 0;
            Factura.pDescripcion = "";
            Factura.pClave = "";

            Factura.pTamanoPaginacion = parseInt($('#grdFacturas_Detalle').getGridParam('rowNum'));
            Factura.pPaginaActual = parseInt($('#grdFacturas_Detalle').getGridParam('page'));
            Factura.pColumnaOrden = $('#grdFacturas_Detalle').getGridParam('sortname');
            Factura.pTipoOrden = $('#grdFacturas_Detalle').getGridParam('sortorder');
            Factura.pSerieFactura = ($("#gs_SerieFactura").val() != null) ? $("#gs_SerieFactura").val() : "";
            Factura.pNumeroFactura = ($("#gs_NumeroFactura").val() != null) ? $("#gs_NumeroFactura").val() : "";
            Factura.pFechaInicial = ($("#txtFechaInicial").val() != null) ? $("#txtFechaInicial").val() : "";
            Factura.pFechaFinal = ($("#txtFechaFinal").val() != null) ? $("#txtFechaFinal").val() : "";
            Factura.pPorFecha = ($("#chkPorFecha").val() != null && $("#chkPorFecha").is(':checked')) ? 1 : 0;
            Factura.pAI = ($("#gs_AI").val() != null) ? parseInt($("#gs_AI").val()) : -1;
            Factura.pFiltroTimbrado = ($("#cmbFiltroTimbrado").val() != null) ? parseInt($("#cmbFiltroTimbrado").val()) : 0;
            Factura.pIdDivision = ($("#gs_IdDivision").val() != null) ? parseInt($("#gs_IdDivision").val()) : -1;
            Factura.pRazonSocial = ($("#gs_RazonSocial").val() != null) ? $("#gs_RazonSocial").val() : "";
            Factura.pAgente = ($("#gs_Agente").val() != null) ? $("#gs_Agente").val() : "";
            Factura.pNumeroPedido = ($("#txtNumeroPedidoBuscador").val() != null) ? $("#txtNumeroPedidoBuscador").val() : "";
            Factura.pBusquedaDocumento = ($("#cmbBusquedaDocumento").val() != null) ? parseInt($("#cmbBusquedaDocumento").val()) : 0;
            Factura.pDescripcion = ($("#gs_Descripcion").val() != null) ? $("#gs_Descripcion").val() : "";
            Factura.pClave = ($("#gs_Clave").val() != null) ? $("#gs_Clave").val() : "";

            $.UnifiedExportFile({ action: '../ExportacionesExcel/ExportarExcelFacturaDetalle.aspx', data: Factura, downloadType: 'Normal' });

        }
    });
}


////////////////////////  Nueva forma de guardar Facturas /////////////////////////////////////

/* Timbrar */
function ObtenerFacturaATimbrar(Request) {
    MostrarBloqueo();
    $.ajax({
        url: "FacturaCliente.aspx/ObtenerDatosFactura",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                TimbrarFact(json);
            }
            else {
                MostrarMensajeError(json.Descripcion);
                OcultarBloqueo();
            }
        }
    });
}

function TimbrarFact(json) {
    var Comprobante = new Object();
    Comprobante.Comprobante = json.Comprobante;
    Comprobante.Id = json.Id;
    Comprobante.Token = json.Token;
    Comprobante.RFC = json.RFC;
    Comprobante.RefID = json.RefID;
    Comprobante.Formato = json.Formato;
    Comprobante.NoCertificado = json.NoCertificado;
    Comprobante.Correos = json.Correos;
    Comprobante.RutaCFDI = json.RutaCFDI;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "http://"+ window.location.hostname +"/WebServiceDiverza/Facturacion.aspx/TimbrarFactura",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                GuardarFacturaTimbrada(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function GuardarFacturaTimbrada(json) {
    var Comprobante = new Object();
    Comprobante.UUId = json.uuid;
    Comprobante.RefId = json.ref_id;
    var Request = JSON.stringify(Comprobante);
    $.ajax({
        url: "FacturaCliente.aspx/GuardarFactura",
        type: "POST",
        data: Request,
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            MostrarMensajeError(json.Descripcion);
            OcultarBloqueo();
        }
    });
}

/* Cancelar */
function ObtenerFacturaACancelar(Request) {
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
    	url: "http://" + window.location.hostname + "/WebServiceDiverza/Facturacion.aspx/CancelarFactura",
        type: "POST",
        data: Request,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (Respuesta) {
            var json = JSON.parse(Respuesta.d);
            console.log(json);
            if (json.Error == 0) {
                EditarFacturaACancelar(json);
            }
            else {
                MostrarMensajeError(json.message);
                OcultarBloqueo();
            }
        }
    });
}

function EditarFacturaACancelar(json) {
    var Comprobante = new Object();
    Comprobante.RefId = json.ref_id;
    Comprobante.Date = json.date;
    Comprobante.message = json.message;
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
            MostrarMensajeError(json.Descripcion);
            OcultarBloqueo();
        }
    });
}

