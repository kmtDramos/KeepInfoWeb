<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoCliente.aspx.cs" Inherits="TipoCliente" Title="Tipo de cliente" %>
<asp:Content ID="headCatalogoTipoCliente" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoCliente.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoCliente" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarTipoCliente" title="Agregar tipo de cliente"></div>
    <div id="dialogConsultarTipoCliente" title="Consultar tipo de cliente"></div>
    <div id="dialogEditarTipoCliente" title="Editar tipo de cliente"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoCliente" value="+ Agregar tipo de cliente" class="buttonLTR" />
        </div>
        <div id="divGridTipoCliente" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoCliente-->
                <table id="grdTipoCliente"></table>
                <div id="pagTipoCliente"></div>
                <!--FIN DE GRID DE TipoCliente-->
            </div>
        </div>
    </div>
</asp:Content>
