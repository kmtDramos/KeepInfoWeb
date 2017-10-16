<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="AsignacionCliente.aspx.cs" Inherits="AsignacionCliente" %>
<asp:Content ID="headAsignacionCliente" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/GestorEmpresarial.AsignacionCliente.js"></script>
</asp:Content>
<asp:Content ID="bodyAsignacionCliente" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<div id="dialogAsignarCliente" title="Agregar cliente"></div>
<div id="dialogConsultarClienteAsignado" title="Consultar cliente"></div>
<div id="dialogConsultarAsignacionCliente" title="Asignar cliente">
     <div class="divAreaBotonesDialog">
        <input type="button" id="btnAgregarCliente" value="+ Agregar cliente" class="buttonLTR" />
    </div>
     <div id="divGridClientes" class="divContGrid renglon-bottom">
        <div id="divContGridCliente">
            <!--INICIO GRID DE CLIENTES-->
            <table id="grdCliente"></table>
            <div id="pagCliente"></div>
            <!--FIN DE GRID DE CLIENTES-->
        </div>
    </div>
</div>
<div id="divContenido">
        <div id="divGridUsuarios" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE USUARIOS-->
                <table id="grdAsignacionCliente"></table>
                <div id="pagAsignacionCliente"></div>
                <!--FIN DE GRID DE USUARIOS-->
            </div>
        </div>
    </div>
</asp:Content>
