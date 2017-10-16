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

public partial class Servicio : System.Web.UI.Page
{
    public string btnObtenerFormaAgregarServicio = "";
    public string btnAgregarDescuentoServicio = "";

    [WebMethod]
    public static string BuscarClave(string pClave)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string repuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CServicio JsonOportunidad = new CServicio();
        JsonOportunidad.StoredProcedure.CommandText = "sp_Servicio_ConsultarCalve";
        JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pClave", pClave);
        string sJson = JsonOportunidad.ObtenerJsonServicio(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return sJson;
    }

    [WebMethod]
    public static string BuscarServicio(string pServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string repuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CServicio JsonOportunidad = new CServicio();
        JsonOportunidad.StoredProcedure.CommandText = "sp_Servicio_ConsultarServicio";
        JsonOportunidad.StoredProcedure.Parameters.AddWithValue("@pServicio", pServicio);
        string sJson = JsonOportunidad.ObtenerJsonServicio(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return sJson;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            if (Usuario.TienePermisos(new string[] { "puedeAgregarServicio" }, ConexionBaseDatos) == "")
            {
                btnObtenerFormaAgregarServicio = "<input type='button' id='btnObtenerFormaAgregarServicio' value='+ Agregar servicio' class='buttonLTR' />";
            }

            if (Usuario.TienePermisos(new string[] { "puedeAgregarDescuentoServicio" }, ConexionBaseDatos) == "")
            {
                btnAgregarDescuentoServicio = "<input type='button' id='btnAgregarDescuentoServicio' value='+ Agregar descuento' class='buttonLTR' />";
            }
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        GeneraGridServicios(ConexionBaseDatos);
        GeneraGridDescuentos(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    #region GeneraGrid
    private void GeneraGridServicios(CConexion pConexion)
    {
        string puedeDesactivarServicio = "true";
        string puedeConsultarServicio = "true";
        string accesoDescuento = "true";

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);

        if (Usuario.TienePermisos(new string[] { "puedeConsultarServicio" }, pConexion) == "")
        {
            puedeConsultarServicio = "false";
        }

        if (Usuario.TienePermisos(new string[] { "puedeDesactivarServicio" }, pConexion) == "")
        {
            puedeDesactivarServicio = "false";
        }

        if (Usuario.TienePermisos(new string[] { "accesoDescuentoServicio" }, pConexion) == "")
        {
            accesoDescuento = "false";
        }

        //GridServicio
        CJQGrid GridServicio = new CJQGrid();
        GridServicio.NombreTabla = "grdServicio";
        GridServicio.CampoIdentificador = "IdServicio";
        GridServicio.ColumnaOrdenacion = "Servicio";
        GridServicio.Metodo = "ObtenerServicio";
        GridServicio.TituloTabla = "Catálogo de servicios";

        //IdServicio
        CJQColumn ColIdServicio = new CJQColumn();
        ColIdServicio.Nombre = "IdServicio";
        ColIdServicio.Oculto = "true";
        ColIdServicio.Encabezado = "IdServicio";
        ColIdServicio.Buscador = "false";
        GridServicio.Columnas.Add(ColIdServicio);

        //Clave
        CJQColumn ColClave = new CJQColumn();
        ColClave.Nombre = "Clave";
        ColClave.Encabezado = "Clave";
        ColClave.Buscador = "true";
        ColClave.Alineacion = "left";
        ColClave.Ancho = "40";
        GridServicio.Columnas.Add(ColClave);

        //Servicio
        CJQColumn ColServicio = new CJQColumn();
        ColServicio.Nombre = "Servicio";
        ColServicio.Encabezado = "Servicio";
        ColServicio.Buscador = "true";
        ColServicio.Alineacion = "left";
        ColServicio.Ancho = "170";
        GridServicio.Columnas.Add(ColServicio);

        //Unidad de medida
        CJQColumn ColUnidadCompraVenta = new CJQColumn();
        ColUnidadCompraVenta.Nombre = "UnidadCompraVenta";
        ColUnidadCompraVenta.Encabezado = "UCV";
        ColUnidadCompraVenta.Buscador = "false";
        ColUnidadCompraVenta.Alineacion = "left";
        ColUnidadCompraVenta.Ancho = "50";
        GridServicio.Columnas.Add(ColUnidadCompraVenta);

        //Precio
        CJQColumn ColPrecio = new CJQColumn();
        ColPrecio.Nombre = "Precio";
        ColPrecio.Encabezado = "Precio";
        ColPrecio.Buscador = "false";
        ColPrecio.Ancho = "40";
        ColPrecio.Formato = "FormatoMoneda";
        ColPrecio.Alineacion = "right";
        GridServicio.Columnas.Add(ColPrecio);

        //Tipo de Servicio
        CJQColumn ColTipoServicio = new CJQColumn();
        ColTipoServicio.Nombre = "TipoServicio";
        ColTipoServicio.Encabezado = "Tipo de servicio";
        ColTipoServicio.Buscador = "false";
        ColTipoServicio.Alineacion = "left";
        ColTipoServicio.Ancho = "85";
        ColTipoServicio.Buscador = "true";
        ColTipoServicio.TipoBuscador = "Combo";
        ColTipoServicio.StoredProcedure.CommandText = "sp_TipoServicio_Consultar_FiltroGridServicios";
        GridServicio.Columnas.Add(ColTipoServicio);

        //Division
        CJQColumn ColDivision = new CJQColumn();
        ColDivision.Nombre = "Division";
        ColDivision.Encabezado = "División";
        ColDivision.Buscador = "false";
        ColDivision.Alineacion = "left";
        ColDivision.Ancho = "75";
        GridServicio.Columnas.Add(ColDivision);

        //Baja
        CJQColumn ColBaja = new CJQColumn();
        ColBaja.Nombre = "AI";
        ColBaja.Encabezado = "A/I";
        ColBaja.Ordenable = "false";
        ColBaja.Etiquetado = "A/I";
        ColBaja.Ancho = "45";
        ColBaja.Buscador = "true";
        ColBaja.TipoBuscador = "Combo";
        ColBaja.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBaja.Oculto = puedeDesactivarServicio;
        GridServicio.Columnas.Add(ColBaja);

        //Descuentos
        CJQColumn ColConsultarDescuento = new CJQColumn();
        ColConsultarDescuento.Nombre = "Descuento";
        ColConsultarDescuento.Encabezado = "D";
        ColConsultarDescuento.Etiquetado = "Imagen";
        ColConsultarDescuento.Imagen = "descuento.png";
        ColConsultarDescuento.Estilo = "divImagenConsultar imgFormaConsultarDescuento";
        ColConsultarDescuento.Buscador = "false";
        ColConsultarDescuento.Ordenable = "false";
        ColConsultarDescuento.Ancho = "20";
        ColConsultarDescuento.Oculto = accesoDescuento;
        GridServicio.Columnas.Add(ColConsultarDescuento);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarServicio";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "20";
        ColConsultar.Oculto = puedeConsultarServicio;
        GridServicio.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdServicio", GridServicio.GeneraGrid(), true);
    }

    private void GeneraGridDescuentos(CConexion pConexion)
    {
        string puedeDesactivarDescuentoServicio = "true";

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), pConexion);

        Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
        ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
        ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);

        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
        SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, pConexion);

        if (Usuario.TienePermisos(new string[] { "puedeDesactivarDescuentoServicio" }, pConexion) == "")
        {
            puedeDesactivarDescuentoServicio = "false";
        }

        //GridDescuento
        CJQGrid GridDescuentoServicio = new CJQGrid();
        GridDescuentoServicio.NombreTabla = "grdDescuentoServicio";
        GridDescuentoServicio.CampoIdentificador = "IdDescuentoServicio";
        GridDescuentoServicio.ColumnaOrdenacion = "DescuentoServicio";
        GridDescuentoServicio.TipoOrdenacion = "DESC";
        GridDescuentoServicio.Metodo = "ObtenerDescuentoServicio";
        GridDescuentoServicio.TituloTabla = "Descuentos del servicio";
        GridDescuentoServicio.GenerarFuncionFiltro = false;
        GridDescuentoServicio.GenerarFuncionTerminado = false;
        GridDescuentoServicio.Altura = 300;
        GridDescuentoServicio.Ancho = 600;
        GridDescuentoServicio.NumeroRegistros = 15;
        GridDescuentoServicio.RangoNumeroRegistros = "15,30,60";

        //IdServicio
        CJQColumn ColIdDescuentoServicio = new CJQColumn();
        ColIdDescuentoServicio.Nombre = "IdDescuentoServicio";
        ColIdDescuentoServicio.Oculto = "true";
        ColIdDescuentoServicio.Encabezado = "IdServicio";
        ColIdDescuentoServicio.Buscador = "false";
        GridDescuentoServicio.Columnas.Add(ColIdDescuentoServicio);

        //Servicio
        CJQColumn ColServicioDelDescuento = new CJQColumn();
        ColServicioDelDescuento.Nombre = "Servicio";
        ColServicioDelDescuento.Encabezado = "Servicio";
        ColServicioDelDescuento.Buscador = "false";
        ColServicioDelDescuento.Alineacion = "left";
        ColServicioDelDescuento.Ancho = "80";
        GridDescuentoServicio.Columnas.Add(ColServicioDelDescuento);

        //Descuento del Servicio
        CJQColumn ColDescuentoServicio = new CJQColumn();
        ColDescuentoServicio.Nombre = "DescuentoServicio";
        ColDescuentoServicio.Encabezado = "Motivo del descuento";
        ColDescuentoServicio.Buscador = "false";
        ColDescuentoServicio.Alineacion = "left";
        ColDescuentoServicio.Ancho = "200";
        GridDescuentoServicio.Columnas.Add(ColDescuentoServicio);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "Descuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Ancho = "80";
        ColDescuento.Alineacion = "right";
        GridDescuentoServicio.Columnas.Add(ColDescuento);

        //Baja
        CJQColumn ColBajaDescuento = new CJQColumn();
        ColBajaDescuento.Nombre = "AI";
        ColBajaDescuento.Encabezado = "A/I";
        ColBajaDescuento.Ordenable = "false";
        ColBajaDescuento.Etiquetado = "A/I";
        ColBajaDescuento.Ancho = "55";
        ColBajaDescuento.Buscador = "true";
        ColBajaDescuento.TipoBuscador = "Combo";
        ColBajaDescuento.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        ColBajaDescuento.Oculto = puedeDesactivarDescuentoServicio;
        GridDescuentoServicio.Columnas.Add(ColBajaDescuento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDescuentoServicio", GridDescuentoServicio.GeneraGrid(), true);
    }
    #endregion

    #region ObtenerInformacionGrids
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerServicio(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pClave, string pServicio, int pTipoServicio, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdServicio", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pClave", SqlDbType.VarChar, 250).Value = Convert.ToString(pClave);
        Stored.Parameters.Add("pServicio", SqlDbType.VarChar, 250).Value = Convert.ToString(pServicio);
        Stored.Parameters.Add("pIdTipoServicio", SqlDbType.Int).Value = pTipoServicio;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;
        Stored.Parameters.Add("pIdUsuario", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDescuentoServicio(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdServicio)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDescuentoServicio", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("IdServicio", SqlDbType.Int).Value = pIdServicio;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = -1;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }
    #endregion

    #region Buscadores
    //[WebMethod]
    //public static string BuscarServicio(string pServicio)
    //{
    //    //Abrir Conexion
    //    CConexion ConexionBaseDatos = new CConexion();
    //    string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

    //    CJson jsonServicio = new CJson();
    //    jsonServicio.StoredProcedure.CommandText = "sp_Servicio_Consultar_FiltroPorServicio";
    //    jsonServicio.StoredProcedure.Parameters.AddWithValue("@pServicio", pServicio);
    //    return jsonServicio.ObtenerJsonString(ConexionBaseDatos);
    //}
    #endregion

    #region ObtenerFormas
    [WebMethod]
    public static string ObtenerFormaAgregarServicio()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            JObject oPermisos = new JObject();

            #region PERMISOS
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            int accesoTipoServicio = 0;
            if (Usuario.TienePermisos(new string[] { "accesoTipoServicio" }, ConexionBaseDatos) == "")
            {
                accesoTipoServicio = 1;
            }
            oPermisos.Add("accesoTipoServicio", accesoTipoServicio);

            int accesoUnidadCompraVenta = 0;
            if (Usuario.TienePermisos(new string[] { "accesoUnidadCompraVenta" }, ConexionBaseDatos) == "")
            {
                accesoUnidadCompraVenta = 1;
            }
            oPermisos.Add("accesoUnidadCompraVenta", accesoUnidadCompraVenta);

            int accesoDivision = 0;
            if (Usuario.TienePermisos(new string[] { "accesoDivision" }, ConexionBaseDatos) == "")
            {
                accesoDivision = 1;
            }
            oPermisos.Add("accesoDivision", accesoDivision);

            int accesoTipoVenta = 0;
            if (Usuario.TienePermisos(new string[] { "accesoTipoVenta" }, ConexionBaseDatos) == "")
            {
                accesoTipoVenta = 1;
            }
            oPermisos.Add("accesoTipoVenta", accesoTipoVenta);
            #endregion

            #region MODELO
            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Baja", 0);
            CTipoServicio TipoServicio = new CTipoServicio();
            JArray JTipoServicios = new JArray();
            foreach (CTipoServicio oTipoServicio in TipoServicio.LlenaObjetosFiltros(ParametrosTS, ConexionBaseDatos))
            {
                JObject JTipoServicio = new JObject();
                JTipoServicio.Add(new JProperty("IdTipoServicio", oTipoServicio.IdTipoServicio));
                JTipoServicio.Add(new JProperty("TipoServicio", oTipoServicio.TipoServicio));
                JTipoServicios.Add(JTipoServicio);
            }
            Modelo.Add(new JProperty("TipoServicios", JTipoServicios));
            Modelo.Add("TiposIVA", CTipoIVA.ObtenerJsonTiposIVA(ConexionBaseDatos));

            Dictionary<string, object> ParametrosTV = new Dictionary<string, object>();
            ParametrosTV.Add("Baja", 0);
            CTipoVenta TipoVenta = new CTipoVenta();
            JArray JTipoVentas = new JArray();
            foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(ParametrosTV, ConexionBaseDatos))
            {
                JObject JTipoVenta = new JObject();
                JTipoVenta.Add(new JProperty("IdTipoVenta", oTipoVenta.IdTipoVenta));
                JTipoVenta.Add(new JProperty("TipoVenta", oTipoVenta.TipoVenta));
                JTipoVentas.Add(JTipoVenta);
            }
            Modelo.Add(new JProperty("TipoVentas", JTipoVentas));


            Dictionary<string, object> ParametrosUCV = new Dictionary<string, object>();
            ParametrosUCV.Add("Baja", 0);
            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            JArray JUnidadCompraVentas = new JArray();
            foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(ParametrosUCV, ConexionBaseDatos))
            {
                JObject JUnidadCompraVenta = new JObject();
                JUnidadCompraVenta.Add(new JProperty("IdUnidadCompraVenta", oUnidadCompraVenta.IdUnidadCompraVenta));
                JUnidadCompraVenta.Add(new JProperty("UnidadCompraVenta", oUnidadCompraVenta.UnidadCompraVenta));
                JUnidadCompraVentas.Add(JUnidadCompraVenta);
            }
            Modelo.Add(new JProperty("UnidadCompraVentas", JUnidadCompraVentas));

            Dictionary<string, object> ParametrosTM = new Dictionary<string, object>();
            ParametrosTM.Add("Baja", 0);
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            JArray JTipoMonedas = new JArray();
            foreach (CTipoMoneda oTipoMoneda in TipoMoneda.LlenaObjetosFiltros(ParametrosTM, ConexionBaseDatos))
            {
                JObject JTipoMoneda = new JObject();
                JTipoMoneda.Add(new JProperty("IdTipoMoneda", oTipoMoneda.IdTipoMoneda));
                JTipoMoneda.Add(new JProperty("TipoMoneda", oTipoMoneda.TipoMoneda));
                JTipoMonedas.Add(JTipoMoneda);
            }
            Modelo.Add(new JProperty("TipoMonedas", JTipoMonedas));
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(ConexionBaseDatos));
            #endregion

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
    public static string ObtenerFormaServicio(int pIdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarServicio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CServicio Servicio = new CServicio();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
        ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
        ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);

        CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
        SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarServicio" }, ConexionBaseDatos) == "")
        {
            puedeEditarServicio = 1;
        }
        oPermisos.Add("puedeEditarServicio", puedeEditarServicio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();

            Servicio.LlenaObjeto(pIdServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdServicio", Servicio.IdServicio));
            Modelo.Add(new JProperty("Servicio", Servicio.Servicio));

            CTipoServicio TipoServicio = new CTipoServicio();//aqui llena el campo de consulta
            TipoServicio.LlenaObjeto(Servicio.IdTipoServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoServicio", TipoServicio.TipoServicio));

            Modelo.Add(new JProperty("Clave", Servicio.Clave));

            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.LlenaObjeto(Servicio.IdTipoVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoVenta", TipoVenta.TipoVenta));


            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta));

            Modelo.Add(new JProperty("Precio", Servicio.Precio));

            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(Servicio.IdTipoMoneda, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

            CTipoIVA TipoIVA = new CTipoIVA();
            TipoIVA.LlenaObjeto(Servicio.IdTipoIVA, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoIVA", TipoIVA.TipoIVA));

            CDivision Division = new CDivision();
            Division.LlenaObjeto(Servicio.IdDivision, ConexionBaseDatos);
            Modelo.Add(new JProperty("Division", Division.Division));

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
    public static string ObtenerFormaEditarServicio(int IdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            JObject oPermisos = new JObject();

            #region PERMISOS
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            Dictionary<string, object> ParametrosSucursalAsignada = new Dictionary<string, object>();
            ParametrosSucursalAsignada.Add("IdUsuario", Usuario.IdUsuario);
            ParametrosSucursalAsignada.Add("IdSucursal", Usuario.IdSucursalActual);

            CSucursalAsignada SucursalAsignada = new CSucursalAsignada();
            SucursalAsignada.LlenaObjetoFiltros(ParametrosSucursalAsignada, ConexionBaseDatos);

            int puedeEditarServicio = 0;
            if (Usuario.TienePermisos(new string[] { "puedeEditarServicio" }, ConexionBaseDatos) == "")
            {
                puedeEditarServicio = 1;
            }
            oPermisos.Add("puedeEditarServicio", puedeEditarServicio);

            int accesoTipoServicio = 0;
            if (Usuario.TienePermisos(new string[] { "accesoTipoServicio" }, ConexionBaseDatos) == "")
            {
                accesoTipoServicio = 1;
            }
            oPermisos.Add("accesoTipoServicio", accesoTipoServicio);

            int accesoUnidadCompraVenta = 0;
            if (Usuario.TienePermisos(new string[] { "accesoUnidadCompraVenta" }, ConexionBaseDatos) == "")
            {
                accesoUnidadCompraVenta = 1;
            }
            oPermisos.Add("accesoUnidadCompraVenta", accesoUnidadCompraVenta);

            int accesoDivision = 0;
            if (Usuario.TienePermisos(new string[] { "accesoDivision" }, ConexionBaseDatos) == "")
            {
                accesoDivision = 1;
            }
            oPermisos.Add("accesoDivision", accesoDivision);

            int accesoTipoVenta = 0;
            if (Usuario.TienePermisos(new string[] { "accesoTipoVenta" }, ConexionBaseDatos) == "")
            {
                accesoTipoVenta = 1;
            }
            oPermisos.Add("accesoTipoVenta", accesoTipoVenta);
            #endregion

            #region MODELO
            CServicio Servicio = new CServicio();
            Servicio.LlenaObjeto(IdServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdServicio", Servicio.IdServicio));
            Modelo.Add(new JProperty("Servicio", Servicio.Servicio));
            Modelo.Add(new JProperty("Clave", Servicio.Clave));
            Modelo.Add(new JProperty("Precio", Servicio.Precio));

            CTipoServicio TipoServicio = new CTipoServicio();//aqui llena el combo para su edicion
            TipoServicio.LlenaObjeto(Servicio.IdTipoServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoServicio", TipoServicio.IdTipoServicio));
            Modelo.Add(new JProperty("TipoServicio", TipoServicio.TipoServicio));
            JArray JTipoServicios = new JArray();
            Dictionary<string, object> ParametrosTS = new Dictionary<string, object>();
            ParametrosTS.Add("Baja", 0);
            foreach (CTipoServicio oTipoServicio in TipoServicio.LlenaObjetosFiltros(ParametrosTS, ConexionBaseDatos))
            {
                JObject JTipoServicio = new JObject();
                JTipoServicio.Add(new JProperty("IdTipoServicio", oTipoServicio.IdTipoServicio));
                JTipoServicio.Add(new JProperty("TipoServicio", oTipoServicio.TipoServicio));
                if (TipoServicio.IdTipoServicio == oTipoServicio.IdTipoServicio)
                {
                    JTipoServicio.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JTipoServicio.Add(new JProperty("Selected", 0));
                }
                JTipoServicios.Add(JTipoServicio);
            }
            Modelo.Add(new JProperty("TipoServicios", JTipoServicios));

            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.LlenaObjeto(Servicio.IdTipoVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoVenta", TipoVenta.IdTipoVenta));
            Modelo.Add(new JProperty("TipoVenta", TipoVenta.TipoVenta));

            JArray JTipoVentas = new JArray();
            Dictionary<string, object> ParametrosTV = new Dictionary<string, object>();
            ParametrosTV.Add("Baja", 0);
            foreach (CTipoVenta oTipoVenta in TipoVenta.LlenaObjetosFiltros(ParametrosTV, ConexionBaseDatos))
            {
                JObject JTipoVenta = new JObject();
                JTipoVenta.Add(new JProperty("IdTipoVenta", oTipoVenta.IdTipoVenta));
                JTipoVenta.Add(new JProperty("TipoVenta", oTipoVenta.TipoVenta));
                if (TipoVenta.IdTipoVenta == oTipoVenta.IdTipoVenta)
                {
                    JTipoVenta.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JTipoVenta.Add(new JProperty("Selected", 0));
                }
                JTipoVentas.Add(JTipoVenta);
            }
            Modelo.Add(new JProperty("TipoVentas", JTipoVentas));


            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdUnidadCompraVenta", UnidadCompraVenta.IdUnidadCompraVenta));
            Modelo.Add(new JProperty("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta));
            JArray JUnidadCompraVentas = new JArray();
            Dictionary<string, object> ParametrosUM = new Dictionary<string, object>();
            ParametrosUM.Add("Baja", 0);
            foreach (CUnidadCompraVenta oUnidadCompraVenta in UnidadCompraVenta.LlenaObjetosFiltros(ParametrosUM, ConexionBaseDatos))
            {
                JObject JUnidadCompraVenta = new JObject();
                JUnidadCompraVenta.Add(new JProperty("IdUnidadCompraVenta", oUnidadCompraVenta.IdUnidadCompraVenta));
                JUnidadCompraVenta.Add(new JProperty("UnidadCompraVenta", oUnidadCompraVenta.UnidadCompraVenta));
                if (UnidadCompraVenta.IdUnidadCompraVenta == oUnidadCompraVenta.IdUnidadCompraVenta)
                {
                    JUnidadCompraVenta.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JUnidadCompraVenta.Add(new JProperty("Selected", 0));
                }
                JUnidadCompraVentas.Add(JUnidadCompraVenta);
            }
            Modelo.Add(new JProperty("UnidadCompraVentas", JUnidadCompraVentas));

            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(Servicio.IdTipoMoneda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
            Modelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));
            JArray JTipoMonedas = new JArray();
            Dictionary<string, object> ParametrosMO = new Dictionary<string, object>();
            ParametrosMO.Add("Baja", 0);
            foreach (CTipoMoneda oTipoMoneda in TipoMoneda.LlenaObjetosFiltros(ParametrosMO, ConexionBaseDatos))
            {
                JObject JTipoMoneda = new JObject();
                JTipoMoneda.Add(new JProperty("IdTipoMoneda", oTipoMoneda.IdTipoMoneda));
                JTipoMoneda.Add(new JProperty("TipoMoneda", oTipoMoneda.TipoMoneda));
                if (TipoMoneda.IdTipoMoneda == oTipoMoneda.IdTipoMoneda)
                {
                    JTipoMoneda.Add(new JProperty("Selected", 1));
                }
                else
                {
                    JTipoMoneda.Add(new JProperty("Selected", 0));
                }
                JTipoMonedas.Add(JTipoMoneda);
            }
            Modelo.Add("TipoMonedas", JTipoMonedas);
            Modelo.Add("IdTipoIVA", Servicio.IdTipoIVA);
            Modelo.Add("TiposIVA", CTipoIVA.ObtenerJsonTiposIVA(Servicio.IdTipoIVA, ConexionBaseDatos));
            Modelo.Add("Divisiones", CJson.ObtenerJsonDivision(Servicio.IdDivision, ConexionBaseDatos));
            #endregion

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
    public static string ObtenerFormaServicioAEnrolar(int IdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarServicio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarServicio" }, ConexionBaseDatos) == "")
        {
            puedeEditarServicio = 1;
        }
        oPermisos.Add("puedeEditarServicio", puedeEditarServicio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CServicio Servicio = new CServicio();
            Servicio.LlenaObjeto(IdServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdServicio", Servicio.IdServicio));
            Modelo.Add(new JProperty("Servicio", Servicio.Servicio));

            CTipoServicio TipoServicio = new CTipoServicio();//aqui llena el campo de consulta
            TipoServicio.LlenaObjeto(Servicio.IdTipoServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoServicio", TipoServicio.TipoServicio));

            Modelo.Add(new JProperty("Clave", Servicio.Clave));

            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.LlenaObjeto(Servicio.IdTipoVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoVenta", TipoVenta.TipoVenta));


            CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
            UnidadCompraVenta.LlenaObjeto(Servicio.IdUnidadCompraVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("UnidadCompraVenta", UnidadCompraVenta.UnidadCompraVenta));

            Modelo.Add(new JProperty("Precio", Servicio.Precio));

            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(Servicio.IdTipoMoneda, ConexionBaseDatos);
            Modelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));

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
    public static string ObtenerFormaDescuento(int pIdServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarServicio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarServicio" }, ConexionBaseDatos) == "")
        {
            puedeEditarServicio = 1;
        }
        oPermisos.Add("puedeEditarServicio", puedeEditarServicio);

        if (respuesta == "Conexion Establecida")
        {

            JObject Modelo = new JObject();
            CServicio Servicio = new CServicio();
            Servicio.LlenaObjeto(pIdServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdServicio", Servicio.IdServicio));

            Modelo.Add(new JProperty("Permisos", oPermisos));
            oRespuesta.Add(new JProperty("Error", 0));
            oRespuesta.Add(new JProperty("Modelo", Modelo));

        }

        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }
    #endregion

    #region ObtenerListas
    [WebMethod]
    public static string ObtenerListaTipoServicio()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonTipoServicio(true, ConexionBaseDatos));
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
    public static string ObtenerListaTipoVenta()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonTipoVenta(true, ConexionBaseDatos));
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
    public static string ObtenerListaUnidadCompraVenta()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CJson.ObtenerJsonUnidadCompraVenta(true, ConexionBaseDatos));
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
    public static string ObtenerListaDivision()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
            Modelo.Add("Opciones", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));

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
    #endregion

    #region Metodos de Accion
    [WebMethod]
    public static string AgregarServicio(Dictionary<string, object> pServicio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CServicio Servicio = new CServicio();
            Servicio.Servicio = Convert.ToString(pServicio["Servicio"]);
            Servicio.IdTipoServicio = Convert.ToInt32(pServicio["IdTipoServicio"]);
            Servicio.Clave = Convert.ToString(pServicio["Clave"]);
            Servicio.IdTipoVenta = Convert.ToInt32(pServicio["IdTipoVenta"]);
            Servicio.IdUnidadCompraVenta = Convert.ToInt32(pServicio["IdUnidadCompraVenta"]);
            Servicio.Precio = Convert.ToDecimal(pServicio["Precio"]);
            Servicio.IdTipoMoneda = Convert.ToInt32(pServicio["IdTipoMoneda"]);
            Servicio.IdDivision = Convert.ToInt32(pServicio["IdDivision"]);
            Servicio.IdTipoIVA = Convert.ToInt32(pServicio["IdTipoIVA"]);
            Servicio.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Servicio.Fecha = Convert.ToDateTime(DateTime.Now);

            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
            string validacion = ValidarServicio(Servicio, Usuario, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Servicio.Agregar(ConexionBaseDatos);
                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Servicio.IdServicio;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
                HistorialGenerico.Comentario = "Se inserto el servicio";
                HistorialGenerico.AgregarHistorialGenerico("Servicio", ConexionBaseDatos);

                oRespuesta.Add(new JProperty("Error", 0));
            }
            else
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", validacion));

                Dictionary<string, object> ParametrosS = new Dictionary<string, object>();
                ParametrosS.Add("Clave", Servicio.Clave);
                JArray JServicios = new JArray();
                foreach (CServicio oServicio in Servicio.LlenaObjetosFiltros(ParametrosS, ConexionBaseDatos))
                {
                    JObject Modelo = new JObject();
                    Modelo.Add("IdServicio", oServicio.IdServicio);
                    oRespuesta.Add(new JProperty("Modelo", Modelo));
                }
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        { return "1|" + respuesta; }
    }

    [WebMethod]
    public static string EditarServicio(Dictionary<string, object> pServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CServicio Servicio = new CServicio();
        Servicio.LlenaObjeto(Convert.ToInt32(pServicio["IdServicio"]), ConexionBaseDatos);
        int idTipoIVA_Anterior = Servicio.IdTipoIVA;

        Servicio = new CServicio();
        Servicio.LlenaObjeto(Convert.ToInt32(pServicio["IdServicio"]), ConexionBaseDatos);
        Servicio.IdServicio = Convert.ToInt32(pServicio["IdServicio"]);
        Servicio.Servicio = Convert.ToString(pServicio["Servicio"]);
        Servicio.IdTipoServicio = Convert.ToInt32(pServicio["IdTipoServicio"]);
        Servicio.Clave = Convert.ToString(pServicio["Clave"]);
        Servicio.IdTipoVenta = Convert.ToInt32(pServicio["IdTipoVenta"]);
        Servicio.IdUnidadCompraVenta = Convert.ToInt32(pServicio["IdUnidadCompraVenta"]);
        Servicio.Precio = Convert.ToInt32(pServicio["Precio"]);
        Servicio.IdTipoIVA = Convert.ToInt32(pServicio["IdTipoIVA"]);
        Servicio.IdTipoMoneda = Convert.ToInt32(pServicio["IdTipoMoneda"]);
        Servicio.IdDivision = Convert.ToInt32(pServicio["IdDivision"]);

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);
        string validacion = ValidarServicio(Servicio, Usuario, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Servicio.Editar(ConexionBaseDatos);
            string cambioIVA = string.Empty;
            if (idTipoIVA_Anterior != Servicio.IdTipoIVA)
            {
                string TipoIVA_Anterior = string.Empty;
                string TipoIVA_Actual = string.Empty;

                CTipoIVA TipoIVA = new CTipoIVA();
                TipoIVA.LlenaObjeto(idTipoIVA_Anterior, ConexionBaseDatos);
                TipoIVA_Anterior = TipoIVA.TipoIVA;

                TipoIVA = new CTipoIVA();
                TipoIVA.LlenaObjeto(Servicio.IdTipoIVA, ConexionBaseDatos);
                TipoIVA_Actual = TipoIVA.TipoIVA;

                cambioIVA = "El IVA cambio de" + TipoIVA_Anterior + " a " + TipoIVA_Actual;
            }
            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Servicio.IdServicio;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se actualizo el servicio. " + cambioIVA;
            HistorialGenerico.AgregarHistorialGenerico("Servicio", ConexionBaseDatos);

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
    public static string AgregarDescuento(Dictionary<string, object> pDescuento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CDescuentoServicio DescuentoServicio = new CDescuentoServicio();
            DescuentoServicio.IdServicio = Convert.ToInt32(pDescuento["IdServicio"]);
            DescuentoServicio.Descuento = Convert.ToDecimal(pDescuento["Descuento"]);
            DescuentoServicio.DescuentoServicio = Convert.ToString(pDescuento["MotivoDescuento"]);
            DescuentoServicio.Baja = false;
            string validacion = ValidarDescuento(DescuentoServicio, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                DescuentoServicio.Agregar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdServicio, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CServicio Servicio = new CServicio();
            Servicio.IdServicio = pIdServicio;
            Servicio.Baja = pBaja;
            Servicio.Eliminar(ConexionBaseDatos);
            CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
            HistorialGenerico.IdGenerico = Servicio.IdServicio;
            HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            HistorialGenerico.Fecha = Convert.ToDateTime(DateTime.Now);
            HistorialGenerico.Comentario = "Se dio de baja el servicio";
            HistorialGenerico.AgregarHistorialGenerico("Servicio", ConexionBaseDatos);
            respuesta = "0|ServicioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string CambiarEstatusDescuento(int pIdDescuentoServicio, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDescuentoServicio DescuentoServicio = new CDescuentoServicio();
            DescuentoServicio.IdDescuentoServicio = pIdDescuentoServicio;
            DescuentoServicio.Baja = pBaja;
            DescuentoServicio.Eliminar(ConexionBaseDatos);
            respuesta = "0|DescuentoServicioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }
    #endregion

    #region Validaciones
    private static string ValidarServicio(CServicio pServicio, CUsuario pUsuario, CConexion pConexion)
    {
        string errores = "";
        if (pServicio.IdTipoServicio == 0)
        { errores = errores + "<span>*</span> El campo tipo de servicio esta vac&iacute;o, favor de seleccionarlo.<br />"; }

        if (pServicio.Clave == "")
        { errores = errores + "<span>*</span> El campo clave esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pServicio.IdUnidadCompraVenta == 0)
        { errores = errores + "<span>*</span> El campo unidad de media esta vac&iacute;o, favor de seleccionarlo.<br />"; }

        if (pServicio.IdDivision == 0)
        { errores = errores + "<span>*</span> El campo división esta vac&iacute;o, favor de seleccionarlo.<br />"; }

        if (pServicio.Servicio == "")
        { errores = errores + "<span>*</span> El campo descripción esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pServicio.Precio == 0)
        { errores = errores + "<span>*</span> El campo precio esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pServicio.IdTipoMoneda == 0)
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vac&iacute;o, favor de seleccionarlo.<br />"; }

        if (pServicio.IdTipoVenta == 0)
        { errores = errores + "<span>*</span> El campo tipo de venta esta vac&iacute;o, favor de seleccionarlo.<br />"; }

        if (pServicio.IdServicio == 0)
        {
            if (pServicio.ExisteServicio(pServicio.Clave, pUsuario.IdSucursalActual, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de servicio " + pServicio.Clave + " ya esta dada de alta.<br />";
            }
        }
        else
        {
            if (pServicio.ExisteServicioEditar(pServicio.Clave, pServicio.IdServicio, pUsuario.IdSucursalActual, pConexion) == 1)
            {
                errores = errores + "<span>*</span> Esta clave de servicio " + pServicio.Clave + " ya esta dada de alta.<br />";
            }
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarDescuento(CDescuentoServicio pDescuento, CConexion pConexion)
    {
        string errores = "";
        if (pDescuento.Descuento == 0)
        { errores = errores + "<span>*</span> El campo descuento esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (pDescuento.DescuentoServicio == "")
        { errores = errores + "<span>*</span> El campo descripción del descuento esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        //¿Usuario o correo existen?
        if (pDescuento.Descuento != 0)
        {
            if (pDescuento.ExisteDescuento(pDescuento.Descuento, pDescuento.IdServicio, pConexion))
            { errores = errores + "<span>*</span> Este monto de descuento " + pDescuento.Descuento + " ya esta dado de alta.<br />"; }
        }

        return errores;
    }
    #endregion
}