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

public partial class CostoCampana : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CUsuario Usuario = new CUsuario();
        Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

        //GridCostoCampana
        CJQGrid GridCostoCampana = new CJQGrid();
        GridCostoCampana.NombreTabla = "grdCostoCampana";
        GridCostoCampana.CampoIdentificador = "IdCostoCampana";
        GridCostoCampana.ColumnaOrdenacion = "CostoCampana";
        GridCostoCampana.Metodo = "ObtenerCostoCampana";
        GridCostoCampana.TituloTabla = "Catálogo costo campaña";

        //IdCostoCampana
        CJQColumn ColIdCostoCampana = new CJQColumn();
        ColIdCostoCampana.Nombre = "IdCostoCampana";
        ColIdCostoCampana.Oculto = "true";
        ColIdCostoCampana.Encabezado = "IdCostoCampana";
        ColIdCostoCampana.Buscador = "false";
        GridCostoCampana.Columnas.Add(ColIdCostoCampana);

        //CostoCampana
        CJQColumn ColCostoCampana = new CJQColumn();
        ColCostoCampana.Nombre = "CostoCampana";
        ColCostoCampana.Encabezado = "Descripción";
        ColCostoCampana.Ancho = "250";
        ColCostoCampana.Alineacion = "Left";
        GridCostoCampana.Columnas.Add(ColCostoCampana);

        //Monto
        CJQColumn ColMonto = new CJQColumn();
        ColMonto.Nombre = "Monto";
        ColMonto.Encabezado = "Monto";
        ColMonto.Ancho = "100";
        ColMonto.Alineacion = "Left";
        GridCostoCampana.Columnas.Add(ColMonto);

        //CostoCampana
        CJQColumn ColCampana = new CJQColumn();
        ColCampana.Nombre = "Campana";
        ColCampana.Encabezado = "Campaña";
        ColCampana.Ancho = "250";
        ColCampana.Alineacion = "Left";
        GridCostoCampana.Columnas.Add(ColCampana);

        //Usuario
        CJQColumn ColUsuario = new CJQColumn();
        ColUsuario.Nombre = "Usuario";
        ColUsuario.Encabezado = "Usuario";
        ColUsuario.Ancho = "300";
        ColUsuario.Alineacion = "Left";
        GridCostoCampana.Columnas.Add(ColUsuario);

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
        GridCostoCampana.Columnas.Add(ColBaja);

        //Consultar
        /*CJQColumn ColConsultar = new CJQColumn();
        ColConsultar.Nombre = "Consultar";
        ColConsultar.Encabezado = "Ver";
        ColConsultar.Etiquetado = "ImagenConsultar";
        ColConsultar.Estilo = "divImagenConsultar imgFormaConsultarCostoCampana";
        ColConsultar.Buscador = "false";
        ColConsultar.Ordenable = "false";
        ColConsultar.Ancho = "25";
        GridCostoCampana.Columnas.Add(ColConsultar);*/

        ClientScript.RegisterStartupScript(this.GetType(), "grdCostoCampana", GridCostoCampana.GeneraGrid(), true);

        if (Usuario.TienePermisos(new string[] { "puedeAgregarCostoCampana" }, ConexionBaseDatos) != "")
        {
            divAreaBotonesDialog.InnerHtml = "";
        }
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static CJQGridJsonResponse ObtenerCostoCampana(int pTamanoPaginacion, int pPaginaActual, string pColumnaOrden, string pTipoOrden, string pCostoCampana, int pAI)
    {
        SqlConnection sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionArqNetLocal"].ConnectionString);
        SqlCommand Stored = new SqlCommand("spg_grdCostoCampana", sqlCon);
        Stored.CommandType = CommandType.StoredProcedure;
        Stored.Parameters.Add("TamanoPaginacion", SqlDbType.Int).Value = pTamanoPaginacion;
        Stored.Parameters.Add("PaginaActual", SqlDbType.Int).Value = pPaginaActual;
        Stored.Parameters.Add("ColumnaOrden", SqlDbType.VarChar, 20).Value = pColumnaOrden;
        Stored.Parameters.Add("TipoOrden", SqlDbType.VarChar, 4).Value = pTipoOrden;
        Stored.Parameters.Add("pCostoCampana", SqlDbType.VarChar, 250).Value = Convert.ToString(pCostoCampana);
        Stored.Parameters.Add("pBaja", SqlDbType.Int).Value = pAI;

        DataSet dataSet = new DataSet();
        SqlDataAdapter dataAdapter = new SqlDataAdapter(Stored);
        dataAdapter.Fill(dataSet);
        return new CJQGridJsonResponse(dataSet);
    }

    [WebMethod]
    public static string BuscarCostoCampana(string pCostoCampana)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CJson jsonCostoCampana = new CJson();
        jsonCostoCampana.StoredProcedure.CommandText = "sp_CostoCampana_Consultar_FiltroPorCostoCampana";
        jsonCostoCampana.StoredProcedure.Parameters.AddWithValue("@pCostoCampana", pCostoCampana);
        string jsonCostoCampanaString = jsonCostoCampana.ObtenerJsonString(ConexionBaseDatos);
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return jsonCostoCampanaString;
    }

    [WebMethod]
    public static string AgregarCostoCampana(Dictionary<string, object> pCostoCampana)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CUsuario Usuario = new CUsuario();
            Usuario.LlenaObjeto(Convert.ToInt32(HttpContext.Current.Session["IdUsuario"]), ConexionBaseDatos);

            CCostoCampana CostoCampana = new CCostoCampana();
            CostoCampana.CostoCampana = Convert.ToString(pCostoCampana["CostoCampana"]);
            CostoCampana.IdCampana = Convert.ToInt32(pCostoCampana["IdCampana"]);
            CostoCampana.Monto = Convert.ToDecimal(pCostoCampana["Monto"]);
            CostoCampana.IdUsuario = Convert.ToInt32(Usuario.IdUsuario);
            CostoCampana.FechaAlta = Convert.ToDateTime(DateTime.Now);

            string validacion = ValidarCostoCampana(CostoCampana, ConexionBaseDatos);

            JObject oRespuesta = new JObject();
            if (validacion == "")
            {
                CostoCampana.Agregar(ConexionBaseDatos);
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
    public static string ObtenerFormaAgregarCostoCampana()
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        JObject oRespuesta = new JObject();

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            Modelo.Add("Campanas", CCampana.ObtenerJsonCampana(true, ConexionBaseDatos));
            Modelo.Add("ValorDefault", "0");
            Modelo.Add("DescripcionDefault", "Elegir una opción...");
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
    public static string ObtenerFormaCostoCampana(int pIdCostoCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCostoCampana = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCostoCampana" }, ConexionBaseDatos) == "")
        {
            puedeEditarCostoCampana = 1;
        }
        oPermisos.Add("puedeEditarCostoCampana", puedeEditarCostoCampana);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCostoCampana CostoCampana = new CCostoCampana();
            CostoCampana.LlenaObjeto(pIdCostoCampana, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCostoCampana", CostoCampana.IdCostoCampana));
            Modelo.Add(new JProperty("CostoCampana", CostoCampana.CostoCampana));

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
    public static string ObtenerFormaEditarCostoCampana(int IdCostoCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();
        int puedeEditarCostoCampana = 0;
        JObject oRespuesta = new JObject();
        JObject oPermisos = new JObject();
        CUsuario Usuario = new CUsuario();
        if (Usuario.TienePermisos(new string[] { "puedeEditarCostoCampana" }, ConexionBaseDatos) == "")
        {
            puedeEditarCostoCampana = 1;
        }
        oPermisos.Add("puedeEditarCostoCampana", puedeEditarCostoCampana);

        if (respuesta == "Conexion Establecida")
        {
            JObject Modelo = new JObject();
            CCostoCampana CostoCampana = new CCostoCampana();
            CostoCampana.LlenaObjeto(IdCostoCampana, ConexionBaseDatos);
            Modelo.Add(new JProperty("IdCostoCampana", CostoCampana.IdCostoCampana));
            Modelo.Add(new JProperty("CostoCampana", CostoCampana.CostoCampana));
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
    public static string EditarCostoCampana(Dictionary<string, object> pCostoCampana)
    {
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        CCostoCampana CostoCampana = new CCostoCampana();
        CostoCampana.IdCostoCampana = Convert.ToInt32(pCostoCampana["IdCostoCampana"]); ;
        CostoCampana.CostoCampana = Convert.ToString(pCostoCampana["CostoCampana"]);

        string validacion = ValidarCostoCampana(CostoCampana, ConexionBaseDatos);

        JObject oRespuesta = new JObject();
        if (validacion == "")
        {
            CostoCampana.Editar(ConexionBaseDatos);
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
    public static string CambiarEstatus(int pIdCostoCampana, bool pBaja)
    {
        //Abrir Conexion
        CConexion ConexionBaseDatos = new CConexion();
        string respuesta = ConexionBaseDatos.ConectarBaseDatosSqlServer();

        //¿La conexion se establecio?
        if (respuesta == "Conexion Establecida")
        {
            CCostoCampana CostoCampana = new CCostoCampana();
            CostoCampana.IdCostoCampana = pIdCostoCampana;
            CostoCampana.Baja = pBaja;
            CostoCampana.Eliminar(ConexionBaseDatos);
            respuesta = "0|CostoCampanaEliminado";
        }

        //Cerrar Conexion
        ConexionBaseDatos.CerrarBaseDatosSqlServer();
        return respuesta;
    }

    //Validaciones
    private static string ValidarCostoCampana(CCostoCampana pCostoCampana, CConexion pConexion)
    {
        string errores = "";
        if (pCostoCampana.CostoCampana == "")
        { errores = errores + "<span>*</span> El campo costo campana esta vacío, favor de capturarlo.<br />"; }

        if (pCostoCampana.IdCampana == 0)
        { errores = errores + "<span>*</span> El campo tipo campaña esta vacío, favor de capturarlo.<br />"; }

        if (pCostoCampana.Monto == 0)
        { errores = errores + "<span>*</span> El monto esta vacío, favor de capturarlo.<br />"; }

        if (errores != "")
        { errores = "<p>Favor de completar los siguientes requisitos:</p>" + errores; }

        return errores;
    }
}