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
using System.Xml;
using System.Net;
using arquitecturaNet;
using System.IO;
using System.Diagnostics;

public partial class OrdenCompra : System.Web.UI.Page
{
    public static CConexion ConexionBaseDatos;
    public static string ticks = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CEmpresa Empresa = new CEmpresa();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        int idUsuario = Usuario.IdUsuario;
        int idSucursal = Sucursal.IdSucursal;
        int idEmpresa = Empresa.IdEmpresa;
        string logoEmpresa = Empresa.Logo;

        int puedeAgregarOrdenCompra = Usuario.TienePermisos(new string[] { "puedeAgregarOrdenCompra" }, ConexionBaseDatos) == "" ? 1 : 0;
        int puedeEditarOrdenCompra = Usuario.TienePermisos(new string[] { "puedeEditarOrdenCompra" }, ConexionBaseDatos) == "" ? 1 : 0;
        int puedeEliminarOrdenCompra = Usuario.TienePermisos(new string[] { "puedeEliminarOrdenCompra" }, ConexionBaseDatos) == "" ? 1 : 0;

        //GridOrganizacion
        CJQGrid GridOrdenCompra = new CJQGrid();
        GridOrdenCompra.NombreTabla = "grdOrdenCompra";
        GridOrdenCompra.CampoIdentificador = "IdOrdenCompraEncabezado";
        GridOrdenCompra.ColumnaOrdenacion = "Folio";
        GridOrdenCompra.Metodo = "ObtenerOrdenCompra";
        GridOrdenCompra.TituloTabla = "Órdenes de compra";
        GridOrdenCompra.TipoOrdenacion = "DESC";
        GridOrdenCompra.GenerarFuncionFiltro = false;


        //IdOrganizacion
        CJQColumn ColIdOrganizacion = new CJQColumn();
        ColIdOrganizacion.Nombre = "IdOrdenCompraEncabezado";
        ColIdOrganizacion.Oculto = "true";
        ColIdOrganizacion.Encabezado = "IdOrdenCompraEncabezado";
        ColIdOrganizacion.Buscador = "false";
        GridOrdenCompra.Columnas.Add(ColIdOrganizacion);

        //Folio
        CJQColumn ColFolio = new CJQColumn();
        ColFolio.Nombre = "Folio";
        ColFolio.Encabezado = "Folio";
        ColFolio.Buscador = "true";
        ColFolio.Alineacion = "left";
        ColFolio.Ancho = "70";
        GridOrdenCompra.Columnas.Add(ColFolio);

        //RazonSocial
        CJQColumn ColRazonSocial = new CJQColumn();
        ColRazonSocial.Nombre = "RazonSocial";
        ColRazonSocial.Encabezado = "Razón social";
        ColRazonSocial.Buscador = "true";
        ColRazonSocial.Alineacion = "left";
        ColRazonSocial.Ancho = "180";
        GridOrdenCompra.Columnas.Add(ColRazonSocial);

        //Fecha
        CJQColumn ColFecha = new CJQColumn();
        ColFecha.Nombre = "FechaAlta";
        ColFecha.Encabezado = "Fecha";
        ColFecha.Buscador = "false";
        ColFecha.Alineacion = "left";
        ColFecha.Ancho = "70";
        GridOrdenCompra.Columnas.Add(ColFecha);

        //FechaEntrega
        CJQColumn ColFechaEntrega = new CJQColumn();
        ColFechaEntrega.Nombre = "FechaRealEntrega";
        ColFechaEntrega.Encabezado = "Fecha entrega";
        ColFechaEntrega.Oculto = "true";
        ColFechaEntrega.Buscador = "false";
        ColFechaEntrega.Alineacion = "left";
        ColFechaEntrega.Ancho = "80";
        GridOrdenCompra.Columnas.Add(ColFechaEntrega);

        //DireccionEntrega
        CJQColumn ColDireccionEntrega = new CJQColumn();
        ColDireccionEntrega.Nombre = "DireccionEntrega";
        ColDireccionEntrega.Encabezado = "Dirección entrega";
        ColDireccionEntrega.Buscador = "false";
        ColDireccionEntrega.Alineacion = "left";
        ColDireccionEntrega.Ancho = "100";
        GridOrdenCompra.Columnas.Add(ColDireccionEntrega);

        //Estado
        CJQColumn ColEstatus = new CJQColumn();
        ColEstatus.Nombre = "OCE.Baja, Consolidado";
        ColEstatus.Encabezado = "Estatus";
        ColEstatus.Buscador = "false";
        ColEstatus.Alineacion = "left";
        ColEstatus.Ancho = "60";
        GridOrdenCompra.Columnas.Add(ColEstatus);

        //Subtotal
        CJQColumn ColSubtotal = new CJQColumn();
        ColSubtotal.Nombre = "Subtotal";
        ColSubtotal.Encabezado = "Subtotal";
        ColSubtotal.Oculto = "true";
        ColSubtotal.Buscador = "false";
        ColSubtotal.Alineacion = "right";
        ColSubtotal.Ancho = "80";
        ColSubtotal.Formato = "FormatoMoneda";
        GridOrdenCompra.Columnas.Add(ColSubtotal);

        //IVA
        CJQColumn ColIVA = new CJQColumn();
        ColIVA.Nombre = "IVA";
        ColIVA.Encabezado = "IVA";
        ColIVA.Oculto = "true";
        ColIVA.Buscador = "false";
        ColIVA.Alineacion = "right";
        ColIVA.Ancho = "80";
        ColIVA.Formato = "FormatoMoneda";
        GridOrdenCompra.Columnas.Add(ColIVA);

        //Total
        CJQColumn ColTotal = new CJQColumn();
        ColTotal.Nombre = "Total";
        ColTotal.Encabezado = "Total";
        ColTotal.Buscador = "false";
        ColTotal.Alineacion = "right";
        ColTotal.Ancho = "70";
        ColTotal.Formato = "FormatoMoneda";
        GridOrdenCompra.Columnas.Add(ColTotal);

        //Saldo
        CJQColumn ColSaldo = new CJQColumn();
        ColSaldo.Nombre = "Saldo";
        ColSaldo.Encabezado = "Saldo";
        ColSaldo.Oculto = "true";
        ColSaldo.Buscador = "false";
        ColSaldo.Alineacion = "right";
        ColSaldo.Ancho = "80";
        ColSaldo.Formato = "FormatoMoneda";
        GridOrdenCompra.Columnas.Add(ColSaldo);

        CJQColumn ColAsociada = new CJQColumn();
        ColAsociada.Nombre = "Asociada";
        ColAsociada.Encabezado = "Asociada";
        ColAsociada.Buscador = "true";
        ColAsociada.TipoBuscador = "Combo";
        ColAsociada.StoredProcedure.CommandText = "sp_OrdenCompra_TipoAsociacion";
        ColAsociada.Alineacion = "center";
        ColAsociada.Ancho = "60";
        GridOrdenCompra.Columnas.Add(ColAsociada);

        //Nota
        CJQColumn ColNota = new CJQColumn();
        ColNota.Nombre = "Nota";
        ColNota.Encabezado = "Nota";
        ColNota.Buscador = "false";
        ColNota.Alineacion = "left";
        ColNota.Ancho = "80";
        GridOrdenCompra.Columnas.Add(ColNota);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "55";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBaja.Oculto = puedeEliminarOrdenCompra == 1 ? "false" : "true";
        GridOrdenCompra.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultarOC";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarOrdenCompra";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridOrdenCompra.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdOrdenCompra", GridOrdenCompra.GeneraGrid(), true);

        PintaGridDetalleOrdenCompra();
        PintaGridDetalleOrdenCompraConsulta();
        PintaGridDetalleOrdenCompraEditar();
        PintaGridDetallePedido();
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

        ticks = DateTime.Now.Ticks.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerOrdenCompra(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pRazonSocial, string pFolio, int pIdEstatusRecepcion, int pAI, string pFechaInicial, string pFechaFinal, int pPorFecha, int pAsociado, int pFolioPedido, int pNoProyecto)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdOrdenCompra", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("RazonSocial", SqlDbType.VarChar, 250).Value = Convert.ToString(pRazonSocial);
        Stored.Parameters.Add("Folio", SqlDbType.VarChar, 250).Value = Convert.ToString(pFolio);
        Stored.Parameters.Add("pFolioPedido", SqlDbType.Int).Value = pFolioPedido;
        Stored.Parameters.Add("pNoProyecto", SqlDbType.Int).Value = pNoProyecto;
        Stored.Parameters.Add("pIdEstatusRecepcion", SqlDbType.Int).Value = pIdEstatusRecepcion;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
        Stored.Parameters.Add("pFechaInicial", SqlDbType.VarChar, 255).Value = pFechaInicial;
        Stored.Parameters.Add("pFechaFinal", SqlDbType.VarChar, 255).Value = pFechaFinal;
        Stored.Parameters.Add("pPorFecha", SqlDbType.Int).Value = pPorFecha;
        Stored.Parameters.Add("pAsociado", SqlDbType.Int).Value = pAsociado;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerOrdenCompraDetalle(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdOrdenCompraEncabezado)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdOrdenCompraDetalle", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdOrdenCompraEncabezado", SqlDbType.Int).Value = pIdOrdenCompraEncabezado;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDetallePedido(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdPedido)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDetallePedidoOrdenCompraConsulta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 40).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdPedido", SqlDbType.Int).Value = pIdPedido;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerDatosDetallePedido(int pIdDetallePedido)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();

            CotizacionDetalle.LlenaObjeto(pIdDetallePedido, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCotizacionDetalle", CotizacionDetalle.IdCotizacionDetalle));
            Modelo.Add(new JProperty("IdProducto", CotizacionDetalle.IdProducto));
            Modelo.Add(new JProperty("IdServicio", CotizacionDetalle.IdServicio));
            Modelo.Add(new JProperty("Clave", CotizacionDetalle.Clave));
            Modelo.Add(new JProperty("ClaveProdServ", CotizacionDetalle.ClaveProdServ));
            Modelo.Add(new JProperty("Descripcion", CotizacionDetalle.Descripcion));
            Modelo.Add(new JProperty("Cantidad", CotizacionDetalle.Cantidad));
            Modelo.Add(new JProperty("OrdenCompraCantidad", CotizacionDetalle.OrdenDeCompraCantidad));
            Modelo.Add(new JProperty("Costo", CotizacionDetalle.PrecioUnitario));
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
    public static string BuscarFolio(int pFolio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonFolio = new CJson();
        jsonFolio.StoredProcedure.CommandText = "sp_OrdenCompra_ConsultarFiltros";
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonFolio.StoredProcedure.Parameters.AddWithValue("@pFolio", pFolio);
        return jsonFolio.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarRazonSocial(string pRazonSocial)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrganizacion jsonOrganizacion = new COrganizacion();
        jsonOrganizacion.StoredProcedure.CommandText = "sp_Organizacion_ConsultarFiltros";
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", pRazonSocial);
        jsonOrganizacion.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        return jsonOrganizacion.ObtenerJsonRazonSocial(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarProducto(Dictionary<string, object> pProducto)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CProducto jsonProducto = new CProducto();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_ConsultarFiltros";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);

        string Busqueda = Convert.ToString(pProducto["TipoBusqueda"]);

        if (Busqueda == Convert.ToString("P"))
        {
            jsonProducto.StoredProcedure.Parameters.AddWithValue("@pProducto", Convert.ToString(pProducto["Producto"]));
        }
        else
        {
            jsonProducto.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pProducto["Producto"]));
        }

        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        return jsonProducto.ObtenerJsonProducto(ConexionBaseDatos);

    }

    [WebMethod]
    public static string BuscarServicio(Dictionary<string, object> pServicio)
    {

        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CServicio jsonServicio = new CServicio();
        jsonServicio.StoredProcedure.CommandText = "sp_Servicio_ConsultarFiltros";
        jsonServicio.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);

        string Busqueda = Convert.ToString(pServicio["TipoBusqueda"]);

        if (Busqueda == Convert.ToString("P"))
        {
            jsonServicio.StoredProcedure.Parameters.AddWithValue("@pServicio", Convert.ToString(pServicio["Servicio"]));
        }
        else
        {
            jsonServicio.StoredProcedure.Parameters.AddWithValue("@pClave", Convert.ToString(pServicio["Servicio"]));
        }

        jsonServicio.StoredProcedure.Parameters.AddWithValue("@pBaja", false);
        string jsonServicioString = jsonServicio.ObtenerJsonServicio(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonServicioString;

    }

    [WebMethod]
    public static string ObtenerPrecioPorMoneda(Dictionary<string, object> pDato)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();


            if (Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()) != 0 && Convert.ToInt32(pDato["IdTipoMonedaDestino"].ToString()) != 0)
            {
                CTipoCambio TipoCambio = new CTipoCambio();
                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(pDato["IdTipoMonedaOrigen"].ToString()));
                Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(pDato["IdTipoMonedaDestino"].ToString()));
                Parametros.Add("Fecha", DateTime.Today);
                TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);

                Modelo.Add("MonedaPrecio", Convert.ToDecimal(pDato["Precio"].ToString()) / Convert.ToDecimal(TipoCambio.TipoCambio.ToString()));
                Modelo.Add("TipoCambioActual", TipoCambio.TipoCambio);
            }
            else
            {
                Modelo.Add("MonedaPrecio", Convert.ToDecimal(0));
                Modelo.Add("TipoCambioActual", 1);
            }

            oRespuesta.Add(new JProperty("Error", 0));
            Modelo.Add(new JProperty("Permisos", oPermisos));
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
    public static string ObtenerTipoCambio(int IdTipoMonedaOrigen, int IdTipoMonedaDestino, string ClaveAutorizacion, int IdOrdenCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        decimal vAutTipoCambio = 0;
        decimal vTipoCambio = 0;

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
        Parametros.Add("Fecha", DateTime.Today);

        CTipoCambio TipoCambio = new CTipoCambio();
        TipoCambio.LlenaObjetoFiltrosTipoCambio(Parametros, ConexionBaseDatos);

        Parametros.Clear();
        Parametros.Add("Opcion", 1);
        Parametros.Add("Baja", 0);
        Parametros.Add("IdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
        Parametros.Add("ClaveAutorizacion", ClaveAutorizacion);
        Parametros.Add("IdDocumento", IdOrdenCompra);
        Parametros.Add("TipoDocumento", "OrdenCompra");

        CAutorizacionTipoCambio AutTipoCambio = new CAutorizacionTipoCambio();
        AutTipoCambio.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
        vAutTipoCambio = AutTipoCambio.TipoCambio;

        Parametros.Clear();
        Parametros.Add("IdTipoMonedaOrigen", Convert.ToInt32(IdTipoMonedaOrigen));
        Parametros.Add("IdTipoMonedaDestino", Convert.ToInt32(IdTipoMonedaDestino));
        Parametros.Add("IdOrdenCompraEncabezado", IdOrdenCompra);
        Parametros.Add("Fecha", DateTime.Today);

        CTipoCambioOrdenCompra TipoCambioOC = new CTipoCambioOrdenCompra();
        TipoCambioOC.LlenaObjetoFiltros(Parametros, ConexionBaseDatos);
        vTipoCambio = TipoCambioOC.TipoCambio;

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Modelo.Add("TipoCambioActual", vTipoCambio > 0 ? vTipoCambio : vAutTipoCambio > 0 ? vAutTipoCambio : TipoCambio.TipoCambio);
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
    public static string ObtenerAutorizacionTipoCambio(int IdTipoMonedaOrigen, int IdTipoMonedaDestino, string ClaveAutorizacion, int IdDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionTipoCambio jsonTipoCambio = new CAutorizacionTipoCambio();
        jsonTipoCambio.StoredProcedure.CommandText = "sp_AutorizacionTipoCambio_Obtener";
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", IdTipoMonedaOrigen);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", IdTipoMonedaDestino);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", ClaveAutorizacion);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdDocumento", IdDocumento);
        string jsonTipoCambioString = jsonTipoCambio.ObtenerJsonAutorizacionTipoCambio(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonTipoCambioString;
    }

    [WebMethod]
    public static string DesactivaAutorizacionTipoCambio(int IdTipoMonedaOrigen, int IdTipoMonedaDestino, string ClaveAutorizacion)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CAutorizacionTipoCambio jsonTipoCambio = new CAutorizacionTipoCambio();
        jsonTipoCambio.StoredProcedure.CommandText = "sp_AutorizacionTipoCambio_Obtener";
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@Opcion", 2);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdUsuarioSolicito", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaOrigen", IdTipoMonedaOrigen);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pIdTipoMonedaDestino", IdTipoMonedaDestino);
        jsonTipoCambio.StoredProcedure.Parameters.AddWithValue("@pClaveAutorizacion", ClaveAutorizacion);
        return jsonTipoCambio.ObtenerJsonAutorizacionTipoCambio(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaFiltroOrdenCompraEncabezado()
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
    public static string BuscarProyectoCliente(string pNombreProyecto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CProyecto jsonProyecto = new CProyecto();
        jsonProyecto.StoredProcedure.CommandText = "sp_Proyecto_ConsultarNombreProyecto";
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pNombreProyecto", pNombreProyecto);
        jsonProyecto.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        return jsonProyecto.ObtenerJsonNombreProyecto(ConexionBaseDatos);
    }

    [WebMethod]
    public static string ObtenerFormaOrdenCompra(int pIdOrdenCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarOrdenCompra = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarOrdenCompra" }, ConexionBaseDatos) == "")
        {
            puedeEditarOrdenCompra = 1;
        }
        oPermisos.Add("puedeEditarOrdenCompra", puedeEditarOrdenCompra);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = COrdenCompraEncabezado.ObtenerOrdenCompraEncabezado(Modelo, pIdOrdenCompra, ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarOrdenCompra()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CMunicipio Municipio = new CMunicipio();
        CEstado Estado = new CEstado();
        CTipoCambio TipoCambio = new CTipoCambio();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
        Municipio.LlenaObjeto(Sucursal.IdMunicipio, ConexionBaseDatos);
        Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            COrganizacion Organizacion = new COrganizacion();
            string direccionEntrega = "";

            if (Sucursal.DireccionFiscal == true)
            {
                CEmpresa Empresa = new CEmpresa();
                Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);
                Municipio.LlenaObjeto(Empresa.IdMunicipio, ConexionBaseDatos);
                Estado.LlenaObjeto(Municipio.IdEstado, ConexionBaseDatos);
                if (Empresa.Calle != "")
                {
                    direccionEntrega = Empresa.Calle + " " + Empresa.NumeroExterior + ", " + Empresa.Colonia + ", " + Municipio.Municipio + ", " + Estado.Estado;
                }
            }
            else
            {
                if (Sucursal.Calle != "")
                {
                    direccionEntrega = Sucursal.Calle + " " + Sucursal.NumeroExterior + ", " + Sucursal.Colonia + ", " + Municipio.Municipio + ", " + Estado.Estado;
                }
            }

            Modelo.Add("FechaOrdenCompra", DateTime.Now.ToShortDateString());
            Modelo.Add("Divisiones", CSucursalDivision.ObtenerJsonSucursalDivision(Convert.ToInt32(HttpContext.Current.Session["IdSucursal"]), ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(Sucursal.IdTipoMoneda, ConexionBaseDatos));
            Modelo.Add("UnidadesCompraVenta", CUnidadCompraVenta.ObtenerJsonUnidadesCompraVenta(ConexionBaseDatos));
            Modelo.Add("TiposCompra", CJson.ObtenerJsonTipoCompra(ConexionBaseDatos));
            Modelo.Add("DireccionEntrega", direccionEntrega);
            Modelo.Add(new JProperty("FechaEntrega", Convert.ToDateTime(DateTime.Now).ToShortDateString()));

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
    public static string ObtenerFormaEditarOrdenCompra(int pIdOrdenCompra)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarOrdenCompra = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        CSucursal Sucursal = new CSucursal();
        CTipoCambio TipoCambio = new CTipoCambio();
        COrdenCompraEncabezado OrdenCompra = new COrdenCompraEncabezado();
        CProveedor Proveedor = new CProveedor();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        Sucursal.LlenaObjeto(Usuario.IdSucursalPredeterminada, ConexionBaseDatos);
        OrdenCompra.LlenaObjeto(pIdOrdenCompra, ConexionBaseDatos);
        Proveedor.LlenaObjeto(OrdenCompra.IdProveedor, ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarOrdenCompra" }, ConexionBaseDatos) == "")
        {
            puedeEditarOrdenCompra = 1;
        }
        oPermisos.Add("puedeEditarOrdenCompra", puedeEditarOrdenCompra);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            COrganizacion Organizacion = new COrganizacion();

            Modelo = COrdenCompraEncabezado.ObtenerOrdenCompraEncabezado(Modelo, pIdOrdenCompra, ConexionBaseDatos);
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(OrdenCompra.IdDivision, ConexionBaseDatos));
            Modelo.Add("TipoMonedas", CTipoMoneda.ObtenerJsonTiposMoneda(OrdenCompra.IdTipoMoneda, ConexionBaseDatos));
            Modelo.Add("UnidadesCompraVenta", CUnidadCompraVenta.ObtenerJsonUnidadesCompraVenta(ConexionBaseDatos));
            Modelo.Add("TiposCompra", CJson.ObtenerJsonTipoCompra(ConexionBaseDatos));
            Modelo.Add("IVAProveedor", Proveedor.IVAActual);
            Modelo.Add("Pedidos", CCotizacion.ObtenerPedidosCliente(Convert.ToInt32(OrdenCompra.IdCliente), OrdenCompra.IdPedidoEncabezado, ConexionBaseDatos));
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
    public static string obtenerProducto(int IdProducto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CProducto Producto = new CProducto();
            CTipoMoneda TipoMoneda = new CTipoMoneda();

            Producto.LlenaObjeto(IdProducto, ConexionBaseDatos);
            TipoMoneda.LlenaObjeto(Producto.IdTipoMoneda, ConexionBaseDatos);

            Modelo.Add("Descripcion", Producto.Descripcion +  " No. parte: " + Producto.NumeroParte);
            Modelo.Add("ClaveProdServ", Producto.ClaveProdServ);
            Modelo.Add("IdTipoIVA", Producto.IdTipoIVA);
            if (Producto.IdTipoIVA == 1)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                Modelo.Add("IVA", Sucursal.IVAActual);
            }
            else
            {
                Modelo.Add("IVA", 0);
            }

            Modelo.Add("Costo", Producto.Costo);
            Modelo.Add("IdTipoMonedaProducto", Producto.IdTipoMoneda);
            Modelo.Add("TipoMonedaProducto", TipoMoneda.TipoMoneda);
            Modelo.Add("IdUnidadCompraVenta", Producto.IdUnidadCompraVenta);
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
    public static string obtenerServicio(int IdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CServicio Servicio = new CServicio();
            CTipoMoneda TipoMoneda = new CTipoMoneda();

            Servicio.LlenaObjeto(IdServicio, ConexionBaseDatos);
            TipoMoneda.LlenaObjeto(Servicio.IdTipoMoneda, ConexionBaseDatos);
            Modelo.Add("Descripcion", Servicio.Servicio);
            Modelo.Add("ClaveProdServ", Servicio.ClaveProdServ);

            Modelo.Add("IdTipoIVA", Servicio.IdTipoIVA);
            if (Servicio.IdTipoIVA == 1)
            {
                CUsuario Usuario = new CUsuario();
                Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
                CSucursal Sucursal = new CSucursal();
                Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);
                Modelo.Add("IVA", Sucursal.IVAActual);
            }
            else
            {
                Modelo.Add("IVA", 0);
            }


            Modelo.Add("Costo", Servicio.Precio);
            Modelo.Add("IdTipoMonedaServicio", Servicio.IdTipoMoneda);
            Modelo.Add("TipoMonedaServicio", TipoMoneda.TipoMoneda);
            Modelo.Add("IdUnidadCompraVenta", Servicio.IdUnidadCompraVenta);
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
    public static string ObtenerOrdenCompraIndicadores(string pFechaInicial, string pFechaFinal, int pPorFecha, int pFolio, string pRazonSocial, int pAI)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        COrdenCompraEncabezado jsonOrdenCompra = new COrdenCompraEncabezado();
        jsonOrdenCompra.StoredProcedure.CommandText = "sp_OrdenCompra_ConsultarIndicadoresRecepcion";
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@Opcion", 1);
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pIdUsuario", Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]));
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pFechaInicial", Convert.ToString(pFechaInicial));
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pFechaFinal", Convert.ToString(pFechaFinal));
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pPorFecha", Convert.ToInt32(pPorFecha));
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pFolio", Convert.ToInt32(pFolio));
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pRazonSocial", Convert.ToString(pRazonSocial));
        jsonOrdenCompra.StoredProcedure.Parameters.AddWithValue("@pBaja", Convert.ToInt32(pAI));
        return jsonOrdenCompra.ObtenerJsonIndicadores(ConexionBaseDatos);

    }

    [WebMethod]
    public static string obtenerPedidosCliente(int IdCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CCotizacion.ObtenerPedidosClienteOrdenCompra(Convert.ToInt32(IdCliente), ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir un pedido...");

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

    // DML - INI //
    [WebMethod]
    public static string AgregarOrdenCompra(Dictionary<string, object> pOrdenCompra)
    {
        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
            if (Error == 0)
            {

                COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
                OrdenCompraEncabezado.LlenaObjeto(OrdenCompraEncabezado.IdOrdenCompraEncabezado, pConexion);

                if (OrdenCompraEncabezado.IdOrdenCompraEncabezado == 0)
                {
                    OrdenCompraEncabezado.AgregarOrdenCompraEncabezado(pConexion);
                    COrdenCompraEncabezadoSucursal OrdenCompraEncabezadoSucursal = new COrdenCompraEncabezadoSucursal();

                    OrdenCompraEncabezadoSucursal.IdOrdenCompraEncabezado = OrdenCompraEncabezado.IdOrdenCompraEncabezado;
                    OrdenCompraEncabezadoSucursal.IdSucursal = UsuarioSesion.IdSucursalActual;
                    OrdenCompraEncabezadoSucursal.FechaAlta = Convert.ToDateTime(DateTime.Now);
                    OrdenCompraEncabezadoSucursal.IdUsuarioAlta = Convert.ToInt32(UsuarioSesion.IdUsuario);
                    OrdenCompraEncabezadoSucursal.Agregar(pConexion);

                }

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", Descripcion);
        });

        return Respuesta.ToString();

    }

    [WebMethod]
    public static string EditarOrdenCompra(Dictionary<string, object> pOrdenCompra)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        CUsuario Usuario = new CUsuario();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
            OrdenCompraEncabezado.LlenaObjeto(Convert.ToInt32(pOrdenCompra["IdOrdenCompra"]), ConexionBaseDatos);
            OrdenCompraEncabezado.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            OrdenCompraEncabezado.IdProveedor = Convert.ToInt32(pOrdenCompra["IdProveedor"]);
            OrdenCompraEncabezado.IdTipoMoneda = Convert.ToInt32(pOrdenCompra["IdTipoMoneda"]);
            OrdenCompraEncabezado.IdProyecto = Convert.ToInt32(pOrdenCompra["IdProyecto"]);
            OrdenCompraEncabezado.IdDivision = Convert.ToInt32(pOrdenCompra["IdDivision"]);
            OrdenCompraEncabezado.IdCliente = Convert.ToInt32(pOrdenCompra["IdCliente"]);
            OrdenCompraEncabezado.IdPedidoEncabezado = Convert.ToInt32(pOrdenCompra["IdPedido"]);
            OrdenCompraEncabezado.FechaAlta = Convert.ToDateTime(pOrdenCompra["FechaAlta"]);
            OrdenCompraEncabezado.FechaRequerida = Convert.ToDateTime(pOrdenCompra["FechaRequerida"]);
            OrdenCompraEncabezado.DireccionEntrega = Convert.ToString(pOrdenCompra["DireccionEntrega"]);
            OrdenCompraEncabezado.Nota = Convert.ToString(pOrdenCompra["Nota"]);
            OrdenCompraEncabezado.IVA = Convert.ToDecimal(pOrdenCompra["IVA"]);
            OrdenCompraEncabezado.Subtotal = Convert.ToDecimal(pOrdenCompra["Subtotal"]);
            OrdenCompraEncabezado.Total = Convert.ToDecimal(pOrdenCompra["Total"]);
            OrdenCompraEncabezado.Saldo = Convert.ToDecimal(pOrdenCompra["Saldo"]);
            OrdenCompraEncabezado.TipoCambio = Convert.ToDecimal(pOrdenCompra["TipoCambio"]);
            OrdenCompraEncabezado.Consolidado = Convert.ToBoolean(pOrdenCompra["Consolidado"]);
            OrdenCompraEncabezado.SinPedido = Convert.ToBoolean(pOrdenCompra["SinPedido"]);
            OrdenCompraEncabezado.CantidadTotalLetra = Convert.ToString(pOrdenCompra["CantidadTotalLetra"]);

            string validacion = ValidarOrdenCompraEncabezado(OrdenCompraEncabezado, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                oRespuesta.Add("IdOrdenCompraEncabezado", OrdenCompraEncabezado.IdOrdenCompraEncabezado);
                oRespuesta.Add("Folio", OrdenCompraEncabezado.Folio);
                oRespuesta.Add(new JProperty("Error", 0));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
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
    public static string ConsolidarOrdenCompra(Dictionary<string, object> pOrdenCompra)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        CUsuario Usuario = new CUsuario();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
            OrdenCompraEncabezado.LlenaObjeto(Convert.ToInt32(pOrdenCompra["IdOrdenCompra"]), ConexionBaseDatos);

            JObject oRespuesta = new JObject();

            OrdenCompraEncabezado.Consolidado = true;
            OrdenCompraEncabezado.Editar(ConexionBaseDatos);

            oRespuesta.Add(new JProperty("Error", 0));
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string AgregarOrdenCompraDetalle(Dictionary<string, object> pOrdenCompra)
    {

        //Datos de Detalle
        int Cantidad = Convert.ToInt32(pOrdenCompra["Cantidad"]);//
        int RecepcionCantidad = Convert.ToInt32(pOrdenCompra["Cantidad"]);//----------
        int Descuento = Convert.ToInt32(pOrdenCompra["Descuento"]);//------------------
        decimal Costo = Convert.ToDecimal(pOrdenCompra["Costo"]);//
        string Descripcion = Convert.ToString(pOrdenCompra["DescripcionDetallePartida"]);
        int IdUnidadCompraVenta = Convert.ToInt32(pOrdenCompra["IdUnidadCompraVenta"]);//------------
        int IdPedidoDetalle = Convert.ToInt32(pOrdenCompra["IdPedidoDetalle"]);//-------------
        int IdTipoIVA = Convert.ToInt32(pOrdenCompra["IdTipoIVA"]);//
        int IVA = Convert.ToInt32(pOrdenCompra["IVA"]);//
        int IdProducto = Convert.ToInt32(pOrdenCompra["IdProducto"]);//----------
        int IdServicio = Convert.ToInt32(pOrdenCompra["IdServicio"]);//------
        decimal CostoDescuento = Convert.ToDecimal(pOrdenCompra["CostoDescuento"]);//
        string ClaveProdServ = Convert.ToString(pOrdenCompra["ClaveProdServ"]);

        //Datos Compartidos
        int IdPedidoEncabezado = Convert.ToInt32(pOrdenCompra["IdPedidoEncabezado"]);//------------------
        int IdAutorizacionTipoCambio = Convert.ToInt32(pOrdenCompra["IdAutorizacionTipoCambio"]);//
        int IdProyecto = Convert.ToInt32(pOrdenCompra["IdProyecto"]);//---------------

        //Datos del encabezado
        int IdOrdenCompra = Convert.ToInt32(pOrdenCompra["IdOrdenCompra"]);//
        int IdProveedor = Convert.ToInt32(pOrdenCompra["IdProveedor"]);//
        int IdTipoMoneda = Convert.ToInt32(pOrdenCompra["IdTipoMoneda"]);//
        int IdDivision = Convert.ToInt32(pOrdenCompra["IdDivision"]);//
        DateTime FechaAlta = Convert.ToDateTime(pOrdenCompra["FechaAlta"]);//
        string DireccionEntrega = Convert.ToString(pOrdenCompra["DireccionEntrega"]);//
        string Nota = Convert.ToString(pOrdenCompra["Nota"]);//
        decimal TipoCambio = Convert.ToDecimal(pOrdenCompra["TipoCambio"]);//
        int SinPedido = Convert.ToInt32(pOrdenCompra["SinPedido"]);//
        int IdTipoCompra = Convert.ToInt32(pOrdenCompra["IdTipoCompra"]);//-----------
        int IdCliente = Convert.ToInt32(pOrdenCompra["IdCliente"]);//
        DateTime FechaRequerida = Convert.ToDateTime(pOrdenCompra["FechaRequerida"]);

        JObject Respuesta = new JObject();
        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionE, CUsuario UsuarioSesion) {
            if (Error == 0)
            {
                // Validacion 
                int puedeAgregarOrdenCompra = UsuarioSesion.TienePermisos(new string[] { "puedeAgregarOrdenCompra" }, pConexion) == "" ? 1 : 0;
                int puedeEditarOrdenCompra = UsuarioSesion.TienePermisos(new string[] { "puedeEditarOrdenCompra" }, pConexion) == "" ? 1 : 0;
                int puedeEliminarOrdenCompra = UsuarioSesion.TienePermisos(new string[] { "puedeEliminarOrdenCompra" }, pConexion) == "" ? 1 : 0;

                CCotizacion Cotizacion = new CCotizacion();
                Dictionary<string, object> pParametros = new Dictionary<string, object>();
                pParametros.Add("IdCotizacion", IdPedidoEncabezado);
                pParametros.Add("Baja", 0);
                Cotizacion.LlenaObjetoFiltros(pParametros, pConexion);

                CCotizacionDetalle CotizacionDetalle = new CCotizacionDetalle();
                pParametros.Add("IdCotizacionDetalle", IdPedidoDetalle);
                CotizacionDetalle.LlenaObjetoFiltros(pParametros, pConexion);
                pParametros.Clear();

                CProyecto Proyecto = new CProyecto();
                pParametros.Add("IdProyecto", IdProyecto);
                pParametros.Add("Baja", 0);
                Proyecto.LlenaObjetoFiltros(pParametros, pConexion);

                CCliente Cliente = new CCliente();
                Cliente.LlenaObjeto(IdCliente, pConexion);
                COrganizacion Organizacion = new COrganizacion();
                Organizacion.LlenaObjeto(Cliente.IdOrganizacion, pConexion);

                COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
                if (IdOrdenCompra != 0) {
                    OrdenCompraEncabezado.LlenaObjeto(IdOrdenCompra, pConexion);
                    OrdenCompraEncabezado.IdTipoMoneda = IdTipoMoneda;
                    OrdenCompraEncabezado.IdProveedor = IdProveedor;
                    OrdenCompraEncabezado.IdDivision = IdDivision;
                    OrdenCompraEncabezado.Editar(pConexion);
                } else {
                    OrdenCompraEncabezado.Baja = false;
                    OrdenCompraEncabezado.IdTipoMoneda = IdTipoMoneda;
                    OrdenCompraEncabezado.IdProyecto = IdProyecto;
                    OrdenCompraEncabezado.IdProveedor = IdProveedor;
                    OrdenCompraEncabezado.IdCliente = IdCliente;
                    OrdenCompraEncabezado.IdDivision = IdDivision;
                    OrdenCompraEncabezado.IdPedidoEncabezado = IdPedidoEncabezado;
                    OrdenCompraEncabezado.IdUsuario = UsuarioSesion.IdUsuario;
                    OrdenCompraEncabezado.Nota = Nota;
                    OrdenCompraEncabezado.DireccionEntrega = DireccionEntrega;
                    OrdenCompraEncabezado.TipoCambio = TipoCambio;
                    OrdenCompraEncabezado.Consolidado = false;
                    OrdenCompraEncabezado.FechaAlta = DateTime.Today;
                    OrdenCompraEncabezado.FechaRequerida = FechaRequerida;
                    OrdenCompraEncabezado.FechaRecepcion = DateTime.Now;
                    OrdenCompraEncabezado.SinPedido = (SinPedido == 1);

                    string ClienteProyecto = "Sin Pedido/Proyecto";

                    if (Proyecto.IdProyecto != 0 && !OrdenCompraEncabezado.SinPedido) {
                        ClienteProyecto = Proyecto.NombreProyecto;
                    } else if (Organizacion.IdOrganizacion != 0 && !OrdenCompraEncabezado.SinPedido) {
                        ClienteProyecto = Organizacion.RazonSocial;
                    }

                    OrdenCompraEncabezado.ClienteProyecto = ClienteProyecto;
                    OrdenCompraEncabezado.Folio = COrdenCompraEncabezado.ObtenerSiguienteFolio(UsuarioSesion.IdSucursalActual, pConexion);

                    OrdenCompraEncabezado.Agregar(pConexion);

                    COrdenCompraEncabezadoSucursal OrdenCompraEncabezadoSucursal = new COrdenCompraEncabezadoSucursal();
                    OrdenCompraEncabezadoSucursal.IdOrdenCompraEncabezado = OrdenCompraEncabezado.IdOrdenCompraEncabezado;
                    OrdenCompraEncabezadoSucursal.IdSucursal = UsuarioSesion.IdSucursalActual;
                    OrdenCompraEncabezadoSucursal.IdUsuarioAlta = UsuarioSesion.IdUsuario;
                    OrdenCompraEncabezadoSucursal.FechaAlta = DateTime.Now;
                    OrdenCompraEncabezadoSucursal.Agregar(pConexion);
                }

                if ((OrdenCompraEncabezado.IdOrdenCompraEncabezado != 0) && ((CotizacionDetalle.IdCotizacionDetalle != 0) || (Proyecto.IdProyecto != 0) || (SinPedido == 1))) {
                    CProducto Producto = new CProducto();
                    Producto.LlenaObjeto(IdProducto, pConexion);
                    CServicio Servicio = new CServicio();
                    Servicio.LlenaObjeto(IdServicio, pConexion);

                    COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();
                    OrdenCompraDetalle.Cantidad = Cantidad;
                    OrdenCompraDetalle.Costo = Costo;
                    OrdenCompraDetalle.Descuento = Descuento;
                    OrdenCompraDetalle.Descripcion = Descripcion;
                    OrdenCompraDetalle.Total = (Costo - (Costo * ((100 - (100 - Descuento)) / 100))) * Cantidad;
                    OrdenCompraDetalle.Clave = Producto.Clave;
                    OrdenCompraDetalle.Saldo = OrdenCompraDetalle.Total;
                    OrdenCompraDetalle.IdPedidoEncabezado = IdPedidoEncabezado;
                    OrdenCompraDetalle.IdPedidoDetalle = IdPedidoDetalle;
                    OrdenCompraDetalle.Descuento = Descuento;
                    OrdenCompraDetalle.IdOrdenCompraEncabezado = OrdenCompraEncabezado.IdOrdenCompraEncabezado;
                    OrdenCompraDetalle.IdTipoCompra = IdTipoCompra;
                    OrdenCompraDetalle.IdProducto = IdProducto;
                    OrdenCompraDetalle.IdUnidadCompraVenta = IdUnidadCompraVenta;
                    OrdenCompraDetalle.IdServicio = IdServicio;
                    OrdenCompraDetalle.FechaAlta = FechaAlta;
                    OrdenCompraDetalle.RecepcionCantidad = RecepcionCantidad;
                    OrdenCompraDetalle.IdTipoIVA = IdTipoIVA;
                    OrdenCompraDetalle.IVA = IVA;
                    OrdenCompraDetalle.ClaveProdServ = ClaveProdServ;
                    OrdenCompraDetalle.Agregar(pConexion);

                }

                CotizacionDetalle.ClaveProdServ = ClaveProdServ;
                if (IdProducto != 0)
                {
                    CProducto producto = new CProducto();
                    producto.LlenaObjeto(IdProducto,pConexion);
                    producto.ClaveProdServ = Convert.ToString(ClaveProdServ);
                    producto.Editar(pConexion);

                }
                else
                {
                    CServicio servicio = new CServicio();
                    servicio.LlenaObjeto(IdServicio, pConexion);
                    servicio.ClaveProdServ = ClaveProdServ;
                    servicio.Editar(pConexion);
                }

                if (CotizacionDetalle.IdCotizacionDetalle != 0 && CotizacionDetalle.OrdenDeCompraCantidad > 0) {
                    CotizacionDetalle.OrdenDeCompraCantidad = CotizacionDetalle.OrdenDeCompraCantidad - Cantidad;
                    CotizacionDetalle.Editar(pConexion);
                }

                COrdenCompraDetalle Partidas = new COrdenCompraDetalle();
                pParametros.Clear();
                pParametros.Add("IdOrdenCompraEncabezado", OrdenCompraEncabezado.IdOrdenCompraEncabezado);
                pParametros.Add("Baja", 0);

                decimal dSubtotal = 0;
                decimal dIVA = 0;

                foreach (COrdenCompraDetalle Partida in Partidas.LlenaObjetosFiltros(pParametros, pConexion)) {
                    dSubtotal += (Partida.Cantidad * Partida.Costo) - (Partida.Cantidad * Partida.Costo) * (Partida.Descuento / 100);
                    dIVA += ((Partida.Cantidad * Partida.Costo) - (Partida.Cantidad * Partida.Costo) * (Partida.Descuento / 100)) * (Partida.IVA / 100);
                }

                OrdenCompraEncabezado.Subtotal = dSubtotal;
                OrdenCompraEncabezado.IVA = dIVA;
                OrdenCompraEncabezado.Total = dSubtotal + dIVA;
                OrdenCompraEncabezado.Saldo = dSubtotal + dIVA;
                OrdenCompraEncabezado.Editar(pConexion);

                Respuesta.Add("IdOrdenCompraEncabezado", OrdenCompraEncabezado.IdOrdenCompraEncabezado);
                Respuesta.Add("SubtotalDetalle", OrdenCompraEncabezado.Subtotal);
                Respuesta.Add("IVADetalle", OrdenCompraEncabezado.IVA);
                Respuesta.Add("TotalDetalle", OrdenCompraEncabezado.Total);
                Respuesta.Add("Folio", OrdenCompraEncabezado.Folio);

            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", DescripcionE);
        });
        return Respuesta.ToString();

    }

    [WebMethod]
    public static string EliminarOrdenCompraDetalle(Dictionary<string, object> pOrdenCompraDetalle)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        JObject Modelo = new JObject();

        COrdenCompraDetalle OrdenCompraDetalle = new COrdenCompraDetalle();
        OrdenCompraDetalle.LlenaObjeto(Convert.ToInt32(pOrdenCompraDetalle["pIdOrdenCompraDetalle"]), ConexionBaseDatos);
        OrdenCompraDetalle.IdOrdenCompraDetalle = Convert.ToInt32(pOrdenCompraDetalle["pIdOrdenCompraDetalle"]);
        OrdenCompraDetalle.Baja = true;
        OrdenCompraDetalle.EliminarOrdenCompraDetalle(ConexionBaseDatos);

        //total = OrdenCompraDetalle.Total;

        /*** INI - Restaura saldos Cotizacion/Pedido ***/
        CCotizacionDetalle PedidoDetalle = new CCotizacionDetalle();
        PedidoDetalle.LlenaObjeto(OrdenCompraDetalle.IdPedidoDetalle, ConexionBaseDatos);
        PedidoDetalle.OrdenDeCompraCantidad += OrdenCompraDetalle.Cantidad;
        PedidoDetalle.OrdenDeCompra = Convert.ToDateTime(null);
        PedidoDetalle.Editar(ConexionBaseDatos);
        /*** FIN - Restaura saldos Cotizacion/Pedido ***/

        COrdenCompraEncabezado OrdenCompra = new COrdenCompraEncabezado();
        OrdenCompra.LlenaObjeto(OrdenCompraDetalle.IdOrdenCompraEncabezado, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        oRespuesta.Add("SubtotalDetalle", OrdenCompra.Subtotal);
        oRespuesta.Add("IVADetalle", OrdenCompra.IVA);
        oRespuesta.Add("TotalDetalle", OrdenCompra.Total);
        oRespuesta.Add(new JProperty("Error", 0));
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdOrdenCompraEncabezado, bool pBaja)
    {
        JObject Respuesta = new JObject();

        bool reactiva = true;
        bool cancela = true;

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion){
            // Validacion 
            int puedeAgregarOrdenCompra = UsuarioSesion.TienePermisos(new string[] { "puedeAgregarOrdenCompra" }, pConexion) == "" ? 1 : 0;
            int puedeEditarOrdenCompra = UsuarioSesion.TienePermisos(new string[] { "puedeEditarOrdenCompra" }, pConexion) == "" ? 1 : 0;
            int puedeEliminarOrdenCompra = UsuarioSesion.TienePermisos(new string[] { "puedeEliminarOrdenCompra" }, pConexion) == "" ? 1 : 0;

            if (Error == 0) {
                COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
                OrdenCompraEncabezado.IdOrdenCompraEncabezado = pIdOrdenCompraEncabezado;
                OrdenCompraEncabezado.Baja = pBaja;

                COrdenCompraDetalle ListaPartidas = new COrdenCompraDetalle();

                Dictionary<string, object> ParametrosOCD = new Dictionary<string, object>();
                ParametrosOCD.Add("Baja", 0);
                ParametrosOCD.Add("IdOrdenCompraEncabezado", pIdOrdenCompraEncabezado);

                if (pBaja) {
                    foreach (COrdenCompraDetalle oOrdenCompraDetalle in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, pConexion)) {
                        int vCantidad = oOrdenCompraDetalle.RecepcionCantidad;
                        vCantidad -= oOrdenCompraDetalle.Cantidad;
                        cancela = !(vCantidad < 0);
                        if (!cancela) break;
                    }

                    if (cancela) {
                        if (puedeEliminarOrdenCompra == 1) {
                            foreach (COrdenCompraDetalle oOrdenCompraDetalle in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, pConexion)) {
                                CCotizacionDetalle PedidoDetalle = new CCotizacionDetalle();
                                PedidoDetalle.LlenaObjeto(oOrdenCompraDetalle.IdPedidoDetalle, pConexion);
                                PedidoDetalle.OrdenDeCompraCantidad += oOrdenCompraDetalle.Cantidad;
                                PedidoDetalle.OrdenDeCompraCantidad = PedidoDetalle.OrdenDeCompraCantidad > PedidoDetalle.Cantidad ? PedidoDetalle.Cantidad : PedidoDetalle.OrdenDeCompraCantidad;
                                PedidoDetalle.OrdenDeCompra = new DateTime(1, 1, 1);
                                PedidoDetalle.Editar(pConexion);
                            }
                            OrdenCompraEncabezado.Eliminar(pConexion);
                            Error = 0;
                            Descripcion = "OrdenCompraEncabezado Eliminado";
                        } else {
                            Error = 1;
                            Descripcion = "El saldo de alguna de las pártidas del pedido fue tomada por otra orden de compra. \n No se puede reactivar la orden de compra.";
                        }
                    } else {
                        Error = 1;
                        Descripcion = "No se puede cancelar esta orden de compra, ya que alguna de sus partidas ya esta recepcionada.";
                    }
                } else {
                    foreach (COrdenCompraDetalle oOrdenCompraDetalle in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, pConexion))
                    {
                        CCotizacionDetalle PedidoDetalle = new CCotizacionDetalle();
                        PedidoDetalle.LlenaObjeto(oOrdenCompraDetalle.IdPedidoDetalle, pConexion);
                        int vCantidad = PedidoDetalle.OrdenDeCompraCantidad;
                        vCantidad -= oOrdenCompraDetalle.Cantidad;
                        reactiva = !(vCantidad < 0);
                        if (!reactiva) break;
                    }

                    if (reactiva) {
                        foreach (COrdenCompraDetalle oOrdenCompraDetalle in ListaPartidas.LlenaObjetosFiltros(ParametrosOCD, pConexion)) {
                            CCotizacionDetalle PedidoDetalle = new CCotizacionDetalle();
                            PedidoDetalle.LlenaObjeto(oOrdenCompraDetalle.IdPedidoDetalle, pConexion);
                            PedidoDetalle.OrdenDeCompraCantidad -= oOrdenCompraDetalle.Cantidad;
                            PedidoDetalle.Editar(pConexion);
                        }
                    }

                    if (reactiva) {
                        OrdenCompraEncabezado.Eliminar(pConexion);
                        Error = 0;
                        Descripcion = "OrdenCompraEncabezado Eliminado";
                    } else {
                        Error = 1;
                        Descripcion = "El saldo de alguna de las pártidas del pedido fue tomada por otra orden de compra. \n No se puede reactivar la orden de compra.";
                    }
                }
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", Descripcion);
        });

        return Respuesta.ToString();
    }

    // DML - FIN//
    private void PintaGridDetalleOrdenCompra()
    {
        //GridOrdenCompraDetalle
        CJQGrid grdOrdenCompraDetalle = new CJQGrid();
        grdOrdenCompraDetalle.NombreTabla = "grdOrdenCompraDetalle";
        grdOrdenCompraDetalle.CampoIdentificador = "IdOrdenCompraDetalle";
        grdOrdenCompraDetalle.ColumnaOrdenacion = "IdOrdenCompraDetalle";
        grdOrdenCompraDetalle.TipoOrdenacion = "ASC";
        grdOrdenCompraDetalle.Metodo = "ObtenerOrdenCompraDetalle";
        grdOrdenCompraDetalle.TituloTabla = "Detalle de orden de compra";
        grdOrdenCompraDetalle.GenerarGridCargaInicial = false;
        grdOrdenCompraDetalle.GenerarFuncionFiltro = false;
        grdOrdenCompraDetalle.GenerarFuncionTerminado = false;
        grdOrdenCompraDetalle.Altura = 150;
        grdOrdenCompraDetalle.Ancho = 845;
        grdOrdenCompraDetalle.NumeroRegistros = 15;
        grdOrdenCompraDetalle.RangoNumeroRegistros = "15,30,60";

        //IdOrdenCompraDetalle
        CJQColumn ColIdOrdenCompraDetalle = new CJQColumn();
        ColIdOrdenCompraDetalle.Nombre = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Oculto = "true";
        ColIdOrdenCompraDetalle.Encabezado = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Buscador = "false";
        grdOrdenCompraDetalle.Columnas.Add(ColIdOrdenCompraDetalle);

        //ClaveDetalle
        CJQColumn ColClaveDetalle = new CJQColumn();
        ColClaveDetalle.Nombre = "Clave";
        ColClaveDetalle.Encabezado = "Clave";
        ColClaveDetalle.Buscador = "false";
        ColClaveDetalle.Alineacion = "left";
        ColClaveDetalle.Ancho = "50";
        grdOrdenCompraDetalle.Columnas.Add(ColClaveDetalle);

        //DescripcionDetalle
        CJQColumn ColDescripcionDetalle = new CJQColumn();
        ColDescripcionDetalle.Nombre = "Descripcion";
        ColDescripcionDetalle.Encabezado = "Descripción";
        ColDescripcionDetalle.Buscador = "false";
        ColDescripcionDetalle.Alineacion = "left";
        ColDescripcionDetalle.Ancho = "120";
        grdOrdenCompraDetalle.Columnas.Add(ColDescripcionDetalle);

        //ClaveDetalle
        CJQColumn ColClaveProdServDetalle = new CJQColumn();
        ColClaveProdServDetalle.Nombre = "ClaveProdServ";
        ColClaveProdServDetalle.Encabezado = "Clave (SAT)";
        ColClaveProdServDetalle.Buscador = "false";
        ColClaveProdServDetalle.Alineacion = "left";
        ColClaveProdServDetalle.Ancho = "50";
        grdOrdenCompraDetalle.Columnas.Add(ColClaveProdServDetalle);

        //CantidadDetalle
        CJQColumn ColCantidadDetalle = new CJQColumn();
        ColCantidadDetalle.Nombre = "Cantidad";
        ColCantidadDetalle.Encabezado = "Cantidad";
        ColCantidadDetalle.Buscador = "false";
        ColCantidadDetalle.Alineacion = "right";
        ColCantidadDetalle.Ancho = "50";
        grdOrdenCompraDetalle.Columnas.Add(ColCantidadDetalle);

        //CostoDetalle
        CJQColumn ColCostoDetalle = new CJQColumn();
        ColCostoDetalle.Nombre = "Costo";
        ColCostoDetalle.Encabezado = "Costo";
        ColCostoDetalle.Buscador = "false";
        ColCostoDetalle.Alineacion = "right";
        ColCostoDetalle.Ancho = "50";
        ColCostoDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalle.Columnas.Add(ColCostoDetalle);

        //SubtotalDetalle
        CJQColumn ColSubtotalDetalle = new CJQColumn();
        ColSubtotalDetalle.Nombre = "Subtotal";
        ColSubtotalDetalle.Encabezado = "Subtotal";
        ColSubtotalDetalle.Buscador = "false";
        ColSubtotalDetalle.Oculto = "true";
        ColSubtotalDetalle.Alineacion = "right";
        ColSubtotalDetalle.Ancho = "50";
        ColSubtotalDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalle.Columnas.Add(ColSubtotalDetalle);

        //IVADetalle
        CJQColumn ColIVADetalle = new CJQColumn();
        ColIVADetalle.Nombre = "IVA";
        ColIVADetalle.Encabezado = "IVA";
        ColIVADetalle.Buscador = "false";
        ColIVADetalle.Oculto = "false";
        ColIVADetalle.Alineacion = "right";
        ColIVADetalle.Ancho = "50";
        ColIVADetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalle.Columnas.Add(ColIVADetalle);

        //TotalDetalle
        CJQColumn ColTotalDetalle = new CJQColumn();
        ColTotalDetalle.Nombre = "Total";
        ColTotalDetalle.Encabezado = "Total";
        ColTotalDetalle.Buscador = "false";
        ColTotalDetalle.Alineacion = "right";
        ColTotalDetalle.Ancho = "50";
        ColTotalDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalle.Columnas.Add(ColTotalDetalle);

        //Eliminar partida de proveedor consultar
        CJQColumn ColEliminarPartidaEditar = new CJQColumn();
        ColEliminarPartidaEditar.Nombre = "Eliminar";
        ColEliminarPartidaEditar.Encabezado = "Eliminar";
        ColEliminarPartidaEditar.Etiquetado = "Imagen";
        ColEliminarPartidaEditar.Imagen = "eliminar.png";
        ColEliminarPartidaEditar.Estilo = "divImagenConsultar imgEliminarPartida";
        ColEliminarPartidaEditar.Buscador = "false";
        ColEliminarPartidaEditar.Ordenable = "false";
        ColEliminarPartidaEditar.Ancho = "25";
        grdOrdenCompraDetalle.Columnas.Add(ColEliminarPartidaEditar);


        ClientScript.RegisterStartupScript(this.GetType(), "grdOrdenCompraDetalle", grdOrdenCompraDetalle.GeneraGrid(), true);

    }

    private void PintaGridDetalleOrdenCompraConsulta()
    {
        //GridOrdenCompraDetalle
        CJQGrid grdOrdenCompraDetalleConsultar = new CJQGrid();
        grdOrdenCompraDetalleConsultar.NombreTabla = "grdOrdenCompraDetalleConsultar";
        grdOrdenCompraDetalleConsultar.CampoIdentificador = "IdOrdenCompraDetalle";
        grdOrdenCompraDetalleConsultar.ColumnaOrdenacion = "IdOrdenCompraDetalle";
        grdOrdenCompraDetalleConsultar.TipoOrdenacion = "ASC";
        grdOrdenCompraDetalleConsultar.Metodo = "ObtenerOrdenCompraDetalle";
        grdOrdenCompraDetalleConsultar.TituloTabla = "Detalle de orden de compra";
        grdOrdenCompraDetalleConsultar.GenerarGridCargaInicial = false;
        grdOrdenCompraDetalleConsultar.GenerarFuncionFiltro = false;
        grdOrdenCompraDetalleConsultar.GenerarFuncionTerminado = false;
        grdOrdenCompraDetalleConsultar.Altura = 200;
        grdOrdenCompraDetalleConsultar.Ancho = 870;
        grdOrdenCompraDetalleConsultar.NumeroRegistros = 15;
        grdOrdenCompraDetalleConsultar.RangoNumeroRegistros = "15,30,60";

        //IdOrdenCompraDetalle
        CJQColumn ColIdOrdenCompraDetalle = new CJQColumn();
        ColIdOrdenCompraDetalle.Nombre = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Oculto = "true";
        ColIdOrdenCompraDetalle.Encabezado = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Buscador = "false";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColIdOrdenCompraDetalle);

        //ClaveDetalle
        CJQColumn ColClaveDetalle = new CJQColumn();
        ColClaveDetalle.Nombre = "Clave";
        ColClaveDetalle.Encabezado = "Clave";
        ColClaveDetalle.Buscador = "false";
        ColClaveDetalle.Alineacion = "left";
        ColClaveDetalle.Ancho = "50";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColClaveDetalle);

        //DescripcionDetalle
        CJQColumn ColDescripcionDetalle = new CJQColumn();
        ColDescripcionDetalle.Nombre = "Descripcion";
        ColDescripcionDetalle.Encabezado = "Descripción";
        ColDescripcionDetalle.Buscador = "false";
        ColDescripcionDetalle.Alineacion = "left";
        ColDescripcionDetalle.Ancho = "120";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColDescripcionDetalle);

        //ClaveProdServDetalle
        CJQColumn ClaveProdServDetalle = new CJQColumn();
        ClaveProdServDetalle.Nombre = "ClaveProdServ";
        ClaveProdServDetalle.Encabezado = "Clave (SAT)";
        ClaveProdServDetalle.Buscador = "false";
        ClaveProdServDetalle.Alineacion = "left";
        ClaveProdServDetalle.Ancho = "50";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ClaveProdServDetalle);

        //Pedido
        CJQColumn ColPedido = new CJQColumn();
        ColPedido.Nombre = "Folio";
        ColPedido.Encabezado = "Pedido";
        ColPedido.Buscador = "false";
        ColPedido.Alineacion = "right";
        ColPedido.Ancho = "60";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColPedido);

        //Proyect
        CJQColumn ColProyecto = new CJQColumn();
        ColProyecto.Nombre = "IdProyecto";
        ColProyecto.Encabezado = "Proyecto";
        ColProyecto.Buscador = "false";
        ColProyecto.Alineacion = "right";
        ColProyecto.Ancho = "60";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColProyecto);

        //CantidadDetalle
        CJQColumn ColCantidadDetalle = new CJQColumn();
        ColCantidadDetalle.Nombre = "Cantidad";
        ColCantidadDetalle.Encabezado = "Cantidad";
        ColCantidadDetalle.Buscador = "false";
        ColCantidadDetalle.Alineacion = "right";
        ColCantidadDetalle.Ancho = "50";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColCantidadDetalle);

        //CostoDetalle
        CJQColumn ColCostoDetalle = new CJQColumn();
        ColCostoDetalle.Nombre = "Costo";
        ColCostoDetalle.Encabezado = "Costo";
        ColCostoDetalle.Buscador = "false";
        ColCostoDetalle.Alineacion = "right";
        ColCostoDetalle.Ancho = "50";
        ColCostoDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColCostoDetalle);

        //SubtotalDetalle
        CJQColumn ColSubtotalDetalle = new CJQColumn();
        ColSubtotalDetalle.Nombre = "Subtotal";
        ColSubtotalDetalle.Encabezado = "Subtotal";
        ColSubtotalDetalle.Buscador = "false";
        ColSubtotalDetalle.Oculto = "true";
        ColSubtotalDetalle.Alineacion = "right";
        ColSubtotalDetalle.Ancho = "50";
        ColSubtotalDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColSubtotalDetalle);

        //IVADetalle
        CJQColumn ColIVADetalle = new CJQColumn();
        ColIVADetalle.Nombre = "IVA";
        ColIVADetalle.Encabezado = "IVA";
        ColIVADetalle.Buscador = "false";
        ColIVADetalle.Oculto = "false";
        ColIVADetalle.Alineacion = "right";
        ColIVADetalle.Ancho = "50";
        ColIVADetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColIVADetalle);

        //TotalDetalle
        CJQColumn ColTotalDetalle = new CJQColumn();
        ColTotalDetalle.Nombre = "Total";
        ColTotalDetalle.Encabezado = "Total";
        ColTotalDetalle.Buscador = "false";
        ColTotalDetalle.Alineacion = "right";
        ColTotalDetalle.Ancho = "50";
        ColTotalDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleConsultar.Columnas.Add(ColTotalDetalle);

        ClientScript.RegisterStartupScript(this.GetType(), "grdOrdenCompraDetalleConsultar", grdOrdenCompraDetalleConsultar.GeneraGrid(), true);

    }

    private void PintaGridDetalleOrdenCompraEditar()
    {
        //GridOrdenCompraDetalle
        CJQGrid grdOrdenCompraDetalleEditar = new CJQGrid();
        grdOrdenCompraDetalleEditar.NombreTabla = "grdOrdenCompraDetalleEditar";
        grdOrdenCompraDetalleEditar.CampoIdentificador = "IdOrdenCompraDetalle";
        grdOrdenCompraDetalleEditar.ColumnaOrdenacion = "IdOrdenCompraDetalle";
        grdOrdenCompraDetalleEditar.TipoOrdenacion = "ASC";
        grdOrdenCompraDetalleEditar.Metodo = "ObtenerOrdenCompraDetalle";
        grdOrdenCompraDetalleEditar.TituloTabla = "Detalle de orden de compra";
        grdOrdenCompraDetalleEditar.GenerarGridCargaInicial = false;
        grdOrdenCompraDetalleEditar.GenerarFuncionFiltro = false;
        grdOrdenCompraDetalleEditar.GenerarFuncionTerminado = false;
        grdOrdenCompraDetalleEditar.Altura = 150;
        grdOrdenCompraDetalleEditar.Ancho = 870;
        grdOrdenCompraDetalleEditar.NumeroRegistros = 15;
        grdOrdenCompraDetalleEditar.RangoNumeroRegistros = "15,30,60";

        //IdOrdenCompraDetalle
        CJQColumn ColIdOrdenCompraDetalle = new CJQColumn();
        ColIdOrdenCompraDetalle.Nombre = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Oculto = "true";
        ColIdOrdenCompraDetalle.Encabezado = "IdOrdenCompraDetalle";
        ColIdOrdenCompraDetalle.Buscador = "false";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColIdOrdenCompraDetalle);

        //ClaveDetalle
        CJQColumn ColClaveDetalle = new CJQColumn();
        ColClaveDetalle.Nombre = "Clave";
        ColClaveDetalle.Encabezado = "Clave";
        ColClaveDetalle.Buscador = "false";
        ColClaveDetalle.Alineacion = "left";
        ColClaveDetalle.Ancho = "50";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColClaveDetalle);

        //DescripcionDetalle
        CJQColumn ColDescripcionDetalle = new CJQColumn();
        ColDescripcionDetalle.Nombre = "Descripcion";
        ColDescripcionDetalle.Encabezado = "Descripción";
        ColDescripcionDetalle.Buscador = "false";
        ColDescripcionDetalle.Alineacion = "left";
        ColDescripcionDetalle.Ancho = "120";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColDescripcionDetalle);

        //ClaveProdServDetalle
        CJQColumn ClaveProdServDetalle = new CJQColumn();
        ClaveProdServDetalle.Nombre = "ClaveProdServ";
        ClaveProdServDetalle.Encabezado = "Clave (SAT)";
        ClaveProdServDetalle.Buscador = "false";
        ClaveProdServDetalle.Alineacion = "left";
        ClaveProdServDetalle.Ancho = "50";
        grdOrdenCompraDetalleEditar.Columnas.Add(ClaveProdServDetalle);

        // FolioPedido (oculto)
        CJQColumn ColFolioPedido = new CJQColumn();
        ColFolioPedido.Nombre = "FolioPedido";
        ColFolioPedido.Encabezado = "Pedido";
        ColFolioPedido.Buscador = "false";
        ColFolioPedido.Oculto = "true";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColFolioPedido);

        // IdProyecto(oculto)
        CJQColumn ColIdProyecto = new CJQColumn();
        ColIdProyecto.Nombre = "IdProyecto";
        ColIdProyecto.Encabezado = "Proyecto";
        ColIdProyecto.Buscador = "false";
        ColIdProyecto.Oculto = "true";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColIdProyecto);

        //CantidadDetalle
        CJQColumn ColCantidadDetalle = new CJQColumn();
        ColCantidadDetalle.Nombre = "Cantidad";
        ColCantidadDetalle.Encabezado = "Cantidad";
        ColCantidadDetalle.Buscador = "false";
        ColCantidadDetalle.Alineacion = "right";
        ColCantidadDetalle.Ancho = "50";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColCantidadDetalle);

        //CostoDetalle
        CJQColumn ColCostoDetalle = new CJQColumn();
        ColCostoDetalle.Nombre = "Costo";
        ColCostoDetalle.Encabezado = "Costo";
        ColCostoDetalle.Buscador = "false";
        ColCostoDetalle.Alineacion = "right";
        ColCostoDetalle.Ancho = "50";
        ColCostoDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColCostoDetalle);

        //SubtotalDetalle
        CJQColumn ColSubtotalDetalle = new CJQColumn();
        ColSubtotalDetalle.Nombre = "Subtotal";
        ColSubtotalDetalle.Encabezado = "Subtotal";
        ColSubtotalDetalle.Buscador = "false";
        ColSubtotalDetalle.Oculto = "true";
        ColSubtotalDetalle.Alineacion = "right";
        ColSubtotalDetalle.Ancho = "50";
        ColSubtotalDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColSubtotalDetalle);

        //IVADetalle
        CJQColumn ColIVADetalle = new CJQColumn();
        ColIVADetalle.Nombre = "IVA";
        ColIVADetalle.Encabezado = "IVA";
        ColIVADetalle.Buscador = "false";
        ColIVADetalle.Oculto = "false";
        ColIVADetalle.Alineacion = "right";
        ColIVADetalle.Ancho = "50";
        ColIVADetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColIVADetalle);

        //TotalDetalle
        CJQColumn ColTotalDetalle = new CJQColumn();
        ColTotalDetalle.Nombre = "Total";
        ColTotalDetalle.Encabezado = "Total";
        ColTotalDetalle.Buscador = "false";
        ColTotalDetalle.Alineacion = "right";
        ColTotalDetalle.Ancho = "50";
        ColTotalDetalle.Formato = "FormatoMoneda";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColTotalDetalle);

        //Eliminar partida de proveedor consultar
        CJQColumn ColEliminarPartidaEditar = new CJQColumn();
        ColEliminarPartidaEditar.Nombre = "Eliminar";
        ColEliminarPartidaEditar.Encabezado = "Eliminar";
        ColEliminarPartidaEditar.Etiquetado = "Imagen";
        ColEliminarPartidaEditar.Imagen = "eliminar.png";
        ColEliminarPartidaEditar.Estilo = "divImagenConsultar imgEliminarPartidaEditar";
        ColEliminarPartidaEditar.Buscador = "false";
        ColEliminarPartidaEditar.Ordenable = "false";
        ColEliminarPartidaEditar.Ancho = "25";
        grdOrdenCompraDetalleEditar.Columnas.Add(ColEliminarPartidaEditar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdOrdenCompraDetalleEditar", grdOrdenCompraDetalleEditar.GeneraGrid(), true);

    }

    private void PintaGridDetallePedido()
    {
        //GridDetallePedido
        CJQGrid GridDetallePedido = new CJQGrid();
        GridDetallePedido.NombreTabla = "grdDetallePedido";
        GridDetallePedido.CampoIdentificador = "IdCotizacionDetalle";
        GridDetallePedido.ColumnaOrdenacion = "Clave";
        GridDetallePedido.TipoOrdenacion = "DESC";
        GridDetallePedido.Metodo = "ObtenerDetallePedido";
        GridDetallePedido.TituloTabla = "Detalle pedido";
        GridDetallePedido.GenerarFuncionFiltro = false;
        GridDetallePedido.GenerarFuncionTerminado = false;
        GridDetallePedido.Altura = 200;
        GridDetallePedido.Ancho = 800;
        GridDetallePedido.NumeroRegistros = 15;
        GridDetallePedido.RangoNumeroRegistros = "15,30,60";

        //IdCotizacionDetalle
        CJQColumn ColIdCotizacionDetalle = new CJQColumn();
        ColIdCotizacionDetalle.Nombre = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Oculto = "true";
        ColIdCotizacionDetalle.Encabezado = "IdCotizacionDetalle";
        ColIdCotizacionDetalle.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColIdCotizacionDetalle);

        //IdProductoPedido
        CJQColumn ColIdProductoPedido = new CJQColumn();
        ColIdProductoPedido.Nombre = "IdProducto";
        ColIdProductoPedido.Oculto = "true";
        ColIdProductoPedido.Encabezado = "IdProducto";
        ColIdProductoPedido.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColIdProductoPedido);

        //ClavePedido
        CJQColumn ColClaveProductoPedido = new CJQColumn();
        ColClaveProductoPedido.Nombre = "Clave";
        ColClaveProductoPedido.Encabezado = "Clave";
        ColClaveProductoPedido.Buscador = "false";
        ColClaveProductoPedido.Alineacion = "left";
        ColClaveProductoPedido.Ancho = "30";
        GridDetallePedido.Columnas.Add(ColClaveProductoPedido);

        //DescripcionPedido
        CJQColumn ColDescripcionProductoPedido = new CJQColumn();
        ColDescripcionProductoPedido.Nombre = "Descripcion";
        ColDescripcionProductoPedido.Encabezado = "Descripcion";
        ColDescripcionProductoPedido.Alineacion = "left";
        ColDescripcionProductoPedido.Buscador = "false";
        ColDescripcionProductoPedido.Ancho = "80";
        GridDetallePedido.Columnas.Add(ColDescripcionProductoPedido);

        //ClaveProdServPedido
        CJQColumn ColClaveProdServPedido = new CJQColumn();
        ColClaveProdServPedido.Nombre = "ClaveProdServ";
        ColClaveProdServPedido.Encabezado = "Clave (SAT)";
        ColClaveProdServPedido.Buscador = "false";
        ColClaveProdServPedido.Alineacion = "left";
        ColClaveProdServPedido.Ancho = "80";
        GridDetallePedido.Columnas.Add(ColClaveProdServPedido);

        //CantidadProductoPedido
        CJQColumn ColCantidadProductoPedido = new CJQColumn();
        ColCantidadProductoPedido.Nombre = "Cantidad";
        ColCantidadProductoPedido.Encabezado = "Cantidad";
        ColCantidadProductoPedido.Buscador = "false";
        ColCantidadProductoPedido.Alineacion = "right";
        ColCantidadProductoPedido.Ancho = "20";
        GridDetallePedido.Columnas.Add(ColCantidadProductoPedido);

        //CantidadRestante
        CJQColumn ColCantidadRestante = new CJQColumn();
        ColCantidadRestante.Nombre = "Restante";
        ColCantidadRestante.Encabezado = "Pendientes de comprar";
        ColCantidadRestante.Buscador = "false";
        ColCantidadRestante.Alineacion = "right";
        ColCantidadRestante.Ancho = "50";
        GridDetallePedido.Columnas.Add(ColCantidadRestante);

        //PrecioProductoPedido
        CJQColumn ColPrecioProductoPedido = new CJQColumn();
        ColPrecioProductoPedido.Nombre = "PrecioUnitario";
        ColPrecioProductoPedido.Encabezado = "Precio";
        ColPrecioProductoPedido.Buscador = "false";
        ColPrecioProductoPedido.Alineacion = "right";
        ColPrecioProductoPedido.Ancho = "30";
        ColPrecioProductoPedido.Formato = "FormatoMoneda";
        GridDetallePedido.Columnas.Add(ColPrecioProductoPedido);

        //TotalProductoPedido
        CJQColumn ColTotalProductoPedido = new CJQColumn();
        ColTotalProductoPedido.Nombre = "Total";
        ColTotalProductoPedido.Encabezado = "Total";
        ColTotalProductoPedido.Buscador = "false";
        ColTotalProductoPedido.Alineacion = "right";
        ColTotalProductoPedido.Ancho = "30";
        ColTotalProductoPedido.Formato = "FormatoMoneda";
        GridDetallePedido.Columnas.Add(ColTotalProductoPedido);

        //TipoMoneda produto pedido
        CJQColumn ColTipoMonedaProductoPedido = new CJQColumn();
        ColTipoMonedaProductoPedido.Nombre = "TipoMoneda";
        ColTipoMonedaProductoPedido.Encabezado = "Tipo de moneda";
        ColTipoMonedaProductoPedido.Ancho = "40";
        ColTipoMonedaProductoPedido.Alineacion = "left";
        ColTipoMonedaProductoPedido.Buscador = "false";
        GridDetallePedido.Columnas.Add(ColTipoMonedaProductoPedido);

        //SeleccionarDetallePedido
        CJQColumn ColSeleccionarDetallePedido = new CJQColumn();
        ColSeleccionarDetallePedido.Nombre = "Seleccionar";
        ColSeleccionarDetallePedido.Encabezado = "Seleccionar";
        ColSeleccionarDetallePedido.Etiquetado = "Imagen";
        ColSeleccionarDetallePedido.Imagen = "select.png";
        ColSeleccionarDetallePedido.Estilo = "divImagenConsultar imgSeleccionarDetallePedido";
        ColSeleccionarDetallePedido.Buscador = "false";
        ColSeleccionarDetallePedido.Ordenable = "false";
        ColSeleccionarDetallePedido.Ancho = "25";
        GridDetallePedido.Columnas.Add(ColSeleccionarDetallePedido);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDetallePedido", GridDetallePedido.GeneraGrid(), true);
    }

    //Validaciones
    private static string ValidarOrdenCompraEncabezado(COrdenCompraEncabezado pOrdenCompraEncabezado, CConexion pConexion)
    {
        string errores = "";
        if (pOrdenCompraEncabezado.IdProveedor == 0)
        { errores = errores + "<span>*</span> No hay Proveedor por asociar, favor de elegir alguno.<br />"; }

        if (pOrdenCompraEncabezado.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> No hay tipo de moneda asociado, favor de elegir alguno.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarOrdenCompraDetalle(COrdenCompraDetalle pOrdenCompraDetalle, CConexion pConexion)
    {
        string errores = "";

        if (pOrdenCompraDetalle.Cantidad <= 0)
        { errores = errores + "<span>*</span> El campo cantidad debe de ser mayor a 0.<br />"; }

        //if (pOrdenCompraDetalle.Total == 0)
        //{ errores = errores + "<span>*</span> El campo total esta vacío, favor de capturarlo.<br />"; }

        if (pOrdenCompraDetalle.IdUnidadCompraVenta == 0)
        { errores = errores + "<span>*</span> No hay unidad de compra venta asociado, favor de elegir alguno.<br />"; }

        if (pOrdenCompraDetalle.IdTipoCompra == 0)
        { errores = errores + "<span>*</span> No hay tipo de compra asociado, favor de elegir alguno.<br />"; }

        if (pOrdenCompraDetalle.Clave == "")
        { errores = errores + "<span>*</span> El campo clave esta vacío, favor de capturarlo.<br />"; }

        if (pOrdenCompraDetalle.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción de la partida esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    /****** Imprimir ****/
    [WebMethod]
    public static string Imprimir(int pIdOrdenCompra, string pFolio, string pTemplate)
    {
        JObject Respuesta = new JObject();

        CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string Descripcion, CUsuario UsuarioSesion) {
            if(Error == 0){

                CSucursal SucursalActual = new CSucursal();
                SucursalActual.LlenaObjeto(UsuarioSesion.IdSucursalActual, pConexion);

                CEmpresa EmpresaActual = new CEmpresa();
                EmpresaActual.LlenaObjeto(SucursalActual.IdEmpresa, pConexion);

                string logoEmpresa = EmpresaActual.Logo;

                JObject oPermisos = new JObject();
                CUtilerias Util = new CUtilerias();

                Dictionary<string, object> Parametros = new Dictionary<string, object>();
                Parametros.Add("ImpresionDocumento", pTemplate);

                CImpresionDocumento ImpresionDocumento = new CImpresionDocumento();
                ImpresionDocumento.LlenaObjetoFiltros(Parametros, pConexion);

                Dictionary<string, object> ParametrosTempl = new Dictionary<string, object>();
                //ParametrosTempl.Add("IdEmpresa", idEmpresa);
                ParametrosTempl.Add("Baja", 0);
                ParametrosTempl.Add("IdImpresionDocumento", ImpresionDocumento.IdImpresionDocumento);

                CImpresionTemplate ImpresionTemplate = new CImpresionTemplate();
                ImpresionTemplate.LlenaObjetoFiltros(ParametrosTempl, pConexion);

                COrdenCompraEncabezado OrdenCompraEncabezado = new COrdenCompraEncabezado();
                OrdenCompraEncabezado.LlenaObjeto(pIdOrdenCompra, pConexion);

                CTipoMoneda Moneda = new CTipoMoneda();
                Moneda.LlenaObjeto(OrdenCompraEncabezado.IdTipoMoneda, pConexion);

                OrdenCompraEncabezado.CantidadTotalLetra = Util.ConvertLetter(OrdenCompraEncabezado.Total.ToString(), Moneda.TipoMoneda.ToUpper());
                OrdenCompraEncabezado.Editar(pConexion);

                CUsuario UsuarioComprador = new CUsuario();
                UsuarioComprador.LlenaObjeto(OrdenCompraEncabezado.IdUsuario, pConexion);
                
                JArray datos = (JArray)CJson.obtenerDatosImpresionOrdenCompra(pIdOrdenCompra.ToString(), Convert.ToInt32(UsuarioComprador.IdUsuario));

                string rutaPDF = HttpContext.Current.Server.MapPath("~/Archivos/Impresiones/") + "OrdenCompra_" + pFolio + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".pdf";
                string rutaTemplate = HttpContext.Current.Server.MapPath("~/Archivos/TemplatesImpresion/" + ImpresionTemplate.RutaTemplate);
                string rutaCSS = HttpContext.Current.Server.MapPath("~/Archivos/TemplatesImpresion/" + ImpresionTemplate.RutaCSS);
                string imagenLogo = HttpContext.Current.Server.MapPath("~/Archivos/EmpresaLogo/") + logoEmpresa;

                if (!File.Exists(rutaTemplate))
                {
                    Error = 1;
                    Descripcion = "No hay un template válido para esta empresa.";
                } else {

                    JObject Modelo = new JObject();

                    Modelo = COrdenCompraEncabezado.ObtenerOrdenCompraEncabezado(Modelo, pIdOrdenCompra, pConexion);
                    Modelo.Add(new JProperty("Archivo", Util.ReportePDFTemplate(rutaPDF, rutaTemplate, rutaCSS, imagenLogo, ImpresionTemplate.IdImpresionTemplate, datos, pConexion)));
                    Modelo.Add(new JProperty("Permisos", oPermisos));
                    Respuesta.Add(new JProperty("Modelo", Modelo));

                    HttpContext.Current.Application.Set("PDFDescargar", Path.GetFileName(rutaPDF));
                }
            }
            Respuesta.Add("Error", Error);
            Respuesta.Add("Descripcion", Descripcion);
        });
        return Respuesta.ToString();
    }

	[WebMethod]
	public static string ImprimirOrdenCompra(int IdOrdenCompra)
	{
		JObject Respuesta = new JObject();

		CUtilerias.DelegarAccion(delegate(CConexion pConexion, int Error, string DescripcionError, CUsuario UsuarioSesion) {
			if (Error == 0)
			{
				JObject Modelo = new JObject();

				COrdenCompraEncabezado OrdenCompra = new COrdenCompraEncabezado();
				OrdenCompra.LlenaObjeto(IdOrdenCompra, pConexion);

				COrdenCompraEncabezadoSucursal OrdenCompraSucursal = new COrdenCompraEncabezadoSucursal();
				Dictionary<string, object> pParametros = new Dictionary<string, object>();
				pParametros.Add("IdOrdenCompraEncabezado", OrdenCompra.IdOrdenCompraEncabezado);
				OrdenCompraSucursal.LlenaObjetoFiltros(pParametros, pConexion);

				CSucursal Sucursal = new CSucursal();
				Sucursal.LlenaObjeto(OrdenCompraSucursal.IdSucursal, pConexion);

				int IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
				CEmpresa Empresa = new CEmpresa();
				Empresa.LlenaObjeto(IdEmpresa, pConexion);

				CMunicipio Municipio = new CMunicipio();
				Municipio.LlenaObjeto(Empresa.IdMunicipio, pConexion);

				CEstado Estado = new CEstado();
				Estado.LlenaObjeto(Municipio.IdEstado, pConexion);

				CProveedor Proveedor = new CProveedor();
				Proveedor.LlenaObjeto(OrdenCompra.IdProveedor, pConexion);

				COrganizacion Organizacion = new COrganizacion();
				Organizacion.LlenaObjeto(Proveedor.IdOrganizacion, pConexion);

				CDireccionOrganizacion Direccion = new CDireccionOrganizacion();
				pParametros.Clear();
				pParametros.Add("IdOrganizacion", Organizacion.IdOrganizacion);
				pParametros.Add("IdTipoDireccion", 1);
				Direccion.LlenaObjetoFiltros(pParametros, pConexion);

				CUsuario Usuario = new CUsuario();
				Usuario.LlenaObjeto(OrdenCompra.IdUsuario, pConexion);

				CTipoMoneda TipoMoneda = new CTipoMoneda();
				TipoMoneda.LlenaObjeto(OrdenCompra.IdTipoMoneda, pConexion);

				CTipoCambioOrdenCompra TipoCambio = new CTipoCambioOrdenCompra();
				pParametros.Clear();
				pParametros.Add("IdOrdenCompraEncabezado", OrdenCompra.IdOrdenCompraEncabezado);
				pParametros.Add("IdTipoMonedaOrigen", OrdenCompra.IdTipoMoneda);
				pParametros.Add("IdTipoMonedaDestino", 1);

				TipoCambio.LlenaObjetoFiltros(pParametros, pConexion);

				Modelo.Add("NUMEROORDENCOMPRA", OrdenCompra.Folio);
				Modelo.Add("SUCURSAL", Sucursal.Sucursal);
				Modelo.Add("IMAGEN_LOGO", Empresa.Logo);
				Modelo.Add("RAZONSOCIAL", Empresa.RazonSocial);
				Modelo.Add("RFCEmpresa", Empresa.RFC);
				Modelo.Add("CALLE", Empresa.Calle);
				Modelo.Add("NUMEROEXTERIOR", Empresa.NumeroExterior);
				Modelo.Add("COLONIA", Empresa.Colonia);
				Modelo.Add("CODIGOPOSTAL", Empresa.CodigoPostal);
				Modelo.Add("MUNICIPIO", Municipio.Municipio);
				Modelo.Add("ESTADO", Estado.Estado);
				Modelo.Add("FECHAORDENCOMPRA", OrdenCompra.FechaRequerida.ToShortDateString());
				Modelo.Add("RFC", Organizacion.RFC);
				Modelo.Add("PROVEEDOR", Organizacion.RazonSocial);
				Modelo.Add("DIRECCION", Direccion.Calle + " " + Direccion.NumeroExterior);
				Modelo.Add("AGENTE", Usuario.Nombre + " " + Usuario.ApellidoPaterno + " " + Usuario.ApellidoMaterno);
				Modelo.Add("TIPOMONEDA", TipoMoneda.TipoMoneda);
				Modelo.Add("TIPOCAMBIO", TipoCambio.TipoCambio);
				Modelo.Add("SUBTOTALORDENCOMPRA", OrdenCompra.Subtotal.ToString("C"));
				Modelo.Add("IVA", OrdenCompra.IVA.ToString("C"));
				Modelo.Add("TOTAL", OrdenCompra.Total.ToString("C"));
				CUtilerias utl = new CUtilerias();
				Modelo.Add("TOTALLETRA", utl.ConvertLetter(OrdenCompra.Total.ToString(), TipoMoneda.TipoMoneda));
				Modelo.Add("OBSERVACIONES", OrdenCompra.Nota);

				CSelectEspecifico Conceptos = new CSelectEspecifico();
				Conceptos.StoredProcedure.CommandText = "sp_OrdenCompra_Detalle";
				Conceptos.StoredProcedure.Parameters.Add("IdOrdenCompraEncabezado", SqlDbType.Int).Value = OrdenCompra.IdOrdenCompraEncabezado;
				Modelo.Add("Conceptos", CUtilerias.ObtenerConsulta(Conceptos, pConexion));

				Respuesta.Add("Modelo", Modelo);
			}
			Respuesta.Add("Error", Error);
			Respuesta.Add("DescripcionError", DescripcionError);
		});

		return Respuesta.ToString();
	}

    protected void btnDescarga_Click(object sender, EventArgs e)
    {
        string PDFDescarga = HttpContext.Current.Application.Get("PDFDescargar").ToString();

        Response.Clear();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + PDFDescarga);
        Response.WriteFile((HttpContext.Current.Server.MapPath("../Archivos/Impresiones/" + PDFDescarga)));
        Response.Flush();
        Response.End();
    }

    [WebMethod]
    public static string ObtenerListaTipoCompra()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonTipoCompra(true, ConexionBaseDatos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));
        }
        else
        {
            oRespuesta.Add(new JProperty("Error", 1));
            oRespuesta.Add(new JProperty("Descripcion", "No hay conexión a Base de Datos"));
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

}