<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReportesKeep.aspx.cs" Inherits="ReportesKeep" Title ="Reportes" %>
<asp:Content ID="headCatalogoReportesKeep" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <script type="text/javascript" src="../js/jquery.multiselect.es.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Reportes.ReportesKeep.js"></script>
</asp:Content>

<asp:Content ID="bodyCatalogoReportesKeep" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
   <!--Dialogs-->
    <div id="dialogReporteKeep" title ="Reporte"></div>
    <div id="dialogFiltrosReportesKeep" title ="Filtros"></div>
    <div id="divContenido">
        <div id="divGridReportesKeep" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE REPORTES-->
            <table id="grdReportesKeep"></table>
            <div id="pagReportesKeep"></div>
            <!--FIN DE GRID DE REPORTES-->
        </div>
    </div>     
        <%--<div id="divGridCotizacion" class="divContGrid renglon-bottom">
            <div id="divContGrid">                
                <table id="grdlist"></table>
                <div id="paglist"></div>
            </div>
        </div>--%>
        
    </div>
</asp:Content>