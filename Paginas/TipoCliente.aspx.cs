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

public partial class TipoCliente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridTipoCliente
        CJQGrid GridTipoCliente = new CJQGrid();
        GridTipoCliente.NombreTabla = "grdTipoCliente";
        GridTipoCliente.CampoIdentificador = "IdTipoCliente";
        GridTipoCliente.ColumnaOrdenacion = "TipoCliente";
        GridTipoCliente.Metodo = "ObtenerTipoCliente";
        GridTipoCliente.TituloTabla = "Catálogo de tipos de clientes";

        //IdTipoCliente
        CJQColumn ColIdTipoCliente = new CJQColumn();
        ColIdTipoCliente.Nombre = "IdTipoCliente";
        ColIdTipoCliente.Oculto = "true";
        ColIdTipoCliente.Encabezado = "IdTipoCliente";
        ColIdTipoCliente.Buscador = "false";
        GridTipoCliente.Columnas.Add(ColIdTipoCliente);

        //TipoCliente
        CJQColumn ColTipoCliente = new CJQColumn();
        ColTipoCliente.Nombre = "TipoCliente";
        ColTipoCliente.Encabezado = "Tipo de cliente";
        ColTipoCliente.Ancho = "400";
        ColTipoCliente.Alineacion = "Left";
        GridTipoCliente.Columnas.Add(ColTipoCliente);

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
        GridTipoCliente.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoCliente";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoCliente.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoCliente", GridTipoCliente.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTipoCliente" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoCliente(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoCliente, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoCliente", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoCliente", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoCliente);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();

        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoCliente(string pTipoCliente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoCliente = new CJson();
        jsonTipoCliente.StoredProcedure.CommandText = "sp_TipoCliente_Consultar_FiltroPorTipoCliente";
        jsonTipoCliente.StoredProcedure.Parameters.AddWithValue("@pTipoCliente", pTipoCliente);
        return jsonTipoCliente.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoCliente(Dictionary<string, object> pTipoCliente)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoCliente TipoCliente = new CTipoCliente();
            TipoCliente.TipoCliente = Convert.ToString(pTipoCliente["TipoCliente"]);

            string validacion = ValidarTipoCliente(TipoCliente, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoCliente.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoCliente(int pIdTipoCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCliente = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCliente" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCliente = 1;
        }
        oPermisos.Add("puedeEditarTipoCliente", puedeEditarTipoCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCliente TipoCliente = new CTipoCliente();
            TipoCliente.LlenaObjeto(pIdTipoCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoCliente", TipoCliente.IdTipoCliente));
            Modelo.Add(new JProperty("TipoCliente", TipoCliente.TipoCliente));

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
    public static string ObtenerFormaEditarTipoCliente(int IdTipoCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoCliente = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoCliente" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoCliente = 1;
        }
        oPermisos.Add("puedeEditarTipoCliente", puedeEditarTipoCliente);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoCliente TipoCliente = new CTipoCliente();
            TipoCliente.LlenaObjeto(IdTipoCliente, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoCliente", TipoCliente.IdTipoCliente));
            Modelo.Add(new JProperty("TipoCliente", TipoCliente.TipoCliente));
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
    public static string EditarTipoCliente(Dictionary<string, object> pTipoCliente)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoCliente TipoCliente = new CTipoCliente();
        TipoCliente.IdTipoCliente = Convert.ToInt32(pTipoCliente["IdTipoCliente"]); ;
        TipoCliente.TipoCliente = Convert.ToString(pTipoCliente["TipoCliente"]);

        string validacion = ValidarTipoCliente(TipoCliente, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoCliente.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoCliente, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoCliente TipoCliente = new CTipoCliente();
            TipoCliente.IdTipoCliente = pIdTipoCliente;
            TipoCliente.Baja = pBaja;
            TipoCliente.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoClienteEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoCliente(CTipoCliente pTipoCliente, CConexion pConexion)
    {
        string errores = "";
        if (pTipoCliente.TipoCliente == "")
        { errores = errores + "<span>*</span> El campo TipoCliente esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}