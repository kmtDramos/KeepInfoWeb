<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="IngresosNoDepositados.aspx.cs" Inherits="IngresosNoDepositados" Title="Ingresos no depositados" %>
<asp:Content ID="headIngresosNoDepositados" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.IngresosNoDepositados.js"></script>
</asp:Content>
<asp:Content ID="bodyIngresosNoDepositados" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarIngresosNoDepositados" title ="Agregar ingreso no depositado"></div>
<div id="dialogConsultarIngresosNoDepositados" title ="Consultar ingreso no depositado"></div>
<div id="dialogEditarIngresosNoDepositados" title ="Editar ingreso no depositado"></div>
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
    <div id="divFiltrosIngresosNoDepositados"></div>
    <div class="divAreaBotonesDialog">
        <%= puedeAgregarIngresosNoDepositados == 1 ? "<input type='button' id='btnObtenerFormaAgregarIngresosNoDepositados' value='+ Agregar ingreso no depositado' class='buttonLTR'/>" : ""%>
    </div>    
    <div id="divGridIngresosNoDepositados" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE CUENTAS POR COBRAR-->
            <table id="grdIngresosNoDepositados"></table>
            <div id="pagIngresosNoDepositados"></div>
            <!--FIN DE GRID DE CUENTAS POR COBRAR-->
        </div>
    </div>
</div>
</asp:Content>
