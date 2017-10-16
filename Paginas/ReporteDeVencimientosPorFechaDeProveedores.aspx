<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteDeVencimientosPorFechaDeProveedores.aspx.cs" Inherits="ReporteDeVencimientosPorFechaDeProveedores" Title ="Reporte de vencimientos por fecha de proveedores" %>
<asp:Content ID="headReporteDeVencimientosPorFechaDeProveedores" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Reportes.ReportesDeVencimientosPorFechaDeProveedores.js"></script>
    
</asp:Content>

<asp:Content ID="bodyReporteDeVencimientosPorFechaDeProveedores" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
   <!--Dialogs-->
    <div id="divContenido">
        <div id="divFiltrosReporteVencimientosPorFechaDeProveedores" idProveedor="0"></div>
        <br />
        <div id="divReporteDeProveedoresDeVencimientosPorFecha" idProveedor="0"></div>       
    </div>     
    
</asp:Content>