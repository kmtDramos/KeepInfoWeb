<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="SolicitudPago.aspx.cs" Inherits="Paginas_SolicitudPago" Title="Solicitud de pago" %>
<asp:Content ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">
    
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <script type="text/javascript" src="../js/upload/fileuploader.js"></script>

    <script type="text/javascript" src="../js/Contabilidad.SolicitudPago.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div style="padding:0px 8px;">
        <div>
            <table>
                <tr>
                    <td><button id="btnAgregarSolicitudPago" class="buttonLTR">Agregar Solicitud de Pago</button></td>
                </tr>
            </table>
        </div>
        <hr />
        <div id="divGridSolicitudPago" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Solicitud de Pago-->
                <table id="grdSolicitudPago"></table>
                <div id="pagSolicitudPago"></div>
                <!--FIN DE GRID DE Cotizador-->
            </div>
        </div>
    </div>
</asp:Content>