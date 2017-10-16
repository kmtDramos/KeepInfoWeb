<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="GestionCobranza.aspx.cs" Inherits="GestionCobranza" Title="Gestión de cobranza" %>
<asp:Content ID="headGestionCobranza" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Operacion.GestionCobranza.js"></script>
</asp:Content>
<asp:Content ID="bodyGestionCobranza" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogAgregarProximaGestion" title="Programar próxima gestión"></div>
    <div id="dialogAgregarFechaPago" title="Programar fecha de pago"></div>
    <div id="dialogConsultarGestionCobranzaSeguimientos" title="Seguimientos de gestión de cobranza"></div>
    <div id="dialogExportarSeguimientosCliente" title="Exportar seguimientos de clientes"></div>
    <div id="divContenido">
        <div id="divFiltrosGestionCobranza"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaExportarSeguiemientoCliente" value="Exportar todos los seguimientos" class="buttonLTR" />
            <input type="text" id="txtFacturasSeleccionadas" style="display:none;"/>
        </div>
        <div id="divGridGestionCobranza" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <table id="grdGestionCobranza"></table>
                <div id="pagGestionCobranza"></div>
            </div>
        </div>
    </div>
</asp:Content>
