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

public partial class TiempoEntrega : System.Web.UI.Page
{
    private static int idUsuario;
    private static int idSucursal;
    public static int puedeAgregarTiempoEntrega = 0;
    public static int puedeEditarTiempoEntrega = 0;
    public static int puedeEliminarTiempoEntrega = 0;
    public static int puedeConsultarTiempoEntrega = 0;

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

        if (Usuario.TienePermisos(new string[] { "puedeAgregarTiempoEntrega" }, ConexionBaseDatos) == "") { puedeAgregarTiempoEntrega = 1; }
        else { puedeAgregarTiempoEntrega = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEditarTiempoEntrega" }, ConexionBaseDatos) == "") { puedeEditarTiempoEntrega = 1; }
        else { puedeEditarTiempoEntrega = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeEliminarTiempoEntrega" }, ConexionBaseDatos) == "") { puedeEliminarTiempoEntrega = 1; }
        else { puedeEliminarTiempoEntrega = 0; }

        if (Usuario.TienePermisos(new string[] { "puedeConsultarTiempoEntrega" }, ConexionBaseDatos) == "") { puedeConsultarTiempoEntrega = 1; }
        else { puedeConsultarTiempoEntrega = 0; }

        //GridTiempoEntrega
        CJQGrid GridTiempoEntrega = new CJQGrid();
        GridTiempoEntrega.NombreTabla = "grdTiempoEntrega";
        GridTiempoEntrega.CampoIdentificador = "IdTiempoEntrega";
        GridTiempoEntrega.ColumnaOrdenacion = "TiempoEntrega";
        GridTiempoEntrega.Metodo = "ObtenerTiempoEntrega";
        GridTiempoEntrega.TituloTabla = "Catálogo de tiempo de entrega";

        //IdTiempoEntrega
        CJQColumn ColIdTiempoEntrega = new CJQColumn();
        ColIdTiempoEntrega.Nombre = "IdTiempoEntrega";
        ColIdTiempoEntrega.Oculto = "true";
        ColIdTiempoEntrega.Encabezado = "IdTiempoEntrega";
        ColIdTiempoEntrega.Buscador = "false";
        GridTiempoEntrega.Columnas.Add(ColIdTiempoEntrega);

        //TiempoEntrega
        CJQColumn ColTiempoEntrega = new CJQColumn();
        ColTiempoEntrega.Nombre = "TiempoEntrega";
        ColTiempoEntrega.Encabezado = "Tiempo de entrega";
        ColTiempoEntrega.Ancho = "400";
        ColTiempoEntrega.Alineacion = "Left";
        GridTiempoEntrega.Columnas.Add(ColTiempoEntrega);

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
        ColBaja.Oculto = puedeEliminarTiempoEntrega == 1 ? "false" : "true";
        GridTiempoEntrega.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTiempoEntrega";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Oculto = puedeConsultarTiempoEntrega == 1 ? "false" : "true";
        ColConsultar.Ancho = "25";
        GridTiempoEntrega.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTiempoEntrega", GridTiempoEntrega.GeneraGrid(), true);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTiempoEntrega(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTiempoEntrega, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTiempoEntrega", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTiempoEntrega", SqlDbType.VarChar, 250).Value = Convert.ToString(pTiempoEntrega);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTiempoEntrega(string pTiempoEntrega)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonTiempoEntrega = new CJson();
        jsonTiempoEntrega.StoredProcedure.CommandText = "spb_TiempoEntrega_ConsultarFiltros";
        jsonTiempoEntrega.StoredProcedure.Parameters.AddWithValue("Opcion", 2);
        jsonTiempoEntrega.StoredProcedure.Parameters.AddWithValue("@pTiempoEntrega", pTiempoEntrega);
        return jsonTiempoEntrega.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTiempoEntrega(Dictionary<string, object> pTiempoEntrega)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
            TiempoEntrega.TiempoEntrega = Convert.ToString(pTiempoEntrega["TiempoEntrega"]);

            string validacion = ValidarTiempoEntrega(TiempoEntrega, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TiempoEntrega.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaTiempoEntrega(int pIdTiempoEntrega)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarTiempoEntrega", puedeEditarTiempoEntrega);
        oPermisos.Add("puedeConsultarTiempoEntrega", puedeConsultarTiempoEntrega);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
            TiempoEntrega.LlenaObjeto(pIdTiempoEntrega, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTiempoEntrega", TiempoEntrega.IdTiempoEntrega));
            Modelo.Add(new JProperty("TiempoEntrega", TiempoEntrega.TiempoEntrega));

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
    public static string ObtenerFormaEditarTiempoEntrega(int IdTiempoEntrega)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();

        oPermisos.Add("puedeEditarTiempoEntrega", puedeEditarTiempoEntrega);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
            TiempoEntrega.LlenaObjeto(IdTiempoEntrega, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTiempoEntrega", TiempoEntrega.IdTiempoEntrega));
            Modelo.Add(new JProperty("TiempoEntrega", TiempoEntrega.TiempoEntrega));
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
    public static string EditarTiempoEntrega(Dictionary<string, object> pTiempoEntrega)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
        TiempoEntrega.IdTiempoEntrega = Convert.ToInt32(pTiempoEntrega["IdTiempoEntrega"]); ;
        TiempoEntrega.TiempoEntrega = Convert.ToString(pTiempoEntrega["TiempoEntrega"]);

        string validacion = ValidarTiempoEntrega(TiempoEntrega, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TiempoEntrega.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTiempoEntrega, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTiempoEntrega TiempoEntrega = new CTiempoEntrega();
            TiempoEntrega.IdTiempoEntrega = pIdTiempoEntrega;
            TiempoEntrega.Baja = pBaja;
            TiempoEntrega.Eliminar(ConexionBaseDatos);
            respuesta = "0|TiempoEntregaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTiempoEntrega(CTiempoEntrega pTiempoEntrega, CConexion pConexion)
    {
        string errores = "";
        if (pTiempoEntrega.TiempoEntrega == "")
        { errores = errores + "<span>*</span> El campo tiempo de entrega esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}