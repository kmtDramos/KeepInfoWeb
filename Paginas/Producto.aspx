<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Producto.aspx.cs" Inherits="Producto" Title="Productos" %>
<asp:Content ID="headProducto" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <link href="../js/upload/fileuploader.css" rel="stylesheet" type="text/css">	
    <script src="../js/upload/fileuploader.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Catalogo.Producto.js?_=<% Response.Write(ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyProducto" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogConsultarProducto" title="Consultar producto"></div>
    <div id="dialogAgregarProducto" title="Agregar producto"></div>
    <div id="dialogEditarProducto" title="Editar producto"></div>
    <div id="dialogAgregarCaracteristica" title="Agregar caracteristica"></div>
    <div id="dialogConsultarDescuentoProducto" title="Descuentos">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnAgregarDescuentoProducto" value="+ Agregar descuento" class="buttonLTR" />
        </div>
        <div id="divGridDescuentos" class="divContGrid renglon-bottom">
            <div id="divContGrid2">
                <!--INICIO GRID DE DESCUENTO DE PRODUCTO-->
                <table id="grdDescuentoProducto"></table>
                <div id="pagDescuentoProducto"></div>
                <!--FIN DE GRID DE DESCUENTO DE PRODUCTO-->
            </div>
        </div>
    </div>
    <div id="dialogAgregarDescuentoProducto" title="Agregar descuento"></div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarProducto" value="+ Agregar producto" class="buttonLTR" />
        </div>
        <div id="divGridSucursal" class="divContGrid renglon-bottom">
            <div id="divContGrid">               
                <table id="grdProducto"></table>
                <div id="pagProducto"></div>
            </div>
        </div>
    </div>
</asp:Content>
