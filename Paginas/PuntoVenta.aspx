<%@ Page Title="Punto de venta" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="PuntoVenta.aspx.cs" Inherits="Paginas_PuntoVenta"%>
<asp:Content ID="headPuntoVenta" ContentPlaceHolderID="headMasterPageSeguridad" Runat="Server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Operacion.PuntoVenta.js"></script>
</asp:Content>
<asp:Content ID="bodyPuntoVenta" ContentPlaceHolderID="bodyMasterPageSeguridad" Runat="Server">        
    <div id="divContenido" class="divFormaPuntoVenta" style="padding-bottom: 20px;">
        <div id="divAreaBotonesPuntoVenta"></div>
        <div style="border:1px solid #d3d3d3; border-radius:3px;">
            <div style="overflow:auto;">
                <div style="float:left;">
                     <table>
                        <tr>
                            <td>
                                <input type="text" placeholder="Ingresar codigo de barras" style="width:365px;" /> <img src="../images/search.png" style="cursor:pointer;" title="Buscar por nombre del producto"/>
                            </td>
                        </tr>
                    </table>
                    <table id="Table1">
                        <tr>
                            <td class="tblFichaProducto-col-1" colspan="2">
                                <span class="spanBold">Producto:</span>
                            </td>
                            <td class="tblFichaProducto-col-1" colspan="2">
                                Taladro
                            </td>
                        </tr>
                        <tr>
                            <td class="tblFichaProducto-col-1" colspan="2">
                                <span class="spanBold">Marca:</span>
                            </td>
                            <td class="tblFichaProducto-col-1" colspan="2">
                                ToolKraft
                            </td>
                        </tr>
                    </table>              
                    <table id="tblProducto">
                        <tr>
                            <td>
                                <div id="divImagenProducto" archivo="">
                                    <div class="divImagenSombra"><img class="imagenNoDisponible" src="../images/NoImage.png"/></div>
                                </div>
                                <div id="divSubirImagenProducto">
		                            <noscript>
			                            <p>Favor de habilitar JavaScript para poder subir la imagen.</p>
			                            <!-- or put a simple form for upload here -->
		                            </noscript>
	                            </div>
                            </td>
                            <td>
                                <table id="tblFichaProducto">
                                    <tr>
                                        <td class="tblFichaProducto-col-1"><span class="spanBold">Existencia:</span> 5</td>
                                    </tr>
                                    <tr>
                                        <td class="tblFichaProducto-col-1"><span class="spanBold">Medida:</span> pieza</td>
                                    </tr>
                                   
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table id="Table2" style="font-size:16px;">
                        <tr>
                            <td class="tblFichaProducto-col-1"  style="text-align:right;width:200px;">
                                <span class="spanBold">
                                    Precio unitario:
                                <span class="spanBold">
                            </td>
                            <td style="text-align:right;width:200px;border-bottom:1px solid black;">
                                $ 2,000.00
                            </td>
                        </tr>
                        <tr>
                            <td class="tblFichaProducto-col-1" style="text-align:right;width:200px;">
                                <span class="spanBold">
                                    IVA:
                                <span class="spanBold">
                            </td>
                            <td style="text-align:right;width:200px;border-bottom:1px solid black;">
                               $ 320.00
                            </td>
                        </tr>
                        <tr>
                            <td class="tblFichaProducto-col-1" style="text-align:right;width:200px;">
                                <span class="spanBold">
                                    Precio total:
                                <span class="spanBold">
                            </td>
                            <td style="text-align:right;width:200px;border-bottom:1px solid black;">
                                $ 2,320.00
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float:left;">
                    <div id="divGridDetalleVenta" class="divContGrid renglon-bottom">
                        <div id="divContGrid">               
                            <table id="grdDetalleVenta"></table>
                            <div id="pagDetalleVenta"></div>
                        </div>
                    </div>
                </div>
            </div>
            <table id="tblFormaPago">
                <tr>
                    <td class="tblFormaPago-col-1" style="width:452px;">
                        <div style="border:1px solid #d3d3d3; padding:5px; font-size:22px;">
                            <span class="spanBold">Ahorro: </span> $ 0.00
                        </div>
                    </td>
                    <td class="tblFormaPago-col-2" style="width:452px;">
                        <div style="border:1px solid #d3d3d3; padding:5px; font-size:22px;" >
                            <span class="spanBold">Total: </span> $ 145.00
                        </div>
                    </td>
                </tr>
            </table>
            <table id="Table3" style="float:right; margin-top:15px;">
                <tr>
                    <td class="tblFormaPago-col-1" style="width:452px;">
                        <input type="button" id="btnProcesarVenta" value="Procesar venta" class="buttonRTL" style="font-size:16px; float:right;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>