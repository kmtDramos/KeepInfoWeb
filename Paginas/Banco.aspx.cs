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

public partial class Banco : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridBanco
        CJQGrid GridBanco = new CJQGrid();
        GridBanco.NombreTabla = "grdBanco";
        GridBanco.CampoIdentificador = "IdBanco";
        GridBanco.ColumnaOrdenacion = "Banco";
        GridBanco.Metodo = "ObtenerBanco";
        GridBanco.TituloTabla = "Catálogo de bancos";

        //IdBanco
        CJQColumn ColIdBanco = new CJQColumn();
        ColIdBanco.Nombre = "IdBanco";
        ColIdBanco.Oculto = "true";
        ColIdBanco.Encabezado = "IdBanco";
        ColIdBanco.Buscador = "false";
        GridBanco.Columnas.Add(ColIdBanco);

        //Banco
        CJQColumn ColBanco = new CJQColumn();
        ColBanco.Nombre = "Banco";
        ColBanco.Encabezado = "Banco";
        ColBanco.Ancho = "400";
        ColBanco.Alineacion = "Left";
        GridBanco.Columnas.Add(ColBanco);

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
        GridBanco.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarBanco";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridBanco.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdBanco", GridBanco.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarBanco" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerBanco(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pBanco, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdBanco", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pBanco", SqlDbType.VarChar, 250).Value = Convert.ToString(pBanco);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarBanco(string pBanco)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonBanco = new CJson();
        jsonBanco.StoredProcedure.CommandText = "sp_Banco_Consultar_FiltroPorBanco";
        jsonBanco.StoredProcedure.Parameters.AddWithValue("@pBanco", pBanco);
        string jsonBancoString = jsonBanco.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonBancoString;
    }

    [WebMethod]
    public static string AgregarBanco(Dictionary<string, object> pBanco)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CBanco Banco = new CBanco();
            Banco.Banco = Convert.ToString(pBanco["Banco"]);

            string validacion = ValidarBanco(Banco, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                Banco.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaBanco(int pIdBanco)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarBanco = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarBanco" }, ConexionBaseDatos) == "")
        {
            puedeEditarBanco = 1;
        }
        oPermisos.Add("puedeEditarBanco", puedeEditarBanco);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CBanco Banco = new CBanco();
            Banco.LlenaObjeto(pIdBanco, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdBanco", Banco.IdBanco));
            Modelo.Add(new JProperty("Banco", Banco.Banco));

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
    public static string ObtenerFormaEditarBanco(int IdBanco)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarBanco = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarBanco" }, ConexionBaseDatos) == "")
        {
            puedeEditarBanco = 1;
        }
        oPermisos.Add("puedeEditarBanco", puedeEditarBanco);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CBanco Banco = new CBanco();
            Banco.LlenaObjeto(IdBanco, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdBanco", Banco.IdBanco));
            Modelo.Add(new JProperty("Banco", Banco.Banco));
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
    public static string EditarBanco(Dictionary<string, object> pBanco)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CBanco Banco = new CBanco();
        Banco.IdBanco = Convert.ToInt32(pBanco["IdBanco"]); ;
        Banco.Banco = Convert.ToString(pBanco["Banco"]);

        string validacion = ValidarBanco(Banco, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            Banco.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdBanco, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CBanco Banco = new CBanco();
            Banco.IdBanco = pIdBanco;
            Banco.Baja = pBaja;
            Banco.Eliminar(ConexionBaseDatos);
            respuesta = "0|BancoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarBanco(CBanco pBanco, CConexion pConexion)
    {
        string errores = "";
        if (pBanco.Banco == "")
        { errores = errores + "<span>*</span> El campo banco esta vacio, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}