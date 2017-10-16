<%@ Page Title="Remisión" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Remision.aspx.cs" Inherits="Remision" %>
<asp:Content ID="headRemision" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Operacion.Remision.js"></script>
</asp:Content>
<asp:Content ID="bodyRemision" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogAgregarRemision" title ="Agregar remisión"></div>
    <div id="dialogConsultarEncabezadoRemision" title ="Consultar remisión"></div>
    <div id="dialogEditarEncabezadoRemision" title ="Editar remisión"></div>
    <div id="dialogSeleccionarAlmacenRemision" title ="Seleccionar almacén"></div>
    <div id="dialogCancelarRemision" title ="Cancelar remisión"><p>¿Está seguro que desea cancelar la remisión?</p></div>
    <div id="dialogDetallePedido">
        <div id="divFormaDetallePedido"></div>
        <div id="divGridDetallePedido" class="divContGrid renglon-bottom">
            <div id="divContGridDetallePedido">
                <!--INICIO GRID DETALLE PEDIDO-->
                <table id="grdDetallePedido"></table>
                <div id="pagDetallePedido"></div>
                <!--FIN DE GRID DETALLE PEDIDO-->
            </div>
        </div>
    </div>
    <div id="divContenido">
        <div id="divTotalesEstatus">
            <table id="tblTotalesEstatus" idEstatusRemisionSeleccionado="">
                <tr>                    
                    <td>
                        <div class="divTituloTotal">Canceladas</div>
                    </td>
                    <td>
                        <div class="divTituloTotal">Activas</div>
                    </td>                                        
                    <td>
                        <div class="divTituloTotal">Total</div>
                    </td>
                </tr>
                <tr>                    
                    <td>
                        <span id ="span-E1" title="Filtrar por canceladas" class="spanFiltroTotal" IdEstatusRemision="1">
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E0" title="Filtrar por activas" class="spanFiltroTotal" IdEstatusRemision="0" >
                        0</span>
                    </td>                    
                    <td>
                        <span id ="span-E4" title="Todos" class="spanFiltroTotal" IdEstatusRemision="-1">
                        0</span>
                    </td>
                </tr>
             </table>
        </div>
        <div id="divFiltrosEncabezadoRemision"></div>      
        <div class="divAreaBotonesDialog">
            <%= puedeAgregarEncabezadoRemision == 1 ? "<input type='button' id='btnObtenerFormaAgregarRemision' value='+ Agregar remisi&oacute;n' class='buttonLTR'/>" : ""%>
        </div>
        <div id="divGridEncabezadoRemision" class="divContGrid renglon-bottom">
            <div id="divContGrid">               
                <table id="grdEncabezadoRemision"></table>
                <div id="pagEncabezadoRemision"></div>
            </div>
        </div>
    </div>
    <asp:Button ID="btnDescarga" UseSubmitBehavior="false" style="visibility: hidden; display: none;" Text="Descarga" Font-Bold="true" runat="server" OnClick="btnDescarga_Click" />
</asp:Content>
