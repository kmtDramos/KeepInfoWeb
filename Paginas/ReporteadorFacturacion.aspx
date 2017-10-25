<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteadorFacturacion.aspx.cs" Inherits="Paginas_ReporteadorFacturacion" Title="Reporteador Facturación" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	<script type="text/javascript" src="../JS/Ventas.ReporteadorFacturacion.js?_=<%=DateTime.Now.Ticks %>"></script>
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
                    <select id="txtSucursal" style="width:120px;">
                        <option value="-1">Todas</option>
                    </select>
				</td>
			</tr>
			<tr>
				<td>Vendedor:</td>
				<td>
					<input type="text" id="txtUsuario" style="width:180px;"/>
				</td>
			</tr>
            <tr>
                <td></td>
                <td>
                    <button id="btnActualizarReporteador" class="buttonLTR">Buscar</button>

                </td>
            </tr>
		</table>
	</div>
	
	<!--/ Filtros -->


	<!-- Reportes -->

	<div id="divReportes" style="margin:10px;">
		<ul>
			<li><a href="#tabDivisiones">Divisiones</a></li>
			<li><a href="#tabVendedores">Vendedores</a></li>
		</ul>
		<div id="tabDivisiones" style="font-size:9px;min-height:450px;">
		</div>
		<div id="tabVendedores" style="font-size:9px;min-height:450px;">
		</div>
	</div>
	
	<!--/ Reportes -->

</asp:Content>