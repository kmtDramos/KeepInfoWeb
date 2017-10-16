<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteExistenciaPorProducto.aspx.cs" Inherits="ReporteExistenciaPorProducto" Title="Página sin título" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Reportes.ReporteExistenciaPorProducto.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->

<div id="divContenido">
    <div id="divFiltrosTraspasos"></div>
    <div id="divContGridAlmacenProductoResumen">
    <!--INICIO GRID DETALLE PRODUCTO PROVEEDOR-->
    <div id="divGridAlmacenProductoResumen" class="divContGrid renglon-bottom" IdProducto="${IdProducto}">
        <div id="divContGrid">
            <table id="grdAlmacenProductoResumen"></table>
            <div id="pagAlmacenProductoResumen"></div>
        </div>
    </div>
    <div>
        <table>
        </table>
    </div>
    <!--FIN DE GRID DETALLE PRODUCTO PROVEEDOR-->
    <div id="divContGridAlmacenProductoDetalle">
        <!--INICIO GRID DETALLE PRODUCTO PROVEEDOR-->
        <div id="divGridAlmacenProductoDetalle" class="divContGrid renglon-bottom" IdProducto="${IdProducto}" IdAlmacen="${IdAlmacen}">
            <div id="divContGrid">
                <table id="grdAlmacenProductoDetalle"></table>
                <div id="pagAlmacenProductoDetalle"></div>
            </div>
            
            
        </div>
        
        
        <!--FIN DE GRID DETALLE PRODUCTO PROVEEDOR-->
    </div>
</asp:Content>
