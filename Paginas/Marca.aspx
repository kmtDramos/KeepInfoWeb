<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Marca.aspx.cs" Inherits="Marca" Title="Marcas" %>
<asp:Content ID="headCatalogoMarca" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Marca.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoMarca" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarMarca" title="Agregar marca"></div>
    <div id="dialogConsultarMarca" title="Consultar Marca"></div>
    <div id="dialogEditarMarca" title="Editar Marca"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarMarca" value="+ Agregar marca" class="buttonLTR" />
        </div>
        <div id="divGridMarca" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Marca-->
                <table id="grdMarca"></table>
                <div id="pagMarca"></div>
                <!--FIN DE GRID DE Marca-->
            </div>
        </div>
    </div>
</asp:Content>
