﻿<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteEstadoCrediticioProveedores.aspx.cs" Inherits="ReporteEstadoCrediticioProveedores" Title ="Reporte estado crediticio de proveedores" %>
<asp:Content ID="headReportesEstadoCrediticioProveedores" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    
    <!--jQuery-->
    <script type="text/javascript"  src="../js/Reportes.ReportesEstadoCrediticioProveedores.js"></script>
    
</asp:Content>

<asp:Content ID="bodyCatalogoReportesKeep" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
   <!--Dialogs-->
   <div id="dialogConsultarProyecto" title ="Consultar proyecto"></div>
    <div id="divContenido">
        <div id="divFiltrosReporteEstadoCrediticioProveedores"></div>
        <br />
        <div id="divEstadoCrediticioProveedores"></div>
    </div>     
    </asp:Content>