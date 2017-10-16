<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="SubCuentaContable.aspx.cs" Inherits="SubCuentaContable" Title="Centro de costos" %>
<asp:Content ID="headSubCuentaContable" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.SubCuentaContable.js"></script>
</asp:Content>
<asp:Content ID="bodySubCuentaContable" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarSubCuentaContable" title ="Agregar subcuenta contable"></div>
<div id="dialogConsultarSubCuentaContable" title ="Consultar subcuenta contable"></div>
<div id="dialogEditarSubCuentaContable" title ="Editar subcuenta contable"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarSubCuentaContable" value="+ Agregar subcuenta contable" class="buttonLTR" />
    </div>
    <div id="divGridSubCuentaContable" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE MEDIO ENTERO-->
            <table id="grdSubCuentaContable"></table>
            <div id="pagSubCuentaContable"></div>
            <!--FIN DE GRID DE MEDIO ENTERO-->
        </div>
    </div>
</div>
</asp:Content>
