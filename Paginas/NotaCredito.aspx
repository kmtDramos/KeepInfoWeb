<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="NotaCredito.aspx.cs" Inherits="NotaCredito" Title="Nota de crédito" %>
<asp:Content ID="headNotaCredito" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.NotaCredito.js?_=<% Response.Write(DateTime.Now.Ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyNotaCredito" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarNotaCredito" title ="Agregar nota de crédito"></div>
<div id="dialogAgregarNotaCreditoDevolucionCancelacion" title ="Agregar nota de crédito"></div>
<div id="dialogConsultarNotaCredito" title ="Consultar nota de crédito"></div>
<div id="dialogEditarNotaCredito" title ="Editar nota de crédito"></div>
<div id="dialogEditarNotaCreditoDevolucionCancelacion" title ="Editar nota de crédito"></div>
<div id="dialogDatosFiscales" title ="Datos fiscales"></div>
<div id="dialogMotivoCancelacion" title ="Motivo de cancelación"></div>
<div id="dialogSeleccionarTipoNotaCredito" title ="Tipo de nota de crédito"></div>
<div id="dialogMuestraAsociarProductosDevolucionCancelacion" title ="Asociar productos">
     <div id="divGridProductosNotaCreditoDevolucionCancelacion" class="divContGrid renglon-bottom">
                    <div id="divContGridProductosNotaCreditoDevolucionCancelacion">
                        <!--INICIO GRID PRODUCTOS-->
                        <table id="grdProductosNotaCreditoDevolucionCancelacion"></table>
                        <div id="pagProductosNotaCreditoDevolucionCancelacion"></div>
                        <!--FIN DE GRID PRODUCTOS-->
                    </div>
      </div>
</div>
<div id="dialogFacturaFormato" title ="Documento"></div>

<div id="dialogMuestraProductosAsociados" title ="Eliminar productos asociados">
     <div id="divGridProductosAsociadosNotaCreditoDevolucionCancelacion" class="divContGrid renglon-bottom">
                    <div id="div3">
                        <!--INICIO GRID PRODUCTOSASOCIADOS-->
                        <table id="grdProductosAsociadosNotaCreditoDevolucionCancelacion"></table>
                        <div id="pagProductosAsociadosNotaCreditoDevolucionCancelacion"></div>
                        <!--FIN DE GRID PRODUCTOSASOCIADOS-->
                    </div>
      </div>
</div>
<div id="divFormaAsociarProductosDevolucionCancelacion" title ="Asociar productos">
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
<div id="dialogAgregarProductosCantidades" title ="Cantidades de productos a devolver" idFacturaDetalle="${IdFacturaDetalle}">
    <label style="font-weight:bold">Cantidad facturada:</label><span id="Cantidad"></span><br /><br />
    <label style="font-weight:bold">Cantidad disponible:</label> <span id="CantidadDisponible"></span><br /><br />
    <label style="font-weight:bold">Cantidad a devolver:</label><input type="text" id="txtCantidadDevolver" size="20"  onkeypress="javascript:return ValidarNumero(event, this.value);"  />

</div>

<div id="divContenido">
    <div id="divFiltrosNotaCredito"></div>
    <div class="divAreaBotonesDialog">
        <%= puedeAgregarNotaCredito == 1 ? "<input type='button' id='btnObtenerFormaAgregarNotaCredito' value='+ Agregar nota de crédito' class='buttonLTR'/>" : ""%>
        <input type="text" id="txtFacturasSeleccionadas" style="display:none;"/>
        <input type="text" id="txtProductosSeleccionados" style="display:none;"/>
    </div>
    <div id="divGridNotaCredito" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE NOTA DE CREDITO-->
            <table id="grdNotaCredito"></table>
            <div id="pagNotaCredito"></div>
            <!--FIN DE GRID DE NOTA DE CREDITO-->
        </div>
    </div>
</div>
 
</asp:Content>
