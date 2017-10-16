<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CondicionPago.aspx.cs" Inherits="CondicionPago" Title="Condiciones de pago" %>
<asp:Content ID="headCondicionPago" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.CondicionPago.js"></script>
</asp:Content>
<asp:Content ID="bodyCondicionPago" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarCondicionPago" title ="Agregar condición de pago"></div>
<div id="dialogConsultarCondicionPago" title ="Consultar condición de pago"></div>
<div id="dialogEditarCondicionPago" title ="Editar condición de pago"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarCondicionPago" value="+ Agregar condición de pago" class="buttonLTR" />
    </div>
    <div id="divGridCondicionPago" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE MEDIO ENTERO-->
            <table id="grdCondicionPago"></table>
            <div id="pagCondicionPago"></div>
            <!--FIN DE GRID DE MEDIO ENTERO-->
        </div>
    </div>
</div>
</asp:Content>
