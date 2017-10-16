<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoCambioDiarioOficial.aspx.cs" Inherits="TipoCambioDiarioOficial" Title="Tipo de cambio del diario oficial" %>
<asp:Content ID="headTipoCambioDiarioOficial" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoCambioDiarioOficial.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyTipoCambioDiarioOficial" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarTipoCambioDiarioOficial" title="Agregar tipo de cambio diario oficial"></div>
    <div id="dialogConsultarTipoCambioDiarioOficial" title="Consultar tipo de cambio diario oficial"></div>
    <div id="dialogEditarTipoCambioDiarioOficial" title="Editar tipo de cambio diario oficial"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoCambioDiarioOficial" value="+ Agregar tipo de cambio diario oficial" class="buttonLTR" />
        </div>
        <div id="divGridTipoCambioDiarioOficial" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoCambioDiarioOficial-->
                <table id="grdTipoCambioDiarioOficial"></table>
                <div id="pagTipoCambioDiarioOficial"></div>
                <!--FIN DE GRID DE TipoCambioDiarioOficial-->
            </div>
        </div>
    </div>
</asp:Content>
