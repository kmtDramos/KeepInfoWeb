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

public partial class TipoIndustria : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoIndustria
        CJQGrid GridTipoIndustria = new CJQGrid();
        GridTipoIndustria.NombreTabla = "grdTipoIndustria";
        GridTipoIndustria.CampoIdentificador = "IdTipoIndustria";
        GridTipoIndustria.ColumnaOrdenacion = "TipoIndustria";
        GridTipoIndustria.Metodo = "ObtenerTipoIndustria";
        GridTipoIndustria.TituloTabla = "Catálogo de tipos de industrias";

        //IdTipoIndustria
        CJQColumn ColIdTipoIndustria = new CJQColumn();
        ColIdTipoIndustria.Nombre = "IdTipoIndustria";
        ColIdTipoIndustria.Oculto = "true";
        ColIdTipoIndustria.Encabezado = "IdTipoIndustria";
        ColIdTipoIndustria.Buscador = "false";
        GridTipoIndustria.Columnas.Add(ColIdTipoIndustria);

        //TipoIndustria
        CJQColumn ColTipoIndustria = new CJQColumn();
        ColTipoIndustria.Nombre = "TipoIndustria";
        ColTipoIndustria.Encabezado = "Tipo de industria";
        ColTipoIndustria.Ancho = "400";
        ColTipoIndustria.Alineacion = "Left";
        GridTipoIndustria.Columnas.Add(ColTipoIndustria);

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
        GridTipoIndustria.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoIndustria";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoIndustria.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoIndustria", GridTipoIndustria.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTipoIndustria" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoIndustria(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoIndustria, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoIndustria", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoIndustria", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoIndustria);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoIndustria(string pTipoIndustria)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoIndustria = new CJson();
        jsonTipoIndustria.StoredProcedure.CommandText = "sp_TipoIndustria_Consultar_FiltroPorTipoIndustria";
        jsonTipoIndustria.StoredProcedure.Parameters.AddWithValue("@pTipoIndustria", pTipoIndustria);
        return jsonTipoIndustria.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoIndustria(Dictionary<string, object> pTipoIndustria)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoIndustria TipoIndustria = new CTipoIndustria();
            TipoIndustria.TipoIndustria = Convert.ToString(pTipoIndustria["TipoIndustria"]);

            string validacion = ValidarTipoIndustria(TipoIndustria, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoIndustria.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoIndustria(int pIdTipoIndustria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoIndustria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoIndustria" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoIndustria = 1;
        }
        oPermisos.Add("puedeEditarTipoIndustria", puedeEditarTipoIndustria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoIndustria TipoIndustria = new CTipoIndustria();
            TipoIndustria.LlenaObjeto(pIdTipoIndustria, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoIndustria", TipoIndustria.IdTipoIndustria));
            Modelo.Add(new JProperty("TipoIndustria", TipoIndustria.TipoIndustria));

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
    public static string ObtenerFormaEditarTipoIndustria(int IdTipoIndustria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoIndustria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoIndustria" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoIndustria = 1;
        }
        oPermisos.Add("puedeEditarTipoIndustria", puedeEditarTipoIndustria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoIndustria TipoIndustria = new CTipoIndustria();
            TipoIndustria.LlenaObjeto(IdTipoIndustria, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoIndustria", TipoIndustria.IdTipoIndustria));
            Modelo.Add(new JProperty("TipoIndustria", TipoIndustria.TipoIndustria));
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
    public static string EditarTipoIndustria(Dictionary<string, object> pTipoIndustria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoIndustria TipoIndustria = new CTipoIndustria();
        TipoIndustria.IdTipoIndustria = Convert.ToInt32(pTipoIndustria["IdTipoIndustria"]); ;
        TipoIndustria.TipoIndustria = Convert.ToString(pTipoIndustria["TipoIndustria"]);

        string validacion = ValidarTipoIndustria(TipoIndustria, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoIndustria.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoIndustria, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoIndustria TipoIndustria = new CTipoIndustria();
            TipoIndustria.IdTipoIndustria = pIdTipoIndustria;
            TipoIndustria.Baja = pBaja;
            TipoIndustria.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoIndustriaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoIndustria(CTipoIndustria pTipoIndustria, CConexion pConexion)
    {
        string errores = "";
        if (pTipoIndustria.TipoIndustria == "")
        { errores = errores + "<span>*</span> El campo TipoIndustria esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }

}