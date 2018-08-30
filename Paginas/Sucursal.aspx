<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPageSeguridad.Master" AutoEventWireup="true" CodeFile="Sucursal.aspx.cs" Inherits="Sucursal" title="Sucursales"%>
<asp:Content ID="headCatalogoSucursal" ContentPlaceHolderID="headMasterPageSeguridad" runat="server">
    <!--The jQuery UI theme that will be used by the grid-->
    <link type="text/css" rel="stylesheet" href="../js/jqgrid/css/ui.jqgrid.css" />
    <!--jQuery runtime minified-->
    <script type="text/javascript" src="../js/jqgrid/js/i18n/grid.locale-es.js"></script>
    <script type="text/javascript" src="../js/jqgrid/js/jquery.jqGrid.min.js" ></script>
    <script type="text/javascript" src="../js/jqgrid/src/grid.custom.js" ></script>
    <!--jQuery-->
    <script type="text/javascript" src="../js/Catalogo.Sucursal.js"></script>
</asp:Content>
<asp:Content ID="bodySucursal" ContentPlaceHolderID="bodyMasterPageSeguridad" runat="server">
    <div id="dialogConsultarSucursal" title="Consultar sucursal"></div>
    <div id="dialogEditarSucursal" title="Editar sucursal"></div>
    <div id="dialogAgregarSucursal" title="Agregar sucursal"></div>
    <div id="dialogConsultarDivisionAsignada" title="Consultar División asignada"></div>
    <div id="dialogConsultarCuentaBancariaAsignada" title="Consultar cuenta bancaria asignada"></div>
    <div id="dialogConsultarSerieFacturaConsultar" title ="Consultar serie factura"></div>
    <div id="dialogConsultarSerieNotaCreditoConsultar" title ="Consultar serie nota de crédito"></div>
    <div id="dialogConsultarSeriePagoConsultar" title ="Consultar serie Complemento de pago"></div>
    <div id="dialogConsultarRutaCFDIConsultar" title ="Consultar ruta CFDI"></div>
    <div id="dialogEditarSerieFactura" title ="Editar serie factura"></div>
    <div id="dialogEditarSerieNotaCredito" title ="Editar serie nota de crédito"></div>
    <div id="dialogEditarSeriePago" title ="Editar serie Complemento de pago"></div>
    <div id="dialogEditarRutaCFDI" title ="Editar ruta CFDI"></div>
    <div id="dialogAsignarConexionContpaq" title ="Asignar conexión a CONTPAQ"></div>
    <div id="dialogAgregarCuentaBancaria" title="Agregar cuenta bancaria">
        <div id="divFormaAgregarCuentaBancaria"></div>
        <div id="divGridCuentaBancaria" class="divContGrid renglon-bottom">
            <div id="divContGridCuentaBancaria">
                <table id="grdCuentaBancaria"></table>
                <div id="pagCuentaBancaria"></div>
            </div>
        </div>    
    </div>
    
    <div id="dialogAgregarSerieFactura" title="Agregar serie de factura">
        <div id="divFormaAgregarSerieFactura"></div>
        <div id="divGridSerieFactura" class="divContGrid renglon-bottom">
            <div id="divContGridSerieFactura">
                <table id="grdSerieFactura"></table>
                <div id="pagSerieFactura"></div>
            </div>
        </div>    
    </div>
    
    <div id="dialogAgregarSerieNotaCredito" title="Agregar serie de nota de crédito">
        <div id="divFormaAgregarSerieNotaCredito"></div>
        <div id="divGridSerieNotaCredito" class="divContGrid renglon-bottom">
            <div id="divContGridSerieNotaCredito">
                <table id="grdSerieNotaCredito"></table>
                <div id="pagSerieNotaCredito"></div>
            </div>
        </div>    
    </div>
    
    <div id="dialogAgregarSeriePago" title="Agregar serie de complemento de pago">
        <div id="divFormaAgregarSeriePago"></div>
        <div id="divGridSeriePago" class="divContGrid renglon-bottom">
            <div id="divContGridSeriePago">
                <table id="grdSeriePago"></table>
                <div id="pagSeriePago"></div>
            </div>
        </div>    
    </div>

    <div id="dialogAgregarRutaCFDI" title="Agregar serie de nota de crédito">
        <div id="divFormaAgregarRutaCFDI"></div>
        <div id="divGridRutaCFDI" class="divContGrid renglon-bottom">
            <div id="divContGridRutaCFDI">
                <table id="grdRutaCFDI"></table>
                <div id="pagRutaCFDI"></div>
            </div>
        </div>    
    </div>
    
    
    <div id="dialogConsultarCuentaBancaria" title ="Cuentas Bancarias">
        <div id="divFormaConsultarCuentaBancaria"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarCuentaBancaria" value="+ Agregar cuenta bancaria" class="buttonLTR" />
        </div>
        <div id="divGridCuentaBancaria" class="divContGrid renglon-bottom">
            <div id="divContGridCuentaBancaria">
                <table id="grdCuentaBancaria"></table>
                <div id="pagCuentaBancaria"></div>
            </div>
        </div>
    </div>
    
    <div id="dialogConsultarSerieFactura" title ="Series de factura">
        <div id="divFormaConsultarSerieFactura"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarSerieFactura" value="+ Agregar Serie de factura" class="buttonLTR" />
        </div>
        <div id="divGridSerieFactura" class="divContGrid renglon-bottom">
            <div id="divContGridSerieFactura">
                <table id="grdSerieFactura"></table>
                <div id="pagSerieFactura"></div>
            </div>
        </div>
    </div>
    
     <div id="dialogConsultarSerieNotaCredito" title ="Series de notas de crédito">
        <div id="divFormaConsultarSerieNotaCredito"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarSerieNotaCredito" value="+ Agregar Serie de nota de crédito" class="buttonLTR" />
        </div>
        <div id="divGridSerieNotaCredito" class="divContGrid renglon-bottom">
            <div id="divContGridSerieNotaCredito">
                <table id="grdSerieNotaCredito"></table>
                <div id="pagSerieNotaCredito"></div>
            </div>
        </div>
    </div>

    <div id="dialogConsultarSeriePago" title ="Series de complemento de pago">
        <div id="divFormaConsultarSeriePago"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarSeriePago" value="+ Agregar Serie de Complemento de pago" class="buttonLTR" />
        </div>
        <div id="divGridSeriePago" class="divContGrid renglon-bottom">
            <div id="divContGridSeriePago">
                <table id="grdSeriePago"></table>
                <div id="pagSeriePago"></div>
            </div>
        </div>
    </div>
    
    <div id="dialogConsultarRutaCFDI" title ="Rutas CFDI">
        <div id="divFormaConsultarRutaCFDI"></div>
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarRutaCFDI" value="+ Agregar ruta CFDI" class="buttonLTR" />
        </div>
        <div id="divGridRutaCFDI" class="divContGrid renglon-bottom">
            <div id="divContGridRutaCFDI">
                <table id="grdRutaCFDI"></table>
                <div id="pagRutaCFDI"></div>
            </div>
        </div>
    </div> 
        
    <div id="divContenido">
        <div class="divAreaBotonesDialog">
            <input type="button" id="btnObtenerFormaAgregarSucursal" value="+ Agregar sucursal" class="buttonLTR" />
        </div>
        <div id="divGridSucursal" class="divContGrid renglon-bottom">
            <div id="divContGrid">               
                <table id="grdSucursal"></table>
                <div id="pagSucursal"></div>
            </div>
        </div>
    </div>
</asp:Content>
