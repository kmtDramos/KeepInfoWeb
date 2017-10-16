<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Banco.aspx.cs" Inherits="Banco" Title="Banco" %>
<asp:Content ID="headCatalogoBanco" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Banco.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoBanco" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<div id="dialogAgregarBanco" title="Agregar banco"></div>
<div id="dialogConsultarBanco" title="Consultar banco"></div>
<div id="dialogEditarBanco" title="Editar banco"></div>
<div id="divContenido">
    <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
        <input type="button" id="btnObtenerFormaAgregarBanco" value="+ Agregar banco" class="buttonLTR" />
    </div>
    <div id="divGridBanco" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <table id="grdBanco"></table>
            <div id="pagBanco"></div>
        </div>
    </div>
</div>
</asp:Content>
