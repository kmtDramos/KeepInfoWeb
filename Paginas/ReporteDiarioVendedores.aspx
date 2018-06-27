<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteDiarioVendedores.aspx.cs" Inherits="Paginas_ReporteDiarioVendedores" Title="Reporte Diario de Vendedores" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	<script src="../JS/Ventas.ReporteDiarioVendedores.js?_=<%=DateTime.Now.Ticks %>"></script>
	<style>
		.tab{
			height:450px;
			overflow:auto;
		}
	</style>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div>
		<p><%=Mes%> <%=DateTime.Today.Year%></p>
		<div id="tabs">
			<ul>
				<li><a href="#tabProspeccion">Prospección</a></li>
				<li><a href="#tabOportunidad">Oportunidades</a></li>
				<li><a href="#tabFacturacion">Facturación</a></li>
				<li><a href="#tabFamilias">Familias</a></li>
				<li><a href="#tabVentas">Ventas</a></li>
				<li><a href="#tabNoAutorizado">No Autorizados</a></li>
				<li><a href="#tabCartera">Cartera</a></li>
				<li><a href="#tabCobranza">Cobranza</a></li>
			</ul>
			<div id="tabFacturacion" class="tab"></div>
			<div id="tabFamilias" class="tab"></div>
			<div id="tabProspeccion" class="tab"></div>
			<div id="tabOportunidad" class="tab"></div>
			<div id="tabVentas" class="tab"></div>
			<div id="tabNoAutorizado" class="tab"></div>
			<div id="tabCartera" class="tab"></div>
			<div id="tabCobranza" class="tab"></div>
		</div>
	</div>
</asp:Content>