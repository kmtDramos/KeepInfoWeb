<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="DevolucionProveedor.aspx.cs" Inherits="DevolucionProveedor" Title="DevolucionProveedor" %>
<asp:Content ID="headDevolucionProveedor" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.DevolucionProveedor.js"></script>
</asp:Content>
<asp:Content ID="bodyDevolucionProveedor" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogConfirmaAccion" title="Confirmación"><p id="textoConfirmacion">¿Está seguro que desea realizar esta acción?</p></div>
<div id="divContenido">
    
    <div id="divContGridDevolucionProveedor">
        <!--INICIO GRID-->
        <div id="divGridDevolucionProveedor" class="divContGrid renglon-bottom" IdDevolucionProveedor="${IdDevolucionProveedor}">
            <div id="divContGrid">
                <table id="grdDevolucionProveedor"></table>
                <div id="pagDevolucionProveedor"></div>
            </div>
        </div>
        <div>
            <table>
            </table>
        </div>
        <!--FIN DE GRID-->
    </div>
</div>
</asp:Content>  
