//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos;

    LimpiarGrid();

    Inicializar_grdFactura();
    Inicializar_grdFacturaDetalleCliente();
    Inicializar_grdRemision();
    Inicializar_grdDetalleFactura();
    Inicializar_grdFacturaProveedor();
    Inicializar_grdPedido();

    $("#txtFechaInicial").change(function() {
        FiltroFactura();
    });

    $("#txtFechaFinal").change(function() {
        FiltroFactura();
    });

});

function FiltroFactura (){
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFactura').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFactura').getGridParam('page');
    request.pColumnaOrden = $('#grdFactura').getGridParam('sortname');
    request.pTipoOrden = $('#grdFactura').getGridParam('sortorder');
    request.pFechaInicio = $('#txtFechaInicial').val();
    request.pFechaFin = $('#txtFechaFinal').val();
    request.pSerieFactura = '';
    request.pNumeroFactura = '';
    request.pAI = 0;   
    
    if ($('#gs_SerieFactura').val() != null) { request.pSerieFactura = $("#gs_SerieFactura").val(); }
    
    if ($('#gs_NumeroFactura').val() != null) { request.pNumeroFactura = $("#gs_NumeroFactura").val();} 

    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ConsultaDocumento.aspx/ObtenerConsultaDocumentoFacturaCliente',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFactura')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });    
}

function FiltroFacturaDetalleCliente (IdFacturaEncabezado){
    var IdFacturaEncabezado = validaNumero(IdFacturaEncabezado) ? IdFacturaEncabezado : 0; 
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaDetalleCliente').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaDetalleCliente').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaDetalleCliente').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaDetalleCliente').getGridParam('sortorder');
    request.pIdFacturaEncabezado = 0;
    request.pAI = 0;  
    
    //var registro = $("#grdFactura tr[aria-selected='true']");
    //var IdFacturaEncabezado = $(registro).children("td[aria-describedby='grdFactura_IdFacturaEncabezado']").html();
    //IdFacturaEncabezado = validaNumero(IdFacturaEncabezado) ? IdFacturaEncabezado : 0; 
       

    if (IdFacturaEncabezado!= 0) { request.pIdFacturaEncabezado = IdFacturaEncabezado;}
        
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ConsultaDocumento.aspx/ObtenerConsultaDocumentoFacturaDetalleCliente',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaDetalleCliente')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });
}

function DetalleFactura (id){
    var id = validaNumero(id) ? id : 0;

    var request = new Object();
    request.pTamanoPaginacion = $('#grdDetalleFactura').getGridParam('rowNum');
    request.pPaginaActual = $('#grdDetalleFactura').getGridParam('page');
    request.pColumnaOrden = $('#grdDetalleFactura').getGridParam('sortname');
    request.pTipoOrden = $('#grdDetalleFactura').getGridParam('sortorder');
    request.pIdPedidoDetalle = 0;

    var registroR = $("#grdFacturaDetalleCliente tr[aria-selected='true']");
    var IdEncabezadoPedido = $(registroR).children("td[aria-describedby='grdFacturaDetalleCliente_Pedido']").html();
    IdEncabezadoPedido = validaNumero(IdEncabezadoPedido) ? IdEncabezadoPedido : 0;
    if (IdEncabezadoPedido != 0) { request.pIdEncabezadoPedido = IdEncabezadoPedido; }

    if ($('#gs_NumeroFacturaProveedor').val() != null) {
        if (id != 0) { request.pIdPedidoDetalle = id; }
    }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ConsultaDocumento.aspx/ObtenerConsultaDetalleFactura',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdDetalleFactura')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
        }
    });
    
}

function FiltroRemision (id){ 

    var id = validaNumero(id) ? id : 0; 
     
    var request = new Object();
    request.pTamanoPaginacion = $('#grdRemision').getGridParam('rowNum');
    request.pPaginaActual = $('#grdRemision').getGridParam('page');
    request.pColumnaOrden = $('#grdRemision').getGridParam('sortname');
    request.pTipoOrden = $('#grdRemision').getGridParam('sortorder');
    request.pIdPedidoDetalle = 0;
    request.pAI = 0;    
    
    var registroR = $("#grdFacturaDetalleCliente tr[aria-selected='true']");
    var IdEncabezadoPedido = $(registroR).children("td[aria-describedby='grdFacturaDetalleCliente_Pedido']").html();
    IdEncabezadoPedido= validaNumero(IdEncabezadoPedido) ? IdEncabezadoPedido : 0;        
    if (IdEncabezadoPedido!= 0) { request.pIdEncabezadoPedido = IdEncabezadoPedido;}
    
    if ($('#gs_NumeroFacturaProveedor').val() != null) {             
        if (id!= 0) { request.pIdPedidoDetalle = id;}    
    } 
    
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ConsultaDocumento.aspx/ObtenerConsultaDocumentoRemision',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdRemision')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });
}

function FiltroFacturaProveedor (id){
    var id = validaNumero(id) ? id : 0;
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaProveedor').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaProveedor').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaProveedor').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaProveedor').getGridParam('sortorder');
    request.pIdPedidoDetalle = 0;
    request.pNumeroFacturaProveedor = "";
    request.pAI = 0;    
           
    if (id!= 0) { request.pIdPedidoDetalle = id;}


    if ($('#gs_NumeroFacturaProveedor').val() != "" && $('#gs_NumeroFacturaProveedor').val() != null) { 
        request.pNumeroFacturaProveedor = $("#gs_NumeroFacturaProveedor").val();
        request.pIdEncabezadoPedido = 0;
    } 
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ConsultaDocumento.aspx/ObtenerConsultaDocumentoFacturaProveedor',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaProveedor')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });
}

function FiltroPedido (id){
    var id = validaNumero(id) ? id : 0;
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPedido').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPedido').getGridParam('page');
    request.pColumnaOrden = $('#grdPedido').getGridParam('sortname');
    request.pTipoOrden = $('#grdPedido').getGridParam('sortorder');
    request.pIdPedidoDetalle = 0;
    request.pAI = 0;    
    
    var registroFP = $("#grdFacturaDetalleCliente tr[aria-selected='true']");
    var IdPedidoDetalle = $(registroFP).children("td[aria-describedby='grdFacturaDetalleCliente_IdPedidoDetalle']").html();
    IdPedidoDetalle= validaNumero(IdPedidoDetalle) ? IdPedidoDetalle : 0; 
       
    if (IdPedidoDetalle!= 0) { request.pIdPedidoDetalle = IdPedidoDetalle;}
    
    if ($('#gs_NumeroFacturaProveedor').val() != null) {             
        if (id!= 0) { request.pIdPedidoDetalle = id;}    
    }
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'ConsultaDocumento.aspx/ObtenerConsultaDocumentoPedido',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdPedido')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });
}

function LimpiarGrid(){

    $("#grdFactura").jqGrid("clearGridData", true); 
    $("#grdFacturaDetalleCliente").jqGrid("clearGridData", true); 
    $("#grdRemision").jqGrid("clearGridData", true);
    $("#grdFacturaProveedor").jqGrid("clearGridData", true);                      
    $("#grdPedido").jqGrid("clearGridData", true);
    $('#gs_NumeroFacturaProveedor').val("")
    $('#gs_SerieFactura').val("")
    $('#gs_NumeroFactura').val("")
}

function FacturaClienteSeleccionado(id){   
    var registro = $("#grdFactura tr[id='" + id + "']"); 
    IdFacturaEncabezado = parseInt($(registro).children("td[aria-describedby='grdFactura_IdFacturaEncabezado']").html());    
                                      
    $("#grdFacturaDetalleCliente").jqGrid("clearGridData", true); 
    $("#grdRemision").jqGrid("clearGridData", true);
    $("#grdFacturaProveedor").jqGrid("clearGridData", true);                      
    $("#grdPedido").jqGrid("clearGridData", true);
    $('#gs_NumeroFacturaProveedor').val("")
    FiltroFacturaDetalleCliente(IdFacturaEncabezado); 
}

function DetalleFacturaClienteSeleccionado(id){
    var registro = $("#grdFacturaDetalleCliente tr[id='" + id + "']"); 
    idPedido = parseInt($(registro).children("td[aria-describedby='grdFacturaDetalleCliente_IdPedido']").html());
    idPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdFacturaDetalleCliente_IdPedidoDetalle']").html());
    $("#grdRemision").jqGrid("clearGridData", true);
    $("#grdFacturaProveedor").jqGrid("clearGridData", true);                      
    $("#grdPedido").jqGrid("clearGridData", true); 
    $('#gs_NumeroFacturaProveedor').val("")
    
    FiltroRemision(idPedidoDetalle);
    FiltroFacturaProveedor(idPedidoDetalle);
    FiltroPedido(idPedidoDetalle);
    DetalleFactura(idPedidoDetalle);
}


function DetalleFacturaProveedorSeleccionado(id){
    var registro = $("#grdFacturaProveedor tr[id='" + id + "']"); 
    idPedido = parseInt($(registro).children("td[aria-describedby='grdFacturaProveedor_IdPedido']").html());
    idPedidoDetalle = parseInt($(registro).children("td[aria-describedby='grdFacturaProveedor_IdPedidoDetalle']").html());

    $("#grdFactura").jqGrid("clearGridData", true);                      
    $("#grdFacturaDetalleCliente").jqGrid("clearGridData", true);                    
    FiltroRemision(idPedidoDetalle);
    FiltroPedido(idPedidoDetalle);        

    $('#gs_SerieFactura').val('')
    $('#gs_NumeroFactura').val('')

}

function FiltroDetalleFactura() { }