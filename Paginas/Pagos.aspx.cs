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

public partial class Pagos : System.Web.UI.Page
{
    public static int puedeAgregarPagos = 0;
    public static int puedeEditarPagos = 0;
    public static int puedeEliminarPagos = 0;
    public static int puedeEliminarCobroCuentasPorCobrar = 0;
    public static int puedeConciliarPagos = 0;
    public static string ticks = "";

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

        puedeAgregarPagos = Usuario.TienePermisos(new string[] { "puedeAgregarPagos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEditarPagos = Usuario.TienePermisos(new string[] { "puedeEditarPagos" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarPagos = Usuario.TienePermisos(new string[] { "puedeEliminarCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeEliminarCobroCuentasPorCobrar = Usuario.TienePermisos(new string[] { "puedeEliminarCobroCuentasPorCobrar" }, ConexionBaseDatos) == "" ? 1 : 0;
        puedeConciliarPagos = Usuario.TienePermisos(new string[] { "puedeConciliarPagos" }, ConexionBaseDatos) == "" ? 1 : 0;

        GenerarGridPagos();
        GenerarGridPagosConciliar();
        GenerarGridCuentasBancarias();
        GenerarGridFacturas();
        GenerarGridMovimientosCobros();
        GenerarGridMovimientosCobrosConsultar();
        GenerarGridMovimientosCobrosEditar();

        ticks = DateTime.Now.Ticks.ToString();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    public void GenerarGridPagos()
    {
        //GridPagos
        CJQGrid GridPagos = new CJQGrid();
        GridPagos.NombreTabla = "grdPagos";
        GridPagos.CampoIdentificador = "IdCuentasPorCobrar";
        GridPagos.ColumnaOrdenacion = "Sucursal,Folio";
        GridPagos.Metodo = "ObtenerPagos";
        GridPagos.TituloTabla = "Ingresos";
        GridPagos.ColumnaOrdenacion = "Folio";
        GridPagos.TipoOrdenacion = "DESC";
        GridPagos.GenerarFuncionFiltro = false;

        //IdCuentasPorCobrar
        CJQColumn ColIdCuentasPorCobrar = new CJQColumn();
        ColIdCuentasPorCobrar.Nombre = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrar.Oculto = "true";
        ColIdCuentasPorCobrar.Encabezado = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrar.Buscador = "false";
        GridPagos.Columnas.Add(ColIdCuentasPorCobrar);

        //Sucursal
        CJQColumn ColSucursal = new CJQColumn();
        ColSucursal.Nombre = "Sucursal";
        ColSucursal.Encabezado = "Sucursal";
        ColSucursal.Ancho = "10";
        ColSucursal.Buscador = "false";
        ColSucursal.Alineacion = "left";
        ColSucursal.Oculto = "true";
        GridPagos.Columnas.Add(ColSucursal);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Ancho = "60";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        GridPagos.Columnas.Add(ColFolio);

        //Importe
        CJQColumn ColImporte = new CJQColumn();
        ColImporte.Nombre = "Importe";
        ColImporte.Encabezado = "Importe";
        ColImporte.Buscador = "false";
        ColImporte.Formato = "FormatoMoneda";
        ColImporte.Alineacion = "right";
        ColImporte.Ancho = "70";
        GridPagos.Columnas.Add(ColImporte);

        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "Saldo";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Buscador = "false";
        ColSaldo.Formato = "FormatoMoneda";
        ColSaldo.Alineacion = "right";
        ColSaldo.Ancho = "70";
        GridPagos.Columnas.Add(ColSaldo);

        //Cuenta
        CJQColumn ColCuenta = new CJQColumn();
        ColCuenta.Nombre = "CuentaBancaria";
        ColCuenta.Encabezado = "Cuenta bancaria";
        ColCuenta.Buscador = "false";
        ColCuenta.Alineacion = "left";
        ColCuenta.Ancho = "100";
        GridPagos.Columnas.Add(ColCuenta);

        //CuentaBancaria
        CJQColumn ColNombreCuentaBancaria = new CJQColumn();
        ColNombreCuentaBancaria.Nombre = "NombreCuentaBancaria";
        ColNombreCuentaBancaria.Encabezado = "Nombre cuenta bancaria";
        ColNombreCuentaBancaria.Buscador = "false";
        ColNombreCuentaBancaria.Alineacion = "left";
        ColNombreCuentaBancaria.Ancho = "100";
        GridPagos.Columnas.Add(ColNombreCuentaBancaria);

        //Referencia
        CJQColumn ColReferencia = new CJQColumn();
        ColReferencia.Nombre = "Referencia";
        ColReferencia.Encabezado = "Referencia";
        ColReferencia.Buscador = "false";
        ColReferencia.Alineacion = "left";
        ColReferencia.Ancho = "100";
        GridPagos.Columnas.Add(ColReferencia);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Buscador = "false";
        ColTipoMoneda.Alineacion = "left";
        ColTipoMoneda.Ancho = "60";
        GridPagos.Columnas.Add(ColTipoMoneda);

        //FechaEmision
        CJQColumn ColFechaEmision = new CJQColumn();
        ColFechaEmision.Nombre = "FechaEmision";
        ColFechaEmision.Encabezado = "Fecha de captura";
        ColFechaEmision.Buscador = "false";
        ColFechaEmision.Alineacion = "left";
        ColFechaEmision.Ancho = "70";
        GridPagos.Columnas.Add(ColFechaEmision);

        //FechaAplicacion
        CJQColumn ColFechaAplicacion = new CJQColumn();
        ColFechaAplicacion.Nombre = "FechaAplicacion";
        ColFechaAplicacion.Encabezado = "Fecha acreditado";
        ColFechaAplicacion.Buscador = "false";
        ColFechaAplicacion.Alineacion = "left";
        ColFechaAplicacion.Ancho = "70";
        GridPagos.Columnas.Add(ColFechaAplicacion);

        //Asociado
        CJQColumn ColAsociado = new CJQColumn();
        ColAsociado.Nombre = "Asociado";
        ColAsociado.Encabezado = "Asociado";
        ColAsociado.Buscador = "true";
        ColAsociado.Alineacion = "left";
        ColAsociado.Ancho = "40";
        GridPagos.Columnas.Add(ColAsociado);

        //Conciliado
        CJQColumn ColConciliado = new CJQColumn();
        ColConciliado.Nombre = "Conciliado";
        ColConciliado.Encabezado = "Conciliado";
        ColConciliado.Buscador = "false";
        ColConciliado.Alineacion = "left";
        ColConciliado.Ancho = "40";
        GridPagos.Columnas.Add(ColConciliado);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "150";
        GridPagos.Columnas.Add(ColRazonSocial);

        //Gestor
        CJQColumn ColGestor = new CJQColumn();
        ColGestor.Nombre = "Gestor";
        ColGestor.Encabezado = "Gestor";
        ColGestor.Buscador = "true";
        ColGestor.Alineacion = "left";
        ColGestor.Ancho = "150";
        GridPagos.Columnas.Add(ColGestor);
        
        //Timbrado
        CJQColumn Timbrado = new CJQColumn();
        Timbrado.Nombre = "Timbrado";
        Timbrado.Encabezado = "Timbrado";
        Timbrado.Ordenable = "true";
        Timbrado.Ancho = "50";
        Timbrado.Etiquetado = "EstatusTimbradoFile";
        Timbrado.Buscador = "false";
        Timbrado.StoredProcedure.CommandText = "spc_ManejadorConvertirAPedido_Consulta";
        GridPagos.Columnas.Add(Timbrado);
        
        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.Estilo = "divImagenDeshabilitada";
        ColBaja.Etiquetado = puedeEliminarPagos == 1 ? "A/I" : "A/IDeshabilitado";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridPagos.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarPago";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridPagos.Columnas.Add(ColConsultar);

        //Formato
        CJQColumn ColFormato = new CJQColumn();
        ColFormato.Nombre = "Formato";
        ColFormato.Encabezado = "Imprimir";
        ColFormato.Etiquetado = "Imagen";
        ColFormato.Imagen = "Imprimir.png";
        ColFormato.Estilo = "divImagenConsultar imgFormaConsultarFacturaFormato";
        ColFormato.Buscador = "false";
        ColFormato.Ordenable = "false";
        ColFormato.Ancho = "50";
        GridPagos.Columnas.Add(ColFormato);

        //XML
        CJQColumn ColXML = new CJQColumn();
        ColXML.Nombre = "XML";
        ColXML.Encabezado = "XML";
        ColXML.Etiquetado = "Imagen";
        ColXML.Imagen = "xml-file.png";
        ColXML.Estilo = "divImagenConsultar imgFormaConsultarFacturaXML";
        ColXML.Buscador = "false";
        ColXML.Ordenable = "false";
        ColXML.Ancho = "50";
        GridPagos.Columnas.Add(ColXML);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPagos", GridPagos.GeneraGrid(), true);
    }

    public void GenerarGridPagosConciliar()
    {
        //GridConciliarPagos
        CJQGrid grdConciliarPagos = new CJQGrid();
        grdConciliarPagos.NombreTabla = "grdPagosConciliar";
        grdConciliarPagos.CampoIdentificador = "IdCuentasPorCobrar";
        grdConciliarPagos.ColumnaOrdenacion = "Sucursal,Folio";
        grdConciliarPagos.TipoOrdenacion = "DESC";
        grdConciliarPagos.Metodo = "ObtenerPagosConciliar";
        grdConciliarPagos.TituloTabla = "Conciliar pagos";
        grdConciliarPagos.GenerarGridCargaInicial = false;
        grdConciliarPagos.GenerarFuncionFiltro = false;
        grdConciliarPagos.GenerarFuncionTerminado = false;
        grdConciliarPagos.Altura = 340;
        grdConciliarPagos.Ancho = 960;
        grdConciliarPagos.NumeroRegistros = 30;
        grdConciliarPagos.RangoNumeroRegistros = "15,30,60";

        //IdCuentasPorCobrar
        CJQColumn ColIdCuentasPorCobrarConciliar = new CJQColumn();
        ColIdCuentasPorCobrarConciliar.Nombre = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrarConciliar.Oculto = "true";
        ColIdCuentasPorCobrarConciliar.Encabezado = "IdCuentasPorCobrar";
        ColIdCuentasPorCobrarConciliar.Buscador = "false";
        grdConciliarPagos.Columnas.Add(ColIdCuentasPorCobrarConciliar);

        //Sucursal
        CJQColumn ColSucursalConciliar = new CJQColumn();
        ColSucursalConciliar.Nombre = "Sucursal";
        ColSucursalConciliar.Encabezado = "Sucursal";
        ColSucursalConciliar.Ancho = "10";
        ColSucursalConciliar.Buscador = "false";
        ColSucursalConciliar.Alineacion = "left";
        ColSucursalConciliar.Oculto = "true";
        grdConciliarPagos.Columnas.Add(ColSucursalConciliar);

        //Folio
        CJQColumn ColFolioConciliar = new CJQColumn();
        ColFolioConciliar.Nombre = "FolioConciliar";
        ColFolioConciliar.Encabezado = "Folio";
        ColFolioConciliar.Ancho = "60";
        ColFolioConciliar.Buscador = "true";
        ColFolioConciliar.Alineacion = "left";
        grdConciliarPagos.Columnas.Add(ColFolioConciliar);

        //Importe
        CJQColumn ColImporteConciliar = new CJQColumn();
        ColImporteConciliar.Nombre = "Importe";
        ColImporteConciliar.Encabezado = "Importe";
        ColImporteConciliar.Buscador = "false";
        ColImporteConciliar.Formato = "FormatoMoneda";
        ColImporteConciliar.Alineacion = "right";
        ColImporteConciliar.Ancho = "80";
        grdConciliarPagos.Columnas.Add(ColImporteConciliar);

        //Cuenta
        CJQColumn ColCuentaConciliar = new CJQColumn();
        ColCuentaConciliar.Nombre = "CuentaBancaria";
        ColCuentaConciliar.Encabezado = "Cuenta bancaria";
        ColCuentaConciliar.Buscador = "false";
        ColCuentaConciliar.Alineacion = "left";
        ColCuentaConciliar.Ancho = "100";
        grdConciliarPagos.Columnas.Add(ColCuentaConciliar);

        //Referencia
        CJQColumn ColReferenciaConciliar = new CJQColumn();
        ColReferenciaConciliar.Nombre = "Referencia";
        ColReferenciaConciliar.Encabezado = "Referencia";
        ColReferenciaConciliar.Buscador = "false";
        ColReferenciaConciliar.Alineacion = "left";
        ColReferenciaConciliar.Ancho = "100";
        grdConciliarPagos.Columnas.Add(ColReferenciaConciliar);

        //TipoMoneda
        CJQColumn ColTipoMonedaConciliar = new CJQColumn();
        ColTipoMonedaConciliar.Nombre = "TipoMoneda";
        ColTipoMonedaConciliar.Encabezado = "Tipo de moneda";
        ColTipoMonedaConciliar.Buscador = "false";
        ColTipoMonedaConciliar.Alineacion = "left";
        ColTipoMonedaConciliar.Ancho = "60";
        grdConciliarPagos.Columnas.Add(ColTipoMonedaConciliar);

        //FechaEmision
        CJQColumn ColFechaEmisionConciliar = new CJQColumn();
        ColFechaEmisionConciliar.Nombre = "FechaEmision";
        ColFechaEmisionConciliar.Encabezado = "Fecha de captura";
        ColFechaEmisionConciliar.Buscador = "false";
        ColFechaEmisionConciliar.Alineacion = "left";
        ColFechaEmisionConciliar.Ancho = "80";
        grdConciliarPagos.Columnas.Add(ColFechaEmisionConciliar);

        //FechaAplicacion
        CJQColumn ColFechaAplicacionConciliar = new CJQColumn();
        ColFechaAplicacionConciliar.Nombre = "FechaAplicacion";
        ColFechaAplicacionConciliar.Encabezado = "Fecha acreditado";
        ColFechaAplicacionConciliar.Buscador = "false";
        ColFechaAplicacionConciliar.Alineacion = "left";
        ColFechaAplicacionConciliar.Ancho = "80";
        grdConciliarPagos.Columnas.Add(ColFechaAplicacionConciliar);

        //Asociado
        CJQColumn ColAsociadoConciliar = new CJQColumn();
        ColAsociadoConciliar.Nombre = "Asociado";
        ColAsociadoConciliar.Encabezado = "Asociado";
        ColAsociadoConciliar.Buscador = "false";
        ColAsociadoConciliar.Alineacion = "left";
        ColAsociadoConciliar.Ancho = "50";
        grdConciliarPagos.Columnas.Add(ColAsociadoConciliar);

        //Conciliado
        CJQColumn ColConciliadoConciliar = new CJQColumn();
        ColConciliadoConciliar.Nombre = "Conciliado";
        ColConciliadoConciliar.Encabezado = "Conciliado";
        ColConciliadoConciliar.Buscador = "false";
        ColConciliadoConciliar.Alineacion = "left";
        ColConciliadoConciliar.Ancho = "50";
        grdConciliarPagos.Columnas.Add(ColConciliadoConciliar);

        //RazonSocial
        CJQColumn ColRazonSocialConciliar = new CJQColumn();
        ColRazonSocialConciliar.Nombre = "RazonSocialConciliar";
        ColRazonSocialConciliar.Encabezado = "Razón social";
        ColRazonSocialConciliar.Buscador = "true";
        ColRazonSocialConciliar.Alineacion = "left";
        ColRazonSocialConciliar.Ancho = "250";
        grdConciliarPagos.Columnas.Add(ColRazonSocialConciliar);

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
        grdConciliarPagos.Columnas.Add(ColConciliar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdPagosConciliar", grdConciliarPagos.GeneraGrid(), true);
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
    public static CJQGridJsonResponse ObtenerPagos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, string pAsociado, string pGestor)
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
    public static CJQGridJsonResponse ObtenerPagosConciliar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio)
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
    public static CJQGridJsonResponse ObtenerMovimientosCobros(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPago)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentasPorCobrar", SqlDbType.Int).Value = pIdPago;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosConsultar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPago)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentasPorCobrar", SqlDbType.Int).Value = pIdPago;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerMovimientosCobrosEditar(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPago)
    {

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCuentasPorCobrarEncabezadoFacturaConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdCuentasPorCobrar", SqlDbType.Int).Value = pIdPago;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);

    }

    [WebMethod]
    public static string ObtenerFormaFiltroPagos()
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
    public static string ObtenerFormaAgregarPago()
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarTipoCambioIngresos = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, pConexion) == "")
                {
                    puedeEditarTipoCambioIngresos = 1;
                }
                oPermisos.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);

                JObject Modelo = new JObject();
                CMetodoPago MetodoPago = new CMetodoPago();
                Modelo.Add("MetodoPagos", CJson.ObtenerJsonMetodoPago(pConexion, 1));
                Modelo.Add("TipoMonedas", CJson.ObtenerJsonTipoMoneda(pConexion));
                Modelo.Add(new JProperty("Fecha", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("FechaAplicacion", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("FechaConciliacion", Convert.ToDateTime(DateTime.Now).ToShortDateString()));
                Modelo.Add(new JProperty("Permisos", oPermisos));

                Error = 0;
                Respuesta.Add("Modelo", Modelo);

            }
            else
            {
                Error = 1;
                DescripcionError = "No hay conexion a Base de Datos";
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaPago(int pIdPago)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarPago = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {

                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarPago" }, pConexion) == "")
                {
                    puedeEditarPago = 1;
                }
                oPermisos.Add("puedeEditarPago", puedeEditarPago);

                JObject Modelo = new JObject();
                Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, pIdPago, pConexion);
                Respuesta.Add("Modelo", Modelo);
                Modelo.Add("Permisos", oPermisos);

                Error = 0;

            }
            else
            {
                Error = 1;
                DescripcionError = "No hay conexion a Base de Datos";
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaEditarPago(int IdPago)
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
                Ingreso.LlenaObjeto(IdPago, pConexion);

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
    public static string ObtenerFormaConciliarPagos()
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                
                Error = 0;
                Respuesta.Add("Modelo", Modelo);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtieneFacturaFormato(int IdPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CRutaCFDI RutaCFDI = new CRutaCFDI();
        CCuentasPorCobrar pago = new CCuentasPorCobrar();
        CCliente cliente = new CCliente();
        COrganizacion organizacion = new COrganizacion();
        string NombreArchivo = "";
        string Ruta = "";

        pago.LlenaObjeto(IdPago, ConexionBaseDatos);


        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
        ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
        ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
        ParametrosTS.Add("Baja", Convert.ToInt32(0));
        RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

        cliente.LlenaObjeto(pago.IdCliente,ConexionBaseDatos);
        organizacion.LlenaObjeto(cliente.IdOrganizacion, ConexionBaseDatos);
       

        NombreArchivo = "PGO" + pago.Folio;
        //Ruta = RutaCFDI.RutaCFDI + "/out/" + NombreArchivo + ".pdf";
        Ruta = RutaCFDI.RutaCFDI + "/Pagos/out/" + organizacion.RFC + "/" + NombreArchivo + ".pdf";

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
    public static string ObtieneFacturaXML(int IdPago)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject Respuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {

            JObject oRespuesta = new JObject();
            JObject oPermisos = new JObject();
            CUsuario Usuario = new CUsuario();
            CSucursal Sucursal = new CSucursal();
            CRutaCFDI RutaCFDI = new CRutaCFDI();
            CRutaCFDI RutaCFDIF = new CRutaCFDI();
            CCuentasPorCobrar pago = new CCuentasPorCobrar();
            CCliente cliente = new CCliente();
            COrganizacion organizacion = new COrganizacion();
            string NombreArchivo = "";
            string Ruta = "";
            string RutaF = "";

            pago.LlenaObjeto(IdPago, ConexionBaseDatos);
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
            ParametrosTS.Add("TipoRuta", Convert.ToInt32(2));
            ParametrosTS.Add("Baja", Convert.ToInt32(0));
            RutaCFDI.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

            ParametrosTS.Clear();
            ParametrosTS.Add("IdSucursal", Convert.ToInt32(Usuario.IdSucursalActual));
            ParametrosTS.Add("TipoRuta", Convert.ToInt32(1));
            ParametrosTS.Add("Baja", Convert.ToInt32(0));
            RutaCFDIF.LlenaObjetoFiltros(ParametrosTS, ConexionBaseDatos);

            cliente.LlenaObjeto(pago.IdCliente, ConexionBaseDatos);
            organizacion.LlenaObjeto(cliente.IdOrganizacion, ConexionBaseDatos);

            NombreArchivo = "PGO" + pago.Folio;
            //Ruta = RutaCFDI.RutaCFDI + "/out/" + NombreArchivo + ".xml";
            Ruta = RutaCFDI.RutaCFDI + "/Pagos/out/" + organizacion.RFC + "/" + NombreArchivo + ".xml";
            RutaF = RutaCFDIF.RutaCFDI + "\\Pagos\\out\\" + organizacion.RFC + "\\" + NombreArchivo + ".xml";

            if (File.Exists(RutaF))
            {
                Respuesta.Add("Error", 0);
                Respuesta.Add("xml", Ruta);
                Respuesta.Add("name", NombreArchivo);

            }
            else
            {
                Respuesta.Add("Error", 1);
                Respuesta.Add("Descripcion", "No se encontro el XML");
            }

        }
        else
        {
            Respuesta.Add("Error", 1);
            Respuesta.Add("Descripcion", respuesta);
        }

        return Respuesta.ToString();

    }

    [WebMethod]
    public static string ObtenerFormaDatosCliente(int pIdCliente)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CSelect facturas = new CSelect();

                CSerieFactura serie = new CSerieFactura();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdSucursal", UsuarioSesion.IdSucursalActual);
                pParametros.Add("Baja", 0);
                pParametros.Add("Timbrado", 1);
                pParametros.Add("EsParcialidad", 0);
                pParametros.Add("EsVenta", 1);
                serie.LlenaObjetoFiltros(pParametros, pConexion);

                facturas.StoredProcedure.CommandText = "sp_FacturasPorPagar";
                facturas.StoredProcedure.Parameters.Add("@IdCliente", SqlDbType.VarChar, 50).Value = Convert.ToString(pIdCliente);
                facturas.StoredProcedure.Parameters.Add("@Serie", SqlDbType.VarChar, 10).Value = Convert.ToString(serie.SerieFactura);
                //facturas.ObtenerJsonJObject(pConexion);

                facturas.Llena<CFacturaEncabezado>(typeof(CFacturaEncabezado), pConexion);
                JArray JAfacturas = new JArray();
                foreach (CFacturaEncabezado oFacturas in facturas.ListaRegistros)
                {
                    JObject Jfacturas = new JObject();
                    Jfacturas.Add("Valor", oFacturas.IdFacturaEncabezado);
                    Jfacturas.Add("Saldo", oFacturas.SaldoFactura);
                    Jfacturas.Add("Descripcion", oFacturas.Serie + oFacturas.NumeroFactura);

                    JAfacturas.Add(Jfacturas);
                }
                
                //FormasPago
                JObject JComboFaturas = new JObject();
                JComboFaturas.Add("DescripcionDefault", "Seleccionar...");
                JComboFaturas.Add("ValorDefault", "0");
                JComboFaturas.Add("Opciones", JAfacturas);
                Respuesta.Add("Modelo", JComboFaturas);
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }


    [WebMethod]
    public static string ConciliarIngreso(Dictionary<string, object> pPago)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
                CuentasPorCobrar.LlenaObjeto(Convert.ToInt32(pPago["pIdPago"]), pConexion);
                CuentasPorCobrar.Conciliado = true;
                CuentasPorCobrar.FechaConciliacion = DateTime.Now;
                string validacion = "";

                if (validacion == "")
                {
                    CuentasPorCobrar.Editar(pConexion);
                    Error = 0;
                }
                else
                {
                    Error = 1;
                    DescripcionError = validacion;

                }
                
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarMontos(Dictionary<string, object> pPago)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                FacturaEncabezado.LlenaObjeto(Convert.ToInt32(pPago["IdEncabezadoFactura"]), pConexion);

                CCuentasPorCobrarEncabezadoFactura CuentasPorCobrarEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
                CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar = Convert.ToInt32(pPago["IdPago"]);
                CuentasPorCobrarEncabezadoFactura.IdEncabezadoFactura = Convert.ToInt32(pPago["IdEncabezadoFactura"]);
                CuentasPorCobrarEncabezadoFactura.Monto = Convert.ToDecimal(pPago["Monto"]);
                CuentasPorCobrarEncabezadoFactura.FechaPago = Convert.ToDateTime(DateTime.Now);
                CuentasPorCobrarEncabezadoFactura.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                CuentasPorCobrarEncabezadoFactura.IdTipoMoneda = Convert.ToInt32(pPago["IdTipoMoneda"]);
                CuentasPorCobrarEncabezadoFactura.TipoCambio = Convert.ToDecimal(pPago["TipoCambio"]);
                CuentasPorCobrarEncabezadoFactura.Nota = "pago de la factura";

                CCuentasPorCobrar cuentasPorCobrar = new CCuentasPorCobrar();
                cuentasPorCobrar.LlenaObjeto(Convert.ToInt32(pPago["IdPago"]),pConexion);
                cuentasPorCobrar.Asociado = true;

                string validacion = ValidarMontos(CuentasPorCobrarEncabezadoFactura, FacturaEncabezado, pConexion);
                if (validacion == "")
                {
                    CuentasPorCobrarEncabezadoFactura.AgregarCuentasPorCobrarEncabezadoFactura(pConexion);
                    cuentasPorCobrar.Editar(pConexion);
                    Respuesta.Add("Monto", Convert.ToDecimal(pPago["Monto"]));
                    Respuesta.Add("rowid", Convert.ToDecimal(pPago["rowid"]));
                    Respuesta.Add("TipoMoneda", Convert.ToString(pPago["TipoMoneda"]));
                    Respuesta.Add("AbonosCuentasPorCobrar", CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(Convert.ToInt32(pPago["IdPago"]), pConexion));

                    if (Convert.ToInt32(pPago["EsParcialidad"]) == 1)
                    {
                        FacturaEncabezado.SaldoFactura -= Convert.ToDecimal(pPago["Monto"]);
                        FacturaEncabezado.NumeroParcialidadesPendientes = FacturaEncabezado.NumeroParcialidadesPendientes - 1;
                        FacturaEncabezado.Editar(pConexion);
                        Respuesta.Add("EsParcialidad", 1);

                    }
                    else
                    {
                        Respuesta.Add("EsParcialidad", 0);
                    }

                    CFacturaDetalle Detalle = new CFacturaDetalle();
                    Dictionary<string, object> pParametros = new Dictionary<string, object>();
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
                else
                {
                    Error = 1;
                    DescripcionError = validacion;

                }

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EliminarPagoEncabezadoFactura(Dictionary<string, object> pPagoEncabezadoFactura)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CFacturaEncabezado FacturaEncabezado = new CFacturaEncabezado();
                CCuentasPorCobrar cuentasPorCobrar = new CCuentasPorCobrar();
                CCuentasPorCobrarEncabezadoFactura CuentasPorCobrarEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
                CuentasPorCobrarEncabezadoFactura.LlenaObjeto(Convert.ToInt32(pPagoEncabezadoFactura["pIdPagoEncabezadoFactura"]), pConexion);
                FacturaEncabezado.LlenaObjeto(CuentasPorCobrarEncabezadoFactura.IdEncabezadoFactura, pConexion);
                cuentasPorCobrar.LlenaObjeto(CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar,pConexion);

                if (FacturaEncabezado.Parcialidades == true)
                {                       
                    Respuesta.Add("EsParcialidad", 1);
}
                else
                {
                    Respuesta.Add("EsParcialidad", 0);   
                }
                cuentasPorCobrar.Asociado = false;
                cuentasPorCobrar.Editar(pConexion);
                CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrarEncabezadoFactura = Convert.ToInt32(pPagoEncabezadoFactura["pIdPagoEncabezadoFactura"]);
                CuentasPorCobrarEncabezadoFactura.Baja = true;
                CuentasPorCobrarEncabezadoFactura.EliminarCuentasPorCobrarEncabezadoFactura(pConexion);
                Respuesta.Add("AbonosCuentasPorCobrar", CuentasPorCobrarEncabezadoFactura.TotalAbonosCuentasPorCobrar(CuentasPorCobrarEncabezadoFactura.IdCuentasPorCobrar, pConexion));
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerHabilitaMonto(int pIdPago)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Modelo = new JObject();
                
                int puedeEditarTipoCambioIngresos = UsuarioSesion.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, pConexion) == "" ? 1 : 0;
                
                JObject oPermisos = new JObject();
                CUsuario Usuario = new CUsuario();

                Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, pIdPago, pConexion);
                Modelo.Add(new JProperty("Permisos", oPermisos));
                Modelo.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);
                Respuesta.Add("Modelo", Modelo);

                Error = 0;
                
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerFormaAsociarDocumentos(Dictionary<string, object> pPago)
    {
        JObject Respuesta = new JObject();
        int puedeEditarCuentasPorCobrar = 0;
        JObject oPermisos = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarCuentasPorCobrar" }, pConexion) == "")
                {
                    puedeEditarCuentasPorCobrar = 1;
                }
                oPermisos.Add("puedeEditarCuentasPorCobrar", puedeEditarCuentasPorCobrar);


                CCuentasPorCobrar CuentasPorCobrarCliente = new CCuentasPorCobrar();
                CuentasPorCobrarCliente.LlenaObjeto(Convert.ToInt32(pPago["pIdPago"]), pConexion);
                CuentasPorCobrarCliente.IdCliente = Convert.ToInt32(pPago["pIdCliente"]);
                CuentasPorCobrarCliente.TipoCambio = Convert.ToDecimal(pPago["TipoCambio"]);
                CuentasPorCobrarCliente.TipoCambioDOF = Convert.ToDecimal(pPago["TipoCambioDOF"]);
                CuentasPorCobrarCliente.EditarCuentasPorCobrar(pConexion);

                JObject Modelo = new JObject();
                Modelo = CCuentasPorCobrar.ObtenerCuentasPorCobrar(Modelo, Convert.ToInt32(pPago["pIdPago"]), pConexion);
                Modelo.Add("IdPago", Convert.ToInt32(pPago["pIdPago"]));
                Modelo.Add("Permisos", oPermisos);
                Respuesta.Add("Modelo", Modelo);

                Error = 0;
            }
            else
            {
                Error = 1;
                DescripcionError = "No hay conexion a Base de Datos";
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);

        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerTipoCambioDiarioOficial(int pIdTipoMonedaDestino, string pFecha)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                Respuesta.Add("TipoCambioDiarioOficial", CTipoCambioDiarioOficial.ObtenerTipoCambio(pIdTipoMonedaDestino, Convert.ToDateTime(pFecha), pConexion));
                Error = 0;
                DescripcionError = "Se obtuvó el tipo de cambio del diario oficial con éxito.";
            }
            else
            {
                Error = 1;
                DescripcionError = "No hay conexion a Base de Datos";
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);

        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string ObtenerDatosCuentaBancaria(int pIdCuentaBancaria)
    {
        JObject Respuesta = new JObject();
        JObject oPermisos = new JObject();
        int puedeEditarTipoCambioIngresos = 0;

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                if (UsuarioSesion.TienePermisos(new string[] { "puedeEditarTipoCambioIngresos" }, pConexion) == "")
                {
                    puedeEditarTipoCambioIngresos = 1;
                }
                oPermisos.Add("puedeEditarTipoCambioIngresos", puedeEditarTipoCambioIngresos);

                JObject Modelo = new JObject();
                Modelo = CCuentaBancaria.ObtenerCuentaBancariaDOF(Modelo, pIdCuentaBancaria, pConexion);

                Modelo.Add("Permisos", oPermisos);
                Respuesta.Add("Modelo", Modelo);

                Error = 0;
            }
            else
            {
                Error = 1;
                DescripcionError = "No hay conexion a Base de Datos";
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);

        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string BuscarRazonSocialCliente(string pRazonSocial)
    {

        JObject Respuesta = new JObject();
        string response = "";

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                COrganizacion jsonOrganizacion = new COrganizacion();
                jsonOrganizacion.StoredProcedure.CommandText = "sp_Cliente_ConsultarRazonSocial";
                jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
                jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
                jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
                response = jsonOrganizacion.ObtenerJsonRazonSocial(pConexion);
            }

        });

        return response;

    }

    [WebMethod]
    public static string AgregarPago(Dictionary<string, object> pPago)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CCuentasPorCobrar CuentasPorCobrar = new CCuentasPorCobrar();
                CuentasPorCobrar.IdCuentaBancaria = Convert.ToInt32(pPago["IdCuentaBancaria"]);
                CuentasPorCobrar.IdCliente = Convert.ToInt32(pPago["IdCliente"]);
                CuentasPorCobrar.IdMetodoPago = Convert.ToInt32(pPago["IdMetodoPago"]);
                CuentasPorCobrar.FechaEmision = Convert.ToDateTime(pPago["Fecha"]);
                CuentasPorCobrar.Importe = Convert.ToDecimal(pPago["Importe"]);
                CuentasPorCobrar.Referencia = Convert.ToString(pPago["Referencia"]);
                CuentasPorCobrar.ConceptoGeneral = Convert.ToString(pPago["ConceptoGeneral"]);
                CuentasPorCobrar.FechaAplicacion = Convert.ToDateTime(pPago["FechaAplicacion"]);
                if (CuentasPorCobrar.Conciliado == true)
                {
                    CuentasPorCobrar.FechaConciliacion = Convert.ToDateTime(pPago["FechaConciliacion"]);
                }

                CuentasPorCobrar.Conciliado = Convert.ToBoolean(pPago["Conciliado"]);
                CuentasPorCobrar.Asociado = Convert.ToBoolean(pPago["Asociado"]);
                CuentasPorCobrar.TipoCambio = Convert.ToDecimal(pPago["TipoCambio"]);
                CuentasPorCobrar.IdTipoMoneda = Convert.ToInt32(pPago["IdTipoMoneda"]);
                CuentasPorCobrar.TipoCambioDOF = Convert.ToDecimal(pPago["TipoCambioDOF"]);
                CuentasPorCobrar.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);

                string validacion = ValidarPago(CuentasPorCobrar, pConexion);
                
                if (validacion == "")
                {
                    CuentasPorCobrar.AgregarCuentasPorCobrar(pConexion);

                    CCuentasPorCobrarSucursal CuentasPorCobrarSucursal = new CCuentasPorCobrarSucursal();
                    CuentasPorCobrarSucursal.IdCuentasPorCobrar = CuentasPorCobrar.IdCuentasPorCobrar;
                    CuentasPorCobrarSucursal.IdSucursal = Usuario.IdSucursalActual;
                    CuentasPorCobrarSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    CuentasPorCobrarSucursal.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    CuentasPorCobrarSucursal.Agregar(pConexion);

                    CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                    HistorialGenerico.IdGenerico = CuentasPorCobrar.IdCuentasPorCobrar;
                    HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                    HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                    HistorialGenerico.Comentario = "Se inserto un ingreso";
                    HistorialGenerico.AgregarHistorialGenerico("CuentasPorCobrar", pConexion);

                    CuentasPorCobrar.LlenaObjeto(CuentasPorCobrar.IdCuentasPorCobrar, pConexion);
                    Respuesta.Add("IdPago", CuentasPorCobrar.IdCuentasPorCobrar);
                    Respuesta.Add("Folio", CuentasPorCobrar.Folio);

                    Error =0;
                }
                else
                {
                    Error=1;
                    DescripcionError=validacion;
                }
            }
            else
            {
                Error = 1;
                DescripcionError = "No hay conexion a Base de Datos";
            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);

        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string EditarPago(Dictionary<string, object> pPago)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CCuentasPorCobrar pago = new CCuentasPorCobrar();
                pago.LlenaObjeto(Convert.ToInt32(pPago["IdPago"]), pConexion);
                pago.IdCuentasPorCobrar = Convert.ToInt32(pPago["IdPago"]);
                pago.IdCliente = Convert.ToInt32(pPago["IdCliente"]);
                pago.Referencia = Convert.ToString(pPago["Referencia"]);
                pago.ConceptoGeneral = Convert.ToString(pPago["ConceptoGeneral"]);
                pago.Conciliado = Convert.ToBoolean(pPago["Conciliado"]);
                pago.Importe = Convert.ToDecimal(pPago["Importe"]);
                pago.TipoCambio = Convert.ToDecimal(pPago["TipoCambio"]);
                pago.TipoCambioDOF = Convert.ToDecimal(pPago["TipoCambioDOF"]);
                pago.FechaEmision = Convert.ToDateTime(pPago["Fecha"]);
                pago.IdMetodoPago = Convert.ToInt32(pPago["IdMetodoPago"]);

                CCuentaBancaria Cuenta = new CCuentaBancaria();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("CuentaBancaria", Convert.ToString(pPago["CuentaBancaria"]));
                Cuenta.LlenaObjetoFiltros(pParametros, pConexion);
                pago.IdCuentaBancaria = Cuenta.IdCuentaBancaria;
                pago.FechaAplicacion = Convert.ToDateTime(pPago["FechaAplicacion"]);

                if (pago.Conciliado == true && Convert.ToString(pPago["FechaConciliacion"]) != "-")
                {
                    pago.FechaConciliacion = Convert.ToDateTime(pPago["FechaConciliacion"]);
                }

                string validacion = ValidarPago(pago, pConexion);
                
                if (validacion == "")
                {
                    pago.Editar(pConexion);
                    
                }
                else
                {
                    Error = 1;
                    DescripcionError = validacion;
                }

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });
        return Respuesta.ToString();
    }

    //Validaciones
    private static string ValidarPago(CCuentasPorCobrar pCuentasPorCobrar, CConexion pConexion)
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
    public static string ObtenerDatosTimbradoPago(int IdPago)
    { 
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                JObject Comprobante = new JObject();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                CCuentasPorCobrarEncabezadoFactura pagoEncabezadoFactura = new CCuentasPorCobrarEncabezadoFactura();
                pParametros.Add("IdCuentasPorCobrar",IdPago);
                pParametros.Add("Baja",0);
                pagoEncabezadoFactura.LlenaObjetoFiltros(pParametros, pConexion);

                if (pagoEncabezadoFactura.IdEncabezadoFactura != 0)
                {
                    
                    CFacturaEncabezado FacturaPadre = new CFacturaEncabezado();
                    FacturaPadre.LlenaObjeto(pagoEncabezadoFactura.IdEncabezadoFactura, pConexion);

                    int NumeroParcialidadActual = 0;
                    NumeroParcialidadActual = (FacturaPadre.NumeroParcialidades - FacturaPadre.NumeroParcialidadesPendientes);
                    //NumeroParcialidadActual = (NumeroParcialidadActual == 0) ? 10 : NumeroParcialidadActual;

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

                    CTxtTimbradosFactura TimbradoPadre = new CTxtTimbradosFactura();
                    pParametros.Clear();
                    pParametros.Add("Refid", FacturaPadre.Refid);
                    TimbradoPadre.LlenaObjetoFiltros(pParametros, pConexion);

                    CCuentasPorCobrar pago = new CCuentasPorCobrar();
                    pago.LlenaObjeto(IdPago, pConexion);

                    CMetodoPago FormaPago = new CMetodoPago();
                    FormaPago.LlenaObjeto(pago.IdMetodoPago, pConexion);

                    CRutaCFDI RutaCFDI = new CRutaCFDI();
                    pParametros.Clear();
                    pParametros.Add("IdSucursal", Convert.ToInt32(UsuarioSesion.IdSucursalActual));
                    pParametros.Add("TipoRuta", Convert.ToInt32(1));
                    pParametros.Add("Baja", Convert.ToInt32(0));
                    RutaCFDI.LlenaObjetoFiltros(pParametros, pConexion);

                    // datos del comprobante
                    Comprobante.Add("Serie", "PGO");//SerieFactura.SerieFactura);
                    Comprobante.Add("Folio", pago.Folio);
                    Comprobante.Add("Fecha", pago.FechaEmision);
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
                    Emisor.Add("RFC", "MAG041126GT8"); // RFC example // ClearString(Empresa.RFC)); 
                    Emisor.Add("RegimenFiscal", "601"); // Catalogo SAT

                    Comprobante.Add("Emisor", Emisor);

                    // datos del receptor
                    JObject Receptor = new JObject();
                    Receptor.Add("Nombre", ClearString(Organizacion.RazonSocial));
                    Receptor.Add("RFC", ClearString(Organizacion.RFC));
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
                    DoctoRelacionadoContenido.Add("Serie", FacturaPadre.Serie);
                    DoctoRelacionadoContenido.Add("Folio", FacturaPadre.NumeroFactura);
                    DoctoRelacionadoContenido.Add("MonedaDR", (FacturaPadre.IdTipoMoneda == 1) ? "MXN" : "USD"); // Catalogo SAT

                    string tipoCambioDR = "";
                    if (FacturaPadre.IdTipoMoneda == 1)
                    {
                        if (FacturaPadre.IdTipoMoneda != pago.IdTipoMoneda)
                        {
                            tipoCambioDR = "1";
                        }
                    }
                    else if (FacturaPadre.IdTipoMoneda != pago.IdTipoMoneda)
                    {
                        tipoCambioDR = Convert.ToString(FacturaPadre.TipoCambio);
                    }

                    DoctoRelacionadoContenido.Add("TipoCambioDR", tipoCambioDR);
                    DoctoRelacionadoContenido.Add("MetodoDePagoDR", (FacturaPadre.Parcialidades) ? "PPD" : "PUE"); // Catalogo SAT
                    DoctoRelacionadoContenido.Add("NumParcialidad", NumeroParcialidadActual);
                    DoctoRelacionadoContenido.Add("ImpSaldoAnt", FacturaPadre.SaldoFactura + pagoEncabezadoFactura.Monto);
                    DoctoRelacionadoContenido.Add("ImpPagado", pagoEncabezadoFactura.Monto);
                    DoctoRelacionadoContenido.Add("ImpSaldoInsoluto", (FacturaPadre.SaldoFactura + pagoEncabezadoFactura.Monto) - pagoEncabezadoFactura.Monto);

                    Complemento.Add("FechaPago", pago.FechaConciliacion);
                    Complemento.Add("FormaDePagoP", FormaPago.Clave); // Catalogo SAT
                    Complemento.Add("MonedaP", (pago.IdTipoMoneda == 1) ? "MXN" : "USD"); // Catalogo SAT
                    Complemento.Add("TipoCambioP", (pago.TipoCambio == 1) ? "" : Convert.ToString(pago.TipoCambio));
                    Complemento.Add("Monto", pagoEncabezadoFactura.Monto);
                    Complemento.Add("DoctoRelacionado", DoctoRelacionadoContenido);

                    Comprobante.Add("Complemento", Complemento);

                    //Ruta CFDI
                    Respuesta.Add("RutaCFDI", RutaCFDI.RutaCFDI);

                    string Correos = "";

                    Correos = "facturacion@grupoasercom.com";

                    // Terminado de datos de comprobate
                    Respuesta.Add("Id", 94327); // Id example // Empresa.IdTimbrado);
                    Respuesta.Add("Token", "$2b$12$pj0NTsT/brybD2cJrNa8iuRRE5KoxeEFHcm/yJooiSbiAdbiTGzIq"); // Token example // Empresa.Token);
                    Respuesta.Add("Comprobante", Comprobante);
                    Respuesta.Add("RFC", "MAG041126GT8"); // RFC example // Empresa.RFC); 
                    Respuesta.Add("RefID", pago.IdCuentasPorCobrar);
                    Respuesta.Add("NoCertificado", "20001000000300022755"); // NoCertificado example  // Sucursal.NoCertificado);
                    Respuesta.Add("Formato", "zip"); // xml, pdf, zip
                    Respuesta.Add("Correos", Correos);

                    Error = 0;
                    DescripcionError = "Datos cargados correctamente.";
                }
                else
                {
                    Error = 1;
                    DescripcionError = "No tiene documentos Asociados.";
                }
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
    }

    [WebMethod]
    public static string GuardarTimbradoPago(string UUId, int RefId)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate (CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion)
        {
            if (Error == 0)
            {
                CCuentasPorCobrar Pago = new CCuentasPorCobrar();
                Pago.LlenaObjeto(RefId,pConexion);

                CTxtTimbradosPagos Timbrado = new CTxtTimbradosPagos();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();

                Timbrado.Uuid = UUId;
                Timbrado.RefId = RefId.ToString();
                Timbrado.Serie = "PGO";
                Timbrado.Fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                Timbrado.FechaTimbrado = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                Timbrado.Folio = Convert.ToString(Pago.Folio);

                CTxtTimbradosPagos ValidarTimbrado = new CTxtTimbradosPagos();
                pParametros.Clear();
                pParametros.Add("Serie", Timbrado.Serie);
                pParametros.Add("Folio", Timbrado.Folio);

                if (ValidarTimbrado.LlenaObjetosFiltros(pParametros, pConexion).Count == 0)
                {
                    Timbrado.Agregar(pConexion);

                }
                else
                {
                    Error = 1;
                    DescripcionError = "Ya se habia emitido esta Factura.";
                }

                Error = 0;
                DescripcionError = "Se ha guardado con éxito la Factura.";

            }

            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionError);
        });

        return Respuesta.ToString();
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