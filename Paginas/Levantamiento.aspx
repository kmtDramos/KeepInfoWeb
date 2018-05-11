<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Levantamiento.aspx.cs" Inherits="Levantamiento" Title="Levantamiento" %>
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
    <script type="text/javascript"  src="../js/Levantamiento.js?_=<% Response.Write(ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoLevantamiento" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarLevantamiento" title ="Agregar Levantamiento"></div> 
    <div id="dialogConsultarLevantamiento" title ="Consultar Levantamiento"></div> 
    <div id="dialogEditarLevantamiento" title ="Editar Levantamiento"></div> 
    <div id="divContenido">
        <div id="divLevantamientoTotalesEstatus">
            <table id="tblLevantamientoTotalesEstatus" idEstatusLevantamientoSeleccionado="">
                <tr>
                    <td>
                        <div class="divLevantamientoTituloTotal">En Espera</div>
                    </td>
                    <td>
                        <div class="divLevantamientoTituloTotal">Cotizados</div>
                    </td>
                    <td>
                        <div class="divLevantamientoTituloTotal">Vencidos</div>
                    </td>
                    <td>
                        <div class="divLevantamientoTituloTotal">Bajas</div>
                    </td>                     
                    <td>
                        <div class="divLevantamientoTituloTotal">Total</div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span id ="span-E1" title="Filtrar por enEspera" class="spanFiltroTotal" idEstatusLevantamiento="1">
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E2" title="Filtrar por cotizados" class="spanFiltroTotal" idEstatusLevantamiento="2">
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E4" title="Filtrar por vencidos" class="spanFiltroTotal" idEstatusLevantamiento="4" >
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E3" title="Filtrar por baja" class="spanFiltroTotal" idEstatusLevantamiento="3" >
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E-1" title="Todos" class="spanFiltroTotal" idEstatusLevantamiento="-1">
                        0</span>
                    </td>
                </tr>
             </table>
        </div>
        <div id="divFiltrosLevantamiento"></div>
        <div class="divAreaBotonesDialog">
            <input type='button' id='btnObtenerFormaAgregarLevantamiento' value='+ Agregar Levantamiento' class='buttonLTR' />
        </div>
        <div id="divGridLevantamiento" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <table id="grdLevantamiento"></table>
                <div id="pagLevantamiento"></div>                
            </div>
        </div>
    </div>
</asp:Content>