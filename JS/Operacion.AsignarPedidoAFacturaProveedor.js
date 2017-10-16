//----------DHTMLX----------//
//--------------------------//

//----------JQuery----------//
//--------------------------//
$(document).ready(function() {
    setInterval(MantenerSesion, 150000); //2.5 minutos
        
    $(".divAreaBotonesDialog").on("click", "#btnObtenerFormaAgregarPedidoAFacturaProveedor", function() {
        ObtenerFormaAgregarPedidoAFacturaProveedor();
    });
    
    $('#dialogFacturaProveedorSinAsignacionPedido').dialog({
        autoOpen: false,
        height: 'auto',
        width: 'auto',
        modal: true,
        draggable: false,
        resizable: false,
        show: 'fade',
        hide: 'fade',
        close: function() {
            $("#divFormaAgregarPedidoAFacturaProveedor").remove();
        },
        buttons: {
            //"Agregar": function() {
              //  AgregarPedidoAFacturaProveedor();
            //},
            "Cancelar": function() {
                $(this).dialog("close")
            }
        }
    });
    
})

//-----------AJAX-----------//
//-Funciones Obtener Formas-//
function ObtenerFormaAgregarPedidoAFacturaProveedor()
{
    $("#dialogFacturaProveedorSinAsignacionPedido").obtenerVista({
        nombreTemplate: "tmplAgregarPedidoAFacturaProveedor.html",
        despuesDeCompilar: function(pRespuesta) 
        {
            $("#dialogFacturaProveedorSinAsignacionPedido").dialog("open");
            
            $("#divGridPedidoSinAsignar").show();
            $("#divGridProyectoSinAsignar").hide();
            
            
            $("#PedidoProyecto input[type='radio']:checked").val(); // ?
            
    
            $("#txtFechaInicial, #txtFechaFinal, #txtFechaInicialDerecha, #txtFechaFinalDerecha").datepicker();  
	        $("#txtFechaInicial, #txtFechaFinal, #txtFechaInicialDerecha, #txtFechaFinalDerecha").datepicker("setDate", "+0m");          
            
            $("#divFormaAgregarPedidoAFacturaProveedor").on('click','#btnActualizaPedidoSinAsignar', function(event){                
                if ($("#col-2 input[type='radio']:checked").val()== 1) {
                    FiltroPedidoSinAsignar();  
                } else {
                    FiltroProyectoSinAsignar();  
                }                         
            });
            
            $("#divFormaAgregarPedidoAFacturaProveedor").on('click','#btnActualizaFacturaProveedorSinAsignacionPedido', function(event){
                FiltroFacturaProveedorSinAsignacionPedido();            
            });
            
            $('#divFormaAgregarPedidoAFacturaProveedor').on('click', '#chkMultiplesAsignaciones', function(event) {
                FiltroPedidoSinAsignar();
            });
            
            $('#divFormaAgregarPedidoAFacturaProveedor').on('click', '#chkMostrarTodoMaterial', function(event) {
                FiltroFacturaProveedorSinAsignacionPedido();
            });
            
            $("input[name=PedidoProyecto]:radio").click(function(evento) {
                if (this.value == 1) {
                    $("#divGridPedidoSinAsignar").show();
                    $("#divGridProyectoSinAsignar").hide();
                } else {
                    $("#divGridProyectoSinAsignar").show();
                    $("#divGridPedidoSinAsignar").hide();
                }
            });
            
            Inicializar_grdFacturaProveedorSinAsignacionPedido();
            Inicializar_grdPedidoSinAsignar();
            Inicializar_grdProyectoSinAsignar();
            
            $("#divFormaAgregarPedidoAFacturaProveedor").on('click','#btnAsignar', function(event){
                Asignar();            
            });                             
        }
    });
}

function FiltroFacturaProveedorSinAsignacionPedido() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdFacturaProveedorSinAsignacionPedido').getGridParam('rowNum');
    request.pPaginaActual = $('#grdFacturaProveedorSinAsignacionPedido').getGridParam('page');
    request.pColumnaOrden = $('#grdFacturaProveedorSinAsignacionPedido').getGridParam('sortname');
    request.pTipoOrden = $('#grdFacturaProveedorSinAsignacionPedido').getGridParam('sortorder');
    request.pNumeroFacturaAsignar = "";
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pIdPedidoDetalle = 0;
    request.pAI = 0;
    

    if ($('#txtFechaInicial').val() != null) {
        var elementi = $("#txtFechaInicial").val().split(' ');
        var fechai = elementi[0].split('/');
        fechai= fechai[2]+fechai[1]+fechai[0];
    }
    
    if ($('#txtFechaFinal').val() != null) {
        var elementf = $("#txtFechaFinal").val().split(' ');
        var fechaf = elementf[0].split('/');
        fechaf = fechaf[2]+fechaf[1]+fechaf[0];
    }

    if ($('#txtFechaInicial').val() != null) { request.pFechaInicial = fechai;}
    if ($('#txtFechaFinal').val() != null) { request.pFechaFinal = fechaf;}
    if ($("#chkMostrarTodoMaterial").is(':checked')) { request.pIdPedidoDetalle = -1; }     
    if ($('#gs_NumeroFacturaAsignar').val() != null) { request.pNumeroFacturaAsignar = $("#gs_NumeroFacturaAsignar").val();}
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'AsignarPedidoAFacturaProveedor.aspx/ObtenerFacturaProveedorSinAsignacionPedido',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdFacturaProveedorSinAsignacionPedido')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });
}

function FiltroPedidoSinAsignar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdPedidoSinAsignar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdPedidoSinAsignar').getGridParam('page');
    request.pColumnaOrden = $('#grdPedidoSinAsignar').getGridParam('sortname');
    request.pTipoOrden = $('#grdPedidoSinAsignar').getGridParam('sortorder');
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pFolio = "";
    request.pClave = "";
    request.pDescripcion = "";
    request.pSucursal = -1;
    request.pTodos = 0; //pedidos asignados y no asignados a una factura proveedor
    request.pAI = 0;

    if ($("#gs_Folio").length > 0) {
        request.pFolio = ($("#gs_Folio").val().length > 0) ? $("#gs_Folio").val() : "";
    }

    if ($("#gs_Clave").length > 0) {
        request.pClave = ($("#gs_Clave").val().length > 0) ? $("#gs_Clave").val() : "";
    }

    if ($("#gs_Descripcion").length > 0) {
        request.pDescripcion = ($("#gs_Descripcion").val().length > 0) ? $("#gs_Descripcion").val() : "";
    }

    if ($("#gs_Sucursal").length > 0) {
        request.pSucursal = ($("#gs_Sucursal").val().length > 0) ? $("#gs_Sucursal").val() : "";
    }

    if ($('#txtFechaInicialDerecha').val() != null) {
        request.pFechaInicial = ConvertirFecha($("#txtFechaInicialDerecha").val(), 'aaaammdd'); 
    }
    if ($('#txtFechaFinalDerecha').val() != null) {
        request.pFechaFinal = ConvertirFecha($("#txtFechaFinalDerecha").val(), 'aaaammdd');
    }
    
    if ($("#chkMultiplesAsignaciones").is(':checked')) { request.pTodos = -1; }

    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'AsignarPedidoAFacturaProveedor.aspx/ObtenerPedidoSinAsignar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdPedidoSinAsignar')[0].addJSONData(JSON.parse(jsondata.responseText).d); }
        }
    });
}

function FiltroProyectoSinAsignar() {
    var request = new Object();
    request.pTamanoPaginacion = $('#grdProyectoSinAsignar').getGridParam('rowNum');
    request.pPaginaActual = $('#grdProyectoSinAsignar').getGridParam('page');
    request.pColumnaOrden = $('#grdProyectoSinAsignar').getGridParam('sortname');
    request.pTipoOrden = $('#grdProyectoSinAsignar').getGridParam('sortorder');
    request.pFechaInicial = "";
    request.pFechaFinal = "";
    request.pTodos = 0; //proyectos no asignados a una factura proveedor
    request.pIdProyecto = 0;
    request.pNombreProyecto = "";
    request.pAI = 0;
    
    
    if ($('#txtFechaInicialDerecha').val() != null) {
        var element1 = $("#txtFechaInicialDerecha").val().split(' ');
        var fechai = element1[0].split('/');
        fechai= fechai[2]+fechai[1]+fechai[0];
    }
    
    if ($('#txtFechaFinalDerecha').val() != null) {
        var element2 = $("#txtFechaFinalDerecha").val().split(' ');
        var fechaf = element2[0].split('/');
        fechaf = fechaf[2]+fechaf[1]+fechaf[0];
    }

    if ($('#txtFechaInicialDerecha').val() != null) { request.pFechaInicial = fechai;}
    if ($('#txtFechaFinalDerecha').val() != null) { request.pFechaFinal = fechaf;}


    if ($("#chkMultiplesAsignaciones").is(':checked')) { request.pTodos = -1; }
    if ($("#gs_IdProyecto").val() != null && $("#gs_IdProyecto").val() != "") { request.pIdProyecto = $("#gs_IdProyecto").val(); }
    if ($('#gs_NombreProyecto').val() != null) { request.pNombreProyecto = $("#gs_NombreProyecto").val(); }   
    
    var pRequest = JSON.stringify(request);
    $.ajax({
        url: 'AsignarPedidoAFacturaProveedor.aspx/ObtenerProyectoSinAsignar',
        data: pRequest,
        dataType: 'json',
        type: 'post',
        contentType: 'application/json; charset=utf-8',
        complete: function(jsondata, stat) {
            if (stat == 'success')
            { $('#grdProyectoSinAsignar')[0].addJSONData(JSON.parse(jsondata.responseText).d); } 
        }
    });
}

function Asignar(){
    var Valor = 0;
    var IdPedido = 0;
    var IdPedidoDetalle = 0;
    var IdProyecto = 0;
    var registroFP = $("#grdFacturaProveedorSinAsignacionPedido tr[aria-selected='true']");
    var idDetalleFacturaProveedor = $(registroFP).children("td[aria-describedby='grdFacturaProveedorSinAsignacionPedido_IdDetalleFacturaProveedor']").html();
    idDetalleFacturaProveedor= validaNumero(idDetalleFacturaProveedor) ? idDetalleFacturaProveedor : 0;    
    
    if ($("#col-2 input[type='radio']:checked").val() == 1) {   
        var registroPedido = $("#grdPedidoSinAsignar tr[aria-selected='true']");
               
        IdPedidoDetalle = $(registroPedido).children("td[aria-describedby='grdPedidoSinAsignar_IdPedidoDetalle']").html();
        IdPedidoDetalle= validaNumero(IdPedidoDetalle) ? IdPedidoDetalle : 0;
        
        IdPedido = $(registroPedido).children("td[aria-describedby='grdPedidoSinAsignar_IdPedido']").html();
        IdPedido= validaNumero(IdPedido) ? IdPedido : 0;
        
    } else {
        var registroProyecto = $("#grdProyectoSinAsignar tr[aria-selected='true']");
        IdProyecto = $(registroProyecto).children("td[aria-describedby='grdProyectoSinAsignar_IdProyecto']").html();
        IdProyecto= validaNumero(IdProyecto) ? IdProyecto : 0;
    }   
      
    if ($("#col-2 input[type='radio']:checked").val() == 1) {    
        Valor = IdPedido;
    } else {       
        Valor= IdProyecto;
    }
    
    if ( idDetalleFacturaProveedor == 0 || Valor ==0)
    {
        MostrarMensajeError("Seleccione un registro de cada tabla"); return false; 
    }    
     
    var pDatos = new Object();
    
    pDatos.IdDetalleFacturaProveedor = idDetalleFacturaProveedor
    pDatos.IdPedido =IdPedido
    pDatos.IdPedidoDetalle =IdPedidoDetalle 
    pDatos.IdProyecto = IdProyecto  
    var oRequest = new Object();
    oRequest.pDatos = pDatos;
    SetActualizarEncabezadoFacturaProveedor(JSON.stringify(oRequest));                     
}


function SetActualizarEncabezadoFacturaProveedor(pRequest){
    MostrarBloqueo();
    $.ajax({
        type: "POST",
        url: "AsignarPedidoAFacturaProveedor.aspx/AsignarAFacturaProveedor",
        data: pRequest,
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function(pRespuesta) {
            respuesta = jQuery.parseJSON(pRespuesta.d);
            if (respuesta.Error == 0) {            
                $("#grdFacturaProveedorSinAsignacionPedido").trigger("reloadGrid");
                $("#grdPedidoSinAsignar").trigger("reloadGrid");
                $("#grdProyectoSinAsignar").trigger("reloadGrid");
                MostrarMensajeError("Datos actualizados"); 
            }
            else {
                MostrarMensajeError(respuesta.Descripcion); 
                return false;              
            }
        },
        complete: function() {
            OcultarBloqueo();
           // $("#dialogAgregarCotizacion").dialog("close");
        }
    });
}