<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Estado.aspx.cs" Inherits="Estado" Title="Estado" %>
<asp:Content ID="headEstado" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Estado.js"></script>
</asp:Content>
<asp:Content ID="BodyEstado" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarEstado" title ="Agregar estado"></div>
    <div id="dialogConsultarEstado" title ="Consultar estado"></div>
    <div id="dialogEditarEstado" title ="Editar estado"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarEstado" value="+ Agregar estado" class="buttonLTR" />
        </div>
        <div id="divGridEstado" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdEstado"></table>
                <div id="pagEstado"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>

</asp:Content>
