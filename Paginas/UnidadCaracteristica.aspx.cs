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

public partial class UnidadCaracteristica : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridUnidadCaracteristica
        CJQGrid GridUnidadCaracteristica = new CJQGrid();
        GridUnidadCaracteristica.NombreTabla = "grdUnidadCaracteristica";
        GridUnidadCaracteristica.CampoIdentificador = "IdUnidadCaracteristica";
        GridUnidadCaracteristica.ColumnaOrdenacion = "UnidadCaracteristica";
        GridUnidadCaracteristica.Metodo = "ObtenerUnidadCaracteristica";
        GridUnidadCaracteristica.TituloTabla = "Catálogo de unidad de características";

        //IdUnidadCaracteristica
        CJQColumn ColIdUnidadCaracteristica = new CJQColumn();
        ColIdUnidadCaracteristica.Nombre = "IdUnidadCaracteristica";
        ColIdUnidadCaracteristica.Oculto = "true";
        ColIdUnidadCaracteristica.Encabezado = "IdUnidadCaracteristica";
        ColIdUnidadCaracteristica.Buscador = "false";
        GridUnidadCaracteristica.Columnas.Add(ColIdUnidadCaracteristica);

        //UnidadCaracteristica
        CJQColumn ColUnidadCaracteristica = new CJQColumn();
        ColUnidadCaracteristica.Nombre = "UnidadCaracteristica";
        ColUnidadCaracteristica.Encabezado = "Unidad de característica";
        ColUnidadCaracteristica.Ancho = "400";
        ColUnidadCaracteristica.Alineacion = "Left";
        GridUnidadCaracteristica.Columnas.Add(ColUnidadCaracteristica);

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
        GridUnidadCaracteristica.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarUnidadCaracteristica";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridUnidadCaracteristica.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdUnidadCaracteristica", GridUnidadCaracteristica.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarUnidadCaracteristica" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerUnidadCaracteristica(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pUnidadCaracteristica, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdUnidadCaracteristica", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pUnidadCaracteristica", SqlDbType.VarChar, 250).Value = Convert.ToString(pUnidadCaracteristica);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarUnidadCaracteristica(string pUnidadCaracteristica)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonUnidadCaracteristica = new CJson();
        jsonUnidadCaracteristica.StoredProcedure.CommandText = "sp_UnidadCaracteristica_Consultar_FiltroPorUnidadCaracteristica";
        jsonUnidadCaracteristica.StoredProcedure.Parameters.AddWithValue("@pUnidadCaracteristica", pUnidadCaracteristica);
        return jsonUnidadCaracteristica.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarUnidadCaracteristica(Dictionary<string, object> pUnidadCaracteristica)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
            UnidadCaracteristica.UnidadCaracteristica = Convert.ToString(pUnidadCaracteristica["UnidadCaracteristica"]);

            string validacion = ValidarUnidadCaracteristica(UnidadCaracteristica, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                UnidadCaracteristica.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaUnidadCaracteristica(int pIdUnidadCaracteristica)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarUnidadCaracteristica = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarUnidadCaracteristica" }, ConexionBaseDatos) == "")
        {
            puedeEditarUnidadCaracteristica = 1;
        }
        oPermisos.Add("puedeEditarUnidadCaracteristica", puedeEditarUnidadCaracteristica);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
            UnidadCaracteristica.LlenaObjeto(pIdUnidadCaracteristica, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdUnidadCaracteristica", UnidadCaracteristica.IdUnidadCaracteristica));
            Modelo.Add(new JProperty("UnidadCaracteristica", UnidadCaracteristica.UnidadCaracteristica));

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
    public static string ObtenerFormaEditarUnidadCaracteristica(int IdUnidadCaracteristica)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarUnidadCaracteristica = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarUnidadCaracteristica" }, ConexionBaseDatos) == "")
        {
            puedeEditarUnidadCaracteristica = 1;
        }
        oPermisos.Add("puedeEditarUnidadCaracteristica", puedeEditarUnidadCaracteristica);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
            UnidadCaracteristica.LlenaObjeto(IdUnidadCaracteristica, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdUnidadCaracteristica", UnidadCaracteristica.IdUnidadCaracteristica));
            Modelo.Add(new JProperty("UnidadCaracteristica", UnidadCaracteristica.UnidadCaracteristica));
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
    public static string EditarUnidadCaracteristica(Dictionary<string, object> pUnidadCaracteristica)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
        UnidadCaracteristica.IdUnidadCaracteristica = Convert.ToInt32(pUnidadCaracteristica["IdUnidadCaracteristica"]); ;
        UnidadCaracteristica.UnidadCaracteristica = Convert.ToString(pUnidadCaracteristica["UnidadCaracteristica"]);

        string validacion = ValidarUnidadCaracteristica(UnidadCaracteristica, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            UnidadCaracteristica.Editar(ConexionBaseDatos);
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

    [WebMethod]
    public static string CambiarEstatus(int pIdUnidadCaracteristica, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUnidadCaracteristica UnidadCaracteristica = new CUnidadCaracteristica();
            UnidadCaracteristica.IdUnidadCaracteristica = pIdUnidadCaracteristica;
            UnidadCaracteristica.Baja = pBaja;
            UnidadCaracteristica.Eliminar(ConexionBaseDatos);
            respuesta = "0|UnidadCaracteristicaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarUnidadCaracteristica(CUnidadCaracteristica pUnidadCaracteristica, CConexion pConexion)
    {
        string errores = "";
        if (pUnidadCaracteristica.UnidadCaracteristica == "")
        { errores = errores + "<span>*</span> El campo unidad de característica esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}