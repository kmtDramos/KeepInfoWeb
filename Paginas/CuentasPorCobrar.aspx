<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CuentasPorCobrar.aspx.cs" Inherits="CuentasPorCobrar" Title="Ingresos" %>
<asp:Content ID="headCuentasPorCobrar" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.CuentasPorCobrar.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyCuentasPorCobrar" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarCuentasPorCobrar" title ="Agregar cobro"></div>
<div id="dialogConsultarCuentasPorCobrar" title ="Consultar cobro"></div>
<div id="dialogEditarCuentasPorCobrar" title ="Editar cobro"></div>
<div id="dialogConciliarCuentasPorCobrar" title ="Conciliar ingresos"></div>
<div id="dialogDatosFiscales" title ="Datos fiscales"></div>
<div id="dialogMuestraCuentasBancarias" title ="Cuentas bancarias">
    <div id="divFormaCuentaBancaria"></div>
    <div id="divGridCuentaBancaria" class="divContGrid renglon-bottom">
        <div id="divContGridCuentaBancaria">
            <!--INICIO GRID CUENTAS BANCARIAS-->
            <table id="grdCuentaBancaria"></table>
            <div id="pagCuentaBancaria"></div>
            <!--FIN DE GRID CUENTAS BANCARIAS-->
        </div>
    </div>
</div>

<div id="dialogFacturaFormato" title ="Documento"></div>

<div id="dialogMuestraAsociarDocumentos" title ="Facturas">
    <div id="divFormaAsociarDocumentosF"></div>
    
    <div id="divGridAsociarDocumentos" class="divContGrid renglon-bottom">
        <div id="divContGridAsociarDocumento">
            <!--INICIO GRID DOCUMENTOS-->
            <table id="grdFacturas"></table>
            <div id="pagFacturas"></div>
            <!--FIN DE GRID DOCUMENTOS-->
        </div>
    </div>
    
    <div id="divGridMovimientosCobros" class="divContGrid renglon-bottom">
        <div id="divContGridMovimientosCobros">
            <!--INICIO GRID MOVIMIENTOS DE COBROS-->
            <table id="grdMovimientosCobros"></table>
            <div id="pagMovimientosCobros"></div>
            <!--FIN DE GRID MOVIMIENTOS DE COBROS-->
        </div>
    </div>
    
</div>

<div id="divContenido">
    <div id="divFiltrosCuentasPorCobrar"></div>
    <div class="divAreaBotonesDialog">
        <%= puedeAgregarCuentasPorCobrar == 1 ? "<input type='button' id='btnObtenerFormaAgregarCuentasPorCobrar' value='+ Agregar cobro' class='buttonLTR'/>" : ""%>
        <%= puedeConciliarCuentasPorCobrar == 1 ? "<input type='button' id='btnObtenerFormaConciliarCuentasPorCobrar' value='+ Conciliar ingresos' class='buttonLTR'/>" : ""%>
    </div>    
    <div id="divGridCuentasPorCobrar" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE CUENTAS POR COBRAR-->
            <table id="grdCuentasPorCobrar"></table>
            <div id="pagCuentasPorCobrar"></div>
            <!--FIN DE GRID DE CUENTAS POR COBRAR-->
        </div>
    </div>
</div>
</asp:Content>
