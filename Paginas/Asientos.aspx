<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Asientos.aspx.cs" Inherits="Asientos" Title="Asientos" %>
<asp:Content ID="headAsientos" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/GestorEmpresarial.Asientos.js"></script>
</asp:Content>
<asp:Content ID="bodyAsientos" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogRevisarAsientosPendientes" title="Revisar asientos pendientes"></div>
    <div id="dialogConsultarFacturaEncabezado" title="Factura de cliente"></div>
    <div id="dialogConsultarEncabezadoFacturaProveedor" title="Factura de proveedor"></div>
    <div id="dialogConsultarCuentasPorCobrar" title="Cobro al cliente"></div>
    <div id="dialogConsultarDepositos" title="Cobro al cliente"></div>
    <div id="dialogConsultarEgresos" title="Pago a proveedor"></div>
    <div id="dialogConsultarCheques" title="Pago a proveedor"></div>
    <div id="dialogEditarCuentaMovimientosFacturaCliente" title="Editar cuenta de movimientos"></div>
    <div id="dialogEditarCuentaMovimientosFacturaProveedor" title="Editar cuenta de movimientos"></div>
    <div id="dialogEditarCuentaMovimientosDetalleFacturaProveedor" title="Editar cuenta de movimientos"></div>
    <div id="dialogAgregarAsiento" title="Agregar asiento">¿Esta seguro de agregar el asiento?</div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaRevisarAsientosPendientes" value="Revisar asientos pendientes" class="buttonLTR" />
        </div>
        <div id="divGridAsientos" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE ASIENTOS-->
                <table id="grdAsientosContables"></table>
                <div id="pagAsientosContables"></div>
                <!--FIN DE GRID DE ASIENTOS-->
            </div>
        </div>
    </div>
</asp:Content>
