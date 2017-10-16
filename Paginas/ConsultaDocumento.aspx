<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ConsultaDocumento.aspx.cs" Inherits="ConsultaDocumento" Title="Consulta de Documentos" %>
<asp:Content ID="headConsultaDocumento" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.ConsultaDocumento.js"></script>
</asp:Content>
<asp:Content ID="bodyConsultaDocumento" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido">

    <div class="divAreaBotonesDialog">        
    </div>
    
    <div style="border:1px solid #999; padding:4px; margin:4px 0; border-radius:4px;">
        <table>
            <tr>
                <td>
                    <strong>Fecha inicial:</strong>
                </td>
                <td>
                    <strong>Fecha final:</strong>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="text" id="txtFechaInicial" value="<% Response.Write(FechaInicial);%>" />
                </td>
                <td>
                    <input type="text" id="txtFechaFinal" value="<% Response.Write(FechaFinal);%>" />
                </td>
            </tr>
        </table>
    </div>
    <script>
        if ($("#txtFechaInicial").length) {
            $("#txtFechaInicial").datepicker({
                maxDate: (0)
//                onSelect: function() {
////                    FiltroFacturas();
//                }
            });

            var fechainicio = $("#txtFechaInicial").val();
            
        }

        if ($("#txtFechaFinal").length) {
            $("#txtFechaFinal").datepicker({
            maxDate: (0)
//                onSelect: function() {
////                    FiltroFacturas();
//                }
            });
        }  
    </script>
      
    <div id="divGridFactura" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--grid FACTURA-->
            <table id="grdFactura"></table>
            <div id="pagFactura"></div>
            <!--grid FACTURA-->
        </div>
    </div>
    
    <div id="divGridFacturaDetalleCliente" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--grid FACTURA DETALLE-->
            <table id="grdFacturaDetalleCliente"></table>
            <div id="pagFacturaDetalleCliente"></div>
            <!--grid FACTURA DETALLE-->
        </div>
    </div> 
    
    <div id="divGridRemision" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--grid REMISION-->
            <table id="grdRemision"></table>
            <div id="pagRemision"></div>
            <!--grid REMISION-->
        </div>
    </div>
    <div id="divGridDetalleFactura" class="divContGrid renglon-bottom">
        <div id="divConGrid">
            <table id="grdDetalleFactura"></table>
            <div id="pagDetalleFactura"></div>
        </div>
    </div>
    <div id="divGridFacturaProveedor" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--grid REMISION-->
            <table id="grdFacturaProveedor"></table>
            <div id="pagFacturaProveedor"></div>
            <!--grid REMISION-->
        </div>
    </div>
    
    <div id="divGridPedido" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--grid REMISION-->
            <table id="grdPedido"></table>
            <div id="pagPedido"></div>
            <!--grid REMISION-->
        </div>
    </div>
    
</div>
</asp:Content>