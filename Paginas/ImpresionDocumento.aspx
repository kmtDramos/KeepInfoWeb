<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ImpresionDocumento.aspx.cs" Inherits="ImpresionDocumento" Title="Impresión Documento" %>
<asp:Content ID="headImpresionDocumento" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.ImpresionDocumento.js"></script>
</asp:Content>
<asp:Content ID="bodyImpresionDocumento" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarImpresionDocumento" title ="Agregar impresión de documento"></div>
<div id="dialogConsultarImpresionDocumento" title ="Consultar impresión de documento"></div>
<div id="dialogEditarImpresionDocumento" title ="Editar impresión de documento"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarImpresionDocumento" value="+ Agregar impresión de documento" class="buttonLTR" />
    </div>
    <div id="divGridImpresionDocumento" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE ImpresionDocumento-->
            <table id="grdImpresionDocumento"></table>
            <div id="pagImpresionDocumento"></div>
            <!--FIN DE GRID DE ImpresionDocumento-->
        </div>
    </div>
</div>
</asp:Content>
