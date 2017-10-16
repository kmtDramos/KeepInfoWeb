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

public partial class Grupo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridGrupo
        CJQGrid GridGrupo = new CJQGrid();
        GridGrupo.NombreTabla = "grdGrupo";
        GridGrupo.CampoIdentificador = "IdGrupo";
        GridGrupo.ColumnaOrdenacion = "Grupo";
        GridGrupo.Metodo = "ObtenerGrupo";
        GridGrupo.TituloTabla = "Catálogo de grupos";

        //IdGrupo
        CJQColumn ColIdGrupo = new CJQColumn();
        ColIdGrupo.Nombre = "IdGrupo";
        ColIdGrupo.Oculto = "true";
        ColIdGrupo.Encabezado = "IdGrupo";
        ColIdGrupo.Buscador = "false";
        GridGrupo.Columnas.Add(ColIdGrupo);

        //Grupo
        CJQColumn ColGrupo = new CJQColumn();
        ColGrupo.Nombre = "Grupo";
        ColGrupo.Encabezado = "Grupo";
        ColGrupo.Ancho = "400";
        ColGrupo.Alineacion = "Left";
        GridGrupo.Columnas.Add(ColGrupo);

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
        GridGrupo.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarGrupo";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridGrupo.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdGrupo", GridGrupo.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarGrupo" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerGrupo(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pGrupo, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdGrupo", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pGrupo", SqlDbType.VarChar, 250).Value = Convert.ToString(pGrupo);
        Stored.Parameters.Add("pIdEmpresa", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarGrupo(string pGrupo)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonGrupo = new CJson();
        jsonGrupo.StoredProcedure.CommandText = "sp_Grupo_Consultar_FiltroPorGrupo";
        jsonGrupo.StoredProcedure.Parameters.AddWithValue("@pGrupo", pGrupo);
        string jsonGrupoString = jsonGrupo.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonGrupoString;
    }

    [WebMethod]
    public static string AgregarGrupo(Dictionary<string, object> pGrupo)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CGrupo Grupo = new CGrupo();
            Grupo.Grupo = Convert.ToString(pGrupo["Grupo"]);
            Grupo.IdEmpresa = Convert.ToInt32(HttpContext.Current.Session["IdEmpresa"]);

            string validacion = ValidarGrupo(Grupo, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Grupo.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaGrupo(int pIdGrupo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarGrupo = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarGrupo" }, ConexionBaseDatos) == "")
        {
            puedeEditarGrupo = 1;
        }
        oPermisos.Add("puedeEditarGrupo", puedeEditarGrupo);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CGrupo Grupo = new CGrupo();
            Grupo.LlenaObjeto(pIdGrupo, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdGrupo", Grupo.IdGrupo));
            Modelo.Add(new JProperty("Grupo", Grupo.Grupo));

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
    public static string ObtenerFormaEditarGrupo(int IdGrupo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarGrupo = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarGrupo" }, ConexionBaseDatos) == "")
        {
            puedeEditarGrupo = 1;
        }
        oPermisos.Add("puedeEditarGrupo", puedeEditarGrupo);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CGrupo Grupo = new CGrupo();
            Grupo.LlenaObjeto(IdGrupo, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdGrupo", Grupo.IdGrupo));
            Modelo.Add(new JProperty("Grupo", Grupo.Grupo));
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
    public static string EditarGrupo(Dictionary<string, object> pGrupo)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CGrupo Grupo = new CGrupo();
        Grupo.IdGrupo = Convert.ToInt32(pGrupo["IdGrupo"]); ;
        Grupo.Grupo = Convert.ToString(pGrupo["Grupo"]);

        string validacion = ValidarGrupo(Grupo, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Grupo.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdGrupo, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CGrupo Grupo = new CGrupo();
            Grupo.IdGrupo = pIdGrupo;
            Grupo.Baja = pBaja;
            Grupo.Eliminar(ConexionBaseDatos);
            respuesta = "0|GrupoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarGrupo(CGrupo pGrupo, CConexion pConexion)
    {
        string errores = "";
        if (pGrupo.Grupo == "")
        { errores = errores + "<span>*</span> El campo grupo esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}