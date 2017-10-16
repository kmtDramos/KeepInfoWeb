<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="CuentaBancaria.aspx.cs" Inherits="CuentaBancaria" Title="Cuenta bancaria" %>
<asp:Content ID="headCuentaBancaria" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.CuentaBancaria.js"></script>
</asp:Content>
<asp:Content ID="bodyCuentaBancaria" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarCuentaBancaria" title ="Agregar cuenta bancaria"></div>
    <div id="dialogConsultarCuentaBancaria" title ="Consultar cuenta bancaria"></div>
    <div id="dialogEditarCuentaBancaria" title ="Editar cuenta bancaria"></div>
    <div id="dialogConsultarCuentaBancariaAsignadaUsuario" title="Consultar cuenta bancaria asignada"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarCuentaBancaria" value="+ Agregar cuenta bancaria" class="buttonLTR" />
        </div>
        <div id="divGridCuentaBancaria" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdCuentaBancaria"></table>
                <div id="pagCuentaBancaria"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>
</asp:Content>
