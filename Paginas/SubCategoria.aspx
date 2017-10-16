<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="SubCategoria.aspx.cs" Inherits="SubCategoria" Title="Subcategoría" %>
<asp:Content ID="headCatalogoSubCategoria" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.SubCategoria.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoSubCategoria" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarSubCategoria" title="Agregar subgrupo"></div>
    <div id="dialogConsultarSubCategoria" title="Consultar subgrupo"></div>
    <div id="dialogEditarSubCategoria" title="Editar subgrupo"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <%= puedeAgregarSubCategoria == 1 ? "<input type='button' id='btnObtenerFormaAgregarSubCategoria' value='+ Agregar subgrupo' class='buttonLTR' />" : "" %>
        </div>
        <div id="divGridSubCategoria" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE SubCategoria-->
                <table id="grdSubCategoria"></table>
                <div id="pagSubCategoria"></div>
                <!--FIN DE GRID DE SubCategoria-->
            </div>
        </div>
    </div>
</asp:Content>
