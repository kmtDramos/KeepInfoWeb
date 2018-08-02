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

public partial class Producto : System.Web.UI.Page
{
    public static string ticks = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridProducto
        CJQGrid GridProducto = new CJQGrid();
        GridProducto.NombreTabla = "grdProducto";
        GridProducto.CampoIdentificador = "IdProducto";
        GridProducto.ColumnaOrdenacion = "Producto";
        GridProducto.Metodo = "ObtenerProductos";
        GridProducto.TituloTabla = "Catálogo de productos";

        //IdProducto
        CJQColumn ColIdProducto = new CJQColumn();
        ColIdProducto.Nombre = "IdProducto";
        ColIdProducto.Oculto = "true";
        ColIdProducto.Encabezado = "IdProducto";
        ColIdProducto.Buscador = "false";
        GridProducto.Columnas.Add(ColIdProducto);

        //Clave Interna
        CJQColumn ColClaveInterna = new CJQColumn();
        ColClaveInterna.Nombre = "ClaveInterna";
        ColClaveInterna.Encabezado = "Codigo Interno";
        ColClaveInterna.Ancho = "100";
        ColClaveInterna.Alineacion = "Left";
        ColClaveInterna.Buscador = "true";
        GridProducto.Columnas.Add(ColClaveInterna);

        //Numero Parte
        CJQColumn ColNumeroParte = new CJQColumn();
        ColNumeroParte.Nombre = "NumeroParte";
        ColNumeroParte.Encabezado = "Numero Parte";
        ColNumeroParte.Ancho = "100";
        ColNumeroParte.Alineacion = "Left";
        ColNumeroParte.Buscador = "true";
        GridProducto.Columnas.Add(ColNumeroParte);

        //Producto
        CJQColumn ColProducto = new CJQColumn();
        ColProducto.Nombre = "Producto";
        ColProducto.Encabezado = "Producto";
        ColProducto.Ancho = "200";
        ColProducto.Alineacion = "Left";
        GridProducto.Columnas.Add(ColProducto);

        //Clave
        CJQColumn ColClaveSKU = new CJQColumn();
        ColClaveSKU.Nombre = "Clave";
        ColClaveSKU.Encabezado = "Clave SKU";
        ColClaveSKU.Ancho = "100";
        ColClaveSKU.Alineacion = "Left";
        ColClaveSKU.Buscador = "true";
        GridProducto.Columnas.Add(ColClaveSKU);

        //Grupo
        CJQColumn ColGrupo = new CJQColumn();
        ColGrupo.Nombre = "Grupo";
        ColGrupo.Encabezado = "Grupo";
        ColGrupo.Ancho = "100";
        ColGrupo.Alineacion = "Left";
        ColGrupo.Buscador = "true";
        ColGrupo.TipoBuscador = "Combo";
        ColGrupo.StoredProcedure.CommandText = "spc_ProductoGrupo_Consulta";
        GridProducto.Columnas.Add(ColGrupo);

        //Categoria
        CJQColumn ColCategoria = new CJQColumn();
        ColCategoria.Nombre = "Categoria";
        ColCategoria.Encabezado = "Categoría";
        ColCategoria.Ancho = "100";
        ColCategoria.Alineacion = "Left";
        ColCategoria.Buscador = "true";
        ColCategoria.TipoBuscador = "Combo";
        ColCategoria.StoredProcedure.CommandText = "spc_ProductoCategorias_Consulta";
        GridProducto.Columnas.Add(ColCategoria);

        //SubGrupo
        CJQColumn ColSubGrupo = new CJQColumn();
        ColSubGrupo.Nombre = "SubGrupo";
        ColSubGrupo.Encabezado = "SubGrupo";
        ColSubGrupo.Ancho = "100";
        ColSubGrupo.Alineacion = "Left";
        ColSubGrupo.Buscador = "true";
        ColSubGrupo.TipoBuscador = "Combo";
        ColSubGrupo.StoredProcedure.CommandText = "spc_ProductoSubCategorias_Consulta";
        GridProducto.Columnas.Add(ColSubGrupo);

        //Marca
        CJQColumn ColMarca = new CJQColumn();
        ColMarca.Nombre = "Marca";
        ColMarca.Encabezado = "Marca";
        ColMarca.Ancho = "100";
        ColMarca.Alineacion = "Left";
        ColMarca.Buscador = "true";
        GridProducto.Columnas.Add(ColMarca);

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
        GridProducto.Columnas.Add(ColBaja);

        //Descuentos
        CJQColumn ColConsultarDescuento = new CJQColumn();
        ColConsultarDescuento.Nombre = "Descuento";
        ColConsultarDescuento.Encabezado = "Descuento";
        ColConsultarDescuento.Etiquetado = "Imagen";
        ColConsultarDescuento.Imagen = "descuento.png";
        ColConsultarDescuento.Estilo = "divImagenConsultar imgFormaConsultarDescuentoProducto";
        ColConsultarDescuento.Buscador = "false";
        ColConsultarDescuento.Ordenable = "false";
        ColConsultarDescuento.Ancho = "45";
        GridProducto.Columnas.Add(ColConsultarDescuento);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarProducto";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridProducto.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdProducto", GridProducto.GeneraGrid(), true);

        //GridDescuento
        CJQGrid GridDescuentoProducto = new CJQGrid();
        GridDescuentoProducto.NombreTabla = "grdDescuentoProducto";
        GridDescuentoProducto.CampoIdentificador = "IdDescuentoProducto";
        GridDescuentoProducto.ColumnaOrdenacion = "DescuentoProducto";
        GridDescuentoProducto.TipoOrdenacion = "DESC";
        GridDescuentoProducto.Metodo = "ObtenerDescuentoProducto";
        GridDescuentoProducto.TituloTabla = "Descuentos del producto";
        GridDescuentoProducto.GenerarFuncionFiltro = false;
        GridDescuentoProducto.GenerarFuncionTerminado = false;
        GridDescuentoProducto.Altura = 300;
        GridDescuentoProducto.Ancho = 600;
        GridDescuentoProducto.NumeroRegistros = 15;
        GridDescuentoProducto.RangoNumeroRegistros = "15,30,60";

        //IdProducto
        CJQColumn ColIdDescuentoProducto = new CJQColumn();
        ColIdDescuentoProducto.Nombre = "IdDescuentoProducto";
        ColIdDescuentoProducto.Oculto = "true";
        ColIdDescuentoProducto.Encabezado = "IdProducto";
        ColIdDescuentoProducto.Buscador = "false";
        GridDescuentoProducto.Columnas.Add(ColIdDescuentoProducto);

        //Producto
        CJQColumn ColProductoDelDescuento = new CJQColumn();
        ColProductoDelDescuento.Nombre = "Producto";
        ColProductoDelDescuento.Encabezado = "Producto";
        ColProductoDelDescuento.Buscador = "false";
        ColProductoDelDescuento.Alineacion = "left";
        ColProductoDelDescuento.Ancho = "80";
        GridDescuentoProducto.Columnas.Add(ColProductoDelDescuento);

        //Descuento del Producto
        CJQColumn ColDescuentoProducto = new CJQColumn();
        ColDescuentoProducto.Nombre = "DescuentoProducto";
        ColDescuentoProducto.Encabezado = "Descripción";
        ColDescuentoProducto.Buscador = "false";
        ColDescuentoProducto.Alineacion = "left";
        ColDescuentoProducto.Ancho = "200";
        GridDescuentoProducto.Columnas.Add(ColDescuentoProducto);

        //Descuento
        CJQColumn ColDescuento = new CJQColumn();
        ColDescuento.Nombre = "Descuento";
        ColDescuento.Encabezado = "Descuento";
        ColDescuento.Buscador = "false";
        ColDescuento.Ancho = "80";
        ColDescuento.Alineacion = "right";
        GridDescuentoProducto.Columnas.Add(ColDescuento);

        //Baja
        CJQColumn ColBajaDescuento = new CJQColumn();
        ColBajaDescuento.Nombre = "AI_DescuentoProducto";
        ColBajaDescuento.Encabezado = "A/I";
        ColBajaDescuento.Ordenable = "false";
        ColBajaDescuento.Etiquetado = "A/I";
        ColBajaDescuento.Ancho = "55";
        ColBajaDescuento.Buscador = "true";
        ColBajaDescuento.TipoBuscador = "Combo";
        ColBajaDescuento.StoredProcedure.CommandText = "spc_ManejadorActivos_Consulta";
        GridDescuentoProducto.Columnas.Add(ColBajaDescuento);

        ClientScript.RegisterStartupScript(this.GetType(), "grdDescuentoProducto", GridDescuentoProducto.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();

        ticks = DateTime.Now.Ticks.ToString();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerProductos(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pClaveInterna, string pNumeroParte, string pProducto, string pClave, int pCategoria, int pGrupo, int pSubGrupo, string pMarca, int pAI)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdProducto", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pNumeroParte", SqlDbType.VarChar, 255).Value = Convert.ToString(pNumeroParte);
        Stored.Parameters.Add("pCodigoInterno", SqlDbType.VarChar, 255).Value = Convert.ToString(pClaveInterna);
        Stored.Parameters.Add("pProducto", SqlDbType.VarChar, 250).Value = Convert.ToString(pProducto);
        Stored.Parameters.Add("pClave", SqlDbType.VarChar, 250).Value = Convert.ToString(pClave);
        Stored.Parameters.Add("pCategoria", SqlDbType.Int).Value = Convert.ToInt32(pCategoria);
        Stored.Parameters.Add("pGrupo", SqlDbType.Int).Value = Convert.ToInt32(pGrupo);
        Stored.Parameters.Add("pSubGrupo", SqlDbType.Int).Value = Convert.ToInt32(pSubGrupo);
        Stored.Parameters.Add("pMarca", SqlDbType.VarChar, 250).Value = Convert.ToString(pMarca);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerDescuentoProducto(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, int pIdProducto, int pBaja)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdDescuentoProducto", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pIdProducto", SqlDbType.Int).Value = pIdProducto;
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pBaja;
        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string ObtenerFiltroCategorias(int pIdGrupo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CCategoria.ObtenerJsonCategoriasFiltroPorGrupo(pIdGrupo, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerFiltroSubGrupos(int pIdCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Grupos", CSubCategoria.ObtenerListadoSubCategoria(pIdCategoria, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string BuscarProducto(string pProducto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonProducto = new CJson();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_Consultar_FiltroPorProducto";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pProducto", pProducto);
        return jsonProducto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarClave(string pClave)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonProducto = new CJson();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_Consultar_FiltroPorMarca";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pClave", pClave);
        return jsonProducto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarMarca(string pMarca)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonProducto = new CJson();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_Consultar_FiltroPorMarca";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pMarca", pMarca);
        return jsonProducto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarNumeroParte(string pNumeroParte)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonProducto = new CJson();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_Consultar_FiltroPorNumeroPartida";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pNumeroParte", pNumeroParte);
        return jsonProducto.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string BuscarClaveInterna(string pClaveInterna)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonProducto = new CJson();
        jsonProducto.StoredProcedure.CommandText = "sp_Producto_Consultar_FiltroPorClaveInterna";
        jsonProducto.StoredProcedure.Parameters.AddWithValue("@pClaveInterna", pClaveInterna);
        return jsonProducto.ObtenerJsonString(ConexionBaseDatos);
    }

    public static JArray ObtenerJsonLinea(CConexion pConexion, int IdLinea)
    {
        CLinea Linea = new CLinea();
        JArray JLinea = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        foreach (CLinea oLinea in Linea.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject Jlinea = new JObject();
            Jlinea.Add("Valor", oLinea.IdLinea);
            Jlinea.Add("Descripcion", oLinea.Descripcion);
            if (oLinea.IdLinea == IdLinea)
            {
                Jlinea.Add(new JProperty("Selected", "1"));
            }
            else
            {
                Jlinea.Add(new JProperty("Selected", "0"));
            }

            JLinea.Add(Jlinea);
        }
        return JLinea;
    }

    public static JArray ObtenerJsonEstante(CConexion pConexion, int IdLinea, int IdEstante)
    {
        CEstante Estante = new CEstante();
        JArray JEstante = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        Parametros.Add("IdLinea", IdLinea);
        foreach (CEstante oEstante in Estante.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject Jestante = new JObject();
            Jestante.Add("Valor", oEstante.IdEstante);
            Jestante.Add("Descripcion", oEstante.Descripcion);
            if (oEstante.IdEstante == IdEstante)
            {
                Jestante.Add(new JProperty("Selected", "1"));
            }
            else
            {
                Jestante.Add(new JProperty("Selected", "0"));
            }

            JEstante.Add(Jestante);
        }
        return JEstante;
    }

    public static JArray ObtenerJsonRepisa(CConexion pConexion, int IdEstante, int IdRepisa)
    {
        CRepisa Repisa = new CRepisa();
        JArray JRepisa = new JArray();

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("Baja", 0);
        Parametros.Add("IdEstante", IdEstante);
        foreach (CRepisa oRepisa in Repisa.LlenaObjetosFiltros(Parametros, pConexion))
        {
            JObject Jrepisa = new JObject();
            Jrepisa.Add("Valor", oRepisa.IdRepisa);
            Jrepisa.Add("Descripcion", oRepisa.Descripcion);
            if (oRepisa.IdRepisa == IdRepisa)
            {
                Jrepisa.Add(new JProperty("Selected", "1"));
            }
            else
            {
                Jrepisa.Add(new JProperty("Selected", "0"));
            }

            JRepisa.Add(Jrepisa);
        }
        return JRepisa;
    }

    [WebMethod]
    public static string ObtenerListaEstantes(int pIdLinea)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", ObtenerJsonEstante(ConexionBaseDatos, pIdLinea, 0));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaRepisas(int pIdEstante)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", ObtenerJsonRepisa(ConexionBaseDatos, pIdEstante, 0));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerFormaAgregarProducto()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Marcas", CMarca.ObtenerJsonMarcas(ConexionBaseDatos));
            Modelo.Add("Grupos", CGrupo.ObtenerJsonGrupos(ConexionBaseDatos));
            //Modelo.Add("Categorias", CCategoria.ObtenerJsonCategorias(ConexionBaseDatos));
            Modelo.Add("TiposVenta", CTipoVenta.ObtenerJsonTiposVenta(1, ConexionBaseDatos));
            Modelo.Add("UnidadesCompraVenta", CUnidadCompraVenta.ObtenerJsonUnidadesCompraVenta(ConexionBaseDatos));
            Modelo.Add("TiposMoneda", CTipoMoneda.ObtenerJsonTiposMoneda(ConexionBaseDatos));
            Modelo.Add("TiposIVA", CTipoIVA.ObtenerJsonTiposIVA(ConexionBaseDatos));
            Modelo.Add("Linea",ObtenerJsonLinea(ConexionBaseDatos,0));
            Modelo.Add("Division", CDivision.ObtenerJsonDivisionesActivas(ConexionBaseDatos));
            //Modelo.Add("Rack", ObtenerJsonEstante(ConexionBaseDatos, 0));
            //Modelo.Add("Repisa", ObtenerJsonRepisa(ConexionBaseDatos, 0));

            int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            CUsuario Usario = new CUsuario();
            Usario.LlenaObjeto(idUsuario, ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usario.IdSucursalActual, ConexionBaseDatos);

            Modelo.Add("IVASucursal", Sucursal.IVAActual);

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
    public static string ObtenerFormaConsultarProducto(int pIdProducto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarProducto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarProducto" }, ConexionBaseDatos) == "")
        {
            puedeEditarProducto = 1;
        }
        oPermisos.Add("puedeEditarProducto", puedeEditarProducto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CProducto.ObtenerJsonProducto(Modelo, pIdProducto, ConexionBaseDatos);
            Modelo.Add("CaracteristicasProducto", CCaracteristicaProducto.ObtenerJsonCaracteristicasProducto(pIdProducto, ConexionBaseDatos));

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
    public static string ObtenerFormaEditarProducto(int pIdProducto)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarProducto = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarProducto" }, ConexionBaseDatos) == "")
        {
            puedeEditarProducto = 1;
        }
        oPermisos.Add("puedeEditarProducto", puedeEditarProducto);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo = CProducto.ObtenerJsonProducto(Modelo, pIdProducto, ConexionBaseDatos);
            Modelo.Add("Marcas", CMarca.ObtenerJsonMarcas(Convert.ToInt32(Modelo["IdMarca"].ToString()), ConexionBaseDatos));
            Modelo.Add("Grupos", CGrupo.ObtenerJsonGrupos(Convert.ToInt32(Modelo["IdGrupo"].ToString()), ConexionBaseDatos));
            Modelo.Add("Categorias", CCategoria.ObtenerJsonCategorias(Convert.ToInt32(Modelo["IdCategoria"].ToString()), ConexionBaseDatos));
            Modelo.Add("SubCategorias", CSubCategoria.ObtenerJsonSubCategoria(Convert.ToInt32(Modelo["IdSubCategoria"].ToString()), ConexionBaseDatos));
            Modelo.Add("TiposVenta", CTipoVenta.ObtenerJsonTiposVenta(Convert.ToInt32(Modelo["IdTipoVenta"].ToString()), ConexionBaseDatos));
            Modelo.Add("UnidadesCompraVenta", CUnidadCompraVenta.ObtenerJsonUnidadesCompraVenta(Convert.ToInt32(Modelo["IdUnidadCompraVenta"].ToString()), ConexionBaseDatos));
            Modelo.Add("TiposMoneda", CTipoMoneda.ObtenerJsonTiposMoneda(Convert.ToInt32(Modelo["IdTipoMoneda"].ToString()), ConexionBaseDatos));
            Modelo.Add("CaracteristicasProducto", CCaracteristicaProducto.ObtenerJsonCaracteristicasProducto(pIdProducto, ConexionBaseDatos));
            Modelo.Add("TiposIVA", CTipoIVA.ObtenerJsonTiposIVA(Convert.ToInt32(Modelo["IdTipoIVA"].ToString()), ConexionBaseDatos));


            Modelo.Add("Divisionn", CDivision.ObtenerJsonDivisionesActivas(Convert.ToInt32(Modelo["IdDivision"].ToString()), ConexionBaseDatos));
            Modelo.Add("Lineaa", ObtenerJsonLinea(ConexionBaseDatos, Convert.ToInt32(Modelo["IdLinea"].ToString())));
            Modelo.Add("Rackk", ObtenerJsonEstante(ConexionBaseDatos, Convert.ToInt32(Modelo["IdLinea"].ToString()), Convert.ToInt32(Modelo["IdEstante"].ToString())));
            Modelo.Add("Repisaa", ObtenerJsonRepisa(ConexionBaseDatos, Convert.ToInt32(Modelo["IdEstante"].ToString()), Convert.ToInt32(Modelo["IdRepisa"].ToString())));

            int idUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            CUsuario Usario = new CUsuario();
            Usario.LlenaObjeto(idUsuario, ConexionBaseDatos);

            CSucursal Sucursal = new CSucursal();
            Sucursal.LlenaObjeto(Usario.IdSucursalActual, ConexionBaseDatos);

            Modelo.Add("IVASucursal", Sucursal.IVAActual);


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
    public static string ObtenerFormaAgregarCaracteristica()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Caracteristicas", CCaracteristica.ObtenerJsonCaracteristicas(ConexionBaseDatos));
            Modelo.Add("UnidadesCaracteristica", CUnidadCaracteristica.ObtenerJsonUnidadesCaracteristica(ConexionBaseDatos));

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
    public static string AgregarProducto(Dictionary<string, object> pProducto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProducto Producto = new CProducto();
            Producto.Producto = Convert.ToString(pProducto["Producto"]);
            Producto.Clave = Convert.ToString(pProducto["Clave"]);
            Producto.NumeroParte = Convert.ToString(pProducto["NumeroParte"]);
            Producto.Modelo = Convert.ToString(pProducto["Modelo"]);
            Producto.CodigoBarra = Convert.ToString(pProducto["CodigoBarra"]);
            Producto.IdMarca = Convert.ToInt32(pProducto["IdMarca"]);
            Producto.IdGrupo = Convert.ToInt32(pProducto["IdGrupo"]);
            Producto.IdCategoria = Convert.ToInt32(pProducto["IdCategoria"]);
            Producto.Descripcion = Convert.ToString(pProducto["Descripcion"]);
            Producto.Costo = Convert.ToDecimal(pProducto["Costo"]);
            Producto.IdTipoMoneda = Convert.ToInt32(pProducto["IdTipoMoneda"]);
            Producto.MargenUtilidad = Convert.ToDecimal(pProducto["MargenUtilidad"]);
            Producto.IdTipoVenta = Convert.ToInt32(pProducto["IdTipoVenta"]);
            Producto.IdTipoIVA = Convert.ToInt32(pProducto["IdTipoIVA"]);
            Producto.Precio = ((Producto.Costo * 100) / (100 - Producto.MargenUtilidad));
            Producto.IdUnidadCompraVenta = Convert.ToInt32(pProducto["IdUnidadCompraVenta"]);
            Producto.ValorMedida = Convert.ToDecimal(pProducto["ValorMedida"]);
            Producto.Imagen = Convert.ToString(pProducto["Imagen"]);
            Producto.IdUsuarioAlta = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Producto.FechaAlta = DateTime.Now;
            Producto.IdSubCategoria = Convert.ToInt32(pProducto["IdSubCategoria"]);
            Producto.ClaveProdServ = Convert.ToString(pProducto["ClaveProdServ"]);
            Producto.IdLinea = Convert.ToInt32(pProducto["IdLinea"]);
            Producto.IdEstante = Convert.ToInt32(pProducto["IdEstante"]);
            Producto.IdRepisa = Convert.ToInt32(pProducto["IdRepisa"]);
            Producto.IdDivision = Convert.ToInt32(pProducto["IdDivision"]);
            Producto.ClaveInterna = Convert.ToString(pProducto["CodigoInterno"]);
            Producto.Baja = false;

            CProducto ValidarProductoSKU = new CProducto();
            Dictionary<string, object> ParametrosSKU = new Dictionary<string, object>();
            ParametrosSKU.Add("Clave", Convert.ToString(pProducto["Clave"]));
            ValidarProductoSKU.LlenaObjetoFiltros(ParametrosSKU, ConexionBaseDatos);

            /*if (ValidarProductoSKU.IdProducto != 0)
            {
                oRespuesta.Add(new JProperty("Error", 1));
                oRespuesta.Add(new JProperty("Descripcion", "La clave SKU \"" + Convert.ToString(pProducto["Clave"]) + "\" ya existe"));
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
                return oRespuesta.ToString();
            }*/

            string validacion = ValidarProducto(Producto, ConexionBaseDatos);
            if (validacion == "")
            {
                Producto.Agregar(ConexionBaseDatos);
                foreach (Dictionary<string, object> oCategoriaProducto in (Array)pProducto["CaracteristicasProducto"])
                {
                    CCaracteristicaProducto CaracteristicaProducto = new CCaracteristicaProducto();
                    CaracteristicaProducto.IdCaracteristica = Convert.ToInt32(oCategoriaProducto["IdCaracteristica"]);
                    CaracteristicaProducto.Valor = Convert.ToString(oCategoriaProducto["Valor"]);
                    CaracteristicaProducto.IdUnidadCaracteristica = Convert.ToInt32(oCategoriaProducto["IdUnidadCaracteristica"]);
                    CaracteristicaProducto.IdProducto = Producto.IdProducto;
                    CaracteristicaProducto.Agregar(ConexionBaseDatos);
                }

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Producto.IdProducto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = DateTime.Now;
                HistorialGenerico.Comentario = "Se agregó el producto " + Producto.Producto + ".";
                HistorialGenerico.AgregarHistorialGenerico("Producto", ConexionBaseDatos);

                oRespuesta.Add("Error", 0);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add("Error", 1);
                oRespuesta.Add("Descripcion", validacion);
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string EditarProducto(Dictionary<string, object> pProducto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProducto Producto = new CProducto();
            Producto.LlenaObjeto(Convert.ToInt32(pProducto["IdProducto"]), ConexionBaseDatos);
            int idTipoIVA_Anterior = Producto.IdTipoIVA;

            Producto = new CProducto();
            Producto.IdProducto = Convert.ToInt32(pProducto["IdProducto"]);
            Producto.Producto = Convert.ToString(pProducto["Producto"]);
            Producto.Clave = Convert.ToString(pProducto["Clave"]);
            Producto.NumeroParte = Convert.ToString(pProducto["NumeroParte"]);
            Producto.Modelo = Convert.ToString(pProducto["Modelo"]);
            Producto.CodigoBarra = Convert.ToString(pProducto["CodigoBarra"]);
            Producto.IdMarca = Convert.ToInt32(pProducto["IdMarca"]);
            Producto.IdGrupo = Convert.ToInt32(pProducto["IdGrupo"]);
            Producto.IdCategoria = Convert.ToInt32(pProducto["IdCategoria"]);
            Producto.Descripcion = Convert.ToString(pProducto["Descripcion"]);
            Producto.Costo = Convert.ToDecimal(pProducto["Costo"]);
            Producto.IdTipoMoneda = Convert.ToInt32(pProducto["IdTipoMoneda"]);
            Producto.MargenUtilidad = Convert.ToDecimal(pProducto["MargenUtilidad"]);
            Producto.IdTipoVenta = Convert.ToInt32(pProducto["IdTipoVenta"]);
            Producto.IdTipoIVA = Convert.ToInt32(pProducto["IdTipoIVA"]);
            Producto.Precio = ((Producto.Costo * 100) / (100 - Producto.MargenUtilidad));
            Producto.IdUnidadCompraVenta = Convert.ToInt32(pProducto["IdUnidadCompraVenta"]);
            Producto.ValorMedida = Convert.ToDecimal(pProducto["ValorMedida"]);
            Producto.Imagen = Convert.ToString(pProducto["Imagen"]);
            Producto.IdUsuarioModifico = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
            Producto.FechaModificacion = DateTime.Now;
            Producto.IdSubCategoria = Convert.ToInt32(pProducto["IdSubCategoria"]);
            Producto.ClaveProdServ = Convert.ToString(pProducto["ClaveProdServ"]);
            Producto.IdLinea = Convert.ToInt32(pProducto["IdLinea"]);
            Producto.IdEstante = Convert.ToInt32(pProducto["IdEstante"]);
            Producto.IdRepisa = Convert.ToInt32(pProducto["IdRepisa"]);
            Producto.IdDivision = Convert.ToInt32(pProducto["IdDivision"]);
            Producto.ClaveInterna = Convert.ToString(pProducto["CodigoInterno"]);
            Producto.Baja = false;

            string validacion = ValidarProducto(Producto, ConexionBaseDatos);
            if (validacion == "")
            {
                Producto.Editar(ConexionBaseDatos);

                string cambioIVA = string.Empty;
                if (idTipoIVA_Anterior != Producto.IdTipoIVA)
                {
                    string TipoIVA_Anterior = string.Empty;
                    string TipoIVA_Actual = string.Empty;

                    CTipoIVA TipoIVA = new CTipoIVA();
                    TipoIVA.LlenaObjeto(idTipoIVA_Anterior, ConexionBaseDatos);
                    TipoIVA_Anterior = TipoIVA.TipoIVA;

                    TipoIVA = new CTipoIVA();
                    TipoIVA.LlenaObjeto(Producto.IdTipoIVA, ConexionBaseDatos);
                    TipoIVA_Actual = TipoIVA.TipoIVA;

                    cambioIVA = "El IVA cambio de" + TipoIVA_Anterior + " a " + TipoIVA_Actual;
                }


                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = Producto.IdProducto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = DateTime.Now;
                HistorialGenerico.Comentario = "Se editó el producto " + Producto.Producto + "." + cambioIVA;
                HistorialGenerico.AgregarHistorialGenerico("Producto", ConexionBaseDatos);

                //Actualiza todas las CotizacionDetalle y OrdenCompraDetalle y FacturaDetalle
                CSelectEspecifico actualizar = new CSelectEspecifico();
                actualizar.StoredProcedure.CommandText = "sp_CotizacionDetalle_ActualizarClaveProdServ";
                actualizar.StoredProcedure.Parameters.Add("@ClaveProdServ",SqlDbType.VarChar,50).Value = Convert.ToString(pProducto["ClaveProdServ"]);
                actualizar.StoredProcedure.Parameters.Add("@IdProducto", SqlDbType.Int).Value = Producto.IdProducto;
                actualizar.StoredProcedure.Parameters.Add("@IdServicio", SqlDbType.Int).Value = 0;
                actualizar.Llena(ConexionBaseDatos);
                actualizar.CerrarConsulta();

                actualizar = new CSelectEspecifico();
                actualizar.StoredProcedure.CommandText = "sp_OrdenCompraDetalle_ActualizarClaveProdServ";
                actualizar.StoredProcedure.Parameters.Add("@ClaveProdServ", SqlDbType.VarChar, 50).Value = Convert.ToString(pProducto["ClaveProdServ"]);
                actualizar.StoredProcedure.Parameters.Add("@IdProducto", SqlDbType.Int).Value = Producto.IdProducto;
                actualizar.StoredProcedure.Parameters.Add("@IdServicio", SqlDbType.Int).Value = 0;
                actualizar.Llena(ConexionBaseDatos);
                actualizar.CerrarConsulta();

                actualizar = new CSelectEspecifico();
                actualizar.StoredProcedure.CommandText = "sp_FacturaDetalle_ActualizarClaveProdServ";
                actualizar.StoredProcedure.Parameters.Add("@ClaveProdServ", SqlDbType.VarChar, 50).Value = Convert.ToString(pProducto["ClaveProdServ"]);
                actualizar.StoredProcedure.Parameters.Add("@IdProducto", SqlDbType.Int).Value = Producto.IdProducto;
                actualizar.StoredProcedure.Parameters.Add("@IdServicio", SqlDbType.Int).Value = 0;
                actualizar.Llena(ConexionBaseDatos);
                actualizar.CerrarConsulta();
                

                oRespuesta.Add("Error", 0);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add("Error", 1);
                oRespuesta.Add("Descripcion", validacion);
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string AgregarCaracteristicaProducto(int pIdProducto, int pIdCaracteristica, string pCaracteristica, int pIdUnidadCaracteristica, string pUnidadCaracteristica, string pValor)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCaracteristicaProducto CaracteristicaProducto = new CCaracteristicaProducto();
            CaracteristicaProducto.IdCaracteristica = pIdCaracteristica;
            CaracteristicaProducto.IdUnidadCaracteristica = pIdUnidadCaracteristica;
            CaracteristicaProducto.Valor = pValor;
            CaracteristicaProducto.IdProducto = pIdProducto;

            string validacion = ValidarCaracteristicaProducto(CaracteristicaProducto, ConexionBaseDatos);
            if (validacion == "")
            {
                CaracteristicaProducto.Agregar(ConexionBaseDatos);

                JObject Modelo = new JObject();
                Modelo.Add("IdCaracteristicaProducto", CaracteristicaProducto.IdCaracteristicaProducto);
                Modelo.Add("Caracteristica", pCaracteristica);
                Modelo.Add("UnidadCaracteristica", pUnidadCaracteristica);
                Modelo.Add("Valor", pValor);

                CProducto Producto = new CProducto();
                Producto.LlenaObjeto(pIdProducto, ConexionBaseDatos);
                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = CaracteristicaProducto.IdCaracteristicaProducto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = DateTime.Now;
                HistorialGenerico.Comentario = "Se agregó una caracteristica al producto: " + Producto.Producto + ".";
                HistorialGenerico.AgregarHistorialGenerico("CaracteristicaProducto", ConexionBaseDatos);

                oRespuesta.Add("Modelo", Modelo);
                oRespuesta.Add("Error", 0);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add("Error", 1);
                oRespuesta.Add("Descripcion", validacion);
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string EliminarCaracteristicaProducto(int pIdCaracteristicaProducto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCaracteristicaProducto CaracteristicaProducto = new CCaracteristicaProducto();
            CaracteristicaProducto.LlenaObjeto(pIdCaracteristicaProducto, ConexionBaseDatos);
            CaracteristicaProducto.Baja = true;

            string validacion = ValidarCaracteristicaProducto(CaracteristicaProducto, ConexionBaseDatos);
            if (validacion == "")
            {
                CaracteristicaProducto.Eliminar(ConexionBaseDatos);
                CProducto Producto = new CProducto();
                Producto.LlenaObjeto(CaracteristicaProducto.IdProducto, ConexionBaseDatos);

                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdGenerico = CaracteristicaProducto.IdCaracteristicaProducto;
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = DateTime.Now;
                HistorialGenerico.Comentario = "Se eliminó una caracteristica al producto: " + Producto.Producto + ".";
                HistorialGenerico.AgregarHistorialGenerico("CaracteristicaProducto", ConexionBaseDatos);

                oRespuesta.Add("Error", 0);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add("Error", 1);
                oRespuesta.Add("Descripcion", validacion);
            }
            ConexionBaseDatos.CerrarBaseDatosSqlServer();
            return oRespuesta.ToString();
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
            return oRespuesta.ToString();
        }
    }

    [WebMethod]
    public static string AgregarDescuentoProducto(Dictionary<string, object> pDescuentoProducto)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CDescuentoProducto DescuentoProducto = new CDescuentoProducto();
            DescuentoProducto.IdProducto = Convert.ToInt32(pDescuentoProducto["IdProducto"]);
            DescuentoProducto.Descuento = Convert.ToDecimal(pDescuentoProducto["Descuento"]);
            DescuentoProducto.DescuentoProducto = Convert.ToString(pDescuentoProducto["DescripcionDescuento"]);
            DescuentoProducto.Baja = false;

            string validacion = ValidarDescuentoProducto(DescuentoProducto, ConexionBaseDatos);
            if (validacion == "")
            {
                DescuentoProducto.Agregar(ConexionBaseDatos);
                CHistorialGenerico HistorialGenerico = new CHistorialGenerico();
                HistorialGenerico.IdUsuario = Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]);
                HistorialGenerico.Fecha = DateTime.Now;
                HistorialGenerico.IdGenerico = DescuentoProducto.IdDescuentoProducto;
                HistorialGenerico.Comentario = "Se agregó un descuesto con el " + DescuentoProducto.DescuentoProducto.ToString() + " al producto con Id: " + DescuentoProducto.IdProducto;
                HistorialGenerico.Agregar(ConexionBaseDatos);

                oRespuesta.Add("Error", 0);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
            else
            {
                oRespuesta.Add("Error", 1);
                oRespuesta.Add("Descripcion", validacion);
                ConexionBaseDatos.CerrarBaseDatosSqlServer();
            }
        }
        else
        {
            oRespuesta.Add("Error", 1);
            oRespuesta.Add("Descripcion", respuesta);
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return oRespuesta.ToString();
    }

    [WebMethod]
    public static string CambiarEstatus(int pIdProducto, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CProducto Producto = new CProducto();
            Producto.IdProducto = pIdProducto;
            Producto.Baja = pBaja;
            Producto.Eliminar(ConexionBaseDatos);
            respuesta = "0|ProductoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerListaMarcas(int pIdMarca)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CMarca.ObtenerJsonMarcas(pIdMarca, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaGrupos(int pIdGrupo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CGrupo.ObtenerJsonGrupos(pIdGrupo, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaCategorias(int pIdGrupo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CCategoria.ObtenerJsonCategoriasFiltroPorGrupo(pIdGrupo, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaTiposVenta(int pIdTipoVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CTipoVenta.ObtenerJsonTiposVenta(pIdTipoVenta, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerListaUnidadesCompraVenta(int pIdUnidadCompraVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CUnidadCompraVenta.ObtenerJsonUnidadesCompraVenta(pIdUnidadCompraVenta, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string CambiarEstatusDescuentoProducto(int pIdDescuentoProducto, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CDescuentoProducto DescuentoProducto = new CDescuentoProducto();
            DescuentoProducto.IdDescuentoProducto = pIdDescuentoProducto;
            DescuentoProducto.Baja = pBaja;
            DescuentoProducto.Eliminar(ConexionBaseDatos);
            respuesta = "0|DescuentoProductoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    [WebMethod]
    public static string ObtenerListaSubCategorias(int pIdCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Opciones", CSubCategoria.ObtenerListadoSubCategoria(pIdCategoria, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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

    //Validaciones
    private static string ValidarProducto(CProducto pProducto, CConexion pConexion)
    {
        var errores = "";

        if (pProducto.Producto == "")
        { errores = errores + "<span>*</span> El campo nombre del producto esta vacío, favor de capturarlo.<br />"; }
        /*if (pProducto.Clave == "")
        { errores = errores + "<span>*</span> El campo clave esta vacío, favor de capturarlo.<br />"; }*/
        if (pProducto.NumeroParte == "")
        { errores = errores + "<span>*</span> El campo numero de parte esta vacío, favor de capturarlo.<br />"; }
        if (pProducto.Modelo == "")
        { errores = errores + "<span>*</span> El campo modelo esta vacío, favor de capturarlo.<br />"; }
        /*if (pProducto.IdMarca == 0)
        { errores = errores + "<span>*</span> El campo marca esta vacío, favor de capturarlo.<br />"; }*/
        /*if (pProducto.IdGrupo == 0)
        { errores = errores + "<span>*</span> El campo grupo esta vacío, favor de capturarlo.<br />"; }*/
        /*if (pProducto.IdCategoria == 0)
        { errores = errores + "<span>*</span> El campo categoría esta vacío, favor de capturarlo.<br />"; }*/
        if (pProducto.Descripcion == "")
        { errores = errores + "<span>*</span> El campo descripción esta vacío, favor de capturarlo.<br />"; }
        /*if (pProducto.Costo == 0)
        { errores = errores + "<span>*</span> El campo costo esta vacío, favor de capturarlo.<br />"; }*/
        /*if (pProducto.MargenUtilidad == 0)
        { errores = errores + "<span>*</span> El campo margen de utilidad esta vacío, favor de capturarlo.<br />"; }*/
        if (pProducto.IdTipoVenta == 0)
        { errores = errores + "<span>*</span> El campo tipo de venta esta vacío, favor de capturarlo.<br />"; }
        if (pProducto.IdUnidadCompraVenta != 0)
        {
            /*if (pProducto.ValorMedida == 0)
            {
                CUnidadCompraVenta UnidadCompraVenta = new CUnidadCompraVenta();
                UnidadCompraVenta.LlenaObjeto(pProducto.IdUnidadCompraVenta, pConexion);
                errores = errores + "<span>*</span> El campo " + UnidadCompraVenta.UnidadCompraVenta.ToLower() + " esta vacío, favor de capturarlo.<br />";
            }*/
        }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarCaracteristicaProducto(CCaracteristicaProducto pCaracteristicaProducto, CConexion pConexion)
    {
        string errores = "";

        if (pCaracteristicaProducto.IdCaracteristica == 0)
        { errores = errores + "<span>*</span> El campo caracteristica del producto esta vacío, favor de capturarlo.<br />"; }
        if (pCaracteristicaProducto.Valor == "")
        { errores = errores + "<span>*</span> El campo valor esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

    private static string ValidarDescuentoProducto(CDescuentoProducto pDescuentoProducto, CConexion pConexion)
    {
        string errores = "";

        if (pDescuentoProducto.Descuento == 0)
        { errores = errores + "<span>*</span> El campo descuento esta vacío, favor de capturarlo.<br />"; }
        if (pDescuentoProducto.DescuentoProducto == "")
        { errores = errores + "<span>*</span> El campo descripción del descuento esta vacío, favor de capturarlo.<br />"; }

        Dictionary<string, object> Parametros = new Dictionary<string, object>();
        Parametros.Add("IdProducto", pDescuentoProducto.IdProducto);
        Parametros.Add("Descuento", pDescuentoProducto.Descuento);

        /*
        CDescuentoProducto ValidarDescuentoProducto = new CDescuentoProducto();
        ValidarDescuentoProducto.LlenaObjetoFiltros(Parametros, pConexion);
        if (ValidarDescuentoProducto.IdDescuentoProducto != 0)
        {
            errores = errores + "<span>*</span> El descuento ya existe para este producto, favor de verificar.<br />";
        }
        */
        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}