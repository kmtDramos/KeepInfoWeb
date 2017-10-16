<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoServicio.aspx.cs" Inherits="TipoServicio" Title="Tipo de servicio" %>
<asp:Content ID="headTipoServicio" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.TipoServicio.js"></script>
</asp:Content>
<asp:Content ID="bodyTipoServicio" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarTipoServicio" title ="Agregar tipo de servicio"></div>
<div id="dialogConsultarTipoServicio" title ="Consultar tipo de servicio"></div>
<div id="dialogEditarTipoServicio" title ="Editar tipo de servicio"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarTipoServicio" value="+ Agregar tipo de servicio" class="buttonLTR" />
    </div>
    <div id="divGridTipoServicio" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE MEDIO ENTERO-->
            <table id="grdTipoServicio"></table>
            <div id="pagTipoServicio"></div>
            <!--FIN DE GRID DE MEDIO ENTERO-->
        </div>
    </div>
</div>
</asp:Content>
