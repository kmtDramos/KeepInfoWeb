<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="PlaneacionVentas.aspx.cs" Inherits="Paginas_PlaneacionVentas" Title="Planeación de venta"%>
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
    <script type="text/javascript" src="../js/datetimepicker.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-timepicker-addon.js"></script>

	<!--  -->
	<script type="text/javascript" src="../js/Ventas.PlaneacionVentas.js?_=<%=DateTime.Now.Ticks %>"></script>
	<style>
		input.monto{
			text-align: right;
		}
		input.underline{
			text-decoration: underline;
		}
	</style>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div style="padding:10px;">
		<div style="border:1px solid #BBB;background-color: #BBB;border-radius:4px;">
			<table width="100%" cellspacing="0">
				<tr>
					<th width="20%">Facturado</th>
					<th width="20%" style="background-color:#EEE;"><%=Mes1 %></th>
					<th width="20%">Plan de cierre</th>
					<th width="20%" style="background-color:#DDD;"><%=Mes2 %></th>
					<th width="20%" style="background-color:#CCC;"><%=Mes3 %></th>
				</tr>
				<tr>
					<th><span id="facturado">$0.00</span></th>
					<th style="background-color:#EEE;"><span id="Mes1">$0.00</span></th>
					<th><span id="plan">$0.00</span></th>
					<th style="background-color:#DDD;"><span id="Mes2">$0.00</span></th>
					<th style="background-color:#CCC;"><span id="Mes3">$0.00</span></th>
				</tr>
			</table>
		</div>
		<p style="text-align:center">Planeación de ventas del mes de <%=Mes1 %>.</p>
		<div style="border:1px solid #BBB;background-color: #BBB;border-radius:4px;">
			<table width="100%" cellspacing="0">
				<tr>
					<th width="33%">Monterrey</th>
					<th width="33%">México</th>
					<th width="33%">Guadalajara</th>
				</tr>
				<tr>
					<th><span id="mty">$0.00</span></th>
					<th><span id="mex">$0.00</span></th>
					<th><span id="gdl">$0.00</span></th>
				</tr>
			</table>
		</div>
		<p style="text-align:center">Total de oportunidades de mes de <%=Mes1 %> por resolver por departamento</p>
		<div style="border:1px solid #BBB;background-color: #BBB;border-radius:4px;">
			<table width="100%" cellspacing="0">
				<tr>
					<th width="20%">Preventa</th>
					<th width="20%">Ventas</th>
					<th width="20%">Compras</th>
					<th width="20%">Proyectos</th>
					<th width="20%">Finanzas</th>
				</tr>
				<tr>
					<th><span id="preventa">$0.00</span> / <span id="TotalPreventa">0</span></th>
					<th><span id="venta">$0.00</span> / <span id="TotalVenta">0</span></th>
					<th><span id="compras">$0.00</span> / <span id="TotalCompras">0</span></th>
					<th><span id="proyectos">$0.00</span> / <span id="TotalProyectos">0</span></th>
					<th><span id="finanzas">$0.00</span> / <span id="TotalFinanzas">0</span></th>
				</tr>
			</table>
		</div>
	</div>
	<div style="padding: 0px 20px;">
		<table>
			<tr>
				<td><lable>Sin planeación</lable></td>
				<td><lable>Con planeación en <%=Mes1 %></lable></td>
				<td>
					<table>
						<tr>
							<td></td>
							<td>
								Proyectos
							</td>
							<td>
								Pedidos
							</td>
							<td>
								Totales
							</td>
                            <td>
                                Para Cierre
                            </td>
						</tr>
						<tr>
							<td style="border-bottom:1px solid #999;border-right:1px solid #999;" align="right">Autrizados</td>
							<td style="border-bottom:1px solid #999;border-right:1px solid #999;" align="right"><span id="proyectosAutorizados">$0.00/0</span></td>
							<td style="border-bottom:1px solid #999;border-right:1px solid #999;" align="right"><span id="pedidosAutorizado">$0.00/0</span></td>
							<td style="border-bottom:1px solid #999;border-right:1px solid #999;" align="right"><span id="totalAutorizado">$0.00/0</span></td>
                            <td style="border-bottom:1px solid #999;" align="right"><span id="paraCierre">$0.00/0</span></td>
						</tr>
						<tr>
							<td style="border-right:1px solid #999;" align="right">Sin autorizar</td>
							<td style="border-right:1px solid #999;" align="right"><span id="proyectosSinAutorizar">$0.00/0</span></td>
							<td style="border-right:1px solid #999;" align="right"><span id="pedidosSinAutorizar">$0.00/0</span></td>
							<td style="border-right:1px solid #999;" align="right"><span id="totaSinAutorizar">$0.00/0</span></td>
                            <td align="right"><span id="planCierre">$0.00/0</span></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td><input type="checkbox" id="sinPlaneacion" /></td>
				<td><input type="checkbox" id="planeacionMes1" /></td>
			</tr>
			<tr>
				<td colspan="2">
                    <!--Dialogs-->
                    
					<button class="buttonLTR" id="btnReporteCompras">Reporte Ordenes de compra</button>
                    <button class="buttonLTR" id="btnAgregarOportunidad">Agregar oportunidad</button>
				</td>
			</tr>
		</table>
	</div>
	<div style="padding:10px;">
		<div id="divGridPlanVentas" class="divContGrid renglon-bottom">
			<div id="divContGrid">
				<!--INICIO GRID DE PlanVentas-->
				<table id="grdPlanVentas"></table>
				<div id="pagPlanVentas"></div>
				<!--FIN DE GRID DE PlanVentas-->
			</div>
		</div>
	</div>
    <div id="dialogAgregarOportunidad" title="Agregar oportunidad"></div>
</asp:Content>