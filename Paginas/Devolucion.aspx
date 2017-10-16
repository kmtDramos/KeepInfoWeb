<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Devolucion.aspx.cs" Inherits="Devolucion" Title="Devolucion" %>
<asp:Content ID="headDevolucion" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.Devolucion.js"></script>
</asp:Content>
<asp:Content ID="bodyDevolucion" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogConfirmaAccion" title="Confirmación"><p id="textoConfirmacion">¿Está seguro que desea realizar esta acción?</p></div>
<div id="divContenido">
    
    <div id="divContGridDevolucion">
        <!--INICIO GRID-->
        <div id="divGridDevolucion" class="divContGrid renglon-bottom" IdDevolucion="${IdDevolucion}">
            <div id="divContGrid">
                <table id="grdDevolucion"></table>
                <div id="pagDevolucion"></div>
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
