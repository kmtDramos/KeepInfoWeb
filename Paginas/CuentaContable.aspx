<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CuentaContable.aspx.cs" Inherits="CuentaContable" Title="Cuentas contables" %>
<asp:Content ID="headCuentaContable" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.CuentaContable.js"></script>
</asp:Content>
<asp:Content ID="bodyCuentaContable" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarCuentaContable" title ="Agregar cuentas contables"></div>
    <div id="dialogConsultarCuentaContable" title ="Consultar cuentas contables"></div>
    <div id="dialogEditarCuentaContable" title ="Editar cuentas contables"></div>
    <div id="dialogTipoCuentaContable" title ="Tipo de cuenta contable"></div>
    <div id="dialogAgregarCuentaContableComplementos" title ="Agregar cuentas contables complementos"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarCuentaContable" value="+ Agregar cuenta contable" class="buttonLTR" />
        </div>
        <div id="divGridCuentaContable" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE CUENTAS CONTABLES-->
                <table id="grdCuentaContable"></table>
                <div id="pagCuentaContable"></div>
                <!--FIN DE GRID DE CUENTAS CONTABLES-->
            </div>
        </div>
    </div>
</asp:Content>
