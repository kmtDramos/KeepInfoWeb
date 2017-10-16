//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
    ObtenerFormaFiltrosTraspaso();
    Inicializar_grdAlmacenProductoResumen();
    
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

    ObtenerFormaSeleccionarAlmacen();

    $("#txtCantidadDeEntrada").keypress(function(event) { return ValidarNumero(event, this.value); });

});


function ObtenerFormaFiltrosTraspaso() {
$("#divFiltrosTraspasos").obtenerVista({
    nombreTemplate: "tmplFiltrosReporteExistenciaPorProducto.html",
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
                url: 'ReporteExistenciaPorProducto.aspx/BuscarClave',
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
        url: 'ReporteExistenciaPorProducto.aspx/ObtenerAlmacenProductoDetalle',
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
