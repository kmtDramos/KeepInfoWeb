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

public partial class Categoria : System.Web.UI.Page
{
    public static int puedeEditarCategoria = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        if (Usuario.TienePermisos(new string[] { "puedeEditarCategoria" }, ConexionBaseDatos) == "") { puedeEditarCategoria = 1; }
        else { puedeEditarCategoria = 0; }

        //GridCategoria
        CJQGrid GridCategoria = new CJQGrid();
        GridCategoria.NombreTabla = "grdCategoria";
        GridCategoria.CampoIdentificador = "IdCategoria";
        GridCategoria.ColumnaOrdenacion = "Categoria";
        GridCategoria.Metodo = "ObtenerCategoria";
        GridCategoria.TituloTabla = "Catálogo de categorías";

        //IdCategoria
        CJQColumn ColIdCategoria = new CJQColumn();
        ColIdCategoria.Nombre = "IdCategoria";
        ColIdCategoria.Oculto = "true";
        ColIdCategoria.Encabezado = "IdCategoria";
        ColIdCategoria.Buscador = "false";
        GridCategoria.Columnas.Add(ColIdCategoria);

        //Categoria
        CJQColumn ColCategoria = new CJQColumn();
        ColCategoria.Nombre = "Categoria";
        ColCategoria.Encabezado = "Categoría";
        ColCategoria.Ancho = "400";
        ColCategoria.Alineacion = "Left";
        GridCategoria.Columnas.Add(ColCategoria);

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
        GridCategoria.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCategoria";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCategoria.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdCategoria", GridCategoria.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarCategoria" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCategoria(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCategoria, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCategoria", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCategoria", SqlDbType.VarChar, 250).Value = Convert.ToString(pCategoria);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCategoria(string pCategoria)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonCategoria = new CJson();
        jsonCategoria.StoredProcedure.CommandText = "sp_Categoria_Consultar_FiltroPorCategoria";
        jsonCategoria.StoredProcedure.Parameters.AddWithValue("@pCategoria", pCategoria);
        string jsonCategoriaString = jsonCategoria.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCategoriaString;
    }

    [WebMethod]
    public static string ObtenerFormaAgregarCategoria()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Grupos", CGrupo.ObtenerJsonGrupos(ConexionBaseDatos));
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
    public static string AgregarCategoria(Dictionary<string, object> pCategoria)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CCategoria Categoria = new CCategoria();
            Categoria.Categoria = Convert.ToString(pCategoria["Categoria"]);
            Categoria.IdGrupo = Convert.ToInt32(pCategoria["IdGrupo"]);

            string validacion = ValidarCategoria(Categoria, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Categoria.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaCategoria(int pIdCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCategoria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCategoria" }, ConexionBaseDatos) == "")
        {
            puedeEditarCategoria = 1;
        }
        oPermisos.Add("puedeEditarCategoria", puedeEditarCategoria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCategoria Categoria = new CCategoria();
            Categoria.LlenaObjeto(pIdCategoria, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCategoria", Categoria.IdCategoria));
            Modelo.Add(new JProperty("Categoria", Categoria.Categoria));

            CGrupo Grupo = new CGrupo();
            Grupo.LlenaObjeto(Categoria.IdGrupo, ConexionBaseDatos);
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
    public static string ObtenerFormaEditarCategoria(int IdCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCategoria = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCategoria" }, ConexionBaseDatos) == "")
        {
            puedeEditarCategoria = 1;
        }
        oPermisos.Add("puedeEditarCategoria", puedeEditarCategoria);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCategoria Categoria = new CCategoria();
            Categoria.LlenaObjeto(IdCategoria, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCategoria", Categoria.IdCategoria));
            Modelo.Add(new JProperty("Categoria", Categoria.Categoria));
            Modelo.Add("Grupos", CGrupo.ObtenerJsonGrupos(Categoria.IdGrupo, ConexionBaseDatos));
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
    public static string EditarCategoria(Dictionary<string, object> pCategoria)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCategoria Categoria = new CCategoria();
        Categoria.IdCategoria = Convert.ToInt32(pCategoria["IdCategoria"]); ;
        Categoria.Categoria = Convert.ToString(pCategoria["Categoria"]);
        Categoria.IdGrupo = Convert.ToInt32(pCategoria["IdGrupo"]);

        string validacion = ValidarCategoria(Categoria, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Categoria.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCategoria, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCategoria Categoria = new CCategoria();
            Categoria.IdCategoria = pIdCategoria;
            Categoria.Baja = pBaja;
            Categoria.Eliminar(ConexionBaseDatos);
            respuesta = "0|CategoriaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarCategoria(CCategoria pCategoria, CConexion pConexion)
    {
        string errores = "";
        if (pCategoria.Categoria == "")
        { errores = errores + "<span>*</span> El campo categoría esta vacío, favor de capturarlo.<br />"; }

        if (pCategoria.IdGrupo == 0)
        { errores = errores + "<span>*</span> El campo grupo esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}