<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="ImpresionTemplate.aspx.cs" Inherits="ImpresionTemplate" Title="Impresión template" %>
<asp:Content ID="headCatalogoImpresionTemplate" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">	
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <script src="../js/upload/fileuploader.js" type="text/javascript"></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.ImpresionTemplate.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoImpresionTemplate" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarImpresionTemplate" title="Agregar impresión template"></div>
    <div id="dialogConsultarImpresionTemplate" title="Consultar impresión template"></div>
    <div id="dialogEditarImpresionTemplate" title="Editar impresión template"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarImpresionTemplate" value="+ Agregar impresión template" class="buttonLTR" />
        </div>
        <div id="divGridImpresionTemplate" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE ImpresionTemplate-->
                <table id="grdImpresionTemplate"></table>
                <div id="pagImpresionTemplate"></div>
                <!--FIN DE GRID DE ImpresionTemplate-->
            </div>
        </div>
    </div>
</asp:Content>
