<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="GraficaAvanceVenta.aspx.cs" Inherits="GraficaAvanceVenta" Title="Grafica avance de meta" %>
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
    
    <!--jqPlot-->
    <link rel="stylesheet" type="text/css" href="../js/jqplot/dist/jquery.jqplot.min.css"/>
    <script src="../js/jqplot/excanvas.min.js" type="text/javascript" ></script>
    <script src="../js/jqplot/jquery.jqplot.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.barRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.meterGaugeRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.categoryAxisRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.highlighter.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.canvasTextRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.canvasAxisTickRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.canvasAxisLabelRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.pieRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.pointLabels.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.enhancedLegendRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.mekkoRenderer.min.js" type="text/javascript"></script>
    <script src="../js/jqplot/jqplot.funnelRenderer.min.js" type="text/javascript"></script>
    
    <!--Actividades-->
    <link rel="Stylesheet" type="text/css" href="../CSS/GraficaAvanceVenta.css" />
    
    <!--Oportunidades-->
    <script type="text/javascript" src="../js/Graficas.GraficaAvanceVenta.js?_=<%=DateTime.Now.Ticks %>"></script>
    
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div style="padding:10px;">
		<table>
			<tbody>
				<tr>
					<td><strong>Mostrar por</strong></td>
					<td><strong>Fecha Inicial:</strong></td>
					<td><strong>Fecha Final:</strong></td>
					<td><strong>Sucursal:</strong></td>
					<td><strong>Agente:</strong></td>
					<td></td>
				</tr>
				<tr>
					<td>
						<select id="cmbMostrarPor">
							<option value="1">Días</option>
							<option value="2">Semanas</option>
							<option value="3">Meses</option>
						</select>
					</td>
					<td>
						<input type="text" id="txtFechaInicial" style="width:90px;" value="<%=DateTime.Now.AddDays(-7).ToShortDateString() %>" />
					</td>
					<td>
						<input type="text" id="txtFechaFinal" style="width:90px;" value="<%=DateTime.Now.ToShortDateString() %>" />
					</td>
					<td>
						<select id="cmbSucursal" style="width:90px;"></select>
					</td>
					<td>
						<select id="cmbAgente" style="width:90px;"></select>
					</td>
					<td>
						<button id="btnGraficaVentaMeta" class="buttonLTR">Graficar</button>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<div id="divContenedorGraficaVentaMeta">
		<div id="tabsGraficasVentaMeta">
			<ul>
				<li><a href="#tabGraficaVentaMeta">Grafica de ventas</a></li>
			</ul>
			<div id="tabGraficaVentaMeta" style="padding:10px;">
				<div id="divGraficaVentaMeta" style="width:800px;"></div>
			</div>
		</div>
	</div>
</asp:Content>