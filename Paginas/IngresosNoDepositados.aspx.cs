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

public partial class IngresosNoDepositados : System.Web.UI.Page
{
    public static int puedeAgregarIngresosNoDepositados = 0;
    public static int puedeEditarIngresosNoDepositados = 0;
    public static int puedeEliminarIngresosNoDepositados = 0;
    public static int puedeEliminarCobroIngresosNoDepositados = 0;

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

        puedeAgregarIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeAgregarIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeEditarIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeEliminarIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarCobroIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeEliminarCobroIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridIngresosNoDepositados
        CJQGrid GridIngresosNoDepositados = new CJQGrid();
        GridIngresosNoDepositados.NombreTabla = "grdIngresosNoDepositados";
        GridIngresosNoDepositados.CampoIdentificador = "IdIngresosNoDepositados";
        GridIngresosNoDepositados.ColumnaOrdenacion = "Sucursal,Folio";
        GridIngresosNoDepositados.Metodo = "ObtenerIngresosNoDepositados";
        GridIngresosNoDepositados.TituloTabla = "Ingresos no depositados";
        GridIngresosNoDepositados.GenerarFuncionFiltro = false;

        //IdIngresosNoDepositados
        CJQColumn ColIdIngresosNoDepositados = new CJQColumn();
        ColIdIngresosNoDepositados.Nombre = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositados.Oculto = "true";
        ColIdIngresosNoDepositados.Encabezado = "IdIngresosNoDepositados";
        ColIdIngresosNoDepositados.Buscador = "false";
        GridIngresosNoDepositados.Columnas.Add(ColIdIngresosNoDepositados);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "true";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridIngresosNoDepositados.Columnas.Add(ColSucursal);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "80";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        GridIngresosNoDepositados.Columnas.Add(ColFolio);

        //Importe
        CJQColumn ColImporte = new CJQColumn();
        ColImporte.Nombre = "Importe";
        ColImporte.Encabezado = "Importe";
        ColImporte.Buscador = "false";
        ColImporte.Formato = "FormatoMoneda";
        ColImporte.Alineacion = "right";
        ColImporte.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColImporte);

        //Cuenta
        CJQColumn ColCuenta = new CJQColumn();
        ColCuenta.Nombre = "CuentaBancaria";
        ColCuenta.Encabezado = "Cuenta bancaria";
        ColCuenta.Buscador = "false";
        ColCuenta.Alineacion = "left";
        ColCuenta.Ancho = "100";
        GridIngresosNoDepositados.Columnas.Add(ColCuenta);

        //CuentaBancaria
        CJQColumn ColNombreCuentaBancaria = new CJQColumn();
        ColNombreCuentaBancaria.Nombre = "NombreCuentaBancaria";
        ColNombreCuentaBancaria.Encabezado = "Banco";
        ColNombreCuentaBancaria.Buscador = "false";
        ColNombreCuentaBancaria.Alineacion = "left";
        ColNombreCuentaBancaria.Ancho = "100";
        GridIngresosNoDepositados.Columnas.Add(ColNombreCuentaBancaria);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColTipoMoneda);

        //FechaEmision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha de captura";
        ColFechaEmision.Buscador = "false";
        ColFechaEmision.Alineacion = "left";
        ColFechaEmision.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColFechaEmision);

        //FechaDeposito
        CJQColumn ColFechaDeposito = new CJQColumn();
        ColFechaDeposito.Nombre = "FechaDeposito";
        ColFechaDeposito.Encabezado = "Fecha de depósito";
        ColFechaDeposito.Buscador = "false";
        ColFechaDeposito.Alineacion = "left";
        ColFechaDeposito.Ancho = "80";
        GridIngresosNoDepositados.Columnas.Add(ColFechaDeposito);

        //Asociado
        CJQColumn ColAsociado = new CJQColumn();
        ColAsociado.Nombre = "Asociado";
        ColAsociado.Encabezado = "Asociado";
        ColAsociado.Buscador = "false";
        ColAsociado.Alineacion = "left";
        ColAsociado.Ancho = "40";
        GridIngresosNoDepositados.Columnas.Add(ColAsociado);

        //Depositado
        CJQColumn ColDepositado = new CJQColumn();
        ColDepositado.Nombre = "Depositado";
        ColDepositado.Encabezado = "Depositado";
        ColDepositado.Buscador = "false";
        ColDepositado.Alineacion = "left";
        ColDepositado.Ancho = "40";
        GridIngresosNoDepositados.Columnas.Add(ColDepositado);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razon social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "100";
        GridIngresosNoDepositados.Columnas.Add(ColRazonSocial);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarIngresosNoDepositados == 1 ? "A/I" : "A/IDeshabilitado";       
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridIngresosNoDepositados.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarIngresosNoDepositados";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridIngresosNoDepositados.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdIngresosNoDepositados", GridIngresosNoDepositados.GeneraGrid(), true);

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

        //Forma de pago
        CJQColumn ColFormaPago = new CJQColumn();
        ColFormaPago.Nombre = "FormaPago";
        ColFormaPago.Encabezado = "Forma de pago";
        ColFormaPago.Buscador = "false";
        ColFormaPago.Alineacion = "left";
        ColFormaPago.Ancho = "100";
        GridFacturas.Columnas.Add(ColFormaPago);

        //Numero de parcialidades
        CJQColumn ColNumeroParcialidades = new CJQColumn();
        ColNumeroParcialidades.Nombre = "Parcialidades";
        ColNumeroParcialidades.Encabezado = "Parcialidades";
        ColNumeroParcialidades.Buscador = "false";
        ColNumeroParcialidades.Alineacion = "left";
        ColNumeroParcialidades.Ancho = "60";
        GridFacturas.Columnas.Add(ColNumeroParcialidades);

        //Parcialidades
        CJQColumn ColEsParcialidad = new CJQColumn();
        ColEsParcialidad.Nombre = "EsParcialidad";
        ColEsParcialidad.Oculto = "true";
        ColEsParcialidad.Encabezado = "Es parcialidad";
        ColEsParcialidad.Buscador = "false";
        ColEsParcialidad.Ancho = "10";
        GridFacturas.Columnas.Add(ColEsParcialidad);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturas", GridFacturas.GeneraGrid(), true);

        //GridMovimientosCobros
        CJQGrid grdMovimientosCobros = new CJQGrid();
        grdMovimientosCobros.NombreTabla = "grdMovimientosCobros";
        grdMovimientosCobros.CampoIdentificador = "IdIngresosNoDepositadosEncabezadoFactura";
        grdMovimientosCobros.ColumnaOrdenacion = "IdIngresosNoDepositadosEncabezadoFactura";
        grdMovimientosCobros.TipoOrdenacion = "DESC";
        grdMovimientosCobros.Metodo = "ObtenerMovimientosCobros";
        grdMovimientosCobros.TituloTabla = "Documentos asociados";
        grdMovimientosCobros.GenerarFuncionFiltro = false;
        grdMovimientosCobros.GenerarFuncionTerminado = false;
        grdMovimientosCobros.Altura = 120;
        grdMovimientosCobros.NumeroRegistros = 15;
        grdMovimientosCobros.RangoNumeroRegistros = "15,30,60";

        //IdIngresosNoDepositadosEncabezadoFactura
        CJQColumn ColIdIngresosNoDepositadosEncabezadoFactura = new CJQColumn();
        ColIdIngresosNoDepositadosEncabezadoFactura.Nombre = "IdIngresosNoDepositadosEncabezadoFactura";
        ColIdIngresosNoDepositadosEncabezadoFactura.Oculto = "true";
        ColIdIngresosNoDepositadosEncabezadoFactura.Encabezado = "IdIngresosNoDepositadosEncabezadoFactura";
        ColIdIngresosNoDepositadosEncabezadoFactura.Buscador = "false";
        grdMovimientosCobros.Columnas.Add(ColIdIngresosNoDepositadosEncabezadoFactura);

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
        ColEliminarMovimiento.Imagen = "eliminar.png";
        ColEliminarMovimiento.Estilo = "divImagenConsultar imgEliminarMovimiento";
        ColEliminarMovimiento.Buscador = "false";
        ColEliminarMovimiento.Ordenable = "false";
        ColEliminarMovimiento.Oculto = puedeEliminarCobroIngresosNoDepositados == 1 ? "false" : "true";
        ColEliminarMovimiento.Ancho = "25";
        grdMovimientosCobros.Columnas.Add(ColEliminarMovimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobros", grdMovimientosCobros.GeneraGrid(), true);

        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdIngresosNoDepositadosEncabezadoFactura";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdIngresosNoDepositadosEncabezadoFactura";
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
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdIngresosNoDepositadosEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdIngresosNoDepositadosEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Buscador = "false";
        grdMovimientosCobrosConsultar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaConsultar);

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
        grdMovimientosCobrosEditar.CampoIdentificador = "IdIngresosNoDepositadosEncabezadoFactura";
        grdMovimientosCobrosEditar.ColumnaOrdenacion = "IdIngresosNoDepositadosEncabezadoFactura";
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
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Nombre = "IdIngresosNoDepositadosEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Encabezado = "IdIngresosNoDepositadosEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Buscador = "false";
        grdMovimientosCobrosEditar.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFacturaEditar);

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
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerIngresosNoDepositados(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositados", sqlCon);
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
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdIngresosNoDepositados)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdIngresosNoDepositados", SqlDbType.Int).Value = pIdIngresosNoDepositados;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdIngresosNoDepositados)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdIngresosNoDepositados", SqlDbType.Int).Value = pIdIngresosNoDepositados;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdIngresosNoDepositados)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdIngresosNoDepositadosEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdIngresosNoDepositados", SqlDbType.Int).Value = pIdIngresosNoDepositados;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarIngresosNoDepositados(string pIngresosNoDepositados)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonIngresosNoDepositados = new CJson();
        jsonIngresosNoDepositados.StoredProcedure.CommandText = "sp_IngresosNoDepositados_Consultar_FiltroPorIngresosNoDepositados";
        jsonIngresosNoDepositados.StoredProcedure.Parameters.AddWithValue("@pIngresosNoDepositados", pIngresosNoDepositados);
        return jsonIngresosNoDepositados.ObtenerJsonString(ConexionBaseDatos);
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
    public static string AgregarIngresosNoDepositados(Dictionary<string, object> pIngresosNoDepositados)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
            IngresosNoDepositados.IdCuentaBancaria = Convert.ToInt32(pIngresosNoDepositados["IdCuentaBancaria"]);
            IngresosNoDepositados.IdCliente = Convert.ToInt32(pIngresosNoDepositados["IdCliente"]);
            IngresosNoDepositados.IdMetodoPago = Convert.ToInt32(pIngresosNoDepositados["IdMetodoPago"]);
            IngresosNoDepositados.FechaEmision = Convert.ToDateTime(pIngresosNoDepositados["Fecha"]);
            IngresosNoDepositados.Importe = Convert.ToDecimal(pIngresosNoDepositados["Importe"]);
            IngresosNoDepositados.Referencia = Convert.ToString(pIngresosNoDepositados["Referencia"]);
            IngresosNoDepositados.ConceptoGeneral = Convert.ToString(pIngresosNoDepositados["ConceptoGeneral"]);
            IngresosNoDepositados.FechaDeposito = Convert.ToDateTime(pIngresosNoDepositados["FechaDeposito"]);
            IngresosNoDepositados.Depositado = Convert.ToBoolean(pIngresosNoDepositados["Depositado"]);
            IngresosNoDepositados.Asociado = Convert.ToBoolean(pIngresosNoDepositados["Asociado"]);
            IngresosNoDepositados.TipoCambio = Convert.ToDecimal(pIngresosNoDepositados["TipoCambio"]);
            IngresosNoDepositados.TipoCambioDOF = Convert.ToDecimal(pIngresosNoDepositados["TipoCambioDOF"]);
            IngresosNoDepositados.IdTipoMoneda = Convert.ToInt32(pIngresosNoDepositados["IdTipoMoneda"]);
            IngresosNoDepositados.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            IngresosNoDepositados.FechaPago = Convert.ToDateTime(pIngresosNoDepositados["FechaPago"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarIngresosNoDepositados(IngresosNoDepositados, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                IngresosNoDepositados.AgregarIngresosNoDepositados(ConexionBaseDatos);

                CIngresosNoDepositadosSucursal IngresosNoDepositadosSucursal = new CIngresosNoDepositadosSucursal();
                IngresosNoDepositadosSucursal.IdIngresosNoDepositados = IngresosNoDepositados.IdIngresosNoDepositados;
                IngresosNoDepositadosSucursal.IdSucursal = Usuario.IdSucursalActual;
                IngresosNoDepositadosSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                IngresosNoDepositadosSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                IngresosNoDepositadosSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = IngresosNoDepositados.IdIngresosNoDepositados;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("IngresosNoDepositados", ConexionBaseDatos);

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
    public static string AgregarIngresosNoDepositadosEdicion(Dictionary<string, object> pIngresosNoDepositados)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
            IngresosNoDepositados.IdCuentaBancaria = Convert.ToInt32(pIngresosNoDepositados["IdCuentaBancaria"]);
            IngresosNoDepositados.IdCliente = Convert.ToInt32(pIngresosNoDepositados["IdCliente"]);
            IngresosNoDepositados.IdMetodoPago = Convert.ToInt32(pIngresosNoDepositados["IdMetodoPago"]);
            IngresosNoDepositados.FechaEmision = Convert.ToDateTime(pIngresosNoDepositados["Fecha"]);
            IngresosNoDepositados.Importe = Convert.ToDecimal(pIngresosNoDepositados["Importe"]);
            IngresosNoDepositados.Referencia = Convert.ToString(pIngresosNoDepositados["Referencia"]);
            IngresosNoDepositados.ConceptoGeneral = Convert.ToString(pIngresosNoDepositados["ConceptoGeneral"]);
            IngresosNoDepositados.FechaDeposito = Convert.ToDateTime(pIngresosNoDepositados["FechaDeposito"]);
            IngresosNoDepositados.Depositado = Convert.ToBoolean(pIngresosNoDepositados["Depositado"]);
            IngresosNoDepositados.Asociado = Convert.ToBoolean(pIngresosNoDepositados["Asociado"]);
            IngresosNoDepositados.TipoCambio = Convert.ToDecimal(pIngresosNoDepositados["TipoCambio"]);
            IngresosNoDepositados.IdTipoMoneda = Convert.ToInt32(pIngresosNoDepositados["IdTipoMoneda"]);
            IngresosNoDepositados.TipoCambioDOF = Convert.ToDecimal(pIngresosNoDepositados["TipoCambioDOF"]);
            IngresosNoDepositados.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            IngresosNoDepositados.FechaPago = Convert.ToDateTime(pIngresosNoDepositados["FechaPago"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarIngresosNoDepositados(IngresosNoDepositados, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                IngresosNoDepositados.AgregarIngresosNoDepositados(ConexionBaseDatos);

                CIngresosNoDepositadosSucursal IngresosNoDepositadosSucursal = new CIngresosNoDepositadosSucursal();
                IngresosNoDepositadosSucursal.IdIngresosNoDepositados = IngresosNoDepositados.IdIngresosNoDepositados;
                IngresosNoDepositadosSucursal.IdSucursal = Usuario.IdSucursalActual;
                IngresosNoDepositadosSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                IngresosNoDepositadosSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                IngresosNoDepositadosSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = IngresosNoDepositados.IdIngresosNoDepositados;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("IngresosNoDepositados", ConexionBaseDatos);

                IngresosNoDepositados.LlenaObjeto(IngresosNoDepositados.IdIngresosNoDepositados, ConexionBaseDatos);
                oRespuesta.Add("IdIngresosNoDepositados", IngresosNoDepositados.IdIngresosNoDepositados);
                oRespuesta.Add("Folio", IngresosNoDepositados.Folio);
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
    public static string ObtenerFormaIngresosNoDepositados(int pIdIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarIngresosNoDepositados = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarIngresosNoDepositados" }, ConexionBaseDatos) == "")
        {
            puedeEditarIngresosNoDepositados = 1;
        }
        oPermisos.Add("puedeEditarIngresosNoDepositados", puedeEditarIngresosNoDepositados);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CIngresosNoDepositados.ObtenerIngresosNoDepositados(Modelo, pIdIngresosNoDepositados, ConexionBaseDatos);
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
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> IngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarIngresosNoDepositados = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarIngresosNoDepositados" }, ConexionBaseDatos) == "")
        {
            puedeEditarIngresosNoDepositados = 1;
        }
        oPermisos.Add("puedeEditarIngresosNoDepositados", puedeEditarIngresosNoDepositados);

        if (respuesta == "Conexion Establecida")
        {
            CIngresosNoDepositados IngresosNoDepositadosCliente = new CIngresosNoDepositados();
            IngresosNoDepositadosCliente.LlenaObjeto(Convert.ToInt32(IngresosNoDepositados["pIdIngresosNoDepositados"]), ConexionBaseDatos);
            IngresosNoDepositadosCliente.IdCliente = Convert.ToInt32(IngresosNoDepositados["pIdCliente"]);
            IngresosNoDepositadosCliente.TipoCambio = Convert.ToDecimal(IngresosNoDepositados["TipoCambio"]);
            IngresosNoDepositadosCliente.TipoCambioDOF = Convert.ToDecimal(IngresosNoDepositados["TipoCambioDOF"]);
            IngresosNoDepositadosCliente.Editar(ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CIngresosNoDepositados.ObtenerIngresosNoDepositados(Modelo, Convert.ToInt32(IngresosNoDepositados["pIdIngresosNoDepositados"]), ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarIngresosNoDepositados()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        int puedeEditarTipoCambioIngresosNoDepositados = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresosNoDepositados" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioIngresosNoDepositados = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioIngresosNoDepositados", puedeEditarTipoCambioIngresosNoDepositados);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMetodoPago MetodoPago = new CMetodoPago();
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(ConexionBaseDatos, 5));
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(ConexionBaseDatos));
            Modelo.Add(new JProperty("Fecha", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
            Modelo.Add(new JProperty("FechaAplicacion", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
            Modelo.Add(new JProperty("FechaPago", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
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
    public static string ObtenerDatosCuentaBancaria(int pIdCuentaBancaria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        int puedeEditarTipoCambioIngresosNoDepositados = 0;
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresosNoDepositados" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioIngresosNoDepositados = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioIngresosNoDepositados", puedeEditarTipoCambioIngresosNoDepositados);

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
    public static string ObtenerHabilitaMonto(int pIdIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        int puedeEditarTipoCambioIngresosNoDepositados = 0;
        puedeEditarTipoCambioIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CIngresosNoDepositados.ObtenerIngresosNoDepositados(Modelo, pIdIngresosNoDepositados, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            Modelo.Add("puedeEditarTipoCambioIngresosNoDepositados", puedeEditarTipoCambioIngresosNoDepositados);
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
    public static string ObtenerFormaEditarIngresosNoDepositados(int IdIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarIngresosNoDepositados = 0;
        int puedeEditarTipoCambioIngresosNoDepositados = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        puedeEditarIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeEditarIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarTipoCambioIngresosNoDepositados = Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresosNoDepositados" }, ConexionBaseDatos) == "" ? 1 : 0;

        
        oPermisos.Add("puedeEditarIngresosNoDepositados", puedeEditarIngresosNoDepositados);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CIngresosNoDepositados.ObtenerIngresosNoDepositados(Modelo, IdIngresosNoDepositados, ConexionBaseDatos);
            Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(Convert.ToInt32(Modelo["IdMetodoPago"].ToString()), ConexionBaseDatos));
            Modelo.Add("puedeEditarTipoCambioIngresosNoDepositados", puedeEditarTipoCambioIngresosNoDepositados);
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
    public static string ObtenerFormaFiltroIngresosNoDepositados()
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
    public static string EditarIngresosNoDepositados(Dictionary<string, object> pIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
        IngresosNoDepositados.LlenaObjeto(Convert.ToInt32(pIngresosNoDepositados["IdIngresosNoDepositados"]), ConexionBaseDatos);
        IngresosNoDepositados.IdIngresosNoDepositados = Convert.ToInt32(pIngresosNoDepositados["IdIngresosNoDepositados"]);
        IngresosNoDepositados.IdCliente = Convert.ToInt32(pIngresosNoDepositados["IdCliente"]);
        IngresosNoDepositados.Referencia = Convert.ToString(pIngresosNoDepositados["Referencia"]);
        IngresosNoDepositados.ConceptoGeneral = Convert.ToString(pIngresosNoDepositados["ConceptoGeneral"]);
        IngresosNoDepositados.Depositado = Convert.ToBoolean(pIngresosNoDepositados["Depositado"]);
        IngresosNoDepositados.Asociado = Convert.ToBoolean(pIngresosNoDepositados["Asociado"]);
        IngresosNoDepositados.Importe = Convert.ToDecimal(pIngresosNoDepositados["Importe"]);
        IngresosNoDepositados.IdTipoMoneda = Convert.ToInt32(pIngresosNoDepositados["IdTipoMoneda"]);
        IngresosNoDepositados.TipoCambio = Convert.ToDecimal(pIngresosNoDepositados["TipoCambio"]);
        IngresosNoDepositados.TipoCambioDOF = Convert.ToDecimal(pIngresosNoDepositados["TipoCambioDOF"]);
        IngresosNoDepositados.FechaDeposito = Convert.ToDateTime(pIngresosNoDepositados["FechaDeposito"]);
        IngresosNoDepositados.FechaPago = Convert.ToDateTime(pIngresosNoDepositados["FechaPago"]);

        string validacion = ValidarIngresosNoDepositados(IngresosNoDepositados, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            IngresosNoDepositados.EditarIngresosNoDepositados(ConexionBaseDatos);
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
    public static string EditarMontos(Dictionary<string, object> pIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();

        CUtilerias Utilerias = new CUtilerias();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pIngresosNoDepositados["IdEncabezadoFactura"]), ConexionBaseDatos);

        CIngresosNoDepositadosEncabezadoFactura IngresosNoDepositadosEncabezadoFactura = new CIngresosNoDepositadosEncabezadoFactura();
        IngresosNoDepositadosEncabezadoFactura.IdIngresosNoDepositados = Convert.ToInt32(pIngresosNoDepositados["IdIngresosNoDepositados"]);
        IngresosNoDepositadosEncabezadoFactura.IdEncabezadoFactura = Convert.ToInt32(pIngresosNoDepositados["IdEncabezadoFactura"]);
        IngresosNoDepositadosEncabezadoFactura.Monto = Convert.ToDecimal(pIngresosNoDepositados["Monto"]);
        IngresosNoDepositadosEncabezadoFactura.FechaPago = Convert.ToDateTime(DateTime.Now);
        IngresosNoDepositadosEncabezadoFactura.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        IngresosNoDepositadosEncabezadoFactura.IdTipoMoneda = Convert.ToInt32(pIngresosNoDepositados["IdTipoMoneda"]);
        IngresosNoDepositadosEncabezadoFactura.TipoCambio = Convert.ToDecimal(pIngresosNoDepositados["TipoCambio"]);
        IngresosNoDepositadosEncabezadoFactura.Nota = "pago de la factura";

        string validacion = ValidarMontos(IngresosNoDepositadosEncabezadoFactura, FacturaEncabezado, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            IngresosNoDepositadosEncabezadoFactura.AgregarIngresosNoDepositadosEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("Monto", Convert.ToDecimal(pIngresosNoDepositados["Monto"]));
            oRespuesta.Add("rowid", Convert.ToDecimal(pIngresosNoDepositados["rowid"]));
            oRespuesta.Add("TipoMoneda", Convert.ToString(pIngresosNoDepositados["TipoMoneda"]));
            oRespuesta.Add("AbonosIngresosNoDepositados", IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(Convert.ToInt32(pIngresosNoDepositados["IdIngresosNoDepositados"]), ConexionBaseDatos));

            if (Convert.ToInt32(pIngresosNoDepositados["EsParcialidad"]) == 1)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                int NumeroParcialidadActual = 0;
                NumeroParcialidadActual = (FacturaEncabezado.NumeroParcialidades - FacturaEncabezado.NumeroParcialidadesPendientes) + 1;
                string Descripcion = "";
                Descripcion = "Parcialidad " + NumeroParcialidadActual + " de " + FacturaEncabezado.NumeroParcialidades;
                FacturaEncabezado.AgregarFacturaIndividual(ConexionBaseDatos, Descripcion, Convert.ToDecimal(pIngresosNoDepositados["Monto"]));

                CFacturaEncabezadoSucursal FacturaEncabezadoSucursal = new CFacturaEncabezadoSucursal();
                FacturaEncabezadoSucursal.IdFacturaEncabezado = FacturaEncabezado.IdFacturaEncabezado;
                FacturaEncabezadoSucursal.IdSucursal = Usuario.IdSucursalActual;
                FacturaEncabezadoSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                FacturaEncabezadoSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                FacturaEncabezadoSucursal.Agregar(ConexionBaseDatos);

                string TotalLetras = "";


                CTipoMoneda TipoMoneda = new CTipoMoneda();
                CFacturaEncabezado FacturaEncabezadoTotal = new CFacturaEncabezado();
                FacturaEncabezadoTotal.LlenaObjeto(Convert.ToInt32(FacturaEncabezado.IdFacturaEncabezado), ConexionBaseDatos);

                TipoMoneda.LlenaObjeto(FacturaEncabezadoTotal.IdTipoMoneda, ConexionBaseDatos);
                TotalLetras = Utilerias.ConvertLetter(FacturaEncabezadoTotal.Total.ToString(), TipoMoneda.TipoMoneda.ToString());
                FacturaEncabezadoTotal.TotalLetra = TotalLetras;
                FacturaEncabezadoTotal.Editar(ConexionBaseDatos);

                CFacturaEncabezado FacturaEncabezadoGlobal = new CFacturaEncabezado();
                FacturaEncabezadoGlobal.LlenaObjeto(FacturaEncabezadoTotal.IdFacturaGlobal, ConexionBaseDatos);
                FacturaEncabezadoGlobal.NumeroParcialidadesPendientes = FacturaEncabezadoGlobal.NumeroParcialidadesPendientes - 1;
                FacturaEncabezadoGlobal.Editar(ConexionBaseDatos);

                //Abrir Conexion

                CConfiguracion Configuracion = new CConfiguracion();
                Configuracion.LlenaObjeto(1, ConexionBaseDatos);
                //¿La conexion se establecio?

                string validacionFactura = CrearArchivoBuzonFiscal(Convert.ToInt32(FacturaEncabezadoTotal.IdFacturaEncabezado), ConexionBaseDatos);
                if (validacionFactura == "1")
                {
                    Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                    validacionFactura = BuzonFiscalTimbrado(Convert.ToInt32(FacturaEncabezadoTotal.IdFacturaEncabezado), ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", validacionFactura));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacionFactura));
                }

                oRespuesta.Add("EsParcialidad", 1);

            }
            else
            {
                oRespuesta.Add("EsParcialidad", 0);
                oRespuesta.Add(new JProperty("Error", 0));
            }

            ConexionBaseDatos.CerrarBaseDatosSqlServer();

        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", validacion));
        }
        return oRespuesta.ToString();

    }

    //timbrado de factura///////////////////////////////////////////////////////////////////////////////////        

    private static string CrearArchivoBuzonFiscal(int pIdFactura, CConexion pConexion)
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

    private static string BuzonFiscalTimbrado(int pIdFactura, CConexion pConexion)
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
    public static string EliminarIngresosNoDepositadosEncabezadoFactura(Dictionary<string, object> pIngresosNoDepositadosEncabezadoFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        int IdFacturaEncabezadoParcial = 0;

        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);

        CIngresosNoDepositadosEncabezadoFactura IngresosNoDepositadosEncabezadoFactura = new CIngresosNoDepositadosEncabezadoFactura();
        IngresosNoDepositadosEncabezadoFactura.LlenaObjeto(Convert.ToInt32(pIngresosNoDepositadosEncabezadoFactura["pIdIngresosNoDepositadosEncabezadoFactura"]), ConexionBaseDatos);
        FacturaEncabezado.LlenaObjeto(IngresosNoDepositadosEncabezadoFactura.IdEncabezadoFactura, ConexionBaseDatos);

        JObject oRespuesta = new JObject();

        if (FacturaEncabezado.Parcialidades == true)
        {
            IdFacturaEncabezadoParcial = IngresosNoDepositadosEncabezadoFactura.ValidaEliminarCuentasPorCobrarDetalle(Convert.ToInt32(FacturaEncabezado.IdFacturaEncabezado), ConexionBaseDatos);
            if (IdFacturaEncabezadoParcial != 0)
            {
                string MotivoCancelacion = "Se cancela la factura parcial por eliminación de cobro";

                string validacion = CancelarArchivoBuzonFiscal(Convert.ToInt32(IdFacturaEncabezadoParcial), ConexionBaseDatos);
                if (validacion == "1")
                {
                    CUtilerias Utilerias = new CUtilerias();
                    Utilerias.WaitSeconds(Convert.ToDouble(Configuracion.ValorLogico));
                    validacion = BuzonFiscalTimbradoCancelacion(Convert.ToInt32(IdFacturaEncabezadoParcial), MotivoCancelacion, ConexionBaseDatos);
                    if (validacion == "Factura cancelada correctamente")
                    {
                        IngresosNoDepositadosEncabezadoFactura.IdIngresosNoDepositadosEncabezadoFactura = Convert.ToInt32(pIngresosNoDepositadosEncabezadoFactura["pIdIngresosNoDepositadosEncabezadoFactura"]);
                        IngresosNoDepositadosEncabezadoFactura.Baja = true;
                        IngresosNoDepositadosEncabezadoFactura.EliminarIngresosNoDepositadosEncabezadoFactura(ConexionBaseDatos);
                        oRespuesta.Add("AbonosIngresosNoDepositados", IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(IngresosNoDepositadosEncabezadoFactura.IdIngresosNoDepositados, ConexionBaseDatos));
                        oRespuesta.Add(new JProperty("Error", 0));
                    }
                    else
                    {
                        oRespuesta.Add(new JProperty("Error", 1));
                    }
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                else
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", validacion));
                }
                oRespuesta.Add("EsParcialidad", 1);

            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "No se puede eliminar este movimiento porque la factura parcial no esta timbrada."));
            }
        }
        else
        {
            IngresosNoDepositadosEncabezadoFactura.IdIngresosNoDepositadosEncabezadoFactura = Convert.ToInt32(pIngresosNoDepositadosEncabezadoFactura["pIdIngresosNoDepositadosEncabezadoFactura"]);
            IngresosNoDepositadosEncabezadoFactura.Baja = true;
            IngresosNoDepositadosEncabezadoFactura.EliminarIngresosNoDepositadosEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("AbonosIngresosNoDepositados", IngresosNoDepositadosEncabezadoFactura.TotalAbonosIngresosNoDepositados(IngresosNoDepositadosEncabezadoFactura.IdIngresosNoDepositados, ConexionBaseDatos));
            oRespuesta.Add("EsParcialidad", 0);
            oRespuesta.Add(new JProperty("Error", 0));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();

    }

    private static string CancelarArchivoBuzonFiscal(int pIdFacturaEncabezado, CConexion pConexion)
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

    private static string BuzonFiscalTimbradoCancelacion(int pIdFacturaEncabezado, string MotivoCancelacion, CConexion pConexion)
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
    public static string EditarIngresosNoDepositadosCliente(Dictionary<string, object> pIngresosNoDepositados)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
        IngresosNoDepositados.LlenaObjeto(Convert.ToInt32(pIngresosNoDepositados["IdIngresosNoDepositados"]), ConexionBaseDatos);
        IngresosNoDepositados.IdIngresosNoDepositados = Convert.ToInt32(pIngresosNoDepositados["IdIngresosNoDepositados"]);
        IngresosNoDepositados.IdCliente = Convert.ToInt32(pIngresosNoDepositados["IdCliente"]);
        string validacion = ValidarIngresosNoDepositados(IngresosNoDepositados, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            IngresosNoDepositados.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdIngresosNoDepositados, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CIngresosNoDepositados IngresosNoDepositados = new CIngresosNoDepositados();
            IngresosNoDepositados.LlenaObjeto(pIdIngresosNoDepositados, ConexionBaseDatos);
            IngresosNoDepositados.Baja = pBaja;

            CDepositos Depositos = new CDepositos();
            Depositos.LlenaObjeto(IngresosNoDepositados.IdDeposito, ConexionBaseDatos);

            if (pBaja == true)
            {
                if (IngresosNoDepositados.ExisteIngresosNoDepositadosMovimientos(pIdIngresosNoDepositados, ConexionBaseDatos) > 0)
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", "No se puede dar de baja este cobro, porque existen documentos asociados."));
                }
                else if (IngresosNoDepositados.IdDeposito != 0 && Depositos.Baja == false)
                {
                    oRespuesta.Add(new JProperty("Error", 1));
                    oRespuesta.Add(new JProperty("Descripcion", "No se puede dar de baja este cobro, porque ya fue depositado."));
                }
                else
                {
                    IngresosNoDepositados.Eliminar(ConexionBaseDatos);
                    oRespuesta.Add(new JProperty("Error", 0));
                    oRespuesta.Add(new JProperty("Descripcion", "Baja correctamente"));
                }
            }
            else
            {
                IngresosNoDepositados.Eliminar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
                oRespuesta.Add(new JProperty("Descripcion", "Activado correctamente"));
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

    //Validaciones
    private static string ValidarIngresosNoDepositados(CIngresosNoDepositados pIngresosNoDepositados, CConexion pConexion)
    {
        string errores = "";

        if (pIngresosNoDepositados.IdCuentaBancaria == 0)
        { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

        if (pIngresosNoDepositados.IdMetodoPago == 0)
        { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

        if (pIngresosNoDepositados.Importe == 0)
        { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

        if (pIngresosNoDepositados.Referencia == "")
        { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

        if (pIngresosNoDepositados.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarMontos(CIngresosNoDepositadosEncabezadoFactura IngresosNoDepositados, CFacturaEncabezado FacturaEncabezado, CConexion pConexion)
    {
        string errores = "";

        if (FacturaEncabezado.Parcialidades == true)
        {
            if (FacturaEncabezado.NumeroParcialidadesPendientes == 1)
            {
                if (Convert.ToDecimal(IngresosNoDepositados.Monto) != Convert.ToDecimal(FacturaEncabezado.SaldoFactura))
                {
                    errores = errores + "<span>*</span> Es la ultima parcialidad, favor de pagar todo el saldo<br />";
                }
            }
        }

        if (IngresosNoDepositados.IdEncabezadoFactura == 0)
        { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

        if (IngresosNoDepositados.IdIngresosNoDepositados == 0)
        { errores = errores + "<span>*</span> No hay cuenta por cobrar seleccionada, favor de elegir alguna.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}