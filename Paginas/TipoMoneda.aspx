<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoMoneda.aspx.cs" Inherits="TipoMoneda" Title="Tipos de moneda" %>
<asp:Content ID="headCatalogoTipoMoneda" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoMoneda.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoMoneda" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarTipoMoneda" title="Agregar tipo de moneda"></div>
    <div id="dialogConsultarTipoMoneda" title="Consultar tipo de moneda"></div>
    <div id="dialogEditarTipoMoneda" title="Editar tipo de moneda"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoMoneda" value="+ Agregar tipo de moneda" class="buttonLTR" />
        </div>
        <div id="divGridTipoMoneda" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoMoneda-->
                <table id="grdTipoMoneda"></table>
                <div id="pagTipoMoneda"></div>
                <!--FIN DE GRID DE TipoMoneda-->
            </div>
        </div>
    </div>
</asp:Content>
