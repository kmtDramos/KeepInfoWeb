<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="MovimeintosBancarios.aspx.cs" Inherits="Paginas_MovimeintosBancarios" Title="Movimientos" %>
<asp:Content ContentPlaceHolderID="headMasterPageSeguridad" runat="server">

    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    
	<!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>

    <link type="text/css" rel="stylesheet" href="../css/actividad.css" />
    <script type="text/javascript" src="../js/datetimepicker.js"></script>
    <script type="text/javascript" src="../js/moment.min.js"></script>
    <script type="text/javascript" src="../js/lang-all.js"></script>
    
    <script type="text/javascript" src="../js/Contabilidad.Movimientos.js?_=<%=DateTime.Now.Ticks %>"></script>

</asp:Content>
<asp:Content ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
	<div style="padding:10px;">
        <table>
            <tr>
                <td></td>
                <td>Banco:</td>
            </tr>
            <tr>
                <td>
                    <button id="btnAgregarMovimiento" class="buttonLTR">Agregar Movimiento</button>
                </td>
                <td>
                    <select id="cmbBanco">
                        <option value="-1">-Todos-</option>
                    </select>
                </td>
            </tr>
        </table>
		<div id="divGridMovimientos" class="divContGrid renglon-bottom">
			<div id="divContGrid">
				<!--INICIO GRID DE Movimientos-->
				<table id="grdMovimientos"></table>
				<div id="pagMovimientos"></div>
				<!--FIN DE GRID DE Movimientos-->
			</div>
		</div>
	</div>
</asp:Content>