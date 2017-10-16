<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="IVA.aspx.cs" Inherits="IVA" Title="IVA" %>
<asp:Content ID="headIVA" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.IVA.js"></script>
</asp:Content>
<asp:Content ID="bodyIVA" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarIVA" title ="Agregar IVA"></div>
<div id="dialogConsultarIVA" title ="Consultar IVA"></div>
<div id="dialogEditarIVA" title ="Editar IVA"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarIVA" value="+ Agregar IVA" class="buttonLTR" />
    </div>
    <div id="divGridIVA" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE IVA-->
            <table id="grdIVA"></table>
            <div id="pagIVA"></div>
            <!--FIN DE GRID DE IVA-->
        </div>
    </div>
</div>
</asp:Content>
