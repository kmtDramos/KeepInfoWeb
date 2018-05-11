<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteDiarioVendedores.aspx.cs" Inherits="Paginas_ReporteDiarioVendedores" Title="Reporte Diario de Vendedores" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	<script src="../JS/Ventas.ReporteDiarioVendedores.js"></script>
	<style>
		.tab{
			height:450px;
		}
	</style>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div>
		<p><%=Mes%> <%=DateTime.Today.Year%></p>
		<div id="tabs">
			<ul>
				<li><a href="#tabFacturacion">Facturación</a></li>
				<li><a href="#tabProspeccion">Prospección</a></li>
			</ul>
			<div id="tabFacturacion" class="tab"></div>
			<div id="tabProspeccion" class="tab"></div>
		</div>
	</div>
</asp:Content>