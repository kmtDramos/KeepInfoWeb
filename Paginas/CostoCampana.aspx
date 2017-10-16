<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CostoCampana.aspx.cs" Inherits="CostoCampana" Title="Costo de campaña" %>
<asp:Content ID="headCatalogoCostoCampana" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.CostoCampana.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoCostoCampana" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarCostoCampana" title="Agregar costo campaña"></div>
    <div id="dialogConsultarCostoCampana" title="Consultar costo campaña"></div>
    <div id="dialogEditarCostoCampana" title="Editar costo campaña"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarCostoCampana" value="+ Agregar costo campaña" class="buttonLTR" />
        </div>
        <div id="divGridCostoCampana" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE CostoCampana-->
                <table id="grdCostoCampana"></table>
                <div id="pagCostoCampana"></div>
                <!--FIN DE GRID DE CostoCampana-->
            </div>
        </div>
    </div>
</asp:Content>
