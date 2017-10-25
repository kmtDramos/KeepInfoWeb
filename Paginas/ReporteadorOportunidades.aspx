<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteadorOportunidades.aspx.cs" Inherits="Paginas_ReporteadorOportunidades" Title="Reporteador de Oportunidades" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">

	<!-- Script -->
	<script type="text/javascript" src="../JS/Ventas.ReporteadorOportunidades.js?_=<%=DateTime.Now.Ticks %>"></script>

</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div>
		<table width="100%">
			<tr>
				<td style="padding: 10px;" valign="top">
					<table>
						<tr>
							<td>
								Cliente:
							</td>
							<td>
								<input type="text" id="txtCliente" />
							</td>
						</tr>
						<tr>
							<td>
								Agente:
							</td>
							<td>
								<input type="text" id="txtAgente" />
							</td>
						</tr>
						<tr>
							<td>
								Sucursal:
							</td>
							<td>
								<select id="cmbSucursal">
									<option value="-1">Elegir una opción...</option>
								</select>
							</td>
						</tr>
						<tr>
							<td>
								Familia:
							</td>
							<td>
								<select id="cmbDivision">
									<option value="-1">Elegir una opción...</option>
								</select>
							</td>
						</tr>
						<tr>
							<td>
								Fecha inicial:
							</td>
							<td>
								<input type="text" id="txtFechaInicial" value="<%=DateTime.Today.AddDays(-DateTime.Today.Day+1).ToShortDateString() %>" style="width:90px;" />
							</td>
						</tr>
						<tr>
							<td>
								Fecha final:
							</td>
							<td>
								<input type="text" id="txtFechaFinal" value="<%=DateTime.Today.ToShortDateString() %>" style="width:90px;" />
							</td>
						</tr>
						<tr>
							<td></td>
							<td>
								<button id="btnActualizarReporteador" class="buttonLTR">Buscar</button>
							</td>
						</tr>
					</table>
				</td>
				<td valign="top" style="font-size:10px;">
					<table id="tblTotales" width="100%" height="150">
						<thead>
							<tr>
								<th>Familia</th>
								<th>Oportunidades</th>
								<th>Costos</th>
								<th>Margen opt</th>
								<th>Margen familia</th>
								<th>Utilidad</th>
							</tr>
						</thead>
						<tbody></tbody>
					</table>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<div style="padding:0p 10px 10px 10px;font-size:10px;">
						<table id="tblDetalleOportunidad" width="100%" height="250">
							<thead>
								<tr>
									<th width="10">#</th>
									<th width="40">Oportunidad</th>
									<th>Cliente</th>
									<th>Agente</th>
									<th width="40">Sucursal</th>
									<th width="40">División</th>
									<th width="20">Dias</th>
									<th width="30">Monto</th>
									<th width="30">Costo</th>
									<th width="30">Utilidad</th>
								</tr>
							</thead>
							<tbody>
							</tbody>
						</table>
					</div>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>