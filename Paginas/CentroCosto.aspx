<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CentroCosto.aspx.cs" Inherits="CentroCosto" Title="Centro de costos" %>
<asp:Content ID="headCentroCosto" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.CentroCosto.js"></script>
</asp:Content>
<asp:Content ID="bodyCentroCosto" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarCentroCosto" title ="Agregar centro de costos"></div>
<div id="dialogConsultarCentroCosto" title ="Consultar centro de costos"></div>
<div id="dialogEditarCentroCosto" title ="Editar centro de costos"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarCentroCosto" value="+ Agregar centro de costos" class="buttonLTR" />
    </div>
    <div id="divGridCentroCosto" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE MEDIO ENTERO-->
            <table id="grdCentroCosto"></table>
            <div id="pagCentroCosto"></div>
            <!--FIN DE GRID DE MEDIO ENTERO-->
        </div>
    </div>
</div>
</asp:Content>
