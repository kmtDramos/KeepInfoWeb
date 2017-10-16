<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteadorFacturacion.aspx.cs" Inherits="Paginas_ReporteadorFacturacion" Title="Reporteador Facturación" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	
	<!-- DataTables -->
	<link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/v/ju/dt-1.10.16/datatables.min.css"/>
	<script type="text/javascript" src="https://cdn.datatables.net/v/ju/dt-1.10.16/datatables.min.js"></script>

	<!--  -->
	<script type="text/javascript" src="../JS/Ventas.ReporteadorFacturacion.js"
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	
	<!-- Filtros -->
	
	<div style="margin:10px;">
		<table>
			<tr>
				<td>Fecha Inicial:</td>
				<td>
					<input type="text" id="txtFechaInicial" style="width:120px;" value="<%=DateTime.Today.AddDays(-DateTime.Today.Day+1).ToShortDateString() %>"
				</td>
			</tr>
			<tr>
				<td>Fecha Final:</td>
				<td>
					<input type="text" id="txtFechaFinal" style="width:120px;" value="<%=DateTime.Today.ToShortDateString() %>"
				</td>
			</tr>
			<tr>
				<td>Sucursal:</td>
				<td>
					<select id="cmbSucursal" style="width:90px;">
						<option value="-1">-Todas-</option>
					</select>
				</td>
			</tr>
			<tr>
				<td>Familia:</td>
				<td>
					<select id="cmbDivision" style="width:90px;">
						<option value="-1">-Todas-</option>
					</select>
				</td>
			</tr>
			<tr>
				<td>Vendedor:</td>
				<td>
					<input type="text" style="width:120px;" id="txtAgente" />
				</td>
			</tr>
		</table>
	</div>
	
	<!--/ Filtros -->
	
	<!-- Totales -->

	<div style="border: 1px solid #000;border-radius:4px;background-color: #BBB;margin: 10px;">
		<table width="100%">
			<tr>
				<th colspan="3">Total</th>
			</tr>
			<tr>
				<th colspan="3"><span id="spnTotal">$0.00</span></th>
			</tr>
			<tr>
				<th width="33%">Monterrey</th>
				<th>México</th>
				<th width="33%">Guadalajara</th>
			</tr>
			<tr>
				<th><span id="spnMonterrey">$0.00</span></th>
				<th><span id="spnMexico">$0.00</span></th>
				<th><span id="spnGuadalajara">$0.00</span></th>
			</tr>
		</table>
	</div>
	
	<!--/ Totales -->

	<!-- Reportes -->

	<div id="divReportes" style="margin:10px;">
		<ul>
			<li><a href="#tabDetalle">Facturas</a></li>
			<li><a href="#tabSucursales">Sucursales</a></li>
			<li><a href="#tabFamilias">Familias</a></li>
			<li><a href="#tabVendedores">Vendedores</a></li>
		</ul>
		<div id="tabDetalle">
			<table id="tblDetalle" height="300">
				<thead>
					<tr>
						<th>Sucursal</th>
						<th>Familia</th>
						<th>Vendedor</th>
						<th>Meta</th>
						<th>Plan</th>
						<th>Logro</th>
						<th>Diferencia</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>
		<div id="tabSucursales">
			<table id="tblSucursales" height="300">
				<thead>
					<tr>
						<th>Sucursal</th>
						<th>Meta</th>
						<th>Plan</th>
						<th>Logro</th>
						<th>Diferencia</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>
		<div id="tabFamilias">
			<table id="tblFamilias" height="300">
				<thead>
					<tr>
						<th>Familia</th>
						<th>Meta</th>
						<th>Plan</th>
						<th>Logro</th>
						<th>Diferencia</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>
		<div id="tabVendedores">
			<table id="tblVendedores" height="300">
				<thead>
					<tr>
						<th>Vendedor</th>
						<th>Meta</th>
						<th>Plan</th>
						<th>Logro</th>
						<th>Diferencia</th>
					</tr>
				</thead>
				<tbody></tbody>
			</table>
		</div>
	</div>
	
	<!--/ Reportes -->

</asp:Content>