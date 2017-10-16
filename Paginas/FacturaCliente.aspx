<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="FacturaCliente.aspx.cs" Inherits="FacturaCliente" Title="Facturación" %>
<asp:Content ID="headFacturaCliente" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
     <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">	
    <script src="../js/upload/fileuploader.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../js/Operacion.FacturaCliente.js?_=<%=DateTime.Now.Ticks %>>"></script>
</asp:Content>
<asp:Content ID="bodyFacturaCliente" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarFactura" title="Agregar factura"></div>
    <div id="dialogConsultarFacturaEncabezado" title="Consultar factura"></div>
    <div id="dialogEditarFacturaEncabezado" title="Editar factura"></div>
    <div id="dialogDatosFiscalesFactura" title ="Datos fiscales"></div>
    <div id="dialogMotivoCancelacionFactura" title ="Motivo de cancelación"></div>
    <div id="dialogFacturasSustituye" title ="Facturas que sustituye"></div>
    <div id="dialogFacturaFormato" title ="Documento"></div>
    <div id="dialogFacturaAddenda" title ="Addenda"></div>
    <div id="dialogEditarDescripcionPartida" title="Editar descripcion de partida"></div>
    <!--Dialogs-->
    <div id="divContenido">
        <div id="divFiltrosFacturaCliente"></div>
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">            
            <div class="divAreaBotonesDialog">
                <%= puedeAgregarFacturaEncabezado == 1 ? "<input type='button' id='btnObtenerFormaAgregarFactura' value='+ Agregar factura' class='buttonLTR'/>" : ""%>
				<button class="buttonLTR" id="btnVerDetalle">Ver Detalle</button>
            </div>
        </div>
        <div id="divGridFacturas" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE FACTURAS-->
                <table id="grdFacturas"></table>
                <div id="pagFacturas"></div>
                <!--FIN DE GRID DE FACTURAS-->
            </div>
        </div>
    </div>
    <asp:Button ID="btnDescarga" UseSubmitBehavior="false" style="visibility: hidden; display: none;" Text="Descarga" Font-Bold="true" runat="server" OnClick="btnDescarga_Click" />
</asp:Content>