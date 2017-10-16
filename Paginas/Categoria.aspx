<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Categoria.aspx.cs" Inherits="Categoria" Title="Categorías" %>
<asp:Content ID="headCatalogoCategoria" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Categoria.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoCategoria" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarCategoria" title="Agregar categoría"></div>
    <div id="dialogConsultarCategoria" title="Consultar Categoría"></div>
    <div id="dialogEditarCategoria" title="Editar categoría"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarCategoria" value="+ Agregar categoría" class="buttonLTR" />
        </div>
        <div id="divGridCategoria" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Categoria-->
                <table id="grdCategoria"></table>
                <div id="pagCategoria"></div>
                <!--FIN DE GRID DE Categoria-->
            </div>
        </div>
    </div>
</asp:Content>
