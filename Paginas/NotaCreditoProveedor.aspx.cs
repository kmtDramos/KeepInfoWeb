using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class NotaCreditoProveedor : System.Web.UI.Page
{
    public static int puedeAgregarNotaCreditoProveedor = 0;
    public static int puedeEditarNotaCreditoProveedor = 0;
    public static int puedeEliminarNotaCreditoProveedor = 0;
    public static int puedeEliminarCobroNotaCreditoProveedor = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CEmpresa Empresa = new CEmpresa();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        puedeAgregarNotaCreditoProveedor = Usuario.TienePermisos(new string[] { "puedeAgregarNotaCreditoProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarNotaCreditoProveedor = Usuario.TienePermisos(new string[] { "puedeEditarNotaCreditoProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarNotaCreditoProveedor = Usuario.TienePermisos(new string[] { "puedeEliminarNotaCreditoProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarCobroNotaCreditoProveedor = Usuario.TienePermisos(new string[] { "puedeEliminarCobroNotaCreditoProveedor" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridNotaCreditoProveedor
        CJQGrid GridNotaCreditoProveedor = new CJQGrid();
        GridNotaCreditoProveedor.NombreTabla = "grdNotaCreditoProveedor";
        GridNotaCreditoProveedor.CampoIdentificador = "IdNotaCreditoProveedor";
        GridNotaCreditoProveedor.ColumnaOrdenacion = "Descripcion";
        GridNotaCreditoProveedor.Metodo = "ObtenerNotaCreditoProveedor";
        GridNotaCreditoProveedor.GenerarFuncionFiltro = false;
        GridNotaCreditoProveedor.TituloTabla = "Notas de crédito de proveedor";

        //IdNotaCreditoProveedor
        CJQColumn ColIdNotaCreditoProveedor = new CJQColumn();
        ColIdNotaCreditoProveedor.Nombre = "IdNotaCreditoProveedor";
        ColIdNotaCreditoProveedor.Oculto = "true";
        ColIdNotaCreditoProveedor.Encabezado = "IdNotaCreditoProveedor";
        ColIdNotaCreditoProveedor.Buscador = "false";
        GridNotaCreditoProveedor.Columnas.Add(ColIdNotaCreditoProveedor);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "true";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridNotaCreditoProveedor.Columnas.Add(ColSucursal);

        //SerieNotaCreditoProveedor
        CJQColumn ColSerieNotaCreditoProveedor = new CJQColumn();
        ColSerieNotaCreditoProveedor.Nombre = "SerieNotaCredito";
        ColSerieNotaCreditoProveedor.Encabezado = "Serie";
        ColSerieNotaCreditoProveedor.Ancho = "50";
        ColSerieNotaCreditoProveedor.Buscador = "true";
        ColSerieNotaCreditoProveedor.Alineacion = "left";
        GridNotaCreditoProveedor.Columnas.Add(ColSerieNotaCreditoProveedor);

        //FolioNotaCreditoProveedor
        CJQColumn ColFolioNotaCreditoProveedor = new CJQColumn();
        ColFolioNotaCreditoProveedor.Nombre = "FolioNotaCredito";
        ColFolioNotaCreditoProveedor.Encabezado = "Folio";
        ColFolioNotaCreditoProveedor.Ancho = "50";
        ColFolioNotaCreditoProveedor.Buscador = "true";
        ColFolioNotaCreditoProveedor.Alineacion = "left";
        GridNotaCreditoProveedor.Columnas.Add(ColFolioNotaCreditoProveedor);

        //Importe
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Formato = "FormatoMoneda";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "80";
        GridNotaCreditoProveedor.Columnas.Add(ColTotal);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "80";
        GridNotaCreditoProveedor.Columnas.Add(ColTipoMoneda);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "FechaNotaCreditoProveedor";
        ColFecha.Encabezado = "FechaNotaCreditoProveedor";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridNotaCreditoProveedor.Columnas.Add(ColFecha);

        //Descripcion
        CJQColumn ColDescripcion = new CJQColumn();
        ColDescripcion.Nombre = "Descripcion";
        ColDescripcion.Encabezado = "Descripción";
        ColDescripcion.Ancho = "150";
        ColDescripcion.Alineacion = "left";
        ColDescripcion.Buscador = "false";
        GridNotaCreditoProveedor.Columnas.Add(ColDescripcion);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "150";
        GridNotaCreditoProveedor.Columnas.Add(ColRazonSocial);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        //ColBaja.Oculto = puedeEliminarNotaCreditoProveedor == 1 ? "false" : "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarNotaCreditoProveedor == 1 ? "A/I" : "A/IDeshabilitado"; 
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridNotaCreditoProveedor.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarNotaCreditoProveedor";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridNotaCreditoProveedor.Columnas.Add(ColConsultar);

        ////Formato
        //CJQColumn ColFormato = new CJQColumn();
        //ColFormato.Nombre = "Formato";
        //ColFormato.Encabezado = "Imprimir";
        //ColFormato.Etiquetado = "Imagen";
        //ColFormato.Imagen = "imprimir.png";
        //ColFormato.Estilo = "divImagenConsultar imgFormaConsultarFacturaFormato";
        //ColFormato.Buscador = "false";
        //ColFormato.Ordenable = "false";
        //ColFormato.Ancho = "50";
        //GridNotaCreditoProveedor.Columnas.Add(ColFormato);  

        ClientScript.RegisterStartupScript(this.GetType(), "grdNotaCreditoProveedor", GridNotaCreditoProveedor.GeneraGrid(), true);

        //GridFacturas
        CJQGrid GridFacturas = new CJQGrid();
        GridFacturas.NombreTabla = "grdFacturas";
        GridFacturas.CampoIdentificador = "IdEncabezadoFacturaProveedor";
        GridFacturas.ColumnaOrdenacion = "IdEncabezadoFacturaProveedor";
        GridFacturas.TipoOrdenacion = "DESC";
        GridFacturas.Metodo = "ObtenerFacturas";
        GridFacturas.TituloTabla = "Facturas";
        GridFacturas.GenerarFuncionFiltro = false;
        GridFacturas.GenerarFuncionTerminado = false;
        GridFacturas.Altura = 120;
        GridFacturas.Editable = true;
        GridFacturas.NumeroRegistros = 15;
        GridFacturas.RangoNumeroRegistros = "15,30,60";

        //IdFactura
        CJQColumn ColIdFactura = new CJQColumn();
        ColIdFactura.Nombre = "IdEncabezadoFacturaProveedor";
        ColIdFactura.Oculto = "true";
        ColIdFactura.Encabezado = "IdEncabezadoFacturaProveedor";
        ColIdFactura.Buscador = "false";
        GridFacturas.Columnas.Add(ColIdFactura);

        //Fecha factura
        CJQColumn ColFechaFactura = new CJQColumn();
        ColFechaFactura.Nombre = "Fecha";
        ColFechaFactura.Encabezado = "Fecha";
        ColFechaFactura.Buscador = "false";
        ColFechaFactura.Alineacion = "left";
        ColFechaFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColFechaFactura);

        //NumeroFactura
        CJQColumn ColNumeroFactura = new CJQColumn();
        ColNumeroFactura.Nombre = "NumeroFactura";
        ColNumeroFactura.Encabezado = "Número de factura";
        ColNumeroFactura.Buscador = "true";
        ColNumeroFactura.Alineacion = "left";
        ColNumeroFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColNumeroFactura);

        //Total factura
        CJQColumn ColTotalFactura = new CJQColumn();
        ColTotalFactura.Nombre = "Total";
        ColTotalFactura.Encabezado = "Total";
        ColTotalFactura.Buscador = "false";
        ColTotalFactura.Formato = "FormatoMoneda";
        ColTotalFactura.Alineacion = "right";
        ColTotalFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColTotalFactura);

        //Tipo moneda
        CJQColumn ColTipoMonedaFactura = new CJQColumn();
        ColTipoMonedaFactura.Nombre = "TipoMoneda";
        ColTipoMonedaFactura.Encabezado = "Tipo de moneda";
        ColTipoMonedaFactura.Buscador = "false";
        ColTipoMonedaFactura.Alineacion = "left";
        ColTipoMonedaFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColTipoMonedaFactura);

        // Tipo de cambio
        CJQColumn ColTipoCambioFactura = new CJQColumn();
        ColTipoCambioFactura.Nombre = "TipoCambioFactra";
        ColTipoCambioFactura.Encabezado = "Tipo de cambio";
        ColTipoCambioFactura.Buscador = "false";
        ColTipoCambioFactura.Alineacion = "left";
        ColTipoCambioFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColTipoCambioFactura);

        //Abono
        CJQColumn ColAbonoFactura = new CJQColumn();
        ColAbonoFactura.Nombre = "Abono";
        ColAbonoFactura.Encabezado = "Introducir abono";
        ColAbonoFactura.Buscador = "false";
        ColAbonoFactura.Editable = "true";
        ColAbonoFactura.Alineacion = "right";
        ColAbonoFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColAbonoFactura);

        //Saldo factura
        CJQColumn ColSaldoFactura = new CJQColumn();
        ColSaldoFactura.Nombre = "Saldo";
        ColSaldoFactura.Encabezado = "Saldo";
        ColSaldoFactura.Buscador = "false";
        ColSaldoFactura.Formato = "FormatoMoneda";
        ColSaldoFactura.Alineacion = "right";
        ColSaldoFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColSaldoFactura);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturas", GridFacturas.GeneraGrid(), true);

        //GridMovimientosCobros
        CJQGrid grdMovimientosCobros = new CJQGrid();
        grdMovimientosCobros.NombreTabla = "grdMovimientosCobros";
        grdMovimientosCobros.CampoIdentificador = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        grdMovimientosCobros.ColumnaOrdenacion = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        grdMovimientosCobros.TipoOrdenacion = "DESC";
        grdMovimientosCobros.Metodo = "ObtenerMovimientosCobros";
        grdMovimientosCobros.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobros.GenerarFuncionFiltro = false;
        grdMovimientosCobros.GenerarFuncionTerminado = false;
        grdMovimientosCobros.Altura = 120;
        grdMovimientosCobros.NumeroRegistros = 15;
        grdMovimientosCobros.RangoNumeroRegistros = "15,30,60";

        //IdNotaCreditoProveedorEncabezadoFacturaProveedor
        CJQColumn ColIdNotaCreditoProveedorEncabezadoFacturaProveedor = new CJQColumn();
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedor.Nombre = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedor.Oculto = "true";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedor.Encabezado = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedor.Buscador = "false";
        grdMovimientosCobros.Columnas.Add(ColIdNotaCreditoProveedorEncabezadoFacturaProveedor);

        //Serie
        CJQColumn ColSerieMovimiento = new CJQColumn();
        ColSerieMovimiento.Nombre = "Serie";
        ColSerieMovimiento.Encabezado = "Serie";
        ColSerieMovimiento.Buscador = "false";
        ColSerieMovimiento.Alineacion = "left";
        ColSerieMovimiento.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColSerieMovimiento);

        //Folio
        CJQColumn ColFolioMovimiento = new CJQColumn();
        ColFolioMovimiento.Nombre = "Folio";
        ColFolioMovimiento.Encabezado = "Folio";
        ColFolioMovimiento.Buscador = "false";
        ColFolioMovimiento.Alineacion = "left";
        ColFolioMovimiento.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColFolioMovimiento);

        //Fecha pago
        CJQColumn ColFechaPago = new CJQColumn();
        ColFechaPago.Nombre = "FechaPago";
        ColFechaPago.Encabezado = "FechaPago";
        ColFechaPago.Buscador = "false";
        ColFechaPago.Alineacion = "left";
        ColFechaPago.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColFechaPago);

        //Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Buscador = "false";
        ColMonto.Formato = "FormatoMoneda";
        ColMonto.Alineacion = "right";
        ColMonto.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColMonto);

        //Eliminar movimiento
        CJQColumn ColEliminarMovimiento = new CJQColumn();
        ColEliminarMovimiento.Nombre = "Eliminar";
        ColEliminarMovimiento.Encabezado = "Eliminar";
        ColEliminarMovimiento.Etiquetado = "Imagen";
        ColEliminarMovimiento.Imagen = "eliminar.png";
        ColEliminarMovimiento.Estilo = "divImagenConsultar imgEliminarMovimiento";
        ColEliminarMovimiento.Buscador = "false";
        ColEliminarMovimiento.Ordenable = "false";
        ColEliminarMovimiento.Oculto = puedeEliminarCobroNotaCreditoProveedor == 1 ? "false" : "true";
        ColEliminarMovimiento.Ancho = "25";
        grdMovimientosCobros.Columnas.Add(ColEliminarMovimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobros", grdMovimientosCobros.GeneraGrid(), true);

        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 670;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdNotaCreditoProveedorEncabezadoFacturaProveedor
        CJQColumn ColIdNotaCreditoProveedorEncabezadoFacturaProveedorConsultar = new CJQColumn();
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorConsultar.Nombre = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorConsultar.Oculto = "true";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorConsultar.Encabezado = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdNotaCreditoProveedorEncabezadoFacturaProveedorConsultar);

        //Serie
        CJQColumn ColSerieMovimientoConsultar = new CJQColumn();
        ColSerieMovimientoConsultar.Nombre = "Serie";
        ColSerieMovimientoConsultar.Encabezado = "Serie";
        ColSerieMovimientoConsultar.Buscador = "false";
        ColSerieMovimientoConsultar.Alineacion = "left";
        ColSerieMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColSerieMovimientoConsultar);

        //Folio
        CJQColumn ColFolioMovimientoConsultar = new CJQColumn();
        ColFolioMovimientoConsultar.Nombre = "Folio";
        ColFolioMovimientoConsultar.Encabezado = "Folio";
        ColFolioMovimientoConsultar.Buscador = "false";
        ColFolioMovimientoConsultar.Alineacion = "left";
        ColFolioMovimientoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFolioMovimientoConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoConsultar = new CJQColumn();
        ColFechaPagoConsultar.Nombre = "FechaPago";
        ColFechaPagoConsultar.Encabezado = "FechaPago";
        ColFechaPagoConsultar.Buscador = "false";
        ColFechaPagoConsultar.Alineacion = "left";
        ColFechaPagoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColFechaPagoConsultar);

        //Monto
        CJQColumn ColMontoConsultar = new CJQColumn();
        ColMontoConsultar.Nombre = "Monto";
        ColMontoConsultar.Encabezado = "Monto";
        ColMontoConsultar.Buscador = "false";
        ColMontoConsultar.Formato = "FormatoMoneda";
        ColMontoConsultar.Alineacion = "right";
        ColMontoConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColMontoConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosConsultar", grdMovimientosCobrosConsultar.GeneraGrid(), true);

        //GridMovimientosCobrosEditar
        CJQGrid grdMovimientosCobrosEditar = new CJQGrid();
        grdMovimientosCobrosEditar.NombreTabla = "grdMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.CampoIdentificador = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        grdMovimientosCobrosEditar.ColumnaOrdenacion = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        grdMovimientosCobrosEditar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosEditar.Metodo = "ObtenerMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.TituloTabla = "Movimientos de cobros";
        grdMovimientosCobrosEditar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosEditar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosEditar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosEditar.Altura = 120;
        grdMovimientosCobrosEditar.Ancho = 670;
        grdMovimientosCobrosEditar.NumeroRegistros = 15;
        grdMovimientosCobrosEditar.RangoNumeroRegistros = "15,30,60";

        //IdNotaCreditoProveedorEncabezadoFacturaProveedor
        CJQColumn ColIdNotaCreditoProveedorEncabezadoFacturaProveedorEditar = new CJQColumn();
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorEditar.Nombre = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorEditar.Oculto = "true";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorEditar.Encabezado = "IdNotaCreditoProveedorEncabezadoFacturaProveedor";
        ColIdNotaCreditoProveedorEncabezadoFacturaProveedorEditar.Buscador = "false";
        grdMovimientosCobrosEditar.Columnas.Add(ColIdNotaCreditoProveedorEncabezadoFacturaProveedorEditar);

        //Serie
        CJQColumn ColSerieMovimientoEditar = new CJQColumn();
        ColSerieMovimientoEditar.Nombre = "Serie";
        ColSerieMovimientoEditar.Encabezado = "Serie";
        ColSerieMovimientoEditar.Buscador = "false";
        ColSerieMovimientoEditar.Alineacion = "left";
        ColSerieMovimientoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColSerieMovimientoEditar);

        //Folio
        CJQColumn ColFolioMovimientoEditar = new CJQColumn();
        ColFolioMovimientoEditar.Nombre = "Folio";
        ColFolioMovimientoEditar.Encabezado = "Folio";
        ColFolioMovimientoEditar.Buscador = "false";
        ColFolioMovimientoEditar.Alineacion = "left";
        ColFolioMovimientoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColFolioMovimientoEditar);

        //Fecha pago
        CJQColumn ColFechaPagoEditar = new CJQColumn();
        ColFechaPagoEditar.Nombre = "FechaPago";
        ColFechaPagoEditar.Encabezado = "FechaPago";
        ColFechaPagoEditar.Buscador = "false";
        ColFechaPagoEditar.Alineacion = "left";
        ColFechaPagoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColFechaPagoEditar);

        //Monto
        CJQColumn ColMontoEditar = new CJQColumn();
        ColMontoEditar.Nombre = "Monto";
        ColMontoEditar.Encabezado = "Monto";
        ColMontoEditar.Buscador = "false";
        ColMontoEditar.Formato = "FormatoMoneda";
        ColMontoEditar.Alineacion = "right";
        ColMontoEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColMontoEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobrosEditar", grdMovimientosCobrosEditar.GeneraGrid(), true);
        CrearGridFacturasRegistradas();
        CrearGridProductosAsociados();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }


    private void CrearGridFacturasRegistradas()
    {
        CJQGrid GridProductosNotaCreditoProveedorDevolucionCancelacion = new CJQGrid();
        GridProductosNotaCreditoProveedorDevolucionCancelacion.NombreTabla = "grdProductosNotaCreditoProveedorDevolucionCancelacion";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.CampoIdentificador = "IdDetalleFacturaProveedor";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.ColumnaOrdenacion = "IdDetalleFacturaProveedor";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.TipoOrdenacion = "DESC";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Metodo = "ObtenerProductosNotaCreditoProveedorDevolucionCancelacion";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.TituloTabla = "Facturas pendientes";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Altura = 290;
        GridProductosNotaCreditoProveedorDevolucionCancelacion.GenerarFuncionFiltro = false;
        GridProductosNotaCreditoProveedorDevolucionCancelacion.GenerarFuncionTerminado = true;

        //IdDetalleFacturaProveedor
        CJQColumn ColIdDetalleFacturaProveedor = new CJQColumn();
        ColIdDetalleFacturaProveedor.Nombre = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Oculto = "true";
        ColIdDetalleFacturaProveedor.Encabezado = "IdDetalleFacturaProveedor";
        ColIdDetalleFacturaProveedor.Buscador = "false";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColIdDetalleFacturaProveedor);

        //Factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "Factura";
        ColFactura.Oculto = "false";
        ColFactura.Encabezado = "Factura";
        ColFactura.Buscador = "false";
        ColFactura.Ancho = "80";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColFactura);

        //Numero de serie
        CJQColumn ColNumeroSerie = new CJQColumn();
        ColNumeroSerie.Nombre = "NumeroSerie";
        ColNumeroSerie.Encabezado = "Número de serie";
        ColNumeroSerie.Buscador = "true";
        ColNumeroSerie.Alineacion = "left";
        ColNumeroSerie.Ancho = "500";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColNumeroSerie);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Descripcion";
        ColProducto.Encabezado = "Producto";
        ColProducto.Buscador = "true";
        ColProducto.Alineacion = "left";
        ColProducto.Ancho = "500";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColProducto);

        //Cantidad
        CJQColumn ColCantidad = new CJQColumn();
        ColCantidad.Nombre = "Cantidad";
        ColCantidad.Encabezado = "Cantidad";
        ColCantidad.Buscador = "true";
        ColCantidad.Alineacion = "center";
        ColCantidad.Ancho = "70";
        ColCantidad.Buscador = "false";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColCantidad);

        //Disponible
        CJQColumn ColDisponible = new CJQColumn();
        ColDisponible.Nombre = "Disponible";
        ColDisponible.Encabezado = "Disponible";
        ColDisponible.Buscador = "true";
        ColDisponible.Alineacion = "center";
        ColDisponible.Ancho = "90";
        ColDisponible.Buscador = "false";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColDisponible);

        //TipoMoneda
        CJQColumn ColTipoMonedaProducto = new CJQColumn();
        ColTipoMonedaProducto.Nombre = "TipoMoneda";
        ColTipoMonedaProducto.Encabezado = "Moneda";
        ColTipoMonedaProducto.Buscador = "false";
        ColTipoMonedaProducto.Alineacion = "center";
        ColTipoMonedaProducto.Ancho = "85";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColTipoMonedaProducto);

        //PrecioUnitario
        CJQColumn ColPrecioUnitario = new CJQColumn();
        ColPrecioUnitario.Nombre = "PrecioUnitario";
        ColPrecioUnitario.Encabezado = "Precio unitario";
        ColPrecioUnitario.Buscador = "false";
        ColPrecioUnitario.Alineacion = "right";
        ColPrecioUnitario.Ancho = "105";
        ColPrecioUnitario.Formato = "FormatoMoneda";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColPrecioUnitario);

        //PrecioUnitarioIVA
        CJQColumn ColPrecioIVA = new CJQColumn();
        ColPrecioIVA.Nombre = "PrecioUnitarioIVA";
        ColPrecioIVA.Encabezado = "Total IVA";
        ColPrecioIVA.Buscador = "false";
        ColPrecioIVA.Alineacion = "right";
        ColPrecioIVA.Ancho = "125";
        ColPrecioIVA.Formato = "FormatoMoneda";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColPrecioIVA);


        //SeleccionarVarios
        CJQColumn ColSeleccionarVarios = new CJQColumn();
        ColSeleccionarVarios.Nombre = "Sel";
        ColSeleccionarVarios.Encabezado = "Sel.";
        ColSeleccionarVarios.Buscador = "false";
        ColSeleccionarVarios.Ordenable = "false";
        ColSeleccionarVarios.Ancho = "50";
        ColSeleccionarVarios.Etiquetado = "CheckBox";
        ColSeleccionarVarios.Estilo = "checkAsignarVarios";
        GridProductosNotaCreditoProveedorDevolucionCancelacion.Columnas.Add(ColSeleccionarVarios);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProductosNotaCreditoProveedorDevolucionCancelacion", GridProductosNotaCreditoProveedorDevolucionCancelacion.GeneraGrid(), true);
    }

    private void CrearGridProductosAsociados()
    {
        CJQGrid GridProductosAsociados = new CJQGrid();
        GridProductosAsociados.NombreTabla = "grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion";
        GridProductosAsociados.CampoIdentificador = "IdDevolucionProveedor";
        GridProductosAsociados.ColumnaOrdenacion = "IdDevolucionProveedor";
        GridProductosAsociados.TipoOrdenacion = "DESC";
        GridProductosAsociados.Metodo = "ObtenerProductosAsociadosNotaCreditoProveedorDevolucionCancelacion";
        GridProductosAsociados.TituloTabla = "Productos asociados";
        GridProductosAsociados.Altura = 290;
        GridProductosAsociados.GenerarFuncionFiltro = false;
        GridProductosAsociados.GenerarFuncionTerminado = true;

        //IdDevolucionProveedor
        CJQColumn ColIdDevolucionProveedor = new CJQColumn();
        ColIdDevolucionProveedor.Nombre = "IdDevolucionProveedor";
        ColIdDevolucionProveedor.Oculto = "true";
        ColIdDevolucionProveedor.Encabezado = "IdDevolucionProveedor";
        ColIdDevolucionProveedor.Buscador = "false";
        GridProductosAsociados.Columnas.Add(ColIdDevolucionProveedor);

        //Factura
        CJQColumn ColFactura = new CJQColumn();
        ColFactura.Nombre = "Factura";
        ColFactura.Oculto = "false";
        ColFactura.Encabezado = "Factura";
        ColFactura.Buscador = "false";
        GridProductosAsociados.Columnas.Add(ColFactura);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "DescripcionProducto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Buscador = "true";
        ColProducto.Alineacion = "left";
        ColProducto.Ancho = "550";
        GridProductosAsociados.Columnas.Add(ColProducto);

        //PrecioUnitario
        CJQColumn ColPrecioUnitario = new CJQColumn();
        ColPrecioUnitario.Nombre = "PrecioUnitario";
        ColPrecioUnitario.Encabezado = "Precio unitario";
        ColPrecioUnitario.Buscador = "false";
        ColPrecioUnitario.Alineacion = "right";
        ColPrecioUnitario.Ancho = "100";
        ColPrecioUnitario.Formato = "FormatoMoneda";
        GridProductosAsociados.Columnas.Add(ColPrecioUnitario);

        //PrecioUnitarioIVA
        CJQColumn ColPrecioIVA = new CJQColumn();
        ColPrecioIVA.Nombre = "PrecioUnitarioIVA";
        ColPrecioIVA.Encabezado = "Total";
        ColPrecioIVA.Buscador = "false";
        ColPrecioIVA.Alineacion = "right";
        ColPrecioIVA.Ancho = "125";
        ColPrecioIVA.Formato = "FormatoMoneda";
        GridProductosAsociados.Columnas.Add(ColPrecioIVA);


        //SeleccionarVarios
        CJQColumn ColSeleccionarVarios = new CJQColumn();
        ColSeleccionarVarios.Nombre = "SelProductos";
        ColSeleccionarVarios.Encabezado = "Sel.";
        ColSeleccionarVarios.Buscador = "false";
        ColSeleccionarVarios.Ordenable = "false";
        ColSeleccionarVarios.Ancho = "50";
        ColSeleccionarVarios.Etiquetado = "CheckBox";
        ColSeleccionarVarios.Estilo = "checkAsignarVarios";
        GridProductosAsociados.Columnas.Add(ColSeleccionarVarios);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProductosAsociadosNotaCreditoProveedorDevolucionCancelacion", GridProductosAsociados.GeneraGrid(), true);
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerNotaCreditoProveedor(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerieNotaCredito, string pFolioNotaCredito, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAI, string pRazonSocial)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoProveedor", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerieNotaCredito);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolioNotaCredito);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 255).Value = pRazonSocial;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pNumeroFactura, int pIdProveedor)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = Convert.ToString(pNumeroFactura);
        Stored.Parameters.Add("pIdProveedor", SqlDbType.Int).Value = pIdProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCreditoProveedor)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoProveedorEncabezadoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 60).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCreditoProveedor", SqlDbType.Int).Value = pIdNotaCreditoProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCreditoProveedor)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoProveedorEncabezadoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 60).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCreditoProveedor", SqlDbType.Int).Value = pIdNotaCreditoProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCreditoProveedor)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdNotaCreditoProveedorEncabezadoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 60).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCreditoProveedor", SqlDbType.Int).Value = pIdNotaCreditoProveedor;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProductosNotaCreditoProveedorDevolucionCancelacion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCreditoProveedor, int pIdTipoNotaCreditoProveedor, int pIdProveedor, int pIdTipoMoneda, decimal pTipoCambio, string pDescripcion, string pNumeroSerie)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_grdNotaCreditoProveedorAsociarProductosDevolucionCancelacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCreditoProveedor", SqlDbType.Int).Value = pIdNotaCreditoProveedor;
        Stored.Parameters.Add("pIdTipoNotaCreditoProveedor", SqlDbType.Int).Value = pIdTipoNotaCreditoProveedor;
        Stored.Parameters.Add("pIdProveedor", SqlDbType.Int).Value = pIdProveedor;
        Stored.Parameters.Add("pIdTipoMoneda", SqlDbType.Int).Value = pIdTipoMoneda;
        Stored.Parameters.Add("pTipoCambio", SqlDbType.Decimal).Value = pTipoCambio;
        Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 200).Value = pDescripcion;
        Stored.Parameters.Add("pNumeroSerie", SqlDbType.VarChar, 200).Value = pNumeroSerie;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProductosAsociadosNotaCreditoProveedorDevolucionCancelacion(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCreditoProveedor, int pIdTipoNotaCreditoProveedor, int pIdProveedor, string pDescripcion)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_grdNotaCreditoProveedorProductosDevolucion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCreditoProveedor", SqlDbType.Int).Value = pIdNotaCreditoProveedor;
        Stored.Parameters.Add("pIdTipoNotaCreditoProveedor", SqlDbType.Int).Value = pIdTipoNotaCreditoProveedor;
        Stored.Parameters.Add("pIdProveedor", SqlDbType.Int).Value = pIdProveedor;
        Stored.Parameters.Add("pDescripcion", SqlDbType.VarChar, 200).Value = pDescripcion;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string TimbrarNotaCreditoProveedor(Dictionary<string, object> pNotaCreditoProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();
            string validacion = CrearArchivoBuzonFiscal(Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]), pNotaCreditoProveedor["LugarExpedicion"].ToString(), pNotaCreditoProveedor["MetodoPago"].ToString(), pNotaCreditoProveedor["FormaPago"].ToString(), pNotaCreditoProveedor["CondicionPago"].ToString(), pNotaCreditoProveedor["CuentaBancariaCliente"].ToString(), pNotaCreditoProveedor["Referencia"].ToString(), pNotaCreditoProveedor["Observaciones"].ToString(), ConexionBaseDatos);
            if (validacion == "1")
            {
                CUtilerias Utilerias = new CUtilerias();
                Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                //validacion = BuzonFiscalTimbrado(Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]), ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string CancelarNotaCreditoProveedor(Dictionary<string, object> pNotaCreditoProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);
        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject oRespuesta = new JObject();

            if (NotaCreditoProveedor.ExisteNotaCreditoProveedorMovimientos(Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]), ConexionBaseDatos) == 0)
            {
                NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]), ConexionBaseDatos);
                NotaCreditoProveedor.Baja = true;
                NotaCreditoProveedor.MotivoCancelacion = pNotaCreditoProveedor["MotivoCancelacion"].ToString();
                NotaCreditoProveedor.FechaCancelacion = Convert.ToDateTime(DateTime.Now);
                NotaCreditoProveedor.IdUsuarioCancelacion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                NotaCreditoProveedor.EditarNotaCreditoProveedorCancelacion(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "La nota de crédito se cancelo correctamente"));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede cancelar esta nota de crédito de proveedor, porque tiene movimientos de cobros asociados a facturas"));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarNotaCreditoProveedor(Dictionary<string, object> pNotaCreditoProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
            NotaCreditoProveedor.IdProveedor = Convert.ToInt32(pNotaCreditoProveedor["IdProveedor"]);
            NotaCreditoProveedor.SerieNotaCredito = Convert.ToString(pNotaCreditoProveedor["SerieNotaCredito"]);
            NotaCreditoProveedor.FolioNotaCredito = Convert.ToInt32(pNotaCreditoProveedor["FolioNotaCredito"]);
            NotaCreditoProveedor.Descripcion = Convert.ToString(pNotaCreditoProveedor["Descripcion"]);
            NotaCreditoProveedor.Fecha = Convert.ToDateTime(pNotaCreditoProveedor["Fecha"]);
            NotaCreditoProveedor.IdTipoMoneda = Convert.ToInt32(pNotaCreditoProveedor["IdTipoMoneda"]);
            NotaCreditoProveedor.TipoCambio = Convert.ToDecimal(pNotaCreditoProveedor["TipoCambio"]);
            NotaCreditoProveedor.Referencia = Convert.ToString(pNotaCreditoProveedor["Referencia"]);
            NotaCreditoProveedor.Monto = Convert.ToDecimal(pNotaCreditoProveedor["Monto"]);
            NotaCreditoProveedor.PorcentajeIVA = Convert.ToDecimal(pNotaCreditoProveedor["PorcentajeIVA"]);
            NotaCreditoProveedor.IVA = Convert.ToDecimal(pNotaCreditoProveedor["IVA"]);
            NotaCreditoProveedor.Total = Convert.ToDecimal(pNotaCreditoProveedor["Total"]);
            NotaCreditoProveedor.SaldoDocumento = Convert.ToDecimal(pNotaCreditoProveedor["Total"]);
            NotaCreditoProveedor.FechaAlta = Convert.ToDateTime(DateTime.Now);
            NotaCreditoProveedor.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            NotaCreditoProveedor.IdTipoNotaCredito = Convert.ToInt32(pNotaCreditoProveedor["IdTipoNotaCreditoProveedor"]);

            string validacion = ValidarNotaCreditoProveedor(NotaCreditoProveedor, ConexionBaseDatos);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                NotaCreditoProveedor.AgregarNotaCreditoProveedor(ConexionBaseDatos);

                CNotaCreditoProveedorSucursal NotaCreditoProveedorSucursal = new CNotaCreditoProveedorSucursal();
                NotaCreditoProveedorSucursal.IdNotaCreditoProveedor = NotaCreditoProveedor.IdNotaCreditoProveedor;
                NotaCreditoProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                NotaCreditoProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                NotaCreditoProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                NotaCreditoProveedorSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = NotaCreditoProveedor.IdNotaCreditoProveedor;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nota de credito de proveedor";
                HistorialGenerico.AgregarHistorialGenerico("NotaCreditoProveedor", ConexionBaseDatos);

                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CNotaCreditoProveedor NotaCreditoProveedorTotal = new CNotaCreditoProveedor();
                NotaCreditoProveedorTotal.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdNotaCreditoProveedor), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCreditoProveedorTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(NotaCreditoProveedorTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                NotaCreditoProveedorTotal.TotalLetra = TotalLetras;
                NotaCreditoProveedorTotal.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarNotaCreditoProveedorEdicion(Dictionary<string, object> pNotaCreditoProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
            NotaCreditoProveedor.IdProveedor = Convert.ToInt32(pNotaCreditoProveedor["IdProveedor"]);
            NotaCreditoProveedor.SerieNotaCredito = Convert.ToString(pNotaCreditoProveedor["SerieNotaCredito"]);
            NotaCreditoProveedor.FolioNotaCredito = Convert.ToInt32(pNotaCreditoProveedor["FolioNotaCredito"]);
            NotaCreditoProveedor.Descripcion = Convert.ToString(pNotaCreditoProveedor["Descripcion"]);
            NotaCreditoProveedor.Fecha = Convert.ToDateTime(pNotaCreditoProveedor["Fecha"]);
            NotaCreditoProveedor.IdTipoMoneda = Convert.ToInt32(pNotaCreditoProveedor["IdTipoMoneda"]);
            NotaCreditoProveedor.TipoCambio = Convert.ToDecimal(pNotaCreditoProveedor["TipoCambio"]);
            NotaCreditoProveedor.Referencia = Convert.ToString(pNotaCreditoProveedor["Referencia"]);
            NotaCreditoProveedor.Monto = Convert.ToDecimal(pNotaCreditoProveedor["Monto"]);
            NotaCreditoProveedor.PorcentajeIVA = Convert.ToDecimal(pNotaCreditoProveedor["PorcentajeIVA"]);
            NotaCreditoProveedor.IVA = Convert.ToDecimal(pNotaCreditoProveedor["IVA"]);
            NotaCreditoProveedor.Total = Convert.ToDecimal(pNotaCreditoProveedor["Total"]);
            NotaCreditoProveedor.SaldoDocumento = Convert.ToDecimal(pNotaCreditoProveedor["Total"]);
            NotaCreditoProveedor.FechaAlta = Convert.ToDateTime(DateTime.Now);
            NotaCreditoProveedor.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            NotaCreditoProveedor.IdTipoNotaCredito = Convert.ToInt32(pNotaCreditoProveedor["IdTipoNotaCreditoProveedor"]);

            string validacion = "";
            if (Convert.ToInt32(pNotaCreditoProveedor["IdTipoNotaCreditoProveedor"]) == 1)
            {
                ValidarNotaCreditoProveedorDevolucionCancelacion(NotaCreditoProveedor, ConexionBaseDatos);

            }
            else
            {
                validacion = ValidarNotaCreditoProveedor(NotaCreditoProveedor, ConexionBaseDatos);
            }

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                NotaCreditoProveedor.AgregarNotaCreditoProveedor(ConexionBaseDatos);
                int IdNotaCreditoProveedor = NotaCreditoProveedor.IdNotaCreditoProveedor;

                CNotaCreditoProveedorSucursal NotaCreditoProveedorSucursal = new CNotaCreditoProveedorSucursal();
                NotaCreditoProveedorSucursal.IdNotaCreditoProveedor = NotaCreditoProveedor.IdNotaCreditoProveedor;
                NotaCreditoProveedorSucursal.IdSucursal = Usuario.IdSucursalActual;
                NotaCreditoProveedorSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                NotaCreditoProveedorSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                NotaCreditoProveedorSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = NotaCreditoProveedor.IdNotaCreditoProveedor;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto una nota de crédito de proveedor";
                HistorialGenerico.AgregarHistorialGenerico("NotaCreditoProveedor", ConexionBaseDatos);
                oRespuesta.Add("IdNotaCreditoProveedor", NotaCreditoProveedor.IdNotaCreditoProveedor);

                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CNotaCreditoProveedor NotaCreditoProveedorTotal = new CNotaCreditoProveedor();
                NotaCreditoProveedorTotal.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdNotaCreditoProveedor), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCreditoProveedorTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(NotaCreditoProveedorTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                NotaCreditoProveedorTotal.TotalLetra = TotalLetras;
                NotaCreditoProveedorTotal.Editar(ConexionBaseDatos);


                oRespuesta.Add(new JProperty("Error", 0));

            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string ObtenerFormaFiltroNotaCreditoProveedor()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject Modelo = new JObject();
        DateTime Fecha = DateTime.Now;
        Modelo.Add("FechaInicial", Convert.ToString(Fecha.ToShortDateString()));
        Modelo.Add("FechaFinal", Convert.ToString(Fecha.ToShortDateString()));
        oRespuesta.Add(new JProperty("Error", 0));
        oRespuesta.Add(new JProperty("Modelo", Modelo));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneFacturaFormato(int pIdNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
        string NombreArchivo = "";
        string Ruta = "";

        NotaCreditoProveedor.LlenaObjeto(pIdNotaCreditoProveedor, ConexionBaseDatos);
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

        NombreArchivo = NotaCreditoProveedor.SerieNotaCredito + NotaCreditoProveedor.FolioNotaCredito;
        Ruta = RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".pdf";

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add(new JProperty("Ruta", Ruta));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> NotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCreditoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCreditoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCreditoProveedor = 1;
        }
        oPermisos.Add("puedeEditarNotaCreditoProveedor", puedeEditarNotaCreditoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            CNotaCreditoProveedor NotaCreditoProveedorCliente = new CNotaCreditoProveedor();
            NotaCreditoProveedorCliente.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor["pIdNotaCreditoProveedor"]), ConexionBaseDatos);
            NotaCreditoProveedorCliente.IdProveedor = Convert.ToInt32(NotaCreditoProveedor["pIdProveedor"]);
            NotaCreditoProveedorCliente.Editar(ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CNotaCreditoProveedor.ObtenerNotaCreditoProveedorAsociarDocumentos(Modelo, Convert.ToInt32(NotaCreditoProveedor["pIdNotaCreditoProveedor"]), ConexionBaseDatos);


            //if (Convert.ToInt32(NotaCreditoProveedor["IdTipoNotaCreditoProveedor"]) == 1)
            //{
            //    int TotalNotaCreditoProveedorTimbrada = NotaCreditoProveedorCliente.ObtieneNotaTimbrada(Convert.ToInt32(NotaCreditoProveedor["pIdNotaCreditoProveedor"]), ConexionBaseDatos);
            //    oPermisos.Add(new JProperty("TotalNotaCreditoProveedorTimbrada", TotalNotaCreditoProveedorTimbrada));
            //}
            //else
            //{
            //    oPermisos.Add(new JProperty("TotalNotaCreditoProveedorTimbrada", 0));
            //}
            oPermisos.Add(new JProperty("IdTipoNotaCreditoProveedor", Convert.ToInt32(NotaCreditoProveedor["IdTipoNotaCreditoProveedor"])));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No ObtenerFormaAsociarDocumentos conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAsociarProductosDevolucionCancelacion(Dictionary<string, object> NotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCreditoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCreditoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCreditoProveedor = 1;
        }
        oPermisos.Add("puedeEditarNotaCreditoProveedor", puedeEditarNotaCreditoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("IdCliente", Convert.ToInt32(NotaCreditoProveedor["pIdNotaCreditoProveedor"]));
            Modelo.Add("IdNotaCreditoProveedor", Convert.ToInt32(NotaCreditoProveedor["pIdCliente"]));
            Modelo.Add("IdTipoNotaCreditoProveedor", Convert.ToInt32(NotaCreditoProveedor["pIdTipoNotaCreditoProveedor"]));

            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaNotaCreditoProveedor(int pIdNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCreditoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCreditoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCreditoProveedor = 1;
        }
        oPermisos.Add("puedeEditarNotaCreditoProveedor", puedeEditarNotaCreditoProveedor);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            Modelo = CNotaCreditoProveedor.ObtenerNotaCreditoProveedor(Modelo, pIdNotaCreditoProveedor, ConexionBaseDatos);

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarNotaCreditoProveedor(int pIdTipoNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            //Modelo.Add("SerieNotaCreditoProveedors", CJson.ObtenerJsonSerieNotaCreditoProveedor(Usuario.IdSucursalActual, ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("PorcentajeIVA", Sucursal.IVAActual);
            Modelo.Add("Fecha", DateTime.Today.ToShortDateString());
            Modelo.Add("IdTipoNotaCreditoProveedor", pIdTipoNotaCreditoProveedor);
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]//en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarNotaCreditoProveedorDevolucionCancelacion(int pIdTipoNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add("PorcentajeIVA", Sucursal.IVAActual);
            Modelo.Add("Fecha", DateTime.Today.ToShortDateString());
            Modelo.Add("IdTipoNotaCreditoProveedor", pIdTipoNotaCreditoProveedor);
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }


    ////////[WebMethod]
    ////////public static string ObtenerNumeroNotaCreditoProveedor(Dictionary<string, object> pNotaCreditoProveedor)
    ////////{
    ////////    //Abrir Conexion
    ////////    CConexion ConexionBaseDatos = new CConexion();
    ////////    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    ////////    //¿La conexion se establecio?
    ////////    if (respuesta == "Conexion Establecida")
    ////////    {
    ////////        CSerieNotaCreditoProveedor SerieNotaCreditoProveedor = new CSerieNotaCreditoProveedor();
    ////////        SerieNotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pNotaCreditoProveedor["IdSerieNotaCreditoProveedor"]), ConexionBaseDatos);
    ////////        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
    ////////        CUsuario Usuario = new CUsuario();
    ////////        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
    ////////        int NumeroNotaCreditoProveedor = 0;
    ////////        NumeroNotaCreditoProveedor = NotaCreditoProveedor.ObtieneNumeroNotaCreditoProveedor(SerieNotaCreditoProveedor.SerieNotaCreditoProveedor, Usuario.IdSucursalActual, ConexionBaseDatos);

    ////////        string validacion = "";
    ////////        JObject oRespuesta = new JObject();
    ////////        if (validacion == "")
    ////////        {
    ////////            JObject Modelo = new JObject();
    ////////            Modelo.Add("NumeroNotaCreditoProveedor", NumeroNotaCreditoProveedor);
    ////////            oRespuesta.Add(new JProperty("Error", 0));
    ////////            oRespuesta.Add(new JProperty("Modelo", Modelo));
    ////////        }
    ////////        else
    ////////        {
    ////////            oRespuesta.Add(new JProperty("Error", 1));
    ////////            oRespuesta.Add(new JProperty("Descripcion", validacion));
    ////////        }
    ////////        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    ////////        return oRespuesta.ToString();
    ////////    }
    ////////    else
    ////////    { return "1|" + respuesta; }
    ////////}

    [WebMethod]
    public static string ObtenerTipoCambio(Dictionary<string, object> pTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambio TipoCambio = new CTipoCambio();

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Opcion", 1);
            ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(pTipoCambio["IdTipoCambioOrigen"]));
            ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(pTipoCambio["IdTipoCambioDestino"]));
            ParametrosTS.Add("Fecha", DateTime.Today);
            TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }


    [WebMethod]
    public static string ObtenerFormaEditarNotaCreditoProveedor(int IdNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCreditoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCreditoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCreditoProveedor = 1;
        }
        oPermisos.Add("puedeEditarNotaCreditoProveedor", puedeEditarNotaCreditoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CNotaCreditoProveedor.ObtenerNotaCreditoProveedor(Modelo, IdNotaCreditoProveedor, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));

            string Baja = Convert.ToString(Modelo["Baja"]);
            if (Baja == "true" || Baja == "True")
            {
                Modelo.Add("EstatusNota", 1);
            }
            else
            {
                Modelo.Add("EstatusNota", 0);
            }

            CNotaCreditoProveedor NC = new CNotaCreditoProveedor();
            NC.LlenaObjeto(IdNotaCreditoProveedor, ConexionBaseDatos);
            int pIdTipoNotaCreditoProveedor = NC.IdTipoNotaCredito;
            Modelo.Add("IdTipoNotaCreditoProveedor", pIdTipoNotaCreditoProveedor);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarNotaCreditoProveedor(Dictionary<string, object> pNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
        NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]), ConexionBaseDatos);
        NotaCreditoProveedor.IdNotaCreditoProveedor = Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]);
        NotaCreditoProveedor.IdProveedor = Convert.ToInt32(pNotaCreditoProveedor["IdProveedor"]);
        NotaCreditoProveedor.SerieNotaCredito = Convert.ToString(pNotaCreditoProveedor["SerieNotaCredito"]);
        NotaCreditoProveedor.FolioNotaCredito = Convert.ToInt32(pNotaCreditoProveedor["FolioNotaCredito"]);
        NotaCreditoProveedor.Descripcion = Convert.ToString(pNotaCreditoProveedor["Descripcion"]);
        NotaCreditoProveedor.Fecha = Convert.ToDateTime(pNotaCreditoProveedor["Fecha"]);
        NotaCreditoProveedor.IdTipoMoneda = Convert.ToInt32(pNotaCreditoProveedor["IdTipoMoneda"]);
        NotaCreditoProveedor.TipoCambio = Convert.ToDecimal(pNotaCreditoProveedor["TipoCambio"]);
        NotaCreditoProveedor.Referencia = Convert.ToString(pNotaCreditoProveedor["Referencia"]);
        NotaCreditoProveedor.Monto = Convert.ToDecimal(pNotaCreditoProveedor["Monto"]);
        NotaCreditoProveedor.PorcentajeIVA = Convert.ToDecimal(pNotaCreditoProveedor["PorcentajeIVA"]);
        NotaCreditoProveedor.IVA = Convert.ToDecimal(pNotaCreditoProveedor["IVA"]);
        NotaCreditoProveedor.Total = Convert.ToDecimal(pNotaCreditoProveedor["Total"]);
        NotaCreditoProveedor.SaldoDocumento = Convert.ToDecimal(pNotaCreditoProveedor["Total"]);
        NotaCreditoProveedor.IdTipoNotaCredito = Convert.ToInt32(pNotaCreditoProveedor["IdTipoNotaCreditoProveedor"]);

        string validacion = ValidarNotaCreditoProveedor(NotaCreditoProveedor, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            NotaCreditoProveedor.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string LlenaDatosFiscales(int IdNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarNotaCreditoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
        CSucursal Sucursal = new CSucursal();
        CEmpresa Empresa = new CEmpresa();
        CLocalidad Localidad = new CLocalidad();
        CMunicipio Municipio = new CMunicipio();
        CEstado Estado = new CEstado();
        CPais Pais = new CPais();
        CCliente Cliente = new CCliente();
        NotaCreditoProveedor.LlenaObjeto(IdNotaCreditoProveedor, ConexionBaseDatos);
        Cliente.LlenaObjeto(NotaCreditoProveedor.IdProveedor, ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarNotaCreditoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEditarNotaCreditoProveedor = 1;
        }
        oPermisos.Add("puedeEditarNotaCreditoProveedor", puedeEditarNotaCreditoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CNotaCreditoProveedor.ObtenerNotaCreditoProveedor(Modelo, IdNotaCreditoProveedor, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdUsuarioAlta), ConexionBaseDatos);
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
            Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
            if (Sucursal.DireccionFiscal == true)
            {
                Localidad.LlenaObjeto(Empresa.IdLocalidad, ConexionBaseDatos);
                Municipio.LlenaObjeto(Empresa.IdMunicipio, ConexionBaseDatos);
            }
            else
            {
                Localidad.LlenaObjeto(Sucursal.IdLocalidad, ConexionBaseDatos);
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);
            }
            Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
            Pais.LlenaObjeto(Estado.IdPais, ConexionBaseDatos);
            Modelo.Add(new JProperty("LugarExpedicion", Estado.Estado + ", " + Pais.Pais));
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(ConexionBaseDatos));
            Modelo.Add("FormaPagos", CJson.ObtenerJsonFormaPago(1, ConexionBaseDatos));
            Modelo.Add("CondicionPagos", CJson.ObtenerJsonCondicionPago(Cliente.IdCondicionPago, ConexionBaseDatos));
            Modelo.Add("CuentaBancariaClientes", CJson.ObtenerJsonCuentaBancariaCliente(Cliente.IdCliente, ConexionBaseDatos));
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string LlenaMotivoCancelacion(int IdNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEliminarNotaCreditoProveedor = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (Usuario.TienePermisos(new string[] { "puedeEliminarNotaCreditoProveedor" }, ConexionBaseDatos) == "")
        {
            puedeEliminarNotaCreditoProveedor = 1;
        }
        oPermisos.Add("puedeEliminarNotaCreditoProveedor", puedeEliminarNotaCreditoProveedor);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CNotaCreditoProveedor.ObtenerNotaCreditoProveedor(Modelo, IdNotaCreditoProveedor, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarMontos(Dictionary<string, object> pNotaCreditoProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();

        CUtilerias Utilerias = new CUtilerias();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pNotaCreditoProveedor["IdEncabezadoFacturaProveedor"]), ConexionBaseDatos);

        CNotaCreditoProveedorEncabezadoFacturaProveedor NotaCreditoProveedorEncabezadoFacturaProveedor = new CNotaCreditoProveedorEncabezadoFacturaProveedor();
        NotaCreditoProveedorEncabezadoFacturaProveedor.IdNotaCreditoProveedor = Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = Convert.ToInt32(pNotaCreditoProveedor["IdEncabezadoFacturaProveedor"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.Monto = Convert.ToDecimal(pNotaCreditoProveedor["Monto"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.FechaPago = Convert.ToDateTime(DateTime.Now);
        NotaCreditoProveedorEncabezadoFacturaProveedor.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pNotaCreditoProveedor["IdTipoMoneda"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.TipoCambio = Convert.ToDecimal(pNotaCreditoProveedor["TipoCambio"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.Nota = "pago de la factura";

        string validacion = ValidarMontos(NotaCreditoProveedorEncabezadoFacturaProveedor, FacturaEncabezado, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            NotaCreditoProveedorEncabezadoFacturaProveedor.AgregarNotaCreditoProveedorEncabezadoFacturaProveedor(ConexionBaseDatos);
            oRespuesta.Add("Monto", Convert.ToDecimal(pNotaCreditoProveedor["Monto"]));
            oRespuesta.Add("rowid", Convert.ToDecimal(pNotaCreditoProveedor["rowid"]));
            oRespuesta.Add("TipoMoneda", Convert.ToString(pNotaCreditoProveedor["TipoMoneda"]));
            oRespuesta.Add("AbonosNotaCreditoProveedor", NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(Convert.ToInt32(pNotaCreditoProveedor["IdNotaCreditoProveedor"]), ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        return oRespuesta.ToString();
    }

    //timbrado de factura///////////////////////////////////////////////////////////////////////////////////        

    private static string CrearArchivoBuzonFiscalFactura(int pIdFactura, CConexion pConexion)
    {
        string errores = "";
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CEmpresa Empresa = new CEmpresa();
        CLocalidad Localidad = new CLocalidad();
        CMunicipio Municipio = new CMunicipio();
        CEstado Estado = new CEstado();
        CPais Pais = new CPais();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CSerieFactura SerieFactura = new CSerieFactura();
        CValidacion ValidacionRFC = new CValidacion();
        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFactura), pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);
        Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
        Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        Cliente.LlenaObjeto(FacturaEncabezado.IdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        TipoMoneda.LlenaObjeto(FacturaEncabezado.IdTipoMoneda, pConexion);

        Dictionary<string, object> ParametrosDO = new Dictionary<string, object>();
        ParametrosDO.Add("IdOrganizacion", Convert.ToInt32(Organizacion.IdOrganizacion));
        ParametrosDO.Add("IdTipoDireccion", Convert.ToInt32(1));
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosDO, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;


        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
            file.WriteLine("HOJA");
            file.WriteLine("########################################################################");
            file.WriteLine("[Datos Generales]");
            file.WriteLine("serie|" + SerieFactura.SerieFactura);
            file.WriteLine("folio|" + FacturaEncabezado.NumeroFactura);
            file.WriteLine("asignaFolio|FALSE");

            file.WriteLine("");

            file.WriteLine("[Datos del Emisor]");
            file.WriteLine("emRegimen|" + Convert.ToString(Empresa.RegimenFiscal));
            file.WriteLine("emRfc|" + Empresa.RFC);
            file.WriteLine("emNombre|" + Empresa.RazonSocial);
            file.WriteLine("emCalle|" + Empresa.Calle);
            file.WriteLine("emNoExterior|" + Empresa.NumeroExterior);
            file.WriteLine("emNoInterior|" + Empresa.NumeroInterior);
            file.WriteLine("emColonia|" + Empresa.Colonia);
            file.WriteLine("emLocalidad|" + Localidad.Localidad);
            file.WriteLine("emReferencia|" + Empresa.Referencia);
            file.WriteLine("emMunicipio|" + Municipio.Municipio);
            file.WriteLine("emEstado|" + Estado.Estado);
            file.WriteLine("emPais|" + Pais.Pais);
            file.WriteLine("emCodigoPostal|" + Empresa.CodigoPostal);
            file.WriteLine("emProveedor|");
            file.WriteLine("emGLN|");

            file.WriteLine("");

            file.WriteLine("[Datos de Expedicion]");

            if (Sucursal.DireccionFiscal == true)
            {
                file.WriteLine("exAlias|");
                file.WriteLine("exTelefono|");
                file.WriteLine("exCalle|");
                file.WriteLine("exNoExterior|");
                file.WriteLine("exNoInterior|");
                file.WriteLine("exColonia|");
                file.WriteLine("exLocalidad|");
                file.WriteLine("exReferencia|");
                file.WriteLine("exMunicipio|");
                file.WriteLine("exEstado|");
                file.WriteLine("exPais|");
                file.WriteLine("exCodigoPostal|");
            }
            else
            {
                Localidad.LlenaObjeto(Sucursal.IdLocalidad, pConexion);
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, pConexion);
                Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
                Pais.LlenaObjeto(Estado.IdPais, pConexion);

                file.WriteLine("exAlias|" + Sucursal.Alias);
                file.WriteLine("exTelefono|" + Sucursal.Telefono);
                file.WriteLine("exCalle|" + Sucursal.Calle);
                file.WriteLine("exNoExterior|" + Sucursal.NumeroExterior);
                file.WriteLine("exNoInterior|" + Sucursal.NumeroInterior);
                file.WriteLine("exColonia|" + Sucursal.Colonia);
                file.WriteLine("exLocalidad|" + Localidad.Localidad);
                file.WriteLine("exReferencia|" + Sucursal.Referencia);
                file.WriteLine("exMunicipio|" + Municipio.Municipio);
                file.WriteLine("exEstado|" + Estado.Estado);
                file.WriteLine("exPais|" + Pais.Pais);
                file.WriteLine("exCodigoPostal|" + Sucursal.CodigoPostal);
            }

            file.WriteLine("");

            Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("[Datos del Receptor]");
            file.WriteLine("reRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("reNombre|" + Organizacion.RazonSocial);
            file.WriteLine("reCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("reNoExterior|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("reNoInterior|" + DireccionOrganizacion.NumeroInterior);
            file.WriteLine("reColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("reLocalidad|" + Localidad.Localidad);
            file.WriteLine("reReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("reMunicipio|" + Municipio.Municipio);
            file.WriteLine("reEstado|" + Estado.Estado);
            file.WriteLine("rePais|" + Pais.Pais);
            file.WriteLine("reCodigoPostal|" + DireccionOrganizacion.CodigoPostal);
            file.WriteLine("reNoCliente|" + Cliente.IdCliente);
            file.WriteLine("reEmail|" + Cliente.Correo);
            file.WriteLine("reTelefono|" + DireccionOrganizacion.ConmutadorTelefono);
            file.WriteLine("reFax|");
            file.WriteLine("reComprador|");
            file.WriteLine("reNIM|");

            file.WriteLine("");

            Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("[Datos del Remitente]");

            file.WriteLine("remRfc|" + Empresa.RFC);
            file.WriteLine("remNombre|" + Empresa.RazonSocial);
            file.WriteLine("remClaveIdentificacion|");
            file.WriteLine("remCalle|" + Empresa.Calle);
            file.WriteLine("remNumero|" + Empresa.NumeroExterior);
            file.WriteLine("remReferencia|" + Empresa.Referencia);
            file.WriteLine("remColonia|" + Empresa.Colonia);
            file.WriteLine("remCiudad|" + Localidad.Localidad);
            file.WriteLine("remMunicipio|" + Municipio.Municipio);
            file.WriteLine("remEstado|" + Estado.Estado);
            file.WriteLine("remPais|" + Pais.Pais);
            file.WriteLine("remCodigoPostal|" + Empresa.CodigoPostal);

            file.WriteLine("");

            file.WriteLine("[Datos del Destino]");

            Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("desRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("desNombre|" + Organizacion.RazonSocial);
            file.WriteLine("desClaveIdentificacion|");
            file.WriteLine("desCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("desNumero|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("desReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("desColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("desCiudad|" + Localidad.Localidad);
            file.WriteLine("desMunicipio|" + Municipio.Municipio);
            file.WriteLine("desEstado|" + Estado.Estado);
            file.WriteLine("desPais|" + Pais.Pais);
            file.WriteLine("desCodigoPostal|" + DireccionOrganizacion.CodigoPostal);

            file.WriteLine("");

            //aqui hace el ciclo de las partidas
            CFacturaDetalle ListaPartidas = new CFacturaDetalle();
            CProducto Producto = new CProducto();
            CServicio Servicio = new CServicio();
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            CConceptoProyecto ConceptoProyecto = new CConceptoProyecto();

            Dictionary<string, object> ParametrosOCD = new Dictionary<string, object>();
            ParametrosOCD.Add("Baja", 0);
            ParametrosOCD.Add("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado);

            foreach (CFacturaDetalle oFacturaDetalle in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, pConexion))
            {
                if (oFacturaDetalle.IdProyecto == 0)
                {
                    if (oFacturaDetalle.IdProducto != 0)
                    {
                        Producto.LlenaObjeto(oFacturaDetalle.IdProducto, pConexion);
                        UnidadCompraVenta.LlenaObjeto(Producto.IdUnidadCompraVenta, pConexion);
                    }
                    else
                    {
                        Servicio.LlenaObjeto(oFacturaDetalle.IdServicio, pConexion);
                        UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, pConexion);
                    }
                }
                else
                {
                    ConceptoProyecto.LlenaObjeto(oFacturaDetalle.IdConceptoProyecto, pConexion);
                    UnidadCompraVenta.LlenaObjeto(ConceptoProyecto.IdUnidadCompraVenta, pConexion);
                }
                file.WriteLine("[Datos de Conceptos]");
                file.WriteLine("cantidad|" + oFacturaDetalle.Cantidad);
                if (FacturaEncabezado.ParcialidadIndividual == true)
                {
                    file.WriteLine("unidad|" + "No aplica");
                }
                else
                {
                    file.WriteLine("unidad|" + UnidadCompraVenta.UnidadCompraVenta);
                }
                file.WriteLine("numIdentificacion|" + oFacturaDetalle.Clave);
                file.WriteLine("descripcion|" + oFacturaDetalle.Descripcion.Replace('\n', ' '));
                file.WriteLine("valorUnitario|" + oFacturaDetalle.PrecioUnitario);
                file.WriteLine("importe|" + oFacturaDetalle.Total);
                file.WriteLine("#1  Cuenta Predial,");
                file.WriteLine("cpNumero|");
                file.WriteLine("#2  Informacion Aduanera");
                file.WriteLine("iaNumero|");
                file.WriteLine("iaFecha|");
                file.WriteLine("#3  Parte");
                file.WriteLine("parteCantidad|");
                file.WriteLine("parteUnidad|");
                file.WriteLine("parteNumIdentificacion|");
                file.WriteLine("parteDescripcion|");
                file.WriteLine("parteValorUnitario|");
                file.WriteLine("parteImporte|");
                file.WriteLine("#Bloque de Datos opcionales para");
                file.WriteLine("# introducir la informacion aduanera");
                file.WriteLine("parteIaNumero|");
                file.WriteLine("parteIaFecha|");
                file.WriteLine("parteIaAduana|");
                file.WriteLine("");
                file.WriteLine("#4 Complemento Concepto");
                file.WriteLine("");
                file.WriteLine("[Datos Complementarios para especificar la venta de vehiculos]");
                file.WriteLine("");
                file.WriteLine("claveVehicular|");
                file.WriteLine("");
                file.WriteLine("vehiculoIaNumero|");
                file.WriteLine("");
                file.WriteLine("vehiculoIaFecha|");
                file.WriteLine("");
                file.WriteLine("vehiculoIaAduana|");
                file.WriteLine("");
                file.WriteLine("#PARTES");
                file.WriteLine("vehiculoparteCantidad|");
                file.WriteLine("vehiculoparteUnidad|");
                file.WriteLine("vehiculopartenoIdentificacion|");
                file.WriteLine("vehiculoparteDescripcion|");
                file.WriteLine("vehiculoparteValorUnitario|");
                file.WriteLine("vehiculoparteImporte|");
                file.WriteLine("vehiculoparteIaNumero|");
                file.WriteLine("vehiculoparteIaFecha|");
                file.WriteLine("vehiculoparteIaAduana|");
                file.WriteLine("");
                file.WriteLine("[Complemento Dutty Free]");
                file.WriteLine("");
                file.WriteLine("dutFreeVersion|");
                file.WriteLine("dutFreeFechaTran|");
                file.WriteLine("dutFreeTipoTran|");
                file.WriteLine("# Datos Transito");
                file.WriteLine("dutFreeDatVia|");
                file.WriteLine("dutFreeDatTipoID|");
                file.WriteLine("dutFreeDatNumeroId|");
                file.WriteLine("dutFreeDatNacio|");
                file.WriteLine("dutFreeDatTransporte|");
                file.WriteLine("dutFreeDatidTransporte|");
                file.WriteLine("");
                file.WriteLine("[Datos Extra Conceptos]");
                file.WriteLine("ConExReferencia1|");
                file.WriteLine("ConExReferencia2|");
                file.WriteLine("ConExIndicador|");
                file.WriteLine("ConExDescripcionIngles|");
                file.WriteLine("ConExNumRemision|0");
                file.WriteLine("ConExCargo|");
                file.WriteLine("ConExDescuento|0");
                file.WriteLine("ConExMensaje|");
                file.WriteLine("ConExTasaImpuesto|");
                file.WriteLine("ConExImpuesto|");
                file.WriteLine("ConExValorUnitarioMonedaExtranjera|");
                file.WriteLine("ConExImporteMonedaExtranjera|");
                file.WriteLine("ConExtunitarioBruto|");
                file.WriteLine("ConExtImporteBruto|");
                file.WriteLine("ConExCvDivisas|");
                file.WriteLine("ConExtItemIdAlterno|");
                file.WriteLine("ConExtItemAlternoClave|");
                file.WriteLine("ConExtItemAlternoValor|");
                file.WriteLine("ConExtVendorPack|");
                file.WriteLine("");

            }

            ///////////////////////////////////////////////////////////////////////////

            file.WriteLine("[Datos Complementarios del Comprobante a nivel global]");
            file.WriteLine("subtotalConceptos|" + FacturaEncabezado.Subtotal);
            file.WriteLine("descuentoPorcentaje|");
            file.WriteLine("descuentoMonto|");
            file.WriteLine("descuentoMotivo|");
            file.WriteLine("cargos|");
            file.WriteLine("totalConceptos|" + FacturaEncabezado.Subtotal);
            file.WriteLine("");

            if (FacturaEncabezado.Parcialidades == true || FacturaEncabezado.ParcialidadIndividual == true)
            {
                if (FacturaEncabezado.Parcialidades == true)
                {
                    file.WriteLine("pagoForma|Parcialidades (" + FacturaEncabezado.NumeroParcialidades + ")");
                }
                else
                {
                    file.WriteLine("pagoForma|Pago en una sola exhibición");
                }
            }
            else
            {
                file.WriteLine("pagoForma|Pago en una sola exhibición");
            }

            file.WriteLine("pagoCondiciones|" + FacturaEncabezado.CondicionPago);
            file.WriteLine("pagoMetodo|" + FacturaEncabezado.MetodoPago);
            file.WriteLine("numCtaPago|" + FacturaEncabezado.NumeroCuenta);
            file.WriteLine("lugarExpedicion|" + FacturaEncabezado.LugarExpedicion);

            if (FacturaEncabezado.ParcialidadIndividual == true)
            {
                file.WriteLine("serieFolioFiscalOrig|" + FacturaEncabezado.SerieGlobal);
                file.WriteLine("folioFiscalOrig|" + FacturaEncabezado.FolioGlobal);
                file.WriteLine("montoFolioFiscalOrig|" + FacturaEncabezado.MontoGlobal);
                file.WriteLine("fechaFolioFiscalOrig|" + FacturaEncabezado.FechaGlobal);
            }

            file.WriteLine("");

            file.WriteLine("[Datos Complementarios del Comprobante a nivel global para casos de importaci—n o exportaci—n de bienes]");
            file.WriteLine("#Datos Globales de Aduana, el bloque es opcional y se constituye por los siguientes tres datos, el bloque se repite para cada aduana que aplique.");
            file.WriteLine("comiaNumero|");
            file.WriteLine("comiaFecha|");
            file.WriteLine("comiaAduana|");
            file.WriteLine("embarque|");
            file.WriteLine("fob|");
            file.WriteLine("");
            file.WriteLine("[Datos Comerciales del Comprobante a nivel global]  Datos adicionales de tipo comercial comœnmente usados.");
            if (FacturaEncabezado.Refid != "")
            {
                file.WriteLine("refID|" + FacturaEncabezado.Refid);
            }
            else
            {
                file.WriteLine("refID|" + "1" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            }

            file.WriteLine("tipoDocumento|Factura");

            Usuario.LlenaObjeto(FacturaEncabezado.IdUsuarioAgente, pConexion);

            file.WriteLine("ordenCompra|");
            file.WriteLine("agente|" + Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
            file.WriteLine("observaciones|" + FacturaEncabezado.Nota);
            file.WriteLine("nombreMoneda|" + TipoMoneda.TipoMoneda);
            file.WriteLine("tipoCambio|" + FacturaEncabezado.TipoCambio);
            file.WriteLine("");
            file.WriteLine("[Impuestos Trasladados]");
            file.WriteLine("trasladadoImpuesto|IVA");
            file.WriteLine("trasladadoImporte|" + FacturaEncabezado.IVA);
            file.WriteLine("trasladadoTasa|" + Sucursal.IVAActual);
            file.WriteLine("subtotalTrasladados|" + FacturaEncabezado.IVA);
            file.WriteLine("");
            file.WriteLine("[Impuestos Retenidos]");
            file.WriteLine("retenidoImpuesto|");
            file.WriteLine("retenidoImporte|");
            file.WriteLine("subtotalRetenidos|");
            file.WriteLine("");
            file.WriteLine("[IMPUESTOS LOCALES]");
            file.WriteLine("version|");
            file.WriteLine("totalTraslados|");
            file.WriteLine("totalRetenciones|");
            file.WriteLine("");
            file.WriteLine("[TRASLADOS LOCALES]");
            file.WriteLine("impLocTrasladado|");
            file.WriteLine("tasaDeTraslado|");
            file.WriteLine("importeTraslados|");
            file.WriteLine("");
            file.WriteLine("[RETENCIONES LOCALES]");
            file.WriteLine("impLocRetenido|");
            file.WriteLine("tasaDeRetencion|");
            file.WriteLine("importeRetenciones|");
            file.WriteLine("");
            file.WriteLine("[Datos Totales]");
            file.WriteLine("montoTotal|" + FacturaEncabezado.Total);
            file.WriteLine("montoTotalTexto|" + FacturaEncabezado.TotalLetra);
            file.WriteLine("");

            file.WriteLine("[Otros]");
            file.WriteLine("ClaveTransportista|");
            file.WriteLine("NoRelacionPemex|");
            file.WriteLine("NoConvenioPemex|");
            file.WriteLine("NoCedulaPemex|");
            file.WriteLine("AireacionYSecado|");
            file.WriteLine("ApoyoEducampo|");
            file.WriteLine("Sanidad|");
            file.WriteLine("");
            file.WriteLine("[Otros]");
            file.WriteLine("LeyendaEspecial1|");
            file.WriteLine("LeyendaEspecial2|");
            file.WriteLine("LeyendaEspecial3|");
            file.Close();

            errores = "1";

        }
        else
        {
            errores = "La ruta de la carpeta no es valida";
        }
        return errores;
    }

    private static string BuzonFiscalTimbradoFactura(int pIdFactura, CConexion pConexion)
    {

        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CSerieFactura SerieFactura = new CSerieFactura();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();

        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFactura), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;

        if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
        {

            StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();
            string Campo = "";


            CTxtTimbradosFactura TxtTimbradosFactura = new CTxtTimbradosFactura();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    string[] split = sLine.Split('|');
                    Campo = split[0];
                    switch (Campo)
                    {
                        case "refId":
                            TxtTimbradosFactura.Refid = Convert.ToString(split[1]);
                            break;
                        case "noCertificadoSAT":
                            TxtTimbradosFactura.NoCertificadoSAT = Convert.ToString(split[1]);
                            break;
                        case "fechaTimbrado":
                            TxtTimbradosFactura.FechaTimbrado = Convert.ToString(split[1]);
                            break;
                        case "uuid":
                            TxtTimbradosFactura.Uuid = Convert.ToString(split[1]);
                            break;
                        case "noCertificado":
                            TxtTimbradosFactura.NoCertificado = Convert.ToString(split[1]);
                            break;
                        case "selloSAT":
                            TxtTimbradosFactura.SelloSAT = Convert.ToString(split[1]);
                            break;
                        case "sello":
                            TxtTimbradosFactura.Sello = Convert.ToString(split[1]);
                            break;
                        case "fecha":
                            TxtTimbradosFactura.Fecha = Convert.ToString(split[1]);
                            break;
                        case "folio":
                            TxtTimbradosFactura.Folio = Convert.ToString(split[1]);
                            break;
                        case "serie":
                            TxtTimbradosFactura.Serie = Convert.ToString(split[1]);
                            break;
                        case "leyendaImpresion":
                            TxtTimbradosFactura.LeyendaImpresion = Convert.ToString(split[1]);
                            break;
                        case "cadenaOriginal":
                            TxtTimbradosFactura.CadenaOriginal = Convert.ToString(split[1]);
                            break;
                        case "totalConLetra":
                            TxtTimbradosFactura.TotalConLetra = Convert.ToString(split[1]);
                            break;
                    }
                }
            }
            TxtTimbradosFactura.Agregar(pConexion);
            FacturaEncabezado.Refid = TxtTimbradosFactura.Refid;
            FacturaEncabezado.Editar(pConexion);
            objReader.Close();

            NombreArchivo = "Factura timbrada correctamente";
        }
        else
        {
            NombreArchivo = "no existe el archivo";
        }

        return NombreArchivo;

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    [WebMethod]
    public static string EliminarNotaCreditoProveedorEncabezadoFacturaProveedor(Dictionary<string, object> pNotaCreditoProveedorEncabezadoFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        CNotaCreditoProveedorEncabezadoFacturaProveedor NotaCreditoProveedorEncabezadoFacturaProveedor = new CNotaCreditoProveedorEncabezadoFacturaProveedor();
        NotaCreditoProveedorEncabezadoFacturaProveedor.LlenaObjeto(Convert.ToInt32(pNotaCreditoProveedorEncabezadoFacturaProveedor["pIdNotaCreditoProveedorEncabezadoFacturaProveedor"]), ConexionBaseDatos);
        NotaCreditoProveedorEncabezadoFacturaProveedor.IdNotaCreditoProveedorEncabezadoFacturaProveedor = Convert.ToInt32(pNotaCreditoProveedorEncabezadoFacturaProveedor["pIdNotaCreditoProveedorEncabezadoFacturaProveedor"]);
        NotaCreditoProveedorEncabezadoFacturaProveedor.Baja = true;
        NotaCreditoProveedorEncabezadoFacturaProveedor.EliminarNotaCreditoProveedorEncabezadoFacturaProveedor(ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        oRespuesta.Add("AbonosNotaCreditoProveedor", NotaCreditoProveedorEncabezadoFacturaProveedor.TotalAbonosNotaCreditoProveedor(NotaCreditoProveedorEncabezadoFacturaProveedor.IdNotaCreditoProveedor, ConexionBaseDatos));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    private static string CancelarArchivoBuzonFiscalFactura(int pIdFacturaEncabezado, CConexion pConexion)
    {
        string errores = "";
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CEmpresa Empresa = new CEmpresa();
        CTxtTimbradosFactura TxtTimbradosFacturaEncabezado = new CTxtTimbradosFactura();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CSerieFactura SerieFactura = new CSerieFactura();
        CValidacion ValidacionRFC = new CValidacion();

        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFacturaEncabezado), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

        Cliente.LlenaObjeto(FacturaEncabezado.IdCliente, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
        ParametrosTxt.Add("Folio", Convert.ToString(FacturaEncabezado.NumeroFactura));
        ParametrosTxt.Add("Serie", Convert.ToString(SerieFactura.SerieFactura));
        TxtTimbradosFacturaEncabezado.LlenaObjetoFiltros(ParametrosTxt, pConexion);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = "cancela_" + SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;

        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
            file.WriteLine("cancela");
            file.WriteLine("########################################################################");
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("uuid|" + TxtTimbradosFacturaEncabezado.Uuid);
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("emRfc|" + Empresa.RFC);
            file.WriteLine("");
            file.WriteLine("#Requerido");
            file.WriteLine("reRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("");
            file.WriteLine("#Opcional");
            file.WriteLine("refID|" + TxtTimbradosFacturaEncabezado.Refid);

            file.Close();

            errores = "1";
        }
        else
        {
            errores = "La ruta de la carpeta no es valida";
        }
        return errores;

    }

    private static string BuzonFiscalTimbradoCancelacionFactura(int pIdFacturaEncabezado, string MotivoCancelacion, CConexion pConexion)
    {
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CSerieFactura SerieFactura = new CSerieFactura();
        CTxtTimbradosFactura TxtTimbradosFacturaEncabezado = new CTxtTimbradosFactura();
        string NombreArchivo = "";

        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIdFacturaEncabezado), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdUsuario), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        SerieFactura.LlenaObjeto(FacturaEncabezado.IdSerieFactura, pConexion);

        Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
        ParametrosTxt.Add("Folio", Convert.ToString(FacturaEncabezado.NumeroFactura));
        ParametrosTxt.Add("Serie", Convert.ToString(SerieFactura.SerieFactura));
        TxtTimbradosFacturaEncabezado.LlenaObjetoFiltros(ParametrosTxt, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = "cancela_" + SerieFactura.SerieFactura + FacturaEncabezado.NumeroFactura;

        if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
        {

            StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();
            string Campo = "";

            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                {
                    string[] split = sLine.Split('|');
                    Campo = split[0];
                    switch (Campo)
                    {
                        case "fecha":
                            TxtTimbradosFacturaEncabezado.FechaCancelacion = Convert.ToString(split[1]);
                            string[] fechaFormateada = split[1].Split('T');
                            if (fechaFormateada[1].Length == 13)
                            {
                                fechaFormateada[1] = "0" + fechaFormateada[1];
                            }
                            FacturaEncabezado.FechaCancelacion = Convert.ToDateTime(fechaFormateada[0] + "T" + fechaFormateada[1]);
                            break;
                    }
                }
            }
            TxtTimbradosFacturaEncabezado.Editar(pConexion);
            FacturaEncabezado.Baja = true;
            FacturaEncabezado.IdEstatusFacturaEncabezado = 2;
            FacturaEncabezado.MotivoCancelacion = MotivoCancelacion;
            FacturaEncabezado.IdUsuarioCancelacion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            FacturaEncabezado.EditarFacturaEncabezado(pConexion);

            objReader.Close();

            NombreArchivo = "Factura cancelada correctamente";
        }
        else
        {
            NombreArchivo = "no existe el archivo";
        }

        return NombreArchivo;

    }


    [WebMethod]
    public static string CambiarEstatus(int pIdNotaCreditoProveedor, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
            NotaCreditoProveedor.IdNotaCreditoProveedor = pIdNotaCreditoProveedor;
            NotaCreditoProveedor.Baja = pBaja;
            NotaCreditoProveedor.Eliminar(ConexionBaseDatos);
            respuesta = "0|NotaCreditoProveedorEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    ////////Nota de credito devolucion
    //////[WebMethod]
    //////public static string ObtenerNumeroNotaCreditoProveedorDevolucion(Dictionary<string, object> pNotaCreditoProveedor)
    //////{
    //////    //Abrir Conexion
    //////    CConexion ConexionBaseDatos = new CConexion();
    //////    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //////    //¿La conexion se establecio?
    //////    if (respuesta == "Conexion Establecida")
    //////    {
    //////        CSerieNotaCreditoProveedor SerieNotaCreditoProveedor = new CSerieNotaCreditoProveedor();
    //////        SerieNotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pNotaCreditoProveedor["IdSerieNotaCreditoProveedor"]), ConexionBaseDatos);
    //////        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
    //////        CUsuario Usuario = new CUsuario();
    //////        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
    //////        int NumeroNotaCreditoProveedor = 0;
    //////        NumeroNotaCreditoProveedor = NotaCreditoProveedor.ObtieneNumeroNotaCreditoProveedor(SerieNotaCreditoProveedor.SerieNotaCreditoProveedor, Usuario.IdSucursalActual, ConexionBaseDatos);

    //////        string validacion = "";
    //////        JObject oRespuesta = new JObject();
    //////        if (validacion == "")
    //////        {
    //////            JObject Modelo = new JObject();
    //////            Modelo.Add("NumeroNotaCreditoProveedor", NumeroNotaCreditoProveedor);
    //////            oRespuesta.Add(new JProperty("Error", 0));
    //////            oRespuesta.Add(new JProperty("Modelo", Modelo));
    //////        }
    //////        else
    //////        {
    //////            oRespuesta.Add(new JProperty("Error", 1));
    //////            oRespuesta.Add(new JProperty("Descripcion", validacion));
    //////        }
    //////        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    //////        return oRespuesta.ToString();
    //////    }
    //////    else
    //////    { return "1|" + respuesta; }
    //////}

    [WebMethod]
    public static string ObtenerTipoCambioDevolucion(Dictionary<string, object> pTipoCambio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCambio TipoCambio = new CTipoCambio();

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Opcion", 1);
            ParametrosTS.Add("IdTipoMonedaOrigen", Convert.ToInt32(pTipoCambio["IdTipoCambioOrigen"]));
            ParametrosTS.Add("IdTipoMonedaDestino", Convert.ToInt32(pTipoCambio["IdTipoCambioDestino"]));
            ParametrosTS.Add("Fecha", DateTime.Today);
            TipoCambio.LlenaObjetoFiltrosTipoCambio(ParametrosTS, ConexionBaseDatos);

            string validacion = "";
            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                JObject Modelo = new JObject();
                Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturasRegistradas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdNotaCreditoProveedor, int pIdTipoNotaCreditoProveedor, int pIdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("sp_grdNotaCreditoProveedorAsociarProductosDevolucionCancelacion", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 30).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdNotaCreditoProveedor", SqlDbType.Int).Value = pIdNotaCreditoProveedor;
        Stored.Parameters.Add("pIdTipoNotaCreditoProveedor", SqlDbType.Int).Value = pIdTipoNotaCreditoProveedor;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarRazonSocialProveedor(string pRazonSocial)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Proveedor_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string LlenaComboTipoNotaCreditoProveedor()
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        JObject NotaCreditoProveedor = new JObject();
        CJson jsonNotaCreditoProveedor = new CJson();
        jsonNotaCreditoProveedor.StoredProcedure.CommandText = "sp_TipoNotaCreditoProveedor_ConsultarTipoNotaCreditoProveedor";
        NotaCreditoProveedor.Add("Opciones", jsonNotaCreditoProveedor.ObtenerJsonJObject(ConexionBaseDatos));

        oRespuesta.Add(new JProperty("Modelo", NotaCreditoProveedor));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }


    //agregar devoluciones
    [WebMethod]
    public static string AgregarDevoluciones(Dictionary<string, object> pRequest)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            if (pRequest["pDevoluciones"].ToString().Length > 0)
            {
                string[] DevolucionesSeleccionadas = { };
                if (pRequest["pDevoluciones"].ToString().Length > 0)
                {
                    DevolucionesSeleccionadas = pRequest["pDevoluciones"].ToString().Split(',');
                }

                decimal totalMonto = 0;
                decimal totalMontoProductos = 0;

                List<CDevolucionProveedor> DevolucionProveedor = new List<CDevolucionProveedor>();
                foreach (Dictionary<string, object> oProductos in (Array)pRequest["IdsFacturas"])
                {
                    CDevolucionProveedor Devoluciones = new CDevolucionProveedor();
                    Devoluciones.IdNotaCreditoProveedor = Convert.ToInt32(pRequest["pIdNotaCreditoProveedor"]);
                    Devoluciones.IdDetalleFacturaProveedor = Convert.ToInt32(oProductos["IdDetalleFactura"]);
                    Devoluciones.Cantidad = Convert.ToInt32(oProductos["Cantidad"]);
                    Devoluciones.AgregarDevolucion(ConexionBaseDatos);
                }
                //Sumar los montos de los productos activos
                CDevolucionProveedor DevolucionesProductosActivos = new CDevolucionProveedor();
                totalMontoProductos = DevolucionesProductosActivos.ObtieneMonto(Convert.ToInt32(pRequest["pIdNotaCreditoProveedor"]), ConexionBaseDatos);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

                CIVA oIVA = new CIVA();
                oIVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

                decimal IVA = 0;
                IVA = Decimal.Round((totalMontoProductos * oIVA.IVA) / 100, 2);

                decimal total = 0;
                total = Decimal.Round(totalMontoProductos + IVA, 2);

                CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pRequest["pIdNotaCreditoProveedor"]), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCreditoProveedor.IdTipoMoneda, ConexionBaseDatos);
                NotaCreditoProveedor.Monto = totalMontoProductos;
                NotaCreditoProveedor.IVA = IVA;
                NotaCreditoProveedor.Total = total;
                NotaCreditoProveedor.SaldoDocumento = total;
                TotalLetras = Utilerias.ConvertLetter(NotaCreditoProveedor.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                NotaCreditoProveedor.TotalLetra = TotalLetras;
                NotaCreditoProveedor.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("TotalMonto", totalMontoProductos));
                oRespuesta.Add(new JProperty("IVA", IVA));
                oRespuesta.Add(new JProperty("Total", total));

                oRespuesta.Add(new JProperty("Error", 0));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();

            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se seleccionaron productos."));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();
            }

        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string EliminarProductosAsociados(string pIdDevoluciones, int pIdNotaCreditoProveedor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        decimal totalMontoProductos = 0;
        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            if (pIdDevoluciones.Length > 0)
            {
                string[] productosSeleccionados = { };
                if (pIdDevoluciones.Length > 0)
                {
                    productosSeleccionados = pIdDevoluciones.Split(',');
                }

                foreach (string oIdDevolucion in productosSeleccionados)
                {
                    CDevolucionProveedor Devolucion = new CDevolucionProveedor();
                    Devolucion.IdDevolucionProveedor = Convert.ToInt32(oIdDevolucion);
                    Devolucion.Baja = true;
                    Devolucion.Eliminar(ConexionBaseDatos);
                }


                //Sumar los montos de los productos activos
                CDevolucionProveedor DevolucionesProductosActivos = new CDevolucionProveedor();
                totalMontoProductos = DevolucionesProductosActivos.ObtieneMonto(pIdNotaCreditoProveedor, ConexionBaseDatos);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

                CIVA oIVA = new CIVA();
                oIVA.LlenaObjeto(Sucursal.IdIVA, ConexionBaseDatos);

                decimal IVA = 0;
                IVA = Decimal.Round((totalMontoProductos * oIVA.IVA) / 100, 2);

                decimal total = 0;
                total = Decimal.Round(totalMontoProductos + IVA, 2);

                CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
                string TotalLetras = "";
                CUtilerias Utilerias = new CUtilerias();
                CTipoMoneda TipoMoneda = new CTipoMoneda();
                NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pIdNotaCreditoProveedor), ConexionBaseDatos);
                TipoMoneda.LlenaObjeto(NotaCreditoProveedor.IdTipoMoneda, ConexionBaseDatos);
                NotaCreditoProveedor.Monto = totalMontoProductos;
                NotaCreditoProveedor.IVA = IVA;
                NotaCreditoProveedor.Total = total;
                NotaCreditoProveedor.SaldoDocumento = total;
                TotalLetras = Utilerias.ConvertLetter(NotaCreditoProveedor.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                NotaCreditoProveedor.TotalLetra = TotalLetras;
                NotaCreditoProveedor.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("TotalMonto", totalMontoProductos));
                oRespuesta.Add(new JProperty("IVA", IVA));
                oRespuesta.Add(new JProperty("Total", total));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                oRespuesta.Add(new JProperty("Error", 0));
                return oRespuesta.ToString();
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "no hay productos seleccionados"));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();
            }
        }
        else
        { return "1|" + respuesta; }
    }
    //Validaciones
    private static string ValidarNotaCreditoProveedor(CNotaCreditoProveedor pNotaCreditoProveedor, CConexion pConexion)
    {
        string errores = "";
        if (pNotaCreditoProveedor.IdProveedor == 0)
        { errores = errores + "<span>*</span> El campo proveedor esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.SerieNotaCredito == "")
        { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.FolioNotaCredito == 0)
        { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCreditoProveedor.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.TipoCambio == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.Monto == 0)
        { errores = errores + "<span>*</span> El campo monto esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCreditoProveedor.PorcentajeIVA == 0)
        { errores = errores + "<span>*</span> El campo porcentaje IVA esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCreditoProveedor.IVA == 0)
        { errores = errores + "<span>*</span> El campo IVA esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCreditoProveedor.Total == 0)
        { errores = errores + "<span>*</span> El campo total esta vacio, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarNotaCreditoProveedorDevolucionCancelacion(CNotaCreditoProveedor pNotaCreditoProveedor, CConexion pConexion)
    {
        string errores = "";
        if (pNotaCreditoProveedor.IdProveedor == 0)
        { errores = errores + "<span>*</span> El campo proveedor esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.SerieNotaCredito == "")
        { errores = errores + "<span>*</span> El campo serie esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.FolioNotaCredito == 0)
        { errores = errores + "<span>*</span> El campo folio esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción esta vacio, favor de capturarlo.<br />"; }

        if (pNotaCreditoProveedor.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (pNotaCreditoProveedor.TipoCambio == 0)
        { errores = errores + "<span>*</span> El campo tipo de cambio esta vacio, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarMontos(CNotaCreditoProveedorEncabezadoFacturaProveedor NotaCreditoProveedor, CFacturaEncabezado FacturaEncabezado, CConexion pConexion)
    {
        string errores = "";

        if (FacturaEncabezado.Parcialidades == true)
        {
            if (FacturaEncabezado.NumeroParcialidadesPendientes == 1)
            {
                if (Convert.ToDecimal(NotaCreditoProveedor.Monto) != Convert.ToDecimal(FacturaEncabezado.SaldoFactura))
                {
                    errores = errores + "<span>*</span> Es la ultima parcialidad, favor de pagar todo el saldo<br />";
                }
            }
        }

        if (NotaCreditoProveedor.IdEncabezadoFacturaProveedor == 0)
        { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

        if (NotaCreditoProveedor.IdNotaCreditoProveedor == 0)
        { errores = errores + "<span>*</span> No hay cuenta por cobrar seleccionada, favor de elegir alguna.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string CrearArchivoBuzonFiscal(int pIdNotaCreditoProveedor, string LugarExpedicion, string MetodoPago, string FormaPago, string CondicionPago, string CuentaBancariaCliente, string Referencia, string Observaciones, CConexion pConexion)
    {
        string errores = "";
        CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CEmpresa Empresa = new CEmpresa();
        CLocalidad Localidad = new CLocalidad();
        CMunicipio Municipio = new CMunicipio();
        CEstado Estado = new CEstado();
        CPais Pais = new CPais();
        CCliente Cliente = new CCliente();
        COrganizacion Organizacion = new COrganizacion();
        CDireccionOrganizacion DireccionOrganizacion = new CDireccionOrganizacion();
        CTipoMoneda TipoMoneda = new CTipoMoneda();
        CValidacion LimpiarRFC = new CValidacion();

        string NombreArchivo = "";

        NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pIdNotaCreditoProveedor), pConexion);
        Usuario.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdUsuarioAlta), pConexion);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);
        Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
        Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
        Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
        Pais.LlenaObjeto(Estado.IdPais, pConexion);
        Cliente.LlenaObjeto(NotaCreditoProveedor.IdProveedor, pConexion);
        Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);
        TipoMoneda.LlenaObjeto(NotaCreditoProveedor.IdTipoMoneda, pConexion);

        Dictionary<string, object> ParametrosDO = new Dictionary<string, object>();
        ParametrosDO.Add("IdOrganizacion", Convert.ToInt32(Organizacion.IdOrganizacion));
        ParametrosDO.Add("IdTipoDireccion", Convert.ToInt32(1));
        DireccionOrganizacion.LlenaObjetoFiltros(ParametrosDO, pConexion);


        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

        NombreArchivo = NotaCreditoProveedor.SerieNotaCredito + NotaCreditoProveedor.FolioNotaCredito;

        if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
            file.WriteLine("HOJA");
            file.WriteLine("########################################################################");
            file.WriteLine("[Datos Generales]");
            file.WriteLine("serie|" + NotaCreditoProveedor.SerieNotaCredito);
            file.WriteLine("folio|" + NotaCreditoProveedor.FolioNotaCredito);
            file.WriteLine("asignaFolio|FALSE");

            file.WriteLine("");

            file.WriteLine("[Datos del Emisor]");
            file.WriteLine("emRegimen|" + Convert.ToString(Empresa.RegimenFiscal));
            file.WriteLine("emRfc|" + Empresa.RFC);
            file.WriteLine("emNombre|" + Empresa.RazonSocial);
            file.WriteLine("emCalle|" + Empresa.Calle);
            file.WriteLine("emNoExterior|" + Empresa.NumeroExterior);
            file.WriteLine("emNoInterior|" + Empresa.NumeroInterior);
            file.WriteLine("emColonia|" + Empresa.Colonia);
            file.WriteLine("emLocalidad|" + Localidad.Localidad);
            file.WriteLine("emReferencia|" + Empresa.Referencia);
            file.WriteLine("emMunicipio|" + Municipio.Municipio);
            file.WriteLine("emEstado|" + Estado.Estado);
            file.WriteLine("emPais|" + Pais.Pais);
            file.WriteLine("emCodigoPostal|" + Empresa.CodigoPostal);
            file.WriteLine("emProveedor|");
            file.WriteLine("emGLN|");

            file.WriteLine("");

            file.WriteLine("[Datos de Expedicion]");

            if (Sucursal.DireccionFiscal == true)
            {
                file.WriteLine("exAlias|");
                file.WriteLine("exTelefono|");
                file.WriteLine("exCalle|");
                file.WriteLine("exNoExterior|");
                file.WriteLine("exNoInterior|");
                file.WriteLine("exColonia|");
                file.WriteLine("exLocalidad|");
                file.WriteLine("exReferencia|");
                file.WriteLine("exMunicipio|");
                file.WriteLine("exEstado|");
                file.WriteLine("exPais|");
                file.WriteLine("exCodigoPostal|");
            }
            else
            {
                Localidad.LlenaObjeto(Sucursal.IdLocalidad, pConexion);
                Municipio.LlenaObjeto(Sucursal.IdMunicipio, pConexion);
                Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
                Pais.LlenaObjeto(Estado.IdPais, pConexion);

                file.WriteLine("exAlias|" + Sucursal.Alias);
                file.WriteLine("exTelefono|" + Sucursal.Telefono);
                file.WriteLine("exCalle|" + Sucursal.Calle);
                file.WriteLine("exNoExterior|" + Sucursal.NumeroExterior);
                file.WriteLine("exNoInterior|" + Sucursal.NumeroInterior);
                file.WriteLine("exColonia|" + Sucursal.Colonia);
                file.WriteLine("exLocalidad|" + Localidad.Localidad);
                file.WriteLine("exReferencia|" + Sucursal.Referencia);
                file.WriteLine("exMunicipio|" + Municipio.Municipio);
                file.WriteLine("exEstado|" + Estado.Estado);
                file.WriteLine("exPais|" + Pais.Pais);
                file.WriteLine("exCodigoPostal|" + Sucursal.CodigoPostal);
            }

            file.WriteLine("");

            Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("[Datos del Receptor]");
            file.WriteLine("reRfc|" + LimpiarRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("reNombre|" + Organizacion.RazonSocial);
            file.WriteLine("reCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("reNoExterior|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("reNoInterior|" + DireccionOrganizacion.NumeroInterior);
            file.WriteLine("reColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("reLocalidad|" + Localidad.Localidad);
            file.WriteLine("reReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("reMunicipio|" + Municipio.Municipio);
            file.WriteLine("reEstado|" + Estado.Estado);
            file.WriteLine("rePais|" + Pais.Pais);
            file.WriteLine("reCodigoPostal|" + DireccionOrganizacion.CodigoPostal);
            file.WriteLine("reNoCliente|" + Cliente.IdCliente);
            file.WriteLine("reEmail|" + Cliente.Correo);
            file.WriteLine("reTelefono|" + DireccionOrganizacion.ConmutadorTelefono);
            file.WriteLine("reFax|");
            file.WriteLine("reComprador|");
            file.WriteLine("reNIM|");

            file.WriteLine("");

            Localidad.LlenaObjeto(Empresa.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);

            file.WriteLine("[Datos del Remitente]");

            file.WriteLine("remRfc|" + Empresa.RFC);
            file.WriteLine("remNombre|" + Empresa.RazonSocial);
            file.WriteLine("remClaveIdentificacion|");
            file.WriteLine("remCalle|" + Empresa.Calle);
            file.WriteLine("remNumero|" + Empresa.NumeroExterior);
            file.WriteLine("remReferencia|" + Empresa.Referencia);
            file.WriteLine("remColonia|" + Empresa.Colonia);
            file.WriteLine("remCiudad|" + Localidad.Localidad);
            file.WriteLine("remMunicipio|" + Municipio.Municipio);
            file.WriteLine("remEstado|" + Estado.Estado);
            file.WriteLine("remPais|" + Pais.Pais);
            file.WriteLine("remCodigoPostal|" + Empresa.CodigoPostal);

            file.WriteLine("");

            file.WriteLine("[Datos del Destino]");

            Localidad.LlenaObjeto(DireccionOrganizacion.IdLocalidad, pConexion);
            Municipio.LlenaObjeto(DireccionOrganizacion.IdMunicipio, pConexion);
            Estado.LlenaObjeto(Municipio.IdEstado, pConexion);
            Pais.LlenaObjeto(Estado.IdPais, pConexion);


            file.WriteLine("desRfc|" + LimpiarRFC.LimpiarRFC(Organizacion.RFC));
            file.WriteLine("desNombre|" + Organizacion.RazonSocial);
            file.WriteLine("desClaveIdentificacion|");
            file.WriteLine("desCalle|" + DireccionOrganizacion.Calle);
            file.WriteLine("desNumero|" + DireccionOrganizacion.NumeroExterior);
            file.WriteLine("desReferencia|" + DireccionOrganizacion.Referencia);
            file.WriteLine("desColonia|" + DireccionOrganizacion.Colonia);
            file.WriteLine("desCiudad|" + Localidad.Localidad);
            file.WriteLine("desMunicipio|" + Municipio.Municipio);
            file.WriteLine("desEstado|" + Estado.Estado);
            file.WriteLine("desPais|" + Pais.Pais);
            file.WriteLine("desCodigoPostal|" + DireccionOrganizacion.CodigoPostal);

            file.WriteLine("");

            file.WriteLine("[Datos de Conceptos]");
            file.WriteLine("cantidad|1");
            file.WriteLine("unidad|No Aplica");
            file.WriteLine("numIdentificacion|Nota de Credito");
            file.WriteLine("descripcion|" + NotaCreditoProveedor.Descripcion);
            file.WriteLine("valorUnitario|" + NotaCreditoProveedor.Monto);
            file.WriteLine("importe|" + NotaCreditoProveedor.Monto);
            file.WriteLine("#1  Cuenta Predial,");
            file.WriteLine("cpNumero|");
            file.WriteLine("#2  Informacion Aduanera");
            file.WriteLine("iaNumero|");
            file.WriteLine("iaFecha|");
            file.WriteLine("#3  Parte");
            file.WriteLine("parteCantidad|");
            file.WriteLine("parteUnidad|");
            file.WriteLine("parteNumIdentificacion|");
            file.WriteLine("parteDescripcion|");
            file.WriteLine("parteValorUnitario|");
            file.WriteLine("parteImporte|");
            file.WriteLine("#Bloque de Datos opcionales para");
            file.WriteLine("# introducir la informacion aduanera");
            file.WriteLine("parteIaNumero|");
            file.WriteLine("parteIaFecha|");
            file.WriteLine("parteIaAduana|");
            file.WriteLine("");
            file.WriteLine("#4 Complemento Concepto");
            file.WriteLine("");
            file.WriteLine("[Datos Complementarios para especificar la venta de vehiculos]");
            file.WriteLine("");
            file.WriteLine("claveVehicular|");
            file.WriteLine("");
            file.WriteLine("vehiculoIaNumero|");
            file.WriteLine("");
            file.WriteLine("vehiculoIaFecha|");
            file.WriteLine("");
            file.WriteLine("vehiculoIaAduana|");
            file.WriteLine("");
            file.WriteLine("#PARTES");
            file.WriteLine("vehiculoparteCantidad|");
            file.WriteLine("vehiculoparteUnidad|");
            file.WriteLine("vehiculopartenoIdentificacion|");
            file.WriteLine("vehiculoparteDescripcion|");
            file.WriteLine("vehiculoparteValorUnitario|");
            file.WriteLine("vehiculoparteImporte|");
            file.WriteLine("vehiculoparteIaNumero|");
            file.WriteLine("vehiculoparteIaFecha|");
            file.WriteLine("vehiculoparteIaAduana|");
            file.WriteLine("");
            file.WriteLine("[Complemento Dutty Free]");
            file.WriteLine("");
            file.WriteLine("dutFreeVersion|");
            file.WriteLine("dutFreeFechaTran|");
            file.WriteLine("dutFreeTipoTran|");
            file.WriteLine("# Datos Transito");
            file.WriteLine("dutFreeDatVia|");
            file.WriteLine("dutFreeDatTipoID|");
            file.WriteLine("dutFreeDatNumeroId|");
            file.WriteLine("dutFreeDatNacio|");
            file.WriteLine("dutFreeDatTransporte|");
            file.WriteLine("dutFreeDatidTransporte|");
            file.WriteLine("");
            file.WriteLine("[Datos Extra Conceptos]");
            file.WriteLine("ConExReferencia1|");
            file.WriteLine("ConExReferencia2|");
            file.WriteLine("ConExIndicador|");
            file.WriteLine("ConExDescripcionIngles|");
            file.WriteLine("ConExNumRemision|0");
            file.WriteLine("ConExCargo|");
            file.WriteLine("ConExDescuento|0");
            file.WriteLine("ConExMensaje|");
            file.WriteLine("ConExTasaImpuesto|");
            file.WriteLine("ConExImpuesto|");
            file.WriteLine("ConExValorUnitarioMonedaExtranjera|");
            file.WriteLine("ConExImporteMonedaExtranjera|");
            file.WriteLine("ConExtunitarioBruto|");
            file.WriteLine("ConExtImporteBruto|");
            file.WriteLine("ConExCvDivisas|");
            file.WriteLine("ConExtItemIdAlterno|");
            file.WriteLine("ConExtItemAlternoClave|");
            file.WriteLine("ConExtItemAlternoValor|");
            file.WriteLine("ConExtVendorPack|");

            file.WriteLine("");


            file.WriteLine("[Datos Complementarios del Comprobante a nivel global]");
            file.WriteLine("subtotalConceptos|" + NotaCreditoProveedor.Monto);
            file.WriteLine("descuentoPorcentaje|");
            file.WriteLine("descuentoMonto|");
            file.WriteLine("descuentoMotivo|");
            file.WriteLine("cargos|");
            file.WriteLine("totalConceptos|" + NotaCreditoProveedor.Monto);
            file.WriteLine("");
            file.WriteLine("pagoForma|" + FormaPago);
            file.WriteLine("pagoCondiciones|" + CondicionPago);
            file.WriteLine("pagoMetodo|" + MetodoPago);
            file.WriteLine("numCtaPago|" + CuentaBancariaCliente);
            file.WriteLine("lugarExpedicion|" + LugarExpedicion);
            file.WriteLine("serieFolioFiscalOrig|");
            file.WriteLine("folioFiscalOrig|");
            file.WriteLine("montoFolioFiscalOrig|");
            file.WriteLine("fechaFolioFiscalOrig|");

            file.WriteLine("");

            file.WriteLine("[Datos Complementarios del Comprobante a nivel global para casos de importaci—n o exportaci—n de bienes]");
            file.WriteLine("#Datos Globales de Aduana, el bloque es opcional y se constituye por los siguientes tres datos, el bloque se repite para cada aduana que aplique.");
            file.WriteLine("comiaNumero|");
            file.WriteLine("comiaFecha|");
            file.WriteLine("comiaAduana|");
            file.WriteLine("embarque|");
            file.WriteLine("fob|");
            file.WriteLine("");
            file.WriteLine("[Datos Comerciales del Comprobante a nivel global]  Datos adicionales de tipo comercial comœnmente usados.");
            if (NotaCreditoProveedor.Refid != "")
            {
                file.WriteLine("refID|" + NotaCreditoProveedor.Refid);
            }
            else
            {
                file.WriteLine("refID|" + "2" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString());
            }
            file.WriteLine("tipoDocumento|Nota de Credito");

            file.WriteLine("ordenCompra|");
            file.WriteLine("agente|" + Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
            file.WriteLine("observaciones|" + Observaciones);
            file.WriteLine("nombreMoneda|" + TipoMoneda.TipoMoneda);
            file.WriteLine("tipoCambio|" + NotaCreditoProveedor.TipoCambio);
            file.WriteLine("");
            file.WriteLine("[Impuestos Trasladados]");
            file.WriteLine("trasladadoImpuesto|IVA");
            file.WriteLine("trasladadoImporte|" + NotaCreditoProveedor.IVA);
            file.WriteLine("trasladadoTasa|" + Sucursal.IVAActual);
            file.WriteLine("subtotalTrasladados|" + NotaCreditoProveedor.IVA);
            file.WriteLine("");
            file.WriteLine("[Impuestos Retenidos]");
            file.WriteLine("retenidoImpuesto|");
            file.WriteLine("retenidoImporte|");
            file.WriteLine("subtotalRetenidos|");
            file.WriteLine("");
            file.WriteLine("[IMPUESTOS LOCALES]");
            file.WriteLine("version|");
            file.WriteLine("totalTraslados|");
            file.WriteLine("totalRetenciones|");
            file.WriteLine("");
            file.WriteLine("[TRASLADOS LOCALES]");
            file.WriteLine("impLocTrasladado|");
            file.WriteLine("tasaDeTraslado|");
            file.WriteLine("importeTraslados|");
            file.WriteLine("");
            file.WriteLine("[RETENCIONES LOCALES]");
            file.WriteLine("impLocRetenido|");
            file.WriteLine("tasaDeRetencion|");
            file.WriteLine("importeRetenciones|");
            file.WriteLine("");
            file.WriteLine("[Datos Totales]");
            file.WriteLine("montoTotal|" + NotaCreditoProveedor.Total);
            file.WriteLine("montoTotalTexto|" + NotaCreditoProveedor.TotalLetra);
            file.WriteLine("");

            file.WriteLine("[Otros]");
            file.WriteLine("ClaveTransportista|");
            file.WriteLine("NoRelacionPemex|");
            file.WriteLine("NoConvenioPemex|");
            file.WriteLine("NoCedulaPemex|");
            file.WriteLine("AireacionYSecado|");
            file.WriteLine("ApoyoEducampo|");
            file.WriteLine("Sanidad|");
            file.WriteLine("");
            file.WriteLine("[Otros]");
            file.WriteLine("LeyendaEspecial1|");
            file.WriteLine("LeyendaEspecial2|");
            file.WriteLine("LeyendaEspecial3|");
            file.Close();

            errores = "1";

        }
        else
        {
            errores = "La ruta de la carpeta no es valida";
        }
        return errores;
    }

    ////////private static string CancelarArchivoBuzonFiscal(int pIdNotaCreditoProveedor, CConexion pConexion)
    ////////{
    ////////    string errores = "";
    ////////    CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
    ////////    CUsuario Usuario = new CUsuario();
    ////////    CSucursal Sucursal = new CSucursal();
    ////////    CRutaCFDI RutaCFDI = new CRutaCFDI();
    ////////    CEmpresa Empresa = new CEmpresa();
    ////////    CTxtTimbradosNotaCreditoProveedor TxtTimbradosNotaCreditoProveedor = new CTxtTimbradosNotaCreditoProveedor();
    ////////    CCliente Cliente = new CCliente();
    ////////    COrganizacion Organizacion = new COrganizacion();
    ////////    CValidacion ValidacionRFC = new CValidacion();

    ////////    string NombreArchivo = "";

    ////////    NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pIdNotaCreditoProveedor), pConexion);
    ////////    Usuario.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdUsuarioAlta), pConexion);
    ////////    Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);
    ////////    Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

    ////////    Cliente.LlenaObjeto(NotaCreditoProveedor.IdCliente, pConexion);
    ////////    Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);            

    ////////    Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
    ////////    ParametrosTxt.Add("Folio", NotaCreditoProveedor.FolioNotaCreditoProveedor);
    ////////    ParametrosTxt.Add("Serie", NotaCreditoProveedor.SerieNotaCreditoProveedor);
    ////////    TxtTimbradosNotaCreditoProveedor.LlenaObjetoFiltros(ParametrosTxt, pConexion);

    ////////    Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
    ////////    ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
    ////////    ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
    ////////    RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

    ////////    NombreArchivo = "cancela_" + NotaCreditoProveedor.SerieNotaCreditoProveedor + NotaCreditoProveedor.FolioNotaCreditoProveedor;

    ////////    if (Directory.Exists(RutaCFDI.RutaCFDI + "\\in"))
    ////////    {
    ////////        System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt");
    ////////        file.WriteLine("cancela");
    ////////        file.WriteLine("########################################################################");
    ////////        file.WriteLine("");
    ////////        file.WriteLine("#Requerido");
    ////////        file.WriteLine("uuid|" + TxtTimbradosNotaCreditoProveedor.Uuid);
    ////////        file.WriteLine("");
    ////////        file.WriteLine("#Requerido");
    ////////        file.WriteLine("emRfc|" + Empresa.RFC);
    ////////        file.WriteLine("");
    ////////        file.WriteLine("#Requerido");
    ////////        file.WriteLine("reRfc|" + ValidacionRFC.LimpiarRFC(Organizacion.RFC));
    ////////        file.WriteLine("");
    ////////        file.WriteLine("#Opcional");
    ////////        file.WriteLine("refID|" + TxtTimbradosNotaCreditoProveedor.Refid);

    ////////        file.Close();

    ////////        errores = "1";
    ////////    }
    ////////    else
    ////////    {
    ////////        errores = "La ruta de la carpeta no es valida";
    ////////    }
    ////////    return errores;

    ////////}

    //////private static string BuzonFiscalTimbrado(int pIdNotaCreditoProveedor, CConexion pConexion)
    //////{
    //////    CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
    //////    CUsuario Usuario = new CUsuario();
    //////    CSucursal Sucursal = new CSucursal();
    //////    CRutaCFDI RutaCFDI = new CRutaCFDI();

    //////    string NombreArchivo = "";

    //////    NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pIdNotaCreditoProveedor), pConexion);
    //////    Usuario.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdUsuarioAlta), pConexion);
    //////    Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);


    //////    Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
    //////    ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
    //////    ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
    //////    RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

    //////    NombreArchivo = NotaCreditoProveedor.SerieNotaCreditoProveedor + NotaCreditoProveedor.FolioNotaCreditoProveedor;

    //////    if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
    //////    {

    //////        StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
    //////        string sLine = "";
    //////        ArrayList arrText = new ArrayList();
    //////        string Campo ="";
    //////        CTxtTimbradosNotaCreditoProveedor TxtTimbradosNotaCreditoProveedor = new CTxtTimbradosNotaCreditoProveedor(); 
    //////        while (sLine != null)
    //////        {
    //////            sLine = objReader.ReadLine();
    //////            if (sLine != null)
    //////            {
    //////                string[] split = sLine.Split('|');
    //////                Campo = split[0];
    //////                switch (Campo)
    //////                {
    //////                    case "refId":
    //////                        TxtTimbradosNotaCreditoProveedor.Refid = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "noCertificadoSAT":
    //////                        TxtTimbradosNotaCreditoProveedor.NoCertificadoSAT = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "fechaTimbrado":
    //////                        TxtTimbradosNotaCreditoProveedor.FechaTimbrado = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "uuid":
    //////                        TxtTimbradosNotaCreditoProveedor.Uuid = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "noCertificado":
    //////                        TxtTimbradosNotaCreditoProveedor.NoCertificado = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "selloSAT":
    //////                        TxtTimbradosNotaCreditoProveedor.SelloSAT = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "sello":
    //////                        TxtTimbradosNotaCreditoProveedor.Sello = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "fecha":
    //////                        TxtTimbradosNotaCreditoProveedor.Fecha = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "folio":
    //////                        TxtTimbradosNotaCreditoProveedor.Folio = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "serie":
    //////                        TxtTimbradosNotaCreditoProveedor.Serie = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "leyendaImpresion":
    //////                        TxtTimbradosNotaCreditoProveedor.LeyendaImpresion = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "cadenaOriginal":
    //////                        TxtTimbradosNotaCreditoProveedor.CadenaOriginal = Convert.ToString(split[1]);
    //////                        break;
    //////                    case "totalConLetra":
    //////                        TxtTimbradosNotaCreditoProveedor.TotalConLetra = Convert.ToString(split[1]);
    //////                        break;
    //////                }
    //////            }
    //////        }
    //////        TxtTimbradosNotaCreditoProveedor.Agregar(pConexion);
    //////        NotaCreditoProveedor.Refid = TxtTimbradosNotaCreditoProveedor.Refid;
    //////        NotaCreditoProveedor.Editar(pConexion);
    //////        objReader.Close();               

    //////        NombreArchivo = "Nota de crédito timbrada correctamente";
    //////    }
    //////    else            
    //////    {
    //////        NombreArchivo = "no existe el archivo";
    //////    }

    //////    return NombreArchivo;

    //////}

    //////////private static string BuzonFiscalTimbradoCancelacion(int pIdNotaCreditoProveedor, string MotivoCancelacion, CConexion pConexion)
    //////////{
    //////////    CNotaCreditoProveedor NotaCreditoProveedor = new CNotaCreditoProveedor();
    //////////    CUsuario Usuario = new CUsuario();
    //////////    CSucursal Sucursal = new CSucursal();
    //////////    CRutaCFDI RutaCFDI = new CRutaCFDI();
    //////////    CTxtTimbradosNotaCreditoProveedor TxtTimbradosNotaCreditoProveedor = new CTxtTimbradosNotaCreditoProveedor();
    //////////    string NombreArchivo = "";

    //////////    NotaCreditoProveedor.LlenaObjeto(Convert.ToInt32(pIdNotaCreditoProveedor), pConexion);
    //////////    Usuario.LlenaObjeto(Convert.ToInt32(NotaCreditoProveedor.IdUsuarioAlta), pConexion);
    //////////    Sucursal.LlenaObjeto(Usuario.IdSucursalActual, pConexion);


    //////////    Dictionary<string, object> ParametrosTxt = new Dictionary<string, object>();
    //////////    ParametrosTxt.Add("Folio", Convert.ToString(NotaCreditoProveedor.FolioNotaCreditoProveedor));
    //////////    ParametrosTxt.Add("Serie", Convert.ToString(NotaCreditoProveedor.SerieNotaCreditoProveedor));
    //////////    TxtTimbradosNotaCreditoProveedor.LlenaObjetoFiltros(ParametrosTxt, pConexion);         


    //////////    Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
    //////////    ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
    //////////    ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
    //////////    RutaCFDI.LlenaObjetoFiltros(ParametrosTS, pConexion);

    //////////    NombreArchivo = "cancela_" + NotaCreditoProveedor.SerieNotaCreditoProveedor + NotaCreditoProveedor.FolioNotaCreditoProveedor;

    //////////    if (File.Exists(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt"))
    //////////    {

    //////////        StreamReader objReader = new StreamReader(RutaCFDI.RutaCFDI + "\\out\\" + NombreArchivo + ".txt");
    //////////        string sLine = "";
    //////////        ArrayList arrText = new ArrayList();
    //////////        string Campo = "";

    //////////        while (sLine != null)
    //////////        {
    //////////            sLine = objReader.ReadLine();
    //////////            if (sLine != null)
    //////////            {
    //////////                string[] split = sLine.Split('|');
    //////////                Campo = split[0];
    //////////                switch (Campo)
    //////////                {
    //////////                    case "fecha":                                
    //////////                        TxtTimbradosNotaCreditoProveedor.FechaCancelacion = Convert.ToString(split[1]);
    //////////                        string[] fechaFormateada = split[1].Split('T');
    //////////                        if (fechaFormateada[1].Length == 13)
    //////////                        {
    //////////                            fechaFormateada[1] = "0" + fechaFormateada[1];
    //////////                        }
    //////////                        NotaCreditoProveedor.FechaCancelacion = Convert.ToDateTime(fechaFormateada[0] + "T" + fechaFormateada[1]);
    //////////                        break;
    //////////                }
    //////////            }
    //////////        }
    //////////        TxtTimbradosNotaCreditoProveedor.Editar(pConexion);
    //////////        NotaCreditoProveedor.Baja = true;
    //////////        NotaCreditoProveedor.MotivoCancelacion = MotivoCancelacion;
    //////////        NotaCreditoProveedor.IdUsuarioCancelacion = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
    //////////        NotaCreditoProveedor.EditarNotaCreditoProveedorCancelacion(pConexion);

    //////////        objReader.Close();               

    //////////        NombreArchivo = "Nota de crédito cancelada correctamente";
    //////////    }
    //////////    else
    //////////    {
    //////////        NombreArchivo = "no existe el archivo";
    //////////    }

    //////////    return NombreArchivo;

    //////////}
}