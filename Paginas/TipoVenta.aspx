<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoVenta.aspx.cs" Inherits="TipoVenta" Title="Tipo de venta" %>
<asp:Content ID="headTipoVenta" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.TipoVenta.js"></script>
</asp:Content>
<asp:Content ID="bodyTipoVenta" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarTipoVenta" title ="Agregar tipo de venta"></div>
<div id="dialogConsultarTipoVenta" title ="Consultar tipo de venta"></div>
<div id="dialogEditarTipoVenta" title ="Editar tipo de venta"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarTipoVenta" value="+ Agregar tipo de venta" class="buttonLTR" />
    </div>
    <div id="divGridTipoVenta" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE MEDIO ENTERO-->
            <table id="grdTipoVenta"></table>
            <div id="pagTipoVenta"></div>
            <!--FIN DE GRID DE MEDIO ENTERO-->
        </div>
    </div>
</div>
</asp:Content>
