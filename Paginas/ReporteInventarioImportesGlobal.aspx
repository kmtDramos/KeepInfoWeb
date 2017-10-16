<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteInventarioImportesGlobal.aspx.cs" Inherits="ReporteInventarioImportesGlobal" %>
<asp:Content ID="headReporte" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Reportes.ReportesInventarioImportesGlobal.js"></script>
</asp:Content>
<asp:Content ID="bodyReporte" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido">
        <div id="divFiltrosReporteInventarioImportesGlobal" idAlmacen="0"></div> 
        <br />  
        <div id="divInventarioImportesGlobal" idAlmacen="0"></div>
    </div>
    <asp:Button ID="btnDescarga" UseSubmitBehavior="false" style="visibility: hidden; display: none;" Text="Descarga" Font-Bold="true" runat="server" OnClick="btnDescarga_Click" />   
</asp:Content>
