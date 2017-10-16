<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Cotizacion.aspx.cs" Inherits="Cotizacion" Title="Cotización" %>
<asp:Content ID="headCatalogoCotizacion" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <script type="text/javascript"  src="../js/Catalogo.Cotizacion.js?_=<% Response.Write(ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoCotizacion" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
   <!--Dialogs-->
    <div id="dialogAgregarCotizacion" title ="Agregar cotización"></div>   
    <div id="dialogConsultarCotizacion" title ="Consultar cotización"></div>
    <div id="dialogEditarCotizacion" title ="Editar cotización">
        <a href="Cotizacion.aspx">Cotizacion.aspx</a></div>
    <div id="dialogGenerarPedido" title ="Convertir a Pedido"><p>¿Está seguro que desea convertir la cotización en pedido?</p></div>
    <div id="dialogGenerarCotizacion" title ="Generar Cotizacion"><p>¿Está seguro que desea generar la cotización?</p></div>
    <div id="dialogRegresarABorrador" title ="Regresar la cotización a borrador"><p>Al editar una cotización pasará a borrador y tendrá que volver a generar la cotización ¿Desea continuar?</p></div>
    <div id="dialogProporcionarClaveAutorizacion" title ="Proporcionar clave autorización"></div> 
    <div id="dialogProporcionarClaveAutorizacionIVA" title ="Proporcionar clave autorización IVA"></div>    
    <div id="dialogActivarCotizacionVencida" title ="Activar la cotización vencida"><div id="divFormaAgregarNuevaVigencia"><span class="spanObligatorio">*</span><span class="lblEtiqueta">Nueva fecha de vigencia:</span><br /><br /><input type="text" id="txtNuevaVigencia" class="txtCajaTexto" /></div></div>
    <div id="dialogRegresarACotizacion" title ="Regresar el pedido a cotización" puedePasarPedidoACotizado="<%=puedePasarPedidoACotizado%>"><p>¿Está seguro que desea regresar el pedido a cotización?</p></div>
    <div id="dialogDeclinarCotizacion" title="Declinar cotizacion"></div>
    <div id="divContenido">
	<div id="divCotizacionTotalesEstatus">
            <table id="tblCotizacionTotalesEstatus" idEstatusCotizacionSeleccionado="" puedeEditarVigenciaCotizacion="<%=puedeEditarVigenciaCotizacion%>">
                <tr>
                    <td>
                        <div class="divCotizacionTituloTotal">Borradores </div>
                    </td>
                    <td>
                        <div class="divCotizacionTituloTotal">Cotizados</div>
                    </td>
                    <td>
                        <div class="divCotizacionTituloTotal">Pedidos</div>
                    </td>
                    <td>
                        <div class="divCotizacionTituloTotal">Vencidos</div>
                    </td>
                    <td>
                        <div class="divCotizacionTituloTotal">Bajas</div>
                    </td>                     
                    <td>
                        <div class="divCotizacionTituloTotal">Total</div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <span id ="span-E1" title="Filtrar por borrador" class="spanFiltroTotal" IdEstatusCotizacion="1">
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E2" title="Filtrar por cotizados" class="spanFiltroTotal" IdEstatusCotizacion="2">
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E3" title="Filtrar por pedidos" class="spanFiltroTotal" IdEstatusCotizacion="3" >
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E5" title="Filtrar por vencidos" class="spanFiltroTotal" IdEstatusCotizacion="5" >
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E4" title="Filtrar por bajas" class="spanFiltroTotal" IdEstatusCotizacion="4" >
                        0</span>
                    </td>
                    <td>
                        <span id ="span-E0" title="Todos" class="spanFiltroTotal" IdEstatusCotizacion="-1">
                        0</span>
                    </td>
                </tr>
             </table>
        </div>
        <div id="divFiltrosCotizacion"></div>
        <div class="divAreaBotonesDialog">
            <%= puedeAgregarCotizacion == 1 ? "<input type='button' id='btnObtenerFormaAgregarCotizacion' value='+ Agregar cotización' class='buttonLTR' />" : "" %>
        </div>
        <div id="divGridCotizacion" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <table id="grdCotizacion"></table>
                <div id="pagCotizacion"></div>                
            </div>
        </div>
        
    </div>
    <asp:Button ID="btnDescarga" UseSubmitBehavior="false" style="visibility: hidden; display: none;" Text="Descarga" Font-Bold="true" runat="server" OnClick="btnDescarga_Click" />    
</asp:Content>
