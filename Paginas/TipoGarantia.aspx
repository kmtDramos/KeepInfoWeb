<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoGarantia.aspx.cs" Inherits="TipoGarantia" Title="Tipo garantía" %>
<asp:Content ID="headCatalogoTipoGarantia" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoGarantia.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoGarantia" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarTipoGarantia" title="Agregar tipo de garantía"></div>
    <div id="dialogConsultarTipoGarantia" title="Consultar tipo de garantía"></div>
    <div id="dialogEditarTipoGarantia" title="Editar tipo garantía"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoGarantia" value="+ Agregar tipo de garantía" class="buttonLTR" />
        </div>
        <div id="divGridTipoGarantia" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoGarantia-->
                <table id="grdTipoGarantia"></table>
                <div id="pagTipoGarantia"></div>
                <!--FIN DE GRID DE TipoGarantia-->
            </div>
        </div>
    </div>
</asp:Content>
