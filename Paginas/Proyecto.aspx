<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Proyecto.aspx.cs" Inherits="Proyecto" Title="Proyectos" %>
<asp:Content ID="headProyecto" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <link type="text/css" rel="stylesheet" href="../js/MultipleSelect/multiple-select.css" />
    <script type="text/javascript" src="../js/MultipleSelect/multiple-select.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Proyecto.js?_=<% Response.Write(DateTime.Now.Ticks); %>"></script>
    <!-- -->
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
	<style>
		.ms-choice{line-height:25px;height:25px;}
	</style>
</asp:Content>
<asp:Content ID="bodyProyecto" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogConsultarSolicitudesProyectos" title ="Solicitudes proyectos"></div>
    <div id="dialogConsultarSolicitudProyecto" title ="Solicitud proyecto"></div>
    <div id="dialogAgregarProyecto" title ="Agregar proyecto"></div>
    <div id="dialogAgregarConcepto" title ="Agregar concepto"></div>
    <div id="dialogAgregarConceptoProyectoEdicion" title ="Agregar concepto a la factura"></div>
    <div id="dialogAgregarConceptoSolicitudFactura" title ="Agregar concepto a la factura"></div>
    <div id="dialogConsultarProyecto" title ="Consultar proyecto"></div>
    <div id="dialogConsultarConcepto" title ="Agregar solicitud de factura"></div>
    <div id="dialogConsultarSolicitudFactura" title ="Solicitud de facturas"></div>
    <div id="dialogConsultarProyectoAEnrolar" title ="Proyecto existente a enrolar"></div>
    <div id="dialogEditarProyecto" title ="Editar proyecto"></div>
    <div id="dialogEditarConceptoSolicitudFactura" title="Editar concepto"></div>
    <div id="dialogEditarSolicitudFactura" title="Editar solicitud de factura"></div>
    <div id="dialogGraficasProyectos" title="Gráficas de proyectos"></div>
    <div id="divContenido">
        <div id="divMetricasProyecto" style="border:3px solid #666666; background-color:#999999; padding:3px; margin:5px 0px; border-radius:4px;">
            <div id="divResultadosProyectos" style="color:#FEFEFE;font-size:12px;">
                <table width="100%">
                    <tbody>
                        <tr>
                            <th>Total proyectos</th>
                            <th>Programado</th>
                            <th>Facturado</th>
                            <th>Cobrado</th>
                            <th>Por cobrar</th>
                            <th>Costo teorico</th>
                            <th>Costo real</th>
                            <th>Diferencia</th>
                        </tr>
                        <tr>
                            <th><span id="lblTotalProyecto">0</span></th>
                            <th><span id="lblProgramado">$0.00</span></th>
                            <th><span id="lblFacturado">$0.00</span></th>
                            <th><span id="lblCobrado">$0.00</span></th>
                            <th><span id="lblPorCobrar">$0.00</span></th>
                            <th><span id="lblCostoTeorico">$0.00</span></th>
                            <th><span id="lblCostoReal">$0.00</span></th>
                            <th><span id="lblDiferencia">$0.00</span></th>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="divAreaBotonesDialog">
			<table>
				<tbody>
					<tr>
						<td></td>
						<td></td>
						<!--<td></td>-->
						<td><lable>Estatus proyecto:</lable></td>
					</tr>
					<tr>
						<td>
							<%=btnObtenerFormaAgregarProyecto%>
						</td>
						<td>
							<input type="button" id="btnGraficasProyectos" value="Ver graficas proyectos" class="buttonLTR"/>
						</td><!--
						<td>
							<button id="btnSabanaProyectos" class="buttonLTR">Sabana proyectos</button>
						</td>-->
						<td>
							<select id="cmbEstatsuProyecto" style="width:130px;>"></select>
						</td>
                        <td>
                            <input type="button" id="btnVerSolicitudesProyectos" value="Solicitudes Proyecto" class="buttonLTR"/>
                        </td>
					</tr>
				</tbody>
			</table>
        </div>
        <div id="divGridProyecto" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE PROYECTOS-->
                <table id="grdProyecto"></table>
                <div id="pagProyecto"></div>
                <!--FIN DE GRID DE PROYECTOS-->
            </div>
        </div>
    </div>
</asp:Content>
