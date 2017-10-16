<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="OrdenCompra.aspx.cs" Inherits="OrdenCompra" Title="Orden de compra" %>
<asp:Content ID="headOperacionOrdenCompra" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Operacion.OrdenCompra.js?_=20150518"></script>
</asp:Content>
<asp:Content ID="bodyOperacionOrdenCompra" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
   <!--Dialogs-->
    <div id="dialogAgregarOrdenCompra" title ="Agregar orden de compra"></div>
    <div id="dialogConsultarOrdenCompra" title ="Consultar orden de compra"></div>
    <div id="dialogEditarOrdenCompra" title ="Editar orden de compra"></div>
    <div id="dialogProporcionarClaveAutorizacion" title ="Proporcionar clave autorización"></div>
    <div id="dialogConfirmaAccion" title ="Confirmación"><p id="textoConfirmacion">¿Está seguro que desea realizar esta acción?</p></div>
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
    
    <div id="divContenido">
        <div id="divTotalesEstatus">
            <table id="tblTotalesEstatus" idEstatusRecepcionSeleccionado="">
                <tr>
                    <td><div class="divTituloTotal">Pendientes por recepcionar</div></td>
                    <td><div class="divTituloTotal">Facturados/Recepcionados</div></td>                    
                    <td><div class="divTituloTotal">Canceladas</div></td>
                    <td><div class="divTituloTotal">Total</div></td>
                </tr>
                <tr>
                    <td><span title="Pendientes" class="spanFiltroTotal" id="Pendientes" IdEstatusRecepcion="0">0</span></td>
                    <td><span title="Facturado" class="spanFiltroTotal" id="Facturados" IdEstatusRecepcion="1">0</span></td>
                    <td><span title="Canceladas" class="spanFiltroTotal" id="Canceladas" IdEstatusRecepcion="2">0</span></td>
                    <td><span title="Total" class="spanFiltroTotal" id="Total" IdEstatusRecepcion="-1">0</span></td>                    
                </tr>
             </table>
        </div>
                
        <div id="divFiltrosOrdenCompraEncabezado"></div>
        
        <div class="divAreaBotonesDialog">
            <%= CUsuario.PermisoUsuarioSesion("puedeAgregarOrdenCompra") == true ? "<input type='button' id='btnObtenerFormaAgregarOrdenCompra' value='+ Agregar orden de compra' class='buttonLTR'/>" : ""%>
        </div>
        
        <div id="divGridOrdenCompra" class="divContGrid renglon-bottom">
            <div id="divContGrid">
            <!--INICIO GRID ORDEN COMPRA-->
                <table id="grdOrdenCompra"></table>
                <div id="pagOrdenCompra"></div>
            <!--FIN GRID ORDEN COMPRA-->
            </div>
        </div>
    </div>
    <asp:Button ID="btnDescarga" UseSubmitBehavior="false" style="visibility: hidden; display: none;" Text="Descarga" Font-Bold="true" runat="server" OnClick="btnDescarga_Click" />
</asp:Content>
