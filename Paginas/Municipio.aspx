<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Municipio.aspx.cs" Inherits="Municipio" Title="Municipios" %>
<asp:Content ID="headMunicipio" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Municipio.js"></script>
</asp:Content>
<asp:Content ID="bodyMunicipio" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarMunicipio" title ="Agregar estado"></div>
    <div id="dialogConsultarMunicipio" title ="Consultar estado"></div>
    <div id="dialogEditarMunicipio" title ="Editar estado"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarMunicipio" value="+ Agregar municipio" class="buttonLTR" />
        </div>
        <div id="divGridMunicipio" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdMunicipio"></table>
                <div id="pagMunicipio"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>

</asp:Content>
