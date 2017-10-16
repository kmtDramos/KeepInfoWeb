<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoCambio.aspx.cs" Inherits="TipoCambio" Title="Tipo de cambio" %>
<asp:Content ID="headCatalogoTipoCambio" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoCambio.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoCambio" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarTipoCambio" title="Agregar tipo de cambio"></div>
    <div id="dialogConsultarTipoCambio" title="Consultar tipo de cambio"></div>
    <div id="dialogEditarTipoCambio" title="Editar tipo de cambio"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoCambio" value="+ Agregar tipo de cambio" class="buttonLTR" />
        </div>
        <div id="divGridTipoCambio" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoCambio-->
                <table id="grdTipoCambio"></table>
                <div id="pagTipoCambio"></div>
                <!--FIN DE GRID DE TipoCambio-->
            </div>
        </div>
    </div>
</asp:Content>
