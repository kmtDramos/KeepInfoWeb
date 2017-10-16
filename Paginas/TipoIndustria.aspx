<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="TipoIndustria.aspx.cs" Inherits="TipoIndustria" Title="Tipos de industria" %>
<asp:Content ID="headCatalogoTipoIndustria" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.TipoIndustria.js"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoTipoIndustria" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
    <div id="dialogAgregarTipoIndustria" title="Agregar tipo de industria"></div>
    <div id="dialogConsultarTipoIndustria" title="Consultar tipo de industria"></div>
    <div id="dialogEditarTipoIndustria" title="Editar tipo de industria"></div>
    <div id="divContenido">
        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
            <input type="button" id="btnObtenerFormaAgregarTipoIndustria" value="+ Agregar tipo de industria" class="buttonLTR" />
        </div>
        <div id="divGridTipoIndustria" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE TipoIndustria-->
                <table id="grdTipoIndustria"></table>
                <div id="pagTipoIndustria"></div>
                <!--FIN DE GRID DE TipoIndustria-->
            </div>
        </div>
    </div>
</asp:Content>
