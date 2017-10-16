<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Servicio.aspx.cs" Inherits="Servicio" Title="Servicios" %>
<asp:Content ID="headServicio" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Servicio.js"></script>
</asp:Content>
<asp:Content ID="bodyServicio" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarServicio" title ="Agregar servicio"></div>
    <div id="dialogConsultarServicio" title ="Consultar servicio"></div>
    <div id="dialogConsultarServicioAEnrolar" title ="Servicio existente a enrolar"></div>
    <div id="dialogEditarServicio" title ="Editar servicio"></div>
    <div id="dialogAgregarDescuentoServicio" title="Agregar descuento"></div>
    <div id="dialogConsultarDescuentoServicio" title ="Consultar descuento">
        <div class="divAreaBotonesDialog">
            <%=btnAgregarDescuentoServicio%>
        </div>
        <div id="divGridDescuentos" class="divContGrid renglon-bottom">
            <div id="divContGrid2">
                <!--INICIO GRID DE DESCUENTOS-->
                <table id="grdDescuentoServicio"></table>
                <div id="pagDescuentoServicio"></div>
                <!--FIN DE GRID DE DESCUENTOS-->
            </div>
        </div>
    </div>
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <%=btnObtenerFormaAgregarServicio%>
        </div>
        <div id="divGridServicio" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdServicio"></table>
                <div id="pagServicio"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>
</asp:Content>
