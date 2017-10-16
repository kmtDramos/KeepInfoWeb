<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TiempoEntrega.aspx.cs" Inherits="TiempoEntrega" Title="Tiempo de entrega" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/JSON/json2.js"></script>
    <script type="text/javascript" src="../js/Catalogo.TiempoEntrega.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTiempoEntrega" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarTiempoEntrega" title="Agregar tiempo de entrega"></div>
    <div id="dialogConsultarTiempoEntrega" title="Consultar tiempo de entrega"></div>
    <div id="dialogEditarTiempoEntrega" title="Editar tiempo de entrega"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <%= puedeAgregarTiempoEntrega == 1 ? "<input type='button' id='btnObtenerFormaAgregarTiempoEntrega' value='+ Agregar tiempo de entrega' class='buttonLTR' />" : "" %>
        </div>
        <div id="divGridTiempoEntrega" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TiempoEntrega-->
                <table id="grdTiempoEntrega"></table>
                <div id="pagTiempoEntrega"></div>
                <!--FIN DE GRID DE TiempoEntrega-->
            </div>
        </div>
    </div>
</asp:Content>
