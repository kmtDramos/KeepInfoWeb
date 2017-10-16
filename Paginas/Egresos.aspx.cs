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

public partial class Egresos : System.Web.UI.Page
{
    public static int puedeAgregarEgresos = 0;
    public static int puedeEditarEgresos = 0;
    public static int puedeEliminarEgresos = 0;
    public static int puedeEliminarPagoEgresos = 0;

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

        puedeAgregarEgresos = Usuario.TienePermisos(new string[] { "puedeAgregarEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarEgresos = Usuario.TienePermisos(new string[] { "puedeEditarEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarEgresos = Usuario.TienePermisos(new string[] { "puedeEliminarEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarPagoEgresos = Usuario.TienePermisos(new string[] { "puedeEliminarPagoEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridEgresos
        CJQGrid GridEgresos = new CJQGrid();
        GridEgresos.NombreTabla = "grdEgresos";
        GridEgresos.CampoIdentificador = "IdEgresos";
        GridEgresos.ColumnaOrdenacion = "Sucursal,Folio";
		GridEgresos.TipoOrdenacion = "DESC";
        GridEgresos.Metodo = "ObtenerEgresos";
        GridEgresos.TituloTabla = "Pagos";
        GridEgresos.GenerarFuncionFiltro = false;

        //IdEgresos
        CJQColumn ColIdEgresos = new CJQColumn();
        ColIdEgresos.Nombre = "IdEgresos";
        ColIdEgresos.Oculto = "true";
        ColIdEgresos.Encabezado = "IdEgresos";
        ColIdEgresos.Buscador = "false";
        GridEgresos.Columnas.Add(ColIdEgresos);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "false";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridEgresos.Columnas.Add(ColSucursal);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "80";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        GridEgresos.Columnas.Add(ColFolio);

        //Importe
        CJQColumn ColImporte = new CJQColumn();
        ColImporte.Nombre = "Importe";
        ColImporte.Encabezado = "Importe";
        ColImporte.Buscador = "true";
        ColImporte.Formato = "FormatoMoneda";
        ColImporte.Alineacion = "right";
        ColImporte.Ancho = "80";
        GridEgresos.Columnas.Add(ColImporte);

        //Cuenta
        CJQColumn ColCuenta = new CJQColumn();
        ColCuenta.Nombre = "CuentaBancaria";
        ColCuenta.Encabezado = "Cuenta bancaria";
        ColCuenta.Buscador = "false";
        ColCuenta.Alineacion = "left";
        ColCuenta.Ancho = "100";
        GridEgresos.Columnas.Add(ColCuenta);

        //Banco
        CJQColumn ColNombreCuentaBancaria = new CJQColumn();
        ColNombreCuentaBancaria.Nombre = "NombreCuentaBancaria";
        ColNombreCuentaBancaria.Encabezado = "Banco";
        ColNombreCuentaBancaria.Buscador = "true";
        ColNombreCuentaBancaria.Alineacion = "left";
        ColNombreCuentaBancaria.Ancho = "100";
        GridEgresos.Columnas.Add(ColNombreCuentaBancaria);

        //Referencia
        CJQColumn ColReferencia = new CJQColumn();
        ColReferencia.Nombre = "Referencia";
        ColReferencia.Encabezado = "Referencia";
        ColReferencia.Buscador = "true";
        ColReferencia.Alineacion = "left";
        ColReferencia.Ancho = "100";
        GridEgresos.Columnas.Add(ColReferencia);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "80";
        GridEgresos.Columnas.Add(ColTipoMoneda);

        //FechaEmision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha de captura";
        ColFechaEmision.Buscador = "false";
        ColFechaEmision.Alineacion = "left";
        ColFechaEmision.Ancho = "80";
        GridEgresos.Columnas.Add(ColFechaEmision);

        //FechaAplicacion
        CJQColumn ColFechaAplicacion = new CJQColumn();
        ColFechaAplicacion.Nombre = "FechaAplicacion";
        ColFechaAplicacion.Encabezado = "Fecha de aplicación";
        ColFechaAplicacion.Buscador = "false";
        ColFechaAplicacion.Alineacion = "left";
        ColFechaAplicacion.Ancho = "80";
        GridEgresos.Columnas.Add(ColFechaAplicacion);

        //Asociado
        CJQColumn ColAsociado = new CJQColumn();
        ColAsociado.Nombre = "Asociado";
        ColAsociado.Encabezado = "Asociado";
        ColAsociado.Buscador = "false";
        ColAsociado.Alineacion = "left";
        ColAsociado.Ancho = "40";
        GridEgresos.Columnas.Add(ColAsociado);

        //Conciliado
        CJQColumn ColConciliado = new CJQColumn();
        ColConciliado.Nombre = "Conciliado";
        ColConciliado.Encabezado = "Conciliado";
        ColConciliado.Buscador = "false";
        ColConciliado.Alineacion = "left";
        ColConciliado.Ancho = "40";
        GridEgresos.Columnas.Add(ColConciliado);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "150";
        GridEgresos.Columnas.Add(ColRazonSocial);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        //ColBaja.Oculto = puedeEliminarEgresos == 1 ? "false" : "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarEgresos == 1 ? "A/I" : "A/IDeshabilitado";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridEgresos.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarEgresos";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridEgresos.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdEgresos", GridEgresos.GeneraGrid(), true);

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
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "Fecha";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "80";
        GridFacturas.Columnas.Add(ColFecha);

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
        grdMovimientosCobros.CampoIdentificador = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobros.ColumnaOrdenacion = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobros.TipoOrdenacion = "DESC";
        grdMovimientosCobros.Metodo = "ObtenerMovimientosCobros";
        grdMovimientosCobros.TituloTabla = "Documentos asociados";
        grdMovimientosCobros.GenerarFuncionFiltro = false;
        grdMovimientosCobros.GenerarFuncionTerminado = false;
        grdMovimientosCobros.Altura = 120;
        grdMovimientosCobros.NumeroRegistros = 15;
        grdMovimientosCobros.RangoNumeroRegistros = "15,30,60";

        //IdEgresosEncabezadoFacturaProveedor
        CJQColumn ColIdEgresosEncabezadoFacturaProveedor = new CJQColumn();
        ColIdEgresosEncabezadoFacturaProveedor.Nombre = "IdEgresosEncabezadoFacturaProveedor";
        ColIdEgresosEncabezadoFacturaProveedor.Oculto = "true";
        ColIdEgresosEncabezadoFacturaProveedor.Encabezado = "IdEgresosEncabezadoFacturaProveedor";
        ColIdEgresosEncabezadoFacturaProveedor.Buscador = "false";
        grdMovimientosCobros.Columnas.Add(ColIdEgresosEncabezadoFacturaProveedor);

        //NumeroFacturaCobro
        CJQColumn ColNumeroFacturaCobro = new CJQColumn();
        ColNumeroFacturaCobro.Nombre = "NumeroFactura";
        ColNumeroFacturaCobro.Encabezado = "Número de factura";
        ColNumeroFacturaCobro.Buscador = "false";
        ColNumeroFacturaCobro.Alineacion = "left";
        ColNumeroFacturaCobro.Ancho = "80";
        grdMovimientosCobros.Columnas.Add(ColNumeroFacturaCobro);

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
        ColEliminarMovimiento.Imagen = "eliminar.png";
        ColEliminarMovimiento.Estilo = "divImagenConsultar imgEliminarMovimiento";
        ColEliminarMovimiento.Buscador = "false";
        ColEliminarMovimiento.Ordenable = "false";
        ColEliminarMovimiento.Oculto = puedeEliminarPagoEgresos == 1 ? "false" : "true";
        ColEliminarMovimiento.Ancho = "25";
        grdMovimientosCobros.Columnas.Add(ColEliminarMovimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobros", grdMovimientosCobros.GeneraGrid(), true);

        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobrosConsultar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosConsultar.Metodo = "ObtenerMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.TituloTabla = "Documentos asociados";
        grdMovimientosCobrosConsultar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosConsultar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosConsultar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosConsultar.Altura = 120;
        grdMovimientosCobrosConsultar.Ancho = 770;
        grdMovimientosCobrosConsultar.NumeroRegistros = 15;
        grdMovimientosCobrosConsultar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaConsultar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdEgresosEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdEgresosEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaConsultar);

        //NumeroFacturaConsultar
        CJQColumn ColNumeroFacturaConsultar = new CJQColumn();
        ColNumeroFacturaConsultar.Nombre = "NumeroFactura";
        ColNumeroFacturaConsultar.Encabezado = "Número de factura";
        ColNumeroFacturaConsultar.Buscador = "false";
        ColNumeroFacturaConsultar.Alineacion = "left";
        ColNumeroFacturaConsultar.Ancho = "80";
        grdMovimientosCobrosConsultar.Columnas.Add(ColNumeroFacturaConsultar);

        //Fecha pago
        CJQColumn ColFechaPagoConsultar = new CJQColumn();
        ColFechaPagoConsultar.Nombre = "FechaPago";
        ColFechaPagoConsultar.Encabezado = "Fecha asociado";
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
        grdMovimientosCobrosEditar.CampoIdentificador = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobrosEditar.ColumnaOrdenacion = "IdEgresosEncabezadoFacturaProveedor";
        grdMovimientosCobrosEditar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosEditar.Metodo = "ObtenerMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.TituloTabla = "Documentos asociados";
        grdMovimientosCobrosEditar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosEditar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosEditar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosEditar.Altura = 120;
        grdMovimientosCobrosEditar.Ancho = 770;
        grdMovimientosCobrosEditar.NumeroRegistros = 15;
        grdMovimientosCobrosEditar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaEditar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Nombre = "IdEgresosEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Encabezado = "IdEgresosEncabezadoFacturaProveedor";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Buscador = "false";
        grdMovimientosCobrosEditar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaEditar);

        //NumeroFacturaEditar
        CJQColumn ColNumeroFacturaEditar = new CJQColumn();
        ColNumeroFacturaEditar.Nombre = "NumeroFactura";
        ColNumeroFacturaEditar.Encabezado = "Número de factura";
        ColNumeroFacturaEditar.Buscador = "false";
        ColNumeroFacturaEditar.Alineacion = "left";
        ColNumeroFacturaEditar.Ancho = "80";
        grdMovimientosCobrosEditar.Columnas.Add(ColNumeroFacturaEditar);

        //Fecha pago
        CJQColumn ColFechaPagoEditar = new CJQColumn();
        ColFechaPagoEditar.Nombre = "FechaPago";
        ColFechaPagoEditar.Encabezado = "Fecha asociado";
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
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerEgresos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCuentaBancaria, string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pReferencia, decimal pImporte)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEgresos", sqlCon);
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
        Stored.Parameters.Add("pCuentaBancaria", SqlDbType.VarChar, 255).Value = pCuentaBancaria;
        Stored.Parameters.Add("pReferencia", SqlDbType.VarChar, 255).Value = pReferencia;
        Stored.Parameters.Add("pImporte", SqlDbType.Decimal).Value = pImporte;

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
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEgresos)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEgresosEncabezadoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEgresos", SqlDbType.Int).Value = pIdEgresos;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEgresos)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEgresosEncabezadoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEgresos", SqlDbType.Int).Value = pIdEgresos;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdEgresos)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdEgresosEncabezadoFacturaProveedorConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdEgresos", SqlDbType.Int).Value = pIdEgresos;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string BuscarEgresos(string pEgresos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonEgresos = new CJson();
        jsonEgresos.StoredProcedure.CommandText = "sp_Egresos_Consultar_FiltroPorEgresos";
        jsonEgresos.StoredProcedure.Parameters.AddWithValue("@pEgresos", pEgresos);
        return jsonEgresos.ObtenerJsonString(ConexionBaseDatos);
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
    public static string AgregarEgresos(Dictionary<string, object> pEgresos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CEgresos Egresos = new CEgresos();
            Egresos.IdCuentaBancaria = Convert.ToInt32(pEgresos["IdCuentaBancaria"]);
            Egresos.IdProveedor = Convert.ToInt32(pEgresos["IdProveedor"]);
            Egresos.IdMetodoPago = Convert.ToInt32(pEgresos["IdMetodoPago"]);
            Egresos.FechaEmision = Convert.ToDateTime(pEgresos["Fecha"]);
            Egresos.Importe = Convert.ToDecimal(pEgresos["Importe"]);
            Egresos.Referencia = Convert.ToString(pEgresos["Referencia"]);
            Egresos.ConceptoGeneral = Convert.ToString(pEgresos["ConceptoGeneral"]);
            Egresos.FechaAplicacion = Convert.ToDateTime(pEgresos["FechaAplicacion"]);
            Egresos.Conciliado = Convert.ToBoolean(pEgresos["Conciliado"]);
            Egresos.Asociado = Convert.ToBoolean(pEgresos["Asociado"]);
            Egresos.TipoCambio = Convert.ToDecimal(pEgresos["TipoCambio"]);
            Egresos.IdTipoMoneda = Convert.ToInt32(pEgresos["IdTipoMoneda"]);
            Egresos.TipoCambioDOF = Convert.ToDecimal(pEgresos["TipoCambioDOF"]);
            Egresos.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

            if (Convert.ToBoolean(pEgresos["Conciliado"]))
            {
                Egresos.FechaConciliacion = Convert.ToDateTime(pEgresos["FechaConciliacion"]);
            }

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarEgresos(Egresos, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Egresos.AgregarEgresos(ConexionBaseDatos);

                CEgresosSucursal EgresosSucursal = new CEgresosSucursal();
                EgresosSucursal.IdEgresos = Egresos.IdEgresos;
                EgresosSucursal.IdSucursal = Usuario.IdSucursalActual;
                EgresosSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                EgresosSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                EgresosSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Egresos.IdEgresos;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("Egresos", ConexionBaseDatos);

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
    public static string AgregarEgresosEdicion(Dictionary<string, object> pEgresos)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CEgresos Egresos = new CEgresos();
            Egresos.IdCuentaBancaria = Convert.ToInt32(pEgresos["IdCuentaBancaria"]);
            Egresos.IdProveedor = Convert.ToInt32(pEgresos["IdProveedor"]);
            Egresos.IdMetodoPago = Convert.ToInt32(pEgresos["IdMetodoPago"]);
            Egresos.FechaEmision = Convert.ToDateTime(pEgresos["Fecha"]);
            Egresos.Importe = Convert.ToDecimal(pEgresos["Importe"]);
            Egresos.Referencia = Convert.ToString(pEgresos["Referencia"]);
            Egresos.ConceptoGeneral = Convert.ToString(pEgresos["ConceptoGeneral"]);
            Egresos.FechaAplicacion = Convert.ToDateTime(pEgresos["FechaAplicacion"]);
            Egresos.FechaConciliacion = Convert.ToDateTime(pEgresos["FechaConciliacion"]);
            Egresos.Conciliado = Convert.ToBoolean(pEgresos["Conciliado"]);
            Egresos.Asociado = Convert.ToBoolean(pEgresos["Asociado"]);
            Egresos.TipoCambio = Convert.ToDecimal(pEgresos["TipoCambio"]);
            Egresos.IdTipoMoneda = Convert.ToInt32(pEgresos["IdTipoMoneda"]);
            Egresos.TipoCambioDOF = Convert.ToDecimal(pEgresos["TipoCambioDOF"]);
            Egresos.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarEgresos(Egresos, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Egresos.AgregarEgresos(ConexionBaseDatos);

                CEgresosSucursal EgresosSucursal = new CEgresosSucursal();
                EgresosSucursal.IdEgresos = Egresos.IdEgresos;
                EgresosSucursal.IdSucursal = Usuario.IdSucursalActual;
                EgresosSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                EgresosSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                EgresosSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Egresos.IdEgresos;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("Egresos", ConexionBaseDatos);

                Egresos.LlenaObjeto(Egresos.IdEgresos, ConexionBaseDatos);
                oRespuesta.Add("IdEgresos", Egresos.IdEgresos);
                oRespuesta.Add("Folio", Egresos.Folio);
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
    public static string ObtenerFormaEgresos(int pIdEgresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEgresos = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEgresos" }, ConexionBaseDatos) == "")
        {
            puedeEditarEgresos = 1;
        }
        oPermisos.Add("puedeEditarEgresos", puedeEditarEgresos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEgresos.ObtenerEgresos(Modelo, pIdEgresos, ConexionBaseDatos);
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
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> Egresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEgresos = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarEgresos" }, ConexionBaseDatos) == "")
        {
            puedeEditarEgresos = 1;
        }
        oPermisos.Add("puedeEditarEgresos", puedeEditarEgresos);

        if (respuesta == "Conexion Establecida")
        {
            CEgresos EgresosProveedor = new CEgresos();
            EgresosProveedor.LlenaObjeto(Convert.ToInt32(Egresos["pIdEgresos"]), ConexionBaseDatos);
            EgresosProveedor.IdProveedor = Convert.ToInt32(Egresos["pIdProveedor"]);
            EgresosProveedor.TipoCambio = Convert.ToDecimal(Egresos["TipoCambio"]);
            EgresosProveedor.TipoCambioDOF = Convert.ToDecimal(Egresos["TipoCambioDOF"]);
            EgresosProveedor.EditarEgresos(ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CEgresos.ObtenerEgresos(Modelo, Convert.ToInt32(Egresos["pIdEgresos"]), ConexionBaseDatos);
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

    [WebMethod] //en este metodo es donde hace referencia al combo que se llena en la pantalla
    public static string ObtenerFormaAgregarEgresos()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();

        int puedeEditarTipoCambioEgresos = 0;
        JObject oPermisos = new JObject();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioEgresos" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioEgresos = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioEgresos", puedeEditarTipoCambioEgresos);

        if (respuesta == "Conexion Establecida")
        {
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Convert.ToInt32(Usuario.IdSucursalActual), ConexionBaseDatos);
            DateTime Fecha = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            string existeTipoCambio = ValidarExisteTipoCambio(Sucursal, Fecha, ConexionBaseDatos);
            if (existeTipoCambio == "")
            {
                JObject Modelo = new JObject();
                CMetodoPago MetodoPago = new CMetodoPago();
                Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(ConexionBaseDatos, 2));
                Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
                Modelo.Add(new JProperty("Fecha", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("FechaAplicacion", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("FechaConciliacion", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("Permisos", oPermisos));
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Modelo", Modelo));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No hay tipo de cambio del dia"));
            }
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

        int puedeEditarTipoCambioEgresos = 0;
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioEgresos" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioEgresos = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioEgresos", puedeEditarTipoCambioEgresos);

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
    public static string ObtenerFormaEditarEgresos(int IdEgresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarEgresos = 0;
        int puedeEditarTipoCambioEgresos = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        puedeEditarEgresos = Usuario.TienePermisos(new string[] { "puedeEditarEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarTipoCambioEgresos = Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;
       
        oPermisos.Add("puedeEditarEgresos", puedeEditarEgresos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEgresos.ObtenerEgresos(Modelo, IdEgresos, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(Convert.ToInt32(Modelo["IdMetodoPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("puedeEditarTipoCambioEgresos", puedeEditarTipoCambioEgresos);
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
    public static string ObtenerFormaFiltroEgresos()
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
    public static string ObtenerHabilitaMonto(int pIdEgresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        int puedeEditarTipoCambioEgresos = 0;
        puedeEditarTipoCambioEgresos = Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioEgresos" }, ConexionBaseDatos) == "" ? 1 : 0;

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CEgresos.ObtenerEgresos(Modelo, pIdEgresos, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            Modelo.Add("puedeEditarTipoCambioEgresos", puedeEditarTipoCambioEgresos);
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
    public static string EditarEgresos(Dictionary<string, object> pEgresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEgresos Egresos = new CEgresos();
        Egresos.LlenaObjeto(Convert.ToInt32(pEgresos["IdEgresos"]), ConexionBaseDatos);
        Egresos.IdEgresos = Convert.ToInt32(pEgresos["IdEgresos"]);
        Egresos.IdProveedor = Convert.ToInt32(pEgresos["IdProveedor"]);
        Egresos.Referencia = Convert.ToString(pEgresos["Referencia"]);
        Egresos.ConceptoGeneral = Convert.ToString(pEgresos["ConceptoGeneral"]);
        Egresos.Conciliado = Convert.ToBoolean(pEgresos["Conciliado"]);
        Egresos.Asociado = Convert.ToBoolean(pEgresos["Asociado"]);
        Egresos.Importe = Convert.ToDecimal(pEgresos["Importe"]);
        Egresos.TipoCambio = Convert.ToDecimal(pEgresos["TipoCambio"]);
        Egresos.TipoCambioDOF = Convert.ToDecimal(pEgresos["TipoCambioDOF"]);
        if (Convert.ToBoolean(pEgresos["Conciliado"]))
        {
            Egresos.FechaConciliacion = Convert.ToDateTime(pEgresos["FechaConciliacion"]);
        }

        string validacion = ValidarEgresos(Egresos, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Egresos.EditarEgresos(ConexionBaseDatos);
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
    public static string EditarMontos(Dictionary<string, object> pEgresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        CEgresosEncabezadoFacturaProveedor EgresosEncabezadoFacturaProveedor = new CEgresosEncabezadoFacturaProveedor();
        EgresosEncabezadoFacturaProveedor.IdEgresos = Convert.ToInt32(pEgresos["IdEgresos"]);
        EgresosEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor = Convert.ToInt32(pEgresos["IdEncabezadoFacturaProveedor"]);
        EgresosEncabezadoFacturaProveedor.Monto = Convert.ToDecimal(pEgresos["Monto"]);
        EgresosEncabezadoFacturaProveedor.FechaPago = Convert.ToDateTime(DateTime.Now);
        EgresosEncabezadoFacturaProveedor.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        EgresosEncabezadoFacturaProveedor.IdTipoMoneda = Convert.ToInt32(pEgresos["IdTipoMoneda"]);
        EgresosEncabezadoFacturaProveedor.TipoCambio = Convert.ToDecimal(pEgresos["TipoCambio"]);
        EgresosEncabezadoFacturaProveedor.Nota = "pago de la factura";
        string validacion = ValidarMontos(EgresosEncabezadoFacturaProveedor, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            EgresosEncabezadoFacturaProveedor.AgregarEgresosEncabezadoFacturaProveedor(ConexionBaseDatos);

            oRespuesta.Add("Monto", Convert.ToDecimal(pEgresos["Monto"]));
            oRespuesta.Add("rowid", Convert.ToDecimal(pEgresos["rowid"]));
            oRespuesta.Add("TipoMoneda", Convert.ToString(pEgresos["TipoMoneda"]));
            oRespuesta.Add("AbonosEgresos", EgresosEncabezadoFacturaProveedor.TotalAbonosEgresos(Convert.ToInt32(pEgresos["IdEgresos"]), ConexionBaseDatos));
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
    public static string EliminarEgresosEncabezadoFacturaProveedor(Dictionary<string, object> pEgresosEncabezadoFacturaProveedor)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        CEgresosEncabezadoFacturaProveedor EgresosEncabezadoFacturaProveedor = new CEgresosEncabezadoFacturaProveedor();
        EgresosEncabezadoFacturaProveedor.LlenaObjeto(Convert.ToInt32(pEgresosEncabezadoFacturaProveedor["pIdEgresosEncabezadoFacturaProveedor"]), ConexionBaseDatos);
        EgresosEncabezadoFacturaProveedor.IdEgresosEncabezadoFacturaProveedor = Convert.ToInt32(pEgresosEncabezadoFacturaProveedor["pIdEgresosEncabezadoFacturaProveedor"]);
        EgresosEncabezadoFacturaProveedor.Baja = true;
        EgresosEncabezadoFacturaProveedor.EliminarEgresosEncabezadoFacturaProveedor(ConexionBaseDatos);
        CEncabezadoFacturaProveedor EncabezadoFacturaProveedor = new CEncabezadoFacturaProveedor();
        EncabezadoFacturaProveedor.LlenaObjeto(EgresosEncabezadoFacturaProveedor.IdEncabezadoFacturaProveedor, ConexionBaseDatos);
        EncabezadoFacturaProveedor.IdEstatusEncabezadoFacturaProveedor = 5;
        EncabezadoFacturaProveedor.Editar(ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        oRespuesta.Add("AbonosEgresos", EgresosEncabezadoFacturaProveedor.TotalAbonosEgresos(EgresosEncabezadoFacturaProveedor.IdEgresos, ConexionBaseDatos));
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string EditarEgresosProveedor(Dictionary<string, object> pEgresos)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CEgresos Egresos = new CEgresos();
        Egresos.LlenaObjeto(Convert.ToInt32(pEgresos["IdEgresos"]), ConexionBaseDatos);
        Egresos.IdEgresos = Convert.ToInt32(pEgresos["IdEgresos"]);
        Egresos.IdProveedor = Convert.ToInt32(pEgresos["IdProveedor"]);
        string validacion = ValidarEgresos(Egresos, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Egresos.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdEgresos, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CEgresos Egresos = new CEgresos();
            Egresos.IdEgresos = pIdEgresos;
            Egresos.Baja = pBaja;

            if (pBaja == true)
            {
                if (Egresos.ExisteEgresosMovimientos(Convert.ToInt32(pIdEgresos), ConexionBaseDatos) == 0)
                {
                    Egresos.Eliminar(ConexionBaseDatos);
                    Egresos.LlenaObjeto(pIdEgresos, ConexionBaseDatos);
                    CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
                    CuentaBancaria.LlenaObjeto(Egresos.IdCuentaBancaria, ConexionBaseDatos);
                    CuentaBancaria.Saldo = CuentaBancaria.Saldo + Egresos.Importe;
                    CuentaBancaria.Editar(ConexionBaseDatos);

                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", "Baja correctamente"));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", "No se puede dar de baja este pago, porque existen documentos asociados."));
                }
            }
            else
            {
                Egresos.Eliminar(ConexionBaseDatos);
                Egresos.LlenaObjeto(pIdEgresos, ConexionBaseDatos);
                CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
                CuentaBancaria.LlenaObjeto(Egresos.IdCuentaBancaria, ConexionBaseDatos);
                CuentaBancaria.Saldo = CuentaBancaria.Saldo - Egresos.Importe;
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

    //Validaciones
    private static string ValidarEgresos(CEgresos pEgresos, CConexion pConexion)
    {
        string errores = "";

        if (pEgresos.IdCuentaBancaria == 0)
        { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

        if (pEgresos.IdMetodoPago == 0)
        { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

        if (pEgresos.Importe == 0)
        { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

        if (pEgresos.Referencia == "")
        { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

        if (pEgresos.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarMontos(CEgresosEncabezadoFacturaProveedor Egresos, CConexion pConexion)
    {
        string errores = "";

        if (Egresos.IdEncabezadoFacturaProveedor == 0)
        { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

        if (Egresos.IdEgresos == 0)
        { errores = errores + "<span>*</span> No hay cuenta por cobrar seleccionada, favor de elegir alguna.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarExisteTipoCambio(CSucursal Sucursal, DateTime Fecha, CConexion pConexion)
    {
        string errores = "";
        bool ExisteTipoCambio = false;
        CTipoCambio TipoCambio = new CTipoCambio();

        ExisteTipoCambio = TipoCambio.ExisteTipoCambioOrigen(Sucursal.IdTipoMoneda, Fecha, pConexion);
        if (ExisteTipoCambio == false)
        {
            errores = errores + "<span>*</span> No existe tipo cambio para hoy.<br />";
        }
        return errores;
    }
}