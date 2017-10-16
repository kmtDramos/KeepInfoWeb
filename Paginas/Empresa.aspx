<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Empresa.aspx.cs" Inherits="Empresa" Title="Empresas" %>
<asp:Content ID="headCatalogoEmpresa" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">	
    <script src="../js/upload/fileuploader.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Empresa.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoEmpresa" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarEmpresa" title="Agregar empresa"></div>
    <div id="dialogConsultarEmpresa" title="Consultar empresa"></div>
    <div id="dialogEditarEmpresa" title="Editar empresa"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarEmpresa" value="+ Agregar empresa" class="buttonLTR" />
        </div>
        <div id="divGridEmpresa" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE EMPRESA-->
                <table id="grdEmpresa"></table>
                <div id="pagEmpresa"></div>
                <!--FIN DE GRID DE EMPRESA-->
            </div>
        </div>
    </div>
</asp:Content>