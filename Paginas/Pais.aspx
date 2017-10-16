<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Pais.aspx.cs" Inherits="Pais" Title="Pa&iacute;s" %>
<asp:Content ID="headPais" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Pais.js"></script>
</asp:Content>
<asp:Content ID="BodyPais" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarPais" title ="Agregar país"></div>
    <div id="dialogConsultarPais" title ="Consultar país"></div>
    <div id="dialogEditarPais" title ="Editar país"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarPais" value="+ Agregar país" class="buttonLTR" />
        </div>
        <div id="divGridPais" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdPais"></table>
                <div id="pagPais"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>

</asp:Content>
