<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="UnidadCompraVenta.aspx.cs" Inherits="UnidadCompraVenta" Title="Unidad de compra venta" %>
<asp:Content ID="headUnidadCompraVenta" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.UnidadCompraVenta.js"></script>
</asp:Content>
<asp:Content ID="bodyUnidadCompraVenta" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarUnidadCompraVenta" title ="Agregar unidad de compra venta"></div>
<div id="dialogConsultarUnidadCompraVenta" title ="Consultar unidad de compra venta"></div>
<div id="dialogEditarUnidadCompraVenta" title ="Editar unidad de compra venta"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarUnidadCompraVenta" value="+ Agregar unidad de compra venta" class="buttonLTR" />
    </div>
    <div id="divGridUnidadCompraVenta" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE MEDIO ENTERO-->
            <table id="grdUnidadCompraVenta"></table>
            <div id="pagUnidadCompraVenta"></div>
            <!--FIN DE GRID DE MEDIO ENTERO-->
        </div>
    </div>
</div>
</asp:Content>
