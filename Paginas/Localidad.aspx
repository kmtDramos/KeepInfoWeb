<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Localidad.aspx.cs" Inherits="Localidad" Title="Localidad" %>
<asp:Content ID="headLocalidad" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Localidad.js"></script>
</asp:Content>
<asp:Content ID="bodyLocalidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarLocalidad" title ="Agregar estado"></div>
    <div id="dialogConsultarLocalidad" title ="Consultar estado"></div>
    <div id="dialogEditarLocalidad" title ="Editar estado"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarLocalidad" value="+ Agregar localidad" class="buttonLTR" />
        </div>
        <div id="divGridLocalidad" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdLocalidad"></table>
                <div id="pagLocalidad"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>

</asp:Content>
