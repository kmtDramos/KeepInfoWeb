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


public partial class TipoCuentaContable : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoCuentaContable
        CJQGrid GridTipoCuentaContable = new CJQGrid();
        GridTipoCuentaContable.NombreTabla = "grdTipoCuentaContable";
        GridTipoCuentaContable.CampoIdentificador = "IdTipoCuentaContable";
        GridTipoCuentaContable.ColumnaOrdenacion = "TipoCuentaContable";
        GridTipoCuentaContable.Metodo = "ObtenerTipoCuentaContable";
        GridTipoCuentaContable.TituloTabla = "Catálogo de tipos de cuentas contables";

        //IdTipoCuentaContable
        CJQColumn ColIdTipoCuentaContable = new CJQColumn();
        ColIdTipoCuentaContable.Nombre = "IdTipoCuentaContable";
        ColIdTipoCuentaContable.Encabezado = "IdTipoCuentaContable";
        ColIdTipoCuentaContable.Buscador = "false";
        GridTipoCuentaContable.Columnas.Add(ColIdTipoCuentaContable);

        //TipoCuentaContable
        CJQColumn ColTipoCuentaContable = new CJQColumn();
        ColTipoCuentaContable.Nombre = "TipoCuentaContable";
        ColTipoCuentaContable.Encabezado = "Tipo de cuenta contable";
        ColTipoCuentaContable.Ancho = "400";
        ColTipoCuentaContable.Alineacion = "Left";
        GridTipoCuentaContable.Columnas.Add(ColTipoCuentaContable);

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
        GridTipoCuentaContable.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoCuentaContable";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoCuentaContable.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoCuentaContable", GridTipoCuentaContable.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoCuentaContable(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoCuentaContable, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoCuentaContable", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoCuentaContable", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoCuentaContable);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoCuentaContable(string pTipoCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoCuentaContable = new CJson();
        jsonTipoCuentaContable.StoredProcedure.CommandText = "sp_TipoCuentaContable_Consultar_FiltroPorTipoCuentaContable";
        jsonTipoCuentaContable.StoredProcedure.Parameters.AddWithValue("@pTipoCuentaContable", pTipoCuentaContable);
        return jsonTipoCuentaContable.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoCuentaContable(Dictionary<string, object> pTipoCuentaContable)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoCuentaContable TipoCuentaContable = new CTipoCuentaContable();
            TipoCuentaContable.TipoCuentaContable = Convert.ToString(pTipoCuentaContable["TipoCuentaContable"]);

            string validacion = ValidarTipoCuentaContable(TipoCuentaContable, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoCuentaContable.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoCuentaContable(int pIdTipoCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarTipoCuentaContable", puedeEditarTipoCuentaContable);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCuentaContable TipoCuentaContable = new CTipoCuentaContable();
            TipoCuentaContable.LlenaObjeto(pIdTipoCuentaContable, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoCuentaContable", TipoCuentaContable.IdTipoCuentaContable));
            Modelo.Add(new JProperty("TipoCuentaContable", TipoCuentaContable.TipoCuentaContable));

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
    public static string ObtenerFormaEditarTipoCuentaContable(int IdTipoCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCuentaContable = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCuentaContable" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCuentaContable = 1;
        }
        oPermisos.Add("puedeEditarTipoCuentaContable", puedeEditarTipoCuentaContable);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCuentaContable TipoCuentaContable = new CTipoCuentaContable();
            TipoCuentaContable.LlenaObjeto(IdTipoCuentaContable, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoCuentaContable", TipoCuentaContable.IdTipoCuentaContable));
            Modelo.Add(new JProperty("TipoCuentaContable", TipoCuentaContable.TipoCuentaContable));
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
    public static string EditarTipoCuentaContable(Dictionary<string, object> pTipoCuentaContable)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoCuentaContable TipoCuentaContable = new CTipoCuentaContable();
        TipoCuentaContable.IdTipoCuentaContable = Convert.ToInt32(pTipoCuentaContable["IdTipoCuentaContable"]); ;
        TipoCuentaContable.TipoCuentaContable = Convert.ToString(pTipoCuentaContable["TipoCuentaContable"]);

        string validacion = ValidarTipoCuentaContable(TipoCuentaContable, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoCuentaContable.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoCuentaContable, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCuentaContable TipoCuentaContable = new CTipoCuentaContable();
            TipoCuentaContable.IdTipoCuentaContable = pIdTipoCuentaContable;
            TipoCuentaContable.Baja = pBaja;
            TipoCuentaContable.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoCuentaContableEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoCuentaContable(CTipoCuentaContable pTipoCuentaContable, CConexion pConexion)
    {
        string errores = "";
        if (pTipoCuentaContable.TipoCuentaContable == "")
        { errores = errores + "<span>*</span> El campo tipo de cuenta contable esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}