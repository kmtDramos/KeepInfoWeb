<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Cotizador.aspx.cs" Inherits="Paginas_Cotizador" Title="Cotizaciones" %>
<asp:Content ID="headCatalogoTiempoEntrega" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--<script src="../JS/librerias/jquery.iframer.min.js" type="text/javascript"></script> -->
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">
    
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <script type="text/javascript" src="../js/upload/fileuploader.js"></script>

    <!-- Cotizador -->
    <script type="text/javascript" src="../js/Ventas.Cotizador.js?_=<%=DateTime.Now.Ticks %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoOportunidad" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div style="padding: 10px;">
        <div>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <button id="btnAgregarCotizacion" class="buttonLTR" onclick="return false;">+ Agregar Cotización</button>
                        </td>
                        <td></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <hr />
        <div id="divGridCotizador" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE Cotizador-->
                <table id="grdCotizador"></table>
                <div id="pagCotizador"></div>
                <!--FIN DE GRID DE Cotizador-->
            </div>
        </div>
    </div>
</asp:Content>