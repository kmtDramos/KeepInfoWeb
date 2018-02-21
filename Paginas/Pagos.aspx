<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Pagos.aspx.cs" Inherits="Pagos" Title="Complemento de Pagos" %>
<asp:Content ID="headPagos" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Pagos.js?_=<% Response.Write(ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyPagos" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogCrearComplementoPago" title ="Complemento de Pago"></div>
<div id="dialogConsultarPago" title ="Consultar cobro"></div>

<div id="divContenido">
    <div id="divFiltrosPagos"></div>
    <div class="divAreaBotonesDialog">
        <%= puedeCrearComplementoPago == 1 ? "<input type='button' id='btnObtenerFormaCrearComplementoPago' value='Crear Complemento de Pago' class='buttonLTR'/>" : ""%>
    </div>    
    <div id="divGridCuentasPorCobrar" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE PAGOS-->
            <table id="grdPagos"></table>
            <div id="pagPagos"></div>
            <!--FIN DE GRID DE PAGOS-->
        </div>
    </div>
</div>
</asp:Content>
