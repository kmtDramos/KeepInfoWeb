<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Cliente.aspx.cs" Inherits="Cliente" Title="Clientes" %>
<asp:Content ID="headCatalogoCliente" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <script type="text/javascript" src="../js/jquery-unified-export-file-v1.0/jquery-unified-export-file-1.0.min.js"></script>
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript"  src="../js/JSON/json2.js"></script>
    <!--Actividades-->
    <link rel="Stylesheet" type="text/css" href="../CSS/actividad.css" />
    <script type="text/javascript" src="../js/datetimepicker.js"></script>
    <script type="text/javascript"  src="../js/Operacion.Actividad.js?_=<% Response.Write(DateTime.Now.Ticks); %>"></script>
    <script type="text/javascript"  src="../js/Catalogo.Cliente.js?_=<% Response.Write(DateTime.Now.Ticks); %>"></script>
</asp:Content>
<asp:Content ID="bodyCatalogoCliente" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <!--Dialogs-->
    <div id="dialogAgregarCliente" title ="Agregar cliente"></div>
    <div id="dialogAgregarDireccion" title ="Agregar dirección"></div>
    <div id="dialogAgregarCuentaBancariaCliente" title ="Agregar cuenta bancaria del cliente"></div>
    <div id="dialogAgregarContacto" title ="Agregar contacto"></div>
    <div id="dialogAgregarTelefono" title="Alta de teléfono"></div>
    <div id="dialogAgregarTelefonoEditar" title="Alta de teléfono"></div>
    <div id="dialogAgregarCorreo" title="Agregar correo"></div>
    <div id="dialogAgregarCorreoEditar" title="Editar correo"></div>
    <div id="dialogAgregarDescuentoCliente" title="Agregar descuento a cliente"></div>
    <div id="dialogConsultarCliente" title ="Consultar cliente"></div>
    <div id="dialogConsultarDireccion" title ="Consultar dirección"></div>
    <div id="dialogConsultarContacto" title ="Consultar contacto"></div>
    <div id="dialogConsultarClienteAEnrolar" title ="Cliente existente a enrolar"></div>
    <div id="dialogConsultarProveedorAEnrolar" title ="Proveedor existente a enrolar"></div>
    <div id="dialogConsultarDescuentoCliente" title="Descuentos">
        <div id="divFormaDescuentos"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnAgregarDescuentoCliente" value="+ Agregar descuento" class="buttonLTR" />
        </div>
        <div id="divGridDescuentos" class="divContGrid renglon-bottom">
            <div id="divContGridCliente">
                <!--INICIO GRID DE DESCUENTO DE CLIENTE-->
                <table id="grdDescuentoCliente"></table>
                <div id="pagDescuentoCliente"></div>
                <!--FIN DE GRID DE DESCUENTO DE CLIENTE-->
            </div>
        </div>
    </div>
    <div id="dialogEditarCliente" title ="Editar cliente"></div>
    <div id="dialogEditarDireccion" title ="Editar dirección"></div>
    <div id="dialogEditarContacto" title ="Editar contacto"></div> 
    <div id="dialogConsultarCuentaBancariaCliente" title ="Consultar cuenta bancaria del cliente"></div>
     <div id="dialogEditarCuentaBancariaCliente" title ="Editar cuenta bancaria del cliente"></div> 
       
    <div id="dialogDirecciones" title ="Direcciones">   
        <div id="divFormaDirecciones"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarDireccion" value="+ Agregar dirección" class="buttonLTR" />
        </div>
        <div id="divGridDirecciones" class="divContGrid renglon-bottom">
            <div id="divContGridDireccion">
                <!--INICIO GRID DE MEDIO ENTERO-->
                <table id="grdDirecciones"></table>
                <div id="pagDirecciones"></div>
                <!--FIN DE GRID DE MEDIO ENTERO-->
            </div>
        </div>
    </div>
    
    <div id="dialogCuentaBancariaCliente" title ="Cuentas bancarias del cliente">   
        <div id="divFormaCuentaBancariaCliente"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarCuentaBancariaCliente" value="+ Agregar cuenta bancaria" class="buttonLTR" />
        </div>
        <div id="divGridCuentaBancariaCliente" class="divContGrid renglon-bottom">
            <div id="divContGridCuentaBancariaCliente">
                <!--INICIO GRID CUENTA BANCARIA-->
                <table id="grdCuentaBancariaCliente"></table>
                <div id="pagCuentaBancariaCliente"></div>
                <!--FIN DE GRID CUENTA BANCARIA-->
            </div>
        </div>
    </div>
    
    <div id="dialogContactos" title ="Contactos">
        <div id="divFormaContactos"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarContactoOrganizacion" value="+ Agregar contacto" class="buttonLTR" />
        </div>
        <div id="divGridContactoOrganizacion" class="divContGrid renglon-bottom">
            <div id="divContContactoOrganizacion">
                <!--INICIO GRID DE CONTACTOS-->
                <table id="grdContactoOrganizacion"></table>
                <div id="pagContactoOrganizacion"></div>
                <!--FIN DE GRID DE CONTACTOS-->
            </div>
        </div>
    </div>
    <div id="divContenido">
        <table cellpadding="3">
            <tbody>
                <tr>
                    <td>
                        <div id="divAreaBotonesDialog" class="divAreaBotonesDialog" runat="server">
                            <input type="button" id="btnObtenerFormaAgregarCliente" value="+ Agregar cliente" class="buttonLTR" />
                        </div>
                    </td>
                    <td><span class="lblEtiqueta">Clientes con oportunidad: <span id="ConOportunidades"></span></span></td>
                    <td><span class="lblEtiqueta">Sin oportunidad: <span id="SinOportunidades"></span></span></td>
                    <td>
                        <input type="checkbox" id="ckbVerTodosClinetes" />
                        <label for="ckbVerTodosClinetes"><strong>Ver todos los clientes</strong></label>
                    </td>
                </tr>
            </tbody>
        </table>
        <div id="divGridCliente" class="divContGrid renglon-bottom">
            <div id="divContGrid">
                <table id="grdCliente"></table>
                <div id="pagCliente"></div>
            </div>
        </div>
    </div>
</asp:Content>