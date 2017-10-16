<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Grupo.aspx.cs" Inherits="Grupo" %>
<asp:Content ID="headCatalogoGrupo" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Grupo.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoGrupo" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarGrupo" title="Agregar grupo"></div>
    <div id="dialogConsultarGrupo" title="Consultar grupo"></div>
    <div id="dialogEditarGrupo" title="Editar grupo"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarGrupo" value="+ Agregar grupo" class="buttonLTR" />
        </div>
        <div id="divGridGrupo" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE GRUPO-->
                <table id="grdGrupo"></table>
                <div id="pagGrupo"></div>
                <!--FIN DE GRID DE GRUPO-->
            </div>
        </div>
    </div>
</asp:Content>
