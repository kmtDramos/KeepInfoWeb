<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="InventarioReal.aspx.cs" Inherits="Paginas_InventarioReal" Title="Inventario" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">

    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    
	<!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>

	<script type="text/javascript" src="../js/Almacen.InventarioReal.js?_=<%= DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div style="padding:10px;">
        <div class="divAreaBotonesDialog">
            <input type='button' id='btnObtenerFormaAgregarCatalago' value='+ Agregar Produto' class='buttonLTR'/>
        </div>
		<div id="divGridInventario" class="divContGrid renglon-bottom">
			<div id="divContGrid">
				<!--INICIO GRID DE PlanVentas-->
				<table id="grdInventario"></table>
				<div id="pagInventario"></div>
				<!--FIN DE GRID DE PlanVentas-->
			</div>
		</div>
	</div>
</asp:Content>