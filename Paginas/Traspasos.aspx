<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Traspasos.aspx.cs" Inherits="Traspasos" Title="Traspasos" %>
<asp:Content ID="headTraspasos" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.Traspasos.js"></script>
</asp:Content>
<asp:Content ID="bodyTraspasos" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->

<div id="divContenido">
    <div id="divFiltrosTraspasos"></div>
    <div id="divContGridAlmacenProductoResumen">
    <!--INICIO GRID DETALLE PRODUCTO PROVEEDOR-->
    <div id="divGridAlmacenProductoResumen" class="divContGrid renglon-bottom" IdProducto="${IdProducto}">
        <div id="divContGrid">
            <table id="grdAlmacenProductoResumen"></table>
            <div id="pagAlmacenProductoResumen"></div>
        </div>
    </div>
    <div>
        <table>
        </table>
    </div>
    <!--FIN DE GRID DETALLE PRODUCTO PROVEEDOR-->
    <div id="dialogAgregarTraspaso" title="Agregar Traspaso"></div>
    <div id="divContGridAlmacenProductoDetalle">
        <!--INICIO GRID DETALLE PRODUCTO PROVEEDOR-->
        <div id="divGridAlmacenProductoDetalle" class="divContGrid renglon-bottom" IdProducto="${IdProducto}" IdAlmacen="${IdAlmacen}">
            <div id="divContGrid">
                <table id="grdAlmacenProductoDetalle"></table>
                <div id="pagAlmacenProductoDetalle"></div>
            </div>
            
            
        </div>
        
        
        <!--FIN DE GRID DETALLE PRODUCTO PROVEEDOR-->
    </div>
    <div>
        <table width="100%">
            <tr>
    	        <td colspan="2">Traspaso del Almacén Origen al Almacén Destino</td>
            </tr>
            <tr>
                <td style="width:50%"></td>
                <td style="width:50%"></td>
            </tr>
            <tr>
	            <td style="width:50%">Almacén Origen:</td>
		        <td style="width:50%">Almacén Destino:</td>
            </tr>
            <tr>
		        <td>
		            <input type="text" id="txtAlmacenOrigen" readonly="true" />
		            <input type="text" id="txtIdAlmacenOrigen" visible="false" readonly="true" style="visibility:hidden" />
		        </td>
		        <td colspan="2">
		            <select id="cmbAlmacenDestino">
                        <option value="0">Elegir una opci&oacute;n...</option>
                        {{each Opciones}}
                            <option value="${$value.Valor}" {{if $value.Selected === 1}}selected="selected"{{/if}}>${$value.Descripcion}</option>
                        {{/each}}
                    </select>
		        </td>
	        </tr>
	        <tr>
	            <td>Número de factura de proveedor:</td>
            </tr>
            <tr>
		        <td><input type="text" id="txtNumeroFactura" readonly="true" /></td>
		        <td><input type="text" id="txtIdExistenciaDistribuida" readonly style="visibility:hidden"/></td>
	        </tr>
	        <tr>
		        <td>Disponible:</td>
		        <td>Cantidad:</td>
            </tr>
	        <tr>
		        <td><input type="text" id="txtSaldo" readonly="true"/><input type="text" id="txtCantidad" visible="false" style="visibility:hidden" readonly="true"/></td>
		        <td><input type="text" id="txtCantidadDeEntrada" onkeypress="javascript:return ValidarNumero(event, this.value);" /></td>
	        </tr>
            <tr>
    	        <td colspan="2">
    	        <input type='button' id='btnGenerarTraspaso' value='Generar traspaso' class='buttonLTR'/>
    	        </td>
            </tr>
            <tr>
            </tr>
	        <tr>
    	        <td colspan="2">&nbsp;</td>
            </tr>
            <tr></tr>
        </table>
    </div>
</div>
</asp:Content>  
