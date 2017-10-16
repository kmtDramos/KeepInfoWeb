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

public partial class Depositos : System.Web.UI.Page
{
    public static int puedeAgregarDepositos = 0;
    public static int puedeEditarDepositos = 0;
    public static int puedeEliminarDepositos = 0;

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

        puedeAgregarDepositos = Usuario.TienePermisos(new string[] { "puedeAgregarDepositos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarDepositos = Usuario.TienePermisos(new string[] { "puedeEditarDepositos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarDepositos = Usuario.TienePermisos(new string[] { "puedeEliminarDepositos" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridDepositos
        CJQGrid GridDepositos = new CJQGrid();
        GridDepositos.NombreTabla = "grdDepositos";
        GridDepositos.CampoIdentificador = "IdDepositos";
        GridDepositos.ColumnaOrdenacion = "Sucursal, Folio";
        GridDepositos.Metodo = "ObtenerDepositos";
        GridDepositos.TituloTabla = "Depositos";
        GridDepositos.GenerarFuncionFiltro = false;

        //IdDepositos
        CJQColumn ColIdDepositos = new CJQColumn();
        ColIdDepositos.Nombre = "IdDepositos";
        ColIdDepositos.Oculto = "true";
        ColIdDepositos.Encabezado = "IdDepositos";
        ColIdDepositos.Buscador = "false";
        GridDepositos.Columnas.Add(ColIdDepositos);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "100";
        ColSucursal.Buscador = "false";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridDepositos.Columnas.Add(ColSucursal);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "80";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        GridDepositos.Columnas.Add(ColFolio);

        //Importe
        CJQColumn ColImporte = new CJQColumn();
        ColImporte.Nombre = "Importe";
        ColImporte.Encabezado = "Importe";
        ColImporte.Buscador = "false";
        ColImporte.Formato = "FormatoMoneda";
        ColImporte.Alineacion = "right";
        ColImporte.Ancho = "80";
        GridDepositos.Columnas.Add(ColImporte);

        //Cuenta
        CJQColumn ColCuenta = new CJQColumn();
        ColCuenta.Nombre = "CuentaBancaria";
        ColCuenta.Encabezado = "Cuenta bancaria";
        ColCuenta.Buscador = "false";
        ColCuenta.Alineacion = "left";
        ColCuenta.Ancho = "100";
        GridDepositos.Columnas.Add(ColCuenta);

        //CuentaBancaria
        CJQColumn ColNombreCuentaBancaria = new CJQColumn();
        ColNombreCuentaBancaria.Nombre = "NombreCuentaBancaria";
        ColNombreCuentaBancaria.Encabezado = "Banco";
        ColNombreCuentaBancaria.Buscador = "false";
        ColNombreCuentaBancaria.Alineacion = "left";
        ColNombreCuentaBancaria.Ancho = "100";
        GridDepositos.Columnas.Add(ColNombreCuentaBancaria);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "80";
        GridDepositos.Columnas.Add(ColTipoMoneda);

        //FechaEmision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha de captura";
        ColFechaEmision.Buscador = "false";
        ColFechaEmision.Alineacion = "left";
        ColFechaEmision.Ancho = "80";
        GridDepositos.Columnas.Add(ColFechaEmision);

        //FechaAplicacion
        CJQColumn ColFechaAplicacion = new CJQColumn();
        ColFechaAplicacion.Nombre = "FechaAplicacion";
        ColFechaAplicacion.Encabezado = "Fecha acreditado";
        ColFechaAplicacion.Buscador = "false";
        ColFechaAplicacion.Alineacion = "left";
        ColFechaAplicacion.Ancho = "80";
        GridDepositos.Columnas.Add(ColFechaAplicacion);

        //Conciliado
        CJQColumn ColConciliado = new CJQColumn();
        ColConciliado.Nombre = "Conciliado";
        ColConciliado.Encabezado = "Conciliado";
        ColConciliado.Buscador = "false";
        ColConciliado.Alineacion = "left";
        ColConciliado.Ancho = "40";
        GridDepositos.Columnas.Add(ColConciliado);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "150";
        GridDepositos.Columnas.Add(ColRazonSocial);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        //ColBaja.Oculto = puedeEliminarDepositos == 1 ? "false" : "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarDepositos == 1 ? "A/I" : "A/IDeshabilitado";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridDepositos.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarDepositos";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridDepositos.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDepositos", GridDepositos.GeneraGrid(), true);

        //GridCuentaBancaria
        CJQGrid GridCuentaBancaria = new CJQGrid();
        GridCuentaBancaria.NombreTabla = "grdCuentaBancaria";
        GridCuentaBancaria.CampoIdentificador = "IdCuentaBancaria";
        GridCuentaBancaria.ColumnaOrdenacion = "CuentaBancaria";
        GridCuentaBancaria.TipoOrdenacion = "DESC";
        GridCuentaBancaria.Metodo = "ObtenerCuentaBancaria";
        GridCuentaBancaria.TituloTabla = "Cuentas bancarias";
        GridCuentaBancaria.GenerarFuncionFiltro = true;
        GridCuentaBancaria.GenerarFuncionTerminado = false;
        GridCuentaBancaria.Altura = 300;
        GridCuentaBancaria.Ancho = 600;
        GridCuentaBancaria.NumeroRegistros = 30;
        GridCuentaBancaria.RangoNumeroRegistros = "15,30,60";

        //IdCuentaBancaria
        CJQColumn ColIdCuentaBancaria = new CJQColumn();
        ColIdCuentaBancaria.Nombre = "IdCuentaBancaria";
        ColIdCuentaBancaria.Oculto = "true";
        ColIdCuentaBancaria.Encabezado = "IdCuentaBancaria";
        ColIdCuentaBancaria.Buscador = "false";
        GridCuentaBancaria.Columnas.Add(ColIdCuentaBancaria);

        //CuentaBancaria
        CJQColumn ColCuentaBancaria = new CJQColumn();
        ColCuentaBancaria.Nombre = "CuentaBancaria";
        ColCuentaBancaria.Encabezado = "Cuenta bancaria";
        ColCuentaBancaria.Buscador = "true";
        ColCuentaBancaria.Alineacion = "left";
        ColCuentaBancaria.Ancho = "200";
        GridCuentaBancaria.Columnas.Add(ColCuentaBancaria);

        //Banco
        CJQColumn ColBanco = new CJQColumn();
        ColBanco.Nombre = "Banco";
        ColBanco.Encabezado = "Banco";
        ColBanco.Ancho = "50";
        ColBanco.Buscador = "false";
        ColBanco.Alineacion = "left";
        GridCuentaBancaria.Columnas.Add(ColBanco);

        //TipoMonedaCuentaBancaria
        CJQColumn ColTipoMonedaCuentaBancaria = new CJQColumn();
        ColTipoMonedaCuentaBancaria.Nombre = "TipoMoneda";
        ColTipoMonedaCuentaBancaria.Encabezado = "Tipo de moneda";
        ColTipoMonedaCuentaBancaria.Buscador = "false";
        ColTipoMonedaCuentaBancaria.Alineacion = "left";
        ColTipoMonedaCuentaBancaria.Ancho = "50";
        GridCuentaBancaria.Columnas.Add(ColTipoMonedaCuentaBancaria);

        //SeleccionarCuentaBancaria
        CJQColumn ColSeleccionarCuenta = new CJQColumn();
        ColSeleccionarCuenta.Nombre = "Seleccionar";
        ColSeleccionarCuenta.Encabezado = "Seleccionar";
        ColSeleccionarCuenta.Etiquetado = "Imagen";
        ColSeleccionarCuenta.Imagen = "select.png";
        ColSeleccionarCuenta.Estilo = "divImagenConsultar imgSeleccionarCuentaBancaria";
        ColSeleccionarCuenta.Buscador = "false";
        ColSeleccionarCuenta.Ordenable = "false";
        ColSeleccionarCuenta.Ancho = "25";
        GridCuentaBancaria.Columnas.Add(ColSeleccionarCuenta);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCuentaBancaria", GridCuentaBancaria.GeneraGrid(), true);

        //GridFacturas
        CJQGrid GridFacturas = new CJQGrid();
        GridFacturas.NombreTabla = "grdFacturas";
        GridFacturas.CampoIdentificador = "IdEncabezadoFactura";
        GridFacturas.ColumnaOrdenacion = "IdFacturaEncabezado";
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
        ColIdFactura.Nombre = "IdEncabezadoFactura";
        ColIdFactura.Oculto = "true";
        ColIdFactura.Encabezado = "IdEncabezadoFactura";
        ColIdFactura.Buscador = "false";
        GridFacturas.Columnas.Add(ColIdFactura);

        //Fecha factura
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridFacturas.Columnas.Add(ColFecha);

        //Serie factura
        CJQColumn ColSerie = new CJQColumn();
        ColSerie.Nombre = "Serie";
        ColSerie.Encabezado = "Serie";
        ColSerie.Buscador = "true";
        ColSerie.Alineacion = "left";
        ColSerie.Ancho = "80";
        GridFacturas.Columnas.Add(ColSerie);

        //Folio factura
        CJQColumn ColFolioFactura = new CJQColumn();
        ColFolioFactura.Nombre = "Folio";
        ColFolioFactura.Encabezado = "Folio";
        ColFolioFactura.Buscador = "false";
        ColFolioFactura.Alineacion = "left";
        ColFolioFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColFolioFactura);

        //Total factura
        CJQColumn ColTotalFactura = new CJQColumn();
        ColTotalFactura.Nombre = "Total";
        ColTotalFactura.Encabezado = "Total";
        ColTotalFactura.Buscador = "false";
        ColTotalFactura.Formato = "FormatoMoneda";
        ColTotalFactura.Alineacion = "right";
        ColTotalFactura.Ancho = "80";
        GridFacturas.Columnas.Add(ColTotalFactura);

        //Tipo cambio
        CJQColumn ColTipoCambio = new CJQColumn();
        ColTipoCambio.Nombre = "TipoCambio";
        ColTipoCambio.Encabezado = "Tipo de cambio";
        ColTipoCambio.Buscador = "false";
        ColTipoCambio.Alineacion = "left";
        ColTipoCambio.Ancho = "80";
        GridFacturas.Columnas.Add(ColTipoCambio);

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
        grdMovimientosCobros.CampoIdentificador = "IdDepositosEncabezadoFactura";
        grdMovimientosCobros.ColumnaOrdenacion = "IdDepositosEncabezadoFactura";
        grdMovimientosCobros.TipoOrdenacion = "DESC";
        grdMovimientosCobros.Metodo = "ObtenerMovimientosCobros";
        grdMovimientosCobros.TituloTabla = "Documentos asociados";
        grdMovimientosCobros.GenerarFuncionFiltro = false;
        grdMovimientosCobros.GenerarFuncionTerminado = false;
        grdMovimientosCobros.Altura = 120;
        grdMovimientosCobros.NumeroRegistros = 15;
        grdMovimientosCobros.RangoNumeroRegistros = "15,30,60";

        //IdDepositosEncabezadoFactura
        CJQColumn ColIdDepositosEncabezadoFactura = new CJQColumn();
        ColIdDepositosEncabezadoFactura.Nombre = "IdDepositosEncabezadoFactura";
        ColIdDepositosEncabezadoFactura.Oculto = "true";
        ColIdDepositosEncabezadoFactura.Encabezado = "IdDepositosEncabezadoFactura";
        ColIdDepositosEncabezadoFactura.Buscador = "false";
        grdMovimientosCobros.Columnas.Add(ColIdDepositosEncabezadoFactura);

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
        ColFechaPago.Encabezado = "Fecha asociado";
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
        ColEliminarMovimiento.Imagen = "off.png";
        ColEliminarMovimiento.Estilo = "divImagenConsultar imgEliminarMovimiento";
        ColEliminarMovimiento.Buscador = "false";
        ColEliminarMovimiento.Ordenable = "false";
        ColEliminarMovimiento.Ancho = "25";
        grdMovimientosCobros.Columnas.Add(ColEliminarMovimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobros", grdMovimientosCobros.GeneraGrid(), true);

        //GridIngresosNoDepositados
        CJQGrid GridIngresosNoDepositados = new CJQGrid();
        GridIngresosNoDepositados.NombreTabla = "grdIngresosNoDepositados";
        GridIngresosNoDepositados.CampoIdentificador = "IdIngresosNoDepositados";
        GridIngresosNoDepositados.ColumnaOrdenacion = "Sucursal,Folio";
        GridIngresosNoDepositados.Metodo = "ObtenerIngresosNoDepositados";
        GridIngresosNoDepositados.TituloTabla = "Ingresos no depositados";
        GridIngresosNoDepositados.GenerarGridCargaInicial = false;
        GridIngresosNoDepositados.GenerarFuncionFiltro = false;
        GridIngresosNoDepositados.GenerarFuncionTerminado = false;
        GridIngresosNoDepositados.Altura = 120;
        GridIngresosNoDepositados.Ancho = 770;
        GridIngresosNoDepositados.NumeroRegistros = 15;
        GridIngresosNoDepositados.RangoNumeroRegistros = "15,30,60";

        //IdIngresosNoDepositados
        CJQColumn ColIdIngresosNoDepositados = new CJQColumn();
        ColIdIngresosNoDepositados.Nombre = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositados.Oculto = "true";
        ColIdIngresosNoDepositados.Encabezado = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositados.Buscador = "false";
        GridIngresosNoDepositados.Columnas.Add(ColIdIngresosNoDepositados);

        //Sucursal
        CJQColumn ColSucursalIND = new CJQColumn();
        ColSucursalIND.Nombre = "Sucursal";
        ColSucursalIND.Encabezado = "Sucursal";
        ColSucursalIND.Ancho = "10";
        ColSucursalIND.Buscador = "true";
        ColSucursalIND.Alineacion = "left";
        ColSucursalIND.Oculto = "true";
        GridIngresosNoDepositados.Columnas.Add(ColSucursalIND);

        //Folio
        CJQColumn ColFolioIND = new CJQColumn();
        ColFolioIND.Nombre = "Folio";
        ColFolioIND.Encabezado = "Folio";
        ColFolioIND.Ancho = "80";
        ColFolioIND.Buscador = "true";
        ColFolioIND.Alineacion = "left";
        GridIngresosNoDepositados.Columnas.Add(ColFolioIND);

        //Importe
        CJQColumn ColImporteIND = new CJQColumn();
        ColImporteIND.Nombre = "Importe";
        ColImporteIND.Encabezado = "Importe";
        ColImporteIND.Buscador = "false";
        ColImporteIND.Formato = "FormatoMoneda";
        ColImporteIND.Alineacion = "right";
        ColImporteIND.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColImporteIND);

        //Cuenta
        CJQColumn ColCuentaIND = new CJQColumn();
        ColCuentaIND.Nombre = "CuentaBancaria";
        ColCuentaIND.Encabezado = "Cuenta bancaria";
        ColCuentaIND.Buscador = "false";
        ColCuentaIND.Alineacion = "left";
        ColCuentaIND.Ancho = "100";
        GridIngresosNoDepositados.Columnas.Add(ColCuentaIND);

        //TipoMoneda
        CJQColumn ColTipoMonedaIND = new CJQColumn();
        ColTipoMonedaIND.Nombre = "TipoMoneda";
        ColTipoMonedaIND.Encabezado = "Tipo de moneda";
        ColTipoMonedaIND.Buscador = "false";
        ColTipoMonedaIND.Alineacion = "left";
        ColTipoMonedaIND.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColTipoMonedaIND);

        //FechaEmision
        CJQColumn ColFechaEmisionIND = new CJQColumn();
        ColFechaEmisionIND.Nombre = "FechaEmision";
        ColFechaEmisionIND.Encabezado = "Fecha de captura";
        ColFechaEmisionIND.Buscador = "false";
        ColFechaEmisionIND.Alineacion = "left";
        ColFechaEmisionIND.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColFechaEmisionIND);

        //FechaDeposito
        CJQColumn ColFechaDepositoIND = new CJQColumn();
        ColFechaDepositoIND.Nombre = "FechaDeposito";
        ColFechaDepositoIND.Encabezado = "Fecha acreditado";
        ColFechaDepositoIND.Buscador = "false";
        ColFechaDepositoIND.Alineacion = "left";
        ColFechaDepositoIND.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColFechaDepositoIND);

        //Asociado
        CJQColumn ColAsociadoIND = new CJQColumn();
        ColAsociadoIND.Nombre = "Asociado";
        ColAsociadoIND.Encabezado = "Asociado";
        ColAsociadoIND.Buscador = "false";
        ColAsociadoIND.Alineacion = "left";
        ColAsociadoIND.Ancho = "40";
        GridIngresosNoDepositados.Columnas.Add(ColAsociadoIND);

        //Depositado
        CJQColumn ColDepositadoIND = new CJQColumn();
        ColDepositadoIND.Nombre = "Depositado";
        ColDepositadoIND.Encabezado = "Depositado";
        ColDepositadoIND.Buscador = "false";
        ColDepositadoIND.Alineacion = "left";
        ColDepositadoIND.Ancho = "40";
        GridIngresosNoDepositados.Columnas.Add(ColDepositadoIND);

        //RazonSocial
        CJQColumn ColRazonSocialIND = new CJQColumn();
        ColRazonSocialIND.Nombre = "RazonSocial";
        ColRazonSocialIND.Encabezado = "Razón social";
        ColRazonSocialIND.Buscador = "false";
        ColRazonSocialIND.Alineacion = "left";
        ColRazonSocialIND.Ancho = "100";
        GridIngresosNoDepositados.Columnas.Add(ColRazonSocialIND);

        //Elegir monto
        CJQColumn ColElegirMonto = new CJQColumn();
        ColElegirMonto.Nombre = "Elegir";
        ColElegirMonto.Encabezado = "Elegir";
        ColElegirMonto.Buscador = "false";
        ColElegirMonto.Alineacion = "left";
        ColElegirMonto.Funcion = "ElegirMonto";
        ColElegirMonto.Etiquetado = "CheckBox";
        ColElegirMonto.Id = "chkElegirMonto";
        ColElegirMonto.Estilo = "chkElegirMonto";
        ColElegirMonto.Ancho = "25";
        GridIngresosNoDepositados.Columnas.Add(ColElegirMonto);

        ClientScript.RegisterStartupScript(this.GetType(), "grdIngresosNoDepositados", GridIngresosNoDepositados.GeneraGrid(), true);


        //GridIngresosNoDepositadosConsultar
        CJQGrid GridIngresosNoDepositadosConsultar = new CJQGrid();
        GridIngresosNoDepositadosConsultar.NombreTabla = "grdIngresosNoDepositadosConsultar";
        GridIngresosNoDepositadosConsultar.CampoIdentificador = "IdIngresosNoDepositados";
        GridIngresosNoDepositadosConsultar.ColumnaOrdenacion = "Sucursal,Folio";
        GridIngresosNoDepositadosConsultar.Metodo = "ObtenerIngresosNoDepositadosConsultar";
        GridIngresosNoDepositadosConsultar.TituloTabla = "Ingresos no depositados";
        GridIngresosNoDepositadosConsultar.GenerarGridCargaInicial = false;
        GridIngresosNoDepositadosConsultar.GenerarFuncionFiltro = false;
        GridIngresosNoDepositadosConsultar.GenerarFuncionTerminado = false;
        GridIngresosNoDepositadosConsultar.Altura = 120;
        GridIngresosNoDepositadosConsultar.Ancho = 770;
        GridIngresosNoDepositadosConsultar.NumeroRegistros = 15;
        GridIngresosNoDepositadosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdIngresosNoDepositados
        CJQColumn ColIdIngresosNoDepositadosConsultar = new CJQColumn();
        ColIdIngresosNoDepositadosConsultar.Nombre = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositadosConsultar.Oculto = "true";
        ColIdIngresosNoDepositadosConsultar.Encabezado = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositadosConsultar.Buscador = "false";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColIdIngresosNoDepositadosConsultar);

        //Sucursal
        CJQColumn ColSucursalINDConsultar = new CJQColumn();
        ColSucursalINDConsultar.Nombre = "Sucursal";
        ColSucursalINDConsultar.Encabezado = "Sucursal";
        ColSucursalINDConsultar.Ancho = "10";
        ColSucursalINDConsultar.Buscador = "false";
        ColSucursalINDConsultar.Alineacion = "left";
        ColSucursalINDConsultar.Oculto = "true";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColSucursalINDConsultar);

        //Folio
        CJQColumn ColFolioINDConsultar = new CJQColumn();
        ColFolioINDConsultar.Nombre = "Folio";
        ColFolioINDConsultar.Encabezado = "Folio";
        ColFolioINDConsultar.Ancho = "80";
        ColFolioINDConsultar.Buscador = "false";
        ColFolioINDConsultar.Alineacion = "left";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColFolioINDConsultar);

        //Importe
        CJQColumn ColImporteINDConsultar = new CJQColumn();
        ColImporteINDConsultar.Nombre = "Importe";
        ColImporteINDConsultar.Encabezado = "Importe";
        ColImporteINDConsultar.Buscador = "false";
        ColImporteINDConsultar.Formato = "FormatoMoneda";
        ColImporteINDConsultar.Alineacion = "right";
        ColImporteINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColImporteINDConsultar);

        //Cuenta
        CJQColumn ColCuentaINDConsultar = new CJQColumn();
        ColCuentaINDConsultar.Nombre = "CuentaBancaria";
        ColCuentaINDConsultar.Encabezado = "Cuenta bancaria";
        ColCuentaINDConsultar.Buscador = "false";
        ColCuentaINDConsultar.Alineacion = "left";
        ColCuentaINDConsultar.Ancho = "100";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColCuentaINDConsultar);

        //TipoMoneda
        CJQColumn ColTipoMonedaINDConsultar = new CJQColumn();
        ColTipoMonedaINDConsultar.Nombre = "TipoMoneda";
        ColTipoMonedaINDConsultar.Encabezado = "Tipo de moneda";
        ColTipoMonedaINDConsultar.Buscador = "false";
        ColTipoMonedaINDConsultar.Alineacion = "left";
        ColTipoMonedaINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColTipoMonedaINDConsultar);

        //FechaEmision
        CJQColumn ColFechaEmisionINDConsultar = new CJQColumn();
        ColFechaEmisionINDConsultar.Nombre = "FechaEmision";
        ColFechaEmisionINDConsultar.Encabezado = "Fecha de captura";
        ColFechaEmisionINDConsultar.Buscador = "false";
        ColFechaEmisionINDConsultar.Alineacion = "left";
        ColFechaEmisionINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColFechaEmisionINDConsultar);

        //FechaDeposito
        CJQColumn ColFechaDepositoINDConsultar = new CJQColumn();
        ColFechaDepositoINDConsultar.Nombre = "FechaDeposito";
        ColFechaDepositoINDConsultar.Encabezado = "Fecha acreditado";
        ColFechaDepositoINDConsultar.Buscador = "false";
        ColFechaDepositoINDConsultar.Alineacion = "left";
        ColFechaDepositoINDConsultar.Ancho = "80";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColFechaDepositoINDConsultar);

        //Asociado
        CJQColumn ColAsociadoINDConsultar = new CJQColumn();
        ColAsociadoINDConsultar.Nombre = "Asociado";
        ColAsociadoINDConsultar.Encabezado = "Asociado";
        ColAsociadoINDConsultar.Buscador = "false";
        ColAsociadoINDConsultar.Alineacion = "left";
        ColAsociadoINDConsultar.Ancho = "40";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColAsociadoINDConsultar);

        //Depositado
        CJQColumn ColDepositadoINDConsultar = new CJQColumn();
        ColDepositadoINDConsultar.Nombre = "Depositado";
        ColDepositadoINDConsultar.Encabezado = "Depositado";
        ColDepositadoINDConsultar.Buscador = "false";
        ColDepositadoINDConsultar.Alineacion = "left";
        ColDepositadoINDConsultar.Ancho = "40";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColDepositadoINDConsultar);

        //RazonSocial
        CJQColumn ColRazonSocialINDConsultar = new CJQColumn();
        ColRazonSocialINDConsultar.Nombre = "RazonSocial";
        ColRazonSocialINDConsultar.Encabezado = "Razón social";
        ColRazonSocialINDConsultar.Buscador = "false";
        ColRazonSocialINDConsultar.Alineacion = "left";
        ColRazonSocialINDConsultar.Ancho = "100";
        GridIngresosNoDepositadosConsultar.Columnas.Add(ColRazonSocialINDConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdIngresosNoDepositadosConsultar", GridIngresosNoDepositadosConsultar.GeneraGrid(), true);


        //GridIngresosNoDepositadosEditar
        CJQGrid GridIngresosNoDepositadosEditar = new CJQGrid();
        GridIngresosNoDepositadosEditar.NombreTabla = "grdIngresosNoDepositadosEditar";
        GridIngresosNoDepositadosEditar.CampoIdentificador = "IdIngresosNoDepositados";
        GridIngresosNoDepositadosEditar.ColumnaOrdenacion = "Sucursal,Folio";
        GridIngresosNoDepositadosEditar.Metodo = "ObtenerIngresosNoDepositadosEditar";
        GridIngresosNoDepositadosEditar.TituloTabla = "Ingresos no depositados";
        GridIngresosNoDepositadosEditar.GenerarGridCargaInicial = false;
        GridIngresosNoDepositadosEditar.GenerarFuncionFiltro = false;
        GridIngresosNoDepositadosEditar.GenerarFuncionTerminado = false;
        GridIngresosNoDepositadosEditar.Altura = 120;
        GridIngresosNoDepositadosEditar.Ancho = 770;
        GridIngresosNoDepositadosEditar.NumeroRegistros = 15;
        GridIngresosNoDepositadosEditar.RangoNumeroRegistros = "15,30,60";

        //IdIngresosNoDepositados
        CJQColumn ColIdIngresosNoDepositadosEditar = new CJQColumn();
        ColIdIngresosNoDepositadosEditar.Nombre = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositadosEditar.Oculto = "true";
        ColIdIngresosNoDepositadosEditar.Encabezado = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositadosEditar.Buscador = "false";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColIdIngresosNoDepositadosEditar);

        //Sucursal
        CJQColumn ColSucursalINDEditar = new CJQColumn();
        ColSucursalINDEditar.Nombre = "Sucursal";
        ColSucursalINDEditar.Encabezado = "Sucursal";
        ColSucursalINDEditar.Ancho = "10";
        ColSucursalINDEditar.Buscador = "false";
        ColSucursalINDEditar.Alineacion = "left";
        ColSucursalINDEditar.Oculto = "true";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColSucursalINDEditar);

        //Folio
        CJQColumn ColFolioINDEditar = new CJQColumn();
        ColFolioINDEditar.Nombre = "Folio";
        ColFolioINDEditar.Encabezado = "Folio";
        ColFolioINDEditar.Ancho = "80";
        ColFolioINDEditar.Buscador = "false";
        ColFolioINDEditar.Alineacion = "left";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColFolioINDEditar);

        //Importe
        CJQColumn ColImporteINDEditar = new CJQColumn();
        ColImporteINDEditar.Nombre = "Importe";
        ColImporteINDEditar.Encabezado = "Importe";
        ColImporteINDEditar.Buscador = "false";
        ColImporteINDEditar.Formato = "FormatoMoneda";
        ColImporteINDEditar.Alineacion = "right";
        ColImporteINDEditar.Ancho = "80";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColImporteINDEditar);

        //Cuenta
        CJQColumn ColCuentaINDEditar = new CJQColumn();
        ColCuentaINDEditar.Nombre = "CuentaBancaria";
        ColCuentaINDEditar.Encabezado = "Cuenta bancaria";
        ColCuentaINDEditar.Buscador = "false";
        ColCuentaINDEditar.Alineacion = "left";
        ColCuentaINDEditar.Ancho = "100";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColCuentaINDEditar);

        //TipoMoneda
        CJQColumn ColTipoMonedaINDEditar = new CJQColumn();
        ColTipoMonedaINDEditar.Nombre = "TipoMoneda";
        ColTipoMonedaINDEditar.Encabezado = "Tipo de moneda";
        ColTipoMonedaINDEditar.Buscador = "false";
        ColTipoMonedaINDEditar.Alineacion = "left";
        ColTipoMonedaINDEditar.Ancho = "80";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColTipoMonedaINDEditar);

        //FechaEmision
        CJQColumn ColFechaEmisionINDEditar = new CJQColumn();
        ColFechaEmisionINDEditar.Nombre = "FechaEmision";
        ColFechaEmisionINDEditar.Encabezado = "Fecha de captura";
        ColFechaEmisionINDEditar.Buscador = "false";
        ColFechaEmisionINDEditar.Alineacion = "left";
        ColFechaEmisionINDEditar.Ancho = "80";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColFechaEmisionINDEditar);

        //FechaDeposito
        CJQColumn ColFechaDepositoINDEditar = new CJQColumn();
        ColFechaDepositoINDEditar.Nombre = "FechaDeposito";
        ColFechaDepositoINDEditar.Encabezado = "Fecha de depósito";
        ColFechaDepositoINDEditar.Buscador = "false";
        ColFechaDepositoINDEditar.Alineacion = "left";
        ColFechaDepositoINDEditar.Ancho = "80";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColFechaDepositoINDEditar);

        //Asociado
        CJQColumn ColAsociadoINDEditar = new CJQColumn();
        ColAsociadoINDEditar.Nombre = "Asociado";
        ColAsociadoINDEditar.Encabezado = "Asociado";
        ColAsociadoINDEditar.Buscador = "false";
        ColAsociadoINDEditar.Alineacion = "left";
        ColAsociadoINDEditar.Ancho = "40";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColAsociadoINDEditar);

        //Depositado
        CJQColumn ColDepositadoINDEditar = new CJQColumn();
        ColDepositadoINDEditar.Nombre = "Depositado";
        ColDepositadoINDEditar.Encabezado = "Depositado";
        ColDepositadoINDEditar.Buscador = "false";
        ColDepositadoINDEditar.Alineacion = "left";
        ColDepositadoINDEditar.Ancho = "40";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColDepositadoINDEditar);

        //RazonSocial
        CJQColumn ColRazonSocialINDEditar = new CJQColumn();
        ColRazonSocialINDEditar.Nombre = "RazonSocial";
        ColRazonSocialINDEditar.Encabezado = "Razón social";
        ColRazonSocialINDEditar.Buscador = "false";
        ColRazonSocialINDEditar.Alineacion = "left";
        ColRazonSocialINDEditar.Ancho = "100";
        GridIngresosNoDepositadosEditar.Columnas.Add(ColRazonSocialINDEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdIngresosNoDepositadosEditar", GridIngresosNoDepositadosEditar.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDepositos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDepositos", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCuentaBancaria(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCuentaBancaria)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentaBancariaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCuentaBancaria", SqlDbType.VarChar, 250).Value = Convert.ToString(pCuentaBancaria);
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerFacturas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerie, int pIdCliente)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerie);
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdDepositos)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDepositosEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdDepositos", SqlDbType.Int).Value = pIdDepositos;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerIngresosNoDepositados(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCuentaBancaria)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosADepositar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentaBancaria", SqlDbType.Int).Value = pIdCuentaBancaria;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerIngresosNoDepositadosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdDepositos)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosADepositarConsultar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdDepositos", SqlDbType.Int).Value = pIdDepositos;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerIngresosNoDepositadosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdDepositos)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosADepositarConsultar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdDepositos", SqlDbType.Int).Value = pIdDepositos;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarDepositos(string pDepositos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonDepositos = new CJson();
        jsonDepositos.StoredProcedure.CommandText = "sp_Depositos_Consultar_FiltroPorDepositos";
        jsonDepositos.StoredProcedure.Parameters.AddWithValue("@pDepositos", pDepositos);
        return jsonDepositos.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarCuentaBancaria(string pCuentaBancaria)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCuentaBancaria = new CJson();
        jsonCuentaBancaria.StoredProcedure.CommandText = "sp_CuentaBancaria_Consultar_FiltroPorCuentaBancaria";
        jsonCuentaBancaria.StoredProcedure.Parameters.AddWithValue("@pCuentaBancaria", pCuentaBancaria);
        return jsonCuentaBancaria.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarDepositos(Dictionary<string, object> pDepositos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            string cadena = "";

            CDepositos Depositos = new CDepositos();
            Depositos.IdCuentaBancaria = Convert.ToInt32(pDepositos["IdCuentaBancaria"]);
            Depositos.IdCliente = Convert.ToInt32(pDepositos["IdCliente"]);
            Depositos.IdMetodoPago = Convert.ToInt32(pDepositos["IdMetodoPago"]);
            Depositos.FechaEmision = Convert.ToDateTime(pDepositos["Fecha"]);
            Depositos.Importe = Convert.ToDecimal(pDepositos["Importe"]);
            Depositos.Referencia = Convert.ToString(pDepositos["Referencia"]);
            Depositos.ConceptoGeneral = Convert.ToString(pDepositos["ConceptoGeneral"]);
            Depositos.FechaAplicacion = Convert.ToDateTime(pDepositos["FechaAplicacion"]);
            Depositos.Conciliado = Convert.ToBoolean(pDepositos["Conciliado"]);
            Depositos.Asociado = Convert.ToBoolean(pDepositos["Asociado"]);
            Depositos.IdTipoMoneda = Convert.ToInt32(pDepositos["IdTipoMoneda"]);
            Depositos.TipoCambio = Convert.ToDecimal(pDepositos["TipoCambio"]);
            Depositos.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            foreach (string oIds in (Array)pDepositos["IdsIngresosNoDepositados"])
            {
                cadena = cadena + Convert.ToString(oIds) + ",";
            }
            cadena = cadena.Substring(0, cadena.Length - 1);

            Depositos.IdsIngresosNoDepositados = Convert.ToString(cadena);
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarDepositos(Depositos, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Depositos.AgregarDepositos(ConexionBaseDatos);

                CDepositosSucursal DepositosSucursal = new CDepositosSucursal();
                DepositosSucursal.IdDepositos = Depositos.IdDepositos;
                DepositosSucursal.IdSucursal = Usuario.IdSucursalActual;
                DepositosSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                DepositosSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                DepositosSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Depositos.IdDepositos;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("Depositos", ConexionBaseDatos);

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
    public static string AgregarDepositosEdicion(Dictionary<string, object> pDepositos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CDepositos Depositos = new CDepositos();
            Depositos.IdCuentaBancaria = Convert.ToInt32(pDepositos["IdCuentaBancaria"]);
            Depositos.IdCliente = Convert.ToInt32(pDepositos["IdCliente"]);
            Depositos.IdMetodoPago = Convert.ToInt32(pDepositos["IdMetodoPago"]);
            Depositos.FechaEmision = Convert.ToDateTime(pDepositos["Fecha"]);
            Depositos.Importe = Convert.ToDecimal(pDepositos["Importe"]);
            Depositos.Referencia = Convert.ToString(pDepositos["Referencia"]);
            Depositos.ConceptoGeneral = Convert.ToString(pDepositos["ConceptoGeneral"]);
            Depositos.FechaAplicacion = Convert.ToDateTime(pDepositos["FechaAplicacion"]);
            Depositos.Conciliado = Convert.ToBoolean(pDepositos["Conciliado"]);
            Depositos.Asociado = Convert.ToBoolean(pDepositos["Asociado"]);
            Depositos.IdTipoMoneda = Convert.ToInt32(pDepositos["IdTipoMoneda"]);
            Depositos.TipoCambio = Convert.ToDecimal(pDepositos["TipoCambio"]);
            Depositos.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarDepositos(Depositos, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Depositos.AgregarDepositos(ConexionBaseDatos);

                CDepositosSucursal DepositosSucursal = new CDepositosSucursal();
                DepositosSucursal.IdDepositos = Depositos.IdDepositos;
                DepositosSucursal.IdSucursal = Usuario.IdSucursalActual;
                DepositosSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                DepositosSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                DepositosSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Depositos.IdDepositos;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("Depositos", ConexionBaseDatos);

                Depositos.LlenaObjeto(Depositos.IdDepositos, ConexionBaseDatos);
                oRespuesta.Add("IdDepositos", Depositos.IdDepositos);
                oRespuesta.Add("Folio", Depositos.Folio);
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
    public static string ObtenerFormaDepositos(int pIdDepositos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDepositos = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDepositos" }, ConexionBaseDatos) == "")
        {
            puedeEditarDepositos = 1;
        }
        oPermisos.Add("puedeEditarDepositos", puedeEditarDepositos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CDepositos.ObtenerDepositos(Modelo, pIdDepositos, ConexionBaseDatos);
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

    [WebMethod]
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> Depositos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDepositos = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDepositos" }, ConexionBaseDatos) == "")
        {
            puedeEditarDepositos = 1;
        }
        oPermisos.Add("puedeEditarDepositos", puedeEditarDepositos);

        if (respuesta == "Conexion Establecida")
        {
            CDepositos DepositosCliente = new CDepositos();
            DepositosCliente.LlenaObjeto(Convert.ToInt32(Depositos["pIdDepositos"]), ConexionBaseDatos);
            DepositosCliente.IdCliente = Convert.ToInt32(Depositos["pIdCliente"]);
            DepositosCliente.Editar(ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CDepositos.ObtenerDepositos(Modelo, Convert.ToInt32(Depositos["pIdDepositos"]), ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarDepositos()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMetodoPago MetodoPago = new CMetodoPago();
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(ConexionBaseDatos, 3));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add(new JProperty("Fecha", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
            Modelo.Add(new JProperty("FechaAplicacion", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
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
    public static string ObtenerDatosCuentaBancaria(int pIdCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCuentaBancaria.ObtenerCuentaBancariaDOF(Modelo, pIdCuentaBancaria, ConexionBaseDatos);
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

    [WebMethod]
    public static string ObtenerFormaEditarDepositos(int IdDepositos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarDepositos = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarDepositos" }, ConexionBaseDatos) == "")
        {
            puedeEditarDepositos = 1;
        }
        oPermisos.Add("puedeEditarDepositos", puedeEditarDepositos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CDepositos.ObtenerDepositos(Modelo, IdDepositos, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(Convert.ToInt32(Modelo["IdMetodoPago"].ToString()), ConexionBaseDatos));
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
    public static string ObtenerFormaFiltroDepositos()
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
    public static string EditarDepositos(Dictionary<string, object> pDepositos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CDepositos Depositos = new CDepositos();
        Depositos.LlenaObjeto(Convert.ToInt32(pDepositos["IdDepositos"]), ConexionBaseDatos);
        Depositos.IdDepositos = Convert.ToInt32(pDepositos["IdDepositos"]);
        Depositos.IdCliente = Convert.ToInt32(pDepositos["IdCliente"]);
        Depositos.Referencia = Convert.ToString(pDepositos["Referencia"]);
        Depositos.ConceptoGeneral = Convert.ToString(pDepositos["ConceptoGeneral"]);
        Depositos.Conciliado = Convert.ToBoolean(pDepositos["Conciliado"]);
        Depositos.Asociado = Convert.ToBoolean(pDepositos["Asociado"]);

        string validacion = ValidarDepositos(Depositos, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Depositos.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarMontos(Dictionary<string, object> pDepositos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        CDepositosEncabezadoFactura DepositosEncabezadoFactura = new CDepositosEncabezadoFactura();
        DepositosEncabezadoFactura.IdDepositos = Convert.ToInt32(pDepositos["IdDepositos"]);
        DepositosEncabezadoFactura.IdEncabezadoFactura = Convert.ToInt32(pDepositos["IdEncabezadoFactura"]);
        DepositosEncabezadoFactura.Monto = Convert.ToDecimal(pDepositos["Monto"]);
        DepositosEncabezadoFactura.FechaPago = Convert.ToDateTime(DateTime.Now);
        DepositosEncabezadoFactura.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        DepositosEncabezadoFactura.Nota = "pago de la factura";
        string validacion = ValidarMontos(DepositosEncabezadoFactura, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            DepositosEncabezadoFactura.AgregarDepositosEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("Monto", Convert.ToDecimal(pDepositos["Monto"]));
            oRespuesta.Add("rowid", Convert.ToDecimal(pDepositos["rowid"]));
            oRespuesta.Add("AbonosDepositos", DepositosEncabezadoFactura.TotalAbonosDepositos(Convert.ToInt32(pDepositos["IdDepositos"]), ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerMontoIngresoNoDepositado(Dictionary<string, object> pIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
        IngresosNoDepositados.LlenaObjeto(Convert.ToInt32(pIngresosNoDepositados["pIdIngresoNoDepositado"]), ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        oRespuesta.Add("ImporteIngresoNoDepositado", IngresosNoDepositados.Importe);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EliminarDepositosEncabezadoFactura(Dictionary<string, object> pDepositosEncabezadoFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        CDepositosEncabezadoFactura DepositosEncabezadoFactura = new CDepositosEncabezadoFactura();
        DepositosEncabezadoFactura.LlenaObjeto(Convert.ToInt32(pDepositosEncabezadoFactura["pIdDepositosEncabezadoFactura"]), ConexionBaseDatos);
        DepositosEncabezadoFactura.IdDepositosEncabezadoFactura = Convert.ToInt32(pDepositosEncabezadoFactura["pIdDepositosEncabezadoFactura"]);
        DepositosEncabezadoFactura.Baja = true;
        DepositosEncabezadoFactura.EliminarDepositosEncabezadoFactura(ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        oRespuesta.Add("AbonosDepositos", DepositosEncabezadoFactura.TotalAbonosDepositos(DepositosEncabezadoFactura.IdDepositos, ConexionBaseDatos));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarDepositosCliente(Dictionary<string, object> pDepositos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CDepositos Depositos = new CDepositos();
        Depositos.LlenaObjeto(Convert.ToInt32(pDepositos["IdDepositos"]), ConexionBaseDatos);
        Depositos.IdDepositos = Convert.ToInt32(pDepositos["IdDepositos"]);
        Depositos.IdCliente = Convert.ToInt32(pDepositos["IdCliente"]);
        string validacion = ValidarDepositos(Depositos, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Depositos.Editar(ConexionBaseDatos);
            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdDepositos, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDepositos Depositos = new CDepositos();
            Depositos.IdDepositos = pIdDepositos;
            Depositos.Baja = pBaja;

            if (pBaja == true)
            {
                if (Depositos.ExisteDepositosMovimientos(Convert.ToInt32(pIdDepositos), ConexionBaseDatos) == 0)
                {
                    Depositos.Eliminar(ConexionBaseDatos);
                    Depositos.LlenaObjeto(pIdDepositos, ConexionBaseDatos);
                    CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
                    CuentaBancaria.LlenaObjeto(Depositos.IdCuentaBancaria, ConexionBaseDatos);
                    CuentaBancaria.Saldo = CuentaBancaria.Saldo - Depositos.Importe;
                    CuentaBancaria.Editar(ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", "Baja correctamente"));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", "No se puede dar de baja este cobro, porque existen documentos asociados."));
                }
            }
            else
            {
                Depositos.Eliminar(ConexionBaseDatos);
                Depositos.LlenaObjeto(pIdDepositos, ConexionBaseDatos);
                CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
                CuentaBancaria.LlenaObjeto(Depositos.IdCuentaBancaria, ConexionBaseDatos);
                CuentaBancaria.Saldo = CuentaBancaria.Saldo + Depositos.Importe;
                CuentaBancaria.Editar(ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "Activada correctamente"));
            }
            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string ObtenerTipoCambioDiarioOficial(int pIdTipoMonedaDestino, string pFecha)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            oRespuesta.Add("TipoCambioDiarioOficial", CTipoCambioDiarioOficial.ObtenerTipoCambio(pIdTipoMonedaDestino, Convert.ToDateTime(pFecha), ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Descripcion", "Se obtuvó el tipo de cambio del diario oficial con éxito."));

            //Cerrar Conexion
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", respuesta));
            return oRespuesta.ToString();
        }
    }

    //Busquedas
    [WebMethod]
    public static string BuscarRazonSocialCliente(string pRazonSocial)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    //Validaciones
    private static string ValidarDepositos(CDepositos pDepositos, CConexion pConexion)
    {
        string errores = "";

        if (pDepositos.IdCuentaBancaria == 0)
        { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

        if (pDepositos.IdMetodoPago == 0)
        { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

        if (pDepositos.Importe == 0)
        { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

        if (pDepositos.Referencia == "")
        { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

        if (pDepositos.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarMontos(CDepositosEncabezadoFactura Depositos, CConexion pConexion)
    {
        string errores = "";

        if (Depositos.IdEncabezadoFactura == 0)
        { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

        if (Depositos.IdDepositos == 0)
        { errores = errores + "<span>*</span> No hay cuenta por cobrar seleccionada, favor de elegir alguna.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}