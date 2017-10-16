<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="UnidadCaracteristica.aspx.cs" Inherits="UnidadCaracteristica" Title="Unidad de características" %>
<asp:Content ID="headCatalogoUnidadCaracteristica" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.UnidadCaracteristica.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoUnidadCaracteristica" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarUnidadCaracteristica" title="Agregar unidad de característica"></div>
    <div id="dialogConsultarUnidadCaracteristica" title="Consultar unidad de característica"></div>
    <div id="dialogEditarUnidadCaracteristica" title="Editar unidad de característica"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarUnidadCaracteristica" value="+ Agregar unidad de característica" class="buttonLTR" />
        </div>
        <div id="divGridUnidadCaracteristica" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE UnidadCaracteristica-->
                <table id="grdUnidadCaracteristica"></table>
                <div id="pagUnidadCaracteristica"></div>
                <!--FIN DE GRID DE UnidadCaracteristica-->
            </div>
        </div>
    </div>
</asp:Content>
