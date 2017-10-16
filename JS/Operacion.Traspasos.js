//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosTraspaso();
    Inicializar_grdAlmacenProductoResumen();
    //Inicializar_grdAlmacenProductoDetalle();
    $("#btnGenerarTraspaso").livequery('click', function() {
        GenerarTraspaso();
    });

    $("#grdAlmacenProductoResumen").on("click", ".imgConsultarAlmacenDetalle", function() {
        Inicializar_grdAlmacenProductoDetalle();
        var registro = $(this).parents("tr");
        var Almacen = new Object();
        Almacen.pIdAlmacen = parseInt($(registro).children("td[aria-describedby='grdAlmacenProductoResumen_IdAlmacen']").html());
        Almacen.pIdProducto = parseInt($(registro).children("td[aria-describedby='grdAlmacenProductoResumen_IdProducto']").html());
        $("#divGridAlmacenProductoDetalle").attr("IdAlmacen", Almacen.pIdAlmacen);
        $("#divGridAlmacenProductoDetalle").attr("IdProducto", Almacen.pIdProducto);
        $("#grdAlmacenProductoDetalle").trigger("reloadGrid");

    });

    $("#grdAlmacenProductoDetalle").on("click", ".imgObtenerDatosTraspaso", function() {
        var registro = $(this).parents("tr");
        var Traspaso = new Object();
        $("#txtIdAlmacenOrigen").val($(registro).children("td[aria-describedby='grdAlmacenProductoDetalle_IdAlmacenOrigen']").html());
        $("#txtAlmacenOrigen").val($(registro).children("td[aria-describedby='grdAlmacenProductoDetalle_AlmacenDetalle']").html());
        $("#txtSaldo").val(parseInt($(registro).children("td[aria-describedby='grdAlmacenProductoDetalle_Saldo']").html()));
        $("#txtCantidad").val(parseInt($(registro).children("td[aria-describedby='grdAlmacenProductoDetalle_CantidadDetalle']").html()));
        $("#txtNumeroFactura").val($(registro).children("td[aria-describedby='grdAlmacenProductoDetalle_NumeroFactura']").html());
        $("#txtIdExistenciaDistribuida").val($(registro).children("td[aria-describedby='grdAlmacenProductoDetalle_IdExistenciaDistribuida']").html());
    });

    $('#dialogObtenerDatosTraspaso').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divObtenerDatosTraspaso").remove();
        },
        buttons: {
            "Aceptar": function() {
                $(this).dialog("close");
            }
        }
    });

    ObtenerFormaSeleccionarAlmacen();

    $("#txtCantidadDeEntrada").keypress(function(event) { return ValidarNumero(event, this.value); });

});

//function ObtenerDatosTraspaso(pExistenciaDistribuida) {
//    alert('pasas');
//    $("#dialogObtenerDatosTraspaso").obtenerVista({
//        nombreTemplate: "tmplObtenerDatosTraspaso.html",
//        url: "Traspasos.aspx/ObtenerDatosTraspaso",
//        parametros: pIdCuentaContable,
//        despuesDeCompilar: function(pRespuesta) {
//            if (pRespuesta.modelo.Permisos.puedeEditarCuentaContable == 1) {
//                $("#dialogObtenerDatosTraspaso").dialog("option", "buttons", {
//                    "Editar": function() {
//                        $(this).dialog("close");
//                        var CuentaContable = new Object();
//                        CuentaContable.IdCuentaContable = parseInt($("#divFormaConsultarCuentaContable").attr("IdCuentaContable"));
//                        ObtenerFormaEditarCuentaContable(JSON.stringify(CuentaContable))
//                    }
//                });
//                $("#dialogConsultarCuentaContable").dialog("option", "height", "auto");
//            }
//            else {
//                $("#dialogConsultarCuentaContable").dialog("option", "buttons", {});
//                $("#dialogConsultarCuentaContable").dialog("option", "height", "100");
//            }
//            $("#dialogConsultarCuentaContable").dialog("open");
//        }
//    });
//}

function ObtenerDatosTraspaso(pRequest) {
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        //url: "Traspasos.aspx/ObtenerTipoCambio",
        url: "Traspasos.aspx/ObtenerDatosTraspaso",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
//                if (respuesta.Modelo.TipoCambioActual == 0) {
//                    $("#spanTipoCambioFactura").text(1);
//                }
//                else {
//              
//                }
                //                $("#grdPedidoDetalle").trigger("reloadGrid");
                $("#spanTipoCambioFactura").text(respuesta.Modelo.TipoCambioActual);
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



function ObtenerFormaFiltrosTraspaso() {
$("#divFiltrosTraspasos").obtenerVista({
    nombreTemplate: "tmplFiltrosTraspasos.html",
    despuesDeCompilar: function(pRespuesta) {
    autocompletarClaveProducto();        
    }
    });
}

function autocompletarClaveProducto() {
    $('#txtClaveProducto').autocomplete({
        source: function(request, response) {
            var pRequest = new Object();
            pRequest.pClave = $("#txtClaveProducto").val();
            $.ajax({
                type: 'POST',
                url: 'Traspasos.aspx/BuscarClave',
                data: JSON.stringify(pRequest),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                $("#divGridAlmacenProductoResumen").attr("IdProducto", "0");
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Producto, value: item.Producto, id: item.IdProducto }
                        //return { label: item.NombreProyecto, value: item.NombreProyecto, id: item.IdProyecto }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdProducto = ui.item.id;
            $("#divGridAlmacenProductoResumen").attr('IdProducto', pIdProducto);
            FiltroAlmacenProductoResumen();
            //$("#grdAlmacenProductoResumen").trigger("reloadGrid");
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}


function CrearAutocompletarBuscador() {
    $('#txtBuscador').autocomplete({
        source: function(request, response) {
            var pBuscador = new Object();
            pBuscador.pTipoBusqueda = $("#cmbTipoBusqueda").val();
            pBuscador.pBusqueda = $("#txtBuscador").val();

            $.ajax({
                type: 'POST',
                url: 'Traspasos.aspx/BuscarClienteProyecto',
                data: JSON.stringify(pBuscador),
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function(pRespuesta) {
                    var json = jQuery.parseJSON(pRespuesta.d);
                    response($.map(json.Table, function(item) {
                        return { label: item.Descripcion, value: item.Descripcion, id: item.IdGenerico }
                    }));
                }
            });
        },
        minLength: 2,
        select: function(event, ui) {
            var pIdGenerico = ui.item.id;
            $("#txtBuscador").attr("idGenerico", pIdGenerico);
            //var Cliente = new Object();
            //Cliente.pIdCliente = pIdCliente;
            //Cliente.pIdUsuario = $("#divFormaAsignarClienteUsuario").attr("idUsuario");
            //obtenerCliente(JSON.stringify(Cliente));
        },
        change: function(event, ui) { },
        open: function() { $(this).removeClass("ui-corner-all").addClass("ui-corner-top"); },
        close: function() { $(this).removeClass("ui-corner-top").addClass("ui-corner-all"); }
    });
}

function FiltroAlmacenProductoResumen() {
    
    var request = new Object();
    request.pTamanoPaginacion = $('#grdAlmacenProductoResumen').getGridParam('rowNum');
    request.pPaginaActual = $('#grdAlmacenProductoResumen').getGridParam('page');
    request.pColumnaOrden = $('#grdAlmacenProductoResumen').getGridParam('sortname');
    request.pTipoOrden = $('#grdAlmacenProductoResumen').getGridParam('sortorder');
    request.pIdProducto = 0;

    if ($("#divGridAlmacenProductoResumen").attr("IdProducto") != null && $("#divGridAlmacenProductoResumen").attr("IdProducto") != "") {
        request.pIdProducto = $("#divGridAlmacenProductoResumen").attr("IdProducto");
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Traspasos.aspx/ObtenerAlmacenProductoResumen',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdAlmacenProductoResumen')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function FiltroAlmacenProductoDetalle() {//modificar

    var request = new Object();
    request.pTamanoPaginacion = $('#grdAlmacenProductoDetalle').getGridParam('rowNum');
    request.pPaginaActual = $('#grdAlmacenProductoDetalle').getGridParam('page');
    request.pColumnaOrden = $('#grdAlmacenProductoDetalle').getGridParam('sortname');
    request.pTipoOrden = $('#grdAlmacenProductoDetalle').getGridParam('sortorder');
    request.pIdProducto = 0;
    request.pIdAlmacen = 0;

    if ($("#divGridAlmacenProductoDetalle").attr("IdProducto") != null && $("#divGridAlmacenProductoDetalle").attr("IdProducto") != "") {
        request.pIdProducto = $("#divGridAlmacenProductoDetalle").attr("IdProducto");
    }
    if ($("#divGridAlmacenProductoDetalle").attr("IdAlmacen") != null && $("#divGridAlmacenProductoDetalle").attr("IdAlmacen") != "") {
        request.pIdAlmacen = $("#divGridAlmacenProductoDetalle").attr("IdAlmacen");
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'Traspasos.aspx/ObtenerAlmacenProductoDetalle',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdAlmacenProductoDetalle')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
            else
            { alert(JSON.parse(jsondata.responseText).Message); }
        }
    });
}

function ObtenerFormaSeleccionarAlmacen() {
    $("#cmbAlmacenDestino").obtenerVista({
        nombreTemplate: "tmplComboGenerico.html",
        url: "Traspasos.aspx/ObtenerListaAlmacenDestino",
        despuesDeCompilar: function(pRespuesta) {
        }
    });
}

function GenerarTraspaso() {
    var pTraspaso = new Object();
    
    pTraspaso.pIdAlmacenDestino = $("#cmbAlmacenDestino").val();
    pTraspaso.pNumeroFactura = $("#txtNumeroFactura").val();
    pTraspaso.pIdExistenciaDistribuida = $("#txtIdExistenciaDistribuida").val();
    pTraspaso.pSaldo = $("#txtSaldo").val();
    pTraspaso.pCantidad = $("#txtCantidad").val();
    pTraspaso.pCantidadDeEntrada = $("#txtCantidadDeEntrada").val();
    pTraspaso.pIdAlmacenOrigen = $("#txtIdAlmacenOrigen").val();

    var oRequest = new Object();
    oRequest.pTraspaso = pTraspaso;
    SetGenerarTraspaso(JSON.stringify(oRequest));

    $("#txtAlmacenOrigen").val("");
    $("#txtIdAlmacenOrigen").val("");
    $("#cmbAlmacenDestino").val("0");
    $("#txtNumeroFactura").val("");
    $("#txtIdExistenciaDistribuida").val("");
    $("#txtSaldo").val("");
    $("#txtCantidad").val("");
    $("#txtCantidadDeEntrada").val("");
}

function SetGenerarTraspaso(pRequest) {
    $.ajax({
        type: "POST",
        url: "Traspasos.aspx/GenerarTraspaso",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {
                $("#grdAlmacenProductoDetalle").trigger("reloadGrid");
                $("#grdAlmacenProductoResumen").trigger("reloadGrid");
            }
            else if (respuesta.Descripcion == "valida") {
                ObtenerFormaConsultarProveedorAEnrolar(JSON.stringify(respuesta.Modelo));
            }
            else {
                MostrarMensajeError(respuesta.Descripcion);
                return false;
            }
        },
        complete: function() {
        }
    });
}

