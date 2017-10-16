<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Proveedor.aspx.cs" Inherits="Proveedor" Title="Proveedor" %>
<asp:Content ID="headProveedor" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
<script type="text/javascript" src="../js/librerias/jquery.maskedinput-1.2.2.min.js"></script>
<!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Proveedor.js"></script>
</asp:Content>
<asp:Content ID="bodyProveedor" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
<!--Dialogs-->
<div id="dialogAgregarProveedor" title ="Agregar proveedor"></div>
<div id="dialogAgregarDireccion" title ="Agregar dirección"></div>
<div id="dialogAgregarContacto" title ="Agregar contacto"></div>
<div id="dialogAgregarOrganizacionIVA" title ="Agregar IVA"></div>
<div id="dialogAgregarTelefono" title="Alta de teléfono"></div>
<div id="dialogAgregarTelefonoEditar" title="Alta de teléfono"></div>
<div id="dialogAgregarCorreo" title="Agregar de correo"></div>
<div id="dialogAgregarCorreoEditar" title="Agregar de correo"></div>
<div id="dialogAgregarTipoIndustria" title="Agregar tipo industria"></div>
<div id="dialogConsultarProveedor" title ="Consultar proveedor"></div>
<div id="dialogConsultarDireccion" title ="Consultar Direccion"></div>
<div id="dialogConsultarContacto" title ="Consultar Contacto"></div>
<div id="dialogConsultarOrganizacionIVA" title ="Consultar IVA"></div>
<div id="dialogConsultarProveedorAEnrolar" title ="Proveedor existente a enrolar"></div>
<div id="dialogEditarProveedor" title ="Editar proveedor"></div>
<div id="dialogEditarDireccion" title ="Editar dirección"></div>
<div id="dialogEditarContacto" title ="Editar contacto"></div>
<div id="dialogEditarOrganizacionIVA" title ="Editar IVA"></div>
<div id="dialogConsultarClienteAEnrolar" title ="Cliente existente a enrolar"></div>
<div id="dialogConsultarCliente" title ="Consultar cliente"></div>

<div id="dialogDirecciones" title ="Direcciones">
    <div id="divFormaDirecciones"></div>
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarDireccion" value="+ Agregar dirección" class="buttonLTR" />
    </div>
    <div id="divGridDirecciones" class="divContGrid renglon-bottom">
        <div id="divContGridDireccion">
            <!--INICIO GRID DE DIRECCIONES-->
            <table id="grdDirecciones"></table>
            <div id="pagDirecciones"></div>
            <!--FIN DE GRID DE DIRECCIONES-->
        </div>
    </div>
</div>

<div id="dialogContactos" title ="Contactos">
    <div id="divFormaContactos"></div>
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarContacto" value="+ Agregar contacto" class="buttonLTR" />
    </div>
    <div id="divGridContactos" class="divContGrid renglon-bottom">
        <div id="divContGridContacto">
            <!--INICIO GRID DE CONTACTOS-->
            <table id="grdContactos"></table>
            <div id="pagContactos"></div>
            <!--FIN DE GRID DE CONTACTOS-->
        </div>
    </div>
</div>
<div id="dialogOrganizacionIVA" title ="OrganizacionIVA">
    <div id="divFormaOrganizacionIVA"></div>
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarOrganizacionIVA" value="+ Agregar IVA" class="buttonLTR" />
    </div>
    <div id="divGridOrganizacionIVA" class="divContGrid renglon-bottom">
        <div id="divContGridOrganizacionIVA">
            <!--INICIO GRID DE OrganizacionIVA-->
            <table id="grdOrganizacionIVA"></table>
            <div id="pagOrganizacionIVA"></div>
            <!--FIN DE GRID DE OrganizacionIVA-->
        </div>
    </div>
</div>
<div id="divContenido">
    <div class="divAreaBotonesDialog">
        <input type="button" id="btnObtenerFormaAgregarProveedor" value="+ Agregar proveedor" class="buttonLTR" />
    </div>
    <div id="divGridProveedor" class="divContGrid renglon-bottom">
        <div id="divContGrid">
            <!--INICIO GRID DE PROVEEDOR-->
            <table id="grdProveedor"></table>
            <div id="pagProveedor"></div>
            <!--FIN DE GRID DE PROVEEDOR-->
        </div>
    </div>
</div>
</asp:Content>
