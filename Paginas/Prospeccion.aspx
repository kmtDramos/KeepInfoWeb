<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Prospeccion.aspx.cs" Inherits="Paginas_Prospeccion" Title="Prospección" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
	<script type="text/javascript" src="../js/Ventas.Prospeccion.js?_=<%=DateTime.Now.Ticks %>"></script>
	
	<style>
		.rotate th {
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
		<div style="border:1px solid #BBB;background-color: #BBB;border-radius:4px;">
			<table width="100%" cellspacing="0">
				<tr>
					<th width="20%"># Total Prospectos</th>
					<th width="20%">Dias Promedio</th>
					<th width="20%">Ganados</th>
					<th width="20%">Perdidos</th>
				</tr>
				<tr>
					<th><span id="totalProspectos">0</span></th>
					<th><span id="diasPromedio">0</span></th>
					<th><span id="ganadas">0</span></th>
					<th><span id="perdidas">0</span></th>
				</tr>
			</table>
		</div>
	</div>
	<div style="padding:20px; overflow: auto;">
		<table>
			<tr>
                <td>Fecha Inicial:</td>
                <td>Fecha Final:</td>
				<td>Agente:</td>
				<td></td>
			</tr>
			<tr>
				<td>
					<input type="text" id="txtFechaInicial" style="width:120px;" value="<%=DateTime.Today.AddDays(-DateTime.Today.Day+1).AddMonths(-1).ToShortDateString() %>"
				</td>
				<td>
					<input type="text" id="txtFechaFinal" style="width:120px;" value="<%=DateTime.Today.ToShortDateString() %>"
				</td>
				<td>
					<select id="cmbUsuario">
                        <option value="0">-</option>
						<option value="-1">Todos</option>
					</select>
				</td>
                <td><button id="btnActualizarProspeccion" class="buttonLTR">Actualizar</button></td>
                <td><button id="btnAgregarFilaProspeccion" class="buttonLTR">+ Agregar</button></td>
                <td><button id="btnCargarArchivo" class="buttonLTR">Cargar Archivo</button></td>
            </tr>
		</table>
		<hr />
		<div style="width:910px;height:400px;overflow: auto;" id="divTablaProspeccion"></div>
        <hr />
        <img src="../Images/actualizar.png" height="20" style="cursor:pointer;" onclick="ObtenerTablaProspeccion();" />
	</div>
</asp:Content>