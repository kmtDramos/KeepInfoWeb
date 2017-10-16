<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="AutorizacionIVA.aspx.cs" Inherits="AutorizacionIVA" Title="Página sin título" %>
<asp:Content ID="headGestorEmpresarialAutorizacionIVA" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/GestorEmpresarial.AutorizacionIVA.js"></script>
</asp:Content>
<asp:Content ID="bodyGestorEmpresarialAutorizacionIVA" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarAutorizacionIVA" title="Agregar autorización de IVA"></div>
    <div id="dialogConsultarAutorizacionIVA" title="Consultar autorización de IVA"></div>
    <div id="dialogEditarAutorizacionIVA" title="Editar autorización de IVA"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarAutorizacionIVA" value="+ Agregar autorización IVA" class="buttonLTR" />
        </div>
        <div id="divGridAutorizacionIVA" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE AutorizacionIVA-->
                <table id="grdAutorizacionIVA"></table>
                <div id="pagAutorizacionIVA"></div>
                <!--FIN DE GRID DE AutorizacionIVA-->
            </div>
        </div>
    </div>
</asp:Content>
