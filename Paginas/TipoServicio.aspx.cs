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

public partial class TipoServicio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridTipoServicio
        CJQGrid GridTipoServicio = new CJQGrid();
        GridTipoServicio.NombreTabla = "grdTipoServicio";
        GridTipoServicio.CampoIdentificador = "IdTipoServicio";
        GridTipoServicio.ColumnaOrdenacion = "TipoServicio";
        GridTipoServicio.Metodo = "ObtenerTipoServicio";
        GridTipoServicio.TituloTabla = "Catálogo de Tipo de servicio";

        //IdTipoServicio
        CJQColumn ColIdTipoServicio = new CJQColumn();
        ColIdTipoServicio.Nombre = "IdTipoServicio";
        ColIdTipoServicio.Oculto = "true";
        ColIdTipoServicio.Encabezado = "IdTipoServicio";
        ColIdTipoServicio.Buscador = "false";
        GridTipoServicio.Columnas.Add(ColIdTipoServicio);

        //TipoServicio
        CJQColumn ColTipoServicio = new CJQColumn();
        ColTipoServicio.Nombre = "TipoServicio";
        ColTipoServicio.Encabezado = "Tipo de servicio";
        ColTipoServicio.Ancho = "400";
        ColTipoServicio.Alineacion = "left";
        GridTipoServicio.Columnas.Add(ColTipoServicio);

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
        GridTipoServicio.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoServicio";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoServicio.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoServicio", GridTipoServicio.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoServicio(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoServicio, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoServicio", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoServicio", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoServicio);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoServicio(string pTipoServicio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoServicio = new CJson();
        jsonTipoServicio.StoredProcedure.CommandText = "sp_TipoServicio_Consultar_FiltroPorTipoServicio";
        jsonTipoServicio.StoredProcedure.Parameters.AddWithValue("@pTipoServicio", pTipoServicio);
        return jsonTipoServicio.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoServicio(Dictionary<string, object> pTipoServicio)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoServicio TipoServicio = new CTipoServicio();
            TipoServicio.TipoServicio = Convert.ToString(pTipoServicio["TipoServicio"]);

            string validacion = ValidarTipoServicio(TipoServicio, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoServicio.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoServicio(int pIdTipoServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoServicio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoServicio" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoServicio = 1;
        }
        oPermisos.Add("puedeEditarTipoServicio", puedeEditarTipoServicio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoServicio TipoServicio = new CTipoServicio();
            TipoServicio.LlenaObjeto(pIdTipoServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoServicio", TipoServicio.IdTipoServicio));
            Modelo.Add(new JProperty("TipoServicio", TipoServicio.TipoServicio));

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
    public static string ObtenerFormaEditarTipoServicio(int IdTipoServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoServicio = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoServicio" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoServicio = 1;
        }
        oPermisos.Add("puedeEditarTipoServicio", puedeEditarTipoServicio);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoServicio TipoServicio = new CTipoServicio();
            TipoServicio.LlenaObjeto(IdTipoServicio, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoServicio", TipoServicio.IdTipoServicio));
            Modelo.Add(new JProperty("TipoServicio", TipoServicio.TipoServicio));
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
    public static string EditarTipoServicio(Dictionary<string, object> pTipoServicio)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoServicio TipoServicio = new CTipoServicio();
        TipoServicio.IdTipoServicio = Convert.ToInt32(pTipoServicio["IdTipoServicio"]); ;
        TipoServicio.TipoServicio = Convert.ToString(pTipoServicio["TipoServicio"]);

        string validacion = ValidarTipoServicio(TipoServicio, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoServicio.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoServicio, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoServicio TipoServicio = new CTipoServicio();
            TipoServicio.IdTipoServicio = pIdTipoServicio;
            TipoServicio.Baja = pBaja;
            TipoServicio.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoServicioEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoServicio(CTipoServicio pTipoServicio, CConexion pConexion)
    {
        string errores = "";
        if (pTipoServicio.TipoServicio == "")
        { errores = errores + "<span>*</span> El campo tipo de servicio esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}