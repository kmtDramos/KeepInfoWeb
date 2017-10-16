<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ImpresionEtiquetas.aspx.cs" Inherits="ImpresionEtiquetas" Title="Impresión etiquetas" %>
<asp:Content ID="headCatalogoImpresionEtiquetas" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    	
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.ImpresionEtiquetas.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoImpresionEtiquetas" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarImpresionEtiquetas" title="Agregar impresión etiqueta"></div>
    <div id="dialogConsultarImpresionEtiquetas" title="Consultar impresión etiqueta"></div>
    <div id="dialogEditarImpresionEtiquetas" title="Editar impresión etiqueta"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarImpresionEtiquetas" value="+ Agregar impresión etiqueta" class="buttonLTR" />
        </div>
        <div id="divGridImpresionEtiquetas" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE ImpresionEtiquetas-->
                <table id="grdImpresionEtiquetas"></table>
                <div id="pagImpresionEtiquetas"></div>
                <!--FIN DE GRID DE ImpresionEtiquetas-->
            </div>
        </div>
    </div>
</asp:Content>
