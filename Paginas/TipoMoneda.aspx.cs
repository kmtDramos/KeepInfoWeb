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

public partial class TipoMoneda : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoMoneda
        CJQGrid GridTipoMoneda = new CJQGrid();
        GridTipoMoneda.NombreTabla = "grdTipoMoneda";
        GridTipoMoneda.CampoIdentificador = "IdTipoMoneda";
        GridTipoMoneda.ColumnaOrdenacion = "TipoMoneda";
        GridTipoMoneda.Metodo = "ObtenerTipoMoneda";
        GridTipoMoneda.TituloTabla = "Catálogo de tipos de moneda";

        //IdTipoMoneda
        CJQColumn ColIdTipoMoneda = new CJQColumn();
        ColIdTipoMoneda.Nombre = "IdTipoMoneda";
        ColIdTipoMoneda.Oculto = "true";
        ColIdTipoMoneda.Encabezado = "IdTipoMoneda";
        ColIdTipoMoneda.Buscador = "false";
        GridTipoMoneda.Columnas.Add(ColIdTipoMoneda);

        //TipoMoneda
        CJQColumn ColTipoMoneda = new CJQColumn();
        ColTipoMoneda.Nombre = "TipoMoneda";
        ColTipoMoneda.Encabezado = "Tipo de moneda";
        ColTipoMoneda.Ancho = "400";
        ColTipoMoneda.Alineacion = "Left";
        GridTipoMoneda.Columnas.Add(ColTipoMoneda);

        //Simbolo
        CJQColumn ColSimbolo = new CJQColumn();
        ColSimbolo.Nombre = "Simbolo";
        ColSimbolo.Encabezado = "Símbolo";
        ColSimbolo.Ancho = "50";
        ColSimbolo.Buscador = "false";
        ColSimbolo.Alineacion = "Center";
        GridTipoMoneda.Columnas.Add(ColSimbolo);

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
        GridTipoMoneda.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoMoneda";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoMoneda.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoMoneda", GridTipoMoneda.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTipoMoneda" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoMoneda(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoMoneda, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoMoneda", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoMoneda", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoMoneda);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoMoneda(string pTipoMoneda)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoMoneda = new CJson();
        jsonTipoMoneda.StoredProcedure.CommandText = "sp_TipoMoneda_Consultar_FiltroPorTipoMoneda";
        jsonTipoMoneda.StoredProcedure.Parameters.AddWithValue("@pTipoMoneda", pTipoMoneda);
        return jsonTipoMoneda.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoMoneda(Dictionary<string, object> pTipoMoneda)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.TipoMoneda = Convert.ToString(pTipoMoneda["TipoMoneda"]);
            TipoMoneda.Simbolo = Convert.ToString(pTipoMoneda["Simbolo"]);

            string validacion = ValidarTipoMoneda(TipoMoneda, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoMoneda.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoMoneda(int pIdTipoMoneda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoMoneda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoMoneda" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoMoneda = 1;
        }
        oPermisos.Add("puedeEditarTipoMoneda", puedeEditarTipoMoneda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(pIdTipoMoneda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
            Modelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));
            Modelo.Add(new JProperty("Simbolo", TipoMoneda.Simbolo));

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
    public static string ObtenerFormaEditarTipoMoneda(int IdTipoMoneda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoMoneda = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoMoneda" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoMoneda = 1;
        }
        oPermisos.Add("puedeEditarTipoMoneda", puedeEditarTipoMoneda);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.LlenaObjeto(IdTipoMoneda, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoMoneda", TipoMoneda.IdTipoMoneda));
            Modelo.Add(new JProperty("TipoMoneda", TipoMoneda.TipoMoneda));
            Modelo.Add(new JProperty("Simbolo", TipoMoneda.Simbolo));
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
    public static string EditarTipoMoneda(Dictionary<string, object> pTipoMoneda)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoMoneda TipoMoneda = new CTipoMoneda();
        TipoMoneda.IdTipoMoneda = Convert.ToInt32(pTipoMoneda["IdTipoMoneda"]); ;
        TipoMoneda.TipoMoneda = Convert.ToString(pTipoMoneda["TipoMoneda"]);
        TipoMoneda.Simbolo = Convert.ToString(pTipoMoneda["Simbolo"]);

        string validacion = ValidarTipoMoneda(TipoMoneda, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoMoneda.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoMoneda, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoMoneda TipoMoneda = new CTipoMoneda();
            TipoMoneda.IdTipoMoneda = pIdTipoMoneda;
            TipoMoneda.Baja = pBaja;
            TipoMoneda.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoMonedaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoMoneda(CTipoMoneda pTipoMoneda, CConexion pConexion)
    {
        string errores = "";
        if (pTipoMoneda.TipoMoneda == "")
        { errores = errores + "<span>*</span> El campo tipo de moneda esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}