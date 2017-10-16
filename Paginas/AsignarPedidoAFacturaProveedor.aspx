<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="AsignarPedidoAFacturaProveedor.aspx.cs" Inherits="AsignarPedidoAFacturaProveedor" Title="Asignar pedidos a facturas proveedor" %>
<asp:Content ID="headAsignarPedidoAFacturaProveedor" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/Operacion.AsignarPedidoAFacturaProveedor.js"></script>
</asp:Content>
<asp:Content ID="bodyAsignarPedidoAFacturaProveedor" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<div id="dialogFacturaProveedorSinAsignacionPedido" title="Asignar pedido a factura proveedor"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarPedidoAFacturaProveedor" value="+ Asignar pedido a factura proveedor" class="buttonLTR" />
    </div>
    <div id="divGridPedidoAFacturaProveedor" class="divContGrid renglon-bottom">
        <div id="divContGrid">               
            <table id="grdPedidoFacturaProveedor"></table>
            <div id="pagPedidoFacturaProveedor"></div>
        </div>
    </div>
</div>
</asp:Content>