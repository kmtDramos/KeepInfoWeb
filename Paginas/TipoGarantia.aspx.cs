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

public partial class TipoGarantia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoGarantia
        CJQGrid GridTipoGarantia = new CJQGrid();
        GridTipoGarantia.NombreTabla = "grdTipoGarantia";
        GridTipoGarantia.CampoIdentificador = "IdTipoGarantia";
        GridTipoGarantia.ColumnaOrdenacion = "TipoGarantia";
        GridTipoGarantia.Metodo = "ObtenerTipoGarantia";
        GridTipoGarantia.TituloTabla = "Catálogo de tipos de garantías";

        //IdTipoGarantia
        CJQColumn ColIdTipoGarantia = new CJQColumn();
        ColIdTipoGarantia.Nombre = "IdTipoGarantia";
        ColIdTipoGarantia.Oculto = "true";
        ColIdTipoGarantia.Encabezado = "IdTipoGarantia";
        ColIdTipoGarantia.Buscador = "false";
        GridTipoGarantia.Columnas.Add(ColIdTipoGarantia);

        //TipoGarantia
        CJQColumn ColTipoGarantia = new CJQColumn();
        ColTipoGarantia.Nombre = "TipoGarantia";
        ColTipoGarantia.Encabezado = "Tipo de garantía";
        ColTipoGarantia.Ancho = "400";
        ColTipoGarantia.Alineacion = "Left";
        GridTipoGarantia.Columnas.Add(ColTipoGarantia);

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
        GridTipoGarantia.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoGarantia";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoGarantia.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoGarantia", GridTipoGarantia.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoGarantia(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoGarantia, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoGarantia", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoGarantia", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoGarantia);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoGarantia(string pTipoGarantia)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoGarantia = new CJson();
        jsonTipoGarantia.StoredProcedure.CommandText = "sp_TipoGarantia_Consultar_FiltroPorTipoGarantia";
        jsonTipoGarantia.StoredProcedure.Parameters.AddWithValue("@pTipoGarantia", pTipoGarantia);
        return jsonTipoGarantia.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoGarantia(Dictionary<string, object> pTipoGarantia)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoGarantia TipoGarantia = new CTipoGarantia();
            TipoGarantia.TipoGarantia = Convert.ToString(pTipoGarantia["TipoGarantia"]);

            string validacion = ValidarTipoGarantia(TipoGarantia, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoGarantia.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoGarantia(int pIdTipoGarantia)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoGarantia = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoGarantia" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoGarantia = 1;
        }
        oPermisos.Add("puedeEditarTipoGarantia", puedeEditarTipoGarantia);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoGarantia TipoGarantia = new CTipoGarantia();
            TipoGarantia.LlenaObjeto(pIdTipoGarantia, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoGarantia", TipoGarantia.IdTipoGarantia));
            Modelo.Add(new JProperty("TipoGarantia", TipoGarantia.TipoGarantia));

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
    public static string ObtenerFormaEditarTipoGarantia(int IdTipoGarantia)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoGarantia = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoGarantia" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoGarantia = 1;
        }
        oPermisos.Add("puedeEditarTipoGarantia", puedeEditarTipoGarantia);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoGarantia TipoGarantia = new CTipoGarantia();
            TipoGarantia.LlenaObjeto(IdTipoGarantia, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoGarantia", TipoGarantia.IdTipoGarantia));
            Modelo.Add(new JProperty("TipoGarantia", TipoGarantia.TipoGarantia));
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
    public static string EditarTipoGarantia(Dictionary<string, object> pTipoGarantia)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoGarantia TipoGarantia = new CTipoGarantia();
        TipoGarantia.IdTipoGarantia = Convert.ToInt32(pTipoGarantia["IdTipoGarantia"]); ;
        TipoGarantia.TipoGarantia = Convert.ToString(pTipoGarantia["TipoGarantia"]);

        string validacion = ValidarTipoGarantia(TipoGarantia, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoGarantia.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoGarantia, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoGarantia TipoGarantia = new CTipoGarantia();
            TipoGarantia.IdTipoGarantia = pIdTipoGarantia;
            TipoGarantia.Baja = pBaja;
            TipoGarantia.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoGarantiaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoGarantia(CTipoGarantia pTipoGarantia, CConexion pConexion)
    {
        string errores = "";
        if (pTipoGarantia.TipoGarantia == "")
        { errores = errores + "<span>*</span> El campo tipo de garantía esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}