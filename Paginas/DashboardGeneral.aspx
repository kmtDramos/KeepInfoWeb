<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="DashboardGeneral.aspx.cs" Inherits="Paginas_DashboardGeneral" Title="Dashboard"%>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	<script type="text/javascript" src="../js/Ventas.DashboardGeneral.js?_=<%=DateTime.Now.Ticks %>"></script>
	<style>
		.rotate th {
			height: 120px;
			vertical-align: bottom;
			position: relative;
		}
		.rotate th div{
			white-space: nowrap;
			width: 10px;
			margin-left: 10px;
			transform: rotate(-90deg);
		}
		.rotate th div.not{
			width: auto;
			margin: 0px;
			line-height: auto;
			transform: rotate(0deg);
		}
		
		.cells tr:nth-child(even) td { background-color: #DDD; }

		.cantidad{ background-color: #42a824;}
		.monto{ background-color: #236db5;}
		.compras{ background-color: #ebe125;}
		.almacen{ background-color: #999;}
		.proyectos{ background-color: #2ad1b8;}
		.cobranza{ background-color: #ff6a00;}
	</style>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div style="padding:10px;">
		<button id="btnPantallaCompleta" class="buttonLTR">Pantalla completa</button>
	</div>
	<div style="padding:10px; width: 930px; height: 500px; overflow: auto;">
		<div id="reporte">
			<table id="tblReporte" border="1" width="100%" cellpadding="0" cellspacing="0">
				<thead>
					<tr>
						<th colspan="2"></th>
						<th colspan="6" class="cantidad">Ventas Cantidad</th>
						<th colspan="6" class="monto">Ventas Montos</th>
						<th colspan="3" class="compras">Compras</th>
						<th colspan="3" class="almacen">Almacen</th>
						<th colspan="3" class="proyectos">Proyectos</th>
						<th colspan="6" class="cobranza">Cobranza</th>
					</tr>
					<tr class="rotate">
						<th><div class="not">Sucursal</div></th>
						<th><div class="not">Vendedor</div></th>
						<th class="cantidad"><div>Baja</div></th>
						<th class="cantidad"><div>Media</div></th>
						<th class="cantidad"><div>Alta</div></th>
						<th class="cantidad"><div>Nuevos</div></th>
						<th class="cantidad"><div>Actual</div></th>
						<th class="cantidad"><div>Total</div></th>
						<th class="monto"><div>Baja</div></th>
						<th class="monto"><div>Media</div></th>
						<th class="monto"><div>Alta</div></th>
						<th class="monto"><div>Nuevos</div></th>
						<th class="monto"><div>Actual</div></th>
						<th class="monto"><div>Total</div></th>
						<th class="compras"><div>Colocado</div></th>
						<th class="compras"><div>Detenido</div></th>
						<th class="compras"><div>Total</div></th>
						<th class="almacen"><div>Recibido</div></th>
						<th class="almacen"><div>Pndiente</div></th>
						<th class="almacen"><div>Total</div></th>
						<th class="proyectos"><div>En Tiempo</div></th>
						<th class="proyectos"><div>Fuera de Tiempo</div></th>
						<th class="proyectos"><div>Total</div></th>
						<th class="cobranza"><div>A Tiempo</div></th>
						<th class="cobranza"><div>1-30 Dias</div></th>
						<th class="cobranza"><div>31-60 Dias</div></th>
						<th class="cobranza"><div>61-90 Dias</div></th>
						<th class="cobranza"><div>90+</div></th>
						<th class="cobranza"><div>Total</div></th>
					</tr>
				</thead>
				<tbody class="cells"></tbody>
			</table>
		</div>
	</div>
</asp:Content>