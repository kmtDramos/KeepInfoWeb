<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="EncabezadoFacturaProveedor.aspx.cs" Inherits="EncabezadoFacturaProveedor" Title="Factura de proveedor" %>
<asp:Content ID="headEncabezadoFacturaProveedor" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.EncabezadoFacturaProveedor.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyEncabezadoFacturaProveedor" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarEncabezadoFacturaProveedor" title ="Agregar factura de proveedor"></div>
    <div id="dialogConsultarEncabezadoFacturaProveedor" title ="Consultar factura de proveedor"></div>
    <div id="dialogEditarEncabezadoFacturaProveedor" title ="Editar factura de proveedor"></div>

    <div id="dialogAgregarReingresoMaterial" title ="Agregar Reingreso Material"></div>
    <div id="dialogConsultarReingresoMaterial" title ="Consultar Reingreso Material"></div>

    <div id="dialogMuestraDetalleFacturaProveedor" title ="Detalle de partidas">
        <div id="divFormaDetalleFacturaProveedor"></div>
        <div id="divGridProductoNumeroSerie" class="divContGrid renglon-bottom">
            <div id="divContGridProductoNumeroSerie">
                <!--INICIO GRID PRODUCTO NUMERO DE SERIE-->
                <table id="grdProductoNumeroSerie"></table>
                <div id="pagProductoNumeroSerie"></div>
                <!--FIN DE GRID PRODUCTO NUMERO DE SERIE-->
            </div>
        </div>
    </div>
    <div id="dialogSeleccionarAlmacen" title ="Seleccionar almacén"></div>
    <div id="dialogConfirmarRevision" title ="Confirmar revision" idEncabezadoFacturaProveedor="0">
        ¿Estas seguro que quieres marcar la factura como revisada?<br />
        ¡Al aceptar, ya no se puede editar la factura!
    </div>

    <div id="dialogCancelarFacturaProveedor" title ="Cancelar factura de proveedor"><p>¿Está seguro que desea cancelar la factura de proveedor?</p></div>
    <div id="dialogMuestraDetallePedido" title ="Detalle del pedido">
        <div id="divFormaDetallePedido"></div>
        <div id="divGridDetallePedido" class="divContGrid renglon-bottom">
            <div id="divContGridDetallePedido">
                <!--INICIO GRID DETALLE PEDIDO-->
                <table id="grdDetallePedido"></table>
                <div id="pagDetallePedido"></div>
                <!--FIN DE GRID DETALLE PEDIDO-->
            </div>
        </div>
    </div>

    <div id="dialogMuestraDetalleOrdenCompra" title ="Detalle de orden de compra">
        <div id="divFormaDetalleOrdenCompra"></div>
        <div id="divGridDetalleOrdenCompra" class="divContGrid renglon-bottom">
            <div id="divContGridDetalleOrdenCompra">
                <!--INICIO GRID DETALLE ORDEN DE COMPRA-->
                <table id="grdDetalleOrdenCompra"></table>
                <div id="pagDetalleOrdenCompra"></div>
                <!--FIN DE GRID DETALLE ORDEN DE COMPRA-->
            </div>
        </div>
    </div>
    <div id="dialogFacturasPendientesPorValidar" title ="Facturas pendientes por validar">
        <div id="divFormaFacturasPendientesPorValidar"></div>
        <div id="divGridFacturasPendientesPorValidar" class="divContGrid renglon-bottom">
            <div id="divContGridFacturasPendientesPorValidar">
                <!--INICIO GRID DETALLE ORDEN DE COMPRA-->
                <table id="grdFacturasPendientesPorValidar"></table>
                <div id="pagFacturasPendientesPorValidar"></div>
                <!--FIN DE GRID DETALLE ORDEN DE COMPRA-->
            </div>
        </div>
    </div>
    <!--Dialogs-->
    <div id="divReportes" style="margin:10px;">
		<ul>
			<li><a href="#tabFacturaProveedor">Factura de Proveedor</a></li>
			<li><a href="#tabReingresoMaterial">Reingreso Material</a></li>
		</ul>
		<div id="tabFacturaProveedor" style="font-size:9px;min-height:450px;">
            <div id="divContenido">
                <div id="divTotalesEstatus">
                    <table id="tblTotalesEstatus" idEstatusRecepcionSeleccionado="">
                        <tr>                    
                            <td>
                                <div class="divTituloTotal">Canceladas</div>
                            </td>
                            <td>
                                <div class="divTituloTotal">Activas</div>
                            </td>                                        
                            <td>
                                <div class="divTituloTotal">Total</div>
                            </td>
                        </tr>
                        <tr>                    
                            <td>
                                <span id ="span-E1" title="Filtrar por canceladas" class="spanFiltroTotal" IdEstatusRecepcion="1">
                                0</span>
                            </td>
                            <td>
                                <span id ="span-E0" title="Filtrar por activas" class="spanFiltroTotal" IdEstatusRecepcion="0" >
                                0</span>
                            </td>                    
                            <td>
                                <span id ="span-E4" title="Todos" class="spanFiltroTotal" IdEstatusRecepcion="-1">
                                0</span>
                            </td>
                        </tr>
                     </table>
                </div>
                <div id="divFiltrosEncabezadoFacturaProveedor"></div>  
                <div class="divAreaBotonesDialog">
                    <%= puedeAgregarEncabezadoFacturaProveedor == 1 ? "<input type='button' id='btnObtenerFormaAgregarEncabezadoFacturaProveedor' value='+ Agregar factura de proveedor' class='buttonLTR'/>" : ""%>
                    <%= puedeRevisarFacturaProveedor == 1 ? "<input type='button' id='btnObtenerFormaFacturasPendientesPorValidar' value='Facturas pendientes por validar' class='buttonLTR'/>" : ""%>
                </div>
                <div id="divGridEncabezadoFacturaProveedor" class="divContGrid renglon-bottom">
                    <div id="divContGrid">
                        <!--INICIO GRID DE MEDIO ENTERO-->
                        <table id="grdEncabezadoFacturaProveedor"></table>
                        <div id="pagEncabezadoFacturaProveedor"></div>
                        <!--FIN DE GRID DE MEDIO ENTERO-->
                    </div>
                </div>
            </div>
		</div>
		<div id="tabReingresoMaterial" style="font-size:9px;min-height:450px;">
            <div id="divContenido">
                <div id="divFiltrosReingresoMaterial"></div>  
                <div class="divAreaBotonesDialog">
                    <input type='button' id='btnObtenerFormaAgregarReingresoMaterial' value='+ Agregar Reingreso Material' class='buttonLTR'/>
                </div>
                <div id="divGridReingresoMaterial" class="divContGrid renglon-bottom">
                    <div id="divContGrid">
                        <!--INICIO GRID DE MEDIO ENTERO-->
                        <table id="grdReingresoMaterial"></table>
                        <div id="pagReingresoMaterial"></div>
                        <!--FIN DE GRID DE MEDIO ENTERO-->
                    </div>
                </div>
            </div>
		</div>
	</div>
    
    <asp:GridView ID="tblDatos" runat="server" AutoGenerateColumns="True"></asp:GridView>
</asp:Content>
