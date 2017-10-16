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

public partial class Campana : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridCampana
        CJQGrid GridCampana = new CJQGrid();
        GridCampana.NombreTabla = "grdCampana";
        GridCampana.CampoIdentificador = "IdCampana";
        GridCampana.ColumnaOrdenacion = "Campana";
        GridCampana.Metodo = "ObtenerCampana";
        GridCampana.TituloTabla = "Catálogo campañas";

        //IdCampana
        CJQColumn ColIdCampana = new CJQColumn();
        ColIdCampana.Nombre = "IdCampana";
        ColIdCampana.Oculto = "true";
        ColIdCampana.Encabezado = "IdCampana";
        ColIdCampana.Buscador = "false";
        GridCampana.Columnas.Add(ColIdCampana);

        //Campana
        CJQColumn ColCampana = new CJQColumn();
        ColCampana.Nombre = "Campana";
        ColIdCampana.Oculto = "true";
        ColCampana.Encabezado = "Campaña";
        ColCampana.Ancho = "400";
        ColCampana.Alineacion = "Left";
        GridCampana.Columnas.Add(ColCampana);


        //Usuario
        CJQColumn ColUsuario = new CJQColumn();
        ColUsuario.Nombre = "Usuario";
        ColIdCampana.Oculto = "true";
        ColUsuario.Encabezado = "Usuario";
        ColUsuario.Ancho = "200";
        ColUsuario.Alineacion = "Left";
        GridCampana.Columnas.Add(ColUsuario);

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
        GridCampana.Columnas.Add(ColBaja);

        //Consultar
        /*CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCampana";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCampana.Columnas.Add(ColConsultar);*/

        ClientScript.RegisterStartupScript(this.GetType(), "grdCampana", GridCampana.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCampana(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCampana, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCampana", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCampana", SqlDbType.VarChar, 250).Value = Convert.ToString(pCampana);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCampana(string pCampana)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonCampana = new CJson();
        jsonCampana.StoredProcedure.CommandText = "sp_Campana_Consultar_FiltroPorCampana";
        jsonCampana.StoredProcedure.Parameters.AddWithValue("@pCampana", pCampana);
        string jsonCampanaString = jsonCampana.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCampanaString;
    }

    [WebMethod]
    public static string AgregarCampana(Dictionary<string, object> pCampana)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CCampana Campana = new CCampana();
            Campana.Campana = Convert.ToString(pCampana["Campana"]);
            Campana.IdUsuario = Convert.ToInt32(Usuario.IdUsuario);
            Campana.FechaAlta = Convert.ToDateTime(DateTime.Now);

            string validacion = ValidarCampana(Campana, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Campana.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaCampana(int pIdCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCampana = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCampana" }, ConexionBaseDatos) == "")
        {
            puedeEditarCampana = 1;
        }
        oPermisos.Add("puedeEditarCampana", puedeEditarCampana);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCampana Campana = new CCampana();
            Campana.LlenaObjeto(pIdCampana, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCampana", Campana.IdCampana));
            Modelo.Add(new JProperty("Campana", Campana.Campana));

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
    public static string ObtenerFormaEditarCampana(int IdCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCampana = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCampana" }, ConexionBaseDatos) == "")
        {
            puedeEditarCampana = 1;
        }
        oPermisos.Add("puedeEditarCampana", puedeEditarCampana);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCampana Campana = new CCampana();
            Campana.LlenaObjeto(IdCampana, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCampana", Campana.IdCampana));
            Modelo.Add(new JProperty("Campana", Campana.Campana));
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
    public static string EditarCampana(Dictionary<string, object> pCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCampana Campana = new CCampana();
        Campana.IdCampana = Convert.ToInt32(pCampana["IdCampana"]); ;
        Campana.Campana = Convert.ToString(pCampana["Campana"]);

        string validacion = ValidarCampana(Campana, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Campana.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCampana, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCampana Campana = new CCampana();
            Campana.IdCampana = pIdCampana;
            Campana.Baja = pBaja;
            Campana.Eliminar(ConexionBaseDatos);
            respuesta = "0|CampanaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarCampana(CCampana pCampana, CConexion pConexion)
    {
        string errores = "";
        if (pCampana.Campana == "")
        { errores = errores + "<span>*</span> El campo Campana esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}