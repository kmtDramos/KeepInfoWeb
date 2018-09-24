/**/

$(function () {

    MantenerSesion();

    setInterval(MantenerSesion, 1000 * 60 * 1.5);

    $("#divReportes").tabs();
    ObtenerFormaFiltrosSalidaMaterial();

    $("#grdEntregaMaterial").on("click", ".imgFormaConsultarSolicitudMaterial", function () {
        console.log("Click");
        var registro = $(this).parents("tr");
        var SolicitudMaterial = new Object();
        SolicitudMaterial.pIdSolicitudMaterial = parseInt($(registro).children("td[aria-describedby='grdEntregaMaterial_IdSolicitudMaterial']").html());
        ObtenerFormaConsultarSolicitudMaterial(JSON.stringify(SolicitudMaterial));
    });

    $('#dialogConsultarSolicitudEntregaMaterial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConsultarSolicitudEntregaMaterial").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });
    
    $('#dialogEditarSolicitudEntregaMaterial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaEditarSolicitudEntregaMaterial").remove();
        },
        buttons: {
            "Salir": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialogConsultarSolicitudEntregaMaterial").on("click", "#divImprimirSolMaterial", function () {
        var IdSolicitudMaterial = $("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
        Imprimir(IdSolicitudMaterial);
    });

    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarSalidaMaterial", function () {
        ObtenerFormaAgregarSalidaMaterial();
    });

    $("#grdSalidaMaterial").on("click", ".imgFormaConsultarSalidaMaterial", function () {
        var registro = $(this).parents("tr");
        var SalidaMaterial = new Object();
        SalidaMaterial.pIdSalidaMaterial = parseInt($(registro).children("td[aria-describedby='grdSalidaMaterial_IdSalidaMaterial']").html());
        ObtenerFormaConsultarSalidaMaterial(JSON.stringify(SalidaMaterial));
    });

    $('#dialogAgregarSalidaMaterial').dialog({
        autoOpen: false,
        height: 'auto',
        width: '900px',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        open: function () {

        },
        close: function () {
            $("#divFormaAgregarSalidaMaterial").remove();
        },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");//AgregarReingresoMaterial();
            }
        }
    });

    $('#dialogConsultarSalidaMaterial').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function () {
            $("#divFormaConsultarSalidaMaterial").remove();
        },
        buttons: {
            "Aceptar": function () {
                $(this).dialog("close");
            }
        }
    });
});

function ObtenerFormaFiltrosSalidaMaterial() {
    $("#divFiltrosSalidaMaterial").obtenerVista({
        nombreTemplate: "tmplFiltrosSalidaMaterial.html",
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaFiltroSalidaMaterial",
        despuesDeCompilar: function (pRespuesta) {

            if ($("#txtFechaInicialS").length) {
                $("#txtFechaInicialS").datepicker({
                    onSelect: function () {
                        FiltroSalidaMaterial();
                    }
                });
            }

            if ($("#txtFechaFinalS").length) {
                $("#txtFechaFinalS").datepicker({
                    onSelect: function () {
                        FiltroSalidaMaterial();
                    }
                });
            }

            $('#divFiltrosReingresoMaterial').on('click', '#chkPorFechaS', function (event) {
                FiltroSalidaMaterial();
            });

            $('#divFiltrosReingresoMaterial').on('focusout', '#txtFolioS', function (event) {
                FiltroSalidaMaterial();
            });
        }
    });
}

function ObtenerFormaAgregarSalidaMaterial(pRequest) {
    MostrarBloqueo();
    $("#dialogAgregarSalidaMaterial").obtenerVista({
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaAgregarSalidaMaterial",
        parametros: pRequest,
        nombreTemplate: "tmplAgregarSalidaMaterial.html",
        despuesDeCompilar: function (pRespuesta) {

            AutocompletarProductoClave();
            //AutocompletarProductoDescripcion();
            //AutocompletarServicioClave();
            //AutocompletarServicioDescripcion();
            AutocompletarCliente();
            AutocompletarProyecto();

            Inicializar_grdDetalleSalidaMaterial();

            $("input[name=ProductoServicio]:radio").click(function (evento) {
                if (this.value == 1) {
                    MuestraObjetos(1);
                } else {
                    MuestraObjetos(0);
                }
                LimpiarDatosDetalleFactura();
            });
            $('#divFormaAgregarSalidaMaterial').on('focusout', '#txtCantidad', function (event) {
                //RecalculaDatosCantidad();
            });

            $("#divFormaAgregarSalidaMaterial").on("click", "#btnAgregarPartidaSalidaMaterial", function () {
                var pEncabezadoSalidaMaterial = new Object();
                if ($("#divFormaAgregarSalidaMaterial").attr("idProducto") == "" || $("#divFormaAgregarSalidaMaterial").attr("idProducto") == null) {
                    pEncabezadoSalidaMaterial.IdProducto = 0;
                }
                else {
                    pEncabezadoSalidaMaterial.IdProducto = $("#divFormaAgregarSalidaMaterial, #divFormaEditarSalidaMaterial").attr("idProducto");
                }

                if (pEncabezadoSalidaMaterial.IdProducto != 0) {
                    AgregarDetalleSalidaMaterial();
                }

            });

            $("input[name=ClienteProyecto]:radio").click(function (evento) {
                if (this.value == 1) {
                    MuestraObjetosClienteProyecto(1);
                } else {
                    MuestraObjetosClienteProyecto(0);
                }
            });

            $("#grdDetalleSalidaMaterial").on("click", ".imgEliminarConceptoSalidaMaterial", function () {

                var registro = $(this).parents("tr");
                var pDetalleSalidaMaterial = new Object();
                pDetalleSalidaMaterial.pIdDetalleSalidaMaterial = parseInt($(registro).children("td[aria-describedby='grdDetalleSalidaMaterial_IdDetalleSalidaMaterial']").html());
                var oRequest = new Object();
                oRequest.pDetalleSalidaMaterial = pDetalleSalidaMaterial;
                SetEliminarDetalleSalidaMaterial(JSON.stringify(oRequest));
            });

            $("#dialogAgregarSalidaMaterial").dialog("open");
        }
    });
}

function ObtenerFormaConsultarSalidaMaterial(pIdSalidaMaterial) {
    console.log(pIdSalidaMaterial);
    $("#dialogConsultarSalidaMaterial").obtenerVista({
        nombreTemplate: "tmplConsultarSalidaMaterial.html",
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaSalidaMaterial",
        parametros: pIdSalidaMaterial,
        despuesDeCompilar: function (pRespuesta) {
            Inicializar_grdDetalleSalidaMaterialConsultar();

            $("#dialogConsultarSalidaMaterial").dialog("open");
        }
    });
}

function LimpiarDatosDetalleSalida() {
    $("#divFormaAgregarSalidaMaterial, #divFormaEditarSalidaMaterial").attr("idProducto", "0");
    $("#divFormaAgregarSalidaMaterial, #divFormaEditarSalidaMaterial").attr("idServicio", "0");
    $("#txtClaveProducto").val("");
    $("#txtClaveServicio").val("");
    $("#txtDescripcionProducto").val("");
    $("#txtDescripcionServicio").val("");
    $("#txtCantidad").val("1");
    $('input[name=ClienteProyecto]').attr("disabled", true);
}

function MuestraObjetosClienteProyecto(opcion) {
    if (opcion == 1) {
        $("#txtCliente").css("display", "block");
        $("#txtProyecto").css("display", "none");
        $("#txtProyecto").val("");
        $("#divFormaAgregarSalidaMaterial").attr("idCliente", "0");
        $("#divFormaAgregarSalidaMaterial").attr("idProyecto", "0");
    }
    else {
        $("#txtCliente").css("display", "none");
        $("#txtProyecto").css("display", "block");
        $("#txtCliente").val("");
        $("#divFormaAgregarSalidaMaterial").attr("idCliente", "0");
        $("#divFormaAgregarSalidaMaterial").attr("idProyecto", "0");

        AutocompletarProductoClave();
        AutocompletarProductoDescripcion();
    }
}

function AgregarDetalleSalidaMaterial() {

    var pEncabezadoSalida = new Object();
    if ($("#divFormaAgregarSalidaMaterial").attr("idSalidaMaterial") == "" || $("#divFormaAgregarSalidaMaterial").attr("idSalidaMaterial") == null) {
        pEncabezadoSalida.IdSalidaMaterial = 0;
    }
    else {
        pEncabezadoSalida.IdSalidaMaterial = $("#divFormaAgregarSalidaMaterial").attr("idSalidaMaterial");
    }

    if ($("#divFormaAgregarSalidaMaterial").attr("idProducto") == "" || $("#divFormaAgregarSalidaMaterial").attr("idProducto") == null) {
        pEncabezadoSalida.IdProducto = 0;
    }
    else {
        pEncabezadoSalida.IdProducto = $("#divFormaAgregarSalidaMaterial").attr("idProducto");
    }

    if ($("#divFormaAgregarSalidaMaterial").attr("idServicio") == "" || $("#divFormaAgregarSalidaMaterial").attr("idServicio") == null) {
        pEncabezadoSalida.IdServicio = 0;
    }
    else {
        pEncabezadoSalida.IdServicio = $("#divFormaAgregarSalidaMaterial").attr("idServicio");
    }

    pEncabezadoSalida.Cantidad = $("#txtCantidad").val();

    if ($("#divFormaAgregarSalidaMaterial").attr("idCliente") == "" || $("#divFormaAgregarSalidaMaterial").attr("idCliente") == null) {
        pEncabezadoSalida.IdCliente = 0;
    }
    else {
        pEncabezadoSalida.IdCliente = $("#divFormaAgregarSalidaMaterial").attr("idCliente");
    }

    if ($("#divFormaAgregarSalidaMaterial").attr("idProyecto") == "" || $("#divFormaAgregarSalidaMaterial").attr("idProyecto") == null) {
        pEncabezadoSalida.IdProyecto = 0;
    }
    else {
        pEncabezadoSalida.IdProyecto = $("#divFormaAgregarSalidaMaterial").attr("idProyecto");
    }

    pEncabezadoSalida.IdMotivoSalida = $("#cmbMotivo").val();

    //var validacion = ValidaDetalleFacturaProveedor(pEncabezadoFacturaProveedor);
    //if (validacion != "")
    //{ MostrarMensajeError(validacion); return false; }

    var oRequest = new Object();
    oRequest.pSalidaMaterial = pEncabezadoSalida;
    SetAgregarDetalleSalidaMaterial(JSON.stringify(oRequest));
}

function SetAgregarDetalleSalidaMaterial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SalidaEntregaMaterial.aspx/AgregarDetalleSalidaMaterialNormal",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {

            console.log(pRespuesta);
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarSalidaMaterial").attr("idSalidaMaterial", respuesta.IdSalidaMaterial);

                $("#grdSalidaMaterial").trigger("reloadGrid");

                $("#grdDetalleSalidaMaterial").trigger("reloadGrid");
                $("#grdDetalleSalidaMaterialConsultar").trigger("reloadGrid");
                $("#grdDetalleSalidaMaterialEditar").trigger("reloadGrid");
                console.log("se actualiza las grillas");
                LimpiarDatosDetalleSalida();
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

function SetEliminarDetalleSalidaMaterial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SalidaEntregaMaterial.aspx/EliminarDetalleSalidaMaterial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            console.log(pRespuesta);
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#divFormaAgregarSalidaMaterial").attr("idSalidaMaterial", respuesta.IdSalidaMaterial);

                $("#grdDetalleSalidaMaterialEditar").trigger("reloadGrid");
                $("#grdDetalleSalidaMaterialConsultar").trigger("reloadGrid");
                $("#grdDetalleSalidaMaterial").trigger("reloadGrid");
                $("#grdSalidaMaterial").trigger("reloadGrid");
                console.log("se actualiza las grillas");
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

function FiltroSalidaMaterial() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdSalidaMaterial').getGridParam('rowNum');
    request.pPaginaActual = $('#grdSalidaMaterial').getGridParam('page');
    request.pColumnaOrden = $('#grdSalidaMaterial').getGridParam('sortname');
    request.pTipoOrden = $('#grdSalidaMaterial').getGridParam('sortorder');
    request.pFolio = "";
    request.pRazonSocial = "";
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pPorFecha = 0;

    if ($('#gs_RazonSocialS').val() != null) { request.pRazonSocial = $("#gs_RazonSocialS").val(); }
    if ($("#txtFolioS").val() != "" && $("#txtFolioS").val() != null) { request.pFolio = $("#txtFolioS").val(); }

    if ($("#txtFechaInicialS").val() != "" && $("#txtFechaInicialS").val() != null) {

        if ($("#chkPorFechaS").is(':checked')) {
            request.pPorFecha = 1;
        }
        else {
            request.pPorFecha = 0;
        }

        request.pFechaInicial = $("#txtFechaInicialS").val();
        request.pFechaInicial = ConvertirFecha(request.pFechaInicial, 'aaaammdd');
    }
    if ($("#txtFechaFinalS").val() != "" && $("#txtFechaFinalS").val() != null) {
        request.pFechaFinal = $("#txtFechaFinalS").val();
        request.pFechaFinal = ConvertirFecha(request.pFechaFinal, 'aaaammdd');
    }


    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerSalidaMaterial',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdSalidaMaterial')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleSalidaMaterial() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleSalidaMaterial').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleSalidaMaterial').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleSalidaMaterial').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleSalidaMaterial').getGridParam('sortorder');
    request.pIdSalidaMaterial = 0;
    if ($("#divFormaAgregarSalidaMaterial, #divFormaConsultarSalidaMaterial").attr("IdSalidaMaterial") != null && $("#divFormaAgregarSalidaMaterial, #divFormaConsultarSalidaMaterial").attr("IdSalidaMaterial") != "") {
        request.pIdSalidaMaterial = $("#divFormaAgregarSalidaMaterial, #divFormaConsultarSalidaMaterial").attr("IdSalidaMaterial");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerDetalleSalidaMaterial',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdDetalleSalidaMaterial')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroDetalleSalidaMaterialConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleSalidaMaterialConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleSalidaMaterialConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleSalidaMaterialConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleSalidaMaterialConsultar').getGridParam('sortorder');
    request.pIdSalidaMaterial = 0;
    if ($("#divFormaAgregarSalidaMaterial, #divFormaConsultarSalidaMaterial").attr("IdSalidaMaterial") != null && $("#divFormaAgregarSalidaMaterial, #divFormaConsultarSalidaMaterial").attr("IdReingresoMaterial") != "") {
        request.pIdSalidaMaterial = $("#divFormaAgregarSalidaMaterial, #divFormaConsultarSalidaMaterial").attr("IdSalidaMaterial");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerDetalleSalidaMaterialConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdDetalleSalidaMaterialConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function AutocompletarProductoClave() {

    $('#txtClaveProducto').autocomplete({
        source: function (request, response) {
            var pProducto = new Object();
            pProducto.Producto = $("#txtClaveProducto").val();
            pProducto.TipoBusqueda = "C";

            var oRequest = new Object();
            oRequest.pProducto = pProducto;
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarProducto',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarSalidaMaterial").attr("idProducto", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idServicio", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idCotizacionDetalle", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idOrdenCompraDetalle", "0");
                    $("#txtDescripcionProducto").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        if (pProducto.TipoBusqueda == 'C') {
                            return { label: item.Producto, value: item.Producto, id: item.IdProducto }
                        }
                        else {
                            return { label: item.Clave, value: item.Clave, id: item.IdProducto }
                        }
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            var pIdProducto = ui.item.id;
            $("#divFormaAgregarSalidaMaterial").attr("idProducto", pIdProducto);
            $("#divFormaAgregarSalidaMaterial").attr("idServicio", "0");
            $("#divFormaAgregarSalidaMaterial").attr("idCotizacionDetalle", "0");
            $("#divFormaAgregarSalidaMaterial").attr("idOrdenCompraDetalle", "0");

            var Producto = new Object();
            Producto.IdProducto = pIdProducto;
            obtenerProducto(Producto);

        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function AutocompletarProductoDescripcion() {
    $('#txtDescripcionProducto').autocomplete({
        source: function (request, response) {
            var pProducto = new Object();
            pProducto.Producto = $("#txtDescripcionProducto").val();
            pProducto.TipoBusqueda = "P";

            var oRequest = new Object();
            oRequest.pProducto = pProducto;
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarProducto',
                data: JSON.stringify(oRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarSalidaMaterial").attr("idProducto", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idServicio", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idCotizacionDetalle", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idOrdenCompraDetalle", "0");
                    $("#txtClaveProducto").val("");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        if (pProducto.TipoBusqueda == 'P') {
                            return { label: item.Producto, value: item.Producto, id: item.IdProducto }
                        }
                        else {
                            return { label: item.Clave, value: item.Clave, id: item.IdProducto }
                        }
                    }));
                }
            });
        },
        minLength: 2,
        select: function (event, ui) {
            var pIdProducto = ui.item.id;
            $("#divFormaAgregarSalidaMaterial").attr("idProducto", pIdProducto);
            $("#divFormaAgregarSalidaMaterial").attr("idServicio", "0");
            $("#divFormaAgregarSalidaMaterial").attr("idCotizacionDetalle", "0");
            $("#divFormaAgregarSalidaMaterial").attr("idOrdenCompraDetalle", "0");
            var Producto = new Object();
            Producto.IdProducto = pIdProducto;
            obtenerProducto(Producto);
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function obtenerProducto(pRequest) {
    $.ajax({
        type: "POST",
        url: "EncabezadoFacturaProveedor.aspx/obtenerProducto",
        data: JSON.stringify(pRequest),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                Modelo = respuesta.Modelo;
                $("#txtDescripcionProducto").val(Modelo.Producto);
                $("#txtClaveProducto").val(Modelo.Producto);
                //$("#txtCosto").val(formato.moneda(Modelo.Costo,'$'));
                //$("#txtDescuento").val(0);
                //$("#txtCostoDescuento").val(formato.moneda(Modelo.Costo,'$'));
                //$("#txtTotalSinTipoCambio").val(formato.moneda(Modelo.Costo,'$'));
                $("#cmbUnidadCompraVenta option[value=" + Modelo.IdUnidadCompraVenta + "]").attr("selected", true);
                $("#cmbUnidadCompraVenta").attr("disabled", "true");
                //$("#cmbTipoMonedaConcepto option[value=" + Modelo.IdTipoMoneda + "]").attr("selected", true);
                $("#divFormaAgregarEncabezadoFacturaProveedor, #divFormaEditarEncabezadoFacturaProveedor").attr("idTipoIVA", Modelo.IdTipoIVA);
                $("#txtIVA").val(Modelo.IVA);
                var pTipoCambio = new Object();
                pTipoCambio.IdTipoCambioOrigen = parseInt($("#cmbTipoMonedaConcepto").val());;
                pTipoCambio.IdTipoCambioDestino = parseInt($("#cmbTipoMoneda").val());
                var oRequest = new Object();
                oRequest.pTipoCambio = pTipoCambio;
                //ObtenerTipoCambioDetalle(JSON.stringify(oRequest))

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

function AutocompletarCliente() {

    $('#txtCliente').autocomplete({
        source: function (request, response) {
            var pRequest = new Object();
            pRequest.pRazonSocial = $('#txtCliente').val();
            $.ajax({
                type: 'POST',
                url: 'EncabezadoFacturaProveedor.aspx/BuscarRazonSocialCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarSalidaMaterial").attr("idCliente", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idProyecto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.RazonSocial, value: item.RazonSocial, id: item.IdCliente }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var pIdCliente = ui.item.id;
            $("#divFormaAgregarSalidaMaterial").attr("idCliente", pIdCliente);
            $("#divFormaAgregarSalidaMaterial").attr("idProyecto", "0");
            var Cliente = new Object();
            Cliente.IdCliente = pIdCliente;
            //obtenerPedidosCliente(JSON.stringify(Cliente));

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
                url: 'EncabezadoFacturaProveedor.aspx/BuscarProyectoCliente',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (pRespuesta) {
                    $("#divFormaAgregarSalidaMaterial").attr("idCliente", "0");
                    $("#divFormaAgregarSalidaMaterial").attr("idProyecto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function (item) {
                        return { label: item.NombreProyecto, value: item.NombreProyecto, id: item.IdProyecto }
                    }));
                }
            });
        },
        minLength: 1,
        select: function (event, ui) {
            var pIdProyecto = ui.item.id;
            $("#divFormaAgregarSalidaMaterial").attr("idCliente", "0");
            $("#divFormaAgregarSalidaMaterial").attr("idProyecto", pIdProyecto);
        },
        change: function (event, ui) { },
        open: function () { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function () { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}
///****///////
function ObtenerFormaConsultarSolicitudMaterial(pIdSolicitudMaterial) {
    $("#dialogConsultarSolicitudEntregaMaterial").obtenerVista({
        nombreTemplate: "tmplConsultarSolicitudEntregaMaterial.html",
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaSolicitudEntregaMaterial",
        parametros: pIdSolicitudMaterial,
        despuesDeCompilar: function (pRespuesta) {
            console.log("Forma Consulta Solicitud")
            console.log(pRespuesta.modelo);
            Inicializar_grdPartidasSolicitudMaterialConsultar();
            if (pRespuesta.modelo.Permisos.puedeEditarSalidaEntregaMaterial == 1 && pRespuesta.modelo.Confirmado == 0) {
                
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "buttons", {
                    "Editar": function () {
                        $(this).dialog("close");
                        var SolicitudMaterial = new Object();
                        SolicitudMaterial.IdSolicitudMaterial = parseInt($("#divFormaConsultarSolicitudEntregaMaterial").attr("IdSolicitudMaterial"));
                        ObtenerFormaEditarSolicitudMaterial(JSON.stringify(SolicitudMaterial))
                    },
                    "Salir": function () {
                        $(this).dialog("close");
                    }
                });
                
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "height", "auto");
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("open");
            }
            else {
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "buttons", {});
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("option", "height", "auto");
                $("#dialogConsultarSolicitudEntregaMaterial").dialog("open");
            }
            
        }
    });
}

function ObtenerFormaEditarSolicitudMaterial(pIdSolicitudMaterial) {
    $("#dialogEditarSolicitudEntregaMaterial").obtenerVista({
        nombreTemplate: "tmplEditarSolicitudMaterial.html",
        url: "SalidaEntregaMaterial.aspx/ObtenerFormaEditarSolicitudEntregaMaterial",
        parametros: pIdSolicitudMaterial,
        despuesDeCompilar: function (pRespuesta) {
            console.log("Forma Edita Solicitud")
            console.log(pRespuesta.modelo);
            Inicializar_grdPartidasSolicitudMaterialEditar();
            if (pRespuesta.modelo.Permisos.puedeEditarSalidaEntregaMaterial == 1) {

                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "buttons", {
                    "Guardar": function () {
                        $(this).dialog("close");
                        var SolicitudMaterial = new Object();
                        SolicitudMaterial.IdSolicitudMaterial = parseInt($("#divFormaEditarSolicitudEntregaMaterial").attr("IdSolicitudMaterial"));
                        EditarSolicitudMaterial(SolicitudMaterial);
                    },
                    "Salir": function () {
                        $(this).dialog("close");
                    }
                });

                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "height", "auto");
            }
            else {
                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "buttons", {});
                $("#dialogEditarSolicitudEntregaMaterial").dialog("option", "height", "100");
            }
            $("#dialogEditarSolicitudEntregaMaterial").dialog("open");
        }
    });
}

function FiltroEntregaMaterial() {
    var SolicitudMaterial = new Object();
    SolicitudMaterial.pTamanoPaginacion = $('#grdEntregaMaterial').getGridParam('rowNum');
    SolicitudMaterial.pPaginaActual = $('#grdEntregaMaterial').getGridParam('page');
    SolicitudMaterial.pColumnaOrden = $('#grdEntregaMaterial').getGridParam('sortname');
    SolicitudMaterial.pTipoOrden = $('#grdEntregaMaterial').getGridParam('sortorder');
    SolicitudMaterial.pIdSolicitudMaterial = ($("#gs_IdSolicitudMaterial").val() == null) ? "" : $("#gs_IdSolicitudMaterial").val();

    var Request = JSON.stringify(SolicitudMaterial);
    $.ajax({
        url: "SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterial",
        data: Request,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdEntregaMaterial')[0].addJSONData(JSON.parse(jsondata.responseText).d); OcultarBloqueo(); }
            else { console.log(JSON.parse(jsondata.responseText).Message); }
            TerminoInventario();
        }
    });
}

function FiltroPartidasSolicitudMaterialConsultar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('page');
    request.pColumnaOrden = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('sortname');
    request.pTipoOrden = $('#grdPartidasSolicitudMaterialConsultar').getGridParam('sortorder');
    console.log(parseInt($("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial")));
    request.pIdSolicitudMaterial = 0;
    if ($("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != null && $("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != "") {
        request.pIdSolicitudMaterial = $("#divFormaConsultarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterialConceptosConsultar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdPartidasSolicitudMaterialConsultar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroPartidasSolicitudMaterialEditar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPartidasSolicitudMaterialEditar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPartidasSolicitudMaterialEditar').getGridParam('page');
    request.pColumnaOrden = $('#grdPartidasSolicitudMaterialEditar').getGridParam('sortname');
    request.pTipoOrden = $('#grdPartidasSolicitudMaterialEditar').getGridParam('sortorder');
    console.log(parseInt($("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial")));
    request.pIdSolicitudMaterial = 0;
    if ($("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != null && $("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial") != "") {
        request.pIdSolicitudMaterial = $("#divFormaEditarSolicitudEntregaMaterial").attr("idsolicitudmaterial");
    }
    var pRequest = JSON.stringify(request);
    console.log(pRequest);
    $.ajax({
        url: 'SalidaEntregaMaterial.aspx/ObtenerSolicitudEntregaMaterialConceptosEditar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function (jsondata, stat) {
            if (stat == 'success') { $('#grdPartidasSolicitudMaterialEditar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function TerminoInventario() {
    $("td[aria-describedby=grdEntregaMaterial_Existencia]", "#grdEntregaMaterial tbody").each(function (index, element) {
        var IdExperienciaReal = $(element).parent("tr").children("td[aria-describedby=grdEntregaMaterial_IdExperienciaReal]").text();
        var input = $('<input type="text" value="' + $(element).text() + '" style="width:96%;text-align:right;"/>');
        //$(element).html(input);
        //$(input).change(function () { ActualizarExistenciaProducto(IdExperienciaReal, $(this).val()); });
    });
}

function Imprimir(pIdSolicitudMaterial) {
    MostrarBloqueo();

    var SolicitudMaterial = new Object();
    SolicitudMaterial.IdSolicitudMaterial = pIdSolicitudMaterial;

    var Request = JSON.stringify(SolicitudMaterial);

    var contenedor = $("<div></div>");

    $(contenedor).obtenerVista({
        url: "SalidaEntregaMaterial.aspx/Imprimir",
        parametros: Request,
        nombreTemplate: "tmplImprimirSalidaMaterial.html",
        despuesDeCompilar: function (Respuesta) {
            var plantilla = $(contenedor).html();
            var Impresion = window.open("", "");
            Impresion.document.write(plantilla);
            Impresion.print();
            Impresion.close();
        }
    });

}

function EditarSolicitudMaterial(IdSolicitudMaterial) {
    var pSolicitudMaterial = new Object();

    pSolicitudMaterial.IdSolicitudMaterial = IdSolicitudMaterial.IdSolicitudMaterial;

    if ($("#chkConfirmado").is(':checked')) {
        pSolicitudMaterial.Aprobar = 1;
    }
    else {
        pSolicitudMaterial.Aprobar = 0;
    }

    pSolicitudMaterial.Comentarios = $("textarea#txtComentarios").val();
    
    var validacion = ValidaInventario();
    if (validacion != "") { MostrarMensajeError(validacion); return false; }
    
    SetEditarSolicitudMaterial(JSON.stringify(pSolicitudMaterial));
    
}

function SetEditarSolicitudMaterial(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "SalidaEntregaMaterial.aspx/EditarSolicitudEntregaMaterial",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                MostrarMensajeError("Se ha editado con exito!.");
                $("#grdEntregaMaterial").trigger("reloadGrid");
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

function ValidaInventario() {

    var errores = "";
    
    var ids = $('#grdPartidasSolicitudMaterialEditar').jqGrid('getDataIDs');
    console.log(ids);
    for (var i = 0; i < ids.length; i++) {

        var producto = $('#grdPartidasSolicitudMaterialEditar #' + ids[i] + ' td[aria-describedby="grdPartidasSolicitudMaterialEditar_NumeroParte"]').html();
        var disponible = $('#grdPartidasSolicitudMaterialEditar #' + ids[i] + ' td[aria-describedby="grdPartidasSolicitudMaterialEditar_DisponibleInventario"]').html();
        var cantidad = $('#grdPartidasSolicitudMaterialEditar #' + ids[i] + ' td[aria-describedby="grdPartidasSolicitudMaterialEditar_Cantidad"]').html();
        console.log(producto);
        console.log(cantidad);
        console.log(disponible);
        if (disponible == 0 || cantidad > disponible) {

            errores = errores + "<span>*</span> No hay en el inventario la cantidad suficiente para el producto: '" + producto + "'";
        }
        
    }

    if (errores != "") { errores = "<p>Favor de revisar lo siguiente:</p>" + errores; }

    return errores;
}
