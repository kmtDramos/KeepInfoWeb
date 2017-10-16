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

public partial class TipoDocumento : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    public static int puedeAgregarTipoDocumento = 0;
    public static int puedeEditarTipoDocumento = 0;
    public static int puedeEliminarTipoDocumento = 0;
    public static int puedeConsultarTipoDocumento = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        CSucursal Sucursal = new CSucursal();
        Sucursal.LlenaObjeto(Usuario.IdSucursalActual, ConexionBaseDatos);

        CEmpresa Empresa = new CEmpresa();
        Empresa.LlenaObjeto(Sucursal.IdEmpresa, ConexionBaseDatos);

        idUsuario = Usuario.IdUsuario;
        idSucursal = Sucursal.IdSucursal;

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTipoDocumento" }, ConexionBaseDatos) == "") { puedeAgregarTipoDocumento = 1; }
        else { puedeAgregarTipoDocumento = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoDocumento" }, ConexionBaseDatos) == "") { puedeEditarTipoDocumento = 1; }
        else { puedeEditarTipoDocumento = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEliminarTipoDocumento" }, ConexionBaseDatos) == "") { puedeEliminarTipoDocumento = 1; }
        else { puedeEliminarTipoDocumento = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarTipoDocumento" }, ConexionBaseDatos) == "") { puedeConsultarTipoDocumento = 1; }
        else { puedeConsultarTipoDocumento = 0; }

        //GridTipoDocumento
        CJQGrid GridTipoDocumento = new CJQGrid();
        GridTipoDocumento.NombreTabla = "grdTipoDocumento";
        GridTipoDocumento.CampoIdentificador = "IdTipoDocumento";
        GridTipoDocumento.ColumnaOrdenacion = "TipoDocumento";
        GridTipoDocumento.Metodo = "ObtenerTipoDocumento";
        GridTipoDocumento.TituloTabla = "Catálogo de tipo de documento";

        //IdTipoDocumento
        CJQColumn ColIdTipoDocumento = new CJQColumn();
        ColIdTipoDocumento.Nombre = "IdTipoDocumento";
        ColIdTipoDocumento.Oculto = "true";
        ColIdTipoDocumento.Encabezado = "IdTipoDocumento";
        ColIdTipoDocumento.Buscador = "false";
        GridTipoDocumento.Columnas.Add(ColIdTipoDocumento);

        //TipoDocumento
        CJQColumn ColTipoDocumento = new CJQColumn();
        ColTipoDocumento.Nombre = "TipoDocumento";
        ColTipoDocumento.Encabezado = "Tipo de documento";
        ColTipoDocumento.Ancho = "400";
        ColTipoDocumento.Alineacion = "Left";
        GridTipoDocumento.Columnas.Add(ColTipoDocumento);

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
        ColBaja.Oculto = puedeEliminarTipoDocumento == 1 ? "false" : "true";
        GridTipoDocumento.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoDocumento";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoDocumento.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoDocumento", GridTipoDocumento.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoDocumento(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoDocumento, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoDocumento", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoDocumento", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoDocumento);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoDocumento(string pTipoDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTipoDocumento = new CJson();
        jsonTipoDocumento.StoredProcedure.CommandText = "spb_TipoDocumento_ConsultarFiltros";
        jsonTipoDocumento.StoredProcedure.Parameters.AddWithValue("Opcion", 2);
        jsonTipoDocumento.StoredProcedure.Parameters.AddWithValue("@pTipoDocumento", pTipoDocumento);
        return jsonTipoDocumento.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoDocumento(Dictionary<string, object> pTipoDocumento)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoDocumento TipoDocumento = new CTipoDocumento();
            TipoDocumento.TipoDocumento = Convert.ToString(pTipoDocumento["TipoDocumento"]);
            TipoDocumento.Comando = Convert.ToString(pTipoDocumento["Comando"]);

            string validacion = ValidarTipoDocumento(TipoDocumento, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoDocumento.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTipoDocumento(int pIdTipoDocumento)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeConsultarTipoDocumento", puedeConsultarTipoDocumento);
        oPermisos.Add("puedeEditarTipoDocumento", puedeEditarTipoDocumento);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoDocumento TipoDocumento = new CTipoDocumento();
            TipoDocumento.LlenaObjeto(pIdTipoDocumento, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoDocumento", TipoDocumento.IdTipoDocumento));
            Modelo.Add(new JProperty("TipoDocumento", TipoDocumento.TipoDocumento));
            Modelo.Add(new JProperty("Comando", TipoDocumento.Comando));

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
    public static string ObtenerFormaEditarTipoDocumento(int IdTipoDocumento)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarTipoDocumento", puedeEditarTipoDocumento);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoDocumento TipoDocumento = new CTipoDocumento();
            TipoDocumento.LlenaObjeto(IdTipoDocumento, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoDocumento", TipoDocumento.IdTipoDocumento));
            Modelo.Add(new JProperty("TipoDocumento", TipoDocumento.TipoDocumento));
            Modelo.Add(new JProperty("Comando", TipoDocumento.Comando));
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
    public static string EditarTipoDocumento(Dictionary<string, object> pTipoDocumento)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoDocumento TipoDocumento = new CTipoDocumento();
        TipoDocumento.IdTipoDocumento = Convert.ToInt32(pTipoDocumento["IdTipoDocumento"]); ;
        TipoDocumento.TipoDocumento = Convert.ToString(pTipoDocumento["TipoDocumento"]);
        TipoDocumento.Comando = Convert.ToString(pTipoDocumento["Comando"]);

        string validacion = ValidarTipoDocumento(TipoDocumento, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoDocumento.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoDocumento, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoDocumento TipoDocumento = new CTipoDocumento();
            TipoDocumento.IdTipoDocumento = pIdTipoDocumento;
            TipoDocumento.Baja = pBaja;
            TipoDocumento.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoDocumentoEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoDocumento(CTipoDocumento pTipoDocumento, CConexion pConexion)
    {
        string errores = "";
        if (pTipoDocumento.TipoDocumento == "")
        { errores = errores + "<span>*</span> El campo tipo de documento esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}