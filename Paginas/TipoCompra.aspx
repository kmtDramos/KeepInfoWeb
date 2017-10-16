<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoCompra.aspx.cs" Inherits="TipoCompra" Title="Tipos de compra" %>
<asp:Content ID="headTipoCompra" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.TipoCompra.js"></script>
</asp:Content>
<asp:Content ID="bodyTipoCompra" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarTipoCompra" title ="Agregar tipo de compra"></div>
<div id="dialogConsultarTipoCompra" title ="Consultar tipo de compra"></div>
<div id="dialogEditarTipoCompra" title ="Editar tipo de compra"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarTipoCompra" value="+ Agregar tipo de compra" class="buttonLTR" />
    </div>
    <div id="divGridTipoCompra" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE TipoCompra-->
            <table id="grdTipoCompra"></table>
            <div id="pagTipoCompra"></div>
            <!--FIN DE GRID DE TipoCompra-->
        </div>
    </div>
</div>
</asp:Content>
