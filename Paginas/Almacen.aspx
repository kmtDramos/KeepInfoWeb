<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Almacen.aspx.cs" Inherits="Almacen" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Almacen.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogAgregarAlmacen" title="Agregar almacén"></div>
    <div id="dialogConsultarAlmacen" title="Conusltar almacén"></div>
    <div id="dialogEditarAlmacen" title="Editar almacén"></div>    
    <div id="dialogConsultarSucursalAsignada" title="Consultar sucursal asignada"></div>    
        
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarAlmacen" value="+ Agregar almacén" class="buttonLTR" />
        </div>
        <div id="divGridAlmacen" class="divContGrid renglon-bottom">
            <div id="divContGrid">               
                <table id="grdAlmacen"></table>
                <div id="pagAlmacen"></div>
            </div>
        </div>
    </div>
</asp:Content>
