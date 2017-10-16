<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoDocumento.aspx.cs" Inherits="TipoDocumento" Title="Página sin título" %>
<asp:Content ID="headCatalogoTipoDocumento" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoDocumento.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoDocumento" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarTipoDocumento" title="Agregar TipoDocumento"></div>
    <div id="dialogConsultarTipoDocumento" title="Consultar TipoDocumento"></div>
    <div id="dialogEditarTipoDocumento" title="Editar TipoDocumento"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoDocumento" value="+ Agregar tipo de documento" class="buttonLTR" />
        </div>
        <div id="divGridTipoDocumento" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoDocumento-->
                <table id="grdTipoDocumento"></table>
                <div id="pagTipoDocumento"></div>
                <!--FIN DE GRID DE TipoDocumento-->
            </div>
        </div>
    </div>
</asp:Content>
