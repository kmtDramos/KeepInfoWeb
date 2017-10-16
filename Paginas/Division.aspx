<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Division.aspx.cs" Inherits="Division" Title="Divisiones" %>
<asp:Content ID="headDivision" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Division.js"></script>
</asp:Content>
<asp:Content ID="bodyDivision" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarDivision" title ="Agregar división"></div>
<div id="dialogConsultarDivision" title ="Consultar división"></div>
<div id="dialogEditarDivision" title ="Editar división"></div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarDivision" value="+ Agregar división" class="buttonLTR" />
    </div>
    <div id="divGridDivision" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE DIVISION-->
            <table id="grdDivision"></table>
            <div id="pagDivision"></div>
            <!--FIN DE GRID DE DIVISION-->
        </div>
    </div>
</div>
</asp:Content>
