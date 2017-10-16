<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Oportunidad.aspx.cs" Inherits="Oportunidad" Title="Oportunidad" %>
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
    <script type="text/javascript" src="../js/Operacion.Actividad.js?_=<% Response.Write(ticks); %>"></script>
    
    <!--Oportunidades-->
    <script type="text/javascript" src="../js/vertical-tabs.js"></script>
    <script type="text/javascript" src="../js/Catalogo.Oportunidad.js?_=<% Response.Write(ticks); %>"></script>
    
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarOportunidad" title="Agregar oportunidad"></div>
    <div id="dialogConsultarOportunidad" title="Consultar oportunidad"></div>
    <div id="dialogComentariosOportunidad" title="Comentarios oportunidad"></div>
    <div id="dialogArchivoOportunidad" title="Archivo oportunidad"></div>
    <div id="dialogEditarOportunidad" title="Editar oportunidad"></div>
    <div id="dialogAgregarMotivoCancelacionOportunidad" title="Motivo cancelación"></div>
    <div id="dialogMuestraCuentasBancarias" title ="Cuentas bancarias">
    <div id="divFormaCuentaBancaria"></div>
    <!-- Dialog Reporte Comisiones -->
    <div id="dialogReporteComision" title="Reporte comisiones"></div>
    <div id="dialogClientesOportunidades" title="Oportunidades/Clientes"></div>
    <div id="dialogFiltroReporteComision" title="Filtros comisiones"></div>
    <div id="dialogGraficasOportunidades" title="Gráficas de funnel"></div>
    <div id="dialogGraficaResultadosUltimosMeses" title="Resultado Ultimos 6 meses"></div>
    <div id="dialogGraficaMesPasado" title="Resultado de Ventas por Division del Mes de <% Response.Write(mesAnterior); %>"></div>
    <div id="dialogGraficaMesActual" title="Resultado de Ventas por Division del Mes de <% Response.Write(mesActual); %>"></div>
</div>
    <div id="divContenido">
        <div id="divLeyenda" style="margin-top:5px;">Metas:</div>
        <div id="divMetaUsuario" style="margin-top:5px; width:100%;">
            <table cellspacing="0" cellpadding="5" style="border:3px solid #777;border-radius:3px;color:#fff;width:100%;font-size:11px;font-weight:bold;">
                <tbody>
                    <tr style="background-color:#0AC92A;">
                        <th width="22%">Alcance 1: <span id="lblAlcance1"></span></th>
                        <th width="22%">Alcance 2: <span id="lblAlcance2"></span></th>
                        <th width="22%">Meta: <span id="lblMeta"></span></th>
                        <th width="22%">Clientes nuevos: <span id="lblClientesNuevos"></span></th>
                    </tr>
					<tr>
						<th colspan="4" style="background-color: #0AC92A;">
							<table cellspacing="0" cellpadding="0" width="100%">
								<tbody>
									<tr>
										<td>Monterrey</td>
										<td>México</td>
										<td>Guadalajara</td>
									</tr>
									<tr>
										<td><span>Meta:</span> <span id="metaMty">$0</span></td>
										<td><span id="metaMex">$0</span></td>
										<td><span id="metaGdl">$0</span></td>
									</tr>
									<tr>
										<td><span>Logro:</span> <span id="logroMty">$0</span></td>
										<td><span id="logroMex">$0</span></td>
										<td><span id="logroGdl">$0</span></td>
									</tr>
									<tr>
										<td><span>Hoy:</span><span id="hoyMty">$0</span></td>
										<td><span id="hoyMex">$0</span></td>
										<td><span id="hoyGdl">$0</span></td>
									</tr>
								</tbody>
							</table>
						</th>
					</tr>
                    <tr style="background-color:#8c8c8c;">
                        <th colspan="4">
                            <span style="font-size:11px;" title="Resultado de Ventas menos Cancelaciones menos Notas de Crédito del Mes de <% Response.Write(mesActual); %>">
                                Resultado de Ventas de <% Response.Write(mesActual); %>
                            </span>
                            <span id="lblResultado">0</span>/<span id="lblClientes">0</span>
                            <img src="../Images/pie-chart.png" id="pieChartMesActual" style="cursor:pointer;"/>
                            <span id="lblAvanceVenta">0%</span> Avance de meta |
							<span id="lblAvanceNecesario">0%</span> /
							<span id="lblMetaNecesario">$0.00</span> Avance planeado
                        </th>
                    </tr>
                    <tr style="font-size:11px; background-color:#cccccc;">
                        <th colspan="4">
                            <span style="font-size:11px;" title="Resultado de Ventas menos Cancelaciones menos Notas de Crédito del Mes de <% Response.Write(mesAnterior); %>">
                                Resultado de Ventas de <% Response.Write(mesAnterior); %>
                            </span>
                            <span id="lblResAnterior">0</span>/<span id="lblCliAnterior">0</span>
                            <img src="../Images/pie-chart.png" id="pieChartMesAnterior" style="cursor:pointer;"/>
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
        
        <div id="divSeleccionFechaAlcances">
            <input type="hidden" id="txtFechaInicioAlcance" value="<% Response.Write(FechaInicio); %>" />
            <input type="hidden" id="txtFechaFinAlcance" value="<% Response.Write(FechaFinal); %>" />
        </div>
        <div id="divSeleccionFechaResultado">
            <input type="hidden" id="txtFechaInicio" value="<% Response.Write(FechaInicio); %>" />
            <input type="hidden" id="txtFechaFinal" value="<% Response.Write(FechaFinal); %>" />
        </div>
        <div id="divSeleccionFechaResAnterior">
            <input type="hidden" id="txtFechaInicio2" value="<% Response.Write(FechaInicio2); %>" />
            <input type="hidden" id="txtFechaFinal2" value="<% Response.Write(FechaFinal2); %>" />
        </div>
        <div id="div1">
            <input type="hidden" id="txtFechaInicio3" value="<% Response.Write(FechaInicio3); %>" />
            <input type="hidden" id="txtFechaFinal3" value="<% Response.Write(FechaFinal3); %>" />
        </div>
        <div id="cvsDatosNivelesInteres" style="margin-top:5px; font-weight:bold;">
            <tbody>
        <table width="100%">
                <tr>
                    <td width="20%">Total en pesos:</td>
                    <td width="20%">N.I. Bajo: <span id="lblNivelInteresBajo"></span></td>
                    <td width="20%">N.I. Medio: <span id="lblNivelInteresMedio"></span></td>
                    <td width="20%">N.I. Alto: <span id="lblNivelInteresAlto"></span></td>
                    <td width="20%">Pronostico: <span id="lblPronostico"></span></td>
                </tr>
            </tbody>
        </table>
        </div>
        <div id="divOportunidadTotales" style="margin-top:5px; width:100%;">
            <table style="background-color:#8c8c8c; border:3px solid #6b6b6b;border-radius:3px; color:#fff; width:100%; font-weight:bold;" >
                <tbody>
                    <tr>
                        <th width="12%">Opts/Cts/Acts</th>
                        <th width="12%">Oportunidades</th>
                        <th width="12%">Cotizaciones</th>
                        <th width="12%">Pedidos</th>
                        <th width="12%">Proyectos</th>
                        <th width="12%">Facturado</th>
                        <th width="12%">Monto Real</th>
                    </tr>
                    <tr style="font-size:14px;">
                        <th height="20">
                            <span id="lblOportunidadesClientes">
                                <span id="sp-total">0</span>/<span id="sp-total-clientes">0</span>/<span id="sp-total-actividades">0</span>
                            </span>
                        </th>
                        <th><span id="sp-monto">0</span></th>
                        <th><span id="sp-cotizciones">0</span></th>
                        <th><span id="sp-pedidos">0</span></th>
                        <th><span id="sp-proyectos">0</span></th>
                        <th><span id="sp-facturado">0</span></th>
                        <th><span id="sp-real">0</span></th>
                    </tr>
                </tbody>
            </table>
        </div>
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <%= puedeAgregarOportunidad == 1 ? "<input type='button' id='btnObtenerFormaAgregarOportunidad' value='+ Agregar oportunidad' class='buttonLTR' />" : "" %>
            <input type="button" onclick="Javascript:window.open('../AspClassic/ArchivosMercadotecnia.asp');" value="Descargar archivos" class="buttonLTR">
            <%= puedeConsultarReporteComisiones == 1 ? "<input type='button' id='btnObtenerFormaReporteComisiones' value='Reporte de comisiones' class='buttonLTR' />" : ""%>
            <input type='button' id='btnVerGraficasOportunidad' value='Ver gráficas de oportunidades' class='buttonLTR'/>
            <!--<input type='button' id='btnReporteSeisMeses' value='Resultado ultimos 6 meses' class='buttonLTR'/>-->
            <input type="button" value="+ Agregar actividad" class="buttonLTR btnAbrirFormaAgregarActividad" />
            <input type="button" value="Abrir agenda" class="buttonLTR btnAbrirAgendaActividades" />
        </div>
        <div id="divGridOportunidad" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Oportunidad-->
                <table id="grdOportunidad"></table>
                <div id="pagOportunidad"></div>
                <!--FIN DE GRID DE Oportunidad-->
            </div>
        </div>
    </div>
</asp:Content>