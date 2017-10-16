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

public partial class TipoVenta : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //GridTipoVenta
        CJQGrid GridTipoVenta = new CJQGrid();
        GridTipoVenta.NombreTabla = "grdTipoVenta";
        GridTipoVenta.CampoIdentificador = "IdTipoVenta";
        GridTipoVenta.ColumnaOrdenacion = "TipoVenta";
        GridTipoVenta.Metodo = "ObtenerTipoVenta";
        GridTipoVenta.TituloTabla = "Catálogo de tipo de venta";

        //IdTipoVenta
        CJQColumn ColIdTipoVenta = new CJQColumn();
        ColIdTipoVenta.Nombre = "IdTipoVenta";
        ColIdTipoVenta.Oculto = "true";
        ColIdTipoVenta.Encabezado = "IdTipoVenta";
        ColIdTipoVenta.Buscador = "false";
        GridTipoVenta.Columnas.Add(ColIdTipoVenta);

        //TipoVenta
        CJQColumn ColTipoVenta = new CJQColumn();
        ColTipoVenta.Nombre = "TipoVenta";
        ColTipoVenta.Encabezado = "Tipo de venta";
        ColTipoVenta.Ancho = "400";
        ColTipoVenta.Alineacion = "left";
        GridTipoVenta.Columnas.Add(ColTipoVenta);

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
        GridTipoVenta.Columnas.Add(ColBaja);

        //Consultar
        CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarTipoVenta";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridTipoVenta.Columnas.Add(ColConsultar);

        ClientScript.RegisterStartupScript(this.GetType(), "grdTipoVenta", GridTipoVenta.GeneraGrid(), true);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerTipoVenta(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pTipoVenta, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdTipoVenta", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pTipoVenta", SqlDbType.VarChar, 250).Value = Convert.ToString(pTipoVenta);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarTipoVenta(string pTipoVenta)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        CJson jsonTipoVenta = new CJson();
        jsonTipoVenta.StoredProcedure.CommandText = "sp_TipoVenta_Consultar_FiltroPorTipoVenta";
        jsonTipoVenta.StoredProcedure.Parameters.AddWithValue("@pTipoVenta", pTipoVenta);
        return jsonTipoVenta.ObtenerJsonString(ConexionBaseDatos);
    }

    [WebMethod]
    public static string AgregarTipoVenta(Dictionary<string, object> pTipoVenta)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {

            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.TipoVenta = Convert.ToString(pTipoVenta["TipoVenta"]);

            string validacion = ValidarTipoVenta(TipoVenta, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                TipoVenta.Agregar(ConexionBaseDatos);
                oRespuesta.Add(new JProperty("Error", 0));
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
    public static string ObtenerFormaTipoVenta(int pIdTipoVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoVenta = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoVenta" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoVenta = 1;
        }
        oPermisos.Add("puedeEditarTipoVenta", puedeEditarTipoVenta);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.LlenaObjeto(pIdTipoVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoVenta", TipoVenta.IdTipoVenta));
            Modelo.Add(new JProperty("TipoVenta", TipoVenta.TipoVenta));

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
    public static string ObtenerFormaEditarTipoVenta(int IdTipoVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarTipoVenta = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarTipoVenta" }, ConexionBaseDatos) == "")
        {
            puedeEditarTipoVenta = 1;
        }
        oPermisos.Add("puedeEditarTipoVenta", puedeEditarTipoVenta);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.LlenaObjeto(IdTipoVenta, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdTipoVenta", TipoVenta.IdTipoVenta));
            Modelo.Add(new JProperty("TipoVenta", TipoVenta.TipoVenta));
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
    public static string EditarTipoVenta(Dictionary<string, object> pTipoVenta)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CTipoVenta TipoVenta = new CTipoVenta();
        TipoVenta.IdTipoVenta = Convert.ToInt32(pTipoVenta["IdTipoVenta"]); ;
        TipoVenta.TipoVenta = Convert.ToString(pTipoVenta["TipoVenta"]);

        string validacion = ValidarTipoVenta(TipoVenta, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            TipoVenta.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdTipoVenta, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CTipoVenta TipoVenta = new CTipoVenta();
            TipoVenta.IdTipoVenta = pIdTipoVenta;
            TipoVenta.Baja = pBaja;
            TipoVenta.Eliminar(ConexionBaseDatos);
            respuesta = "0|TipoVentaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarTipoVenta(CTipoVenta pTipoVenta, CConexion pConexion)
    {
        string errores = "";
        if (pTipoVenta.TipoVenta == "")
        { errores = errores + "<span>*</span> El campo tipo de venta esta vac&iacute;o, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}