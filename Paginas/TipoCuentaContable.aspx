<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoCuentaContable.aspx.cs" Inherits="TipoCuentaContable" %>
<asp:Content ID="headCatalogoTipoCuentaContable" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoCuentaContable.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoCuentaContable" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarTipoCuentaContable" title="Agregar tipo de cuenta contable"></div>
    <div id="dialogConsultarTipoCuentaContable" title="Consultar tipo de cuenta contable"></div>
    <div id="dialogEditarTipoCuentaContable" title="Editar tipo de cuenta contable"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoCuentaContable" value="+ Agregar tipo de cuenta contable" class="buttonLTR" />
        </div>
        <div id="divGridTipoCuentaContable" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TIPO DE CUENTA CONTABLE-->
                <table id="grdTipoCuentaContable"></table>
                <div id="pagTipoCuentaContable"></div>
                <!--FIN DE GRID DE TIPO DE CUENTA CONTABLE-->
            </div>
        </div>
    </div>
</asp:Content>
