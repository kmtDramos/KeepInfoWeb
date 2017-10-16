<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="MetodoPago.aspx.cs" Inherits="MetodoPago" Title="Página sin título" %>
<asp:Content ID="headMetodoPago" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.MetodoPago.js"></script>
</asp:Content>
<asp:Content ID="bodyMetodoPago" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarMetodoPago" title="Agregar tipo metodo de pago"></div>
    <div id="dialogConsultarMetodoPago" title="Consultar tipo metodo de pago"></div>
    <div id="dialogEditarMetodoPago" title="Editar tipo metodo de pago"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarMetodoPago" value="+ Agregar tipo metodo de pago" class="buttonLTR" />
        </div>
        <div id="divGridMetodoPago" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MetodoPago-->
                <table id="grdMetodoPago"></table>
                <div id="pagMetodoPago"></div>
                <!--FIN DE GRID DE MetodoPago-->
            </div>
        </div>
    </div>
</asp:Content>
