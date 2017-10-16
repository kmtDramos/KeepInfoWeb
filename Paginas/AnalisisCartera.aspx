<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="AnalisisCartera.aspx.cs" Inherits="AnalisisCartera" Title="Análisis de Cartera" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">
    
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <script type="text/javascript" src="../js/upload/fileuploader.js"></script>
    
    <!--jQuery-->
    <script type="text/javascript" src="../js/JSON/json2.js"></script>
    
    <!--Oportunidades-->
    <script type="text/javascript" src="../js/Catalogo.AnalisisCartera.js?_=<% Response.Write(DateTime.Now.Ticks); %>"></script>
    
</asp:Content>
<asp:Content ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido" style="padding:10px;">
		<table>
			<tbody>
				<tr>
					<td>Fecha inicio</td>
					<td>Fecha fin:</td>
				</tr>
				<tr>
					<td><input type="text" id="txtFechaInicial" value="01/01/<%=DateTime.Today.Year %>" /></td>
					<td><input type="text" id="txtFechaFinal" value="<%=DateTime.Today.ToShortDateString() %>"</td>
				</tr>
			</tbody>
		</table>
        <div id="divGridAnalisisCartera" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Oportunidad-->
                <table id="grdAnalisisCartera"></table>
                <div id="pagAnalisisCartera"></div>
                <!--FIN DE GRID DE Oportunidad-->
            </div>
        </div>
    </div>
</asp:Content>