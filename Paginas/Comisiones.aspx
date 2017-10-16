<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Comisiones.aspx.cs" Inherits="Comisiones" Title="Comisiones" %>
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
    
    <!--Archivos-->
    <script type="text/javascript" src="../js/Reportes.Comisiones.js?_=<%=DateTime.Now.Ticks %>"></script>
    
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	
	<div style="padding:10px;">
		<table>
			<tbody>
				<tr>
					<td><strong>Fecha Inicial:</strong></td>
					<td><strong>Fecha Final:</strong></td>
					<td><strong>Sucursal:</strong></td>
					<td><strong>Agente:</strong></td>
					<td></td>
				</tr>
				<tr>
					<td>
						<input type="text" id="txtFechaInicial" style="width:90px;" value="<%=DateTime.Now.AddDays(-30).ToShortDateString() %>" />
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
						<button id="btnComisiones" class="buttonLTR">Actualizar</button>
					</td>
				</tr>
			</tbody>
		</table>
	</div>

	<div id="tabsComisiones" style="margin:0px 10px 10px 10px;">
		<ul>
			<li><a href="#divUtilidades">Utilidades</a></li>
			<li><a href="#divUtilidadesFacturas">Facturas Utilidades</a></li>
			<li><a href="#divDetalleUtilidades">Partidas Utilidades</a></li>
			<li><a href="#divFacturasSinCosto">Facturas Sin Costos</a></li>
			<li><a href="#divPartidasSinCosto">Partidas Sin Costos</a></li>
		</ul>
		<div id="divUtilidades" style="height:350px;overflow: auto;">
			<button id="exportarUtlidades" class="buttonLTR">Exportar</button>
			<div class="reporte" style="padding-top:10px;"></div>
		</div>
		<div id="divUtilidadesFacturas" style="height:350px;overflow: auto;">
			<button id="exportarUtlidadesFacturas" class="buttonLTR">Exportar</button>
			<div class="reporte" style="padding-top:10px;"></div>
		</div>
		<div id="divDetalleUtilidades" style="height:350px;overflow: auto;">
			<button id="exportarUtlidadesDetalle" class="buttonLTR">Exportar</button>
			<div class="reporte" style="padding-top:10px;"></div>
		</div>
		<div id="divFacturasSinCosto" style="height:350px;overflow: auto;">
			<button id="exportarFacturasSinCostoe" class="buttonLTR">Exportar</button>
			<div class="reporte" style="padding-top:10px;"></div>
		</div>
		<div id="divPartidasSinCosto" style="height:350px;overflow: auto;">
			<button id="exportarPartidasSinCosto" class="buttonLTR">Exportar</button>
			<div class="reporte" style="padding-top:10px;"></div>
		</div>
	</div>

</asp:Content>