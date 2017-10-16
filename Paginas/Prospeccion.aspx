<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Prospeccion.aspx.cs" Inherits="Paginas_Prospeccion" Title="Prospección" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	<script type="text/javascript" src="../js/Ventas.Prospeccion.js?_=<%=DateTime.Now.Ticks %>"></script>
	
	<style>
		.rotate th {
			height: 220px;
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
	<div style="padding:20px; overflow: auto;">
		<table>
			<tr>
				<td>Agente:</td>
				<td></td>
			</tr>
			<tr>
				<td>
					<select id="cmbUsuario">
						<option value="-1">Todos</option>
					</select>
				</td>
				<td><button id="btnAgregarFilaProspeccion" class="buttonLTR">+ Agregar</button></td>
			</tr>
		</table>
		<hr />
		<div style="width:910px;height:400px;overflow: auto;" id="divTablaProspeccion"></div>
	</div>
</asp:Content>