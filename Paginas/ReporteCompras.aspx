<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ReporteCompras.aspx.cs" Inherits="Paginas_ReporteCompras" Title="Reporte de compras" %>
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
	<script type="text/javascript" src="../js/Compras.ReporteCompras.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">

	<table style="width:100%;">
		<tr>
			<td>
				<table>
					<tr>
						<td>
							Año:Mes
						</td>
						<td>
							<input type="text" id="txtYear" value="<%=DateTime.Today.Year %>" style="width:40px;" />
							<select>
								<option value="1">Enero</option>
								<option value="2">Febrero</option>
								<option value="3">Marzo</option>
								<option value="4">Abril</option>
								<option value="5">Mayo</option>
								<option value="6">Junio</option>
								<option value="7">Julio</option>
								<option value="8">Agosto</option>
								<option value="9">Septiembre</option>
								<option value="10">Octubre</option>
								<option value="11">Noviembre</option>
								<option value="12">Diciembre</option>
							</select>
						</td>
					</tr>
					<tr>
						<td>Sucursal</td>
						<td>
							<select id="cmbSucursal">
								<option value="-1">-Todas-</option>
								<option value="7">Guadalajara</option>
								<option value="6">México</option>
								<option value="1">Monterrey</option>
							</select>
						</td>
					</tr>
					<tr>
						<td>Comprador</td>
						<td>
							<input type="text" id="txtComprador" />
						</td>
					</tr>
					<tr>
						<td>Proveedor:</td>
						<td>
							<input type="text" id="txtProveedor"/>
						</td>
					</tr>
					<tr>
						<td></td>
						<td></td>
					</tr>
					<tr>
						<td></td>
						<td></td>
					</tr>
				</table>
			</td>

			<td>

			</td>
		</tr>
		<tr>
			<td colspan="2">
				<div id="tabs" style="min-height:400px;">
					<ul>
						<li><a href="#tabVentas">Ventas</a></li>
						<li><a href="#tabCompras">Compras</a></li>
						<li><a href="#tabFinanzas">Finanzas</a></li>
						<li><a href="#tabAlmacen">Almacén</a></li>
					</ul>
					<div id="tabVentas">
						<div id="divGridVentas" class="divContGrid renglon-bottom" style="max-width:920px;overflow-x:auto;padding-bottom:5px;">
							<div id="divContGrid">
								<!--INICIO GRID DE Ventas-->
								<table id="grdVentas"></table>
								<div id="pagVentas"></div>
								<!--FIN DE GRID DE Ventas-->
							</div>
						</div>
					</div>
					<div id="tabCompras">
						<div id="divGridCompras" class="divContGrid renglon-bottom">
							<div id="divContGrid">
								<!--INICIO GRID DE Compras-->
								<table id="grdCompras"></table>
								<div id="pagCompras"></div>
								<!--FIN DE GRID DE Compras-->
							</div>
						</div>
					</div>
					<div id="tabFinanzas">
						<div id="divGridFinanzas" class="divContGrid renglon-bottom">
							<div id="divContGrid">
								<!--INICIO GRID DE Finanzas-->
								<table id="grdFinanzas"></table>
								<div id="pagFinanzas"></div>
								<!--FIN DE GRID DE Finanzas-->
							</div>
						</div>
					</div>
					<div id="tabAlmacen">
						<div id="divGridAlmacen" class="divContGrid renglon-bottom">
							<div id="divContGrid">
								<!--INICIO GRID DE Almacen-->
								<table id="grdAlmacen"></table>
								<div id="pagAlmacen"></div>
								<!--FIN DE GRID DE Almacen-->
							</div>
						</div>
					</div>
				</div>
			</td>
		</tr>
	</table>

</asp:Content>