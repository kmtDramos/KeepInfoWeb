<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="SolicitudLevantamiento.aspx.cs" Inherits="SolicitudLevantamiento" Title="SolicitudLevantamiento" %>
<asp:Content ID="headCatalogoLevantamiento" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>

    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/SolicitudLevantamiento.js?_=<% Response.Write(ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoLevantamiento" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogConsultarSolicitudLevantamiento" title ="Consultar Levantamiento"></div> 
    <div id="divContenido">
        <div id="divFiltrosSolicitudLevantamiento"></div>
        <!--div class="divAreaBotonesDialog">
            <input type='button' id='btnObtenerFormaAgregarLevantamiento' value='+ Agregar Levantamiento' class='buttonLTR' />
        </div-->
        <div id="divGridLevantamiento" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <table id="grdSolicitudLevantamiento"></table>
                <div id="pagSolicitudLevantamiento"></div>                
            </div>
        </div>
    </div>
</asp:Content>