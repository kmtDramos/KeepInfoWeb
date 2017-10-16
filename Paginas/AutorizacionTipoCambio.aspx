<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="AutorizacionTipoCambio.aspx.cs" Inherits="AutorizacionTipoCambio" Title="Autorizaciones de tipos de cambio" %>
<asp:Content ID="headCatalogoAutorizacionTipoCambio" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/GestorEmpresarial.AutorizacionTipoCambio.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoAutorizacionTipoCambio" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarAutorizacionTipoCambio" title="Agregar autorización de tipo de cambio"></div>
    <div id="dialogConsultarAutorizacionTipoCambio" title="Consultar autorización de tipo de cambio"></div>
    <div id="dialogEditarAutorizacionTipoCambio" title="Editar autorización de tipo de cambio"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarAutorizacionTipoCambio" value="+ Agregar autorización" class="buttonLTR" />
        </div>
        <div id="divGridAutorizacionTipoCambio" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE AutorizacionTipoCambio-->
                <table id="grdAutorizacionTipoCambio"></table>
                <div id="pagAutorizacionTipoCambio"></div>
                <!--FIN DE GRID DE AutorizacionTipoCambio-->
            </div>
        </div>
    </div>
</asp:Content>
