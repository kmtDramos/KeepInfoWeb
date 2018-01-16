<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Indicadores.aspx.cs" Inherits="Paginas_Indicadores" Title="Reporte seguimiento de ventas (funnel)" %>
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
    <link rel="Stylesheet" type="text/css" href="../CSS/actividad.css" />
    <script type="text/javascript" src="../js/datetimepicker.js"></script>
    <script type="text/javascript" src="../js/moment.min.js"></script>
    <link rel="Stylesheet" type="text/css" href="../CSS/fullcalendar.css" />
    <script type="text/javascript" src="../js/fullcalendar.min.js"></script>
    <script type="text/javascript" src="../js/lang-all.js"></script>
    <script type="text/javascript" src="../js/Ventas.Indicadores.js?_=<%=DateTime.Now.Ticks %>"></script>
    
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div style="min-height:450px;padding:10px;">
		<table width="100%">
			<tr>
				<td valign="top" style="border-right:1px solid #999;height:200px;width:200px;">
					<table>
						<tr>
							<td>Agente:</td>
							<td>
								<select id="cmbAgente" style="width:100%;">
									<option value="-1">-Todos-</option>
								</select>
							</td>
						</tr>
						<tr>
							<td>Sucursal:</td>
							<td>
								<select id="cmbSucursal" style="width:100%;">
									<option value="-1">-Todas-</option>
								</select>
							</td>
						</tr>
						<tr>
							<td>Familia</td>
							<td>
								<select id="cmbDivision" style="width:100%;">
									<option value="-1">-Todas-</option>
								</select>
							</td>
						</tr>
						<tr>
						<tr>
							<td></td>
							<td>
								<button id="btnBuscar" class="buttonLTR" onclick="ObtenerKPIs();">Buscar</button>
							</td>
						</tr>
					</table>
				</td>
				<td>
					<table>
						<tr>
							<td align="center" valign="midle" width="25%">
								<div id="cnvPresentacion" style="height:200px;width:380px;"></div>
							</td>
							<td align="center" valign="midle" width="25%">
								<div id="cnvPreventa" style="height:200px;width:380px;"></div>
							</td>
						</tr>
						<tr>
							<td align="center" valign="midle" width="25%">
								<div id="cnvMeta" style="height:200px;width:380px;"></div>
							</td>
							<td align="center" valign="midle" width="25%">
								<div id="cnvPlaneacion" style="height:200px;width:380px;"></div>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<hr />
		<div id="divTabla"></div>
	</div>
</asp:Content>