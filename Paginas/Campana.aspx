<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Campana.aspx.cs" Inherits="Campana" Title="Campaña" %>
<asp:Content ID="headCatalogoCampana" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Campana.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoCampana" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarCampana" title="Agregar campaña"></div>
    <div id="dialogConsultarCampana" title="Consultar campaña"></div>
    <div id="dialogEditarCampana" title="Editar campaña"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarCampana" value="+ Agregar campaña" class="buttonLTR" />
        </div>
        <div id="divGridCampana" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Campana-->
                <table id="grdCampana"></table>
                <div id="pagCampana"></div>
                <!--FIN DE GRID DE Campana-->
            </div>
        </div>
    </div>
</asp:Content>
