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
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class CuentasPorCobrar : System.Web.UI.Page
{
    public static int puedeAgregarCuentasPorCobrar = 0;
    public static int puedeEditarCuentasPorCobrar = 0;
    public static int puedeEliminarCuentasPorCobrar = 0;
    public static int puedeEliminarCobroCuentasPorCobrar = 0;
    public static int puedeConciliarCuentasPorCobrar = 0;

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

        puedeAgregarCuentasPorCobrar = Usuario.TienePermisos(new string[] { "puedeAgregarCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarCuentasPorCobrar = Usuario.TienePermisos(new string[] { "puedeEditarCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarCuentasPorCobrar = Usuario.TienePermisos(new string[] { "puedeEliminarCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarCobroCuentasPorCobrar = Usuario.TienePermisos(new string[] { "puedeEliminarCobroCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeConciliarCuentasPorCobrar = Usuario.TienePermisos(new string[] { "puedeConciliarCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;

        GenerarGridCuentasPorCobrar();
        GenerarGridCuentasPorCobrarConciliar();
        GenerarGridCuentasBancarias();
        GenerarGridFacturas();
        GenerarGridMovimientosCobros();
        GenerarGridMovimientosCobrosConsultar();
        GenerarGridMovimientosCobrosEditar();


        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    public void GenerarGridCuentasPorCobrar()
    {
        //GridCuentasPorCobrar
        CJQGrid GridCuentasPorCobrar = new CJQGrid();
        GridCuentasPorCobrar.NombreTabla = "grdCuentasPorCobrar";
        GridCuentasPorCobrar.CampoIdentificador = "IdCuentasPorCobrar";
        GridCuentasPorCobrar.ColumnaOrdenacion = "Sucursal,Folio";
        GridCuentasPorCobrar.Metodo = "ObtenerCuentasPorCobrar";
        GridCuentasPorCobrar.TituloTabla = "Ingresos";
        GridCuentasPorCobrar.ColumnaOrdenacion = "Folio";
        GridCuentasPorCobrar.TipoOrdenacion = "DESC";
        GridCuentasPorCobrar.GenerarFuncionFiltro = false;

        //IdCuentasPorCobrar
        CJQColumn ColIdCuentasPorCobrar = new CJQColumn();
        ColIdCuentasPorCobrar.Nombre = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrar.Oculto = "true";
        ColIdCuentasPorCobrar.Encabezado = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrar.Buscador = "false";
        GridCuentasPorCobrar.Columnas.Add(ColIdCuentasPorCobrar);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "false";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridCuentasPorCobrar.Columnas.Add(ColSucursal);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "60";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        GridCuentasPorCobrar.Columnas.Add(ColFolio);

        //Importe
        CJQColumn ColImporte = new CJQColumn();
        ColImporte.Nombre = "Importe";
        ColImporte.Encabezado = "Importe";
        ColImporte.Buscador = "false";
        ColImporte.Formato = "FormatoMoneda";
        ColImporte.Alineacion = "right";
        ColImporte.Ancho = "70";
        GridCuentasPorCobrar.Columnas.Add(ColImporte);

        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "Saldo";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Buscador = "false";
        ColSaldo.Formato = "FormatoMoneda";
        ColSaldo.Alineacion = "right";
        ColSaldo.Ancho = "70";
        GridCuentasPorCobrar.Columnas.Add(ColSaldo);

        //Cuenta
        CJQColumn ColCuenta = new CJQColumn();
        ColCuenta.Nombre = "CuentaBancaria";
        ColCuenta.Encabezado = "Cuenta bancaria";
        ColCuenta.Buscador = "false";
        ColCuenta.Alineacion = "left";
        ColCuenta.Ancho = "100";
        GridCuentasPorCobrar.Columnas.Add(ColCuenta);

        //CuentaBancaria
        CJQColumn ColNombreCuentaBancaria = new CJQColumn();
        ColNombreCuentaBancaria.Nombre = "NombreCuentaBancaria";
        ColNombreCuentaBancaria.Encabezado = "Nombre cuenta bancaria";
        ColNombreCuentaBancaria.Buscador = "false";
        ColNombreCuentaBancaria.Alineacion = "left";
        ColNombreCuentaBancaria.Ancho = "100";
        GridCuentasPorCobrar.Columnas.Add(ColNombreCuentaBancaria);

        //Referencia
        CJQColumn ColReferencia = new CJQColumn();
        ColReferencia.Nombre = "Referencia";
        ColReferencia.Encabezado = "Referencia";
        ColReferencia.Buscador = "false";
        ColReferencia.Alineacion = "left";
        ColReferencia.Ancho = "100";
        GridCuentasPorCobrar.Columnas.Add(ColReferencia);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "60";
        GridCuentasPorCobrar.Columnas.Add(ColTipoMoneda);

        //FechaEmision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha de captura";
        ColFechaEmision.Buscador = "false";
        ColFechaEmision.Alineacion = "left";
        ColFechaEmision.Ancho = "70";
        GridCuentasPorCobrar.Columnas.Add(ColFechaEmision);

        //FechaAplicacion
        CJQColumn ColFechaAplicacion = new CJQColumn();
        ColFechaAplicacion.Nombre = "FechaAplicacion";
        ColFechaAplicacion.Encabezado = "Fecha acreditado";
        ColFechaAplicacion.Buscador = "false";
        ColFechaAplicacion.Alineacion = "left";
        ColFechaAplicacion.Ancho = "70";
        GridCuentasPorCobrar.Columnas.Add(ColFechaAplicacion);

        //Asociado
        CJQColumn ColAsociado = new CJQColumn();
        ColAsociado.Nombre = "Asociado";
        ColAsociado.Encabezado = "Asociado";
        ColAsociado.Buscador = "true";
        ColAsociado.Alineacion = "left";
        ColAsociado.Ancho = "40";
        GridCuentasPorCobrar.Columnas.Add(ColAsociado);

        //Conciliado
        CJQColumn ColConciliado = new CJQColumn();
        ColConciliado.Nombre = "Conciliado";
        ColConciliado.Encabezado = "Conciliado";
        ColConciliado.Buscador = "false";
        ColConciliado.Alineacion = "left";
        ColConciliado.Ancho = "40";
        GridCuentasPorCobrar.Columnas.Add(ColConciliado);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "150";
        GridCuentasPorCobrar.Columnas.Add(ColRazonSocial);

        //Gestor
        CJQColumn ColGestor = new CJQColumn();
        ColGestor.Nombre = "Gestor";
        ColGestor.Encabezado = "Gestor";
        ColGestor.Buscador = "true";
        ColGestor.Alineacion = "left";
        ColGestor.Ancho = "150";
        GridCuentasPorCobrar.Columnas.Add(ColGestor);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarCuentasPorCobrar == 1 ? "A/I" : "A/IDeshabilitado";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridCuentasPorCobrar.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCuentasPorCobrar";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCuentasPorCobrar.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCuentasPorCobrar", GridCuentasPorCobrar.GeneraGrid(), true);
    }

    public void GenerarGridCuentasPorCobrarConciliar()
    {
        //GridConciliarCuentasPorCobrar
        CJQGrid grdConciliarCuentasPorCobrar = new CJQGrid();
        grdConciliarCuentasPorCobrar.NombreTabla = "grdCuentasPorCobrarConciliar";
        grdConciliarCuentasPorCobrar.CampoIdentificador = "IdCuentasPorCobrar";
        grdConciliarCuentasPorCobrar.ColumnaOrdenacion = "Sucursal,Folio";
        grdConciliarCuentasPorCobrar.TipoOrdenacion = "DESC";
        grdConciliarCuentasPorCobrar.Metodo = "ObtenerCuentasPorCobrarConciliar";
        grdConciliarCuentasPorCobrar.TituloTabla = "Conciliar ingresos";
        grdConciliarCuentasPorCobrar.GenerarGridCargaInicial = false;
        grdConciliarCuentasPorCobrar.GenerarFuncionFiltro = false;
        grdConciliarCuentasPorCobrar.GenerarFuncionTerminado = false;
        grdConciliarCuentasPorCobrar.Altura = 340;
        grdConciliarCuentasPorCobrar.Ancho = 960;
        grdConciliarCuentasPorCobrar.NumeroRegistros = 30;
        grdConciliarCuentasPorCobrar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrar
        CJQColumn ColIdCuentasPorCobrarConciliar = new CJQColumn();
        ColIdCuentasPorCobrarConciliar.Nombre = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrarConciliar.Oculto = "true";
        ColIdCuentasPorCobrarConciliar.Encabezado = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrarConciliar.Buscador = "false";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColIdCuentasPorCobrarConciliar);

        //Sucursal
        CJQColumn ColSucursalConciliar = new CJQColumn();
        ColSucursalConciliar.Nombre = "Sucursal";
        ColSucursalConciliar.Encabezado = "Sucursal";
        ColSucursalConciliar.Ancho = "10";
        ColSucursalConciliar.Buscador = "false";
        ColSucursalConciliar.Alineacion = "left";
        ColSucursalConciliar.Oculto = "true";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColSucursalConciliar);

        //Folio
        CJQColumn ColFolioConciliar = new CJQColumn();
        ColFolioConciliar.Nombre = "FolioConciliar";
        ColFolioConciliar.Encabezado = "Folio";
        ColFolioConciliar.Ancho = "60";
        ColFolioConciliar.Buscador = "true";
        ColFolioConciliar.Alineacion = "left";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColFolioConciliar);

        //Importe
        CJQColumn ColImporteConciliar = new CJQColumn();
        ColImporteConciliar.Nombre = "Importe";
        ColImporteConciliar.Encabezado = "Importe";
        ColImporteConciliar.Buscador = "false";
        ColImporteConciliar.Formato = "FormatoMoneda";
        ColImporteConciliar.Alineacion = "right";
        ColImporteConciliar.Ancho = "80";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColImporteConciliar);

        //Cuenta
        CJQColumn ColCuentaConciliar = new CJQColumn();
        ColCuentaConciliar.Nombre = "CuentaBancaria";
        ColCuentaConciliar.Encabezado = "Cuenta bancaria";
        ColCuentaConciliar.Buscador = "false";
        ColCuentaConciliar.Alineacion = "left";
        ColCuentaConciliar.Ancho = "100";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColCuentaConciliar);

        //Referencia
        CJQColumn ColReferenciaConciliar = new CJQColumn();
        ColReferenciaConciliar.Nombre = "Referencia";
        ColReferenciaConciliar.Encabezado = "Referencia";
        ColReferenciaConciliar.Buscador = "false";
        ColReferenciaConciliar.Alineacion = "left";
        ColReferenciaConciliar.Ancho = "100";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColReferenciaConciliar);

        //TipoMoneda
        CJQColumn ColTipoMonedaConciliar = new CJQColumn();
        ColTipoMonedaConciliar.Nombre = "TipoMoneda";
        ColTipoMonedaConciliar.Encabezado = "Tipo de moneda";
        ColTipoMonedaConciliar.Buscador = "false";
        ColTipoMonedaConciliar.Alineacion = "left";
        ColTipoMonedaConciliar.Ancho = "60";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColTipoMonedaConciliar);

        //FechaEmision
        CJQColumn ColFechaEmisionConciliar = new CJQColumn();
        ColFechaEmisionConciliar.Nombre = "FechaEmision";
        ColFechaEmisionConciliar.Encabezado = "Fecha de captura";
        ColFechaEmisionConciliar.Buscador = "false";
        ColFechaEmisionConciliar.Alineacion = "left";
        ColFechaEmisionConciliar.Ancho = "80";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColFechaEmisionConciliar);

        //FechaAplicacion
        CJQColumn ColFechaAplicacionConciliar = new CJQColumn();
        ColFechaAplicacionConciliar.Nombre = "FechaAplicacion";
        ColFechaAplicacionConciliar.Encabezado = "Fecha acreditado";
        ColFechaAplicacionConciliar.Buscador = "false";
        ColFechaAplicacionConciliar.Alineacion = "left";
        ColFechaAplicacionConciliar.Ancho = "80";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColFechaAplicacionConciliar);

        //Asociado
        CJQColumn ColAsociadoConciliar = new CJQColumn();
        ColAsociadoConciliar.Nombre = "Asociado";
        ColAsociadoConciliar.Encabezado = "Asociado";
        ColAsociadoConciliar.Buscador = "false";
        ColAsociadoConciliar.Alineacion = "left";
        ColAsociadoConciliar.Ancho = "50";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColAsociadoConciliar);

        //Conciliado
        CJQColumn ColConciliadoConciliar = new CJQColumn();
        ColConciliadoConciliar.Nombre = "Conciliado";
        ColConciliadoConciliar.Encabezado = "Conciliado";
        ColConciliadoConciliar.Buscador = "false";
        ColConciliadoConciliar.Alineacion = "left";
        ColConciliadoConciliar.Ancho = "50";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColConciliadoConciliar);

        //RazonSocial
        CJQColumn ColRazonSocialConciliar = new CJQColumn();
        ColRazonSocialConciliar.Nombre = "RazonSocialConciliar";
        ColRazonSocialConciliar.Encabezado = "Razón social";
        ColRazonSocialConciliar.Buscador = "true";
        ColRazonSocialConciliar.Alineacion = "left";
        ColRazonSocialConciliar.Ancho = "250";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColRazonSocialConciliar);

        //Conciliar ingreso
        CJQColumn ColConciliar = new CJQColumn();
        ColConciliar.Nombre = "Conciliar";
        ColConciliar.Encabezado = "Conciliar";
        ColConciliar.Buscador = "false";
        ColConciliar.Alineacion = "center";
        ColConciliar.Funcion = "ConciliarIngreso";
        ColConciliar.Etiquetado = "CheckBox";
        ColConciliar.Id = "chkConciliarIngreso";
        ColConciliar.Estilo = "chkConciliarIngreso";
        ColConciliar.Ancho = "60";
        grdConciliarCuentasPorCobrar.Columnas.Add(ColConciliar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCuentasPorCobrarConciliar", grdConciliarCuentasPorCobrar.GeneraGrid(), true);
    }

    public void GenerarGridCuentasBancarias()
    {
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
    }

    public void GenerarGridFacturas()
    {
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
        ColFolioFactura.Nombre = "NumeroFactura";
        ColFolioFactura.Encabezado = "Folio";
        ColFolioFactura.Buscador = "true";
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
        ColEsParcialidad.Encabezado = "EsParcialidad";
        ColEsParcialidad.Buscador = "false";
        ColEsParcialidad.Ancho = "10";
        GridFacturas.Columnas.Add(ColEsParcialidad);

        ClientScript.RegisterStartupScript(this.GetType(), "grdFacturas", GridFacturas.GeneraGrid(), true);
    }

    public void GenerarGridMovimientosCobros()
    {
        //GridMovimientosCobros
        CJQGrid grdMovimientosCobros = new CJQGrid();
        grdMovimientosCobros.NombreTabla = "grdMovimientosCobros";
        grdMovimientosCobros.CampoIdentificador = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobros.ColumnaOrdenacion = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobros.TipoOrdenacion = "DESC";
        grdMovimientosCobros.Metodo = "ObtenerMovimientosCobros";
        grdMovimientosCobros.TituloTabla = "Documentos asociados";
        grdMovimientosCobros.GenerarFuncionFiltro = false;
        grdMovimientosCobros.GenerarFuncionTerminado = false;
        grdMovimientosCobros.Altura = 120;
        grdMovimientosCobros.NumeroRegistros = 15;
        grdMovimientosCobros.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFactura = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFactura.Nombre = "IdCuentasPorCobrarEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFactura.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFactura.Encabezado = "IdCuentasPorCobrarEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFactura.Buscador = "false";
        grdMovimientosCobros.Columnas.Add(ColIdCuentasPorCobrarEncabezadoFactura);

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
        ColEliminarMovimiento.Oculto = puedeEliminarCobroCuentasPorCobrar == 1 ? "false" : "true";
        ColEliminarMovimiento.Ancho = "25";
        grdMovimientosCobros.Columnas.Add(ColEliminarMovimiento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdMovimientosCobros", grdMovimientosCobros.GeneraGrid(), true);
    }

    public void GenerarGridMovimientosCobrosConsultar()
    {
        //GridMovimientosCobrosConsultar
        CJQGrid grdMovimientosCobrosConsultar = new CJQGrid();
        grdMovimientosCobrosConsultar.NombreTabla = "grdMovimientosCobrosConsultar";
        grdMovimientosCobrosConsultar.CampoIdentificador = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosConsultar.ColumnaOrdenacion = "IdCuentasPorCobrarEncabezadoFactura";
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
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Nombre = "IdCuentasPorCobrarEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaConsultar.Encabezado = "IdCuentasPorCobrarEncabezadoFactura";
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
    }

    public void GenerarGridMovimientosCobrosEditar()
    {
        //GridMovimientosCobrosEditar
        CJQGrid grdMovimientosCobrosEditar = new CJQGrid();
        grdMovimientosCobrosEditar.NombreTabla = "grdMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.CampoIdentificador = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosEditar.ColumnaOrdenacion = "IdCuentasPorCobrarEncabezadoFactura";
        grdMovimientosCobrosEditar.TipoOrdenacion = "DESC";
        grdMovimientosCobrosEditar.Metodo = "ObtenerMovimientosCobrosEditar";
        grdMovimientosCobrosEditar.TituloTabla = "Documentos asociado";
        grdMovimientosCobrosEditar.GenerarGridCargaInicial = false;
        grdMovimientosCobrosEditar.GenerarFuncionFiltro = false;
        grdMovimientosCobrosEditar.GenerarFuncionTerminado = false;
        grdMovimientosCobrosEditar.Altura = 120;
        grdMovimientosCobrosEditar.Ancho = 770;
        grdMovimientosCobrosEditar.NumeroRegistros = 15;
        grdMovimientosCobrosEditar.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrarEncabezadoFactura
        CJQColumn ColIdCuentasPorCobrarEncabezadoFacturaEditar = new CJQColumn();
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Nombre = "IdCuentasPorCobrarEncabezadoFactura";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Oculto = "true";
        ColIdCuentasPorCobrarEncabezadoFacturaEditar.Encabezado = "IdCuentasPorCobrarEncabezadoFactura";
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
    public static CJQGridJsonResponse ObtenerCuentasPorCobrar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pAsociado, string pGestor)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrar", sqlCon);
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
        Stored.Parameters.Add("pAsociado", SqlDbType.VarChar, 250).Value = Convert.ToString(pAsociado);
        Stored.Parameters.Add("pGestor", SqlDbType.VarChar, 250).Value = Convert.ToString(pGestor);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCuentasPorCobrarConciliar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarConciliar", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pRazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("pFolio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
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
    public static CJQGridJsonResponse ObtenerFacturas(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pSerie, string pNumeroFactura, int pIdCliente)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pSerie", SqlDbType.VarChar, 250).Value = Convert.ToString(pSerie);
        Stored.Parameters.Add("pNumeroFactura", SqlDbType.VarChar, 250).Value = pNumeroFactura;
        Stored.Parameters.Add("pIdCliente", SqlDbType.Int).Value = pIdCliente;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCuentasPorCobrar)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentasPorCobrar", SqlDbType.Int).Value = pIdCuentasPorCobrar;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCuentasPorCobrar)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentasPorCobrar", SqlDbType.Int).Value = pIdCuentasPorCobrar;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdCuentasPorCobrar)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentasPorCobrar", SqlDbType.Int).Value = pIdCuentasPorCobrar;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string BuscarGestor(string pGestor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCuentasPorCobrar = new CJson();
        jsonCuentasPorCobrar.StoredProcedure.CommandText = "sp_CuentasPorCobrar_Consultar_Gestor";
        jsonCuentasPorCobrar.StoredProcedure.Parameters.AddWithValue("@pGestor", pGestor);
        string jsonCuentasPorCobrarString = jsonCuentasPorCobrar.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCuentasPorCobrarString;
    }

    [WebMethod]
    public static string BuscarCuentasPorCobrar(string pCuentasPorCobrar)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonCuentasPorCobrar = new CJson();
        jsonCuentasPorCobrar.StoredProcedure.CommandText = "sp_CuentasPorCobrar_Consultar_FiltroPorCuentasPorCobrar";
        jsonCuentasPorCobrar.StoredProcedure.Parameters.AddWithValue("@pCuentasPorCobrar", pCuentasPorCobrar);
        string jsonCuentasPorCobrarString = jsonCuentasPorCobrar.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCuentasPorCobrarString;
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
    public static string AgregarCuentasPorCobrar(Dictionary<string, object> pCuentasPorCobrar)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
            CuentasPorCobrar.IdCuentaBancaria = Convert.ToInt32(pCuentasPorCobrar["IdCuentaBancaria"]);
            CuentasPorCobrar.IdCliente = Convert.ToInt32(pCuentasPorCobrar["IdCliente"]);
            CuentasPorCobrar.IdMetodoPago = Convert.ToInt32(pCuentasPorCobrar["IdMetodoPago"]);
            CuentasPorCobrar.FechaEmision = Convert.ToDateTime(pCuentasPorCobrar["Fecha"]);
            CuentasPorCobrar.Importe = Convert.ToDecimal(pCuentasPorCobrar["Importe"]);
            CuentasPorCobrar.Referencia = Convert.ToString(pCuentasPorCobrar["Referencia"]);
            CuentasPorCobrar.ConceptoGeneral = Convert.ToString(pCuentasPorCobrar["ConceptoGeneral"]);
            CuentasPorCobrar.FechaAplicacion = Convert.ToDateTime(pCuentasPorCobrar["FechaAplicacion"]);
            if (CuentasPorCobrar.Conciliado == true)
            {
                CuentasPorCobrar.FechaConciliacion = Convert.ToDateTime(pCuentasPorCobrar["FechaConciliacion"]);
            }

            CuentasPorCobrar.Conciliado = Convert.ToBoolean(pCuentasPorCobrar["Conciliado"]);
            CuentasPorCobrar.Asociado = Convert.ToBoolean(pCuentasPorCobrar["Asociado"]);
            CuentasPorCobrar.TipoCambio = Convert.ToDecimal(pCuentasPorCobrar["TipoCambio"]);
            CuentasPorCobrar.IdTipoMoneda = Convert.ToInt32(pCuentasPorCobrar["IdTipoMoneda"]);
            CuentasPorCobrar.TipoCambioDOF = Convert.ToDecimal(pCuentasPorCobrar["TipoCambioDOF"]);
            CuentasPorCobrar.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarCuentasPorCobrar(CuentasPorCobrar, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                CuentasPorCobrar.AgregarCuentasPorCobrar(ConexionBaseDatos);

                CCuentasPorCobrarSucursal CuentasPorCobrarSucursal = new CCuentasPorCobrarSucursal();
                CuentasPorCobrarSucursal.IdCuentasPorCobrar = CuentasPorCobrar.IdCuentasPorCobrar;
                CuentasPorCobrarSucursal.IdSucursal = Usuario.IdSucursalActual;
                CuentasPorCobrarSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                CuentasPorCobrarSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                CuentasPorCobrarSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = CuentasPorCobrar.IdCuentasPorCobrar;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("CuentasPorCobrar", ConexionBaseDatos);

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
    public static string AgregarCuentasPorCobrarEdicion(Dictionary<string, object> pCuentasPorCobrar)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
            CuentasPorCobrar.IdCuentaBancaria = Convert.ToInt32(pCuentasPorCobrar["IdCuentaBancaria"]);
            CuentasPorCobrar.IdCliente = Convert.ToInt32(pCuentasPorCobrar["IdCliente"]);
            CuentasPorCobrar.IdMetodoPago = Convert.ToInt32(pCuentasPorCobrar["IdMetodoPago"]);
            CuentasPorCobrar.FechaEmision = Convert.ToDateTime(pCuentasPorCobrar["Fecha"]);
            CuentasPorCobrar.Importe = Convert.ToDecimal(pCuentasPorCobrar["Importe"]);
            CuentasPorCobrar.Referencia = Convert.ToString(pCuentasPorCobrar["Referencia"]);
            CuentasPorCobrar.ConceptoGeneral = Convert.ToString(pCuentasPorCobrar["ConceptoGeneral"]);
            CuentasPorCobrar.FechaAplicacion = Convert.ToDateTime(pCuentasPorCobrar["FechaAplicacion"]);
            CuentasPorCobrar.Conciliado = Convert.ToBoolean(pCuentasPorCobrar["Conciliado"]);
            CuentasPorCobrar.Asociado = Convert.ToBoolean(pCuentasPorCobrar["Asociado"]);
            CuentasPorCobrar.TipoCambio = Convert.ToDecimal(pCuentasPorCobrar["TipoCambio"]);
            CuentasPorCobrar.IdTipoMoneda = Convert.ToInt32(pCuentasPorCobrar["IdTipoMoneda"]);
            CuentasPorCobrar.TipoCambioDOF = Convert.ToDecimal(pCuentasPorCobrar["TipoCambioDOF"]);
            CuentasPorCobrar.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            string validacion = ValidarCuentasPorCobrar(CuentasPorCobrar, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                CuentasPorCobrar.AgregarCuentasPorCobrar(ConexionBaseDatos);

                CCuentasPorCobrarSucursal CuentasPorCobrarSucursal = new CCuentasPorCobrarSucursal();
                CuentasPorCobrarSucursal.IdCuentasPorCobrar = CuentasPorCobrar.IdCuentasPorCobrar;
                CuentasPorCobrarSucursal.IdSucursal = Usuario.IdSucursalActual;
                CuentasPorCobrarSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                CuentasPorCobrarSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                CuentasPorCobrarSucursal.Agregar(ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = CuentasPorCobrar.IdCuentasPorCobrar;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto un ingreso";
                HistorialGenerico.AgregarHistorialGenerico("CuentasPorCobrar", ConexionBaseDatos);

                CuentasPorCobrar.LlenaObjeto(CuentasPorCobrar.IdCuentasPorCobrar, ConexionBaseDatos);
                oRespuesta.Add("IdCuentasPorCobrar", CuentasPorCobrar.IdCuentasPorCobrar);
                oRespuesta.Add("Folio", CuentasPorCobrar.Folio);
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
    public static string ObtenerFormaCuentasPorCobrar(int pIdCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentasPorCobrar = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentasPorCobrar" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentasPorCobrar = 1;
        }
        oPermisos.Add("puedeEditarCuentasPorCobrar", puedeEditarCuentasPorCobrar);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, pIdCuentasPorCobrar, ConexionBaseDatos);
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
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> pCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCuentasPorCobrar = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCuentasPorCobrar" }, ConexionBaseDatos) == "")
        {
            puedeEditarCuentasPorCobrar = 1;
        }
        oPermisos.Add("puedeEditarCuentasPorCobrar", puedeEditarCuentasPorCobrar);

        if (respuesta == "Conexion Establecida")
        {
            CCuentasPorCobrar CuentasPorCobrarCliente = new CCuentasPorCobrar();
            CuentasPorCobrarCliente.LlenaObjeto(Convert.ToInt32(pCuentasPorCobrar["pIdCuentasPorCobrar"]), ConexionBaseDatos);
            CuentasPorCobrarCliente.IdCliente = Convert.ToInt32(pCuentasPorCobrar["pIdCliente"]);
            CuentasPorCobrarCliente.TipoCambio = Convert.ToDecimal(pCuentasPorCobrar["TipoCambio"]);
            CuentasPorCobrarCliente.TipoCambioDOF = Convert.ToDecimal(pCuentasPorCobrar["TipoCambioDOF"]);
            CuentasPorCobrarCliente.EditarCuentasPorCobrar(ConexionBaseDatos);
            JObject Modelo = new JObject();
            Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, Convert.ToInt32(pCuentasPorCobrar["pIdCuentasPorCobrar"]), ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarCuentasPorCobrar()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        int puedeEditarTipoCambioIngresos = 0;
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioIngresos = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);


        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CMetodoPago MetodoPago = new CMetodoPago();
            Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(ConexionBaseDatos, 1));
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
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexion a Base de Datos"));
        }
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaConciliarCuentasPorCobrar()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
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

        int puedeEditarTipoCambioIngresos = 0;
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCambioIngresos = 1;
        }
        oPermisos.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);

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
    public static string ObtenerHabilitaMonto(int pIdCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        int puedeEditarTipoCambioIngresos = 0;
        puedeEditarTipoCambioIngresos = Usuario.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, ConexionBaseDatos) == "" ? 1 : 0;

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, pIdCuentasPorCobrar, ConexionBaseDatos);
            Modelo.Add(new JProperty("Permisos", oPermisos));
            Modelo.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);
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
    public static string ObtenerFormaEditarCuentasPorCobrar(int IdCuentasPorCobrar)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();

                int puedeEditarCuentasPorCobrar = UsuarioSesion.TienePermisos(new string[] { "puedeEditarCuentasPorCobrar" }, pConexion) == "" ? 1 : 0;
                int puedeEditarTipoCambioIngresos = UsuarioSesion.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, pConexion) == "" ? 1 : 0;
                int puedeEditarIngresosContabilidad = UsuarioSesion.TienePermisos(new string[] { "PuedeEditarIngresosContabilidad" }, pConexion) == "" ? 1 : 0;
                JObject oPermisos = new JObject();
                CUsuario Usuario = new CUsuario();

                oPermisos.Add("puedeEditarCuentasPorCobrar", puedeEditarCuentasPorCobrar);
                oPermisos.Add("PuedeEditarIngresosContabilidad", puedeEditarIngresosContabilidad);

                CCuentasPorCobrar Ingreso = new CCuentasPorCobrar();
                Ingreso.LlenaObjeto(IdCuentasPorCobrar, pConexion);

                Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, Ingreso.IdCuentasPorCobrar, pConexion);
                Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(Ingreso.IdTipoMoneda, pConexion));
                Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPagoBaja(Ingreso.IdMetodoPago, pConexion));
                Modelo.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);
                Modelo.Add("Permisos", oPermisos);
                Modelo.Add("PuedeEditarIngresosContabilidad", puedeEditarIngresosContabilidad);

                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaFiltroCuentasPorCobrar()
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
    public static string EditarCuentasPorCobrar(Dictionary<string, object> pCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
        CuentasPorCobrar.LlenaObjeto(Convert.ToInt32(pCuentasPorCobrar["IdCuentasPorCobrar"]), ConexionBaseDatos);
        CuentasPorCobrar.IdCuentasPorCobrar = Convert.ToInt32(pCuentasPorCobrar["IdCuentasPorCobrar"]);
        CuentasPorCobrar.IdCliente = Convert.ToInt32(pCuentasPorCobrar["IdCliente"]);
        CuentasPorCobrar.Referencia = Convert.ToString(pCuentasPorCobrar["Referencia"]);
        CuentasPorCobrar.ConceptoGeneral = Convert.ToString(pCuentasPorCobrar["ConceptoGeneral"]);
        CuentasPorCobrar.Conciliado = Convert.ToBoolean(pCuentasPorCobrar["Conciliado"]);
        CuentasPorCobrar.Asociado = Convert.ToBoolean(pCuentasPorCobrar["Asociado"]);
        CuentasPorCobrar.Importe = Convert.ToDecimal(pCuentasPorCobrar["Importe"]);
        CuentasPorCobrar.TipoCambio = Convert.ToDecimal(pCuentasPorCobrar["TipoCambio"]);
        CuentasPorCobrar.TipoCambioDOF = Convert.ToDecimal(pCuentasPorCobrar["TipoCambioDOF"]);
        CuentasPorCobrar.FechaEmision = Convert.ToDateTime(pCuentasPorCobrar["Fecha"]);
        CuentasPorCobrar.IdMetodoPago = Convert.ToInt32(pCuentasPorCobrar["IdMetodoPago"]);
        CCuentaBancaria Cuenta = new CCuentaBancaria();
        Dictionary<string, object> pParametros = new Dictionary<string, object>();
        pParametros.Add("CuentaBancaria", Convert.ToString(pCuentasPorCobrar["CuentaBancaria"]));
        Cuenta.LlenaObjetoFiltros(pParametros, ConexionBaseDatos);
        CuentasPorCobrar.IdCuentaBancaria = Cuenta.IdCuentaBancaria;
        CuentasPorCobrar.FechaAplicacion = Convert.ToDateTime(pCuentasPorCobrar["FechaAplicacion"]);

        if (CuentasPorCobrar.Conciliado == true && Convert.ToString(pCuentasPorCobrar["FechaConciliacion"]) != "-")
        {
            CuentasPorCobrar.FechaConciliacion = Convert.ToDateTime(pCuentasPorCobrar["FechaConciliacion"]);
        }

        string validacion = ValidarCuentasPorCobrar(CuentasPorCobrar, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CuentasPorCobrar.Editar(ConexionBaseDatos);
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
    public static string ConciliarIngreso(Dictionary<string, object> pCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
        CuentasPorCobrar.LlenaObjeto(Convert.ToInt32(pCuentasPorCobrar["pIdCuentasPorCobrar"]), ConexionBaseDatos);
        CuentasPorCobrar.Conciliado = true;
        string validacion = "";

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CuentasPorCobrar.Editar(ConexionBaseDatos);
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
    public static string EditarMontos(Dictionary<string, object> pCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();

        CUtilerias Utilerias = new CUtilerias();
        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pCuentasPorCobrar["IdEncabezadoFactura"]), ConexionBaseDatos);

        CCuentasPorCobrarEncabezadoFactura CuentasPorCobrarEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
        CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar = Convert.ToInt32(pCuentasPorCobrar["IdCuentasPorCobrar"]);
        CuentasPorCobrarEncabezadoFactura.IdEncabezadoFactura = Convert.ToInt32(pCuentasPorCobrar["IdEncabezadoFactura"]);
        CuentasPorCobrarEncabezadoFactura.Monto = Convert.ToDecimal(pCuentasPorCobrar["Monto"]);
        CuentasPorCobrarEncabezadoFactura.FechaPago = Convert.ToDateTime(DateTime.Now);
        CuentasPorCobrarEncabezadoFactura.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        CuentasPorCobrarEncabezadoFactura.IdTipoMoneda = Convert.ToInt32(pCuentasPorCobrar["IdTipoMoneda"]);
        CuentasPorCobrarEncabezadoFactura.TipoCambio = Convert.ToDecimal(pCuentasPorCobrar["TipoCambio"]);
        CuentasPorCobrarEncabezadoFactura.Nota = "pago de la factura";

        string validacion = ValidarMontos(CuentasPorCobrarEncabezadoFactura, FacturaEncabezado, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CuentasPorCobrarEncabezadoFactura.AgregarCuentasPorCobrarEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("Monto", Convert.ToDecimal(pCuentasPorCobrar["Monto"]));
            oRespuesta.Add("rowid", Convert.ToDecimal(pCuentasPorCobrar["rowid"]));
            oRespuesta.Add("TipoMoneda", Convert.ToString(pCuentasPorCobrar["TipoMoneda"]));
            oRespuesta.Add("AbonosCuentasPorCobrar", CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(Convert.ToInt32(pCuentasPorCobrar["IdCuentasPorCobrar"]), ConexionBaseDatos));

            if (Convert.ToInt32(pCuentasPorCobrar["EsParcialidad"]) == 1)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

                int NumeroParcialidadActual = 0;
                NumeroParcialidadActual = (FacturaEncabezado.NumeroParcialidades - FacturaEncabezado.NumeroParcialidadesPendientes) + 1;
                string Descripcion = "";
                Descripcion = "Parcialidad " + NumeroParcialidadActual + " de " + FacturaEncabezado.NumeroParcialidades;
                FacturaEncabezado.AgregarFacturaIndividual(ConexionBaseDatos, Descripcion, Convert.ToDecimal(pCuentasPorCobrar["Monto"]));

                FacturaEncabezado.SaldoFactura -= Convert.ToDecimal(pCuentasPorCobrar["Monto"]);


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

            CFacturaDetalle Detalle = new CFacturaDetalle();
            Dictionary<string, object> pParametros = new Dictionary<string, object>();
            pParametros.Add("Baja", 0);
            pParametros.Add("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado);

            foreach (CFacturaDetalle oDetalle in Detalle.LlenaObjetosFiltros(pParametros, ConexionBaseDatos))
            {
                if (oDetalle.IdProyecto != 0)
                {
                    CProyecto.ActualizarTotales(oDetalle.IdProyecto, ConexionBaseDatos);
                }
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
            Encoding ANSI = Encoding.GetEncoding(1252);
            System.IO.StreamWriter file = new System.IO.StreamWriter(RutaCFDI.RutaCFDI + "\\in\\" + NombreArchivo + ".txt", false, ANSI);
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

    [WebMethod]
    public static string EliminarCuentasPorCobrarEncabezadoFactura(Dictionary<string, object> pCuentasPorCobrarEncabezadoFactura)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject Modelo = new JObject();
        int IdFacturaEncabezadoParcial = 0;

        CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
        CConfiguracion Configuracion = new CConfiguracion();
        Configuracion.LlenaObjeto(1, ConexionBaseDatos);

        CCuentasPorCobrarEncabezadoFactura CuentasPorCobrarEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
        CuentasPorCobrarEncabezadoFactura.LlenaObjeto(Convert.ToInt32(pCuentasPorCobrarEncabezadoFactura["pIdCuentasPorCobrarEncabezadoFactura"]), ConexionBaseDatos);
        FacturaEncabezado.LlenaObjeto(CuentasPorCobrarEncabezadoFactura.IdEncabezadoFactura, ConexionBaseDatos);

        JObject oRespuesta = new JObject();

        if (FacturaEncabezado.Parcialidades == true)
        {
            IdFacturaEncabezadoParcial = CuentasPorCobrarEncabezadoFactura.ValidaEliminarCuentasPorCobrarDetalle(Convert.ToInt32(FacturaEncabezado.IdFacturaEncabezado), ConexionBaseDatos);
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
                        CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrarEncabezadoFactura = Convert.ToInt32(pCuentasPorCobrarEncabezadoFactura["pIdCuentasPorCobrarEncabezadoFactura"]);
                        CuentasPorCobrarEncabezadoFactura.Baja = true;
                        CuentasPorCobrarEncabezadoFactura.EliminarCuentasPorCobrarEncabezadoFactura(ConexionBaseDatos);
                        oRespuesta.Add("AbonosCuentasPorCobrar", CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar, ConexionBaseDatos));
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
            CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrarEncabezadoFactura = Convert.ToInt32(pCuentasPorCobrarEncabezadoFactura["pIdCuentasPorCobrarEncabezadoFactura"]);
            CuentasPorCobrarEncabezadoFactura.Baja = true;
            CuentasPorCobrarEncabezadoFactura.EliminarCuentasPorCobrarEncabezadoFactura(ConexionBaseDatos);
            oRespuesta.Add("EsParcialidad", 0);
            oRespuesta.Add("AbonosCuentasPorCobrar", CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar, ConexionBaseDatos));
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
    public static string EditarCuentasPorCobrarCliente(Dictionary<string, object> pCuentasPorCobrar)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
        CuentasPorCobrar.LlenaObjeto(Convert.ToInt32(pCuentasPorCobrar["IdCuentasPorCobrar"]), ConexionBaseDatos);
        CuentasPorCobrar.IdCuentasPorCobrar = Convert.ToInt32(pCuentasPorCobrar["IdCuentasPorCobrar"]);
        CuentasPorCobrar.IdCliente = Convert.ToInt32(pCuentasPorCobrar["IdCliente"]);
        string validacion = ValidarCuentasPorCobrar(CuentasPorCobrar, ConexionBaseDatos);
        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CuentasPorCobrar.EditarCuentasPorCobrar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCuentasPorCobrar, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
            CuentasPorCobrar.IdCuentasPorCobrar = pIdCuentasPorCobrar;
            CuentasPorCobrar.Baja = pBaja;

            if (pBaja == true)
            {
                if (CuentasPorCobrar.ExisteCuentasPorCobrarMovimientos(Convert.ToInt32(pIdCuentasPorCobrar), ConexionBaseDatos) == 0)
                {
                    CuentasPorCobrar.Eliminar(ConexionBaseDatos);
                    CuentasPorCobrar.LlenaObjeto(pIdCuentasPorCobrar, ConexionBaseDatos);
                    CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
                    CuentaBancaria.LlenaObjeto(CuentasPorCobrar.IdCuentaBancaria, ConexionBaseDatos);
                    CuentaBancaria.Saldo = CuentaBancaria.Saldo - CuentasPorCobrar.Importe;
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
                CuentasPorCobrar.Eliminar(ConexionBaseDatos);
                CuentasPorCobrar.LlenaObjeto(pIdCuentasPorCobrar, ConexionBaseDatos);
                CCuentaBancaria CuentaBancaria = new CCuentaBancaria();
                CuentaBancaria.LlenaObjeto(CuentasPorCobrar.IdCuentaBancaria, ConexionBaseDatos);
                CuentaBancaria.Saldo = CuentaBancaria.Saldo + CuentasPorCobrar.Importe;
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
    private static string ValidarCuentasPorCobrar(CCuentasPorCobrar pCuentasPorCobrar, CConexion pConexion)
    {
        string errores = "";

        if (pCuentasPorCobrar.IdCuentaBancaria == 0)
        { errores = errores + "<span>*</span> La cuenta bancaria no esta seleccionada, favor de elegirla de la lista.<br />"; }

        if (pCuentasPorCobrar.IdMetodoPago == 0)
        { errores = errores + "<span>*</span> El campo metodo de pago esta vacio, favor de seleccionarlo.<br />"; }

        if (pCuentasPorCobrar.Importe == 0)
        { errores = errores + "<span>*</span> El campo importe esta vacio, favor de introducirlo.<br />"; }

        if (pCuentasPorCobrar.Referencia == "")
        { errores = errores + "<span>*</span> El campo referencia esta vacio, favor de introducirlo.<br />"; }

        if (pCuentasPorCobrar.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacio, favor de seleccionarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarMontos(CCuentasPorCobrarEncabezadoFactura CuentasPorCobrar, CFacturaEncabezado FacturaEncabezado, CConexion pConexion)
    {
        string errores = "";

        if (FacturaEncabezado.Parcialidades == true)
        {
            if (FacturaEncabezado.NumeroParcialidadesPendientes == 1)
            {
                if (Convert.ToDecimal(CuentasPorCobrar.Monto) != Convert.ToDecimal(FacturaEncabezado.SaldoFactura))
                {
                    errores = errores + "<span>*</span> Es la ultima parcialidad, favor de pagar todo el saldo<br />";
                }
            }
        }

        if (CuentasPorCobrar.IdEncabezadoFactura == 0)
        { errores = errores + "<span>*</span> No hay factura seleccionada, favor de elegir alguna.<br />"; }

        if (CuentasPorCobrar.IdCuentasPorCobrar == 0)
        { errores = errores + "<span>*</span> No hay cuenta por cobrar seleccionada, favor de elegir alguna.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    ////////////////////////  Nueva forma de guardar Complementos de Pagos /////////////////////////////////////

    /* Timbrar */
    [WebMethod]
    public static string ObtenerDatosTimbradoPago(int IdEncabezadoFactura, int IdCuentasPorCobrar, string EsParcialidad, int Monto, float Saldo, int IdTipoMoneda, int TipoCambio)
    { 
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Comprobante = new JObject();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                // Llenado de clases necesarias para la creación del comprobante
                CFacturaEncabezado FacturaPadre = new CFacturaEncabezado();
                FacturaPadre.LlenaObjeto(IdEncabezadoFactura, pConexion);

                int NumeroParcialidadActual = 0;
                NumeroParcialidadActual = (FacturaPadre.NumeroParcialidades - FacturaPadre.NumeroParcialidadesPendientes) + 1;
                string Descripcion = "PGO";

                CFacturaEncabezadoSucursal FacturaSucural = new CFacturaEncabezadoSucursal();
                pParametros.Clear();
                pParametros.Add("IdFacturaEncabezado", FacturaPadre.IdFacturaEncabezado);
                FacturaSucural.LlenaObjetoFiltros(pParametros, pConexion);

                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(FacturaSucural.IdSucursal, pConexion);

                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(Sucursal.IdEmpresa, pConexion);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(FacturaPadre.IdCliente, pConexion);

                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                CSerieFactura SerieFactura = new CSerieFactura();
                SerieFactura.LlenaObjeto(FacturaPadre.IdSerieFactura, pConexion);

                CTxtTimbradosFactura TimbradoPadre = new CTxtTimbradosFactura();
                pParametros.Clear();
                pParametros.Add("Refid", FacturaPadre.Refid);
                TimbradoPadre.LlenaObjetoFiltros(pParametros, pConexion);

                CCuentasPorCobrar cuentasPorCobrar = new CCuentasPorCobrar();
                cuentasPorCobrar.LlenaObjeto(IdCuentasPorCobrar, pConexion);

                CMetodoPago FormaPago = new CMetodoPago();
                FormaPago.LlenaObjeto(cuentasPorCobrar.IdMetodoPago, pConexion);

                CCuentasPorCobrarEncabezadoFactura Pago = new CCuentasPorCobrarEncabezadoFactura();
                Pago.IdCuentasPorCobrar = IdCuentasPorCobrar;
                Pago.IdEncabezadoFactura = IdEncabezadoFactura;
                Pago.Monto = Monto;
                Pago.TipoCambio = TipoCambio;
                Pago.Nota = "pago de factura";
                Pago.IdUsuario = UsuarioSesion.IdUsuario;
                Pago.FechaPago = DateTime.Now;
                Pago.IdTipoMoneda = IdTipoMoneda;
                Pago.Baja = true;
                Pago.Agregar(pConexion);

                // datos del comprobante
                Comprobante.Add("Serie", Descripcion);
                Comprobante.Add("Folio", cuentasPorCobrar.Folio);
                Comprobante.Add("Fecha", cuentasPorCobrar.FechaEmision);
                Comprobante.Add("LugarExpedicion", Empresa.CodigoPostal); // Catalogo SAT
                Comprobante.Add("Moneda", "XXX"); // Catalogo SAT
                Comprobante.Add("TipoDeComprobante", "P"); // Catalogo SAT
                Comprobante.Add("SubTotal", "0");
                Comprobante.Add("Total", "0");
                Comprobante.Add("NoCertificado", "20001000000300022755"); // NoCertificado Example // Sucursal.NoCertificado);
                Comprobante.Add("Certificado", ""); // Llenado por SAT
                Comprobante.Add("Sello", ""); // Llenado por SAT

                // datos del emisor
                JObject Emisor = new JObject();
                Emisor.Add("Nombre", ClearString(Empresa.RazonSocial));
                Emisor.Add("RFC", "MAG041126GT8"); // RFC example // Empresa.RFC); 
                Emisor.Add("RegimenFiscal", "601"); // Catalogo SAT

                Comprobante.Add("Emisor", Emisor);

                // datos del receptor
                JObject Receptor = new JObject();
                Receptor.Add("Nombre", ClearString(Organizacion.RazonSocial));
                Receptor.Add("RFC", Organizacion.RFC);
                Receptor.Add("UsoCFDI", "P01"); // Catalogo SAT

                Comprobante.Add("Receptor", Receptor);

                JObject Concepto = new JObject();

                // Valores default para pagos
                Concepto.Add("ClaveProdServ", "84111506"); // Catalogo SAT
                Concepto.Add("Cantidad", "1");
                Concepto.Add("ClaveUnidad", "ACT"); // Catalogo SAT
                Concepto.Add("Descripcion", "Pago");
                Concepto.Add("ValorUnitario", "0");
                Concepto.Add("Importe", "0");

                Comprobante.Add("Concepto", Concepto);

                // Llenado de complementos de la factura
                JObject Complemento = new JObject();

                JObject DoctoRelacionadoContenido = new JObject();

                DoctoRelacionadoContenido.Add("IdDocumento", TimbradoPadre.Uuid);
                DoctoRelacionadoContenido.Add("Serie", SerieFactura.SerieFactura);
                DoctoRelacionadoContenido.Add("Folio", FacturaPadre.NumeroFactura);
                DoctoRelacionadoContenido.Add("MonedaDR", (FacturaPadre.IdTipoMoneda == 1) ? "MXN" : "USD"); // Catalogo SAT

                string tipoCambioDR = "";
                if (FacturaPadre.IdTipoMoneda == 1)
                {
                    if (FacturaPadre.IdTipoMoneda != IdTipoMoneda)
                    {
                        tipoCambioDR = "1";
                    }
                }
                else if (FacturaPadre.IdTipoMoneda != IdTipoMoneda)
                {
                    tipoCambioDR = Convert.ToString(TipoCambio);
                }

                DoctoRelacionadoContenido.Add("TipoCambioDR", tipoCambioDR);
                DoctoRelacionadoContenido.Add("MetodoDePagoDR", "PPD"); // Catalogo SAT
                DoctoRelacionadoContenido.Add("NumParcialidad", NumeroParcialidadActual);
                DoctoRelacionadoContenido.Add("ImpSaldoAnt", FacturaPadre.SaldoFactura);
                DoctoRelacionadoContenido.Add("ImpPagado", Monto);
                DoctoRelacionadoContenido.Add("ImpSaldoInsoluto", FacturaPadre.SaldoFactura - Monto);

                Complemento.Add("FechaPago",cuentasPorCobrar.FechaAplicacion);
                Complemento.Add("FormaDePagoP",FormaPago.Clave); // Catalogo SAT
                Complemento.Add("MonedaP", (IdTipoMoneda == 1) ? "MXN" : "USD"); // Catalogo SAT
                Complemento.Add("TipoCambioP", (TipoCambio == 1) ? "" : Convert.ToString(TipoCambio));
                Complemento.Add("Monto", Monto);
                Complemento.Add("DoctoRelacionado", DoctoRelacionadoContenido);
                
                Comprobante.Add("Complemento",Complemento);
                
                string Correos = "";

                Correos = "fespino@grupoasercom.com,mferna.92@gmail.com";
                
                // Terminado de datos de comprobate
                Respuesta.Add("Id", 94327); // Id example // Empresa.IdToken);
                Respuesta.Add("Token", "$2b$12$pj0NTsT/brybD2cJrNa8iuRRE5KoxeEFHcm/yJooiSbiAdbiTGzIq"); // Token example // Empresa.Token);
                Respuesta.Add("Comprobante", Comprobante);
                Respuesta.Add("RFC", "MAG041126GT8"); // RFC example // Empresa.RFC); 
                Respuesta.Add("RefID", Pago.IdCuentasPorCobrarEncabezadoFactura);
                Respuesta.Add("NoCertificado", "20001000000300022755"); // NoCertificado example  // Sucursal.NoCertificado);
                Respuesta.Add("Formato", "pdf"); // xml, pdf, zip
                Respuesta.Add("Correos", Correos);

                //Datos para actualizar facturaencabezado y cuentasporcobrarencabezado
                JObject ActualizarMontos = new JObject();
                ActualizarMontos.Add("IdFacturaEncabezado", IdEncabezadoFactura);
                ActualizarMontos.Add("IdCuentasPorCobrar", IdCuentasPorCobrar);
                ActualizarMontos.Add("EsParcialidad", EsParcialidad);
                ActualizarMontos.Add("Monto", Monto);
                ActualizarMontos.Add("Saldo", Saldo);
                ActualizarMontos.Add("IdTipoMoneda", IdTipoMoneda);
                ActualizarMontos.Add("TipoCambio", TipoCambio);

                Respuesta.Add("ActualizarMontos", ActualizarMontos);
                
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string GuardarTimbradoPago(string UUId, int RefId, string Contenido, string RFC, string Serie, string Folio, Dictionary<string, object> ActualizarMontos)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                bool correcto = false;
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                CTxtTimbradosPagos ValidarTimbrado = new CTxtTimbradosPagos();
                pParametros.Clear();
                pParametros.Add("UUId", UUId);

                if (ValidarTimbrado.LlenaObjetosFiltros(pParametros, pConexion).Count == 0)
                {
                    correcto = true;
                }
                else
                {
                    correcto = false;
                    Error = 1;
                    DescripcionError = "Ya existe el documento a timbrar";
                }

                CCuentasPorCobrarEncabezadoFactura CuentasPorCobrarEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
                CuentasPorCobrarEncabezadoFactura.LlenaObjeto(RefId, pConexion);
                CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar = Convert.ToInt32(ActualizarMontos["IdCuentasPorCobrar"]);
                CuentasPorCobrarEncabezadoFactura.IdEncabezadoFactura = Convert.ToInt32(ActualizarMontos["IdFacturaEncabezado"]);
                CuentasPorCobrarEncabezadoFactura.Monto = Convert.ToDecimal(ActualizarMontos["Monto"]);
                CuentasPorCobrarEncabezadoFactura.FechaPago = Convert.ToDateTime(DateTime.Now);
                CuentasPorCobrarEncabezadoFactura.IdUsuario = UsuarioSesion.IdUsuario;
                CuentasPorCobrarEncabezadoFactura.IdTipoMoneda = Convert.ToInt32(ActualizarMontos["IdTipoMoneda"]);
                CuentasPorCobrarEncabezadoFactura.TipoCambio = Convert.ToDecimal(ActualizarMontos["TipoCambio"]);
                CuentasPorCobrarEncabezadoFactura.Nota = "pago de la factura";

                CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                FacturaEncabezado.LlenaObjeto(Convert.ToInt32(ActualizarMontos["IdFacturaEncabezado"]), pConexion);

                string validacion = ValidarMontos(CuentasPorCobrarEncabezadoFactura, FacturaEncabezado, pConexion);

                if (validacion == "")
                {
                    correcto = true;
                }
                else
                {
                    correcto = false;
                    Error = 1;
                    DescripcionError = validacion;
                }

                if (correcto)
                {
                    CuentasPorCobrarEncabezadoFactura.Baja = false;
                    CuentasPorCobrarEncabezadoFactura.Editar(pConexion);
                    CTxtTimbradosPagos Pago = new CTxtTimbradosPagos();
                    Pago.Uuid = UUId;
                    Pago.RefId = RefId.ToString();
                    Pago.Serie = Serie;
                    Pago.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    Pago.FechaTimbrado = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    Pago.Folio = Folio;

                    Pago.Agregar(pConexion);
                    System.IO.Directory.CreateDirectory(@"C:\inetpub\wwwroot\WebServiceDiverza\PDF\" + RFC);
                    System.IO.File.WriteAllBytes(@"C:\inetpub\wwwroot\WebServiceDiverza\PDF\" + RFC + @"\" + RefId + ".pdf", Decode(Contenido));

                    CUtilerias Utilerias = new CUtilerias();
                    
                    if (Convert.ToInt32(ActualizarMontos["EsParcialidad"]) == 1)
                    {
                        CFacturaEncabezado FacturaEncabezadoGlobal = new CFacturaEncabezado();
                        FacturaEncabezadoGlobal.LlenaObjeto(Convert.ToInt32(ActualizarMontos["IdFacturaEncabezado"]), pConexion);
                        FacturaEncabezadoGlobal.NumeroParcialidadesPendientes = FacturaEncabezadoGlobal.NumeroParcialidadesPendientes - 1;
                        FacturaEncabezadoGlobal.SaldoFactura -= Convert.ToDecimal(ActualizarMontos["Monto"]);
                        FacturaEncabezadoGlobal.Editar(pConexion);

                        Respuesta.Add("EsParcialidad", 1);
                    }
                    else
                    {
                        Respuesta.Add("EsParcialidad", 0);
                        Error = 0;
                    }

                    CFacturaDetalle Detalle = new CFacturaDetalle();
                    pParametros.Clear();
                    pParametros.Add("Baja", 0);
                    pParametros.Add("IdFacturaEncabezado", FacturaEncabezado.IdFacturaEncabezado);

                    foreach (CFacturaDetalle oDetalle in Detalle.LlenaObjetosFiltros(pParametros, pConexion))
                    {
                        if (oDetalle.IdProyecto != 0)
                        {
                            CProyecto.ActualizarTotales(oDetalle.IdProyecto, pConexion);
                        }
                    }
                }

                Respuesta.Add("AbonosCuentasPorCobrar", CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(Convert.ToInt32(ActualizarMontos["IdCuentasPorCobrar"]), pConexion));

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    private static void ActualizarSaldos(int IdCuentasPorCobrarEncabezadoFactura, CConexion pConexion)
    {
        CCuentasPorCobrarEncabezadoFactura Pago = new CCuentasPorCobrarEncabezadoFactura();
        Pago.LlenaObjeto(IdCuentasPorCobrarEncabezadoFactura, pConexion);

        CCuentasPorCobrar Ingreso = new CCuentasPorCobrar();
        Ingreso.LlenaObjeto(Pago.IdCuentasPorCobrar, pConexion);

        CFacturaEncabezado Factura = new CFacturaEncabezado();
        Factura.LlenaObjeto(Pago.IdEncabezadoFactura, pConexion);

        //Ingreso

    }

    /* Funciones para nuevo Timbrado */
    private static byte[] Decode(string Hash)
    {
        byte[] bytes = System.Convert.FromBase64String(Hash);
        return bytes;// System.Text.Encoding.UTF8.GetString(bytes);
    }

    private static string ClearString(string data)
    {
        string d = data.Replace("\"", "&quot;");
        d = d.Replace("“", "&quot;");
        d = d.Replace("&", "&amp;");
        return d;
    }
}