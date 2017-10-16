<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CargaMasivaInventario.aspx.cs" Inherits="CargaMasivaInventario" %>
<asp:Content ID="headCargaMasivaInventario" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Desarrollo.CargaMasivaInventario.js"></script>
</asp:Content>
<asp:Content ID="bodyCargaMasivaInventario" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnSubirArchivo" value="Subir archivo" class="buttonLTR" />
        </div>
        <div id="divAgregarCargaMasivaInventario"></div>
        <div id="divPanelIntegrador">
            <ul>
                <li><a href="#tab-Productos">Producto</a></li>
            </ul>
            <div id="tab-Productos">
                <div class="contenedorTab">
                    <div class="divListaProductosNoRegistrados">
	                    <div class="alignCenter">
		                    <table class="tblEncabezadoProductosNoRegistrados">
			                    <tr>
				                    <td>
					                    <div id="divProductosNoRegistrados">
						                    No registrados
					                    </div>
				                    </td>
			                    </tr>
		                    </table>
	                    </div>
	                    <div class="alignCenter">
		                    <table class="tblEncabezadoProductosNoRegistrados">
			                    <tr>
				                    <td>
					                    <input id="txtBuscadorProductosNoRegistrados" type="text" value="" style="padding:0px; margin:0px; width:98%;"/>
				                    </td>
			                    </tr>
		                    </table>
	                    </div>
	                    <div class="alignCenter">
		                    <table class="tblEncabezadoProductosNoRegistrados">
			                    <tr>
				                    <td class="centerMe borderRight" style="width:21px;" title="Nuevo">
				                       <div id="divEncabezadoNuevo">
						                    N
					                    </div> 
				                    </td>
				                    <td class="centerMe borderRight" style="width:21px;" title="Asignar">
					                    <div id="divEncabezadoAsignar">
						                    A
					                    </div>
				                    </td>
				                    <td class="centerMe borderRight" style="width:36px;" title="Identificador">
				                       <div id="divEncabezadoNuevo">
						                    Id
					                    </div> 
				                    </td>
				                    <td class="centerMe borderRight" style="width:343px;">
					                    <div id="divEncabezadoNombre">
						                    Descripción
					                    </div>
				                    </td>
				                    <td style="width:10px;">
					                    &nbsp;
				                    </td>
			                    </tr>
		                    </table>
	                    </div>
	                    <ul id="ulListadoProductosNoRegistrados" class="ulListadoProductosNoRegistrados"></ul>
                    </div>
                    <div class="divListaProductosExistentes">
	                    <div class="alignCenter">
		                    <table class="tblEncabezadoProductosExistentes">
			                    <tr>
				                    <td>
					                    <div id="divProductosExistentes">
						                    Existentes
					                    </div>
				                    </td>
			                    </tr>
		                    </table>
	                    </div>
	                    <div class="alignCenter">
		                    <table class="tblEncabezadoProductosExistentes">
			                    <tr>
				                    <td>
					                    <input id="txtBuscadorProductosExistentes" type="text" value="" style="padding:0px; margin:0px; width:98%;"/>
				                    </td>
			                    </tr>
		                    </table>
	                    </div>
	                    <div class="alignCenter">
		                    <table class="tblEncabezadoProductosExistentes">
			                    <tr>
				                    <td class="centerMe borderRight" style="width:36px;" title="Identificador">
				                       <div id="divEncabezadoNuevo">
						                    Id
					                    </div>
				                    </td>
				                    <td class="centerMe borderRight" style="width:385px;">
					                    <div id="divEncabezadoNombre">
						                    Descripción
					                    </div>
				                    </td>
				                    <td style="width:10px;">
					                    &nbsp;
				                    </td>
			                    </tr>
		                    </table>
	                    </div>
	                    <ul id="ulListadoProductosExistentes" class="ulListadoProductosExistentes"></ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>